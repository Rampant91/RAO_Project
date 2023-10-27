using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain212 : TableData
{
    //"null-2", "Код операции", "2"
    [JsonProperty("g2")] public string OperationCode { get; set; }

    //"null-3", "Код типа объектов учета", "3"
    [JsonProperty("g3")] public string ObjectTypeCode { get; set; }

    //"Сведения о радионуклидных источниках", "радионуклиды", "4"
    [JsonProperty("g4")] public string Radionuclids { get; set; }

    //"Сведения о радионуклидных источниках", "активность, Бк", "5"
    [JsonProperty("g5")] public string Activity { get; set; }

    //"null-4", "ОКПО поставщика/получателя", "6"
    [JsonProperty("g6")] public string ProviderOrRecieverOKPO { get; set; }
}