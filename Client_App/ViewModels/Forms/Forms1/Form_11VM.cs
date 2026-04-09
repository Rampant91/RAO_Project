using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Calculator;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Collections;
using System;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_11VM : BaseFormVM
{
    public override string FormType => "1.1";

    /// <summary>
    /// Справочник кодов операции для AutoCompleteBox
    /// </summary>
    public string[] OperationCodes { get; } =
    [
        "01", "02", "03", "04", "05",  // Захоронение
        "11", "12", "13", "14", "15",  // Переработка
        "21", "22", "23", "24", "25",  // Обработка
        "31", "32", "33", "34", "35",  // Хранение
        "41", "42", "43", "44", "45",  // Транспортировка
        "51", "52", "53", "54", "55",  // Утилизация
        "61", "62", "63", "64", "65",  // Дезактивация
        "71", "72", "73", "74", "75",  // Кондиционирование
        "81", "82", "83", "84", "85"   // Сбор
    ];

    #region Constructors

    public Form_11VM() { }

    public Form_11VM(Report report) : base(report) { }

    public Form_11VM(in Reports reps)
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

    }

    #endregion

    #region Commands

    public ICommand CategoryCalculationFromReport => new CategoryCalculationFromReportAsyncCommand();
    public ICommand CopyPasName => new CopyPasNameAsyncCommand();
    public ICommand ExcelExportSourceMovementHistory => new ExcelExportSourceMovementHistoryAsyncCommand();
    public ICommand OpenPas => new OpenPasAsyncCommand();
    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    #endregion

    //public ObservableCollection<Form12> Form12List => new(FormList.Cast<Form12>());

    //public ObservableCollection<Form12> SelectedForms12 => new(SelectedForms.Cast<Form12>());

    //public Form12 SelectedForm12
    //{
    //    get => SelectedForm as Form12;
    //    set
    //    {
    //        SelectedForm = value;
    //        UpdateFormList();
    //    }
    //}

    //#region FilterProperty



    //#endregion

    /*
    #region UpdateFormList
    public new async void UpdateFormList()
    {
        base.UpdateFormList();
        
        //OnPropertyChanged(nameof(Form12List));
        //OnPropertyChanged(nameof(SelectedForms12));
        //OnPropertyChanged(nameof(SelectedForm12));
    }

    #endregion
    */
}