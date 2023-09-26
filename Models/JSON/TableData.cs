using Models.JSON.TableDataMain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.JSON;

public abstract class TableData
{

}

public class TableData11 : TableData
{
    [JsonProperty("main_table")]
    public IList<TableDataMain11> TableData { get; set; }
}

public class TableData12 : TableData
{
    [JsonProperty("main_table")]
    public IList<TableDataMain12> TableData { get; set; }
}

public class TableData13 : TableData
{
    [JsonProperty("main_table")]
    public IList<TableDataMain13> TableData { get; set; }
}

public class TableData14 : TableData
{
    [JsonProperty("main_table")]
    public IList<TableDataMain14> TableData { get; set; }
}

public class TableData15 : TableData
{
    [JsonProperty("main_table")]
    public IList<TableDataMain15> TableData { get; set; }
}

public class TableData16 : TableData
{
    [JsonProperty("main_table")]
    public List<TableDataMain16> TableData { get; set; }
}

public class TableData17 : TableData
{
    [JsonProperty("main_table")]
    public IList<TableDataMain17> TableData { get; set; }
}

public class TableData18 : TableData
{
    [JsonProperty("main_table")]
    public IList<TableDataMain18> TableData { get; set; }
}

public class TableData19 : TableData
{
    [JsonProperty("main_table")]
    public IList<TableDataMain19> TableData { get; set; }
}