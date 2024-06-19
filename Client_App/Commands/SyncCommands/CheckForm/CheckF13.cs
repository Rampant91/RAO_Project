using Models.CheckForm;
using Models.Collections;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using Models.Forms;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

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
        errorList.AddRange(Check_002(formsList, rep));
        errorList.AddRange(Check_003(formsList));
        foreach (var key in rep.Rows13)
        {
            var form = (Form13)key;
            
            var notes = rep.Notes.ToList<Note>();
            var forms10 = reps.Master_DB.Rows10.ToList<Form10>();
            errorList.AddRange(Check_001(formsList, currentFormLine));
            errorList.AddRange(Check_004(formsList, currentFormLine));
            errorList.AddRange(Check_005(formsList, currentFormLine));
            errorList.AddRange(Check_006(formsList, currentFormLine));
            errorList.AddRange(Check_007(formsList, currentFormLine));
            errorList.AddRange(Check_008(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_009(formsList, currentFormLine));
            errorList.AddRange(Check_010(formsList, currentFormLine));
            errorList.AddRange(Check_011(formsList, notes, currentFormLine));
            errorList.AddRange(Check_012(formsList, currentFormLine));
            errorList.AddRange(Check_013(formsList, currentFormLine));
            errorList.AddRange(Check_014(formsList, currentFormLine));
            errorList.AddRange(Check_015(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_016(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_017(formsList, currentFormLine));
            errorList.AddRange(Check_018(formsList, currentFormLine));
            errorList.AddRange(Check_019(formsList, currentFormLine));
            errorList.AddRange(Check_020(formsList, currentFormLine));
            errorList.AddRange(Check_021(formsList, currentFormLine));
            errorList.AddRange(Check_022(formsList, rep, currentFormLine));
            errorList.AddRange(Check_023(formsList, rep, currentFormLine));
            errorList.AddRange(Check_024(formsList, currentFormLine));
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
            errorList.AddRange(Check_036(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_037(formsList, currentFormLine));
            errorList.AddRange(Check_038(formsList, notes, currentFormLine));
            errorList.AddRange(Check_039(formsList, currentFormLine));
            errorList.AddRange(Check_040(formsList, currentFormLine));
            errorList.AddRange(Check_041(formsList, currentFormLine));
            errorList.AddRange(Check_042(formsList, notes, currentFormLine));
            errorList.AddRange(Check_043(formsList, notes, currentFormLine));
            errorList.AddRange(Check_044(formsList, notes, currentFormLine));
            errorList.AddRange(Check_045(formsList, currentFormLine));
            errorList.AddRange(Check_046(formsList, currentFormLine));
            errorList.AddRange(Check_047(formsList, notes, currentFormLine));
            errorList.AddRange(Check_048(formsList, currentFormLine));
            errorList.AddRange(Check_049(formsList, currentFormLine));
            errorList.AddRange(Check_050(formsList, currentFormLine));
            errorList.AddRange(Check_051(formsList, currentFormLine));
            errorList.AddRange(Check_052(formsList, rep, currentFormLine));
            errorList.AddRange(Check_053(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_054(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_055(formsList, currentFormLine));
            errorList.AddRange(Check_056(formsList, notes, currentFormLine));
            errorList.AddRange(Check_057(formsList, currentFormLine));
            errorList.AddRange(Check_058(formsList, notes, currentFormLine));
            errorList.AddRange(Check_059(formsList, currentFormLine));
            errorList.AddRange(Check_060(formsList, currentFormLine));
            errorList.AddRange(Check_061(formsList, currentFormLine));
            errorList.AddRange(Check_062(formsList, currentFormLine));
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

    private static List<CheckError> Check_001(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        return result;
    }

    #endregion

    #region Check002

    //Проверка даты окончания периода
    private static List<CheckError> Check_002(List<Form13> forms13, Report rep)
    {
        var forms = forms13.Cast<Form1>().ToList();
        return CheckRepPeriod(forms, rep);
    }

    #endregion

    #region Check003

    //Наличие строк дубликатов
    private static List<CheckError> Check_003(List<Form13> forms)
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
        var duplicateLines = string.Join(", ", duplicatesLinesSet.Order());
        if (duplicatesLinesSet.Count != 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = duplicateLines,
                Column = "2 - 17",
                Value = "",
                Message = $"Данные граф 2-17 в строках {duplicateLines} продублированы. Следует проверить правильность предоставления данных."
            });
        }
        return result;
    }

    #endregion

    #region Check004

    private static List<CheckError> Check_004(List<Form13> forms, int line)
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
                Message = "Номера строк должны располагаться по порядку, без пропусков или дублирования номеров."
            });
        }
        return result;
    }

    #endregion

    #region Check005

    //Код операции из списка (колонка 2)
    private static List<CheckError> Check_005(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var operationCodeValid = new[]
        {
            "10", "11", "12", "15", "17", "18", "21", "22", "25", "27", "28", "29", "31", "32", "35", "37", "38", "39",
            "41", "42", "43", "46", "47", "48", "53", "54", "58", "61", "62", "63", "64", "65", "67", "68", "71", "72", "73",
            "74", "75", "81", "82", "83", "84", "85", "86", "87", "88", "97", "98", "99"
        };
        var valid = operationCode != null && operationCodeValid.Contains(operationCode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = Convert.ToString(operationCode),
                Message = $"Код операции {operationCode} не может быть использован в форме 1.3."
            });
        }
        return result;
    }

    #endregion

    #region Check006

    private static List<CheckError> Check_006(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check007

    //Для кодов операции 12, 42, в радионуклидах должен быть указан хоть один из списка (колонка 6)
    private static List<CheckError> Check_007(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var radionuclid = forms[line].Radionuclids_DB;
        string[] applicableOperationCodes = { "12", "42" };
        var radionuclidValid = new[]
        {
            "плутоний-238", "плутоний-239", "плутоний-240", "уран-233", "уран-235", "уран-238", "нептуний-237",
            "америций-241", "америций 243", "калифорний-252", "торий-232", "литий-6", "дейтерий", "тритий"
        };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = radionuclid != null && radionuclidValid.Any(nuclid => radionuclid.ToLower().Contains(nuclid));
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = Convert.ToString(forms[line].Radionuclids_DB),
                Message = "В графе 6 не представлены сведения о радионуклидах, которые могут быть отнесены к ЯМ. Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check008

    private static List<CheckError> Check_008(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "11", "12", "15", "28", "38", "41", "48", "63", "64", "65", "73", "81", "85", "88" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var owner = forms[line].Owner_DB;
        var okpoRepJur = forms10[0].Okpo_DB ?? "";
        var okpoRepTerPodr = forms10[1].Okpo_DB ?? "";
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

    #region Check009

    private static List<CheckError> Check_009(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check010

    private static List<CheckError> Check_010(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check011

    //Для определенных кодов операции должно быть примечание (колонка 2)
    private static List<CheckError> Check_011(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 2;
        var operationCode = forms[line].OperationCode_DB;
        string[] applicableOperationCodes = { "29", "39", "97", "98", "99" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = Convert.ToString(operationCode),
                Message = "Необходимо дать пояснение об осуществленной операции."
            });
        }
        return result;
    }

    #endregion

    #region Check012

    private static List<CheckError> Check_012(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check013

    private static List<CheckError> Check_013(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check014

    private static List<CheckError> Check_014(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check015

    //Для кода операции 53, ОКПО организации должен быть равен ОКПО поставщика или получателя (колонка 17)
    private static List<CheckError> Check_015(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOkpo = forms[line].ProviderOrRecieverOKPO_DB;
        var okpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        string[] applicableOperationCodes = { "53" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = !string.IsNullOrWhiteSpace(providerOrRecieverOkpo) && providerOrRecieverOkpo.Equals(okpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = Convert.ToString(operationCode),
                Message = "В графе 17 необходимо указать код ОКПО своей организации. В случае, если зарядку/разрядку осуществляла подрядная организация, следует использовать код операции 54."
            });
        }
        return result;
    }

    #endregion

    #region Check016

    //Для кода операции 54, ОКПО организации не должен быть равен ОКПО поставщика или получателя (колонка 17)
    private static List<CheckError> Check_016(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOkpo = forms[line].ProviderOrRecieverOKPO_DB;
        var okpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        string[] applicableOperationCodes = { "54" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = !string.IsNullOrWhiteSpace(providerOrRecieverOkpo) && !providerOrRecieverOkpo.Equals(okpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = Convert.ToString(operationCode),
                Message = "В графе 17 необходимо указать код ОКПО подрядной организации. В случае, если зарядка/разрядка осуществлялась силами отчитывающейся организации, следует использовать код операции 53."
            });
        }
        return result;
    }

    #endregion

    #region Check017

    private static List<CheckError> Check_017(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check018

    private static List<CheckError> Check_018(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check019

    private static List<CheckError> Check_019(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check020

    //Код ОКПО состоит из 8/14 чисел для определенных кодов операции (колонка 9)
    private static List<CheckError> Check_020(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "81", "88" };
        var operationCode = forms[line].OperationCode_DB;
        var creatorOkpo = forms[line].CreatorOKPO_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = creatorOkpo != null && okpoRegex.IsMatch(creatorOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = Convert.ToString(creatorOkpo),
                Message = "Код используется для предоставления сведений о ОРИ, произведенных в Российской Федерации."
            });
        }
        return result;
    }

    #endregion

    #region Check021

    //Коды операции 83-86 используются для зарубежных стран 
    private static List<CheckError> Check_021(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var creatorOkpo = forms[line].CreatorOKPO_DB;
        var operationCode = forms[line].OperationCode_DB;
        string[] applicableOperationCodes = { "83", "84", "85", "86" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OKSM
                        .Any(oksmEntry => oksmEntry["shortname"] == creatorOkpo) 
                    && creatorOkpo?.ToLower() is not "россия";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = Convert.ToString(creatorOkpo),
                Message = "Код используется для предоставления сведений об ОРИ, произведенных за пределами Российской Федерации. " +
                          "Для импортированных ОРИ необходимо указать краткое наименование государства в соответствии с ОКСМ."
            });
        }
        return result;
    }

    #endregion

    #region Check022

    //Дата документа входит в отчетный период с учетом срока подачи отчета в днях (колонка 3)
    private static List<CheckError> Check_022(List<Form13> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] nonApplicableOperationCodes = { "10" };
        var operationCode = forms[line].OperationCode_DB;
        var operationDate = forms[line].OperationDate_DB;
        if (nonApplicableOperationCodes.Contains(operationCode)) return result;
        var valid = operationDate != null;
        if (valid && rep is { StartPeriod_DB: not null, EndPeriod_DB: not null })
        {
            valid = DateOnly.TryParse(rep.StartPeriod_DB, out var pStart)
                    && DateOnly.TryParse(rep.EndPeriod_DB, out var pEnd)
                    && DateOnly.TryParse(operationDate, out var pMid)
                    && pMid >= pStart && pMid <= pEnd;
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = Convert.ToString(operationDate),
                Message = "Дата операции не входит в отчетный период."
            });
        }
        return result;
    }

    #endregion

    #region Check023

    //При коде операции 10, дата документа должна попадать в отчетный период
    private static List<CheckError> Check_023(List<Form13> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "10" };
        var documentDate = forms[line].DocumentDate_DB;
        var operationCode = forms[line].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var valid = DateOnly.TryParse(documentDate, out var documentDateReal)
                    && DateOnly.TryParse(rep.StartPeriod_DB, out var dateBeginReal)
                    && DateOnly.TryParse(rep.EndPeriod_DB, out var dateEndReal)
                    && documentDateReal >= dateBeginReal && documentDateReal <= dateEndReal;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = documentDate,
                Message = "Дата документа не входит в отчетный период. " +
                          "Для операции инвентаризации срок предоставления отчета исчисляется с даты утверждения акта инвентаризации."
            });
        }
        return result;
    }

    #endregion

    #region Check024

    //Номер паспорта не пустая строка (колонка 4)
    private static List<CheckError> Check_024(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var passportNumber = forms[line].PassportNumber_DB;
        var valid = !string.IsNullOrWhiteSpace(passportNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PassportNumber_DB",
                Value = Convert.ToString(passportNumber),
                Message = "Графа не может быть пустой. Укажите номера паспорта. " +
                          "В случае, если номер отсутствует укажите символ \"-\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check025

    private static List<CheckError> Check_025(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check026

    private static List<CheckError> Check_026(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check027

    //Тип не пустая строка (колонка 4)
    private static List<CheckError> Check_027(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var type = forms[line].Type_DB;
        var valid = !string.IsNullOrWhiteSpace(type);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Type_DB",
                Value = Convert.ToString(type),
                Message = "Укажите тип ОРИ, в соответствии с паспортом (сертификатом)."
            });
        }
        return result;
    }

    #endregion

    #region Check028

    //Радионуклиды не пустая строка (колонка 6)
    private static List<CheckError> Check_028(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var radionuclids = forms[line].Radionuclids_DB;
        var valid = !string.IsNullOrWhiteSpace(radionuclids);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = Convert.ToString(radionuclids),
                Message = "Графа должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check029

    //У радионуклидов в качестве разделителя использовать ; (колонка 6)
    private static List<CheckError> Check_029(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var rads = forms[line].Radionuclids_DB ?? string.Empty;
        if (string.IsNullOrWhiteSpace(rads) || rads.Trim() == "-") return result;
        var radsSet = rads
            .ToLower()
            .Replace(" ", string.Empty)
            .Replace(',', ';')
            .Split(';')
            .ToHashSet();
        if (radsSet.Count == 1 && R.All(phEntry => phEntry["name"] != radsSet.First()))
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = rads,
                Message = "Формат ввода данных не соответствует приказу. " +
                          "Наименование радионуклида указывается названием химического элемента на русском языке " +
                          "с указанием через дефис массового числа изотопа."
            });
            return result;
        }
        if (rads.Contains(',') || !radsSet
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
            return result;
        }
        return result;
    }

    #endregion

    #region Check030

    //Все радионуклиды есть в справочнике (колонка 6)
    private static List<CheckError> Check_030(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var radionuclids = forms[line].Radionuclids_DB;
        if (string.IsNullOrWhiteSpace(radionuclids)) return result;
        var radArray = radionuclids.Replace(" ", string.Empty).ToLower()
            .Replace(',', ';')
            .Split(';');
        if (radArray.Length == 1 && string.Equals(radArray[0], "-")) return result;
        var valid = radArray.All(rad => R.Any(phEntry => phEntry["name"] == rad));
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = Convert.ToString(radionuclids),
                Message = "Формат ввода данных не соответствует приказу. Радионуклид отсутствует в справочнике."
            });
        }
        return result;
    }

    #endregion

    #region Check031

    private static List<CheckError> Check_031(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check032

    private static List<CheckError> Check_032(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check033

    private static List<CheckError> Check_033(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check034

    //Номер не пустая строка (колонка 7)
    private static List<CheckError> Check_034(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var factoryNumber = forms[line].FactoryNumber_DB;
        var valid = !string.IsNullOrWhiteSpace(factoryNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "FactoryNumber_DB",
                Value = Convert.ToString(factoryNumber),
                Message = "Заполните сведения о заводском номере ОРИ. " +
                          "Если номер отсутствует, в ячейке следует указать символ \"-\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check035

    //"Суммарная активность, Бк" положительное число, <= 10е+20 (колонка 9)
    private static List<CheckError> Check_035(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var activity = ConvertStringToExponential(forms[line].Activity_DB);
        if (activity is null or "" or "-") return result;
        if (!double.TryParse(activity,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var activityReal)
            || activity.Contains('-')
            || activityReal is <= 0 or > 10e+20)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Проверьте правильность предоставления сведений по суммарной активности."
            });
        }
        return result;
    }

    #endregion

    #region Check036

    //"код ОКПО изготовителя" = "ОКПО организации" для кодов операции 11 (колонка 9)
    private static List<CheckError> Check_036(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "11" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var creatorOkpo = forms[line].CreatorOKPO_DB;
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        var valid = creatorOkpo == repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = Convert.ToString(creatorOkpo),
                Message = "Проверьте код ОКПО организации-изготовителя."
            });
        }
        return result;
    }

    #endregion

    #region Check037

    //Соответствие формата данных "код ОКПО изготовителя" (колонка 9)
    private static List<CheckError> Check_037(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var creatorOkpo = forms[line].CreatorOKPO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = !string.IsNullOrEmpty(creatorOkpo)
                    && (okpoRegex.IsMatch(creatorOkpo)
                        || creatorOkpo is "прим."
                        || OKSM.Any(oksmEntry => oksmEntry["shortname"] == creatorOkpo));
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = Convert.ToString(creatorOkpo),
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
        if (OKSM.All(oksmEntry => oksmEntry["shortname"] != forms[line].CreatorOKPO_DB?.ToLower()) 
            && !creatorOkpoValid.Contains(forms[line].CreatorOKPO_DB?.ToLower())) return result;
        const byte graphNumber = 9;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = forms[line].CreatorOKPO_DB,
                Message = "Необходимо указать в примечании наименование и адрес организации-изготовителя ОРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check039

    //Дата выпуска (колонка 10) <= Дате операции (колонка 3)
    private static List<CheckError> Check_039(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var valid = DateOnly.TryParse(forms[line].OperationDate_DB, out var operDate)
                    && DateOnly.TryParse(forms[line].CreationDate_DB, out var createDate)
                    && createDate <= operDate;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "CreationDate_DB",
                Value = forms[line].CreationDate_DB,
                Message = "Дата выпуска не может быть позже даты операции."
            });
        }
        return result;
    }

    #endregion

    #region Check040

    //Агрегатное состояния равно 1-3 (колонка 11)
    private static List<CheckError> Check_040(List<Form13> forms, int line)
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

    #region Check041

    //Код формы собственности от 1 до 6, 9 (колонка 12)
    private static List<CheckError> Check_041(List<Form13> forms, int line)
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
                Message = "Формат ввода данных не соответствует приказу. Выберите идентификатор, соответствующий форме собственности ОРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check042

    // Если код формы собственности 2, необходимо указать примечание к ячейке
    private static List<CheckError> Check_042(List<Form13> forms, List<Note> notes, int line)
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
                Message = "Необходимо указать в примечании наименование субъекта Российской Федерации, в собственности которого находится объект учета."
            });
        }
        return result;
    }

    #endregion

    #region Check043

    // Если код формы собственности 3, необходимо указать примечание к ячейке
    private static List<CheckError> Check_043(List<Form13> forms, List<Note> notes, int line)
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
                Message = "Необходимо указать в примечании наименование муниципального образования, в собственности которого находится объект учета."
            });
        }
        return result;
    }

    #endregion

    #region Check044

    // Если код формы собственности 9, необходимо указать примечание к ячейке
    private static List<CheckError> Check_044(List<Form13> forms, List<Note> notes, int line)
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
                Message = "Необходимо указать в примечании наименование и адрес правообладателя (собственника или обладателя вещного права) объекта учета."
            });
        }
        return result;
    }

    #endregion

    #region Check045

    //8 или 14 чисел (колонка 13) если код формы собственности от 1 до 4 (колонка 12)
    private static List<CheckError> Check_045(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var owner = forms[line].Owner_DB;
        var propertyCode = forms[line].PropertyCode_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (propertyCode is < 1 or > 4) return result;
        var valid = owner != null && okpoRegex.IsMatch(owner);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = Convert.ToString(owner),
                Message = "Формат ввода данных не соответствует приказу. В случае, если правообладатель российское юридическое лицо, необходимо указать его код ОКПО."
            });
        }
        return result;
    }

    #endregion

    #region Check046

    //Правообладатель (колонка 13) из справочника ОКСМ, если код формы собственности 5 (колонка 12)
    private static List<CheckError> Check_046(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var propertyCode = forms[line].PropertyCode_DB;
        var owner = forms[line].Owner_DB;
        if (propertyCode is not 5) return result;
        var valid = owner != null
                    && OKSM.Any(oksmEntry => oksmEntry["shortname"] == owner)
                    && !owner.Equals("россия", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = Convert.ToString(owner),
                Message = "Формат ввода данных не соответствует приказу. В случае, если правообладатель иностранное государство, необходимо указать его краткое наименование в соответствии с ОКСМ."
            });
        }
        return result;
    }

    #endregion

    #region Check047

    //Правообладатель (колонка 13) из справочника ОКСМ и наличие примечания, если код формы собственности 6
    private static List<CheckError> Check_047(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 13;
        var propertyCode = forms[line].PropertyCode_DB;
        var owner = forms[line].Owner_DB;
        if (propertyCode is not 6) return result;
        if (OKSM.All(oksmEntry => oksmEntry["shortname"] != owner)
            || owner.Equals("россия", StringComparison.CurrentCultureIgnoreCase))
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = Convert.ToString(owner),
                Message = "Формат ввода данных не соответствует приказу. В графе \"код ОКПО правообладателя\" необходимо указать краткое наименование страны в соответствии с ОКСМ."
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
                Value = Convert.ToString(owner),
                Message = "Необходимо указать в примечании наименование и адрес правообладателя (собственника или обладателя иного вещного права) на ОРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check048

    //Вид документа от 1 до 15, 19 (колонка 14)
    private static List<CheckError> Check_048(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
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
                Message = "Формат ввода данных не соответствует приказу. Необходимо указать вид документа в соответствии с таблицей 3 приложения №2 к приказу Госкорпорации \"Росатом\" от 07.12.2020 №1/13-НПА."
            });
        }
        valid = forms[line].OperationCode_DB != "10" || documentVid == 1;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_12",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "При коде операции инвентаризации вид документа должен быть равен 1."
            });
        }
        return result;
    }

    #endregion

    #region Check049

    //Номер документа не пустой (колонка 15)
    private static List<CheckError> Check_049(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var documentNumber = forms[line].DocumentNumber_DB;
        var valid = !string.IsNullOrWhiteSpace(documentNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentNumber_DB",
                Value = Convert.ToString(documentNumber),
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check050

    //Дата документа <= дате операции для всех кодов операции, кроме 10 и 41
    private static List<CheckError> Check_050(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var operationDate = forms[line].OperationDate_DB;
        var documentDate = forms[line].DocumentDate_DB;
        if (operationCode is "10" or "41") return result;
        var valid = DateOnly.TryParse(documentDate, out var documentDateReal)
                    && DateOnly.TryParse(operationDate, out var operationDateReal)
                    && documentDateReal <= operationDateReal;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = Convert.ToString(documentDate),
                Message = "Дата документа не может быть позже даты операции."
            });
        }
        return result;
    }

    #endregion

    #region Check051

    //Дата документа = дате операции, если код операции 41
    private static List<CheckError> Check_051(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var documentDate = forms[line].DocumentDate_DB;
        var operationCode = forms[line].OperationCode_DB;
        var operationDate = forms[line].OperationDate_DB;
        if (operationCode is not "41") return result;
        var valid = DateOnly.TryParse(documentDate, out var documentDateReal)
                    && DateOnly.TryParse(operationDate, out var operationDateReal)
                    && documentDateReal == operationDateReal;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = Convert.ToString(documentDate),
                Message = "Дата документа должна соответствовать дате операции."
            });
        }
        return result;
    }

    #endregion

    #region Check052

    //При коде операции 10, дата окончания ОП не позднее даты документа + 10 дней
    private static List<CheckError> Check_052(List<Form13> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "10" };
        var documentDate = forms[line].DocumentDate_DB;

        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB) 
            || !(DateOnly.TryParse(rep.EndPeriod_DB, out var pEnd)
                && DateOnly.TryParse(documentDate, out var pMid))) return result; 

        var valid = WorkdaysBetweenDates(pMid, pEnd) <= 10;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = documentDate,
                Message = "Дата документа не входит в отчетный период. " +
                          "Для операции инвентаризации срок предоставления отчета исчисляется с даты утверждения акта инвентаризации."
            });
        }
        return result;
    }

    #endregion

    #region Check053

    //Код ОКПО поставщика/получателя равен коду ОКПО отчитывающейся организации + 8/14 цифр (колонка 17)
    private static List<CheckError> Check_053(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "10", "11", "12", "15", "17", "18", "41", "42", "43", "58", "61", "62",
            "65", "67", "68", "71", "72", "73", "74", "75", "97", "98", "99"
        };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOkpo = forms[line].ProviderOrRecieverOKPO_DB;
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(providerOrRecieverOkpo)
                     && providerOrRecieverOkpo == repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOkpo),
                Message = "Для выбранного кода операции указывается код ОКПО отчитывающейся организации."
            });
        }
        return result;
    }

    #endregion

    #region Check054

    //Код ОКПО поставщика/получателя не равен коду ОКПО отчитывающейся организации + 8/14 цифр (колонка 17)
    private static List<CheckError> Check_054(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "25", "27", "28", "29", "35", "37", "38", "39", "63", "64" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOkpo = forms[line].ProviderOrRecieverOKPO_DB;
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(providerOrRecieverOkpo)
                     && providerOrRecieverOkpo != repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOkpo),
                Message = "Для выбранного кода операции указывается код ОКПО контрагента."
            });
        }
        return result;
    }

    #endregion

    #region Check055

    //Код ОКПО поставщика/получателя состоит из 8/14 чисел или "минобороны" (колонка 17)
    private static List<CheckError> Check_055(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOkpo = forms[line].ProviderOrRecieverOKPO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (operationCode is not ("22" or "32")) return result;
        var valid = okpoRegex.IsMatch(providerOrRecieverOkpo)
                    || providerOrRecieverOkpo.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOkpo),
                Message = "Формат ввода данных не соответствует приказу. Следует указать код ОКПО контрагента, либо \"Минобороны\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check056

    //Код ОКПО поставщика/получателя из ОКСМ (не Россия), для определенных кодов операции, с примечанием (колонка 17)
    private static List<CheckError> Check_056(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 17;
        string[] applicableOperationCodes = { "81", "82", "83", "84", "85", "86", "87", "88" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOkpo = forms[line].ProviderOrRecieverOKPO_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
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
                    Value = Convert.ToString(providerOrRecieverOkpo),
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
                    Value = Convert.ToString(providerOrRecieverOkpo),
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
                Value = Convert.ToString(providerOrRecieverOkpo),
                Message = "Необходимо добавить примечание."
            });
        }
        return result;
    }

    #endregion

    #region Check057

    //При определенных кодах операции, код ОКПО перевозчика равен "-" (колонка 18)
    private static List<CheckError> Check_057(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "10", "11", "12", "15", "17", "18", "41", "42", "43", "46", "47", "53",
            "54", "58", "63","64","65", "67", "68", "71", "72", "73", "74", "75", "97", "98"
        };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var transporterOkpo = forms[line].TransporterOKPO_DB;
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

    #region Check058

    //При определенных кодах операции, код ОКПО перевозчика равен 8/14 цифр, допускается "прим." (колонка 18)
    private static List<CheckError> Check_058(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "21", "25", "27", "28", "29", "31", "32", "35", "36", "37", "38",
            "39", "61", "62", "81", "82", "83", "84", "85", "86", "87", "88"
        };
        string[] transporterOkpoValid = { "прим.", "прим", "примечание", "примечания" };
        var operationCode = forms[line].OperationCode_DB;
        var transporterOkpo = forms[line].TransporterOKPO_DB;
        const byte graphNumber = 18;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (okpoRegex.IsMatch(transporterOkpo)) return result;

        var valid = okpoRegex.IsMatch(transporterOkpo)
                    || (transporterOkpoValid.Contains(transporterOkpo.ToLower())
                    && CheckNotePresence(notes, line, graphNumber));
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOkpo),
                Message = "Необходимо указать код ОКПО организации перевозчика."
            });
        }
        return result;
    }

    #endregion

    #region Check059

    //Код ОКПО перевозчика состоит из 8/14 чисел или "минобороны" (колонка 18)
    private static List<CheckError> Check_059(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "22", "32" };
        var operationCode = forms[line].OperationCode_DB;
        var transporterOkpo = forms[line].TransporterOKPO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = okpoRegex.IsMatch(transporterOkpo)
                    || transporterOkpo.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOkpo),
                Message = "Необходимо указать код ОКПО организации перевозчика, либо \"Минобороны\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check060

    //Не пустое поле (колонка 19)
    private static List<CheckError> Check_060(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var packName = forms[line].PackName_DB;
        var valid = !string.IsNullOrWhiteSpace(packName);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PackName_DB",
                Value = Convert.ToString(packName),
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check061

    //Не пустое поле (колонка 20)
    private static List<CheckError> Check_061(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var packType = forms[line].PackType_DB;
        var valid = !string.IsNullOrWhiteSpace(packType);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PackType_DB",
                Value = Convert.ToString(packType),
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check062

    //Не пустое поле (колонка 21)
    private static List<CheckError> Check_062(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var packNumber = forms[line].PackNumber_DB;
        var valid = !string.IsNullOrWhiteSpace(packNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "PackNumber_DB",
                Value = Convert.ToString(packNumber),
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