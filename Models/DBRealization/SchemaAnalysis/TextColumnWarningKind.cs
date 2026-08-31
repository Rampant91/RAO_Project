namespace Models.DBRealization.SchemaAnalysis;

public enum TextColumnWarningKind
{
    None,
    ModelOnly,
    DatabaseOnly,
    TableNotFound,
    MeasureFailed
}
