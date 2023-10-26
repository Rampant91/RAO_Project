using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain22 : TableDataMain
{
    //"Пункт хранения", "наименование", "2"
    [JsonProperty("g2")] public string StoragePlaceName { get; set; }

    //"Пункт хранения", "код", "3"
    [JsonProperty("g3")] public string StoragePlaceCode { get; set; }

    //"УКТ, упаковка ли иная учетная единица", "наименование", "4"
    [JsonProperty("g11")] public string PackName { get; set; }

    //"УКТ, упаковка ли иная учетная единица", "тип", "5"
    [JsonProperty("g12")] public string PackType { get; set; }

    //"УКТ, упаковка ли иная учетная единица", "количество, шт", "6"
    [JsonProperty("g13")] public string PackQuantity { get; set; }

    //"null-7", "Код РАО", "7"
    [JsonProperty("g4")] public string CodeRAO { get; set; }

    //"null-8", "Статус РАО", "8"
    [JsonProperty("g5")] public string StatusRAO { get; set; }

    //"Объем, куб. м", "РАО без упаковки", "9"
    [JsonProperty("g6")] public string VolumeOutOfPack { get; set; }

    //"Объем, куб. м", "РАО с упаковкой", "10"
    [JsonProperty("g7")] public string VolumeInPack { get; set; }

    //"Масса, т", "РАО без упаковки (нетто)", "11"
    [JsonProperty("g8")] public string MassOutOfPack { get; set; }

    //"Масса, т", "РАО с упаковкой (брутто)", "12"
    [JsonProperty("g9")] public string MassInPack { get; set; }

    //"null-13", "Количество ОЗИИИ, шт", "13"
    [JsonProperty("g10")] public string QuantityOZIII { get; set; }

    //"Суммарная активность, Бк", "тритий", "14"
    [JsonProperty("g19")] public string TritiumActivity { get; set; }

    //"Суммарная активность, Бк", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "15"
    [JsonProperty("g15")] public string BetaGammaActivity { get; set; }

    //"Суммарная активность, Бк", "альфа-излучающие радионуклиды (исключая трансурановые)", "16"
    [JsonProperty("g14")] public string AlphaActivity { get; set; }

    //"Суммарная активность, Бк", "трансурановые радионуклиды", "17"
    [JsonProperty("g20")] public string TransuraniumActivity { get; set; }

    //"null-18", "Основные радионуклиды", "18"
    [JsonProperty("g16")] public string MainRadionuclids { get; set; }

    //"null-19", "Субсидия, %", "19"
    [JsonProperty("g21")] public string Subsidy { get; set; }

    //"null-20", "Номер мероприятия ФЦП", "20"
    [JsonProperty("g22")] public string FcpNumber { get; set; }
}