using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.JSON;

public class FormTable
{
    [JsonProperty("main_table")]
    public IList<TableDataMain.TableData> TableData { get; set; }
}