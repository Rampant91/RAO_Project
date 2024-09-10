﻿using Models.CheckForm;
using Models.Collections;
using Models.Forms.Form1;
using System.Collections.Generic;
using Models.Forms;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System;

namespace Client_App.Commands.SyncCommands.CheckForm;

/// <summary>
/// Проверка отчётов по форме 1.8. 
/// </summary>
public abstract class CheckF18 : CheckBase
{
    #region Properties

    private static readonly string[] OperationCode_DB_Valids =
    {
        "01","10","11","12","13","18","21","25",
        "26","27","28","29","31","32","35","36","37","38",
        "39","42","51","52","55","63","64","68","97","98"
    };

    private static readonly Dictionary<string, string> GraphsList = new()
    {
        { "NumberInOrder_DB", "01 - № п/п" },
        { "OperationCode_DB", "02 - Код операции" },
        { "OperationDate_DB", "03 - Дата операции" },
        { "IndividualNumberZHRO_DB", "04 - индивидуальный номер (идентификационный код) партии ЖРО" },
        { "PassportNumber_DB", "05 - Номер паспорта" },
        { "Volume6_DB", "06 - Объем (сведения о партии), куб. м" },
        { "Mass7_DB", "07 - Масса (сведения о партии), т" },
        { "SaltConcentration_DB", "08 - Солесодержание, г/л" },
        { "Radionuclids_DB", "09 - Наименование радионуклида" },
        { "SpecificActivity_DB", "10 - Удельная активность, Бк/г" },
        { "DocumentVid_DB", "11 - Вид документа" },
        { "DocumentNumber_DB", "12 - Номер документа" },
        { "DocumentDate_DB", "13 - Дата документа" },
        { "ProviderOrRecieverOKPO_DB", "14 - Код ОКПО поставщика или получателя" },
        { "TransporterOKPO_DB", "15 - Код ОКПО перевозчика" },
        { "StoragePlaceName_DB", "16 - Наименование пункта хранения" },
        { "StoragePlaceCode_DB", "17 - Код пункта хранения" },
        { "CodeRAO_DB", "18 - Код РАО" },
        { "StatusRAO_DB", "19 - Статус РАО" },
        { "Volume20_DB", "20 - Объем, куб. м" },
        { "Mass21_DB", "21 - Масса, т" },
        { "TritiumActivity_DB", "22 - Суммарная активность, тритий" },
        { "BetaGammaActivity_DB", "23 - Суммарная активность, бета-, гамма-излучающие радионуклиды (исключая тритий)" },
        { "AlphaActivity_DB", "24 - Суммарная активность, альфа-излучающие радионуклиды (исключая трансурановые)" },
        { "TransuraniumActivity_DB", "25 - Суммарная активность, трансурановые" },
        { "RefineOrSortRAOCode_DB", "26 - Код переработки/сортировки" },
        { "Subsidy_DB", "27 - Субсидия" },
        { "FcpNumber_DB", "28 - Номер мероприятия ФЦП" }
    };

    #endregion

    #region CheckTotal

    public static List<CheckError> Check_Total(Reports reps, Report rep)
    {
        var currentFormLine = 0;
        List<CheckError> errorList = new();
        LoadDictionaries();
        var formsList = rep.Rows18.ToList<Form18>();
        var notes = rep.Notes.ToList<Note>();
        var forms10 = reps.Master_DB.Rows10.ToList<Form10>();
        errorList.AddRange(Check_002(forms10));
        errorList.AddRange(Check_003(formsList));
        while (currentFormLine < formsList.Count)
        {
            List<int> packLines = new();
            packLines.Add(currentFormLine);
            currentFormLine++;
            if (currentFormLine >= formsList.Count) break;
            while (string.IsNullOrWhiteSpace(formsList[currentFormLine].IndividualNumberZHRO_DB) 
                   || formsList[currentFormLine].IndividualNumberZHRO_DB.Trim() == "-")
            {
                packLines.Add(currentFormLine);
                currentFormLine++;
                if (currentFormLine >= formsList.Count) break;
            }
            for (var line = 0; line < packLines.Count; line++)
            {
                if (line != 0) continue;
                //checks required only for the head line, also checks that are supposed to only run once per block.
                errorList.AddRange(Check_004(formsList, packLines[line]));
                errorList.AddRange(Check_005(formsList, packLines[line]));
                errorList.AddRange(Check_005_99(formsList, packLines[line]));
                errorList.AddRange(Check_005_29(formsList, notes, packLines[line]));
                errorList.AddRange(Check_005_18(formsList, packLines[line]));
                errorList.AddRange(Check_006_01(formsList, packLines[line]));
                errorList.AddRange(Check_006_non10(formsList, rep, packLines[line]));
                errorList.AddRange(Check_006_10(formsList, rep, packLines[line]));
                errorList.AddRange(Check_007(formsList, packLines[line]));
                errorList.AddRange(Check_008(formsList, packLines[line]));
                errorList.AddRange(Check_009(formsList, packLines[line]));
                errorList.AddRange(Check_010(formsList, packLines[line]));
                errorList.AddRange(Check_011(formsList, packLines[line]));
                errorList.AddRange(Check_012(formsList, packLines));
                errorList.AddRange(Check_013(formsList, packLines));
                errorList.AddRange(Check_014(formsList, notes, packLines[line]));
                errorList.AddRange(Check_015(formsList, packLines[line]));
                errorList.AddRange(Check_016(formsList, rep, packLines[line]));
                errorList.AddRange(Check_017_10(formsList, forms10, packLines[line]));
                errorList.AddRange(Check_017_21(formsList, forms10, packLines[line]));
                errorList.AddRange(Check_017_22(formsList, forms10, packLines[line]));
                errorList.AddRange(Check_018_01(formsList, packLines[line]));
                errorList.AddRange(Check_018_21(formsList, packLines[line]));
                errorList.AddRange(Check_018_22(formsList, forms10, packLines[line]));
                errorList.AddRange(Check_019(formsList, packLines[line]));
                errorList.AddRange(Check_020(formsList, packLines[line]));
                errorList.AddRange(Check_021(formsList, packLines));
                errorList.AddRange(Check021_RAOCODE(formsList, notes, packLines));
                errorList.AddRange(Check_022_11(formsList, forms10, packLines));
                errorList.AddRange(Check_022_26(formsList, forms10, packLines));
                errorList.AddRange(Check_022_38(formsList, forms10, packLines));
                errorList.AddRange(Check_022_42(formsList, forms10, packLines));
                errorList.AddRange(Check_022_22(formsList, packLines));
                errorList.AddRange(Check_022_16(formsList, packLines));
                errorList.AddRange(Check_022_10(formsList, packLines));
                errorList.AddRange(Check_022_other(formsList, packLines));
                errorList.AddRange(Check_023(formsList, packLines[line]));
                errorList.AddRange(Check_024(formsList, packLines[line]));
                errorList.AddRange(Check_025_027(formsList, packLines));
                errorList.AddRange(Check_029_55(formsList, packLines[line]));
                errorList.AddRange(Check_029_10(formsList, packLines[line]));
                errorList.AddRange(Check_030(formsList, packLines[line]));
                errorList.AddRange(Check_031(formsList, packLines[line]));
                errorList.AddRange(Check_Criteria(formsList, packLines));
            }
        }
        errorList.AddRange(Check_032(formsList, rep));
        var index = 0;
        foreach (var error in errorList)
        {
            if (GraphsList.TryGetValue(error.Column, out var columnFrontName))
            {
                error.Column = columnFrontName;
            }
            index++;
            error.Index = index;
        }
        return errorList;
    }

    #endregion

    #region Checks

    #region Check002

    private static List<CheckError> Check_002(List<Form10> forms10)
    {
        List<CheckError> result = new();
        var regNo = !string.IsNullOrWhiteSpace(forms10[1].RegNo_DB)
            ? forms10[1].RegNo_DB
            : forms10[0].RegNo_DB;
        var valid = !string.IsNullOrWhiteSpace(regNo) && Orgs18.Contains(regNo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = "-",
                Column = "-",
                Value = regNo,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Жидкие РАО должны быть отверждены. Сведения об отвержденных РАО, " +
                          "приведенных к критериям приемлемости, приводятся в форме 1.7."
            });
        }
        return result;
    }

    #endregion

    #region Check003

    //Наличие строк дубликатов
    private static List<CheckError> Check_003(List<Form18> forms)
    {
        List<CheckError> result = new();
        HashSet<HashSet<int>> duplicatesLinesSubset = new();
        var comparator = new CustomNullStringWithTrimComparer();
        for (var i = 0; i < forms.Count; i++)
        {
            var currentForm = forms[i];
            for (var j = i + 1; j < forms.Count; j++)
            {
                var formToCompare = forms[j];
                var isDuplicate = formToCompare.NumberInOrder_DB == currentForm.NumberInOrder_DB
                                && comparator.Compare(formToCompare.OperationCode_DB, currentForm.OperationCode_DB) == 0
                                && comparator.Compare(formToCompare.OperationDate_DB, currentForm.OperationDate_DB) == 0
                                && comparator.Compare(formToCompare.IndividualNumberZHRO_DB, currentForm.IndividualNumberZHRO_DB) == 0
                                && comparator.Compare(formToCompare.PassportNumber_DB, currentForm.PassportNumber_DB) == 0
                                && comparator.Compare(formToCompare.Volume6_DB, currentForm.Volume6_DB) == 0
                                && comparator.Compare(formToCompare.Mass7_DB, currentForm.Mass7_DB) == 0
                                && comparator.Compare(formToCompare.SaltConcentration_DB, currentForm.SaltConcentration_DB) == 0
                                && comparator.Compare(formToCompare.Radionuclids_DB, currentForm.Radionuclids_DB) == 0
                                && comparator.Compare(formToCompare.SpecificActivity_DB, currentForm.SpecificActivity_DB) == 0
                                && formToCompare.DocumentVid_DB == currentForm.DocumentVid_DB
                                && comparator.Compare(formToCompare.DocumentNumber_DB, currentForm.DocumentNumber_DB) == 0
                                && comparator.Compare(formToCompare.DocumentDate_DB, currentForm.DocumentDate_DB) == 0
                                && comparator.Compare(formToCompare.ProviderOrRecieverOKPO_DB, currentForm.ProviderOrRecieverOKPO_DB) == 0
                                && comparator.Compare(formToCompare.TransporterOKPO_DB, currentForm.TransporterOKPO_DB) == 0
                                && comparator.Compare(formToCompare.StoragePlaceName_DB, currentForm.StoragePlaceName_DB) == 0
                                && comparator.Compare(formToCompare.StoragePlaceCode_DB, currentForm.StoragePlaceCode_DB) == 0
                                && comparator.Compare(formToCompare.CodeRAO_DB, currentForm.CodeRAO_DB) == 0
                                && comparator.Compare(formToCompare.StatusRAO_DB, currentForm.StatusRAO_DB) == 0
                                && comparator.Compare(formToCompare.Volume20_DB, currentForm.Volume20_DB) == 0
                                && comparator.Compare(formToCompare.Mass21_DB, currentForm.Mass21_DB) == 0
                                && comparator.Compare(formToCompare.TritiumActivity_DB, currentForm.TritiumActivity_DB) == 0
                                && comparator.Compare(formToCompare.BetaGammaActivity_DB, currentForm.BetaGammaActivity_DB) == 0
                                && comparator.Compare(formToCompare.AlphaActivity_DB, currentForm.AlphaActivity_DB) == 0
                                && comparator.Compare(formToCompare.TransuraniumActivity_DB, currentForm.TransuraniumActivity_DB) == 0
                                && comparator.Compare(formToCompare.RefineOrSortRAOCode_DB, currentForm.RefineOrSortRAOCode_DB) == 0
                                && comparator.Compare(formToCompare.Subsidy_DB, currentForm.Subsidy_DB) == 0
                                && comparator.Compare(formToCompare.FcpNumber_DB, currentForm.FcpNumber_DB) == 0;
                if (!isDuplicate) continue;
                var newLine = true;
                foreach (var subset in duplicatesLinesSubset
                             .Where(subset => subset.Contains(i + 1)))
                {
                    subset.Add(j + 1);
                    newLine = false;
                    break;
                }
                if (newLine)
                {
                    HashSet<int> newSubset = [i + 1, j + 1];
                    duplicatesLinesSubset.Add(newSubset);
                }
            }
        }
        foreach (var subset in duplicatesLinesSubset)
        {
            var duplicateLines = string.Join(", ", subset.Order());
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = duplicateLines,
                Column = "2 - 28",
                Value = "",
                Message = $"Данные граф 2-28 в строках {duplicateLines} продублированы. Следует проверить правильность предоставления данных."
            });
        }
        return result;
    }

    #endregion

    #region Check004

    private static List<CheckError> Check_004(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var valid = (line == 0 && forms[line].NumberInOrder_DB is 1 or 0)
                    || line > 0 && forms[line - 1].NumberInOrder_DB == forms[line].NumberInOrder_DB - 1;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "NumberInOrder_DB",
                Value = forms[line].NumberInOrder_DB.ToString(),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Номера строк должны располагаться по порядку, без пропусков или дублирования номеров. " +
                          $"{Environment.NewLine}Для устранения ошибки воспользуйтесь либо кнопкой сортировки " +
                          $"(строки будут отсортированы по №п/п, поменяв свою позицию), " +
                          $"{Environment.NewLine}либо кнопкой выставить порядок строк " +
                          $"(у строк будет изменён №п/п, но они при этом не поменяют свой порядок)."
            });
        }
        return result;
    }

    #endregion

    #region Check005

    private static List<CheckError> Check_005(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var valid = OperationCode_DB_Valids.Contains(operationCode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Код операции не может быть использован в форме 1.8."
            });
        }
        return result;
    }

    #endregion

    #region Check005_99

    private static List<CheckError> Check_005_99(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var codeRao = ReplaceNullAndTrim(forms[line].CodeRAO_DB);
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (codeRao.Length < 10) return result;
        var specialOperationCodes = new[] { "11", "12", "13", "18", "55" };
        var valid = codeRao.Substring(8, 2) != "99";
        if (!valid)
        {
            if (specialOperationCodes.Contains(operationCode))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = codeRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Для вновь образованных РАО код типа РАО «99» не может быть использован."
                });
            }
            else
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = codeRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Код типа РАО «99» не может быть использован для РАО, соответствующим критериям приемлемости. " +
                              "Должны быть представлены сведения для каждого кода РАО."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check005_29

    private static List<CheckError> Check_005_29(List<Form18> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOperationCodes = new[] { "29", "39", "97", "98" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        const byte graphNumber = 2;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Необходимо дать пояснение об осуществленной операции."
            });
        }
        return result;
    }

    #endregion

    #region Check005_18

    private static List<CheckError> Check_005_18(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOperationCodes = new[] { "18", "68" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = true;   //error is disabled because it's instructional
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "К отчету необходимо приложить скан-копию документа характеризующего операцию."
            });
        }
        return result;
    }

    #endregion

    #region Check006_01

    private static List<CheckError> Check_006_01(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "01" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var operationDate = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = DateOnly.TryParse(operationDate, out var operationDateReal) 
                    && operationDateReal is { Year: 2021, Month: 12, Day: 31 };
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationDate_DB",
                Value = Convert.ToString(operationDate),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Сведения с кодом 01 должны быть представлены на 31.12.2021."
            });
        }
        return result;
    }

    #endregion

    #region Check006_non10

    private static List<CheckError> Check_006_non10(List<Form18> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] nonApplicableOperationCodes = { "10", "01" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var operationDate = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        if (nonApplicableOperationCodes.Contains(operationCode)) return result;
        var valid = rep is { StartPeriod_DB: not null, EndPeriod_DB: not null }
                    && DateOnly.TryParse(rep.StartPeriod_DB, out var pStart)
                    && DateOnly.TryParse(rep.EndPeriod_DB, out var pEnd)
                    && DateOnly.TryParse(operationDate, out var pMid)
                    && pMid >= pStart && pMid <= pEnd;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationDate_DB",
                Value = operationDate,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Дата операции не входит в отчетный период."
            });
        }
        return result;
    }

    #endregion

    #region Check006_10

    private static List<CheckError> Check_006_10(List<Form18> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "10" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var documentDate = ReplaceNullAndTrim(forms[line].DocumentDate_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = rep is { StartPeriod_DB: not null, EndPeriod_DB: not null } 
                    && DateOnly.TryParse(rep.StartPeriod_DB, out var pStart)
                    && DateOnly.TryParse(rep.EndPeriod_DB, out var pEnd)
                    && DateOnly.TryParse(documentDate, out var pMid)
                    && pMid >= pStart && pMid <= pEnd;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentDate_DB",
                Value = Convert.ToString(documentDate),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Дата документа не входит в отчетный период. " +
                          "Для операции инвентаризации срок предоставления отчета исчисляется с даты утверждения акта инвентаризации."
            });
        }
        return result;
    }

    #endregion

    #region Check007

    private static List<CheckError> Check_007(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var individualNumberZHRO = ReplaceNullAndTrim(forms[line].IndividualNumberZHRO_DB);
        if (string.IsNullOrWhiteSpace(individualNumberZHRO))
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "IndividualNumberZHRO_DB",
                Value = individualNumberZHRO,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Заполните сведения об индивидуальном номере партии ЖРО."
            });
        }
        return result;
    }

    #endregion

    #region Check008

    private static List<CheckError> Check_008(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var pasNum = ReplaceNullAndTrim(forms[line].PassportNumber_DB);
        if (string.IsNullOrWhiteSpace(pasNum))
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "PassportNumber_DB",
                Value = pasNum,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Заполните сведения об номере паспорта партии ЖРО."
            });
        }
        return result;
    }

    #endregion

    #region Check009

    private static List<CheckError> Check_009(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var volume6 = ConvertStringToExponential(forms[line].Volume6_DB);
        if (string.IsNullOrWhiteSpace(volume6) 
            || !TryParseFloatExtended(volume6, out var volume6Real) 
            || volume6Real <= 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Volume6_DB",
                Value = volume6,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Необходимо заполнить сведения об объеме партии ЖРО."
            });
            return result;
        }
        return result;
    }

    #endregion

    #region Check010

    private static List<CheckError> Check_010(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var mass7 = ConvertStringToExponential(forms[line].Mass7_DB);
        if (string.IsNullOrWhiteSpace(mass7) 
            || !TryParseFloatExtended(mass7, out var mass7Real) 
            || mass7Real <= 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Mass7_DB",
                Value = mass7,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Необходимо заполнить сведения о массе брутто партии ЖРО."
            });
            return result;
        }
        return result;
    }

    #endregion

    #region Check011

    private static List<CheckError> Check_011(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var saltConcentration = ConvertStringToExponential(forms[line].SaltConcentration_DB);
        if (string.IsNullOrWhiteSpace(saltConcentration) 
            || !TryParseFloatExtended(saltConcentration, out var saltConcentrationReal) 
            || saltConcentrationReal <= 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "SaltConcentration_DB",
                Value = saltConcentration,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Необходимо заполнить сведения о солесодержании."
            });
            return result;
        }
        return result;
    }

    #endregion

    #region Check012

    private static List<CheckError> Check_012(List<Form18> forms, List<int> lines)
    {
        List<CheckError> result = new();
        foreach (var line in lines)
        {
            var radionuclids = ReplaceNullAndTrim(forms[line].Radionuclids_DB);
            var rad = radionuclids.ToLower().Replace('\n', ' ').Replace('e', 'е').Replace('a', 'а').Trim();
            if (string.IsNullOrWhiteSpace(rad) || rad == "-") continue;
            if (rad.Replace(" ", "").Replace(',', ';').Split(';').Length != 1)
            {
                //отсебятина, навеяно формированием паспорта, которое считает, что в каждой строчке есть только один нуклид
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Radionuclids_DB",
                    Value = radionuclids,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "В форме 1.8 в каждой строчке указывается до одного радионуклида."
                });
            }
            else if (R.All(phEntry => phEntry["name"] != rad))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Radionuclids_DB",
                    Value = radionuclids,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Наименование радионуклида указывается названием химического элемента на русском языке, " +
                              "с указанием через дефис массового числа изотопа."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check013

    private static List<CheckError> Check_013(List<Form18> forms, List<int> lines)
    {
        List<CheckError> result = new();
        foreach (var line in lines)
        {
            var specificActivity = ConvertStringToExponential(forms[line].SpecificActivity_DB);
            if (string.IsNullOrWhiteSpace(specificActivity) || specificActivity == "-") continue;
            if (!TryParseFloatExtended(specificActivity, out var specificActivityReal) || specificActivityReal <= 0)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "SpecificActivity_DB",
                    Value = specificActivity,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Необходимо заполнить сведения об удельной активности радионуклида."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check014

    private static List<CheckError> Check_014(List<Form18> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        List<string> applicableDocumentVids = new()
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "19"
        };
        var documentVid = forms[line].DocumentVid_DB?.ToString().Trim() ?? "";
        const byte graphNumber = 14;
        var noteExists = CheckNotePresence(notes, line, graphNumber);
        if (string.IsNullOrWhiteSpace(documentVid))
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentVid_DB",
                Value = documentVid,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Графа не может быть пустой."
            });
        }
        else if (!applicableDocumentVids.Contains(documentVid))
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentVid_DB",
                Value = documentVid,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Недопустимый код вида документа."
            });
        }
        else if (documentVid == "19" && !noteExists)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentVid_DB",
                Value = documentVid,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Необходимо примечание с указанием наименования документа."
            });
        }
        return result;
    }

    #endregion

    #region Check015

    private static List<CheckError> Check_015(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var documentNumber = ReplaceNullAndTrim(forms[line].DocumentNumber_DB);
        var valid = !string.IsNullOrWhiteSpace(documentNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentNumber_DB",
                Value = documentNumber,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check016

    private static List<CheckError> Check_016(List<Form18> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var operationDate = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        var documentDate = ReplaceNullAndTrim(forms[line].DocumentDate_DB);
        bool valid;
        if (!DateOnly.TryParse(documentDate, out var pMid))
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentDate_DB",
                Value = Convert.ToString(documentDate),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Необходимо указать дату документа. сопровождающего операцию."
            });
        }
        else if (operationCode == "10")
        {
            valid = DateOnly.TryParse(rep.StartPeriod_DB, out var pStart)
                    && DateOnly.TryParse(rep.EndPeriod_DB, out var pEnd)
                    && pMid >= pStart && pMid <= pEnd;
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "DocumentDate_DB",
                    Value = Convert.ToString(documentDate),
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Дата документа выходит за границы периода."
                });
            }
        }
        else
        {
            valid = DateOnly.TryParse(operationDate, out var pOper)
                    && pMid <= pOper;
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "DocumentDate_DB",
                    Value = Convert.ToString(documentDate),
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Дата документа не может быть позже даты операции."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check017_10

    //Код ОКПО поставщика/получателя равен коду ОКПО отчитывающейся организации
    private static List<CheckError> Check_017_10(List<Form18> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "01", "10", "11", "12", "13", "18", "51", "52", "55", "68", "97", "98" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        var repOkpo = okpoRepTerPodr is not ""
            ? okpoRepTerPodr
            : okpoRepJur;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        //var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = providerOrRecieverOkpo == repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Для выбранного кода операции указывается код ОКПО отчитывающейся организации."
            });
        }
        return result;
    }

    #endregion

    #region Check017_21

    //Код ОКПО поставщика/получателя не равен коду ОКПО отчитывающейся организации + 8/14 цифр
    private static List<CheckError> Check_017_21(List<Form18> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "21", "25", "26", "27", "28", "29", "31", "35", "36", "37", "38", "39" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        var repOkpo = okpoRepTerPodr is not ""
            ? okpoRepTerPodr
            : okpoRepJur;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(providerOrRecieverOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Значение может состоять только из 8 или 14 символов."
            });
        }
        valid = providerOrRecieverOkpo != repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Для выбранного кода операции указывается код ОКПО контрагента."
            });
        }
        return result;
    }

    #endregion

    #region Check017_22

    //Код ОКПО поставщика/получателя состоит из 8/14 чисел или "минобороны"
    private static List<CheckError> Check_017_22(List<Form18> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "22", "32" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        var repOkpo = okpoRepTerPodr is not ""
            ? okpoRepTerPodr
            : okpoRepJur;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = providerOrRecieverOkpo.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase)
                    || (okpoRegex.IsMatch(providerOrRecieverOkpo) && providerOrRecieverOkpo != repOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Для выбранного кода операции указывается код ОКПО контрагента, либо «Минобороны» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check018_01

    //При определенных кодах операции, код ОКПО перевозчика равен "-"
    private static List<CheckError> Check_018_01(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var transporterOkpo = ReplaceNullAndTrim(forms[line].TransporterOKPO_DB);
        string[] applicableOperationCodes =
        {
            "01", "10", "11", "12", "13", "18", "51", "52", "55", "68", "97", "98"
        };
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = transporterOkpo is "-";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = transporterOkpo,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "При выбранном коде операции транспортирование не производится, необходим символ «-» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check018_21

    //При определенных кодах операции, код ОКПО перевозчика равен 8/14 цифр
    private static List<CheckError> Check_018_21(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "21","25","26","27","28","29","31","35","36","37","38","39"
        };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var transporterOKPO = ReplaceNullAndTrim(forms[line].TransporterOKPO_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(transporterOKPO);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = transporterOKPO,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Проверьте правильность предоставленных сведений."
            });
        }
        return result;
    }

    #endregion

    #region Check018_22

    //Код ОКПО перевозчика состоит из 8/14 чисел или "минобороны", не ОКПО отчитывающейся
    private static List<CheckError> Check_018_22(List<Form18> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "22", "32" };
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        var repOkpo = okpoRepTerPodr is not ""
            ? okpoRepTerPodr
            : okpoRepJur;
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var transporterOkpo = ReplaceNullAndTrim(forms[line].TransporterOKPO_DB);
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = transporterOkpo.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase)
                    || (okpoRegex.IsMatch(transporterOkpo) && transporterOkpo != repOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = transporterOkpo,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Для выбранного кода операции указывается код ОКПО перевозчика, либо «Минобороны» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check019

    private static List<CheckError> Check_019(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var storagePlaceName = ReplaceNullAndTrim(forms[line].StoragePlaceName_DB);
        var valid = !string.IsNullOrWhiteSpace(storagePlaceName);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StoragePlaceName_DB",
                Value = storagePlaceName,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Графа 16 должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check020

    private static List<CheckError> Check_020(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var storagePlaceCode = ReplaceNullAndTrim(forms[line].StoragePlaceCode_DB);
        var valid = !string.IsNullOrWhiteSpace(storagePlaceCode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StoragePlaceCode_DB",
                Value = Convert.ToString(storagePlaceCode),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Графа 17 должна быть заполнена. Допускается прочерк."
            });
        }
        return result;
    }

    #endregion

    #region Check021

    private static List<CheckError> Check_021(List<Form18> forms, List<int> lines)
    {
        List<CheckError> result = new();
        var codeRaoRegex = new Regex(@"^\d{11}?$");
        var nuclidsRight =
            ((TryParseFloatExtended(forms[lines[0]].TransuraniumActivity_DB, out _) ? 1 : 0) << 0) +
            ((TryParseFloatExtended(forms[lines[0]].AlphaActivity_DB, out _) ? 1 : 0) << 1) +
            ((TryParseFloatExtended(forms[lines[0]].BetaGammaActivity_DB, out _) ? 1 : 0) << 2) +
            ((TryParseFloatExtended(forms[lines[0]].TritiumActivity_DB, out _) ? 1 : 0) << 3);
        var nuclidsLeft = 0;
        foreach (var nuclid in lines
                     .Where(line => R
                         .Any(phEntry => phEntry["name"] == forms[line].Radionuclids_DB.ToLower().Trim()))
                     .Select(line =>
                         R.First(phEntry => phEntry["name"] == forms[line].Radionuclids_DB.ToLower().Trim())))
        {
            switch (nuclid["code"])
            {
                case "а":
                    nuclidsLeft |= 0b0010;
                    break;
                case "б":
                    nuclidsLeft |= 0b0100;
                    break;
                case "т":
                    nuclidsLeft |= 0b1000;
                    break;
                case "у":
                    nuclidsLeft |= 0b0001;
                    break;
            }
        }
        foreach (var line in lines)
        {
            var codeRao = ReplaceNullAndTrim(forms[line].CodeRAO_DB);
            if (!codeRaoRegex.IsMatch(codeRao)) continue;

            #region 1-й символ кода РАО

            if (codeRao[..1] != "1")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = codeRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "В форме 1.8 приводятся сведения только о жидких кондиционированных РАО (1-й символ кода РАО должен быть равен 1)."
                });
            }

            #endregion

            #region 2-й символ кода РАО

            if (codeRao.Substring(1, 1) is "0")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = codeRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Для жидких РАО категория «очень низкоактивные» не устанавливается (2-й символ кода РАО не может быть равен 0)."
                });
            }
            else if (codeRao.Substring(1, 1) is "3")
            {
                var operationCode = forms[line].OperationCode_DB;
                var operationDate = forms[line].OperationDate_DB;
                var applicableOperationCodes = new List<string>(["11", "12", "13", "55"]);
                var valid = !string.IsNullOrWhiteSpace(operationCode)
                            && applicableOperationCodes.Contains(operationCode)
                            && DateOnly.TryParse(operationDate, out var opDateReal)
                            && opDateReal.Year >= 2024;
                if (!valid)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_18",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = codeRao,
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                  "К пятому классу могут быть отнесены только среднеактивные и низкоактивные ЖРО " +
                                  "(2-й символ кода РАО должен быть равен 1 или 2)."
                    });
                }
            }

            #endregion

            #region 3-й символ кода РАО

            if (codeRao.Substring(2, 1) is "0")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = codeRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Укажите 3-й символ кода РАО в соответствии с радионуклидным составом " +
                              "(3-й символ кода РАО не может быть равен 0)."
                });
            }

            #endregion

            #region 6-й символ кода РАО

            if (codeRao.Substring(5, 1) is "0")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = codeRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Для кондиционированных РАО должен быть определен период потенциальной опасности " +
                              "(6-й символ кода РАО не может быть равен 0)."
                });
            }

            #endregion

            #region 8-й символ кода РАО

            if (codeRao.Substring(7, 1) is not "5")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = codeRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Проверьте правильность выбранной формы для предоставления отчета " +
                              "(8-й символ кода РАО в форме 1.8 должен быть равен 5)."
                });
            }

            #endregion

            #region проверка кода РАО

            //see next check (Check021_RAOCODE)

            #endregion

        }
        return result;
    }

    #endregion

    #region Check021_RAOCODE

    private static List<CheckError> Check021_RAOCODE(List<Form18> forms, List<Note> notes, List<int> lines)
    {
        List<CheckError> result = new();
        var comparator = new CustomNullStringWithTrimComparer();

        #region data fetch

        var operationCode = ReplaceNullAndTrim(forms[lines[0]].OperationCode_DB);
        var radArray = lines
            .Where(line => R
                .Any(phEntry => phEntry["name"] == ReplaceNullAndTrim(forms[line].Radionuclids_DB).ToLower()))
            .Select(line => (ReplaceNullAndTrim(forms[line].Radionuclids_DB).ToLower(), 
                ReplaceNullAndTrim(forms[line].SpecificActivity_DB).ToLower()))
            .ToList();

        #region halflife

        var halfLifeMax = 0.0f;
        var halfLifeMaxId = -1;
        var validUnits = new Dictionary<string, float>()
            {
                { "лет", 1f },
                { "сут", 365.242374f },
                { "час", 365.242374f*24.0f },
                { "мин", 365.242374f*24.0f*60.0f },
                { "сек", 365.242374f*24.0f*60.0f*60.0f }
            };
        foreach (var nuclidId in radArray
                     .Select(nuclid => R
                         .FindIndex(x => comparator.Compare(x["name"], nuclid.Item1) == 0)))
        {
            if (nuclidId < 0
                || !TryParseFloatExtended(R[nuclidId]["value"], out var halfLifeVal)
                || !validUnits.TryGetValue(R[nuclidId]["unit"], out var value)) continue;
            halfLifeMax = Math.Max(halfLifeVal / value, halfLifeMax);
            halfLifeMaxId = nuclidId;
        }

        #endregion

        var codeRaoDBs = lines
            .Select(line => ReplaceNullAndTrim(forms[line].CodeRAO_DB))
            .Where(codeRaoDB => !string.IsNullOrWhiteSpace(codeRaoDB) 
                                && codeRaoDB != "-")
            .ToList();

        #endregion

        foreach (var line in lines)
        {
            var nuclidActivityT = ConvertStringToExponential(forms[line].TritiumActivity_DB);
            var nuclidActivityA = ConvertStringToExponential(forms[line].AlphaActivity_DB);
            var nuclidActivityB = ConvertStringToExponential(forms[line].BetaGammaActivity_DB);
            var nuclidActivityU = ConvertStringToExponential(forms[line].TransuraniumActivity_DB);
            var nuclidMassOutOfPack = ConvertStringToExponential(forms[line].Mass21_DB);
            var refineOrSortRaoCode = ReplaceNullAndTrim(forms[line].RefineOrSortRAOCode_DB);
            var nuclidsExistT = TryParseFloatExtended(nuclidActivityT, out var nuclidActivityTReal);
            var nuclidsExistA = TryParseFloatExtended(nuclidActivityA, out var nuclidActivityAReal);
            var nuclidsExistB = TryParseFloatExtended(nuclidActivityB, out var nuclidActivityBReal);
            var nuclidsExistU = TryParseFloatExtended(nuclidActivityU, out var nuclidActivityUReal);
            var nuclidMassExists = TryParseFloatExtended(nuclidMassOutOfPack, out var nuclidMassOutOfPackReal);
            const byte graphNumber = 18;
            var noteExists = CheckNotePresence(notes, line, graphNumber);
            var codeRao = ReplaceNullAndTrim(forms[line].CodeRAO_DB);
            if (codeRao is "" or "-") continue;
            var valid = codeRao.Length == 11 && codeRao.All(char.IsDigit);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = codeRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Проверьте правильность заполнения кода РАО."
                });
                continue;
            }

            #region setup

            var codeRao1MatterState = codeRao[..1];
            var codeRao2RaoCategory = codeRao.Substring(1, 1);
            var codeRao3NuclidTypes = codeRao.Substring(2, 1);
            var codeRao4HasNuclears = codeRao.Substring(3, 1);
            var codeRao5HalfLife = codeRao.Substring(4, 1);
            var codeRao6DangerPeriod = codeRao.Substring(5, 1);
            var codeRao7RecycleMethod = codeRao.Substring(6, 1);
            var codeRao8RaoClass = codeRao.Substring(7, 1);
            var codeRao910TypeCode = codeRao.Substring(8, 2);
            var codeRao11Flammability = codeRao.Substring(10, 1);
            var codeRao1Allowed = new[] { "1" };
            var codeRao2Allowed = new[] { "1", "2" };
            var codeRao3Allowed = new[] { "1", "2", "3", "4", "5", "6" };
            var codeRao4Allowed = new[] { "1", "2" };
            var codeRao5Allowed = new[] { "1", "2" };
            var codeRao6Allowed = new[] { "1", "2", "3" };
            var codeRao7Allowed = new[] { "0" };
            var codeRao8Allowed = new[] { "5" };
            var codeRao910Allowed = new[]
            {
                "01",
                "11","12","13","14","15","16","17","18","19",
                "21","22","23","24","25","26",          "29",
                "31","32","33","34","35","36","37","38","39",
                "41","42","43","44","45","46",
                "51","52","53","54","55","56","57","58","59",
                "61","62","63","64","65","66","67","68","69",
                "71","72","73","74","75","76","77","78","79",
                "81","82","83","84","85","86","87","88","89",
                "91","92","93","94","95","96","97","98","99"
            };
            var codeRao11Allowed = new[] { "1", "2" };

            var codeRao1Valid = codeRao1Allowed.Contains(codeRao1MatterState);
            var codeRao2Valid = codeRao2Allowed.Contains(codeRao2RaoCategory);
            var codeRao3Valid = codeRao3Allowed.Contains(codeRao3NuclidTypes);
            var codeRao4Valid = codeRao4Allowed.Contains(codeRao4HasNuclears);
            var codeRao5Valid = codeRao5Allowed.Contains(codeRao5HalfLife);
            var codeRao6Valid = codeRao6Allowed.Contains(codeRao6DangerPeriod);
            var codeRao7Valid = codeRao7Allowed.Contains(codeRao7RecycleMethod);
            var codeRao8Valid = codeRao8Allowed.Contains(codeRao8RaoClass);
            var codeRao910Valid = codeRao910Allowed.Contains(codeRao910TypeCode);
            var codeRao11Valid = codeRao11Allowed.Contains(codeRao11Flammability);

            var recyclingTypes = new[]
            {
                     "11","12","13","14","15","16","17","18","19",
                "20","21","22","23","24","25","26","27","28","29",
                "30","31","32","33","34","35","36","37","38","39"
            };

            #endregion

            #region symbol 1

            if (!codeRao1Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao1MatterState} (1-ый символ кода РАО), {codeRao910TypeCode} (9-10 символы кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "В форме 1.8 приводятся сведения только о жидких кондиционированных РАО (1-й символ кода РАО 1)."
                });
            }

            #endregion

            #region symbol 2

            if (!codeRao2Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "К пятому классу могут быть отнесены только среднеактивные и низкоактивные ЖРО."
                });
            }
            else if (codeRaoDBs.Count > 1)
            {

            }
            else switch (codeRao2RaoCategory)
            {
                case "4":
                {
                    var validTypeCode = new[] { "81", "82", "85", "86", "87", "88", "89" };
                    if (!validTypeCode.Contains(codeRao910TypeCode))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_18",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                            Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                      "Значение 2-го символа кода РАО 4 используется только для отработавших ЗРИ."
                        });
                    }
                    break;
                }
                case "9":
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_18",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                  "Для кондиционированных РАО должна быть определена категория."
                    });
                    break;
                }
                default:
                {
                    // 0, 1, 2, 3, 9
                    var codeMax = -1;
                    if (nuclidMassExists && nuclidMassOutOfPackReal > 0)
                    {
                        #region a for tritium

                        if (nuclidsExistT)
                        {
                            var a = nuclidActivityTReal / (nuclidMassOutOfPackReal * 1e6);
                            if (codeRao1MatterState == "1")
                            {
                                if (a < 1e04) { if (codeMax < 1) codeMax = 1; }
                                else if (a < 1e08) { if (codeMax < 2) codeMax = 2; }
                                else { if (codeMax < 3) codeMax = 3; }
                            }
                        }

                        #endregion

                        #region a for beta-gamma

                        if (nuclidsExistB)
                        {
                            var a = nuclidActivityBReal / (nuclidMassOutOfPackReal * 1e6);
                            if (codeRao1MatterState == "1")
                            {
                                if (a < 1e03) { if (codeMax < 1) codeMax = 1; }
                                else if (a < 1e07) { if (codeMax < 2) codeMax = 2; }
                                else { if (codeMax < 3) codeMax = 3; }
                            }
                        }

                        #endregion

                        #region a for alpha

                        if (nuclidsExistA)
                        {
                            var a = nuclidActivityAReal / (nuclidMassOutOfPackReal * 1e6);
                            if (codeRao1MatterState == "1")
                            {
                                if (a < 1e02) { if (codeMax < 1) codeMax = 1; }
                                else if (a < 1e06) { if (codeMax < 2) codeMax = 2; }
                                else { if (codeMax < 3) codeMax = 3; }
                            }
                        }

                        #endregion

                        #region a for transuraniums

                        if (nuclidsExistU)
                        {
                            var a = nuclidActivityUReal / (nuclidMassOutOfPackReal * 1e6);
                            if (codeRao1MatterState == "1")
                            {
                                if (a < 1e01) { if (codeMax < 1) codeMax = 1; }
                                else if (a < 1e05) { if (codeMax < 2) codeMax = 2; }
                                else { if (codeMax < 3) codeMax = 3; }
                            }
                        }

                        #endregion
                    }
                    if (codeMax == -1 && codeRao2RaoCategory != "9")
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_18",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                            Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                      "Проверьте категорию РАО и суммарную активность."
                        });
                    }
                    else if (codeMax != -1
                             && (codeRao2RaoCategory == "9"
                                 || codeRao2RaoCategory != codeMax.ToString("D1")))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_18",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                            Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                      $"По данным, представленным в строке {forms[line].NumberInOrder_DB}, категория РАО {codeMax}."
                        });
                    }
                    break;
                }
            }

            #endregion

            #region symbol 3

            if (!codeRao3Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao3NuclidTypes} (3-ий символ кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Недопустимое значение 3-го символа кода РАО."
                });
            }
            else
            {
                if (codeRao3NuclidTypes == "0")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_18",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao3NuclidTypes} (3-ий символ кода РАО)",
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                  "Укажите 3-й символ кода РАО в соответствии с радионуклидным составом."
                    });
                }
                else
                {
                    var containsB = nuclidsExistB
                                     && radArray
                                         .Any(x => R
                                             .Any(y => comparator.Compare(y["name"], x.Item1) == 0
                                                       && comparator.Compare(y["code"], "б") == 0));
                    var containsA = nuclidsExistA
                                    && radArray
                                        .Any(x => R
                                            .Any(y => comparator.Compare(y["name"], x.Item1) == 0
                                                      && comparator.Compare(y["code"], "а") == 0));
                    var containsU = nuclidsExistU
                                    && radArray
                                        .Any(x => R
                                            .Any(y => comparator.Compare(y["name"], x.Item1) == 0
                                                      && comparator.Compare(y["code"], "у") == 0));
                    var containsT = nuclidsExistT
                                    && radArray
                                        .Any(x => R
                                            .Any(y => comparator.Compare(y["name"], x.Item1) == 0
                                                      && comparator.Compare(y["code"], "т") == 0));
                    var expectedValue = "0";
                    if (!containsT && !containsB && !containsA && containsU) expectedValue = "1";
                    else if (!containsT && !containsB && containsA && !containsU) expectedValue = "2";
                    else if (!containsT && !containsB && containsA && containsU) expectedValue = "3";
                    else if ((containsT || containsB) && !containsA && !containsU) expectedValue = "4";
                    else if (!containsT && containsB && containsA && !containsU) expectedValue = "5";
                    else if (containsT && containsB && containsA && !containsU) expectedValue = "5";
                    else if (containsU) expectedValue = "6";
                    if (expectedValue != codeRao3NuclidTypes)
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_18",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao3NuclidTypes} (3-ий символ кода РАО)",
                            Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                      "Радионуклиды, указанные в графе 9, не соответствуют 3-му символу кода РАО."
                        });
                    }
                }

            }

            #endregion

            #region symbol 4

            if (!codeRao4Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao4HasNuclears} (4-ый символ кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Недопустимое значение 4-го символа кода РАО."
                });
            }
            else
            {
                var nuclears = new[]
                {
                    "плутоний", "уран-233", "уран-235", "уран-238", "нептуний-237", "америций-241",
                    "америций-243", "калифорний-252", "торий", "тритий"
                };
                var nuclearsExist = radArray
                    .Any(x => nuclears
                        .Any(y => x.Item1.Contains(y, StringComparison.CurrentCultureIgnoreCase)));
                if (codeRao4HasNuclears == "1")
                {
                    //anything is allowed
                }
                else if (codeRao4HasNuclears == "2")
                {
                    if (!nuclearsExist)
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_18",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao4HasNuclears} (4-ый символ кода РАО)",
                            Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                      "Не указаны радионуклиды, которые могут быть отнесены к ЯМ."
                        });
                    }
                }
            }

            #endregion

            #region symbol 5

            if (!codeRao5Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao5HalfLife} (5-ый символ кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Недопустимое значение 5-го символа кода РАО."
                });
            }
            else
            {
                if (codeRao5HalfLife != "2" && (long)halfLifeMax <= 31)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_18",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao5HalfLife} (5-ый символ кода РАО)",
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                  $"По данным, представленным в строке {forms[line].NumberInOrder_DB}, " +
                                  "5-ый символ кода РАО (период полураспада) должен быть равен 2."
                    });
                }
                else if (codeRao5HalfLife != "1" && (long)halfLifeMax > 31)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_18",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao5HalfLife} (5-ый символ кода РАО)",
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                  $"По данным, представленным в строке {forms[line].NumberInOrder_DB}, " +
                                  "5-ый символ кода РАО (период полураспада) должен быть равен 1."
                    });
                }
            }

            #endregion

            #region symbol 6

            if (!codeRao6Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao6DangerPeriod} (6-ой символ кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Недопустимое значение 6-го символа кода РАО."
                });
            }
            if (codeRao1MatterState is "2" && radArray.Count > 0)
            {
                var monoNuclid = radArray.Count == 1;
                var expectedPeriod = float.MaxValue;
                var expectedPeriodOutput = "";
                int expectedValue;
                if (monoNuclid)
                {
                    var nuclidData = R
                        .FirstOrDefault(x => comparator.Compare(x["name"], radArray[0].Item1) == 0, []);
                    if (nuclidData.Count > 0
                        && TryParseFloatExtended(radArray[0].Item2, out var nuclidActivity)
                        && TryParseFloatExtended(nuclidData["value"], out var T)
                        && validUnits.TryGetValue(nuclidData["unit"], out var unitAdjustment)
                        && nuclidActivity > 0.0f
                        && TryParseFloatExtended(nuclidData["OSPORB_Liquid"], out var a)
                        && a > 0)
                    {
                        expectedPeriod = T / unitAdjustment * (float)(Math.Log(nuclidActivity / a) / Math.Log(2));
                        expectedPeriodOutput = expectedPeriod.ToString();
                    }

                    expectedValue = expectedPeriod switch
                    {
                        > 500.0f => 3,
                        >= 100.0f => 2,
                        >= 0.0f => 1,
                        _ => 0
                    };
                }
                else
                {
                    var controlValue100 = 0.0f;
                    var controlValue500 = 0.0f;
                    foreach (var nuclid in radArray)
                    {
                        var nuclidData = R
                            .FirstOrDefault(x => comparator.Compare(x["name"], nuclid.Item1) == 0, []);

                        float a;
                        if (TryParseFloatExtended(nuclidData["OSPORB_Liquid"], out var osporbA) && osporbA > 0)
                        {
                            a = osporbA;
                        }
                        else if (TryParseFloatExtended(nuclidData["A_Liquid"], out var solidA) && solidA > 0)
                        {
                            a = solidA;
                        }
                        else return result;

                        if (nuclidData.Count > 0
                            && TryParseFloatExtended(nuclid.Item2, out var nuclidActivity)
                            && TryParseFloatExtended(nuclidData["value"], out var T)
                            && validUnits.TryGetValue(nuclidData["unit"], out var unitAdjustment)
                            && nuclidActivity > 0.0f)
                        {
                            controlValue100 += nuclidActivity / (float)Math.Pow(2, 100 / (T / unitAdjustment)) / a;
                            controlValue500 += nuclidActivity / (float)Math.Pow(2, 500 / (T / unitAdjustment)) / a;
                        }
                    }
                    if (controlValue100 < 1.0f)
                    {
                        expectedValue = 1;
                        expectedPeriodOutput = "менее 100";
                    }
                    else if (controlValue500 < 1.0f)
                    {
                        expectedValue = 2;
                        expectedPeriodOutput = "100-500";
                    }
                    else
                    {
                        expectedValue = 3;
                        expectedPeriodOutput = "более 500";
                    }
                }
                if (expectedValue.ToString("D1") != codeRao6DangerPeriod)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_18",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao6DangerPeriod} (6-ой символ кода РАО)",
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                  $"Расчетное значение периода потенциальной опасности (в годах): " +
                                  $"{expectedPeriodOutput} (6-ой символ кода РАО {expectedValue})."
                    });
                }
            }

            #endregion

            #region symbol 7

            if (!codeRao7Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao7RecycleMethod} (7-ой символ кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "К пятому классу относятся только жидкие РАО. " +
                              "Сведения об омоноличенных (отвержденных) РАО приводятся в формах 1.6 или 1.7"
                });
            }
            else if (forms[lines[0]].OperationCode_DB == "55")
            {
                Dictionary<string, string[]> validRecycles = new()
                {
                    { "0", new[]
                        {
                            "11","12","13","14","15","16","17","19",
                            "21","22","23","24","29",
                            "51","52","53","54","55",
                            "61",
                            "72","73","74","99"
                        }
                    }
                };
                valid = validRecycles.TryGetValue(codeRao7RecycleMethod, out var recycleMethods) && recycleMethods.Contains(refineOrSortRaoCode);
                if (!valid)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_18",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao7RecycleMethod} (7-ой символ кода РАО), {refineOrSortRaoCode} (код переработки/сортировки)",
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                                  "7-ой символ кода РАО не соответствует коду переработки/сортировки, указанному в графе 30."
                    });
                }
            }

            #endregion

            #region symbol 8

            if (!codeRao8Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao8RaoClass} (8-й символ кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Проверьте правильность выбранной формы отчета (в форме 1.8 8-й символ кода РАО должен быть равен 5)."
                });
            }

            #endregion

            #region symbols 9-10

            var requiresNote = new[] { "19", "29", "39", "99" };
            if (requiresNote.Contains(codeRao910TypeCode) && !noteExists)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao910TypeCode} (9-10 символы кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Необходимо заполнить примечание к коду типа РАО."
                });
            }

            #endregion

            #region symbol 11

            // -

            #endregion
        }

        return result;
    }

    #endregion

    #region Check022_11

    private static List<CheckError> Check_022_11(List<Form18> forms, List<Form10> forms10, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new[] { "11", "12", "13" };
        var operationCode = ReplaceNullAndTrim(forms[lines[0]].OperationCode_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var repOkpoList = forms10
            .Select(x => x.Okpo_DB)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        foreach (var line in lines)
        {
            var statusRaoDB = ReplaceNullAndTrim(forms[line].StatusRAO_DB);
            if (string.IsNullOrWhiteSpace(statusRaoDB) || statusRaoDB.Trim() == "-") continue;
            var valid = repOkpoList.Contains(statusRaoDB);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "StatusRAO_DB",
                    Value = statusRaoDB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "РАО, образовавшиеся после 15.07.2011, находятся в собственности организации, " +
                              "в результате деятельности которой они образовались."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check022_26
    private static List<CheckError> Check_022_26(List<Form18> forms, List<Form10> forms10, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new[] { "26", "28", "63" };
        var operationCode = ReplaceNullAndTrim(forms[lines[0]].OperationCode_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var repOkpoList = forms10.Select(x => x.Okpo_DB).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        foreach (var line in lines)
        {
            var statusRao = ReplaceNullAndTrim(forms[line].StatusRAO_DB);
            if (statusRao is "" or "-") continue;
            var valid = repOkpoList.Contains(statusRao);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "StatusRAO_DB",
                    Value = statusRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Операция, соответствующая выбранному коду, может использоваться только для собственных РАО."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check022_38

    private static List<CheckError> Check_022_38(List<Form18> forms, List<Form10> forms10, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new[] { "38", "64" };
        var operationCode = ReplaceNullAndTrim(forms[lines[0]].OperationCode_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var repOKPOList = forms10
            .Select(x => x.Okpo_DB)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        foreach (var line in lines)
        {
            var statusRao = ReplaceNullAndTrim(forms[line].StatusRAO_DB);
            if (statusRao is "" or "-") continue;
            var valid = repOKPOList.Contains(statusRao);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "StatusRAO_DB",
                    Value = statusRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "При операциях, связанных с получением права собственности, " +
                              "в графе статус РАО необходимо отразить код ОКПО отчитывающейся организации."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check022_42

    private static List<CheckError> Check_022_42(List<Form18> forms, List<Form10> forms10, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new[] { "42", "97", "98" };
        var operationCode = ReplaceNullAndTrim(forms[lines[0]].OperationCode_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var repOkpoList = forms10
            .Select(x => x.Okpo_DB)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        foreach (var line in lines)
        {
            var statusRao = ReplaceNullAndTrim(forms[line].StatusRAO_DB);
            if (statusRao is "" or "-") continue;
            var valid = repOkpoList.Contains(statusRao);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "StatusRAO_DB",
                    Value = statusRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Проверьте правильность статуса РАО."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check022_22

    private static List<CheckError> Check_022_22(List<Form18> forms, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new[] { "22", "32" };
        var applicableRaoStatuses = new[] { "1" };
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var operationCode = ReplaceNullAndTrim(forms[lines[0]].OperationCode_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        foreach (var line in lines)
        {
            var statusRao = ReplaceNullAndTrim(forms[line].StatusRAO_DB);
            if (statusRao is "" or "-") continue;
            var valid = okpoRegex.IsMatch(statusRao) || applicableRaoStatuses.Contains(statusRao);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "StatusRAO_DB",
                    Value = statusRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Проверьте правильность статуса РАО."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check022_16

    private static List<CheckError> Check_022_16(List<Form18> forms, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new[] { "16" };
        var applicableRaoStatuses = new[] { "2" };
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var operationCode = ReplaceNullAndTrim(forms[lines[0]].OperationCode_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        foreach (var line in lines)
        {
            var statusRao = ReplaceNullAndTrim(forms[line].StatusRAO_DB);
            if (statusRao is "" or "-") continue;
            var valid = okpoRegex.IsMatch(statusRao) || applicableRaoStatuses.Contains(statusRao);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "StatusRAO_DB",
                    Value = statusRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Проверьте правильность статуса РАО."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check022_10

    private static List<CheckError> Check_022_10(List<Form18> forms, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new[]
        {
            "01", "10", "18", "21", "25", "27", "29", "31", "35", "36", "37", "39", "51", "52", "55", "68"
        };
        var applicableRaoStatuses = new[] { "1", "2", "3", "4", "6", "9" };
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var operationCode = ReplaceNullAndTrim(forms[lines[0]].OperationCode_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        foreach (var line in lines)
        {
            var statusRao = ReplaceNullAndTrim(forms[line].StatusRAO_DB);
            if (statusRao is "" or "-") continue;
            var valid = okpoRegex.IsMatch(statusRao) || applicableRaoStatuses.Contains(statusRao);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "StatusRAO_DB",
                    Value = statusRao,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Заполнение графы 19 не соответствует требованиям приказа Госкорпорации \"Росатом\" от 07.12.2020 № 1/13-НПА."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check022_other

    private static List<CheckError> Check_022_other(List<Form18> forms, List<int> lines)
    {
        List<CheckError> result = new();
        return result;
    }

    #endregion

    #region Check023

    private static List<CheckError> Check_023(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var volumeOutOfPack = ConvertStringToExponential(forms[line].Volume20_DB);
        var valid = TryParseFloatExtended(volumeOutOfPack, out var volumeOutOfPackDBReal) 
                    && volumeOutOfPackDBReal > 0;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Volume20_DB",
                Value = volumeOutOfPack,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Необходимо заполнить сведения об объеме ЖРО."
            });
        }
        return result;
    }

    #endregion

    #region Check024

    private static List<CheckError> Check_024(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var volumeOutOfPack = ConvertStringToExponential(forms[line].Volume20_DB);
        var massOutOfPack = ConvertStringToExponential(forms[line].Mass21_DB);
        var volumeOutOfPackExists = TryParseFloatExtended(volumeOutOfPack, out var volumeOutOfPackDBReal);
        var massOutOfPackExists = TryParseFloatExtended(massOutOfPack, out var massOutOfPackDBReal);
        var valid = volumeOutOfPackExists && massOutOfPackExists;
        if (!valid || massOutOfPackDBReal == 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Mass21_DB",
                Value = massOutOfPack,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Необходимо заполнить сведения о массе ЖРО."
            });
        }
        else
        {
            if (massOutOfPackDBReal < 0.2f * volumeOutOfPackDBReal)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Volume20_DB",
                    Value = volumeOutOfPack,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Проверьте значение массы и объема. Расчетное значение плотности слишком маленькое."
                });
            }
            else if (massOutOfPackDBReal > 2f * volumeOutOfPackDBReal)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_18",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Volume20_DB",
                    Value = volumeOutOfPack,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Проверьте значение массы и объема. Расчетное значение плотности слишком большое."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check025_027

    private static List<CheckError> Check_025_027(List<Form18> forms, List<int> lines)
    {
        List<CheckError> result = new();
        List<string> errorColumns = new();
        List<string> nuclids = new();
        foreach (var nuclid in lines
                     .Where(line => R
                         .Any(phEntry => phEntry["name"] == ReplaceNullAndTrim(forms[line].Radionuclids_DB).ToLower()))
                     .Select(line => R
                         .First(phEntry => phEntry["name"] == ReplaceNullAndTrim(forms[line].Radionuclids_DB).ToLower())))
        {
            if (!nuclids.Contains(nuclid["name"])) nuclids.Add(nuclid["name"]);
            var alphaActivity = ConvertStringToExponential(forms[lines[0]].AlphaActivity_DB);
            var betaGammaActivity = ConvertStringToExponential(forms[lines[0]].BetaGammaActivity_DB);
            var tritiumActivity = ConvertStringToExponential(forms[lines[0]].TritiumActivity_DB);
            var transuraniumActivity = ConvertStringToExponential(forms[lines[0]].TransuraniumActivity_DB);
            switch (nuclid["code"])
            {
                case "а":
                    if (!TryParseFloatExtended(alphaActivity, out _))
                    {
                        if (!errorColumns.Contains("24")) errorColumns.Add("24");
                    }
                    break;
                case "б":
                    if (!TryParseFloatExtended(betaGammaActivity, out _))
                    {
                        if (!errorColumns.Contains("23")) errorColumns.Add("23");
                    }
                    break;
                case "т":
                    if (!TryParseFloatExtended(tritiumActivity, out _))
                    {
                        if (!errorColumns.Contains("22")) errorColumns.Add("22");
                    }
                    break;
                case "у":
                    if (!TryParseFloatExtended(transuraniumActivity, out _))
                    {
                        if (!errorColumns.Contains("25")) errorColumns.Add("25");
                    }
                    break;
            }
        }
        if (errorColumns.Count > 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[lines[0]].NumberInOrder_DB.ToString(),
                Column = "Radionuclids_DB",
                Value = string.Join("; ", nuclids),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Для указанного в графе 9 состава радионуклидов должна быть приведена активность " +
                          $"в граф{(errorColumns.Count > 1 ? "ах" : "е")} {string.Join(", ", errorColumns)}."
            });
        }
        return result;
    }

    #endregion

    #region Check029_55

    private static List<CheckError> Check_029_55(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var refineOrSortRaoCode = ReplaceNullAndTrim(forms[line].RefineOrSortRAOCode_DB);
        if (opCode != "55") return result;
        List<string> applicableRefineOrSortRaoCode = new() 
        {
            "11","12","13","14","15","16","17",     "19",
            "21","22","23","24",                    "29",
            "51","52","53","54","55",
            "61",
                 "72","73","74",
            "99","-"
        };
        if (!applicableRefineOrSortRaoCode.Contains(refineOrSortRaoCode))
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = refineOrSortRaoCode,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Заполните сведения о коде переработки/сортировки. В случае, " +
                          "если при кондиционировании не использовались установки переработки, укажите символ «-» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check029_10

    private static List<CheckError> Check_029_10(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var refineOrSortRaoCode = ReplaceNullAndTrim(forms[line].RefineOrSortRAOCode_DB);
        List<string> applicableOperationCodes = new()
        {
            "01","10","11","12","13","18",
            "21","25","26","27","28","29",
            "31","32","35","36","37","38","39",
            "42","51","52","63","64","68","97","98"
        };
        if (!applicableOperationCodes.Contains(opCode)) return result;
        if (refineOrSortRaoCode != "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = refineOrSortRaoCode,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Для выбранного кода операции в графе 26 следует использовать символ «-» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check030

    private static List<CheckError> Check_030(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var subsidy = ReplaceNullAndTrim(forms[line].Subsidy_DB);
        if (subsidy is "" or "-") return result;
        var valid = TryParseFloatExtended(subsidy, out var valueReal)
                    && valueReal is >= 0 and <= 100;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Subsidy_DB",
                Value = subsidy,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Проверьте значение субсидии."
            });
        }
        return result;
    }

    #endregion

    #region Check031

    private static List<CheckError> Check_031(List<Form18> forms, int line)
    {
        List<CheckError> result = new();
        var fcpNum = ReplaceNullAndTrim(forms[line].FcpNumber_DB);
        var valid = fcpNum is "" or "-" || TryParseFloatExtended(fcpNum, out _);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "FcpNumber_DB",
                Value = fcpNum,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Графу 28 следует либо не заполнять, либо указать числовое значение или прочерк."
            });
        }
        return result;
    }

    #endregion

    #region Check032

    //overdue calculations
    private static List<CheckError> Check_032(List<Form18> forms, Report rep)
    {
        List<CheckError> result = new();
        List<string> overdueSetLines = new();
        var endPeriod = ReplaceNullAndTrim(rep.EndPeriod_DB);
        if (DateOnly.TryParse(endPeriod, out var dateEnd))
        {
            for (var i = 0; i < forms.Count; i++)
            {
                var operationCode = ReplaceNullAndTrim(forms[i].OperationCode_DB);
                var operationDate = ReplaceNullAndTrim(forms[i].OperationDate_DB);
                var documentDate = ReplaceNullAndTrim(forms[i].DocumentDate_DB);
                if ((operationCode == "10" ? documentDate : operationDate) != null
                    && OverduePeriods_RAO.TryGetValue(operationCode, out var days)
                    && DateOnly.TryParse(operationCode == "10"
                        ? documentDate
                        : operationDate, out var dateMid))
                {
                    if (WorkdaysBetweenDates(dateMid, dateEnd) > days)
                    {
                        //overdueSet.Add($"Операция {operationCode} за {date_mid} просрочена на {WorkdaysBetweenDates(date_mid, date_end) - days} дней.");
                        overdueSetLines.Add((i + 1).ToString());
                    }
                }
            }
        }
        if (overdueSetLines.Count > 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = string.Join(", ", overdueSetLines),
                Column = "-",
                Value = "",
                Message = $"Указанные операции просрочены."
            });
        }
        return result;
    }

    #endregion

    #region Check_Criteria

    private static List<CheckError> Check_Criteria(List<Form18> forms, List<int> lines)
    {
        List<CheckError> result = new();
        List<string> activityString = new();
        var criteriaSum = 0.0f;
        var radArray = lines
            .Where(line => R
                .Any(phEntry => phEntry["name"] == ReplaceNullAndTrim(forms[line].Radionuclids_DB).ToLower()))
            .Select(line => (ReplaceNullAndTrim(forms[line].Radionuclids_DB).ToLower(), 
                ReplaceNullAndTrim(forms[line].SpecificActivity_DB).ToLower()))
            .ToList();
        foreach (var nuclid in radArray)
        {
            var nuclidData = R.First(phEntry => phEntry["name"] == nuclid.Item1);
            if (TryParseFloatExtended(nuclid.Item2, out var activityUp)
                && TryParseFloatExtended(nuclidData["A_Liquid"], out var activityDown)
                && activityDown != 0.0f)
            {
                criteriaSum += activityUp / activityDown;
                activityString.Add(nuclid.Item2);
            }
        }
        if (criteriaSum <= 1)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = forms[lines[0]].NumberInOrder_DB.ToString(),
                Column = "SpecificActivity_DB",
                Value = string.Join(", ", activityString),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                          "Отходы ниже уровня отнесения к РАО."
            });
        }
        return result;
    }

    #endregion

    #endregion
}