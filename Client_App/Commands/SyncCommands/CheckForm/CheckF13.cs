using Models.CheckForm;
using Models.Collections;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using Models.Forms;
using System.Linq;

namespace Client_App.Commands.SyncCommands.CheckForm;

public abstract class CheckF13 : CheckBase
{
    #region Check_Total

    public static List<CheckError> Check_Total(Reports reps, Report rep)
    {
        var currentFormLine = 0;
        List<CheckError> errorList = new();
        LoadDictionaries();
        var formsList = rep.Rows13.ToList<Form13>();
        errorList.AddRange(Check_002(rep));
        errorList.AddRange(Check_003(formsList, rep));
        errorList.AddRange(Check_004(formsList));
        foreach (var key in rep.Rows13)
        {
            var form = (Form13)key;
            
            var notes = rep.Notes.ToList<Note>();
            var forms10 = reps.Master_DB.Rows10.ToList<Form10>();
            errorList.AddRange(Check_001(formsList, currentFormLine));
            errorList.AddRange(Check_005(formsList, currentFormLine));
            errorList.AddRange(Check_006(formsList, currentFormLine));
            errorList.AddRange(Check_007(formsList, currentFormLine));
            errorList.AddRange(Check_008(formsList, currentFormLine));
            errorList.AddRange(Check_009(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_010(formsList, currentFormLine));
            errorList.AddRange(Check_011(formsList, currentFormLine));
            errorList.AddRange(Check_012(formsList, notes, currentFormLine));
            errorList.AddRange(Check_013(formsList, currentFormLine));
            errorList.AddRange(Check_014(formsList, currentFormLine));
            errorList.AddRange(Check_015(formsList, currentFormLine));
            errorList.AddRange(Check_016(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_017(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_018(formsList, currentFormLine));
            errorList.AddRange(Check_019(formsList, currentFormLine));
            errorList.AddRange(Check_020(formsList, currentFormLine));
            errorList.AddRange(Check_021(formsList, currentFormLine));
            errorList.AddRange(Check_022(formsList, currentFormLine));
            errorList.AddRange(Check_023(formsList, currentFormLine));
            errorList.AddRange(Check_024(formsList, rep, currentFormLine));
            errorList.AddRange(Check_025(formsList, rep, currentFormLine));
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
            errorList.AddRange(Check_036(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_037(formsList, currentFormLine));
            errorList.AddRange(Check_038(formsList, notes, currentFormLine));
            errorList.AddRange(Check_039(formsList, currentFormLine));
            errorList.AddRange(Check_040(formsList, currentFormLine));
            errorList.AddRange(Check_041(formsList, currentFormLine));
            errorList.AddRange(Check_042(formsList, currentFormLine));
            errorList.AddRange(Check_043(formsList, notes, currentFormLine));
            errorList.AddRange(Check_044(formsList, notes, currentFormLine));
            errorList.AddRange(Check_045(formsList, notes, currentFormLine));
            errorList.AddRange(Check_046(formsList, currentFormLine));
            errorList.AddRange(Check_047(formsList, currentFormLine));
            errorList.AddRange(Check_048(formsList, notes, currentFormLine));
            errorList.AddRange(Check_049(formsList, currentFormLine));
            errorList.AddRange(Check_050(formsList, currentFormLine));
            errorList.AddRange(Check_051(formsList, currentFormLine));
            errorList.AddRange(Check_052(formsList, currentFormLine));
            errorList.AddRange(Check_053(formsList, currentFormLine));
            errorList.AddRange(Check_054(formsList, rep, currentFormLine));
            errorList.AddRange(Check_055(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_056(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_057(formsList, currentFormLine));
            errorList.AddRange(Check_058(formsList, notes, currentFormLine));
            errorList.AddRange(Check_059(formsList, currentFormLine));
            errorList.AddRange(Check_060(formsList, notes, currentFormLine));
            errorList.AddRange(Check_061(formsList, currentFormLine));
            errorList.AddRange(Check_062(formsList, currentFormLine));
            errorList.AddRange(Check_063(formsList, currentFormLine));
            errorList.AddRange(Check_064(formsList, currentFormLine));
            currentFormLine++;
        }
        var index = 0;
        foreach (var error in errorList)
        {
            error.Column = GraphsList.GetValueOrDefault(error.Column, error.Column);
            index++;
            error.Index = index;
        }
        return errorList;
    }

    #endregion

    #region Checks

    #region Check001

    //Заглушка
    private static List<CheckError> Check_001(List<Form13> forms, int line)
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
        if (!DateOnly.TryParse(stPerStr, out var stPer))
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = string.Empty,
                Column = string.Empty,
                Value = stPerStr,
                Message = "Некорректно введена дата начала периода."
            });
        }
        if (!DateOnly.TryParse(endPerStr, out var endPer))
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = string.Empty,
                Column = string.Empty,
                Value = endPerStr,
                Message = "Некорректно введена дата окончания периода."
            });
        }
        if (DateOnly.TryParse(rep.StartPeriod_DB, out _)
            && DateOnly.TryParse(rep.EndPeriod_DB, out _)
            && stPer > endPer)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
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
    private static List<CheckError> Check_003(List<Form13> forms13, Report rep)
    {
        var forms = forms13.Cast<Form1>().ToList();
        return CheckRepPeriod(forms, rep);
    }

    #endregion

    #region Check004

    //Наличие строк дубликатов (графы 2-17)
    private static List<CheckError> Check_004(List<Form13> forms)
    {
        List<CheckError> result = new();
        HashSet<int> duplicatesLinesSet = new();
        var comparator = new CustomNullStringWithTrimComparer();
        for (var i = 0; i < forms.Count; i++)
        {
            var currentForm = forms[i];
            for (var j = i + 1; j < forms.Count; j++)
            {
                var formToCompare = forms[j];
                var isDuplicate = comparator.Compare(formToCompare.OperationCode_DB, currentForm.OperationCode_DB) == 0
                                  && comparator.Compare(formToCompare.OperationDate_DB, currentForm.OperationDate_DB) == 0
                                  && comparator.Compare(formToCompare.PassportNumber_DB, currentForm.PassportNumber_DB) == 0
                                  && comparator.Compare(formToCompare.Type_DB, currentForm.Type_DB) == 0
                                  && comparator.Compare(formToCompare.Radionuclids_DB, currentForm.Radionuclids_DB) == 0
                                  && comparator.Compare(formToCompare.FactoryNumber_DB, currentForm.FactoryNumber_DB) == 0
                                  && comparator.Compare(formToCompare.Activity_DB, currentForm.Activity_DB) == 0
                                  && comparator.Compare(formToCompare.CreatorOKPO_DB, currentForm.CreatorOKPO_DB) == 0
                                  && comparator.Compare(formToCompare.CreationDate_DB, currentForm.CreationDate_DB) == 0
                                  && formToCompare.AggregateState_DB == currentForm.AggregateState_DB
                                  && formToCompare.PropertyCode_DB == currentForm.PropertyCode_DB
                                  && comparator.Compare(formToCompare.Owner_DB, currentForm.Owner_DB) == 0
                                  && formToCompare.DocumentVid_DB == currentForm.DocumentVid_DB
                                  && comparator.Compare(formToCompare.DocumentNumber_DB, currentForm.DocumentNumber_DB) == 0
                                  && comparator.Compare(formToCompare.DocumentDate_DB, currentForm.DocumentDate_DB) == 0
                                  && comparator.Compare(formToCompare.ProviderOrRecieverOKPO_DB, currentForm.ProviderOrRecieverOKPO_DB) == 0;
                if (!isDuplicate) continue;
                duplicatesLinesSet.Add(i + 1);
                duplicatesLinesSet.Add(j + 1);
            }
        }
        var dupStrByGroups = ConvertSequenceSetToRangeStringList(duplicatesLinesSet);
        foreach (var group in dupStrByGroups)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = group,
                Column = "2 - 17",
                Value = "",
                Message = $"Данные граф 2-17 в строках {group} продублированы. Следует проверить правильность предоставления данных."
            });
        }
        return result;
    }

    #endregion

    #region Check005

    //Нумерация строк (графа 1)
    private static List<CheckError> Check_005(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (forms[line].NumberInOrder_DB != line + 1)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "NumberInOrder_DB",
                Value = forms[line].Id.ToString(),
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
    private static List<CheckError> Check_006(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var operationCodeValid = new[]
        {
            "10", "11", "12", "15", "17", "18", "21", "22", "25", "27", "28", "29", "31", "32", "35", "37", "38", 
            "39", "41", "42", "43", "46", "47", "48", "53", "54", "58", "61", "62", "63", "64", "65", "67", "68", 
            "71", "72", "73", "74", "75", "81", "82", "83", "84", "85", "86", "87", "88", "97", "98", "99"
        };
        var valid = operationCodeValid.Contains(operationCode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = $"Код операции {operationCode} не может быть использован в форме 1.3."
            });
        }
        return result;
    }

    #endregion

    #region Check007

    //TODO
    //Соответствие СНК
    private static List<CheckError> Check_007(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check008

    //Для кодов операции 12, 42, в радионуклидах должен быть указан хоть один из списка (колонка 6)
    private static List<CheckError> Check_008(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var radionuclid = (forms[line].Radionuclids_DB ?? string.Empty).Trim();
        string[] applicableOperationCodes = { "12", "42" };
        var radionuclidValid = new[]
        {
            "плутоний-238", "плутоний-239", "плутоний-240", "уран-233", "уран-235", "уран-238", "нептуний-237",
            "америций-241", "америций 243", "калифорний-252", "торий-232", "литий-6", "дейтерий", "тритий"
        };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = radionuclidValid
            .Any(nuclid => radionuclid
                .Contains(nuclid, StringComparison.CurrentCultureIgnoreCase));
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = radionuclid,
                Message = "В графе 6 не представлены сведения о радионуклидах, которые могут быть отнесены к ЯМ. " +
                          "Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check009

    //Код ОКПО правообладателя равен коду ОКПО юридического лица или обособленного подразделения (графа 13)
    private static List<CheckError> Check_009(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "11", "12", "15", "28", "38", "41", "48", "63", "64", "65", "73", "81", "85", "88"
        };
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var owner = forms[line].Owner_DB;
        var okpoRepJur = (forms10[0].Okpo_DB ?? string.Empty).Trim();
        var okpoRepTerPodr = (forms10[1].Okpo_DB ?? string.Empty).Trim();
        var valid = !string.IsNullOrWhiteSpace(owner) 
                    && (owner == okpoRepTerPodr || owner == okpoRepJur);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Уточните правообладателя ОРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check010

    //TODO
    //Наличие в формах 1.5/1.6 кода 44
    private static List<CheckError> Check_010(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check011

    //TODO
    //Наличие в других формах кода 35
    private static List<CheckError> Check_011(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check012

    //Наличие примечания (графа 2)
    private static List<CheckError> Check_012(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 2;
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        string[] applicableOperationCodes = { "29", "39", "97", "98", "99" };
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = opCode,
                Message = "Необходимо дать пояснение об осуществленной операции."
            });
        }
        return result;
    }

    #endregion

    #region Check013

    //TODO
    //Проверка наличия учётной единицы в организации
    private static List<CheckError> Check_013(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check014

    //TODO
    //Наличие в ранних отчётах кода операции 27
    private static List<CheckError> Check_014(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check015

    //TODO
    //Наличие отчёта 1.6
    private static List<CheckError> Check_015(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check016

    //ОКПО организации должен быть равен ОКПО поставщика или получателя (графа 17)
    private static List<CheckError> Check_016(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var providerOrRecieverOkpo = (forms[line].ProviderOrRecieverOKPO_DB ?? string.Empty).Trim();
        var okpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB.Trim()
            : (forms10[0].Okpo_DB ?? string.Empty).Trim();
        string[] applicableOperationCodes = { "53" };
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = providerOrRecieverOkpo != string.Empty && providerOrRecieverOkpo.Equals(okpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = opCode,
                Message = "В графе 17 необходимо указать код ОКПО своей организации. " +
                          "В случае, если зарядку/разрядку осуществляла подрядная организация, " +
                          "следует использовать код операции 54."
            });
        }
        return result;
    }

    #endregion

    #region Check017

    //ОКПО организации не должен быть равен ОКПО поставщика или получателя (графа 17)
    private static List<CheckError> Check_017(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var providerOrRecieverOkpo = (forms[line].ProviderOrRecieverOKPO_DB ?? string.Empty).Trim();
        var okpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB.Trim()
            : (forms10[0].Okpo_DB ?? string.Empty).Trim();
        string[] applicableOperationCodes = { "54" };
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = providerOrRecieverOkpo != string.Empty && !providerOrRecieverOkpo.Equals(okpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = opCode,
                Message = "В графе 17 необходимо указать код ОКПО подрядной организации. В случае, " +
                          "если зарядка/разрядка осуществлялась силами отчитывающейся организации, " +
                          "следует использовать код операции 53."
            });
        }
        return result;
    }

    #endregion

    #region Check018

    //TODO
    //Наличие кода 46 в других формах
    private static List<CheckError> Check_018(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check019

    //TODO
    //Наличие кода 61 в других формах
    private static List<CheckError> Check_019(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check020

    //TODO
    //Наличие формы 1.1 с кодом 65
    private static List<CheckError> Check_020(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check021

    //Код ОКПО изготовителя состоит из 8/14 чисел для определенных кодов операции (графа 9)
    private static List<CheckError> Check_021(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "81", "88" };
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var creatorOkpo = (forms[line].CreatorOKPO_DB ?? string.Empty).Trim();
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OkpoRegex.IsMatch(creatorOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = creatorOkpo,
                Message = "Код используется для предоставления сведений о ОРИ, произведенных в Российской Федерации."
            });
        }
        return result;
    }

    #endregion

    #region Check022

    //Код ОКПО изготовителя из ОКСМ, но не Россия (графа 9)
    private static List<CheckError> Check_022(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var creatorOkpo = (forms[line].CreatorOKPO_DB ?? string.Empty).Trim();
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        string[] applicableOperationCodes = { "83", "84", "85", "86" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OKSM.Any(oksmEntry => oksmEntry["shortname"] == creatorOkpo) 
                    && creatorOkpo.ToLower() is not "россия";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = creatorOkpo,
                Message = "Код используется для предоставления сведений об ОРИ, произведенных за пределами Российской Федерации. " +
                          "Для импортированных ОРИ необходимо указать краткое наименование государства в соответствии с ОКСМ."
            });
        }
        return result;
    }

    #endregion

    #region Check023

    //Дата операции корректно заполнена (графа 3)
    private static List<CheckError> Check_023(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var opDate = (forms[line].OperationDate_DB ?? string.Empty).Trim();
        var valid = opDate is not ("" or "-");
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = opDate,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        else if (!DateOnly.TryParse(opDate, out _))
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = opDate,
                Message = "Формат ввода данных не соответствует приказу. Некорректно заполнена дата операции."
            });
        }
        return result;
    }

    #endregion

    #region Check024

    //Дата документа входит в отчетный период (графа 3)
    private static List<CheckError> Check_024(List<Form13> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var stPerStr = (rep.StartPeriod_DB ?? string.Empty).Trim();
        var endPerStr = (rep.EndPeriod_DB ?? string.Empty).Trim();
        var opDateStr = (forms[line].OperationDate_DB ?? string.Empty).Trim();
        if (opCode is "10" 
            || !DateOnly.TryParse(stPerStr, out var pStart)
            || !DateOnly.TryParse(endPerStr, out var pEnd)
            || !DateOnly.TryParse(opDateStr, out var opDate)) return result;
        var valid = opDate >= pStart && opDate <= pEnd;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = opDateStr,
                Message = "Дата операции не входит в отчетный период."
            });
        }
        return result;
    }

    #endregion

    #region Check025

    //При коде операции 10, дата документа должна попадать в отчетный период
    private static List<CheckError> Check_025(List<Form13> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var stPerStr = (rep.StartPeriod_DB ?? string.Empty).Trim();
        var endPerStr = (rep.EndPeriod_DB ?? string.Empty).Trim();
        var docDateStr = (forms[line].DocumentDate_DB ?? string.Empty).Trim();
        if (opCode is not "10" 
            || !DateOnly.TryParse(docDateStr, out var docDate)
            || !DateOnly.TryParse(stPerStr, out var stDate)
            || !DateOnly.TryParse(endPerStr, out var endDate))
        {
            return result;
        }
        if (docDate < stDate)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDateStr,
                Message = $"Дата документа не входит в отчетный период. " +
                          $"Дата документа ({docDate} не может быть ранее даты начала отчётного периода {stDate}.)"
            });
        }
        if (docDate > endDate)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDateStr,
                Message = $"Дата документа не входит в отчетный период. " +
                          $"Дата документа ({docDate} не может быть позднее даты окончания отчётного периода {endDate}.)"
            });
        }
        return result;
    }

    #endregion

    #region Check026

    //Номер паспорта не пустая строка (графа 4)
    private static List<CheckError> Check_026(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var passportNumber = (forms[line].PassportNumber_DB ?? string.Empty).Trim();
        var valid = passportNumber != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PassportNumber_DB",
                Value = passportNumber,
                Message = "Графа не может быть пустой. Укажите номера паспорта. " +
                          "В случае, если номер отсутствует укажите символ \"-\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check027

    //Тип не пустое поле (графа 5)
    private static List<CheckError> Check_027(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var type = (forms[line].Type_DB ?? string.Empty).Trim();
        var valid = type != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Type_DB",
                Value = type,
                Message = "Формат ввода не соответствует приказу. Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check028

    //Каждый из радионуклидов не короткоживущий, >=60 суток (графа 6)
    private static List<CheckError> Check_028(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var rads = forms[line].Radionuclids_DB ?? string.Empty;
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();
        foreach (var rad in radsSet)
        {
            var phEntry = R.FirstOrDefault(phEntry => phEntry["name"] == rad);
            if (phEntry is null) return result;
            var unit = phEntry["unit"];
            var value = phEntry["value"].Replace('.', ',');
            if (!TryParseDoubleExtended(value, out var halfLife)) continue;
            switch (unit)
            {
                case "лет":
                    halfLife *= 365;
                    break;
                case "сут":
                    break;
                case "час":
                    halfLife /= 24;
                    break;
                case "мин":
                    halfLife /= 1440;
                    break;
                default:
                    continue;
            }
            if (halfLife >= 60 && rad != "иод-125") return result;  //Если встречаем хоть один не короткоживущий - ошибки нет
        }
        result.Add(new CheckError
        {
            FormNum = "form_14",
            Row = (line + 1).ToString(),
            Column = "Radionuclids_DB",
            Value = rads,
            Message = $"Все указанные радионуклиды являются короткоживущими. " +
                      $"ОРИ на основе короткоживущих радионуклидов (включая иод-125) учитываются в формах 1.9 и 2.12."
        });
        return result;
    }

    #endregion

    #region Check029

    //Радионуклиды заполнены (графа 6)
    private static List<CheckError> Check_029(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var radionuclids = (forms[line].Radionuclids_DB ?? string.Empty).Trim();
        var valid = radionuclids != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = radionuclids,
                Message = "Графа должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check030

    //У радионуклидов в качестве разделителя использовать ";", сверка со справочником (графа 6)
    private static List<CheckError> Check_030(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var rads = (forms[line].Radionuclids_DB ?? string.Empty).Trim();
        if (rads is "" or "-") return result;
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
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = rads,
                Message = "Формат ввода данных не соответствует приказу. Наименование радионуклида указывается названием " +
                          "химического элемента на русском языке с указанием через дефис массового числа изотопа."
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
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = rads,
                Message = "Формат ввода данных не соответствует приказу. " +
                          "Наименование радионуклида указывается названием химического элемента на русском языке " +
                          "с указанием через дефис массового числа изотопа, радионуклиды перечисляются через точку с запятой."
            });
        }
        return result;
    }

    #endregion

    #region Check031

    //Сверка с МЗА для одного радионуклида (графа 8)
    private static List<CheckError> Check_031(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var rad = (forms[line].Radionuclids_DB ?? string.Empty).Trim().ToLower();
        var activity = ConvertStringToExponential(forms[line].Activity_DB);

        if (R.All(x => x["name"] != rad)
            || !TryParseDoubleExtended(activity, out var activityDoubleValue)
            || activityDoubleValue <= 0) return result;

        var mza = R.First(x => x["name"] == rad)["MZA"];
        if (!TryParseDoubleExtended(mza, out var mzaDoubleValue)) return result;

        var valid = activityDoubleValue >= mzaDoubleValue;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Сведения, указанные в графе 8 (Суммарная активность) ниже МЗА, " +
                          "ОРИ не является объектом учёта СГУК РВ и РАО. Сверьте сведения, указанные в отчёте, с паспортом на ОРИ. " +
                          "Обратите внимание на размерность активности, указанной в паспорте на ОРИ (Бк, МБк, ТБк)."
            });
        }
        return result;
    }

    #endregion

    #region Check032

    //Сверка с МЗА для нескольких радионуклидов (графа 8)
    private static List<CheckError> Check_032(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var rads = (forms[line].Radionuclids_DB ?? string.Empty).Trim();
        var activity = ConvertStringToExponential(forms[line].Activity_DB);
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();
        var isEqRads = EquilibriumRadionuclids.Any(x =>
        {
            x = x.Replace(" ", "");
            var eqRadsArray = x.Split(',');
            return radsSet
                .All(rad => eqRadsArray
                    .Contains(rad) && radsSet.Count == eqRadsArray.Length);
        });

        if (radsSet.Count == 1
            || isEqRads
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

        var valid = activityDoubleValue >= minimumActivity;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Сведения, указанные в графе 8 (Суммарная активность) ниже МЗА. " +
                          "Сверьте сведения, указанные в отчёте, с паспортом на ОРИ. " +
                          "Обратите внимание на размерность активности, указанной в паспорте на ОРИ (Бк, МБк, ТБк)."
            });
        }
        return result;
    }

    #endregion

    #region Check033

    //Сверка с МЗА для равновесных радионуклидов
    private static List<CheckError> Check_033(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var rads = (forms[line].Radionuclids_DB ?? string.Empty).Trim();
        var activity = ConvertStringToExponential(forms[line].Activity_DB);
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();
        var isEqRads = EquilibriumRadionuclids.Any(x =>
        {
            x = x.Replace(" ", "");
            var eqSet = x.Split(',');
            return radsSet.All(rad => eqSet.Contains(rad));
        });

        if (radsSet.Count == 1
            || !isEqRads
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

        var valid = activityDoubleValue >= mzaDoubleValue;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Сведения, указанные в графе 8 (Суммарная активность) ниже МЗА, " +
                          "ОРИ не является объектом учёта СГУК РВ и РАО. Сверьте сведения, указанные в отчёте, с паспортом на ОРИ. " +
                          "Обратите внимание на размерность активности, указанной в паспорте на ОРИ (Бк, МБк, ТБк)."
            });
        }
        return result;
    }

    #endregion

    #region Check034

    //Номер не пустая строка (графа 7)
    private static List<CheckError> Check_034(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var factoryNumber = (forms[line].FactoryNumber_DB ?? string.Empty).Trim();
        var valid = factoryNumber != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "FactoryNumber_DB",
                Value = factoryNumber,
                Message = "Заполните сведения о заводском номере ОРИ. " +
                          "Если номер отсутствует, в ячейке следует указать символ \"-\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check035

    //"Суммарная активность, Бк" положительное число, <= 10е+20 (графа 8)
    private static List<CheckError> Check_035(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var activity = ConvertStringToExponential(forms[line].Activity_DB);
        if (!TryParseDoubleExtended(activity, out var activityReal))
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Проверьте правильность предоставления сведений по активности."
            });
        }
        else if (activityReal <= 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Суммарная активность должна быть более нуля. Проверьте правильность введённых данных."
            });
        }
        else if (activityReal > 10e+20)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Указано слишком большое значение суммарной активности. Проверьте правильность введённых данных."
            });
        }
        return result;
    }

    #endregion

    #region Check036

    //"код ОКПО изготовителя" = "ОКПО организации" для кодов операции 11 (графа 9)
    private static List<CheckError> Check_036(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "11" };
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var creatorOkpo = (forms[line].CreatorOKPO_DB ?? string.Empty).Trim();
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB.Trim()
            : (forms10[0].Okpo_DB ?? string.Empty).Trim();
        var valid = creatorOkpo == repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = creatorOkpo,
                Message = "Проверьте код ОКПО организации-изготовителя."
            });
        }
        return result;
    }

    #endregion

    #region Check037

    //Код ОКПО изготовителя 8/14 цифр (графа 9)
    private static List<CheckError> Check_037(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var creatorOkpo = (forms[line].CreatorOKPO_DB ?? string.Empty).Trim();
        var valid = OkpoRegex.IsMatch(creatorOkpo)
                        || creatorOkpo is "прим."
                        || OKSM.Any(oksmEntry => oksmEntry["shortname"] == creatorOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = creatorOkpo,
                Message = "Формат ввода данных не соответствует приказу. Укажите код ОКПО организации изготовителя."
            });
        }
        return result;
    }

    #endregion

    #region Check038

    // Если в "код ОКПО изготовителя" (колонка 9) указано примечание, то проверяется наличие примечания для данной строки
    private static List<CheckError> Check_038(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] creatorOkpoValid = { "прим.", "прим", "примечание", "примечания" };
        var creatorOkpo = (forms[line].CreatorOKPO_DB ?? string.Empty).Trim();
        if (OKSM.All(oksmEntry => oksmEntry["shortname"] != creatorOkpo.ToLower()) 
            && !creatorOkpoValid.Contains(creatorOkpo.ToLower())) return result;
        const byte graphNumber = 9;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = creatorOkpo,
                Message = "Необходимо указать в примечании наименование и адрес организации-изготовителя ОРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check039

    //Дата выпуска корректно заполнена (графа 10)
    private static List<CheckError> Check_039(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var creationDate = (forms[line].CreationDate_DB ?? string.Empty).Trim();
        var valid = creationDate is not ("" or "-");
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreationDate_DB",
                Value = creationDate,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        else if (!DateOnly.TryParse(creationDate, out _))
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreationDate_DB",
                Value = creationDate,
                Message = "Формат ввода данных не соответствует приказу. Некорректно заполнена дата выпуска."
            });
        }
        return result;
    }

    #endregion

    #region Check040

    //Дата выпуска (графа 10) <= Дате операции (графа 3)
    private static List<CheckError> Check_040(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var opDateStr = (forms[line].OperationDate_DB ?? string.Empty).Trim();
        var creationDateStr = (forms[line].CreationDate_DB ?? string.Empty).Trim();
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
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreationDate_DB",
                Value = createDate.ToShortDateString(),
                Message = "Дата выпуска не может быть позже даты операции."
            });
        }
        return result;
    }

    #endregion

    #region Check041

    //Агрегатное состояния равно 1-3 (графа 11)
    private static List<CheckError> Check_041(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var aggregateState = forms[line].AggregateState_DB;
        var valid = aggregateState is >= 1 and <= 3;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "AggregateState_DB",
                Value = Convert.ToString(aggregateState),
                Message = "Выберите значение для агрегатного состояния: 1 - твердое, 2 - жидкое, 3 - газообразное."
            });
        }
        return result;
    }

    #endregion

    #region Check042

    //Код формы собственности от 1 до 6, 9 (графа 12)
    private static List<CheckError> Check_042(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var propertyCode = forms[line].PropertyCode_DB;
        if (propertyCode is not (>= 1 and <= 6 or 9))
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = Convert.ToString(propertyCode),
                Message = "Формат ввода данных не соответствует приказу. " +
                          "Выберите идентификатор, соответствующий форме собственности ОРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check043

    // Если код формы собственности 2, необходимо указать примечание к ячейке (графа 12)
    private static List<CheckError> Check_043(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 12;
        var propertyCode = forms[line].PropertyCode_DB;
        if (propertyCode is not 2) return result;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = Convert.ToString(propertyCode),
                Message = "Необходимо указать в примечании наименование субъекта Российской Федерации, " +
                          "в собственности которого находится объект учета."
            });
        }
        return result;
    }

    #endregion

    #region Check044

    // Если код формы собственности 3, необходимо указать примечание к ячейке (графа 12)
    private static List<CheckError> Check_044(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 12;
        var propertyCode = forms[line].PropertyCode_DB;
        if (propertyCode is not 3) return result;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = Convert.ToString(propertyCode),
                Message = "Необходимо указать в примечании наименование муниципального образования, " +
                          "в собственности которого находится объект учета."
            });
        }
        return result;
    }

    #endregion

    #region Check045

    // Если код формы собственности 9, необходимо указать примечание к ячейке (графа 12)
    private static List<CheckError> Check_045(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 12;
        var propertyCode = forms[line].PropertyCode_DB;
        if (propertyCode is not 9) return result;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = Convert.ToString(propertyCode),
                Message = "Необходимо указать в примечании наименование и адрес правообладателя " +
                          "(собственника или обладателя вещного права) объекта учета."
            });
        }
        return result;
    }

    #endregion

    #region Check046

    //8 или 14 чисел (графа 13) если код формы собственности от 1 до 4 (графа 12)
    private static List<CheckError> Check_046(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var owner = (forms[line].Owner_DB ?? string.Empty).Trim();
        var propertyCode = forms[line].PropertyCode_DB;
        if (propertyCode is < 1 or > 4) return result;
        var valid = OkpoRegex.IsMatch(owner);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Формат ввода данных не соответствует приказу. В случае, " +
                          "если правообладатель российское юридическое лицо, необходимо указать его код ОКПО."
            });
        }
        return result;
    }

    #endregion

    #region Check047

    //Правообладатель (графа 13) из справочника ОКСМ, если код формы собственности 5 (графа 12)
    private static List<CheckError> Check_047(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var propertyCode = forms[line].PropertyCode_DB;
        var owner = (forms[line].Owner_DB ?? string.Empty).Trim();
        if (propertyCode is not 5) return result;
        var valid = OKSM.Any(oksmEntry => oksmEntry["shortname"] == owner)
                    && !owner.Equals("россия", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Формат ввода данных не соответствует приказу. " +
                          "В случае, если правообладатель иностранное государство, " +
                          "необходимо указать его краткое наименование в соответствии с ОКСМ."
            });
        }
        return result;
    }

    #endregion

    #region Check048

    //Правообладатель (графа 13) из справочника ОКСМ и наличие примечания, если код формы собственности 6 (графа 12)
    private static List<CheckError> Check_048(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 13;
        var propertyCode = forms[line].PropertyCode_DB;
        var owner = (forms[line].Owner_DB ?? string.Empty).Trim();
        if (propertyCode is not 6) return result;
        if (OKSM.All(oksmEntry => oksmEntry["shortname"] != owner)
            || owner.Equals("россия", StringComparison.CurrentCultureIgnoreCase))
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Формат ввода данных не соответствует приказу. В графе \"код ОКПО правообладателя\" " +
                          "необходимо указать краткое наименование страны в соответствии с ОКСМ."
            });
        }
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Необходимо указать в примечании наименование и адрес правообладателя " +
                          "(собственника или обладателя иного вещного права) на ОРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check049

    //Вид документа от 1 до 15, 19 (графа 14)
    private static List<CheckError> Check_049(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var documentVid = forms[line].DocumentVid_DB;
        var valid = documentVid is >= 1 and <= 15 or 19;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "Формат ввода данных не соответствует приказу. Необходимо указать вид документа " +
                          "в соответствии с таблицей 3 приложения №2 к приказу Госкорпорации \"Росатом\" " +
                          "от 07.12.2020 №1/13-НПА."
            });
        }
        valid = opCode != "10" || documentVid == 1;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "При коде операции инвентаризации вид документа должен быть равен 1."
            });
        }
        return result;
    }

    #endregion

    #region Check050

    //Номер документа не пустой (графа 15)
    private static List<CheckError> Check_050(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var documentNumber = (forms[line].DocumentNumber_DB ?? string.Empty).Trim();
        var valid = documentNumber != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentNumber_DB",
                Value = documentNumber,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check051

    //Дата документа корректно заполнена (графа 16)
    private static List<CheckError> Check_051(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var docDate = (forms[line].DocumentDate_DB ?? string.Empty).Trim();
        if (docDate is "" or "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDate,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        else if (!DateOnly.TryParse(docDate, out _))
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDate,
                Message = "Формат ввода данных не соответствует приказу. Некорректно заполнена дата документа."
            });
        }
        return result;
    }

    #endregion

    #region Check052

    //Дата документа <= дате операции для всех кодов операции, кроме 10 и 41
    private static List<CheckError> Check_052(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var opDateStr = (forms[line].OperationDate_DB ?? string.Empty).Trim();
        var docDateStr = (forms[line].DocumentDate_DB ?? string.Empty).Trim();
        if (opCode is "10" or "41"
            || !DateOnly.TryParse(docDateStr, out var docDate)
            || !DateOnly.TryParse(opDateStr, out var opDate))
        {
            return result;
        }
        var valid = docDate <= opDate;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDateStr,
                Message = "Дата документа не может быть позже даты операции."
            });
        }
        return result;
    }

    #endregion

    #region Check053

    //Дата документа = дате операции, если код операции 41
    private static List<CheckError> Check_053(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var opDateStr = (forms[line].OperationDate_DB ?? string.Empty).Trim();
        var docDateStr = (forms[line].DocumentDate_DB ?? string.Empty).Trim();
        if (opCode is not "41"
            || !DateOnly.TryParse(docDateStr, out var docDate)
            || !DateOnly.TryParse(opDateStr, out var opDate))
        {
            return result;
        }
        var valid = docDate == opDate;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDateStr,
                Message = "Дата документа должна соответствовать дате операции."
            });
        }
        return result;
    }

    #endregion

    #region Check054

    //При коде операции 10, дата окончания ОП не позднее даты документа + 10 дней
    private static List<CheckError> Check_054(List<Form13> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var pEndStr = (rep.EndPeriod_DB ?? string.Empty).Trim();
        var docDateStr = (forms[line].DocumentDate_DB ?? string.Empty).Trim();
        if (opCode is not "10"
            || !DateOnly.TryParse(pEndStr, out var pEnd)
            || !DateOnly.TryParse(docDateStr, out var docDate))
        {
            return result;
        } 
        var valid = WorkdaysBetweenDates(docDate, pEnd) <= 10;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = docDateStr,
                Message = "Дата документа не входит в отчетный период. Для операции инвентаризации, " +
                          "срок предоставления отчета (10 рабочих дней) исчисляется с даты утверждения акта инвентаризации."
            });
        }
        return result;
    }

    #endregion

    #region Check055

    //Код ОКПО поставщика/получателя равен коду ОКПО отчитывающейся организации + 8/14 цифр (графа 17)
    private static List<CheckError> Check_055(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "10", "11", "12", "15", "17", "18", "41", "42", "43", "58", "61", "62",
            "65", "67", "68", "71", "72", "73", "74", "75", "97", "98", "99"
        };
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var providerOrRecieverOkpo = (forms[line].ProviderOrRecieverOKPO_DB ?? string.Empty).Trim();
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB.Trim()
            : (forms10[0].Okpo_DB ?? string.Empty).Trim();
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var valid = OkpoRegex.IsMatch(providerOrRecieverOkpo)
                     && providerOrRecieverOkpo == repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Для выбранного кода операции указывается код ОКПО отчитывающейся организации."
            });
        }
        return result;
    }

    #endregion

    #region Check056

    //Код ОКПО поставщика/получателя не равен коду ОКПО отчитывающейся организации + 8/14 цифр (графа 17)
    private static List<CheckError> Check_056(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "25", "27", "28", "29", "35", "37", "38", "39", "63", "64" };
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var providerOrRecieverOkpo = (forms[line].ProviderOrRecieverOKPO_DB ?? string.Empty).Trim();
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB.Trim()
            : (forms10[0].Okpo_DB ?? string.Empty).Trim();
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var valid = OkpoRegex.IsMatch(providerOrRecieverOkpo)
                     && providerOrRecieverOkpo != repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Для выбранного кода операции указывается код ОКПО контрагента."
            });
        }
        return result;
    }

    #endregion

    #region Check057

    //Код ОКПО поставщика/получателя состоит из 8/14 чисел или "минобороны" (графа 17)
    private static List<CheckError> Check_057(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var providerOrRecieverOkpo = (forms[line].ProviderOrRecieverOKPO_DB ?? string.Empty).Trim();
        if (operationCode is not ("22" or "32")) return result;
        var valid = OkpoRegex.IsMatch(providerOrRecieverOkpo)
                    || providerOrRecieverOkpo.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Формат ввода данных не соответствует приказу. Следует указать код ОКПО контрагента, " +
                          "либо \"Минобороны\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check058

    //Код ОКПО поставщика/получателя из ОКСМ (не Россия), для определенных кодов операции, с примечанием (графа 17)
    private static List<CheckError> Check_058(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 17;
        string[] applicableOperationCodes = { "81", "82", "83", "84", "85", "86", "87", "88" };
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var providerOrRecieverOkpo = (forms[line].ProviderOrRecieverOKPO_DB ?? string.Empty).Trim();
        if (!applicableOperationCodes.Contains(opCode)) return result;
        var valid = OKSM.Any(oksmEntry => oksmEntry["shortname"] == providerOrRecieverOkpo)
                    && !providerOrRecieverOkpo.Equals("россия", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            if (providerOrRecieverOkpo == "-")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_13",
                    Row = (line + 1).ToString(),
                    Column = "ProviderOrRecieverOKPO_DB",
                    Value = providerOrRecieverOkpo,
                    Message = "Значение не может быть \"-\"."
                });
            }
            else
            {
                result.Add(new CheckError
                {
                    FormNum = "form_13",
                    Row = (line + 1).ToString(),
                    Column = "ProviderOrRecieverOKPO_DB",
                    Value = providerOrRecieverOkpo,
                    Message = "Формат ввода данных не соответствует приказу."
                });
            }
        }
        valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOkpo,
                Message = "Необходимо добавить примечание."
            });
        }
        return result;
    }

    #endregion

    #region Check059

    //При определенных кодах операции, код ОКПО перевозчика равен "-" (графа 18)
    private static List<CheckError> Check_059(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "10", "11", "12", "15", "17", "18", "41", "42", "43", "46", "47", "53", "54", 
            "58", "63", "64", "65", "67", "68", "71", "72", "73", "74", "75", "97", "98"
        };
        var opCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var transporterOkpo = (forms[line].TransporterOKPO_DB ?? string.Empty).Trim();
        var valid = transporterOkpo is "-";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOkpo),
                Message = "При выбранном коде операции транспортирование не производится."
            });
        }
        return result;
    }

    #endregion

    #region Check060

    //Код ОКПО перевозчика равен 8/14 цифр, допускается "прим." (графа 18)
    private static List<CheckError> Check_060(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "21", "25", "27", "28", "29", "31", "32", "35", "36", "37", "38",
            "39", "61", "62", "81", "82", "83", "84", "85", "86", "87", "88"
        };
        string[] transporterOkpoValid = { "прим.", "прим", "примечание", "примечания" };
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var transporterOkpo = (forms[line].TransporterOKPO_DB ?? string.Empty).Trim();
        const byte graphNumber = 18;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OkpoRegex.IsMatch(transporterOkpo)
                    || (transporterOkpoValid.Contains(transporterOkpo.ToLower())
                    && CheckNotePresence(notes, line, graphNumber));
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = transporterOkpo,
                Message = "Необходимо указать код ОКПО организации перевозчика."
            });
        }
        return result;
    }

    #endregion

    #region Check061

    //Код ОКПО перевозчика состоит из 8/14 чисел или "минобороны" (графа 18)
    private static List<CheckError> Check_061(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "22", "32" };
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var transporterOkpo = (forms[line].TransporterOKPO_DB ?? string.Empty).Trim();
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OkpoRegex.IsMatch(transporterOkpo)
                    || transporterOkpo.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = transporterOkpo,
                Message = "Необходимо указать код ОКПО организации перевозчика, либо \"Минобороны\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check062

    //Наименование заполнено (графа 19)
    private static List<CheckError> Check_062(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var packName = (forms[line].PackName_DB ?? string.Empty).Trim();
        var valid = packName != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PackName_DB",
                Value = packName,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check063

    //Тип заполнен (графа 20)
    private static List<CheckError> Check_063(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var packType = (forms[line].PackType_DB ?? string.Empty).Trim();
        var valid = packType != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PackType_DB",
                Value = packType,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check064

    //Номер заполнен (графа 21)
    private static List<CheckError> Check_064(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var packNumber = (forms[line].PackNumber_DB ?? string.Empty).Trim();
        var valid = packNumber != string.Empty;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PackNumber_DB",
                Value = packNumber,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #endregion

    #region GraphsList

    private static Dictionary<string, string> GraphsList { get; } = new()
    {
        { "NumberInOrder_DB", "01 - № п/п" },
        { "OperationCode_DB", "02 - Код операции" },
        { "OperationDate_DB", "03 - Дата операции" },
        { "PassportNumber_DB", "04 - Номер паспорта (сертификата)" },
        { "Type_DB", "05 - Тип источника" },
        { "Radionuclids_DB", "06 - Радионуклиды" },
        { "FactoryNumber_DB", "07 - Номер" },
        { "Activity_DB", "08 - Активность, Бк." },
        { "CreatorOKPO_DB", "09 - Код ОКПО изготовителя" },
        { "CreationDate_DB", "10 - Дата выпуска" },
        { "AggregateState_DB", "11 - Агрегатное состояние" },
        { "PropertyCode_DB", "12 - Код формы собственности" },
        { "Owner_DB", "13 - Код ОКПО правообладателя" },
        { "DocumentVid_DB", "14 - Вид документа" },
        { "DocumentNumber_DB", "15 - Номер документа" },
        { "DocumentDate_DB", "16 - Дата документа" },
        { "ProviderOrRecieverOKPO_DB", "17 - Код ОКПО поставщика или получателя" },
        { "TransporterOKPO_DB", "18 - Код ОКПО перевозчика" },
        { "PackName_DB", "19 - Наименование прибора, УКТ, упаковки" },
        { "PackType_DB", "20 - Тип прибора, УКТ, упаковки" },
        { "PackNumber_DB", "21 - Номер прибора, УКТ, упаковки" }
    };

    #endregion
}