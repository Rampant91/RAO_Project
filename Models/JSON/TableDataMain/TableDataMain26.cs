using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain26 : TableData
{
    //"null-1", "Номер наблюдательной скважины", "2"
    [JsonProperty("num_s")] public string ObservedSourceNumber { get; set; }

    //"null-1", "Наименование зоны контроля", "3"
    [JsonProperty("zone")] public string ControlledAreaName { get; set; }

    //"null-1", "Предполагаемый источник поступления радиоактивных веществ", "4"
    [JsonProperty("ist")] public string SupposedWasteSource { get; set; }

    //"null-1", "Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м", "5"
    [JsonProperty("rust")] public string DistanceToWasteSource { get; set; }

    //"null-1", "Глубина отбора проб, м", "6"
    [JsonProperty("gl")] public string TestDepth { get; set; }

    //"null-1", "Наименование радионуклида", "7"
    [JsonProperty("nuk")] public string RadionuclidName { get; set; }

    //"null-1", "Среднегодовое содержание радионуклида, Бк/кг", "8"
    [JsonProperty("radio")] public string AverageYearConcentration { get; set; }
}