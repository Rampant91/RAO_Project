using Newtonsoft.Json;

namespace Models.JSON;

public class FormTableDataMainTable11 : FormTableDataMainTable
{
    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "номер паспорта (сертификата)", "4"
    [JsonProperty("PaspN")] public string PassportNumber { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "тип", "5"
    [JsonProperty("Typ")] public string Type { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "радионуклиды", "6"
    [JsonProperty("Nuclid")] public string Radionuclids { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "номер", "7"
    [JsonProperty("Numb")] public string FactoryNumber { get; set; }

    //"Количество, шт", "8"
    [JsonProperty("Sht")] public string Quantity { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "суммарная активность, Бк", "9"
    [JsonProperty("Activn")] public string Activity { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "код ОКПО изготовителя", "10"
    [JsonProperty("IzgotOKPO")] public string CreatorOKPO { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "дата выпуска", "11"
    [JsonProperty("IzgotDate")] public string CreationDate { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "категория", "12"
    [JsonProperty("Kateg")] public string Category { get; set; }

    //"НСС, месяцев", "13"
    [JsonProperty("Nss")] public string SignedServicePeriod { get; set; }

    //"Право собственности на ЗРИ", "код формы собственности", "14"
    [JsonProperty("FormSobst")] public string PropertyCode { get; set; }

    //"Право собственности на ЗРИ", "код ОКПО правообладателя", "15"
    [JsonProperty("Pravoobl")] public string Owner { get; set; }

    //"Код ОКПО", "поставщика или получателя", "19"
    [JsonProperty("OkpoPIP")] public string ProviderOrRecieverOKPO { get; set; }

    //"Код ОКПО", "перевозчика", "20"
    [JsonProperty("OkpoPrv")] public string TransporterOKPO { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "наименование", "21"
    [JsonProperty("PrName")] public string PackName { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "тип", "22"
    [JsonProperty("UktPrTyp")] public string PackType { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "номер", "23"
    [JsonProperty("UktPrN")] public string PackNumber { get; set; }
}