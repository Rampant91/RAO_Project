using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain19 : TableDataMain
{
    //"Сведения об операции", "код", "2"
    [JsonProperty("g2")] public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("g3")] public string OperationDate { get; set; }

    //"Документ","вид", "4"
    [JsonProperty("g4")] public string DocumentVid { get; set; }

    //"Документ", "номер", "5"
    [JsonProperty("g5")] public string DocumentNumber { get; set; }

    //"Документ", "дата", "6"
    [JsonProperty("g6")] public string DocumentDate { get; set; }

    //"null-7", "Код типа объектов учета", "7"
    [JsonProperty("g7")]
    public string CodeTypeAccObject { get; set; }

    //"Сведения о радиоактивных веществах", "радионуклиды", "8"
    [JsonProperty("g8")]
    public string Radionuclids { get; set; }

    //"Сведения о радиоактивных веществах", "активность, Бк", "9"
    [JsonProperty("g9")]
    public string Activity { get; set; }
}