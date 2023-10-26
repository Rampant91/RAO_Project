using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain15 : TableDataMain
{
    //"Сведения об операции", "код", "2"
    [JsonProperty("OpCod")] 
    public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("OpDate")] 
    public string OperationDate { get; set; }

    //"Сведения об отработавших закрытых источниках ионизирующего излучения", "номер паспорта (сертификата) ЗРИ, акта определения характеристик ОЗИИ", "4"
    [JsonProperty("PaspN")]
    public string PassportNumber { get; set; }

    //"Сведения об отработавших закрытых источниках ионизирующего излучения", "тип", "5"
    [JsonProperty("Typ")]
    public string Type { get; set; }

    //"Сведения об отработавших закрытых источниках ионизирующего излучения", "радионуклиды", "6"
    [JsonProperty("Nuclid")]
    public string Radionuclids { get; set; }

    //"Сведения об отработавших закрытых источниках ионизирующего излучения", "номер", "7"
    [JsonProperty("Numb")]
    public string FactoryNumber { get; set; }

    //"Сведения об отработавших закрытых источниках ионизирующего излучения", "количество, шт", "8"
    [JsonProperty("Sht")]
    public string Quantity { get; set; }

    //"Сведения об отработавших закрытых источниках ионизирующего излучения", "суммарная активность, Бк","9"
    [JsonProperty("Activn")]
    public string Activity { get; set; }

    //"Сведения об отработавших закрытых источниках ионизирующего излучения", "дата выпуска","10"
    [JsonProperty("IzgotDate")]
    public string CreationDate { get; set; }

    //"null-11","Статус РАО","11"
    [JsonProperty("BCod")]
    public string StatusRAO { get; set; }

    //"Документ","вид", "12"
    [JsonProperty("DocVid")] 
    public string DocumentVid { get; set; }

    //"Документ", "номер", "13"
    [JsonProperty("DocN")] 
    public string DocumentNumber { get; set; }

    //"Документ", "дата", "14"
    [JsonProperty("DocDate")] 
    public string DocumentDate { get; set; }

    //"Код ОКПО","поставщика или получателя", "15"
    [JsonProperty("OkpoPIP")] 
    public string ProviderOrRecieverOKPO { get; set; }

    //"Код ОКПО", "перевозчика", "16"
    [JsonProperty("OkpoPrv")] 
    public string TransporterOKPO { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "наименование","17"
    [JsonProperty("PrName")] 
    public string PackName { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "тип", "18"
    [JsonProperty("UktPrTyp")]
    public string PackType { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "заводской номер", "19"
    [JsonProperty("UktPrN")]
    public string PackNumber { get; set; }

    //"Пункт хранения", "наименование","20"
    [JsonProperty("PH_Name")]
    public string StoragePlaceName { get; set; }

    //"Пункт хранения", "код","21"
    [JsonProperty("PH_Cod")]
    public string StoragePlaceCode { get; set; }

    //"null-22", "Код переработки / сортировки РАО", "22"
    [JsonProperty("CodRAO")]
    public string RefineOrSortRAOCode { get; set; }

    //"null-23","Субсидия, %","23"
    [JsonProperty("Subsid")]
    public string Subsidy { get; set; }

    //"null-24","Номер мероприятия ФЦП", "24"
    [JsonProperty("FCP")]
    public string FcpNumber { get; set; }
}