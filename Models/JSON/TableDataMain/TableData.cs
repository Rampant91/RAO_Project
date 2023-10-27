using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Serialization;

namespace Models.JSON.TableDataMain;

[JsonConverter(typeof(TableDataConverter))]
public abstract class TableData 
{
    #region ContractResolver

    private class TableDataSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(TableData).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }

    #endregion

    #region TableDataConverter

    private class TableDataConverter : JsonConverter
    {
        private static JsonSerializerSettings SpecifiedSubclassConversion = new()
        {
            ContractResolver = new TableDataSpecifiedConcreteClassConverter()
        };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(JsonForm));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            return objectType.Name switch
            {
                "TableDataMain11" => JsonConvert.DeserializeObject<TableDataMain11>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain12" => JsonConvert.DeserializeObject<TableDataMain12>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain13" => JsonConvert.DeserializeObject<TableDataMain13>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain14" => JsonConvert.DeserializeObject<TableDataMain14>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain15" => JsonConvert.DeserializeObject<TableDataMain15>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain16" => JsonConvert.DeserializeObject<TableDataMain16>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain17" => JsonConvert.DeserializeObject<TableDataMain17>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain18" => JsonConvert.DeserializeObject<TableDataMain18>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain19" => JsonConvert.DeserializeObject<TableDataMain19>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain21" => JsonConvert.DeserializeObject<TableDataMain21>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain22" => JsonConvert.DeserializeObject<TableDataMain22>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain23" => JsonConvert.DeserializeObject<TableDataMain23>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain24" => JsonConvert.DeserializeObject<TableDataMain24>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain25" => JsonConvert.DeserializeObject<TableDataMain25>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain26" => JsonConvert.DeserializeObject<TableDataMain26>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain27" => JsonConvert.DeserializeObject<TableDataMain27>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain28" => JsonConvert.DeserializeObject<TableDataMain28>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain29" => JsonConvert.DeserializeObject<TableDataMain29>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain210" => JsonConvert.DeserializeObject<TableDataMain210>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain211" => JsonConvert.DeserializeObject<TableDataMain211>(jo.ToString(), SpecifiedSubclassConversion),
                "TableDataMain212" => JsonConvert.DeserializeObject<TableDataMain212>(jo.ToString(), SpecifiedSubclassConversion),
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
}