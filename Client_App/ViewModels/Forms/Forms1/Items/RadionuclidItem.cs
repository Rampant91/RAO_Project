namespace Client_App.ViewModels.Forms.Forms1.Items;

/// <summary>
/// Радионуклид для списка выбора
/// </summary>
public class RadionuclidItem
{
    public string Name { get; set; } = string.Empty;

    public override string ToString() => Name;
}
