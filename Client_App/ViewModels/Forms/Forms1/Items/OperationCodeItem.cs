namespace Client_App.ViewModels.Forms.Forms1.Items;

/// <summary>
/// Код операции с описанием
/// </summary>
public class OperationCodeItem
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public override string ToString() => Code;
}
