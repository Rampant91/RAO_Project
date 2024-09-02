using Models.CheckForm;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Client_App.Commands.SyncCommands.CheckForm;

public abstract class CheckF17 : CheckBase
{
    #region Properties

    private static readonly string[] OperationCode_DB_Valids =
    {
        "10","11","12","13","14","16","18",
        "21","22","25","26","27","28","29",
        "31","32","35","36","37","38","39",
        "43","44","45","51","52","55","63",
        "64","68","71","97","98"
    };

    private static readonly Dictionary<string, string> GraphsList = new()
    {
        { "NumberInOrder_DB", "01 - № п/п" },
        { "OperationCode_DB", "02 - Код операции" },
        { "OperationDate_DB", "03 - Дата операции" },
        { "PackName_DB", "04 - Наименование упаковки" },
        { "PackType_DB", "05 - Тип упаковки" },
        { "PackFactoryNumber_DB", "06 - Заводской номер упаковки" },
        { "PackNumber_DB", "07 - Номер упаковки (сертификационный код)" },
        { "FormingDate_DB", "08 - Дата формирования" },
        { "PassportNumber_DB", "09 - Номер паспорта" },
        { "Volume_DB", "10 - Объем, куб.м" },
        { "Mass_DB", "11 - Масса брутто, т" },
        { "Radionuclids_DB", "12 - Наименование радионуклида" },
        { "SpecificActivity_DB", "13 - Удельная активность, Бк/г" },
        { "DocumentVid_DB", "14 - Вид документа" },
        { "DocumentNumber_DB", "15 - Номер документа" },
        { "DocumentDate_DB", "16 - Дата документа" },
        { "ProviderOrRecieverOKPO_DB", "17 - Код ОКПО поставщика или получателя" },
        { "TransporterOKPO_DB", "18 - Код ОКПО перевозчика" },
        { "StoragePlaceName_DB", "19 - Наименование пункта хранения" },
        { "StoragePlaceCode_DB", "20 - Код пункта хранения" },
        { "CodeRAO_DB", "21 - Код РАО" },
        { "StatusRAO_DB", "22 - Статус РАО" },
        { "VolumeOutOfPack_DB", "23 - Объем без упаковки, куб.м" },
        { "MassOutOfPack_DB", "24 - Масса без упаковки (нетто), т" },
        { "Quantity_DB", "25 - Количество ОЗИИИ, шт." },
        { "TritiumActivity_DB", "26 - Суммарная активность, Бк: тритий" },
        { "BetaGammaActivity_DB", "27 - Суммарная активность, Бк: бета-, гамма-излучающие радионуклиды (исключая трансурановые)" },
        { "AlphaActivity_DB", "28 - Суммарная активность, Бк: альфа-излучающие радионуклиды (исключая трансурановые)" },
        { "TransuraniumActivity_DB", "29 - Суммарная активность, Бк: трансурановые радионуклиды" },
        { "RefineOrSortRAOCode_DB", "30 - Код переработки/сортировки РАО" },
        { "Subsidy_DB", "31 - Субсидия, %" },
        { "FcpNumber_DB", "32 - Номер мероприятия ФЦП" },
    };

    #endregion

    #region CheckTotal

    public static List<CheckError> Check_Total(Reports reps, Report rep)
    {
        var currentFormLine = 0;
        List<CheckError> errorList = new();
        LoadDictionaries();
        var formsList = rep.Rows17.ToList<Form17>();
        var notes = rep.Notes.ToList<Note>();
        var forms10 = reps.Master_DB.Rows10.ToList<Form10>();
        while (currentFormLine < formsList.Count)
        {
            List<int> packLines = new();
            packLines.Add(currentFormLine);
            currentFormLine++;
            if (currentFormLine >= formsList.Count) break;
            while (string.IsNullOrWhiteSpace(formsList[currentFormLine].PackType_DB) || formsList[currentFormLine].PackType_DB.Trim() == "-")
            {
                packLines.Add(currentFormLine);
                currentFormLine++;
                if (currentFormLine >= formsList.Count) break;
            }
            for (var line = 0; line < packLines.Count; line++)
            {
                if (line == 0)
                {
                    //checks required only for the head line, also checks that are supposed to only run once per block.
                    errorList.AddRange(Check_002(formsList, packLines[line]));
                    errorList.AddRange(Check_002_99(formsList, packLines[line]));
                    errorList.AddRange(Check_002_12(formsList, packLines[line]));
                    errorList.AddRange(Check_002_29(formsList, notes, packLines[line]));
                    errorList.AddRange(Check_002_11(formsList, packLines[line]));
                    errorList.AddRange(Check_003_01(formsList, packLines[line]));
                    errorList.AddRange(Check_003_10(formsList, rep, packLines[line]));
                    errorList.AddRange(Check_003_non10(formsList, rep, packLines[line]));
                    errorList.AddRange(Check_004(formsList, packLines[line]));
                    errorList.AddRange(Check_005(formsList, packLines[line]));
                    errorList.AddRange(Check_006(formsList, packLines[line]));
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
                    errorList.AddRange(Check_023(formsList, packLines[line]));
                    errorList.AddRange(Check_024(formsList, packLines[line]));
                    errorList.AddRange(Check_025(formsList, packLines));
                    errorList.AddRange(Check_026_029(formsList, packLines));
                    errorList.AddRange(Check_030_55(formsList, packLines[line]));
                    errorList.AddRange(Check_030_10(formsList, packLines[line]));
                    errorList.AddRange(Check_031(formsList, packLines[line]));
                    errorList.AddRange(Check_032(formsList, packLines[line]));
                    errorList.AddRange(Check_Criteria(formsList, packLines));
                }
                //all other checks
                errorList.AddRange(Check_001(formsList, packLines[line]));
            }
        }
        errorList.AddRange(Check_033(formsList));
        errorList.AddRange(Check_034(formsList, rep));
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

    private static List<CheckError> Check_001(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var valid = (line == 0 && forms[line].NumberInOrder_DB is 1 or 0) 
                    || line > 0 && forms[line - 1].NumberInOrder_DB == forms[line].NumberInOrder_DB - 1;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "NumberInOrder_DB",
                Value = forms[line].NumberInOrder_DB.ToString(),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") +
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

    #region Check002

    private static List<CheckError> Check_002(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB ?? string.Empty;
        var valid = OperationCode_DB_Valids.Contains(operationCode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Код операции не может быть использован в форме 1.7."
            });
        }
        return result;
    }

    #endregion

    #region Check002_99

    private static List<CheckError> Check_002_99(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var codeRao = (forms[line].CodeRAO_DB ?? string.Empty).Trim();
        if (forms[line].CodeRAO_DB.Length < 10) return result;
        var valid = codeRao.Substring(8, 2) != "99";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = codeRao,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Код типа РАО «99» не может быть использован для РАО, соответствующих критериям приемлемости. " +
                          "Должны быть представлены сведения для каждого кода РАО."
            });
        }
        return result;
    }

    #endregion

    #region Check002_12

    private static List<CheckError> Check_002_12(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var rads = (forms[line].Radionuclids_DB ?? string.Empty).Trim();
        var applicableOperationCodes = new [] { "12,42" };
        var requiredNuclids = new []
        {
            "плутоний", "уран-233", "уран-235", "уран-238", "нептуний-237", "америций-241", 
            "америций-243", "калифорний-252", "торий", "литий-6", "тритий"
        };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var nuclids = rads.Split(';');
        var valid = false;
        for (var i = 0; i < nuclids.Length; i++)
        {
            nuclids[i] = nuclids[i].Trim().ToLower();
            if (!requiredNuclids.Contains(nuclids[i])) continue;
            valid = true;
            break;
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Radionuclids_DB",
                Value = rads,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "В графе 12 не представлены сведения о радионуклидах, которые могут быть отнесены к ЯМ. " +
                          "Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check002_29

    private static List<CheckError> Check_002_29(List<Form17> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var applicableOperationCodes = new [] { "29", "39", "97", "98" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        const byte graphNumber = 2;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Необходимо дать пояснение об осуществленной операции."
            });
        }
        return result;
    }

    #endregion

    #region Check002_11

    private static List<CheckError> Check_002_11(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var codeRao = (forms[line].CodeRAO_DB ?? string.Empty).Trim();
        var applicableOperationCodes = new [] { "11", "12", "13", "14", "16" };
        var applicableRao8 = new [] { "4", "6" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        if (codeRao.Length < 8) return result;
        var codeRao8 = codeRao.Substring(7, 1);
        var valid = applicableRao8.Contains(codeRao8);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          $"Для РАО класса {codeRao8} необходимо предоставить сведения о кондиционировании с кодом операции 55."
            });
        }
        return result;
    }

    #endregion

    #region Check003_01

    private static List<CheckError> Check_003_01(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "01" };
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var operationDate = (forms[line].OperationDate_DB ?? string.Empty).Trim();
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = DateOnly.TryParse(operationDate, out var operationDateReal) 
                    && operationDateReal is { Year: 2021, Month: 12, Day: 31 };
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
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

    #region Check003_non10

    private static List<CheckError> Check_003_non10(List<Form17> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] nonApplicableOperationCodes = { "10", "01" };
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var operationDate = (forms[line].OperationDate_DB ?? string.Empty).Trim(); 
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
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationDate_DB",
                Value = Convert.ToString(operationDate),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Дата операции не входит в отчетный период."
            });
        }
        return result;
    }

    #endregion

    #region Check003_10

    private static List<CheckError> Check_003_10(List<Form17> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "10" };
        var operationCode = forms[line].OperationCode_DB;
        var documentDate = forms[line].DocumentDate_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = documentDate != null;
        if (valid && rep is { StartPeriod_DB: not null, EndPeriod_DB: not null })
        {
            valid = DateOnly.TryParse(rep.StartPeriod_DB, out var pStart)
                    && DateOnly.TryParse(rep.EndPeriod_DB, out var pEnd)
                    && DateOnly.TryParse(documentDate, out var pMid)
                    && pMid >= pStart && pMid <= pEnd;
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentDate_DB",
                Value = Convert.ToString(documentDate),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Дата документа не входит в отчетный период. " +
                          "Для операции инвентаризации срок предоставления отчета исчисляется с даты утверждения акта инвентаризации."
            });
        }
        return result;
    }

    #endregion

    #region Check004

    private static List<CheckError> Check_004(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var packName = forms[line].PackName_DB ?? string.Empty;
        var graph5 = forms[line].PackType_DB ?? string.Empty;
        var graph6 = forms[line].PackFactoryNumber_DB ?? string.Empty;
        var graph10 = forms[line].Volume_DB ?? string.Empty;
        var graph11 = forms[line].Mass_DB ?? string.Empty;
        if (string.IsNullOrWhiteSpace(packName))
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "PackName_DB",
                Value = packName,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Заполните сведения об упаковке РАО. Если РАО размещены без упаковки, то в графе 4 указывается «без упаковки»."
            });
        }
        else if (packName.ToLower().Trim() == "без упаковки")
        {
            if (graph5.Trim() != "-")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "PackType_DB",
                    Value = graph5,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "При указании в графе 4 «Без упаковки» в графе 5 должен быть символ «-»."
                });
            }
            if (graph6.Trim() != "-")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "PackFactoryNumber_DB",
                    Value = graph6,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "При указании в графе 4 «без упаковки» в графе 6 должен быть символ «-»."
                });
            }
            if (graph10.Trim() != "-")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Volume_DB",
                    Value = graph10,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "При указании в графе 4 «без упаковки» в графе 10 должен быть символ «-»."
                });
            }
            if (graph11.Trim() != "-")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Mass_DB",
                    Value = graph11,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "При указании в графе 4 «без упаковки» в графе 11 должен быть символ «-»."
                });
            }
        }
        else
        {
            var comparator = new CustomNullStringWithTrimComparer();
            if (Packs.All(phEntry => comparator.Compare(StringRemoveSpecials(phEntry["name"]), StringRemoveSpecials(graph5)) != 0))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "PackType_DB",
                    Value = graph5,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") +
                              "Указанный тип контейнера (емкости) не найден в справочнике контейнеров, соответствующих частным критериям приемлемости для захоронения РАО существующих ППЗРО"
                });
            }
        }
        return result;
    }

    #endregion

    #region Check005

    private static List<CheckError> Check_005(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var packName = forms[line].PackName_DB ?? string.Empty; ;
        var graph5 = forms[line].PackType_DB ?? string.Empty; ;
        if (graph5.Trim() == "-")
        {
            if (packName.ToLower().Trim() == "без упаковки")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "PackType_DB",
                    Value = forms[line].PackType_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "В графе 5 символ «-» допускается только при указании в графе 4 «Без упаковки»."
                });
            }
        }
        else
        {
            var valid = !string.IsNullOrWhiteSpace(graph5);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "PackType_DB",
                    Value = forms[line].PackType_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте тип упаковки. Введённое значение не соответствует критериям приемлемости."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check006

    private static List<CheckError> Check_006(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var packName = forms[line].PackName_DB ?? string.Empty;
        var graph6 = forms[line].PackFactoryNumber_DB ?? string.Empty;
        if (graph6.Trim() == "-")
        {
            if (packName.ToLower().Trim() == "без упаковки")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "PackFactoryNumber_DB",
                    Value = forms[line].PackFactoryNumber_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "В графе 6 символ «-» допускается только при указании в графе 4 «Без упаковки»."
                });
            }
        }
        else if(string.IsNullOrWhiteSpace(graph6))
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "PackFactoryNumber_DB",
                Value = forms[line].PackFactoryNumber_DB,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Заполните сведения о заводском номере упаковки. Если РАО размещены без упаковки, то в графе 4 указывается «Без упаковки»."
            });
        }
        return result;
    }

    #endregion

    #region Check007

    private static List<CheckError> Check_007(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var graph7 = forms[line].PackNumber_DB;
        if (string.IsNullOrWhiteSpace(graph7))
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "PackNumber_DB",
                Value = forms[line].PackNumber_DB,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Заполните сведения о номере (идентификационном коде) упаковки."
            });
        }
        return result;
    }

    #endregion

    #region Check008

    private static List<CheckError> Check_008(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var operationDate = forms[line].OperationDate_DB;
        var formingDate = forms[line].FormingDate_DB;
        if (string.IsNullOrEmpty(formingDate))
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "FormingDate_DB",
                Value = formingDate,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Заполните сведения о дате формирования упаковки. Если РАО размещены без упаковки, укажите дату паспорта."
            });
        }
        else
        {
            if (DateTime.TryParse(operationDate, out var operationDateReal) 
                && DateTime.TryParse(formingDate, out var formingDateReal) 
                && !(formingDateReal <= operationDateReal))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "FormingDate_DB",
                    Value = formingDate,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Дата формирования упаковки не может быть позже даты операции."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check009

    private static List<CheckError> Check_009(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var graph9 = forms[line].PassportNumber_DB;
        if (string.IsNullOrWhiteSpace(graph9))
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "PassportNumber_DB",
                Value = forms[line].PassportNumber_DB,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Укажите номер паспорта."
            });
        }
        return result;
    }

    #endregion

    #region Check010

    private static List<CheckError> Check_010(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var comparator = new CustomNullStringWithTrimComparer();
        var packName = forms[line].PackType_DB ?? string.Empty;
        var graph10 = forms[line].Volume_DB ?? string.Empty;
        if (string.IsNullOrWhiteSpace(graph10))
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Volume_DB",
                Value = forms[line].Volume_DB,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Графа 10 должна быть обязательно заполнена."
            });
            return result;
        }
        if (graph10.Trim() == "-")
        {
            if (packName.ToLower().Trim() == "без упаковки")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Volume_DB",
                    Value = forms[line].Volume_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "В графе 10 символ «-» допускается только при указании в графе 4 «Без упаковки»."
                });
            }
            return result;
        }

        if (Packs.All(phEntry => comparator.Compare(phEntry["name"], packName) != 0)) return result;
        var packVolume = Packs.First(phEntry => comparator.Compare(phEntry["name"], packName) == 0)["volume"];
        if (!TryParseFloatExtended(graph10, out var graph10Real) ||
            !TryParseFloatExtended(packVolume, out var packVolumeReal)) return result;
        if (graph10Real > packVolumeReal * 1.1f || graph10Real < packVolumeReal * 0.9f)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Volume_DB",
                Value = forms[line].Volume_DB,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Сведения об объеме не соответствуют представленному типу упаковок. Проверьте правильность предоставленных сведений."
            });
        }
        return result;
    }

    #endregion

    #region Check011

    private static List<CheckError> Check_011(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var packName = forms[line].PackName_DB;
        var graph11 = forms[line].Mass_DB;
        var graph24 = forms[line].MassOutOfPack_DB;
        if (string.IsNullOrWhiteSpace(graph11))
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Mass_DB",
                Value = forms[line].Mass_DB,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Графа 11 должна быть обязательно заполнена."
            });
            return result;
        }
        if (graph11.Trim() == "-")
        {
            if (packName.ToLower().Trim() == "без упаковки")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Mass_DB",
                    Value = forms[line].Mass_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "В графе 11 символ «-» допускается только при указании в графе 4 «Без упаковки»."
                });
            }
            return result;
        }
        if (true) //pack name is within the pack table
        {
            if (!TryParseFloatExtended(graph24, out var graph24Real))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "MassOutOfPack_DB",
                    Value = forms[line].MassOutOfPack_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте правильность предоставления сведений о массе."
                });
            }
            else if (!TryParseFloatExtended(graph11, out var graph11Real))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Mass_DB",
                    Value = forms[line].Mass_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте правильность предоставления сведений о массе."
                });
            }
            else
            {
                var totalMassExpected = (graph11Real - graph24Real) + graph24Real;    //replace the value in parentheses with data from the pack table
                if (graph11Real > totalMassExpected * 1.2f || graph11Real < totalMassExpected / 1.2f)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_17",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "Mass_DB",
                        Value = forms[line].Mass_DB,
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                  "Проверьте правильность предоставления сведений о массе."
                    });
                }
            }
        }
        return result;
    }

    #endregion

    #region Check012

    private static List<CheckError> Check_012(List<Form17> forms, List<int> lines)
    {
        List<CheckError> result = new();
        foreach (int line in lines)
        {
            var radionuclids = forms[line].Radionuclids_DB ?? string.Empty;
            var rad = radionuclids.ToLower().Trim();
            if (string.IsNullOrWhiteSpace(rad) || rad == "-") continue;
            if (rad.Replace(" ", "").Replace(',', ';').Split(';').Length != 1)
            {
                //отсебятина, навеяно формированием паспорта, которое считает, что в каждой строчке есть только один нуклид
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Radionuclids_DB",
                    Value = forms[line].Radionuclids_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "В форме 1.7 в каждой строчке указывается до одного радионуклида."
                });
            }
            else if (R.All(phEntry => phEntry["name"] != rad))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Radionuclids_DB",
                    Value = forms[line].Radionuclids_DB,
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

    private static List<CheckError> Check_013(List<Form17> forms, List<int> lines)
    {
        List<CheckError> result = new();
        var massSum = 0.0f;
        var activitySumA = 0.0f;
        var activitySumB = 0.0f;
        var activitySumT = 0.0f;
        var activitySumU = 0.0f;
        var activitySetExpected = 0b0000;
        var activitySetReal = 0b0000;
        var partialActivitySumA = 0.0f;
        var partialActivitySumB = 0.0f;
        var partialActivitySumT = 0.0f;
        var partialActivitySumU = 0.0f;
        if (TryParseFloatExtended(forms[lines[0]].MassOutOfPack_DB, out var massReal))
        {
            massSum += massReal * 1e6f;
        }
        if (TryParseFloatExtended(forms[lines[0]].AlphaActivity_DB, out var activityAReal))
        {
            activitySumA += activityAReal; activitySetExpected |= 0b0010;
        }
        if (TryParseFloatExtended(forms[lines[0]].BetaGammaActivity_DB, out var activityBReal))
        {
            activitySumB += activityBReal; activitySetExpected |= 0b0100;
        }
        if (TryParseFloatExtended(forms[lines[0]].TritiumActivity_DB, out var activityTReal))
        {
            activitySumT += activityTReal; activitySetExpected |= 0b1000;
        }
        if (TryParseFloatExtended(forms[lines[0]].TransuraniumActivity_DB, out var activityUReal))
        {
            activitySumU += activityUReal; activitySetExpected |= 0b0001;
        }
        foreach (var line in lines)
        {
            var rads = forms[line].Radionuclids_DB ?? string.Empty;
            if (R.Any(phEntry => phEntry["name"] == rads.ToLower().Trim()) 
                && TryParseFloatExtended(forms[line].SpecificActivity_DB, out var partialActivityReal))
            {
                var nuclid = R
                    .First(phEntry => phEntry["name"] == rads.ToLower().Trim());
                switch (nuclid["code"])
                {
                    case "а":
                        partialActivitySumA += partialActivityReal;
                        activitySetReal |= 0b0010;
                        break;
                    case "б":
                        partialActivitySumB += partialActivityReal;
                        activitySetReal |= 0b0100;
                        break;
                    case "т":
                        partialActivitySumT += partialActivityReal;
                        activitySetReal |= 0b1000;
                        break;
                    case "у":
                        partialActivitySumU += partialActivityReal;
                        activitySetReal |= 0b0001;
                        break;
                }
            }
        }
        if (activitySetReal == 0 || activitySetExpected == 0 || activitySetExpected != activitySetReal)
        {
            //nuclid data incomplete and cannot be worked with
        }
        else
        {
            partialActivitySumA *= massSum;
            partialActivitySumB *= massSum;
            partialActivitySumT *= massSum;
            partialActivitySumU *= massSum;
            if (partialActivitySumA > activitySumA * 1.05f || activitySumA > 1.05f * partialActivitySumA)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = lines.Count == 1 
                        ? forms[lines[0]].NumberInOrder_DB.ToString() 
                        : $"{forms[lines[0]].NumberInOrder_DB} - {forms[lines[^1]].NumberInOrder_DB}",
                    Column = "AlphaActivity_DB",
                    Value = forms[lines[0]].AlphaActivity_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Сведения об удельной активности радионуклидов, представленные в графе 13, " +
                              $"не соответствуют сведениям о суммарной активности в графе 28 (ожидается {partialActivitySumA:0.00e+0})."
                });
            }
            if (partialActivitySumB > activitySumB * 1.05f || activitySumB > 1.05f * partialActivitySumB)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = lines.Count == 1 
                        ? forms[lines[0]].NumberInOrder_DB.ToString() 
                        : $"{forms[lines[0]].NumberInOrder_DB} - {forms[lines[^1]].NumberInOrder_DB}",
                    Column = "BetaGammaActivity_DB",
                    Value = forms[lines[0]].BetaGammaActivity_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Сведения об удельной активности радионуклидов, представленные в графе 13, " +
                              $"не соответствуют сведениям о суммарной активности в графе 27 (ожидается {partialActivitySumB:0.00e+0})."
                });
            }
            if (partialActivitySumT > activitySumT * 1.05f || activitySumT > 1.05f * partialActivitySumT)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = lines.Count == 1 
                        ? forms[lines[0]].NumberInOrder_DB.ToString() 
                        : $"{forms[lines[0]].NumberInOrder_DB} - {forms[lines[^1]].NumberInOrder_DB}",
                    Column = "TritiumActivity_DB",
                    Value = forms[lines[0]].TritiumActivity_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              $"Сведения об удельной активности радионуклидов, представленные в графе 13, " +
                              $"не соответствуют сведениям о суммарной активности в графе 26 (ожидается {partialActivitySumT:0.00e+0})."
                });
            }
            if (partialActivitySumU > activitySumU * 1.05f || activitySumU > 1.05f * partialActivitySumU)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = lines.Count == 1 
                        ? forms[lines[0]].NumberInOrder_DB.ToString() 
                        : $"{forms[lines[0]].NumberInOrder_DB} - {forms[lines[^1]].NumberInOrder_DB}",
                    Column = "TransuraniumActivity_DB",
                    Value = forms[lines[0]].TransuraniumActivity_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              $"Сведения об удельной активности радионуклидов, представленные в графе 13, " +
                              $"не соответствуют сведениям о суммарной активности в графе 29 (ожидается {partialActivitySumU:0.00e+0})."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check014

    private static List<CheckError> Check_014(List<Form17> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        List<string> applicableDocumentVids = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "19" };
        var documentVid = forms[line].DocumentVid_DB?.ToString().Trim()??"";
        const byte graphNumber = 14;
        var noteExists = CheckNotePresence(notes, line, graphNumber);
        if (string.IsNullOrWhiteSpace(documentVid))
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentVid_DB",
                Value = forms[line].DocumentVid_DB.ToString(),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Графа не может быть пустой."
            });
        }
        else if (!applicableDocumentVids.Contains(documentVid))
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentVid_DB",
                Value = forms[line].DocumentVid_DB.ToString(),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Недопустимый код вида документа."
            });
        }
        else if (documentVid == "19" && !noteExists)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentVid_DB",
                Value = forms[line].DocumentVid_DB.ToString(),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Необходимо примечание с указанием наименования документа."
            });
        }
        return result;
    }

    #endregion

    #region Check015

    private static List<CheckError> Check_015(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var documentNumber = forms[line].DocumentNumber_DB;
        var valid = !string.IsNullOrWhiteSpace(documentNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentNumber_DB",
                Value = forms[line].DocumentNumber_DB,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check016

    private static List<CheckError> Check_016(List<Form17> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var operationDate = forms[line].OperationDate_DB;
        var documentDate = forms[line].DocumentDate_DB;
        DateOnly pMid;
        bool valid;
        if (operationCode == "10")
        {
            valid = DateOnly.TryParse(rep.StartPeriod_DB, out var pStart)
                    && DateOnly.TryParse(rep.EndPeriod_DB, out var pEnd)
                    && DateOnly.TryParse(documentDate, out pMid)
                    && pMid >= pStart && pMid <= pEnd;
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
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
            valid = DateOnly.TryParse(documentDate, out pMid)
                    && DateOnly.TryParse(operationDate, out var pOper)
                    && pMid <= pOper;
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
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
    private static List<CheckError> Check_017_10(List<Form17> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "01", "10", "11", "12", "13", "14", "16", "18", "43", "44", "45", "51", "52", "55", "68", "71", "97", "98" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOKPO = forms[line].ProviderOrRecieverOKPO_DB;
        var repOKPO = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        //var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = providerOrRecieverOKPO == repOKPO;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Для выбранного кода операции указывается код ОКПО отчитывающейся организации."
            });
        }
        return result;
    }

    #endregion

    #region Check017_21

    //Код ОКПО поставщика/получателя не равен коду ОКПО отчитывающейся организации + 8/14 цифр
    private static List<CheckError> Check_017_21(List<Form17> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "21", "25", "26", "27", "28", "29", "31", "35", "36", "37", "38", "39", "63", "64" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOKPO = forms[line].ProviderOrRecieverOKPO_DB;
        var repOKPO = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;

        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(providerOrRecieverOKPO);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Значение может состоять только из 8 или 14 символов."
            });
        }
        valid = providerOrRecieverOKPO != repOKPO;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Для выбранного кода операции указывается код ОКПО контрагента."
            });
        }
        return result;
    }

    #endregion

    #region Check017_22

    //Код ОКПО поставщика/получателя состоит из 8/14 чисел или "минобороны"
    private static List<CheckError> Check_017_22(List<Form17> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "22", "32" };
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOKPO = (forms[line].ProviderOrRecieverOKPO_DB ?? string.Empty).Trim();
        var repOKPO = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = providerOrRecieverOKPO.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase)
                    || (okpoRegex.IsMatch(providerOrRecieverOKPO) && providerOrRecieverOKPO != repOKPO);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOKPO),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Для выбранного кода операции указывается код ОКПО контрагента, либо «Минобороны» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check018_01

    //При определенных кодах операции, код ОКПО перевозчика равен "-"
    private static List<CheckError> Check_018_01(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "01", "10", "11", "12", "13", "14", "16", "18", "43", "44", "45", "51", "52", "55", "68", "71", "97", "98"
        };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var transporterOKPO = (forms[line].TransporterOKPO_DB ?? string.Empty).Trim();
        var valid = transporterOKPO is "-";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "При выбранном коде операции транспортирование не производится, необходим символ «-» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check018_21

    //При определенных кодах операции, код ОКПО перевозчика равен 8/14 цифр
    private static List<CheckError> Check_018_21(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "21","25","26","27","28","29","31","35","36","37","38","39"
        };
        var operationCode = forms[line].OperationCode_DB;
        var transporterOKPO = forms[line].TransporterOKPO_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(transporterOKPO);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Проверьте правильность предоставленных сведений."
            });
        }
        return result;
    }

    #endregion

    #region Check018_22

    //Код ОКПО перевозчика состоит из 8/14 чисел или "минобороны", не ОКПО отчитывающейся
    private static List<CheckError> Check_018_22(List<Form17> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "22", "32" };
        var repOKPO = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB)
            ? forms10[1].Okpo_DB
            : forms10[0].Okpo_DB;
        var operationCode = forms[line].OperationCode_DB;
        var transporterOKPO = (forms[line].TransporterOKPO_DB ?? string.Empty).Trim();
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = transporterOKPO.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase)
                    || (okpoRegex.IsMatch(transporterOKPO) && transporterOKPO != repOKPO);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Для выбранного кода операции указывается код ОКПО перевозчика, либо «Минобороны» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check019

    private static List<CheckError> Check_019(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var storagePlaceNameDB = forms[line].StoragePlaceName_DB;
        var valid = !string.IsNullOrWhiteSpace(storagePlaceNameDB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StoragePlaceName_DB",
                Value = Convert.ToString(storagePlaceNameDB),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Графа 19 должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check020

    private static List<CheckError> Check_020(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var storagePlaceCodeDB = forms[line].StoragePlaceCode_DB;
        var valid = !string.IsNullOrWhiteSpace(storagePlaceCodeDB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StoragePlaceCode_DB",
                Value = Convert.ToString(storagePlaceCodeDB),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Графа 20 должна быть заполнена. Допускается прочерк."
            });
        }
        return result;
    }

    #endregion

    #region Check021

    private static List<CheckError> Check_021(List<Form17> forms, List<int> lines)
    {
        List<CheckError> result = new();
        var kodRAORegex = new Regex(@"^\d{11}?$");
        var nuclidsRight =
            ((TryParseFloatExtended(forms[lines[0]].TransuraniumActivity_DB, out _) ? 1 : 0) << 0) +
            ((TryParseFloatExtended(forms[lines[0]].AlphaActivity_DB, out _) ? 1 : 0) << 1) +
            ((TryParseFloatExtended(forms[lines[0]].BetaGammaActivity_DB, out _) ? 1 : 0) << 2) +
            ((TryParseFloatExtended(forms[lines[0]].TritiumActivity_DB, out _) ? 1 : 0) << 3);
        var nuclidsLeft = 0;
        foreach (var nuclid in lines
                     .Where(line => R
                         .Any(phEntry => phEntry["name"] == (forms[line].Radionuclids_DB ?? string.Empty).ToLower().Trim()))
                     .Select(line =>
                         R.First(phEntry => phEntry["name"] == (forms[line].Radionuclids_DB ?? string.Empty).ToLower().Trim())))
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
            var kodRao = (forms[line].CodeRAO_DB ?? string.Empty).Trim();
            if (!kodRAORegex.IsMatch(kodRao)) continue;

            #region 1-й символ кода РАО

            if (kodRao[..1] != "2")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = forms[line].CodeRAO_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "В форме 1.7 приводятся сведения только о твёрдых кондиционированных РАО (1-й символ кода РАО должен быть равен 2)."
                });
            }

            #endregion

            #region 2-й символ кода РАО

            if (kodRao.Substring(1, 1) == "9")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = forms[line].CodeRAO_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Для кондиционированных РАО должна быть определена категория (2-й символ кода РАО не может быть равен 9)."
                });
            }

            #endregion

            #region 3-й символ кода РАО

            Dictionary<int, int> expectedSymbols = new() {
                { 0b0001, 1 },
                { 0b0010, 2 },
                { 0b0011, 3 },
                { 0b0100, 4 },
                { 0b0101, 6 },  //not in spec
                { 0b0110, 5 },
                { 0b0111, 6 },
                { 0b1000, 4 },
                { 0b1001, 6 },  //not in spec
                { 0b1010, 5 },  //not in spec
                { 0b1011, 6 },  //not in spec
                { 0b1100, 4 },
                { 0b1101, 3 },  //not in spec
                { 0b1110, 5 },
                { 0b1111, 6 },
                //tritium is considered a beta nuclid for the "not in spec" entries
            };
            var expectedSymbolRight = nuclidsRight == 0 ? 0 : expectedSymbols[nuclidsRight];
            var expectedSymbolLeft = nuclidsLeft == 0 ? 0 : expectedSymbols[nuclidsLeft];
            if (expectedSymbolRight == expectedSymbolLeft)
            {
                if (kodRao.Substring(2, 1) == "0" 
                    || kodRao.Substring(2, 1) != expectedSymbolRight.ToString())
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_17",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = forms[line].CodeRAO_DB,
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                  $"Укажите 3-й символ кода РАО в соответствии с радионуклидным составом (ожидается {expectedSymbolRight})."
                    });
                }
            }

            #endregion

            #region 6-й символ кода РАО

            if (kodRao.Substring(5, 1) == "0")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = forms[line].CodeRAO_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Для РАО, приведенных к критериям приемлемости, должен быть определен период потенциальной опасности " +
                              "(6-й символ кода РАО не может быть равен 0)."
                });
            }

            #endregion

            #region 8-й символ кода РАО

            List<string> applicableRao3 = new() { "1", "2", "3", "4", "6" };
            if (!applicableRao3.Contains(kodRao.Substring(7, 1)))
            {

                if (kodRao.Substring(7, 1) == "5")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_17",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = forms[line].CodeRAO_DB,
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                  "В форме 1.7 приводятся сведения только о твердых РАО, приведенных к критериям приемлемости " +
                                  "(8-й символ кода РАО не может быть равен 5)."
                    });
                }
                else
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_17",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = forms[line].CodeRAO_DB,
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                  "Сведения о некондиционированных отходах необходимо представлять в формах 1.5 и 1.6 " +
                                  "(8-й символ кода РАО в форме 1.7 ожидается равным 1, 2, 3, 4, 6)."
                    });
                }
            }
            else
            {
                if (kodRao.Substring(7, 1) == "6" 
                    && kodRao.Substring(1, 1) != "0")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_17",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = forms[line].CodeRAO_DB,
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                  "Твердые РАО, содержащие природные радионуклиды, относятся к ОНАО " +
                                  "(при 8-ом символе кода РАО, равном 6, ожидается 2-й символ кода РАО, равный 0)."
                    });
                }
            }

            #endregion

            #region проверка кода РАО

            //

            #endregion

        }
        return result;
    }

    #endregion

    #region Check021_RAOCODE

    private static List<CheckError> Check021_RAOCODE(List<Form17> forms, List<Note> notes, List<int> lines)
    {
        List<CheckError> result = new();
        var comparator = new CustomNullStringWithTrimComparer();

        #region data fetch

        var operationCode = forms[lines[0]].OperationCode_DB;
        var radArray = (lines
            .Where(line => R
                .Any(phEntry => phEntry["name"] == (forms[line].Radionuclids_DB ?? string.Empty).ToLower().Trim()))
            .Select(line => ((forms[line].Radionuclids_DB ?? string.Empty).ToLower().Trim(),
                (forms[line].SpecificActivity_DB ?? string.Empty).ToLower().Trim())))
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

        List<string> codeRaoDBs = new();
        foreach (var line in lines)
        {
            var codeRaoDB = (forms[line].CodeRAO_DB ?? string.Empty).Trim();
            if (codeRaoDB is "" or "-") continue;
            codeRaoDBs.Add(codeRaoDB);
        }

        #endregion

        foreach (var line in lines)
        {
            var nuclidActivityT = forms[line].TritiumActivity_DB;
            var nuclidActivityA = forms[line].AlphaActivity_DB;
            var nuclidActivityB = forms[line].BetaGammaActivity_DB;
            var nuclidActivityU = forms[line].TransuraniumActivity_DB;
            var nuclidMassOutOfPack = forms[line].MassOutOfPack_DB;
            var refineOrSortRaoCode = forms[line].RefineOrSortRAOCode_DB;
            var nuclidsExistT = TryParseFloatExtended(nuclidActivityT, out var nuclidActivityTReal);
            var nuclidsExistA = TryParseFloatExtended(nuclidActivityA, out var nuclidActivityAReal);
            var nuclidsExistB = TryParseFloatExtended(nuclidActivityB, out var nuclidActivityBReal);
            var nuclidsExistU = TryParseFloatExtended(nuclidActivityU, out var nuclidActivityUReal);
            var nuclidMassExists = TryParseFloatExtended(nuclidMassOutOfPack, out var nuclidMassOutOfPackReal);
            const byte graphNumber = 21;
            var noteExists = CheckNotePresence(notes, line, graphNumber);
            var codeRaoDB = (forms[line].CodeRAO_DB ?? string.Empty).Trim();
            if (codeRaoDB is "" or "-") continue;
            var valid = codeRaoDB.Length == 11 && codeRaoDB.All(char.IsDigit);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = codeRaoDB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте правильность заполнения кода РАО."
                });
                continue;
            }

            #region setup

            var codeRao1MatterState = codeRaoDB[..1];
            var codeRao2RaoCategory = codeRaoDB.Substring(1, 1);
            var codeRao3NuclidTypes = codeRaoDB.Substring(2, 1);
            var codeRao4HasNuclears = codeRaoDB.Substring(3, 1);
            var codeRao5HalfLife = codeRaoDB.Substring(4, 1);
            var codeRao6DangerPeriod = codeRaoDB.Substring(5, 1);
            var codeRao7RecycleMethod = codeRaoDB.Substring(6, 1);
            var codeRao8RaoClass = codeRaoDB.Substring(7, 1);
            var codeRao910TypeCode = codeRaoDB.Substring(8, 2);
            var codeRao11Flammability = codeRaoDB.Substring(10, 1);
            var codeRao1Allowed = new [] { "2" };
            var codeRao2Allowed = new [] { "0", "1", "2", "3", "4" };
            var codeRao3Allowed = new [] { "1", "2", "3", "4", "5", "6" };
            var codeRao4Allowed = new [] { "1", "2" };
            var codeRao5Allowed = new [] { "1", "2" };
            var codeRao6Allowed = new [] { "1", "2", "3" };
            var codeRao7Allowed = new [] { "0", "1", "2", "3", "4", "9" };
            var codeRao8Allowed = new [] { "1", "2", "3", "4", "6" };
            var codeRao910Allowed = new []
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
            var codeRao11Allowed = new [] { "1", "2" };

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

            var recyclingTypes = new [] 
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
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao1MatterState} (1-ый символ кода РАО), {codeRao910TypeCode} (9-10 символы кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "В форме 1.7 приводятся сведения только о твердых кондиционированных РАО (1-й символ кода РАО 2)."
                });
            }
            else
            {
                var validTypeCodeSolid = new List<string>();
                //solids: symbols 9-10 are 11-39
                for (var i = 11; i <= 39; i++)
                {
                    validTypeCodeSolid.Add(i.ToString("D2"));
                }
                if (codeRao1MatterState != "2")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_17",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao1MatterState} (1-ый символ кода РАО)",
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                  "В форме 1.7 приводятся сведения только о твердых кондиционированных РАО."
                    });
                }
                else if (validTypeCodeSolid.Contains(codeRao910TypeCode) && codeRao7RecycleMethod == "0")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_17",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao7RecycleMethod} (7 символ кода РАО), {codeRao910TypeCode} (9-10 символы кода РАО)",
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                  "В форме 1.7 приводятся сведения только о твердых кондиционированных РАО."
                    });
                }
            }

            #endregion

            #region symbol 2
            if (line == 24811)
            {

            }
            if (!codeRao2Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Недопустимое значение 2-го символа кода РАО."
                });
            }
            else if (codeRaoDBs.Count > 1)
            {

            }
            else switch (codeRao2RaoCategory)
            {
                case "4":
                {
                    var validTypeCode = new [] { "81", "82", "85", "86", "87", "88", "89" };
                    if (!validTypeCode.Contains(codeRao910TypeCode))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_17",
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
                        FormNum = "form_17",
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
                            if (codeRao1MatterState == "2")
                            {
                                if (a < 1e07) { if (codeMax < 0) codeMax = 0; }
                                else if (a < 1e08) { if (codeMax < 1) codeMax = 1; }
                                else if (a < 1e11) { if (codeMax < 2) codeMax = 2; }
                                else { if (codeMax < 3) codeMax = 3; }
                            }
                        }

                        #endregion

                        #region a for beta-gamma

                        if (nuclidsExistB)
                        {
                            var a = nuclidActivityBReal / (nuclidMassOutOfPackReal * 1e6);
                            if (codeRao1MatterState == "2")
                            {
                                if (a < 1e03) { if (codeMax < 0) codeMax = 0; }
                                else if (a < 1e04) { if (codeMax < 1) codeMax = 1; }
                                else if (a < 1e07) { if (codeMax < 2) codeMax = 2; }
                                else { if (codeMax < 3) codeMax = 3; }
                            }
                        }

                        #endregion

                        #region a for alpha

                        if (nuclidsExistA)
                        {
                            var a = nuclidActivityAReal / (nuclidMassOutOfPackReal * 1e6);
                            if (codeRao1MatterState == "2")
                            {
                                if (a < 1e02) { if (codeMax < 0) codeMax = 0; }
                                else if (a < 1e03) { if (codeMax < 1) codeMax = 1; }
                                else if (a < 1e06) { if (codeMax < 2) codeMax = 2; }
                                else { if (codeMax < 3) codeMax = 3; }
                            }
                        }

                        #endregion

                        #region a for transuraniums

                        if (nuclidsExistU)
                        {
                            var a = nuclidActivityUReal / (nuclidMassOutOfPackReal * 1e6);
                            if (codeRao1MatterState == "2")
                            {
                                if (a < 1e01) { if (codeMax < 0) codeMax = 0; }
                                else if (a < 1e02) { if (codeMax < 1) codeMax = 1; }
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
                            FormNum = "form_17",
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
                            FormNum = "form_17",
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
                    FormNum = "form_17",
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
                        FormNum = "form_17",
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
                            FormNum = "form_17",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao3NuclidTypes} (3-ий символ кода РАО)",
                            Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                      "Радионуклиды, указанные в графе 12 не соответствуют 3-му символу кода РАО."
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
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao4HasNuclears} (4-ый символ кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Недопустимое значение 4-го символа кода РАО."
                });
            }
            else
            {
                var nuclears = new []
                {
                    "плутоний", "уран-233", "уран-235", "уран-238", "нептуний-237", "америций-241", 
                    "америций-243", "калифорний-252", "торий", "литий-6", "тритий"
                };
                var operations12 = new [] { "12" };
                var operations11 = new [] { "11", "13", "14", "16", "41" };
                var nuclearsExist = radArray
                    .Any(x => nuclears
                        .Any(y => x.Item1.Contains(y, StringComparison.CurrentCultureIgnoreCase)));
                if (codeRao4HasNuclears == "1")
                {
                    if (operations12.Contains(operationCode))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_17",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = codeRao4HasNuclears,
                            Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                      "4-ый символ кода РАО не может быть равен 1 при коде операции 12."
                        });
                    }
                    else if (operations11.Contains(operationCode))
                    {
                        //anything is allowed
                    }
                    else
                    {
                        //anything is allowed
                    }
                }
                else if (codeRao4HasNuclears == "2")
                {
                    if (operations12.Contains(operationCode))
                    {
                        if (!nuclearsExist)
                        {
                            result.Add(new CheckError
                            {
                                FormNum = "form_17",
                                Row = forms[line].NumberInOrder_DB.ToString(),
                                Column = "CodeRAO_DB",
                                Value = $"{codeRao4HasNuclears} (4-ый символ кода РАО)",
                                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                          "4-ый символ кода РАО может быть равен 2 при коде операции 12 только при указании радионуклидов, " +
                                          "которые могут быть отнесены к ЯМ."
                            });
                        }
                    }
                    else if (operations11.Contains(operationCode))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_17",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao4HasNuclears} (4-ый символ кода РАО)",
                            Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                      "4-ый символ кода РАО не может быть равен 2 при кодах операции 11, 13, 14, 16 и 41."
                        });
                    }
                    else
                    {
                        if (!nuclearsExist)
                        {
                            result.Add(new CheckError
                            {
                                FormNum = "form_17",
                                Row = forms[line].NumberInOrder_DB.ToString(),
                                Column = "CodeRAO_DB",
                                Value = $"{codeRao4HasNuclears} (4-ый символ кода РАО)",
                                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                          "4-ый символ кода РАО может быть равен 2 при данном коде операции только при указании радионуклидов, " +
                                          "которые могут быть отнесены к ЯМ."
                            });
                        }
                    }
                }
            }

            #endregion

            #region symbol 5

            if (!codeRao5Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
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
                        FormNum = "form_17",
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
                        FormNum = "form_17",
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
                    FormNum = "form_17",
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
                string expectedPeriodOutput = "";
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
                        && TryParseFloatExtended(nuclidData["OSPORB_Solid"], out var a)
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
                        if (TryParseFloatExtended(nuclidData["OSPORB_Solid"], out var osporbA) && osporbA > 0)
                        {
                            a = osporbA;
                        }
                        else if (TryParseFloatExtended(nuclidData["A_Solid"], out var solidA) && solidA > 0)
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
                            controlValue100 += nuclidActivity / (float)Math.Pow(2,100/ (T / unitAdjustment)) / a;
                            controlValue500 += nuclidActivity / (float)Math.Pow(2,500/ (T / unitAdjustment)) / a;
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
                        FormNum = "form_17",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao6DangerPeriod} (6-ой символ кода РАО)",
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                  $"Расчетное значение периода потенциальной опасности (в годах): {expectedPeriodOutput} (6-ой символ кода РАО {expectedValue})."
                    });
                }
            }
            
            #endregion

            #region symbol 7

            if (!codeRao7Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao7RecycleMethod} (7-ой символ кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Недопустимое значение 7-го символа кода РАО."
                });
            }
            else if (forms[lines[0]].OperationCode_DB == "55")
            {
                Dictionary<string, string[]> validRecycles = new() 
                {
                    { "0", new[]
                        {
                            "21","22","23","24","29",
                            "51","52","53","54","55",
                            "72","73","74","79","99",
                            "-"
                        }
                    },
                    { "1", new[]
                        {
                            "31","32","39"
                        }
                    },
                    { "2", new[]
                        {
                            "41"
                        }
                    },
                    { "3", new[]
                        {
                            "42","71"
                        }
                    },
                    { "4", new[]
                        {
                            "43"
                        }
                    },
                    { "9", new[]
                        {
                            "49"
                        }
                    }
                };
                valid = validRecycles[codeRao7RecycleMethod].Contains(refineOrSortRaoCode);
                if (!valid)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_17",
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
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao8RaoClass} (8-ой символ кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Сведения о некондиционированных отходах необходимо представлять в формах 1.5 и 1.6."
                });
            }
            else
            {
                var formingDateDB = forms[lines[0]].FormingDate_DB;
                if (DateOnly.TryParse(formingDateDB, out var formingDateReal))
                {
                    List<(string, string, float)> nuclidsAShort = new();
                    List<(string, string, float)> nuclidsBShort = new();
                    List<(string, string, float)> nuclidsTShort = new();
                    List<(string, string, float)> nuclidsUBetaShort = new();
                    List<(string, string, float)> nuclidsUAlphaShort = new();
                    List<(string, string, float)> nuclidsALong = new();
                    List<(string, string, float)> nuclidsBLong = new();
                    List<(string, string, float)> nuclidsTLong = new();
                    List<(string, string, float)> nuclidsUBetaLong = new();
                    List<(string, string, float)> nuclidsUAlphaLong = new();
                    List<(string, string, float)> nuclidsSr90 = new();
                    var nuclidActivitySumAShort = 0.0f;
                    var nuclidActivitySumBShort = 0.0f;
                    var nuclidActivitySumTShort = 0.0f;
                    var nuclidActivitySumUBetaShort = 0.0f;
                    var nuclidActivitySumUAlphaShort = 0.0f;
                    var nuclidActivitySr90 = 0.0f;
                    var nuclidActivitySumALong = 0.0f;
                    var nuclidActivitySumBLong = 0.0f;
                    var nuclidActivitySumTLong = 0.0f;
                    var nuclidActivitySumUBetaLong = 0.0f;
                    var nuclidActivitySumUAlphaLong = 0.0f;
                    var wattSum = 0.0f;
                    var nuclidActivityClass = 0;
                    var nuclidsLongExist = "2";
                    const float yearEdge = 31.0f;
                    List<string> nuclidClass = new();
                    foreach (var nuclid in radArray)
                    {
                        var nuclidId = R.FindIndex(x => comparator.Compare(x["name"], nuclid.Item1) == 0);
                        if (nuclidId < 0
                            || !TryParseFloatExtended(R[nuclidId]["value"], out var halfLifeVal)
                            || !validUnits.TryGetValue(R[nuclidId]["unit"], out var halfLifeUnit)) continue;
                        TryParseFloatExtended(R[nuclidId]["Ki"], out var watt);
                        var nuclidLong = halfLifeVal / halfLifeUnit > yearEdge;
                        if (nuclidLong) nuclidsLongExist = "1";
                        if (comparator.Compare("стронций-90", nuclid.Item1) == 0)
                        {
                            nuclidsSr90.Add((nuclid.Item1,nuclid.Item2,watt));
                        }
                        else switch (R[nuclidId]["code"])
                        {
                            case "а":
                                (nuclidLong ? nuclidsALong : nuclidsAShort).Add((nuclid.Item1, nuclid.Item2, watt));
                                break;
                            case "б":
                                (nuclidLong ? nuclidsBLong : nuclidsBShort).Add((nuclid.Item1, nuclid.Item2, watt));
                                break;
                            case "т":
                                (nuclidLong ? nuclidsTLong : nuclidsTShort).Add((nuclid.Item1, nuclid.Item2, watt));
                                break;
                            case "у":
                                if (R[nuclidId]["decay"] == "альфа")
                                    (nuclidLong ? nuclidsUAlphaLong : nuclidsUAlphaShort).Add((nuclid.Item1, nuclid.Item2, watt));
                                else
                                    (nuclidLong ? nuclidsUBetaLong : nuclidsUBetaShort).Add((nuclid.Item1, nuclid.Item2, watt));
                                break;
                        }
                    }
                    foreach (var nuclid in nuclidsAShort)
                    {
                        if (!TryParseFloatExtended(nuclid.Item2, out var nuclidActivityReal)) continue;
                        nuclidActivitySumAShort += nuclidActivityReal; 
                        wattSum += nuclid.Item3;
                    }
                    foreach (var nuclid in nuclidsBShort)
                    {
                        if (!TryParseFloatExtended(nuclid.Item2, out var nuclidActivityReal)) continue;
                        nuclidActivitySumBShort += nuclidActivityReal;
                        wattSum += nuclid.Item3;
                    }
                    foreach (var nuclid in nuclidsTShort)
                    {
                        if (!TryParseFloatExtended(nuclid.Item2, out var nuclidActivityReal)) continue;
                        nuclidActivitySumTShort += nuclidActivityReal; 
                        wattSum += nuclid.Item3;
                    }
                    foreach (var nuclid in nuclidsUAlphaShort)
                    {
                        if (!TryParseFloatExtended(nuclid.Item2, out var nuclidActivityReal)) continue;
                        nuclidActivitySumUAlphaShort += nuclidActivityReal; 
                        wattSum += nuclid.Item3;
                    }
                    foreach (var nuclid in nuclidsUBetaShort)
                    {
                        if (!TryParseFloatExtended(nuclid.Item2, out var nuclidActivityReal)) continue;
                        nuclidActivitySumUBetaShort += nuclidActivityReal; 
                        wattSum += nuclid.Item3;
                    }
                    foreach (var nuclid in nuclidsSr90)
                    {
                        if (!TryParseFloatExtended(nuclid.Item2, out var nuclidActivityReal)) continue;
                        nuclidActivitySr90 += nuclidActivityReal; 
                        wattSum += nuclid.Item3;
                    }
                    foreach (var nuclid in nuclidsALong)
                    {
                        if (!TryParseFloatExtended(nuclid.Item2, out var nuclidActivityReal)) continue;
                        nuclidActivitySumALong += nuclidActivityReal; 
                        wattSum += nuclid.Item3;
                    }
                    foreach (var nuclid in nuclidsBLong)
                    {
                        if (!TryParseFloatExtended(nuclid.Item2, out var nuclidActivityReal)) continue;
                        nuclidActivitySumBLong += nuclidActivityReal; 
                        wattSum += nuclid.Item3;
                    }
                    foreach (var nuclid in nuclidsTLong)
                    {
                        if (!TryParseFloatExtended(nuclid.Item2, out var nuclidActivityReal)) continue;
                        nuclidActivitySumTLong += nuclidActivityReal; 
                        wattSum += nuclid.Item3;
                    }
                    foreach (var nuclid in nuclidsUAlphaLong)
                    {
                        if (!TryParseFloatExtended(nuclid.Item2, out var nuclidActivityReal)) continue;
                        nuclidActivitySumUAlphaLong += nuclidActivityReal; 
                        wattSum += nuclid.Item3;
                    }
                    foreach (var nuclid in nuclidsUBetaLong)
                    {
                        if (!TryParseFloatExtended(nuclid.Item2, out var nuclidActivityReal)) continue;
                        nuclidActivitySumUBetaLong += nuclidActivityReal; 
                        wattSum += nuclid.Item3;
                    }
                    if (formingDateReal.Year < 2024)
                    {
                        //old classification
                        var nuclidActivityTotalA = nuclidActivitySumAShort + nuclidActivitySumALong;
                        var nuclidActivityTotalB = nuclidActivitySumBShort + nuclidActivitySumBLong + nuclidActivitySr90;
                        var nuclidActivityTotalT = nuclidActivitySumTShort + nuclidActivitySumTLong;
                        var nuclidActivityTotalU = nuclidActivitySumUAlphaShort + nuclidActivitySumUAlphaLong + 
                                                   nuclidActivitySumUBetaShort + nuclidActivitySumUBetaLong;
                        if (nuclidActivityTotalA <= 1e2) nuclidActivityClass = Math.Max(nuclidActivityClass, 0);
                        else if (nuclidActivityTotalA <= 1e3) nuclidActivityClass = Math.Max(nuclidActivityClass, 1);
                        else if (nuclidActivityTotalA <= 1e6) nuclidActivityClass = Math.Max(nuclidActivityClass, 2);
                        else nuclidActivityClass = Math.Max(nuclidActivityClass, 3);
                        if (nuclidActivityTotalB <= 1e3) nuclidActivityClass = Math.Max(nuclidActivityClass, 0);
                        else if (nuclidActivityTotalB <= 1e4) nuclidActivityClass = Math.Max(nuclidActivityClass, 1);
                        else if (nuclidActivityTotalB <= 1e7) nuclidActivityClass = Math.Max(nuclidActivityClass, 2);
                        else nuclidActivityClass = Math.Max(nuclidActivityClass, 3);
                        if (nuclidActivityTotalT <= 1e7) nuclidActivityClass = Math.Max(nuclidActivityClass, 0);
                        else if (nuclidActivityTotalT <= 1e8) nuclidActivityClass = Math.Max(nuclidActivityClass, 1);
                        else if (nuclidActivityTotalT <= 1e11) nuclidActivityClass = Math.Max(nuclidActivityClass, 2);
                        else nuclidActivityClass = Math.Max(nuclidActivityClass, 3);
                        if (nuclidActivityTotalU <= 1e1) nuclidActivityClass = Math.Max(nuclidActivityClass, 0);
                        else if (nuclidActivityTotalU <= 1e2) nuclidActivityClass = Math.Max(nuclidActivityClass, 1);
                        else if (nuclidActivityTotalU <= 1e5) nuclidActivityClass = Math.Max(nuclidActivityClass, 2);
                        else nuclidActivityClass = Math.Max(nuclidActivityClass, 3);
                        switch (nuclidActivityClass)
                        {
                            case 0:
                                nuclidClass.Add("4");
                                nuclidClass.Add("6");
                                break;
                            case 1:
                                nuclidClass.Add(nuclidsLongExist == "1" ? "3" : "4");
                                break;
                            case 2:
                                nuclidClass.Add(nuclidsLongExist == "1" ? "2" : "3");
                                break;
                            case 3:
                                nuclidClass.Add("1");
                                nuclidClass.Add("2");
                                break;
                        }
                    }
                    else
                    {
                        //new classification
                        var mass = forms[lines[0]].MassOutOfPack_DB; //OutOfPack
                        var volume = forms[lines[0]].Volume_DB;  //not OutOfPack
                        if (TryParseFloatExtended(mass, out var massReal) && TryParseFloatExtended(volume, out var volumeReal) && volumeReal != 0.0f)
                        {
                            var aSumUdH3 = nuclidActivitySumTLong + nuclidActivitySumTLong;
                            var aSumUdBDzn = nuclidActivitySumBLong + nuclidActivitySumUBetaLong;
                            var aSumUdBKzn = nuclidActivitySumBShort + nuclidActivitySumUBetaShort + nuclidActivitySr90;
                            var aSumUdBKznWoSr90 = nuclidActivitySumBShort + nuclidActivitySumUBetaShort;
                            var aSumUdADzn = nuclidActivitySumALong + nuclidActivitySumUAlphaLong;
                            //var ASumObA = (nuclidActivitySumALong + nuclidActivitySumUAlphaLong + nuclidActivitySumAShort + nuclidActivitySumUAlphaShort) * massReal * 1e6f / volumeReal;
                            var M = massReal * 1e6f;
                            var V = volumeReal;
                            if (aSumUdBDzn < 1e4
                                && aSumUdADzn < 4e2
                                && (aSumUdBKzn / 1e4 + aSumUdH3 / 1e8) < 1)
                            {
                                nuclidClass.Add("4");
                                nuclidClass.Add("6");
                            }
                            else
                            if (aSumUdBDzn < 1e6
                                && aSumUdADzn < 4e3
                                && M * nuclidActivitySr90 < 2e12
                                && (aSumUdBKznWoSr90 / 1e7 + aSumUdH3 / 1e11) < 1)
                                nuclidClass.Add("3");
                            else
                            if (wattSum * M / V < 100)
                                nuclidClass.Add("2");
                            else
                                nuclidClass.Add("1");
                        }
                    }
                    valid = nuclidClass.Contains(codeRao8RaoClass);
                    if (!valid)
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_17",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao8RaoClass} (8 символ кода РАО)",
                            Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                      $"Проверьте правильность выбора класса РАО (ожидается {string.Join(", ", nuclidClass)})."
                        });
                    }
                }
            }

            #endregion

            #region symbols 9-10

            if (!codeRao910Valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao910TypeCode} (9-10 символы кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Недопустимое значение 9-10 символов кода РАО."
                });
            }
            else if (codeRao910TypeCode == "94")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao910TypeCode} (9-10 символы кода РАО)",
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Сведения о РАО, подготовленных для передачи национальному оператору, предоставляются с форме 1.8."
                });
            }
            else
            {
                var requiresNote = new [] { "19", "29", "39", "59", "69", "78", "79", "89" };
                if (requiresNote.Contains(codeRao910TypeCode) && !noteExists)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_17",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao910TypeCode} (9-10 символы кода РАО)",
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                  "Необходимо заполнить примечание к коду типа РАО."
                    });
                }
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

    private static List<CheckError> Check_022_11(List<Form17> forms, List<Form10> forms10, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "11", "12", "13", "14", "16" };
        var operationCode = forms[lines[0]].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var repOKPOList = forms10
            .Select(x => x.Okpo_DB)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        foreach (var line in lines)
        {
            var statusRaoDB = (forms[line].StatusRAO_DB ?? string.Empty).Trim();
            if (statusRaoDB is "" or "-") continue;
            var valid = repOKPOList.Contains(statusRaoDB);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
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
    private static List<CheckError> Check_022_26(List<Form17> forms, List<Form10> forms10, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new string[] { "26", "28", "63" };
        var operationCode = forms[lines[0]].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var repOKPO_List = forms10.Select(x => x.Okpo_DB).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        foreach (var line in lines)
        {
            var statusRaoDB = (forms[line].StatusRAO_DB ?? string.Empty).Trim();
            if (statusRaoDB is "" or "-") continue;
            var valid = repOKPO_List.Contains(statusRaoDB);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "StatusRAO_DB",
                    Value = statusRaoDB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Операция, соответствующая выбранному коду, может использоваться только для собственных РАО."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check022_38

    private static List<CheckError> Check_022_38(List<Form17> forms, List<Form10> forms10, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "38", "64" };
        var operationCode = forms[lines[0]].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var repOKPOList = forms10
            .Select(x => x.Okpo_DB)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        foreach (var line in lines)
        {
            var statusRao = (forms[line].StatusRAO_DB ?? string.Empty).Trim();
            if (statusRao is "" or "-") continue;
            var valid = repOKPOList.Contains(statusRao);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
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

    private static List<CheckError> Check_022_42(List<Form17> forms, List<Form10> forms10, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "42", "43", "73", "97", "98" };
        var operationCode = forms[lines[0]].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var repOKPOList = forms10
            .Select(x => x.Okpo_DB)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        foreach (var line in lines)
        {
            var statusRao = (forms[line].StatusRAO_DB ?? string.Empty).Trim();
            if (statusRao is "" or "-") continue;
            var valid = repOKPOList.Contains(statusRao);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
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

    private static List<CheckError> Check_022_22(List<Form17> forms, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "22", "32" };
        var applicableRaoStatuses = new [] { "1" };
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var operationCode = forms[lines[0]].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        foreach (var line in lines)
        {
            var statusRaoDB = forms[line].StatusRAO_DB;
            if (string.IsNullOrWhiteSpace(statusRaoDB) || statusRaoDB.Trim() == "-") continue;
            var valid = okpoRegex.IsMatch(statusRaoDB) || applicableRaoStatuses.Contains(statusRaoDB);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "StatusRAO_DB",
                    Value = statusRaoDB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте правильность статуса РАО."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check022_16
    private static List<CheckError> Check_022_16(List<Form17> forms, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "16" };
        var applicableRaoStatuses = new [] { "2" };
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var operationCode = forms[lines[0]].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        foreach (var line in lines)
        {
            var StatusRAO_DB = forms[line].StatusRAO_DB;
            if (string.IsNullOrWhiteSpace(StatusRAO_DB) || StatusRAO_DB.Trim() == "-") continue;
            var valid = okpoRegex.IsMatch(StatusRAO_DB) || applicableRaoStatuses.Contains(StatusRAO_DB);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "StatusRAO_DB",
                    Value = StatusRAO_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте правильность статуса РАО."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check022_10
    private static List<CheckError> Check_022_10(List<Form17> forms, List<int> lines)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new []
        {
            "10", "18", "21", "25", "27", "29", "31", "35", "36", "37", "39", "43", "44", "45", "51", "52", "55", "68", "71"
        };
        var applicableRaoStatuses = new [] { "1", "2", "3", "4", "6", "9" };
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var operationCode = forms[lines[0]].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        foreach (var line in lines)
        {
            var statusRaoDB = forms[line].StatusRAO_DB;
            if (string.IsNullOrWhiteSpace(statusRaoDB) || statusRaoDB.Trim() == "-") continue;
            var valid = okpoRegex.IsMatch(statusRaoDB) || applicableRaoStatuses.Contains(statusRaoDB);
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "StatusRAO_DB",
                    Value = statusRaoDB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Заполнение графы 5 не соответствует требованиям приказа Госкорпорации \"Росатом\" от 07.12.2020 № 1/13-НПА."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check023

    private static List<CheckError> Check_023(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var volumeOutOfPackDB = forms[line].VolumeOutOfPack_DB;
        var valid = TryParseFloatExtended(volumeOutOfPackDB, out var volumeOutOfPackDBReal) && volumeOutOfPackDBReal > 0;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "VolumeOutOfPack_DB",
                Value = forms[line].VolumeOutOfPack_DB,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Необходимо заполнить сведения об объеме РАО."
            });
        }
        return result;
    }

    #endregion

    #region Check024

    private static List<CheckError> Check_024(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var volumeOutOfPackDB = forms[line].VolumeOutOfPack_DB;
        var massOutOfPackDB = forms[line].MassOutOfPack_DB;
        var volumeOutOfPackDBExists = TryParseFloatExtended(volumeOutOfPackDB, out var volumeOutOfPackDBReal);
        var massOutOfPackDBExists = TryParseFloatExtended(massOutOfPackDB, out var massOutOfPackDBReal);
        var valid = volumeOutOfPackDBExists && massOutOfPackDBExists;
        if (!valid || massOutOfPackDBReal == 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "VolumeOutOfPack_DB",
                Value = forms[line].VolumeOutOfPack_DB,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Необходимо заполнить сведения об объеме РАО."
            });
        }
        else
        {
            if (massOutOfPackDBReal < 0.005f * volumeOutOfPackDBReal)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "VolumeOutOfPack_DB",
                    Value = forms[line].VolumeOutOfPack_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте значение массы и объема. Расчетное значение плотности слишком маленькое."
                });
            }
            else if (massOutOfPackDBReal > 21f * volumeOutOfPackDBReal)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "VolumeOutOfPack_DB",
                    Value = forms[line].VolumeOutOfPack_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте значение массы и объема. Расчетное значение плотности слишком большое."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check025

    private static List<CheckError> Check_025(List<Form17> forms, List<int> lines)
    {
        List<CheckError> result = new();
        var kodRaoRegex = new Regex(@"^\d{11}?$");
        List<string> applicableRao910 = new() { "81", "82", "84", "85", "86", "87", "88", "89" };
        foreach (var line in lines)
        {
            var kodRao = forms[line].CodeRAO_DB.Trim();
            if (!kodRaoRegex.IsMatch(kodRao)) continue;
            if (applicableRao910.Contains(kodRao.Substring(8, 2)))
            {
                if (!TryParseFloatExtended(forms[line].Quantity_DB, out _))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_17",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "Quantity_DB",
                        Value = forms[line].Quantity_DB,
                        Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                  "Необходимо заполнить сведения о количестве ОЗИИИ."
                    });
                }
            }
            else if (forms[line].Quantity_DB.Trim() != "-")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_17",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Quantity_DB",
                    Value = forms[line].Quantity_DB,
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Графа заполняется только для ОЗИИИ, в остальных случаях используется символ «-» без кавычек."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check026_029

    private static List<CheckError> Check_026_029(List<Form17> forms, List<int> lines)
    {
        List<CheckError> result = new();
        List<string> errorColumns = new();
        List<string> nuclids = new();
        foreach (var nuclid in lines
                     .Where(line => R
                         .Any(phEntry => phEntry["name"] == forms[line].Radionuclids_DB.ToLower().Trim()))
                     .Select(line => R
                         .First(phEntry => phEntry["name"] == forms[line].Radionuclids_DB.ToLower().Trim())))
        {
            if (!nuclids.Contains(nuclid["name"])) nuclids.Add(nuclid["name"]);
            switch (nuclid["code"])
            {
                case "а":
                    if (!TryParseFloatExtended(forms[lines[0]].AlphaActivity_DB, out _))
                    {
                        if (!errorColumns.Contains("28")) errorColumns.Add("28");
                    }
                    break;
                case "б":
                    if (!TryParseFloatExtended(forms[lines[0]].BetaGammaActivity_DB, out _))
                    {
                        if (!errorColumns.Contains("27")) errorColumns.Add("27");
                    }
                    break;
                case "т":
                    if (!TryParseFloatExtended(forms[lines[0]].TritiumActivity_DB, out _))
                    {
                        if (!errorColumns.Contains("26")) errorColumns.Add("26");
                    }
                    break;
                case "у":
                    if (!TryParseFloatExtended(forms[lines[0]].TransuraniumActivity_DB, out _))
                    {
                        if (!errorColumns.Contains("29")) errorColumns.Add("29");
                    }
                    break;
            }
        }
        if (errorColumns.Count > 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[lines[0]].NumberInOrder_DB.ToString(),
                Column = "Radionuclids_DB",
                Value = string.Join("; ", nuclids),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Для указанного в графе 12 состава радионуклидов должна быть приведена активность " +
                          $"в граф{(errorColumns.Count>1?"ах":"е")} {string.Join(", ",errorColumns)}."
            });
        }
        return result;
    }

    #endregion

    #region Check030_55

    private static List<CheckError> Check_030_55(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        if (forms[line].OperationCode_DB != "55") return result;
        List<string> applicableRefineOrSortRaoCode = new() {
            "11","12","13","14","15","16","17",     "19",
            "21","22","23","24",                    "29",
            "31","32",                              "39",
            "41","42","43",                         "49",
            "51","52","53","54","55",
            "61","62","63",
            "71","72","73","74",                    "79",
            "99","-"
        };
        if (!applicableRefineOrSortRaoCode.Contains(forms[line].RefineOrSortRAOCode_DB.Trim()))
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = forms[line].RefineOrSortRAOCode_DB,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Заполните сведения о коде переработки/сортировки. В случае, если при кондиционировании ничего не использовалось, " +
                          "укажите символ «-» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check030_10

    private static List<CheckError> Check_030_10(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        List<string> applicableOperationCodes = new() 
        {
            "10","11","12","13","14","16","18",
            "21","22","25","26","27","28","29",
            "31","32","35","36","37","38","39",
            "43","51","52","63","64","68","97","98"
        };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        if (forms[line].RefineOrSortRAOCode_DB.Trim() != "-")
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = forms[line].RefineOrSortRAOCode_DB,
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Для выбранного кода операции в графе 30 следует использовать символ «-» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check031

    private static List<CheckError> Check_031(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var subsidy = (forms[line].Subsidy_DB ?? string.Empty).Trim();
        if (subsidy is "" or "-") return result;
        var valid = float.TryParse(subsidy, out var valueReal)
                    && valueReal is >= 0 and <= 100;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Subsidy_DB",
                Value = Convert.ToString(subsidy),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Проверьте значение субсидии."
            });
        }
        return result;
    }

    #endregion

    #region Check032

    private static List<CheckError> Check_032(List<Form17> forms, int line)
    {
        List<CheckError> result = new();
        var fcpNum = (forms[line].FcpNumber_DB ?? string.Empty).Trim();
        var valid = fcpNum is "" or "-" || TryParseFloatExtended(fcpNum, out _);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "FcpNumber_DB",
                Value = Convert.ToString(fcpNum),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Графу 32 следует либо не заполнять, либо указать числовое значение или прочерк."
            });
        }
        return result;
    }

    #endregion

    #region Check033

    //Наличие строк дубликатов
    private static List<CheckError> Check_033(List<Form17> forms)
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
                                && comparator.Compare(formToCompare.PackName_DB, currentForm.PackName_DB) == 0
                                && comparator.Compare(formToCompare.PackType_DB, currentForm.PackType_DB) == 0
                                && comparator.Compare(formToCompare.PackFactoryNumber_DB, currentForm.PackFactoryNumber_DB) == 0
                                && comparator.Compare(formToCompare.PackNumber_DB, currentForm.PackNumber_DB) == 0
                                && comparator.Compare(formToCompare.FormingDate_DB, currentForm.FormingDate_DB) == 0
                                && comparator.Compare(formToCompare.PassportNumber_DB, currentForm.PassportNumber_DB) == 0
                                && comparator.Compare(formToCompare.Volume_DB, currentForm.Volume_DB) == 0
                                && comparator.Compare(formToCompare.Mass_DB, currentForm.Mass_DB) == 0
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
                                && comparator.Compare(formToCompare.VolumeOutOfPack_DB, currentForm.VolumeOutOfPack_DB) == 0
                                && comparator.Compare(formToCompare.MassOutOfPack_DB, currentForm.MassOutOfPack_DB) == 0
                                && comparator.Compare(formToCompare.Quantity_DB, currentForm.Quantity_DB) == 0
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
                FormNum = "form_17",
                Row = duplicateLines,
                Column = "2 - 18",
                Value = "",
                Message = $"Данные граф 2-18 в строках {duplicateLines} продублированы. Следует проверить правильность предоставления данных."
            });
        }
        return result;
    }

    #endregion

    #region Check034

    //overdue calculations
    private static List<CheckError> Check_034(List<Form17> forms, Report rep)
    {
        List<CheckError> result = new();
        List<string> overdueSetLines = new();
        var endPeriodDB = rep.EndPeriod_DB;
        if (DateOnly.TryParse(endPeriodDB, out var dateEnd))
        {
            for (var i = 0; i < forms.Count; i++)
            {
                var operationCode = forms[i].OperationCode_DB;
                var operationDate = forms[i].OperationDate_DB;
                var documentDate = forms[i].DocumentDate_DB;
                if (operationCode != null 
                    && (operationCode == "10" ? documentDate : operationDate) != null 
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
                FormNum = "form_17",
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

    private static List<CheckError> Check_Criteria(List<Form17> forms, List<int> lines)
    {
        List<CheckError> result = new();
        List<string> activityString = new();
        var criteriaSum = 0.0f;
        var radArray = lines
            .Where(line => R.Any(phEntry => phEntry["name"] == forms[line].Radionuclids_DB.ToLower().Trim()))
            .Select(line => (forms[line].Radionuclids_DB.ToLower().Trim(), forms[line].SpecificActivity_DB.ToLower().Trim()))
            .ToList();
        foreach (var nuclid in radArray)
        {
            var nuclidData = R.First(phEntry => phEntry["name"] == nuclid.Item1);
            if (TryParseFloatExtended(nuclid.Item2, out var activityUp) 
                && TryParseFloatExtended(nuclidData["A_Solid"], out var activityDown) 
                && activityDown != 0.0f)
            {
                criteriaSum += activityUp / activityDown;
                activityString.Add(nuclid.Item2);
            }
        }
        if (criteriaSum < 1)
        {
            result.Add(new CheckError
            {
                FormNum = "form_17",
                Row = forms[lines[0]].NumberInOrder_DB.ToString(),
                Column = "SpecificActivity_DB",
                Value = string.Join(", ",activityString),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Отходы ниже уровня отнесения к РАО."
            });
        }
        return result;
    }

    #endregion

    #endregion
}