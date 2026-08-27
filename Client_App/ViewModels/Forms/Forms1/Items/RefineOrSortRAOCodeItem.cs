namespace Client_App.ViewModels.Forms.Forms1.Items;

public class RefineOrSortRAOCodeItem
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public override string ToString()
    {
        return Code;
    }
}
