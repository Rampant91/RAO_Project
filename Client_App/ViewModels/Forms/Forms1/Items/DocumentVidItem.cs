namespace Client_App.ViewModels.Forms.Forms1.Items;

public class DocumentVidItem
{
    public byte? Code { get; set; }
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Отображаемый код. Для null возвращает "-" (прочерк).
    /// </summary>
    public string DisplayCode => Code?.ToString() ?? "-";

    public override string ToString()
    {
        return DisplayCode;
    }
}
