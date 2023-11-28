namespace Models.DTO;

public class Form11DTO
{
    #region ReportsProperties

    public string RegNoRep { get; set; }

    public string ShortJurLico { get; set; }

    public string OkpoRep { get; set; }

    #endregion


    #region ReportProperties

    public string FormNum { get; set; }

    public string StartPeriod { get; set; }

    public string EndPeriod { get; set; }

    public byte CorrectionNumber { get; set; }

    public int RowCount { get; set; }

    #endregion

    #region Form11Properties

    public int NumberInOrder { get; set; }

    public string OperationCode { get; set; }

    public string OperationDate { get; set; }

    public string PassportNumber { get; set; }

    public string Type { get; set; }

    public string Radionuclids { get; set; }

    public string FactoryNumber { get; set; }

    public int? Quantity { get; set; }

    public string Activity { get; set; }

    public string CreatorOKPO { get; set; }

    public string CreationDate { get; set; }

    public short? Category { get; set; }

    public float? SignedServicePeriod { get; set; }

    public byte? PropertyCode { get; set; }

    public string Owner { get; set; }

    public byte? DocumentVid { get; set; }

    public string DocumentNumber { get; set; }

    public string DocumentDate { get; set; }

    public string ProviderOrRecieverOKPO { get; set; }

    public string TransporterOKPO { get; set; }

    public string PackName { get; set; }

    public string PackType { get; set; }

    public string PackNumber { get; set; } 

    #endregion
}