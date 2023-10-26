using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain12 : TableDataMain
{
    //"Сведения об операции", "код", "2"
    [JsonProperty("OpCod")]
    public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("OpDate")]
    public string OperationDate { get; set; }

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

    //"Право собственности на ИОУ","код формы собственности", "11"
    [JsonProperty("FormSobst")]
    public string PropertyCode { get; set; }

    //"Право собственности на ИОУ", "код ОКПО правообладателя", "12"
    [JsonProperty("Pravoobl")]
    public string Owner { get; set; }

    //"Документ","вид", "13"
    [JsonProperty("DocVid")]
    public string DocumentVid { get; set; }

    //"Документ", "номер", "14"
    [JsonProperty("DocN")]
    public string DocumentNumber { get; set; }

    //"Документ", "дата", "15"
    [JsonProperty("DocDate")]
    public string DocumentDate { get; set; }

    //"Код ОКПО","поставщика или получателя", "16"
    [JsonProperty("OkpoPIP")]
    public string ProviderOrRecieverOKPO { get; set; }

    //"Код ОКПО", "перевозчика", "17"
    [JsonProperty("OkpoPrv")]
    public string TransporterOKPO { get; set; }

    //"Прибор (установка), УКТ или иная упаковка","наименование", "18"
    [JsonProperty("PrName")]
    public string PackName { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "тип", "19"
    [JsonProperty("UktPrTyp")]
    public string PackType { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "номер", "20"
    [JsonProperty("UktPrN")]
    public string PackNumber { get; set; }
}
