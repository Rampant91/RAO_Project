﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Models.CheckForm;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.CheckForm;

public abstract class CheckBase : BaseAsyncCommand
{
    #region Properties
    
    protected static bool checkNumPrint = false;

    private protected static List<Dictionary<string, string>> OKSM = new();

    private protected static List<Dictionary<string, string>> R = new();

    private protected static List<Dictionary<string, string>> Packs = new();

    private protected static List<string> IOU11 = new();

    private protected static List<string> Orgs18 = new();

    private protected static readonly bool DB_Ignore = true;

    private protected static readonly bool ZRI_Ignore = true;

    private protected static readonly Regex OkpoRegex = new(@"^\d{8}([\d_][Мм\d]\d{4})?$");

    private protected static readonly string[] EquilibriumRadionuclids =
    {
        "германий-68, галлий-68",
        "рубидий-83, криптон-83м",
        "стронций-82, рубидий-82",
        "стронций-90, иттрий-90",
        "иттрий-87, стронций-87м",
        "цирконий-93, ниобий-93м",
        "цирконий-97, ниобий-97",
        "рутений-106, родий-106",
        "серебро-108м, серебро-108",
        "олово-121м, олово-121",
        "олово-126, олово-126м",
        "ксенон-122, иод-122",
        "цезий-137, барий-137м",
        "барий-140, лантан-140",
        "церий-134, лантан-134",
        "церий-144, празеодим-144",
        "гадолиний-146, европий-146",
        "гафний-172, лютеций-172",
        "вольфрам-178, тантал-178",
        "вольфрам-188, рений-188",
        "рений-189, осмий-189м",
        "иридий-189, осмий-189м",
        "платина-188, иридий-188",
        "ртуть-194, золото-194",
        "ртуть-195м, ртуть-195",
        "свинец-210, висмут-210, полоний-210",
        "свинец-212, висмут-212, титан-208, полоний-212",
        "висмут-210м, титан-206",
        "висмут-212, титан-208, полоний-212",
        "радон-220, полоний-216",
        "радон-222, полоний-218, свинец-214, висмут-214, полоний-214",
        "радий-223, радон-219, полоний-215, свинец-211, висмут-214, титан-207",
        "радий-224, радон-220, полоний-216, свинец-212, висмут-212, титан-208, полоний-212",
        "радий-226, радон-222, полоний-218, свинец-214, висмут-214, полоний-214, свинец-210, висмут-210, полоний-210",
        "радий-228, актиний-228",
        "актиний-225, франций-221, астат-217, висмут-213, полоний-213, титан-209, свинец-209",
        "актиний-227, франций-223",
        "торий-226, радий-222, радон-218, полоний-214",
        "торий-228, радий-224, радон-220, полоний-216, свинец-212, висмут-212, титан-208, полоний-212",
        "торий-229, радий-225, актиний-225, франций-221, астат-217, висмут-213, полоний-213, свинец-209",
        "торий-232, радий-228, актиний-228, торий-228, радий-224, радон-220, полоний-216, свинец-212, висмут-212, титан-208, полоний-212",
        "торий природный, радий-228, актиний-228, торий-228, радий-224, радон-220, полоний-216, свинец-212, висмут-212, титан-208, полоний-212",
        "торий-234, протактиний-234м",
        "уран-230, торий-226, радий-222, радон-218, полоний-214",
        "уран-232, торий-228, радий-224, радон-220, полоний-216, свинец-212, висмут-212, титан-208, полоний-212",
        "уран-235, торий-231",
        "уран-238, торий-234, протактиний-234м",
        "уран природный, торий-234, протактиний-234м, уран-234, торий-230, радий-226, радон-222, полоний-218, свинец-214, висмут-214, полоний-214, свинец-210, висмут-210, полоний-210",
        "уран-240, нептуний-240м",
        "нептуний-237, протактиний-233",
        "америций-242м, америций-242",
        "америций-243, нептуний-239"
    };

    #endregion

    #region Methods

    #region CheckRepPeriod

    private protected static List<CheckError> CheckRepPeriod(List<Form1> forms, Report rep)
    {
        List<CheckError> result = new();
        var endPerString = (rep.EndPeriod_DB ?? string.Empty).Trim();
        if (!DateOnly.TryParse(endPerString, out var endPeriod)) return result;
        DateOnly minOpDate = new();
        DateOnly maxEndPeriodDate = new();
        var opCode = string.Empty;
        var line = 0;
        string[] operationCodeWithDeadline1 = { "71" };
        string[] operationCodeWithDeadline5 = { "73", "74", "75" };
        string[] operationCodeWithDeadline10 =
        {
            "11", "12", "13", "14", "15", "16", "17", "18", "21", "22", "25", "26", "27", "28", "29",
            "31", "32", "35", "37", "38", "39", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "51", "52", "53", "54", "55", "56", "57", "58", "59", "61", "62", "63", "64", "65", "66",
            "67", "68", "72", "76", "81", "82", "83", "84", "85", "86", "87", "88", "97", "98", "99"
        };
        string[] operationCodeWithDeadline90 = { "01" };
        var formNum = rep.FormNum_DB.Replace(".", "");
        foreach (var form in forms)
        {
            var curOpCode = (form.OperationCode_DB ?? string.Empty).Trim();
            var curOpDate = (form.OperationDate_DB ?? string.Empty).Trim();
            DateOnly curMaxEndPeriodDate = new();
            if (!DateOnly.TryParse(curOpDate, out var opDate)) continue;
            if (operationCodeWithDeadline1.Contains(curOpCode)) curMaxEndPeriodDate = AddNWorkingDays(opDate, 1);
            if (operationCodeWithDeadline5.Contains(curOpCode)) curMaxEndPeriodDate = AddNWorkingDays(opDate, 5);
            if (operationCodeWithDeadline10.Contains(curOpCode)) curMaxEndPeriodDate = AddNWorkingDays(opDate, 10);
            if (operationCodeWithDeadline90.Contains(curOpCode)) curMaxEndPeriodDate = AddNWorkingDays(opDate, 90);
            if (minOpDate == DateOnly.MinValue || curMaxEndPeriodDate < maxEndPeriodDate)
            {
                minOpDate = opDate;
                maxEndPeriodDate = curMaxEndPeriodDate;
                opCode = form.OperationCode_DB ?? string.Empty;
                line = form.NumberInOrder_DB - 1;
            }
        }
        if (operationCodeWithDeadline90.Contains(opCode) && endPeriod > maxEndPeriodDate)
        {
            result.Add(new CheckError
            {
                FormNum = $"form_{formNum}",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = Convert.ToString(forms[line].OperationDate_DB),
                Message = $"Дата окончания отчетного периода {endPerString} превышает дату операции {minOpDate} " +
                          $"более чем на 90 рабочих дней."
            });
        }
        if (operationCodeWithDeadline10.Contains(opCode) && endPeriod > maxEndPeriodDate)
        {
            result.Add(new CheckError
            {
                FormNum = $"form_{formNum}",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = Convert.ToString(forms[line].OperationDate_DB),
                Message = $"Дата окончания отчетного периода {endPerString} превышает дату операции {minOpDate} " +
                          $"более чем на 10 рабочих дней."
            });
        }
        else if (operationCodeWithDeadline5.Contains(opCode) && endPeriod > maxEndPeriodDate)
        {
            result.Add(new CheckError
            {
                FormNum = $"form_{formNum}",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = Convert.ToString(forms[line].OperationDate_DB),
                Message = $"Дата окончания отчетного периода {endPerString} превышает дату операции {minOpDate} " +
                          $"более чем на 5 рабочих дней."
            });
        }
        else if (operationCodeWithDeadline1.Contains(opCode) && endPeriod > maxEndPeriodDate)
        {
            result.Add(new CheckError
            {
                FormNum = $"form_{formNum}",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = Convert.ToString(forms[line].OperationDate_DB),
                Message = $"Дата окончания отчетного периода {endPerString} превышает дату операции {minOpDate} " +
                          $"более чем на 1 рабочий день."
            });
        }
        return result;
    }

    #endregion

    #region CheckNotePresence

    private protected static bool CheckNotePresence(List<Note> notes, int line, byte graphNumber)
    {
        var valid = false;
        foreach (var note in notes)
        {
            if (note.RowNumber_DB == null) continue;
            var noteRowsSet = GetNumSetFromSequence(note.RowNumber_DB);
            var noteGraphSet = GetNumSetFromSequence(note.GraphNumber_DB);
            if (noteRowsSet.Any(noteRowNumber =>
                    noteRowNumber == line + 1
                    && noteGraphSet.Contains(graphNumber)
                    && !string.IsNullOrWhiteSpace(note.Comment_DB)))
            {
                valid = true;
                break;
            }
        }
        return valid;
    }

    private static HashSet<int> GetNumSetFromSequence(string? sequence)
    {
        var numSet = new HashSet<int>();
        var clusters = (sequence ?? string.Empty)
            .Replace(" ", string.Empty)
            .Replace(';', ',')
            .Split(',')
            .ToHashSet();
        foreach (var cluster in clusters)
        {
            if (cluster.Contains('-'))
            {
                var bounds = cluster.Split('-');
                if (bounds.Length != 2
                    || !int.TryParse(bounds[0], out var boundBegin)
                    || !int.TryParse(bounds[1], out var boundEnd)) continue;
                for (var i = boundBegin; i <= boundEnd; i++)
                {
                    numSet.Add(i);
                }
            }
            else if (int.TryParse(cluster, out var clusterIntValue))
            {
                numSet.Add(clusterIntValue);
            }
        }
        return numSet;
    }

    #endregion

    #region ConvertSequenceSetToRangeString

    private protected static string ConvertSequenceSetToRangeString(HashSet<int> duplicatesLinesSet)
    {
        // Отсортируем группу (если она не отсортирована изначально)
        var sortedGroup = duplicatesLinesSet
            .OrderBy(x => x)
            .ToList();

        var start = sortedGroup[0];
        var end = sortedGroup[0];
        var ranges = new List<string>();
        for (var i = 1; i < sortedGroup.Count; i++)
        {
            if (sortedGroup[i] == end + 1)
            {
                end = sortedGroup[i]; // продолжаем диапазон
            }
            else
            {
                // добавляем текущий диапазон в список
                ranges.Add(start == end
                    ? start.ToString()
                    : end == start + 1
                        ? $"{start}, {end}"
                        : $"{start}-{end}");
                // начинаем новый диапазон
                start = sortedGroup[i];
                end = sortedGroup[i];
            }
        }
        // добавляем последний диапазон
        ranges.Add(start == end
            ? start.ToString()
            : end == start + 1
                ? $"{start}, {end}"
                : $"{start}-{end}");
        // объединяем диапазоны в строку и добавляем в общий список
        return string.Join(", ", ranges);
    }

    #endregion

    #region ConvertStringToExponential

    protected static string ConvertStringToExponential(string? str) =>
        (str ?? string.Empty)
        .ToLower()
        .Trim()
        .TrimStart('(')
        .TrimEnd(')')
        .Replace(".", ",")
        .Replace("*", "")
        .Replace('е', 'e');

    #endregion

    #region CustomComparator

    //  Performance realisation for Compare with Trim()
    private protected class CustomNullStringWithTrimComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (ReferenceEquals(x, y))
                return 0;
            if (x is null)
                return -1;
            if (y is null)
                return 1;

            // Memory allocation free trimming
            var span1 = x.AsSpan().Trim();
            var span2 = y.AsSpan().Trim();

            return span1.CompareTo(span2, StringComparison.OrdinalIgnoreCase);

            // Old realisation
            //var strA = ReplaceNullAndTrim(x).ToLower();
            //var strB = ReplaceNullAndTrim(y).ToLower();
            //return string.CompareOrdinal(strA, strB);
        }
    }

    #endregion

    #region LoadDictionaries

    private protected static void LoadDictionaries()
    {
        if (OKSM.Count == 0)
        {
#if DEBUG
            OKSM_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "oksm.xlsx"));
#else
            OKSM_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"oksm.xlsx"));
#endif
        }

        if (R.Count == 0)
        {
#if DEBUG
            R_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx"));
#else
            R_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"R.xlsx"));
#endif
        }

        if (Packs.Count == 0)
        {
#if DEBUG
            Packs_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "Packs.xlsx"));
#else
            Packs_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"Packs.xlsx"));
#endif
        }

        if (Orgs18.Count == 0)
        {
#if DEBUG
            Orgs18_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "Orgs_1.8.xlsx"));
#else
            Orgs18_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"Orgs_1.8.xlsx.xlsx"));
#endif
        }

        if (HolidaysSpecific.Count == 0)
        {
#if DEBUG
            Holidays_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "Holidays.xlsx"));
#else
            Holidays_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"Holidays.xlsx"));
#endif
        }

        if (IOU11.Count == 0)
        {
#if DEBUG
            IOU11_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "IOU11.xlsx"));
#else
            IOU11_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"IOU11.xlsx"));
#endif
        }
    }

    #region IOU11_From_File

    private static void IOU11_From_File(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(filePath)) return;
        FileInfo excelImportFile = new(filePath);
        var xls = new ExcelPackage(excelImportFile);
        var worksheet = xls.Workbook.Worksheets["Лист1"];
        var i = 1;
        IOU11.Clear();
        while (worksheet.Cells[i, 1].Text != string.Empty)
        {
            IOU11.Add(worksheet.Cells[i, 1].Text);
            i++;
        }
    }

    #endregion

    #region  HolidaysFromFile

    //функция импорта праздничных и рабочих дат из справочника
    private static void Holidays_Populate_From_File(string fileAddress)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(fileAddress)) return;
        FileInfo excelImportFile = new(fileAddress);
        var xls = new ExcelPackage(excelImportFile);
        var worksheet1 = xls.Workbook.Worksheets["Выходные"];
        var i = 1;
        HolidaysSpecific.Clear();
        while (worksheet1.Cells[i, 1].Text != string.Empty)
        {
            var valueDate = worksheet1.Cells[i, 1].Text;
            if (DateOnly.TryParse(valueDate, out var fixDate))
            {
                HolidaysSpecific.Add(fixDate);
            }
            i++;
        }
        var worksheet2 = xls.Workbook.Worksheets["Рабочие"];
        i = 1;
        WorkDaysSpecific.Clear();
        while (worksheet2.Cells[i, 1].Text != string.Empty)
        {
            var valueDate = worksheet2.Cells[i, 1].Text;
            if (DateOnly.TryParse(valueDate, out var fixDate))
            {
                WorkDaysSpecific.Add(fixDate);
            }
            i++;
        }
    }

    #endregion

    #region OKSMFromFile

    private static void OKSM_Populate_From_File(string fileAddress)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(fileAddress)) return;
        FileInfo excel_import_file = new(fileAddress);
        var xls = new ExcelPackage(excel_import_file);
        var worksheet1 = xls.Workbook.Worksheets["Лист1"];
        var i = 8;
        OKSM.Clear();
        while (worksheet1.Cells[i, 1].Text != string.Empty)
        {
            OKSM.Add(new Dictionary<string, string>
            {
                {"kod", worksheet1.Cells[i, 2].Text},
                {"shortname", worksheet1.Cells[i, 3].Text},
                {"longname", worksheet1.Cells[i, 4].Text},
                {"alpha2", worksheet1.Cells[i, 5].Text},
                {"alpha3", worksheet1.Cells[i, 6].Text}
            });
            i++;
        }
    }

    #endregion

    #region RFromFile

    private static void R_Populate_From_File(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(filePath)) return;
        FileInfo excelImportFile = new(filePath);
        var xls = new ExcelPackage(excelImportFile);
        var worksheet = xls.Workbook.Worksheets["Лист1"];
        var i = 2;
        R.Clear();
        while (worksheet.Cells[i, 1].Text != string.Empty)
        {
            R.Add(new Dictionary<string, string>
            {
                {"name", worksheet.Cells[i, 1].Text},
                {"value", worksheet.Cells[i, 5].Text},
                {"unit", worksheet.Cells[i, 6].Text},
                {"decay", worksheet.Cells[i, 7].Text},
                {"code", worksheet.Cells[i, 8].Text},
                {"D", worksheet.Cells[i, 15].Text},
                {"MZUA", worksheet.Cells[i, 16].Text},
                {"MZA", worksheet.Cells[i, 17].Text},
                {"A_Liquid", worksheet.Cells[i, 18].Text},
                {"A_Solid", worksheet.Cells[i, 20].Text},
                {"OSPORB_Solid", worksheet.Cells[i, 22].Text},
                {"OSPORB_Liquid", worksheet.Cells[i, 23].Text},
                {"Ki", worksheet.Cells[i, 25].Text}
            });
            if (string.IsNullOrWhiteSpace(R[^1]["D"]) || !double.TryParse(R[^1]["D"], out var val1) || val1 < 0)
            {
                R[^1]["D"] = double.MaxValue.ToString(CultureInfo.CreateSpecificCulture("ru-RU"));
            }
            i++;
        }
    }

    #endregion

    #region PacksFromFile

    private static void Packs_Populate_From_File(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(filePath)) return;
        FileInfo excelImportFile = new(filePath);
        var xls = new ExcelPackage(excelImportFile);
        var worksheet = xls.Workbook.Worksheets["Лист1"];
        var i = 2;
        Packs.Clear();
        while (worksheet.Cells[i, 1].Text != string.Empty)
        {
            Packs.Add(new Dictionary<string, string>
            {
                {"name", worksheet.Cells[i, 1].Text},
                {"dimensions", worksheet.Cells[i, 2].Text},
                {"volume", worksheet.Cells[i, 3].Text},
                {"mass", worksheet.Cells[i, 4].Text},
                {"coef", worksheet.Cells[i, 5].Text},
                {"class", worksheet.Cells[i, 6].Text},
                {"mass_total", worksheet.Cells[i, 7].Text}
            });
            i++;
        }
    }

    #endregion

    #region Orgs18FromFile

    private static void Orgs18_Populate_From_File(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(filePath)) return;
        FileInfo excelImportFile = new(filePath);
        var xls = new ExcelPackage(excelImportFile);
        var worksheet = xls.Workbook.Worksheets["Лист1"];
        var i = 1;
        Orgs18.Clear();
        while (worksheet.Cells[i, 1].Text != string.Empty)
        {
            Orgs18.Add(worksheet.Cells[i, 1].Text);
            i++;
        }
    }

    #endregion

    #endregion

    #region OverdueCalculations

    //вычисление нарушения сроков предоставления отчётов с учетом праздников

    //кол-во дней между датой операции (документа для 10) и окончанием ОП

    protected static readonly Dictionary<string, int> OverduePeriods_RV = new()
    {
        { "10", 10 },{ "11", 10 },{ "12", 10 },                          { "15", 10 },             { "17", 10 },{ "18", 10 },
                     { "21", 10 },{ "22", 10 },                          { "25", 10 },             { "27", 10 },{ "28", 10 },{ "29", 10 },
                     { "31", 10 },{ "32", 10 },                          { "35", 10 },             { "37", 10 },{ "38", 10 },{ "39", 10 },
                     { "41", 10 },{ "42", 10 },{ "43", 10 },                          { "46", 10 },{ "47", 10 },{ "48", 10 },
                                               { "53", 10 },{ "54", 10 },                                       { "58", 10 },
                     { "61", 10 },{ "62", 10 },{ "63", 10 },{ "64", 10 },{ "65", 10 },{ "66", 10 },{ "67", 10 },{ "68", 10 },
                     { "71", 01 },{ "72", 10 },{ "73", 05 },{ "74", 05 },{ "75", 05 },
                     { "81", 10 },{ "82", 10 },{ "83", 10 },{ "84", 10 },{ "85", 10 },{ "86", 10 },{ "87", 10 },{ "88", 10 },
                                                                                                   { "97", 10 },{ "98", 10 },{ "99", 10 }
    };

    protected static readonly Dictionary<string, int> OverduePeriods_RAO = new()
    {
        { "01", 90 },
        { "10", 10 },{ "11", 10 },{ "12", 10 },{ "13", 10 },{ "14", 10 },             { "16", 10 },             { "18", 10 },
                     { "21", 10 },{ "22", 10 },                          { "25", 10 },{ "26", 10 },{ "27", 10 },{ "28", 10 },{ "29", 10 },
                     { "31", 10 },{ "32", 10 },                          { "35", 10 },{ "36", 10 },{ "37", 10 },{ "38", 10 },{ "39", 10 },
                     { "41", 10 },{ "42", 10 },{ "43", 10 },{ "44", 10 },{ "45", 10 },                          { "48", 10 },{ "49", 10 },
                     { "51", 10 },{ "52", 10 },                          { "55", 10 },{ "56", 10 },{ "57", 10 },             { "59", 10 },
                                               { "63", 10 },{ "64", 10 },                                       { "68", 10 },
                     { "71", 01 },{ "72", 10 },{ "73", 05 },{ "74", 05 },{ "75", 05 },{ "76", 10 },
                                                            { "84", 10 },                                       { "88", 10 },
                                                                                                   { "97", 10 },{ "98", 10 },{ "99", 10 }
    };

    //праздники из года в год
    private static readonly List<DateOnly> HolidaysGeneric = new()
    {
        new DateOnly(1,1,1), //1 января
        new DateOnly(1,1,2), //2 января
        new DateOnly(1,1,3), //3 января
        new DateOnly(1,1,4), //4 января
        new DateOnly(1,1,5), //5 января
        new DateOnly(1,1,6), //6 января
        new DateOnly(1,1,7), //7 января
        new DateOnly(1,1,8), //8 января
        new DateOnly(1,2,23), //23 февраля
        new DateOnly(1,3,8), //8 марта
        new DateOnly(1,5,1), //1 мая
        new DateOnly(1,5,9), //9 мая
        new DateOnly(1,6,12), //12 июня
        new DateOnly(1,11,4), //4 ноября
    };

    //рабочие дни по выходным, уникальные для каждого года, берутся из файла ./Spravochniki/Holidays.xlsx
    private static readonly List<DateOnly> WorkDaysSpecific = new();

    //праздники конкретных годов, берутся из файла ./Spravochniki/Holidays.xlsx
    private protected static readonly List<DateOnly> HolidaysSpecific = new();

    //расчет кол-ва рабочих дней между двумя датами
    //strict_order - ожидать даты в правильном порядке
    private protected static int WorkdaysBetweenDates(DateOnly date1, DateOnly date2, bool strictOrder = true)
    {
        var result = 0;
        DateOnly dateMin;
        DateOnly dateMax;
        if (strictOrder)
        {
            dateMin = date1;
            dateMax = date2;
        }
        else
        {
            dateMin = date1 < date2 ? date1 : date2;
            dateMax = dateMin == date2 ? date1 : date2;
        }
        if (dateMin > dateMax)
        {
            return int.MaxValue;
        }
        for (var day = dateMin.AddDays(1); day <= dateMax; day = day.AddDays(1))
        {
            if (IsWorkingDay(day))
            {
                result++;
            }
        }
        return result;
    }

    #endregion

    #region ReplaceNullAndTrim

    private protected static string ReplaceNullAndTrim(string? str) => (str ?? string.Empty).Trim();

    #endregion

    #region StringRemoveSpecials

    protected static string StringRemoveSpecials(string? str) =>
        (str ?? string.Empty)
        .Replace("\\", "")
        .Replace("(", "")
        .Replace(")", "")
        .Replace("/", "")
        .Replace(".", "")
        .Replace(",", "")
        .Replace("-", "")
        .Replace("_", "")
        .Replace(" ", "")
        .Replace("`", "")
        .Replace("'", "")
        .Replace("\"", "")
        .Replace("ё", "е")
        .Replace("—", "");

    #endregion

    #region TryParseFloatExtended

    protected static bool TryParseFloatExtended(string? str, out float val)
    {
        return float.TryParse(ConvertStringToExponential(str),
            NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
            CultureInfo.CreateSpecificCulture("ru-RU"),
            out val);
    }

    #endregion

    #region TryParseDoubleExtended

    protected static bool TryParseDoubleExtended(string? str, out double val)
    {
        return double.TryParse(ConvertStringToExponential(str),
            NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
            CultureInfo.CreateSpecificCulture("ru-RU"),
            out val);
    }

    #endregion

    #region WorkingDay

    #region AddNWorkingDays

    private static DateOnly AddNWorkingDays(DateOnly day, int daysNum)
    {
        for (var i = 1; i <= daysNum; i++)
        {
            while (!IsWorkingDay(day.AddDays(1)))
            {
                day = day.AddDays(1);
            }
            day = day.AddDays(1);
        }
        return day;
    }

    #endregion

    #region IsWorkingDay

    private static bool IsWorkingDay(DateOnly day)
    {
        return WorkDaysSpecific.Any(x => Equals(x, day))
               || !(day.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
                    || HolidaysSpecific.Any(x => Equals(x, day))
                    || HolidaysGeneric.Any(x => Equals(x.Month, day.Month) && Equals(x.Day, day.Day)));
    }

    #endregion

    #endregion 

    #endregion
}