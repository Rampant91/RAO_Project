using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace Client_App.Resources;

/// <summary>
/// Общая логика вычисления кода РАО (форма 1.6), используемая при переносе РВ → РАО
/// (<see cref="Client_App.Commands.AsyncCommands.SourceTransmission.SourceTransmissionBaseAsyncCommand"/>)
/// и при сопоставлении операций 41 (ExcelExportCheckPairingOfCode41AsyncCommand).
/// </summary>
public static class RaoCodeHelper
{
    /// <summary>Код РАО для формы 1.2 (изделия из природного урана) — всегда один и тот же.</summary>
    public const string Form12CodeRao = "22511300522";

    /// <summary>Заголовок колонки с полным кодом РАО из формы 1.6.</summary>
    public const string FullCodeRaoColumnHeader = "Код РАО";

    /// <summary>Заголовок колонки с рассчитанным по данным 1.2/1.3/1.4 шаблоном кода РАО для 1.6.</summary>
    public const string CalculatedCodeRaoColumnHeader = "Рассчётный код РАО в 1.6";

    private static List<Dictionary<string, string>> R = [];

    #region R.xlsx

    /// <summary>Загружает справочник радионуклидов R.xlsx (однократно, кешируется в процессе).</summary>
    public static void EnsureRLoaded()
    {
        if (R.Count != 0) return;

        string filePath;
#if DEBUG
        filePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx");
#else
        filePath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", "R.xlsx");
#endif
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(filePath)) return;

        FileInfo excelImportFile = new(filePath);
        var xls = new ExcelPackage(excelImportFile);
        var worksheet = xls.Workbook.Worksheets["Лист1"];
        if (worksheet is null) return;

        var i = 2;
        var loaded = new List<Dictionary<string, string>>();
        while (worksheet.Cells[i, 1].Text != string.Empty)
        {
            loaded.Add(new Dictionary<string, string>
            {
                { "name", worksheet.Cells[i, 1].Text },
                { "periodValue", worksheet.Cells[i, 5].Text },
                { "periodUnit", worksheet.Cells[i, 6].Text },
                { "code", worksheet.Cells[i, 8].Text }
            });
            i++;
        }

        R = loaded;
    }

    #endregion

    #region ComputeCodeRao

    /// <summary>Код РАО для строки формы 1.3 (агрегатное состояние + радионуклиды, девятый-десятый символ «84»).</summary>
    public static string ComputeCodeRaoFromForm13(string? radionuclids, byte? aggregateState) =>
        ComputeCodeRao(radionuclids, aggregateState, ninthTenthSymbols: "84");

    /// <summary>Код РАО для строки формы 1.4 (агрегатное состояние + радионуклиды, девятый-десятый символ «__»).</summary>
    public static string ComputeCodeRaoFromForm14(string? radionuclids, byte? aggregateState) =>
        ComputeCodeRao(radionuclids, aggregateState, ninthTenthSymbols: "__");

    private static string ComputeCodeRao(string? radionuclids, byte? aggregateState, string ninthTenthSymbols)
    {
        EnsureRLoaded();

        var nuclidsArray = (radionuclids ?? string.Empty)
            .Replace(" ", string.Empty)
            .ToLower()
            .Replace(',', ';')
            .Split(';');

        var nuclidTypeArray = R
            .Where(x => nuclidsArray.Contains(x["name"]))
            .Select(x => x["code"])
            .ToArray();

        var thirdSymbolCodeRao = GetThirdSymbolCodeRao(nuclidTypeArray);
        var fifthSymbolCodeRao = GetFifthSymbolCodeRao(nuclidsArray);
        var agrState = aggregateState != null ? aggregateState.ToString()![..1] : "";

        return $"{agrState}_{thirdSymbolCodeRao}1{fifthSymbolCodeRao}_00{ninthTenthSymbols}_";
    }

    /// <summary>Шаблон кода РАО для строки 1.2/1.3/1.4 — как после Load (пустой CodeRao пересчитывается).</summary>
    public static string GetCalculatedCodeRaoTemplate(
        string formNum,
        string? codeRao,
        string? radionuclids,
        string? mainRadionuclids,
        byte? aggregateState) =>
        formNum switch
        {
            "1.2" => string.IsNullOrEmpty(codeRao) ? Form12CodeRao : codeRao,
            "1.3" => string.IsNullOrEmpty(codeRao)
                ? ComputeCodeRaoFromForm13(radionuclids ?? mainRadionuclids, aggregateState)
                : codeRao,
            "1.4" => string.IsNullOrEmpty(codeRao)
                ? ComputeCodeRaoFromForm14(radionuclids ?? mainRadionuclids, aggregateState)
                : codeRao,
            _ => codeRao ?? string.Empty
        };

    /// <summary>Сравнение шаблона 1.2/1.3/1.4 с полным кодом РАО строки 1.6.</summary>
    public static bool CodeRaoPairingMatches(
        string rvFormNum,
        string? rvCodeRao,
        string? rvRadionuclids,
        string? rvMainRadionuclids,
        byte? rvAggregateState,
        string? form16CodeRao) =>
        CalculatedCodeRaoMatchesFull(
            GetCalculatedCodeRaoTemplate(rvFormNum, rvCodeRao, rvRadionuclids, rvMainRadionuclids, rvAggregateState),
            form16CodeRao ?? string.Empty);

    /// <summary>Для тестовых строк 1.6 без CodeRao: заполнить «_» нулями, чтобы получить полный код из шаблона.</summary>
    public static string ExpandCalculatedTemplateToFull(string calculatedTemplate) =>
        calculatedTemplate.Replace('_', '0');

    private static string GetThirdSymbolCodeRao(string[] nuclidTypeArray)
    {
        var thirdSymbolCodeRao = "0";
        if (nuclidTypeArray.Contains("а")
            && (nuclidTypeArray.Contains("б") || nuclidTypeArray.Contains("т"))
            && nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "6";
        }
        else if (nuclidTypeArray.Contains("а")
                 && (nuclidTypeArray.Contains("б") || nuclidTypeArray.Contains("т"))
                 && !nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "5";
        }
        else if (!nuclidTypeArray.Contains("а")
                 && (nuclidTypeArray.Contains("б") || nuclidTypeArray.Contains("т"))
                 && !nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "4";
        }
        else if (nuclidTypeArray.Contains("а")
                 && !nuclidTypeArray.Contains("б") && !nuclidTypeArray.Contains("т")
                 && nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "3";
        }
        else if (nuclidTypeArray.Contains("а")
                 && !nuclidTypeArray.Contains("б") && !nuclidTypeArray.Contains("т")
                 && !nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "2";
        }
        else if (!nuclidTypeArray.Contains("а")
                 && !nuclidTypeArray.Contains("б") && !nuclidTypeArray.Contains("т")
                 && nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "1";
        }
        return thirdSymbolCodeRao;
    }

    private static string GetFifthSymbolCodeRao(IEnumerable<string> nuclidsArray)
    {
        double maxPeriod = 0;
        foreach (var nuclidName in nuclidsArray)
        {
            var nuclidDictionary = R.FirstOrDefault(x => x["name"] == nuclidName);
            if (nuclidDictionary == null) continue;

            var unit = nuclidDictionary["periodUnit"];
            var periodValue = nuclidDictionary["periodValue"].Replace('.', ',');
            if (!double.TryParse(periodValue,
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                    new CultureInfo("ru-RU", useUserOverride: false),
                    out var halfLife
                )) continue;
            switch (unit)
            {
                case "лет":
                    break;
                case "сут":
                    halfLife /= 365;
                    break;
                case "час":
                    halfLife /= 8760;   //365*24
                    break;
                case "мин":
                    halfLife /= 525_600;   //365*24*60
                    break;
                default: continue;
            }
            if (halfLife > maxPeriod)
            {
                maxPeriod = halfLife;
            }
        }
        return maxPeriod > 31
            ? "1"
            : "2";
    }

    #endregion

    #region Partial match (шаблон ↔ полный код 1.6)

    /// <summary>
    /// Совпадают ли все рассчитанные символы шаблона (не «_») с полным кодом РАО 1.6 на тех же позициях.
    /// </summary>
    public static bool CalculatedCodeRaoMatchesFull(string? calculatedTemplate, string? fullCode)
    {
        if (string.IsNullOrEmpty(calculatedTemplate) || string.IsNullOrEmpty(fullCode))
        {
            return false;
        }

        for (var i = 0; i < calculatedTemplate.Length; i++)
        {
            var templateChar = calculatedTemplate[i];
            if (templateChar == '_')
            {
                continue;
            }

            if (i >= fullCode.Length || fullCode[i] != templateChar)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region AggregateState ↔ CodeRao

    /// <summary>Первый символ кода РАО (цифра агрегатного состояния) или пустая строка.</summary>
    public static string GetAggregateStateDigitFromCodeRao(string? codeRao) =>
        string.IsNullOrEmpty(codeRao) ? string.Empty : codeRao[..1];

    /// <summary>Совпадает ли цифра агрегатного состояния с первым символом кода РАО.</summary>
    public static bool AggregateStateMatchesCodeRao(byte? aggregateState, string? codeRao)
    {
        if (aggregateState is null)
        {
            return false;
        }

        var digit = GetAggregateStateDigitFromCodeRao(codeRao);
        return digit.Length > 0 && digit == aggregateState.Value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
}
