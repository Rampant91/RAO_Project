using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Models.DBRealization;

// Silent repair: released builds recorded DataModel_42 as 20260617074927_DataModel_42.
public static class DatabaseMigrationHelper
{
    public const string BrokenDataModel42MigrationId = "20260617074927_DataModel_42";
    public const string CorrectDataModel42MigrationId = "20260213092206_DataModel_42";

    public static void Migrate(DatabaseFacade database)
    {
        RepairBrokenDataModel42History(database);
        database.Migrate();
    }

    public static async Task MigrateAsync(DatabaseFacade database, CancellationToken cancellationToken = default)
    {
        await RepairBrokenDataModel42HistoryAsync(database, cancellationToken).ConfigureAwait(false);
        await database.MigrateAsync(cancellationToken).ConfigureAwait(false);
    }

    public static void RepairBrokenDataModel42History(DatabaseFacade database)
    {
        var applied = database.GetAppliedMigrations().ToHashSet();
        if (!applied.Contains(BrokenDataModel42MigrationId))
        {
            return;
        }

        if (applied.Contains(CorrectDataModel42MigrationId))
        {
            database.ExecuteSqlRaw(
                $@"DELETE FROM ""__EFMigrationsHistory"" WHERE ""MigrationId"" = '{BrokenDataModel42MigrationId}'");
        }
        else
        {
            database.ExecuteSqlRaw(
                $@"UPDATE ""__EFMigrationsHistory"" SET ""MigrationId"" = '{CorrectDataModel42MigrationId}' WHERE ""MigrationId"" = '{BrokenDataModel42MigrationId}'");
        }
    }

    public static async Task RepairBrokenDataModel42HistoryAsync(
        DatabaseFacade database,
        CancellationToken cancellationToken = default)
    {
        var applied = (await database.GetAppliedMigrationsAsync(cancellationToken).ConfigureAwait(false)).ToHashSet();
        if (!applied.Contains(BrokenDataModel42MigrationId))
        {
            return;
        }

        if (applied.Contains(CorrectDataModel42MigrationId))
        {
            await database.ExecuteSqlRawAsync(
                    $@"DELETE FROM ""__EFMigrationsHistory"" WHERE ""MigrationId"" = '{BrokenDataModel42MigrationId}'",
                    cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            await database.ExecuteSqlRawAsync(
                    $@"UPDATE ""__EFMigrationsHistory"" SET ""MigrationId"" = '{CorrectDataModel42MigrationId}' WHERE ""MigrationId"" = '{BrokenDataModel42MigrationId}'",
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
