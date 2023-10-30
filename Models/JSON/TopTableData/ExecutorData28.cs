using Newtonsoft.Json;

namespace Models.JSON.TopTableData;

public class ExecutorData28 : ExecutorData
{
    #region Permission "Разрешение на сброс радионуклидов в водные объекты"

    //"Разрешение на сброс радионуклидов в водные объекты №"
    [JsonProperty("g11")] public string PermissionNumber_28 { get; set; }

    //"Разрешение на сброс радионуклидов в водные объекты", "от"
    [JsonProperty("g12")] public string PermissionIssueDate_28 { get; set; }

    //"Разрешение на сброс радионуклидов в водные объекты", "Срок действия с"
    [JsonProperty("per1")] public string ValidBegin_28 { get; set; }

    //"Разрешение на сброс радионуклидов в водные объекты", "по"
    [JsonProperty("to1")] public string ValidThru_28 { get; set; }

    //"Разрешение на сброс радионуклидов в водные объекты", "Наименование разрешительного документа на сброс"
    [JsonProperty("g13")] public string PermissionDocumentName_28 { get; set; }

    #endregion

    #region Permission "Разрешение на сброс радионуклидов на рельеф местности"

    //"Разрешение на сброс радионуклидов на рельеф местности №"
    [JsonProperty("g21")] public string PermissionNumber1_28 { get; set; }

    //"Разрешение на сброс радионуклидов на рельеф местности", "от"
    [JsonProperty("g22")] public string PermissionIssueDate1_28 { get; set; }

    //"Разрешение на сброс радионуклидов на рельеф местности", "Срок действия с"
    [JsonProperty("per2")] public string ValidBegin1_28 { get; set; }

    //"Разрешение на сброс радионуклидов на рельеф местности", "по"
    [JsonProperty("to2")] public string ValidThru1_28 { get; set; }

    //"Разрешение на сброс радионуклидов на рельеф местности", "Наименование разрешительного документа на сброс"
    [JsonProperty("g23")] public string PermissionDocumentName1_28 { get; set; }

    #endregion

    #region Permission "Договор на передачу сточных вод в сети канализации"

    //"Договор на передачу сточных вод в сети канализации №"
    [JsonProperty("g31")] public string ContractNumber_28 { get; set; }

    //"Договор на передачу сточных вод в сети канализации", "от"
    [JsonProperty("g32")] public string ContractIssueDate2_28 { get; set; }

    //"Договор на передачу сточных вод в сети канализации", "Срок действия с"
    [JsonProperty("per3")] public string ValidBegin2_28 { get; set; }

    //"Договор на передачу сточных вод в сети канализации", "по"
    [JsonProperty("to3")] public string ValidThru2_28 { get; set; }

    //"Договор на передачу сточных вод в сети канализации", "Организация, осуществляющая прием сточных вод"
    [JsonProperty("g33")] public string OrganisationReciever_28 { get; set; }

    #endregion
}