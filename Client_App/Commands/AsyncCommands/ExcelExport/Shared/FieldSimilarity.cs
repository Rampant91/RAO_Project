using System;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Shared;

public readonly record struct FieldSimilarity(double Score, FieldMatchLevel Level)
{
    public static FieldSimilarity Exact => new(1.0, FieldMatchLevel.Exact);

    public static FieldSimilarity Near(double score) =>
        new(Math.Clamp(score, 0, 0.999), FieldMatchLevel.Near);

    public static FieldSimilarity Mismatch(double score = 0) =>
        new(Math.Clamp(score, 0, 1), FieldMatchLevel.Mismatch);
}
