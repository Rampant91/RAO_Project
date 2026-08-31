using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DBRealization.SchemaAnalysis;

public sealed class TextColumnDescriptor
{
    public required string Table { get; init; }
    public required string Column { get; init; }

    public TextColumnStorageKind StorageKind { get; set; } = TextColumnStorageKind.Unknown;
    public int? DeclaredLength { get; set; }
    public int MaxCharLength { get; set; }
    public long NonNullCount { get; set; }
    public int ProposedVarchar { get; set; }
    public bool KeepBlob { get; set; }
    public int SuggestedWave { get; set; }
    public string? EntityTypeName { get; set; }
    public string? PropertyName { get; set; }
    public TextColumnWarningKind WarningKind { get; set; } = TextColumnWarningKind.None;
    public string? WarningMessage { get; set; }
    public string? MeasureError { get; set; }

    public bool IsMigrationTarget =>
        StorageKind == TextColumnStorageKind.BlobText && !KeepBlob && WarningKind != TextColumnWarningKind.ModelOnly;

    public static List<TextColumnDescriptor> Merge(
        IReadOnlyList<TextColumnDescriptor> fromDatabase,
        IReadOnlyList<TextColumnDescriptor> fromModel)
    {
        var dbByKey = fromDatabase.ToDictionary(
            x => Key(x.Table, x.Column),
            x => x,
            StringComparer.OrdinalIgnoreCase);
        var modelByKey = fromModel.ToDictionary(
            x => Key(x.Table, x.Column),
            x => x,
            StringComparer.OrdinalIgnoreCase);

        var keys = new HashSet<string>(dbByKey.Keys, StringComparer.OrdinalIgnoreCase);
        keys.UnionWith(modelByKey.Keys);

        var result = new List<TextColumnDescriptor>(keys.Count);
        foreach (var key in keys.OrderBy(k => k, StringComparer.OrdinalIgnoreCase))
        {
            dbByKey.TryGetValue(key, out var dbRow);
            modelByKey.TryGetValue(key, out var modelRow);

            if (dbRow is not null && modelRow is not null)
            {
                dbRow.EntityTypeName = modelRow.EntityTypeName;
                dbRow.PropertyName = modelRow.PropertyName;
                result.Add(dbRow);
                continue;
            }

            if (dbRow is not null)
            {
                dbRow.WarningKind = TextColumnWarningKind.DatabaseOnly;
                dbRow.WarningMessage = "Column exists in database but not in EF model.";
                result.Add(dbRow);
                continue;
            }

            modelRow!.WarningKind = TextColumnWarningKind.ModelOnly;
            modelRow.WarningMessage = "Column expected by EF model but missing in database metadata.";
            result.Add(modelRow);
        }

        return result;
    }

    private static string Key(string table, string column) =>
        $"{table}\0{column}";
}
