namespace Client_App.ViewModels.Forms.Forms4.Items;

/// <summary>
/// Code of RF subject with full name for dropdown display and search.
/// </summary>
public class SubjectRfCodeItem
{
    public string Code { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public override string ToString() => $"{Code} {Description}";
}
