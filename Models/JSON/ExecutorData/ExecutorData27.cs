using Newtonsoft.Json;

namespace Models.JSON.ExecutorData;

public class ExecutorData27 : ExecutorData
{
    //"Разрешение на допустимые выбросы радионуклидов в атмосферу №"
    [JsonProperty("g7")] public string PermissionNumber27 { get; set; }

    //"Разрешение на допустимые выбросы радионуклидов в атмосферу", "от"
    [JsonProperty("g8")] public string PermissionIssueDate27 { get; set; }

    //"Разрешение на допустимые выбросы радионуклидов в атмосферу", "Срок действия с"
    [JsonProperty("per_from")] public string ValidBegin27 { get; set; }

    //"Разрешение на допустимые выбросы радионуклидов в атмосферу", "по"
    [JsonProperty("per_to")] public string ValidThru27 { get; set; }

    //"Разрешение на допустимые выбросы радионуклидов в атмосферу", "Наименование разрешительного документа на допустимые выбросы радионуклидов в атмосферу"
    [JsonProperty("g9")] public string PermissionDocumentName27 { get; set; }
}