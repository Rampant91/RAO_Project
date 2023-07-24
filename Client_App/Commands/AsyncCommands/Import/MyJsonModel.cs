using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client_App.Commands.AsyncCommands.Import;

public class MyJsonModel
{
    [JsonProperty("forms")]
    //[JsonConverter(typeof(FormsConverter))]
    public JsonForm[] Forms { get; set; }

    [JsonProperty("orgs")]
    public Dictionary<string, IList<object>> Orgs { get; set; }

    [JsonProperty("vocs")]
    public IList<object> Vocs { get; set; }
}

public class JsonForm
{
    [JsonProperty("form_no")]
    [JsonConverter(typeof(FormNumConverter))]
    public string FormNum { get; set; }

    [JsonProperty("report_form_id")]
    public int ReportFormId { get; set; }

    [JsonProperty("form_table_data")]
    public FormTableData FormTableData { get; set; }
}

#region Form1

//public class JsonForm1 : JsonForm
//{
//    [JsonProperty("form_table_data")]
//    public FormTableData FormTableData { get; set; }
//}

//public class JsonForm11 : JsonForm1
//{

//}

public partial class FormTableData
{
    [JsonProperty("main_table")]
    public List<FormTableDataMainTable> MainTable { get; set; }
}

public partial class FormTableDataMainTable
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

    //"Пункт хранения", "наименование","20"
    [JsonProperty("PH_Name")]
    public string StoragePlaceName { get; set; }

    //"Пункт хранения", "код","21"
    [JsonProperty("PH_Cod")]
    public string PH_Cod { get; set; }

    //"null-22", "Код переработки / сортировки РАО", "22"
    [JsonProperty("CodRAO")]
    public string RefineOrSortRAOCode { get; set; }

    //"null-23","Субсидия, %","23"
    [JsonProperty("Subsid")]
    public string Subsidy { get; set; }

    //"null-24","Номер мероприятия ФЦП","24"
    [JsonProperty("FCP")]
    public string FcpNumber { get; set; }

    #endregion
}

#endregion

#region Form2

public abstract class JsonForm2 : JsonForm
{
    [JsonProperty("adjustment_no")]
    public byte CorrectionNumber { get; set; }
}

#endregion

#region Convertors

public class FormsConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(JsonForm);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var a = objectType;
        var jsonFormlist = new List<JsonForm>();

        while (reader.Read() && reader.TokenType is not (JsonToken.EndArray or JsonToken.EndObject))
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                var item = serializer.Deserialize<JsonForm>(reader);
                
                if (item is not null)
                {
                    jsonFormlist.Add(item);
                }
            }
        }
        return jsonFormlist;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
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