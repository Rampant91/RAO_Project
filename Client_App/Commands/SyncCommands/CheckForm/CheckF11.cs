using System;
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

namespace Client_App.Commands.SyncCommands.CheckForm;

public class CheckF11
{
    private static readonly string[] OperationCode_DB_Valids =
    {
        "1","10","11","12","15","17","18","21",
        "22","25","27","28","29","31","32","35",
        "37","38","39","41","42","43","48","53",
        "54","58","61","62","63","64","65","66",
        "67","68","71","72","73","74","75","81",
        "82","83","84","85","86","87","88","97",
        "98","99"
    };

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

    public static string[] Type_DB_Valids =
    {

    };

    private static List<Dictionary<string, string>> OKSM = new()
    {
    };

    private static Dictionary<string, double> D = new()
    {
    };

    private static bool DB_Ignore = true;

    private static bool ZRI_Ignore = true;

    private static bool MZA_Ignore = true;

    #region DFromFile

    private static void D_Populate_From_File(string file_address)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(file_address)) return;
        FileInfo excel_import_file = new(file_address);
        var xls = new ExcelPackage(excel_import_file);
        var wrksht1 = xls.Workbook.Worksheets["Лист1"];
        var i = 2;
        string name_1, name_2, name_base, name_real;
        name_base = "аврорий";
        string value_base;
        double value_real;
        D.Clear();
        while (wrksht1.Cells[i, 1].Text != string.Empty)
        {
            name_1 = wrksht1.Cells[i, 2].Text;
            name_2 = wrksht1.Cells[i, 3].Text;
            if (name_1 != string.Empty)
            {
                name_base = name_1.ToLower();
            }
            if (name_2.Contains('-'))
            {
                if (name_2.Contains('+'))
                {
                    name_real = name_base + name_2[name_2.IndexOf('-')..name_2.IndexOf('+')];
                }
                else
                {
                    name_real = name_base + name_2[name_2.IndexOf('-')..];
                }
                value_base = wrksht1.Cells[i, 4].Text;
                if (value_base.Contains("Неограниченно"))
                {
                    value_real = double.MaxValue;
                }
                else
                {
                    value_real = 1e12 * double.Parse(value_base[..6].Replace(" ", ""), NumberStyles.Float);
                }
                D[name_real] = value_real;
                if (name_real.Contains("йод"))
                {
                    D[name_real.Replace('й', 'и')] = value_real;
                }
                else if (name_real.Contains("иод"))
                {
                    D[name_real.Replace('и', 'й')] = value_real;
                }
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
        var wrksht1 = xls.Workbook.Worksheets["Лист1"];
        var i = 8;
        OKSM.Clear();
        while (wrksht1.Cells[i, 1].Text != string.Empty)
        {
            OKSM.Add(new Dictionary<string, string>
            {
                {"kod", wrksht1.Cells[i, 2].Text},
                {"shortname", wrksht1.Cells[i, 3].Text},
                {"longname", wrksht1.Cells[i, 4].Text},
                {"alpha2", wrksht1.Cells[i, 5].Text},
                {"alpha3", wrksht1.Cells[i, 6].Text}
            });
            i++;
        }
    }

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
        foreach (var key in rep.Rows11)
        {
            var form = (Form11)key;
            var formsList = rep.Rows11.ToList<Form11>();
            var notes = rep.Notes.ToList<Note>();
            var forms10 = reps.Master_DB.Rows10.ToList<Form10>();
            errorList.AddRange(Check_001(formsList, currentFormLine));
            errorList.AddRange(Check_002(formsList, currentFormLine));
            errorList.AddRange(Check_003(formsList, currentFormLine));
            errorList.AddRange(Check_004(formsList, currentFormLine));
            errorList.AddRange(Check_005(formsList, currentFormLine));
            errorList.AddRange(Check_006(formsList, currentFormLine));
            errorList.AddRange(Check_007(formsList, notes, currentFormLine));
            errorList.AddRange(Check_008(formsList, currentFormLine));
            errorList.AddRange(Check_009(formsList, currentFormLine));
            errorList.AddRange(Check_010(formsList, currentFormLine));
            errorList.AddRange(Check_011(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_012(formsList, currentFormLine));
            errorList.AddRange(Check_013(formsList, currentFormLine));
            errorList.AddRange(Check_014(formsList, currentFormLine));
            errorList.AddRange(Check_015(formsList, currentFormLine));
            errorList.AddRange(Check_016(formsList, currentFormLine));
            errorList.AddRange(Check_017(formsList, rep, currentFormLine));
            errorList.AddRange(Check_018(formsList, rep, currentFormLine));
            errorList.AddRange(Check_019(formsList, currentFormLine));
            errorList.AddRange(Check_020(formsList, currentFormLine));
            errorList.AddRange(Check_021(formsList, currentFormLine));
            errorList.AddRange(Check_022(formsList, currentFormLine));
            errorList.AddRange(Check_023(formsList, currentFormLine));
            errorList.AddRange(Check_024(formsList, currentFormLine));
            errorList.AddRange(Check_025(formsList, currentFormLine));
            errorList.AddRange(Check_026(formsList, currentFormLine));
            errorList.AddRange(Check_027(formsList, currentFormLine));
            errorList.AddRange(Check_028(formsList, currentFormLine));
            errorList.AddRange(Check_029(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_030(formsList, currentFormLine));
            errorList.AddRange(Check_031(formsList, notes, currentFormLine));
            errorList.AddRange(Check_032(formsList, currentFormLine));
            errorList.AddRange(Check_033(formsList, currentFormLine));
            errorList.AddRange(Check_034(formsList, currentFormLine));
            errorList.AddRange(Check_035(formsList, currentFormLine));
            errorList.AddRange(Check_036(formsList, currentFormLine));
            errorList.AddRange(Check_037(formsList, currentFormLine));
            errorList.AddRange(Check_038(formsList, notes, currentFormLine));
            errorList.AddRange(Check_039(formsList, notes, currentFormLine));
            errorList.AddRange(Check_040(formsList, currentFormLine));
            errorList.AddRange(Check_041(formsList, currentFormLine));
            errorList.AddRange(Check_042(formsList, currentFormLine));
            errorList.AddRange(Check_043(formsList, currentFormLine));
            errorList.AddRange(Check_044(formsList, rep, currentFormLine));
            errorList.AddRange(Check_045(formsList, forms10, currentFormLine));
            errorList.AddRange(Check_046(formsList, currentFormLine));
            errorList.AddRange(Check_047(formsList, currentFormLine));
            errorList.AddRange(Check_048(formsList, notes, currentFormLine));
            errorList.AddRange(Check_049(formsList, currentFormLine));
            errorList.AddRange(Check_050(formsList, currentFormLine));
            errorList.AddRange(Check_051(formsList, currentFormLine));
            errorList.AddRange(Check_052(formsList, currentFormLine));
            errorList.AddRange(Check_053(formsList, currentFormLine));
            errorList.AddRange(Check_054(formsList, currentFormLine));
            errorList.AddRange(Check_055(formsList, currentFormLine));

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

    private static IEnumerable<CheckError> Check_001(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        return result;
    }

    private static IEnumerable<CheckError> Check_002(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (forms[line].Id < 1)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Id",
                Value = forms[line].Id.ToString(),
                Message = "-"
            });
        }
        return result;
    }

    private static List<CheckError> Check_003(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var valid = forms[line].OperationCode_DB != null && OperationCode_DB_Valids.Contains(forms[line].OperationCode_DB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = forms[line].OperationCode_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_004(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "10" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    private static List<CheckError> Check_005(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "12", "42" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = false;
        if (forms[line].Radionuclids_DB != null)
        {
            foreach (var nucleid in Radionuclids_DB_Valids)
            {
                if (forms[line].Radionuclids_DB != null && forms[line].Radionuclids_DB!.ToLower().Contains(nucleid))
                {
                    valid = true;
                    break;
                }
            }
        }
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

    private static List<CheckError> Check_006(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] ApplicableOperationCodes = { "15" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    private static List<CheckError> Check_007(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "29", "39", "97", "98", "99" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = false;
        List<int> note_rows_final_int;
        string note_rows_real;
        int note_rows_begin;
        int note_rows_end;
        List<string> note_rows_real_str;
        foreach (var note in notes)
        {
            if (note.RowNumber_DB != null && forms[line].ReportId != null)
            {
                note_rows_real = note.RowNumber_DB.Replace(" ", string.Empty);
                note_rows_final_int = new();
                note_rows_real_str = new(note_rows_real.Split(','));
                foreach (var note_row_cluster in note_rows_real_str)
                {
                    if (note_row_cluster.Contains('-'))
                    {
                        try
                        {
                            var note_row_bounds = note_row_cluster.Split('-');
                            if (note_row_bounds.Length != 2) throw new Exception();
                            note_rows_begin = int.Parse(note_row_bounds[0]);
                            note_rows_end = int.Parse(note_row_bounds[1]);
                            for (var i = note_rows_begin; i <= note_rows_end; i++)
                            {
                                note_rows_final_int.Add(i);
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }
                    else
                    {
                        try
                        {
                            note_rows_final_int.Add(int.Parse(note_row_cluster));
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
                foreach (var note_rownumber in note_rows_final_int)
                {
                    if (note_rownumber == line + 1 && note.GraphNumber_DB == 2.ToString())
                    {
                        if (note.Comment_DB != null && note.Comment_DB.Trim() != string.Empty)
                        {
                            valid = true;
                            break;
                        }
                    }
                }
                if (valid) break;
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = "-",
                Message = "Необходимо дать пояснение об осуществленной операции."
            });
        }
        return result;
    }

    private static List<CheckError> Check_008(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] ApplicableOperationCodes = { "21", "22", "25", "27", "28", "29", "41", "42", "43", "46", "53", "54", "61", "62", "65", "66", "67", "68", "71", "72", "81", "82", "83", "84", "88", "98" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    private static List<CheckError> Check_009(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] ApplicableOperationCodes = { "37" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    private static List<CheckError> Check_010(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] ApplicableOperationCodes = { "41" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    private static List<CheckError> Check_011(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "54" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].ProviderOrRecieverOKPO_DB != string.Empty;
        if (valid)
        {
            foreach (var org in forms10)
            {
                if (forms[line].ProviderOrRecieverOKPO_DB == org.Okpo_DB)
                {
                    valid = false;
                    break;
                }
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = forms[line].ProviderOrRecieverOKPO_DB,
                Message = "В графе 19 необходимо указать ОКПО подрядной организации."
            });
        }
        return result;
    }

    private static List<CheckError> Check_012(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] ApplicableOperationCodes = { "58" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    private static List<CheckError> Check_013(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] ApplicableOperationCodes = { "62" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    private static List<CheckError> Check_014(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        string[] ApplicableOperationCodes = { "65" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
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

    private static List<CheckError> Check_015(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "81", "88" };
        char[] numerics = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].CreatorOKPO_DB != null && (forms[line].CreatorOKPO_DB!.Length == 8 || forms[line].CreatorOKPO_DB!.Length == 14);
        if (valid)
        {
            foreach (var chr in forms[line].CreatorOKPO_DB!)
            {
                if (!numerics.Contains(chr))
                {
                    valid = false;
                    break;
                }
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = forms[line].CreatorOKPO_DB,
                Message = "Код используется для предоставления сведений о ЗРИ, произведенных в Российской Федерации."
            });
        }
        return result;
    }

    private static List<CheckError> Check_016(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "83", "84", "85", "86" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = false;

        foreach (var OKSM_Entry in OKSM)
        {
            if (OKSM_Entry["shortname"] == forms[line].CreatorOKPO_DB)
            {
                valid = true;
                break;
            }
        }
        if (!valid || forms[line].CreatorOKPO_DB.ToLower() is "россия")
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = forms[line].CreatorOKPO_DB,
                Message = "Код используется для предоставления сведений о ЗРИ, произведенных за пределами Российской Федерации."
            });
        }
        return result;
    }

    private static List<CheckError> Check_017(List<Form11> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] nonApplicableOperationCodes = { "10" };
        var opCode = forms[line].OperationCode_DB;
        if (nonApplicableOperationCodes.Contains(opCode)) return result;
        var valid = forms[line].OperationDate_DB != null;
        var pEnd = DateTime.MinValue;
        var pMid = DateTime.MinValue;
        if (valid && rep is { StartPeriod_DB: not null, EndPeriod_DB: not null })
        {
            valid = DateTime.TryParse(rep.StartPeriod_DB, out var pStart)
                    && DateTime.TryParse(rep.EndPeriod_DB, out pEnd)
                    && DateTime.TryParse(forms[line].OperationDate_DB!, out pMid)
                    && pMid >= pStart && pMid <= pEnd;
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
        else
        {
            string[] operationCodeWithDeadline1 = { "71" };
            string[] operationCodeWithDeadline5 = { "73", "74", "75" };
            string[] operationCodeWithDeadline10 =
            {
                "11", "12", "15", "17", "18", "21", "22", "25", "27", "28", "29", "31", "32", "35", "37", "38", "39",
                "41", "42", "43", "46", "47", "48", "53", "54", "58", "61", "62", "63", "64", "65", "66", "67", "68",
                "72", "81", "82", "83", "84", "85", "86", "87", "88", "97", "98", "99"
            };
            if (operationCodeWithDeadline10.Contains(opCode) && (pEnd - pMid).Days > 10)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_11",
                    Row = (line + 1).ToString(),
                    Column = "OperationDate_DB",
                    Value = forms[line].OperationDate_DB,
                    Message = "Дата окончания отчетного периода превышает дату операции более чем на 10 дней."
                });
            }
            else if (operationCodeWithDeadline5.Contains(opCode) && (pEnd - pMid).Days > 5)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_11",
                    Row = (line + 1).ToString(),
                    Column = "OperationDate_DB",
                    Value = forms[line].OperationDate_DB,
                    Message = "Дата окончания отчетного периода превышает дату операции более чем на 5 дней."
                });
            }
            else if (operationCodeWithDeadline1.Contains(opCode) && (pEnd - pMid).Days > 1)
            {
                result.Add(new CheckError
                {
                    FormNum = "form_11",
                    Row = (line + 1).ToString(),
                    Column = "OperationDate_DB",
                    Value = forms[line].OperationDate_DB,
                    Message = "Дата окончания отчетного периода превышает дату операции более чем на 1 день."
                });
            }
        }
        return result;
    }

    private static List<CheckError> Check_018(List<Form11> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] applicableOperationCodes = { "10" };
        if (!applicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].DocumentDate_DB != null;
        var pEnd = DateTime.MinValue;
        var pMid = DateTime.MinValue;
        if (valid && rep is { StartPeriod_DB: not null, EndPeriod_DB: not null })
        {
            valid = DateTime.TryParse(rep.StartPeriod_DB, out var pStart)
                    && DateTime.TryParse(rep.EndPeriod_DB, out pEnd)
                    && DateTime.TryParse(forms[line].DocumentDate_DB!, out pMid)
                    && pMid >= pStart && pMid <= pEnd;
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = forms[line].DocumentDate_DB,
                Message = "Дата акта инвентаризации не входит в отчетный период."
            });
        }
        else if ((pEnd - pMid).Days > 10)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = forms[line].OperationDate_DB,
                Message = "Дата окончания отчетного периода превышает дату операции более чем на 10 дней."
            });
        }
        return result;
    }

    private static List<CheckError> Check_019(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var valid = !string.IsNullOrEmpty(forms[line].PassportNumber_DB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PassportNumber_DB",
                Value = forms[line].PassportNumber_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_020(List<Form11> forms, int line)
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

    private static List<CheckError> Check_021(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (ZRI_Ignore) return result;
        var valid = !string.IsNullOrEmpty(forms[line].Type_DB)
                    && !string.IsNullOrEmpty(forms[line].Radionuclids_DB);
        if (!Type_DB_Valids.Contains(forms[line].PackType_DB)) return result;
        if (valid)
        {
            List<string> Radionuclids_DB_Valids_Full = new();
            List<string> Radionuclids_DB_Fact_Full = new(forms[line].Radionuclids_DB!.Replace(" ", string.Empty).ToLower().Split(";"));
            foreach (var radionuclid_fact in Radionuclids_DB_Fact_Full)
            {
                if (!Radionuclids_DB_Valids_Full.Contains(radionuclid_fact))
                {
                    valid = false;
                    break;
                }
                else
                {
                    Radionuclids_DB_Valids_Full.Remove(radionuclid_fact);
                }
            }
            valid = Radionuclids_DB_Valids_Full.Count == 0;
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

    private static List<CheckError> Check_022(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (string.IsNullOrEmpty(forms[line].Radionuclids_DB)) return result;
        var valid = !forms[line].Radionuclids_DB!.Contains(',');
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = forms[line].Radionuclids_DB,
                Message = "Формат ввода данных не соответствует приказу"
            });
        }
        return result;
    }

    private static List<CheckError> Check_023(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var pasNum = forms[line].PassportNumber_DB;
        if (pasNum == null) return result;
        var regex = new Regex(@".\d{1}-\d{1}.");
        var valid = !string.IsNullOrWhiteSpace(pasNum) 
                    || pasNum == "-" 
                    || !pasNum.Contains(',') 
                    || regex.IsMatch(pasNum);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PassportNumber_DB",
                Value = forms[line].Radionuclids_DB,
                Message = "Формат ввода данных не соответствует приказу"
            });
        }
        return result;
    }

    private static List<CheckError> Check_024(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var valid = forms[line].Quantity_DB != null && forms[line].Quantity_DB > 0;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Quantity_DB",
                Value = forms[line].Quantity_DB.ToString(),
                Message = "Формат ввода данных не соответствует приказу"
            });
        }
        return result;
    }

    private static List<CheckError> Check_025(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (MZA_Ignore) return result;
        if (string.IsNullOrEmpty(forms[line].Radionuclids_DB)
            || forms[line].Activity_DB == null || forms[line].Activity_DB == string.Empty
            || forms[line].Activity_DB == "-") return result;
        var nuclids_list = forms[line].Radionuclids_DB!.ToLower().Replace(" ", string.Empty).Split(';');
        if (nuclids_list.Length != 1) return result;
        if (!Radionuclids_DB_Valids.Contains(nuclids_list[0])) return result;
        //find the minimum activity
        double activity_minimum = float.MaxValue;
        if (D.TryGetValue(nuclids_list[0], out var value))
        {
            activity_minimum = value;
        }
        var activity_real = double.Parse(forms[line].Activity_DB!.Replace(".", ","), NumberStyles.Float);
        var valid = activity_real >= activity_minimum;
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

    private static List<CheckError> Check_026(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (MZA_Ignore) return result;
        if (string.IsNullOrEmpty(forms[line].Radionuclids_DB)
            || forms[line].Activity_DB == null
            || forms[line].Activity_DB == string.Empty
            || forms[line].Activity_DB == "-") return result;
        var nuclids_list = forms[line].Radionuclids_DB!.ToLower().Replace(" ", string.Empty).Split(';');
        if (nuclids_list.Length == 1) return result;
        foreach (var nuclid in nuclids_list)
        {
            if (!Radionuclids_DB_Valids.Contains(nuclid)) return result;
        }
        //find the minimum activity
        var activity_minimum = double.MaxValue;
        double activity_considered;
        foreach (var nuclid in nuclids_list)
        {
            activity_considered = float.MaxValue;
            if (D.TryGetValue(nuclid, out var value))
            {
                activity_considered = value;
            }
            if (activity_considered < activity_minimum)
            {
                activity_minimum = activity_considered;
            }
        }
        var activity_real = double.Parse(forms[line].Activity_DB!.Replace(".", ","), NumberStyles.Float);
        var valid = activity_real >= activity_minimum;
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

    private static List<CheckError> Check_027(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (MZA_Ignore) return result;
        return result;
    }

    private static List<CheckError> Check_028(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (string.IsNullOrEmpty(forms[line].Activity_DB) || forms[line].Activity_DB == "-") return result;
        if (double.TryParse(forms[line].Activity_DB!.Replace(".", ",").Replace("(", "").Replace(")", ""),
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var activity_real))
        {

        }
        var valid = activity_real <= 10e+20;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Activity_DB",
                Value = forms[line].Activity_DB,
                Message = "Проверьте правильность предоставления сведений по суммарной активности."
            });
        }
        return result;
    }

    private static List<CheckError> Check_029(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "11", "58" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = false;
        foreach (var org in forms10)
        {
            if (org.Okpo_DB == forms[line].CreatorOKPO_DB)
            {
                valid = true;
                break;
            }
        }
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

    private static List<CheckError> Check_030(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        char[] CreatorOKPO_DB_Valids = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_' };
        var valid = !string.IsNullOrEmpty(forms[line].CreatorOKPO_DB);
        if (valid)
        {
            foreach (var chr in forms[line].CreatorOKPO_DB!)
            {
                if (!CreatorOKPO_DB_Valids.Contains(chr))
                {
                    valid = false;
                    break;
                }
            }
        }
        if (!valid)
        {
            //a miniature Check_031 (see if the cell has a comment; don't check OKSM yet)
            foreach (var OKSM_Entry in OKSM)
            {
                if (OKSM_Entry["shortname"] == forms[line].CreatorOKPO_DB!)
                {
                    valid = true;
                    break;
                }
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "CreatorOKPO_DB",
                Value = forms[line].CreatorOKPO_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_031(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] CreatorOKPO_DB_Valids = { "прим.", "прим" };
        var valid = !string.IsNullOrEmpty(forms[line].CreatorOKPO_DB);
        List<int> note_rows_final_int;
        string note_rows_real;
        int note_rows_begin;
        int note_rows_end;
        List<string> note_rows_real_str;
        if (valid)
        {
            valid = false;
            foreach (var OKSM_Entry in OKSM)
            {
                if (OKSM_Entry["shortname"] == forms[line].CreatorOKPO_DB!)
                {
                    valid = true;
                    break;
                }
            }
            if (valid)
            {
                foreach (var note in notes)
                {
                    if (note.RowNumber_DB != null && forms[line].ReportId != null)
                    {
                        note_rows_real = note.RowNumber_DB.Replace(" ", string.Empty);
                        note_rows_final_int = new();
                        note_rows_real_str = new(note_rows_real.Split(','));
                        foreach (var note_row_cluster in note_rows_real_str)
                        {
                            if (note_row_cluster.Contains('-'))
                            {
                                try
                                {
                                    string[] note_row_bounds = note_row_cluster.Split('-');
                                    if (note_row_bounds.Length != 2) throw new Exception();
                                    note_rows_begin = int.Parse(note_row_bounds[0]);
                                    note_rows_end = int.Parse(note_row_bounds[1]);
                                    for (var i = note_rows_begin; i <= note_rows_end; i++)
                                    {
                                        note_rows_final_int.Add(i);
                                    }
                                }
                                catch
                                {
                                    break;
                                }
                            }
                            else
                            {
                                try
                                {
                                    note_rows_final_int.Add(int.Parse(note_row_cluster));
                                }
                                catch
                                {
                                    break;
                                }
                            }
                        }
                        foreach (var note_rownumber in note_rows_final_int)
                        {
                            if (note_rownumber == line + 1 && note.GraphNumber_DB == 10.ToString())
                            {
                                if (note.Comment_DB != null && note.Comment_DB.Trim() != string.Empty)
                                {
                                    valid = true;
                                    break;
                                }
                            }
                        }
                        if (valid) break;
                    }
                }
            }
            else
            {
                valid = true;
            }
        }
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

    private static List<CheckError> Check_032(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var valid = forms[line].OperationDate_DB != null && forms[line].CreationDate_DB != null;
        if (valid)
        {
            DateTime date_oper;
            DateTime date_create;
            try
            {
                date_oper = DateTime.Parse(forms[line].OperationDate_DB!);
                date_create = DateTime.Parse(forms[line].CreationDate_DB!);
                valid = date_create <= date_oper;
            }
            catch
            {
                valid = false;
            }
        }
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

    private static List<CheckError> Check_033(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        short[] Category_DB_Valids = { 1, 2, 3, 4, 5 };
        var DB_Bounds = new Dictionary<short, (double, double)>
        {
            { 1, (1000, double.MaxValue) },
            { 2, (10, 1000) },
            { 3, (1, 10) },
            { 4, (0.01, 1) },
            { 5, (0, 0.01) },
        };
        var valid = forms[line].Category_DB != null
                    && forms[line].Activity_DB != null
                    && DB_Bounds.ContainsKey((short)forms[line].Category_DB!);
        if (valid)
        {
            var A_value = 0.0;
            List<double> D_value_list = new();
            double D_min_value, D_max_value;
            double AD_min_bound, AD_max_bound;
            var nuclids_list = forms[line].Radionuclids_DB!.ToLower().Replace(" ", string.Empty).Split(';');
            valid = nuclids_list.Length > 0;
            if (valid)
            {
                foreach (var nuclid in nuclids_list)
                {
                    //get the values from the table, this is a placeholder
                    if (D.TryGetValue(nuclid, out var value))
                    {
                        D_value_list.Add(value);
                    }
                }
                if (D_value_list.Count == 0)
                {
                    foreach (var nuclid in nuclids_list)
                    {
                        foreach (var key in D.Keys)
                        {
                            if (key.Contains(nuclid))
                            {
                                D_value_list.Add(D[key] / (forms[line].Quantity_DB != null && forms[line].Quantity_DB != 0 ? (double)forms[line].Quantity_DB! : 1.0));
                                break;
                            }
                        }
                    }
                }
                if (D_value_list.Count == 0)
                {
                    result.Add(new CheckError
                    {
                        FormNum = "form_11",
                        Row = (line + 1).ToString(),
                        Column = "Radionuclids_DB",
                        Value = forms[line].Radionuclids_DB,
                        Message = "Проверьте правильность заполнения графы 6."
                    });
                    return result;
                }
                else
                {
                    D_min_value = D_value_list.Min();
                    D_max_value = D_value_list.Max();
                    try
                    {
                        A_value = double.Parse(forms[line].Activity_DB!.Replace(".", ","), NumberStyles.Float);
                    }
                    catch
                    {
                        valid = false;
                    }
                    if (valid)
                    {
                        AD_min_bound = D_max_value == 0.0 ? double.MaxValue : A_value / D_max_value;
                        AD_max_bound = D_min_value == 0.0 ? double.MaxValue : A_value / D_min_value;
                        valid = DB_Bounds[(short)forms[line].Category_DB!].Item1 <= AD_min_bound
                                && DB_Bounds[(short)forms[line].Category_DB!].Item2 > AD_max_bound;
                    }
                }
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Category_DB",
                Value = forms[line].Category_DB.ToString(),
                Message = "Проверьте правильность указания категории ЗРИ."
            });
        }
        return result;
    }

    private static List<CheckError> Check_034(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var valid = forms[line].SignedServicePeriod_DB != null
                    && forms[line].CreationDate_DB != null
                    && forms[line].CreationDate_DB != string.Empty
                    && !string.IsNullOrEmpty(forms[line].OperationDate_DB);
        if (valid)
        {
            DateTime CreationDate_Real = new();
            DateTime OperationDate_Real = new();
            try
            {
                CreationDate_Real = DateTime.Parse(forms[line].CreationDate_DB!);
                OperationDate_Real = DateTime.Parse(forms[line].OperationDate_DB!);
            }
            catch
            {
                valid = false;
            }
            if (valid)
            {
                CreationDate_Real = CreationDate_Real.AddMonths((int)forms[line].SignedServicePeriod_DB!);
                CreationDate_Real = CreationDate_Real.AddDays(Math.Round((double)(30 * (forms[line].SignedServicePeriod_DB % 1.0))!));
                valid = CreationDate_Real >= OperationDate_Real;
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "SignedServicePeriod_DB",
                Value = forms[line].SignedServicePeriod_DB.ToString(),
                Message = "Для ЗРИ истек НСС, следует продлить НСС либо снять с учета с одновременной постановкой на учет как РАО (при выполнении критериев отнесения к РАО)."
            });
        }
        return result;
    }

    private static List<CheckError> Check_035(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        short?[] PropertyCode_DB_Valids = { 1, 2, 3, 4, 5, 6, 9 };
        var valid = PropertyCode_DB_Valids.Contains(forms[line].PropertyCode_DB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PropertyCode_DB",
                Value = forms[line].PropertyCode_DB.ToString(),
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_036(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        short?[] PropertyCode_DB_Valids = { 1, 2, 3, 4 };
        if (!PropertyCode_DB_Valids.Contains(forms[line].PropertyCode_DB)) return result;
        char[] CreatorOKPO_DB_Valids = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_' };
        var valid = !string.IsNullOrEmpty(forms[line].Owner_DB);
        if (valid)
        {
            foreach (var chr in forms[line].Owner_DB!)
            {
                if (!CreatorOKPO_DB_Valids.Contains(chr))
                {
                    valid = false;
                    break;
                }
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = forms[line].Owner_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_037(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        short?[] PropertyCode_DB_Valids = { 5 };
        if (!PropertyCode_DB_Valids.Contains(forms[line].PropertyCode_DB)) return result;
        var valid = false;
        foreach (var OKSM_Entry in OKSM)
        {
            if (OKSM_Entry["shortname"] == forms[line].Owner_DB)
            {
                valid = true;
                break;
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "Owner_DB",
                Value = forms[line].Owner_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_038(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        short?[] PropertyCode_DB_Valids = { 6 };
        if (!PropertyCode_DB_Valids.Contains(forms[line].PropertyCode_DB)) return result;
        List<int> note_rows_final_int;
        string note_rows_real;
        int note_rows_begin;
        int note_rows_end;
        List<string> note_rows_real_str;
        var valid = false;
        foreach (var note in notes)
        {
            if (note.RowNumber_DB != null && forms[line].ReportId != null)
            {
                note_rows_real = note.RowNumber_DB.Replace(" ", string.Empty);
                note_rows_final_int = new();
                note_rows_real_str = new(note_rows_real.Split(','));
                foreach (var note_row_cluster in note_rows_real_str)
                {
                    if (note_row_cluster.Contains('-'))
                    {
                        try
                        {
                            var note_row_bounds = note_row_cluster.Split('-');
                            if (note_row_bounds.Length != 2) throw new Exception();
                            note_rows_begin = int.Parse(note_row_bounds[0]);
                            note_rows_end = int.Parse(note_row_bounds[1]);
                            for (var i = note_rows_begin; i <= note_rows_end; i++)
                            {
                                note_rows_final_int.Add(i);
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }
                    else
                    {
                        try
                        {
                            note_rows_final_int.Add(int.Parse(note_row_cluster));
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
                foreach (var note_rownumber in note_rows_final_int)
                {
                    if (note_rownumber == line + 1 && note.GraphNumber_DB == 15.ToString())
                    {
                        if (note.Comment_DB != null && note.Comment_DB.Trim() != string.Empty)
                        {
                            valid = true;
                            break;
                        }
                    }
                }
                if (valid) break;
            }
        }
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

    private static List<CheckError> Check_039(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        short?[] PropertyCode_DB_Valids = { 9 };
        if (!PropertyCode_DB_Valids.Contains(forms[line].PropertyCode_DB)) return result;
        List<int> note_rows_final_int;
        string note_rows_real;
        int note_rows_begin;
        int note_rows_end;
        List<string> note_rows_real_str;
        var valid = false;
        foreach (var note in notes)
        {
            if (note.RowNumber_DB != null && forms[line] != null)
            {
                note_rows_real = note.RowNumber_DB.Replace(" ", string.Empty);
                note_rows_final_int = new();
                note_rows_real_str = new(note_rows_real.Split(','));
                foreach (var note_row_cluster in note_rows_real_str)
                {
                    if (note_row_cluster.Contains('-'))
                    {
                        try
                        {
                            var note_row_bounds = note_row_cluster.Split('-');
                            if (note_row_bounds.Length != 2) throw new Exception();
                            note_rows_begin = int.Parse(note_row_bounds[0]);
                            note_rows_end = int.Parse(note_row_bounds[1]);
                            for (var i = note_rows_begin; i <= note_rows_end; i++)
                            {
                                note_rows_final_int.Add(i);
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }
                    else
                    {
                        try
                        {
                            note_rows_final_int.Add(int.Parse(note_row_cluster));
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
                foreach (var note_rownumber in note_rows_final_int)
                {
                    if (note_rownumber == line + 1 && note.GraphNumber_DB == 15.ToString())
                    {
                        if (note.Comment_DB != null && note.Comment_DB.Trim() != string.Empty)
                        {
                            valid = true;
                            break;
                        }
                    }
                }
                if (valid) break;
            }
        }
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

    private static List<CheckError> Check_040(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        short?[] DocumentVid_DB_Valids = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 19 };
        short?[] DocumentVid_DB_Valids_For_11 = { 9, 15 };
        var valid =
            forms[line].OperationCode_DB == "11" ?
                DocumentVid_DB_Valids_For_11.Contains(forms[line].DocumentVid_DB) :
                DocumentVid_DB_Valids.Contains(forms[line].DocumentVid_DB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentVid_DB",
                Value = forms[line].DocumentVid_DB.ToString(),
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_041(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var valid = !string.IsNullOrEmpty(forms[line].DocumentNumber_DB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentNumber_DB",
                Value = forms[line].DocumentNumber_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_042(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] ExcludedOperationCodes = { "10", "41" };
        if (ExcludedOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].DocumentDate_DB != null && forms[line].OperationDate_DB != null;
        if (valid)
        {
            DateTime DocumentDate_Real;
            DateTime OperationDate_Real;
            try
            {
                DocumentDate_Real = DateTime.Parse(forms[line].DocumentDate_DB!);
                OperationDate_Real = DateTime.Parse(forms[line].OperationDate_DB!);
                valid = DocumentDate_Real <= OperationDate_Real;
            }
            catch
            {
                valid = false;
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = forms[line].DocumentDate_DB,
                Message = "Дата документа не может быть позже даты операции."
            });
        }
        return result;
    }

    private static List<CheckError> Check_043(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "41" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].DocumentDate_DB != null && forms[line].OperationDate_DB != null;
        if (valid)
        {
            DateTime DocumentDate_Real;
            DateTime OperationDate_Real;
            try
            {
                DocumentDate_Real = DateTime.Parse(forms[line].DocumentDate_DB!);
                OperationDate_Real = DateTime.Parse(forms[line].OperationDate_DB!);
                valid = DocumentDate_Real.Date == OperationDate_Real.Date;
            }
            catch
            {
                valid = false;
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = forms[line].DocumentDate_DB,
                Message = "Дата документа должна соответствовать дате операции."
            });
        }
        return result;
    }

    private static List<CheckError> Check_044(List<Form11> forms, Report rep, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "10" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].DocumentDate_DB != null && forms[line].OperationDate_DB != null;
        if (valid)
        {
            DateTime DocumentDate_Real;
            var Date_Begin_Real = DateTime.MinValue;
            var Date_End_Real = DateTime.MinValue;
            try
            {
                valid = false;
                DocumentDate_Real = DateTime.Parse(forms[line].DocumentDate_DB!);
                {
                    if (rep is { StartPeriod_DB: not null, EndPeriod_DB: not null })
                    {
                        Date_Begin_Real = DateTime.Parse(rep.StartPeriod_DB);
                        Date_End_Real = DateTime.Parse(rep.EndPeriod_DB);
                    }
                }
                valid = Date_Begin_Real <= DocumentDate_Real && DocumentDate_Real <= Date_End_Real;
            }
            catch
            {
                valid = false;
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "DocumentDate_DB",
                Value = forms[line].DocumentDate_DB,
                Message = "Дата документа выходит за границы периода."
            });
        }
        return result;
    }

    private static List<CheckError> Check_045(List<Form11> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "10", "11", "12", "15", "17", "18", "41", "42", "43", "46", "53", "58", "61", "62", "65", "67", "68", "71", "72", "73", "74", "75", "76" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].ProviderOrRecieverOKPO_DB != null;
        if (valid)
        {
            valid = false;
            foreach (var org in forms10)
            {
                if (org.Okpo_DB == forms[line].ProviderOrRecieverOKPO_DB)
                {
                    valid = true;
                    break;
                }
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = forms[line].ProviderOrRecieverOKPO_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_046(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "21", "25", "27", "28", "29", "31", "35", "37", "38", "39", "54", "63", "64", "66" };
        char[] numerics = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '_' };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].ProviderOrRecieverOKPO_DB != null && (forms[line].ProviderOrRecieverOKPO_DB!.Length == 8 || forms[line].ProviderOrRecieverOKPO_DB!.Length == 14);
        if (valid)
        {
            foreach (var chr in forms[line].ProviderOrRecieverOKPO_DB!)
            {
                if (!numerics.Contains(chr))
                {
                    valid = false;
                    break;
                }
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = forms[line].ProviderOrRecieverOKPO_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_047(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "21", "25", "27", "28", "29", "31", "35", "37", "38", "39", "54", "63", "64", "66" };
        char[] numerics = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '_' };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].ProviderOrRecieverOKPO_DB != null;
        if (valid)
        {
            if (forms[line].ProviderOrRecieverOKPO_DB!.ToLower() == "минобороны")
            {
                return result;
            }
            else
            {
                valid = forms[line].ProviderOrRecieverOKPO_DB!.Length == 8 || forms[line].ProviderOrRecieverOKPO_DB!.Length == 14;
                if (valid)
                {
                    foreach (var chr in forms[line].ProviderOrRecieverOKPO_DB!)
                    {
                        if (!numerics.Contains(chr))
                        {
                            valid = false;
                            break;
                        }
                    }
                }
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = forms[line].ProviderOrRecieverOKPO_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_048(List<Form11> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "81", "82", "83", "84", "85", "86", "87", "88" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = false;
        foreach (var OKSM_Entry in OKSM)
        {
            if (OKSM_Entry["shortname"] == forms[line].ProviderOrRecieverOKPO_DB)
            {
                valid = true;
                break;
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = forms[line].ProviderOrRecieverOKPO_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        valid = false;
        List<int> note_rows_final_int;
        string note_rows_real;
        int note_rows_begin;
        int note_rows_end;
        List<string> note_rows_real_str;
        foreach (var note in notes)
        {
            if (note.RowNumber_DB != null && forms[line].ReportId != null)
            {
                note_rows_real = note.RowNumber_DB.Replace(" ", string.Empty);
                note_rows_final_int = new();
                note_rows_real_str = new(note_rows_real.Split(','));
                foreach (var note_row_cluster in note_rows_real_str)
                {
                    if (note_row_cluster.Contains('-'))
                    {
                        try
                        {
                            var note_row_bounds = note_row_cluster.Split('-');
                            if (note_row_bounds.Length != 2) throw new Exception();
                            note_rows_begin = int.Parse(note_row_bounds[0]);
                            note_rows_end = int.Parse(note_row_bounds[1]);
                            for (var i = note_rows_begin; i <= note_rows_end; i++)
                            {
                                note_rows_final_int.Add(i);
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }
                    else
                    {
                        try
                        {
                            note_rows_final_int.Add(int.Parse(note_row_cluster));
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
                foreach (var note_rownumber in note_rows_final_int)
                {
                    if (note_rownumber == line + 1 && note.GraphNumber_DB == 19.ToString())
                    {
                        if (note.Comment_DB != null && note.Comment_DB.Trim() != string.Empty)
                        {
                            valid = true;
                            break;
                        }
                    }
                }
                if (valid) break;
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = forms[line].ProviderOrRecieverOKPO_DB,
                Message = "Необходимо добавить примечание."
            });
        }

        return result;
    }

    private static List<CheckError> Check_049(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "97", "98", "99" };
        char[] numerics = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '_' };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].ProviderOrRecieverOKPO_DB != null && (forms[line].ProviderOrRecieverOKPO_DB!.Length == 8 || forms[line].ProviderOrRecieverOKPO_DB!.Length == 14);
        if (valid)
        {
            foreach (var chr in forms[line].ProviderOrRecieverOKPO_DB!)
            {
                if (!numerics.Contains(chr))
                {
                    valid = false;
                    break;
                }
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "ProviderOrRecieverOKPO_DB",
                Value = forms[line].ProviderOrRecieverOKPO_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_050(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "10", "11", "12", "15", "17", "18", "41", "42", "43", "46", "53", "54", "58", "65", "66", "67", "68", "71", "72", "73", "74", "75", "76", "97", "98" };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].TransporterOKPO_DB != null && forms[line].TransporterOKPO_DB == "-";
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = forms[line].TransporterOKPO_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_051(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "21", "25", "27", "28", "29", "31", "32", "35", "36", "37", "38", "39", "61", "62", "81", "82", "83", "84", "85", "86", "87", "88" };
        char[] numerics = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '_' };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].TransporterOKPO_DB != null && (forms[line].TransporterOKPO_DB!.Length == 8 || forms[line].TransporterOKPO_DB!.Length == 14);
        if (valid)
        {
            foreach (var chr in forms[line].TransporterOKPO_DB!)
            {
                if (!numerics.Contains(chr))
                {
                    valid = false;
                    break;
                }
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = forms[line].TransporterOKPO_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }
    private static List<CheckError> Check_052(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        string[] ApplicableOperationCodes = { "22", "32" };
        char[] numerics = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '_' };
        if (!ApplicableOperationCodes.Contains(forms[line].OperationCode_DB)) return result;
        var valid = forms[line].TransporterOKPO_DB != null;
        if (valid)
        {
            if (forms[line].TransporterOKPO_DB!.ToLower() == "минобороны")
            {
                return result;
            }
            else
            {
                valid = forms[line].TransporterOKPO_DB!.Length == 8 || forms[line].TransporterOKPO_DB!.Length == 14;
                if (valid)
                {
                    if (forms[line].TransporterOKPO_DB!.Any(chr => !numerics.Contains(chr)))
                    {
                        valid = false;
                    }
                }
            }
        }
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "TransporterOKPO_DB",
                Value = forms[line].TransporterOKPO_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_053(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var valid = !string.IsNullOrEmpty(forms[line].PackName_DB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PackName_DB",
                Value = forms[line].PackName_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_054(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var valid = !string.IsNullOrEmpty(forms[line].PackType_DB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PackType_DB",
                Value = forms[line].PackType_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    private static List<CheckError> Check_055(List<Form11> forms, int line)
    {
        List<CheckError> result = new();
        var valid = !string.IsNullOrEmpty(forms[line].PackNumber_DB);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "PackNumber_DB",
                Value = forms[line].PackNumber_DB,
                Message = "Формат ввода данных не соответствует приказу."
            });
        }
        return result;
    }

    #endregion
}