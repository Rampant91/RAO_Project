using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Models.DBRealization.SchemaAnalysis;

public static class FirebirdTextColumnCatalog
{
    private const int FirebirdBlobType = 261;
    private const int FirebirdBlobTextSubType = 1;
    private const int FirebirdVarcharType = 37;
    private const int FirebirdCharType = 14;

    public static async Task<IReadOnlyList<TextColumnDescriptor>> LoadAsync(
        DatabaseFacade database,
        CancellationToken cancellationToken = default)
    {
        var connection = database.GetDbConnection();
        var shouldClose = connection.State != ConnectionState.Open;
        if (shouldClose)
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            var existingTables = await LoadExistingTablesAsync(connection, cancellationToken)
                .ConfigureAwait(false);
            var rows = await LoadColumnsAsync(connection, cancellationToken).ConfigureAwait(false);

            var result = new List<TextColumnDescriptor>();
            foreach (var row in rows)
            {
                if (!RaodbTextColumnTables.Set.Contains(row.Table))
                    continue;

                if (!existingTables.Contains(row.Table))
                    continue;

                var storageKind = MapStorageKind(row.FieldType, row.FieldSubType);
                if (storageKind == TextColumnStorageKind.Unknown)
                    continue;

                result.Add(new TextColumnDescriptor
                {
                    Table = row.Table,
                    Column = row.Column,
                    StorageKind = storageKind,
                    DeclaredLength = row.CharacterLength
                });
            }

            return result;
        }
        finally
        {
            if (shouldClose)
                await connection.CloseAsync().ConfigureAwait(false);
        }
    }

    private static async Task<HashSet<string>> LoadExistingTablesAsync(
        DbConnection connection,
        CancellationToken cancellationToken)
    {
        var tables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT TRIM(RDB$RELATION_NAME)
            FROM RDB$RELATIONS
            WHERE RDB$SYSTEM_FLAG = 0
              AND RDB$VIEW_BLR IS NULL
            """;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            if (reader.IsDBNull(0))
                continue;

            var name = reader.GetString(0).Trim();
            if (RaodbTextColumnTables.Set.Contains(name))
                tables.Add(name);
        }

        return tables;
    }

    private static async Task<List<ColumnRow>> LoadColumnsAsync(
        DbConnection connection,
        CancellationToken cancellationToken)
    {
        var inList = string.Join(", ", RaodbTextColumnTables.All.Select(t => $"'{t}'"));
        await using var command = connection.CreateCommand();
        command.CommandText =
            $"""
             SELECT TRIM(rf.RDB$RELATION_NAME),
                    TRIM(rf.RDB$FIELD_NAME),
                    f.RDB$FIELD_TYPE,
                    f.RDB$FIELD_SUB_TYPE,
                    f.RDB$CHARACTER_LENGTH
             FROM RDB$RELATION_FIELDS rf
             INNER JOIN RDB$FIELDS f ON f.RDB$FIELD_NAME = rf.RDB$FIELD_SOURCE
             WHERE TRIM(rf.RDB$RELATION_NAME) IN ({inList})
               AND (
                    (f.RDB$FIELD_TYPE = {FirebirdBlobType} AND f.RDB$FIELD_SUB_TYPE = {FirebirdBlobTextSubType})
                 OR f.RDB$FIELD_TYPE = {FirebirdVarcharType}
                 OR f.RDB$FIELD_TYPE = {FirebirdCharType}
               )
             ORDER BY 1, 2
             """;

        var rows = new List<ColumnRow>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            rows.Add(new ColumnRow(
                reader.GetString(0).Trim(),
                reader.GetString(1).Trim(),
                reader.IsDBNull(2) ? (short)0 : Convert.ToInt16(reader.GetValue(2)),
                reader.IsDBNull(3) ? null : Convert.ToInt16(reader.GetValue(3)),
                reader.IsDBNull(4) ? null : Convert.ToInt16(reader.GetValue(4))));
        }

        return rows;
    }

    private static TextColumnStorageKind MapStorageKind(short fieldType, short? fieldSubType)
    {
        if (fieldType == FirebirdBlobType && fieldSubType == FirebirdBlobTextSubType)
            return TextColumnStorageKind.BlobText;

        if (fieldType == FirebirdVarcharType)
            return TextColumnStorageKind.Varchar;

        if (fieldType == FirebirdCharType)
            return TextColumnStorageKind.Char;

        return TextColumnStorageKind.Unknown;
    }

    private sealed record ColumnRow(
        string Table,
        string Column,
        short FieldType,
        short? FieldSubType,
        short? CharacterLength);
}
