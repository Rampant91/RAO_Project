using Client_App.ViewModels;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using MessageBox.Avalonia.DTO;
using Models.Classes;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands;

// Перевод источника из РВ в РАО
public class SourceTransmissionAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Reports SelectedReports => changeOrCreateViewModel.Storages;
    private Report SelectedReport => changeOrCreateViewModel.Storage;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        var form = ((IKeyCollection)parameter).Get<Form1>(0);
        if (!string.Equals(form.OperationCode_DB.Trim(), "41", StringComparison.Ordinal))
        {
            #region MessageSourceTransmissionFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Передача источника",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Перевод источника в РАО осуществляется кодом операции 41",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }
        if (!DateOnly.TryParse(form.OperationDate_DB, out var opDate))
        {
            #region MessageSourceTransmissionFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Передача источника",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Некорректно введена дата операции",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }
        var repInRange = SelectedReports.Report_Collection
            .Where(rep => (form.FormNum_DB == "1.1" && rep.FormNum_DB == "1.5" 
                           || form.FormNum_DB == "1.2" && rep.FormNum_DB == "1.6")
                          && (DateOnly.TryParse(rep.StartPeriod_DB, out var repStartDate) 
                              && DateOnly.TryParse(rep.EndPeriod_DB, out var repEndDate) 
                              && opDate >= repStartDate && opDate <= repEndDate 
                              || (DateOnly.TryParse(rep.StartPeriod_DB, out repStartDate) 
                                  && !DateOnly.TryParse(rep.EndPeriod_DB, out _)
                                  && DateOnly.TryParse(SelectedReport.EndPeriod_DB, out var formEndDate)
                                  && repStartDate == formEndDate)))
            .ToArray();

        await using var db = new DBModel(StaticConfiguration.DBPath);
        switch (repInRange.Length)
        {
            case > 1:   // У организации по ошибке есть несколько отчётов с нужным периодом
            {
                #region MessageSourceTransmissionFailed

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Перевод источника в РАО",
                        ContentHeader = "Ошибка",
                        ContentMessage = "У выбранной организации присутствуют отчёты по форме 1.5 с пересекающимися периодами. " +
                                         $"{Environment.NewLine}Устраните данное несоответствие перед операцией перевода источника в РАО.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                return;
            }
            case 1:   // Если есть подходящий отчет, то добавляем форму в него
            {
                var rep = repInRange.First();
                switch (form.FormNum_DB)
                {
                    case "1.1":
                    {
                        var form11 = (Form11)form;
                        var newForm15 = new Form15
                        {
                            #region BindingData
                
                            ReportId = rep.Id,
                            NumberInOrder_DB = rep.Rows15.Count + 1,
                            OperationCode_DB = form11.OperationCode_DB,
                            OperationDate_DB = form11.OperationDate_DB,
                            PassportNumber_DB = form11.PassportNumber_DB,
                            Type_DB = form11.Type_DB,
                            Radionuclids_DB = form11.Radionuclids_DB,
                            FactoryNumber_DB = form11.FactoryNumber_DB,
                            Activity_DB = form11.Activity_DB,
                            Quantity_DB = form11.Quantity_DB,
                            CreationDate_DB = form11.CreationDate_DB,
                            StatusRAO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                            DocumentVid_DB = form11.DocumentVid_DB,
                            DocumentNumber_DB = form11.DocumentNumber_DB,
                            DocumentDate_DB = form11.DocumentDate_DB,
                            ProviderOrRecieverOKPO_DB = form11.ProviderOrRecieverOKPO_DB,
                            TransporterOKPO_DB = form11.TransporterOKPO_DB,
                            PackName_DB = form11.PackName_DB,
                            PackType_DB = form11.PackType_DB,
                            PackNumber_DB = form11.PackNumber_DB,
                            RefineOrSortRAOCode_DB = "-",
                            Subsidy_DB = "-",
                            FcpNumber_DB = "-"

                            #endregion
                        };
                        db.form_15.Add(newForm15);
                        break;
                    }
                    case "1.2":
                    {
                        var form12 = (Form12)form;
                        var newForm16 = new Form16
                        {
                            #region BindingData

                            ReportId = rep.Id,
                            NumberInOrder_DB = rep.Rows16.Count + 1,
                            OperationCode_DB = form12.OperationCode_DB,
                            OperationDate_DB = form12.OperationDate_DB,

                            #endregion
                        };
                        db.form_16.Add(newForm16);
                        break;
                    }
                }
                var report = await ReportsStorage.GetReportAsync(rep.Id);
                await db.SaveChangesAsync();
                await CloseWindowAndOpenNew(report);
                break;
            }
            default:    // Если отчета с подходящим периодом нет, создаём новый отчёт и добавляем в него форму 
            {
                var repId = 0;
                switch (form.FormNum_DB)
                {
                    case "1.1":
                    {
                        var form11 = (Form11)form;
                        var newRep15 = new Report
                        {
                            FormNum_DB = "1.5",
                            StartPeriod_DB = SelectedReport.EndPeriod_DB,
                            CorrectionNumber_DB = 0
                        };
                        var a = db.ReportCollectionDbSet.Add(newRep15);
                        await db.SaveChangesAsync();
                        repId = a.Entity.Id;    //id обновляется после сохранения БД.
                        var newForm15 = new Form15
                        {
                            #region BindingData
                
                            ReportId = repId,
                            NumberInOrder_DB = 1,
                            OperationCode_DB = form11.OperationCode_DB,
                            OperationDate_DB = form11.OperationDate_DB,
                            PassportNumber_DB = form11.PassportNumber_DB,
                            Type_DB = form11.Type_DB,
                            Radionuclids_DB = form11.Radionuclids_DB,
                            FactoryNumber_DB = form11.FactoryNumber_DB,
                            Activity_DB = form11.Activity_DB,
                            Quantity_DB = form11.Quantity_DB,
                            CreationDate_DB = form11.CreationDate_DB,
                            StatusRAO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                            DocumentVid_DB = form11.DocumentVid_DB,
                            DocumentNumber_DB = form11.DocumentNumber_DB,
                            DocumentDate_DB = form11.DocumentDate_DB,
                            ProviderOrRecieverOKPO_DB = form11.ProviderOrRecieverOKPO_DB,
                            TransporterOKPO_DB = form11.TransporterOKPO_DB,
                            PackName_DB = form11.PackName_DB,
                            PackType_DB = form11.PackType_DB,
                            PackNumber_DB = form11.PackNumber_DB,
                            RefineOrSortRAOCode_DB = "-",
                            Subsidy_DB = "-",
                            FcpNumber_DB = "-"

                            #endregion
                        };
                        db.form_15.Add(newForm15);
                        break;
                    }
                    case "1.2":
                    {
                        var form12 = (Form12)form;
                        var newRep16 = new Report
                        {
                            FormNum_DB = "1.6",
                            StartPeriod_DB = SelectedReport.EndPeriod_DB,
                            CorrectionNumber_DB = 0
                        };
                        var a = db.ReportCollectionDbSet.Add(newRep16);
                        await db.SaveChangesAsync();
                        repId = a.Entity.Id;    //id обновляется после сохранения БД.
                        var newForm16 = new Form16
                        {
                            #region BindingData

                            ReportId = repId,
                            NumberInOrder_DB = 1

                            #endregion
                        };
                        db.form_16.Add(newForm16);
                        break;
                    }
                }
                await db.SaveChangesAsync();
                var report = await ReportsStorage.Api.GetAsync(repId);
                SelectedReports.Report_Collection.Add(report);
                await CloseWindowAndOpenNew(report);
                break;
            }
        }
    }

    private static async Task CloseWindowAndOpenNew(Report rep)
    {
        var window = Desktop.Windows.First(x => x.Name is "1.1" or "1.2");
        var windowParam = new FormParameter()
        {
            Parameter = new ObservableCollectionWithItemPropertyChanged<IKey>(new List<Report> { rep }),
            Window = window
        };
        await new ChangeFormAsyncCommand(windowParam).AsyncExecute(null).ConfigureAwait(false);
    }
}