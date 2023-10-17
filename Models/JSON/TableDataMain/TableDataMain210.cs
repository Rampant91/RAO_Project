using Newtonsoft.Json;

namespace Models.JSON.TableDataMain;

public class TableDataMain210
{
    //"null-2", "Наименование показателя", "2"
    [JsonProperty("g2")] public string IndicatorName { get; set; }

    //"null-3", "Наименование участка", "3"
    [JsonProperty("g3")] public string PlotName { get; set; }

    //"null-4", "Кадастровый номер участка", "4"
    [JsonProperty("g4")] public string PlotKadastrNumber { get; set; }

    //"null-5", "Код участка", "5"
    [JsonProperty("g5")] public string PlotCode { get; set; }

    //"null-6", "Площадь загрязненной территории, кв. м", "6"
    [JsonProperty("g6")] public string InfectedArea { get; set; }

    //"Мощность дозы гамма-излучения, мкЗв/час", "средняя", "7"
    [JsonProperty("g7")] public string AvgGammaRaysDosePower { get; set; }

    //"Мощность дозы гамма-излучения, мкЗв/час", "максимальная", "8"
    [JsonProperty("g8")] public string MaxGammaRaysDosePower { get; set; }

    //"Плотность загрязнения (средняя), Бк/кв.м", "альфа-излучающие радионуклиды", "9"
    [JsonProperty("g9")] public string WasteDensityAlpha { get; set; }

    //"Плотность загрязнения (средняя), Бк/кв.м", "бета-излучающие радионуклиды", "10"
    [JsonProperty("g10")] public string WasteDensityBeta { get; set; }

    //"null-7", "Номер мероприятия ФЦП", "11"
    [JsonProperty("g11")] public string FcpNumber { get; set; }
}