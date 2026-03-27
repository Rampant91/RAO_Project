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
using System.Collections.ObjectModel;
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
            (row21.CodeRAOIn_DB,
            row21.StatusRAOIn_DB
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
                    //SumUp
                    VolumeIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.VolumeIn_DB, out var value) ? value : 0).ToString("e3"),
                    MassIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.MassIn_DB, out var value) ? value : 0).ToString("e3"),
                    QuantityIn_DB = refineMachineGroup.Sum(row21 => int.TryParse(row21.QuantityIn_DB, out var value) ? value : 0).ToString(),
                    TritiumActivityIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.TritiumActivityIn_DB, out var value) ? value : 0).ToString("e3"),
                    BetaGammaActivityIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.BetaGammaActivityIn_DB, out var value) ? value : 0).ToString("e3"),
                    AlphaActivityIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.AlphaActivityIn_DB, out var value) ? value : 0).ToString("e3"),
                    TransuraniumActivityIn_DB = refineMachineGroup.Sum(row21 => double.TryParse(row21.TransuraniumActivityIn_DB, out var value) ? value : 0).ToString("e3"),
                    RowColor = Color.FromArgb(75, 204, 102, 0),
                };



                rows21.Insert(titleIndex, titleRow);

                for(int i= 0; i< leftRaoGroups.Count && i< rightRaoGroups.Count; i++) 
                {
                    var leftRaoGroup = leftRaoGroups[i];
                    var rightRaoGroup = rightRaoGroups[i];

                    var firstLeftRaoGroupElement = leftRaoGroup.ToList()[0];
                    var raoInfoIndex = rows21.IndexOf(firstLeftRaoGroupElement);


                    var raoInfoRow = new Form21()
                    {
                        CodeRAOIn_DB = firstLeftRaoGroupElement.CodeRAOIn_DB, //6
                        StatusRAOIn_DB = firstLeftRaoGroupElement.StatusRAOIn_DB, //7

                        //SumUp
                        VolumeIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.VolumeIn_DB, out var value) ? value : 0).ToString("e3"),
                        MassIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.MassIn_DB, out var value) ? value : 0).ToString("e3"),
                        QuantityIn_DB = leftRaoGroup.Sum(row21 => int.TryParse(row21.QuantityIn_DB, out var value) ? value : 0).ToString(),
                        TritiumActivityIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.TritiumActivityIn_DB, out var value) ? value : 0).ToString("e3"),
                        BetaGammaActivityIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.BetaGammaActivityIn_DB, out var value) ? value : 0).ToString("e3"),
                        AlphaActivityIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.AlphaActivityIn_DB, out var value) ? value : 0).ToString("e3"),
                        TransuraniumActivityIn_DB = leftRaoGroup.Sum(row21 => double.TryParse(row21.TransuraniumActivityIn_DB, out var value) ? value : 0).ToString("e3"),
                        
                        RowColor = Color.FromArgb(50, 204, 204, 0),
                    };


                    rows21.Insert(raoInfoIndex, raoInfoRow);
                }
            }
        }

        for (int i = 0; i < rows21.Count; i++)
        {
            rows21[i].NumberInOrder_DB = i + 1;
        }

        FormList = new ObservableCollection<Form>(
           rows21.Skip((CurrentPage - 1) * RowCount)
               .Take(RowCount)); //Нужна оптимизация

        _sumRowsTotalRows = rows21.Count;

        _sumRowsTotalPages = rows21.Count / RowCount;
        if (rows21.Count % RowCount != 0)
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