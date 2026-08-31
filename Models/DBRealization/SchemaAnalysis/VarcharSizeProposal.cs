using System;
using System.Collections.Generic;

namespace Models.DBRealization.SchemaAnalysis;

public static class VarcharSizeProposal
{
    private static readonly int[] SizeSteps =
        [2, 8, 14, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192];

    private const int DefaultEmptyColumnSize = 16;
    private const int KeepBlobThreshold = 8192;

    public static void Apply(IEnumerable<TextColumnDescriptor> columns)
    {
        foreach (var column in columns)
        {
            column.SuggestedWave = SuggestWave(column.Table);
            if (column.StorageKind != TextColumnStorageKind.BlobText)
            {
                column.KeepBlob = false;
                if (column.DeclaredLength is > 0)
                    column.ProposedVarchar = column.DeclaredLength.Value;
                continue;
            }

            var rounded = RoundUp(column.MaxCharLength);
            var proposed = ApplyDomainFloor(column.Column, rounded);
            column.ProposedVarchar = proposed;
            column.KeepBlob = ShouldKeepBlob(column.MaxCharLength, proposed);
        }
    }

    public static int RoundUp(int maxLength)
    {
        if (maxLength <= 0)
            return DefaultEmptyColumnSize;

        foreach (var step in SizeSteps)
        {
            if (maxLength <= step)
                return step;
        }

        return maxLength;
    }

    public static int ApplyDomainFloor(string columnName, int proposed)
    {
        if (string.Equals(columnName, "FormNum_DB", StringComparison.OrdinalIgnoreCase))
            return Math.Max(proposed, 8);

        if (string.Equals(columnName, "OperationCode_DB", StringComparison.OrdinalIgnoreCase))
            return Math.Max(proposed, 2);

        if (columnName.Contains("OKPO", StringComparison.OrdinalIgnoreCase))
            return Math.Max(proposed, 14);

        return proposed;
    }

    /// <summary>
    /// Оставляем BLOB только если фактическая или предложенная длина превышает верхнюю ступень sizing.
    /// </summary>
    public static bool ShouldKeepBlob(int maxCharLength, int proposedVarchar) =>
        maxCharLength > KeepBlobThreshold
        || proposedVarchar > KeepBlobThreshold;

    public static int SuggestWave(string table)
    {
        if (string.Equals(table, "ReportCollection_DbSet", StringComparison.OrdinalIgnoreCase)
            || string.Equals(table, "notes", StringComparison.OrdinalIgnoreCase))
            return 1;

        if (table.StartsWith("form_1", StringComparison.OrdinalIgnoreCase))
            return 2;

        if (table.StartsWith("form_2", StringComparison.OrdinalIgnoreCase))
            return 3;

        if (string.Equals(table, "form_40", StringComparison.OrdinalIgnoreCase)
            || string.Equals(table, "form_41", StringComparison.OrdinalIgnoreCase)
            || table.StartsWith("form_5", StringComparison.OrdinalIgnoreCase))
            return 4;

        return 5;
    }
}
