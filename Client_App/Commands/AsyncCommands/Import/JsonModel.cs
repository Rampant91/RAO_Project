//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;

//namespace Client_App.Commands.AsyncCommands.Import;

//public partial class JsonModel
//{
//    [JsonProperty("vocs", NullValueHandling = NullValueHandling.Ignore)]
//    public List<Voc> Vocs { get; set; }

//    [JsonProperty("forms", NullValueHandling = NullValueHandling.Ignore)]
//    public List<Form> Forms { get; set; }

//    [JsonProperty("orgs", NullValueHandling = NullValueHandling.Ignore)]
//    public Dictionary<string, List<Org>> Orgs { get; set; }
//}

//#region Form
//public partial class Form
//{
//    [JsonProperty("report_form_id", NullValueHandling = NullValueHandling.Ignore)]
//    public long? ReportFormId { get; set; }

//    [JsonProperty("form_state", NullValueHandling = NullValueHandling.Ignore)]
//    public FormState? FormState { get; set; }

//    [JsonProperty("form_no", NullValueHandling = NullValueHandling.Ignore)]
//    public string FormNo { get; set; }

//    [JsonProperty("form_part")]
//    public dynamic FormPart { get; set; }

//    [JsonProperty("form_version")]
//    public dynamic FormVersion { get; set; }

//    [JsonProperty("form_name", NullValueHandling = NullValueHandling.Ignore)]
//    public string FormName { get; set; }

//    [JsonProperty("form_main_data", NullValueHandling = NullValueHandling.Ignore)]
//    public FormMainData FormMainData { get; set; }

//    [JsonProperty("form_table_data", NullValueHandling = NullValueHandling.Ignore)]
//    public FormTableData FormTableData { get; set; }

//    [JsonProperty("form_remarks", NullValueHandling = NullValueHandling.Ignore)]
//    public FormCommentsClass FormRemarks { get; set; }

//    [JsonProperty("form_comments", NullValueHandling = NullValueHandling.Ignore)]
//    public FormCommentsClass FormComments { get; set; }

//    [JsonProperty("form_warnings", NullValueHandling = NullValueHandling.Ignore)]
//    public FormCommentsClass FormWarnings { get; set; }

//    [JsonProperty("form_notifications")]
//    public FormNotifications FormNotifications { get; set; }

//    [JsonProperty("sign_info")]
//    public dynamic SignInfo { get; set; }

//    [JsonProperty("fill_info")]
//    public dynamic FillInfo { get; set; }

//    [JsonProperty("form_period_start", NullValueHandling = NullValueHandling.Ignore)]
//    public DateTimeOffset? FormPeriodStart { get; set; }

//    [JsonProperty("form_period_end")]
//    public DateTimeOffset? FormPeriodEnd { get; set; }

//    [JsonProperty("adjustment_no", NullValueHandling = NullValueHandling.Ignore)]
//    public long? AdjustmentNo { get; set; }

//    [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
//    public long? UserId { get; set; }

//    [JsonProperty("kod_org", NullValueHandling = NullValueHandling.Ignore)]
//    public string KodOrg { get; set; }

//    [JsonProperty("author_id", NullValueHandling = NullValueHandling.Ignore)]
//    public long? AuthorId { get; set; }

//    [JsonProperty("form_export_ts")]
//    public dynamic FormExportTs { get; set; }

//    [JsonProperty("form_import_ts")]
//    public DateTimeOffset? FormImportTs { get; set; }

//    [JsonProperty("reporting_year")]
//    public dynamic ReportingYear { get; set; }

//    [JsonProperty("notification_date")]
//    public DateTimeOffset? NotificationDate { get; set; }

//    [JsonProperty("form_comment")]
//    public dynamic FormComment { get; set; }

//    [JsonProperty("form_errors")]
//    public FormErrors FormErrors { get; set; }

//    [JsonProperty("uniq_numb")]
//    public string UniqNumb { get; set; }

//    [JsonProperty("import_author")]
//    public string ImportAuthor { get; set; }

//    [JsonProperty("import_src_id")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? ImportSrcId { get; set; }

//    [JsonProperty("import_hash")]
//    public string ImportHash { get; set; }

//    [JsonProperty("log_id")]
//    public long? LogId { get; set; }
//}

//public partial class FormCommentsClass
//{
//    [JsonProperty("main_table", NullValueHandling = NullValueHandling.Ignore)]
//    public List<FormCommentsMainTable> MainTable { get; set; }
//}

//public partial class FormCommentsMainTable
//{
//    [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
//    public PurpleValue Value { get; set; }

//    [JsonProperty("pointer", NullValueHandling = NullValueHandling.Ignore)]
//    public PurplePointer Pointer { get; set; }
//}

//public partial class PurplePointer
//{
//    [JsonProperty("row", NullValueHandling = NullValueHandling.Ignore)]
//    public long? Row { get; set; }

//    [JsonProperty("col_name", NullValueHandling = NullValueHandling.Ignore)]
//    public ColName? ColName { get; set; }
//}

//public partial class PurpleValue
//{
//    [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
//    public DateTimeOffset? Date { get; set; }

//    [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
//    public string Text { get; set; }

//    [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
//    public User? User { get; set; }
//}

//public partial class FormErrors
//{
//    [JsonProperty("main_table", NullValueHandling = NullValueHandling.Ignore)]
//    public List<FormErrorsMainTable> MainTable { get; set; }
//}

//public partial class FormErrorsMainTable
//{
//    [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
//    public FluffyValue Value { get; set; }

//    [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
//    public Source? Source { get; set; }

//    [JsonProperty("pointer", NullValueHandling = NullValueHandling.Ignore)]
//    public FluffyPointer Pointer { get; set; }
//}

//public partial class FluffyPointer
//{
//    [JsonProperty("row", NullValueHandling = NullValueHandling.Ignore)]
//    public UdATro? Row { get; set; }

//    [JsonProperty("col_name", NullValueHandling = NullValueHandling.Ignore)]
//    public string ColName { get; set; }
//}

//public partial class FluffyValue
//{
//    [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
//    public string Text { get; set; }
//}

//public partial class FormMainData
//{
//    [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
//    public string Email { get; set; }

//    [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
//    public string Phone { get; set; }

//    [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
//    public string Position { get; set; }

//    [JsonProperty("full_name", NullValueHandling = NullValueHandling.Ignore)]
//    public string FullName { get; set; }

//    [JsonProperty("q2", NullValueHandling = NullValueHandling.Ignore)]
//    public DateTimeOffset? Q2 { get; set; }

//    [JsonProperty("q3", NullValueHandling = NullValueHandling.Ignore)]
//    public string Q3 { get; set; }

//    [JsonProperty("q4", NullValueHandling = NullValueHandling.Ignore)]
//    public string Q4 { get; set; }

//    [JsonProperty("q5", NullValueHandling = NullValueHandling.Ignore)]
//    public Q5? Q5 { get; set; }

//    [JsonProperty("q6", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? Q6 { get; set; }

//    [JsonProperty("q7", NullValueHandling = NullValueHandling.Ignore)]
//    public Q5? Q7 { get; set; }

//    [JsonProperty("q8", NullValueHandling = NullValueHandling.Ignore)]
//    public DateTimeOffset? Q8 { get; set; }

//    [JsonProperty("q9", NullValueHandling = NullValueHandling.Ignore)]
//    public Q5? Q9 { get; set; }

//    [JsonProperty("q10", NullValueHandling = NullValueHandling.Ignore)]
//    public string Q10 { get; set; }

//    [JsonProperty("q11", NullValueHandling = NullValueHandling.Ignore)]
//    public string Q11 { get; set; }

//    [JsonProperty("q12", NullValueHandling = NullValueHandling.Ignore)]
//    public string Q12 { get; set; }

//    [JsonProperty("q13", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? Q13 { get; set; }
//}

//public partial class FormNotifications
//{
//    [JsonProperty("main_table", NullValueHandling = NullValueHandling.Ignore)]
//    public List<FormNotificationsMainTable> MainTable { get; set; }
//}

//public partial class FormNotificationsMainTable
//{
//    [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
//    public FluffyValue Value { get; set; }

//    [JsonProperty("pointer", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? Pointer { get; set; }
//}

//public partial class FormTableData
//{
//    [JsonProperty("main_table", NullValueHandling = NullValueHandling.Ignore)]
//    public List<FormTableDataMainTable> MainTable { get; set; }
//}

//public partial class FormTableDataMainTable
//{
//    [JsonProperty("Kg", NullValueHandling = NullValueHandling.Ignore)]
//    public DVal? Kg { get; set; }

//    [JsonProperty("Nss", NullValueHandling = NullValueHandling.Ignore)]
//    public DocVid? Nss { get; set; }

//    [JsonProperty("Typ", NullValueHandling = NullValueHandling.Ignore)]
//    public string Typ { get; set; }

//    [JsonProperty("DocN", NullValueHandling = NullValueHandling.Ignore)]
//    public string DocN { get; set; }

//    [JsonProperty("Numb", NullValueHandling = NullValueHandling.Ignore)]
//    public string Numb { get; set; }

//    [JsonProperty("OpCod", NullValueHandling = NullValueHandling.Ignore)]
//    public DocVid? OpCod { get; set; }

//    [JsonProperty("PaspN", NullValueHandling = NullValueHandling.Ignore)]
//    public string PaspN { get; set; }

//    [JsonProperty("DocVid", NullValueHandling = NullValueHandling.Ignore)]
//    public DocVid? DocVid { get; set; }

//    [JsonProperty("OpDate", NullValueHandling = NullValueHandling.Ignore)]
//    public Date? OpDate { get; set; }

//    [JsonProperty("PrName", NullValueHandling = NullValueHandling.Ignore)]
//    public string PrName { get; set; }

//    [JsonProperty("UktPrN")]
//    public string UktPrN { get; set; }

//    [JsonProperty("DocDate", NullValueHandling = NullValueHandling.Ignore)]
//    public Date? DocDate { get; set; }

//    [JsonProperty("OkpoPIP", NullValueHandling = NullValueHandling.Ignore)]
//    public string OkpoPip { get; set; }

//    [JsonProperty("OkpoPrv", NullValueHandling = NullValueHandling.Ignore)]
//    public DocVid? OkpoPrv { get; set; }

//    [JsonProperty("Pravoobl", NullValueHandling = NullValueHandling.Ignore)]
//    public string Pravoobl { get; set; }

//    [JsonProperty("UktPrTyp", NullValueHandling = NullValueHandling.Ignore)]
//    public string UktPrTyp { get; set; }

//    [JsonProperty("FormSobst", NullValueHandling = NullValueHandling.Ignore)]
//    public DocVid? FormSobst { get; set; }

//    [JsonProperty("IzgotDate", NullValueHandling = NullValueHandling.Ignore)]
//    public string IzgotDate { get; set; }

//    [JsonProperty("IzgotOKPO", NullValueHandling = NullValueHandling.Ignore)]
//    public string IzgotOkpo { get; set; }

//    [JsonProperty("Sht", NullValueHandling = NullValueHandling.Ignore)]
//    public DocVid? Sht { get; set; }

//    [JsonProperty("Kateg", NullValueHandling = NullValueHandling.Ignore)]
//    public string Kateg { get; set; }

//    [JsonProperty("Activn", NullValueHandling = NullValueHandling.Ignore)]
//    public UdATro? Activn { get; set; }

//    [JsonProperty("Nuclid", NullValueHandling = NullValueHandling.Ignore)]
//    public string Nuclid { get; set; }

//    [JsonProperty("g2", NullValueHandling = NullValueHandling.Ignore)]
//    public string G2 { get; set; }

//    [JsonProperty("g3", NullValueHandling = NullValueHandling.Ignore)]
//    public string G3 { get; set; }

//    [JsonProperty("g4", NullValueHandling = NullValueHandling.Ignore)]
//    public G4Union? G4 { get; set; }

//    [JsonProperty("g5", NullValueHandling = NullValueHandling.Ignore)]
//    public G5Union? G5 { get; set; }

//    [JsonProperty("g6", NullValueHandling = NullValueHandling.Ignore)]
//    public string G6 { get; set; }

//    [JsonProperty("g7", NullValueHandling = NullValueHandling.Ignore)]
//    public string G7 { get; set; }

//    [JsonProperty("g8", NullValueHandling = NullValueHandling.Ignore)]
//    public G8Union? G8 { get; set; }

//    [JsonProperty("g9", NullValueHandling = NullValueHandling.Ignore)]
//    public string G9 { get; set; }

//    [JsonProperty("Agr", NullValueHandling = NullValueHandling.Ignore)]
//    public Agr? Agr { get; set; }

//    [JsonProperty("Arg", NullValueHandling = NullValueHandling.Ignore)]
//    public Agr? Arg { get; set; }

//    [JsonProperty("Kbm", NullValueHandling = NullValueHandling.Ignore)]
//    public UdATro? Kbm { get; set; }

//    [JsonProperty("ActDate", NullValueHandling = NullValueHandling.Ignore)]
//    public ActDateUnion? ActDate { get; set; }

//    [JsonProperty("IstName", NullValueHandling = NullValueHandling.Ignore)]
//    public string IstName { get; set; }

//    [JsonProperty("g10", NullValueHandling = NullValueHandling.Ignore)]
//    public string G10 { get; set; }

//    [JsonProperty("g11", NullValueHandling = NullValueHandling.Ignore)]
//    public G11Union? G11 { get; set; }

//    [JsonProperty("g12", NullValueHandling = NullValueHandling.Ignore)]
//    public string G12 { get; set; }

//    [JsonProperty("g13", NullValueHandling = NullValueHandling.Ignore)]
//    public string G13 { get; set; }

//    [JsonProperty("g14", NullValueHandling = NullValueHandling.Ignore)]
//    public string G14 { get; set; }

//    [JsonProperty("g15", NullValueHandling = NullValueHandling.Ignore)]
//    public string G15 { get; set; }

//    [JsonProperty("g16", NullValueHandling = NullValueHandling.Ignore)]
//    public string G16 { get; set; }

//    [JsonProperty("g17", NullValueHandling = NullValueHandling.Ignore)]
//    public string G17 { get; set; }

//    [JsonProperty("g18", NullValueHandling = NullValueHandling.Ignore)]
//    public G18Union? G18 { get; set; }

//    [JsonProperty("g19", NullValueHandling = NullValueHandling.Ignore)]
//    public G19Union? G19 { get; set; }

//    [JsonProperty("g20", NullValueHandling = NullValueHandling.Ignore)]
//    public G20? G20 { get; set; }

//    [JsonProperty("g21", NullValueHandling = NullValueHandling.Ignore)]
//    public G21? G21 { get; set; }

//    [JsonProperty("g22", NullValueHandling = NullValueHandling.Ignore)]
//    public string G22 { get; set; }

//    [JsonProperty("g23", NullValueHandling = NullValueHandling.Ignore)]
//    public G23? G23 { get; set; }

//    [JsonProperty("g24", NullValueHandling = NullValueHandling.Ignore)]
//    public G24? G24 { get; set; }

//    [JsonProperty("g25", NullValueHandling = NullValueHandling.Ignore)]
//    public Q5? G25 { get; set; }

//    [JsonProperty("g26", NullValueHandling = NullValueHandling.Ignore)]
//    public Q5? G26 { get; set; }

//    [JsonProperty("g27", NullValueHandling = NullValueHandling.Ignore)]
//    public G27? G27 { get; set; }

//    [JsonProperty("g28", NullValueHandling = NullValueHandling.Ignore)]
//    public Q5? G28 { get; set; }

//    [JsonProperty("factory", NullValueHandling = NullValueHandling.Ignore)]
//    public The1? Factory { get; set; }

//    [JsonProperty("FCP", NullValueHandling = NullValueHandling.Ignore)]
//    public DocVid? Fcp { get; set; }

//    [JsonProperty("ActA", NullValueHandling = NullValueHandling.Ignore)]
//    public string ActA { get; set; }

//    [JsonProperty("BCod", NullValueHandling = NullValueHandling.Ignore)]
//    public string BCod { get; set; }

//    [JsonProperty("ActBG", NullValueHandling = NullValueHandling.Ignore)]
//    public string ActBg { get; set; }

//    [JsonProperty("ActTR", NullValueHandling = NullValueHandling.Ignore)]
//    public Q5? ActTr { get; set; }

//    [JsonProperty("ActUR", NullValueHandling = NullValueHandling.Ignore)]
//    public string ActUr { get; set; }

//    [JsonProperty("Tonne", NullValueHandling = NullValueHandling.Ignore)]
//    public string Tonne { get; set; }

//    [JsonProperty("UpCod", NullValueHandling = NullValueHandling.Ignore)]
//    public DocVid? UpCod { get; set; }

//    [JsonProperty("PH_Cod", NullValueHandling = NullValueHandling.Ignore)]
//    public DocVid? PhCod { get; set; }

//    [JsonProperty("RAOCod", NullValueHandling = NullValueHandling.Ignore)]
//    public string RaoCod { get; set; }

//    [JsonProperty("Subsid", NullValueHandling = NullValueHandling.Ignore)]
//    public DocVid? Subsid { get; set; }

//    [JsonProperty("PH_Name", NullValueHandling = NullValueHandling.Ignore)]
//    public string PhName { get; set; }

//    [JsonProperty("RAOCodMax", NullValueHandling = NullValueHandling.Ignore)]
//    public string RaoCodMax { get; set; }

//    [JsonProperty("g29", NullValueHandling = NullValueHandling.Ignore)]
//    public Q5? G29 { get; set; }

//    [JsonProperty("g30", NullValueHandling = NullValueHandling.Ignore)]
//    public DocVid? G30 { get; set; }

//    [JsonProperty("g31", NullValueHandling = NullValueHandling.Ignore)]
//    public Q5? G31 { get; set; }

//    [JsonProperty("g32", NullValueHandling = NullValueHandling.Ignore)]
//    public string G32 { get; set; }

//    [JsonProperty("CodRAO", NullValueHandling = NullValueHandling.Ignore)]
//    public Q5? CodRao { get; set; }
//} 
//#endregion

//#region Org
//public partial class Org
//{
//    [JsonProperty("record_id", NullValueHandling = NullValueHandling.Ignore)]
//    public long? RecordId { get; set; }

//    [JsonProperty("voc_type", NullValueHandling = NullValueHandling.Ignore)]
//    public OrgVocType? VocType { get; set; }

//    [JsonProperty("value_id", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? ValueId { get; set; }

//    [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
//    public string Label { get; set; }

//    [JsonProperty("attrs", NullValueHandling = NullValueHandling.Ignore)]
//    public OrgAttrs Attrs { get; set; }

//    [JsonProperty("valid_from", NullValueHandling = NullValueHandling.Ignore)]
//    public DateTimeOffset? ValidFrom { get; set; }

//    [JsonProperty("valid_to")]
//    public dynamic ValidTo { get; set; }

//    [JsonProperty("value_status", NullValueHandling = NullValueHandling.Ignore)]
//    public long? ValueStatus { get; set; }

//    [JsonProperty("creator_id")]
//    public string CreatorId { get; set; }

//    [JsonProperty("import_author")]
//    public string ImportAuthor { get; set; }

//    [JsonProperty("import_src_id")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? ImportSrcId { get; set; }

//    [JsonProperty("import_hash")]
//    public string ImportHash { get; set; }

//    [JsonProperty("log_id")]
//    public long? LogId { get; set; }

//    [JsonProperty("KOD_ORG")]
//    public string KodOrg { get; set; }

//    [JsonProperty("ORG_TYPE")]
//    public dynamic OrgType { get; set; }

//    [JsonProperty("KOD_IAE")]
//    public string KodIae { get; set; }

//    [JsonProperty("KOD_SRF", NullValueHandling = NullValueHandling.Ignore)]
//    public string KodSrf { get; set; }

//    [JsonProperty("NAME_FULL", NullValueHandling = NullValueHandling.Ignore)]
//    public string NameFull { get; set; }

//    [JsonProperty("NAME_SHORT", NullValueHandling = NullValueHandling.Ignore)]
//    public string NameShort { get; set; }

//    [JsonProperty("ADDRESS_U", NullValueHandling = NullValueHandling.Ignore)]
//    public string AddressU { get; set; }

//    [JsonProperty("ADDRESS_P", NullValueHandling = NullValueHandling.Ignore)]
//    public string AddressP { get; set; }

//    [JsonProperty("RUKOVODITEL", NullValueHandling = NullValueHandling.Ignore)]
//    public string Rukovoditel { get; set; }

//    [JsonProperty("OKPO", NullValueHandling = NullValueHandling.Ignore)]
//    public string Okpo { get; set; }

//    [JsonProperty("OKVED", NullValueHandling = NullValueHandling.Ignore)]
//    public string Okved { get; set; }

//    [JsonProperty("OKATO")]
//    public dynamic Okato { get; set; }

//    [JsonProperty("OKTMO", NullValueHandling = NullValueHandling.Ignore)]
//    public string Oktmo { get; set; }

//    [JsonProperty("INN", NullValueHandling = NullValueHandling.Ignore)]
//    public string Inn { get; set; }

//    [JsonProperty("KPP", NullValueHandling = NullValueHandling.Ignore)]
//    public string Kpp { get; set; }

//    [JsonProperty("OKOPF", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? Okopf { get; set; }

//    [JsonProperty("OKFS", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? Okfs { get; set; }

//    [JsonProperty("PRIM")]
//    public dynamic Prim { get; set; }

//    [JsonProperty("PRZN_ORG", NullValueHandling = NullValueHandling.Ignore)]
//    public long? PrznOrg { get; set; }

//    [JsonProperty("PRZN_OP", NullValueHandling = NullValueHandling.Ignore)]
//    public long? PrznOp { get; set; }

//    [JsonProperty("PRZN_ACTIV", NullValueHandling = NullValueHandling.Ignore)]
//    public long? PrznActiv { get; set; }

//    [JsonProperty("LVL_ORG")]
//    public dynamic LvlOrg { get; set; }

//    [JsonProperty("OKPO_PRP")]
//    public dynamic OkpoPrp { get; set; }

//    [JsonProperty("RIAC")]
//    public dynamic Riac { get; set; }

//    [JsonProperty("VIAC")]
//    public dynamic Viac { get; set; }

//    [JsonProperty("OUIAE")]
//    public dynamic Ouiae { get; set; }

//    [JsonProperty("DT_KORR")]
//    public dynamic DtKorr { get; set; }

//    [JsonProperty("USER_KORR")]
//    public dynamic UserKorr { get; set; }

//    [JsonProperty("DT_REG")]
//    public DateTimeOffset? DtReg { get; set; }

//    [JsonProperty("DT_SU")]
//    public dynamic DtSu { get; set; }

//    [JsonProperty("category1")]
//    public dynamic Category1 { get; set; }

//    [JsonProperty("category2")]
//    public dynamic Category2 { get; set; }

//    [JsonProperty("category3")]
//    public dynamic Category3 { get; set; }

//    [JsonProperty("category4")]
//    public dynamic Category4 { get; set; }

//    [JsonProperty("category5")]
//    public dynamic Category5 { get; set; }

//    [JsonProperty("YROO")]
//    public bool? Yroo { get; set; }

//    [JsonProperty("KOD_GOL_ORG")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? KodGolOrg { get; set; }
//}

//public partial class OrgAttrs
//{
//    [JsonProperty("FAX", NullValueHandling = NullValueHandling.Ignore)]
//    public string Fax { get; set; }

//    [JsonProperty("EMAIL", NullValueHandling = NullValueHandling.Ignore)]
//    public string Email { get; set; }

//    [JsonProperty("OKOGU", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? Okogu { get; set; }

//    [JsonProperty("TLFON", NullValueHandling = NullValueHandling.Ignore)]
//    public string Tlfon { get; set; }
//} 
//#endregion

//#region Voc
//public partial class Voc
//{
//    [JsonProperty("record_id", NullValueHandling = NullValueHandling.Ignore)]
//    public long? RecordId { get; set; }

//    [JsonProperty("voc_type", NullValueHandling = NullValueHandling.Ignore)]
//    public VocVocType? VocType { get; set; }

//    [JsonProperty("value_id", NullValueHandling = NullValueHandling.Ignore)]
//    public string ValueId { get; set; }

//    [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
//    public string Label { get; set; }

//    [JsonProperty("attrs", NullValueHandling = NullValueHandling.Ignore)]
//    public VocAttrs Attrs { get; set; }

//    [JsonProperty("valid_from")]
//    public dynamic ValidFrom { get; set; }

//    [JsonProperty("valid_to")]
//    public dynamic ValidTo { get; set; }

//    [JsonProperty("value_status", NullValueHandling = NullValueHandling.Ignore)]
//    public long? ValueStatus { get; set; }

//    [JsonProperty("creator_id", NullValueHandling = NullValueHandling.Ignore)]
//    public string CreatorId { get; set; }

//    [JsonProperty("import_author")]
//    public string ImportAuthor { get; set; }

//    [JsonProperty("import_src_id")]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? ImportSrcId { get; set; }

//    [JsonProperty("import_hash")]
//    public string ImportHash { get; set; }

//    [JsonProperty("log_id")]
//    public long? LogId { get; set; }
//}

//public partial class VocAttrs
//{
//    [JsonProperty("2", NullValueHandling = NullValueHandling.Ignore)]
//    public string The2 { get; set; }

//    [JsonProperty("3", NullValueHandling = NullValueHandling.Ignore)]
//    public string The3 { get; set; }

//    [JsonProperty("4", NullValueHandling = NullValueHandling.Ignore)]
//    public string The4 { get; set; }

//    [JsonProperty("5", NullValueHandling = NullValueHandling.Ignore)]
//    public The5_Union? The5 { get; set; }

//    [JsonProperty("6", NullValueHandling = NullValueHandling.Ignore)]
//    public string The6 { get; set; }

//    [JsonProperty("7", NullValueHandling = NullValueHandling.Ignore)]
//    public The7_Union? The7 { get; set; }

//    [JsonProperty("8", NullValueHandling = NullValueHandling.Ignore)]
//    public string The8 { get; set; }

//    [JsonProperty("9", NullValueHandling = NullValueHandling.Ignore)]
//    public string The9 { get; set; }

//    [JsonProperty("10", NullValueHandling = NullValueHandling.Ignore)]
//    public The10_Union? The10 { get; set; }

//    [JsonProperty("x", NullValueHandling = NullValueHandling.Ignore)]
//    public string X { get; set; }

//    [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
//    public string Status { get; set; }

//    [JsonProperty("adding_date", NullValueHandling = NullValueHandling.Ignore)]
//    public string AddingDate { get; set; }

//    [JsonProperty("code_OKPO_org", NullValueHandling = NullValueHandling.Ignore)]
//    public string CodeOkpoOrg { get; set; }

//    [JsonProperty("prim", NullValueHandling = NullValueHandling.Ignore)]
//    public string Prim { get; set; }

//    [JsonProperty("Op_Cod", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? OpCod { get; set; }

//    [JsonProperty("Op_Name", NullValueHandling = NullValueHandling.Ignore)]
//    public string OpName { get; set; }

//    [JsonProperty("deadline", NullValueHandling = NullValueHandling.Ignore)]
//    public string Deadline { get; set; }

//    [JsonProperty("DocCod", NullValueHandling = NullValueHandling.Ignore)]
//    public long? DocCod { get; set; }

//    [JsonProperty("DocName", NullValueHandling = NullValueHandling.Ignore)]
//    public string DocName { get; set; }

//    [JsonProperty("FSCod", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? FsCod { get; set; }

//    [JsonProperty("FSName", NullValueHandling = NullValueHandling.Ignore)]
//    public string FsName { get; set; }

//    [JsonProperty("attrs1")]
//    public dynamic Attrs1 { get; set; }

//    [JsonProperty("11", NullValueHandling = NullValueHandling.Ignore)]
//    public string The11 { get; set; }

//    [JsonProperty("12", NullValueHandling = NullValueHandling.Ignore)]
//    public The12? The12 { get; set; }

//    [JsonProperty("AM", NullValueHandling = NullValueHandling.Ignore)]
//    public long? Am { get; set; }

//    [JsonProperty("MOI", NullValueHandling = NullValueHandling.Ignore)]
//    public DVal? Moi { get; set; }

//    [JsonProperty("MZA", NullValueHandling = NullValueHandling.Ignore)]
//    public long? Mza { get; set; }

//    [JsonProperty("MZUA", NullValueHandling = NullValueHandling.Ignore)]
//    public long? Mzua { get; set; }

//    [JsonProperty("D_val", NullValueHandling = NullValueHandling.Ignore)]
//    public DVal? DVal { get; set; }

//    [JsonProperty("Num_TM", NullValueHandling = NullValueHandling.Ignore)]
//    public long? NumTm { get; set; }

//    [JsonProperty("Name_RN", NullValueHandling = NullValueHandling.Ignore)]
//    public string NameRn { get; set; }

//    [JsonProperty("UdA_GRO", NullValueHandling = NullValueHandling.Ignore)]
//    public DVal? UdAGro { get; set; }

//    [JsonProperty("UdA_TRO", NullValueHandling = NullValueHandling.Ignore)]
//    public UdATro? UdATro { get; set; }

//    [JsonProperty("ObA_GaRO", NullValueHandling = NullValueHandling.Ignore)]
//    public DVal? ObAGaRo { get; set; }

//    [JsonProperty("Kod_gruppy", NullValueHandling = NullValueHandling.Ignore)]
//    public KodGruppy? KodGruppy { get; set; }

//    [JsonProperty("Period_p_r", NullValueHandling = NullValueHandling.Ignore)]
//    public double? PeriodPR { get; set; }

//    [JsonProperty("Vid_izluch", NullValueHandling = NullValueHandling.Ignore)]
//    public VidIzluch? VidIzluch { get; set; }

//    [JsonProperty("Name_RN_Lat", NullValueHandling = NullValueHandling.Ignore)]
//    public string NameRnLat { get; set; }

//    [JsonProperty("Edinica_izmer_p_r", NullValueHandling = NullValueHandling.Ignore)]
//    public EdinicaIzmerPR? EdinicaIzmerPR { get; set; }

//    [JsonProperty("Osn_vid_raspad_text", NullValueHandling = NullValueHandling.Ignore)]
//    public OsnVidRaspadText? OsnVidRaspadText { get; set; }

//    [JsonProperty("import_hash")]
//    public dynamic ImportHash { get; set; }

//    [JsonProperty("import_author")]
//    public dynamic ImportAuthor { get; set; }

//    [JsonProperty("import_src_id")]
//    public dynamic ImportSrcId { get; set; }

//    [JsonProperty("g1_cod", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? G1Cod { get; set; }

//    [JsonProperty("g1_types", NullValueHandling = NullValueHandling.Ignore)]
//    public string G1Types { get; set; }

//    [JsonProperty("cifra", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? Cifra { get; set; }

//    [JsonProperty("Agr_name", NullValueHandling = NullValueHandling.Ignore)]
//    public string AgrName { get; set; }

//    [JsonProperty("ORICod", NullValueHandling = NullValueHandling.Ignore)]
//    public long? OriCod { get; set; }

//    [JsonProperty("ORI_ty", NullValueHandling = NullValueHandling.Ignore)]
//    public string OriTy { get; set; }

//    [JsonProperty("13", NullValueHandling = NullValueHandling.Ignore)]
//    public string The13 { get; set; }

//    [JsonProperty("14", NullValueHandling = NullValueHandling.Ignore)]
//    public string The14 { get; set; }

//    [JsonProperty("15", NullValueHandling = NullValueHandling.Ignore)]
//    public string The15 { get; set; }

//    [JsonProperty("16", NullValueHandling = NullValueHandling.Ignore)]
//    public string The16 { get; set; }

//    [JsonProperty("17", NullValueHandling = NullValueHandling.Ignore)]
//    public string The17 { get; set; }

//    [JsonProperty("18", NullValueHandling = NullValueHandling.Ignore)]
//    public string The18 { get; set; }

//    [JsonProperty("19", NullValueHandling = NullValueHandling.Ignore)]
//    public string The19 { get; set; }

//    [JsonProperty("20", NullValueHandling = NullValueHandling.Ignore)]
//    public string The20 { get; set; }

//    [JsonProperty("21", NullValueHandling = NullValueHandling.Ignore)]
//    public string The21 { get; set; }

//    [JsonProperty("22", NullValueHandling = NullValueHandling.Ignore)]
//    public string The22 { get; set; }

//    [JsonProperty("23", NullValueHandling = NullValueHandling.Ignore)]
//    public string The23 { get; set; }

//    [JsonProperty("24", NullValueHandling = NullValueHandling.Ignore)]
//    public string The24 { get; set; }

//    [JsonProperty("25", NullValueHandling = NullValueHandling.Ignore)]
//    public string The25 { get; set; }

//    [JsonProperty("26", NullValueHandling = NullValueHandling.Ignore)]
//    public string The26 { get; set; }

//    [JsonProperty("27", NullValueHandling = NullValueHandling.Ignore)]
//    public string The27 { get; set; }

//    [JsonProperty("28", NullValueHandling = NullValueHandling.Ignore)]
//    public string The28 { get; set; }

//    [JsonProperty("29", NullValueHandling = NullValueHandling.Ignore)]
//    public string The29 { get; set; }

//    [JsonProperty("30", NullValueHandling = NullValueHandling.Ignore)]
//    public string The30 { get; set; }

//    [JsonProperty("31", NullValueHandling = NullValueHandling.Ignore)]
//    public string The31 { get; set; }

//    [JsonProperty("32", NullValueHandling = NullValueHandling.Ignore)]
//    public string The32 { get; set; }

//    [JsonProperty("33", NullValueHandling = NullValueHandling.Ignore)]
//    public string The33 { get; set; }

//    [JsonProperty("34", NullValueHandling = NullValueHandling.Ignore)]
//    public string The34 { get; set; }

//    [JsonProperty("a", NullValueHandling = NullValueHandling.Ignore)]
//    public A? A { get; set; }

//    [JsonProperty("1", NullValueHandling = NullValueHandling.Ignore)]
//    public The1? The1 { get; set; }

//    [JsonProperty("BCodCod", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? BCodCod { get; set; }

//    [JsonProperty("BCod_Name", NullValueHandling = NullValueHandling.Ignore)]
//    public string BCodName { get; set; }

//    [JsonProperty("BCod_Clarif", NullValueHandling = NullValueHandling.Ignore)]
//    public string BCodClarif { get; set; }

//    [JsonProperty("c1", NullValueHandling = NullValueHandling.Ignore)]
//    [JsonConverter(typeof(ParseStringConverter))]
//    public long? C1 { get; set; }

//    [JsonProperty("c11", NullValueHandling = NullValueHandling.Ignore)]
//    public string C11 { get; set; }
//} 
//#endregion

//#region Enums
//public enum ColName { Activn, DocDate, DocVid, FormSobst, IzgotDate, IzgotOkpo, Nss, Nuclid, Numb, OkpoPip, OkpoPrv, OpCod, OpDate, PaspN, PhCod, PhName, PrName, Pravoobl, Typ, UktPrN };

//public enum User { ИвановаОльгаИвановна, ИсайчеваИринаАнатольевна, ОсетровКириллЮрьевич, ПанфиловаЛарисаЛеонидовна, СуховаАнастасияЮрьевна };

//public enum Source { SgukCheck };

//public enum Q5 { Empty, Q5, The000E00, The02572737, The03293882, The07517462, The07622740, The08577905, The08625082, The32802451080002, The77132143_18035, Бюб, Прим, Текст };

//public enum FormState { Confirmed, Draft, Loaded, Remarks, Verified };

//public enum ActDate { Empty, Прим };

//public enum The1 { Empty, The1_, КомплексСпецводоочистки, УстановкаЦементироания };

//public enum G11Enum { Empty, The500E00 };

//public enum G18Enum { Empty, G18, The12412204142 };

//public enum G19Enum { Empty, Сооружения202 };

//public enum G21 { Empty, The20412234341, The41E01 };

//public enum G23 { Empty, The165E11, The35E00 };

//public enum G24 { Empty, G24, The400E00 };

//public enum G27 { Empty, G27, The176E08 };

//public enum G4Enum { Empty, Контейнер };

//public enum G5Enum { Empty, The09, The12352, Нзк15015П };

//public enum G8Enum { Empty, The16E00, Америций241, Технеций99М };

//public enum OrgVocType { Organizations };

//public enum A { Empty, МестоСбораРао, ПриреакторноеХранилище };

//public enum EdinicaIzmerPR { Лет, Сут, Час };

//public enum KodGruppy { А, Б, Трансурановый, Тритий };

//public enum OsnVidRaspadText { Альфа, Бета, ЗахватЭлектрона, ИзомерныйПереход };

//public enum The10_Enum { Empty, ЗаоРитверц, Канада, Нииар, ПоМаяк };

//public enum The12 { Empty, ИзОтчетаСпбИзотоп, МежпроверочныйИнтервал1Год, НазначенныйСрокСлужбыИсточников70000ЧСДатыИхВыпуска, Степанов, Степапнов };

//public enum The5_Enum { Empty, КомнатаРао };

//public enum The7_Enum { Empty, The00, The10000, The12000000000, The130000, The2100, The370000, The400, The700000, The7_ };

//public enum VidIzluch { Альфа, Бета, Гамма };

//public enum VocVocType { FormSobstVoc, RadionuclideVoc1, The112_Voc, The11_Voc, The120_Voc, The12_Voc, The13_Voc, The16_Voc, The17_Voc, The21_Voc, The23_Voc, The24_Voc, The27_Voc, The28_Voc };
//#endregion

//#region Structs
//public partial struct UdATro
//{
//    public long? Integer;
//    public string String;

//    public static implicit operator UdATro(long Integer) => new UdATro { Integer = Integer };
//    public static implicit operator UdATro(string String) => new UdATro { String = String };
//}

//public partial struct ActDateUnion
//{
//    public DateTimeOffset? DateTime;
//    public ActDate? Enum;

//    public static implicit operator ActDateUnion(DateTimeOffset DateTime) => new ActDateUnion { DateTime = DateTime };
//    public static implicit operator ActDateUnion(ActDate Enum) => new ActDateUnion { Enum = Enum };
//}

//public partial struct Agr
//{
//    public ActDate? Enum;
//    public long? Integer;

//    public static implicit operator Agr(ActDate Enum) => new Agr { Enum = Enum };
//    public static implicit operator Agr(long Integer) => new Agr { Integer = Integer };
//}

//public partial struct Date
//{
//    public DateTimeOffset? DateTime;
//    public Q5? Enum;

//    public static implicit operator Date(DateTimeOffset DateTime) => new Date { DateTime = DateTime };
//    public static implicit operator Date(Q5 Enum) => new Date { Enum = Enum };
//}

//public partial struct DocVid
//{
//    public Q5? Enum;
//    public long? Integer;

//    public static implicit operator DocVid(Q5 Enum) => new DocVid { Enum = Enum };
//    public static implicit operator DocVid(long Integer) => new DocVid { Integer = Integer };
//}

//public partial struct G11Union
//{
//    public G11Enum? Enum;
//    public long? Integer;

//    public static implicit operator G11Union(G11Enum Enum) => new G11Union { Enum = Enum };
//    public static implicit operator G11Union(long Integer) => new G11Union { Integer = Integer };
//}

//public partial struct G18Union
//{
//    public G18Enum? Enum;
//    public long? Integer;

//    public static implicit operator G18Union(G18Enum Enum) => new G18Union { Enum = Enum };
//    public static implicit operator G18Union(long Integer) => new G18Union { Integer = Integer };
//}

//public partial struct G19Union
//{
//    public G19Enum? Enum;
//    public long? Integer;

//    public static implicit operator G19Union(G19Enum Enum) => new G19Union { Enum = Enum };
//    public static implicit operator G19Union(long Integer) => new G19Union { Integer = Integer };
//}

//public partial struct G20
//{
//    public G21? Enum;
//    public long? Integer;

//    public static implicit operator G20(G21 Enum) => new G20 { Enum = Enum };
//    public static implicit operator G20(long Integer) => new G20 { Integer = Integer };
//}

//public partial struct G4Union
//{
//    public G4Enum? Enum;
//    public long? Integer;

//    public static implicit operator G4Union(G4Enum Enum) => new G4Union { Enum = Enum };
//    public static implicit operator G4Union(long Integer) => new G4Union { Integer = Integer };
//}

//public partial struct G5Union
//{
//    public G5Enum? Enum;
//    public long? Integer;

//    public static implicit operator G5Union(G5Enum Enum) => new G5Union { Enum = Enum };
//    public static implicit operator G5Union(long Integer) => new G5Union { Integer = Integer };
//}

//public partial struct G8Union
//{
//    public DateTimeOffset? DateTime;
//    public G8Enum? Enum;

//    public static implicit operator G8Union(DateTimeOffset DateTime) => new G8Union { DateTime = DateTime };
//    public static implicit operator G8Union(G8Enum Enum) => new G8Union { Enum = Enum };
//}

//public partial struct DVal
//{
//    public double? Double;
//    public string String;

//    public static implicit operator DVal(double Double) => new DVal { Double = Double };
//    public static implicit operator DVal(string String) => new DVal { String = String };
//}

//public partial struct The10_Union
//{
//    public DateTimeOffset? DateTime;
//    public The10_Enum? Enum;

//    public static implicit operator The10_Union(DateTimeOffset DateTime) => new The10_Union { DateTime = DateTime };
//    public static implicit operator The10_Union(The10_Enum Enum) => new The10_Union { Enum = Enum };
//}

//public partial struct The5_Union
//{
//    public The5_Enum? Enum;
//    public long? Integer;

//    public static implicit operator The5_Union(The5_Enum Enum) => new The5_Union { Enum = Enum };
//    public static implicit operator The5_Union(long Integer) => new The5_Union { Integer = Integer };
//}

//public partial struct The7_Union
//{
//    public The7_Enum? Enum;
//    public long? Integer;

//    public static implicit operator The7_Union(The7_Enum Enum) => new The7_Union { Enum = Enum };
//    public static implicit operator The7_Union(long Integer) => new The7_Union { Integer = Integer };
//} 
//#endregion

//#region Convertors
//internal static class Converter
//{
//    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
//    {
//        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
//        DateParseHandling = DateParseHandling.None,
//        Converters =
//        {
//            ColNameConverter.Singleton,
//            UserConverter.Singleton,
//            UdATroConverter.Singleton,
//            SourceConverter.Singleton,
//            Q5Converter.Singleton,
//            FormStateConverter.Singleton,
//            ActDateUnionConverter.Singleton,
//            ActDateConverter.Singleton,
//            AgrConverter.Singleton,
//            DateConverter.Singleton,
//            PurpleDocVidConverter.Singleton,
//            DValConverter.Singleton,
//            FluffyDocVidConverter.Singleton,
//            The1Converter.Singleton,
//            G11UnionConverter.Singleton,
//            G11EnumConverter.Singleton,
//            G18UnionConverter.Singleton,
//            G18EnumConverter.Singleton,
//            G19UnionConverter.Singleton,
//            G19EnumConverter.Singleton,
//            G20Converter.Singleton,
//            G21Converter.Singleton,
//            G23Converter.Singleton,
//            G24Converter.Singleton,
//            G27Converter.Singleton,
//            G4UnionConverter.Singleton,
//            G4EnumConverter.Singleton,
//            G5UnionConverter.Singleton,
//            G5EnumConverter.Singleton,
//            G8UnionConverter.Singleton,
//            G8EnumConverter.Singleton,
//            OrgVocTypeConverter.Singleton,
//            The10UnionConverter.Singleton,
//            The10EnumConverter.Singleton,
//            The12Converter.Singleton,
//            The5UnionConverter.Singleton,
//            The5EnumConverter.Singleton,
//            The7UnionConverter.Singleton,
//            The7EnumConverter.Singleton,
//            EdinicaIzmerPrConverter.Singleton,
//            KodGruppyConverter.Singleton,
//            OsnVidRaspadTextConverter.Singleton,
//            VidIzluchConverter.Singleton,
//            AConverter.Singleton,
//            VocVocTypeConverter.Singleton,
//            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
//        },
//    };
//}

//internal class ColNameConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(ColName) || t == typeof(ColName?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "Activn":
//                return ColName.Activn;
//            case "DocDate":
//                return ColName.DocDate;
//            case "DocVid":
//                return ColName.DocVid;
//            case "FormSobst":
//                return ColName.FormSobst;
//            case "IzgotDate":
//                return ColName.IzgotDate;
//            case "IzgotOKPO":
//                return ColName.IzgotOkpo;
//            case "Nss":
//                return ColName.Nss;
//            case "Nuclid":
//                return ColName.Nuclid;
//            case "Numb":
//                return ColName.Numb;
//            case "OkpoPIP":
//                return ColName.OkpoPip;
//            case "OkpoPrv":
//                return ColName.OkpoPrv;
//            case "OpCod":
//                return ColName.OpCod;
//            case "OpDate":
//                return ColName.OpDate;
//            case "PH_Cod":
//                return ColName.PhCod;
//            case "PH_Name":
//                return ColName.PhName;
//            case "PaspN":
//                return ColName.PaspN;
//            case "PrName":
//                return ColName.PrName;
//            case "Pravoobl":
//                return ColName.Pravoobl;
//            case "Typ":
//                return ColName.Typ;
//            case "UktPrN":
//                return ColName.UktPrN;
//        }
//        throw new Exception("Cannot unmarshal type ColName");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (ColName)untypedValue;
//        switch (value)
//        {
//            case ColName.Activn:
//                serializer.Serialize(writer, "Activn");
//                return;
//            case ColName.DocDate:
//                serializer.Serialize(writer, "DocDate");
//                return;
//            case ColName.DocVid:
//                serializer.Serialize(writer, "DocVid");
//                return;
//            case ColName.FormSobst:
//                serializer.Serialize(writer, "FormSobst");
//                return;
//            case ColName.IzgotDate:
//                serializer.Serialize(writer, "IzgotDate");
//                return;
//            case ColName.IzgotOkpo:
//                serializer.Serialize(writer, "IzgotOKPO");
//                return;
//            case ColName.Nss:
//                serializer.Serialize(writer, "Nss");
//                return;
//            case ColName.Nuclid:
//                serializer.Serialize(writer, "Nuclid");
//                return;
//            case ColName.Numb:
//                serializer.Serialize(writer, "Numb");
//                return;
//            case ColName.OkpoPip:
//                serializer.Serialize(writer, "OkpoPIP");
//                return;
//            case ColName.OkpoPrv:
//                serializer.Serialize(writer, "OkpoPrv");
//                return;
//            case ColName.OpCod:
//                serializer.Serialize(writer, "OpCod");
//                return;
//            case ColName.OpDate:
//                serializer.Serialize(writer, "OpDate");
//                return;
//            case ColName.PhCod:
//                serializer.Serialize(writer, "PH_Cod");
//                return;
//            case ColName.PhName:
//                serializer.Serialize(writer, "PH_Name");
//                return;
//            case ColName.PaspN:
//                serializer.Serialize(writer, "PaspN");
//                return;
//            case ColName.PrName:
//                serializer.Serialize(writer, "PrName");
//                return;
//            case ColName.Pravoobl:
//                serializer.Serialize(writer, "Pravoobl");
//                return;
//            case ColName.Typ:
//                serializer.Serialize(writer, "Typ");
//                return;
//            case ColName.UktPrN:
//                serializer.Serialize(writer, "UktPrN");
//                return;
//        }
//        throw new Exception("Cannot marshal type ColName");
//    }

//    public static readonly ColNameConverter Singleton = new ColNameConverter();
//}

//internal class UserConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(User) || t == typeof(User?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "Иванова Ольга Ивановна":
//                return User.ИвановаОльгаИвановна;
//            case "Исайчева Ирина Анатольевна":
//                return User.ИсайчеваИринаАнатольевна;
//            case "Осетров Кирилл Юрьевич":
//                return User.ОсетровКириллЮрьевич;
//            case "Панфилова Лариса Леонидовна":
//                return User.ПанфиловаЛарисаЛеонидовна;
//            case "Сухова Анастасия Юрьевна":
//                return User.СуховаАнастасияЮрьевна;
//        }
//        throw new Exception("Cannot unmarshal type User");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (User)untypedValue;
//        switch (value)
//        {
//            case User.ИвановаОльгаИвановна:
//                serializer.Serialize(writer, "Иванова Ольга Ивановна");
//                return;
//            case User.ИсайчеваИринаАнатольевна:
//                serializer.Serialize(writer, "Исайчева Ирина Анатольевна");
//                return;
//            case User.ОсетровКириллЮрьевич:
//                serializer.Serialize(writer, "Осетров Кирилл Юрьевич");
//                return;
//            case User.ПанфиловаЛарисаЛеонидовна:
//                serializer.Serialize(writer, "Панфилова Лариса Леонидовна");
//                return;
//            case User.СуховаАнастасияЮрьевна:
//                serializer.Serialize(writer, "Сухова Анастасия Юрьевна");
//                return;
//        }
//        throw new Exception("Cannot marshal type User");
//    }

//    public static readonly UserConverter Singleton = new UserConverter();
//}

//internal class UdATroConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(UdATro) || t == typeof(UdATro?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.Integer:
//                var integerValue = serializer.Deserialize<long>(reader);
//                return new UdATro { Integer = integerValue };
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                return new UdATro { String = stringValue };
//        }
//        throw new Exception("Cannot unmarshal type UdATro");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (UdATro)untypedValue;
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value);
//            return;
//        }
//        if (value.String != null)
//        {
//            serializer.Serialize(writer, value.String);
//            return;
//        }
//        throw new Exception("Cannot marshal type UdATro");
//    }

//    public static readonly UdATroConverter Singleton = new UdATroConverter();
//}

//internal class SourceConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(Source) || t == typeof(Source?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        if (value == "sguk_check")
//        {
//            return Source.SgukCheck;
//        }
//        throw new Exception("Cannot unmarshal type Source");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (Source)untypedValue;
//        if (value == Source.SgukCheck)
//        {
//            serializer.Serialize(writer, "sguk_check");
//            return;
//        }
//        throw new Exception("Cannot marshal type Source");
//    }

//    public static readonly SourceConverter Singleton = new SourceConverter();
//}

//internal class ParseStringConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        long l;
//        if (Int64.TryParse(value, out l))
//        {
//            return l;
//        }
//        throw new Exception("Cannot unmarshal type long");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (long)untypedValue;
//        serializer.Serialize(writer, value.ToString());
//        return;
//    }

//    public static readonly ParseStringConverter Singleton = new ParseStringConverter();
//}

//internal class Q5Converter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(Q5) || t == typeof(Q5?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return Q5.Q5;
//            case "-":
//                return Q5.Empty;
//            case "0.00e+00":
//                return Q5.The000E00;
//            case "02572737":
//                return Q5.The02572737;
//            case "03293882":
//                return Q5.The03293882;
//            case "07517462":
//                return Q5.The07517462;
//            case "07622740":
//                return Q5.The07622740;
//            case "08577905":
//                return Q5.The08577905;
//            case "08625082":
//                return Q5.The08625082;
//            case "32802451080002":
//                return Q5.The32802451080002;
//            case "77132143_18035":
//                return Q5.The77132143_18035;
//            case "бюб":
//                return Q5.Бюб;
//            case "прим.":
//                return Q5.Прим;
//            case "текст":
//                return Q5.Текст;
//        }
//        throw new Exception("Cannot unmarshal type Q5");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (Q5)untypedValue;
//        switch (value)
//        {
//            case Q5.Q5:
//                serializer.Serialize(writer, "");
//                return;
//            case Q5.Empty:
//                serializer.Serialize(writer, "-");
//                return;
//            case Q5.The000E00:
//                serializer.Serialize(writer, "0.00e+00");
//                return;
//            case Q5.The02572737:
//                serializer.Serialize(writer, "02572737");
//                return;
//            case Q5.The03293882:
//                serializer.Serialize(writer, "03293882");
//                return;
//            case Q5.The07517462:
//                serializer.Serialize(writer, "07517462");
//                return;
//            case Q5.The07622740:
//                serializer.Serialize(writer, "07622740");
//                return;
//            case Q5.The08577905:
//                serializer.Serialize(writer, "08577905");
//                return;
//            case Q5.The08625082:
//                serializer.Serialize(writer, "08625082");
//                return;
//            case Q5.The32802451080002:
//                serializer.Serialize(writer, "32802451080002");
//                return;
//            case Q5.The77132143_18035:
//                serializer.Serialize(writer, "77132143_18035");
//                return;
//            case Q5.Бюб:
//                serializer.Serialize(writer, "бюб");
//                return;
//            case Q5.Прим:
//                serializer.Serialize(writer, "прим.");
//                return;
//            case Q5.Текст:
//                serializer.Serialize(writer, "текст");
//                return;
//        }
//        throw new Exception("Cannot marshal type Q5");
//    }

//    public static readonly Q5Converter Singleton = new Q5Converter();
//}

//internal class FormStateConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(FormState) || t == typeof(FormState?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "confirmed":
//                return FormState.Confirmed;
//            case "draft":
//                return FormState.Draft;
//            case "loaded":
//                return FormState.Loaded;
//            case "remarks":
//                return FormState.Remarks;
//            case "verified":
//                return FormState.Verified;
//        }
//        throw new Exception("Cannot unmarshal type FormState");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (FormState)untypedValue;
//        switch (value)
//        {
//            case FormState.Confirmed:
//                serializer.Serialize(writer, "confirmed");
//                return;
//            case FormState.Draft:
//                serializer.Serialize(writer, "draft");
//                return;
//            case FormState.Loaded:
//                serializer.Serialize(writer, "loaded");
//                return;
//            case FormState.Remarks:
//                serializer.Serialize(writer, "remarks");
//                return;
//            case FormState.Verified:
//                serializer.Serialize(writer, "verified");
//                return;
//        }
//        throw new Exception("Cannot marshal type FormState");
//    }

//    public static readonly FormStateConverter Singleton = new FormStateConverter();
//}

//internal class ActDateUnionConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(ActDateUnion) || t == typeof(ActDateUnion?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                DateTimeOffset dt;
//                if (DateTimeOffset.TryParse(stringValue, out dt))
//                {
//                    return new ActDateUnion { DateTime = dt };
//                }
//                switch (stringValue)
//                {
//                    case "":
//                        return new ActDateUnion { Enum = ActDate.Empty };
//                    case "прим":
//                        return new ActDateUnion { Enum = ActDate.Прим };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type ActDateUnion");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (ActDateUnion)untypedValue;
//        if (value.DateTime != null)
//        {
//            serializer.Serialize(writer, value.DateTime.Value.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
//            return;
//        }
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case ActDate.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case ActDate.Прим:
//                    serializer.Serialize(writer, "прим");
//                    return;
//            }
//        }
//        throw new Exception("Cannot marshal type ActDateUnion");
//    }

//    public static readonly ActDateUnionConverter Singleton = new ActDateUnionConverter();
//}

//internal class ActDateConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(ActDate) || t == typeof(ActDate?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return ActDate.Empty;
//            case "прим":
//                return ActDate.Прим;
//        }
//        throw new Exception("Cannot unmarshal type ActDate");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (ActDate)untypedValue;
//        switch (value)
//        {
//            case ActDate.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case ActDate.Прим:
//                serializer.Serialize(writer, "прим");
//                return;
//        }
//        throw new Exception("Cannot marshal type ActDate");
//    }

//    public static readonly ActDateConverter Singleton = new ActDateConverter();
//}

//internal class AgrConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(Agr) || t == typeof(Agr?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.Integer:
//                var integerValue = serializer.Deserialize<long>(reader);
//                return new Agr { Integer = integerValue };
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                switch (stringValue)
//                {
//                    case "":
//                        return new Agr { Enum = ActDate.Empty };
//                    case "прим":
//                        return new Agr { Enum = ActDate.Прим };
//                }
//                long l;
//                if (Int64.TryParse(stringValue, out l))
//                {
//                    return new Agr { Integer = l };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type Agr");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (Agr)untypedValue;
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value);
//            return;
//        }
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case ActDate.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case ActDate.Прим:
//                    serializer.Serialize(writer, "прим");
//                    return;
//            }
//        }
//        throw new Exception("Cannot marshal type Agr");
//    }

//    public static readonly AgrConverter Singleton = new AgrConverter();
//}

//internal class DateConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(Date) || t == typeof(Date?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                DateTimeOffset dt;
//                if (DateTimeOffset.TryParse(stringValue, out dt))
//                {
//                    return new Date { DateTime = dt };
//                }
//                switch (stringValue)
//                {
//                    case "":
//                        return new Date { Enum = Q5.Q5 };
//                    case "-":
//                        return new Date { Enum = Q5.Empty };
//                    case "0.00e+00":
//                        return new Date { Enum = Q5.The000E00 };
//                    case "02572737":
//                        return new Date { Enum = Q5.The02572737 };
//                    case "03293882":
//                        return new Date { Enum = Q5.The03293882 };
//                    case "07517462":
//                        return new Date { Enum = Q5.The07517462 };
//                    case "07622740":
//                        return new Date { Enum = Q5.The07622740 };
//                    case "08577905":
//                        return new Date { Enum = Q5.The08577905 };
//                    case "08625082":
//                        return new Date { Enum = Q5.The08625082 };
//                    case "32802451080002":
//                        return new Date { Enum = Q5.The32802451080002 };
//                    case "77132143_18035":
//                        return new Date { Enum = Q5.The77132143_18035 };
//                    case "бюб":
//                        return new Date { Enum = Q5.Бюб };
//                    case "прим.":
//                        return new Date { Enum = Q5.Прим };
//                    case "текст":
//                        return new Date { Enum = Q5.Текст };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type Date");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (Date)untypedValue;
//        if (value.DateTime != null)
//        {
//            serializer.Serialize(writer, value.DateTime.Value.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
//            return;
//        }
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case Q5.Q5:
//                    serializer.Serialize(writer, "");
//                    return;
//                case Q5.Empty:
//                    serializer.Serialize(writer, "-");
//                    return;
//                case Q5.The000E00:
//                    serializer.Serialize(writer, "0.00e+00");
//                    return;
//                case Q5.The02572737:
//                    serializer.Serialize(writer, "02572737");
//                    return;
//                case Q5.The03293882:
//                    serializer.Serialize(writer, "03293882");
//                    return;
//                case Q5.The07517462:
//                    serializer.Serialize(writer, "07517462");
//                    return;
//                case Q5.The07622740:
//                    serializer.Serialize(writer, "07622740");
//                    return;
//                case Q5.The08577905:
//                    serializer.Serialize(writer, "08577905");
//                    return;
//                case Q5.The08625082:
//                    serializer.Serialize(writer, "08625082");
//                    return;
//                case Q5.The32802451080002:
//                    serializer.Serialize(writer, "32802451080002");
//                    return;
//                case Q5.The77132143_18035:
//                    serializer.Serialize(writer, "77132143_18035");
//                    return;
//                case Q5.Бюб:
//                    serializer.Serialize(writer, "бюб");
//                    return;
//                case Q5.Прим:
//                    serializer.Serialize(writer, "прим.");
//                    return;
//                case Q5.Текст:
//                    serializer.Serialize(writer, "текст");
//                    return;
//            }
//        }
//        throw new Exception("Cannot marshal type Date");
//    }

//    public static readonly DateConverter Singleton = new DateConverter();
//}

//internal class PurpleDocVidConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(DocVid) || t == typeof(DocVid?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                switch (stringValue)
//                {
//                    case "":
//                        return new DocVid { Enum = Q5.Q5 };
//                    case "-":
//                        return new DocVid { Enum = Q5.Empty };
//                    case "0.00e+00":
//                        return new DocVid { Enum = Q5.The000E00 };
//                    case "02572737":
//                        return new DocVid { Enum = Q5.The02572737 };
//                    case "03293882":
//                        return new DocVid { Enum = Q5.The03293882 };
//                    case "07517462":
//                        return new DocVid { Enum = Q5.The07517462 };
//                    case "07622740":
//                        return new DocVid { Enum = Q5.The07622740 };
//                    case "08577905":
//                        return new DocVid { Enum = Q5.The08577905 };
//                    case "08625082":
//                        return new DocVid { Enum = Q5.The08625082 };
//                    case "32802451080002":
//                        return new DocVid { Enum = Q5.The32802451080002 };
//                    case "77132143_18035":
//                        return new DocVid { Enum = Q5.The77132143_18035 };
//                    case "бюб":
//                        return new DocVid { Enum = Q5.Бюб };
//                    case "прим.":
//                        return new DocVid { Enum = Q5.Прим };
//                    case "текст":
//                        return new DocVid { Enum = Q5.Текст };
//                }
//                long l;
//                if (Int64.TryParse(stringValue, out l))
//                {
//                    return new DocVid { Integer = l };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type DocVid");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (DocVid)untypedValue;
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case Q5.Q5:
//                    serializer.Serialize(writer, "");
//                    return;
//                case Q5.Empty:
//                    serializer.Serialize(writer, "-");
//                    return;
//                case Q5.The000E00:
//                    serializer.Serialize(writer, "0.00e+00");
//                    return;
//                case Q5.The02572737:
//                    serializer.Serialize(writer, "02572737");
//                    return;
//                case Q5.The03293882:
//                    serializer.Serialize(writer, "03293882");
//                    return;
//                case Q5.The07517462:
//                    serializer.Serialize(writer, "07517462");
//                    return;
//                case Q5.The07622740:
//                    serializer.Serialize(writer, "07622740");
//                    return;
//                case Q5.The08577905:
//                    serializer.Serialize(writer, "08577905");
//                    return;
//                case Q5.The08625082:
//                    serializer.Serialize(writer, "08625082");
//                    return;
//                case Q5.The32802451080002:
//                    serializer.Serialize(writer, "32802451080002");
//                    return;
//                case Q5.The77132143_18035:
//                    serializer.Serialize(writer, "77132143_18035");
//                    return;
//                case Q5.Бюб:
//                    serializer.Serialize(writer, "бюб");
//                    return;
//                case Q5.Прим:
//                    serializer.Serialize(writer, "прим.");
//                    return;
//                case Q5.Текст:
//                    serializer.Serialize(writer, "текст");
//                    return;
//            }
//        }
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value.ToString());
//            return;
//        }
//        throw new Exception("Cannot marshal type DocVid");
//    }

//    public static readonly PurpleDocVidConverter Singleton = new PurpleDocVidConverter();
//}

//internal class DValConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(DVal) || t == typeof(DVal?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.Integer:
//            case JsonToken.Float:
//                var doubleValue = serializer.Deserialize<double>(reader);
//                return new DVal { Double = doubleValue };
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                return new DVal { String = stringValue };
//        }
//        throw new Exception("Cannot unmarshal type DVal");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (DVal)untypedValue;
//        if (value.Double != null)
//        {
//            serializer.Serialize(writer, value.Double.Value);
//            return;
//        }
//        if (value.String != null)
//        {
//            serializer.Serialize(writer, value.String);
//            return;
//        }
//        throw new Exception("Cannot marshal type DVal");
//    }

//    public static readonly DValConverter Singleton = new DValConverter();
//}

//internal class FluffyDocVidConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(DocVid) || t == typeof(DocVid?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.Integer:
//                var integerValue = serializer.Deserialize<long>(reader);
//                return new DocVid { Integer = integerValue };
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                switch (stringValue)
//                {
//                    case "":
//                        return new DocVid { Enum = Q5.Q5 };
//                    case "-":
//                        return new DocVid { Enum = Q5.Empty };
//                    case "0.00e+00":
//                        return new DocVid { Enum = Q5.The000E00 };
//                    case "02572737":
//                        return new DocVid { Enum = Q5.The02572737 };
//                    case "03293882":
//                        return new DocVid { Enum = Q5.The03293882 };
//                    case "07517462":
//                        return new DocVid { Enum = Q5.The07517462 };
//                    case "07622740":
//                        return new DocVid { Enum = Q5.The07622740 };
//                    case "08577905":
//                        return new DocVid { Enum = Q5.The08577905 };
//                    case "08625082":
//                        return new DocVid { Enum = Q5.The08625082 };
//                    case "32802451080002":
//                        return new DocVid { Enum = Q5.The32802451080002 };
//                    case "77132143_18035":
//                        return new DocVid { Enum = Q5.The77132143_18035 };
//                    case "бюб":
//                        return new DocVid { Enum = Q5.Бюб };
//                    case "прим.":
//                        return new DocVid { Enum = Q5.Прим };
//                    case "текст":
//                        return new DocVid { Enum = Q5.Текст };
//                }
//                long l;
//                if (Int64.TryParse(stringValue, out l))
//                {
//                    return new DocVid { Integer = l };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type DocVid");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (DocVid)untypedValue;
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value);
//            return;
//        }
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case Q5.Q5:
//                    serializer.Serialize(writer, "");
//                    return;
//                case Q5.Empty:
//                    serializer.Serialize(writer, "-");
//                    return;
//                case Q5.The000E00:
//                    serializer.Serialize(writer, "0.00e+00");
//                    return;
//                case Q5.The02572737:
//                    serializer.Serialize(writer, "02572737");
//                    return;
//                case Q5.The03293882:
//                    serializer.Serialize(writer, "03293882");
//                    return;
//                case Q5.The07517462:
//                    serializer.Serialize(writer, "07517462");
//                    return;
//                case Q5.The07622740:
//                    serializer.Serialize(writer, "07622740");
//                    return;
//                case Q5.The08577905:
//                    serializer.Serialize(writer, "08577905");
//                    return;
//                case Q5.The08625082:
//                    serializer.Serialize(writer, "08625082");
//                    return;
//                case Q5.The32802451080002:
//                    serializer.Serialize(writer, "32802451080002");
//                    return;
//                case Q5.The77132143_18035:
//                    serializer.Serialize(writer, "77132143_18035");
//                    return;
//                case Q5.Бюб:
//                    serializer.Serialize(writer, "бюб");
//                    return;
//                case Q5.Прим:
//                    serializer.Serialize(writer, "прим.");
//                    return;
//                case Q5.Текст:
//                    serializer.Serialize(writer, "текст");
//                    return;
//            }
//        }
//        throw new Exception("Cannot marshal type DocVid");
//    }

//    public static readonly FluffyDocVidConverter Singleton = new FluffyDocVidConverter();
//}

//internal class The1Converter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(The1) || t == typeof(The1?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return The1.The1_;
//            case "-":
//                return The1.Empty;
//            case "комплекс спецводоочистки":
//                return The1.КомплексСпецводоочистки;
//            case "установка цементироания":
//                return The1.УстановкаЦементироания;
//        }
//        throw new Exception("Cannot unmarshal type The1");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (The1)untypedValue;
//        switch (value)
//        {
//            case The1.The1_:
//                serializer.Serialize(writer, "");
//                return;
//            case The1.Empty:
//                serializer.Serialize(writer, "-");
//                return;
//            case The1.КомплексСпецводоочистки:
//                serializer.Serialize(writer, "комплекс спецводоочистки");
//                return;
//            case The1.УстановкаЦементироания:
//                serializer.Serialize(writer, "установка цементироания");
//                return;
//        }
//        throw new Exception("Cannot marshal type The1");
//    }

//    public static readonly The1Converter Singleton = new The1Converter();
//}

//internal class G11UnionConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G11Union) || t == typeof(G11Union?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                switch (stringValue)
//                {
//                    case "":
//                        return new G11Union { Enum = G11Enum.Empty };
//                    case "5.00e+00":
//                        return new G11Union { Enum = G11Enum.The500E00 };
//                }
//                long l;
//                if (Int64.TryParse(stringValue, out l))
//                {
//                    return new G11Union { Integer = l };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type G11Union");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (G11Union)untypedValue;
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case G11Enum.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case G11Enum.The500E00:
//                    serializer.Serialize(writer, "5.00e+00");
//                    return;
//            }
//        }
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value.ToString());
//            return;
//        }
//        throw new Exception("Cannot marshal type G11Union");
//    }

//    public static readonly G11UnionConverter Singleton = new G11UnionConverter();
//}

//internal class G11EnumConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G11Enum) || t == typeof(G11Enum?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return G11Enum.Empty;
//            case "5.00e+00":
//                return G11Enum.The500E00;
//        }
//        throw new Exception("Cannot unmarshal type G11Enum");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (G11Enum)untypedValue;
//        switch (value)
//        {
//            case G11Enum.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case G11Enum.The500E00:
//                serializer.Serialize(writer, "5.00e+00");
//                return;
//        }
//        throw new Exception("Cannot marshal type G11Enum");
//    }

//    public static readonly G11EnumConverter Singleton = new G11EnumConverter();
//}

//internal class G18UnionConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G18Union) || t == typeof(G18Union?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                switch (stringValue)
//                {
//                    case "":
//                        return new G18Union { Enum = G18Enum.Empty };
//                    case "-":
//                        return new G18Union { Enum = G18Enum.G18 };
//                    case "12412204142":
//                        return new G18Union { Enum = G18Enum.The12412204142 };
//                }
//                long l;
//                if (Int64.TryParse(stringValue, out l))
//                {
//                    return new G18Union { Integer = l };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type G18Union");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (G18Union)untypedValue;
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case G18Enum.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case G18Enum.G18:
//                    serializer.Serialize(writer, "-");
//                    return;
//                case G18Enum.The12412204142:
//                    serializer.Serialize(writer, "12412204142");
//                    return;
//            }
//        }
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value.ToString());
//            return;
//        }
//        throw new Exception("Cannot marshal type G18Union");
//    }

//    public static readonly G18UnionConverter Singleton = new G18UnionConverter();
//}

//internal class G18EnumConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G18Enum) || t == typeof(G18Enum?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return G18Enum.Empty;
//            case "-":
//                return G18Enum.G18;
//            case "12412204142":
//                return G18Enum.The12412204142;
//        }
//        throw new Exception("Cannot unmarshal type G18Enum");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (G18Enum)untypedValue;
//        switch (value)
//        {
//            case G18Enum.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case G18Enum.G18:
//                serializer.Serialize(writer, "-");
//                return;
//            case G18Enum.The12412204142:
//                serializer.Serialize(writer, "12412204142");
//                return;
//        }
//        throw new Exception("Cannot marshal type G18Enum");
//    }

//    public static readonly G18EnumConverter Singleton = new G18EnumConverter();
//}

//internal class G19UnionConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G19Union) || t == typeof(G19Union?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                switch (stringValue)
//                {
//                    case "":
//                        return new G19Union { Enum = G19Enum.Empty };
//                    case "Сооружения 20/2":
//                        return new G19Union { Enum = G19Enum.Сооружения202 };
//                }
//                long l;
//                if (Int64.TryParse(stringValue, out l))
//                {
//                    return new G19Union { Integer = l };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type G19Union");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (G19Union)untypedValue;
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case G19Enum.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case G19Enum.Сооружения202:
//                    serializer.Serialize(writer, "Сооружения 20/2");
//                    return;
//            }
//        }
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value.ToString());
//            return;
//        }
//        throw new Exception("Cannot marshal type G19Union");
//    }

//    public static readonly G19UnionConverter Singleton = new G19UnionConverter();
//}

//internal class G19EnumConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G19Enum) || t == typeof(G19Enum?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return G19Enum.Empty;
//            case "Сооружения 20/2":
//                return G19Enum.Сооружения202;
//        }
//        throw new Exception("Cannot unmarshal type G19Enum");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (G19Enum)untypedValue;
//        switch (value)
//        {
//            case G19Enum.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case G19Enum.Сооружения202:
//                serializer.Serialize(writer, "Сооружения 20/2");
//                return;
//        }
//        throw new Exception("Cannot marshal type G19Enum");
//    }

//    public static readonly G19EnumConverter Singleton = new G19EnumConverter();
//}

//internal class G20Converter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G20) || t == typeof(G20?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                switch (stringValue)
//                {
//                    case "":
//                        return new G20 { Enum = G21.Empty };
//                    case "20412234341":
//                        return new G20 { Enum = G21.The20412234341 };
//                    case "4.1e+01":
//                        return new G20 { Enum = G21.The41E01 };
//                }
//                long l;
//                if (Int64.TryParse(stringValue, out l))
//                {
//                    return new G20 { Integer = l };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type G20");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (G20)untypedValue;
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case G21.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case G21.The20412234341:
//                    serializer.Serialize(writer, "20412234341");
//                    return;
//                case G21.The41E01:
//                    serializer.Serialize(writer, "4.1e+01");
//                    return;
//            }
//        }
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value.ToString());
//            return;
//        }
//        throw new Exception("Cannot marshal type G20");
//    }

//    public static readonly G20Converter Singleton = new G20Converter();
//}

//internal class G21Converter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G21) || t == typeof(G21?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return G21.Empty;
//            case "20412234341":
//                return G21.The20412234341;
//            case "4.1e+01":
//                return G21.The41E01;
//        }
//        throw new Exception("Cannot unmarshal type G21");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (G21)untypedValue;
//        switch (value)
//        {
//            case G21.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case G21.The20412234341:
//                serializer.Serialize(writer, "20412234341");
//                return;
//            case G21.The41E01:
//                serializer.Serialize(writer, "4.1e+01");
//                return;
//        }
//        throw new Exception("Cannot marshal type G21");
//    }

//    public static readonly G21Converter Singleton = new G21Converter();
//}

//internal class G23Converter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G23) || t == typeof(G23?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return G23.Empty;
//            case "1.65e+11":
//                return G23.The165E11;
//            case "3.5e+00":
//                return G23.The35E00;
//        }
//        throw new Exception("Cannot unmarshal type G23");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (G23)untypedValue;
//        switch (value)
//        {
//            case G23.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case G23.The165E11:
//                serializer.Serialize(writer, "1.65e+11");
//                return;
//            case G23.The35E00:
//                serializer.Serialize(writer, "3.5e+00");
//                return;
//        }
//        throw new Exception("Cannot marshal type G23");
//    }

//    public static readonly G23Converter Singleton = new G23Converter();
//}

//internal class G24Converter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G24) || t == typeof(G24?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return G24.G24;
//            case "-":
//                return G24.Empty;
//            case "4.00e+00":
//                return G24.The400E00;
//        }
//        throw new Exception("Cannot unmarshal type G24");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (G24)untypedValue;
//        switch (value)
//        {
//            case G24.G24:
//                serializer.Serialize(writer, "");
//                return;
//            case G24.Empty:
//                serializer.Serialize(writer, "-");
//                return;
//            case G24.The400E00:
//                serializer.Serialize(writer, "4.00e+00");
//                return;
//        }
//        throw new Exception("Cannot marshal type G24");
//    }

//    public static readonly G24Converter Singleton = new G24Converter();
//}

//internal class G27Converter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G27) || t == typeof(G27?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return G27.G27;
//            case "-":
//                return G27.Empty;
//            case "1.76e+08":
//                return G27.The176E08;
//        }
//        throw new Exception("Cannot unmarshal type G27");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (G27)untypedValue;
//        switch (value)
//        {
//            case G27.G27:
//                serializer.Serialize(writer, "");
//                return;
//            case G27.Empty:
//                serializer.Serialize(writer, "-");
//                return;
//            case G27.The176E08:
//                serializer.Serialize(writer, "1.76e+08");
//                return;
//        }
//        throw new Exception("Cannot marshal type G27");
//    }

//    public static readonly G27Converter Singleton = new G27Converter();
//}

//internal class G4UnionConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G4Union) || t == typeof(G4Union?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                switch (stringValue)
//                {
//                    case "":
//                        return new G4Union { Enum = G4Enum.Empty };
//                    case "контейнер":
//                        return new G4Union { Enum = G4Enum.Контейнер };
//                }
//                long l;
//                if (Int64.TryParse(stringValue, out l))
//                {
//                    return new G4Union { Integer = l };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type G4Union");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (G4Union)untypedValue;
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case G4Enum.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case G4Enum.Контейнер:
//                    serializer.Serialize(writer, "контейнер");
//                    return;
//            }
//        }
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value.ToString());
//            return;
//        }
//        throw new Exception("Cannot marshal type G4Union");
//    }

//    public static readonly G4UnionConverter Singleton = new G4UnionConverter();
//}

//internal class G4EnumConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G4Enum) || t == typeof(G4Enum?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return G4Enum.Empty;
//            case "контейнер":
//                return G4Enum.Контейнер;
//        }
//        throw new Exception("Cannot unmarshal type G4Enum");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (G4Enum)untypedValue;
//        switch (value)
//        {
//            case G4Enum.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case G4Enum.Контейнер:
//                serializer.Serialize(writer, "контейнер");
//                return;
//        }
//        throw new Exception("Cannot marshal type G4Enum");
//    }

//    public static readonly G4EnumConverter Singleton = new G4EnumConverter();
//}

//internal class G5UnionConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G5Union) || t == typeof(G5Union?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                switch (stringValue)
//                {
//                    case "":
//                        return new G5Union { Enum = G5Enum.Empty };
//                    case "09":
//                        return new G5Union { Enum = G5Enum.The09 };
//                    case "123/52":
//                        return new G5Union { Enum = G5Enum.The12352 };
//                    case "НЗК-150-1,5П":
//                        return new G5Union { Enum = G5Enum.Нзк15015П };
//                }
//                long l;
//                if (Int64.TryParse(stringValue, out l))
//                {
//                    return new G5Union { Integer = l };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type G5Union");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (G5Union)untypedValue;
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case G5Enum.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case G5Enum.The09:
//                    serializer.Serialize(writer, "09");
//                    return;
//                case G5Enum.The12352:
//                    serializer.Serialize(writer, "123/52");
//                    return;
//                case G5Enum.Нзк15015П:
//                    serializer.Serialize(writer, "НЗК-150-1,5П");
//                    return;
//            }
//        }
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value.ToString());
//            return;
//        }
//        throw new Exception("Cannot marshal type G5Union");
//    }

//    public static readonly G5UnionConverter Singleton = new G5UnionConverter();
//}

//internal class G5EnumConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G5Enum) || t == typeof(G5Enum?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return G5Enum.Empty;
//            case "09":
//                return G5Enum.The09;
//            case "123/52":
//                return G5Enum.The12352;
//            case "НЗК-150-1,5П":
//                return G5Enum.Нзк15015П;
//        }
//        throw new Exception("Cannot unmarshal type G5Enum");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (G5Enum)untypedValue;
//        switch (value)
//        {
//            case G5Enum.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case G5Enum.The09:
//                serializer.Serialize(writer, "09");
//                return;
//            case G5Enum.The12352:
//                serializer.Serialize(writer, "123/52");
//                return;
//            case G5Enum.Нзк15015П:
//                serializer.Serialize(writer, "НЗК-150-1,5П");
//                return;
//        }
//        throw new Exception("Cannot marshal type G5Enum");
//    }

//    public static readonly G5EnumConverter Singleton = new G5EnumConverter();
//}

//internal class G8UnionConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G8Union) || t == typeof(G8Union?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                DateTimeOffset dt;
//                if (DateTimeOffset.TryParse(stringValue, out dt))
//                {
//                    return new G8Union { DateTime = dt };
//                }
//                switch (stringValue)
//                {
//                    case "":
//                        return new G8Union { Enum = G8Enum.Empty };
//                    case "1.6e+00":
//                        return new G8Union { Enum = G8Enum.The16E00 };
//                    case "америций-241":
//                        return new G8Union { Enum = G8Enum.Америций241 };
//                    case "технеций-99м":
//                        return new G8Union { Enum = G8Enum.Технеций99М };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type G8Union");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (G8Union)untypedValue;
//        if (value.DateTime != null)
//        {
//            serializer.Serialize(writer, value.DateTime.Value.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
//            return;
//        }
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case G8Enum.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case G8Enum.The16E00:
//                    serializer.Serialize(writer, "1.6e+00");
//                    return;
//                case G8Enum.Америций241:
//                    serializer.Serialize(writer, "америций-241");
//                    return;
//                case G8Enum.Технеций99М:
//                    serializer.Serialize(writer, "технеций-99м");
//                    return;
//            }
//        }
//        throw new Exception("Cannot marshal type G8Union");
//    }

//    public static readonly G8UnionConverter Singleton = new G8UnionConverter();
//}

//internal class G8EnumConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(G8Enum) || t == typeof(G8Enum?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return G8Enum.Empty;
//            case "1.6e+00":
//                return G8Enum.The16E00;
//            case "америций-241":
//                return G8Enum.Америций241;
//            case "технеций-99м":
//                return G8Enum.Технеций99М;
//        }
//        throw new Exception("Cannot unmarshal type G8Enum");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (G8Enum)untypedValue;
//        switch (value)
//        {
//            case G8Enum.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case G8Enum.The16E00:
//                serializer.Serialize(writer, "1.6e+00");
//                return;
//            case G8Enum.Америций241:
//                serializer.Serialize(writer, "америций-241");
//                return;
//            case G8Enum.Технеций99М:
//                serializer.Serialize(writer, "технеций-99м");
//                return;
//        }
//        throw new Exception("Cannot marshal type G8Enum");
//    }

//    public static readonly G8EnumConverter Singleton = new G8EnumConverter();
//}

//internal class OrgVocTypeConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(OrgVocType) || t == typeof(OrgVocType?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        if (value == "organizations")
//        {
//            return OrgVocType.Organizations;
//        }
//        throw new Exception("Cannot unmarshal type OrgVocType");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (OrgVocType)untypedValue;
//        if (value == OrgVocType.Organizations)
//        {
//            serializer.Serialize(writer, "organizations");
//            return;
//        }
//        throw new Exception("Cannot marshal type OrgVocType");
//    }

//    public static readonly OrgVocTypeConverter Singleton = new OrgVocTypeConverter();
//}

//internal class The10UnionConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(The10_Union) || t == typeof(The10_Union?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                DateTimeOffset dt;
//                if (DateTimeOffset.TryParse(stringValue, out dt))
//                {
//                    return new The10_Union { DateTime = dt };
//                }
//                switch (stringValue)
//                {
//                    case "":
//                        return new The10_Union { Enum = The10_Enum.Empty };
//                    case "ЗАО РИТВЕРЦ":
//                        return new The10_Union { Enum = The10_Enum.ЗаоРитверц };
//                    case "Канада":
//                        return new The10_Union { Enum = The10_Enum.Канада };
//                    case "НИИАР":
//                        return new The10_Union { Enum = The10_Enum.Нииар };
//                    case "ПО Маяк":
//                        return new The10_Union { Enum = The10_Enum.ПоМаяк };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type The10_Union");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (The10_Union)untypedValue;
//        if (value.DateTime != null)
//        {
//            serializer.Serialize(writer, value.DateTime.Value.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
//            return;
//        }
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case The10_Enum.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case The10_Enum.ЗаоРитверц:
//                    serializer.Serialize(writer, "ЗАО РИТВЕРЦ");
//                    return;
//                case The10_Enum.Канада:
//                    serializer.Serialize(writer, "Канада");
//                    return;
//                case The10_Enum.Нииар:
//                    serializer.Serialize(writer, "НИИАР");
//                    return;
//                case The10_Enum.ПоМаяк:
//                    serializer.Serialize(writer, "ПО Маяк");
//                    return;
//            }
//        }
//        throw new Exception("Cannot marshal type The10_Union");
//    }

//    public static readonly The10UnionConverter Singleton = new The10UnionConverter();
//}

//internal class The10EnumConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(The10_Enum) || t == typeof(The10_Enum?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return The10_Enum.Empty;
//            case "ЗАО РИТВЕРЦ":
//                return The10_Enum.ЗаоРитверц;
//            case "Канада":
//                return The10_Enum.Канада;
//            case "НИИАР":
//                return The10_Enum.Нииар;
//            case "ПО Маяк":
//                return The10_Enum.ПоМаяк;
//        }
//        throw new Exception("Cannot unmarshal type The10_Enum");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (The10_Enum)untypedValue;
//        switch (value)
//        {
//            case The10_Enum.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case The10_Enum.ЗаоРитверц:
//                serializer.Serialize(writer, "ЗАО РИТВЕРЦ");
//                return;
//            case The10_Enum.Канада:
//                serializer.Serialize(writer, "Канада");
//                return;
//            case The10_Enum.Нииар:
//                serializer.Serialize(writer, "НИИАР");
//                return;
//            case The10_Enum.ПоМаяк:
//                serializer.Serialize(writer, "ПО Маяк");
//                return;
//        }
//        throw new Exception("Cannot marshal type The10_Enum");
//    }

//    public static readonly The10EnumConverter Singleton = new The10EnumConverter();
//}

//internal class The12Converter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(The12) || t == typeof(The12?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return The12.Empty;
//            case "Межпроверочный интервал: 1 год":
//                return The12.МежпроверочныйИнтервал1Год;
//            case "Назначенный срок службы источников – 70000 ч с даты их выпуска.":
//                return The12.НазначенныйСрокСлужбыИсточников70000ЧСДатыИхВыпуска;
//            case "Степанов":
//                return The12.Степанов;
//            case "Степапнов":
//                return The12.Степапнов;
//            case "из отчета Спб Изотоп":
//                return The12.ИзОтчетаСпбИзотоп;
//        }
//        throw new Exception("Cannot unmarshal type The12");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (The12)untypedValue;
//        switch (value)
//        {
//            case The12.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case The12.МежпроверочныйИнтервал1Год:
//                serializer.Serialize(writer, "Межпроверочный интервал: 1 год");
//                return;
//            case The12.НазначенныйСрокСлужбыИсточников70000ЧСДатыИхВыпуска:
//                serializer.Serialize(writer, "Назначенный срок службы источников – 70000 ч с даты их выпуска.");
//                return;
//            case The12.Степанов:
//                serializer.Serialize(writer, "Степанов");
//                return;
//            case The12.Степапнов:
//                serializer.Serialize(writer, "Степапнов");
//                return;
//            case The12.ИзОтчетаСпбИзотоп:
//                serializer.Serialize(writer, "из отчета Спб Изотоп");
//                return;
//        }
//        throw new Exception("Cannot marshal type The12");
//    }

//    public static readonly The12Converter Singleton = new The12Converter();
//}

//internal class The5UnionConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(The5_Union) || t == typeof(The5_Union?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                switch (stringValue)
//                {
//                    case "":
//                        return new The5_Union { Enum = The5_Enum.Empty };
//                    case "Комната РАО":
//                        return new The5_Union { Enum = The5_Enum.КомнатаРао };
//                }
//                long l;
//                if (Int64.TryParse(stringValue, out l))
//                {
//                    return new The5_Union { Integer = l };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type The5_Union");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (The5_Union)untypedValue;
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case The5_Enum.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case The5_Enum.КомнатаРао:
//                    serializer.Serialize(writer, "Комната РАО");
//                    return;
//            }
//        }
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value.ToString());
//            return;
//        }
//        throw new Exception("Cannot marshal type The5_Union");
//    }

//    public static readonly The5UnionConverter Singleton = new The5UnionConverter();
//}

//internal class The5EnumConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(The5_Enum) || t == typeof(The5_Enum?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return The5_Enum.Empty;
//            case "Комната РАО":
//                return The5_Enum.КомнатаРао;
//        }
//        throw new Exception("Cannot unmarshal type The5_Enum");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (The5_Enum)untypedValue;
//        switch (value)
//        {
//            case The5_Enum.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case The5_Enum.КомнатаРао:
//                serializer.Serialize(writer, "Комната РАО");
//                return;
//        }
//        throw new Exception("Cannot marshal type The5_Enum");
//    }

//    public static readonly The5EnumConverter Singleton = new The5EnumConverter();
//}

//internal class The7UnionConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(The7_Union) || t == typeof(The7_Union?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        switch (reader.TokenType)
//        {
//            case JsonToken.String:
//            case JsonToken.Date:
//                var stringValue = serializer.Deserialize<string>(reader);
//                switch (stringValue)
//                {
//                    case "":
//                        return new The7_Union { Enum = The7_Enum.Empty };
//                    case "-":
//                        return new The7_Union { Enum = The7_Enum.The7_ };
//                    case "0.0":
//                        return new The7_Union { Enum = The7_Enum.The00 };
//                    case "1000.0":
//                        return new The7_Union { Enum = The7_Enum.The10000 };
//                    case "1200000000.0":
//                        return new The7_Union { Enum = The7_Enum.The12000000000 };
//                    case "13000.0":
//                        return new The7_Union { Enum = The7_Enum.The130000 };
//                    case "210.0":
//                        return new The7_Union { Enum = The7_Enum.The2100 };
//                    case "37000.0":
//                        return new The7_Union { Enum = The7_Enum.The370000 };
//                    case "40.0":
//                        return new The7_Union { Enum = The7_Enum.The400 };
//                    case "70000.0":
//                        return new The7_Union { Enum = The7_Enum.The700000 };
//                }
//                long l;
//                if (Int64.TryParse(stringValue, out l))
//                {
//                    return new The7_Union { Integer = l };
//                }
//                break;
//        }
//        throw new Exception("Cannot unmarshal type The7_Union");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        var value = (The7_Union)untypedValue;
//        if (value.Enum != null)
//        {
//            switch (value.Enum)
//            {
//                case The7_Enum.Empty:
//                    serializer.Serialize(writer, "");
//                    return;
//                case The7_Enum.The7_:
//                    serializer.Serialize(writer, "-");
//                    return;
//                case The7_Enum.The00:
//                    serializer.Serialize(writer, "0.0");
//                    return;
//                case The7_Enum.The10000:
//                    serializer.Serialize(writer, "1000.0");
//                    return;
//                case The7_Enum.The12000000000:
//                    serializer.Serialize(writer, "1200000000.0");
//                    return;
//                case The7_Enum.The130000:
//                    serializer.Serialize(writer, "13000.0");
//                    return;
//                case The7_Enum.The2100:
//                    serializer.Serialize(writer, "210.0");
//                    return;
//                case The7_Enum.The370000:
//                    serializer.Serialize(writer, "37000.0");
//                    return;
//                case The7_Enum.The400:
//                    serializer.Serialize(writer, "40.0");
//                    return;
//                case The7_Enum.The700000:
//                    serializer.Serialize(writer, "70000.0");
//                    return;
//            }
//        }
//        if (value.Integer != null)
//        {
//            serializer.Serialize(writer, value.Integer.Value.ToString());
//            return;
//        }
//        throw new Exception("Cannot marshal type The7_Union");
//    }

//    public static readonly The7UnionConverter Singleton = new The7UnionConverter();
//}

//internal class The7EnumConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(The7_Enum) || t == typeof(The7_Enum?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return The7_Enum.Empty;
//            case "-":
//                return The7_Enum.The7_;
//            case "0.0":
//                return The7_Enum.The00;
//            case "1000.0":
//                return The7_Enum.The10000;
//            case "1200000000.0":
//                return The7_Enum.The12000000000;
//            case "13000.0":
//                return The7_Enum.The130000;
//            case "210.0":
//                return The7_Enum.The2100;
//            case "37000.0":
//                return The7_Enum.The370000;
//            case "40.0":
//                return The7_Enum.The400;
//            case "70000.0":
//                return The7_Enum.The700000;
//        }
//        throw new Exception("Cannot unmarshal type The7_Enum");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (The7_Enum)untypedValue;
//        switch (value)
//        {
//            case The7_Enum.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case The7_Enum.The7_:
//                serializer.Serialize(writer, "-");
//                return;
//            case The7_Enum.The00:
//                serializer.Serialize(writer, "0.0");
//                return;
//            case The7_Enum.The10000:
//                serializer.Serialize(writer, "1000.0");
//                return;
//            case The7_Enum.The12000000000:
//                serializer.Serialize(writer, "1200000000.0");
//                return;
//            case The7_Enum.The130000:
//                serializer.Serialize(writer, "13000.0");
//                return;
//            case The7_Enum.The2100:
//                serializer.Serialize(writer, "210.0");
//                return;
//            case The7_Enum.The370000:
//                serializer.Serialize(writer, "37000.0");
//                return;
//            case The7_Enum.The400:
//                serializer.Serialize(writer, "40.0");
//                return;
//            case The7_Enum.The700000:
//                serializer.Serialize(writer, "70000.0");
//                return;
//        }
//        throw new Exception("Cannot marshal type The7_Enum");
//    }

//    public static readonly The7EnumConverter Singleton = new The7EnumConverter();
//}

//internal class EdinicaIzmerPrConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(EdinicaIzmerPR) || t == typeof(EdinicaIzmerPR?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "лет":
//                return EdinicaIzmerPR.Лет;
//            case "сут":
//                return EdinicaIzmerPR.Сут;
//            case "час":
//                return EdinicaIzmerPR.Час;
//        }
//        throw new Exception("Cannot unmarshal type EdinicaIzmerPR");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (EdinicaIzmerPR)untypedValue;
//        switch (value)
//        {
//            case EdinicaIzmerPR.Лет:
//                serializer.Serialize(writer, "лет");
//                return;
//            case EdinicaIzmerPR.Сут:
//                serializer.Serialize(writer, "сут");
//                return;
//            case EdinicaIzmerPR.Час:
//                serializer.Serialize(writer, "час");
//                return;
//        }
//        throw new Exception("Cannot marshal type EdinicaIzmerPR");
//    }

//    public static readonly EdinicaIzmerPrConverter Singleton = new EdinicaIzmerPrConverter();
//}

//internal class KodGruppyConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(KodGruppy) || t == typeof(KodGruppy?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "Трансурановый":
//                return KodGruppy.Трансурановый;
//            case "Тритий":
//                return KodGruppy.Тритий;
//            case "а":
//                return KodGruppy.А;
//            case "б":
//                return KodGruppy.Б;
//        }
//        throw new Exception("Cannot unmarshal type KodGruppy");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (KodGruppy)untypedValue;
//        switch (value)
//        {
//            case KodGruppy.Трансурановый:
//                serializer.Serialize(writer, "Трансурановый");
//                return;
//            case KodGruppy.Тритий:
//                serializer.Serialize(writer, "Тритий");
//                return;
//            case KodGruppy.А:
//                serializer.Serialize(writer, "а");
//                return;
//            case KodGruppy.Б:
//                serializer.Serialize(writer, "б");
//                return;
//        }
//        throw new Exception("Cannot marshal type KodGruppy");
//    }

//    public static readonly KodGruppyConverter Singleton = new KodGruppyConverter();
//}

//internal class OsnVidRaspadTextConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(OsnVidRaspadText) || t == typeof(OsnVidRaspadText?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "альфа":
//                return OsnVidRaspadText.Альфа;
//            case "бета":
//                return OsnVidRaspadText.Бета;
//            case "захват электрона":
//                return OsnVidRaspadText.ЗахватЭлектрона;
//            case "изомерный переход":
//                return OsnVidRaspadText.ИзомерныйПереход;
//        }
//        throw new Exception("Cannot unmarshal type OsnVidRaspadText");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (OsnVidRaspadText)untypedValue;
//        switch (value)
//        {
//            case OsnVidRaspadText.Альфа:
//                serializer.Serialize(writer, "альфа");
//                return;
//            case OsnVidRaspadText.Бета:
//                serializer.Serialize(writer, "бета");
//                return;
//            case OsnVidRaspadText.ЗахватЭлектрона:
//                serializer.Serialize(writer, "захват электрона");
//                return;
//            case OsnVidRaspadText.ИзомерныйПереход:
//                serializer.Serialize(writer, "изомерный переход");
//                return;
//        }
//        throw new Exception("Cannot marshal type OsnVidRaspadText");
//    }

//    public static readonly OsnVidRaspadTextConverter Singleton = new OsnVidRaspadTextConverter();
//}

//internal class VidIzluchConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(VidIzluch) || t == typeof(VidIzluch?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "альфа":
//                return VidIzluch.Альфа;
//            case "бета":
//                return VidIzluch.Бета;
//            case "гамма":
//                return VidIzluch.Гамма;
//        }
//        throw new Exception("Cannot unmarshal type VidIzluch");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (VidIzluch)untypedValue;
//        switch (value)
//        {
//            case VidIzluch.Альфа:
//                serializer.Serialize(writer, "альфа");
//                return;
//            case VidIzluch.Бета:
//                serializer.Serialize(writer, "бета");
//                return;
//            case VidIzluch.Гамма:
//                serializer.Serialize(writer, "гамма");
//                return;
//        }
//        throw new Exception("Cannot marshal type VidIzluch");
//    }

//    public static readonly VidIzluchConverter Singleton = new VidIzluchConverter();
//}

//internal class AConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(A) || t == typeof(A?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "":
//                return A.Empty;
//            case "Место сбора РАО":
//                return A.МестоСбораРао;
//            case "приреакторное хранилище":
//                return A.ПриреакторноеХранилище;
//        }
//        throw new Exception("Cannot unmarshal type A");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (A)untypedValue;
//        switch (value)
//        {
//            case A.Empty:
//                serializer.Serialize(writer, "");
//                return;
//            case A.МестоСбораРао:
//                serializer.Serialize(writer, "Место сбора РАО");
//                return;
//            case A.ПриреакторноеХранилище:
//                serializer.Serialize(writer, "приреакторное хранилище");
//                return;
//        }
//        throw new Exception("Cannot marshal type A");
//    }

//    public static readonly AConverter Singleton = new AConverter();
//}

//internal class VocVocTypeConverter : JsonConverter
//{
//    public override bool CanConvert(Type t) => t == typeof(VocVocType) || t == typeof(VocVocType?);

//    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
//    {
//        if (reader.TokenType == JsonToken.Null) return null;
//        var value = serializer.Deserialize<string>(reader);
//        switch (value)
//        {
//            case "112_voc":
//                return VocVocType.The112_Voc;
//            case "11_voc":
//                return VocVocType.The11_Voc;
//            case "120_voc":
//                return VocVocType.The120_Voc;
//            case "12_voc":
//                return VocVocType.The12_Voc;
//            case "13_voc":
//                return VocVocType.The13_Voc;
//            case "16_voc":
//                return VocVocType.The16_Voc;
//            case "17_voc":
//                return VocVocType.The17_Voc;
//            case "21_voc":
//                return VocVocType.The21_Voc;
//            case "23_voc":
//                return VocVocType.The23_Voc;
//            case "24_voc":
//                return VocVocType.The24_Voc;
//            case "27_voc":
//                return VocVocType.The27_Voc;
//            case "28_voc":
//                return VocVocType.The28_Voc;
//            case "FormSobst_voc":
//                return VocVocType.FormSobstVoc;
//            case "radionuclide_voc1":
//                return VocVocType.RadionuclideVoc1;
//        }
//        throw new Exception("Cannot unmarshal type VocVocType");
//    }

//    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
//    {
//        if (untypedValue == null)
//        {
//            serializer.Serialize(writer, null);
//            return;
//        }
//        var value = (VocVocType)untypedValue;
//        switch (value)
//        {
//            case VocVocType.The112_Voc:
//                serializer.Serialize(writer, "112_voc");
//                return;
//            case VocVocType.The11_Voc:
//                serializer.Serialize(writer, "11_voc");
//                return;
//            case VocVocType.The120_Voc:
//                serializer.Serialize(writer, "120_voc");
//                return;
//            case VocVocType.The12_Voc:
//                serializer.Serialize(writer, "12_voc");
//                return;
//            case VocVocType.The13_Voc:
//                serializer.Serialize(writer, "13_voc");
//                return;
//            case VocVocType.The16_Voc:
//                serializer.Serialize(writer, "16_voc");
//                return;
//            case VocVocType.The17_Voc:
//                serializer.Serialize(writer, "17_voc");
//                return;
//            case VocVocType.The21_Voc:
//                serializer.Serialize(writer, "21_voc");
//                return;
//            case VocVocType.The23_Voc:
//                serializer.Serialize(writer, "23_voc");
//                return;
//            case VocVocType.The24_Voc:
//                serializer.Serialize(writer, "24_voc");
//                return;
//            case VocVocType.The27_Voc:
//                serializer.Serialize(writer, "27_voc");
//                return;
//            case VocVocType.The28_Voc:
//                serializer.Serialize(writer, "28_voc");
//                return;
//            case VocVocType.FormSobstVoc:
//                serializer.Serialize(writer, "FormSobst_voc");
//                return;
//            case VocVocType.RadionuclideVoc1:
//                serializer.Serialize(writer, "radionuclide_voc1");
//                return;
//        }
//        throw new Exception("Cannot marshal type VocVocType");
//    }

//    public static readonly VocVocTypeConverter Singleton = new VocVocTypeConverter();
//}
//#endregion