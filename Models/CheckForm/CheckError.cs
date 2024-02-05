namespace Models.CheckForm;

public class CheckError
{
    public string FormNum { get; set; } = "";
    public int Index { get; set; }
    public string Row { get; set; } = "";
    public string Column { get; set; } = "";
    public string? Value { get; set; } = "";
    public string Message { get; set; } = "";
}