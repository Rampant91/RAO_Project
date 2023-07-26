using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain17
{
    //"Сведения об операции", "код", "2"
    [JsonProperty("g2")] public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("g3")] public string OperationDate { get; set; }

    //"Сведения об упаковке", "null-4", "наименование", "4"
    [JsonProperty("g4")]
    public string PackName { get; set; }

    //"Сведения об упаковке", "null-5", "тип", "5"
    [JsonProperty("g5")]
    public string PackType { get; set; }

    //"Сведения об упаковке", "null-6", "заводской номер", "6"
    [JsonProperty("g6")]
    public string PackFactoryNumber { get; set; }

    //"Сведения об упаковке", "null-7", "номер упаковки (идентификационный код)", "7"
    [JsonProperty("g7")]
    public string PackNumber { get; set; }

    //"Сведения об упаковке", "null-8", "дата формирования", "8"
    [JsonProperty("g8")]
    public string FormingDate { get; set; }

    //"Сведения об упаковке", "null-9", "номер паспорта", "9"
    [JsonProperty("g9")]
    public string PassportNumber { get; set; }

    //"Сведения об упаковке", "null-10", "объем, куб. м", "10"
    [JsonProperty("g10")]
    public string Volume { get; set; }

    //"Сведения об упаковке", "null-11", "масса брутто, т", "11"
    [JsonProperty("g11")]
    public string Mass { get; set; }

    //"Сведения об упаковке", "Радионуклидный состав", "наименование радионуклида", "12"
    [JsonProperty("g12")] public string Radionuclids { get; set; }

    //"Сведения об упаковке", "Радионуклидный состав", "удельная активность, Бк/г", "13"
    [JsonProperty("g13")] public string SpecificActivity { get; set; }

    //"Документ","вид", "14"
    [JsonProperty("g14")] public string DocumentVid { get; set; }

    //"Документ", "номер", "15"
    [JsonProperty("g15")] public string DocumentNumber { get; set; }

    //"Документ", "дата", "16"
    [JsonProperty("g16")] public string DocumentDate { get; set; }

    //"null-17-18", "ОКПО", "поставщика или получателя", "17"
    [JsonProperty("g17")] public string ProviderOrRecieverOKPO { get; set; }

    //"null-17-18", "ОКПО", "перевозчика", "18"
    [JsonProperty("g18")] public string TransporterOKPO { get; set; }

    //"null-19-20", "Пункт хранения", "наименование", "19"
    [JsonProperty("g19")]
    public string StoragePlaceName { get; set; }

    //"null-19-20", "Пункт хранения", "код", "20"
    [JsonProperty("g20")]
    public string StoragePlaceCode { get; set; }

    //"Сведения о РАО", "null-21", "код", "21"
    [JsonProperty("g21")]
    public string CodeRAO { get; set; }

    //"Сведения о РАО", "null-22", "статус", "22"
    [JsonProperty("g22")]
    public string StatusRAO { get; set; }

    //"Сведения о РАО", "null-23", "объем без упаковки, куб. м", "23"
    [JsonProperty("g23")]
    public string VolumeOutOfPack { get; set; }

    //"Сведения о РАО", "null-24", "масса без упаковки (нетто), т", "24"
    [JsonProperty("g24")]
    public string MassOutOfPack { get; set; }

    //"Сведения о РАО", "null-25", "количество ОЗИИИ, шт", "25"
    [JsonProperty("g25")]
    public string Quantity { get; set; }

    //"Сведения о РАО", "Суммарная активность", "тритий", "26"
    [JsonProperty("g26")]
    public string TritiumActivity { get; set; }

    //"Сведения о РАО", "Суммарная активность", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "27"
    [JsonProperty("g27")]
    public string BetaGammaActivity { get; set; }

    //"Сведения о РАО", "Суммарная активность", "альфа-излучающие радионуклиды (исключая трансурановые)", "28"
    [JsonProperty("g28")]
    public string AlphaActivity { get; set; }

    //"Сведения о РАО", "Суммарная активность", "трансурановые радионуклиды", "29"
    [JsonProperty("g29")]
    public string TransuraniumActivity { get; set; }

    //"Сведения о РАО", "null-30", "Код переработки/сортировки РАО", "30"
    [JsonProperty("g30")]
    public string RefineOrSortRAOCode { get; set; }

    //"null-31-1", "null-31-2", "Субсидия, %", "31"
    [JsonProperty("g31")]
    public string Subsidy { get; set; }

    //"null-32_1", "null-32_2", "Номер мероприятия ФЦП", "32"
    [JsonProperty("g32")]
    public string FcpNumber { get; set; }
}