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

public sealed class TextColumnLengthMeasurer
{
    private readonly DatabaseFacade _database;

    public TextColumnLengthMeasurer(DatabaseFacade database) => _database = database;

    public async Task MeasureAsync(
        IReadOnlyList<TextColumnDescriptor> columns,
        IProgress<(int current, int total, string message)>? progress,
        CancellationToken cancellationToken = default)
    {
        var measurable = columns
            .Where(c => c.WarningKind is TextColumnWarningKind.None or TextColumnWarningKind.DatabaseOnly)
            .Where(c => c.StorageKind is TextColumnStorageKind.BlobText
                or TextColumnStorageKind.Varchar
                or TextColumnStorageKind.Char)
            .ToList();

        var total = measurable.Count;
        var connection = _database.GetDbConnection();
        var shouldClose = connection.State != ConnectionState.Open;
        if (shouldClose)
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            for (var i = 0; i < measurable.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var column = measurable[i];
                progress?.Report((i + 1, total, $"{column.Table}.{column.Column}"));

                try
                {
                    var (maxLen, nonNull) = await MeasureColumnAsync(
                        connection,
                        column.Table,
                        column.Column,
                        cancellationToken).ConfigureAwait(false);
                    column.MaxCharLength = maxLen;
                    column.NonNullCount = nonNull;
                }
                catch (Exception ex)
                {
                    column.MeasureError = ex.Message;
                    column.WarningKind = TextColumnWarningKind.MeasureFailed;
                    column.WarningMessage = "Failed to measure column length.";
                }
            }
        }
        finally
        {
            if (shouldClose)
                await connection.CloseAsync().ConfigureAwait(false);
        }
    }

    private static async Task<(int maxLength, long nonNullCount)> MeasureColumnAsync(
        DbConnection connection,
        string table,
        string column,
        CancellationToken cancellationToken)
    {
        var quotedTable = QuoteIdentifier(table);
        var quotedColumn = QuoteIdentifier(column);

        await using var command = connection.CreateCommand();
        command.CommandText =
            $"""
             SELECT MAX(CHAR_LENGTH({quotedColumn})),
                    COUNT(*)
             FROM {quotedTable}
             WHERE {quotedColumn} IS NOT NULL
             """;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            return (0, 0);

        var maxLen = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader.GetValue(0));
        var nonNull = reader.IsDBNull(1) ? 0 : Convert.ToInt64(reader.GetValue(1));
        return (maxLen, nonNull);
    }

    private static string QuoteIdentifier(string identifier) =>
        "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
}
