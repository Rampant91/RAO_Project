namespace Client_App.ViewModels.Forms.Forms1.Items;

/// <summary>
/// Категория опасности с описанием
/// </summary>
public class CategoryItem
{
    public short? Code { get; set; }
    public string Description { get; set; } = string.Empty;

    public override string ToString() => Code?.ToString() ?? string.Empty;
}
