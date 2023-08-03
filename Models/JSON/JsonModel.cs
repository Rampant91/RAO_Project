using Models.JSON.TableDataMain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Models.JSON;

[JsonObject]
public class JsonModel
{
    [JsonProperty("forms")] public IList<JsonForm> Forms { get; set; }

    //[JsonProperty("orgs")]
    //public IList<IDictionary<string, JsonOrg>> Orgs { get; set; }
}

public class JsonOrgsModel
{
    [JsonProperty("orgs")] public JsonOrg Orgs { get; set; }
}

[JsonConverter(typeof(JsonOrgConverter))]
public class JsonOrg
{
    public List<JsonReports[]> OrgsList { get; set; }
}

public class JsonReports
{
    public string RegNo { get; set; }
    public string OrganUprav { get; set; }
    public string SubjectRF { get; set; }
    public string JurLico { get; set; }
    public string ShortJurLico { get; set; }
    public string JurLicoAddress { get; set; }
    public string GradeFIO { get; set; }
    public string Telephone { get; set; }
    public string Fax { get; set; }
    public string Email { get; set; }
    public string Okpo { get; set; }
    public string Okved { get; set; }
    public string Okogu { get; set; }
    public string Oktmo { get; set; }
    public string Inn { get; set; }
    public string Kpp { get; set; }
    public string Okopf { get; set; }
    public string Okfs { get; set; }
}

#region ContractResolver

public class JsonOrgSpecifiedConcreteClassConverter : DefaultContractResolver
{
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (typeof(JsonOrg).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
        return base.ResolveContractConverter(objectType);
    }
}

#endregion

#region JsonOrgConverter

public class JsonOrgConverter : JsonConverter
{
    private static JsonSerializerSettings SpecifiedSubclassConversion = new()
    {
        ContractResolver = new JsonOrgSpecifiedConcreteClassConverter()
    };

    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        var deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jo.ToString(), SpecifiedSubclassConversion);

        var orgsList = new List<JsonReports[]>();
        //foreach (var org in deserializeObject)
        //{
        //    var isObosobl = org.Value.Count > 1;
        //    JsonReports[] currentJsonReports;
        //    if (isObosobl)
        //    {
        //        currentJsonReports = new JsonReports[]
        //        {
        //            new()
        //            {
        //                RegNo = org.Value[0]["KOD_ORG"],
        //                OrganUprav = org.Value[0]["KOD_IAE"],
        //                SubjectRF = org.Value[0]["KOD_SRF"],
        //                JurLico = org.Value[0]["NAME_FULL"],
        //                ShortJurLico = org.Value[0]["NAME_SHORT"],
        //                JurLicoAddress = org.Value[0]["ADDRESS_U"],
        //                GradeFIO = org.Value[0]["RUKOVODITEL"],
        //                Telephone = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["TLFON"],
        //                Fax = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["FAX"],
        //                Email = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["EMAIL"],
        //                Okpo = org.Value[0]["OKPO"],
        //                Okved = org.Value[0]["OKVED"],
        //                Okogu = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["OKOGU"],
        //                Oktmo = org.Value[0]["OKTMO"],
        //                Inn = org.Value[0]["INN"],
        //                Kpp = org.Value[0]["KPP"],
        //                Okopf = org.Value[0]["OKOPF"],
        //                Okfs = org.Value[0]["OKFS"]
        //            },
        //            new()
        //            {
        //                RegNo = org.Value[1]["KOD_ORG"],
        //                OrganUprav = org.Value[1]["KOD_IAE"],
        //                SubjectRF = org.Value[1]["KOD_SRF"],
        //                JurLico = org.Value[1]["NAME_FULL"],
        //                ShortJurLico = org.Value[1]["NAME_SHORT"],
        //                JurLicoAddress = org.Value[1]["ADDRESS_P"],
        //                GradeFIO = org.Value[1]["RUKOVODITEL"],
        //                Telephone = org.Value[1]["attrs"].Value is null ? null : org.Value[1]["attrs"]["TLFON"],
        //                Fax = org.Value[1]["attrs"].Value is null ? null : org.Value[1]["attrs"]["FAX"],
        //                Email = org.Value[1]["attrs"].Value is null ? null : org.Value[1]["attrs"]["EMAIL"],
        //                Okpo = org.Value[1]["OKPO"],
        //                Okved = org.Value[1]["OKVED"],
        //                Okogu = org.Value[1]["attrs"].Value is null ? null : org.Value[1]["attrs"]["OKOGU"],
        //                Oktmo = org.Value[1]["OKTMO"],
        //                Inn = org.Value[1]["INN"],
        //                Kpp = org.Value[1]["KPP"],
        //                Okopf = org.Value[1]["OKOPF"],
        //                Okfs = org.Value[1]["OKFS"]
        //            }
        //        };
        //    }
        //    else
        //    {
        //        currentJsonReports = new JsonReports[]
        //        {
        //            new()
        //            {
        //                RegNo = org.Value[0]["KOD_ORG"],
        //                OrganUprav = org.Value[0]["KOD_IAE"],
        //                SubjectRF = org.Value[0]["KOD_SRF"],
        //                JurLico = org.Value[0]["NAME_FULL"],
        //                ShortJurLico = org.Value[0]["NAME_SHORT"],
        //                JurLicoAddress = org.Value[0]["ADDRESS_U"],
        //                GradeFIO = org.Value[0]["RUKOVODITEL"],
        //                Telephone = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["TLFON"],
        //                Fax = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["FAX"],
        //                Email = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["EMAIL"],
        //                Okpo = org.Value[0]["OKPO"],
        //                Okved = org.Value[0]["OKVED"],
        //                Okogu = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["OKOGU"],
        //                Oktmo = org.Value[0]["OKTMO"],
        //                Inn = org.Value[0]["INN"],
        //                Kpp = org.Value[0]["KPP"],
        //                Okopf = org.Value[0]["OKOPF"],
        //                Okfs = org.Value[0]["OKFS"]
        //            }
        //        };
        //    }
        //    orgsList.Add(currentJsonReports);
        //}

        //return orgsList;

        var abc = deserializeObject
            .Select(org => org.Value.Count > 1
                ? new JsonReports[]
                {
                    new()
                    {
                        RegNo = org.Value[0]["KOD_ORG"],
                        OrganUprav = org.Value[0]["KOD_IAE"],
                        SubjectRF = org.Value[0]["KOD_SRF"],
                        JurLico = org.Value[0]["NAME_FULL"],
                        ShortJurLico = org.Value[0]["NAME_SHORT"],
                        JurLicoAddress = org.Value[0]["ADDRESS_U"],
                        GradeFIO = org.Value[0]["RUKOVODITEL"],
                        Telephone = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["TLFON"],
                        Fax = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["FAX"],
                        Email = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["EMAIL"],
                        Okpo = org.Value[0]["OKPO"],
                        Okved = org.Value[0]["OKVED"],
                        Okogu = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["OKOGU"],
                        Oktmo = org.Value[0]["OKTMO"],
                        Inn = org.Value[0]["INN"],
                        Kpp = org.Value[0]["KPP"],
                        Okopf = org.Value[0]["OKOPF"],
                        Okfs = org.Value[0]["OKFS"]
                    },
                    new()
                    {
                        RegNo = org.Value[1]["KOD_ORG"],
                        OrganUprav = org.Value[1]["KOD_IAE"],
                        SubjectRF = org.Value[1]["KOD_SRF"],
                        JurLico = org.Value[1]["NAME_FULL"],
                        ShortJurLico = org.Value[1]["NAME_SHORT"],
                        JurLicoAddress = org.Value[1]["ADDRESS_P"],
                        GradeFIO = org.Value[1]["RUKOVODITEL"],
                        Telephone = org.Value[1]["attrs"].Value is null ? null : org.Value[1]["attrs"]["TLFON"],
                        Fax = org.Value[1]["attrs"].Value is null ? null : org.Value[1]["attrs"]["FAX"],
                        Email = org.Value[1]["attrs"].Value is null ? null : org.Value[1]["attrs"]["EMAIL"],
                        Okpo = org.Value[1]["OKPO"],
                        Okved = org.Value[1]["OKVED"],
                        Okogu = org.Value[1]["attrs"].Value is null ? null : org.Value[1]["attrs"]["OKOGU"],
                        Oktmo = org.Value[1]["OKTMO"],
                        Inn = org.Value[1]["INN"],
                        Kpp = org.Value[1]["KPP"],
                        Okopf = org.Value[1]["OKOPF"],
                        Okfs = org.Value[1]["OKFS"]
                    }
                }
                : new JsonReports[]
                {
                    new()
                    {
                        RegNo = org.Value[0]["KOD_ORG"],
                        OrganUprav = org.Value[0]["KOD_IAE"],
                        SubjectRF = org.Value[0]["KOD_SRF"],
                        JurLico = org.Value[0]["NAME_FULL"],
                        ShortJurLico = org.Value[0]["NAME_SHORT"],
                        JurLicoAddress = org.Value[0]["ADDRESS_U"],
                        GradeFIO = org.Value[0]["RUKOVODITEL"],
                        Telephone = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["TLFON"],
                        Fax = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["FAX"],
                        Email = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["EMAIL"],
                        Okpo = org.Value[0]["OKPO"],
                        Okved = org.Value[0]["OKVED"],
                        Okogu = org.Value[0]["attrs"].Value is null ? null : org.Value[0]["attrs"]["OKOGU"],
                        Oktmo = org.Value[0]["OKTMO"],
                        Inn = org.Value[0]["INN"],
                        Kpp = org.Value[0]["KPP"],
                        Okopf = org.Value[0]["OKOPF"],
                        Okfs = org.Value[0]["OKFS"]
                    }
                })
            .ToList();
        return abc;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException(); // won't be called because CanWrite returns false
    }
}

#endregion