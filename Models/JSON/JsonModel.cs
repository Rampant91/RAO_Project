using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.JSON;

[JsonObject]
public class JsonModel
{
    [JsonProperty("forms")]
    public List<JsonForm> Forms { get; set; }
}