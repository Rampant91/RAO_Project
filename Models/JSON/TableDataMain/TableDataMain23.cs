using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain23
{
    //"Пункт хранения РАО", "наименование", "2"
    [JsonProperty("g2")] public string StoragePlaceName { get; set; }

    //"Пункт хранения РАО", "код", "3"
    [JsonProperty("g3")] public string StoragePlaceCode { get; set; }

    //"Пункт хранения РАО", "проектный объем, куб. м", "4"
    [JsonProperty("g13")] public string ProjectVolume { get; set; }

    //"Разрешено к размещению", "код РАО", "5"
    [JsonProperty("g4")] public string CodeRAO { get; set; }

    //"Разрешено к размещению", "объем, куб. м", "6"
    [JsonProperty("g5")] public string Volume { get; set; }

    //"Разрешено к размещению", "масса, т", "7"
    [JsonProperty("g6")] public string Mass { get; set; }

    //"Разрешено к размещению", "количество ОЗИИИ, шт", "8"
    [JsonProperty("g7")] public string QuantityOZIII { get; set; }

    //"Разрешено к размещению", "суммарная активность, Бк", "9"
    [JsonProperty("g8")] public string SummaryActivity { get; set; }

    //"Наименование и реквизиты документа на размещение РАО", "номер", "10"
    [JsonProperty("g9")] public string DocumentNumber { get; set; }

    //"Наименование и реквизиты документа на размещение РАО", "дата", "11"
    [JsonProperty("g10")] public string DocumentDate { get; set; }

    //"Наименование и реквизиты документа на размещение РАО", "срок действия", "12"
    [JsonProperty("g11")] public string ExpirationDate { get; set; }

    //"Наименование и реквизиты документа на размещение РАО", "наименование документа", "13"
    [JsonProperty("g12")] public string DocumentName { get; set; }
}