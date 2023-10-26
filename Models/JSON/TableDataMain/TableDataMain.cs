using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Serialization;

namespace Models.JSON.TableDataMain;

[JsonConverter(typeof(TableDataMainConverter))]
public abstract class TableDataMain 
{
    #region ContractResolver

    public class TableDataMainSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(JsonForm).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            if (typeof(TableDataMain).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }

    #endregion

    #region TableDataMainConverter

    public class TableDataMainConverter : JsonConverter
    {
        private static JsonSerializerSettings SpecifiedSubclassConversion = new()
        {
            ContractResolver = new TableDataMainSpecifiedConcreteClassConverter()
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