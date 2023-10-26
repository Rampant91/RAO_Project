using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain25 : TableDataMain
{
    //"Пункт хранения ОЯТ", "наименование, номер", "2"
    [JsonProperty("g2")] public string StoragePlaceName { get; set; }

    //"Пункт хранения ОЯТ", "код", "3"
    [JsonProperty("g3")] public string StoragePlaceCode { get; set; }

    //"Наличие на конец отчетного года", "код ОЯТ", "4"
    [JsonProperty("g4")] public string CodeOYAT { get; set; }

    //"Наличие на конец отчетного года", "номер мероприятия ФЦП", "5"
    [JsonProperty("g10")] public string FcpNumber { get; set; }

    //"Наличие на конец отчетного года", "топлива (нетто)", "6"
    [JsonProperty("g5")] public string FuelMass { get; set; }

    //"Наличие на конец отчетного года", "ОТВС(ТВЭЛ, выемной части реактора) брутто", "7"
    [JsonProperty("g6")] public string CellMass { get; set; }

    //"Наличие на конец отчетного года", "количество, шт", "8"
    [JsonProperty("g7")] public string Quantity { get; set; }

    //"Наличие на конец отчетного года", "альфа-излучающих нуклидов", "9"
    [JsonProperty("g8")] public string AlphaActivity { get; set; }

    //"Наличие на конец отчетного года", "бета-, гамма-излучающих нуклидов", "10"
    [JsonProperty("g9")] public string BetaGammaActivity { get; set; }
}