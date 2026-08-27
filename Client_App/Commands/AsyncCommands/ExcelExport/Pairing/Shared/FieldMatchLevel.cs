namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;

/// <summary>Уровень совпадения поля для подсветки closest (зелёный / жёлтый / красный).</summary>
public enum FieldMatchLevel
{
    Exact = 0,
    Near = 1,
    Mismatch = 2
}
