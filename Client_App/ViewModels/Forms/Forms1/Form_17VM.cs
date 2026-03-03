using Client_App.Commands.AsyncCommands.Generate;
using Client_App.Commands.AsyncCommands.Generate.GenerateForm4;
using Client_App.ViewModels.Controls;
using Models.Collections;
using System;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_17VM : BaseFormVM
{
    public override string FormType => "1.7";

    #region Constructors

    public Form_17VM() { }

    public Form_17VM(Report report) : base(report) { }

    public Form_17VM(in Reports reps)
    {
        var formNum = FormType;
        Report = new Report
        {
            FormNum_DB = formNum,
            StartPeriod =
            {
                Value = reps.Report_Collection
                    .Where(x => x.FormNum_DB == formNum && DateOnly.TryParse(x.EndPeriod_DB, out _))
                    .OrderBy(x => DateOnly.Parse(x.EndPeriod_DB))
                    .Select(x => x.EndPeriod_DB)
                    .LastOrDefault() ?? ""
            },
            Reports = reps
        };

        base.InitializeUserControls();
        Reports = reps;

        SelectReportPopupVM = new SelectReportPopupVM(this);
    }

    #endregion
    #region Commands
    public ICommand GenerateForm17 => new GenerateForm17AsyncCommand(this);
    #endregion

}