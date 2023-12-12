using Newtonsoft.Json;

namespace Models.JSON.ExecutorData;

public class ExecutorData26 : ExecutorData
{
    //"Количество наблюдательных скважин, принадлежащих организации"
    [JsonProperty("quantity")] public string SourcesQuantity26 { get; set; }
}