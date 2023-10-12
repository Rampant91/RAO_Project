using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain21
{
    //"Установки переработки", "наименование", "2"
    [JsonProperty("g2")] public string RefineMachineName { get; set; }

    //"Установки переработки", "код", "3"
    [JsonProperty("g3")] public string MachineCode { get; set; }

    //"Установки переработки", "мощность, куб. м/год", "4"
    [JsonProperty("g4")] public string MachinePower { get; set; }

    //"Установки переработки", "количество часов работы за год", "5"
    [JsonProperty("g5")] public string NumberOfHoursPerYear { get; set; }

    //"Поступило РАО на переработку, кондиционирование", "код РАО", "6"
    [JsonProperty("g6")] public string CodeRAOIn { get; set; }

    //"Поступило РАО на переработку, кондиционирование", "статус РАО", "7"
    [JsonProperty("g7")] public string StatusRAOIn { get; set; }

    //"Поступило РАО на переработку, кондиционирование", "куб. м", "8"
    [JsonProperty("g8")] public string VolumeIn { get; set; }

    //"Поступило РАО на переработку, кондиционирование", "т", "9"
    [JsonProperty("g9")] public string MassIn { get; set; }

    //"Поступило РАО на переработку, кондиционирование", "ОЗИИИ, шт", "10"
    [JsonProperty("g10")] public string QuantityIn { get; set; }

    //"Поступило РАО на переработку, кондиционирование", "тритий", "11"
    [JsonProperty("g11")] public string TritiumActivityIn { get; set; }

    //"Поступило РАО на переработку, кондиционирование", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "12"
    [JsonProperty("g12")] public string BetaGammaActivityIn { get; set; }

    //"Поступило РАО на переработку, кондиционирование", "альфа-излучающие радионуклиды (исключая трансурановые)", "13"
    [JsonProperty("g13")] public string AlphaActivityIn { get; set; }

    //"Поступило РАО на переработку, кондиционирование", "трансурановые радионуклиды", "14"
    [JsonProperty("g14")] public string TransuraniumActivityIn { get; set; }

    //"Образовалось РАО после переработки, кондиционирования", "код РАО", "15"
    [JsonProperty("g15")] public string CodeRAOout { get; set; }

    //"Образовалось РАО после переработки, кондиционирования", "статус РАО", "16"
    [JsonProperty("g16")] public string StatusRAOout { get; set; }

    //"Образовалось РАО после переработки, кондиционирования", "куб. м", "17"
    [JsonProperty("g17")] public string VolumeOut { get; set; }

    //"Образовалось РАО после переработки, кондиционирования", "т", "18"
    [JsonProperty("g18")] public string MassOut { get; set; }

    //"Образовалось РАО после переработки, кондиционирования", "ОЗИИИ, шт", "19"
    [JsonProperty("g19")] public string QuantityOZIIIout { get; set; }

    //"Образовалось РАО после переработки, кондиционирования", "тритий", "20"
    [JsonProperty("g20")] public string TritiumActivityOut { get; set; }

    //"Образовалось РАО после переработки, кондиционирования", "бета-, гамма-излучающие радионуклиды (исключая тритий)", "21"
    [JsonProperty("g21")] public string BetaGammaActivityOut { get; set; }

    //"Образовалось РАО после переработки, кондиционирования", "альфа-излучающие радионуклиды (исключая трансурановые)", "22"
    [JsonProperty("g22")] public string AlphaActivityOut { get; set; }

    //"Образовалось РАО после переработки, кондиционирования", "трансурановые радионуклиды", "23"
    [JsonProperty("g23")] public string TransuraniumActivityOut { get; set; }
}