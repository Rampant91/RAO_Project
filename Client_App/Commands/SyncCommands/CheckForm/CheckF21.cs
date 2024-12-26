using System;
using Microsoft.EntityFrameworkCore;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using System.Xml.Linq;

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
        { "MachineCode_DB", "03 - Установки переработки - код" },
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

    private static Form21? FormConvert(Form15 form)
    {
        if (form.RefineOrSortRAOCode_DB.Length == 0
            || form.RefineOrSortRAOCode_DB == "-"
            || string.IsNullOrWhiteSpace(form.RefineOrSortRAOCode_DB)   //refine code doesn't exist
            || form.RefineOrSortRAOCode_DB[0] == '7')                   //7x refine codes are ignored
        {
            return null;
        }
        Form21 res = new()
        {
            FormNum_DB = form.FormNum_DB,
            NumberInOrder_DB = form.NumberInOrder_DB
        };
        switch (form.OperationCode_DB)
        {
            case "44":
            {
                //left
                res.RefineMachineName_DB = "in";
                if (byte.TryParse(form.RefineOrSortRAOCode_DB, out var machineCode)) res.MachineCode_DB = machineCode;
                res.CodeRAOIn_DB = "-";
                res.StatusRAOIn_DB = form.StatusRAO_DB;
                res.VolumeIn_DB = "-";
                res.MassIn_DB = "-";
                res.QuantityIn_DB = form.Quantity_DB.ToString();
                return res;
            }
            case "56":
            {
                //right
                res.RefineMachineName_DB = "out";
                if (byte.TryParse(form.RefineOrSortRAOCode_DB, out var machineCode)) res.MachineCode_DB = machineCode;
                res.CodeRAOout_DB = "-";
                res.StatusRAOout_DB = form.StatusRAO_DB;
                res.VolumeOut_DB = "-";
                res.MassOut_DB = "-";
                res.QuantityOZIIIout_DB = form.Quantity_DB.ToString();
                return res;
            }
            default: return null;
        }
    }

    private static Form21? FormConvert(Form16 form)
    {
        if (form.RefineOrSortRAOCode_DB.Length == 0
            || form.RefineOrSortRAOCode_DB == "-"
            || string.IsNullOrWhiteSpace(form.RefineOrSortRAOCode_DB)   //refine code doesn't exist
            || form.RefineOrSortRAOCode_DB[0] == '7')                   //7x refine codes are ignored
        {
            return null;
        }
        Form21 res = new()
        {
            FormNum_DB = form.FormNum_DB,
            NumberInOrder_DB = form.NumberInOrder_DB
        };
        switch (form.OperationCode_DB)
        {
            case "44":
            {
                //left
                res.RefineMachineName_DB = "in";
                if (byte.TryParse(form.RefineOrSortRAOCode_DB, out var machineCode)) res.MachineCode_DB = machineCode;
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
            case "56":
            {
                //right
                res.RefineMachineName_DB = "out";
                if (byte.TryParse(form.RefineOrSortRAOCode_DB, out var machineCode)) res.MachineCode_DB = machineCode;
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
            default: return null;
        }
    }

    private static Form21? FormConvert(Form17 form, Form17? formHeader = null)
    {
        var formTrue = formHeader ?? form;
        if (formTrue.RefineOrSortRAOCode_DB.Length == 0
            || formTrue.RefineOrSortRAOCode_DB == "-"
            || string.IsNullOrWhiteSpace(formTrue.RefineOrSortRAOCode_DB) //refine code doesn't exist
            || formTrue.RefineOrSortRAOCode_DB[0] == '7' //7x refine codes are ignored
            || form.CodeRAO_DB == "-"
            || string.IsNullOrWhiteSpace(form.CodeRAO_DB))
        {
            return null;
        }
        Form21 res = new()
        {
            FormNum_DB = form.FormNum_DB,
            NumberInOrder_DB = form.NumberInOrder_DB
        };
        switch (formTrue.OperationCode_DB)
        {
            case "44":
            {
                //left
                res.RefineMachineName_DB = "in";
                if (byte.TryParse(form.RefineOrSortRAOCode_DB, out var machineCode)) res.MachineCode_DB = machineCode;
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
            case "55":
            {
                //right
                res.RefineMachineName_DB = "out";
                if (byte.TryParse(form.RefineOrSortRAOCode_DB, out var machineCode)) res.MachineCode_DB = machineCode;
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
            default: return null;
        }
    }

    private static Form21? FormConvert(Form18 form, Form18? formHeader = null)
    {
        var formTrue = formHeader ?? form;
        if (formTrue.RefineOrSortRAOCode_DB.Length == 0
            || formTrue.RefineOrSortRAOCode_DB == "-"
            || string.IsNullOrWhiteSpace(formTrue.RefineOrSortRAOCode_DB)   //refine code doesn't exist
            || formTrue.RefineOrSortRAOCode_DB[0] == '7'                    //7x refine codes are ignored
            || form.CodeRAO_DB == "-" 
            || string.IsNullOrWhiteSpace(form.CodeRAO_DB))
        {
            return null;
        }
        Form21 res = new()
        {
            FormNum_DB = form.FormNum_DB,
            NumberInOrder_DB = form.NumberInOrder_DB
        };
        switch (formTrue.OperationCode_DB)
        {
            case "44":
            {
                //left
                res.RefineMachineName_DB = "in";
                if (byte.TryParse(form.RefineOrSortRAOCode_DB, out var machineCode)) res.MachineCode_DB = machineCode;
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
            case "55":
            {
                //right
                res.RefineMachineName_DB = "out";
                if (byte.TryParse(form.RefineOrSortRAOCode_DB, out var machineCode)) res.MachineCode_DB = machineCode;
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
            default: return null;
        }
    }

    #endregion

    #region Form21_Copy

    private static Form21 Form21_Copy(Form21 form, string? inOrOutParam = null)
    {
        Form21 res = new()
        {
            NumberInOrder_DB = form.NumberInOrder_DB,
            RefineMachineName_DB = form.RefineMachineName_DB,
            MachineCode_DB = form.MachineCode_DB,
            FormNum_DB = form.FormNum_DB
        };
        var inOrOut = inOrOutParam ?? res.RefineMachineName_DB;
        if (inOrOut is not ("in" or "out")) inOrOut = res.RefineMachineName_DB;
        if (inOrOut == "in")
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

    private static Form21 Form21_Copy_In(Form21 form)
    {
        Form21 res = new()
        {
            NumberInOrder_DB = form.NumberInOrder_DB,
            RefineMachineName_DB = form.RefineMachineName_DB,
            MachineCode_DB = form.MachineCode_DB,
            FormNum_DB = form.FormNum_DB,
            CodeRAOIn_DB = form.CodeRAOIn_DB,
            StatusRAOIn_DB = form.StatusRAOIn_DB,
            VolumeIn_DB = form.VolumeIn_DB,
            MassIn_DB = form.MassIn_DB,
            QuantityIn_DB = form.QuantityIn_DB,
            TritiumActivityIn_DB = form.TritiumActivityIn_DB,
            BetaGammaActivityIn_DB = form.BetaGammaActivityIn_DB,
            AlphaActivityIn_DB = form.AlphaActivityIn_DB,
            TransuraniumActivityIn_DB = form.TransuraniumActivityIn_DB,
            QuantityOZIIIout_DB = form.QuantityOZIIIout_DB
        };
        return res;
    }

    private static Form21 Form21_Copy_Out(Form21 form)
    {
        Form21 res = new()
        {
            NumberInOrder_DB = form.NumberInOrder_DB,
            RefineMachineName_DB = form.RefineMachineName_DB,
            MachineCode_DB = form.MachineCode_DB,
            FormNum_DB = form.FormNum_DB,
            CodeRAOout_DB = form.CodeRAOout_DB,
            StatusRAOout_DB = form.StatusRAOout_DB,
            VolumeOut_DB = form.VolumeOut_DB,
            MassOut_DB = form.MassOut_DB,
            QuantityOZIIIout_DB = form.QuantityOZIIIout_DB,
            TritiumActivityOut_DB = form.TritiumActivityOut_DB,
            BetaGammaActivityOut_DB = form.BetaGammaActivityOut_DB,
            AlphaActivityOut_DB = form.AlphaActivityOut_DB,
            TransuraniumActivityOut_DB = form.TransuraniumActivityOut_DB,
            QuantityIn_DB = form.QuantityIn_DB
        };
        return res;
    }

    #endregion

    #region Form21_Add

    private static void Form21_Add(Form21 receiver, Form21 giver, string? direction = null)
    {
        var directionReal = direction ?? receiver.RefineMachineName_DB;
        switch (directionReal)
        {
            case "in":
            {
                receiver.VolumeIn_DB = Form21_SubAdd(receiver.VolumeIn_DB, giver.VolumeIn_DB);
                receiver.MassIn_DB = Form21_SubAdd(receiver.MassIn_DB, giver.MassIn_DB);
                receiver.QuantityIn_DB = Form21_SubAdd(receiver.QuantityIn_DB, giver.QuantityIn_DB);
                receiver.TritiumActivityIn_DB = Form21_SubAdd(receiver.TritiumActivityIn_DB, giver.TritiumActivityIn_DB);
                receiver.BetaGammaActivityIn_DB = Form21_SubAdd(receiver.BetaGammaActivityIn_DB, giver.BetaGammaActivityIn_DB);
                receiver.AlphaActivityIn_DB = Form21_SubAdd(receiver.AlphaActivityIn_DB, giver.AlphaActivityIn_DB);
                receiver.TransuraniumActivityIn_DB = Form21_SubAdd(receiver.TransuraniumActivityIn_DB, giver.TransuraniumActivityIn_DB);
                break;
            }
            case "out":
            {
                receiver.VolumeOut_DB = Form21_SubAdd(receiver.VolumeOut_DB, giver.VolumeOut_DB);
                receiver.MassOut_DB = Form21_SubAdd(receiver.MassOut_DB, giver.MassOut_DB);
                receiver.QuantityOZIIIout_DB = Form21_SubAdd(receiver.QuantityOZIIIout_DB, giver.QuantityOZIIIout_DB);
                receiver.TritiumActivityOut_DB = Form21_SubAdd(receiver.TritiumActivityOut_DB, giver.TritiumActivityOut_DB);
                receiver.BetaGammaActivityOut_DB = Form21_SubAdd(receiver.BetaGammaActivityOut_DB, giver.BetaGammaActivityOut_DB);
                receiver.AlphaActivityOut_DB = Form21_SubAdd(receiver.AlphaActivityOut_DB, giver.AlphaActivityOut_DB);
                receiver.TransuraniumActivityOut_DB = Form21_SubAdd(receiver.TransuraniumActivityOut_DB, giver.TransuraniumActivityOut_DB);
                break;
            }
        }
    }

    /// <summary>
    /// Tries parsing the two parameters and assigning their sum to the return value, converting it back to string afterward.
    /// If the parsing fails, returns the first parameter as is.
    /// </summary>
    /// <param name="receiver">The first parameter</param>
    /// <param name="giver">The second parameter</param>
    /// <returns>A string representation of the sum of the parameters. If the summation fails, returns the first parameter.</returns>
    private static string Form21_SubAdd(string receiver, string giver)
    {
        var res = receiver;
        var receiverReal = receiver == "-" || string.IsNullOrWhiteSpace(receiver) 
            ? "0" 
            : receiver;
        var giverReal = giver == "-" || string.IsNullOrWhiteSpace(giver) 
            ? "0" 
            : giver;
        if (TryParseDoubleExtended(receiverReal, out var receiverTrue) 
            && TryParseDoubleExtended(giverReal, out var giverTrue))
        {
            res = (receiverTrue + giverTrue).ToString("e2").Replace("+0","+");
        }
        return res;
    }

    #endregion

    #region Form21_SubMatch

    private static void Form21_SubMatch(string form1Val, string form2Val, string humanName, double valB, List<(int, string, string, string)> res, int columnNum, string forms1, string forms2)
    {
        TryParseDoubleExtended(form1Val, out var val1);
        TryParseDoubleExtended(form2Val, out var val2);
        if (!((form1Val == "-" && form2Val == "-") 
              || (val1 < 0.001 && form2Val == "-") 
              || (form1Val == "-" && val2 < 0.001) 
              || val1 >= val2 * (1.0 - valB) 
              && val2 >= val1 * (1.0 - valB)))
        {
            res.Add((columnNum, $"{humanName}", $"{forms1}: {form1Val}", $"{forms2}: {form2Val}"));
        }
    }

    #endregion

    #region Form21_Match

    private static List<(int,string,string,string)>? Form21_Match(Form21 form1, Form21 form2, string forms1, string forms2)
    {
        const double valB = 0.1;
        List<(int, string, string, string)> res = [];
        if (form1.CodeRAOIn_DB == form2.CodeRAOIn_DB 
            && form1.CodeRAOIn_DB != "-" 
            && !string.IsNullOrWhiteSpace(form1.CodeRAOIn_DB))
        {
            if (form1.StatusRAOIn_DB == form2.StatusRAOIn_DB
                && form1.CodeRAOIn_DB == form2.CodeRAOIn_DB
                && form1.MachineCode_DB == form2.MachineCode_DB)
            {
                Form21_SubMatch(form1.VolumeIn_DB, form2.VolumeIn_DB, "Объем без упаковки, куб. м", valB, res, 8, forms1, forms2);
                Form21_SubMatch(form1.MassIn_DB, form2.MassIn_DB, "Масса без упаковки (нетто), т", valB, res, 9, forms1, forms2);
                Form21_SubMatch(form1.QuantityIn_DB, form2.QuantityIn_DB, "кол-во ОЗИИИ, шт.", valB, res, 10, forms1, forms2);
                Form21_SubMatch(form1.TritiumActivityIn_DB, form2.TritiumActivityIn_DB, "суммарная активность (тритий), Бк", valB, res, 11, forms1, forms2);
                Form21_SubMatch(form1.BetaGammaActivityIn_DB, form2.BetaGammaActivityIn_DB, "суммарная активность (бета, гамма), Бк", valB, res, 12, forms1, forms2);
                Form21_SubMatch(form1.AlphaActivityIn_DB, form2.AlphaActivityIn_DB, "суммарная активность (альфа), Бк", valB, res, 13, forms1, forms2);
                Form21_SubMatch(form1.TransuraniumActivityIn_DB, form2.TransuraniumActivityIn_DB, "суммарная активность (трансурановые), Бк", valB, res, 14, forms1, forms2);
                return res;
            }
            return null;
        }
        if (form1.CodeRAOout_DB == form2.CodeRAOout_DB 
            && form1.CodeRAOout_DB != "-" 
            && !string.IsNullOrWhiteSpace(form1.CodeRAOout_DB))
        {
            if (form1.StatusRAOout_DB == form2.StatusRAOout_DB
                && form1.CodeRAOout_DB == form2.CodeRAOout_DB
                && form1.MachineCode_DB == form2.MachineCode_DB)
            {
                Form21_SubMatch(form1.VolumeOut_DB, form2.VolumeOut_DB, "Объем без упаковки, куб. м", valB, res, 17, forms1, forms2);
                Form21_SubMatch(form1.MassOut_DB, form2.MassOut_DB, "Масса без упаковки (нетто), т", valB, res, 18, forms1, forms2);
                Form21_SubMatch(form1.QuantityOZIIIout_DB, form2.QuantityOZIIIout_DB, "кол-во ОЗИИИ, шт.", valB, res, 19, forms1, forms2);
                Form21_SubMatch(form1.TritiumActivityOut_DB, form2.TritiumActivityOut_DB, "суммарная активность (тритий), Бк", valB, res, 20, forms1, forms2);
                Form21_SubMatch(form1.BetaGammaActivityOut_DB, form2.BetaGammaActivityOut_DB, "суммарная активность (бета, гамма), Бк", valB, res, 21, forms1, forms2);
                Form21_SubMatch(form1.AlphaActivityOut_DB, form2.AlphaActivityOut_DB, "суммарная активность (альфа), Бк", valB, res, 22, forms1, forms2);
                Form21_SubMatch(form1.TransuraniumActivityOut_DB, form2.TransuraniumActivityOut_DB, "суммарная активность (трансурановые), Бк", valB, res, 23, forms1, forms2);
                return res;
            }
            return null;
        }
        return null;
    }

    #endregion

    #region CheckTotal

    public static async Task<List<CheckError>> Check_Total(DBModel db, Report? rep)
    {
        List<CheckError> errorList = [];
        if (rep == null) return errorList;
        var form20Id = !string.IsNullOrWhiteSpace(rep.Reports.Master_DB.Rows20[0].RegNo_DB) 
            ? 0 
            : !string.IsNullOrWhiteSpace(rep.Reports.Master_DB.Rows20[1].RegNo_DB) 
                ? 1 
                : -1;
        if (form20Id == -1) return errorList;

        await db.form_20.Where(x => x.RegNo_DB == rep.Reports.Master_DB.Rows20[form20Id].RegNo_DB).LoadAsync();   //load forms 2.0 that correspond to the selected report
        await db.form_10.Where(x => x.RegNo_DB == rep.Reports.Master_DB.Rows20[form20Id].RegNo_DB).LoadAsync();   //load forms 1.0 that correspond to the selected report
        var form10 = db.ReportCollectionDbSet
            .Where(x => x.FormNum_DB == "1.0")
            .ToList()
            .SingleOrDefault(x => x.Rows10.Count > 0);    //isolate form 1.0 that matches our form 2.0
        Reports? reps1;
        if (form10 == null)
        {
            var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;

            #region MessageFailedToOpenForm

            var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Выбрать файл БД", IsDefault = true },
                        new ButtonDefinition { Name = "Отмена", IsCancel = true }
                    ],
                    CanResize = true,
                    ContentTitle = "Проверка формы",
                    ContentHeader = "Ошибка",
                    ContentMessage = "В текущей базе данных отсутствует форма 1.0 для проверяемой организации." +
                                     $"{Environment.NewLine}Можете выбрать файл базы данных, содержащий форму 1.0 для данной организации " +
                                     $"{Environment.NewLine}или операция проверки формы будет отменена.",
                    MinWidth = 400,
                    MinHeight = 200,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow));

            #endregion

            if (answer is "Выбрать файл БД")
            {
                OpenFileDialog dial = new() { AllowMultiple = false };
                var filter = new FileDialogFilter
                {
                    Extensions = { "RAODB" }
                };
                dial.Filters = [filter];
                var dbWithForm1 = await dial.ShowAsync(desktop.MainWindow);
                if (dbWithForm1 is null) return errorList;
                var dbWithForm1FullPath = dbWithForm1[0];
                await using DBModel db2 = new(dbWithForm1FullPath);

                await db2.form_10.Where(x => x.RegNo_DB == rep.Reports.Master_DB.Rows20[form20Id].RegNo_DB).LoadAsync();
                form10 = db2.ReportCollectionDbSet
                    .Where(x => x.FormNum_DB == "1.0")
                    .ToList()
                    .SingleOrDefault(x => x.Rows10.Count > 0);    //isolate form 1.0 that matches our form 2.0

                await db2.ReportsCollectionDbSet.Where(x => x.Master_DBId == form10.Id).LoadAsync();
                reps1 = db2.ReportsCollectionDbSet.SingleOrDefault(x => x.Master_DBId == form10.Id);
                await db2.ReportCollectionDbSet
                    .Where(x => x.Reports.Master_DBId == reps1.Master_DBId
                                && (x.FormNum_DB == "1.5"
                                    || x.FormNum_DB == "1.6"
                                    || x.FormNum_DB == "1.7"
                                    || x.FormNum_DB == "1.8")
                                && (x.StartPeriod_DB.Length >= 4
                                    && x.StartPeriod_DB.Substring(x.StartPeriod_DB.Length - 4) == rep.Year_DB
                                    || x.EndPeriod_DB.Length >= 4
                                    && x.EndPeriod_DB.Substring(x.EndPeriod_DB.Length - 4) == rep.Year_DB))
                    .LoadAsync(); //load reports themselves

                foreach (var report in reps1.Report_Collection)
                {
                    await db2.form_15.Where(x => x.ReportId == report.Id).LoadAsync();  //load rows for reports 1.5
                    await db2.form_16.Where(x => x.ReportId == report.Id).LoadAsync();  //load rows for reports 1.6
                    await db2.form_17.Where(x => x.ReportId == report.Id).LoadAsync();  //load rows for reports 1.7
                    await db2.form_18.Where(x => x.ReportId == report.Id).LoadAsync();  //load rows for reports 1.8
                }
            }
            else return errorList;
        }
        else
        {
            await db.ReportsCollectionDbSet.Where(x => x.Master_DBId == form10.Id).LoadAsync();    //load the report collection that corresponds to the isolated form 1.0
            reps1 = db.ReportsCollectionDbSet.SingleOrDefault(x => x.Master_DBId == form10.Id);    //isolate this report collection
            if (reps1 == null) return errorList;
            await db.ReportCollectionDbSet
                .Where(x => x.Reports.Master_DBId == reps1.Master_DBId
                            && (x.FormNum_DB == "1.5"
                                || x.FormNum_DB == "1.6"
                                || x.FormNum_DB == "1.7"
                                || x.FormNum_DB == "1.8")
                            && (x.StartPeriod_DB.Length >= 4
                                && x.StartPeriod_DB.Substring(x.StartPeriod_DB.Length - 4) == rep.Year_DB
                                || x.EndPeriod_DB.Length >= 4
                                && x.EndPeriod_DB.Substring(x.EndPeriod_DB.Length - 4) == rep.Year_DB))
                .LoadAsync(); //load reports themselves

            foreach (var report in reps1.Report_Collection)
            {
                await db.form_15.Where(x => x.ReportId == report.Id).LoadAsync();  //load rows for reports 1.5
                await db.form_16.Where(x => x.ReportId == report.Id).LoadAsync();  //load rows for reports 1.6
                await db.form_17.Where(x => x.ReportId == report.Id).LoadAsync();  //load rows for reports 1.7
                await db.form_18.Where(x => x.ReportId == report.Id).LoadAsync();  //load rows for reports 1.8
            }
        }
        
        //at this point forms1 contains everything that we need to convert.

        List<Form21> forms21ExpectedBase = [];
        List<(string, string, string, string)> forms21MetadataBase = [];
        Form17? formHeader17 = null;
        Form18? formHeader18 = null;
        foreach (var key in reps1.Report_Collection)
        {
            var report = (Report)key;
            Form21? form21New;
            switch (report.FormNum_DB)
            {
                case "1.5":
                {
                    foreach (var key1 in report.Rows15)
                    {
                        var form = (Form15)key1;
                        form21New = FormConvert(form);
                        if (form21New != null)
                        {
                            forms21MetadataBase.Add((form21New.FormNum_DB, report.StartPeriod_DB, report.EndPeriod_DB, form21New.NumberInOrder_DB.ToString()));
                            forms21ExpectedBase.Add(form21New);
                        }
                    }
                    break;
                }
                case "1.6":
                {
                    foreach (var key1 in report.Rows16)
                    {
                        var form = (Form16)key1;
                        form21New = FormConvert(form);
                        if (form21New != null)
                        {
                            forms21MetadataBase.Add((form21New.FormNum_DB, report.StartPeriod_DB, report.EndPeriod_DB, form21New.NumberInOrder_DB.ToString()));
                            forms21ExpectedBase.Add(form21New);
                        }
                    }
                    break;
                }
                case "1.7":
                {
                    foreach (var key1 in report.Rows17)
                    {
                        var form = (Form17)key1;
                        if (form.OperationCode_DB != "-") formHeader17 = form;
                        form21New = FormConvert(form, formHeader17);
                        if (form21New != null)
                        {
                            forms21MetadataBase.Add((form21New.FormNum_DB, report.StartPeriod_DB, report.EndPeriod_DB, form21New.NumberInOrder_DB.ToString()));
                            forms21ExpectedBase.Add(form21New);
                        }
                    }
                    break;
                }
                case "1.8":
                {
                    foreach (var key1 in report.Rows18)
                    {
                        var form = (Form18)key1;
                        if (form.OperationCode_DB != "-") formHeader18 = form;
                        form21New = FormConvert(form, formHeader18);
                        if (form21New != null)
                        {
                            forms21MetadataBase.Add((form21New.FormNum_DB, report.StartPeriod_DB, report.EndPeriod_DB, form21New.NumberInOrder_DB.ToString()));
                            forms21ExpectedBase.Add(form21New);
                        }
                    }
                    break;
                }
            }
        }
        Dictionary<(byte?, string, string), Form21> forms21ExpectedInDict = new();
        Dictionary<(byte?, string, string), Form21> forms21ExpectedOutDict = new();
        Dictionary<(byte?, string, string), Form21> forms21RealInDict = new();
        Dictionary<(byte?, string, string), Form21> forms21RealOutDict = new();
        Dictionary<(byte?, string, string), Dictionary<string, Dictionary<string, List<string>>>> forms21MetadataInDict = new();
        Dictionary<(byte?, string, string), Dictionary<string, Dictionary<string, List<string>>>> forms21MetadataOutDict = new();
        for (var i = 0; i < forms21ExpectedBase.Count; i++)
        {
            switch (forms21ExpectedBase[i].RefineMachineName_DB)
            {
                case "in":
                {
                    var key = (forms21ExpectedBase[i].MachineCode_DB, forms21ExpectedBase[i].CodeRAOIn_DB, forms21ExpectedBase[i].StatusRAOIn_DB);
                    if (!forms21ExpectedInDict.TryGetValue(key, out var form21))
                    {
                        forms21ExpectedInDict[key] = Form21_Copy(forms21ExpectedBase[i]);
                    }
                    else
                    {
                        Form21_Add(form21, forms21ExpectedBase[i]);
                    }
                    if (!forms21MetadataInDict.ContainsKey(key))
                    {
                        forms21MetadataInDict[key] = [];
                    }
                    if (!forms21MetadataInDict[key].ContainsKey(forms21MetadataBase[i].Item1))
                    {
                        forms21MetadataInDict[key][forms21MetadataBase[i].Item1] = [];
                    }
                    if (!forms21MetadataInDict[key][forms21MetadataBase[i].Item1].ContainsKey($"{forms21MetadataBase[i].Item2} - {forms21MetadataBase[i].Item3}"))
                    {
                        forms21MetadataInDict[key][forms21MetadataBase[i].Item1][$"{forms21MetadataBase[i].Item2} - {forms21MetadataBase[i].Item3}"] = [];
                    }
                    forms21MetadataInDict[key][forms21MetadataBase[i].Item1][$"{forms21MetadataBase[i].Item2} - {forms21MetadataBase[i].Item3}"].Add(forms21MetadataBase[i].Item4);
                    break;
                }
                case "out":
                {
                    var key = (forms21ExpectedBase[i].MachineCode_DB, forms21ExpectedBase[i].CodeRAOout_DB, forms21ExpectedBase[i].StatusRAOout_DB);
                    if (!forms21ExpectedOutDict.TryGetValue(key, out var form21))
                    {
                        forms21ExpectedOutDict[key] = Form21_Copy(forms21ExpectedBase[i]);
                    }
                    else
                    {
                        Form21_Add(form21, forms21ExpectedBase[i]);
                    }
                    if (!forms21MetadataOutDict.ContainsKey(key))
                    {
                        forms21MetadataOutDict[key] = [];
                    }
                    if (!forms21MetadataOutDict[key].ContainsKey(forms21MetadataBase[i].Item1))
                    {
                        forms21MetadataOutDict[key][forms21MetadataBase[i].Item1] = [];
                    }
                    if (!forms21MetadataOutDict[key][forms21MetadataBase[i].Item1].ContainsKey($"{forms21MetadataBase[i].Item2} - {forms21MetadataBase[i].Item3}"))
                    {
                        forms21MetadataOutDict[key][forms21MetadataBase[i].Item1][$"{forms21MetadataBase[i].Item2} - {forms21MetadataBase[i].Item3}"] = [];
                    }
                    forms21MetadataOutDict[key][forms21MetadataBase[i].Item1][$"{forms21MetadataBase[i].Item2} - {forms21MetadataBase[i].Item3}"].Add(forms21MetadataBase[i].Item4);
                    break;
                }
            }
        }
        List<Form21> forms21RealIn = [];
        List<Form21> forms21RealOut = [];
        byte? machineCodeHeader = null;
        foreach (var key1 in rep.Rows21)
        {
            var form = (Form21)key1;
            if (form.MachineCode_DB != null) machineCodeHeader = form.MachineCode_DB;
            if (form.CodeRAOIn_DB != "-" && !string.IsNullOrWhiteSpace(form.CodeRAOIn_DB))
            {
                var key = (MachineCode_Header: machineCodeHeader, form.CodeRAOIn_DB, form.StatusRAOIn_DB);
                if (!forms21RealInDict.ContainsKey(key))
                {
                    forms21RealInDict[key] = Form21_Copy_In(form);
                    forms21RealInDict[key].MachineCode_DB = machineCodeHeader;
                }
                else
                {
                    Form21_Add(forms21RealInDict[key], form, "in");
                    errorList.Add(new CheckError
                    {
                        FormNum = "form_21",
                        Row = form.NumberInOrder_DB.ToString(),
                        Column = "6 - 14",
                        Value = $"код РАО {form.CodeRAOIn_DB}, статус РАО {form.StatusRAOIn_DB}, " +
                                $"код переработки/сортировки {machineCodeHeader}",
                        Message = $"В форме 2.1 уже присутствует строка с указанными РАО, " +
                                  $"поступившими на переработку/кондиционирование (строка {forms21RealInDict[key].NumberInOrder_DB})."
                    });
                }
            }
            if (form.CodeRAOout_DB != "-" && !string.IsNullOrWhiteSpace(form.CodeRAOout_DB))
            {
                var key = (MachineCode_Header: machineCodeHeader, form.CodeRAOout_DB, form.StatusRAOout_DB);
                if (!forms21RealOutDict.ContainsKey(key))
                {
                    forms21RealOutDict[key] = Form21_Copy_Out(form);
                    forms21RealOutDict[key].MachineCode_DB = machineCodeHeader;
                }
                else
                {
                    Form21_Add(forms21RealOutDict[key], form, "out");
                    errorList.Add(new CheckError
                    {
                        FormNum = "form_21",
                        Row = form.NumberInOrder_DB.ToString(),
                        Column = "15 - 23",
                        Value = $"код РАО {form.CodeRAOout_DB}, статус РАО {form.StatusRAOout_DB}, " +
                                $"код переработки/сортировки {machineCodeHeader}",
                        Message = $"В форме 2.1 уже присутствует строка с указанными РАО, " +
                                  $"образовавшихся после переработки/кондиционирования (строка {forms21RealOutDict[key].NumberInOrder_DB})."
                    });
                }
            }
        }

        forms21RealIn.AddRange(forms21RealInDict.Keys.Select(key => forms21RealInDict[key]));
        forms21RealOut.AddRange(forms21RealOutDict.Keys.Select(key => forms21RealOutDict[key]));
        //the converted values should be compared to the rows in reps.
        List<(Form21,string,string)> forms21ExpectedIn = [];
        List<(Form21,string,string)> forms21ExpectedOut = [];
        foreach (var key in forms21ExpectedInDict.Keys)
        {
            List<string> addressSubstrings = [];
            List<string> formsSubstrings = [];
            foreach (var keyForm in forms21MetadataInDict[key].Keys)
            {
                var addressSubstring = "";
                addressSubstring += $"форма {keyForm}: \r\n";
                formsSubstrings.Add(keyForm);
                var periods = forms21MetadataInDict[key][keyForm].Keys.ToList();
                for (var i = 0; i < periods.Count; i++)
                    periods[i] = $"{periods[i].Substring(6, 4)}.{periods[i].Substring(3, 2)}.{periods[i][..2]}{periods[i][10..]}";
                periods.Sort();
                for (var i = 0; i < periods.Count; i++)
                    periods[i] = $"{periods[i].Substring(8, 2)}.{periods[i].Substring(5, 2)}.{periods[i][..4]}{periods[i][10..]}";
                foreach (var keyPeriod in periods)
                {
                    addressSubstring += $"отчёт {keyPeriod}: ";
                    var lines = forms21MetadataInDict[key][keyForm][keyPeriod].ToList();
                    List<int> linesReal = [];
                    linesReal.AddRange(lines.Select(int.Parse));
                    linesReal.Sort();
                    for (var i = linesReal.Count - 1; i > 1; i--)
                    {
                        if (linesReal[i] == linesReal[i-1]+1 && (linesReal[i - 1] == linesReal[i - 2] + 1
                            || i<linesReal.Count-1 && linesReal[i+1] == -1))
                        {
                            linesReal[i] = -1;
                        }
                    }
                    if (linesReal.Count >= 3 && linesReal[1] == linesReal[0] + 1 && linesReal[2] == -1) linesReal[1] = -1;
                    lines = [];
                    var lineRange = 0;
                    foreach (var line in linesReal)
                    {
                        if (line == -1) lineRange++;
                        else
                        {
                            if (lineRange > 0) lines[^1] = $"{lines[^1]} - {int.Parse(lines[^1])+lineRange}";
                            lines.Add(line.ToString());
                            lineRange = 0;
                        }
                    }

                    lines[^1] = lineRange switch
                    {
                        > 1 => $"{lines[^1]} - {int.Parse(lines[^1]) + lineRange}",
                        > 0 => $"{lines[^1]}, {int.Parse(lines[^1]) + lineRange}",
                        _ => lines[^1]
                    };
                    addressSubstring += $"строк{(linesReal.Count == 1 ? "а" : "и")} {string.Join(", ", lines)} \r\n";
                }
                addressSubstrings.Add(addressSubstring);
            }
            var addressString = string.Join("; \r\n", addressSubstrings);
            var formsString = string.Join(", ", formsSubstrings);
            forms21ExpectedIn.Add((forms21ExpectedInDict[key], addressString, formsString));
        }
        foreach (var key in forms21ExpectedOutDict.Keys)
        {
            List<string> addressSubstrings = [];
            List<string> formsSubstrings = [];
            foreach (var keyForm in forms21MetadataOutDict[key].Keys)
            {
                var addressSubstring = "";
                addressSubstring += $"форма {keyForm}: \r\n";
                formsSubstrings.Add(keyForm);
                var periods = forms21MetadataOutDict[key][keyForm].Keys.ToList();
                for (var i = 0; i < periods.Count; i++)
                    periods[i] = $"{periods[i].Substring(6, 4)}.{periods[i].Substring(3, 2)}.{periods[i][..2]}{periods[i][10..]}";
                periods.Sort();
                for (var i = 0; i < periods.Count; i++)
                    periods[i] = $"{periods[i].Substring(8, 2)}.{periods[i].Substring(5, 2)}.{periods[i][..4]}{periods[i][10..]}";
                foreach (var keyPeriod in periods)
                {
                    addressSubstring += $"отчёт {keyPeriod}: ";
                    var lines = forms21MetadataOutDict[key][keyForm][keyPeriod].ToList();
                    var linesReal = lines.Select(int.Parse).ToList();
                    linesReal.Sort();
                    for (var i = linesReal.Count - 1; i > 1; i--)
                    {
                        if (linesReal[i] == linesReal[i - 1] + 1 && (linesReal[i - 1] == linesReal[i - 2] + 1
                            || i < linesReal.Count - 1 && linesReal[i + 1] == -1))
                        {
                            linesReal[i] = -1;
                        }
                    }
                    if (linesReal.Count >= 3 && linesReal[1] == linesReal[0] + 1 && linesReal[2] == -1) linesReal[1] = -1;
                    lines = [];
                    var lineRange = 0;
                    foreach (var line in linesReal)
                    {
                        if (line == -1) lineRange++;
                        else
                        {
                            if (lineRange>0) lines[^1] = $"{lines[^1]} - {int.Parse(lines[^1]) + lineRange}";
                            lines.Add(line.ToString());
                            lineRange = 0;
                        }
                    }

                    lines[^1] = lineRange switch
                    {
                        > 1 => $"{lines[^1]} - {int.Parse(lines[^1]) + lineRange}",
                        > 0 => $"{lines[^1]}, {int.Parse(lines[^1]) + lineRange}",
                        _ => lines[^1]
                    };
                    addressSubstring += $"строк{(linesReal.Count == 1 ? "а" : "и")} {string.Join(", ", lines)} \r\n";
                }
                addressSubstrings.Add(addressSubstring);
            }
            var addressString = string.Join("; \r\n", addressSubstrings);
            var formsString = string.Join(", ", formsSubstrings);
            forms21ExpectedOut.Add((forms21ExpectedOutDict[key], addressString, formsString));
        }
        foreach (var formReal in forms21RealIn)
        {
            var matchFound = false;
            foreach (var formExpected in forms21ExpectedIn)
            {
                var mismatches = Form21_Match(formExpected.Item1, formReal, $"форм{(formExpected.Item3.Contains(',') ? "ы":"а")} {formExpected.Item3}", "форма 2.1");
                if (mismatches == null) continue;
                forms21ExpectedIn.Remove(formExpected);
                matchFound = true;
                if (mismatches.Count > 0)
                {
                    List<int> listColumns = [];
                    List<string> listHints = [];
                    foreach (var mismatch in mismatches)
                    {
                        listColumns.Add(mismatch.Item1);
                        listHints.Add($"{mismatch.Item2}: {mismatch.Item3}, {mismatch.Item4}");
                    }
                    var columns = string.Join(", ", listColumns);
                    var hints = string.Join(";\n", listHints);
                    errorList.Add(new CheckError
                    {
                        FormNum = "form_21",
                        Row = formReal.NumberInOrder_DB.ToString(),
                        Column = columns,
                        Value = $"код РАО {formReal.CodeRAOIn_DB}, статус РАО {formReal.StatusRAOIn_DB}, " +
                                $"код переработки/сортировки {formReal.MachineCode_DB}\n - {formExpected.Item2}",
                        Message = $"Сведения о РАО, поступивших на переработку, не совпадают:\n\n{hints}"
                    });
                }
                break;
            }
            if (!matchFound)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_21",
                    Row = formReal.NumberInOrder_DB.ToString(),
                    Column = "6 - 14",
                    Value = $"код РАО {formReal.CodeRAOIn_DB}, статус РАО {formReal.StatusRAOIn_DB}, " +
                            $"код переработки/сортировки {formReal.MachineCode_DB}",
                    Message = "В формах 1.5 - 1.8 не найдена информация об указанных РАО, поступивших на переработку/кондиционирование."
                });
            }
        }
        foreach (var formReal in forms21RealOut)
        {
            var matchFound = false;
            foreach (var formExpected in forms21ExpectedOut)
            {
                var mismatches = Form21_Match(formExpected.Item1, formReal, $"форм{(formExpected.Item3.Contains(',') ? "ы" : "а")} {formExpected.Item3}", "форма 2.1");
                if (mismatches == null) continue;
                forms21ExpectedOut.Remove(formExpected);
                matchFound = true;
                if (mismatches.Count > 0)
                {
                    List<int> listColumns = [];
                    List<string> listHints = [];
                    foreach (var mismatch in mismatches)
                    {
                        listColumns.Add(mismatch.Item1);
                        listHints.Add($"{mismatch.Item2}: {mismatch.Item3}, {mismatch.Item4}");
                    }
                    var columns = string.Join(", ", listColumns);
                    var hints = string.Join(";\n", listHints);
                    errorList.Add(new CheckError
                    {
                        FormNum = "form_21",
                        Row = formReal.NumberInOrder_DB.ToString(),
                        Column = columns,
                        Value = $"код РАО {formReal.CodeRAOout_DB}, статус РАО {formReal.StatusRAOout_DB}, " +
                                $"код переработки/сортировки {formReal.MachineCode_DB}\n - {formExpected.Item2}",
                        Message = $"Сведения о РАО, образовавшихся после переработки, не совпадают:\n\n{hints}"
                    });
                }
                break;
            }
            if (!matchFound)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_21",
                    Row = formReal.NumberInOrder_DB.ToString(),
                    Column = "15 - 23",
                    Value = $"код РАО {formReal.CodeRAOout_DB}, " +
                            $"статус РАО {formReal.StatusRAOout_DB}, код переработки/сортировки {formReal.MachineCode_DB}",
                    Message = "В формах 1.5 - 1.8 не найдена информация об указанных РАО, " +
                              "образовавшихся после переработки/кондиционирования."
                });
            }
        }
        foreach (var formExpected in forms21ExpectedIn)
        {
            errorList.Add(new CheckError
            {
                FormNum = "form_21",
                Row = "-",
                Column = "6 - 14",
                Value = $"код РАО {formExpected.Item1.CodeRAOIn_DB}, статус РАО {formExpected.Item1.StatusRAOIn_DB}, " +
                        $"код переработки/сортировки {formExpected.Item1.MachineCode_DB}\n - {formExpected.Item2}",
                Message = "В форме 2.1 не найдена информация об указанных РАО, поступивших на переработку/кондиционирование."
            });
        }
        errorList.Sort((i, j) => 
            int.TryParse(i.Row, out var iRowReal) 
            && int.TryParse(j.Row, out var jRowReal) 
                ? iRowReal - jRowReal 
                : string.Compare(i.Row, j.Row));
        foreach (var formExpected in forms21ExpectedOut)
        {
            errorList.Insert(0, new CheckError
            {
                FormNum = "form_21",
                Row = "-",
                Column = "15 - 23",
                Value = $"код РАО {formExpected.Item1.CodeRAOout_DB}, статус РАО {formExpected.Item1.StatusRAOout_DB}, " +
                        $"код переработки/сортировки {formExpected.Item1.MachineCode_DB}\n - {formExpected.Item2}",
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
        return errorList;
    }

    public static async Task<List<CheckError>> Check_Total(string dbAddress, int repId)
    {
        await using var db = new DBModel(dbAddress);
        var rep = db.ReportCollectionDbSet
            .AsNoTracking()
            .AsQueryable()
            .AsSplitQuery()
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Include(x => x.Rows21.OrderBy(form => form.NumberInOrder_DB))
            .Include(x => x.Notes.OrderBy(note => note.Order))
            .FirstOrDefault(x => x.Id == repId);
        return await Check_Total(db, rep);
    }

    #endregion
}