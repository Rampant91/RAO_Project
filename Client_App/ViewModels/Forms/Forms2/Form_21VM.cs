using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Calculator;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.ViewModels.Controls;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms2;

public class Form_21VM : BaseFormVM
{
    public override string FormType => "2.1";

    #region Constructors

    public Form_21VM() { }

    public Form_21VM(Report report) : base(report) { }

    public Form_21VM(in Reports reps)
    {
        var formNum = FormType;
        Report = new Report
        {
            FormNum_DB = formNum,
            Reports = reps
        };

        base.InitializeUserControls();
        Reports = reps;

    }

    #endregion

    #region Properties
    private int _sumRowsTotalPages = 0;
    public override int TotalPages
    {
        get
        {
            if (SumMode)
                return _sumRowsTotalPages;

            return base.TotalPages;
        }

    }

    private int _sumRowsTotalRows = 0;
    public override int TotalRows
    {
        get
        {
            if (SumMode)
                return _sumRowsTotalRows;

            return base.TotalRows;
        }

    }

    #region SumMode
    private bool _sumMode;
    public bool SumMode
    {
        get
        {
            return _sumMode;
        }
        set
        {
            _sumMode = value;
            OnPropertyChanged();
            UpdateFormList();
            UpdatePageInfo();
        }
    }
    #endregion

    #endregion

    #region Functions

    #region UpdateFormList
    public override void UpdateFormList()
    {
        if (SumMode)
            SumUpFormList();
        else
            base.UpdateFormList();

    }

    #endregion

    #region SumUpFormList
    public void SumUpFormList()
    {
        var rows21 = StaticConfiguration.DBModel.form_21
            .Where(row21 => row21.ReportId == Report.Id)
            .AsNoTracking()
            .AsEnumerable()
            .OrderBy(row21 => row21.RefineMachineName_DB)
            .ThenBy(row21 => row21.MachineCode_DB)
            .ThenBy(row21 => row21.MachinePower_DB)
            .ThenBy(row21 => row21.NumberOfHoursPerYear_DB)
            .ThenBy(row21 => row21.CodeRAOIn_DB)
            .ThenBy(row21 => row21.StatusRAOIn_DB)
            .ThenBy(row21 => row21.CodeRAOout_DB)
            .ThenBy(row21 => row21.StatusRAOout_DB)
            .ToList();

        var resultRows21 = new List<Form21>();


        var refineMachineGroups = rows21.GroupBy(row21 =>
            (row21.RefineMachineName_DB,
            row21.MachineCode_DB,
            row21.MachinePower_DB,
            row21.NumberOfHoursPerYear_DB));

        foreach (var refineMachineGroup in refineMachineGroups)
        {
            var leftRaoGroups = refineMachineGroup.GroupBy(row21 =>
            (row21.CodeRAOIn_DB,
            row21.StatusRAOIn_DB
            )).ToList(); 

            var rightRaoGroups = refineMachineGroup.GroupBy(row21 =>
            (row21.CodeRAOout_DB,
            row21.StatusRAOout_DB
            )).ToList();

            if (leftRaoGroups.Count > 1 || rightRaoGroups.Count > 1)
            {
                var firstElement = refineMachineGroup.ToList()[0];
                var titleIndex = rows21.IndexOf(firstElement);
                var titleRow = new Form21()
                {
                    RefineMachineName_DB = firstElement.RefineMachineName_DB, //2
                    MachineCode_DB = firstElement.MachineCode_DB, //3
                    MachinePower_DB = firstElement.MachinePower_DB, //4
                    NumberOfHoursPerYear_DB = firstElement.NumberOfHoursPerYear_DB, //5
                    //SumUp LeftGroup
                    VolumeIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.VolumeIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    MassIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.MassIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    QuantityIn_DB = refineMachineGroup.Sum(row21 => int.TryParse(row21.QuantityIn_DB, out var value) ? value : 0).ToString(),
                    TritiumActivityIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.TritiumActivityIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    BetaGammaActivityIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.BetaGammaActivityIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    AlphaActivityIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.AlphaActivityIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    TransuraniumActivityIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.TransuraniumActivityIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    //SumUp RightGroup
                    VolumeOut_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.VolumeOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    MassOut_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.MassOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    QuantityOZIIIout_DB = refineMachineGroup.Sum(row21 => int.TryParse(row21.QuantityOZIIIout_DB.Replace('.', ','), out var value) ? value : 0).ToString(),
                    TritiumActivityOut_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.TritiumActivityOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    BetaGammaActivityOut_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.BetaGammaActivityOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    AlphaActivityOut_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.AlphaActivityOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    TransuraniumActivityOut_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.TransuraniumActivityOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3"),
                    RowColor = Color.FromArgb(75, 204, 102, 0),
                };



                resultRows21.Add(titleRow);

                for(int i= 0; i< leftRaoGroups.Count || i< rightRaoGroups.Count; i++) 
                {
                    List<Form21>? leftRaoGroup = null;
                    if (i < leftRaoGroups.Count)
                        leftRaoGroup = leftRaoGroups[i].ToList();

                    List<Form21>? rightRaoGroup = null;
                    if (i < rightRaoGroups.Count)
                        rightRaoGroup = rightRaoGroups[i].ToList();



                    
                    var raoInfoRow = new Form21()
                    {
                        //RowColor = Color.FromArgb(50, 204, 204, 0),

                        RefineMachineName_DB = firstElement.RefineMachineName_DB, //2
                        MachineCode_DB = firstElement.MachineCode_DB, //3
                        MachinePower_DB = firstElement.MachinePower_DB, //4
                        NumberOfHoursPerYear_DB = firstElement.NumberOfHoursPerYear_DB, //5
                    };

                    if (leftRaoGroup != null)
                    {
                        var firstLeftRaoGroupElement = leftRaoGroup[0];

                        raoInfoRow.CodeRAOIn_DB = firstLeftRaoGroupElement.CodeRAOIn_DB; //6
                        raoInfoRow.StatusRAOIn_DB = firstLeftRaoGroupElement.StatusRAOIn_DB; //7
                        //SumUp LeftGroup
                        raoInfoRow.VolumeIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.VolumeIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");
                        raoInfoRow.MassIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.MassIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");
                        raoInfoRow.QuantityIn_DB = leftRaoGroup.Sum(row21 => int.TryParse(row21.QuantityIn_DB.Replace('.', ','), out var value) ? value : 0).ToString();
                        raoInfoRow.TritiumActivityIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.TritiumActivityIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");
                        raoInfoRow.BetaGammaActivityIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.BetaGammaActivityIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");
                        raoInfoRow.AlphaActivityIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.AlphaActivityIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");
                        raoInfoRow.TransuraniumActivityIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.TransuraniumActivityIn_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");
                    }

                    if(rightRaoGroup != null)
                    {
                        var firstRightRaoGroupElement = rightRaoGroup[0];

                        raoInfoRow.CodeRAOout_DB = firstRightRaoGroupElement.CodeRAOout_DB; //15
                        raoInfoRow.StatusRAOout_DB = firstRightRaoGroupElement.StatusRAOout_DB; //16
                        //SumUp RightGroup
                        raoInfoRow.VolumeOut_DB = rightRaoGroup.Sum(row21 => double.TryParse(row21.VolumeOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");
                        raoInfoRow.MassOut_DB = rightRaoGroup.Sum(row21 => double.TryParse(row21.MassOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");
                        raoInfoRow.QuantityOZIIIout_DB = rightRaoGroup.Sum(row21 => int.TryParse(row21.QuantityOZIIIout_DB.Replace('.', ','), out var value) ? value : 0).ToString();
                        raoInfoRow.TritiumActivityOut_DB = rightRaoGroup.Sum(row21 => double.TryParse(row21.TritiumActivityOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");
                        raoInfoRow.BetaGammaActivityOut_DB = rightRaoGroup.Sum(row21 => double.TryParse(row21.BetaGammaActivityOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");
                        raoInfoRow.AlphaActivityOut_DB = rightRaoGroup.Sum(row21 => double.TryParse(row21.AlphaActivityOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");
                        raoInfoRow.TransuraniumActivityOut_DB = rightRaoGroup.Sum(row21 => double.TryParse(row21.TransuraniumActivityOut_DB.Replace('.', ','), out var value) ? value : 0).ToString("e3");

                    }
                    resultRows21.Add(raoInfoRow);

                    var totalLength = int.Max(
                        leftRaoGroup?.Count ?? 0, rightRaoGroup?.Count ?? 0);

                    for (int j =0; j< totalLength; j++)
                    {
                        Form21? leftRao = null;
                        if (leftRaoGroup!= null &&j < leftRaoGroup.Count)
                            leftRao = leftRaoGroup[j];

                        Form21? rightRao = null;
                        if (rightRaoGroup != null && j < rightRaoGroup.Count)
                            rightRao = rightRaoGroup[j];

                        
                    }
                }
            }
        }

        for (int i = 0; i < resultRows21.Count; i++)
        {
            resultRows21[i].NumberInOrder_DB = i + 1;
        }

        FormList = new ObservableCollection<Form>(
           resultRows21.Skip((CurrentPage - 1) * RowCount)
               .Take(RowCount)); //Нужна оптимизация

        _sumRowsTotalRows = resultRows21.Count;

        _sumRowsTotalPages = resultRows21.Count / RowCount;
        if (resultRows21.Count % RowCount != 0)
            _sumRowsTotalPages++;

    }
    #endregion

    public void ReplaceZeroValueOnDashSignInRow21(Form21 row21)
    {
        if (row21.VolumeIn_DB == 0.ToString("e3"))
            row21.VolumeIn_DB = "-";

        if (row21.MassIn_DB == 0.ToString("e3"))
            row21.MassIn_DB = "-";

        if (row21.QuantityIn_DB == 0.ToString())
            row21.QuantityIn_DB = "-";

        if (row21.TritiumActivityIn_DB == 0.ToString("e3"))
            row21.TritiumActivityIn_DB = "-";

        if (row21.BetaGammaActivityIn_DB == 0.ToString("e3"))
            row21.BetaGammaActivityIn_DB = "-";

        if (row21.AlphaActivityIn_DB == 0.ToString())
            row21.AlphaActivityIn_DB = "-";

        if (row21.TransuraniumActivityIn_DB == 0.ToString("e3"))
            row21.TransuraniumActivityIn_DB = "-";

        if (row21.VolumeOut_DB == 0.ToString("e3"))
            row21.VolumeOut_DB = "-";

        if (row21.MassOut_DB == 0.ToString("e3"))
            row21.MassOut_DB = "-";

        if (row21.QuantityOZIIIout_DB == 0.ToString())
            row21.QuantityOZIIIout_DB = "-";

        if (row21.TritiumActivityOut_DB == 0.ToString("e3"))
            row21.TritiumActivityOut_DB = "-";

        if (row21.BetaGammaActivityOut_DB == 0.ToString("e3"))
            row21.BetaGammaActivityOut_DB = "-";

        if (row21.AlphaActivityOut_DB == 0.ToString())
            row21.AlphaActivityOut_DB = "-";

        if (row21.TransuraniumActivityOut_DB == 0.ToString("e3"))
            row21.TransuraniumActivityOut_DB = "-";
    }

    #endregion


    #region Commands

    public ICommand CategoryCalculationFromReport => new CategoryCalculationFromReportAsyncCommand();
    public ICommand CopyPasName => new CopyPasNameAsyncCommand();
    public ICommand ExcelExportSourceMovementHistory => new ExcelExportSourceMovementHistoryAsyncCommand();
    public ICommand OpenPas => new OpenPasAsyncCommand();
    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    #endregion
}