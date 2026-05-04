namespace Client_App.ViewModels.Forms.Forms1.Items;

public class OwnershipItem
{
    public byte Code { get; set; }
    public string Description { get; set; } = string.Empty;

    public override string ToString()
    {
        return Code.ToString();
    }
}
