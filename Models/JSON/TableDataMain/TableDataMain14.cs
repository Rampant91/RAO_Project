using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain14 : TableData
{
    //"Сведения об операции", "код", "2"
    [JsonProperty("OpCod")] public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("OpDate")] public string OperationDate { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "номер паспорта", "4"
    [JsonProperty("PaspN")] public string PassportNumber { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "наименование", "5"
    [JsonProperty("IstName")] public string Name { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "вид", "6"
    [JsonProperty("Typ")] public string Sort { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "радионуклиды", "7"
    [JsonProperty("Nuclid")] public string Radionuclids { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "активность, Бк", "8"
    [JsonProperty("Activn")] public string Activity { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "дата измерения активности", "9"
    [JsonProperty("ActDate")] public string ActivityMeasurementDate { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "объем, куб. м", "10"
    [JsonProperty("Kbm")] public string Volume { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "масса, кг", "11"
    [JsonProperty("Kg")] public string Mass { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "агрегатное состояние", "12"
    [JsonProperty("Arg")] public string AggregateState { get; set; }

    //"Право собственности на ОРИ", "код формы собственности", "13"
    [JsonProperty("FormSobst")] public string PropertyCode { get; set; }

    //"Право собственности на ОРИ", "код ОКПО правообладателя", "14"
    [JsonProperty("Pravoobl")] public string Owner { get; set; }

    //"Документ","вид", "15"
    [JsonProperty("DocVid")] public string DocumentVid { get; set; }

    //"Документ", "номер", "16"
    [JsonProperty("DocN")] public string DocumentNumber { get; set; }

    //"Документ", "дата", "17"
    [JsonProperty("DocDate")] public string DocumentDate { get; set; }

    //"Код ОКПО", "поставщика или получателя", "18"
    [JsonProperty("OkpoPIP")] public string ProviderOrRecieverOKPO { get; set; }

    //"Код ОКПО", "перевозчика", "19"
    [JsonProperty("OkpoPrv")] public string TransporterOKPO { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "наименование", "20"
    [JsonProperty("PrName")] public string PackName { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "тип", "21"
    [JsonProperty("UktPrTyp")] public string PackType { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "номер упаковки", "22"
    [JsonProperty("UktPrN")] public string PackNumber { get; set; }
}