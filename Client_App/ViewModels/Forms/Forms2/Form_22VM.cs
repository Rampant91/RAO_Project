using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Calculator;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.ViewModels.Controls;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form2;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms2;

public class Form_22VM : BaseFormVM
{
    public override string FormType => "2.2";

    #region Properties

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
        }
    }
    #endregion

    #endregion

    #region Constructors

    public Form_22VM() { }

    public Form_22VM(Report report) : base(report) { }

    public Form_22VM(in Reports reps)
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
        var rows22 = StaticConfiguration.DBModel.form_22
            .Where(row22 => row22.ReportId == Report.Id)
            .AsNoTracking()
            .ToList();

        var storageAndPackageTypeGroups = rows22.GroupBy(row22 =>
            (row22.StoragePlaceName_DB,
            row22.StoragePlaceCode_DB,
            row22.PackName_DB,
            row22.PackType_DB));

        foreach(var storageAndPackageTypeGroup in storageAndPackageTypeGroups)
        {
            var raoGroups = storageAndPackageTypeGroup.GroupBy(row22 =>
            (row22.CodeRAO_DB,
            row22.StatusRAO_DB
            ));
            if(raoGroups.Count()>1)
            {
                var firstElement = storageAndPackageTypeGroup.ToList()[0];
                var index = rows22.IndexOf(firstElement)-1;
                var titleRow = new Form22()
                {
                    StoragePlaceName_DB = firstElement.StoragePlaceName_DB, //2
                    StoragePlaceCode_DB = firstElement.StoragePlaceCode_DB, //3
                    PackName_DB = firstElement.PackName_DB, //4
                    PackType_DB = firstElement.PackType_DB, //5
                    //SumUp
                    PackQuantity_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.PackQuantity_DB, out var value) ? value : 0).ToString(),
                    VolumeOutOfPack_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.VolumeOutOfPack_DB, out var value) ? value : 0).ToString(),
                    VolumeInPack_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.VolumeInPack_DB, out var value) ? value : 0).ToString(),
                    MassOutOfPack_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.MassOutOfPack_DB, out var value) ? value : 0).ToString(),
                    MassInPack_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.MassInPack_DB, out var value) ? value : 0).ToString(),
                    QuantityOZIII_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.QuantityOZIII_DB, out var value) ? value : 0).ToString(),
                    TritiumActivity_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.TritiumActivity_DB, out var value) ? value : 0).ToString(),
                    BetaGammaActivity_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.BetaGammaActivity_DB, out var value) ? value : 0).ToString(),
                    AlphaActivity_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.AlphaActivity_DB, out var value) ? value : 0).ToString(),
                    TransuraniumActivity_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.TransuraniumActivity_DB, out var value) ? value : 0).ToString(),
                };
                #region Change "0" To "-"
                if (titleRow.PackQuantity_DB == "0")
                    titleRow.PackQuantity_DB = "-";

                if (titleRow.VolumeOutOfPack_DB == "0")
                    titleRow.VolumeOutOfPack_DB = "-";

                if (titleRow.VolumeInPack_DB == "0")
                    titleRow.VolumeInPack_DB = "-";

                if (titleRow.MassOutOfPack_DB == "0")
                    titleRow.MassOutOfPack_DB = "-";

                if (titleRow.MassInPack_DB == "0")
                    titleRow.MassInPack_DB = "-";

                if (titleRow.QuantityOZIII_DB == "0")
                    titleRow.QuantityOZIII_DB = "-";

                if (titleRow.TritiumActivity_DB == "0")
                    titleRow.TritiumActivity_DB = "-";

                if (titleRow.BetaGammaActivity_DB == "0")
                    titleRow.BetaGammaActivity_DB = "-";

                if (titleRow.AlphaActivity_DB == "0")
                    titleRow.AlphaActivity_DB = "-";

                if (titleRow.TransuraniumActivity_DB == "0")
                    titleRow.TransuraniumActivity_DB = "-";
                #endregion

                rows22.Insert(index, titleRow);
            }
        }



        FormList = new ObservableCollection<Form>(
           rows22.Skip((CurrentPage - 1) * RowCount)
               .Take(RowCount)); //Нужна оптимизация
    }

    #endregion

    #endregion

    #region Commands

    public ICommand CategoryCalculationFromReport => new CategoryCalculationFromReportAsyncCommand();
    public ICommand CopyPasName => new CopyPasNameAsyncCommand();
    public ICommand ExcelExportSourceMovementHistory => new ExcelExportSourceMovementHistoryAsyncCommand();
    public ICommand OpenPas => new OpenPasAsyncCommand();
    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    #endregion
}