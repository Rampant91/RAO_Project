using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain29
{
    //"null-2", "Наименование, номер выпуска сточных вод", "2"
    [JsonProperty("g2")] public string WasteSourceName { get; set; }

    //"null-3", "Наименование радионуклида", "3"
    [JsonProperty("g3")] public string RadionuclidName { get; set; }

    //"Активность радионуклида, Бк", "допустимая", "4"
    [JsonProperty("g4")] public string AllowedActivity { get; set; }

    //"Активность радионуклида, Бк", "фактическая", "5"
    [JsonProperty("g5")] public string FactedActivity { get; set; }
}