using Newtonsoft.Json;

namespace Models.JSON;
public abstract class FormTableDataMainTable
{
    //"Сведения об операции", "код", "2"
    [JsonProperty("OpCod")] public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("OpDate")] public string OperationDate { get; set; }

    //"Документ","вид", "16"
    [JsonProperty("DocVid")] public string DocumentVid { get; set; }

    //"Документ", "номер", "17"
    [JsonProperty("DocN")] public string DocumentNumber { get; set; }

    //"Документ", "дата", "18"
    [JsonProperty("DocDate")] public string DocumentDate { get; set; }
}