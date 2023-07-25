using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Client_App.Commands.AsyncCommands.Import;

[JsonObject]
public class MyJsonModel2
{
    [JsonProperty("forms")]
    public List<JsonForm> Forms { get; set; }
}

[JsonConverter(typeof(BaseConverter))]
public abstract class JsonForm
{
    [JsonProperty("form_no")]
    [JsonConverter(typeof(FormNumConverter))]
    public string FormNum { get; set; }

    [JsonProperty("report_form_id")]
    public int ReportFormId { get; set; }
}

#region Form1

public abstract class JsonForm1 : JsonForm
{
}

#region Form11

public class JsonForm11 : JsonForm1
{
    [JsonProperty("form_table_data")]
    public FormTableData11 FormTableData { get; set; }
}

public class FormTableData11
{
    [JsonProperty("main_table")]
    public List<FormTableDataMainTable11> MainTable { get; set; }
}

public class FormTableDataMainTable11
{
    //"Сведения об операции", "код", "2"
    [JsonProperty("OpCod")] public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("OpDate")] public string OperationDate { get; set; }

    //"Документ","вид", "16"
    [JsonProperty("DocVid")] public string DocumentVid { get; set; }

    //"Документ", "номер", "17"
    [JsonProperty("DocN")] public string DocumentNumber { get; set; }

    //"Документ", "дата", "18"
    [JsonProperty("DocDate")] public string DocumentDate { get; set; }

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

#endregion

#region Form12

public class JsonForm12 : JsonForm1
{
    [JsonProperty("form_table_data")]
    public FormTableData12 FormTableData { get; set; }
}

public class FormTableData12
{
    [JsonProperty("main_table")]
    public List<FormTableDataMainTable12> MainTable { get; set; }
}

public class FormTableDataMainTable12
{
    //"Сведения об операции", "код", "2"
    [JsonProperty("OpCod")] public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("OpDate")] public string OperationDate { get; set; }

    //"Документ","вид", "16"
    [JsonProperty("DocVid")] public string DocumentVid { get; set; }

    //"Документ", "номер", "17"
    [JsonProperty("DocN")] public string DocumentNumber { get; set; }

    //"Документ", "дата", "18"
    [JsonProperty("DocDate")] public string DocumentDate { get; set; }

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

#endregion

#endregion

public class FormTableData
{
    [JsonProperty("main_table")]
    public List<FormTableDataMainTable> MainTable { get; set; }
}
public class FormTableDataMainTable
{
    #region Form11

    //"Сведения об операции", "код", "2"
    [JsonProperty("OpCod")]
    public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("OpDate")]
    public string OperationDate { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "номер паспорта (сертификата)", "4"
    [JsonProperty("PaspN")]
    public string PassportNumber { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "тип", "5"
    [JsonProperty("Typ")]
    public string Type { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "радионуклиды", "6"
    [JsonProperty("Nuclid")]
    public string Radionuclids { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "номер", "7"
    [JsonProperty("Numb")]
    public string FactoryNumber { get; set; }

    //"Количество, шт", "8"
    [JsonProperty("Sht")]
    public string Quantity { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "суммарная активность, Бк", "9"
    [JsonProperty("Activn")]
    public string Activity { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "код ОКПО изготовителя", "10"
    [JsonProperty("IzgotOKPO")]
    public string CreatorOKPO { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "дата выпуска", "11"
    [JsonProperty("IzgotDate")]
    public string CreationDate { get; set; }

    //"Сведения из паспорта (сертификата) на закрытый радионуклидный источник", "категория", "12"
    [JsonProperty("Kateg")]
    public string Category { get; set; }

    //"НСС, месяцев", "13"
    [JsonProperty("Nss")]
    public string SignedServicePeriod { get; set; }

    //"Право собственности на ЗРИ", "код формы собственности", "14"
    [JsonProperty("FormSobst")]
    public string PropertyCode { get; set; }

    //"Право собственности на ЗРИ", "код ОКПО правообладателя", "15"
    [JsonProperty("Pravoobl")]
    public string Owner { get; set; }

    //"Документ","вид", "16"
    [JsonProperty("DocVid")]
    public string DocumentVid { get; set; }

    //"Документ", "номер", "17"
    [JsonProperty("DocN")]
    public string DocumentNumber { get; set; }

    //"Документ", "дата", "18"
    [JsonProperty("DocDate")]
    public string DocumentDate { get; set; }

    //"Код ОКПО", "поставщика или получателя", "19"
    [JsonProperty("OkpoPIP")]
    public string ProviderOrRecieverOKPO { get; set; }

    //"Код ОКПО", "перевозчика", "20"
    [JsonProperty("OkpoPrv")]
    public string TransporterOKPO { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "наименование", "21"
    [JsonProperty("PrName")]
    public string PackName { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "тип", "22"
    [JsonProperty("UktPrTyp")]
    public string PackType { get; set; }

    //"Прибор (установка), УКТ или иная упаковка", "номер", "23"
    [JsonProperty("UktPrN")]
    public string PackNumber { get; set; }

    #endregion

    #region Form12

    //"Сведения из паспорта на изделие из обедненного урана", "масса обедненного урана, кг", "7"
    [JsonProperty("Kg")]
    public string Mass { get; set; }

    #endregion

    #region Form13

    //"Сведения из паспорта на открытый радионуклидный источник", "агрегатное состояние", "11"
    [JsonProperty("Agr")]
    public string AggregateState { get; set; }

    #endregion

    #region Form14

    //"Сведения из паспорта на открытый радионуклидный источник", "наименование","5"
    [JsonProperty("IstName")]
    public string Name { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "дата измерения активности", "9"
    [JsonProperty("ActDate")]
    public string ActivityMeasurementDate { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "объем, куб.м", "10"
    [JsonProperty("Kbm")]
    public string Volume { get; set; }

    //"Сведения из паспорта на открытый радионуклидный источник", "агрегатное состояние", "12"
    [JsonProperty("Arg")]
    private string AggregateState2 { set => AggregateState = value; }

    #endregion

    #region Form15

    //"null-11","Статус РАО","11"
    [JsonProperty("BCod")]
    public string StatusRAO { get; set; }

    //"Пункт хранения", "наименование", "20"
    [JsonProperty("PH_Name")]
    public string StoragePlaceName { get; set; }

    //"Пункт хранения", "код","21"
    [JsonProperty("PH_Cod")]
    public string PH_Cod { get; set; }

    //"null-22", "Код переработки / сортировки РАО", "22"
    [JsonProperty("CodRAO")]
    public string RefineOrSortRAOCode { get; set; }

    //"null-23", "Субсидия, %", "23"
    [JsonProperty("Subsid")]
    public string Subsidy { get; set; }

    //"null-24", "Номер мероприятия ФЦП", "24"
    [JsonProperty("FCP")]
    public string FcpNumber { get; set; }

    #endregion

    #region Form16

    //"null-4","Код РАО","4"
    [JsonProperty("RAOCod")]
    public string CodeRAO { get; set; }

    //"Количество", "масса без упаковки (нетто), т", "7"
    [JsonProperty("Tonne")]
    public string Mass2 { set => Mass = value; }

    //"null-9", "Основные радионуклиды", "9"
    [JsonProperty("RAOCodMax")]
    public string MainRadionuclids { get; set; }

    //"Суммарная активность, Бк", "тритий", "10"
    [JsonProperty("ActTR")]
    public string TritiumActivity { get; set; }

    //"Суммарная активность, Бк", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "11"
    [JsonProperty("ActBG")]
    public string BetaGammaActivity { get; set; }

    //"Суммарная активность, Бк", "альфа-излучающие радионуклиды (исключая трансурановые)", "12"
    [JsonProperty("ActA")]
    public string AlphaActivity { get; set; }

    //"Суммарная активность, Бк", "трансурановые радионуклиды", "13"
    [JsonProperty("ActUR")]
    public string TransuraniumActivity { get; set; }

    //"null-22", "Код переработки / сортировки РАО", "22"
    [JsonProperty("UpCod")]
    private string RefineOrSortRAOCode2 { set => RefineOrSortRAOCode = value; }

    #endregion

    #region Form17

    //"Сведения об операции", "код", "2"
    [JsonProperty("g2")]
    private string OperationCode2 { set => OperationCode = value; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("g3")]
    private string OperationDate2 { set => OperationDate = value; }

    //"Сведения об упаковке", "null-4", "наименование","4"
    [JsonProperty("g4")]
    private string PackName2 { set => PackName = value; }

    //"Сведения об упаковке", "null-5", "тип", "5"
    [JsonProperty("g5")]
    private string PackType2 { set => PackType = value; }

    //"Сведения об упаковке", "null-6", "заводской номер", "6"
    [JsonProperty("g6")]
    public string PackFactoryNumber { get; set; }

    //"Сведения об упаковке", "null-7", "номер упаковки (идентификационный код)", "7"
    [JsonProperty("g7")]
    private string PackNumber2 { set => PackNumber = value; }

    //"Сведения об упаковке", "null-8", "дата формирования", "8"
    [JsonProperty("g8")]
    public string FormingDate { get; set; }

    //"Сведения об упаковке", "null-9", "номер паспорта", "9"
    [JsonProperty("g9")]
    private string PassportNumber2 { set => PassportNumber = value; }

    //"Сведения об упаковке", "null-10", "объем, куб. м", "10"
    [JsonProperty("g10")]
    private string Volume2 { set => Volume = value; }

    //"Сведения об упаковке", "null-11", "масса брутто, т", "11"
    [JsonProperty("g11")]
    private string Mass3 { set => Mass = value; }

    //"Сведения об упаковке", "Радионуклидный состав", "наименование радионуклида","12"
    [JsonProperty("g12")]
    private string Radionuclids2
    {
        set
        {
            Radionuclids = value;
            
        }
    }

    #endregion
}

#region Convertors

public class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
{
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (typeof(JsonForm).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
        return base.ResolveContractConverter(objectType);
    }
}

public class BaseConverter : JsonConverter
{
    static JsonSerializerSettings SpecifiedSubclassConversion = new() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(JsonForm));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        return jo["form_no"].Value<string>() switch
        {
            "form_1_1_2022" => JsonConvert.DeserializeObject<JsonForm11>(jo.ToString(), SpecifiedSubclassConversion),
            "form_1_2_2022" => JsonConvert.DeserializeObject<JsonForm12>(jo.ToString(), SpecifiedSubclassConversion),
            _ => throw new Exception()
        };
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException(); // won't be called because CanWrite returns false
    }
}

public class FormNumConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(string);

    public override object? ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader) ?? throw new Exception("Cannot deserialize type string");
        Regex reg;
        if ((reg = new Regex("^form_([1-2])_([0-9])_2022$")).IsMatch(value))
        {
            var matches = Regex.Matches(value, reg.ToString());
            return $"{matches[0].Groups[1]}.{matches[0].Groups[2]}";
        }
        throw new Exception("Cannot deserialize type string");
    }
    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

#endregion