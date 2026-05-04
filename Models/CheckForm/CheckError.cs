namespace Models.CheckForm;

public class CheckError
{
    public string FormNum { get; set; } = "";

    public int Index { get; set; }

    public string Row { get; set; } = "";

    public string Column { get; set; } = "";

    //Для форм 4.1
    public string RegNo { get; set; } = "";

    //Для форм 4.1
    public string Okpo { get; set; } = "";

    public string? Value { get; set; } = "";

    //Для форм 4.1
    public string? DbValue { get; set; } = "";

    public string Message { get; set; } = "";

    public bool IsCritical { get; set; } = false;
}