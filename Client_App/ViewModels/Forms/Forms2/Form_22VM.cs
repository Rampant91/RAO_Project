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
using System.Drawing;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms2;

public class Form_22VM : BaseFormVM
{
    public override string FormType => "2.2";

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
            .AsEnumerable()
            .OrderBy(row22 => row22.StoragePlaceCode_DB)
            .ThenBy(row22 => row22.StoragePlaceName_DB)
            .ThenBy(row22 => row22.PackName_DB)
            .ThenBy(row22 => row22.PackType_DB)
            .ThenBy(row22 => row22.CodeRAO_DB)
            .ThenBy(row22 => row22.StatusRAO_DB)
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
                var titleIndex = rows22.IndexOf(firstElement);
                var titleRow = new Form22()
                {
                    StoragePlaceName_DB = firstElement.StoragePlaceName_DB, //2
                    StoragePlaceCode_DB = firstElement.StoragePlaceCode_DB, //3
                    PackName_DB = firstElement.PackName_DB, //4
                    PackType_DB = firstElement.PackType_DB, //5
                    //SumUp
                    PackQuantity_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.PackQuantity_DB, out var value) ? value : 0).ToString(),
                    VolumeOutOfPack_DB = storageAndPackageTypeGroup.Sum(row22 => double.TryParse(row22.VolumeOutOfPack_DB, out var value) ? value : 0).ToString("e3"),
                    VolumeInPack_DB = storageAndPackageTypeGroup.Sum(row22 => double.TryParse(row22.VolumeInPack_DB, out var value) ? value : 0).ToString("e3"),
                    MassOutOfPack_DB = storageAndPackageTypeGroup.Sum(row22 => double.TryParse(row22.MassOutOfPack_DB, out var value) ? value : 0).ToString("e3"),
                    MassInPack_DB = storageAndPackageTypeGroup.Sum(row22 => double.TryParse(row22.MassInPack_DB, out var value) ? value : 0).ToString("e3"),
                    QuantityOZIII_DB = storageAndPackageTypeGroup.Sum(row22 => int.TryParse(row22.QuantityOZIII_DB, out var value) ? value : 0).ToString(),
                    TritiumActivity_DB = storageAndPackageTypeGroup.Sum(row22 => double.TryParse(row22.TritiumActivity_DB, out var value) ? value : 0).ToString("e3"),
                    BetaGammaActivity_DB = storageAndPackageTypeGroup.Sum(row22 => double.TryParse(row22.BetaGammaActivity_DB, out var value) ? value : 0).ToString("e3"),
                    AlphaActivity_DB = storageAndPackageTypeGroup.Sum(row22 => double.TryParse(row22.AlphaActivity_DB, out var value) ? value : 0).ToString("e3"),
                    TransuraniumActivity_DB = storageAndPackageTypeGroup.Sum(row22 => double.TryParse(row22.TransuraniumActivity_DB, out var value) ? value : 0).ToString("e3"),
                    MainRadionuclids_DB = "-",//18
                    RowColor = Color.FromArgb(75, 204, 102, 0),
                };

                ReplaceZeroValueOnDashSignInRow22(titleRow);

                rows22.Insert(titleIndex, titleRow);

                foreach (var raoGroup in raoGroups)
                {
                    var firstRaoGroupElement = raoGroup.ToList()[0];
                    var raoInfoIndex = rows22.IndexOf(firstRaoGroupElement);


                    var raoInfoRow = new Form22()
                    {
                        CodeRAO_DB = firstRaoGroupElement.CodeRAO_DB, //7
                        StatusRAO_DB = firstRaoGroupElement.StatusRAO_DB, //8

                        //SumUp
                        PackQuantity_DB = raoGroup.Sum(row22 => int.TryParse(row22.PackQuantity_DB, out var value) ? value : 0).ToString(),//6
                        VolumeOutOfPack_DB = raoGroup.Sum(row22 => double.TryParse(row22.VolumeOutOfPack_DB, out var value) ? value : 0).ToString("e3"),//9
                        VolumeInPack_DB = "-",//10
                        MassOutOfPack_DB = raoGroup.Sum(row22 => double.TryParse(row22.MassOutOfPack_DB, out var value) ? value : 0).ToString("e3"),//11
                        MassInPack_DB = "-",//12
                        QuantityOZIII_DB = raoGroup.Sum(row22 => int.TryParse(row22.QuantityOZIII_DB, out var value) ? value : 0).ToString(),//13
                        TritiumActivity_DB = raoGroup.Sum(row22 => double.TryParse(row22.TritiumActivity_DB, out var value) ? value : 0).ToString("e3"),//14
                        BetaGammaActivity_DB = raoGroup.Sum(row22 => double.TryParse(row22.BetaGammaActivity_DB, out var value) ? value : 0).ToString("e3"),//15
                        AlphaActivity_DB = raoGroup.Sum(row22 => double.TryParse(row22.AlphaActivity_DB, out var value) ? value : 0).ToString("e3"),//16
                        TransuraniumActivity_DB = raoGroup.Sum(row22 => double.TryParse(row22.TransuraniumActivity_DB, out var value) ? value : 0).ToString("e3"),//17
                        
                        RowColor = Color.FromArgb(50, 204, 204, 0),
                    };
                    foreach(var row22 in raoGroup)
                    {
                        //Суммируем 18 столбец
                        var radionuclidList = row22.MainRadionuclids_DB.Split(";").ToList();
                        for (int i =0; i< radionuclidList.Count; i++)
                        {
                            radionuclidList[i] = radionuclidList[i].Trim();
                            if (string.IsNullOrWhiteSpace(radionuclidList[i])) 
                                continue; 

                            if (!raoInfoRow.MainRadionuclids_DB.Split("; ")
                                .Any(radionuclid => radionuclid == radionuclidList[i]))
                            {
                                if (!string.IsNullOrWhiteSpace(raoInfoRow.MainRadionuclids_DB))
                                    raoInfoRow.MainRadionuclids_DB += "; ";

                                raoInfoRow.MainRadionuclids_DB += radionuclidList[i];
                            }
                        }

                        //из строки очищаем графы 2-5, т.к. эта информация записана в titleRow (строке объединения)
                        row22.StoragePlaceName_DB = ""; //2
                        row22.StoragePlaceCode_DB = ""; //3
                        row22.PackName_DB = ""; //4
                        row22.PackType_DB = ""; //5
                        row22.CodeRAO_DB = ""; //7
                        row22.StatusRAO_DB = ""; //8
                    }

                    ReplaceZeroValueOnDashSignInRow22(raoInfoRow);

                    rows22.Insert(raoInfoIndex, raoInfoRow);
                }
            }
        }

        for(int i=0; i<rows22.Count;i++)
        {
            rows22[i].NumberInOrder_DB = i + 1;
        }

        FormList = new ObservableCollection<Form>(
           rows22.Skip((CurrentPage - 1) * RowCount)
               .Take(RowCount)); //Нужна оптимизация

        _sumRowsTotalRows = rows22.Count;

        _sumRowsTotalPages = rows22.Count / RowCount;
        if (rows22.Count % RowCount != 0)
            _sumRowsTotalPages++;

    }

    public void ReplaceZeroValueOnDashSignInRow22(Form22 row22)
    {
        if (row22.PackQuantity_DB == 0.ToString())
            row22.PackQuantity_DB = "-";

        if (row22.VolumeOutOfPack_DB == 0.ToString("e3"))
            row22.VolumeOutOfPack_DB = "-";

        if (row22.VolumeInPack_DB == 0.ToString("e3"))
            row22.VolumeInPack_DB = "-";

        if (row22.MassOutOfPack_DB == 0.ToString("e3"))
            row22.MassOutOfPack_DB = "-";

        if (row22.MassInPack_DB == 0.ToString("e3"))
            row22.MassInPack_DB = "-";

        if (row22.QuantityOZIII_DB == 0.ToString())
            row22.QuantityOZIII_DB = "-";

        if (row22.TritiumActivity_DB == 0.ToString("e3"))
            row22.TritiumActivity_DB = "-";

        if (row22.BetaGammaActivity_DB == 0.ToString("e3"))
            row22.BetaGammaActivity_DB = "-";

        if (row22.AlphaActivity_DB == 0.ToString("e3"))
            row22.AlphaActivity_DB = "-";

        if (row22.TransuraniumActivity_DB == 0.ToString("e3"))
            row22.TransuraniumActivity_DB = "-";
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