using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Models.JSON;

[JsonObject]
public class JsonModel
{
    [JsonProperty("forms")]
    public IList<JsonForm> Forms { get; set; }

    [JsonProperty("orgs")]
    public IList<JsonOrg> Orgs { get; set; }
}

public class JsonOrg
{

}

#region ContractResolver

public class JsonModelSpecifiedConcreteClassConverter : DefaultContractResolver
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

#region JsonModelConverter

public class JsonModelConverter : JsonConverter
{
    private static JsonSerializerSettings SpecifiedSubclassConversion = new()
    {
        ContractResolver = new JsonModelSpecifiedConcreteClassConverter()
    };

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(JsonModel));
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

#endregion