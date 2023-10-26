using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonConverterAttribute = Newtonsoft.Json.JsonConverterAttribute;

namespace Models.JSON;

[JsonConverter(typeof(JsonFormConverter))]
public abstract class JsonForm
{
    #region ExecutorDataTable

    [JsonProperty("form_main_data")]
    public ExecutorData ExecutorData { get; set; }

    #endregion

    #region Properties

    #region Comments

    [JsonProperty("form_comment")]
    public CommentText CommentText { get; set; }

    #endregion

    #region CorrectionNumber

    [JsonProperty("adjustment_no")]
    public byte CorrectionNumber { get; set; }

    #endregion

    #region FormNum

    [JsonProperty("form_no")]
    [JsonConverter(typeof(FormNumConverter))]
    public string FormNum { get; set; }

    #endregion

    #region ExportDate

    [JsonProperty("form_export_ts")]
    public string ExportDate { get; set; }

    #endregion

    #region Notes

    [JsonProperty("form_remarks")]
    public NotesMainTable NotesMainTable { get; set; }

    #endregion

    #region RegNoRep

    [JsonProperty("kod_org")]
    public string RegNoRep { get; set; }

    #endregion

    #region ReportFormId

    [JsonProperty("report_form_id")]
    public int ReportFormId { get; set; }

    #endregion

    #region StartPeriod

    [JsonProperty("form_period_start")]
    public string StartPeriod { get; set; }

    #endregion

    #region EndPeriod

    [JsonProperty("form_period_end")]
    public string EndPeriod { get; set; }

    #endregion

    #region Year

    [JsonProperty("reporting_year")]
    public string Year { get; set; }

    #endregion

    #region ReportsId

    [JsonProperty("author_id")]
    public int ReportsId { get; set; }

    #endregion

    #endregion

    #region ContractResolver

    public class JsonFormSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(JsonForm).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }

    #endregion

    #region Convertors

    #region JsonFormConverter

    public class JsonFormConverter : JsonConverter
    {
        private static JsonSerializerSettings SpecifiedSubclassConversion = new()
        {
            ContractResolver = new JsonFormSpecifiedConcreteClassConverter()
        };

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
                "form_1_3_2022" => JsonConvert.DeserializeObject<JsonForm13>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_4_2022" => JsonConvert.DeserializeObject<JsonForm14>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_5_2022" => JsonConvert.DeserializeObject<JsonForm15>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_6_2022" => JsonConvert.DeserializeObject<JsonForm16>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_7_2022" => JsonConvert.DeserializeObject<JsonForm17>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_8_2022" => JsonConvert.DeserializeObject<JsonForm18>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_9_2022" => JsonConvert.DeserializeObject<JsonForm19>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_1_2022" => JsonConvert.DeserializeObject<JsonForm21>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_2_2022" => JsonConvert.DeserializeObject<JsonForm22>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_3_2022" => JsonConvert.DeserializeObject<JsonForm23>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_4_2022" => JsonConvert.DeserializeObject<JsonForm24>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_5_2022" => JsonConvert.DeserializeObject<JsonForm25>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_6_2022" => JsonConvert.DeserializeObject<JsonForm26>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_7_2022" => JsonConvert.DeserializeObject<JsonForm27>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_8_2022" => JsonConvert.DeserializeObject<JsonForm28>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_9_2022" => JsonConvert.DeserializeObject<JsonForm29>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_10_2022" => JsonConvert.DeserializeObject<JsonForm210>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_11_2022" => JsonConvert.DeserializeObject<JsonForm211>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_12_2022" => JsonConvert.DeserializeObject<JsonForm212>(jo.ToString(), SpecifiedSubclassConversion),
                _ => null
            };
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }

    #endregion

    #region FormNumConverter

    private class FormNumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(string);

        public override object? ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader) ?? throw new Exception("Cannot deserialize type string");
            Regex reg;
            if ((reg = new Regex("^form_([1-2])_([0-9](?:(?<=1)[0-2])?)_2022$")).IsMatch(value))
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

    #endregion
}

public class CommentText
{
    [JsonProperty("text")]
    public string Comments { get; set; }
}

#region Note

public class NotesMainTable
{
    [JsonProperty("main_table")]
    public IList<Note> Notes { get; set; }
}

public class Note
{
    [JsonProperty("value")]
    public NoteValue NoteValue { get; set; }

    [JsonProperty("pointer")]
    public NotePointer NotePointer { get; set; }
}

public class NoteValue
{
    [JsonProperty("date")]
    public string Date { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("user")]
    public string User { get; set; }
}

public class NotePointer
{
    [JsonProperty("row")]
    public string Row { get; set; }

    [JsonProperty("col_name")]
    public string ColName { get; set; }
}

#endregion

#region JsonForm1

public abstract class JsonForm1 : JsonForm
{
    
}

public class JsonForm11 : JsonForm1
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData11 TableData { get; set; }

    #endregion
}

public class JsonForm12 : JsonForm1
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData12 TableData { get; set; }

    #endregion
}

public class JsonForm13 : JsonForm1
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData13 TableData { get; set; }

    #endregion
}

public class JsonForm14 : JsonForm1
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData14 TableData { get; set; }

    #endregion
}

public class JsonForm15 : JsonForm1
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData15 TableData { get; set; }

    #endregion
}

public class JsonForm16 : JsonForm1
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData16 TableData { get; set; }

    #endregion
}

public class JsonForm17 : JsonForm1
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData17 TableData { get; set; }

    #endregion
}

public class JsonForm18 : JsonForm1
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData18 TableData { get; set; }

    #endregion
}

public class JsonForm19 : JsonForm1
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData19 TableData { get; set; }

    #endregion
}

#endregion

#region JsonForm2

public abstract class JsonForm2 : JsonForm
{
    [JsonProperty("form_main_data")]
    public ExecutorData ExecutorData { get; set; }
}

public class JsonForm21 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData21 TableData { get; set; }

    #endregion
}

public class JsonForm22 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData22 TableData { get; set; }

    #endregion
}

public class JsonForm23 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData23 TableData { get; set; }

    #endregion
}

public class JsonForm24 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData24 TableData { get; set; }

    #endregion
}

public class JsonForm25 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData25 TableData { get; set; }

    #endregion
}

public class JsonForm26 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData26 TableData { get; set; }

    #endregion
}

public class JsonForm27 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData27 TableData { get; set; }

    #endregion
}

public class JsonForm28 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData28 TableData { get; set; }

    #endregion
}

public class JsonForm29 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData29 TableData { get; set; }

    #endregion
}

public class JsonForm210 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData210 TableData { get; set; }

    #endregion
}

public class JsonForm211 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData211 TableData { get; set; }

    #endregion
}

public class JsonForm212 : JsonForm2
{
    #region TableData

    [JsonProperty("form_table_data")]
    public TableData212 TableData { get; set; }

    #endregion
}

#endregion