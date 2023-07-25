using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.JSON;
public class FormTableData12
{
    [JsonProperty("main_table")]
    public List<FormTableDataMainTable12> MainTable { get; set; }
}