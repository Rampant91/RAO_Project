using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain13 : TableData
{
    //"Сведения об операции", "код", "2"
    [JsonProperty("OpCod")] public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("OpDate")] public string OperationDate { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник","номер паспорта (сертификата)", "4"
    [JsonProperty("PaspN")] public string PassportNumber { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "тип", "5"
    [JsonProperty("Typ")] public string Type { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "радионуклиды", "6"
    [JsonProperty("Nuclid")] public string Radionuclids { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "номер", "7"
    [JsonProperty("Numb")] public string FactoryNumber { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "активность, Бк", "8"
    [JsonProperty("Activn")] public string Activity { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "код ОКПО изготовителя", "9"
    [JsonProperty("IzgotOKPO")] public string CreatorOKPO { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "дата выпуска", "10"
    [JsonProperty("IzgotDate")] public string CreationDate { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "агрегатное состояние", "11"
    [JsonProperty("Agr")] public string AggregateState { get; set; }

    //"Право собственности на ОРИ", "код формы собственности", "12"
    [JsonProperty("FormSobst")] public string PropertyCode { get; set; }

    //"Право собственности на ОРИ", "код ОКПО правообладателя", "13"
    [JsonProperty("Pravoobl")] public string Owner { get; set; }

    //"Документ","вид", "14"
    [JsonProperty("DocVid")] public string DocumentVid { get; set; }

    //"Документ", "номер", "15"
    [JsonProperty("DocN")] public string DocumentNumber { get; set; }

    //"Документ", "дата", "16"
    [JsonProperty("DocDate")] public string DocumentDate { get; set; }

    //"Код ОКПО", "поставщика или получателя", "17"
    [JsonProperty("OkpoPIP")] public string ProviderOrRecieverOKPO { get; set; }

    //"Код ОКПО", "перевозчика","18"
    [JsonProperty("OkpoPrv")] public string TransporterOKPO { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "наименование", "19"
    [JsonProperty("PrName")] public string PackName { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "тип", "20"
    [JsonProperty("UktPrTyp")] public string PackType { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "номер","21"
    [JsonProperty("UktPrN")] public string PackNumber { get; set; }
}