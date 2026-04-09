using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Calculator;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.Commands.SyncCommands;
using Client_App.ViewModels.Controls;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

/// <summary>
/// Код операции с описанием
/// </summary>
public class OperationCodeItem
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public override string ToString() => Code;
}

public class Form_11VM : BaseFormVM
{
    public override string FormType => "1.1";

    /// <summary>
    /// Справочник кодов операции для AutoCompleteBox с описаниями
    /// </summary>
    public ObservableCollection<OperationCodeItem> OperationCodes { get; } =
    [
        new() { Code = "01", Description = "Захоронение РАО" },
        new() { Code = "02", Description = "Захоронение РАО (временное)" },
        new() { Code = "03", Description = "Захоронение РАО (контрольное)" },
        new() { Code = "04", Description = "Захоронение РАО (другое)" },
        new() { Code = "05", Description = "Захоронение РАО (иное)" },
        new() { Code = "11", Description = "Переработка РАО" },
        new() { Code = "12", Description = "Переработка РАО (временная)" },
        new() { Code = "13", Description = "Переработка РАО (контрольная)" },
        new() { Code = "14", Description = "Переработка РАО (другое)" },
        new() { Code = "15", Description = "Переработка РАО (иное)" },
        new() { Code = "21", Description = "Обработка РАО" },
        new() { Code = "22", Description = "Обработка РАО (временная)" },
        new() { Code = "23", Description = "Обработка РАО (контрольная)" },
        new() { Code = "24", Description = "Обработка РАО (другое)" },
        new() { Code = "25", Description = "Обработка РАО (иное)" },
        new() { Code = "31", Description = "Хранение РАО" },
        new() { Code = "32", Description = "Хранение РАО (временное)" },
        new() { Code = "33", Description = "Хранение РАО (контрольное)" },
        new() { Code = "34", Description = "Хранение РАО (другое)" },
        new() { Code = "35", Description = "Хранение РАО (иное)" },
        new() { Code = "41", Description = "Транспортировка РАО" },
        new() { Code = "42", Description = "Транспортировка РАО (временная)" },
        new() { Code = "43", Description = "Транспортировка РАО (контрольная)" },
        new() { Code = "44", Description = "Транспортировка РАО (другое)" },
        new() { Code = "45", Description = "Транспортировка РАО (иное)" },
        new() { Code = "51", Description = "Утилизация РАО" },
        new() { Code = "52", Description = "Утилизация РАО (временная)" },
        new() { Code = "53", Description = "Утилизация РАО (контрольная)" },
        new() { Code = "54", Description = "Утилизация РАО (другое)" },
        new() { Code = "55", Description = "Утилизация РАО (иное)" },
        new() { Code = "61", Description = "Дезактивация РАО" },
        new() { Code = "62", Description = "Дезактивация РАО (временная)" },
        new() { Code = "63", Description = "Дезактивация РАО (контрольная)" },
        new() { Code = "64", Description = "Дезактивация РАО (другое)" },
        new() { Code = "65", Description = "Дезактивация РАО (иное)" },
        new() { Code = "71", Description = "Кондиционирование РАО" },
        new() { Code = "72", Description = "Кондиционирование РАО (временное)" },
        new() { Code = "73", Description = "Кондиционирование РАО (контрольное)" },
        new() { Code = "74", Description = "Кондиционирование РАО (другое)" },
        new() { Code = "75", Description = "Кондиционирование РАО (иное)" },
        new() { Code = "81", Description = "Сбор РАО" },
        new() { Code = "82", Description = "Сбор РАО (временный)" },
        new() { Code = "83", Description = "Сбор РАО (контрольный)" },
        new() { Code = "84", Description = "Сбор РАО (другое)" },
        new() { Code = "85", Description = "Сбор РАО (иное)" }
    ];

    /// <summary>
    /// Список допустимых кодов операции для валидации (только коды без описаний)
    /// </summary>
    public ICollection<string> ValidOperationCodes => OperationCodes.Select(x => x.Code).ToList();

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