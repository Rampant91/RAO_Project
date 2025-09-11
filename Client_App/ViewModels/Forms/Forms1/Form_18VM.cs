using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Attributes;
using Models.Collections;
using System;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_18VM : BaseFormVM
{
    public override string FormType => "1.8";

    #region Constructors

    public Form_18VM() { }

    public Form_18VM(Report report) : base(report) { }

    public Form_18VM(in Reports reps)
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
            }
        };

        Reports = reps;

        formNum = formNum.Replace(".", "");
        WindowTitle = $"{((Form_ClassAttribute)Type.GetType($"Models.Forms.Form{formNum[0]}.Form{formNum},Models")!.GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name} "
                      + $"{Reports.Master_DB.RegNoRep.Value} "
                      + $"{Reports.Master_DB.ShortJurLicoRep.Value} "
                      + $"{Reports.Master_DB.OkpoRep.Value}";
    }

    #endregion

    #region Commands

    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    #endregion

    //public ObservableCollection<Form18> Form18List => new(FormList.Cast<Form18>());

    //public ObservableCollection<Form18> SelectedForms18 => new(SelectedForms.Cast<Form18>());

    //public Form18 SelectedForm18
    //{
    //    get => SelectedForm as Form18;
    //    set
    //    {
    //        SelectedForm = value;
    //        UpdateFormList();
    //    }
    //}

    /*
    #region UpdateFormList
    public new async void UpdateFormList()
    {
        base.UpdateFormList();
        
        //OnPropertyChanged(nameof(Form18List));
        //OnPropertyChanged(nameof(SelectedForms18));
        //OnPropertyChanged(nameof(SelectedForm18));
    }

    #endregion
    */
}