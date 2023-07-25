using Newtonsoft.Json;

namespace Models.JSON;

public class JsonForm11 : JsonForm1
{
    [JsonProperty("form_table_data")]
    public FormTableData11 FormTableData { get; set; }
}