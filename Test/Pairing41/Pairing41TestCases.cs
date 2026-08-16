using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.Testing;

namespace Test.Pairing41;

/// <summary>
/// Наборы тестовых сценариев парности операций 41 (перевод РВ → РАО).
/// <para>
/// Группы:
/// <list type="bullet">
/// <item><b>V</b> — smoke, все пары сходятся;</item>
/// <item><b>A</b> — 1.1 ↔ 1.5 (ЗРИ);</item>
/// <item><b>B</b> — 1.2 ↔ 1.6 (ИОУ);</item>
/// <item><b>C</b> — 1.3 ↔ 1.6 (ОРИ-изделия);</item>
/// <item><b>D</b> — 1.4 ↔ 1.6 (ОРИ-прочие);</item>
/// <item><b>F</b> — сторона 1.6 (общий пул 1.6, приоритет 12→13→14, симметрия);</item>
/// <item><b>P</b> — опции диалога (Check* = false снимает поле из ключа);</item>
/// <item><b>H</b> — карты closest-match для непарных;</item>
/// <item><b>S</b> — soft closest: уровни Near, выбор кандидата, tie-break 1.6.</item>
/// </list>
/// </para>
/// <para>
/// Строки в сценариях уже в единицах сравнения (как после LoadForm*):
/// масса 1.2/1.4 — в тоннах; активности 1.2/1.3/1.4 — посчитаны;
/// у 1.2 дата измерения активности = дата операции; у 1.3 = дата выпуска (creation).
/// </para>
/// <para>
/// Типичные даты хелперов: <see cref="Form11OpDate"/> (1.1/1.5),
/// <see cref="Form12OpDate"/> (1.2), <see cref="Form13OpDate"/> (1.3),
/// <see cref="Form14OpDate"/> (1.4). Id строк вымышленные (1 / 101, 2 / 202, …).
/// Код РАО сравнивается при включённом CheckCodeRao (по умолчанию — true у всех
/// хелперов Row12/Row13/Row14/Row16From12/Row16From13/Row16From14 он пустой,
/// поэтому существующие сценарии остаются парными, пока явно не задан другой).
/// Для 1.1↔1.5 пустые паспорт+зав.№ и заглушки («б.н.», «без номера», «-»)
/// идут в ветку суммирования количества.
/// </para>
/// </summary>
internal static partial class Pairing41TestCases
{
    /// <summary>Дата операции по умолчанию для строк 1.1 / 1.5.</summary>
    private const string Form11OpDate = "2024-01-15";

    /// <summary>Дата операции (и AMD) по умолчанию для строк 1.2 / парной 1.6.</summary>
    private const string Form12OpDate = "2024-02-01";

    /// <summary>Дата операции по умолчанию для строк 1.3.</summary>
    private const string Form13OpDate = "2024-03-01";

    /// <summary>Дата выпуска / AMD по умолчанию для строк 1.3.</summary>
    private const string Form13CreationDate = "2023-06-01";

    /// <summary>Дата операции по умолчанию для строк 1.4.</summary>
    private const string Form14OpDate = "2024-04-01";

    public static IEnumerable<object[]> All() =>
        ValidCases()
            .Concat(Form11To15Cases())
            .Concat(Form12To16Cases())
            .Concat(Form13To16Cases())
            .Concat(Form14To16Cases())
            .Concat(Form16Cases())
            .Concat(ParamsCases())
            .Concat(ClosestMatchCases())
            .Concat(SoftClosestCases())
            .Select(testCase => new object[] { testCase.Name, testCase });

    public static IEnumerable<object[]> ClosestMatchOnly() =>
        ClosestMatchCases()
            .Concat(SoftClosestCases())
            .Where(testCase => testCase.ExpectedClosest11 is not null
                               || testCase.ExpectedClosest15 is not null
                               || testCase.ExpectedClosest12 is not null
                               || testCase.ExpectedClosest13 is not null
                               || testCase.ExpectedClosest14 is not null
                               || testCase.ExpectedClosest16 is not null
                               || testCase.ExpectedClosest11Levels is not null
                               || testCase.ExpectedClosest15Levels is not null
                               || testCase.ExpectedClosest12Levels is not null
                               || testCase.ExpectedClosest13Levels is not null
                               || testCase.ExpectedClosest14Levels is not null
                               || testCase.ExpectedClosestCandidate11 is not null
                               || testCase.ExpectedClosestCandidate15 is not null
                               || testCase.ExpectedClosestCandidate12 is not null
                               || testCase.ExpectedClosestCandidate13 is not null
                               || testCase.ExpectedClosestCandidate14 is not null
                               || testCase.ExpectedClosestCandidate16 is not null
                               || testCase.ExpectedAggregateStateMatch13 is not null
                               || testCase.ExpectedAggregateStateMatch14 is not null
                               || testCase.ExpectedConfidenceMinPercent11 is not null
                               || testCase.ExpectedConfidenceMinPercent15 is not null
                               || testCase.ExpectedConfidenceMinPercent12 is not null
                               || testCase.ExpectedConfidenceMinPercent13 is not null
                               || testCase.ExpectedConfidenceMinPercent14 is not null
                               || testCase.ExpectedConfidenceMinPercent16 is not null)
            .Select(testCase => new object[] { testCase.Name, testCase });

    /// <summary>Базовая строка 1.1/1.5 с серийными номерами.</summary>
    private static Pairing41Row Row11(
        int id,
        string pasNum = "P-100",
        string facNum = "F-100",
        string type = "Тип-А",
        string radionuclids = "кобальт-60",
        string activity = "1.0e+06",
        int? quantity = 1,
        string opDate = Form11OpDate,
        string creationDate = "2020-01-01",
        string documentNumber = "DOC-1",
        string packNumber = "УКТ-1",
        string opCode = "41") =>
        new()
        {
            Id = id,
            OpCode = opCode,
            OpDate = opDate,
            PasNum = pasNum,
            FacNum = facNum,
            Type = type,
            Radionuclids = radionuclids,
            CreationDate = creationDate,
            DocumentVid = 1,
            DocumentNumber = documentNumber,
            DocumentDate = "2024-01-10",
            ProviderOrRecieverOkpo = "12345678",
            TransporterOkpo = "87654321",
            PackName = "Упаковка",
            PackType = "ТипУКТ",
            PackNumber = packNumber,
            Activity = activity,
            Quantity = quantity
        };

    /// <summary>
    /// Строка 1.2 уже в единицах сопоставления (масса в тоннах, активности посчитаны),
    /// как после LoadForm12OperationsAsync.
    /// </summary>
    internal static Pairing41Row CreateRow12ForScale(int id, string massTon) =>
        Row12(id, massTon: massTon);

    internal static Pairing41Row CreateRow16From12ForScale(int id, string massTon) =>
        Row16From12(id, massTon: massTon);

    private static Pairing41Row Row12(
        int id,
        string massTon = "1",
        string beta = "2.5e+10",
        string alpha = "1.61e+10",
        string opDate = Form12OpDate,
        string documentNumber = "DOC-12",
        string documentDate = Form12OpDate,
        string packNumber = "УКТ-12",
        string? activityMeasurementDate = null,
        string codeRao = "") =>
        new()
        {
            Id = id,
            OpDate = opDate,
            Mass = massTon,
            BetaGammaActivity = beta,
            AlphaActivity = alpha,
            ActivityMeasurementDate = activityMeasurementDate ?? opDate,
            DocumentVid = 1,
            DocumentNumber = documentNumber,
            DocumentDate = documentDate,
            PackName = "Упаковка",
            PackType = "ТипУКТ",
            PackNumber = packNumber,
            CodeRao = codeRao
        };

    private static Pairing41Row Row16From12(
        int id,
        string massTon = "1",
        string beta = "2.5e+10",
        string alpha = "1.61e+10",
        string opDate = Form12OpDate,
        string documentNumber = "DOC-12",
        string documentDate = Form12OpDate,
        string packNumber = "УКТ-12",
        string? activityMeasurementDate = null,
        string codeRao = "") =>
        new()
        {
            Id = id,
            OpDate = opDate,
            Mass = massTon,
            BetaGammaActivity = beta,
            AlphaActivity = alpha,
            TritiumActivity = "-",
            TransuraniumActivity = "-",
            ActivityMeasurementDate = activityMeasurementDate ?? opDate,
            DocumentVid = 1,
            DocumentNumber = documentNumber,
            DocumentDate = documentDate,
            PackName = "Упаковка",
            PackType = "ТипУКТ",
            PackNumber = packNumber,
            MainRadionuclids = "уран-238; торий-234; протактиний-234м; уран-234",
            CodeRao = codeRao
        };

    /// <summary>Строка 1.3 после расчёта активностей по типу нуклида (как LoadForm13).</summary>
    private static Pairing41Row Row13(
        int id,
        string mainRads = "кобальт-60",
        string beta = "1.0e+06",
        string alpha = "-",
        string tritium = "-",
        string transuranium = "-",
        string type = "Тип-ОРИ",
        string opDate = Form13OpDate,
        string creationDate = Form13CreationDate,
        string documentNumber = "DOC-13",
        string documentDate = Form13OpDate,
        string packNumber = "УКТ-13",
        string codeRao = "",
        byte? aggregateState = null) =>
        new()
        {
            Id = id,
            OpDate = opDate,
            Type = type,
            MainRadionuclids = mainRads,
            Radionuclids = mainRads,
            TritiumActivity = tritium,
            BetaGammaActivity = beta,
            AlphaActivity = alpha,
            TransuraniumActivity = transuranium,
            ActivityMeasurementDate = creationDate,
            CreationDate = creationDate,
            DocumentVid = 1,
            DocumentNumber = documentNumber,
            DocumentDate = documentDate,
            PackName = "Упаковка",
            PackType = "ТипУКТ",
            PackNumber = packNumber,
            CodeRao = codeRao,
            AggregateState = aggregateState
        };

    private static Pairing41Row Row16From13(
        int id,
        string mainRads = "кобальт-60",
        string beta = "1.0e+06",
        string alpha = "-",
        string tritium = "-",
        string transuranium = "-",
        string opDate = Form13OpDate,
        string activityMeasurementDate = Form13CreationDate,
        string documentNumber = "DOC-13",
        string documentDate = Form13OpDate,
        string packNumber = "УКТ-13",
        string codeRao = "") =>
        new()
        {
            Id = id,
            OpDate = opDate,
            MainRadionuclids = mainRads,
            TritiumActivity = tritium,
            BetaGammaActivity = beta,
            AlphaActivity = alpha,
            TransuraniumActivity = transuranium,
            ActivityMeasurementDate = activityMeasurementDate,
            DocumentVid = 1,
            DocumentNumber = documentNumber,
            DocumentDate = documentDate,
            PackName = "Упаковка",
            PackType = "ТипУКТ",
            PackNumber = packNumber,
            CodeRao = codeRao
        };

    /// <summary>Строка 1.4 после ToMassTon и расчёта активностей (как LoadForm14).</summary>
    private static Pairing41Row Row14(
        int id,
        string volume = "2",
        string massTon = "0.5",
        string mainRads = "цезий-137",
        string beta = "5.0e+05",
        string alpha = "-",
        string tritium = "-",
        string transuranium = "-",
        string opDate = Form14OpDate,
        string activityMeasurementDate = "2024-03-15",
        string documentNumber = "DOC-14",
        string documentDate = Form14OpDate,
        string packNumber = "УКТ-14",
        string codeRao = "",
        byte? aggregateState = null) =>
        new()
        {
            Id = id,
            OpDate = opDate,
            Volume = volume,
            Mass = massTon,
            MainRadionuclids = mainRads,
            Radionuclids = mainRads,
            TritiumActivity = tritium,
            BetaGammaActivity = beta,
            AlphaActivity = alpha,
            TransuraniumActivity = transuranium,
            ActivityMeasurementDate = activityMeasurementDate,
            DocumentVid = 1,
            DocumentNumber = documentNumber,
            DocumentDate = documentDate,
            PackName = "Упаковка",
            PackType = "ТипУКТ",
            PackNumber = packNumber,
            CodeRao = codeRao,
            AggregateState = aggregateState
        };

    private static Pairing41Row Row16From14(
        int id,
        string volume = "2",
        string massTon = "0.5",
        string mainRads = "цезий-137",
        string beta = "5.0e+05",
        string alpha = "-",
        string tritium = "-",
        string transuranium = "-",
        string opDate = Form14OpDate,
        string activityMeasurementDate = "2024-03-15",
        string documentNumber = "DOC-14",
        string packNumber = "УКТ-14",
        string codeRao = "") =>
        new()
        {
            Id = id,
            OpDate = opDate,
            Volume = volume,
            Mass = massTon,
            MainRadionuclids = mainRads,
            TritiumActivity = tritium,
            BetaGammaActivity = beta,
            AlphaActivity = alpha,
            TransuraniumActivity = transuranium,
            ActivityMeasurementDate = activityMeasurementDate,
            DocumentVid = 1,
            DocumentNumber = documentNumber,
            DocumentDate = Form14OpDate,
            PackName = "Упаковка",
            PackType = "ТипУКТ",
            PackNumber = packNumber,
            CodeRao = codeRao
        };
}
