using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain18
{
    //"Сведения об операции", "код", "2"
    [JsonProperty("g2")] public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("g3")] public string OperationDate { get; set; }

    //"Сведения о партии ЖРО", "индивидуальный номер (идентификационный код) партии ЖРО", "4"
    [JsonProperty("g4")]
    public string IndividualNumberZHRO { get; set; }

    //"Сведения о партии ЖРО", "номер паспорта", "5"
    [JsonProperty("g5")]
    public string PassportNumber { get; set; }

    //"Сведения о партии ЖРО", "объем, куб. м", "6"
    [JsonProperty("g6")]
    public string Volume6 { get; set; }

    //"Сведения о партии ЖРО", "масса, т", "7"
    [JsonProperty("g7")]
    public string Mass7 { get; set; }

    //"Сведения о партии ЖРО", "солесодержание, г/л", "8"
    [JsonProperty("g8")]
    public string SaltConcentration { get; set; }

    //"Сведения о партии ЖРО", "наименование радионуклида", "9"
    [JsonProperty("g9")]
    public string Radionuclids { get; set; }

    //"Сведения о партии ЖРО", "удельная активность, Бк/г", "10"
    [JsonProperty("g10")]
    public string SpecificActivity { get; set; }

    //"Документ","вид", "11"
    [JsonProperty("g11")] public string DocumentVid { get; set; }

    //"Документ", "номер", "12"
    [JsonProperty("g12")] public string DocumentNumber { get; set; }

    //"Документ", "дата", "13"
    [JsonProperty("g13")] public string DocumentDate { get; set; }

    //"ОКПО", "поставщика или получателя", "14"
    [JsonProperty("g14")] public string ProviderOrRecieverOKPO { get; set; }

    //"ОКПО", "перевозчика", "15"
    [JsonProperty("g15")] public string TransporterOKPO { get; set; }

    //"Пункт хранения", "наименование", "16"
    [JsonProperty("g16")] public string StoragePlaceName { get; set; }

    //"Пункт хранения", "код", "17"
    [JsonProperty("g17")] public string StoragePlaceCode { get; set; }

    //"Характеристика ЖРО", "код", "18"
    [JsonProperty("g18")] public string CodeRAO { get; set; }

    //"Характеристика ЖРО", "статус", "19"
    [JsonProperty("g19")]
    public string StatusRAO { get; set; }

    //"Характеристика ЖРО", "объем, куб. м", "20"
    [JsonProperty("g20")]
    public string Volume20 { get; set; }

    //"Характеристика ЖРО", "масса, т", "21"
    [JsonProperty("g21")]
    public string Mass21 { get; set; }

    //"Характеристика ЖРО", "тритий", "22"
    [JsonProperty("g22")]
    public string TritiumActivity { get; set; }

    //"Характеристика ЖРО", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "23"
    [JsonProperty("g23")]
    public string BetaGammaActivity { get; set; }

    //"Характеристика ЖРО", "альфа-излучающие радионуклиды (исключая трансурановые)", "24"
    [JsonProperty("g24")]
    public string AlphaActivity { get; set; }

    //"Характеристика ЖРО", "трансурановые радионуклиды", "25"
    [JsonProperty("g25")]
    public string TransuraniumActivity { get; set; }

    //"Характеристика ЖРО", "Код переработки / сортировки РАО", "26"
    [JsonProperty("g26")]
    public string RefineOrSortRAOCode { get; set; }

    //"null-27", "Субсидия, %", "27"
    [JsonProperty("g27")]
    public string Subsidy { get; set; }

    //"null-28", "Номер мероприятия ФЦП", "28"
    [JsonProperty("g28")]
    public string FcpNumber { get; set; }
}