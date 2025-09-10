using System;
using System.Collections.Generic;
using System.Linq;
using Models.CheckForm;
using Models.Collections;
using Models.Forms.Form1;
using Note = Models.Forms.Note;

namespace Client_App.Commands.AsyncCommands.CheckForm;

public abstract class CheckF15 : CheckBase
{
    #region CheckTotal

    public static List<CheckError> Check_Total(Reports reps, Report rep)
    {
        var currentFormLine = 0;
        List<CheckError> errorList = new();
        LoadDictionaries();
        var formsList = rep.Rows15.ToList<Form15>();
        errorList.AddRange(Check_001(rep));
        errorList.AddRange(Check_002(formsList, rep));
        errorList.AddRange(Check_003(formsList));
        foreach (var key in rep.Rows15)
        {
            var form = (Form15)key;
            var notes = rep.Notes.ToList<Note>();
            var forms10 = reps.Master_DB.Rows10.ToList<Form10>();
            errorList.AddRange(Check_004(formsList, currentFormLine));
            errorList.AddRange(Check_005(formsList, currentFormLine));
            errorList.AddRange(Check_005_10(formsList, currentFormLine));
            errorList.AddRange(Check_005_29(formsList, notes, currentFormLine));
            errorList.AddRange(Check_005_21(formsList, currentFormLine));
            errorList.AddRange(Check_005_37(formsList, currentFormLine));
            errorList.AddRange(Check_005_41(formsList, currentFormLine));
            errorList.AddRange(Check_005_51(formsList, currentFormLine));
            errorList.AddRange(Check_005_52(formsList, currentFormLine));
            errorList.AddRange(Check_005_57(formsList, currentFormLine));
            errorList.AddRange(Check_005_84(formsList, currentFormLine));
            errorList.AddRange(Check_005_71(formsList, currentFormLine));
            errorList.AddRange(Check_006(formsList, rep, currentFormLine));
            errorList.AddRange(Check_006_03(formsList, rep, currentFormLine));
            errorList.AddRange(Check_007(formsList, currentFormLine));
            errorList.AddRange(Check_008(formsList, currentFormLine));
            errorList.AddRange(Check_009_a(formsList, currentFormLine));
            errorList.AddRange(Check_009_b(formsList, currentFormLine));
            errorList.AddRange(Check_010(formsList, currentFormLine));
            errorList.AddRange(Check_011(formsList, currentFormLine));
            errorList.AddRange(Check_012(formsList, currentFormLine));
            errorList.AddRange(Check_013(formsList, currentFormLine));
            errorList.AddRange(Check_014(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_015(formsList, notes, currentFormLine));
            errorList.AddRange(Check_016(formsList, currentFormLine));
            errorList.AddRange(Check_017_a(formsList, currentFormLine));
            errorList.AddRange(Check_017_b(formsList, currentFormLine));
            errorList.AddRange(Check_017_c(formsList, currentFormLine));
            errorList.AddRange(Check_017_d(formsList, rep, currentFormLine));
            errorList.AddRange(Check_018_01(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_018_21(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_018_22(formsList, currentFormLine));
            errorList.AddRange(Check_018_84(formsList, notes, currentFormLine));
            errorList.AddRange(Check_019_01(formsList, currentFormLine));
            errorList.AddRange(Check_019_21(formsList, currentFormLine));
            errorList.AddRange(Check_019_22(formsList, currentFormLine));
            errorList.AddRange(Check_020(formsList, currentFormLine));
            errorList.AddRange(Check_021(formsList, currentFormLine));
            errorList.AddRange(Check_022(formsList, currentFormLine));
            errorList.AddRange(Check_023(formsList, currentFormLine));
            errorList.AddRange(Check_024(formsList, currentFormLine));
            errorList.AddRange(Check_025(formsList, notes, currentFormLine));
            errorList.AddRange(Check_026(formsList, currentFormLine));
            errorList.AddRange(Check_027(formsList, currentFormLine));

            currentFormLine++;
        }
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

    #region Check001

    //Проверка правильности заполнения дат периода
    private static List<CheckError> Check_001(Report rep)
    {
        List<CheckError> result = new();
        var stPerStr = ReplaceNullAndTrim(rep.StartPeriod_DB);
        var endPerStr = ReplaceNullAndTrim(rep.EndPeriod_DB);
        if (!DateOnly.TryParse(stPerStr, out _))
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = string.Empty,
                Column = string.Empty,
                Value = stPerStr,
                Message = "Некорректно введена дата начала периода."
            });
        }
        if (!DateOnly.TryParse(endPerStr, out _))
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = string.Empty,
                Column = string.Empty,
                Value = endPerStr,
                Message = "Некорректно введена дата окончания периода."
            });
        }
        if (DateOnly.TryParse(stPerStr, out var stPer)
            && DateOnly.TryParse(endPerStr, out var endPer)
            && stPer > endPer)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = string.Empty,
                Column = string.Empty,
                Value = $"{stPerStr} - {endPerStr}",
                Message = "Дата начала периода превышает дату окончания периода."
            });
        }
        return result;
    }

    #endregion

    #region Check002

    //Проверка даты окончания периода
    private static List<CheckError> Check_002(List<Form15> forms15, Report rep)
    {
        var forms = forms15.Cast<Form1>().ToList();
        return CheckRepPeriod(forms, rep);
    }

    #endregion

    #region Check003

    //Наличие строк дубликатов (графы 2-24)
    private static List<CheckError> Check_003(List<Form15> forms)
    {
        List<CheckError> result = new();
        HashSet<int> duplicatesLinesSet = new();
        List<HashSet<int>> duplicatesGroupsSet = new();
        var comparator = new CustomNullStringWithTrimComparer();
        for (var i = 0; i < forms.Count; i++)
        {
            if (duplicatesGroupsSet.Any(set => set.Contains(i + 1))) continue;
            var currentForm = forms[i];
            var hasDuplicate = false;
            for (var j = i + 1; j < forms.Count; j++)
            {
                if (duplicatesGroupsSet.Any(set => set.Contains(j + 1))) continue;
                var formToCompare = forms[j];
                var isDuplicate = comparator.Compare(formToCompare.OperationCode_DB, currentForm.OperationCode_DB) == 0
                                  && comparator.Compare(formToCompare.OperationDate_DB, currentForm.OperationDate_DB) == 0
                                  && comparator.Compare(formToCompare.PassportNumber_DB, currentForm.PassportNumber_DB) == 0
                                  && comparator.Compare(formToCompare.Type_DB, currentForm.Type_DB) == 0
                                  && comparator.Compare(formToCompare.Radionuclids_DB, currentForm.Radionuclids_DB) == 0
                                  && comparator.Compare(formToCompare.FactoryNumber_DB, currentForm.FactoryNumber_DB) == 0
                                  && formToCompare.Quantity_DB == currentForm.Quantity_DB
                                  && comparator.Compare(formToCompare.Activity_DB, currentForm.Activity_DB) == 0
                                  && comparator.Compare(formToCompare.CreationDate_DB, currentForm.CreationDate_DB) == 0
                                  && comparator.Compare(formToCompare.StatusRAO_DB, currentForm.StatusRAO_DB) == 0
                                  && formToCompare.DocumentVid_DB == currentForm.DocumentVid_DB
                                  && comparator.Compare(formToCompare.DocumentNumber_DB, currentForm.DocumentNumber_DB) == 0
                                  && comparator.Compare(formToCompare.DocumentDate_DB, currentForm.DocumentDate_DB) == 0
                                  && comparator.Compare(formToCompare.ProviderOrRecieverOKPO_DB, currentForm.ProviderOrRecieverOKPO_DB) == 0
                                  && comparator.Compare(formToCompare.TransporterOKPO_DB, currentForm.TransporterOKPO_DB) == 0
                                  && comparator.Compare(formToCompare.PackName_DB, currentForm.PackName_DB) == 0
                                  && comparator.Compare(formToCompare.PackType_DB, currentForm.PackType_DB) == 0
                                  && comparator.Compare(formToCompare.PackNumber_DB, currentForm.PackNumber_DB) == 0
                                  && comparator.Compare(formToCompare.StoragePlaceName_DB, currentForm.StoragePlaceName_DB) == 0
                                  && comparator.Compare(formToCompare.StoragePlaceCode_DB, currentForm.StoragePlaceCode_DB) == 0
                                  && comparator.Compare(formToCompare.RefineOrSortRAOCode_DB, currentForm.RefineOrSortRAOCode_DB) == 0
                                  && comparator.Compare(formToCompare.Subsidy_DB, currentForm.Subsidy_DB) == 0
                                  && comparator.Compare(formToCompare.FcpNumber_DB, currentForm.FcpNumber_DB) == 0;
                if (!isDuplicate) continue;
                hasDuplicate = true;
                duplicatesLinesSet.Add(j + 1);
            }
            if (hasDuplicate) duplicatesLinesSet.Add(i + 1);
            if (duplicatesLinesSet.Count > 0)
            {
                duplicatesGroupsSet.Add(duplicatesLinesSet.Order().ToHashSet());
            }
            duplicatesLinesSet.Clear();
        }
        if (duplicatesGroupsSet.Count > 0)
        {
            foreach (var group in duplicatesGroupsSet)
            {
                var dupStrByGroups = ConvertSequenceSetToRangeString(group);
                result.Add(new CheckError
                {
                    FormNum = "form_15",
                    Row = dupStrByGroups,
                    Column = "2 - 24",
                    Value = "",
                    Message = $"Данные граф 2-24 в строках {dupStrByGroups} продублированы. " +
                              $"Следует проверить правильность предоставления данных."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check004

    //Нумерация строк (графа 1)
    private static List<CheckError> Check_004(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        if (forms[line].NumberInOrder_DB != line + 1)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "NumberInOrder_DB",
                Value = forms[line].NumberInOrder_DB.ToString(),
                Message = "Номера строк должны располагаться по порядку, без пропусков или дублирования номеров. " +
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

    //Допустимый код операции (графа 2)
    private static List<CheckError> Check_005(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var opCodeValid = new []
        {
            "01","10","14","21","22","25","26","27","28","29","31","32","35","36","37",
            "38","39","41","43","44","45","49","51","52","57","59","63","64","71","72",
            "73","74","75","76","84","88","97","98","99"
        };
        var valid = opCodeValid.Contains(opCode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = opCode,
                Message = "Код операции не может быть использован в форме 1.5."
            });
        }
        return result;
    }

    #endregion

    #region Check005_10

    //TODO
    //Проверка на соответствие СНК
    private static List<CheckError> Check_005_10(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOperationCodes = new[] { "10" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "Сведения, представленные в инвентаризации, не соответствуют СНК."
            });
        }
        return result;
    }

    #endregion
    
    #region Check005_29

    //Проверка наличия примечания (графа 2)
    private static List<CheckError> Check_005_29(List<Form15> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOpCodes = new[] { "29","39","49","59","97","98","99" };
        if (!applicableOpCodes.Contains(opCode)) return result;
        const byte graphNumber = 2;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = opCode,
                Message = "В примечании к ячейке \"Код операции\" необходимо дать пояснение об осуществлённой операции.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check005_21

    //TODO
    //Проверка наличия учётной единицы в организации
    private static List<CheckError> Check_005_21(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOpCodes = new[]
        {
            "21","22","25","26","27","28","29","42","43","44","45","49","51","71","72","84","98"
        };
        if (!applicableOpCodes.Contains(opCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = opCode,
                Message = "Учетной единицы с такими параметрами нет в организации. " +
                          "Проверьте правильность указываемых сведений для ОЗРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check005_37

    //TODO
    //Проверка наличия кода операции 27 ранее
    private static List<CheckError> Check_005_37(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOperationCodes = new[] { "37" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "В отчетах не найдена строка об осуществлении передачи учетной единицы. " +
                          "Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check005_41

    //TODO
    //Эта проверка по идее никогда не выдаст ошибку, т.к. проверке все равно, появился ли 41 код вручную или автоматически.
    private static List<CheckError> Check_005_41(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOperationCodes = new[] { "41" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = ""
            });
        }
        return result;
    }

    #endregion

    #region Check005_51

    //TODO
    //Проверка наличия кода операции 52
    private static List<CheckError> Check_005_51(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOperationCodes = new[] { "51" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = ""
            });
        }
        return result;
    }

    #endregion

    #region Check005_52

    //TODO
    //Проверка наличия кода операции 51
    private static List<CheckError> Check_005_52(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOperationCodes = new[] { "52" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "В отчетах не найдена строка об изъятии РАО из пункта хранения. " +
                          "Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check005_57

    //TODO
    //Проверка кода операции 45
    private static List<CheckError> Check_005_57(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOperationCodes = new[] { "57" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "В отчетах не найдена строка снятии учетной единицы для упаковки/переупаковки. " +
                          "Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check005_84

    //Статус РАО равен 7 (графа 11)
    private static List<CheckError> Check_005_84(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var statusRao = ReplaceNullAndTrim(forms[line].StatusRAO_DB);
        var applicableOperationCodes = new[] { "84" };
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = statusRao == "7";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = opCode,
                Message = "Для ОЗРИ, которые возвращаются в страну поставщика код статус РАО – 7.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check005_71

    //Справочная "ошибка" - т.е. даже не ошибка.
    private static List<CheckError> Check_005_71(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOpCodes = new[] { "71", "72", "73", "74", "75", "76" };
        if (!applicableOpCodes.Contains(opCode)) return result;
        var valid = false;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = opCode,
                Message = "К отчету необходимо приложить скан-копию документа, характеризующего операцию."
            });
        }
        return result;
    }

    #endregion

    #region Check006

    //Дата операции входит в отчетный период (колонка 3)
    private static List<CheckError> Check_006(List<Form15> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] nonApplicableOperationCodes = { "01" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var opDateStr = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        var stPerStr = ReplaceNullAndTrim(rep.StartPeriod_DB);
        var endPerStr = ReplaceNullAndTrim(rep.EndPeriod_DB);
        if (nonApplicableOperationCodes.Contains(opCode)
            || !DateOnly.TryParse(stPerStr, out var pStart)
            || !DateOnly.TryParse(endPerStr, out var pEnd)
            || !DateOnly.TryParse(opDateStr, out var opDate))
        {
            return result;
        }
        var valid = opDate >= pStart && opDate <= pEnd;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = opDateStr,
                Message = "Дата операции не входит в отчетный период.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check006_03

    //Дата документа входит в отчетный период (колонка 3)
    private static List<CheckError> Check_006_03(List<Form15> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        var docDateStr = ReplaceNullAndTrim(forms[line].DocumentDate_DB);
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var opDateStr = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        var stPerStr = ReplaceNullAndTrim(rep.StartPeriod_DB);
        var endPerStr = ReplaceNullAndTrim(rep.EndPeriod_DB);
        if (opCode is not "10"
            || !DateOnly.TryParse(docDateStr, out var docDate)
            || !DateOnly.TryParse(stPerStr, out var stPer)
            || !DateOnly.TryParse(opDateStr, out var opDate)
            || !DateOnly.TryParse(endPerStr, out var dateEndReal))
        {
            return result;
        }
        if (opDate > docDate)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDateStr,
                Message = "Для операции инвентаризации дата операции не может превышать даты утверждения акта инвентаризации."
            });
        }
        var valid = docDate >= stPer && docDate <= dateEndReal;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDateStr,
                Message = "Дата документа не входит в отчётный период. Для операции инвентаризации, " +
                          "срок предоставления отчёта исчисляется с даты утверждения акта инвентаризации."
            });
        }
        return result;
    }

    #endregion

    #region Check007

    //Номер паспорта заполнен (графа 4)
    private static List<CheckError> Check_007(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var passportNumber = ReplaceNullAndTrim(forms[line].PassportNumber_DB);
        var valid = passportNumber != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "PassportNumber_DB",
                Value = passportNumber,
                Message = "Заполните сведения о номере паспорта (сертификата) ЗРИ, который переведен в ОЗИИИ, или поставьте прочерк.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check008

    //Тип заполнен (графа 5)
    private static List<CheckError> Check_008(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var type = ReplaceNullAndTrim(forms[line].Type_DB);
        var valid = type != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Type_DB",
                Value = type,
                Message = "Заполните сведения о типе ЗРИ, который переведен в ОЗИИИ, или поставьте прочерк.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check009_a

    //Радионуклиды заполнены (графа 6)
    private static List<CheckError> Check_009_a(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var radionuclids = ReplaceNullAndTrim(forms[line].Radionuclids_DB);
        var valid = radionuclids != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = radionuclids,
                Message = "Графа должна быть заполнена.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check009_b

    //Все радионуклиды есть в справочнике (графа 6)
    private static List<CheckError> Check_009_b(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var rads = ReplaceNullAndTrim(forms[line].Radionuclids_DB);
        if (rads is not ("" or "-")) return result;
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();
        if (radsSet.Count == 1 && R.All(phEntry => phEntry["name"] != radsSet.First()))
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = rads,
                Message = "Формат ввода данных не соответствует приказу. " +
                          "Наименование радионуклида указывается названием химического элемента на русском языке " +
                          "с указанием через дефис массового числа изотопа.",
                IsCritical = true
            });
            return result;
        }
        if (rads.Contains(',')
            || rads.Contains('+')
            || !radsSet
                .All(rad => R
                    .Any(phEntry => phEntry["name"] == rad)))
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = rads,
                Message = "Формат ввода данных не соответствует приказу. " +
                          "Наименование радионуклида указывается названием химического элемента на русском языке " +
                          "с указанием через дефис массового числа изотопа, радионуклиды перечисляются через точку с запятой.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check010_a

    //У номеров в качестве разделителя использовать ";"
    private static List<CheckError> Check_010(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var factoryNum = ReplaceNullAndTrim(forms[line].FactoryNumber_DB);

        if (factoryNum is "-") return result;
        var quantity = forms[line].Quantity_DB ?? 0;

        if (string.IsNullOrWhiteSpace(factoryNum))
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "FactoryNumber_DB",
                Value = factoryNum,
                Message = "Заполните сведения о заводском номере ЗРИ, который переведен в ОЗИИИ. Если номер отсутствует, " +
                          "в ячейке следует указать символ \"-\" без кавычек.",
                IsCritical = true
            });
        }
        if (factoryNum.Contains(','))
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "FactoryNumber_DB",
                Value = factoryNum,
                Message = "Заводской номер не должен содержать запятых. Для упаковки однотипных ЗРИ, имеющей один паспорт (сертификат), " +
                          "заводские номера в списке необходимо разделять точкой с запятой " +
                          "(при перечислении номеров использование тире также недопустимо).",
                IsCritical = true
            });
        }
        if (quantity == 1 && factoryNum.Contains(';'))
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "FactoryNumber_DB",
                Value = factoryNum,
                Message = "При указании в графе «Количество» 1 шт., заводской номер не должен содержать символа точка с запятой " +
                          "(используется для перечисления).",
                IsCritical = true
            });
        }
        if (quantity > 1 && !factoryNum.Contains(';'))
        {
            {
                result.Add(new CheckError
                {
                    FormNum = "form_15",
                    Row = (line + 1).ToString(),
                    Column = "FactoryNumber_DB",
                    Value = factoryNum,
                    Message = "Для упаковки однотипных ЗРИ, имеющей один паспорт (сертификат), " +
                              "заводские номера в списке разделяются точкой с запятой.",
                    IsCritical = true
                });
            }
        }

        return result;
    }

    #endregion

    #region Check011

    //Количество заполнено (графа 8)
    private static List<CheckError> Check_011(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var quantity = forms[line].Quantity_DB ?? 0;
        if (quantity <= 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Quantity_DB",
                Value = Convert.ToString(quantity),
                Message = "Заполните сведения о количестве ЗРИ, переведенных в ОЗИИИ.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check012

    //Проверка на корректные данные суммарной активности (графа 9)
    private static List<CheckError> Check_012(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var activity = ConvertStringToExponential(forms[line].Activity_DB);
        if (!TryParseDoubleExtended(activity, out var activityReal))
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Заполните сведения о суммарной активности ЗРИ, переведенных в ОЗИИИ. " +
                          "Оценочные сведения приводятся в круглых скобках."
            });
        }
        else switch (activityReal)
        {
            case < 10:
            {
                result.Add(new CheckError
                {
                    FormNum = "form_15",
                    Row = (line + 1).ToString(),
                    Column = "Activity_DB",
                    Value = activity,
                    Message = "Суммарная активность должна быть более 10 Бк. Проверьте правильность введённых данных."
                });
                break;
            }
            case > 10e+20:
            {
                result.Add(new CheckError
                {
                    FormNum = "form_15",
                    Row = (line + 1).ToString(),
                    Column = "Activity_DB",
                    Value = activity,
                    Message = "Указано слишком большое значение суммарной активности. Проверьте правильность введённых данных."
                });
                break;
            }
        }
        return result;
    }

    #endregion

    #region Check013

    //Дата выпуска <= дате операции
    private static List<CheckError> Check_013(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationDate = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        var creationDate = ReplaceNullAndTrim(forms[line].CreationDate_DB);
        if (!DateOnly.TryParse(creationDate, out var creationDateReal) && creationDate != "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "CreationDate_DB",
                Value = creationDate,
                Message = "Дата выпуска заполнена некорректно."
            });
            return result;
        }
        if (!DateOnly.TryParse(operationDate, out var operationDateReal))
        {
            return result;
        }
        var valid = creationDateReal <= operationDateReal;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "CreationDate_DB",
                Value = creationDate,
                Message = "Дата выпуска не может быть позже даты операции."
            });
        }
        return result;
    }

    #endregion

    #region Check014

    //Проверка соответствия кода операции и статуса РАО (графа 11)
    private static List<CheckError> Check_014(List<Form15> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var status = ReplaceNullAndTrim(forms[line].StatusRAO_DB);
        var jurOkpo = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var obOkpo = ReplaceNullAndTrim(forms10[1].Okpo_DB);

        switch (opCode)
        {
            case "11" or "13" or "41":
            {
                if (status != obOkpo && status != jurOkpo)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = status,
                        Message = "РАО, образовавшиеся после 15.07.2011, находятся в собственности организации, " +
                                  "в результате деятельности которой они образовались.",
                        IsCritical = true
                    });
                }
                break;
            }
            case "12":
            {
                if (status != obOkpo && status != jurOkpo && status is not "2")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = status,
                        Message = "Проверьте правильность статуса РАО.",
                        IsCritical = true
                    });
                }
                break;
            }
            case "14":
            {
                if (status != obOkpo && status != jurOkpo && status is not ("2" or "3" or "4"))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = status,
                        Message = "Проверьте правильность статуса РАО.",
                        IsCritical = true
                    });
                }
                break;
            }
            case "26":
            {
                if (!OkpoRegex.IsMatch(status) && status is not ("1" or "2" or "3" or "4" or "6"))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = status,
                        Message = "Проверьте правильность статуса РАО.",
                        IsCritical = true
                    });
                }
                break;
            }
            case "28" or "63":
            {
                if (status != obOkpo && status != jurOkpo)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = status,
                        Message = "Операция, соответствующая выбранному коду, может использоваться только для собственных РАО.",
                        IsCritical = true
                    });
                }
                break;
            }
            case "38" or "64":
            {
                if (status != obOkpo && status != jurOkpo)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = status,
                        Message = "При операциях, связанных с получением права собственности, " +
                                  "в графе статус РАО необходимо отразить код ОКПО отчитывающейся организации.",
                        IsCritical = true
                    });
                }
                break;
            }
            case "42" or "43" or "73":
            {
                if (status != obOkpo && status != jurOkpo)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = status,
                        Message = "Проверьте правильность статуса РАО.",
                        IsCritical = true
                    });
                }
                break;
            }
            case "22" or "32":
            {
                if (!OkpoRegex.IsMatch(status) && status is not ("1" or "2"))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = status,
                        Message = "Проверьте правильность статуса РАО.",
                        IsCritical = true
                    });
                }
                break;
            }
            case "16":
            {
                if (!OkpoRegex.IsMatch(status) && status != "2")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = status,
                        Message = "Проверьте правильность статуса РАО.",
                        IsCritical = true
                    });
                }
                break;
            }
            case "76":
            {
                if (status is not ("6" or "9"))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = status,
                        Message = "Проверьте правильность статуса РАО.",
                        IsCritical = true
                    });
                }
                break;
            }
            default:
            {
                if (!OkpoRegex.IsMatch(status) && status is not ("1" or "2" or "3" or "4" or "6" or "9"))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = status,
                        Message = "Заполнение графы 5 не соответствует требованиям приказа Госкорпорации \"Росатом\" " +
                                  "от 07.12.2020 №1/13-НПА.",
                        IsCritical = true
                    });
                }
                break;
            }
        }
        return result;
    }

    #endregion

    #region Check015

    //Проверка кода вида документа 
    private static List<CheckError> Check_015(List<Form15> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 12;
        var documentVid = forms[line].DocumentVid_DB ?? 0;
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var valid = documentVid is >= 1 and <= 15 or 19;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "Неверный код вида документа, сопровождающего операцию.",
                IsCritical = true
            });
            return result;
        }
        if (documentVid == 19)
        {
            valid = CheckNotePresence(notes, line, graphNumber);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_15",
                    Row = (line + 1).ToString(),
                    Column = "DocumentVid_DB",
                    Value = Convert.ToString(documentVid),
                    Message = "К коду вида документа 19 необходимо примечание.",
                    IsCritical = true
                });
            }
        } 
        else if (opCode == "41" && documentVid != 1)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "При коде операции 41, вид документа должен быть равен 1."
            });
        }
        return result;
    }

    #endregion

    #region Check016

    //Номер документа заполнен
    private static List<CheckError> Check_016(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var documentNumber = ReplaceNullAndTrim(forms[line].DocumentNumber_DB);
        var valid = documentNumber != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentNumber_DB",
                Value = documentNumber,
                Message = "Графа должна быть заполнена.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check017_a

    //Дата документа корректно заполнена (графа 17)
    private static List<CheckError> Check_017_a(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var docDate = ReplaceNullAndTrim(forms[line].DocumentDate_DB);
        if (docDate is "" or "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDate,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой.",
                IsCritical = true
            });
        }
        else if (!DateOnly.TryParse(docDate, out _))
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDate,
                Message = "Формат ввода данных не соответствует приказу. Некорректно заполнена дата документа.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check017_b

    //Дата документа <= дате операции для всех кодов операции, кроме 10 и 41
    private static List<CheckError> Check_017_b(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var operationDate = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        var documentDate = ReplaceNullAndTrim(forms[line].DocumentDate_DB);
        if (operationCode is "10" or "41"
            || !DateOnly.TryParse(documentDate, out var documentDateReal)
            || !DateOnly.TryParse(operationDate, out var operationDateReal))
        {
            return result;
        }
        var valid = documentDateReal <= operationDateReal.AddDays(30);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = documentDate,
                Message = "Дата документа не может быть позже даты операции."
            });
        }
        return result;
    }

    #endregion

    #region Check017_c

    //Дата документа = дате операции, если код операции 41
    private static List<CheckError> Check_017_c(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var documentDate = ReplaceNullAndTrim(forms[line].DocumentDate_DB);
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var operationDate = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        if (operationCode is not "41"
            || !DateOnly.TryParse(documentDate, out var documentDateReal)
            || !DateOnly.TryParse(operationDate, out var operationDateReal))
        {
            return result;
        }

        var daysBetween = operationDateReal > documentDateReal
            ? operationDateReal.DayNumber - documentDateReal.DayNumber
            : documentDateReal.DayNumber - operationDateReal.DayNumber;

        var valid = daysBetween <= 30;

        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = documentDate,
                Message = "Дата документа должна соответствовать дате операции."
            });
        }
        return result;
    }

    #endregion

    #region Check017_d

    //При коде операции 10, дата окончания ОП не позднее даты документа + 10 дней
    private static List<CheckError> Check_017_d(List<Form15> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var documentDate = ReplaceNullAndTrim(forms[line].DocumentDate_DB);
        var endPeriod = ReplaceNullAndTrim(rep.EndPeriod_DB);

        if (opCode is not "10"
            || !DateOnly.TryParse(endPeriod, out var pEnd)
            || !DateOnly.TryParse(documentDate, out var docDate)) return result;

        var valid = WorkdaysBetweenDates(docDate, pEnd) <= 10;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = documentDate,
                Message = "Нарушен срок предоставления отчётности. Для операций инвентаризации, " +
                          "срок предоставления отчёта исчисляется с даты утверждения акта инвентаризации и не должен превышать 10 рабочих дней."
            });
        }
        return result;
    }

    #endregion

    #region Check018_01

    //Код ОКПО поставщика/получателя не равен коду ОКПО отчитывающейся организации + 8/14 цифр (графа 18)
    private static List<CheckError> Check_018_01(List<Form15> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOpCodes =
        [
            "01", "10", "14", "41", "43", "44", "45", "49", "51", "52", "57", 
            "59", "71", "72", "73", "74", "75", "76", "97", "98", "99"
        ];
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOpCodes.Contains(opCode)) return result;

        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB.Trim()
            : (forms10[0].Okpo_DB ?? string.Empty).Trim();
        
        var valid = providerOrRecieverOkpo == repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Для выбранного кода операции указывается код ОКПО отчитывающейся организации.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check018_84

    //Код ОКПО поставщика/получателя из ОКСМ (не Россия), с примечанием (графа 18)
    private static List<CheckError> Check_018_84(List<Form15> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 15;
        string[] applicableOpCodes = { "84", "88" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOpCodes.Contains(opCode)) return result;
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        
        var valid = OKSM.Any(oksmEntry => oksmEntry["shortname"] == providerOrRecieverOkpo)
                    && !providerOrRecieverOkpo.Equals("россия", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "При операциях, связанных с перемещением ОЗИИИ через государственную границу Российской Федерации, " +
                          "необходимо указывать краткое наименование государства в соответствии с ОКСМ.",
                IsCritical = true
            });
        }
        valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Необходимо добавить примечание.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check018_21

    //Код ОКПО поставщика/получателя не равен коду ОКПО отчитывающейся организации + 8/14 цифр (колонка 18)
    private static List<CheckError> Check_018_21(List<Form15> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "21", "25", "26", "27", "28", "29", "31", "35", "36", "37", "38", "39" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB.Trim()
            : (forms10[0].Okpo_DB ?? string.Empty).Trim();
        if (!applicableOperationCodes.Contains(opCode)) return result;

        var valid = OkpoRegex.IsMatch(providerOrRecieverOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Значение может состоять только из 8 или 14 символов",
                IsCritical = true
            });
        }
        valid = providerOrRecieverOkpo != repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Для выбранного кода операции указывается код ОКПО контрагента.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check018_22

    //Код ОКПО поставщика/получателя состоит из 8/14 чисел или "минобороны" (графа 18)
    private static List<CheckError> Check_018_22(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        if (opCode is not ("22" or "32")) return result;
        var valid = OkpoRegex.IsMatch(providerOrRecieverOkpo)
                    || providerOrRecieverOkpo.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Формат ввода данных не соответствует приказу. Следует указать код ОКПО контрагента, " +
                          "либо \"Минобороны\" без кавычек.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check019_01

    //Код ОКПО перевозчика равен "-" (графа 16)
    private static List<CheckError> Check_019_01(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        [
            "01", "10", "14", "18", "41", "43", "44", "45", "48", "49", "51", 
            "52", "57", "59", "71", "72", "73", "74", "75", "76", "97", "98"
        ];
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var transporterOkpo = ReplaceNullAndTrim(forms[line].TransporterOKPO_DB);
        var valid = transporterOkpo is "-";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = transporterOkpo,
                Message = "При выбранном коде операции транспортирование не производится.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check019_21 (графа 16)

    //При определенных кодах операции, код ОКПО перевозчика равен 8/14 цифр
    private static List<CheckError> Check_019_21(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOpCodes =
        {
            "21","25","26","27","28","29","31","35","36","37","38","39","84","85","86","88"
        };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var transporterOkpo = ReplaceNullAndTrim(forms[line].TransporterOKPO_DB);
        if (!applicableOpCodes.Contains(opCode)) return result;
        var valid = OkpoRegex.IsMatch(transporterOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = transporterOkpo,
                Message = "Необходимо указать код ОКПО организации перевозчика.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check019_22 (графа 16)

    //Код ОКПО перевозчика состоит из 8/14 чисел или "минобороны"
    private static List<CheckError> Check_019_22(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "22", "32" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var transporterOkpo = ReplaceNullAndTrim(forms[line].TransporterOKPO_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OkpoRegex.IsMatch(transporterOkpo)
                    || transporterOkpo.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = transporterOkpo,
                Message = "Необходимо указать код ОКПО организации перевозчика, либо \"Минобороны\" без кавычек.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check020

    //Проверка Наименования прибора (графа 17)
    private static List<CheckError> Check_020(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var packName = ReplaceNullAndTrim(forms[line].PackName_DB);
        var packType = ReplaceNullAndTrim(forms[line].PackType_DB);
        var packNum = ReplaceNullAndTrim(forms[line].PackNumber_DB);
        if (packName.ToLower() is "без упаковки" && packType is not "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "PackType_DB",
                Value = packType,
                Message = "В случае, если упаковка отсутствует, в графе \"Тип прибора\" должен быть указан \"-\" без кавычек."
            });
        }
        if (packName.ToLower() is "без упаковки" && packNum is not "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "PackNumber_DB",
                Value = packNum,
                Message = "В случае, если упаковка отсутствует, " +
                          "в графе \"Заводской номер прибора\" должен быть указан \"-\" без кавычек."
            });
        }
        var valid = packName is not ("" or "-");
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "PackName_DB",
                Value = packName,
                Message = "Заполните сведения об упаковке РАО. Если РАО размещены без упаковки, " +
                          "то в графе 17 указывается \"без упаковки\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check021

    //Тип прибора заполнен (графа 18)
    private static List<CheckError> Check_021(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var packType = ReplaceNullAndTrim(forms[line].PackType_DB);
        var valid = packType != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "PackType_DB",
                Value = packType,
                Message = "Заполните сведения в графе \"Тип прибора\". " +
                          "В случае, если тип отсутствует, укажите символ \"=\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check022

    //Номер прибора заполнен (графа 19)
    private static List<CheckError> Check_022(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var packNum = ReplaceNullAndTrim(forms[line].PackNumber_DB);
        var valid = packNum != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "PackNumber_DB",
                Value = packNum,
                Message = "Заполните сведения о заводском номере упаковки. " +
                          "Если заводской номер отсутствует необходимо привести в круглых скобках номер, присвоенный в организации."
            });
        }
        return result;
    }

    #endregion

    #region Check023

    //Наименование пункта хранения заполнено (графа 20)
    private static List<CheckError> Check_023(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var storagePlaceName = ReplaceNullAndTrim(forms[line].StoragePlaceName_DB);
        var valid = storagePlaceName != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "StoragePlaceName_DB",
                Value = storagePlaceName,
                Message = "Графа должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check024

    //Код пункта хранения заполнен (графа 21)
    private static List<CheckError> Check_024(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var storagePlaceCode = ReplaceNullAndTrim(forms[line].StoragePlaceCode_DB);
        var valid = storagePlaceCode != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "StoragePlaceCode_DB",
                Value = storagePlaceCode,
                Message = "Графа должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check025

    //
    private static List<CheckError> Check_025(List<Form15> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var sortCode = ReplaceNullAndTrim(forms[line].RefineOrSortRAOCode_DB);
        var valid = sortCode != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = sortCode,
                Message = "Графа должна быть заполнена."
            });
        }
        else
        {
            var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
            switch (operationCode)
            {
                case "01" or "10" or "14" or "21" or "22" or "25" or "26" or "27" or "28" or "29" or "31" 
                    or "32" or "35" or "36" or "37" or "38" or "39" or "43" or "51" or "52" or "63" or "64" 
                    or "71" or "72" or "73" or "74" or "75" or "76" or "84" or "88" or "97" or "98" or "99":
                {
                    valid = sortCode == "-";
                    if (!valid)
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_15",
                            Row = (line + 1).ToString(),
                            Column = "RefineOrSortRAOCode_DB",
                            Value = sortCode,
                            Message = "Выбранный код операции не соответствует переработке/сортировке РАО. " +
                                      "Проверьте правильность заполнения граф 2 и 22."
                        });
                    }
                    break;
                }
                case "44":
                {
                    if (sortCode is not ("41" or "42" or "43" or "49" or "54" or "71" or "72" or "73" or "74" or "79" or "99"))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_15",
                            Row = (line + 1).ToString(),
                            Column = "RefineOrSortRAOCode_DB",
                            Value = sortCode,
                            Message = "Несуществующий код переработки/сортировки."
                        });
                    }

                    break;
                }
                case "45" or "57":
                {
                    if (sortCode is not ("-" or "74"))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_15",
                            Row = (line + 1).ToString(),
                            Column = "RefineOrSortRAOCode_DB",
                            Value = sortCode,
                            Message = "Коду операции упаковка/переупаковка не соответствует код переработки/сортировки."
                        });
                    }

                    break;
                }
                case "49" or "59":
                {
                    if (sortCode is not ("-" or "52" or "72" or "74"))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_15",
                            Row = (line + 1).ToString(),
                            Column = "RefineOrSortRAOCode_DB",
                            Value = sortCode,
                            Message = "Коду операции сортировка соответствуют коды сортировки 52, 72, 74."
                        });
                    }

                    break;
                }
            }
            if (sortCode is "49" or "79" or "99")
            {
                const byte graphNumber = 22;
                valid = CheckNotePresence(notes, line, graphNumber);
                if (!valid)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "RefineOrSortRAOCode_DB",
                        Value = sortCode,
                        Message = "К выбранному коду переработки/сортировки нужно привести примечание."
                    });
                }
            }
        }
        return result;
    }

    #endregion

    #region Check026

    //Субсидия от 0 до 100, либо "-", либо "" (графа 23)
    private static List<CheckError> Check_026(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var subsidy = ConvertStringToExponential(forms[line].Subsidy_DB);
        var valid = subsidy is "-" or "" 
                    || (TryParseDoubleExtended(subsidy, out var subsidyNum) 
                        && subsidyNum is >= 0 and <= 100);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Subsidy_DB",
                Value = subsidy,
                Message = "Графа заполнена некорректно. Допустимы значения от 0 до 100, символ \"-\" без кавычек или пустая строка."
            });
        }
        return result;
    }

    #endregion

    #region Check027

    //Номер ФЦП заполнен (графа 24)
    private static List<CheckError> Check_027(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var fcpNum = ConvertStringToExponential(forms[line].FcpNumber_DB);
        var valid = fcpNum is "-" or "" || TryParseDoubleExtended(fcpNum, out _);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "FcpNumber_DB",
                Value = fcpNum,
                Message = "Графа заполнена некорректно. Допустимо цифровое значение, символ \"-\" без кавычек или пустая строка."
            });
        }
        return result;
    }

    #endregion

    #endregion

    #region GraphList

    private static readonly Dictionary<string, string> GraphsList = new()
    {
        { "NumberInOrder_DB", "01 - № п/п" },
        { "OperationCode_DB", "02 - Код операции" },
        { "OperationDate_DB", "03 - Дата операции" },
        { "PassportNumber_DB", "04 - Номер паспорта (сертификата)" },
        { "Type_DB", "05 - Тип ЗРИ" },
        { "Radionuclids_DB", "06 - Радионуклиды" },
        { "FactoryNumber_DB", "07 - Номер" },
        { "Quantity_DB", "08 - Количество, шт." },
        { "Activity_DB", "09 - Суммарная активность, Бк" },
        { "CreationDate_DB", "10 - Дата выпуска" },
        { "StatusRAO_DB", "11 - Статус РАО" },
        { "DocumentVid_DB", "12 - Вид документа" },
        { "DocumentNumber_DB", "13 - Номер документа" },
        { "DocumentDate_DB", "14 - Дата документа" },
        { "ProviderOrRecieverOKPO_DB", "15 - Код ОКПО поставщика или получателя" },
        { "TransporterOKPO_DB", "16 - Код ОКПО перевозчика" },
        { "PackName_DB", "17 - Наименование прибора, УКТ, упаковки" },
        { "PackType_DB", "18 - Тип прибора, УКТ, упаковки" },
        { "PackNumber_DB", "19 - Заводской номер прибора, УКТ, упаковки" },
        { "StoragePlaceName_DB", "20 - Наименование пункта хранения" },
        { "StoragePlaceCode_DB", "21 - Код пункта хранения" },
        { "RefineOrSortRAOCode_DB", "22 - Код переработки/сортировки РАО" },
        { "Subsidy_DB", "23 - Субсидия, %" },
        { "FcpNumber_DB", "24 - Номер мероприятия ФЦП" },
    };

    #endregion
}