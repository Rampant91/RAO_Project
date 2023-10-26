using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static Models.JSON.JsonForm;
using System.Text.RegularExpressions;
using System;

namespace Models.JSON.TableDataMain;


public abstract class TableDataMain
{
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
                "form_1_1_2022" => JsonConvert.DeserializeObject<TableDataMain11>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_2_2022" => JsonConvert.DeserializeObject<TableDataMain12>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_3_2022" => JsonConvert.DeserializeObject<TableDataMain13>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_4_2022" => JsonConvert.DeserializeObject<TableDataMain14>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_5_2022" => JsonConvert.DeserializeObject<TableDataMain15>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_6_2022" => JsonConvert.DeserializeObject<TableDataMain16>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_7_2022" => JsonConvert.DeserializeObject<TableDataMain17>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_8_2022" => JsonConvert.DeserializeObject<TableDataMain18>(jo.ToString(), SpecifiedSubclassConversion),
                "form_1_9_2022" => JsonConvert.DeserializeObject<TableDataMain19>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_1_2022" => JsonConvert.DeserializeObject<TableDataMain21>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_2_2022" => JsonConvert.DeserializeObject<TableDataMain22>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_3_2022" => JsonConvert.DeserializeObject<TableDataMain23>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_4_2022" => JsonConvert.DeserializeObject<TableDataMain24>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_5_2022" => JsonConvert.DeserializeObject<TableDataMain25>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_6_2022" => JsonConvert.DeserializeObject<TableDataMain26>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_7_2022" => JsonConvert.DeserializeObject<TableDataMain27>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_8_2022" => JsonConvert.DeserializeObject<TableDataMain28>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_9_2022" => JsonConvert.DeserializeObject<TableDataMain29>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_10_2022" => JsonConvert.DeserializeObject<TableDataMain210>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_11_2022" => JsonConvert.DeserializeObject<TableDataMain211>(jo.ToString(), SpecifiedSubclassConversion),
                "form_2_12_2022" => JsonConvert.DeserializeObject<TableDataMain212>(jo.ToString(), SpecifiedSubclassConversion),
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
            var reg = new Regex("^form_([1-2])_([0-9](?:(?<=1)[0-2])?)_2022$");
            if (reg.IsMatch(value))
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