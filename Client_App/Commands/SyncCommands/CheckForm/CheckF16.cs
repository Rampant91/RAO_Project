using Models.CheckForm;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Client_App.Commands.SyncCommands.CheckForm;

public abstract class CheckF16 : CheckBase
{
    #region Properties

    private static readonly string[] OperationCode_DB_Valids =
    {
        "10","11","12","13","14","16","18",
        "21","22","25","26","27","28","29",
        "31","32","35","36","37","38","39","41",
        "42","43","44","45","48","49","51",
        "52","56","57","59","63","64","68",
        "71","72","73","74","75","76","97",
        "98","99"
    };

    private static readonly Dictionary<string, string> GraphsList = new()
    {
        { "NumberInOrder_DB", "01 - № п/п" },
        { "OperationCode_DB", "02 - Код операции" },
        { "OperationDate_DB", "03 - Дата операции" },
        { "CodeRAO_DB", "04 - Код РАО" },
        { "StatusRAO_DB", "05 - Статус РАО" },
        { "Volume_DB", "06 - Объем без упаковки, куб. м" },
        { "Mass_DB", "07 - масса без упаковки (нетто), т" },
        { "QuantityOZIII_DB", "08 - Количество ОЗИИИ, шт." },
        { "MainRadionuclids_DB", "09 - Основные радионуклиды" },
        { "TritiumActivity_DB", "10 - Суммарная активность, Бк: тритий" },
        { "BetaGammaActivity_DB", "11 - Суммарная активность, Бк: бета-, гамма-излучающие радионуклиды" },
        { "AlphaActivity_DB", "12 - Суммарная активность, Бк: альфа-излучающие радионуклиды" },
        { "TransuraniumActivity_DB", "13 - Суммарная активность, Бк: трансурановые радионуклиды" },
        { "ActivityMeasurementDate_DB", "14 - Дата измерения активности" },
        { "DocumentVid_DB", "15 - Вид документа" },
        { "DocumentNumber_DB", "16 - Номер документа" },
        { "DocumentDate_DB", "17 - Дата документа" },
        { "ProviderOrRecieverOKPO_DB", "18 - Код ОКПО поставщика или получателя" },
        { "TransporterOKPO_DB", "19 - Код ОКПО перевозчика" },
        { "StoragePlaceName_DB", "20 - Наименование пункта хранения" },
        { "StoragePlaceCode_DB", "21 - Код пункта хранения" },
        { "RefineOrSortRAOCode_DB", "22 - Код переработки/сортировки РАО" },
        { "PackName_DB", "23 - Наименование УКТ, упаковки или иной ученой единицы" },
        { "PackType_DB", "24 - Тип УКТ, упаковки или иной ученой единицы" },
        { "PackNumber_DB", "25 - Заводской номер УКТ, упаковки или иной ученой единицы" },
        { "Subsidy_DB", "26 - Субсидия, %" },
        { "FcpNumber_DB", "27 - Номер мероприятия ФЦП" },
    };

    #endregion

    #region CheckTotal

    public static List<CheckError> Check_Total(Reports reps, Report rep)
    {
        var currentFormLine = 0;
        List<CheckError> errorList = new();
        LoadDictionaries();
        var formsList = rep.Rows16.ToList<Form16>();

        errorList.AddRange(Check_001(formsList));
        foreach (var key in rep.Rows16)
        {
            var form = (Form16)key;
            var notes = rep.Notes.ToList<Note>();
            var forms10 = reps.Master_DB.Rows10.ToList<Form10>();
            errorList.AddRange(Check_002(formsList, currentFormLine));
            errorList.AddRange(Check_003(formsList, currentFormLine));
            errorList.AddRange(Check_003_11(formsList, currentFormLine));
            errorList.AddRange(Check_003_12(formsList, currentFormLine));
            errorList.AddRange(Check_003_29(formsList, notes, currentFormLine));
            errorList.AddRange(Check_003_51(formsList, currentFormLine));
            errorList.AddRange(Check_003_52(formsList, currentFormLine));
            errorList.AddRange(Check_003_57(formsList, currentFormLine));
            errorList.AddRange(Check_003_10(formsList, currentFormLine));
            errorList.AddRange(Check_003_49(formsList, currentFormLine));
            errorList.AddRange(Check_003_71(formsList, currentFormLine));
            errorList.AddRange(Check_004_non10(formsList, rep, currentFormLine));
            errorList.AddRange(Check_004_10(formsList, rep, currentFormLine));
            errorList.AddRange(Check_005(formsList, notes, currentFormLine));
            errorList.AddRange(Check_006_11(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_006_26(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_006_38(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_006_42(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_006_22(formsList, currentFormLine));
            errorList.AddRange(Check_006_16(formsList, currentFormLine));
            errorList.AddRange(Check_006_76(formsList, currentFormLine));
            errorList.AddRange(Check_006_Other(formsList, currentFormLine));
            errorList.AddRange(Check_007(formsList, currentFormLine));
            errorList.AddRange(Check_008(formsList, currentFormLine));
            errorList.AddRange(Check_009(formsList, currentFormLine));
            errorList.AddRange(Check_010(formsList, currentFormLine));
            errorList.AddRange(Check_011(formsList, currentFormLine));
            errorList.AddRange(Check_012(formsList, currentFormLine));
            errorList.AddRange(Check_013(formsList, currentFormLine));
            errorList.AddRange(Check_014(formsList, currentFormLine));
            errorList.AddRange(Check_015(formsList, currentFormLine));
            errorList.AddRange(Check_016(formsList, notes, currentFormLine));
            errorList.AddRange(Check_017(formsList, currentFormLine));
            errorList.AddRange(Check_018(formsList, rep, currentFormLine));
            errorList.AddRange(Check_019_10(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_019_21(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_019_22(formsList, currentFormLine));
            errorList.AddRange(Check_020_01(formsList, currentFormLine));
            errorList.AddRange(Check_020_21(formsList, notes, currentFormLine));
            errorList.AddRange(Check_020_22(formsList, notes, currentFormLine));
            errorList.AddRange(Check_020_29(formsList, notes, currentFormLine));
            errorList.AddRange(Check_021(formsList, currentFormLine));
            errorList.AddRange(Check_022(formsList, currentFormLine));
            errorList.AddRange(Check_023_44(formsList, currentFormLine));
            errorList.AddRange(Check_023_45(formsList, currentFormLine));
            errorList.AddRange(Check_023_49(formsList, currentFormLine));
            errorList.AddRange(Check_023_10(formsList, currentFormLine));
            errorList.AddRange(Check_023_22_44(formsList, currentFormLine));
            errorList.AddRange(Check_023_22_56(formsList, currentFormLine));
            errorList.AddRange(Check_024(formsList, currentFormLine));
            errorList.AddRange(Check_025(formsList, currentFormLine));
            errorList.AddRange(Check_026(formsList, currentFormLine));
            errorList.AddRange(Check_027(formsList, currentFormLine));
            errorList.AddRange(Check_028(formsList, currentFormLine));
            currentFormLine++;
        }
        errorList.AddRange(Check_029(formsList, rep));
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

    //Наличие строк дубликатов
    private static List<CheckError> Check_001(List<Form16> forms)
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
                                  && comparator.Compare(formToCompare.CodeRAO_DB, currentForm.CodeRAO_DB) == 0
                                  && comparator.Compare(formToCompare.StatusRAO_DB, currentForm.StatusRAO_DB) == 0
                                  && comparator.Compare(formToCompare.Volume_DB, currentForm.Volume_DB) == 0
                                  && comparator.Compare(formToCompare.Mass_DB, currentForm.Mass_DB) == 0
                                  && formToCompare.QuantityOZIII_DB == currentForm.QuantityOZIII_DB
                                  && comparator.Compare(formToCompare.MainRadionuclids_DB, currentForm.MainRadionuclids_DB) == 0
                                  && comparator.Compare(formToCompare.TritiumActivity_DB, currentForm.TritiumActivity_DB) == 0
                                  && comparator.Compare(formToCompare.BetaGammaActivity_DB, currentForm.BetaGammaActivity_DB) == 0
                                  && comparator.Compare(formToCompare.AlphaActivity_DB, currentForm.AlphaActivity_DB) == 0
                                  && comparator.Compare(formToCompare.TransuraniumActivity_DB, currentForm.TransuraniumActivity_DB) == 0
                                  && comparator.Compare(formToCompare.ActivityMeasurementDate_DB, currentForm.ActivityMeasurementDate_DB) == 0
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
                FormNum = "form_16",
                Row = group,
                Column = "2 - 18",
                Value = "",
                Message = $"Данные граф 2-18 в строках {group} продублированы. Следует проверить правильность предоставления данных."
            });
        }
        return result;
    }

    #endregion

    #region Check002

    private static List<CheckError> Check_002(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var valid = line == 0 && forms[line].NumberInOrder_DB is 1 or 0 
                    || line > 0 && forms[line - 1].NumberInOrder_DB == forms[line].NumberInOrder_DB - 1;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "NumberInOrder_DB",
                Value = forms[line].NumberInOrder_DB.ToString(),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Номера строк должны располагаться по порядку, без пропусков или дублирования номеров"
            });
        }
        return result;
    }

    #endregion

    #region Check003

    private static List<CheckError> Check_003(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var valid = OperationCode_DB_Valids.Contains(operationCode);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Код операции не может быть использован в форме 1.6."
            });
        }
        return result;
    }

    #endregion

    #region Check003_11

    private static List<CheckError> Check_003_11(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = (forms[line].OperationCode_DB ?? string.Empty).Trim();
        var applicableOperationCodes = new[] { "11", "12", "13", "14", "16", "18", "41", "56", "57", "59" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        if (forms[line].CodeRAO_DB.Length < 10) return result;
        var valid = forms[line].CodeRAO_DB.Substring(8, 2) != "99";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Для вновь образованных РАО код типа РАО «99» не может быть использован."
            });
        }
        return result;
    }

    #endregion

    #region Check003_12

    private static List<CheckError> Check_003_12(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new [] { "12,42" };
        var requiredNuclids = new []
        {
            "плутоний", "уран-233", "уран-235", "уран-238", "нептуний-237", "америций-241", 
            "америций-243", "калифорний-252", "торий", "литий-6", "тритий"
        };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var nuclids = forms[line].MainRadionuclids_DB.Split(';');
        var valid = false;
        for (var i = 0; i < nuclids.Length; i++)
        {
            nuclids[i] = nuclids[i].Trim().ToLower();
            if (requiredNuclids.Contains(nuclids[i]))
            {
                valid = true;
                break;
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "MainRadionuclids_DB",
                Value = forms[line].MainRadionuclids_DB,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "В графе 9 не представлены сведения о радионуклидах, которые могут быть отнесены к ЯМ. " +
                          "Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check003_29

    private static List<CheckError> Check_003_29(List<Form16> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new [] { "29", "39", "49", "59", "97", "98", "99" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        const byte graphNumber = 2;
        var valid = CheckNotePresence(notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
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

    #region Check003_51

    //TODO
    private static List<CheckError> Check_003_51(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new [] { "51" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + ""
            });
        }
        return result;
    }

    #endregion

    #region Check003_52

    //TODO
    private static List<CheckError> Check_003_52(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new [] { "52" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "В отчетах не найдена строка об осуществлении передачи учетной единицы. " +
                          "Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check003_57

    //TODO
    private static List<CheckError> Check_003_57(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new [] { "57" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "В отчетах не найдена строка снятии учетной единицы для упаковки/переупаковки. " +
                          "Проверьте правильность выбранного кода операции."
            });
        }
        return result;
    }

    #endregion

    #region Check003_10

    private static List<CheckError> Check_003_10(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new []
        {
            "10", "11", "12", "13", "14", "16", "18", "31", "32", "35", "36", "37", "38", "41", "56", "57", "59", "97"
        };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var mass = forms[line].Mass_DB;
        var mass_exists = TryParseFloatExtended(mass, out var mass_real);
        if (!mass_exists || mass_real == 0) return result;
        var activityA = forms[line].AlphaActivity_DB;
        var activityB = forms[line].BetaGammaActivity_DB;
        var activityT = forms[line].TritiumActivity_DB;
        var activityU = forms[line].TransuraniumActivity_DB;
        var activityExistsA = TryParseFloatExtended(activityA, out var activityRealA);
        var activityExistsB = TryParseFloatExtended(activityB, out var activityRealB);
        var activityExistsT = TryParseFloatExtended(activityT, out var activityRealT);
        var activityExistsU = TryParseFloatExtended(activityU, out var activityRealU);
        var codeRao1MatterState = forms[line].CodeRAO_DB[..1];
        var rColumn = "";
        var comparator = new CustomNullStringWithTrimComparer();
        rColumn = codeRao1MatterState switch
        {
            "1" => "A_Liquid",
            "2" => "A_Solid",
            _ => rColumn
        };
        var valid = true;
        var success = false;
        if (comparator.Compare(string.Empty, rColumn) != 0)
        {
            var nuclids = forms[line].MainRadionuclids_DB.Split(';');
            for (var i = 0; i < nuclids.Length; i++)
            {
                nuclids[i] = nuclids[i].Trim().ToLower();
            }
            var nuclidsA = new Dictionary<string, Dictionary<string, string>>();
            var nuclidsB = new Dictionary<string, Dictionary<string, string>>();
            var nuclidsT = new Dictionary<string, Dictionary<string, string>>();
            var nuclidsU = new Dictionary<string, Dictionary<string, string>>();

            foreach (var nuclid in nuclids)
            {
                if (R.Any(x => comparator.Compare(x["name"], nuclid) == 0))
                {
                    var rEntry = R.First(x => comparator.Compare(x["name"], nuclid) == 0);
                    switch (rEntry["code"])
                    {
                        case "т":
                            nuclidsT[rEntry["name"]] = rEntry;
                            break;
                        case "а":
                            nuclidsA[rEntry["name"]] = rEntry;
                            break;
                        case "б":
                            nuclidsB[rEntry["name"]] = rEntry;
                            break;
                        case "у":
                            nuclidsU[rEntry["name"]] = rEntry;
                            break;
                    }
                }
            }
            var nuclidMinNameA = ""; var nuclidMinValueA = float.MaxValue;
            var nuclidMinNameB = ""; var nuclidMinValueB = float.MaxValue;
            var nuclidMinNameU = ""; var nuclidMinValueU = float.MaxValue;
            var nuclidMinNameT = ""; var nuclidMinValueT = float.MaxValue;
            foreach (var entry in nuclidsA)
            {
                if (!TryParseFloatExtended(entry.Value[rColumn], out var val)) return result;
                if (val != 0 && val < nuclidMinValueA)
                {
                    nuclidMinValueA = val;
                    nuclidMinNameA = entry.Value["name"];
                }
            }
            foreach (var entry in nuclidsB)
            {
                if (!TryParseFloatExtended(entry.Value[rColumn], out var val)) return result;
                if (val != 0 && val < nuclidMinValueB)
                {
                    nuclidMinValueB = val;
                    nuclidMinNameB = entry.Value["name"];
                }
            }
            foreach (var entry in nuclidsT)
            {
                if (!TryParseFloatExtended(entry.Value[rColumn], out var val)) return result;
                if (val != 0 && val < nuclidMinValueT)
                {
                    nuclidMinValueT = val;
                    nuclidMinNameT = entry.Value["name"];
                }
            }
            foreach (var entry in nuclidsU)
            {
                if (!TryParseFloatExtended(entry.Value[rColumn], out var val)) return result;
                if (val != 0 && val < nuclidMinValueU)
                {
                    nuclidMinValueU = val;
                    nuclidMinNameU = entry.Value["name"];
                }
            }
            var a = 0.0f;
            if (nuclidMinNameB != "" && activityExistsB) { a += activityRealB / (mass_real * 1e6f) / nuclidMinValueB; success = true; }
            if (nuclidMinNameA != "" && activityExistsA) { a += activityRealA / (mass_real * 1e6f) / nuclidMinValueA; success = true; }
            if (nuclidMinNameU != "" && activityExistsU) { a += activityRealU / (mass_real * 1e6f) / nuclidMinValueU; success = true; }
            if (nuclidMinNameT != "" && activityExistsT) { a += activityRealT / (mass_real * 1e6f) / nuclidMinValueT; success = true; }
            valid = a > 1.0f;
        }
        if (!success)
        {
            if (codeRao1MatterState is "1" or "2" && (activityExistsA || activityExistsB))
            {
                valid = codeRao1MatterState switch
                {
                    "1" => (activityExistsA && activityRealA / (mass_real * 1e6) >= 0.05f) 
                           || (activityExistsB && activityRealB / (mass_real * 1e6) >= 0.5f),
                    "2" => (activityExistsA && activityRealA / (mass_real * 1e6) >= 1.0f) 
                           || (activityExistsB && activityRealB / (mass_real * 1e6) >= 100.0f),
                    _ => valid
                };
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "По сведениям, представленным в строке, отходы не относятся к РАО."
            });
        }
        return result;
    }

    #endregion

    #region Check003_49

    private static List<CheckError> Check_003_49(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new [] { "49", "59" };
        var graph22ValidValues = new [] { "-", "52", "72", "74" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = graph22ValidValues.Contains(forms[line].RefineOrSortRAOCode_DB.Trim());
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Код операции не соответствует коду переработки/сортировки, указанному в графе 22."
            });
        }
        return result;
    }

    #endregion

    #region Check003_71

    //TODO
    //Справочная "ошибка" - т.е. даже не ошибка.
    private static List<CheckError> Check_003_71(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var applicableOperationCodes = new [] { "71", "72", "73", "74", "75", "76", "18", "68" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = true;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "К отчету необходимо приложить скан-копию документа характеризующего операцию."
            });
        }
        return result;
    }

    #endregion

    #region Check004_non10

    private static List<CheckError> Check_004_non10(List<Form16> forms, Report rep, int line)
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
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "OperationDate_DB",
                Value = Convert.ToString(operationDate),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Дата операции не входит в отчетный период."
            });
        }
        return result;
    }

    #endregion

    #region Check004_10

    private static List<CheckError> Check_004_10(List<Form16> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "10" };
        var operationCode = forms[line].OperationCode_DB;
        var documentDate = forms[line].DocumentDate_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = documentDate != null;
        if (valid && rep is { StartPeriod_DB: not null, EndPeriod_DB: not null })
        {
            valid = DateTime.TryParse(rep.StartPeriod_DB, out var pStart)
                    && DateTime.TryParse(rep.EndPeriod_DB, out var pEnd)
                    && DateTime.TryParse(documentDate, out var pMid)
                    && pMid >= pStart && pMid <= pEnd;
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
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

    #region Check005

    private static List<CheckError> Check_005(List<Form16> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        var comparator = new CustomNullStringWithTrimComparer();
        var codeRaoDB = forms[line].CodeRAO_DB.Trim();
        var valid = !string.IsNullOrWhiteSpace(codeRaoDB) && codeRaoDB.Length == 11 && codeRaoDB.All(char.IsDigit);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = codeRaoDB,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Проверьте правильность заполнения кода РАО."
            });
            return result;
        }

        #region data fetch

        var operationCode = forms[line].OperationCode_DB;
        var rads = forms[line].MainRadionuclids_DB;
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();
        var halflife_max = 0.0f;
        var halflife_max_id = -1;
        var validUnits = new Dictionary<string, float>
        {
            { "лет", 1f },
            { "сут", 365.242374f },
            { "час", 365.242374f*24.0f },
            { "мин", 365.242374f*24.0f*60.0f },
            { "сек", 365.242374f*24.0f*60.0f*60.0f }
        };
        var nuclidActivityT = forms[line].TritiumActivity_DB;
        var nuclidActivityA = forms[line].AlphaActivity_DB;
        var nuclidActivityB = forms[line].BetaGammaActivity_DB;
        var nuclidActivityU = forms[line].TransuraniumActivity_DB;
        var nuclidMass = forms[line].Mass_DB;
        var nuclidsExistT = TryParseFloatExtended(nuclidActivityT, out var nuclidActivityRealT);
        var nuclidsExistA = TryParseFloatExtended(nuclidActivityA, out var nuclidActivityRealA);
        var nuclidsExistB = TryParseFloatExtended(nuclidActivityB, out var nuclidActivityRealB);
        var nuclidsExistU = TryParseFloatExtended(nuclidActivityU, out var nuclidActivityRealU);
        var nuclidMassExists = TryParseFloatExtended(nuclidMass, out var nuclidMassReal);
        foreach (var nuclid in radsSet)
        {
            var nuclidId = R.FindIndex(x => comparator.Compare(x["name"], nuclid) == 0);
            if (nuclidId >= 0 
                && TryParseFloatExtended(R[nuclidId]["value"], out var halfLifeVal) 
                && validUnits.TryGetValue(R[nuclidId]["unit"], out var value))
            {
                halflife_max = Math.Max(halfLifeVal / value, halflife_max);
                halflife_max_id = nuclidId;
            }
        }
        const byte graphNumber = 4;
        var noteExists = CheckNotePresence(notes, line, graphNumber);

        #endregion

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
        var codeRao1Allowed = new [] { "1", "2", "3" };
        var codeRao2Allowed = new [] { "0", "1", "2", "3", "4", "9" };
        var codeRao3Allowed = new [] { "1", "2", "3", "4", "5", "6" };
        var codeRao4Allowed = new [] { "1", "2" };
        var codeRao5Allowed = new [] { "1", "2" };
        var codeRao6Allowed = new [] { "0", "1", "2", "3" };
        var codeRao7Allowed = new [] { "0", "1", "2", "3", "4", "9" };
        var codeRao8Allowed = new [] { "0", "1", "2", "3", "4", "5", "6", "7", "9" };
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

        var RecyclingTypes = new [] 
        {
            "11","12","13","14","15","16","17","18","19","20","21","22","23","24","25",
            "26","27","28","29","30","31","32","33","34","35","36","37","38","39"
        };

        #endregion

        #region symbol 1

        if (!codeRao1Valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = $"{codeRao1MatterState} (1-ый символ кода РАО), {codeRao910TypeCode} (9-10 символы кода РАО)",
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Недопустимое значение 1-го символа кода РАО."
            });
        }
        else
        {
            var validTypeCodeLiquid = new List<string>();
            var validTypeCodeSolid = new List<string>();
            var validTypeCodeSolid7Not0 = new List<string>();
            var validTypeCodeGaseous = new List<string>();
            //liquids: symbols 9-10 are 11-39, 99.
            for (var i = 11; i <= 39; i++)
            {
                validTypeCodeLiquid.Add(i.ToString("D2"));
            }
            validTypeCodeLiquid.Add(99.ToString("D2"));
            //solids: symbols 9-10 are 41-99; 11-39 if symbol 7 is not 0.
            for (var i = 41; i <= 99; i++)
            {
                validTypeCodeSolid.Add(i.ToString("D2"));
            }
            for (var i = 11; i <= 39; i++)
            {
                validTypeCodeSolid7Not0.Add(i.ToString("D2"));
            }
            //gases: symbols 9-10 are strictly 01.
            for (var i = 1; i <= 1; i++)
            {
                validTypeCodeGaseous.Add(i.ToString("D2"));
            }
            switch (codeRao1MatterState)
            {
                case "1":
                    if (!validTypeCodeLiquid.Contains(codeRao910TypeCode))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_16",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao1MatterState} (1-ый символ кода РАО), {codeRao910TypeCode} (9-10 символы кода РАО)",
                            Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                      "Агрегатное состояние (символ №1) не соответствует типу выбранных РАО (символы №9-10)."
                        });
                    }
                    break;
                case "2":
                    if (!(validTypeCodeSolid.Contains(codeRao910TypeCode) 
                          || (validTypeCodeSolid7Not0.Contains(codeRao910TypeCode) 
                              && codeRao7RecycleMethod != "0")))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_16",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao1MatterState} (1-ый символ кода РАО), {codeRao910TypeCode} (9-10 символы кода РАО)",
                            Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                      "Агрегатное состояние (символ №1) не соответствует типу выбранных РАО (символы №9-10)."
                        });
                    }
                    break;
                case "3":
                    if (!validTypeCodeGaseous.Contains(codeRao910TypeCode))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_16",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao1MatterState} (1-ый символ кода РАО), {codeRao910TypeCode} (9-10 символы кода РАО)",
                            Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                      "Агрегатное состояние (символ №1) не соответствует типу выбранных РАО (символы №9-10)."
                        });
                    }
                    break;
            }
        }
        #endregion

        #region symbol 2

        if (!codeRao2Valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Недопустимое значение 2-го символа кода РАО."
            });
        }
        else
        {
            switch (codeRao2RaoCategory)
            {
                case "4":
                    var validTypeCode = new [] { "81", "82", "85", "86", "87", "88", "89" };
                    if (!validTypeCode.Contains(codeRao910TypeCode))
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_16",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                            Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                      "Значение 2-го символа кода РАО 4 используется только для отработавших ЗРИ."
                        });
                    }
                    break;
                case "9":
                    if (!noteExists)
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_16",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                            Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                      "Необходимо дать пояснение для 2-го символа кода РАО."
                        });
                    }
                    break;
                default:
                    // 0, 1, 2, 3, 9
                    if (codeRao2RaoCategory == "0" && codeRao1MatterState == "1")
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_16",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                            Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                      "Неправильно указана категория РАО."
                        });
                    }
                    else
                    {
                        var codeMax = -1;
                        if (nuclidMassExists && nuclidMassReal > 0)
                        {
                            #region A for tritium

                            if (nuclidsExistT)
                            {
                                var A = nuclidActivityRealT / (nuclidMassReal * 1e6);
                                if (codeRao1MatterState == "2")
                                {
                                    if (A < 1e07) { if (codeMax < 0) codeMax = 0; }
                                    else if (A < 1e08) { if (codeMax < 1) codeMax = 1; }
                                    else if (A < 1e11) { if (codeMax < 2) codeMax = 2; }
                                    else { if (codeMax < 3) codeMax = 3; }
                                }
                                else if (codeRao1MatterState == "1")
                                {
                                    if (A < 1e04) { if (codeMax < 1) codeMax = 1; }
                                    else if (A < 1e08) { if (codeMax < 2) codeMax = 2; }
                                    else { if (codeMax < 3) codeMax = 3; }
                                }
                            }

                            #endregion

                            #region A for beta-gamma

                            if (nuclidsExistB)
                            {
                                var A = nuclidActivityRealB / (nuclidMassReal * 1e6);
                                if (codeRao1MatterState == "2")
                                {
                                    if (A < 1e03) { if (codeMax < 0) codeMax = 0; }
                                    else if (A < 1e04) { if (codeMax < 1) codeMax = 1; }
                                    else if (A < 1e07) { if (codeMax < 2) codeMax = 2; }
                                    else { if (codeMax < 3) codeMax = 3; }
                                }
                                else if (codeRao1MatterState == "1")
                                {
                                    if (A < 1e03) { if (codeMax < 1) codeMax = 1; }
                                    else if (A < 1e07) { if (codeMax < 2) codeMax = 2; }
                                    else { if (codeMax < 3) codeMax = 3; }
                                }
                            }

                            #endregion

                            #region A for alpha

                            if (nuclidsExistA)
                            {
                                var A = nuclidActivityRealA / (nuclidMassReal * 1e6);
                                if (codeRao1MatterState == "2")
                                {
                                    if (A < 1e02) { if (codeMax < 0) codeMax = 0; }
                                    else if (A < 1e03) { if (codeMax < 1) codeMax = 1; }
                                    else if (A < 1e06) { if (codeMax < 2) codeMax = 2; }
                                    else { if (codeMax < 3) codeMax = 3; }
                                }
                                else if (codeRao1MatterState == "1")
                                {
                                    if (A < 1e02) { if (codeMax < 1) codeMax = 1; }
                                    else if (A < 1e06) { if (codeMax < 2) codeMax = 2; }
                                    else { if (codeMax < 3) codeMax = 3; }
                                }
                            }

                            #endregion

                            #region A for transuraniums

                            if (nuclidsExistU)
                            {
                                var A = nuclidActivityRealU / (nuclidMassReal * 1e6);
                                if (codeRao1MatterState == "2")
                                {
                                    if (A < 1e01) { if (codeMax < 0) codeMax = 0; }
                                    else if (A < 1e02) { if (codeMax < 1) codeMax = 1; }
                                    else if (A < 1e05) { if (codeMax < 2) codeMax = 2; }
                                    else { if (codeMax < 3) codeMax = 3; }
                                }
                                else if (codeRao1MatterState == "1")
                                {
                                    if (A < 1e01) { if (codeMax < 1) codeMax = 1; }
                                    else if (A < 1e05) { if (codeMax < 2) codeMax = 2; }
                                    else { if (codeMax < 3) codeMax = 3; }
                                }
                            }

                            #endregion

                        }
                        if (codeMax == -1 && codeRao2RaoCategory != "9")
                        {
                            result.Add(new CheckError
                            {
                                FormNum = "form_16",
                                Row = forms[line].NumberInOrder_DB.ToString(),
                                Column = "CodeRAO_DB",
                                Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                          "Проверьте категорию РАО и суммарную активность."
                            });
                        }
                        else if (codeMax != -1 && (codeRao2RaoCategory == "9" || codeRao2RaoCategory != codeMax.ToString("D1")))
                        {
                            result.Add(new CheckError
                            {
                                FormNum = "form_16",
                                Row = forms[line].NumberInOrder_DB.ToString(),
                                Column = "CodeRAO_DB",
                                Value = $"{codeRao2RaoCategory} (2-ой символ кода РАО)",
                                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                          $"По данным, представленным в строке {forms[line].NumberInOrder_DB}, категория РАО {codeMax}."
                            });
                        }
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
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = $"{codeRao3NuclidTypes} (3-ий символ кода РАО)",
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Недопустимое значение 3-го символа кода РАО."
            });
        }
        else
        {
            if (radsSet.Count == 0)
            {
                if (codeRao3NuclidTypes != "0")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao3NuclidTypes} (3-ий символ кода РАО)",
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "При отсутствии радионуклидов в графе 9 3-й символ кода РАО должен быть равен 0."
                    });
                }
            }
            else
            {
                var containsB = nuclidsExistB && radsSet
                    .Any(x => R
                        .Any(y => comparator.Compare(y["name"], x) == 0 && comparator.Compare(y["code"], "б") == 0));
                var containsA = nuclidsExistA && radsSet
                    .Any(x => R
                        .Any(y => comparator.Compare(y["name"], x) == 0 && comparator.Compare(y["code"], "а") == 0));
                var containsU = nuclidsExistU && radsSet
                    .Any(x => R
                        .Any(y => comparator.Compare(y["name"], x) == 0 && comparator.Compare(y["code"], "у") == 0));
                var containsT = nuclidsExistT && radsSet
                    .Any(x => R
                        .Any(y => comparator.Compare(y["name"], x) == 0 && comparator.Compare(y["code"], "т") == 0));
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
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao3NuclidTypes} (3-ий символ кода РАО)",
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Третий символ кода РАО не соответствует сведениям о суммарной активности (графы 10-13) и/или радионуклидам, " +
                                  "указанным в графе 9."
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
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = $"{codeRao4HasNuclears} (4-ый символ кода РАО)",
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Недопустимое значение 4-го символа кода РАО."
            });
        }
        else
        {
            var nuclids = new []
            {
                "плутоний", "уран-233", "уран-235", "уран-238", "нептуний-237", "америций-241", 
                "америций-243", "калифорний-252", "торий", "литий-6", "тритий"
            };
            var operations11 = new[] { "11", "13", "14", "16", "41" };
            var operations12 = new [] { "12" };
            var nuclidsExist = radsSet
                .Any(x => nuclids
                    .Any(y => x.Contains(y, StringComparison.CurrentCultureIgnoreCase)));
            if (codeRao4HasNuclears == "1")
            {
                if (operations12.Contains(operationCode))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = codeRao4HasNuclears,
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "4-ый символ кода РАО не может быть равен 1 при коде операции 12."
                    });
                }
                //else if (operations11.Contains(operationCode))
                //{
                //    //anything is allowed
                //}
                //else
                //{
                //    //anything is allowed
                //}
            }
            else if (codeRao4HasNuclears == "2")
            {
                if (operations12.Contains(operationCode))
                {
                    if (!nuclidsExist)
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_16",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao4HasNuclears} (4-ый символ кода РАО)",
                            Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                      "4-ый символ кода РАО может быть равен 2 при коде операции 12 только при указании радионуклидов, " +
                                      "которые могут быть отнесены к ЯМ."
                        });
                    }
                }
                else if (operations11.Contains(operationCode))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao4HasNuclears} (4-ый символ кода РАО)",
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "4-ый символ кода РАО не может быть равен 2 при кодах операции 11, 13, 14, 16 и 41."
                    });
                }
                else
                {
                    if (!nuclidsExist)
                    {
                        result.Add(new CheckError
                        {
                            FormNum = "form_16",
                            Row = forms[line].NumberInOrder_DB.ToString(),
                            Column = "CodeRAO_DB",
                            Value = $"{codeRao4HasNuclears} (4-ый символ кода РАО)",
                            Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
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
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = $"{codeRao5HalfLife} (5-ый символ кода РАО)",
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Недопустимое значение 5-го символа кода РАО."
            });
        }
        else
        {
            if (codeRao5HalfLife != "2" && (long)halflife_max <= 31)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao5HalfLife} (5-ый символ кода РАО)",
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              $"По данным, представленным в строке {forms[line].NumberInOrder_DB}, 5-ый символ кода РАО " +
                              $"(период полураспада) должен быть равен 2."
                });
            }
            else if (codeRao5HalfLife != "1" && (long)halflife_max > 31)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao5HalfLife} (5-ый символ кода РАО)",
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              $"По данным, представленным в строке {forms[line].NumberInOrder_DB}, 5-ый символ кода РАО " +
                              $"(период полураспада) должен быть равен 1."
                });
            }
        }

        #endregion

        #region symbol 6

        if (!codeRao6Valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = $"{codeRao6DangerPeriod} (6-ой символ кода РАО)",
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Недопустимое значение 6-го символа кода РАО."
            });
        }
        if (codeRao1MatterState is "1" or "2")
        {
            if (codeRao3NuclidTypes == "0" && !(codeRao6DangerPeriod == "0" && noteExists))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao6DangerPeriod} (6-ой символ кода РАО)",
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Укажите причины невозможности определения периода потенциальной опасности."
                });
            }
            if (radsSet.Count > 0)
            {
                var softWarning = radsSet.Count > 1;
                var expectedPeriod = -1.0f;
                var expectedValue = 0;
                foreach (var nuclidData in radsSet
                             .Select(nuclid => R
                                 .FirstOrDefault(x => comparator.Compare(x["name"], nuclid) == 0, [])))
                {
                    if (nuclidData.Count > 0 
                        && TryParseFloatExtended(nuclidData["value"], out var T) 
                        && validUnits.TryGetValue(nuclidData["unit"], out var unitAdjustment))
                    {
                        var nuclidActivity = nuclidData["code"] switch
                        {
                            "а" when nuclidsExistA => nuclidActivityRealA,
                            "б" when nuclidsExistB => nuclidActivityRealB,
                            "у" when nuclidsExistU => nuclidActivityRealU,
                            "т" when nuclidsExistT => nuclidActivityRealT,
                            _ => -1.0f
                        };
                        if (nuclidActivity <= 0.0f) continue;
                        var t = -1.0f;
                        if (codeRao1MatterState == "1" && TryParseFloatExtended(nuclidData["OSPORB_Liquid"], out var a))
                        {
                            if (a == 0)
                            {
                                t = float.MaxValue;
                            }
                            else
                            {
                                t = T / unitAdjustment * (float)(Math.Log(nuclidActivity / (nuclidMassReal * 1e6) / (a * 0.1f)) / Math.Log(2));
                            }
                        }
                        else if (codeRao1MatterState == "2" && TryParseFloatExtended(nuclidData["OSPORB_Solid"], out a))
                        {
                            if (a == 0)
                            {
                                t = float.MaxValue;
                            }
                            else
                            {
                                t = T / unitAdjustment * (float)(Math.Log(nuclidActivity / (nuclidMassReal * 1e6) / a) / Math.Log(2));
                            }
                        }
                        expectedPeriod = Math.Max(t, expectedPeriod);
                    }
                }
                if (expectedPeriod > 500.0f) { expectedValue = Math.Max(expectedValue, 3); }
                else if (expectedPeriod >= 100.0f) { expectedValue = Math.Max(expectedValue, 2); }
                else if (expectedPeriod >= 0.0f) { expectedValue = Math.Max(expectedValue, 1); }
                else { expectedValue = Math.Max(expectedValue, 0); }
                if (expectedPeriod >= 0.0f)
                {
                    if (expectedValue.ToString("D1") != codeRao6DangerPeriod)
                    {
                        if (!softWarning)
                        {
                            result.Add(new CheckError
                            {
                                FormNum = "form_16",
                                Row = forms[line].NumberInOrder_DB.ToString(),
                                Column = "CodeRAO_DB",
                                Value = $"{codeRao6DangerPeriod} (6-ой символ кода РАО)",
                                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                          $"Расчетное значение периода потенциальной опасности (в годах): {expectedPeriod} " +
                                          $"(6-ой символ кода РАО {expectedValue})."
                            });
                        }
                        else
                        {
                            result.Add(new CheckError
                            {
                                FormNum = "form_16",
                                Row = forms[line].NumberInOrder_DB.ToString(),
                                Column = "CodeRAO_DB",
                                Value = $"{codeRao6DangerPeriod} (6-ой символ кода РАО)",
                                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                                          $"(Справочное сообщение, не обязательно к исправлению) Расчетное значение периода потенциальной " +
                                          $"опасности для приведенного полинуклидного состава (в годах): {expectedPeriod} " +
                                          $"(6-ой символ кода РАО предположительно {expectedValue})."
                            });
                        }
                    }
                }
            }
        }

        #endregion

        #region symbol 7

        if (!codeRao7Valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = $"{codeRao7RecycleMethod} (7-ой символ кода РАО)",
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Недопустимое значение 7-го символа кода РАО."
            });
        }
        else
        {
            if (codeRao1MatterState == "1" && codeRao7RecycleMethod != "0")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao7RecycleMethod} (7-ой символ кода РАО)",
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Для жидких РАО 7-ой символ кода РАО не может быть равным 1, 2, 3, 4, 9."
                });
            }
            else if (codeRao1MatterState == "2" && RecyclingTypes.Contains(codeRao910TypeCode) && codeRao7RecycleMethod is "0" or "1")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao7RecycleMethod} (7-ой символ кода РАО)",
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Сочетание агрегатного состояния 2 (твердые РАО) и кодов типа РАО 11-39 (жидкие РАО) " +
                              "возможно только для кодов переработки (7-ой символ кода РАО) 2-9."
                });
            }
            else if (forms[line].OperationCode_DB == "56")
            {
                Dictionary<string, string[]> validRecycles = new() {
                    { "0", new[]
                        {
                            "11","12","13","14","15","16","17",
                            "19",
                            "21","22","23","24",
                            "29",
                            "51","52","53","54","55",
                            "61",
                            "72","73","74",
                            "99","-"
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
                if (!validRecycles[codeRao7RecycleMethod].Contains(forms[line].RefineOrSortRAOCode_DB))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao7RecycleMethod} (7-ой символ кода РАО)",
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "7-ой символ кода РАО не соответствует коду переработки/сортировки, указанному в графе 22."
                    });
                }
            }
        }

        #endregion

        #region symbol 8

        if (!codeRao8Valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = $"{codeRao8RaoClass} (8-ой символ кода РАО)",
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Недопустимое значение 8-го символа кода РАО."
            });
        }
        else
        {
            var validValues = new [] { "0", "7", "9" };
            if (!validValues.Contains(codeRao8RaoClass))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao8RaoClass} (8-ой символ кода РАО)",
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Сведения о кондиционированных отходах необходимо представлять в формах 1.7 и 1.8. " +
                              "Для некондиционированных РАО возможны только значения «0», «7», «9»."
                });
            }
            else if (codeRao8RaoClass == "7")
            {
                var storageCode1 = forms[line].StoragePlaceCode_DB[..1];
                var storageCodeValid = new [] { "3", "5", "6" };
                var operationCodes = new [] { "11", "12", "13", "14", "16", "41" };
                if (operationCodes.Contains(forms[line].OperationCode_DB) && storageCode1 == "5")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao8RaoClass} (8-ой символ кода РАО)",
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Для вновь образованных РАО 8-ой символ кода РАО должен быть равен 0."
                    });
                }
                if (!operationCodes.Contains(forms[line].OperationCode_DB) 
                    && storageCodeValid.Contains(storageCode1) 
                    && forms[line].StatusRAO_DB != "1")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "StatusRAO_DB",
                        Value = forms[line].StatusRAO_DB,
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Проверьте статус РАО."
                    });
                }
                if (!storageCodeValid.Contains(storageCode1) && forms[line].StatusRAO_DB == "1")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao8RaoClass} (8-ой символ кода РАО)",
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Особые РАО могут быть размещены только в ПРОРАО либо ПКОРАО."
                    });
                }
            }
            else if (codeRao8RaoClass == "9")
            {
                var storageCode1 = forms[line].StoragePlaceCode_DB[..1];
                var operationCodes = new [] { "11", "12", "13", "14", "16", "41" };
                if (operationCodes.Contains(forms[line].OperationCode_DB) && storageCode1 == "5")
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "StatusRAO_DB",
                        Value = forms[line].StatusRAO_DB,
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Для вновь образованных РАО 8-ой символ кода РАО должен быть равен 0."
                    });
                }
                if (!(storageCode1 == "2" && forms[line].StatusRAO_DB == "1"))
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao8RaoClass} (8-ой символ кода РАО)",
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Идентификатор 9 используется только для тех РАО, по которым решение об отнесении к особым " +
                                  "или удаляемым отложено в ходе проведения первичной регистрации."
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
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = $"{codeRao910TypeCode} (9-10 символы кода РАО)",
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Недопустимое значение 9-10 символов кода РАО."
            });
        }
        else
        {
            if (codeRao910TypeCode == "94")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "CodeRAO_DB",
                    Value = $"{codeRao910TypeCode} (9-10 символы кода РАО)",
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Сведения о РАО, подготовленных для передачи национальному оператору, предоставляются с форме 1.8."
                });
            }
            else
            {
                var requiresNote = new [] { "19", "29", "39", "59", "69", "78", "79", "89", "99" };
                if (requiresNote.Contains(codeRao910TypeCode) && !noteExists)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "CodeRAO_DB",
                        Value = $"{codeRao910TypeCode} (9-10 символы кода РАО)",
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Необходимо заполнить примечание к коду типа РАО."
                    });
                }
            }
        }

        #endregion

        #region symbol 11

        if (!codeRao11Valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "CodeRAO_DB",
                Value = $"{codeRao11Flammability} (11-ый символ кода РАО)",
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Недопустимое значение 11-го символа кода РАО."
            });
        }
        else
        {

        }

        #endregion

        return result;
    }

    #endregion

    #region Check006_11

    private static List<CheckError> Check_006_11(List<Form16> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "11", "12", "13", "14", "41" };
        var operationCode = forms[line].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var statusRaoDB = forms[line].StatusRAO_DB;
        var repOKPOList = forms10
            .Select(x => x.Okpo_DB)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        var valid = repOKPOList.Contains(statusRaoDB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StatusRAO_DB",
                Value = statusRaoDB,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "РАО, образовавшиеся после 15.07.2011, находятся в собственности организации, " +
                          "в результате деятельности которой они образовались."
            });
        }
        return result;
    }

    #endregion

    #region Check006_26

    private static List<CheckError> Check_006_26(List<Form16> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "26", "28", "63" };
        var operationCode = forms[line].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var statusRao = forms[line].StatusRAO_DB;
        var repOKPOList = forms10
            .Select(x => x.Okpo_DB)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        var valid = repOKPOList.Contains(statusRao);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StatusRAO_DB",
                Value = statusRao,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Операция, соответствующая выбранному коду, может использоваться только для собственных РАО."
            });
        }
        return result;
    }

    #endregion

    #region Check006_38

    private static List<CheckError> Check_006_38(List<Form16> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "38", "64" };
        var operationCode = forms[line].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var statusRao = forms[line].StatusRAO_DB;
        var repOKPOList = forms10
            .Select(x => x.Okpo_DB)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        var valid = repOKPOList.Contains(statusRao);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StatusRAO_DB",
                Value = statusRao,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "При операциях, связанных с получением права собственности, " +
                          "в графе статус РАО необходимо отразить код ОКПО отчитывающейся организации."
            });
        }
        return result;
    }

    #endregion

    #region Check006_42

    private static List<CheckError> Check_006_42(List<Form16> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "42", "43", "73", "97", "98" };
        var operationCode = forms[line].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var statusRao = forms[line].StatusRAO_DB;
        var repOKPOList = forms10
            .Select(x => x.Okpo_DB)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        var valid = repOKPOList.Contains(statusRao);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StatusRAO_DB",
                Value = statusRao,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Проверьте правильность статуса РАО."
            });
        }
        return result;
    }

    #endregion

    #region Check006_22
    private static List<CheckError> Check_006_22(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "22", "32" };
        var operationCode = forms[line].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var applicableRaoStatuses = new [] { "1" };
        var statusRao = forms[line].StatusRAO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(statusRao) || applicableRaoStatuses.Contains(statusRao);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StatusRAO_DB",
                Value = statusRao,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Проверьте правильность статуса РАО."
            });
        }
        return result;
    }

    #endregion

    #region Check006_16

    private static List<CheckError> Check_006_16(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "16" };
        var operationCode = forms[line].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var applicableRaoStatuses = new [] { "2" };
        var statusRao = forms[line].StatusRAO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(statusRao) || applicableRaoStatuses.Contains(statusRao);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StatusRAO_DB",
                Value = statusRao,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Проверьте правильность статуса РАО"
            });
        }
        return result;
    }

    #endregion

    #region Check006_76

    private static List<CheckError> Check_006_76(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "76" };
        var operationCode = forms[line].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var statusRao = forms[line].StatusRAO_DB;
        var valid = statusRao is "6" or "9";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StatusRAO_DB",
                Value = statusRao,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Проверьте правильность статуса РАО."
            });
        }
        return result;
    }

    #endregion

    #region Check006_Other

    private static List<CheckError> Check_006_Other(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new []
        {
            "10", "18", "21", "25", "27", "29", "31", "35", "36", "37", "39", "44", 
            "45", "48", "49", "51", "52", "56", "57", "59", "68", "71", "74", "75"
        };
        var operationCode = forms[line].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var applicableRaoStatuses = new [] { "1", "2", "3", "4", "6", "9" };
        var statusRao = forms[line].StatusRAO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        var valid = okpoRegex.IsMatch(statusRao) || applicableRaoStatuses.Contains(statusRao);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StatusRAO_DB",
                Value = statusRao,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Заполнение графы 5 не соответствует требованиям приказа Госкорпорации \"Росатом\" от 07.12.2020 № 1/13-НПА."
            });
        }
        return result;
    }

    #endregion

    #region Check007

    private static List<CheckError> Check_007(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var volume = forms[line].Volume_DB;
        var valid = TryParseFloatExtended(volume, out var value) && value > 0;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Volume_DB",
                Value = volume,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Необходимо заполнить сведения об объеме РАО, если представляемые данные являются расчетными, " +
                          "то соответствующие значения указываются в круглых скобках."
            });
        }
        return result;
    }

    #endregion

    #region Check008

    private static List<CheckError> Check_008(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var mass = forms[line].Mass_DB;
        var volume = forms[line].Volume_DB;
        var valid = TryParseFloatExtended(mass, out var massReal) && massReal > 0;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Плотность (07 - масса / 06 - объём)",
                Value = mass,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Необходимо заполнить сведения о массе РАО, если представляемые данные являются расчетными, " +
                          "то соответствующие значения указываются в круглых скобках."
            });
            return result;
        }
        if (!float.TryParse(ConvertStringToExponential(volume), out var volumeReal)) return result;
        if (volumeReal == 0.0) return result;
        var densityReal = massReal / volumeReal;
        if (densityReal > 21.0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Плотность (07 - Масса / 06 - Объём)",
                Value = densityReal.ToString(),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Проверьте значение массы и объема. Расчетное значение плотности слишком большое."
            });
        }
        else if (densityReal < 0.005)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Плотность (07 - Масса / 06 - Объём)",
                Value = densityReal.ToString(),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Проверьте значение массы и объема. Расчетное значение плотности слишком маленькое."
            });
        }
        return result;
    }

    #endregion

    #region Check009

    private static List<CheckError> Check_009(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var codeRao = forms[line].CodeRAO_DB.Trim();
        var quantityOziii = forms[line].QuantityOZIII_DB.Trim();
        var quantityOziiiExists = int.TryParse(quantityOziii, out _);
        var raoTypes1 = new [] { "81", "82", "85", "86", "87", "88", "89" };
        var raoTypes2 = new [] { "99" };
        if (codeRao.Length < 10) return result;
        var typeRao = codeRao.Substring(8, 2);
        var stateRao = codeRao[..1];
        if (raoTypes1.Contains(typeRao))
        {
            if (!quantityOziiiExists)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "QuantityOZIII_DB",
                    Value = quantityOziii,
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Необходимо заполнить сведения о количестве ОЗИИИ."
                });
            }
        }
        else if (raoTypes2.Contains(typeRao))
        {
            if (!quantityOziiiExists && quantityOziii != "-")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "QuantityOZIII_DB",
                    Value = quantityOziii,
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Необходимо заполнить сведения о количестве ОЗИИИ."
                });
            }
        }
        else if (quantityOziii != "-" || quantityOziiiExists)
        {
            var msg = stateRao == "2" 
                ? " (справочное сообщение о возможной ошибке)" 
                : string.Empty;
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "QuantityOZIII_DB",
                Value = quantityOziii,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Графа заполняется только для ОЗИИИ. Поставьте прочерк." + msg
            });
        }
        return result;
    }

    #endregion

    #region Check010

    private static List<CheckError> Check_010(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var rads = forms[line].MainRadionuclids_DB;
        if (string.IsNullOrWhiteSpace(rads))
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Radionuclids_DB",
                Value = Convert.ToString(rads),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Заполните графу 9 «Основные радионуклиды»."
            });
            return result;
        }
        var codeRao = forms[line].CodeRAO_DB.Trim();
        if (codeRao.Length < 3) return result;
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();
        if (radsSet.Count == 1 && string.Equals(radsSet.First(), "-"))
        {
            if (codeRao.Substring(2, 1) != "0")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Radionuclids_DB",
                    Value = Convert.ToString(rads),
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Заполните графу 9 «Основные радионуклиды»"
                });
                return result;
            }
        }
        else
        {
            var unknownNuclids = new List<string>();
            unknownNuclids
                .AddRange(radsSet
                    .Where(rad => R
                        .All(phEntry => phEntry["name"]
                            .Replace(" ", string.Empty) != rad)));
            var valid = unknownNuclids.Count == 0;
            if (!valid)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "Radionuclids_DB",
                    Value = Convert.ToString(rads),
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Формат ввода данных не соответствует приказу. " +
                              "Наименование радионуклида указывается названием химического элемента на русском языке, " +
                              "с указанием через дефис массового числа изотопа."
                });
            }
        }
        return result;
    }
    #endregion

    #region Check011
    private static List<CheckError> Check_011(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var tritiumActivity = ConvertStringToExponential(forms[line].TritiumActivity_DB);
        var rads = forms[line].MainRadionuclids_DB;
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();
        if (!radsSet
                .Any(rad => R
                    .Any(phEntry => phEntry["name"] == rad && phEntry["code"] == "т")))
        {
            if (TryParseFloatExtended(tritiumActivity, out _))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "TritiumActivity_DB",
                    Value = tritiumActivity,
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Проверьте перечень основных радионуклидов: указана суммарная активность для трития, " +
                              "но тритий не приведен в перечне радионуклидов."
                });
            }
            return result;
        }

        if (!double.TryParse(tritiumActivity,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var activityReal))
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TritiumActivity_DB",
                Value = Convert.ToString(forms[line].TritiumActivity_DB),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Для указанного в графе 9 радионуклидного состава должна быть приведена активность в графе 10."
            });
            return result;
        }
        switch (activityReal)
        {
            case <= 10e+01:
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "TritiumActivity_DB",
                    Value = Convert.ToString(forms[line].TritiumActivity_DB),
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте значение суммарной активности в графе 10."
                });
                break;
            }
            case > 10e+20:
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "TritiumActivity_DB",
                    Value = Convert.ToString(forms[line].TritiumActivity_DB),
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте значение суммарной активности в графе 10. " +
                              "Указанная суммарная активность превышает предельное значение."
                });
                break;
            }
        }
        return result;
    }

    #endregion

    #region Check012

    private static List<CheckError> Check_012(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var activity = ConvertStringToExponential(forms[line].BetaGammaActivity_DB);
        var rads = forms[line].MainRadionuclids_DB;
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();
        if (!radsSet
                .Any(rad => R
                    .Any(phEntry => phEntry["name"] == rad && phEntry["code"] == "б")))
        {
            if (TryParseFloatExtended(activity.Trim(), out var val))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "BetaGammaActivity_DB",
                    Value = Convert.ToString(forms[line].BetaGammaActivity_DB),
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Проверьте перечень основных радионуклидов: указана суммарная активность для бета-, " +
                              "гамма-излучающих радионуклидов, но бета-, гамма-излучающие радионуклиды не приведены в перечне радионуклидов."
                });
            }
            return result;
        }

        if (!double.TryParse(activity,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var activityReal))
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "BetaGammaActivity_DB",
                Value = Convert.ToString(forms[line].BetaGammaActivity_DB),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Для указанного в графе 9 радионуклидного состава должна быть приведена активность в графе 11."
            });
            return result;
        }
        switch (activityReal)
        {
            case <= 10e+01:
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "BetaGammaActivity_DB",
                    Value = Convert.ToString(forms[line].BetaGammaActivity_DB),
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте значение суммарной активности в графе 11."
                });
                break;
            }
            case > 10e+20:
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "BetaGammaActivity_DB",
                    Value = Convert.ToString(forms[line].BetaGammaActivity_DB),
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте значение суммарной активности в графе 11. " +
                              "Указанная суммарная активность превышает предельное значение."
                });
                break;
            }
        }
        return result;
    }

    #endregion

    #region Check013

    private static List<CheckError> Check_013(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var activity = ConvertStringToExponential(forms[line].AlphaActivity_DB);
        var rads = forms[line].MainRadionuclids_DB;
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();
        if (!radsSet
                .Any(rad => R
                    .Any(phEntry => phEntry["name"] == rad && phEntry["code"] == "а")))
        {
            if (TryParseFloatExtended(activity, out var val))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "AlphaActivity_DB",
                    Value = Convert.ToString(forms[line].AlphaActivity_DB),
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Проверьте перечень основных радионуклидов: указана суммарная активность для альфа-излучающих радионуклидов, " +
                              "но альфа-излучающие радионуклиды не приведены в перечне радионуклидов."
                });
            }
            return result;
        }

        if (!double.TryParse(ConvertStringToExponential(activity),
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var activityReal))
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "AlphaActivity_DB",
                Value = Convert.ToString(forms[line].AlphaActivity_DB),
                Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                          "Для указанного в графе 9 радионуклидного состава должна быть приведена активность в графе 12."
            });
            return result;
        }
        switch (activityReal)
        {
            case <= 10e+01:
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "AlphaActivity_DB",
                    Value = Convert.ToString(forms[line].AlphaActivity_DB),
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте значение суммарной активности в графе 12."
                });
                break;
            }
            case > 10e+20:
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "AlphaActivity_DB",
                    Value = Convert.ToString(forms[line].AlphaActivity_DB),
                    Message = (checkNumPrint ? $"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - " : "") + 
                              "Проверьте значение суммарной активности в графе 12. " +
                              "Указанная суммарная активность превышает предельное значение."
                });
                break;
            }
        }
        return result;
    }

    #endregion

    #region Check014
    private static List<CheckError> Check_014(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var activity = ConvertStringToExponential(forms[line].TransuraniumActivity_DB);
        var rads = forms[line].MainRadionuclids_DB;
        var radsSet = rads
            .ToLower()
            .Replace(',', ';')
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();
        if (!radsSet
                .Any(rad => R
                    .Any(phEntry => phEntry["name"] == rad && phEntry["code"] == "у")))
        {
            if (TryParseFloatExtended(activity.Trim(), out _))
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "TransuraniumActivity_DB",
                    Value = Convert.ToString(forms[line].TransuraniumActivity_DB),
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "Проверьте перечень основных радионуклидов: указана суммарная активность для трансурановых радионуклидов, " +
                              "но трансурановые радионуклиды не приведены в перечне радионуклидов."
                });
            }
            return result;
        }
        if (TryParseFloatExtended(activity, out var activityReal))
        {
            switch (activityReal)
            {
                case <= 10e+01f:
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "TransuraniumActivity_DB",
                        Value = Convert.ToString(forms[line].TransuraniumActivity_DB),
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Проверьте значение суммарной активности в графе 13."
                    });
                    break;
                }
                case > 10e+20f:
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "TransuraniumActivity_DB",
                        Value = Convert.ToString(forms[line].TransuraniumActivity_DB),
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Проверьте значение суммарной активности в графе 13. " +
                                  "Указанная суммарная активность превышает предельное значение."
                    });
                    break;
                }
            }
        }
        else
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransuraniumActivity_DB",
                Value = Convert.ToString(forms[line].TransuraniumActivity_DB),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Для указанного в графе 9 радионуклидного состава должна быть приведена активность в графе 13."
            });
        }
        return result;
    }

    #endregion

    #region Check015

    private static List<CheckError> Check_015(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var activityDate = forms[line].ActivityMeasurementDate_DB;
        var operationDate = forms[line].OperationDate_DB;
        var operationDateReal = DateTime.UnixEpoch;
        var valid = DateTime.TryParse(activityDate, out var activityDateReal) 
                    && DateTime.TryParse(operationDate, out operationDateReal);
        if (valid)
        {
            valid = activityDateReal <= operationDateReal;
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "ActivityMeasurementDate_DB",
                Value = activityDate,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Проверьте правильность указанной даты измерения активности. Дата не может быть позже даты операции."
            });
        }
        return result;
    }

    #endregion

    #region Check016

    private static List<CheckError> Check_016(List<Form16> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        byte?[] validDocument = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 19 };
        var documentVid = forms[line].DocumentVid_DB;
        const byte graphNumber = 15;
        var noteExists = CheckNotePresence(notes, line, graphNumber);
        var valid = validDocument.Contains(documentVid);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentVid_DB",
                Value = documentVid.ToString(),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Графа не может быть пустой."
            });
        }
        valid = (documentVid is not 19) || noteExists;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentVid_DB",
                Value = documentVid.ToString(),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Для вида документа 19 следует указать примечание с наименованием документа."
            });
        }
        return result;
    }

    #endregion

    #region Check017

    private static List<CheckError> Check_017(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var documentNumberDB = forms[line].DocumentNumber_DB;
        var valid = !string.IsNullOrWhiteSpace(documentNumberDB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "DocumentNumber_DB",
                Value = Convert.ToString(documentNumberDB),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Графа не может быть пустой."
            });
        }
        return result;
    }

    #endregion

    #region Check018

    private static List<CheckError> Check_018(List<Form16> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var operationDate = forms[line].OperationDate_DB;
        var documentDate = forms[line].DocumentDate_DB;
        DateOnly pMid;
        bool valid;
        switch (operationCode)
        {
            case "41":
            {
                valid = DateOnly.TryParse(documentDate, out pMid)
                        && DateOnly.TryParse(operationDate, out var pOper)
                        && pMid == pOper;
                if (!valid)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "DocumentDate_DB",
                        Value = Convert.ToString(documentDate),
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Дата документа должна соответствовать дате операции."
                    });
                }
                break;
            }
            case "10":
            {
                valid = DateOnly.TryParse(rep.StartPeriod_DB, out var pStart)
                        && DateOnly.TryParse(rep.EndPeriod_DB, out var pEnd)
                        && DateOnly.TryParse(documentDate, out pMid)
                        && pMid >= pStart && pMid <= pEnd;
                if (!valid)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "DocumentDate_DB",
                        Value = Convert.ToString(documentDate),
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Дата документа выходит за границы периода."
                    });
                }
                break;
            }
            default:
            {
                valid = DateOnly.TryParse(documentDate, out pMid)
                        && DateOnly.TryParse(operationDate, out var pOper)
                        && pMid <= pOper;
                if (!valid)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_16",
                        Row = forms[line].NumberInOrder_DB.ToString(),
                        Column = "DocumentDate_DB",
                        Value = Convert.ToString(documentDate),
                        Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                                  "Дата документа не может быть позже даты операции."
                    });
                }
                break;
            }
        }
        return result;
    }

    #endregion

    #region Check019_10

    //Код ОКПО поставщика/получателя не равен коду ОКПО отчитывающейся организации + 8/14 цифр (колонка 18)
    private static List<CheckError> Check_019_10(List<Form16> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes =
        {
            "10", "11", "12", "13", "14", "16", "18", "41", "42", "43", "44", "45", "48", "49", 
            "51", "52", "56", "57", "59", "68", "71", "72", "73", "74", "75", "76", "97", "98"
        };
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
                FormNum = "form_16",
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

    #region Check019_21

    //Код ОКПО поставщика/получателя не равен коду ОКПО отчитывающейся организации + 8/14 цифр (колонка 18)
    private static List<CheckError> Check_019_21(List<Form16> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "21", "25", "26", "27", "28", "29", "31", "35", "36", "37", "38", "39", "99" };
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
                FormNum = "form_16",
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
                FormNum = "form_16",
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

    #region Check019_22

    //Код ОКПО поставщика/получателя состоит из 8/14 чисел или "минобороны" (колонка 18)
    private static List<CheckError> Check_019_22(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var providerOrRecieverOKPO = forms[line].ProviderOrRecieverOKPO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (operationCode is not ("22" or "32")) return result;
        var valid = okpoRegex.IsMatch(providerOrRecieverOKPO)
                    || providerOrRecieverOKPO.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = Convert.ToString(providerOrRecieverOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Формат ввода данных не соответствует приказу. Следует указать код ОКПО контрагента, либо \"Минобороны\" без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check020_01

    //При определенных кодах операции, код ОКПО перевозчика равен "-"
    private static List<CheckError> Check_020_01(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = 
        {
            "01", "10", "11", "12", "13", "14", "16", "18", "41", "42", "43", "44", "45", "48", "49", 
            "51", "52", "56", "57", "59", "71", "72", "73", "74", "75", "76", "97", "98", "99"
        };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var transporterOKPO = forms[line].TransporterOKPO_DB.Trim();
        var valid = transporterOKPO is "-";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "При выбранном коде операции транспортирование не производится, в графе 19 должен стоять прочерк."
            });
        }
        return result;
    }

    #endregion

    #region Check020_21

    //При определенных кодах операции, код ОКПО перевозчика равен 8/14 цифр, либо прим
    private static List<CheckError> Check_020_21(List<Form16> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "21","25","26","27","28","31","35","36","37","38" };
        var operationCode = forms[line].OperationCode_DB;
        var transporterOKPO = forms[line].TransporterOKPO_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        const byte graphNumber = 19;
        var noteExists = CheckNotePresence(notes, line, graphNumber);
        var valid = okpoRegex.IsMatch(transporterOKPO) 
                    || transporterOKPO.Equals("прим.", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Необходимо указать код ОКПО организации перевозчика, либо \"прим.\" без кавычек."
            });
        }
        else if (transporterOKPO.Equals("прим.", StringComparison.CurrentCultureIgnoreCase) && !noteExists)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "При указании \"прим.\" требуется примечание к ячейке."
            });
        }
        return result;
    }

    #endregion

    #region Check020_22

    //Код ОКПО перевозчика состоит из 8/14 чисел или "минобороны" или "прим"
    private static List<CheckError> Check_020_22(List<Form16> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "22", "32" };
        var operationCode = forms[line].OperationCode_DB;
        var transporterOKPO = forms[line].TransporterOKPO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        const byte graphNumber = 19;
        var noteExists = CheckNotePresence(notes, line, graphNumber);
        var valid = okpoRegex.IsMatch(transporterOKPO)
                    || transporterOKPO.Equals("минобороны", StringComparison.CurrentCultureIgnoreCase)
                    || transporterOKPO.Equals("прим.", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Необходимо указать код ОКПО организации перевозчика, либо \"Минобороны\" без кавычек, либо \"прим.\" без кавычек."
            });
        }
        else if (transporterOKPO.Equals("прим.", StringComparison.CurrentCultureIgnoreCase) && !noteExists)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "При указании \"прим.\" требуется примечание к ячейке."
            });
        }
        return result;
    }

    #endregion

    #region Check020_29

    //Код ОКПО перевозчика состоит из 8/14 чисел или "-" или "прим"
    private static List<CheckError> Check_020_29(List<Form16> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "29", "39" };
        var operationCode = forms[line].OperationCode_DB;
        var transporterOKPO = forms[line].TransporterOKPO_DB;
        var okpoRegex = new Regex(@"^\d{8}([0123456789_]\d{5})?$");
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        const byte graphNumber = 19;
        var noteExists = CheckNotePresence(notes, line, graphNumber);
        var valid = okpoRegex.IsMatch(transporterOKPO)
                    || transporterOKPO.Equals("-", StringComparison.CurrentCultureIgnoreCase)
                    || transporterOKPO.Equals("прим.", StringComparison.CurrentCultureIgnoreCase);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Необходимо указать код ОКПО организации перевозчика, либо \"-\" без кавычек, либо \"прим.\" без кавычек."
            });
        }
        else if (transporterOKPO.Equals("прим.", StringComparison.CurrentCultureIgnoreCase) && !noteExists)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "TransporterOKPO_DB",
                Value = Convert.ToString(transporterOKPO),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "При указании \"прим.\" требуется примечание к ячейке."
            });
        }
        return result;
    }

    #endregion

    #region Check021

    private static List<CheckError> Check_021(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var storagePlaceNameDB = forms[line].StoragePlaceName_DB;
        var valid = !string.IsNullOrWhiteSpace(storagePlaceNameDB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StoragePlaceName_DB",
                Value = Convert.ToString(storagePlaceNameDB),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Графа 20 должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check022

    private static List<CheckError> Check_022(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var storagePlaceCodeDB = forms[line].StoragePlaceCode_DB;
        var valid = !string.IsNullOrWhiteSpace(storagePlaceCodeDB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "StoragePlaceCode_DB",
                Value = Convert.ToString(storagePlaceCodeDB),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Графа 21 должна быть заполнена."
            });
        }
        return result;
    }

    #endregion

    #region Check023_44

    private static List<CheckError> Check_023_44(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new string[] { "44" };
        var applicableValues = new string[] {
            "11","12","13","14","15","16","17","19",
            "21","22","23","24",               "29",
            "31","32",                         "39",
            "41","42","43",                    "49",
            "51","52","53","54","55",
            "61","62","63",
            "71","72","73","74",               "79",
                                               "99"
        };
        var operationCode = forms[line].OperationCode_DB.Trim();
        var refineOrSortRaoCodeDB = forms[line].RefineOrSortRAOCode_DB.Trim();
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = applicableValues.Contains(refineOrSortRaoCodeDB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = forms[line].RefineOrSortRAOCode_DB,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          $"Для кода операции {operationCode} код переработки/сортировки {refineOrSortRaoCodeDB} недопустим."
            });
        }
        return result;
    }

    #endregion

    #region Check023_45

    private static List<CheckError> Check_023_45(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "45", "57" };
        var applicableValues = new [] { "-", "74" };
        var operationCode = forms[line].OperationCode_DB.Trim();
        var refineOrSortRaoCodeDB = forms[line].RefineOrSortRAOCode_DB.Trim();
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = applicableValues.Contains(refineOrSortRaoCodeDB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = forms[line].RefineOrSortRAOCode_DB,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Коду операции упаковка/переупаковка не соответствует код переработки/сортировки."
            });
        }
        return result;
    }

    #endregion

    #region Check023_49

    private static List<CheckError> Check_023_49(List<Form16> forms, int line)
    {   
        List<CheckError> result = new();
        var applicableOperationCodes = new [] { "49", "59" };
        var applicableRefineCodes = new [] { "52", "72", "74", "-" };
        var operationCode = forms[line].OperationCode_DB.Trim();
        var refineOrSortRaoCodeDB = forms[line].RefineOrSortRAOCode_DB.Trim();
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = applicableRefineCodes.Contains(refineOrSortRaoCodeDB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = forms[line].RefineOrSortRAOCode_DB,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Коду операции сортировка соответствуют коды сортировки 52, 72, 74."
            });
        }
        return result;
    }

    #endregion

    #region Check023_10

    private static List<CheckError> Check_023_10(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var applicableOperationCodes = new string[] { "10","11","12","13","14","16","18","21","22","25","26","27","28","29","31","32","35",
            "36","37","38","39","42","43","48","51","52","63","64","68","71","72","73","74","75",
            "76","97","98","99" };
        var operationCode = forms[line].OperationCode_DB;
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = forms[line].RefineOrSortRAOCode_DB.Trim() == "-";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = forms[line].RefineOrSortRAOCode_DB,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "При данном коде операции для кода переработки/сортировки используется символ «-»."
            });
        }
        return result;
    }

    #endregion

    #region Check023_22_44

    private static List<CheckError> Check_023_22_44(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        if (forms[line].RefineOrSortRAOCode_DB != "22") return result;
        var CodeRAO_DB = forms[line].CodeRAO_DB;
        if (CodeRAO_DB.Length < 11) return result;
        if (forms[line].OperationCode_DB != "44") return result;
        var valid = CodeRAO_DB.Substring(10, 1) == "1";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = forms[line].RefineOrSortRAOCode_DB,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "РАО направлены на установку сжигания. Проверьте значение 11 символа кода РАО."
            });
        }
        return result;
    }

    #endregion

    #region Check023_22_56

    private static List<CheckError> Check_023_22_56(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        if (forms[line].RefineOrSortRAOCode_DB != "22") return result;
        var CodeRAO_DB = forms[line].CodeRAO_DB;
        if (CodeRAO_DB.Length < 10) return result;
        if (forms[line].OperationCode_DB != "56") return result;
        var valid = (CodeRAO_DB[..1] is "2" && CodeRAO_DB.Substring(8, 2) is "66" or "74") 
                    || (CodeRAO_DB[..1] is "1" && CodeRAO_DB.Substring(8, 2) is "14");
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "RefineOrSortRAOCode_DB",
                Value = forms[line].RefineOrSortRAOCode_DB,
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "РАО направлены на переработку на установке сжигания. Проверьте значение кода типа РАО в коде РАО."
            });
        }
        return result;
    }

    #endregion

    #region Check024

    private static List<CheckError> Check_024(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var packName = forms[line].PackName_DB;
        var valid = !string.IsNullOrWhiteSpace(packName);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "PackName_DB",
                Value = Convert.ToString(packName),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Заполните сведения об упаковке РАО. Если РАО размещены без упаковки, то в графе 23 указывается «без упаковки»."
            });
        }
        return result;
    }

    #endregion

    #region Check025

    private static List<CheckError> Check_025(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var packType = forms[line].PackType_DB;
        var valid = !string.IsNullOrWhiteSpace(packType);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "PackType_DB",
                Value = Convert.ToString(packType),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Заполните сведения в графе 24. В случае, если тип отсутствует, укажите символ «-» без кавычек."
            });
        }
        return result;
    }

    #endregion

    #region Check026

    private static List<CheckError> Check_026(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var packNum = forms[line].PackNumber_DB;
        var packName = forms[line].PackName_DB;
        var valid = !string.IsNullOrWhiteSpace(packNum);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "PackNumber_DB",
                Value = Convert.ToString(packNum),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Заполните сведения о заводском номере упаковки. Если заводской номер отсутствует необходимо привести в круглых скобках номер, присвоенный в организации."
            });
        }
        else
        {
            if (packName.ToLower().Trim() == "без упаковки" && packNum != "-")
            {
                result.Add(new CheckError
                {
                    FormNum = "form_16",
                    Row = forms[line].NumberInOrder_DB.ToString(),
                    Column = "PackNumber_DB",
                    Value = Convert.ToString(packNum),
                    Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                              "При указании в графе 23 \"без упаковки\" в графе 25 должен быть прочерк."
                });
            }
        }
        return result;
    }

    #endregion

    #region Check027

    private static List<CheckError> Check_027(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var subsidy = (forms[line].Subsidy_DB ?? string.Empty).Trim();
        if (subsidy is "" or "-") return result;
        var valid = float.TryParse(subsidy, out var subsidyFloatValue)
                    && subsidyFloatValue is >= 0 and <= 100;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "Subsidy_DB",
                Value = Convert.ToString(subsidy),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + "Проверьте значение субсидии."
            });
        }
        return result;
    }

    #endregion

    #region Check028

    private static List<CheckError> Check_028(List<Form16> forms, int line)
    {
        List<CheckError> result = new();
        var fcpNum = (forms[line].FcpNumber_DB ?? string.Empty).Trim();
        var valid = fcpNum is "" or "-" 
                    || TryParseFloatExtended(fcpNum, out _);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_16",
                Row = forms[line].NumberInOrder_DB.ToString(),
                Column = "FcpNumber_DB",
                Value = Convert.ToString(fcpNum),
                Message = (checkNumPrint?$"Проверка {MethodBase.GetCurrentMethod()?.Name.Replace("Check_", "").TrimStart('0')} - ":"") + 
                          "Графу 27 следует либо не заполнять, либо указать числовое значение или прочерк."
            });
        }
        return result;
    }

    #endregion

    #region Check029

    //overdue calculations
    private static List<CheckError> Check_029(List<Form16> forms, Report rep)
    {
        List<CheckError> result = new();
        List<string> overdueSetLines = new();
        var endPeriod = rep.EndPeriod_DB;
        if (DateOnly.TryParse(endPeriod, out var dateEnd))
        {
            for (var i = 0; i < forms.Count; i++)
            {
                var operationCode = forms[i].OperationCode_DB;
                var operationDate = forms[i].OperationDate_DB;
                var documentDate = forms[i].DocumentDate_DB;
                if (OverduePeriods_RAO.TryGetValue(operationCode, out var days) && DateOnly.TryParse(operationCode == "10" 
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
                FormNum = "form_16",
                Row = string.Join(", ", overdueSetLines),
                Column = "-",
                Value = "",
                Message = "Указанные операции просрочены."
            });
        }
        return result;
    }

    #endregion

    #endregion
}