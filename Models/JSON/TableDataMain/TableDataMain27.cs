using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain27 : TableData
{
    //"null-2", "Наименование, номер источника выбросов", "2"
    [JsonProperty("g2")] public string ObservedSourceNumber { get; set; }

    //"null-3", "Наименование радионуклида", "3"
    [JsonProperty("g3")] public string RadionuclidName { get; set; }

    //"Выброс радионуклида в атмосферу за отчетный год, Бк", "разрешенный", "4"
    [JsonProperty("g4")] public string AllowedWasteValue { get; set; }

    //"Выброс радионуклида в атмосферу за отчетный год, Бк", "фактический", "5"
    [JsonProperty("g5")] public string FactedWasteValue { get; set; }

    //"Выброс радионуклида в атмосферу за предыдущий год, Бк", "фактический", "6"
    [JsonProperty("g6")] public string WasteOutbreakPreviousYear { get; set; }

}