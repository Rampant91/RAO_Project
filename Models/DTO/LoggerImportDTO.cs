namespace Models.DTO;

//  DTO для передачи в LoggerImport
public class LoggerImportDTO
{
    public string Act { get; set; }
    public byte CorNum { get; set; }
    public int CurrentLogLine { get; set; }
    public string EndPeriod { get; set; }
    public int FormCount { get; set; }
    public string FormNum { get; set; }
    public string Period => $"{StartPeriod} - {EndPeriod}";
    public string PeriodOrYear => FormNum[0] is '1' ? Period : Year;
    public string StartPeriod { get; set; }
    public string Okpo { get; set; }
    public string OperationDate { get; set; }
    public string RegNum { get; set; }
    public string ShortName { get; set; }
    public string SourceFileFullPath { get; set; }
    public string Year { get; set; }
}
