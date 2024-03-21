using Models.CheckForm;
using Models.Collections;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.IO;
using Models.Forms;
using System.Linq;

namespace Client_App.Commands.SyncCommands.CheckForm;

public class CheckF13 : CheckBase
{
    #region Check_Total
    
    public static List<CheckError> Check_Total(Reports reps, Report rep)
    {
        var currentFormLine = 0;
        List<CheckError> errorList = new();

        if (OKSM.Count == 0)
        {
#if DEBUG
            OKSM_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "oksm.xlsx"));
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

        foreach (var key in rep.Rows13)
        {
            var form = (Form13)key;
            var formsList = rep.Rows13.ToList<Form13>();
            var notes = rep.Notes.ToList<Note>();
            var forms10 = reps.Master_DB.Rows10.ToList<Form10>();
            errorList.AddRange(Check_001(formsList, currentFormLine));
            currentFormLine++;
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
    
    private static List<CheckError> Check_002(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (forms[line].Id < 1)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Id",
                Value = forms[line].Id.ToString(),
                Message = "-"
            });
        }
        return result;
    }

    #endregion

    #region Check003
    
    //Код операции из списка (колонка 2)
    private static List<CheckError> Check_003(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var operationCodeValid = new[]
        {
            "10", "11", "12", "15", "17", "18", "21", "22", "25", "27", "28", "29", "31", "32", "35", "37", "38", "39",
            "41", "42", "43", "46", "47", "53", "54", "58", "61", "62", "63", "64", "65", "67", "68", "71", "72", "73",
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
                Value = operationCode,
                Message = $"Код операции {operationCode} не может быть использован в форме 1.3."
            });
        }
        return result;
    }

    #endregion

    #region Check004
    
    private static List<CheckError> Check_004(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check005
    
    //Для кодов операции 12, 42, в радионуклидах должен быть указан хоть один из списка (колонка 6)
    private static List<CheckError> Check_005(List<Form13> forms, int line)
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
        var valid = radionuclidValid.Any(nuclid => nuclid == radionuclid);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "Radionuclids_DB",
                Value = forms[line].Radionuclids_DB,
                Message = "В графе 6 не представлены сведения о радионуклидах, которые могут быть отнесены к ЯМ. Проверьте правильность выбранного кода операции."
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
    
    private static List<CheckError> Check_007(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check008

    //Для определенных кодов операции должно быть примечание (колонка 2)
    private static List<CheckError> Check_008(List<Form13> forms, List<Note> notes, int line)
    {
        List<CheckError> result = new();
        const byte graphNumber = 2;
        var operationCode = forms[line].OperationCode_DB;
        string[] applicableOperationCodes = { "29", "39", "97", "98", "99" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = CheckNotePresence(forms, notes, line, graphNumber);
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_13",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "Необходимо дать пояснение об осуществленной операции."
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
    
    private static List<CheckError> Check_011(List<Form13> forms, int line)
    {
        List<CheckError> result = new();
        if (DB_Ignore) return result;
        return result;
    }

    #endregion

    #region Check012

    //Для кода операции 53, ОКПО организации должен быть равен ОКПО правообладателя
    private static List<CheckError> Check_012(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var owner = forms[line].Owner_DB;
        var okpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB) ? forms10[1].Okpo_DB : forms10[0].Okpo_DB;
        string[] applicableOperationCodes = { "53" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = !string.IsNullOrWhiteSpace(owner) && owner == okpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "В графе 17 необходимо указать код ОКПО своей организации." 
                          + $"{Environment.NewLine}В случае, если зарядку/разрядку осуществляла подрядная организация, следует использовать код операции 54"
            });
        }
        return result;
    }

    #endregion

    #region Check013

    //Для кода операции 54, ОКПО организации не должен быть равен ОКПО правообладателя
    private static List<CheckError> Check_013(List<Form13> forms, List<Form10> forms10, int line)
    {
        List<CheckError> result = new();
        var operationCode = forms[line].OperationCode_DB;
        var owner = forms[line].Owner_DB;
        var okpo = !string.IsNullOrWhiteSpace(forms10[1].Okpo_DB) ? forms10[1].Okpo_DB : forms10[0].Okpo_DB;
        string[] applicableOperationCodes = { "53" };
        if (!applicableOperationCodes.Contains(operationCode)) return result;
        var valid = !string.IsNullOrWhiteSpace(owner) && owner == okpo;
        if (!valid)
        {
            result.Add(new CheckError
            {
                FormNum = "form_11",
                Row = (line + 1).ToString(),
                Column = "OperationCode_DB",
                Value = operationCode,
                Message = "В графе 17 необходимо указать код ОКПО своей организации." 
                          + $"{Environment.NewLine}В случае, если зарядку/разрядку осуществляла подрядная организация, следует использовать код операции 54"
            });
        }
        return result;
    }

    #endregion

    #endregion
}