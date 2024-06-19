using Models.CheckForm;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Client_App.Commands.SyncCommands.CheckForm;

public abstract class CheckF15 : CheckBase
{
    #region CheckTotal

    public static List<CheckError> Check_Total(Reports reps, Report rep)
    {
        var currentFormLine = 0;
        List<CheckError> errorList = new();
        LoadDictionaries();
        var formsList = rep.Rows15.ToList<Form15>();
        errorList.AddRange(Check_001(formsList, rep));
        errorList.AddRange(Check_002(formsList));
        foreach (var key in rep.Rows15)
        {
            var form = (Form15)key;
            var notes = rep.Notes.ToList<Note>();
            var forms10 = reps.Master_DB.Rows10.ToList<Form10>();
            errorList.AddRange(Check_003(formsList, currentFormLine));
            errorList.AddRange(Check_004(formsList, currentFormLine));
            errorList.AddRange(Check_004_10(formsList, currentFormLine));
            errorList.AddRange(Check_004_29(formsList, notes, currentFormLine));
            errorList.AddRange(Check_004_21(formsList, currentFormLine));
            errorList.AddRange(Check_004_37(formsList, currentFormLine));
            errorList.AddRange(Check_004_41(formsList, currentFormLine));
            errorList.AddRange(Check_004_51(formsList, currentFormLine));
            errorList.AddRange(Check_004_52(formsList, currentFormLine));
            errorList.AddRange(Check_004_57(formsList, currentFormLine));
            errorList.AddRange(Check_004_84(formsList, currentFormLine));
            errorList.AddRange(Check_004_71(formsList, currentFormLine));
            errorList.AddRange(Check_005_03(formsList, rep, currentFormLine));
            errorList.AddRange(Check_006(formsList, currentFormLine));
            errorList.AddRange(Check_007(formsList, currentFormLine));
            errorList.AddRange(Check_008_a(formsList, currentFormLine));
            errorList.AddRange(Check_008_b(formsList, currentFormLine));
            errorList.AddRange(Check_009_a(formsList, currentFormLine));
            errorList.AddRange(Check_009_b(formsList, currentFormLine));
            errorList.AddRange(Check_010(formsList, currentFormLine));
            errorList.AddRange(Check_011(formsList, currentFormLine));
            errorList.AddRange(Check_012(formsList, currentFormLine));
            errorList.AddRange(Check_013(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_014(formsList, notes, currentFormLine));
            errorList.AddRange(Check_015(formsList, currentFormLine));
            errorList.AddRange(Check016(formsList, rep, currentFormLine));
            errorList.AddRange(Check_017_01(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_017_21(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_017_22(formsList, currentFormLine));
            errorList.AddRange(Check_017_84(formsList, notes, currentFormLine));
            errorList.AddRange(Check_018_01(formsList, currentFormLine));
            errorList.AddRange(Check_018_21(formsList, currentFormLine));
            errorList.AddRange(Check_018_22(formsList, currentFormLine));
            errorList.AddRange(Check_019(formsList, currentFormLine));
            errorList.AddRange(Check_020(formsList, currentFormLine));
            errorList.AddRange(Check_021(formsList, currentFormLine));
            errorList.AddRange(Check_022(formsList, currentFormLine));
            errorList.AddRange(Check_023(formsList, currentFormLine));
            errorList.AddRange(Check_024(formsList, notes, currentFormLine));
            errorList.AddRange(Check_025(formsList, currentFormLine));
            errorList.AddRange(Check_026(formsList, currentFormLine));

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

    //Проверка даты окончания периода
    private static List<CheckError> Check_001(List<Form15> forms15, Report rep)
    {
        var forms = forms15.Cast<Form1>().ToList();
        return CheckRepPeriod(forms, rep);
    }

    #endregion

    #region Check002

    //Наличие строк дубликатов
    private static List<CheckError> Check_002(List<Form15> forms)
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
                duplicatesLinesSet.Add(i + 1);
                duplicatesLinesSet.Add(j + 1);
            }
        }
        var duplicateLines = string.Join(", ", duplicatesLinesSet.Order());
        if (duplicatesLinesSet.Count != 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = duplicateLines,
                Column = "2 - 15",
                Value = "",
                Message = $"Данные граф 2-24 в строках {duplicateLines} продублированы. Следует проверить правильность предоставления данных."
            });
        }
        return result;
    }

    #endregion

    #region Check003

    private static List<CheckError> Check_003(List<Form15> forms, int line)
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
                Message = "Номера строк должны располагаться по порядку, без пропусков или дублирования номеров"
            });
        }
        return result;
    }

    #endregion

    #region Check004

    private static List<CheckError> Check_004(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var operationCodeValid = new []
        {
            "01","10","14","21","22","25","26","27","28","29","31","32","35","36","37",
            "38","39","41","43","44","45","49","51","52","57","59","63","64","71","72",
            "73","74","75","76","84","88","97","98","99"
        };
        var valid = operationCode != null && operationCodeValid.Contains(operationCode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "Код операции не может быть использован в форме 1.1."
            });
        }
        return result;
    }

    #endregion

    #region Check004_10

    private static List<CheckError> Check_004_10(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
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
                Message = "Сведения, представленные в инвентаризации, не соответствуют СНК"
            });
        }
        return result;
    }

    #endregion

    #region Check004_29

    private static List<CheckError> Check_004_29(List<Form15> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new[] { "29,39,49,59,97,98,99" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        const byte graphNumber = 2;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "Необходимо дать пояснение об осуществленной операции."
            });
        }
        return result;
    }

    #endregion

    #region Check004_21

    private static List<CheckError> Check_004_21(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new[] { "21,22,25,26,27,28,29,42,43,44,45,49,51,71,72,84,98" };
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
                Message = "Учетной единицы с такими параметрами нет в организации. Проверьте правильность указываемых сведений для ОЗРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check004_37

    private static List<CheckError> Check_004_37(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
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
                Message = "В отчетах не найдена строка об осуществлении передачи учетной единицы. Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check004_41
    //эта проверка по идее никогда не выдаст ошибку, т.к. проверке все равно, появился ли 41 код вручную или автоматически.
    private static List<CheckError> Check_004_41(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
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

    #region Check004_51
    private static List<CheckError> Check_004_51(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
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

    #region Check004_52
    private static List<CheckError> Check_004_52(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
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
                Message = "В отчетах не найдена строка об изъятии РАО из пункта хранения. Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check004_57
    private static List<CheckError> Check_004_57(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
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
                Message = "В отчетах не найдена строка снятии учетной единицы для упаковки/переупаковки. Проверьте правильность выбранного кода операции"
            });
        }
        return result;
    }

    #endregion

    #region Check004_84
    private static List<CheckError> Check_004_84(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new[] { "84" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = forms[line].StatusRAO_DB.Trim() == "7";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "Для ОЗРИ, которые возвращаются в страну поставщика код статус РАО – 7."
            });
        }
        return result;
    }

    #endregion

    #region Check004_71

    //Справочная "ошибка" - т.е. даже не ошибка.
    private static List<CheckError> Check_004_71(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new[] { "71", "72", "73", "74", "75", "76" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = false;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "К отчету необходимо приложить скан-копию документа характеризующего операцию."
            });
        }
        return result;
    }

    #endregion

    #region Check005_03

    //Дата операции входит в отчетный период с учетом срока подачи отчета в днях (колонка 3)
    private static List<CheckError> Check_005_03(List<Form15> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] nonApplicableOperationCodes = { "01" };
        var opCode = forms[line].OperationCode_DB;
        var opDate = forms[line].OperationDate_DB;
        var documentDate = forms[line].DocumentDate_DB;
        if (nonApplicableOperationCodes.Contains(opCode)) return result;
        var valid = opDate != null;
        var pEnd = DateOnly.MinValue;
        if (valid && rep is { StartPeriod_DB: not null, EndPeriod_DB: not null })
        {
            valid = DateOnly.TryParse(rep.StartPeriod_DB, out var pStart)
                    && DateOnly.TryParse(rep.EndPeriod_DB, out pEnd)
                    && DateOnly.TryParse(opDate, out var pMid)
                    && pMid >= pStart && pMid <= pEnd;
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = Convert.ToString(opDate),
                Message = "Дата операции не входит в отчетный период."
            });
        }
        if (opCode is "10")
        {
            if (DateOnly.TryParse(documentDate, out var docDate) 
                && WorkdaysBetweenDates(docDate, pEnd) > 10)
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
        }
        return result;
    }

    #endregion

    #region Check006
    private static List<CheckError> Check_006(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var passportNumber = forms[line].PassportNumber_DB;
        var valid = !string.IsNullOrWhiteSpace(passportNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "PassportNumber_DB",
                Value = Convert.ToString(passportNumber),
                Message = "Заполните сведения о номере паспорта (сертификата) ЗРИ, который переведен в ОЗИИИ."
            });
        }
        return result;
    }

    #endregion

    #region Check007
    private static List<CheckError> Check_007(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var type = forms[line].Type_DB;
        var valid = !string.IsNullOrWhiteSpace(type);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Type_DB",
                Value = Convert.ToString(type),
                Message = "Заполните сведения о типе ЗРИ, который переведен в ОЗИИИ."
            });
        }
        return result;
    }

    #endregion

    #region Check008_a

    //У радионуклидов в качестве разделителя использовать ;
    private static List<CheckError> Check_008_a(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var radionuclids = forms[line].Radionuclids_DB;
        if (radionuclids.Contains(',') || radionuclids.Contains('+'))
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = Convert.ToString(radionuclids),
                Message = "Формат ввода данных не соответствует приказу. Радионуклиды должны быть разделены точкой с запятой."
            });
        }
        return result;
    }

    #endregion

    #region Check008_b

    //Все радионуклиды есть в справочнике
    private static List<CheckError> Check_008_b(List<Form15> forms, int line)
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
                FormNum = "form_15",
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
                FormNum = "form_15",
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

    #region Check009_a

    //У номеров в качестве разделителя использовать ;
    private static List<CheckError> Check_009_a(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var numbers = forms[line].FactoryNumber_DB;
        if (numbers.Contains(',') || numbers.Contains('+'))
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "FactoryNumber_DB",
                Value = Convert.ToString(numbers),
                Message = "Заполните сведения о заводском номере ЗРИ, который переведен в ОЗИИИ. Если номер отсутствует, " +
                          "в ячейке следует указать символ \"-\" без кавычек. Для упаковки однотипных ЗРИ, имеющей один паспорт (сертификат) " +
                          "заводские номера в списке разделяются точкой с запятой."
            });
        }
        return result;
    }

    #endregion

    #region Check009_b

    //Номер не пустая строка (колонка 7)
    private static List<CheckError> Check_009_b(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var factoryNumber = forms[line].FactoryNumber_DB;
        var valid = !string.IsNullOrWhiteSpace(factoryNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "FactoryNumber_DB",
                Value = Convert.ToString(factoryNumber),
                Message = "Заполните сведения о заводском номере ЗРИ, который переведен в ОЗИИИ. Если номер отсутствует, " +
                          "в ячейке следует указать символ \"-\" без кавычек. Для упаковки однотипных ЗРИ, имеющей один паспорт (сертификат) " +
                          "заводские номера в списке разделяются точкой с запятой."
            });
        }
        return result;
    }

    #endregion

    #region Check010
    private static List<CheckError> Check_010(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var factoryNum = forms[line].FactoryNumber_DB;
        var quantity = forms[line].Quantity_DB ?? 0;
        if (factoryNum == null) return result;
        if (quantity == null)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Quantity_DB",
                Value = Convert.ToString(quantity),
                Message = "Заполните сведения о количестве ЗРИ, переведенных в ОЗИИИ."
            });
            return result;
        }
        var valid = !string.IsNullOrWhiteSpace(factoryNum)
                    && !factoryNum.Contains(',');
        if (quantity > 1 && !factoryNum.Contains(';')
            || quantity == 1 && factoryNum.Contains(';'))
        {
            valid = false;
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Quantity_DB",
                Value = quantity.ToString(),
                Message = "Формат ввода данных не соответствует приказу. Номера ЗРИ должны быть разделены точкой с запятой."
            });
        }
        return result;
    }

    #endregion

    #region Check011
    private static List<CheckError> Check_011(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var activity = forms[line].Activity_DB;
        if (string.IsNullOrEmpty(activity) || activity == "-") return result;
        activity = activity
            .Replace(".", ",")
            .Replace("(", "")
            .Replace(")", "");
        if (!double.TryParse(activity,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var activityReal)
            || activity.Contains('-')
            || activityReal is <= 0 or > 10e+20)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = Convert.ToString(forms[line].Activity_DB),
                Message = "Заполните сведения о суммарной активности ЗРИ, переведенных в ОЗИИИ. Оценочные сведения приводятся в круглых скобках"
            });
        }
        return result;
    }
    #endregion

    #region Check012
    private static List<CheckError> Check_012(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var operationDate = forms[line].OperationDate_DB;
        var creationDate = forms[line].CreationDate_DB;
        var valid = DateTime.TryParse(creationDate, out var creationDateReal)
                    && DateTime.TryParse(operationDate, out var operationDateReal)
                    && creationDateReal <= operationDateReal;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "CreationDate_DB",
                Value = Convert.ToString(creationDate),
                Message = "Дата выпуска не может быть позже даты операции."
            });
        }
        return result;
    }

    #endregion

    #region Check013

    private static List<CheckError> Check_013(List<Form15> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var opCode = forms[line].OperationCode_DB ?? string.Empty;
        var status = forms[line].StatusRAO_DB ?? string.Empty;
        var jurOkpo = forms10[0].Okpo_DB ?? string.Empty;
        var obOkpo = forms10[1].Okpo_DB ?? string.Empty;

        switch (opCode)
        {
            case "11" or "12" or "13" or "14" or "41":
            {
                if (status != obOkpo && status != jurOkpo)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = Convert.ToString(status),
                        Message = "РАО, образовавшиеся после 15.07.2011, находятся в собственности организации, " +
                                  "в результате деятельности которой они образовались."
                    });
                }
                break;
            }
            case "26" or "28" or "63":
            {
                if (status != obOkpo && status != jurOkpo)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = Convert.ToString(status),
                        Message = "Операция, соответствующая выбранному коду, может использоваться только для собственных РАО."
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
                        Value = Convert.ToString(status),
                        Message = "При операциях, связанных с получением права собственности, " +
                                  "в графе статус РАО необходимо отразить код ОКПО отчитывающейся организации."
                    });
                }
                break;
            }
            case "42" or "43" or "73" or "97" or "98":
            {
                if (status != obOkpo && status != jurOkpo)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = Convert.ToString(status),
                        Message = "Проверьте правильность статуса РАО."
                    });
                }
                break;
            }
            case "22" or "32":
            {
                var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
                if (status != "1" && !okpoRegex.IsMatch(status))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = Convert.ToString(status),
                        Message = "Проверьте правильность статуса РАО."
                    });
                }
                break;
            }
            case "16":
            {
                var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
                if (status != "2" && !okpoRegex.IsMatch(status))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = Convert.ToString(status),
                        Message = "Проверьте правильность статуса РАО."
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
                        Value = Convert.ToString(status),
                        Message = "Проверьте правильность статуса РАО."
                    });
                }
                break;
            }
            default:
            {
                var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
                if (status is not ("1" or "2" or "3" or "4" or "6" or "9") && !okpoRegex.IsMatch(status))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "StatusRAO_DB",
                        Value = Convert.ToString(status),
                        Message = "Заполнение графы 5 не соответствует требованиям приказа Госкорпорации \"Росатом\" " +
                                  "от 07.12.2020 №1/13-НПА."
                    });
                }
                break;
            }
        }
        return result;
    }

    #endregion

    #region Check014

    private static List<CheckError> Check_014(List<Form15> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 12;
        byte?[] validDocumentVid = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 19 };
        var documentVid = forms[line].DocumentVid_DB;
        var opCode = forms[line].OperationCode_DB ?? "";
        var valid = validDocumentVid.Contains(documentVid);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "Неверный код вида документа, сопровождающего операцию."
            });
        }
        else if (documentVid == 19)
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
                    Message = "К коду вида документа 19 необходимо примечание."
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
                Message = "При коде операции 31, вид документа должен быть равен 1."
            });
        }
        return result;
    }

    #endregion

    #region Check015

    private static List<CheckError> Check_015(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var documentNumber = forms[line].DocumentNumber_DB;
        var valid = !string.IsNullOrWhiteSpace(documentNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "DocumentNumber_DB",
                Value = Convert.ToString(documentNumber),
                Message = "Графа должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check016
    private static List<CheckError> Check016(List<Form15> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var operationDate = forms[line].OperationDate_DB;
        var documentDate = forms[line].DocumentDate_DB;
        DateOnly pMid;
        bool valid;
        if (operationCode == "41")
        {
            valid = DateOnly.TryParse(documentDate, out pMid)
                    && DateOnly.TryParse(operationDate, out var pOper)
                    && pMid == pOper;
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_15",
                    Row = (line + 1).ToString(),
                    Column = "DocumentDate_DB",
                    Value = Convert.ToString(documentDate),
                    Message = "Дата документа должна соответствовать дате операции"
                });
            }
        }
        else if (operationCode == "10")
        {
            valid = !(DateOnly.TryParse(rep.EndPeriod_DB, out var pEnd)
                      && DateOnly.TryParse(documentDate, out pMid)
                      && WorkdaysBetweenDates(pMid, pEnd) <= 10);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_15",
                    Row = (line + 1).ToString(),
                    Column = "DocumentDate_DB",
                    Value = Convert.ToString(documentDate),
                    Message = "Дата документа выходит за границы период. " +
                              "Дата окончания отчетного периода превышает дату документа более чем на 10 рабочих дней."
                });
            }
        }
        else
        {
            valid = DateOnly.TryParse(documentDate, out pMid)
                    && DateOnly.TryParse(operationDate, out var pOper)
                    && pMid <= pOper;
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_15",
                    Row = (line + 1).ToString(),
                    Column = "DocumentDate_DB",
                    Value = Convert.ToString(documentDate),
                    Message = "Дата документа не может быть позже даты операции"
                });
            }
        }
        return result;
    }

    #endregion

    #region Check017_01

    //Код ОКПО поставщика/получателя не равен коду ОКПО отчитывающейся организации + 8/14 цифр (колонка 18)
    private static List<CheckError> Check_017_01(List<Form15> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "01", "10", "14", "41", "43", "44", "45", "49", "51", "52", "57", 
            "59", "71", "72", "73", "74", "75", "76", "97", "98", "99"
        };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOkpo = forms[line].ProviderOrRecieverOKPO_DB;
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var valid = providerOrRecieverOkpo == repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOkpo),
                Message = "Для выбранного кода операции указывается код ОКПО отчитывающейся организации."
            });
        }
        return result;
    }

    #endregion

    #region Check017_21

    //Код ОКПО поставщика/получателя не равен коду ОКПО отчитывающейся организации + 8/14 цифр (колонка 18)
    private static List<CheckError> Check_017_21(List<Form15> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "21", "25", "26", "27", "28", "29", "31", "35", "36", "37", "38", "39" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOkpo = forms[line].ProviderOrRecieverOKPO_DB;
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(providerOrRecieverOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOkpo),
                Message = "Значение может состоять только из 8 или 14 символов"
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
                Value = Convert.ToString(providerOrRecieverOkpo),
                Message = "Для выбранного кода операции указывается код ОКПО контрагента."
            });
        }
        return result;
    }

    #endregion

    #region Check017_22

    //Код ОКПО поставщика/получателя состоит из 8/14 чисел или "минобороны" (колонка 18)
    private static List<CheckError> Check_017_22(List<Form15> forms, int line)
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
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOkpo),
                Message = "Формат ввода данных не соответствует приказу. Следует указать код ОКПО контрагента, " +
                          "либо \"Минобороны\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check017_84

    //Код ОКПО поставщика/получателя из ОКСМ (не Россия), для определенных кодов операции, с примечанием (колонка 18)
    private static List<CheckError> Check_017_84(List<Form15> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 15;
        string[] applicableOperationCodes = { "84", "88" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOkpo = forms[line].ProviderOrRecieverOKPO_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = OKSM.Any(oksmEntry => oksmEntry["shortname"] == providerOrRecieverOkpo)
                    && !providerOrRecieverOkpo.Equals("россия", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOkpo),
                Message = "Формат ввода данных не соответствует приказу."
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
                Value = Convert.ToString(providerOrRecieverOkpo),
                Message = "Необходимо добавить примечание."
            });
        }
        return result;
    }

    #endregion

    #region Check018_01

    //При определенных кодах операции, код ОКПО перевозчика равен "-"
    private static List<CheckError> Check_018_01(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = 
        {
            "01", "10", "14", "18", "41", "43", "44", "45", "48", "49", "51", "52", "57", "59", "71", "72", "73", "74", "75",
            "76", "97", "98"
        };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var transporterOkpo = forms[line].TransporterOKPO_DB;
        var valid = transporterOkpo is "-";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOkpo),
                Message = "При выбранном коде операции транспортирование не производится."
            });
        }
        return result;
    }

    #endregion

    #region Check018_21

    //При определенных кодах операции, код ОКПО перевозчика равен 8/14 цифр
    private static List<CheckError> Check_018_21(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "21","25","26","27","28","29","31","35","36","37","38","39","84","85","86","88"
        };
        var operationCode = forms[line].OperationCode_DB;
        var transporterOkpo = forms[line].TransporterOKPO_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (okpoRegex.IsMatch(transporterOkpo)) return result;

        var valid = okpoRegex.IsMatch(transporterOkpo);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOkpo),
                Message = "Необходимо указать код ОКПО организации перевозчика."
            });
        }
        return result;
    }

    #endregion

    #region Check018_22

    //Код ОКПО перевозчика состоит из 8/14 чисел или "минобороны"
    private static List<CheckError> Check_018_22(List<Form15> forms, int line)
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
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOkpo),
                Message = "Необходимо указать код ОКПО организации перевозчика, либо \"Минобороны\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check019
    private static List<CheckError> Check_019(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var packName = forms[line].PackName_DB;
        var valid = !string.IsNullOrWhiteSpace(packName);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "PackName_DB",
                Value = Convert.ToString(packName),
                Message = "Графа должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check020
    private static List<CheckError> Check_020(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var packType = forms[line].PackType_DB;
        var valid = !string.IsNullOrWhiteSpace(packType);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "PackType_DB",
                Value = Convert.ToString(packType),
                Message = "Графа должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check021
    private static List<CheckError> Check_021(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var packNum = forms[line].PackNumber_DB;
        var packName = forms[line].PackName_DB ?? "";
        var valid = !string.IsNullOrWhiteSpace(packNum);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "PackNumber_DB",
                Value = Convert.ToString(packNum),
                Message = "Заполните сведения о заводском номере упаковки. " +
                          "Если заводской номер отсутствует необходимо привести в круглых скобках номер, присвоенный в организации."
            });
        }
        if (packName is "без упаковки" && packNum != "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "PackNumber_DB",
                Value = Convert.ToString(packNum),
                Message = "При отсутствии упаковки, в графе заводской номер необходимо указать символ «-» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check022
    private static List<CheckError> Check_022(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var storagePlaceName = forms[line].StoragePlaceName_DB;
        var valid = !string.IsNullOrWhiteSpace(storagePlaceName);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "StoragePlaceName_DB",
                Value = Convert.ToString(storagePlaceName),
                Message = "Графа должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check023
    private static List<CheckError> Check_023(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var storagePlaceCode = forms[line].StoragePlaceCode_DB;
        var valid = !string.IsNullOrWhiteSpace(storagePlaceCode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "StoragePlaceCode_DB",
                Value = Convert.ToString(storagePlaceCode),
                Message = "Графа должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check024
    private static List<CheckError> Check_024(List<Form15> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var sortcode = forms[line].RefineOrSortRAOCode_DB;
        var valid = !string.IsNullOrWhiteSpace(sortcode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = Convert.ToString(sortcode),
                Message = "Графа должна быть заполнена."
            });
        }
        else
        {
            var operationCode = forms[line].OperationCode_DB;
            string[] applicableOperationCodes1 = 
            {
                "01", "10", "14", "21", "22", "25", "26", "27", "28", "29", "31", "32", "35", "36", "37", "38", "39", "43", "51",
                "52", "63", "64", "71", "72", "73", "74", "75", "76", "84", "88", "97", "98", "99"
            };
            if (applicableOperationCodes1.Contains(operationCode))
            {
                valid = sortcode == "-";
                if (!valid)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "RefineOrSortRAOCode_DB",
                        Value = Convert.ToString(sortcode),
                        Message = "Выбранный код операции не соответствует переработке/сортировке РАО. " +
                                  "Проверьте правильность заполнения граф 2 и 22/"
                    });
                }
            }
            else switch (operationCode)
            {
                case "44":
                {
                    if (sortcode is not ("41" or "42" or "43" or "49" or "54" or "71" or "72" or "73" or "74" or "79" or "99"))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_15",
                            Row = (line + 1).ToString(),
                            Column = "RefineOrSortRAOCode_DB",
                            Value = Convert.ToString(sortcode),
                            Message = "Несуществующий код переработки/сортировки."
                        });
                    }

                    break;
                }
                case "45" or "57":
                {
                    if (sortcode is not ("-" or "74"))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_15",
                            Row = (line + 1).ToString(),
                            Column = "RefineOrSortRAOCode_DB",
                            Value = Convert.ToString(sortcode),
                            Message = "Коду операции упаковка/переупаковка не соответствует код переработки/сортировки."
                        });
                    }

                    break;
                }
                case "49" or "59":
                {
                    if (sortcode is not ("-" or "52" or "72" or "74"))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_15",
                            Row = (line + 1).ToString(),
                            Column = "RefineOrSortRAOCode_DB",
                            Value = Convert.ToString(sortcode),
                            Message = "Коду операции сортировка соответствуют коды сортировки 52, 72, 74."
                        });
                    }

                    break;
                }
            }
            const byte graphNumber = 22;
            if (sortcode is "49" or "79" or "99")
            {
                valid = CheckNotePresence(notes, line, graphNumber);
                if (!valid)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_15",
                        Row = (line + 1).ToString(),
                        Column = "RefineOrSortRAOCode_DB",
                        Value = Convert.ToString(sortcode),
                        Message = "К выбранному коду переработки/сортировки нужно привести примечание."
                    });
                }
            }
        }
        return result;
    }

    #endregion

    #region Check025
    private static List<CheckError> Check_025(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var subsidy = ConvertStringToExponential(forms[line].Subsidy_DB);
        var valid = subsidy is "-" or "" 
                    || (double.TryParse(subsidy,
                            NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
                            CultureInfo.CreateSpecificCulture("ru-RU"),
                            out var subsidyNum)
                        && subsidyNum is >= 0 and <= 100);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "Subsidy_DB",
                Value = Convert.ToString(subsidy),
                Message = "Графа должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check026
    private static List<CheckError> Check_026(List<Form15> forms, int line)
    {
        List<CheckError> result = new();
        var fcpNum = ConvertStringToExponential(forms[line].FcpNumber_DB);
        var valid = fcpNum is "-" or ""
                    || (double.TryParse(fcpNum,
                            NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
                            CultureInfo.CreateSpecificCulture("ru-RU"),
                            out _));
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_15",
                Row = (line + 1).ToString(),
                Column = "FcpNumber_DB",
                Value = Convert.ToString(fcpNum),
                Message = "Графа должна быть заполнена."
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