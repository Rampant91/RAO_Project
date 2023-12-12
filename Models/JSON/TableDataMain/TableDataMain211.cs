using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain211 : TableData
{
    //"null-1", "null-2", "Наименование участка", "2"
    [JsonProperty("g2")] public string PlotName { get; set; }

    //"null-1", "null-2", "Кадастровый номер участка", "3"
    [JsonProperty("g3")] public string PlotKadastrNumber { get; set; }

    //"null-1", "null-2", "Код участка", "4"
    [JsonProperty("g4")] public string PlotCode { get; set; }

    //"null-1", "null-2", "Площадь загрязненной территории, кв. м", "5"
    [JsonProperty("square")] public string InfectedArea { get; set; }

    //"null-1", "null-2", "Наименования радионуклидов", "6"
    [JsonProperty("g5")] public string Radionuclids { get; set; }

    //"Удельная активность радионуклида, Бк/г", "null-3", "земельный участок", "7"
    [JsonProperty("g6")] public string SpecificActivityOfPlot { get; set; }

    //"Удельная активность радионуклида, Бк/г", "водный объект", "жидкая фаза", "8"
    [JsonProperty("g7")] public string SpecificActivityOfLiquidPart { get; set; }

    //"Удельная активность радионуклида, Бк/г", "водный объект", "донные отложения", "9"
    [JsonProperty("g8")] public string SpecificActivityOfDensePart { get; set; }
}