using Microsoft.EntityFrameworkCore;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Commands.SyncCommands.CheckForm;

/// <summary>
/// Проверка отчётов по форме 2.1. 
/// </summary>
public abstract class CheckF21 : CheckBase
{
    #region Properties

    private static readonly Dictionary<string, string> GraphsList = new()
    {
        { "NumberInOrder_DB", "01 - № п/п" },
        { "RefineMachineName_DB", "02 - Установки переработки - наименование" },
        { "MachineCode_DB", "03 - Установки перереботки - код" },
        { "MachinePower_DB", "04 - Установки переработки - мощность, куб.м/год" },
        { "NumberOfHoursPerYear_DB", "05 - Установки переработки - количество часов работы за год" },
        { "CodeRAOIn_DB", "06 - Поступило РАО - код" },
        { "StatusRAOIn_DB", "07 - Поступило РАО - статус" },
        { "VolumeIn_DB", "08 - Поступило РАО - количество - объём без упаковки, куб.м" },
        { "MassIn_DB", "09 - Поступило РАО - количество - масса без упаковки (нетто), т" },
        { "QuantityIn_DB", "10 - Поступило РАО - количество - ОЗИИИ, шт." },
        { "TritiumActivityIn_DB", "11 - Поступило РАО - суммарная активность, Бк - тритий" },
        { "BetaGammaActivityIn_DB", "12 - Поступило РАО - суммарная активность, Бк - бета-, гамма- излучающие радионуклиды (исключая трансурановые)" },
        { "AlphaActivityIn_DB", "13 - Поступило РАО - суммарная активность, Бк - альфа-излучающий радионуклиды (исключая трансурановые)" },
        { "TransuraniumActivityIn_DB", "14 - Поступило РАО - суммарная активность, Бк - трансурановые радионуклиды" },
        { "CodeRAOout_DB", "15 - Образовалось РАО - код" },
        { "StatusRAOout_DB", "16 - Образовалось РАО - статус" },
        { "VolumeOut_DB", "17 - Образовалось РАО - количество - объём без упаковки, куб.м" },
        { "MassOut_DB", "18 - Образовалось РАО - количество - масса без упаковки (нетто), т" },
        { "QuantityOZIIIout_DB", "19 - Образовалось РАО - количество - ОЗИИИ, шт." },
        { "TritiumActivityOut_DB", "20 - Образовалось РАО - суммарная активность, Бк - тритий" },
        { "BetaGammaActivityOut_DB", "21 - Образовалось РАО - суммарная активность, Бк - бета-, гамма- излучающие радионуклиды (исключая трансурановые)" },
        { "AlphaActivityOut_DB", "22 - Образовалось РАО - суммарная активность, Бк - альфа-излучающий радионуклиды (исключая трансурановые)" },
        { "TransuraniumActivityOut_DB", "23 - Образовалось РАО - суммарная активность, Бк - трансурановые радионуклиды" }
    };

    #endregion

    #region FormConvert

    public static Form21? FormConvert(Form15 form)
    {
        if (form.RefineOrSortRAOCode_DB.Length == 0) return null;
        if (form.RefineOrSortRAOCode_DB == "-" || string.IsNullOrWhiteSpace(form.RefineOrSortRAOCode_DB)) return null; //refine code doesn't exist
        if (form.RefineOrSortRAOCode_DB[0] == '7') return null; //7x refine codes are ignored
        Form21 res = new();
        res.FormNum_DB = form.FormNum_DB;
        res.NumberInOrder_DB = form.NumberInOrder_DB;
        if (form.OperationCode_DB == "44")
        {
            //left
            res.RefineMachineName_DB = "in";
            if (byte.TryParse(form.RefineOrSortRAOCode_DB, out byte MachineCode_DB)) res.MachineCode_DB = MachineCode_DB;
            res.CodeRAOIn_DB = "-";
            res.StatusRAOIn_DB = form.StatusRAO_DB;
            res.VolumeIn_DB = "-";
            res.MassIn_DB = "-";
            res.QuantityIn_DB = form.Quantity_DB.ToString();
            return res;
        }
        else if (form.OperationCode_DB == "56")
        {
            //right
            res.RefineMachineName_DB = "out";
            if (byte.TryParse(form.RefineOrSortRAOCode_DB, out byte MachineCode_DB)) res.MachineCode_DB = MachineCode_DB;
            res.CodeRAOout_DB = "-";
            res.StatusRAOout_DB = form.StatusRAO_DB;
            res.VolumeOut_DB = "-";
            res.MassOut_DB = "-";
            res.QuantityOZIIIout_DB = form.Quantity_DB.ToString();
            return res;
        }
        return null;
    }
    public static Form21? FormConvert(Form16 form)
    {
        if (form.RefineOrSortRAOCode_DB.Length == 0) return null;
        if (form.RefineOrSortRAOCode_DB == "-" || string.IsNullOrWhiteSpace(form.RefineOrSortRAOCode_DB)) return null; //refine code doesn't exist
        if (form.RefineOrSortRAOCode_DB[0] == '7') return null; //7x refine codes are ignored
        Form21 res = new();
        res.FormNum_DB = form.FormNum_DB;
        res.NumberInOrder_DB = form.NumberInOrder_DB;
        if (form.OperationCode_DB == "44")
        {
            //left
            res.RefineMachineName_DB = "in";
            if (byte.TryParse(form.RefineOrSortRAOCode_DB, out byte MachineCode_DB)) res.MachineCode_DB = MachineCode_DB;
            res.CodeRAOIn_DB = form.CodeRAO_DB;
            res.StatusRAOIn_DB = form.StatusRAO_DB;
            res.VolumeIn_DB = form.Volume_DB;
            res.MassIn_DB = form.Mass_DB;
            res.QuantityIn_DB = form.QuantityOZIII_DB;
            res.TritiumActivityIn_DB = form.TritiumActivity_DB;
            res.BetaGammaActivityIn_DB = form.BetaGammaActivity_DB;
            res.AlphaActivityIn_DB = form.AlphaActivity_DB;
            res.TransuraniumActivityIn_DB = form.TransuraniumActivity_DB;
            return res;
        }
        else if (form.OperationCode_DB == "56")
        {
            //right
            res.RefineMachineName_DB = "out";
            if (byte.TryParse(form.RefineOrSortRAOCode_DB, out byte MachineCode_DB)) res.MachineCode_DB = MachineCode_DB;
            res.CodeRAOout_DB = form.CodeRAO_DB;
            res.StatusRAOout_DB = form.StatusRAO_DB;
            res.VolumeOut_DB = form.Volume_DB;
            res.MassOut_DB = form.Mass_DB;
            res.QuantityOZIIIout_DB = form.QuantityOZIII_DB;
            res.TritiumActivityOut_DB = form.TritiumActivity_DB;
            res.BetaGammaActivityOut_DB = form.BetaGammaActivity_DB;
            res.AlphaActivityOut_DB = form.AlphaActivity_DB;
            res.TransuraniumActivityOut_DB = form.TransuraniumActivity_DB;
            return res;
        }
        return null;
    }
    public static Form21? FormConvert(Form17 form, Form17? form_header = null)
    {
        var form_true = form_header == null ? form : form_header;
        if (form_true.RefineOrSortRAOCode_DB.Length == 0) return null;
        if (form_true.RefineOrSortRAOCode_DB == "-" || string.IsNullOrWhiteSpace(form_true.RefineOrSortRAOCode_DB)) return null; //refine code doesn't exist
        if (form_true.RefineOrSortRAOCode_DB[0] == '7') return null; //7x refine codes are ignored
        if (form.CodeRAO_DB == "-" || string.IsNullOrWhiteSpace(form.CodeRAO_DB)) return null;
        Form21 res = new();
        res.FormNum_DB = form.FormNum_DB;
        res.NumberInOrder_DB = form.NumberInOrder_DB;
        if (form_true.OperationCode_DB == "44")
        {
            //left
            res.RefineMachineName_DB = "in";
            if (byte.TryParse(form.RefineOrSortRAOCode_DB, out byte MachineCode_DB)) res.MachineCode_DB = MachineCode_DB;
            res.CodeRAOIn_DB = form.CodeRAO_DB;
            res.StatusRAOIn_DB = form.StatusRAO_DB;
            res.VolumeIn_DB = form.VolumeOutOfPack_DB;
            res.MassIn_DB = form.MassOutOfPack_DB;
            res.QuantityIn_DB = form.Quantity_DB;
            res.TritiumActivityIn_DB = form.TritiumActivity_DB;
            res.BetaGammaActivityIn_DB = form.BetaGammaActivity_DB;
            res.AlphaActivityIn_DB = form.AlphaActivity_DB;
            res.TransuraniumActivityIn_DB = form.TransuraniumActivity_DB;
            return res;
        }
        else if (form_true.OperationCode_DB == "55")
        {
            //right
            res.RefineMachineName_DB = "out";
            if (byte.TryParse(form.RefineOrSortRAOCode_DB, out byte MachineCode_DB)) res.MachineCode_DB = MachineCode_DB;
            res.CodeRAOout_DB = form.CodeRAO_DB;
            res.StatusRAOout_DB = form.StatusRAO_DB;
            res.VolumeOut_DB = form.VolumeOutOfPack_DB;
            res.MassOut_DB = form.MassOutOfPack_DB;
            res.QuantityOZIIIout_DB = form.Quantity_DB;
            res.TritiumActivityOut_DB = form.TritiumActivity_DB;
            res.BetaGammaActivityOut_DB = form.BetaGammaActivity_DB;
            res.AlphaActivityOut_DB = form.AlphaActivity_DB;
            res.TransuraniumActivityOut_DB = form.TransuraniumActivity_DB;
            return res;
        }
        return null;
    }
    public static Form21? FormConvert(Form18 form, Form18? form_header = null)
    {
        var form_true = form_header == null ? form : form_header;
        if (form_true.RefineOrSortRAOCode_DB.Length == 0) return null;
        if (form_true.RefineOrSortRAOCode_DB == "-" || string.IsNullOrWhiteSpace(form_true.RefineOrSortRAOCode_DB)) return null; //refine code doesn't exist
        if (form_true.RefineOrSortRAOCode_DB[0] == '7') return null; //7x refine codes are ignored
        if (form.CodeRAO_DB == "-" || string.IsNullOrWhiteSpace(form.CodeRAO_DB)) return null;
        Form21 res = new();
        res.FormNum_DB = form.FormNum_DB;
        res.NumberInOrder_DB = form.NumberInOrder_DB;
        if (form_true.OperationCode_DB == "44")
        {
            //left
            res.RefineMachineName_DB = "in";
            if (byte.TryParse(form.RefineOrSortRAOCode_DB, out byte MachineCode_DB)) res.MachineCode_DB = MachineCode_DB;
            res.CodeRAOIn_DB = form.CodeRAO_DB;
            res.StatusRAOIn_DB = form.StatusRAO_DB;
            res.VolumeIn_DB = form.Volume20_DB;
            res.MassIn_DB = form.Mass21_DB;
            res.QuantityIn_DB = "1";
            res.TritiumActivityIn_DB = form.TritiumActivity_DB;
            res.BetaGammaActivityIn_DB = form.BetaGammaActivity_DB;
            res.AlphaActivityIn_DB = form.AlphaActivity_DB;
            res.TransuraniumActivityIn_DB = form.TransuraniumActivity_DB;
            return res;
        }
        else if (form_true.OperationCode_DB == "55")
        {
            //right
            res.RefineMachineName_DB = "out";
            if (byte.TryParse(form.RefineOrSortRAOCode_DB, out byte MachineCode_DB)) res.MachineCode_DB = MachineCode_DB;
            res.CodeRAOout_DB = form.CodeRAO_DB;
            res.StatusRAOout_DB = form.StatusRAO_DB;
            res.VolumeOut_DB = form.Volume20_DB;
            res.MassOut_DB = form.Mass21_DB;
            res.QuantityOZIIIout_DB = "1";
            res.TritiumActivityOut_DB = form.TritiumActivity_DB;
            res.BetaGammaActivityOut_DB = form.BetaGammaActivity_DB;
            res.AlphaActivityOut_DB = form.AlphaActivity_DB;
            res.TransuraniumActivityOut_DB = form.TransuraniumActivity_DB;
            return res;
        }
        return null;
    }

    #endregion

    #region Form21_Copy

    public static Form21 Form21_Copy(Form21 form, string? in_or_out_param = null)
    {
        Form21 res = new();
        res.NumberInOrder_DB = form.NumberInOrder_DB;
        res.RefineMachineName_DB = form.RefineMachineName_DB;
        res.MachineCode_DB = form.MachineCode_DB;
        res.FormNum_DB = form.FormNum_DB;
        string in_or_out = in_or_out_param ?? res.RefineMachineName_DB;
        if (!(in_or_out is "in" or "out")) in_or_out = res.RefineMachineName_DB;
        if (in_or_out == "in")
        {
            res.CodeRAOIn_DB = form.CodeRAOIn_DB;
            res.StatusRAOIn_DB = form.StatusRAOIn_DB;
            res.VolumeIn_DB = form.VolumeIn_DB;
            res.MassIn_DB = form.MassIn_DB;
            res.QuantityIn_DB = form.QuantityIn_DB;
            res.TritiumActivityIn_DB = form.TritiumActivityIn_DB;
            res.BetaGammaActivityIn_DB = form.BetaGammaActivityIn_DB;
            res.AlphaActivityIn_DB = form.AlphaActivityIn_DB;
            res.TransuraniumActivityIn_DB = form.TransuraniumActivityIn_DB;
            res.QuantityOZIIIout_DB = form.QuantityOZIIIout_DB;
        }
        else
        {
            res.CodeRAOout_DB = form.CodeRAOout_DB;
            res.StatusRAOout_DB = form.StatusRAOout_DB;
            res.VolumeOut_DB = form.VolumeOut_DB;
            res.MassOut_DB = form.MassOut_DB;
            res.QuantityOZIIIout_DB = form.QuantityOZIIIout_DB;
            res.TritiumActivityOut_DB = form.TritiumActivityOut_DB;
            res.BetaGammaActivityOut_DB = form.BetaGammaActivityOut_DB;
            res.AlphaActivityOut_DB = form.AlphaActivityOut_DB;
            res.TransuraniumActivityOut_DB = form.TransuraniumActivityOut_DB;
            res.QuantityIn_DB = form.QuantityIn_DB;
        }
        return res;
    }
    public static Form21 Form21_Copy_In(Form21 form, string? in_or_out_param = null)
    {
        Form21 res = new();
        res.NumberInOrder_DB = form.NumberInOrder_DB;
        res.RefineMachineName_DB = form.RefineMachineName_DB;
        res.MachineCode_DB = form.MachineCode_DB;
        res.FormNum_DB = form.FormNum_DB;
        res.CodeRAOIn_DB = form.CodeRAOIn_DB;
        res.StatusRAOIn_DB = form.StatusRAOIn_DB;
        res.VolumeIn_DB = form.VolumeIn_DB;
        res.MassIn_DB = form.MassIn_DB;
        res.QuantityIn_DB = form.QuantityIn_DB;
        res.TritiumActivityIn_DB = form.TritiumActivityIn_DB;
        res.BetaGammaActivityIn_DB = form.BetaGammaActivityIn_DB;
        res.AlphaActivityIn_DB = form.AlphaActivityIn_DB;
        res.TransuraniumActivityIn_DB = form.TransuraniumActivityIn_DB;
        res.QuantityOZIIIout_DB = form.QuantityOZIIIout_DB;
        return res;
    }
    public static Form21 Form21_Copy_Out(Form21 form, string? in_or_out_param = null)
    {
        Form21 res = new();
        res.NumberInOrder_DB = form.NumberInOrder_DB;
        res.RefineMachineName_DB = form.RefineMachineName_DB;
        res.MachineCode_DB = form.MachineCode_DB;
        res.FormNum_DB = form.FormNum_DB;
        res.CodeRAOout_DB = form.CodeRAOout_DB;
        res.StatusRAOout_DB = form.StatusRAOout_DB;
        res.VolumeOut_DB = form.VolumeOut_DB;
        res.MassOut_DB = form.MassOut_DB;
        res.QuantityOZIIIout_DB = form.QuantityOZIIIout_DB;
        res.TritiumActivityOut_DB = form.TritiumActivityOut_DB;
        res.BetaGammaActivityOut_DB = form.BetaGammaActivityOut_DB;
        res.AlphaActivityOut_DB = form.AlphaActivityOut_DB;
        res.TransuraniumActivityOut_DB = form.TransuraniumActivityOut_DB;
        res.QuantityIn_DB = form.QuantityIn_DB;
        return res;
    }

    #endregion

    #region Form21_Add

    public static void Form21_Add(Form21 receiver, Form21 giver, string? direction = null)
    {
        string direction_real = direction ?? receiver.RefineMachineName_DB;
        if (direction_real == "in")
        {
            receiver.VolumeIn_DB = Form21_SubAdd(receiver.VolumeIn_DB, giver.VolumeIn_DB);
            receiver.MassIn_DB = Form21_SubAdd(receiver.MassIn_DB, giver.MassIn_DB);
            receiver.QuantityIn_DB = Form21_SubAdd(receiver.QuantityIn_DB, giver.QuantityIn_DB);
            receiver.TritiumActivityIn_DB = Form21_SubAdd(receiver.TritiumActivityIn_DB, giver.TritiumActivityIn_DB);
            receiver.BetaGammaActivityIn_DB = Form21_SubAdd(receiver.BetaGammaActivityIn_DB, giver.BetaGammaActivityIn_DB);
            receiver.AlphaActivityIn_DB = Form21_SubAdd(receiver.AlphaActivityIn_DB, giver.AlphaActivityIn_DB);
            receiver.TransuraniumActivityIn_DB = Form21_SubAdd(receiver.TransuraniumActivityIn_DB, giver.TransuraniumActivityIn_DB);
            
        }
        else if (direction_real == "out")
        {
            receiver.VolumeOut_DB = Form21_SubAdd(receiver.VolumeOut_DB, giver.VolumeOut_DB);
            receiver.MassOut_DB = Form21_SubAdd(receiver.MassOut_DB, giver.MassOut_DB);
            receiver.QuantityOZIIIout_DB = Form21_SubAdd(receiver.QuantityOZIIIout_DB, giver.QuantityOZIIIout_DB);
            receiver.TritiumActivityOut_DB = Form21_SubAdd(receiver.TritiumActivityOut_DB, giver.TritiumActivityOut_DB);
            receiver.BetaGammaActivityOut_DB = Form21_SubAdd(receiver.BetaGammaActivityOut_DB, giver.BetaGammaActivityOut_DB);
            receiver.AlphaActivityOut_DB = Form21_SubAdd(receiver.AlphaActivityOut_DB, giver.AlphaActivityOut_DB);
            receiver.TransuraniumActivityOut_DB = Form21_SubAdd(receiver.TransuraniumActivityOut_DB, giver.TransuraniumActivityOut_DB);

        }
    }
    /// <summary>
    /// Tries parsing the two parameters and assigning their sum to the return value, converting it back to string afterwards. If the parsing fails, returns the first parameter as is.
    /// </summary>
    /// <param name="receiver">The first parameter</param>
    /// <param name="giver">The second parameter</param>
    /// <returns>A string representation of the sum of the parameters. If the summation fails, returns the first parameter.</returns>
    public static string Form21_SubAdd(string receiver, string giver)
    {
        string res = receiver;
        string receiver_real = (receiver == "-" || string.IsNullOrWhiteSpace(receiver)) ? "0" : receiver;
        string giver_real = (giver == "-" || string.IsNullOrWhiteSpace(giver)) ? "0" : giver;
        if (TryParseDoubleExtended(receiver_real, out var receiver_true) && TryParseDoubleExtended(giver_real, out var giver_true))
        {
            res = (receiver_true + giver_true).ToString("e2").Replace("+0","+");
        }
        return res;
    }

    #endregion

    #region Form21_Submatch
    public static void Form21_Submatch(string form1_val, string form2_val, string humanname, double valb, List<(int, string, string, string)> res, int column_num, string forms1, string forms2)
    {
        TryParseDoubleExtended(form1_val, out double val1);
        TryParseDoubleExtended(form2_val, out double val2);
        if (!((form1_val == "-" && form2_val == "-") || (val1 < 0.001 && form2_val == "-") || (form1_val == "-" && val2 < 0.001) || val1 >= val2 * (1.0 - valb) && val2 >= val1 * (1.0 - valb)))
        {
            res.Add((column_num, $"{humanname}", $"{forms1}: {form1_val}",$"{forms2}: {form2_val}"));
        }
    }
    #endregion

    #region Form21_Match

    public static List<(int,string,string,string)>? Form21_Match(Form21 form1, Form21 form2, string forms1, string forms2)
    {
        double valb = 0.1;
        List<(int, string, string, string)> res = new();
        if (form1.CodeRAOIn_DB == form2.CodeRAOIn_DB && form1.CodeRAOIn_DB != "-" && !string.IsNullOrWhiteSpace(form1.CodeRAOIn_DB))
        {
            if (form1.StatusRAOIn_DB == form2.StatusRAOIn_DB
                && form1.CodeRAOIn_DB == form2.CodeRAOIn_DB
                && form1.MachineCode_DB == form2.MachineCode_DB)
            {
                Form21_Submatch(form1.VolumeIn_DB, form2.VolumeIn_DB, "Объем без упаковки, куб. м", valb, res, 8, forms1, forms2);
                Form21_Submatch(form1.MassIn_DB, form2.MassIn_DB, "Масса без упаковки (нетто), т", valb, res, 9, forms1, forms2);
                Form21_Submatch(form1.QuantityIn_DB, form2.QuantityIn_DB, "кол-во ОЗИИИ, шт.", valb, res, 10, forms1, forms2);
                Form21_Submatch(form1.TritiumActivityIn_DB, form2.TritiumActivityIn_DB, "суммарная активность (тритий), Бк", valb, res, 11, forms1, forms2);
                Form21_Submatch(form1.BetaGammaActivityIn_DB, form2.BetaGammaActivityIn_DB, "суммарная активность (бета, гамма), Бк", valb, res, 12, forms1, forms2);
                Form21_Submatch(form1.AlphaActivityIn_DB, form2.AlphaActivityIn_DB, "суммарная активность (альфа), Бк", valb, res, 13, forms1, forms2);
                Form21_Submatch(form1.TransuraniumActivityIn_DB, form2.TransuraniumActivityIn_DB, "суммарная активность (трансурановые), Бк", valb, res, 14, forms1, forms2);
                return res;
            }
            else return null;
        }
        else if (form1.CodeRAOout_DB == form2.CodeRAOout_DB && form1.CodeRAOout_DB != "-" && !string.IsNullOrWhiteSpace(form1.CodeRAOout_DB))
        {
            if (form1.StatusRAOout_DB == form2.StatusRAOout_DB
                && form1.CodeRAOout_DB == form2.CodeRAOout_DB
                && form1.MachineCode_DB == form2.MachineCode_DB)
            {
                Form21_Submatch(form1.VolumeOut_DB, form2.VolumeOut_DB, "Объем без упаковки, куб. м", valb, res, 17, forms1, forms2);
                Form21_Submatch(form1.MassOut_DB, form2.MassOut_DB, "Масса без упаковки (нетто), т", valb, res, 18, forms1, forms2);
                Form21_Submatch(form1.QuantityOZIIIout_DB, form2.QuantityOZIIIout_DB, "кол-во ОЗИИИ, шт.", valb, res, 19, forms1, forms2);
                Form21_Submatch(form1.TritiumActivityOut_DB, form2.TritiumActivityOut_DB, "суммарная активность (тритий), Бк", valb, res, 20, forms1, forms2);
                Form21_Submatch(form1.BetaGammaActivityOut_DB, form2.BetaGammaActivityOut_DB, "суммарная активность (бета, гамма), Бк", valb, res, 21, forms1, forms2);
                Form21_Submatch(form1.AlphaActivityOut_DB, form2.AlphaActivityOut_DB, "суммарная активность (альфа), Бк", valb, res, 22, forms1, forms2);
                Form21_Submatch(form1.TransuraniumActivityOut_DB, form2.TransuraniumActivityOut_DB, "суммарная активность (трансурановые), Бк", valb, res, 23, forms1, forms2);
                return res;
            }
            else return null;
        }
        return null;
    }

    #endregion

    #region CheckTotal

    public static List<CheckError> Check_Total(DBModel db, Report? rep)
    {
        List<CheckError> errorList = new();
        if (rep == null) return errorList;
        int form20_id = !string.IsNullOrWhiteSpace(rep.Reports.Master_DB.Rows20[0].RegNo_DB) ? 0
            : (!string.IsNullOrWhiteSpace(rep.Reports.Master_DB.Rows20[1].RegNo_DB) ? 1
            : -1);
        if (form20_id == -1) return errorList;
        db.form_20.Where(x => x.RegNo_DB == rep.Reports.Master_DB.Rows20[form20_id].RegNo_DB).Load();   //load forms 2.0 that correspond to the selected report
        db.form_10.Where(x => x.RegNo_DB == rep.Reports.Master_DB.Rows20[form20_id].RegNo_DB).Load();   //load forms 1.0 that correspond to the selected report
        var form10 = db.ReportCollectionDbSet.Where(x => x.FormNum_DB == "1.0").ToList().SingleOrDefault(x => x.Rows10.Count > 0);    //isolate form 1.0 that matches our form 2.0
        if (form10 == null) return errorList;
        db.ReportsCollectionDbSet.Where(x => x.Master_DBId == form10.Id).Load();    //load the report collection that corresponds to the isolated form 1.0
        var forms1 = db.ReportsCollectionDbSet.SingleOrDefault(x => x.Master_DBId == form10.Id);    //isolate this report collection
        if (forms1 == null) return errorList;
        db.ReportCollectionDbSet.Where(x => x.Reports.Master_DBId == forms1.Master_DBId
        && (x.FormNum_DB == "1.5" || x.FormNum_DB == "1.6" || x.FormNum_DB == "1.7" || x.FormNum_DB == "1.8")
        && (x.StartPeriod_DB.Length>=4 && x.StartPeriod_DB.Substring(x.StartPeriod_DB.Length - 4) == rep.Year_DB || x.EndPeriod_DB.Length >= 4 && x.EndPeriod_DB.Substring(x.EndPeriod_DB.Length - 4) == rep.Year_DB)).Load(); //load reports themselves
        foreach (var report in forms1.Report_Collection)
        {
            db.form_15.Where(x => x.ReportId == report.Id).Load();  //load rows for reports 1.5
            db.form_16.Where(x => x.ReportId == report.Id).Load();  //load rows for reports 1.6
            db.form_17.Where(x => x.ReportId == report.Id).Load();  //load rows for reports 1.7
            db.form_18.Where(x => x.ReportId == report.Id).Load();  //load rows for reports 1.8
        }
        //at this point forms1 contains everything that we need to convert.
        List<Form21> forms21_expected_base = new();
        List<(string, string, string, string)> forms21_metadata_base = new();
        Form21? form21_new = null;
        Form17? form_header17 = null;
        Form18? form_header18 = null;
        foreach (Report report in forms1.Report_Collection)
        {
            switch (report.FormNum_DB)
            {
                case "1.5":
                    foreach (Form15 form in report.Rows15)
                    {
                        form21_new = FormConvert(form);
                        if (form21_new != null)
                        {
                            forms21_metadata_base.Add((form21_new.FormNum_DB,report.StartPeriod_DB,report.EndPeriod_DB,form21_new.NumberInOrder_DB.ToString()));
                            forms21_expected_base.Add(form21_new);
                        }
                    }
                    break;
                case "1.6":
                    foreach (Form16 form in report.Rows16)
                    {
                        form21_new = FormConvert(form);
                        if (form21_new != null)
                        {
                            forms21_metadata_base.Add((form21_new.FormNum_DB, report.StartPeriod_DB, report.EndPeriod_DB, form21_new.NumberInOrder_DB.ToString()));
                            forms21_expected_base.Add(form21_new);
                        }
                    }
                    break;
                case "1.7":
                    foreach (Form17 form in report.Rows17)
                    {
                        if (form.OperationCode_DB != "-") form_header17 = form;
                        form21_new = FormConvert(form, form_header17);
                        if (form21_new != null)
                        {
                            forms21_metadata_base.Add((form21_new.FormNum_DB, report.StartPeriod_DB, report.EndPeriod_DB, form21_new.NumberInOrder_DB.ToString()));
                            forms21_expected_base.Add(form21_new);
                        }
                    }
                    break;
                case "1.8":
                    foreach (Form18 form in report.Rows18)
                    {
                        if (form.OperationCode_DB != "-") form_header18 = form;
                        form21_new = FormConvert(form, form_header18);
                        if (form21_new != null)
                        {
                            forms21_metadata_base.Add((form21_new.FormNum_DB, report.StartPeriod_DB, report.EndPeriod_DB, form21_new.NumberInOrder_DB.ToString()));
                            forms21_expected_base.Add(form21_new);
                        }
                    }
                    break;
            }
        }
        Dictionary<(byte?, string, string), Form21> forms21_expected_in_dict = new();
        Dictionary<(byte?, string, string), Form21> forms21_expected_out_dict = new();
        Dictionary<(byte?, string, string), Form21> forms21_real_in_dict = new();
        Dictionary<(byte?, string, string), Form21> forms21_real_out_dict = new();
        Dictionary<(byte?, string, string), Dictionary<string, Dictionary<string, List<string>>>> forms21_metadata_in_dict = new();
        Dictionary<(byte?, string, string), Dictionary<string, Dictionary<string, List<string>>>> forms21_metadata_out_dict = new();
        for (var i = 0; i < forms21_expected_base.Count; i++) {
            if (forms21_expected_base[i].RefineMachineName_DB == "in")
            {
                var key = (forms21_expected_base[i].MachineCode_DB, forms21_expected_base[i].CodeRAOIn_DB, forms21_expected_base[i].StatusRAOIn_DB);
                if (!forms21_expected_in_dict.ContainsKey(key))
                {
                    forms21_expected_in_dict[key] = Form21_Copy(forms21_expected_base[i]);
                }
                else
                {
                    Form21_Add(forms21_expected_in_dict[key], forms21_expected_base[i]);
                }
                if (!forms21_metadata_in_dict.ContainsKey(key))
                {
                    forms21_metadata_in_dict[key] = new();
                }
                if (!forms21_metadata_in_dict[key].ContainsKey(forms21_metadata_base[i].Item1))
                {
                    forms21_metadata_in_dict[key][forms21_metadata_base[i].Item1] = new();
                }
                if (!forms21_metadata_in_dict[key][forms21_metadata_base[i].Item1].ContainsKey($"{forms21_metadata_base[i].Item2} - {forms21_metadata_base[i].Item3}"))
                {
                    forms21_metadata_in_dict[key][forms21_metadata_base[i].Item1][$"{forms21_metadata_base[i].Item2} - {forms21_metadata_base[i].Item3}"] = new();
                }
                forms21_metadata_in_dict[key][forms21_metadata_base[i].Item1][$"{forms21_metadata_base[i].Item2} - {forms21_metadata_base[i].Item3}"].Add(forms21_metadata_base[i].Item4);
            }
            else if (forms21_expected_base[i].RefineMachineName_DB == "out")
            {
                var key = (forms21_expected_base[i].MachineCode_DB, forms21_expected_base[i].CodeRAOout_DB, forms21_expected_base[i].StatusRAOout_DB);
                if (!forms21_expected_out_dict.ContainsKey(key))
                {
                    forms21_expected_out_dict[key] = Form21_Copy(forms21_expected_base[i]);
                }
                else
                {
                    Form21_Add(forms21_expected_out_dict[key], forms21_expected_base[i]);
                }
                if (!forms21_metadata_out_dict.ContainsKey(key))
                {
                    forms21_metadata_out_dict[key] = new();
                }
                if (!forms21_metadata_out_dict[key].ContainsKey(forms21_metadata_base[i].Item1))
                {
                    forms21_metadata_out_dict[key][forms21_metadata_base[i].Item1] = new();
                }
                if (!forms21_metadata_out_dict[key][forms21_metadata_base[i].Item1].ContainsKey($"{forms21_metadata_base[i].Item2} - {forms21_metadata_base[i].Item3}"))
                {
                    forms21_metadata_out_dict[key][forms21_metadata_base[i].Item1][$"{forms21_metadata_base[i].Item2} - {forms21_metadata_base[i].Item3}"] = new();
                }
                forms21_metadata_out_dict[key][forms21_metadata_base[i].Item1][$"{forms21_metadata_base[i].Item2} - {forms21_metadata_base[i].Item3}"].Add(forms21_metadata_base[i].Item4);
            }
        }
        List<Form21> forms21_real_in = new();
        List<Form21> forms21_real_out = new();
        byte? MachineCode_Header = null;
        foreach (Form21 form in rep.Rows21)
        {
            if (form.MachineCode_DB != null) MachineCode_Header = form.MachineCode_DB;
            if (form.CodeRAOIn_DB != "-" && !string.IsNullOrWhiteSpace(form.CodeRAOIn_DB))
            {
                var key = (MachineCode_Header, form.CodeRAOIn_DB, form.StatusRAOIn_DB);
                if (!forms21_real_in_dict.ContainsKey(key))
                {
                    forms21_real_in_dict[key] = Form21_Copy_In(form);
                    forms21_real_in_dict[key].MachineCode_DB = MachineCode_Header;
                }
                else
                {
                    Form21_Add(forms21_real_in_dict[key], form, "in");
                    errorList.Add(new CheckError
                    {
                        FormNum = "form_21",
                        Row = form.NumberInOrder_DB.ToString(),
                        Column = "6 - 14",
                        Value = $"код РАО {form.CodeRAOIn_DB}, статус РАО {form.StatusRAOIn_DB}, код переработки/сортировки {MachineCode_Header}",
                        Message = $"В форме 2.1 уже присутствует строка с указанными РАО, поступившими на переработку/кондиционирование (строка {forms21_real_in_dict[key].NumberInOrder_DB})."
                    });
                }
            }
            if (form.CodeRAOout_DB != "-" && !string.IsNullOrWhiteSpace(form.CodeRAOout_DB))
            {
                var key = (MachineCode_Header, form.CodeRAOout_DB, form.StatusRAOout_DB);
                if (!forms21_real_out_dict.ContainsKey(key))
                {
                    forms21_real_out_dict[key] = Form21_Copy_Out(form);
                    forms21_real_out_dict[key].MachineCode_DB = MachineCode_Header;
                }
                else
                {
                    Form21_Add(forms21_real_out_dict[key], form, "out");
                    errorList.Add(new CheckError
                    {
                        FormNum = "form_21",
                        Row = form.NumberInOrder_DB.ToString(),
                        Column = "15 - 23",
                        Value = $"код РАО {form.CodeRAOout_DB}, статус РАО {form.StatusRAOout_DB}, код переработки/сортировки {MachineCode_Header}",
                        Message = $"В форме 2.1 уже присутствует строка с указанными РАО, образовавшихся после переработки/кондиционирования (строка {forms21_real_out_dict[key].NumberInOrder_DB})."
                    });
                }
            }
        }
        foreach (var key in forms21_real_in_dict.Keys)
        {
            forms21_real_in.Add(forms21_real_in_dict[key]);
        }
        foreach (var key in forms21_real_out_dict.Keys)
        {
            forms21_real_out.Add(forms21_real_out_dict[key]);
        }
        //the converted values should be compared to the rows in reps.
        List<(Form21,string,string)> forms21_expected_in = new();
        List<(Form21,string,string)> forms21_expected_out = new();
        foreach (var key in forms21_expected_in_dict.Keys)
        {
            string address_string = "";
            List<string> address_substrings = new();
            string address_substring = "";
            string forms_string = "";
            List<string> forms_substrings = new();
            foreach (var key_form in forms21_metadata_in_dict[key].Keys)
            {
                address_substring = "";
                address_substring += $"форма {key_form}: \r\n";
                forms_substrings.Add(key_form);
                List<string> periods = forms21_metadata_in_dict[key][key_form].Keys.ToList();
                for (int i = 0; i < periods.Count; i++)
                    periods[i] = $"{periods[i].Substring(6, 4)}.{periods[i].Substring(3, 2)}.{periods[i].Substring(0, 2)}{periods[i].Substring(10)}";
                periods.Sort();
                for (int i = 0; i < periods.Count; i++)
                    periods[i] = $"{periods[i].Substring(8, 2)}.{periods[i].Substring(5, 2)}.{periods[i].Substring(0, 4)}{periods[i].Substring(10)}";
                foreach (var key_period in periods)
                {
                    address_substring += $"отчёт {key_period}: ";
                    List<string> lines = forms21_metadata_in_dict[key][key_form][key_period].ToList();
                    List<int> lines_real = new();
                    foreach (string line in lines) lines_real.Add(int.Parse(line));
                    lines_real.Sort();
                    for (int i = lines_real.Count - 1; i > 1; i--)
                    {
                        if (lines_real[i] == lines_real[i-1]+1 && (lines_real[i - 1] == lines_real[i - 2] + 1
                            || i<lines_real.Count-1 && lines_real[i+1] == -1))
                        {
                            lines_real[i] = -1;
                        }
                    }
                    if (lines_real.Count >= 3 && lines_real[1] == lines_real[0] + 1 && lines_real[2] == -1) lines_real[1] = -1;
                    lines = new();
                    int line_range = 0;
                    foreach (int line in lines_real)
                    {
                        if (line == -1) line_range++;
                        else
                        {
                            if (line_range > 0) lines[^1] = $"{lines[^1]} - {int.Parse(lines[^1])+line_range}";
                            lines.Add(line.ToString());
                            line_range = 0;
                        }
                    }
                    if (line_range > 1) lines[^1] = $"{lines[^1]} - {int.Parse(lines[^1]) + line_range}";
                    else if (line_range > 0) lines[^1] = $"{lines[^1]}, {int.Parse(lines[^1]) + line_range}";
                    address_substring += $"строк{(lines_real.Count == 1 ? "а" : "и")} {string.Join(", ", lines)} \r\n";
                }
                address_substrings.Add(address_substring);
            }
            address_string = string.Join("; \r\n", address_substrings);
            forms_string = string.Join(", ", forms_substrings);
            forms21_expected_in.Add((forms21_expected_in_dict[key], address_string, forms_string));
        }
        foreach (var key in forms21_expected_out_dict.Keys)
        {
            string address_string = "";
            List<string> address_substrings = new();
            string address_substring = "";
            string forms_string = "";
            List<string> forms_substrings = new();
            foreach (var key_form in forms21_metadata_out_dict[key].Keys)
            {
                address_substring = "";
                address_substring += $"форма {key_form}: \r\n";
                forms_substrings.Add(key_form);
                List<string> periods = forms21_metadata_out_dict[key][key_form].Keys.ToList();
                for (int i = 0; i < periods.Count; i++)
                    periods[i] = $"{periods[i].Substring(6, 4)}.{periods[i].Substring(3, 2)}.{periods[i].Substring(0, 2)}{periods[i].Substring(10)}";
                periods.Sort();
                for (int i = 0; i < periods.Count; i++)
                    periods[i] = $"{periods[i].Substring(8, 2)}.{periods[i].Substring(5, 2)}.{periods[i].Substring(0, 4)}{periods[i].Substring(10)}";
                foreach (var key_period in periods)
                {
                    address_substring += $"отчёт {key_period}: ";
                    List<string> lines = forms21_metadata_out_dict[key][key_form][key_period].ToList();
                    List<int> lines_real = new();
                    foreach (string line in lines) lines_real.Add(int.Parse(line));
                    lines_real.Sort();
                    for (int i = lines_real.Count - 1; i > 1; i--)
                    {
                        if (lines_real[i] == lines_real[i - 1] + 1 && (lines_real[i - 1] == lines_real[i - 2] + 1
                            || i < lines_real.Count - 1 && lines_real[i + 1] == -1))
                        {
                            lines_real[i] = -1;
                        }
                    }
                    if (lines_real.Count >= 3 && lines_real[1] == lines_real[0] + 1 && lines_real[2] == -1) lines_real[1] = -1;
                    lines = new();
                    int line_range = 0;
                    foreach (int line in lines_real)
                    {
                        if (line == -1) line_range++;
                        else
                        {
                            if (line_range>0) lines[^1] = $"{lines[^1]} - {int.Parse(lines[^1]) + line_range}";
                            lines.Add(line.ToString());
                            line_range = 0;
                        }
                    }
                    if (line_range > 1) lines[^1] = $"{lines[^1]} - {int.Parse(lines[^1]) + line_range}";
                    else if (line_range > 0) lines[^1] = $"{lines[^1]}, {int.Parse(lines[^1]) + line_range}";
                    address_substring += $"строк{(lines_real.Count == 1 ? "а" : "и")} {string.Join(", ", lines)} \r\n";
                }
                address_substrings.Add(address_substring);
            }
            address_string = string.Join("; \r\n", address_substrings);
            forms_string = string.Join(", ", forms_substrings);
            forms21_expected_out.Add((forms21_expected_out_dict[key], address_string, forms_string));
        }
        foreach (Form21 form_real in forms21_real_in)
        {
            bool match_found = false;
            foreach ((Form21,string,string) form_expected in forms21_expected_in)
            {
                List<(int,string,string,string)>? mismatches = Form21_Match(form_expected.Item1, form_real, $"форм{(form_expected.Item3.Contains(",")?"ы":"а")} {form_expected.Item3}", "форма 2.1");
                if (mismatches != null)
                {
                    forms21_expected_in.Remove(form_expected);
                    match_found = true;
                    if (mismatches.Count > 0)
                    {
                        List<int> list_columns = new();
                        List<string> list_hints = new();
                        foreach ((int, string, string, string) mismatch in mismatches)
                        {
                            list_columns.Add(mismatch.Item1);
                            list_hints.Add($"{mismatch.Item2}: {mismatch.Item3}, {mismatch.Item4}");
                        }
                        string columns = string.Join(", ", list_columns);
                        string hints = string.Join(";\n", list_hints);
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_21",
                            Row = form_real.NumberInOrder_DB.ToString(),
                            Column = columns,
                            Value = $"код РАО {form_real.CodeRAOIn_DB}, статус РАО {form_real.StatusRAOIn_DB}, код переработки/сортировки {form_real.MachineCode_DB}\n - {form_expected.Item2}",
                            Message = $"Сведения о РАО, поступивших на переработку, не совпадают:\n\n{hints}"
                        });
                    }
                    break;
                }
            }
            if (!match_found)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_21",
                    Row = form_real.NumberInOrder_DB.ToString(),
                    Column = "6 - 14",
                    Value = $"код РАО {form_real.CodeRAOIn_DB}, статус РАО {form_real.StatusRAOIn_DB}, код переработки/сортировки {form_real.MachineCode_DB}",
                    Message = "В формах 1.5 - 1.8 не найдена инфорация об указанных РАО, поступивших на переработку/кондиционирование."
                });
            }
        }
        foreach (Form21 form_real in forms21_real_out)
        {
            bool match_found = false;
            foreach ((Form21,string,string) form_expected in forms21_expected_out)
            {
                List<(int,string,string,string)>? mismatches = Form21_Match(form_expected.Item1, form_real, $"форм{(form_expected.Item3.Contains(",") ? "ы" : "а")} {form_expected.Item3}", "форма 2.1");
                if (mismatches != null)
                {
                    forms21_expected_out.Remove(form_expected);
                    match_found = true;
                    if (mismatches.Count > 0)
                    {
                        List<int> list_columns = new();
                        List<string> list_hints = new();
                        foreach ((int, string, string, string) mismatch in mismatches)
                        {
                            list_columns.Add(mismatch.Item1);
                            list_hints.Add($"{mismatch.Item2}: {mismatch.Item3}, {mismatch.Item4}");
                        }
                        string columns = string.Join(", ", list_columns);
                        string hints = string.Join(";\n", list_hints);
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_21",
                            Row = form_real.NumberInOrder_DB.ToString(),
                            Column = columns,
                            Value = $"код РАО {form_real.CodeRAOout_DB}, статус РАО {form_real.StatusRAOout_DB}, код переработки/сортировки {form_real.MachineCode_DB}\n - {form_expected.Item2}",
                            Message = $"Сведения о РАО, образовавшихся после переработки, не совпадают:\n\n{hints}"
                        });
                    }
                    break;
                }
            }
            if (!match_found)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_21",
                    Row = form_real.NumberInOrder_DB.ToString(),
                    Column = "15 - 23",
                    Value = $"код РАО {form_real.CodeRAOout_DB}, статус РАО {form_real.StatusRAOout_DB}, код переработки/сортировки {form_real.MachineCode_DB}",
                    Message = "В формах 1.5 - 1.8 не найдена информация об указанных РАО, образовавшихся после переработки/кондиционирования."
                });
            }
        }
        foreach ((Form21,string,string) form_expected in forms21_expected_in)
        {
            errorList.Add(new CheckError
            {
                FormNum = "form_21",
                Row = "-",
                Column = "6 - 14",
                Value = $"код РАО {form_expected.Item1.CodeRAOIn_DB}, статус РАО {form_expected.Item1.StatusRAOIn_DB}, код переработки/сортировки {form_expected.Item1.MachineCode_DB}\n - {form_expected.Item2}",
                Message = "В форме 2.1 не найдена информация об указанных РАО, поступивших на переработку/кондиционирование."
            });
        }
        foreach ((Form21,string,string) form_expected in forms21_expected_out)
        {
            errorList.Add(new CheckError
            {
                FormNum = "form_21",
                Row = "-",
                Column = "15 - 23",
                Value = $"код РАО {form_expected.Item1.CodeRAOout_DB}, статус РАО {form_expected.Item1.StatusRAOout_DB}, код переработки/сортировки {form_expected.Item1.MachineCode_DB}\n - {form_expected.Item2}",
                Message = "В форме 2.1 не найдена информация об указанных РАО, образовавшихся после переработки/кондиционирования."
            });
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
        errorList.Sort((i,j)=>int.TryParse(i.Row,out int i_row_real) && int.TryParse(j.Row,out int j_row_real)?i_row_real-j_row_real:string.Compare(i.Row,j.Row));
        return errorList;
    }
    public static List<CheckError> Check_Total(string db_address, int rep_Id)
    {
        using var db = new DBModel(db_address);
        var rep = db.ReportCollectionDbSet
            .AsNoTracking()
            .AsQueryable()
            .AsSplitQuery()
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Include(x => x.Rows21.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Notes.OrderBy(x => x.Order))
            .FirstOrDefault(x => x.Id == rep_Id);
        return Check_Total(db, rep);
    }

    #endregion
}