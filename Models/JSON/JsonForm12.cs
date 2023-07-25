using Newtonsoft.Json;

namespace Models.JSON;
public class JsonForm12 : JsonForm1
{
    [JsonProperty("form_table_data")]
    public FormTableData12 FormTableData { get; set; }
}