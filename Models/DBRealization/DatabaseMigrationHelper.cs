using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Models.DBRealization;

/// <summary>
/// Единая точка входа для EF-миграций RAODB (Firebird).
/// </summary>
/// <remarks>
/// <para>
/// <b>Как пользоваться</b><br/>
/// Не вызывайте <c>db.Database.Migrate()</c> / <c>MigrateAsync()</c> напрямую
/// (запрещено BannedApiAnalyzers, RS0030).<br/>
/// Используйте:
/// <list type="bullet">
/// <item><c>db.MigrateDatabase()</c> / <c>db.MigrateDatabaseAsync()</c> на <see cref="DataContext"/>;</item>
/// <item>или <see cref="Migrate"/> / <see cref="MigrateAsync"/> этого хелпера.</item>
/// </list>
/// Обёртка сначала чинит <c>__EFMigrationsHistory</c> (если нужно), затем вызывает обычный Migrate.
/// </para>
///
/// <para>
/// <b>Какой баг чинили</b><br/>
/// Две ветки одновременно сделали миграцию с именем DataModel_42. После ручного мерджа
/// один и тот же MigrationId <c>20260617074927_DataModel_42</c> в разных сборках означал разное:
/// <list type="bullet">
/// <item>
/// До/на авторской ветке удаления CorrectionNumber: Up() = DropColumn
/// <c>CorrectionNumber_DB</c> у form_2.x. В БД колонок уже нет, в history лежит этот id.
/// </item>
/// <item>
/// После кривого мерджа: тот же id повесили на Up() коллеги (varchar-правки form_51…),
/// а DropColumn остался в «мёртвом» классе без <c>[Migration]</c> и не применялся.
/// У таких БД колонка ещё есть, id в history «ложный».
/// </item>
/// </list>
/// Из-за этого:
/// <list type="number">
/// <item>выгрузка .raodb падала на NOT NULL CorrectionNumber_DB (колонка в схеме есть, в модели нет);</item>
/// <item>
/// наивный repair «просто удалить/переименовать id» ломал БД автора: EF снова пытался DropColumn.
/// </item>
/// </list>
/// Канонические id в текущей сборке:
/// <list type="bullet">
/// <item><see cref="CanonicalDataModel42MigrationId"/> — varchar-правки коллеги;</item>
/// <item><see cref="RemoveCorrectionNumberMigrationId"/> — идемпотентное удаление CorrectionNumber_DB.</item>
/// </list>
/// </para>
///
/// <para>
/// <b>Матрица repair для ambiguous id</b>
/// (<see cref="AmbiguousDataModel42MigrationId"/>):
/// <list type="table">
/// <listheader>
/// <term>CorrectionNumber_DB на form_21</term>
/// <description>Действие с ambiguous id</description>
/// </listheader>
/// <item>
/// <term>колонки нет</term>
/// <description>
/// DropColumn уже был → переименовать в <see cref="RemoveCorrectionNumberMigrationId"/>
/// (или удалить, если канонический id уже есть).
/// </description>
/// </item>
/// <item>
/// <term>колонка есть</term>
/// <description>
/// Это был Up коллеги под чужим id → переименовать в <see cref="CanonicalDataModel42MigrationId"/>
/// (или удалить дубль). Дальше Migrate накатит RemoveCorrectionNumber.
/// </description>
/// </item>
/// </list>
/// Если ambiguous id в history нет — repair no-op.
/// </para>
///
/// <para>
/// <b>Куда дописывать, если появится похожая проблема</b>
/// <list type="number">
/// <item>Добавьте константы старого/канонического MigrationId в регион Migration ids.</item>
/// <item>
/// Добавьте шаг в <see cref="RepairMigrationHistory"/> / Async
/// (или отдельный private-метод и вызов из него) — <b>до</b> <c>database.Migrate*</c>.
/// </item>
/// <item>
/// Если нужно отличить «что реально сделал старый Up», проверяйте схему
/// (<see cref="FirebirdColumnExists"/>), а не только строки history.
/// </item>
/// <item>
/// Саму DDL-миграцию по возможности делайте идемпотентной
/// (как RemoveCorrectionNumFromForms2 через EXECUTE BLOCK), если БД в поле могли
/// уже частично получить тот же эффект под другим id.
/// </item>
/// <item>Не вызывайте голый <c>Database.Migrate*</c> из нового кода.</item>
/// </list>
/// </para>
/// </remarks>
public static class DatabaseMigrationHelper
{
    #region Migration ids

    /// <summary>
    /// Ambiguous id после мерджа веток: в разных сборках означал либо DropColumn CorrectionNumber,
    /// либо varchar-правки DataModel_42 коллеги.
    /// </summary>
    public const string AmbiguousDataModel42MigrationId = "20260617074927_DataModel_42";

    /// <summary>
    /// Канонический id миграции коллеги (varchar/rename на form_51…form_57).
    /// </summary>
    public const string CanonicalDataModel42MigrationId = "20260213092206_DataModel_42";

    /// <summary>
    /// Канонический id удаления CorrectionNumber_DB с form_2.x.
    /// </summary>
    public const string RemoveCorrectionNumberMigrationId = "20260811103700_RemoveCorrectionNumFromForms2";

    #endregion

    #region Schema probe

    // Достаточно одной таблицы form_2.x: колонка добавлялась/снималась пакетом на все form_21…form_212.
    private const string CorrectionNumberProbeTable = "form_21";
    private const string CorrectionNumberProbeColumn = "CorrectionNumber_DB";

    #endregion

    #region Public API — use these instead of Database.Migrate*

    /// <summary>
    /// Чинит history при необходимости, затем применяет pending-миграции.
    /// </summary>
    public static void Migrate(DatabaseFacade database)
    {
        RepairMigrationHistory(database);
#pragma warning disable RS0030 // единственное место, где разрешён прямой Migrate
        database.Migrate();
#pragma warning restore RS0030
    }

    /// <inheritdoc cref="Migrate"/>
    public static async Task MigrateAsync(DatabaseFacade database, CancellationToken cancellationToken = default)
    {
        await RepairMigrationHistoryAsync(database, cancellationToken).ConfigureAwait(false);
#pragma warning disable RS0030
        await database.MigrateAsync(cancellationToken).ConfigureAwait(false);
#pragma warning restore RS0030
    }

    #endregion

    #region History repair orchestration

    /// <summary>
    /// Все silent-repair шаги для <c>__EFMigrationsHistory</c>. Сюда добавляйте новые кейсы.
    /// </summary>
    private static void RepairMigrationHistory(DatabaseFacade database)
    {
        RepairAmbiguousDataModel42History(database);
    }

    /// <inheritdoc cref="RepairMigrationHistory"/>
    private static async Task RepairMigrationHistoryAsync(
        DatabaseFacade database,
        CancellationToken cancellationToken)
    {
        await RepairAmbiguousDataModel42HistoryAsync(database, cancellationToken).ConfigureAwait(false);
    }

    #endregion

    #region Repair: ambiguous DataModel_42 id

    private static void RepairAmbiguousDataModel42History(DatabaseFacade database)
    {
        var applied = database.GetAppliedMigrations().ToHashSet();
        if (!applied.Contains(AmbiguousDataModel42MigrationId))
        {
            return;
        }

        var correctionNumberExists = FirebirdColumnExists(
            database, CorrectionNumberProbeTable, CorrectionNumberProbeColumn);
        var plan = BuildAmbiguousDataModel42RepairPlan(applied, correctionNumberExists);
        ApplyHistoryRepairPlan(database, plan);
    }

    private static async Task RepairAmbiguousDataModel42HistoryAsync(
        DatabaseFacade database,
        CancellationToken cancellationToken)
    {
        var applied = (await database.GetAppliedMigrationsAsync(cancellationToken).ConfigureAwait(false))
            .ToHashSet();
        if (!applied.Contains(AmbiguousDataModel42MigrationId))
        {
            return;
        }

        var correctionNumberExists = await FirebirdColumnExistsAsync(
                database, CorrectionNumberProbeTable, CorrectionNumberProbeColumn, cancellationToken)
            .ConfigureAwait(false);
        var plan = BuildAmbiguousDataModel42RepairPlan(applied, correctionNumberExists);
        await ApplyHistoryRepairPlanAsync(database, plan, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Матрица: нет колонки → ambiguous = уже выполнен RemoveCorrectionNumber;
    /// колонка есть → ambiguous = канонический DataModel_42 коллеги под чужим id.
    /// </summary>
    private static HistoryRepairPlan BuildAmbiguousDataModel42RepairPlan(
        HashSet<string> applied,
        bool correctionNumberColumnExists)
    {
        if (!correctionNumberColumnExists)
        {
            return applied.Contains(RemoveCorrectionNumberMigrationId)
                ? HistoryRepairPlan.Delete(AmbiguousDataModel42MigrationId)
                : HistoryRepairPlan.Rename(AmbiguousDataModel42MigrationId, RemoveCorrectionNumberMigrationId);
        }

        return applied.Contains(CanonicalDataModel42MigrationId)
            ? HistoryRepairPlan.Delete(AmbiguousDataModel42MigrationId)
            : HistoryRepairPlan.Rename(AmbiguousDataModel42MigrationId, CanonicalDataModel42MigrationId);
    }

    #endregion

    #region Apply history repair plan

    private static void ApplyHistoryRepairPlan(DatabaseFacade database, HistoryRepairPlan plan)
    {
        switch (plan.Kind)
        {
            case HistoryRepairKind.None:
                return;
            case HistoryRepairKind.Delete:
                database.ExecuteSqlRaw(
                    $@"DELETE FROM ""__EFMigrationsHistory"" WHERE ""MigrationId"" = '{plan.SourceMigrationId}'");
                return;
            case HistoryRepairKind.Rename:
                database.ExecuteSqlRaw(
                    $@"UPDATE ""__EFMigrationsHistory"" SET ""MigrationId"" = '{plan.TargetMigrationId}' WHERE ""MigrationId"" = '{plan.SourceMigrationId}'");
                return;
            default:
                throw new InvalidOperationException($"Unknown history repair kind: {plan.Kind}");
        }
    }

    private static async Task ApplyHistoryRepairPlanAsync(
        DatabaseFacade database,
        HistoryRepairPlan plan,
        CancellationToken cancellationToken)
    {
        switch (plan.Kind)
        {
            case HistoryRepairKind.None:
                return;
            case HistoryRepairKind.Delete:
                await database.ExecuteSqlRawAsync(
                        $@"DELETE FROM ""__EFMigrationsHistory"" WHERE ""MigrationId"" = '{plan.SourceMigrationId}'",
                        cancellationToken)
                    .ConfigureAwait(false);
                return;
            case HistoryRepairKind.Rename:
                await database.ExecuteSqlRawAsync(
                        $@"UPDATE ""__EFMigrationsHistory"" SET ""MigrationId"" = '{plan.TargetMigrationId}' WHERE ""MigrationId"" = '{plan.SourceMigrationId}'",
                        cancellationToken)
                    .ConfigureAwait(false);
                return;
            default:
                throw new InvalidOperationException($"Unknown history repair kind: {plan.Kind}");
        }
    }

    #endregion

    #region Firebird schema helpers

    private static bool FirebirdColumnExists(DatabaseFacade database, string tableName, string columnName)
    {
        var connection = database.GetDbConnection();
        var shouldClose = false;
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
            shouldClose = true;
        }

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT COUNT(*) FROM RDB$RELATION_FIELDS
                  WHERE TRIM(RDB$RELATION_NAME) = @tableName
                    AND TRIM(RDB$FIELD_NAME) = @columnName";
            AddParameter(command, "@tableName", tableName);
            AddParameter(command, "@columnName", columnName);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        finally
        {
            if (shouldClose)
            {
                connection.Close();
            }
        }
    }

    private static async Task<bool> FirebirdColumnExistsAsync(
        DatabaseFacade database,
        string tableName,
        string columnName,
        CancellationToken cancellationToken)
    {
        var connection = database.GetDbConnection();
        var shouldClose = false;
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            shouldClose = true;
        }

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT COUNT(*) FROM RDB$RELATION_FIELDS
                  WHERE TRIM(RDB$RELATION_NAME) = @tableName
                    AND TRIM(RDB$FIELD_NAME) = @columnName";
            AddParameter(command, "@tableName", tableName);
            AddParameter(command, "@columnName", columnName);
            var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            return Convert.ToInt32(result) > 0;
        }
        finally
        {
            if (shouldClose)
            {
                await connection.CloseAsync().ConfigureAwait(false);
            }
        }
    }

    private static void AddParameter(DbCommand command, string name, string value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }

    #endregion

    #region Repair plan model

    private enum HistoryRepairKind
    {
        None,
        Delete,
        Rename
    }

    private readonly struct HistoryRepairPlan
    {
        public static HistoryRepairPlan None { get; } = new(HistoryRepairKind.None, null, null);

        public HistoryRepairKind Kind { get; }
        public string SourceMigrationId { get; }
        public string TargetMigrationId { get; }

        private HistoryRepairPlan(HistoryRepairKind kind, string sourceMigrationId, string targetMigrationId)
        {
            Kind = kind;
            SourceMigrationId = sourceMigrationId;
            TargetMigrationId = targetMigrationId;
        }

        public static HistoryRepairPlan Delete(string sourceMigrationId) =>
            new(HistoryRepairKind.Delete, sourceMigrationId, null);

        public static HistoryRepairPlan Rename(string sourceMigrationId, string targetMigrationId) =>
            new(HistoryRepairKind.Rename, sourceMigrationId, targetMigrationId);
    }

    #endregion
}
