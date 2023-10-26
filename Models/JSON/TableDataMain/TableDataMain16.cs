using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain16 : TableDataMain
{
    //"Сведения об операции", "код", "2"
    [JsonProperty("OpCod")] public string OperationCode { get; set; }

    //"Сведения об операции", "дата", "3"
    [JsonProperty("OpDate")] public string OperationDate { get; set; }

    //"null-4","Код РАО", "4"
    [JsonProperty("RAOCod")]
    public string CodeRAO { get; set; }

    //"null-5","Статус РАО", "5"
    [JsonProperty("BCod")]
    public string StatusRAO { get; set; }

    //"Количество", "объем без упаковки, куб. м", "6"
    [JsonProperty("Kbm")]
    public string Volume { get; set; }

    //"Количество", "масса без упаковки (нетто), т", "7"
    [JsonProperty("Tonne")]
    public string Mass { get; set; }

    //"null-8","Количество ОЗИИИ, шт", "8"
    [JsonProperty("Sht")]
    public string QuantityOZIII { get; set; }

    //"null-9","Основные радионуклиды", "9"
    [JsonProperty("RAOCodMax")]
    public string MainRadionuclids { get; set; }

    //"Суммарная активность, Бк", "тритий", "10"
    [JsonProperty("ActTR")]
    public string TritiumActivity { get; set; }

    //"Суммарная активность, Бк","бета-, гамма-излучающие радионуклиды (исключая тритий)", "11"
    [JsonProperty("ActBG")]
    public string BetaGammaActivity { get; set; }

    //"Суммарная активность, Бк","альфа-излучающие радионуклиды (исключая трансурановые)", "12"
    [JsonProperty("ActA")] public string AlphaActivity { get; set; }

    //"Суммарная активность, Бк","трансурановые радионуклиды", "13"
    [JsonProperty("ActUR")] public string TransuraniumActivity { get; set; }

    //"null-14","Дата измерения активности","14"
    [JsonProperty("ActDate")] public string ActivityMeasurementDate { get; set; }

    //"Документ","вид", "15"
    [JsonProperty("DocVid")] public string DocumentVid { get; set; }

    //"Документ", "номер", "16"
    [JsonProperty("DocN")] public string DocumentNumber { get; set; }

    //"Документ", "дата", "17"
    [JsonProperty("DocDate")] public string DocumentDate { get; set; }

    //"ОКПО", "поставщика или получателя", "18"
    [JsonProperty("OkpoPIP")] public string ProviderOrRecieverOKPO { get; set; }

    //"ОКПО", "перевозчика","19"
    [JsonProperty("OkpoPrv")]
    public string TransporterOKPO { get; set; }

    //"Пункт хранения", "наименование","20"
    [JsonProperty("PH_Name")]
    public string StoragePlaceName { get; set; }

    //"Пункт хранения", "код","21"
    [JsonProperty("PH_Cod")]
    public string StoragePlaceCode { get; set; }

    //"null-22", "Код переработки / сортировки РАО", "22"
    [JsonProperty("UpCod")]
    public string RefineOrSortRAOCode { get; set; }

    //"УКТ, упаковка или иная учетная единица", "наименование", "23"
    [JsonProperty("PrName")]
    public string PackName { get; set; }

    //"УКТ, упаковка или иная учетная единица", "тип", "24"
    [JsonProperty("UktPrTyp")]
    public string PackType { get; set; }

    //"УКТ, упаковка или иная учетная единица", "номер упаковки", "25"
    [JsonProperty("UktPrN")]
    public string PackNumber { get; set; }

    //"null-26","Субсидия, %", "26"
    [JsonProperty("Subsid")]
    public string Subsidy { get; set; }

    //"null-27","Номер мероприятия ФЦП", "27"
    [JsonProperty("FCP")]
    public string FcpNumber { get; set; }
}