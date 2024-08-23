using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using Models.JSON.ExecutorData;
using Models.JSON.TableDataMain;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonConverterAttribute = Newtonsoft.Json.JsonConverterAttribute;

namespace Models.JSON;

[JsonConverter(typeof(JsonFormConverter))]
public abstract class JsonForm
{
    #region ExecutorDataTable

    [JsonProperty("form_main_data")]
    public ExecutorData.ExecutorData ExecutorData { get; set; }

    #endregion

    #region TableData

    [JsonProperty("form_table_data")]
    public FormTable FormTable { get; set; }

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

    private class JsonFormSpecifiedConcreteClassConverter : DefaultContractResolver
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

    private class JsonFormConverter : JsonConverter
    {
        private static readonly JsonSerializerSettings SpecifiedSubclassConversion = new()
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
            switch (jo["form_no"]!.Value<string>())
            {
                case "form_1_1_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm11>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain11>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }

                    return jsonForm;
                }
                case "form_1_2_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm12>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain12>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_1_3_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm13>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain13>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_1_4_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm14>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain14>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_1_5_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm15>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain15>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_1_6_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm16>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain16>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_1_7_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm17>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain17>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_1_8_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm18>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain18>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_1_9_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm19>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain19>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_2_1_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm21>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain21>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_2_2_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm22>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain22>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_2_3_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm23>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain23>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_2_4_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm24>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain24>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_2_5_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm25>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain25>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_2_6_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm26>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain26>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    jsonForm.ExecutorData = jo["form_main_data"]!.ToObject<ExecutorData26>(serializer);
                    return jsonForm;
                }
                case "form_2_7_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm27>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain27>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    jsonForm.ExecutorData = jo["form_main_data"]!.ToObject<ExecutorData27>(serializer);
                    return jsonForm;
                }
                case "form_2_8_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm28>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain28>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    jsonForm.ExecutorData = jo["form_main_data"]!.ToObject<ExecutorData28>(serializer);
                    return jsonForm;
                }
                case "form_2_9_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm29>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain29>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_2_10_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm210>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain210>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_2_11_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm211>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain211>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                case "form_2_12_2022":
                {
                    var jsonForm = JsonConvert.DeserializeObject<JsonForm212>(jo.ToString(), SpecifiedSubclassConversion);
                    jsonForm.FormTable.TableData.Clear();
                    foreach (var tableDataMain in jo["form_table_data"]!["main_table"]!.ToObject<List<TableDataMain212>>(serializer))
                    {
                        jsonForm.FormTable.TableData.Add(tableDataMain);
                    }
                    return jsonForm;
                }
                default: return null;
            }
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

#region JsonForm1

public abstract class JsonForm1 : JsonForm;

public class JsonForm11 : JsonForm1;

public class JsonForm12 : JsonForm1;

public class JsonForm13 : JsonForm1;

public class JsonForm14 : JsonForm1;

public class JsonForm15 : JsonForm1;

public class JsonForm16 : JsonForm1;

public class JsonForm17 : JsonForm1;

public class JsonForm18 : JsonForm1;

public class JsonForm19 : JsonForm1;

#endregion

#region JsonForm2

public abstract class JsonForm2 : JsonForm;

public class JsonForm21 : JsonForm2;

public class JsonForm22 : JsonForm2;

public class JsonForm23 : JsonForm2;

public class JsonForm24 : JsonForm2;

public class JsonForm25 : JsonForm2;

public class JsonForm26 : JsonForm2;

public class JsonForm27 : JsonForm2;

public class JsonForm28 : JsonForm2;

public class JsonForm29 : JsonForm2;

public class JsonForm210 : JsonForm2;

public class JsonForm211 : JsonForm2;

public class JsonForm212 : JsonForm2;

#endregion

#region Comment

public class CommentText
{
    [JsonProperty("text")]
    public string Comments { get; set; }
} 

#endregion

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