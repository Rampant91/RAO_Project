using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain28 : TableDataMain
{
    //"null-2", "Наименование, номер выпуска сточных вод", "2"
    [JsonProperty("g2")] public string WasteSourceName { get; set; }

    //"Приемник отведенных вод", "наименование", "3"
    [JsonProperty("g3")] public string WasteRecieverName { get; set; }

    //"Приемник отведенных вод", "код типа приемника", "4"
    [JsonProperty("g4")] public string RecieverTypeCode { get; set; }

    //"Приемник отведенных вод", "наименование бассейнового округа", "5"
    [JsonProperty("g5")] public string PoolDistrictName { get; set; }

    //"null-3", "Допустимый объем водоотведения за год, тыс. куб. м", "6"
    [JsonProperty("g6")] public string AllowedWasteRemovalVolume { get; set; }

    //"null-4", "Отведено за отчетный период, тыс. куб. м", "7"
    [JsonProperty("g7")] public string RemovedWasteVolume { get; set; }
}