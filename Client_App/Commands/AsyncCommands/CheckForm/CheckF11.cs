using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Models.CheckForm;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;

namespace Client_App.Commands.AsyncCommands.CheckForm;

public abstract partial class CheckF11 : CheckBase
{
    #region CheckTotal

    public static List<CheckError> Check_Total(Reports reps, Report rep)
    {
        var currentFormLine = 0;
        List<CheckError> errorList = new();
        LoadDictionaries();
        var formsList = rep.Rows11.ToList<Form11>();
        errorList.AddRange(Check_002(rep));
        errorList.AddRange(Check_003(formsList, rep));
        errorList.AddRange(Check_004(formsList));
        foreach (var key in rep.Rows11)
        {
            var form = (Form11)key;
            var notes = rep.Notes.ToList<Note>();
            var forms10 = reps.Master_DB.Rows10.ToList<Form10>();
            errorList.AddRange(Check_001(formsList, currentFormLine));
            errorList.AddRange(Check_005(formsList, currentFormLine));
            errorList.AddRange(Check_006(formsList, currentFormLine));
            errorList.AddRange(Check_007(formsList, currentFormLine));
            errorList.AddRange(Check_008(formsList, currentFormLine));
            errorList.AddRange(Check_009(formsList, currentFormLine));
            errorList.AddRange(Check_010(formsList, notes, currentFormLine));
            errorList.AddRange(Check_011(formsList, currentFormLine));
            errorList.AddRange(Check_012(formsList, currentFormLine));
            errorList.AddRange(Check_013(formsList, currentFormLine));
            errorList.AddRange(Check_014(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_015(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_016(formsList, currentFormLine));
            errorList.AddRange(Check_017(formsList, currentFormLine));
            errorList.AddRange(Check_018(formsList, currentFormLine));
            errorList.AddRange(Check_019(formsList, currentFormLine));
            errorList.AddRange(Check_020(formsList, currentFormLine));
            errorList.AddRange(Check_021(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_022(formsList, currentFormLine));
            errorList.AddRange(Check_023(formsList, rep, currentFormLine));
            errorList.AddRange(Check_024(formsList, rep, currentFormLine));
            errorList.AddRange(Check_025(formsList, currentFormLine));
            errorList.AddRange(Check_026(formsList, currentFormLine));
            errorList.AddRange(Check_027(formsList, currentFormLine));
            errorList.AddRange(Check_028(formsList, currentFormLine));
            errorList.AddRange(Check_029(formsList, currentFormLine));
            errorList.AddRange(Check_030(formsList, currentFormLine));
            errorList.AddRange(Check_031(formsList, currentFormLine));
            errorList.AddRange(Check_032(formsList, currentFormLine));
            errorList.AddRange(Check_033(formsList, currentFormLine));
            errorList.AddRange(Check_034(formsList, currentFormLine));
            errorList.AddRange(Check_035(formsList, currentFormLine));
            errorList.AddRange(Check_036(formsList, currentFormLine));
            errorList.AddRange(Check_037(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_038(formsList, currentFormLine));
            errorList.AddRange(Check_039(formsList, notes, currentFormLine));
            errorList.AddRange(Check_040(formsList, currentFormLine));
            errorList.AddRange(Check_041(formsList, currentFormLine));
            errorList.AddRange(Check_042(formsList, currentFormLine));
            errorList.AddRange(Check_043(formsList, currentFormLine));
            errorList.AddRange(Check_044(formsList, currentFormLine));
            errorList.AddRange(Check_045(formsList, notes, currentFormLine));
            errorList.AddRange(Check_046(formsList, notes, currentFormLine));
            errorList.AddRange(Check_047(formsList, notes, currentFormLine));
            errorList.AddRange(Check_048(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_049(formsList, currentFormLine));
            errorList.AddRange(Check_050(formsList, notes, currentFormLine));
            errorList.AddRange(Check_051(formsList, notes, currentFormLine));
            errorList.AddRange(Check_052(formsList, currentFormLine));
            errorList.AddRange(Check_053(formsList, currentFormLine));
            errorList.AddRange(Check_054(formsList, notes, currentFormLine));
            errorList.AddRange(Check_055(formsList, currentFormLine));
            errorList.AddRange(Check_056(formsList, currentFormLine));
            errorList.AddRange(Check_057(formsList, currentFormLine));
            errorList.AddRange(Check_058(formsList, currentFormLine));
            errorList.AddRange(Check_059(formsList, rep, currentFormLine));
            errorList.AddRange(Check_060(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_061(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_062(formsList, currentFormLine));
            errorList.AddRange(Check_063(formsList, forms10, notes, currentFormLine));
            errorList.AddRange(Check_064(formsList, currentFormLine));
            errorList.AddRange(Check_065(formsList, notes, currentFormLine));
            errorList.AddRange(Check_066(formsList, currentFormLine));
            errorList.AddRange(Check_067(formsList, currentFormLine));
            errorList.AddRange(Check_068(formsList, currentFormLine));
            errorList.AddRange(Check_069(formsList, currentFormLine));
            errorList.AddRange(Check_070(formsList, currentFormLine));
            errorList.AddRange(Check_071(formsList, currentFormLine));
            errorList.AddRange(Check_072(formsList, currentFormLine));
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

    //Заглушка
    private static List<CheckError> Check_001(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        return result;
    }

    #endregion

    #region Check002

    //Проверка правильности заполнения дат периода
    private static List<CheckError> Check_002(Report rep)
    {
        List<CheckError> result = new();
        var stPerStr = (rep.StartPeriod_DB ?? string.Empty).Trim();
        var endPerStr = (rep.EndPeriod_DB ?? string.Empty).Trim();
        if (!DateOnly.TryParse(stPerStr, out _))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
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
                FormNum = "form_11",
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
                FormNum = "form_11",
                Row = string.Empty,
                Column = string.Empty,
                Value = $"{stPerStr} - {endPerStr}",
                Message = "Дата начала периода превышает дату окончания периода."
            });
        }
        return result;
    }

    #endregion

    #region Check003

    //Проверка даты окончания периода
    private static List<CheckError> Check_003(List<Form11> forms11, Report rep)
    {
        var forms = forms11.Cast<Form1>().ToList();
        return CheckRepPeriod(forms, rep);
    }

    #endregion

    #region Check004

    //Наличие строк дубликатов (графы 2 - 19, 23)
    private static List<CheckError> Check_004(List<Form11> forms)
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
                                  && comparator.Compare(formToCompare.CreatorOKPO_DB, currentForm.CreatorOKPO_DB) == 0
                                  && comparator.Compare(formToCompare.CreationDate_DB, currentForm.CreationDate_DB) == 0
                                  && formToCompare.Category_DB == currentForm.Category_DB
                                  && formToCompare.SignedServicePeriod_DB.Equals(currentForm.SignedServicePeriod_DB)
                                  && formToCompare.PropertyCode_DB == currentForm.PropertyCode_DB
                                  && comparator.Compare(formToCompare.Owner_DB, currentForm.Owner_DB) == 0
                                  && formToCompare.DocumentVid_DB == currentForm.DocumentVid_DB
                                  && comparator.Compare(formToCompare.DocumentNumber_DB, currentForm.DocumentNumber_DB) == 0
                                  && comparator.Compare(formToCompare.DocumentDate_DB, currentForm.DocumentDate_DB) == 0
                                  && comparator.Compare(formToCompare.ProviderOrRecieverOKPO_DB, currentForm.ProviderOrRecieverOKPO_DB) == 0
                                  && comparator.Compare(formToCompare.PackNumber_DB, currentForm.PackNumber_DB) == 0;
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
                    FormNum = "form_11",
                    Row = dupStrByGroups,
                    Column = "2 - 19, 23",
                    Value = string.Empty,
                    Message = $"Данные граф 2 - 19, 23 в строках {dupStrByGroups} продублированы. " +
                              $"Следует проверить правильность предоставления данных."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check005

    //Порядок строк (графа 1)
    private static List<CheckError> Check_005(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (forms[line].NumberInOrder_DB != line + 1)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Id",
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

    #region Check006

    //Код операции из списка (графа 2)
    private static List<CheckError> Check_006(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var applicableOperationCodes = new[]
        {
            "10","11","12","15","17","18","21","22","25","27","28","29","31","32",
            "35","37","38","39","41","42","43","46","47","53","54","58","61","62",
            "63","64","65","66","67","68","71","72","73","74","75","81","82","83",
            "84","85","86","87","88","97","98","99"
        };
        var valid = applicableOperationCodes.Contains(operationCode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "Код операции не может быть использован в форме 1.1.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check007

    //TODO
    //Соответствие СНК при коде операции 10
    private static List<CheckError> Check_007(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "10" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = forms[line].OperationCode_DB,
                Message = "Сведения, представленные в инвентаризации, не соответствуют СНК."
            });
        }
        return result;
    }

    #endregion

    #region Check008

    //Для кодов операции 12, 42, в радионуклидах должен быть указан хоть один из списка (колонка 6)
    private static List<CheckError> Check_008(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "12", "42" };
        var radionuclids = ReplaceNullAndTrim(forms[line].Radionuclids_DB);
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = Radionuclids_YAM_DB_Valids.Any(nuclid =>
            radionuclids.Contains(nuclid, StringComparison.CurrentCultureIgnoreCase));
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = radionuclids,
                Message = "В графе 6 не представлены сведения о радионуклидах, которые могут быть отнесены к ЯМ. " +
                          "Проверьте правильность выбранного кода операции.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check009

    //TODO
    //Наличие записей в 1.5/1.6 при коде операции 15
    private static List<CheckError> Check_009(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "15" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = false;
        //..
        //..
        //..
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "-",
                Value = "-",
                Message = "В предыдущих отчетах не найдена строка об осуществлении операции переработки РАО в виде ОЗИИИ. " +
                          "Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check010

    //Наличие примечания для кодов операции 29, 39, 97-99 (графа 2)
    private static List<CheckError> Check_010(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 2;
        string[] applicableOperationCodes = { "29", "39", "97", "98", "99" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = opCode,
                Message = "Необходимо дать пояснение об осуществленной операции.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check011

    //TODO
    //Проверка наличия СНК в организации
    private static List<CheckError> Check_011(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes =
        {
            "21", "22", "25", "27", "28", "29", "41", "42", "43", "46", "53", "54", "61", 
            "62", "65", "66", "67", "68", "71", "72", "81", "82", "83", "84", "88", "98"
        };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = false;
        //..
        //..
        //..
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "-",
                Value = "-",
                Message = "Учетной единицы с такими параметрами нет в организации. Проверьте правильность указываемых сведений для ЗРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check012

    //TODO
    //Наличие записи с кодом операции 27 в старых отчётах
    private static List<CheckError> Check_012(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "37" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = false;
        //..
        //..
        //..
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = forms[line].OperationCode_DB,
                Message = "В отчетах не найдена строка об осуществлении передачи учетной единицы. " +
                          "Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check013

    //TODO
    //Наличие записи в форме 1.5 с такой же учётной единицей при коде операции 41 (графа 2)
    private static List<CheckError> Check_013(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "41" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = false;
        //..
        //..
        //..
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "-",
                Value = "-",
                Message = "Заполните форму 1.5."
            });
        }
        return result;
    }

    #endregion

    #region Check014

    //Для кода операции 53, ОКПО получателя/перевозчика должен совпадать с ОКПО организации (графа 2)
    private static List<CheckError> Check_014(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "53" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        var repOkpo = okpoRepTerPodr is not ""
            ? okpoRepTerPodr
            : okpoRepJur;
        if (!OkpoRegex.IsMatch(providerOrRecieverOkpo))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Формат ввода данных не соответствует приказу.",
                IsCritical = true
            });
        }
        var valid = providerOrRecieverOkpo == repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "В графе 19 необходимо указать код ОКПО отчитывающейся организации. В случае, " +
                          "если зарядку/разрядку осуществляла подрядная организация, следует использовать код операции 54.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check015

    //Для кода операции 54, ОКПО получателя/перевозчика не должен совпадать с ОКПО организации
    private static List<CheckError> Check_015(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "54" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        var repOkpo = okpoRepTerPodr is not ""
            ? okpoRepTerPodr
            : okpoRepJur;
        if (!OkpoRegex.IsMatch(providerOrRecieverOkpo))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Формат ввода данных не соответствует приказу.",
                IsCritical = true
            });
        }
        var valid = providerOrRecieverOkpo != repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "В графе 19 необходимо указать ОКПО подрядной организации. В случае, " +
                          "если зарядка/разрядка осуществлялась силами отчитывающейся организации, " +
                          "следует использовать код операции 53.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check016

    //TODO
    //Наличие записи с кодом 46
    private static List<CheckError> Check_016(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "58" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = false;
        //..
        //..
        //..
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = forms[line].OperationCode_DB,
                Message = "В отчетах не найдены сведения о снятии с учета учетной единицы для разукомплектования. " +
                          "Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check017

    //TODO
    //Наличие записи с кодом операции 61
    private static List<CheckError> Check_017(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "62" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = false;
        //..
        //..
        //..
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "-",
                Value = forms[line].OperationCode_DB,
                Message = "В отчетах не найдены сведения о вывозе учетной единицы. Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check018

    //TODO
    //Наличие отчёта по форме 1.3
    private static List<CheckError> Check_018(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "65" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = false;
        //..
        //..
        //..
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "-",
                Value = "-",
                Message = "Заполните форму 1.3."
            });
        }
        return result;
    }

    #endregion

    #region Check019

    //Код ОКПО состоит из 8/14 чисел для определенных кодов операции (графа 10)
    private static List<CheckError> Check_019(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "81", "82", "87", "88" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var creatorOkpo = ReplaceNullAndTrim(forms[line].CreatorOKPO_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OkpoRegex.IsMatch(creatorOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = creatorOkpo,
                Message = "Код используется для предоставления сведений о ЗРИ, произведенных в Российской Федерации. " + 
                          "Необходимо ввести ОКПО организации изготовителя ЗРИ.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check020

    //Коды операции 83-86 используются для зарубежных стран (графа 10)
    private static List<CheckError> Check_020(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "83", "84", "85", "86" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var creatorOkpo = ReplaceNullAndTrim(forms[line].CreatorOKPO_DB);
        var valid = !(OKSM.All(oksmEntry => oksmEntry["shortname"] != creatorOkpo)
                    || creatorOkpo.ToLower() is "россия");
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = creatorOkpo,
                Message = "Код используется для предоставления сведений о ЗРИ, произведенных за пределами Российской Федерации. " + 
                          "Для импортированных ЗРИ необходимо указать краткое наименование государства в соответствии с ОКСМ.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check021

    //В поле код ОКПО правообладателя должен быть ОКПО организации (юридического или обособленного) (графа 15)
    private static List<CheckError> Check_021(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        string[] validCodes = ["11", "12", "15", "28", "38", "41", "63", "64", "65", "81", "85", "88"];
        if (!validCodes.Contains(operationCode)) return result;
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        var owner = ReplaceNullAndTrim(forms[line].Owner_DB);
        var valid = !string.IsNullOrWhiteSpace(owner) 
                    && (owner == okpoRepTerPodr || owner == okpoRepJur);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Уточните правообладателя ЗРИ.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check022

    //Дата операции корректно заполнена (графа 3)
    private static List<CheckError> Check_022(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var opDate = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        if (string.IsNullOrWhiteSpace(opDate) || opDate is "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = opDate,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой.",
                IsCritical = true
            });
        }
        else if (!DateOnly.TryParse(opDate, out _))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = opDate,
                Message = "Формат ввода данных не соответствует приказу. Некорректно заполнена дата операции.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check023

    //Дата операции входит в отчетный период (графа 3)
    private static List<CheckError> Check_023(List<Form11> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var stPerStr = ReplaceNullAndTrim(rep.StartPeriod_DB);
        var endPerStr = ReplaceNullAndTrim(rep.EndPeriod_DB);
        var opDateStr = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        if (opCode is "10"
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
                FormNum = "form_11",
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

    #region Check024

    //При коде операции 10, дата документа должна попадать в отчетный период
    private static List<CheckError> Check_024(List<Form11> forms, Report rep, int line)
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
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDateStr,
                Message = "Для операции инвентаризации дата операции не может превышать даты утверждения акта инвентаризации.",
                IsCritical = true
            });
        }
        var valid = docDate >= stPer && docDate <= dateEndReal;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDateStr,
                Message = "Дата документа не входит в отчетный период. Для операции инвентаризации, " +
                          "срок предоставления отчёта исчисляется с даты утверждения акта инвентаризации.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check025

    //Номер паспорта не пустая строка (графа 4)
    private static List<CheckError> Check_025(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var pasNum = ReplaceNullAndTrim(forms[line].PassportNumber_DB);
        var valid = pasNum != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PassportNumber_DB",
                Value = pasNum,
                Message = "Графа не может быть пустой. Укажите номера паспорта (сертификата). " + 
                "В случае, если номер отсутствует, укажите символ \"-\" без кавычек.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check026
    
    //TODO
    //Тип из справочника
    private static List<CheckError> Check_026(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (ZRI_Ignore) return result;
        var type = ReplaceNullAndTrim(forms[line].Type_DB);
        var valid = !string.IsNullOrEmpty(type);
        if (valid)
        {
            valid = Type_DB_Valids.Contains(type);
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Type_DB",
                Value = type,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    #endregion

    #region Check027

    //Проверка типа зри на недопустимые значения (графа 5)
    private static List<CheckError> Check_027(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var type = ReplaceNullAndTrim(forms[line].Type_DB);
        var nonApplicableTypes = new[] { "зри", "изделие", "прибор", "аппарат" };
        var valid = type != string.Empty 
                    && !nonApplicableTypes.Contains(type.ToLower());
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Type_DB",
                Value = type,
                Message = "Укажите тип ЗРИ в соответствии с паспортом (сертификатом)."
            });
        }
        return result;
    }

    #endregion

    #region Check028

    //TODO
    //Проверка по справочнику ЗРИ
    private static List<CheckError> Check_028(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (ZRI_Ignore) return result;
        var valid = !string.IsNullOrEmpty(forms[line].Type_DB)
                    && !string.IsNullOrEmpty(forms[line].Radionuclids_DB);
        if (!Type_DB_Valids.Contains(forms[line].PackType_DB)) return result;
        if (valid)
        {
            List<string> radionuclidsValidFull = new();
            List<string> radionuclidsFactFull = new(forms[line].Radionuclids_DB!
                .Replace(" ", string.Empty)
                .ToLower()
                .Split(";"));
            foreach (var radionuclidFact in radionuclidsFactFull)
            {
                if (!radionuclidsValidFull.Contains(radionuclidFact))
                {
                    valid = false;
                    break;
                }
                radionuclidsValidFull.Remove(radionuclidFact);
            }
            valid = radionuclidsValidFull.Count == 0;
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = forms[line].Radionuclids_DB,
                Message = "Перечень радионуклидов не соответствует справочным данным."
            });
        }
        return result;
    }

    #endregion

    #region Check029

    //Сверка со справочником
    private static List<CheckError> Check_029(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var rads = ReplaceNullAndTrim(forms[line].Radionuclids_DB);
        if (string.IsNullOrWhiteSpace(rads) || rads == "-") return result;
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();
        if (radsSet.Count == 1 
            && R.All(phEntry => phEntry["name"] != radsSet.First()))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
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
                FormNum = "form_11",
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

    #region Check030

    // Не пустое поле (графа 6 - Радионуклиды)
    private static List<CheckError> Check_030(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var rads = ReplaceNullAndTrim(forms[line].Radionuclids_DB);
        if (string.IsNullOrWhiteSpace(rads))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = rads,
                Message = "Заполните сведения о радионуклиде.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check031

    //Разделение через ";", если количество > 1 (графа 7 - Номер ЗРИ).
    private static List<CheckError> Check_031(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var factoryNum = ReplaceNullAndTrim(forms[line].FactoryNumber_DB);
        if (factoryNum is "-") return result;
        var quantity = forms[line].Quantity_DB ?? 0; ;

        if (string.IsNullOrWhiteSpace(factoryNum))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "FactoryNumber_DB",
                Value = factoryNum,
                Message = "Заполните сведения о заводском номере ЗРИ. Если номер отсутствует, " +
                          "в ячейке следует указать символ \"-\" без кавычек.",
                IsCritical = true
            });
        }
        if (factoryNum.Contains(','))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
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
                FormNum = "form_11",
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
                    FormNum = "form_11",
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

    #region Check032

    //Количество больше нуля (графа 8)
    private static List<CheckError> Check_032(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var quantity = forms[line].Quantity_DB ?? 0;
        var valid = quantity is > 0;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Quantity_DB",
                Value = quantity.ToString(),
                Message = "Укажите количество ЗРИ.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check033

    //Суммарная активность из справочника для одного радионуклида
    private static List<CheckError> Check_033(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var rad = ReplaceNullAndTrim(forms[line].Radionuclids_DB).ToLower();
        var activity = ConvertStringToExponential(forms[line].Activity_DB);
        var quantity = forms[line].Quantity_DB ?? 0;

        if (R.All(x => x["name"] != rad) 
            || quantity <= 0
            || !TryParseDoubleExtended(activity, out var activityDoubleValue)
            || activityDoubleValue <= 0) return result;

        var mza = R.First(x => x["name"] == rad)["MZA"];
        if (!TryParseDoubleExtended(mza, out var mzaDoubleValue)) return result;

        var valid = activityDoubleValue / quantity >= mzaDoubleValue;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Сведения, указанные в графе 9 (Суммарная активность) ниже МЗА, " +
                          "ЗРИ не является объектом учёта СГУК РВ и РАО. Сверьте сведения, указанные в отчёте, с паспортом на ЗРИ. " +
                          "Обратите внимание на размерность активности, указанной в паспорте на ЗРИ (Бк, МБк, ТБк).",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check034

    //Суммарная активность из справочника для нескольких радионуклидов
    private static List<CheckError> Check_034(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var rads = ReplaceNullAndTrim(forms[line].Radionuclids_DB);
        var activity = ConvertStringToExponential(forms[line].Activity_DB);
        var quantity = forms[line].Quantity_DB ?? 0;
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();

        var isEqRads = CheckEquilibriumRads(radsSet);

        if (radsSet.Count == 1
            || isEqRads
            || quantity <= 0
            || !TryParseDoubleExtended(activity, out var activityDoubleValue)
            || activityDoubleValue <= 0
            || !radsSet
                .All(rad => R
                    .Any(phEntry => phEntry["name"] == rad))) return result;

        var minimumActivity = double.MaxValue;
        var anyMza = false;
        foreach (var rad in radsSet)
        {
            var mza = R.First(x => x["name"] == rad)["MZA"];
            if (!TryParseDoubleExtended(mza, out var mzaDoubleValue)) continue;
            if (mzaDoubleValue < minimumActivity)
            {
                minimumActivity = mzaDoubleValue;
            }
            anyMza = true;
        }
        if (!anyMza) return result;

        var valid = activityDoubleValue / quantity >= minimumActivity;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Сведения, указанные в графе 9 (Суммарная активность) ниже МЗА, " +
                          "ЗРИ не является объектом учёта СГУК РВ и РАО. Сверьте сведения, указанные в отчёте, с паспортом на ЗРИ. " +
                          "Обратите внимание на размерность активности, указанной в паспорте на ЗРИ (Бк, МБк, ТБк).",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check035

    //Суммарная активность из справочника для равновесных радионуклидов
    private static List<CheckError> Check_035(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var rads = ReplaceNullAndTrim(forms[line].Radionuclids_DB);
        var activity = ConvertStringToExponential(forms[line].Activity_DB);
        var quantity = forms[line].Quantity_DB ?? 0;
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();

        var isEqRads = CheckEquilibriumRads(radsSet);

        if (!isEqRads
            || quantity <= 0
            || !TryParseDoubleExtended(activity, out var activityDoubleValue)
            || activityDoubleValue <= 0
            || !radsSet
                .All(rad => R
                    .Any(phEntry => phEntry["name"] == rad))) return result;

        var baseRad = EquilibriumRadionuclids.First(x =>
            {
                x = x.Replace(" ", string.Empty);
                var eqRadsArray = x.Split(',');
                return radsSet
                    .All(rad => eqRadsArray
                        .Contains(rad) && radsSet.Count == eqRadsArray.Length);
            })
            .Replace(" ", string.Empty)
            .Split(',')[0];

        var mza = R.First(x => x["name"] == baseRad)["MZA"];
        if (!TryParseDoubleExtended(mza, out var mzaDoubleValue)) return result;

        var valid = activityDoubleValue / quantity >= mzaDoubleValue;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Сведения, указанные в графе 9 (Суммарная активность) ниже МЗА, " +
                          "ЗРИ не является объектом учёта СГУК РВ и РАО. Сверьте сведения, указанные в отчёте, с паспортом на ЗРИ. " +
                          "Обратите внимание на размерность активности, указанной в паспорте на ЗРИ (Бк, МБк, ТБк).",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check036

    //"Суммарная активность, Бк" положительное число, менее 10е+20
    private static List<CheckError> Check_036(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var activity = ConvertStringToExponential(forms[line].Activity_DB);
        if (!TryParseDoubleExtended(activity, out var activityReal)
            || activity is "" or "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Заполните сведения о суммарной активности ЗРИ, переведенных в ОЗИИИ. " +
                          "Оценочные сведения приводятся в круглых скобках.",
                IsCritical = true
            });
        }
        else switch (activityReal)
        {
            case <= 0:
            {
                result.Add(new CheckError
                {
                    FormNum = "form_11",
                    Row = (line + 1).ToString(),
                    Column = "Activity_DB",
                    Value = activity,
                    Message = "Суммарная активность должна быть более нуля. Проверьте правильность введённых данных.",
                    IsCritical = true
                });
                break;
            }
            case > 10e+20:
            {
                result.Add(new CheckError
                {
                    FormNum = "form_11",
                    Row = (line + 1).ToString(),
                    Column = "Activity_DB",
                    Value = activity,
                    Message = "Указано слишком большое значение суммарной активности. Проверьте правильность введённых данных.",
                    IsCritical = true
                });
                break;
            }
        }
        return result;
    }

    #endregion

    #region Check037

    //для кодов операции 11, 58 "ОКПО организации" равен "код ОКПО изготовителя" (графа 10)
    private static List<CheckError> Check_037(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "11" };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var creatorOkpo = ReplaceNullAndTrim(forms[line].CreatorOKPO_DB);
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        var repOkpo = okpoRepTerPodr is not ""
            ? okpoRepTerPodr
            : okpoRepJur;
        var valid = creatorOkpo == repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = creatorOkpo,
                Message = "Проверьте код ОКПО организации-изготовителя.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check038

    //Соответствие формата данных "код ОКПО изготовителя" (колонка 10)
    private static List<CheckError> Check_038(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var creatorOkpo = ReplaceNullAndTrim(forms[line].CreatorOKPO_DB);
        var valid = !string.IsNullOrEmpty(creatorOkpo)
                    && (OkpoRegex.IsMatch(creatorOkpo)
                        || creatorOkpo is "прим."
                        || OKSM.Any(oksmEntry => oksmEntry["shortname"] == creatorOkpo));
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = creatorOkpo,
                Message = "Формат ввода данных не соответствует приказу. Укажите код ОКПО организации изготовителя.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check039

    // Если в "код ОКПО изготовителя" (колонка 10) указано примечание, то проверяется наличие примечания для данной строки
    private static List<CheckError> Check_039(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] creatorOkpoValid = { "прим.", "прим", "примечание", "примечания" };
        string[] applicableOperationCodes = { "81", "82", "87", "88" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var creatorOKPO = ReplaceNullAndTrim(forms[line].CreatorOKPO_DB);
        if (applicableOperationCodes.Contains(operationCode)) return result;
        if (!creatorOkpoValid.Contains(creatorOKPO.ToLower()) 
            && OKSM.All(oksmEntry => oksmEntry["shortname"] != creatorOKPO)) return result;
        const byte graphNumber = 10;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = creatorOKPO,
                Message = "Необходимо указать в примечании наименование и адрес организации-изготовителя ЗРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check040

    //Дата выпуска корректно заполнена (графа 10)
    private static List<CheckError> Check_040(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var creationDate = ReplaceNullAndTrim(forms[line].CreationDate_DB);
        if (string.IsNullOrWhiteSpace(creationDate) || creationDate is "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreationDate_DB",
                Value = creationDate,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой. " +
                          "Если известен только год, то указывается 1 января этого года.",
                IsCritical = true
            });
        }
        else if (!DateOnly.TryParse(creationDate, out _))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreationDate_DB",
                Value = creationDate,
                Message = "Формат ввода данных не соответствует приказу. Некорректно заполнена дата выпуска. " +
                          "Если известен только год, то указывается 1 января этого года.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check041

    //Дата выпуска (графа 11) <= Дате операции (графа 3)
    private static List<CheckError> Check_041(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var opDateStr = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        var creationDateStr = ReplaceNullAndTrim(forms[line].CreationDate_DB);
        if (!DateOnly.TryParse(opDateStr, out var opDate)
            || !DateOnly.TryParse(creationDateStr, out var createDate))
        {
            return result;
        }
        var valid = createDate <= opDate;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreationDate_DB",
                Value = creationDateStr,
                Message = "Дата выпуска не может быть позже даты операции.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check042

    //Соответствие категории ЗРИ (графа 12)
    private static List<CheckError> Check_042(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var dbBounds = new Dictionary<short, (decimal, decimal)>
        {
            { 1, (1000, decimal.MaxValue) },
            { 2, (10, 1000) },
            { 3, (1, 10) },
            { 4, (0.01m, 1) },
            { 5, (0, 0.01m) }
        };
        var activity = ConvertStringToExponential(forms[line].Activity_DB);
        var category = forms[line].Category_DB ?? 0;
        var quantity = forms[line].Quantity_DB ?? 0;
        var radionuclids = ReplaceNullAndTrim(forms[line].Radionuclids_DB);

        List<decimal> dValueList = new();

        var radsSet = radionuclids
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();

        _ = CheckEquilibriumRads(radsSet);

        var valid = dbBounds.ContainsKey(category)
                    && radsSet.Count > 0;
        if (valid)
        {
            foreach (var nuclid in radsSet)
            {
                var nuclidFromR = R.FirstOrDefault(x => x["name"] == nuclid);
                if (nuclidFromR is null) continue;
                var expFromR = ConvertStringToExponential(nuclidFromR["D"]);
                if (decimal.TryParse((expFromR),
                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
                        CultureInfo.CreateSpecificCulture("ru-RU"),
                        out var value))
                {
                    dValueList.Add(decimal.Multiply(value, 1e12m));
                }
            }
            //if (dValueList.Count == 0)
            //{
            //    foreach (var nuclid in nuclidsList)
            //    {
            //        foreach (var key in D.Keys.Where(key => key.Contains(nuclid)))
            //        {
            //            dValueList.Add(D[key] / (quantity != null && quantity != 0
            //                ? (double)quantity
            //                : 1.0));
            //            break;
            //        }
            //    }
            //}
            if (dValueList.Count == 0)
            {
                //result.Add(new CheckError
                //{
                //    FormNum = "form_11",
                //    Row = (line + 1).ToString(),
                //    Column = "Radionuclids_DB",
                //    Value = radionuclids,
                //    Message = "Проверьте правильность заполнения графы 6."
                //});
                return result;
            }
            var dMinValue = dValueList.Min();
            var dMaxValue = dValueList.Max();
            valid = decimal.TryParse(ConvertStringToExponential(activity),
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var aValue);
            aValue /= quantity != 0
                ? quantity
                : 1.0m;
            if (valid)
            {
                var adMinBound = dMaxValue == 0.0m
                    ? decimal.MaxValue
                    : aValue / dMaxValue;
                var adMaxBound = dMinValue == 0.0m
                    ? decimal.MaxValue
                    : aValue / dMinValue;
                valid = dbBounds[category].Item1 <= adMinBound
                        && dbBounds[category].Item2 > adMaxBound;
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Category_DB",
                Value = category.ToString(),
                Message = "Расчетное значение категории ЗРИ не соответствует представленному в отчёте. " +
                          "Проверьте правильность указания категории ЗРИ, сведений о суммарной активности и радионуклидах."
            });
        }
        return result;
    }

    #endregion

    #region Check043

    //Дата выпуска + НСС < дата операции
    private static List<CheckError> Check_043(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var signedServicePeriod = forms[line].SignedServicePeriod_DB ?? 0;
        var creationDate = ReplaceNullAndTrim(forms[line].CreationDate_DB);
        var operationDate = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        if (signedServicePeriod <= 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "SignedServicePeriod_DB",
                Value = signedServicePeriod.ToString(CultureInfo.InvariantCulture),
                Message = "Недопустимое значение НСС."
            });
            return result;
        }
        if (opCode is "41"
            || !DateOnly.TryParse(creationDate, out var creationDateReal)
            || !DateOnly.TryParse(operationDate, out var operationDateReal))
        {
            return result;
        }
        creationDateReal = creationDateReal.AddMonths((int)signedServicePeriod);
        creationDateReal = creationDateReal.AddDays((int)Math.Round(30 * (signedServicePeriod % 1.0)));
        var valid = creationDateReal >= operationDateReal;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "SignedServicePeriod_DB",
                Value = signedServicePeriod.ToString(),
                Message = "Для ЗРИ истек НСС, следует продлить НСС либо снять с учета " +
                          "с одновременной постановкой на учет как РАО (при выполнении критериев отнесения к РАО). " +
                          "Проверьте, что НСС указан в месяцах."
            });
        }
        return result;
    }

    #endregion

    #region Check044

    //Код формы собственности (колонка 14) от 1 до 6, 9
    private static List<CheckError> Check_044(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var propertyCode = forms[line].PropertyCode_DB ?? 0;
        if (propertyCode is not (>=1 and <=6 or 9))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = propertyCode.ToString(),
                Message = "Формат ввода данных не соответствует приказу. " +
                          "Выберите идентификатор, соответствующий форме собственности ЗРИ.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check045

    //Код формы собственности равен 2 (графа 14)
    private static List<CheckError> Check_045(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var propertyCode = forms[line].PropertyCode_DB ?? 0;
        const byte graphNumber = 14;
        byte?[] propertyCodeValid = { 2 };
        if (!propertyCodeValid.Contains(propertyCode)) return result;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = propertyCode.ToString(),
                Message = "Необходимо указать в примечании наименование субъекта Российской Федерации, " +
                          "в собственности которого находится объект учета.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check046

    //Код формы собственности равен 3 (графа 14)
    private static List<CheckError> Check_046(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var propertyCode = forms[line].PropertyCode_DB ?? 0;
        const byte graphNumber = 14;
        byte?[] propertyCodeDBValid = { 3 };
        if (!propertyCodeDBValid.Contains(propertyCode)) return result;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = propertyCode.ToString(),
                Message = "Необходимо указать в примечании наименование муниципального образования, " +
                          "в собственности которого находится объект учета.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check047

    //Код формы собственности равен 4 (графа 14)
    private static List<CheckError> Check_047(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var propertyCode = forms[line].PropertyCode_DB ?? 0;
        const byte graphNumber = 14;
        byte?[] propertyCodeValid = { 9 };
        if (!propertyCodeValid.Contains(propertyCode)) return result;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = propertyCode.ToString(),
                Message = "Необходимо указать в примечании наименование и адрес правообладателя " +
                          "(собственника или обладателя иного вещного права) на ЗРИ.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check048

    //8 или 14 чисел (графа 15) если код формы собственности от 1 до 4 (графа 14)
    private static List<CheckError> Check_048(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        byte?[] propertyCodeValid = { 1, 2, 3, 4 };
        string[] applicableOperationCodes = { "11", "12", "28", "38", "41", "63", "64" };
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        var repOkpo = okpoRepTerPodr is not ""
            ? okpoRepTerPodr
            : okpoRepJur;
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var owner = ReplaceNullAndTrim(forms[line].Owner_DB);
        var propertyCode = forms[line].PropertyCode_DB ?? 0;
        if (OperationCode_DB_Check021.Contains(opCode)) return result;

        if (applicableOperationCodes.Contains(opCode) && repOkpo != owner)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = $"Для кода операции {opCode}, " +
                          $"код ОКПО правообладателя должен совпадать с ОКПО отчитывающейся организации"
            });
        }
        if (!propertyCodeValid.Contains(propertyCode)) return result;
        var valid = !string.IsNullOrEmpty(owner)
                    && OkpoRegex.IsMatch(owner);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Формат ввода данных не соответствует приказу. " +
                          "В случае, если правообладатель российское юридическое лицо, необходимо указать его код ОКПО."
            });
        }
        return result;
    }

    #endregion

    #region Check049

    //Правообладатель (графа 15) из справочника ОКСМ, если код формы собственности 5
    private static List<CheckError> Check_049(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        byte?[] propertyCodeValid = { 5 };
        var propertyCode = forms[line].PropertyCode_DB ?? 0;
        var owner = ReplaceNullAndTrim(forms[line].Owner_DB);
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (OperationCode_DB_Check021.Contains(opCode)) return result;
        if (!propertyCodeValid.Contains(propertyCode)) return result;
        var valid = OKSM.Any(oksmEntry => oksmEntry["shortname"] == owner)
                    && !owner.Equals("россия", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Формат ввода данных не соответствует приказу. В случае, если правообладатель иностранное государство " +
                          "необходимо указать его краткое наименование в соответствии с ОКСМ.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check050

    //Правообладатель (графа 15) из справочника ОКСМ и наличие примечания, если код формы собственности 6
    private static List<CheckError> Check_050(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        byte?[] propertyCodeValid = { 6 };
        const byte graphNumber = 15;
        var propertyCode = forms[line].PropertyCode_DB ?? 0;
        var owner = ReplaceNullAndTrim(forms[line].Owner_DB);
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (OperationCode_DB_Check021.Contains(operationCode)) return result;
        if (!propertyCodeValid.Contains(propertyCode)) return result;
        if (OKSM.All(oksmEntry => oksmEntry["shortname"] != owner)
            || owner.Equals("россия", StringComparison.CurrentCultureIgnoreCase))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "При коде формы собственности равном \"6\", " +
                          "в графе \"код ОКПО правообладателя\" необходимо указать краткое наименование страны в соответствии с ОКСМ." ,
                IsCritical = true
            });
            return result;
        }
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Необходимо указать в примечании наименование и адрес правообладателя " +
                          "(собственника или обладателя иного вещного права) на ЗРИ.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check051

    //Если код формы собственности 9 (графа 14), то должно быть примечание и значение "прим."
    private static List<CheckError> Check_051(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] creatorOkpoValid ={ "прим.", "прим", "примечание", "примечания" };
        const byte graphNumber = 15;
        short?[] propertyCodeValid = { 9 };
        string[] nonApplicableOperationCodes = { "11", "12", "15", "28", "38", "41", "63", "64", "65", "73", "81", "85", "88" }; //взяты из Check_018
        var propertyCode = forms[line].PropertyCode_DB ?? 0;
        var owner = ReplaceNullAndTrim(forms[line].Owner_DB);
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (nonApplicableOperationCodes.Contains(operationCode) || !propertyCodeValid.Contains(propertyCode)) return result;
        var valid = creatorOkpoValid.Contains(owner?.ToLower());
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Необходимо указать \"прим.\" и добавить соответствующее примечание с наименованием и адресом правообладателя " +
                          "(собственника или обладателя иного вещного права) на ЗРИ.",
                IsCritical = true
            });
        }
        valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Необходимо указать в примечании наименование и адрес правообладателя " +
                          "(собственника или обладателя иного вещного права) на ЗРИ.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check052

    //При коде операции 10, вид документа равен 1 (графа 16)
    private static List<CheckError> Check_052(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var documentVid = forms[line].DocumentVid_DB ?? 0;
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (opCode != "10") return result;
        if (documentVid != 1)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "Для операции инвентаризации сопровождающий документ - акт инвентаризации (код 1)."
            });
        }
        return result;
    }

    #endregion

    #region Check053

    //При коде операции 66, вид документа равен 13 (графа 16)
    private static List<CheckError> Check_053(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var documentVid = forms[line].DocumentVid_DB ?? 0;
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (opCode != "66") return result;
        if (documentVid != 13)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "Для операции продления НСС сопровождающий документ - решение о продлении НСС (код 13).",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check054

    //Вид документа равен 1-15, 19 (графа 16)
    private static List<CheckError> Check_054(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var documentVid = forms[line].DocumentVid_DB ?? 0;
        const byte graphNumber = 16;
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        string[] applicableOperationCodes = 
        { 
            "11","12","15","17","18","21","22","25","27","28","29","31","32","35",
            "37","38","39","41","42","43","46","47","53","54","58","61","62","63",
            "64","65","67","68","71","72","73","74","75","81","82","83","84","85",
            "86","87","88","97","98","99"
        };
        if (!applicableOperationCodes.Contains(opCode)) return result;
        if (documentVid is 19 && !CheckNotePresence(notes, line, graphNumber))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "Для вида документа 19 необходимо указать примечание.",
                IsCritical = true
            });
        }
        if (documentVid is not (>= 1 and <= 15 or 19))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "Формат ввода данных не соответствует приказу. Необходимо указать вид документа " +
                          "в соответствии с таблицей 3 приложения №2 к приказу Госкорпорации \"Росатом\" " +
                          "от 07.10.2020 №1/13-НПА.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check055

    //Номер документа не пустой (колонка 17)
    private static List<CheckError> Check_055(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var documentNumber = ReplaceNullAndTrim(forms[line].DocumentNumber_DB);
        var valid = !string.IsNullOrWhiteSpace(documentNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentNumber_DB",
                Value = documentNumber,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check056

    //Дата документа корректно заполнена (графа 18)
    private static List<CheckError> Check_056(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var docDate = ReplaceNullAndTrim(forms[line].DocumentDate_DB);
        if (docDate is "" or "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = Convert.ToString(docDate),
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой.",
                IsCritical = true
            });
        }
        else if (!DateOnly.TryParse(docDate, out _))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = Convert.ToString(docDate),
                Message = "Формат ввода данных не соответствует приказу. Некорректно заполнена дата документа.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check057

    //Дата документа <= дате операции для всех кодов операции, кроме 10 и 41
    private static List<CheckError> Check_057(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "11","12","15","17","18","21","22","25","27","28","29","31","32","35",
            "37","38","39","42","43","46","47","53","54","58","61","62","63","64",
            "65","67","68","71","72","73","74","75","81","82","83","84","85","86",
            "87","88","97","98","99"
        };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var operationDate = ReplaceNullAndTrim(forms[line].OperationDate_DB);
        var documentDate = ReplaceNullAndTrim(forms[line].DocumentDate_DB);
        if (!applicableOperationCodes.Contains(operationCode)
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
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = documentDate,
                Message = "Дата документа не может быть позже даты операции.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check058

    //Дата документа = дате операции, если код операции 41
    private static List<CheckError> Check_058(List<Form11> forms, int line)
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
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = documentDate,
                Message = "Дата документа должна соответствовать дате операции.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check059

    //При коде операции 10, дата окончания ОП не позднее даты документа + 10 дней
    private static List<CheckError> Check_059(List<Form11> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        var documentDate = ReplaceNullAndTrim(forms[line].DocumentDate_DB);
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var endPerStr = ReplaceNullAndTrim(rep.EndPeriod_DB);
        if (opCode is not "10"
            || !DateOnly.TryParse(endPerStr, out var pEnd)
            || !DateOnly.TryParse(documentDate, out var docDate)
            || pEnd < docDate)
        {
            return result;
        }
        var valid = WorkdaysBetweenDates(docDate, pEnd) <= 10;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = documentDate,
                Message = "Дата окончания отчетного периода превышает дату акта инвентаризации более, чем на 10 рабочих дней."
            });
        }
        return result;
    }

    #endregion

    #region Check060

    //Код ОКПО поставщика/получателя 8/14 чисел и равен ОКПО отчитывающейся организации (графа 19)
    private static List<CheckError> Check_060(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "10", "11", "12", "15", "17", "18", "41", "42", "43", "46", "47", "58", "61", 
            "62", "65", "67", "68", "71", "72", "73", "74", "75", "97", "98", "99"
        };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        var repOkpo = okpoRepTerPodr is not ""
            ? okpoRepTerPodr
            : okpoRepJur;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        
        var valid = OkpoRegex.IsMatch(providerOrRecieverOkpo)
                    && providerOrRecieverOkpo == repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
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

    #region Check061

    //Код ОКПО поставщика/получателя 8/14 чисел и не равен ОКПО отчитывающейся организации (графа 19)
    private static List<CheckError> Check_061(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "25", "27", "28", "29", "35", "37", "38", "39", "63", "64" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        var repOkpo = okpoRepTerPodr is not ""
            ? okpoRepTerPodr
            : okpoRepJur;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var valid = OkpoRegex.IsMatch(providerOrRecieverOkpo)
                     && providerOrRecieverOkpo != repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
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

    #region Check062

    //Код ОКПО поставщика/получателя состоит из 8/14 чисел для определенных кодов операции (графа 19)
    private static List<CheckError> Check_062(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "21", "25", "27", "28", "29", "31", "35", "37", "38", "39", "63", "64" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OkpoRegex.IsMatch(providerOrRecieverOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Формат ввода данных не соответствует приказу. Укажите код ОКПО контрагента.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check063

    //Код ОКПО поставщика/получателя 8/14 чисел и не равен ОКПО отчитывающейся организации (графа 19)
    private static List<CheckError> Check_063(List<Form11> forms, List<Form10> forms10, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "66" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB).ToLower();
        var okpoRepJur = ReplaceNullAndTrim(forms10[0].Okpo_DB);
        var okpoRepTerPodr = ReplaceNullAndTrim(forms10[1].Okpo_DB);
        string[] correctNotes = ["прим.", "прим", "примечание", "примечания"];
        const byte graphNumber = 19;
        var repOkpo = okpoRepTerPodr is not ""
            ? okpoRepTerPodr
            : okpoRepJur;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        if (!(OkpoRegex.IsMatch(providerOrRecieverOkpo) && providerOrRecieverOkpo != repOkpo) 
            && !correctNotes.Contains(providerOrRecieverOkpo))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Для выбранного кода операции указывается код ОКПО организации, осуществившей продление НСС."
            });
        }

        if (!CheckNotePresence(notes, line, graphNumber))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "В примечаниях к данной ячейке необходимо указать ОКПО, ИНН и наименование организации."
            });
        }
        return result;
    }

    #endregion

    #region Check064

    //Код ОКПО поставщика/получателя состоит из 8/14 чисел или "минобороны" для определенных кодов операции (графа 19)
    private static List<CheckError> Check_064(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "22", "32" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var providerOrRecieverOkpo = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OkpoRegex.IsMatch(providerOrRecieverOkpo)
                    || providerOrRecieverOkpo.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
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

    #region Check065

    //Код ОКПО поставщика/получателя из ОКСМ (не Россия), для определенных кодов операции, с примечанием (графа 19)
    private static List<CheckError> Check_065(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 19;
        string[] applicableOperationCodes = { "81", "82", "83", "84", "85", "86", "87", "88" };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var providerOrRecieverOKPO = ReplaceNullAndTrim(forms[line].ProviderOrRecieverOKPO_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OKSM.Any(oksmEntry => oksmEntry["shortname"] == providerOrRecieverOKPO)
                    && !providerOrRecieverOKPO.Equals("россия", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOKPO,
                Message = "Формат ввода данных не соответствует приказу. " +
                          "Необходимо выбрать краткое наименование государства из ОКСМ.",
                IsCritical = true
            });
        }
        valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOKPO,
                Message = "Необходимо добавить примечание.",
                IsCritical = true
            });
        }
        return result;
    }

    #endregion

    #region Check066

    //При определенных кодах операции, код ОКПО перевозчика равен "-" (графа 20)
    private static List<CheckError> Check_066(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "10", "11", "12", "15", "17", "18", "41", "42", "43", "46", "47", "53", "54", 
            "58", "65", "66", "67", "68", "71", "72", "73", "74", "75", "97", "98"
        };
        var opCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var transporterOkpo = ReplaceNullAndTrim(forms[line].TransporterOKPO_DB);
        var valid = transporterOkpo is "-";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
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

    #region Check067

    //При определенных кодах операции, код ОКПО перевозчика равен 8/14 цифр (графа 20)
    private static List<CheckError> Check_067(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "21", "25", "27", "28", "29", "31", "35", "37", "38", "39", 
            "61", "62", "81", "82", "83", "84", "85", "86", "87", "88"
        };
        var operationCode = ReplaceNullAndTrim(forms[line].OperationCode_DB);
        var transporterOkpo = ReplaceNullAndTrim(forms[line].TransporterOKPO_DB);
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OkpoRegex.IsMatch(transporterOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
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

    #region Check068

    //Код ОКПО перевозчика состоит из 8/14 чисел или "минобороны" для определенных кодов операции (графа 20)
    private static List<CheckError> Check_068(List<Form11> forms, int line)
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
                FormNum = "form_11",
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

    #region Check069

    //Не пустое поле (графа 21)
    private static List<CheckError> Check_069(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var packName = ReplaceNullAndTrim(forms[line].PackName_DB);
        var valid = !string.IsNullOrWhiteSpace(packName);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PackName_DB",
                Value = packName,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check070

    //Не пустое поле (графа 22)
    private static List<CheckError> Check_070(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var packType = ReplaceNullAndTrim(forms[line].PackType_DB);
        var valid = !string.IsNullOrWhiteSpace(packType);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PackType_DB",
                Value = packType,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check071

    //Не пустое поле (колонка 23)
    private static List<CheckError> Check_071(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var packNumber = ReplaceNullAndTrim(forms[line].PackNumber_DB);
        var valid = !string.IsNullOrWhiteSpace(packNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PackNumber_DB",
                Value = packNumber,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check072

    //Если значение из списка, то необходимо заполнить форму 1.2 (колонки 21 и 22)
    private static List<CheckError> Check_072(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var packName = ReplaceNullAndTrim(forms[line].PackName_DB);
        var packType = ReplaceNullAndTrim(forms[line].PackType_DB);

        if (IOU11.Any(x => ReplaceSimilarCharsAndCheckToContains(x, packName)))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PackName_DB",
                Value = packName,
                Message = "Для данного наименования упаковки, должна быть заполнена форма 1.2 (информационное сообщение, не ошибка)."
            });
        }
        else if (IOU11.Any(x => ReplaceSimilarCharsAndCheckToContains(x, packType)))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PackType_DB",
                Value = packType,
                Message = "Для данного типа упаковки, должна быть заполнена форма 1.2 (информационное сообщение, не ошибка)."
            });
        }
        return result;
    }

    private static bool ReplaceSimilarCharsAndCheckToContains(string? str1, string? str2)
    {
        if (ReferenceEquals(str1, str2)) return true;

        if (str1 is null || str2 is null) return false;

        var snkRegex = SpecialSymbolsRegex();

        var tmp1 = snkRegex
            .Replace(str1, "")
            .ToLower()
            .Replace('а', 'a')
            .Replace('б', 'b')
            .Replace('в', 'b')
            .Replace('г', 'r')
            .Replace('е', 'e')
            .Replace('ё', 'e')
            .Replace('к', 'k')
            .Replace('м', 'm')
            .Replace('н', 'h')
            .Replace('о', 'o')
            .Replace('0', 'o')
            .Replace('р', 'p')
            .Replace('с', 'c')
            .Replace('т', 't')
            .Replace('у', 'y')
            .Replace('х', 'x');

        var tmp2 = snkRegex
            .Replace(str2, "")
            .ToLower()
            .Replace('а', 'a')
            .Replace('б', 'b')
            .Replace('в', 'b')
            .Replace('г', 'r')
            .Replace('е', 'e')
            .Replace('ё', 'e')
            .Replace('к', 'k')
            .Replace('м', 'm')
            .Replace('н', 'h')
            .Replace('о', 'o')
            .Replace('0', 'o')
            .Replace('р', 'p')
            .Replace('с', 'c')
            .Replace('т', 't')
            .Replace('у', 'y')
            .Replace('х', 'x');

        return tmp1.Contains(tmp2);
    }

    #endregion

    #endregion

    #region CheckEquilibriumRads

    /// <summary>
    /// Проверяет сет радионуклидов на равновесные, оставляет в нём только главные и возвращает флаг, были ли в сете равновесные радионуклиды.
    /// </summary>
    /// <param name="radsSet">Сет радионуклидов.</param>
    /// <returns>Флаг, были ли в сете равновесные радионуклиды.</returns>
    private static bool CheckEquilibriumRads(HashSet<string> radsSet)
    {
        var isEqRads = false;
        if (radsSet.Count <= 1) return isEqRads;
        isEqRads = EquilibriumRadionuclids.All(x =>
        {
            x = x.Replace(" ", string.Empty);
            var eqSet = x.Split(',').ToHashSet();
            if (radsSet.Intersect(eqSet).Any())
            {
                isEqRads = true;
                radsSet.ExceptWith(eqSet.Skip(1));
            }
            return isEqRads;
        });
        return isEqRads;
    }

    #endregion

    #region GraphsList

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
        { "CreatorOKPO_DB", "10 - код ОКПО изготовителя" },
        { "CreationDate_DB", "11 - Дата выпуска" },
        { "Category_DB", "12 - Категория ЗРИ" },
        { "SignedServicePeriod_DB", "13 - НСС, месяцев" },
        { "PropertyCode_DB", "14 - Код формы собственности" },
        { "Owner_DB", "15 - Код ОКПО правообладателя" },
        { "DocumentVid_DB", "16 - Вид документа" },
        { "DocumentNumber_DB", "17 - Номер документа" },
        { "DocumentDate_DB", "18 - Дата документа" },
        { "ProviderOrRecieverOKPO_DB", "19 - Код ОКПО поставщика или получателя" },
        { "TransporterOKPO_DB", "20 - Код ОКПО перевозчика" },
        { "PackName_DB", "21 - Наименование прибора, УКТ, упаковки" },
        { "PackType_DB", "22 - Тип прибора, УКТ, упаковки" },
        { "PackNumber_DB", "23 - Номер прибора, УКТ, упаковки" },
    };

    #endregion

    #region Properties

    //Заслужил собственную константу, поскольку используется в нескольких проверках (21, 48, 49 и 50).
    private static readonly string[] OperationCode_DB_Check021 =
    [
        "11", "12", "15", "28", "38", "41", "63", "64", "65", "73", "81", "85", "88"
    ];  

    private static readonly string[] Radionuclids_YAM_DB_Valids =
    [
        "америций-241",
        "америций-243",
        "калифорний-252",
        "литий-6",
        "нептуний-237",
        "плутоний-234",
        "плутоний-235",
        "плутоний-236",
        "плутоний-237",
        "плутоний-238",
        "плутоний-239",
        "плутоний-240",
        "плутоний-241",
        "плутоний-242",
        "плутоний-243",
        "плутоний-244",
        "плутоний-245",
        "плутоний-246",
        "сумма радионуклидов урана",
        "торий-226",
        "торий-227",
        "торий-228",
        "торий-229",
        "торий-230",
        "торий-231",
        "торий-232",
        "торий-234",
        "торий естественный",
        "торий-естественный",
        "торий природный",
        "торий-природный",
        "тритий",
        "уран-230",
        "уран-231",
        "уран-232",
        "уран-233",
        "уран-234",
        "уран-235",
        "уран-236",
        "уран-237",
        "уран естественный",
        "уран-естественный",
        "уран-238",
        "уран-239",
        "уран-240",
        "уран природный",
        "уран-природный"
    ];

    private static readonly string[] Type_DB_Valids = Array.Empty<string>();

    #endregion

    #region Regex

    [GeneratedRegex(@"[\\/:*?""<>|.,_\-;:\s+]")]
    private static partial Regex SpecialSymbolsRegex();

    #endregion
}