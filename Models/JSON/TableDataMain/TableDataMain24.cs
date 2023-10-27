using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain24 : TableData
{
    //"null-2", "Код ОЯТ", "2"
    [JsonProperty("g2")] public string CodeOYAT { get; set; }

    //"null-3", "Номер мероприятия ФЦП", "3"
    [JsonProperty("g17")] public string FcpNumber { get; set; }

    //"Поставлено на учет в организации", "масса образованного, т", "4"
    [JsonProperty("g3")] public string MassCreated { get; set; }

    //"Поставлено на учет в организации", "количество образованного, шт", "5"
    [JsonProperty("g4")] public string QuantityCreated { get; set; }

    //"Поставлено на учет в организации", "масса поступивших от сторонних, т", "6"
    [JsonProperty("g5")] public string MassFromAnothers { get; set; }

    //"Поставлено на учет в организации", "количество поступивших от сторонних, шт", "7"
    [JsonProperty("g6")] public string QuantityFromAnothers { get; set; }

    //"Поставлено на учет в организации", "масса импортированных от сторонних, т", "8"
    [JsonProperty("g7")] public string MassFromAnothersImported { get; set; }

    //"Поставлено на учет в организации", "количество импортированных от сторонних, шт", "9"
    [JsonProperty("g8")] public string QuantityFromAnothersImported { get; set; }

    //"Поставлено на учет в организации", "масса учтенных по другим причинам, т", "10"
    [JsonProperty("g9")] public string MassAnotherReasons { get; set; }

    //"Поставлено на учет в организации", "количество учтенных по другим причинам, шт", "11"
    [JsonProperty("g10")] public string QuantityAnotherReasons { get; set; }

    //"Снято с учета в организации", "масса переданных сторонним, т", "12"
    [JsonProperty("g11")] public string MassTransferredToAnother { get; set; }

    //"Снято с учета в организации", "количество переданных сторонним, шт", "13"
    [JsonProperty("g12")] public string QuantityTransferredToAnother { get; set; }

    //"Снято с учета в организации", "масса переработанных, т", "14"
    [JsonProperty("g13")] public string MassRefined { get; set; }

    //"Снято с учета в организации", "количество переработанных, шт", "15"
    [JsonProperty("g14")] public string QuantityRefined { get; set; }

    //"Снято с учета в организации", "масса снятых с учета, т", "16"
    [JsonProperty("g15")] public string MassRemovedFromAccount { get; set; }

    //"Снято с учета в организации", "количество снятых с учета, шт", "17"
    [JsonProperty("g16")] public string QuantityRemovedFromAccount { get; set; }
}