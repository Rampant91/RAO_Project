using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.JSON;
public class FormTableData11
{
    [JsonProperty("main_table")]
    public List<FormTableDataMainTable11> MainTable { get; set; }
}