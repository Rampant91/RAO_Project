using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Models.CheckForm;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using Path = System.IO.Path;

namespace Client_App.Commands.SyncCommands.CheckForm;

public abstract partial class CheckF11 : CheckBase
{
    #region Properties

    private static readonly string[] OperationCode_DB_Valids =
    {
        "10","11","12","15","17","18","21",
        "22","25","27","28","29","31","32","35",
        "37","38","39","41","42","43","46","47","53",
        "54","58","61","62","63","64","65","66",
        "67","68","71","72","73","74","75","81",
        "82","83","84","85","86","87","88","97",
        "98","99"
    };

    private static readonly string[] OperationCode_DB_Check018 =
    {
        "11", "12", "15", "28", "38", "41", "63", "64", "65", "73", "81", "85", "88"
    };  //заслужили собственную константу т.к. используется в нескольких проверках (18, 41, 42 и 43).

    private static readonly string[] Radionuclids_DB_Valids =
    {
        "плутоний","уран-233","уран-235","нептуний-237","америций-241","америций-243","калифорний-252","торий","литий-6","тритий"
    };

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

    private static string[] Type_DB_Valids = { };

    private static bool ZRI_Ignore = true;

    private static bool MZA_Ignore = true;

    #endregion

    #region CheckTotal

    public static List<CheckError> Check_Total(Reports reps, Report rep)
    {
        var currentFormLine = 0;
        List<CheckError> errorList = new();
        if (OKSM.Count == 0)
        {
#if DEBUG
            OKSM_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data", "Spravochniki", "oksm.xlsx"));
#else
            OKSM_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"oksm.xlsx"));
#endif
        }
        if (D.Count == 0)
        {
#if DEBUG
            D_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data", "Spravochniki", "D.xlsx"));
#else
            D_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"D.xlsx"));
#endif
        }
        if (R.Count == 0)
        {
#if DEBUG
            R_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data", "Spravochniki", "R.xlsx"));
#else
            R_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"R.xlsx"));
#endif
        }
        if (HolidaysSpecific.Count == 0)
        {
#if DEBUG
            Holidays_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data", "Spravochniki", "Holidays.xlsx"));
#else
            Holidays_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"Holidays.xlsx"));
#endif
        }
        var formsList = rep.Rows11.ToList<Form11>();
        errorList.AddRange(Check_002(formsList, rep));
        errorList.AddRange(Check_003(formsList));
        foreach (var key in rep.Rows11)
        {
            var form = (Form11)key;
            
            var notes = rep.Notes.ToList<Note>();
            var forms10 = reps.Master_DB.Rows10.ToList<Form10>();
            errorList.AddRange(Check_001(formsList, currentFormLine));
            errorList.AddRange(Check_004(formsList, currentFormLine));
            errorList.AddRange(Check_005(formsList, currentFormLine));
            errorList.AddRange(Check_006(formsList, currentFormLine));
            errorList.AddRange(Check_007(formsList, currentFormLine));
            errorList.AddRange(Check_008(formsList, currentFormLine));
            errorList.AddRange(Check_009(formsList, notes, currentFormLine));
            errorList.AddRange(Check_010(formsList, currentFormLine));
            errorList.AddRange(Check_011(formsList, currentFormLine));
            errorList.AddRange(Check_012(formsList, currentFormLine));
            errorList.AddRange(Check_013(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_014(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_015(formsList, currentFormLine));
            errorList.AddRange(Check_016(formsList, currentFormLine));
            errorList.AddRange(Check_017(formsList, currentFormLine));
            errorList.AddRange(Check_018(formsList, currentFormLine));
            errorList.AddRange(Check_019(formsList, currentFormLine));
            errorList.AddRange(Check_020(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_021(formsList, rep, currentFormLine));
            errorList.AddRange(Check_022(formsList, rep, currentFormLine));
            errorList.AddRange(Check_023(formsList, currentFormLine));
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
            errorList.AddRange(Check_035(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_036(formsList, currentFormLine));
            errorList.AddRange(Check_037(formsList, notes, currentFormLine));
            errorList.AddRange(Check_038(formsList, currentFormLine));
            errorList.AddRange(Check_039(formsList, currentFormLine));
            errorList.AddRange(Check_040(formsList, currentFormLine));
            errorList.AddRange(Check_041(formsList, currentFormLine));
            errorList.AddRange(Check_042(formsList, notes, currentFormLine));
            errorList.AddRange(Check_043(formsList, notes, currentFormLine));
            errorList.AddRange(Check_044(formsList, notes, currentFormLine));
            errorList.AddRange(Check_045(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_046(formsList, currentFormLine));
            errorList.AddRange(Check_047(formsList, notes, currentFormLine));
            errorList.AddRange(Check_048(formsList, notes, currentFormLine));
            errorList.AddRange(Check_049(formsList, currentFormLine));
            errorList.AddRange(Check_050(formsList, currentFormLine));
            errorList.AddRange(Check_051(formsList, currentFormLine));
            errorList.AddRange(Check_052(formsList, currentFormLine));
            errorList.AddRange(Check_053(formsList, currentFormLine));
            errorList.AddRange(Check_054(formsList, currentFormLine));
            errorList.AddRange(Check_055(formsList, rep, currentFormLine));
            errorList.AddRange(Check_056(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_057(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_058(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_059(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_060(formsList, currentFormLine));
            errorList.AddRange(Check_061(formsList, notes, currentFormLine));
            errorList.AddRange(Check_062(formsList, currentFormLine));
            errorList.AddRange(Check_063(formsList, currentFormLine));
            errorList.AddRange(Check_063(formsList, currentFormLine));
            errorList.AddRange(Check_064(formsList, currentFormLine));
            errorList.AddRange(Check_065(formsList, currentFormLine));
            errorList.AddRange(Check_066(formsList, currentFormLine));
            errorList.AddRange(Check_067(formsList, currentFormLine));
            
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

    private static List<CheckError> Check_001(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        return result;
    }

    #endregion

    #region Check002

    //Проверка даты окончания периода
    private static List<CheckError> Check_002(List<Form11> forms, Report rep)
    {
        List<CheckError> result = new();
        if (!DateOnly.TryParse(rep.EndPeriod_DB, out var endPeriod)) return result;
        var minOpDate = DateOnly.MinValue;
        var opCode = string.Empty;
        var line = 0;
        string[] operationCodeWithDeadline1 = { "71" };
        string[] operationCodeWithDeadline5 = { "73", "74", "75" };
        string[] operationCodeWithDeadline10 =
        {
            "11", "12", "15", "17", "18", "21", "22", "25", "27", "28", "29", "31", "32", "35", "37", "38", "39",
            "41", "42", "43", "46", "47", "48", "53", "54", "58", "61", "62", "63", "64", "65", "66", "67", "68",
            "72", "81", "82", "83", "84", "85", "86", "87", "88", "97", "98", "99"
        };
        foreach (var form in forms)
        {
            var curOpCode = form.OperationCode_DB ?? string.Empty;
            var opDatePlus = DateOnly.MinValue;
            if (!DateOnly.TryParse(form.OperationDate_DB, out var opDate)) continue;
            if (operationCodeWithDeadline1.Contains(curOpCode)) opDatePlus = opDate.AddDays(1);
            if (operationCodeWithDeadline5.Contains(curOpCode)) opDatePlus = opDate.AddDays(5);
            if (operationCodeWithDeadline10.Contains(curOpCode)) opDatePlus = opDate.AddDays(10);
            if (opDatePlus > minOpDate)
            {
                minOpDate = opDate;
                opCode = form.OperationCode_DB ?? string.Empty;
                line = form.NumberInOrder_DB;
            }
        }
        if (operationCodeWithDeadline10.Contains(opCode) && WorkdaysBetweenDates(minOpDate, endPeriod) > 10)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = forms[line].OperationDate_DB,
                Message = $"Дата окончания отчетного периода {rep.EndPeriod_DB} превышает дату операции более чем на 10 рабочих дней."
            });
        }
        else if (operationCodeWithDeadline5.Contains(opCode) && WorkdaysBetweenDates(minOpDate, endPeriod) > 5)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = forms[line].OperationDate_DB,
                Message = $"Дата окончания отчетного периода {rep.EndPeriod_DB} превышает дату операции более чем на 5 рабочих дней."
            });
        }
        else if (operationCodeWithDeadline1.Contains(opCode) && WorkdaysBetweenDates(minOpDate, endPeriod) > 1)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = forms[line].OperationDate_DB,
                Message = $"Дата окончания отчетного периода {rep.EndPeriod_DB} превышает дату операции более чем на 1 рабочий день."
            });
        }

        return result;
    }

    #endregion

    #region Check003

    //Наличие строк дубликатов
    private static List<CheckError> Check_003(List<Form11> forms)
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
                                  && comparator.Compare(formToCompare.CreatorOKPO_DB, currentForm.CreatorOKPO_DB) == 0
                                  && comparator.Compare(formToCompare.CreationDate_DB, currentForm.CreationDate_DB) == 0
                                  && formToCompare.Category_DB == currentForm.Category_DB
                                  && formToCompare.SignedServicePeriod_DB.Equals(currentForm.SignedServicePeriod_DB)
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
                FormNum = "form_11",
                Row = duplicateLines,
                Column = "2 - 19",
                Value = "",
                Message = $"Данные граф 2-19 в строках {duplicateLines} продублированы. Следует проверить правильность предоставления данных."
            });
        }
        return result;
    }

    #endregion

    #region Check004

    private static List<CheckError> Check_004(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (forms[line].NumberInOrder_DB != line + 1)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Id",
                Value = forms[line].Id.ToString(),
                Message = "Номера строк должны располагаться по порядку, без пропусков или дублирования номеров"
            });
        }
        return result;
    }

    #endregion

    #region Check005

    private static List<CheckError> Check_005(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var valid = operationCode != null && OperationCode_DB_Valids.Contains(operationCode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "Код операции не может быть использован в форме 1.1."
            });
        }
        return result;
    }

    #endregion

    #region Check006

    private static List<CheckError> Check_006(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "10" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    #region Check007

    //Для кодов операции 12, 42, в радионуклидах должен быть указан хоть один из списка (колонка 6)
    private static List<CheckError> Check_007(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "12", "42" };
        var radionuclids = forms[line].Radionuclids_DB;
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = Radionuclids_DB_Valids.Any(nuclid =>
            radionuclids?.Contains(nuclid, StringComparison.CurrentCultureIgnoreCase) == true);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = forms[line].Radionuclids_DB,
                Message = "В графе 6 не представлены сведения о радионуклидах, которые могут быть отнесены к ЯМ. Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check008

    private static List<CheckError> Check_008(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "15" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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
                Message = "В предыдущих отчетах не найдена строка об осуществлении операции переработки РАО в виде ОЗИИИ. Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check009

    //Наличие примечания для кодов операции 29, 29, 97-99
    private static List<CheckError> Check_009(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 2;
        string[] applicableOperationCodes = { "29", "39", "97", "98", "99" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = CheckNotePresence(new List<Form>(forms), notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = forms[line].OperationCode_DB,
                Message = "Необходимо дать пояснение об осуществленной операции."
            });
        }
        return result;
    }

    #endregion

    #region Check010

    private static List<CheckError> Check_010(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "21", "22", "25", "27", "28", "29", "41", "42", "43", "46", "53", "54", "61", "62", "65", "66", "67", "68", "71", "72", "81", "82", "83", "84", "88", "98" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    #region Check011

    private static List<CheckError> Check_011(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "37" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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
                Message = "В отчетах не найдена строка об осуществлении передачи учетной единицы. Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check012

    private static List<CheckError> Check_012(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "41" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    #region Check013

    //Для кода операции 53, ОКПО получателя/перевозчика должен совпадать с ОКПО организации
    private static List<CheckError> Check_013(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "53" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var providerOrRecieverOKPO = forms[line].ProviderOrRecieverOKPO_DB ?? "";
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        var valid = providerOrRecieverOKPO == repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOKPO,
                Message = "В графе 19 необходимо указать код ОКПО отчитывающейся организации. В случае, если зарядку/разрядку осуществляла подрядная организация, следует использовать код операции 54."
            });
        }
        return result;
    }

    #endregion

    #region Check014

    //Для кода операции 54, ОКПО получателя/перевозчика не должен совпадать с ОКПО организации
    private static List<CheckError> Check_014(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "54" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var providerOrRecieverOKPO = forms[line].ProviderOrRecieverOKPO_DB;
        var repOkpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        var valid = providerOrRecieverOKPO != repOkpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOKPO,
                Message = "В графе 19 необходимо указать ОКПО подрядной организации. В случае, если зарядка/разрядка осуществлялась силами отчитывающейся организации, следует использовать код операции 53."
            });
        }
        return result;
    }

    #endregion

    #region Check015

    private static List<CheckError> Check_015(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "58" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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
                Message = "В отчетах не найдены сведения о снятии с учета учетной единицы для разукомплектования. Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check016

    private static List<CheckError> Check_016(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "62" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    #region Check017

    private static List<CheckError> Check_017(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] applicableOperationCodes = { "65" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    #region Check018

    //Код ОКПО состоит из 8/14 чисел для определенных кодов операции (колонка 10)
    private static List<CheckError> Check_018(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "81", "82", "87", "88" };
        var operationCode = forms[line].OperationCode_DB;
        var сreatorOKPO = forms[line].CreatorOKPO_DB ?? "";
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(сreatorOKPO);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = forms[line].CreatorOKPO_DB,
                Message = "Код используется для предоставления сведений о ЗРИ, произведенных в Российской Федерации. " +
                "Необходимо ввести ОКПО организации изготовителя ЗРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check019

    //Коды операции 83-86 используются для зарубежных стран 
    private static List<CheckError> Check_019(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "83", "84", "85", "86" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var creatorOKPO = forms[line].CreatorOKPO_DB ?? "";
        var valid = !(OKSM.All(oksmEntry => oksmEntry["shortname"] != creatorOKPO)
                    || creatorOKPO.ToLower() is "россия");
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = forms[line].CreatorOKPO_DB,
                Message = "Код используется для предоставления сведений о ЗРИ, произведенных за пределами Российской Федерации. " +
                "Для импортированных ЗРИ необходимо указать краткое наименование государства в соответствии с ОКСМ."
            });
        }
        return result;
    }

    #endregion

    #region Check020

    private static List<CheckError> Check_020(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        if (!OperationCode_DB_Check018.Contains(operationCode)) return result;
        var okpoRepJur = forms10[0].Okpo_DB ?? "";
        var okpoRepTerPodr = forms10[1].Okpo_DB ?? "";
        var owner = forms[line].Owner_DB;
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
                Message = "Уточните правообладателя ЗРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check021

    //Дата операции входит в отчетный период (колонка 3)
    private static List<CheckError> Check_021(List<Form11> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] nonApplicableOperationCodes = { "10" };
        var opCode = forms[line].OperationCode_DB ?? "";
        if (nonApplicableOperationCodes.Contains(opCode)) return result;

        var valid = forms[line].OperationDate_DB != null;
        if (valid && rep is { StartPeriod_DB: not null, EndPeriod_DB: not null })
        {
            valid = DateOnly.TryParse(rep.StartPeriod_DB, out var pStart)
                    && DateOnly.TryParse(rep.EndPeriod_DB, out var pEnd)
                    && DateOnly.TryParse(forms[line].OperationDate_DB!, out var opDate)
                    && opDate >= pStart && opDate <= pEnd;
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationDate_DB",
                Value = forms[line].OperationDate_DB,
                Message = "Дата операции не входит в отчетный период."
            });
        }
        return result;
    }

    #endregion

    #region Check022

    //При коде операции 10, дата документа должна попадать в отчетный период
    private static List<CheckError> Check_022(List<Form11> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "10" };
        var documentDate = forms[line].DocumentDate_DB ?? string.Empty;
        var operationCode = forms[line].OperationCode_DB ?? string.Empty;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = DateOnly.TryParse(documentDate, out var documentDateReal)
                    && DateOnly.TryParse(rep.StartPeriod_DB, out var dateBeginReal)
                    && DateOnly.TryParse(rep.EndPeriod_DB, out var dateEndReal)
                    && documentDateReal >= dateBeginReal && documentDateReal <= dateEndReal;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = documentDate,
                Message = "Дата документа не входит в отчетный период. " + 
                "Для операции инвентаризации срок предоставления отчёта исчисляется с даты утверждения акта инвентаризации."
            });
        }
        return result;
    }

    #endregion

    #region Check023

    //Номер паспорта не пустая строка (колонка 4)
    private static List<CheckError> Check_023(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var valid = !string.IsNullOrWhiteSpace(forms[line].PassportNumber_DB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PassportNumber_DB",
                Value = forms[line].PassportNumber_DB,
                Message = "Графа не может быть пустой. Укажите номера паспорта (сертификата). " + 
                "В случае, если номер отсутвует, укажите символ \"-\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check024

    private static List<CheckError> Check_024(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (ZRI_Ignore) return result;
        var valid = !string.IsNullOrEmpty(forms[line].Type_DB);
        if (valid)
        {
            valid = Type_DB_Valids.Contains(forms[line].Type_DB);
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Type_DB",
                Value = forms[line].Type_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    #endregion

    #region Check025

    private static List<CheckError> Check_025(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var type = forms[line].Type_DB ?? "";
        var nonApplicableTypes = new string[] { "зри", "изделие", "прибор", "аппарат" };
        var valid = !string.IsNullOrWhiteSpace(type) && !nonApplicableTypes.Contains(type.ToLower());
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

    #region Check026

    private static List<CheckError> Check_026(List<Form11> forms, int line)
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

    #region Check027

    //Сверка со справочником
    private static List<CheckError> Check_027(List<Form11> forms, int line)
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
                FormNum = "form_11",
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
                FormNum = "form_11",
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

    #region Check028

    // Не пустое поле (графа 6 - Радионуклиды)
    private static List<CheckError> Check_028(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var rads = forms[line].Radionuclids_DB ?? string.Empty;
        if (string.IsNullOrWhiteSpace(rads))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = rads,
                Message = "Заполните сведения о радионуклиде."
            });
        }
        return result;
    }

    #endregion

    #region Check029

    //Разделение через ";", если количество > 1 (графа 7 - Номер ЗРИ).
    private static List<CheckError> Check_029(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var factoryNum = forms[line].FactoryNumber_DB ?? "";
        var quantity = forms[line].Quantity_DB;
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
                Column = "FactoryNumber_DB",
                Value = quantity?.ToString(),
                Message = "Заполните сведения о заводском номере ЗРИ. Если номер отсутствует, " +
                "в ячейке следует указать символ \"-\" без кавычек. Для упаковки однотипных ЗРИ, " +
                "имеющей один паспорт (сертификат) заводские номера в списке разделяются точкой с запятой."
            });
        }
        return result;
    }

    #endregion

    #region Check030

    //Больше нуля (графа 8 - Количество)
    private static List<CheckError> Check_030(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var quantity = forms[line].Quantity_DB;
        var valid = quantity is > 0;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Quantity_DB",
                Value = quantity.ToString(),
                Message = "Укажите количество ЗРИ"
            });
        }
        return result;
    }

    #endregion

    #region Check031

    private static List<CheckError> Check_031(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (MZA_Ignore) return result;
        return result;
    }

    #endregion

    #region Check032

    private static List<CheckError> Check_032(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (MZA_Ignore) return result;
        if (string.IsNullOrEmpty(forms[line].Radionuclids_DB)
            || forms[line].Activity_DB is null or "" or "-") return result;
        var nuclidsList = forms[line].Radionuclids_DB!.ToLower().Replace(" ", string.Empty).Split(';');
        if (nuclidsList.Length == 1) return result;
        if (nuclidsList.Any(nuclid => !Radionuclids_DB_Valids.Contains(nuclid)))
        {
            return result;
        }
        //find the minimum activity
        var activityMinimum = double.MaxValue;
        foreach (var nuclid in nuclidsList)
        {
            double activityConsidered = float.MaxValue;
            if (D.TryGetValue(nuclid, out var value))
            {
                activityConsidered = value;
            }
            if (activityConsidered < activityMinimum)
            {
                activityMinimum = activityConsidered;
            }
        }
        var activityReal = double.Parse(forms[line].Activity_DB!.Replace(".", ","), NumberStyles.Float);
        var valid = activityReal >= activityMinimum;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = forms[line].Activity_DB,
                Message = "Активность ниже МЗА, ЗРИ не является объектом учета СГУК РВ и РАО."
            });
        }
        return result;
    }

    #endregion

    #region Check033

    private static List<CheckError> Check_033(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (MZA_Ignore) return result;
        return result;
    }

    #endregion

    #region Check034

    //"Суммарная активность, Бк" положительное число, менее 10е+20
    private static List<CheckError> Check_034(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var activity = forms[line].Activity_DB ?? "";
        if (string.IsNullOrEmpty(activity) || activity == "-") return result;
        activity = ConvertStringToExponential(activity);
        if (!double.TryParse(activity,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var activityReal)
            || activity.Contains('-')
            || activityReal is <= 0 or > 10e+20)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = activity,
                Message = "Указанная активность превышает предельное значение." +
                "Сверьте сведения, указанные в отчёте, с паспортом на ЗРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check035

    //"код ОКПО изготовителя" = "ОКПО организации" для кодов операции 11, 58
    private static List<CheckError> Check_035(List<Form11> forms, List<Form10> forms10, int line)
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
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = forms[line].CreatorOKPO_DB,
                Message = "Проверьте код ОКПО организации-изготовителя."
            });
        }
        return result;
    }

    #endregion

    #region Check036

    //Соответствие формата данных "код ОКПО изготовителя" (колонка 10)
    private static List<CheckError> Check_036(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var creatorOKPO = forms[line].CreatorOKPO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = !string.IsNullOrEmpty(creatorOKPO)
                    && (okpoRegex.IsMatch(creatorOKPO)
                        || creatorOKPO is "прим."
                        || OKSM.Any(oksmEntry => oksmEntry["shortname"] == creatorOKPO));
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = creatorOKPO,
                Message = "Формат ввода данных не соответствует приказу. Укажите код ОКПО организации изготовителя."
            });
        }
        return result;
    }

    #endregion

    #region Check037

    // Если в "код ОКПО изготовителя" (колонка 10) указано примечание, то проверяется наличие примечания для данной строки
    private static List<CheckError> Check_037(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] creatorOkpoValid = { "прим.", "прим", "примечание", "примечания" };
        string[] applicableOperationCodes = { "81", "82", "87", "88" };
        var operationCode = forms[line].OperationCode_DB;
        if (applicableOperationCodes.Contains(operationCode)) return result;
        if (!creatorOkpoValid.Contains(forms[line].CreatorOKPO_DB?.ToLower()) && OKSM.All(oksmEntry => oksmEntry["shortname"] != forms[line].CreatorOKPO_DB)) return result;
        const byte graphNumber = 10;
        var valid = CheckNotePresence(new List<Form>(forms), notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = forms[line].CreatorOKPO_DB,
                Message = "Необходимо указать в примечании наименование и адрес организации-изготовителя ЗРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check038

    //Дата выпуска (графа 11) <= Дате операции (графа 3)
    private static List<CheckError> Check_038(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var valid = DateOnly.TryParse(forms[line].OperationDate_DB, out var operDate)
                    && DateOnly.TryParse(forms[line].CreationDate_DB, out var createDate)
                    && createDate <= operDate;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreationDate_DB",
                Value = forms[line].CreationDate_DB,
                Message = "Дата выпуска не может быть позже даты операции."
            });
        }
        return result;
    }

    #endregion

    #region Check039

    //Соответствие категории ЗРИ (колонка 12)
    private static List<CheckError> Check_039(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var dbBounds = new Dictionary<short, (double, double)>
        {
            { 1, (1000, double.MaxValue) },
            { 2, (10, 1000) },
            { 3, (1, 10) },
            { 4, (0.01, 1) },
            { 5, (0, 0.01) }
        };
        var activity = ConvertStringToExponential(forms[line].Activity_DB);
        var category = forms[line].Category_DB;
        var quantity = forms[line].Quantity_DB;
        var radionuclids = forms[line].Radionuclids_DB ?? "";
        List<double> dValueList = new();
        var nuclidsList = radionuclids
            .ToLower()
            .Replace(" ", string.Empty)
            .Split(';');
        var valid = category != null
                    && dbBounds.ContainsKey((short)category)
                    && nuclidsList.Length > 0;
        if (valid)
        {
            foreach (var nuclid in nuclidsList)
            {
                var nuclidFromR = R.FirstOrDefault(x => x["name"] == nuclid);
                if (nuclidFromR is null) continue;
                var expFromR = ConvertStringToExponential(nuclidFromR["D"]);
                if (float.TryParse(expFromR, out var value))
                {
                    dValueList.Add(value * 1e12);
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
                result.Add(new CheckError
                {
                    FormNum = "form_11",
                    Row = (line + 1).ToString(),
                    Column = "Radionuclids_DB",
                    Value = radionuclids,
                    Message = "Проверьте правильность заполнения графы 6."
                });
                return result;
            }
            var dMinValue = dValueList.Min();
            var dMaxValue = dValueList.Max();
            valid = double.TryParse(activity,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var aValue);
            aValue /= quantity != null && quantity != 0 ? (double)quantity : 1.0;
            if (valid)
            {
                var adMinBound = dMaxValue == 0.0
                    ? double.MaxValue
                    : aValue / dMaxValue;
                var adMaxBound = dMinValue == 0.0
                    ? double.MaxValue
                    : aValue / dMinValue;
                valid = dbBounds[(short)category!].Item1 <= adMinBound
                        && dbBounds[(short)category].Item2 > adMaxBound;
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

    #region Check040

    //Дата выпуска + НСС < дата операции
    private static List<CheckError> Check_040(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var opCode = forms[line].OperationCode_DB ?? "";
        var signedServicePeriod = forms[line].SignedServicePeriod_DB ?? 0;
        var creationDate = forms[line].CreationDate_DB;
        var operationDate = forms[line].OperationDate_DB;
        if (opCode == "41") return result;
        var valid = false;
        if (!string.IsNullOrEmpty(creationDate)
            && !string.IsNullOrEmpty(operationDate)
            && DateOnly.TryParse(creationDate, out var creationDateReal)
            && DateOnly.TryParse(operationDate, out var operationDateReal))
        {
            creationDateReal = creationDateReal.AddMonths((int)signedServicePeriod);
            creationDateReal = creationDateReal.AddDays((int)Math.Round(30 * (signedServicePeriod % 1.0)));
            valid = creationDateReal >= operationDateReal;
        }
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

    #region Check041

    //Код формы собственности (колонка 14) от 1 до 6, 9
    private static List<CheckError> Check_041(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var propertyCode = forms[line].PropertyCode_DB;
        byte?[] propertyCodeDBValid = { 1, 2, 3, 4, 5, 6, 9 };
        if (!propertyCodeDBValid.Contains(propertyCode))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = propertyCode.ToString(),
                Message = "Формат ввода данных не соответствует приказу. Выберите идентификатор, соответствующий форме собственности ЗРИ"
            });
        }
        return result;
    }

    #endregion

    #region Check042

    private static List<CheckError> Check_042(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var propertyCode = forms[line].PropertyCode_DB;
        var owner = forms[line].Owner_DB;
        const byte graphNumber = 14;
        byte?[] propertyCodeDBValid = { 2 };
        if (!propertyCodeDBValid.Contains(propertyCode)) return result;
        var valid = CheckNotePresence(new List<Form>(forms), notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = propertyCode.ToString(),
                Message = "Необходимо указать в примечании наименование субъекта Российской Федерации, в собственности которого находится объект учета"
            });
        }
        return result;
    }

    #endregion

    #region Check043

    private static List<CheckError> Check_043(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var propertyCode = forms[line].PropertyCode_DB;
        var owner = forms[line].Owner_DB;
        const byte graphNumber = 14;
        byte?[] propertyCodeDBValid = { 3 };
        if (!propertyCodeDBValid.Contains(propertyCode)) return result;
        var valid = CheckNotePresence(new List<Form>(forms), notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = propertyCode.ToString(),
                Message = "Необходимо указать в примечании наименование муниципального образования, в собственности которого находится объект учета"
            });
        }
        return result;
    }

    #endregion

    #region Check044

    private static List<CheckError> Check_044(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var propertyCode = forms[line].PropertyCode_DB;
        var owner = forms[line].Owner_DB;
        const byte graphNumber = 14;
        byte?[] propertyCodeDBValid = { 9 };
        if (!propertyCodeDBValid.Contains(propertyCode)) return result;
        var valid = CheckNotePresence(new List<Form>(forms), notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = propertyCode.ToString(),
                Message = "Необходимо указать в примечании наименование и адрес правообладателя (собственника или обладателя иного вещного права) на ЗРИ"
            });
        }
        return result;
    }

    #endregion

    #region Check045

    //8 или 14 чисел (колонка 15) если код формы собственности от 1 до 4
    private static List<CheckError> Check_045(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        byte?[] propertyCodeValid = { 1, 2, 3, 4 };
        string[] applicableOperationCodes = { "11", "12", "28", "38", "41", "63", "64" };
        var okpoRep = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        var operationCode = forms[line].OperationCode_DB;
        var owner = forms[line].Owner_DB;
        var propertyCode = forms[line].PropertyCode_DB;
        if (OperationCode_DB_Check018.Contains(operationCode)) return result;

        if (applicableOperationCodes.Contains(operationCode) && okpoRep != owner)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = $"Для кода операции {operationCode}, код ОКПО правообладателя должен совпадать с ОКПО отчитывающейся организации"
            });
        }

        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (!propertyCodeValid.Contains(propertyCode)) return result;
        var valid = !string.IsNullOrEmpty(owner)
                    && okpoRegex.IsMatch(owner);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Формат ввода данных не соответствует приказу. В случае, если правообладатель  российское юридическое лицо необходимо указать его код ОКПО"
            });
        }
        return result;
    }

    #endregion

    #region Check046

    //Правообладатель (колонка 15) из справочника ОКСМ, если код формы собственности 5
    private static List<CheckError> Check_046(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        byte?[] propertyCodeValid = { 5 };
        var propertyCode = forms[line].PropertyCode_DB;
        var owner = forms[line].Owner_DB;
        var operationCode = forms[line].OperationCode_DB;
        if (OperationCode_DB_Check018.Contains(operationCode)) return result;
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
                Message = "Формат ввода данных не соответствует приказу. В случае, если правообладатель  иностранное государство необходимо указать его краткое наименование в соответствии с ОКСМ"
            });
        }
        return result;
    }

    #endregion

    #region Check047

    //Правообладатель (колонка 15) из справочника ОКСМ и наличие примечания, если код формы собственности 6
    private static List<CheckError> Check_047(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        byte?[] propertyCodeValid = { 6 };
        const byte graphNumber = 15;
        var propertyCode = forms[line].PropertyCode_DB;
        var owner = forms[line].Owner_DB;
        var operationCode = forms[line].OperationCode_DB;
        if (OperationCode_DB_Check018.Contains(operationCode)) return result;
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
                Message = "Необходимо указать в примечании наименование и адрес правообладателя (собственника или обладателя иного вещного права) на ЗРИ."
            });
            return result;
        }
        var valid = CheckNotePresence(new List<Form>(forms), notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = forms[line].Owner_DB,
                Message = "Необходимо указать в примечании наименование и адрес правообладателя (собственника или обладателя иного вещного права) на ЗРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check048

    //Если код формы собственности 9, то должно быть примечание и значение "прим."
    private static List<CheckError> Check_048(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] creatorOkpoValid = { "прим.", "прим", "примечание", "примечания" };
        const byte graphNumber = 15;
        short?[] propertyCodeValid = { 9 };
        string[] nonapplicableOperationCodes = { "11", "12", "15", "28", "38", "41", "63", "64", "65", "73", "81", "85", "88" }; //взяты из Check_018
        var propertyCode = forms[line].PropertyCode_DB;
        var owner = forms[line].Owner_DB;
        var operationCode = forms[line].OperationCode_DB;
        if (nonapplicableOperationCodes.Contains(operationCode)) return result;
        if (!propertyCodeValid.Contains(propertyCode)) return result;
        var valid = creatorOkpoValid.Contains(owner?.ToLower());
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Необходимо указать \"прим.\" и добавить соответствующее примечание с наименованием и адресом правообладателя (собственника или обладателя иного вещного права) на ЗРИ."
            });
        }
        valid = CheckNotePresence(new List<Form>(forms), notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = owner,
                Message = "Необходимо указать в примечании наименование и адрес правообладателя (собственника или обладателя иного вещного права) на ЗРИ."
            });
        }
        return result;
    }

    #endregion

    #region Check049

    //При коде операции 10, вид документа равнен 1 (графа 16)
    private static List<CheckError> Check_049(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var documentVid = forms[line].DocumentVid_DB;
        var opCode = forms[line].OperationCode_DB ?? "";
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

    #region Check050

    //При коде операции 66, вид документа равнен 13 (графа 16)
    private static List<CheckError> Check_050(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var documentVid = forms[line].DocumentVid_DB;
        var opCode = forms[line].OperationCode_DB ?? "";
        if (opCode != "66") return result;
        if (documentVid != 13)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "Для операции продления НСС сопровождающий документ - решение о продлении НСС (код 13)."
            });
        }
        return result;
    }

    #endregion

    #region Check051

    //Вид документа равен 1-15, 19 (графа 16)
    private static List<CheckError> Check_051(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var documentVid = forms[line].DocumentVid_DB;
        var opCode = forms[line].OperationCode_DB ?? "";
        string[] applicableOperationCodes = 
        { 
            "11","12","15","17","18","21","22","25","27","28","29","31","32","35",
            "37","38","39","41","42","43","46","47","53","54","58","61","62","63",
            "64","65","67","68","71","72","73","74","75","81","82","83","84","85",
            "86","87","88","97","98","99"
        };
        if (!applicableOperationCodes.Contains(opCode)) return result;
        if (documentVid is not (>= 1 and <= 15 or 19))
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = Convert.ToString(documentVid),
                Message = "Формат ввода данных не соответствует приказу. Необзходимо указать вид документа " +
                          "в соответствии с таблицей 3 приложения №2 к приказу Госкорпорации \"Росатом\" " +
                          "от 07.10.2020 №1/13-НПА."
            });
        }
        return result;
    }

    #endregion

    #region Check052

    //Номер документа не пустой (колонка 17)
    private static List<CheckError> Check_052(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var documentNumber = forms[line].DocumentNumber_DB;
        var valid = !string.IsNullOrWhiteSpace(documentNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentNumber_DB",
                Value = documentNumber,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой"
            });
        }
        return result;
    }

    #endregion

    #region Check053

    //Дата документа <= дате операции для всех кодов операции, кроме 10 и 41
    private static List<CheckError> Check_053(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] excludedOperationCodes = { "10", "41" };
        var operationCode = forms[line].OperationCode_DB;
        var operationDate = forms[line].OperationDate_DB;
        var documentDate = forms[line].DocumentDate_DB;
        if (excludedOperationCodes.Contains(operationCode)) return result;
        var valid = DateOnly.TryParse(documentDate, out var documentDateReal)
                    && DateOnly.TryParse(operationDate, out var operationDateReal)
                    && documentDateReal <= operationDateReal;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = documentDate,
                Message = "Дата документа не может быть позже даты операции."
            });
        }
        return result;
    }

    #endregion

    #region Check054

    //Дата документа = дате операции, если код операции 41
    private static List<CheckError> Check_054(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "41" };
        var documentDate = forms[line].DocumentDate_DB;
        var operationCode = forms[line].OperationCode_DB;
        var operationDate = forms[line].OperationDate_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = DateOnly.TryParse(documentDate, out var documentDateReal)
                    && DateOnly.TryParse(operationDate, out var operationDateReal)
                    && documentDateReal == operationDateReal;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = documentDate,
                Message = "Дата документа должна соответствовать дате операции."
            });
        }
        return result;
    }

    #endregion

    #region Check055

    //При коде операции 10, дата окончания ОП не позднее даты документа + 10 дней
    private static List<CheckError> Check_055(List<Form11> forms, Report rep, int line)
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

    #region Check056

    //Код ОКПО поставщика/получателя 8/14 чисел и равен ОКПО отчитывающейся организации (колонка 19)
    private static List<CheckError> Check_056(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "10", "11", "12", "15", "17", "18", "41", "42", "43", "46", "47", "58", "61", "62", "65", "67", "68", "71", "72", "73", "74", "75", "76", "97", "98", "99" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOKPO = forms[line].ProviderOrRecieverOKPO_DB ?? "";
        var repOKPO = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(providerOrRecieverOKPO)
                    && providerOrRecieverOKPO == repOKPO;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOKPO,
                Message = "Для выбранного кода операции указывается код ОКПО отчитывающейся организации"
            });
        }
        return result;
    }

    #endregion

    #region Check057

    //Код ОКПО поставщика/получателя 8/14 чисел и не равен ОКПО отчитывающейся организации (колонка 19)
    private static List<CheckError> Check_057(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "25", "27", "28", "29", "35", "37", "38", "39", "63", "64" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOKPO = forms[line].ProviderOrRecieverOKPO_DB;
        var repOKPO = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(providerOrRecieverOKPO)
                     && providerOrRecieverOKPO != repOKPO;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOKPO,
                Message = "Для выбранного кода операции указывается код ОКПО контрагента."
            });
        }
        return result;
    }

    #endregion

    #region Check058

    //Код ОКПО поставщика/получателя состоит из 8/14 чисел для определенных кодов операции (колонка 19)
    private static List<CheckError> Check_058(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "21", "25", "27", "28", "29", "31", "35", "37", "38", "39", "63", "64" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOKPO = forms[line].ProviderOrRecieverOKPO_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(providerOrRecieverOKPO);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOKPO,
                Message = "Формат ввода данных не соответствует приказу. Укажите код ОКПО контрагента"
            });
        }
        return result;
    }

    #endregion

    #region Check059

    //Код ОКПО поставщика/получателя 8/14 чисел и не равен ОКПО отчитывающейся организации (колонка 19)
    private static List<CheckError> Check_059(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "66" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOKPO = forms[line].ProviderOrRecieverOKPO_DB ?? "";
        var repOKPO = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(providerOrRecieverOKPO)
                     && providerOrRecieverOKPO != repOKPO;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOKPO,
                Message = "Для выбранного кода операции указывается код ОКПО организации, осуществившей продление НСС"
            });
        }
        return result;
    }

    #endregion

    #region Check060

    //Код ОКПО поставщика/получателя состоит из 8/14 чисел или "минобороны" для определенных кодов операции (колонка 19)
    private static List<CheckError> Check_060(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "22", "32" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOKPO = forms[line].ProviderOrRecieverOKPO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = okpoRegex.IsMatch(providerOrRecieverOKPO)
                    || providerOrRecieverOKPO.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOKPO,
                Message = "Формат ввода данных не соответствует приказу. Следует указать код ОКПО контрагента, либо \"Минобороны\" без кавычек"
            });
        }
        return result;
    }

    #endregion

    #region Check061

    //Код ОКПО поставщика/получателя из ОКСМ (не Россия), для определенных кодов операции, с примечанием (колонка 19)
    private static List<CheckError> Check_061(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 19;
        string[] applicableOperationCodes = { "81", "82", "83", "84", "85", "86", "87", "88" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOKPO = forms[line].ProviderOrRecieverOKPO_DB;
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
                Message = "Формат ввода данных не соответствует приказу. Необходимо выбрать краткое наименование государства из ОКСМ"
            });
        }
        valid = CheckNotePresence(new List<Form>(forms), notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = providerOrRecieverOKPO,
                Message = "Необходимо добавить примечание."
            });
        }
        return result;
    }

    #endregion

    #region Check062

    //При определенных кодах операции, код ОКПО перевозчика равен "-" (колонка 20)
    private static List<CheckError> Check_062(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "10", "11", "12", "15", "17", "18", "41", "42", "43", "46", "47", "53", "54", "58", "65", "66", "67", "68", "71", "72", "73", "74", "75", "97", "98" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var transporterOKPO = forms[line].TransporterOKPO_DB;
        var valid = transporterOKPO is "-";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = transporterOKPO,
                Message = "При выбранном коде операции транспортирование не производится"
            });
        }
        return result;
    }

    #endregion

    #region Check063

    //При определенных кодах операции, код ОКПО перевозчика равен 8/14 цифр (колонка 20)
    private static List<CheckError> Check_063(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "21", "25", "27", "28", "29", "31", "32", "35", "36", "37", "38", "39", "61", "62", "81", "82", "83", "84", "85", "86", "87", "88" };
        var operationCode = forms[line].OperationCode_DB;
        var transporterOKPO = forms[line].TransporterOKPO_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(transporterOKPO);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = transporterOKPO,
                Message = "Необходимо указать код ОКПО организации перевозчика."
            });
        }
        return result;
    }

    #endregion

    #region Check064

    //Код ОКПО перевозчика состоит из 8/14 чисел или "минобороны" для определенных кодов операции (колонка 20)
    private static List<CheckError> Check_064(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "22", "32" };
        var operationCode = forms[line].OperationCode_DB;
        var transporterOKPO = forms[line].TransporterOKPO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = okpoRegex.IsMatch(transporterOKPO)
                    || transporterOKPO.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = transporterOKPO,
                Message = "Необходимо указать код ОКПО организации перевозчика, либо \"Минобороны\" без кавычек"
            });
        }
        return result;
    }

    #endregion

    #region Check065

    //Не пустое поле (колонка 21)
    private static List<CheckError> Check_065(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var packName = forms[line].PackName_DB;
        var valid = !string.IsNullOrWhiteSpace(packName);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PackName_DB",
                Value = packName,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой"
            });
        }
        return result;
    }

    #endregion

    #region Check066

    //Не пустое поле (колонка 22)
    private static List<CheckError> Check_066(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var packType = forms[line].PackType_DB;
        var valid = !string.IsNullOrWhiteSpace(packType);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PackType_DB",
                Value = packType,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой"
            });
        }
        return result;
    }

    #endregion

    #region Check067

    //Не пустое поле (колонка 23)
    private static List<CheckError> Check_067(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var packNumber = forms[line].PackNumber_DB;
        var valid = !string.IsNullOrWhiteSpace(packNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PackNumber_DB",
                Value = packNumber,
                Message = "Формат ввода данных не соответствует приказу. Графа не может быть пустой"
            });
        }
        return result;
    }

    #endregion

    #endregion
}