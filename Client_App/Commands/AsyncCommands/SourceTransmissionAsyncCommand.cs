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

// Перевод источника из РВ в РАО (из формы 1.1 в 1.5)
public class SourceTransmissionAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Reports SelectedReports => changeOrCreateViewModel.Storages;
    private Report SelectedReport => changeOrCreateViewModel.Storage;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        var form11 = ((IKeyCollection)parameter).Get<Form11>(0);
        if (!string.Equals(form11.OperationCode_DB.Trim(), "41", StringComparison.Ordinal))
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
        if (!DateOnly.TryParse(form11.OperationDate_DB, out var opDate))
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
            .Where(rep => string.Equals(rep.FormNum_DB, "1.5", StringComparison.Ordinal) 
                                   && DateOnly.TryParse(rep.StartPeriod_DB, out var repStartDateTime)
                                   && DateOnly.TryParse(rep.EndPeriod_DB, out var repEndDateTime)
                                   && opDate >= repStartDateTime && opDate <= repEndDateTime)
            .ToArray();

        await using var db = new DBModel(StaticConfiguration.DBPath);
        switch (repInRange.Length)
        {
            case > 1:   // У организации по ошибке есть несколько отчётов 1.5 с нужным периодом
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
            case 1:   // Если есть подходящий отчет 1.5, то добавляем форму в него
            {
                var rep = repInRange.First();
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
                var rep15 = await ReportsStorage.GetReportAsync(rep.Id);
                await db.SaveChangesAsync();
                var window11 = Desktop.Windows.First(x => x.Name == "1.1");
                await Dispatcher.UIThread.InvokeAsync(() => window11.Close()).ConfigureAwait(false);
                //window11?.Close();
                new ChangeFormAsyncCommand().Execute(
                    new ObservableCollectionWithItemPropertyChanged<IKey>(new List<Report> { rep15 }));
                break;
            }
            default:    // Если отчета с подходящим периодом нет, создаём новый отчёт 1.5 и добавляем в него форму 
            {
                var newRep15 = new Report
                {
                    FormNum_DB = "1.5",
                    StartPeriod_DB = SelectedReport.EndPeriod_DB,
                    CorrectionNumber_DB = 0
                };
                var a = db.ReportCollectionDbSet.Add(newRep15);
                await db.SaveChangesAsync();
                var repId = a.Entity.Id;    //id обновляется после сохранения БД.

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
                await db.SaveChangesAsync();
                var rep15 = await ReportsStorage.Api.GetAsync(repId);
                SelectedReports.Report_Collection.Add(rep15);
                var window11 = Desktop.Windows.First(x => x.Name == "1.1");
                var windowParam = new Form11Parameter()
                {
                    parameter = new ObservableCollectionWithItemPropertyChanged<IKey>(new List<Report> { rep15 }),
                    window11 = window11
                };
                await new ChangeFormAsyncCommand(windowParam).AsyncExecute(null);
                break;
            }
        }
    }
}