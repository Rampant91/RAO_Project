using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.ComponentModel;
using Models.DBRealization;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.SyncCommands;
using Client_App.Controls.DataGrid;
using Client_App.Controls.DataGrid.DataGrids;
using Client_App.Interfaces.Logger;
using Client_App.VisualRealization.Long_Visual;
using MessageBox.Avalonia.Models;
using Models.Forms;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Views;

public class FormChangeOrCreate : BaseWindow<ChangeOrCreateVM>
{
    private readonly string _param = "";

    #region Constructor

    public FormChangeOrCreate()
    {
#if DEBUG
        this.AttachDevTools();
#endif
    }

public FormChangeOrCreate(ChangeOrCreateVM param)
    {
        _param = param.FormType;
        DataContext = param;
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.WhenActivated(d =>
        {
            var vm = (ChangeOrCreateVM)ViewModel!;
            d(vm.ShowDialogIn.RegisterHandler(DoShowDialogAsync));
            d(vm.ShowDialog.RegisterHandler(DoShowDialogAsync));
            d(vm.ShowMessageT.RegisterHandler(DoShowDialogAsyncT));
        });

        Closing += OnStandardClosing;

        Init();
    }

    #endregion

    #region CheckPeriod

    /// <summary>
    /// Проверяет наличие отчёта с пересекающимся периодом.
    /// </summary>
    /// <param name="vm">Модель открытого отчёта.</param>
    /// <returns>Сообщение о наличии пересечения.</returns>
    private static async Task CheckPeriod(ChangeOrCreateVM vm)
    {
        var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
        if (vm.Storage.FormNum_DB is "1.0" or "2.0") return;
        var reps = vm.Storages;
        var reportCollection = reps.Report_Collection;
        var rep = vm.Storage;
        if (DateOnly.TryParse(rep.StartPeriod_DB, out var startPeriod)
            && DateOnly.TryParse(rep.EndPeriod_DB, out var endPeriod))
        {
            foreach (var currentReport in reportCollection.Where(x => x.FormNum_DB == rep.FormNum_DB && x.Id != rep.Id))
            {
                if (DateOnly.TryParse(currentReport.StartPeriod_DB, out var currentRepStartPeriod)
                    && DateOnly.TryParse(currentReport.EndPeriod_DB, out var currentRepEndPeriod)
                    && startPeriod < currentRepEndPeriod && endPeriod > currentRepStartPeriod)
                {
                    #region MessageFindIntersection

                    await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams()
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Пересечение",
                            ContentHeader = "Уведомление",
                            ContentMessage = $"У организации {reps.Master_DB.RegNoRep.Value}_{reps.Master_DB.OkpoRep.Value} " +
                                             $"{Environment.NewLine}присутствует отчёт по форме " +
                                             $"{currentReport.FormNum_DB} {currentReport.StartPeriod_DB}-{currentReport.EndPeriod_DB}" +
                                             $"{Environment.NewLine}пересекающийся с введённым периодом " +
                                             $"{rep.StartPeriod_DB}-{rep.EndPeriod_DB}.",
                            MinWidth = 450,
                            MinHeight = 170,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(desktop.MainWindow));

                    #endregion

                    return;
                }
            }
        }
    }

    #endregion

    #region RemoveEmptyForms

    /// <summary>
    /// Проверяет на пустые строчки и предлагает их удалить.
    /// </summary>
    /// <param name="vm">Модель открытого отчёта.</param>
    /// <returns>Сообщение с предложением удалить пустые строчки.</returns>
    private static async Task RemoveEmptyForms(ChangeOrCreateVM vm)
    {
        var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
        List<Form> formToDeleteList = [];
        var lst = vm.Storage[vm.FormType];
        foreach (var key in lst)
        {
            switch (vm.FormType)
            {
                #region 1.1

                case "1.1":
                {
                    var form = (Form11)key;
                    if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                        && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                        && string.IsNullOrWhiteSpace(form.PassportNumber_DB)
                        && string.IsNullOrWhiteSpace(form.Type_DB)
                        && string.IsNullOrWhiteSpace(form.Radionuclids_DB)
                        && string.IsNullOrWhiteSpace(form.FactoryNumber_DB)
                        && form.Quantity_DB is null
                        && string.IsNullOrWhiteSpace(form.Activity_DB)
                        && string.IsNullOrWhiteSpace(form.CreatorOKPO_DB)
                        && string.IsNullOrWhiteSpace(form.CreationDate_DB)
                        && form.Category_DB is null
                        && form.SignedServicePeriod_DB is null
                        && form.PropertyCode_DB is null
                        && string.IsNullOrWhiteSpace(form.Owner_DB)
                        && form.DocumentVid_DB is null
                        && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                        && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                        && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                        && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                        && string.IsNullOrWhiteSpace(form.PackName_DB)
                        && string.IsNullOrWhiteSpace(form.PackType_DB)
                        && string.IsNullOrWhiteSpace(form.PackNumber_DB))
                    {
                        formToDeleteList.Add(form);
                    }
                    break;
                }

                #endregion

                #region 1.2

                case "1.2":
                {
                    var form = (Form12)key;
                    if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                        && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                        && string.IsNullOrWhiteSpace(form.PassportNumber_DB)
                        && string.IsNullOrWhiteSpace(form.NameIOU_DB)
                        && string.IsNullOrWhiteSpace(form.FactoryNumber_DB)
                        && string.IsNullOrWhiteSpace(form.Mass_DB)
                        && string.IsNullOrWhiteSpace(form.CreatorOKPO_DB)
                        && string.IsNullOrWhiteSpace(form.CreationDate_DB)
                        && string.IsNullOrWhiteSpace(form.SignedServicePeriod_DB)
                        && form.PropertyCode_DB is null
                        && string.IsNullOrWhiteSpace(form.Owner_DB)
                        && form.DocumentVid_DB is null
                        && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                        && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                        && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                        && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                        && string.IsNullOrWhiteSpace(form.PackName_DB)
                        && string.IsNullOrWhiteSpace(form.PackType_DB)
                        && string.IsNullOrWhiteSpace(form.PackNumber_DB))
                    {
                        formToDeleteList.Add(form);
                    }
                    break;
                }

                #endregion

                #region 1.3

                case "1.3":
                    {
                        var form = (Form13)key;
                        if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                            && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                            && string.IsNullOrWhiteSpace(form.PassportNumber_DB)
                            && string.IsNullOrWhiteSpace(form.Type_DB)
                            && string.IsNullOrWhiteSpace(form.Radionuclids_DB)
                            && string.IsNullOrWhiteSpace(form.FactoryNumber_DB)
                            && string.IsNullOrWhiteSpace(form.Activity_DB)
                            && string.IsNullOrWhiteSpace(form.CreatorOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.CreationDate_DB)
                            && form.AggregateState_DB is null
                            && form.PropertyCode_DB is null
                            && string.IsNullOrWhiteSpace(form.Owner_DB)
                            && form.DocumentVid_DB is null
                            && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                            && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                            && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.PackName_DB)
                            && string.IsNullOrWhiteSpace(form.PackType_DB)
                            && string.IsNullOrWhiteSpace(form.PackNumber_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 1.4

                case "1.4":
                    {
                        var form = (Form14)key;
                        if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                            && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                            && string.IsNullOrWhiteSpace(form.PassportNumber_DB)
                            && string.IsNullOrWhiteSpace(form.Name_DB)
                            && form.Sort_DB is null
                            && string.IsNullOrWhiteSpace(form.Radionuclids_DB)
                            && string.IsNullOrWhiteSpace(form.Activity_DB)
                            && string.IsNullOrWhiteSpace(form.ActivityMeasurementDate_DB)
                            && string.IsNullOrWhiteSpace(form.Volume_DB)
                            && string.IsNullOrWhiteSpace(form.Mass_DB)
                            && form.AggregateState_DB is null
                            && form.PropertyCode_DB is null
                            && string.IsNullOrWhiteSpace(form.Owner_DB)
                            && form.DocumentVid_DB is null
                            && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                            && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                            && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.PackName_DB)
                            && string.IsNullOrWhiteSpace(form.PackType_DB)
                            && string.IsNullOrWhiteSpace(form.PackNumber_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 1.5

                case "1.5":
                    {
                        var form = (Form15)key;
                        if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                            && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                            && string.IsNullOrWhiteSpace(form.PassportNumber_DB)
                            && string.IsNullOrWhiteSpace(form.Type_DB)
                            && string.IsNullOrWhiteSpace(form.Radionuclids_DB)
                            && string.IsNullOrWhiteSpace(form.FactoryNumber_DB)
                            && form.Quantity_DB is null
                            && string.IsNullOrWhiteSpace(form.Activity_DB)
                            && string.IsNullOrWhiteSpace(form.CreationDate_DB)
                            && string.IsNullOrWhiteSpace(form.StatusRAO_DB)
                            && form.DocumentVid_DB is null
                            && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                            && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                            && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.PackName_DB)
                            && string.IsNullOrWhiteSpace(form.PackType_DB)
                            && string.IsNullOrWhiteSpace(form.PackNumber_DB)
                            && string.IsNullOrWhiteSpace(form.StoragePlaceName_DB)
                            && string.IsNullOrWhiteSpace(form.StoragePlaceCode_DB)
                            && string.IsNullOrWhiteSpace(form.RefineOrSortRAOCode_DB)
                            && string.IsNullOrWhiteSpace(form.Subsidy_DB)
                            && string.IsNullOrWhiteSpace(form.FcpNumber_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 1.6

                case "1.6":
                    {
                        var form = (Form16)key;
                        if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                            && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                            && string.IsNullOrWhiteSpace(form.CodeRAO_DB)
                            && string.IsNullOrWhiteSpace(form.StatusRAO_DB)
                            && string.IsNullOrWhiteSpace(form.Volume_DB)
                            && string.IsNullOrWhiteSpace(form.Mass_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityOZIII_DB)
                            && string.IsNullOrWhiteSpace(form.MainRadionuclids_DB)
                            && string.IsNullOrWhiteSpace(form.TritiumActivity_DB)
                            && string.IsNullOrWhiteSpace(form.BetaGammaActivity_DB)
                            && string.IsNullOrWhiteSpace(form.AlphaActivity_DB)
                            && string.IsNullOrWhiteSpace(form.TransuraniumActivity_DB)
                            && string.IsNullOrWhiteSpace(form.ActivityMeasurementDate_DB)
                            && form.DocumentVid_DB is null
                            && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                            && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                            && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.StoragePlaceName_DB)
                            && string.IsNullOrWhiteSpace(form.StoragePlaceCode_DB)
                            && string.IsNullOrWhiteSpace(form.RefineOrSortRAOCode_DB)
                            && string.IsNullOrWhiteSpace(form.PackName_DB)
                            && string.IsNullOrWhiteSpace(form.PackType_DB)
                            && string.IsNullOrWhiteSpace(form.PackNumber_DB)
                            && string.IsNullOrWhiteSpace(form.Subsidy_DB)
                            && string.IsNullOrWhiteSpace(form.FcpNumber_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 1.7

                case "1.7":
                    {
                        var form = (Form17)key;
                        if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                            && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                            && string.IsNullOrWhiteSpace(form.PackName_DB)
                            && string.IsNullOrWhiteSpace(form.PackType_DB)
                            && string.IsNullOrWhiteSpace(form.PackFactoryNumber_DB)
                            && string.IsNullOrWhiteSpace(form.PackNumber_DB)
                            && string.IsNullOrWhiteSpace(form.FormingDate_DB)
                            && string.IsNullOrWhiteSpace(form.PassportNumber_DB)
                            && string.IsNullOrWhiteSpace(form.Volume_DB)
                            && string.IsNullOrWhiteSpace(form.Mass_DB)
                            && string.IsNullOrWhiteSpace(form.Radionuclids_DB)
                            && string.IsNullOrWhiteSpace(form.SpecificActivity_DB)
                            && form.DocumentVid_DB is null
                            && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                            && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                            && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.StoragePlaceName_DB)
                            && string.IsNullOrWhiteSpace(form.StoragePlaceCode_DB)
                            && string.IsNullOrWhiteSpace(form.CodeRAO_DB)
                            && string.IsNullOrWhiteSpace(form.StatusRAO_DB)
                            && string.IsNullOrWhiteSpace(form.VolumeOutOfPack_DB)
                            && string.IsNullOrWhiteSpace(form.MassOutOfPack_DB)
                            && string.IsNullOrWhiteSpace(form.Quantity_DB)
                            && string.IsNullOrWhiteSpace(form.TritiumActivity_DB)
                            && string.IsNullOrWhiteSpace(form.BetaGammaActivity_DB)
                            && string.IsNullOrWhiteSpace(form.AlphaActivity_DB)
                            && string.IsNullOrWhiteSpace(form.TransuraniumActivity_DB)
                            && string.IsNullOrWhiteSpace(form.RefineOrSortRAOCode_DB)
                            && string.IsNullOrWhiteSpace(form.Subsidy_DB)
                            && string.IsNullOrWhiteSpace(form.FcpNumber_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 1.8

                case "1.8":
                    {
                        var form = (Form18)key;
                        if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                            && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                            && string.IsNullOrWhiteSpace(form.IndividualNumberZHRO_DB)
                            && string.IsNullOrWhiteSpace(form.PassportNumber_DB)
                            && string.IsNullOrWhiteSpace(form.Volume6_DB)
                            && string.IsNullOrWhiteSpace(form.Mass7_DB)
                            && string.IsNullOrWhiteSpace(form.SaltConcentration_DB)
                            && string.IsNullOrWhiteSpace(form.Radionuclids_DB)
                            && string.IsNullOrWhiteSpace(form.SpecificActivity_DB)
                            && form.DocumentVid_DB is null
                            && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                            && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                            && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                            && string.IsNullOrWhiteSpace(form.StoragePlaceName_DB)
                            && string.IsNullOrWhiteSpace(form.StoragePlaceCode_DB)
                            && string.IsNullOrWhiteSpace(form.CodeRAO_DB)
                            && string.IsNullOrWhiteSpace(form.StatusRAO_DB)
                            && string.IsNullOrWhiteSpace(form.Volume20_DB)
                            && string.IsNullOrWhiteSpace(form.Mass21_DB)
                            && string.IsNullOrWhiteSpace(form.TritiumActivity_DB)
                            && string.IsNullOrWhiteSpace(form.BetaGammaActivity_DB)
                            && string.IsNullOrWhiteSpace(form.AlphaActivity_DB)
                            && string.IsNullOrWhiteSpace(form.TransuraniumActivity_DB)
                            && string.IsNullOrWhiteSpace(form.RefineOrSortRAOCode_DB)
                            && string.IsNullOrWhiteSpace(form.Subsidy_DB)
                            && string.IsNullOrWhiteSpace(form.FcpNumber_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 1.9

                case "1.9":
                    {
                        var form = (Form19)key;
                        if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                            && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                            && form.DocumentVid_DB is null
                            && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                            && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                            && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                            && form.CodeTypeAccObject_DB is null
                            && string.IsNullOrWhiteSpace(form.Radionuclids_DB)
                            && string.IsNullOrWhiteSpace(form.Activity_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.1

                case "2.1":
                    {
                        var form = (Form21)key;
                        if (string.IsNullOrWhiteSpace(form.RefineMachineName_DB)
                            && form.MachineCode_DB is null
                            && string.IsNullOrWhiteSpace(form.MachinePower_DB)
                            && string.IsNullOrWhiteSpace(form.NumberOfHoursPerYear_DB)
                            && string.IsNullOrWhiteSpace(form.CodeRAOIn_DB)
                            && string.IsNullOrWhiteSpace(form.StatusRAOIn_DB)
                            && string.IsNullOrWhiteSpace(form.VolumeIn_DB)
                            && string.IsNullOrWhiteSpace(form.MassIn_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityIn_DB)
                            && string.IsNullOrWhiteSpace(form.TritiumActivityIn_DB)
                            && string.IsNullOrWhiteSpace(form.BetaGammaActivityIn_DB)
                            && string.IsNullOrWhiteSpace(form.AlphaActivityIn_DB)
                            && string.IsNullOrWhiteSpace(form.TransuraniumActivityIn_DB)
                            && string.IsNullOrWhiteSpace(form.CodeRAOout_DB)
                            && string.IsNullOrWhiteSpace(form.StatusRAOout_DB)
                            && string.IsNullOrWhiteSpace(form.VolumeOut_DB)
                            && string.IsNullOrWhiteSpace(form.MassOut_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityOZIIIout_DB)
                            && string.IsNullOrWhiteSpace(form.TritiumActivityOut_DB)
                            && string.IsNullOrWhiteSpace(form.BetaGammaActivityOut_DB)
                            && string.IsNullOrWhiteSpace(form.AlphaActivityOut_DB)
                            && string.IsNullOrWhiteSpace(form.TransuraniumActivityOut_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.2

                case "2.2":
                    {
                        var form = (Form22)key;
                        if (string.IsNullOrWhiteSpace(form.StoragePlaceName_DB)
                            && string.IsNullOrWhiteSpace(form.StoragePlaceCode_DB)
                            && string.IsNullOrWhiteSpace(form.PackName_DB)
                            && string.IsNullOrWhiteSpace(form.PackType_DB)
                            && string.IsNullOrWhiteSpace(form.PackQuantity_DB)
                            && string.IsNullOrWhiteSpace(form.CodeRAO_DB)
                            && string.IsNullOrWhiteSpace(form.StatusRAO_DB)
                            && string.IsNullOrWhiteSpace(form.VolumeOutOfPack_DB)
                            && string.IsNullOrWhiteSpace(form.VolumeInPack_DB)
                            && string.IsNullOrWhiteSpace(form.MassOutOfPack_DB)
                            && string.IsNullOrWhiteSpace(form.MassInPack_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityOZIII_DB)
                            && string.IsNullOrWhiteSpace(form.TritiumActivity_DB)
                            && string.IsNullOrWhiteSpace(form.BetaGammaActivity_DB)
                            && string.IsNullOrWhiteSpace(form.AlphaActivity_DB)
                            && string.IsNullOrWhiteSpace(form.TransuraniumActivity_DB)
                            && string.IsNullOrWhiteSpace(form.MainRadionuclids_DB)
                            && string.IsNullOrWhiteSpace(form.Subsidy_DB)
                            && string.IsNullOrWhiteSpace(form.FcpNumber_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.3

                case "2.3":
                    {
                        var form = (Form23)key;
                        if (string.IsNullOrWhiteSpace(form.StoragePlaceName_DB)
                            && string.IsNullOrWhiteSpace(form.StoragePlaceCode_DB)
                            && string.IsNullOrWhiteSpace(form.ProjectVolume_DB)
                            && string.IsNullOrWhiteSpace(form.CodeRAO_DB)
                            && string.IsNullOrWhiteSpace(form.Volume_DB)
                            && string.IsNullOrWhiteSpace(form.Mass_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityOZIII_DB)
                            && string.IsNullOrWhiteSpace(form.SummaryActivity_DB)
                            && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                            && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                            && string.IsNullOrWhiteSpace(form.ExpirationDate_DB)
                            && string.IsNullOrWhiteSpace(form.DocumentName_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.4

                case "2.4":
                    {
                        var form = (Form24)key;
                        if (string.IsNullOrWhiteSpace(form.CodeOYAT_DB)
                            && string.IsNullOrWhiteSpace(form.FcpNumber_DB)
                            && string.IsNullOrWhiteSpace(form.MassCreated_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityCreated_DB)
                            && string.IsNullOrWhiteSpace(form.MassFromAnothers_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityFromAnothers_DB)
                            && string.IsNullOrWhiteSpace(form.MassFromAnothersImported_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityFromAnothersImported_DB)
                            && string.IsNullOrWhiteSpace(form.MassAnotherReasons_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityAnotherReasons_DB)
                            && string.IsNullOrWhiteSpace(form.MassTransferredToAnother_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityTransferredToAnother_DB)
                            && string.IsNullOrWhiteSpace(form.MassRefined_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityRefined_DB)
                            && string.IsNullOrWhiteSpace(form.MassRemovedFromAccount_DB)
                            && string.IsNullOrWhiteSpace(form.QuantityRemovedFromAccount_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.5

                case "2.5":
                    {
                        var form = (Form25)key;
                        if (string.IsNullOrWhiteSpace(form.StoragePlaceName_DB)
                            && string.IsNullOrWhiteSpace(form.StoragePlaceCode_DB)
                            && string.IsNullOrWhiteSpace(form.CodeOYAT_DB)
                            && string.IsNullOrWhiteSpace(form.FcpNumber_DB)
                            && string.IsNullOrWhiteSpace(form.FuelMass_DB)
                            && string.IsNullOrWhiteSpace(form.CellMass_DB)
                            && form.Quantity_DB is null
                            && string.IsNullOrWhiteSpace(form.AlphaActivity_DB)
                            && string.IsNullOrWhiteSpace(form.BetaGammaActivity_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.6

                case "2.6":
                    {
                        var form = (Form26)key;
                        if (string.IsNullOrWhiteSpace(form.ObservedSourceNumber_DB)
                            && string.IsNullOrWhiteSpace(form.ControlledAreaName_DB)
                            && string.IsNullOrWhiteSpace(form.SupposedWasteSource_DB)
                            && string.IsNullOrWhiteSpace(form.DistanceToWasteSource_DB)
                            && string.IsNullOrWhiteSpace(form.TestDepth_DB)
                            && string.IsNullOrWhiteSpace(form.RadionuclidName_DB)
                            && string.IsNullOrWhiteSpace(form.AverageYearConcentration_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.7

                case "2.7":
                    {
                        var form = (Form27)key;
                        if (string.IsNullOrWhiteSpace(form.ObservedSourceNumber_DB)
                            && string.IsNullOrWhiteSpace(form.RadionuclidName_DB)
                            && string.IsNullOrWhiteSpace(form.AllowedWasteValue_DB)
                            && string.IsNullOrWhiteSpace(form.FactedWasteValue_DB)
                            && string.IsNullOrWhiteSpace(form.WasteOutbreakPreviousYear_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.8

                case "2.8":
                    {
                        var form = (Form28)key;
                        if (string.IsNullOrWhiteSpace(form.WasteSourceName_DB)
                            && string.IsNullOrWhiteSpace(form.WasteRecieverName_DB)
                            && string.IsNullOrWhiteSpace(form.RecieverTypeCode_DB)
                            && string.IsNullOrWhiteSpace(form.PoolDistrictName_DB)
                            && string.IsNullOrWhiteSpace(form.AllowedWasteRemovalVolume_DB)
                            && string.IsNullOrWhiteSpace(form.RemovedWasteVolume_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.9

                case "2.9":
                    {
                        var form = (Form29)key;
                        if (string.IsNullOrWhiteSpace(form.WasteSourceName_DB)
                            && string.IsNullOrWhiteSpace(form.RadionuclidName_DB)
                            && string.IsNullOrWhiteSpace(form.AllowedActivity_DB)
                            && string.IsNullOrWhiteSpace(form.FactedActivity_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.10

                case "2.10":
                    {
                        var form = (Form210)key;
                        if (string.IsNullOrWhiteSpace(form.IndicatorName_DB)
                            && string.IsNullOrWhiteSpace(form.PlotName_DB)
                            && string.IsNullOrWhiteSpace(form.PlotKadastrNumber_DB)
                            && string.IsNullOrWhiteSpace(form.PlotCode_DB)
                            && string.IsNullOrWhiteSpace(form.InfectedArea_DB)
                            && string.IsNullOrWhiteSpace(form.AvgGammaRaysDosePower_DB)
                            && string.IsNullOrWhiteSpace(form.MaxGammaRaysDosePower_DB)
                            && string.IsNullOrWhiteSpace(form.WasteDensityAlpha_DB)
                            && string.IsNullOrWhiteSpace(form.WasteDensityBeta_DB)
                            && string.IsNullOrWhiteSpace(form.FcpNumber_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.11

                case "2.11":
                    {
                        var form = (Form211)key;
                        if (string.IsNullOrWhiteSpace(form.PlotName_DB)
                            && string.IsNullOrWhiteSpace(form.PlotKadastrNumber_DB)
                            && string.IsNullOrWhiteSpace(form.PlotCode_DB)
                            && string.IsNullOrWhiteSpace(form.InfectedArea_DB)
                            && string.IsNullOrWhiteSpace(form.Radionuclids_DB)
                            && string.IsNullOrWhiteSpace(form.SpecificActivityOfPlot_DB)
                            && string.IsNullOrWhiteSpace(form.SpecificActivityOfLiquidPart_DB)
                            && string.IsNullOrWhiteSpace(form.SpecificActivityOfDensePart_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                #endregion

                #region 2.12

                case "2.12":
                    {
                        var form = (Form212)key;
                        if (form.OperationCode_DB is null
                            && form.ObjectTypeCode_DB is null
                            && string.IsNullOrWhiteSpace(form.Radionuclids_DB)
                            && string.IsNullOrWhiteSpace(form.Activity_DB)
                            && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB))
                        {
                            formToDeleteList.Add(form);
                        }
                        break;
                    }

                    #endregion
            }
        }

        if (formToDeleteList.Count != 0)
        {
            #region MessageRemoveEmptyForms

            var res = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да" },
                        new ButtonDefinition { Name = "Нет" }
                    ],
                    ContentTitle = "Сохранение изменений",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"В форме {vm.FormType} присутствуют пустые строчки." +
                                     $"{Environment.NewLine}Вы хотите их удалить?",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow));

            #endregion

            if (res is "Да")
            {
                await using var db = new DBModel(StaticConfiguration.DBPath);
                foreach (var form in formToDeleteList)
                {
                    vm.Storage.Rows.Remove(form);
                }
                var minItem = formToDeleteList.Min(x => x.Order);
                await vm.Storage.SortAsync();
                var itemQ = vm.Storage.Rows
                    .GetEnumerable()
                    .Where(x => x.Order > minItem)
                    .Select(x => (Form)x)
                    .ToArray();
                foreach (var form in itemQ)
                {
                    form.NumberInOrder_DB = (int)minItem;
                    form.NumberInOrder.OnPropertyChanged();
                    minItem++;
                }
                await db.SaveChangesAsync();
            }
        }
    }

    #endregion

    #region DoShowDialog

    private async Task DoShowDialogAsync(InteractionContext<int, int> interaction)
    {
        RowNumberIn frm = new(interaction.Input);
        await frm.ShowDialog(this);
        interaction.SetOutput(Convert.ToInt32(frm.Number2));
    }

    private async Task DoShowDialogAsync(InteractionContext<object, int> interaction)
    {
        RowNumber frm = new();
        await frm.ShowDialog(this);
        interaction.SetOutput(Convert.ToInt32(frm.Number));
    }

    private async Task DoShowDialogAsyncT(InteractionContext<List<string>, string> interaction)
    {
        MessageBoxCustomParams par = new()
        {
            ContentHeader = "Уведомление",
            ContentMessage = interaction.Input[0],
            ContentTitle = "Уведомление",
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        interaction.Input.RemoveAt(0);
        par.ButtonDefinitions = interaction.Input
            .Select(elem => new ButtonDefinition
            {
                Name = elem
            });

        var message = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(par);
        var answer = await message.ShowDialog(this);

        interaction.SetOutput(answer);
    }

    #endregion

    #region Init

    private void Form1Init(in Panel panel)
    {
        var dataContext = (ChangeOrCreateVM)DataContext;
        switch (_param)
        {
            #region 1.0
            
            case "1.0":
            {
                panel.Children.Add(Form1_Visual.Form10_Visual(this.FindNameScope()));
                break;
            }

            #endregion

            #region 1.1
            
            case "1.1":
            {
                var grd = (ScrollViewer)Form1_Visual.Form11_Visual(this.FindNameScope());

                #region Rows Context Menu

                var Rgrd = (DataGridForm11)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = ["Выделить все                                            Ctrl+A"],
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = ["Добавить строку                                      Ctrl+T"],
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = ["Добавить N строк                                    Ctrl+N"],
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = ["Добавить N строк перед                         Ctrl+I"],
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = ["Копировать                                               Ctrl+C"],
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Insert,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = false,
                    ParamName = "Copy",
                    ContextMenuText = ["Копировать                                               Ctrl+Ins"],
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = ["Вставить                                                    Ctrl+V"],
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    IsUpdateCells = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = ["Удалить строки                                         Ctrl+D"],
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = ["Выставить номер п/п                               Ctrl+O"],
                                        
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.L,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                    Command = dataContext.SortForm
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = ["Очистить ячейки                                      Delete"],
                    Command = dataContext.DeleteDataInRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.P,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = ["Открыть паспорт                                      Ctrl+P"],
                    Command = dataContext.OpenPas
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.P,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = ["Рассчитать категорию"],
                    Command = dataContext.CategoryCalculationFromReport
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.E,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = ["Выгрузка в Excel движения источника   Ctrl+E"],
                    Command = dataContext.ExcelExportSourceMovementHistory
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.K,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = ["Скопировать в буфер имя паспорта      Ctrl+K"],
                    Command = dataContext.CopyPasName
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Z,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = ["Перевести источник в РАО                     Ctrl+Z"],
                    Command = dataContext.SourceTransmission
                });

                #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }

            #endregion

            #region 1.2
            
            case "1.2":
            {
                var grd = (ScrollViewer)Form1_Visual.Form12_Visual(this.FindNameScope());

                #region Rows Context Menu

                    var Rgrd = (DataGridForm12)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                                 Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку                           Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк                         Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед              Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                                   Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                                         Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                              Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п                    Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п        Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки                           Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Z,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Перевести источник в РАО          Ctrl+Z"],
                        Command = dataContext.SourceTransmission
                    });

                    #endregion

                #region Notes Context Menu

                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });

                    #endregion

                panel.Children.Add(grd);
                break;
            }

            #endregion

            #region 1.3
            
            case "1.3":
            {
                var grd = (ScrollViewer)Form1_Visual.Form13_Visual(this.FindNameScope());

                #region Rows Context Menu

                    var Rgrd = (DataGridForm13)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                            Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку                      Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк                    Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед         Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                              Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                                   Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                        Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки                     Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Z,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Перевести источник в РАО    Ctrl+Z"],
                        Command = dataContext.SourceTransmission
                    });

                    #endregion

                #region Notes Context Menu

                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });

                    #endregion

                panel.Children.Add(grd);
                break;
            }

            #endregion

            #region 1.4
            
            case "1.4":
            {
                var grd = (ScrollViewer)Form1_Visual.Form14_Visual(this.FindNameScope());

                #region Rows Context Menu

                    var Rgrd = (DataGridForm14)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                           Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку                      Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк                    Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед         Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                              Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                                   Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                        Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки                     Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Z,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Перевести источник в РАО    Ctrl+Z"],
                        Command = dataContext.SourceTransmission
                    });

                    #endregion

                #region Notes Context Menu

                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });

                    #endregion

                panel.Children.Add(grd);
                break;
            }

            #endregion

            #region 1.5
            
            case "1.5":
            {
                var grd = (ScrollViewer)Form1_Visual.Form15_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm15)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    //Rgrd.CommandsList.Add(new KeyCommand
                    //{
                    //    Key = Avalonia.Input.Key.P,
                    //    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    //    IsDoubleTappedCommand = false,
                    //    IsContextMenuCommand = true,
                    //    ParamName = "Copy",
                    //    ContextMenuText = new[] { "Открыть паспорт                                      Ctrl+P" },
                    //    Command = dataContext.OpenPassport
                    //});
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.E,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Выгрузка в Excel движения источника   Ctrl+E"],
                        Command = dataContext.ExcelExportSourceMovementHistory
                    });
                    //Rgrd.CommandsList.Add(new KeyCommand
                    //{
                    //    Key = Avalonia.Input.Key.K,
                    //    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    //    IsDoubleTappedCommand = false,
                    //    IsContextMenuCommand = true,
                    //    ParamName = "Copy",
                    //    ContextMenuText = new[] { "Скопировать в буфер имя паспорта      Ctrl+K" },
                    //    Command = dataContext.CopyPasName
                    //});
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }

            #endregion

            #region 1.6
            
            case "1.6":
            {
                var grd = (ScrollViewer)Form1_Visual.Form16_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm16)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }

            #endregion

            #region 1.7
            
            case "1.7":
            {
                var grd = (ScrollViewer)Form1_Visual.Form17_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm17)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.P,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Паспорт для упаковки                    Ctrl+P"],
                        Command = dataContext.PassportFill
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.P,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control | Avalonia.Input.KeyModifiers.Shift,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Паспорта для всех упаковок              Ctrl+Shift+P"],
                        Command = dataContext.PassportFillAll
                    });
                    #endregion

                    #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }

            #endregion

            #region 1.8
            
            case "1.8":
            {
                var grd = (ScrollViewer)Form1_Visual.Form18_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm18)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }

            #endregion

            #region 1.9
            
            case "1.9":
            {
                var grd = (ScrollViewer)Form1_Visual.Form19_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm19)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            } 
                
            #endregion
        }
    }

    private void Form2Init(in Panel panel)
    {
        var dataContext = (ChangeOrCreateVM)DataContext;
        switch (_param)
        {
            #region 2.0
            
            case "2.0":
            {
                panel.Children.Add(Form2_Visual.Form20_Visual(this.FindNameScope()));
                break;
            }

            #endregion

            #region 2.1
            
            case "2.1":
            {
                var grd = (ScrollViewer)Form2_Visual.Form21_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm21)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }

            #endregion

            #region 2.2
            
            case "2.2":
            {
                var grd = (ScrollViewer)Form2_Visual.Form22_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm22)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }

            #endregion

            #region 2.3

            case "2.3":
            {
                var grd = (ScrollViewer)Form2_Visual.Form23_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm23)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }
            
            #endregion

            #region 2.4
            
            case "2.4":
            {
                var grd = (ScrollViewer)Form2_Visual.Form24_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm24)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }
            
            #endregion

            #region 2.5
            
            case "2.5":
            {
                var grd = (ScrollViewer)Form2_Visual.Form25_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm25)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }
            
            #endregion

            #region 2.6
            
            case "2.6":
            {
                var grd = (ScrollViewer)Form2_Visual.Form26_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm26)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }
            
            #endregion

            #region 2.7
            
            case "2.7":
            {
                var grd = (ScrollViewer)Form2_Visual.Form27_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm27)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }
            
            #endregion

            #region 2.8
            
            case "2.8":
            {
                var grd = (ScrollViewer)Form2_Visual.Form28_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm28)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }
            
            #endregion

            #region 2.9
            
            case "2.9":
            {
                var grd = (ScrollViewer)Form2_Visual.Form29_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm29)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }
            
            #endregion

            #region 2.10
            
            case "2.10":
            {
                var grd = (ScrollViewer)Form2_Visual.Form210_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm210)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }
            
            #endregion

            #region 2.11
            
            case "2.11":
            {
                var grd = (ScrollViewer)Form2_Visual.Form211_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm211)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            }
            
            #endregion

            #region 2.12
            
            case "2.12":
            {
                var grd = (ScrollViewer)Form2_Visual.Form212_Visual(this.FindNameScope());

                #region Rows Context Menu
                    var Rgrd = (DataGridForm212)((StackPanel)grd.Content).Children[1];
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.A,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectAll",
                        ContextMenuText = ["Выделить все                    Ctrl+A"],
                        Command = null
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку              Ctrl+T"],
                        Command = dataContext.AddRow
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк            Ctrl+N"],
                        Command = dataContext.AddRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.I,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Добавить N строк перед Ctrl+I"],
                        Command = dataContext.AddRowsIn
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                      Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                            Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки                 Ctrl+D"],
                        Command = dataContext.DeleteRows
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.O,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Выставить номер п/п              Ctrl+O"],
                        Command = dataContext.SetNumberOrder
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.L,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Сортировать по номеру п/п                    Ctrl+L"],
                        Command = dataContext.SortForm
                    });
                    Rgrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                #region Notes Context Menu
                    var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.T,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить строку          Ctrl+T"],
                        Command = dataContext.AddNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.N,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "",
                        ContextMenuText = ["Добавить N строк        Ctrl+N"],
                        Command = dataContext.AddNotes
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.C,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Copy",
                        ContextMenuText = ["Копировать                  Ctrl+C"],
                        Command = dataContext.CopyRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.V,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Paste",
                        ContextMenuText = ["Вставить                        Ctrl+V"],
                        Command = dataContext.PasteRows
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.D,
                        KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "SelectedItems",
                        ContextMenuText = ["Удалить строки             Ctrl+D"],
                        Command = dataContext.DeleteNote
                    });
                    Ngrd.CommandsList.Add(new KeyCommand
                    {
                        Key = Avalonia.Input.Key.Delete,
                        IsDoubleTappedCommand = false,
                        IsContextMenuCommand = true,
                        ParamName = "Del",
                        ContextMenuText = ["Очистить ячейки              Delete"],
                        Command = dataContext.DeleteDataInRows
                    });
                    #endregion

                panel.Children.Add(grd);
                break;
            } 
            
            #endregion
        }
    }

    private void Init()
    {
        var panel = this.FindControl<Panel>("ChangingPanel");
        Form1Init(panel);
        Form2Init(panel);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    #endregion

    #region OnStandartClosing

    private async void OnStandardClosing(object? sender, CancelEventArgs args)
    {
        if (DataContext is not ChangeOrCreateVM vm) return;
        try
        {
            await RemoveEmptyForms(vm);
            await CheckPeriod(vm);
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" + 
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }
        var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!;
        try
        {
            if (!StaticConfiguration.DBModel.ChangeTracker.HasChanges())
            {
                desktop.MainWindow.WindowState = WindowState.Normal;
                return;
            }
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" + 
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }

        var flag = false;

        #region MessageRemoveEmptyForms

        var res = Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" }
                ],
                ContentTitle = "Сохранение изменений",
                ContentHeader = "Уведомление",
                ContentMessage = $"Сохранить форму {vm.FormType}?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(desktop.MainWindow));

        #endregion

        await res.WaitAsync(new CancellationToken());
        var dbm = StaticConfiguration.DBModel;
        switch (res.Result)
        {
            case "Да":
            {
                await dbm.SaveChangesAsync();
                await new SaveReportAsyncCommand(vm).AsyncExecute(null);
                if (desktop.Windows.Count == 1)
                {
                    desktop.MainWindow.WindowState = WindowState.Normal;
                }
                return;
            }
            case "Нет":
            {
                flag = true;
                dbm.Restore();
                new SortFormSyncCommand(vm).Execute(null);
                await dbm.SaveChangesAsync();

                var lst = vm.Storage[vm.FormType];

                foreach (var key in lst)
                {
                    var item = (Form)key;
                    if (item.Id == 0)
                    {
                        vm.Storage[vm.Storage.FormNum_DB].Remove(item);
                    }
                }

                var lstNote = vm.Storage.Notes.ToList<Note>();
                foreach (var item in lstNote.Where(item => item.Id == 0))
                {
                    vm.Storage.Notes.Remove(item);
                }

                if (vm.FormType is not "1.0" and not "2.0")
                {
                    if (vm.FormType.Split('.')[0] == "1")
                    {
                        vm.Storage.OnPropertyChanged(nameof(vm.Storage.StartPeriod));
                        vm.Storage.OnPropertyChanged(nameof(vm.Storage.EndPeriod));
                        vm.Storage.OnPropertyChanged(nameof(vm.Storage.CorrectionNumber));
                    }
                    else if (vm.FormType.Split('.')[0] == "2")
                    {
                        vm.Storage.OnPropertyChanged(nameof(vm.Storage.Year));
                        vm.Storage.OnPropertyChanged(nameof(vm.Storage.CorrectionNumber));
                    }
                }
                else
                {
                    vm.Storage.OnPropertyChanged(nameof(vm.Storage.RegNoRep));
                    vm.Storage.OnPropertyChanged(nameof(vm.Storage.ShortJurLicoRep));
                    vm.Storage.OnPropertyChanged(nameof(vm.Storage.OkpoRep));
                }

                break;
            }
        }
        desktop.MainWindow.WindowState = WindowState.Normal;
        if (flag)
        {
            Close();
        }
        args.Cancel = true;
    }

    #endregion
}