using Newtonsoft.Json;

namespace Models.JSON;

public class FormTableDataMainTable12 : FormTableDataMainTable
{
    //"Сведения из паспорта на изделие из обедненного урана", "номер паспорта", "4"
    [JsonProperty("PaspN")]
    public string PassportNumber { get; set; }

    //"Сведения из паспорта на изделие из обедненного урана", "наименование", "5"
    [JsonProperty("Typ")]
    public string NameIOU { get; set; }

    //"Сведения из паспорта на изделие из обедненного урана", "номер", "6"
    [JsonProperty("Numb")] 
    public string FactoryNumber { get; set; }

    //"Сведения из паспорта на изделие из обедненного урана", "масса обедненного урана, кг", "7"
    [JsonProperty("Kg")]
    public string Mass { get; set; }

    //"Сведения из паспорта на изделие из обедненного урана", "код ОКПО изготовителя", "8"
    [JsonProperty("IzgotOKPO")]
    public string CreatorOKPO { get; set; }

    //"Сведения из паспорта на изделие из обедненного урана", "дата выпуска", "9"
    [JsonProperty("IzgotDate")]
    public string CreationDate { get; set; }

    //"Сведения из паспорта на изделие из обедненного урана", "НСС, мес", "10"
    [JsonProperty("Nss")]
    public string SignedServicePeriod { get; set; }
}
