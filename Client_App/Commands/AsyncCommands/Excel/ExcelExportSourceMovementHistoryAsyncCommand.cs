using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using Models.Collections;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.ViewModels;
using Client_App.Resources;

namespace Client_App.Commands.AsyncCommands.Excel;

//  Выгрузка в Excel истории движения источника
internal class ExcelExportSourceMovementHistoryAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        var cts = new CancellationTokenSource();
        ExportType = "История движения источника";
        ChangeOrCreateVM.PassportUniqParam(parameter, out _, out _, out _, out var pasNum, out var factoryNum);
        if (string.IsNullOrEmpty(pasNum) || string.IsNullOrEmpty(factoryNum) || pasNum is "-" && factoryNum is "-")
        {
            #region MessageFailedToLoadPassportUniqParam

                await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в Excel",
                        ContentMessage = "Не удалось выполнить выгрузку в Excel истории движения источника,"
                        + $"{Environment.NewLine}поскольку не заполнено одно из следующих полей:" +
                        $"{Environment.NewLine}- номер паспорта (сертификата);" +
                        $"{Environment.NewLine}- номер источника.",
                        MinHeight = 100,
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow);

                #endregion

            return;
        }

        var fileName = $"{ExportType}_{pasNum}_{factoryNum}_{BaseVM.Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;

        #region FillForm_1.1

        var worksheet = excelPackage.Workbook.Worksheets.Add("Операции по форме 1.1");

        #region ColumnHeaders

        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = "номер паспорта (сертификата)";
        worksheet.Cells[1, 13].Value = "тип";
        worksheet.Cells[1, 14].Value = "радионуклиды";
        worksheet.Cells[1, 15].Value = "номер";
        worksheet.Cells[1, 16].Value = "количество, шт";
        worksheet.Cells[1, 17].Value = "суммарная активность, Бк";
        worksheet.Cells[1, 18].Value = "код ОКПО изготовителя";
        worksheet.Cells[1, 19].Value = "дата выпуска";
        worksheet.Cells[1, 20].Value = "категория";
        worksheet.Cells[1, 21].Value = "НСС, мес";
        worksheet.Cells[1, 22].Value = "код формы собственности";
        worksheet.Cells[1, 23].Value = "код ОКПО правообладателя";
        worksheet.Cells[1, 24].Value = "вид";
        worksheet.Cells[1, 25].Value = "номер";
        worksheet.Cells[1, 26].Value = "дата";
        worksheet.Cells[1, 27].Value = "поставщика или получателя";
        worksheet.Cells[1, 28].Value = "перевозчика";
        worksheet.Cells[1, 29].Value = "наименование";
        worksheet.Cells[1, 30].Value = "тип";
        worksheet.Cells[1, 31].Value = "номер";

        #endregion

        var lastRow = 1;
        foreach (var key in MainWindowVM.LocalReports.Reports_Collection10)
        {
            var reps = (Reports)key;
            var form11 = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null);
            foreach (var rep in form11)
            {
                var repPas = rep.Rows11
                    .Where(x => StaticStringMethods.ComparePasParam(x.PassportNumber_DB, pasNum)
                                && StaticStringMethods.ComparePasParam(x.FactoryNumber_DB, factoryNum));
                foreach (var repForm in repPas)
                {
                    if (lastRow == 1)
                    {
                        #region BindingCells

                        worksheet.Cells[2, 1].Value = reps.Master.RegNoRep.Value;
                        worksheet.Cells[2, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                        worksheet.Cells[2, 3].Value = reps.Master.OkpoRep.Value;
                        worksheet.Cells[2, 4].Value = rep.FormNum_DB;
                        worksheet.Cells[2, 5].Value = rep.StartPeriod_DB;
                        worksheet.Cells[2, 6].Value = rep.EndPeriod_DB;
                        worksheet.Cells[2, 7].Value = rep.CorrectionNumber_DB;
                        worksheet.Cells[2, 8].Value = rep.Rows.Count;
                        worksheet.Cells[2, 9].Value = repForm.NumberInOrder_DB;
                        worksheet.Cells[2, 10].Value = repForm.OperationCode_DB;
                        worksheet.Cells[2, 11].Value = repForm.OperationDate_DB;
                        worksheet.Cells[2, 12].Value = repForm.PassportNumber_DB;
                        worksheet.Cells[2, 13].Value = repForm.Type_DB;
                        worksheet.Cells[2, 14].Value = repForm.Radionuclids_DB;
                        worksheet.Cells[2, 15].Value = repForm.FactoryNumber_DB;
                        worksheet.Cells[2, 16].Value = repForm.Quantity_DB;
                        worksheet.Cells[2, 17].Value = repForm.Activity_DB;
                        worksheet.Cells[2, 18].Value = repForm.CreatorOKPO_DB;
                        worksheet.Cells[2, 19].Value = repForm.CreationDate_DB;
                        worksheet.Cells[2, 20].Value = repForm.Category_DB;
                        worksheet.Cells[2, 21].Value = repForm.SignedServicePeriod_DB;
                        worksheet.Cells[2, 22].Value = repForm.PropertyCode_DB;
                        worksheet.Cells[2, 23].Value = repForm.Owner_DB;
                        worksheet.Cells[2, 24].Value = repForm.DocumentVid_DB;
                        worksheet.Cells[2, 25].Value = repForm.DocumentNumber_DB;
                        worksheet.Cells[2, 26].Value = repForm.DocumentDate_DB;
                        worksheet.Cells[2, 27].Value = repForm.ProviderOrRecieverOKPO_DB;
                        worksheet.Cells[2, 28].Value = repForm.TransporterOKPO_DB;
                        worksheet.Cells[2, 29].Value = repForm.PackName_DB;
                        worksheet.Cells[2, 30].Value = repForm.PackType_DB;
                        worksheet.Cells[2, 31].Value = repForm.PackNumber_DB;
                        
                        #endregion

                        lastRow++;
                        continue;
                    }
                    for (var currentRow = 2; currentRow <= lastRow + 1; currentRow++)
                    {
                        if (new CustomStringDateComparer(StringComparer.CurrentCulture).Compare(
                                repForm.OperationDate_DB, (string)worksheet.Cells[currentRow, 11].Value) < 0)
                        {
                            worksheet.InsertRow(currentRow, 1);

                            #region BindingCells

                            worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                            worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                            worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                            worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                            worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                            worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                            worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                            worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                            worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                            worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                            worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                            worksheet.Cells[currentRow, 12].Value = repForm.PassportNumber_DB;
                            worksheet.Cells[currentRow, 13].Value = repForm.Type_DB;
                            worksheet.Cells[currentRow, 14].Value = repForm.Radionuclids_DB;
                            worksheet.Cells[currentRow, 15].Value = repForm.FactoryNumber_DB;
                            worksheet.Cells[currentRow, 16].Value = repForm.Quantity_DB;
                            worksheet.Cells[currentRow, 17].Value = repForm.Activity_DB;
                            worksheet.Cells[currentRow, 18].Value = repForm.CreatorOKPO_DB;
                            worksheet.Cells[currentRow, 19].Value = repForm.CreationDate_DB;
                            worksheet.Cells[currentRow, 20].Value = repForm.Category_DB;
                            worksheet.Cells[currentRow, 21].Value = repForm.SignedServicePeriod_DB;
                            worksheet.Cells[currentRow, 22].Value = repForm.PropertyCode_DB;
                            worksheet.Cells[currentRow, 23].Value = repForm.Owner_DB;
                            worksheet.Cells[currentRow, 24].Value = repForm.DocumentVid_DB;
                            worksheet.Cells[currentRow, 25].Value = repForm.DocumentNumber_DB;
                            worksheet.Cells[currentRow, 26].Value = repForm.DocumentDate_DB;
                            worksheet.Cells[currentRow, 27].Value = repForm.ProviderOrRecieverOKPO_DB;
                            worksheet.Cells[currentRow, 28].Value = repForm.TransporterOKPO_DB;
                            worksheet.Cells[currentRow, 29].Value = repForm.PackName_DB;
                            worksheet.Cells[currentRow, 30].Value = repForm.PackType_DB;
                            worksheet.Cells[currentRow, 31].Value = repForm.PackNumber_DB;

                            #endregion

                            lastRow++;
                            break;
                        }
                    }
                }
            }
        }

        if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
        {
            worksheet.Cells.AutoFitColumns();
        }

        var headersCellsString = "A1:A" + worksheet.Dimension.End.Column;
        worksheet.Cells[headersCellsString].Style.WrapText = true;
        worksheet.Cells[headersCellsString].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        worksheet.Cells[headersCellsString].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        #endregion

        #region FillForm_1.5

        worksheet = excelPackage.Workbook.Worksheets.Add("Операции по форме 1.5");

        #region ColumnHeaders
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = $"номер паспорта (сертификата) Эри,{Environment.NewLine}акта определения характеристик ОЗИИ";
        worksheet.Cells[1, 13].Value = "тип";
        worksheet.Cells[1, 14].Value = "радионуклиды";
        worksheet.Cells[1, 15].Value = "номер";
        worksheet.Cells[1, 16].Value = "количество, шт";
        worksheet.Cells[1, 17].Value = "суммарная активность, Бк";
        worksheet.Cells[1, 18].Value = "дата выпуска";
        worksheet.Cells[1, 19].Value = "статус РАО";
        worksheet.Cells[1, 20].Value = "вид";
        worksheet.Cells[1, 21].Value = "номер";
        worksheet.Cells[1, 22].Value = "дата";
        worksheet.Cells[1, 23].Value = "поставщика или получателя";
        worksheet.Cells[1, 24].Value = "перевозчика";
        worksheet.Cells[1, 25].Value = "наименование";
        worksheet.Cells[1, 26].Value = "тип";
        worksheet.Cells[1, 27].Value = "заводской номер";
        worksheet.Cells[1, 28].Value = "наименование";
        worksheet.Cells[1, 29].Value = "код";
        worksheet.Cells[1, 30].Value = "Код переработки / сортировки РАО";
        worksheet.Cells[1, 31].Value = "Субсидия, %";
        worksheet.Cells[1, 32].Value = "Номер мероприятия ФЦП";
        #endregion

        lastRow = 1;
        foreach (var key in MainWindowVM.LocalReports.Reports_Collection10)
        {
            var reps = (Reports)key;
            var form15 = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.5") && x.Rows15 != null);
            foreach (var rep in form15)
            {
                var repPas = rep.Rows15
                    .Where(x => StaticStringMethods.ComparePasParam(x.PassportNumber_DB, pasNum)
                                && StaticStringMethods.ComparePasParam(x.FactoryNumber_DB, factoryNum));
                foreach (var repForm in repPas)
                {
                    if (lastRow == 1)
                    {
                        #region BindingCells
                        worksheet.Cells[2, 1].Value = reps.Master.RegNoRep.Value;
                        worksheet.Cells[2, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                        worksheet.Cells[2, 3].Value = reps.Master.OkpoRep.Value;
                        worksheet.Cells[2, 4].Value = rep.FormNum_DB;
                        worksheet.Cells[2, 5].Value = rep.StartPeriod_DB;
                        worksheet.Cells[2, 6].Value = rep.EndPeriod_DB;
                        worksheet.Cells[2, 7].Value = rep.CorrectionNumber_DB;
                        worksheet.Cells[2, 8].Value = rep.Rows.Count;
                        worksheet.Cells[2, 9].Value = repForm.NumberInOrder_DB;
                        worksheet.Cells[2, 10].Value = repForm.OperationCode_DB;
                        worksheet.Cells[2, 11].Value = repForm.OperationDate_DB;
                        worksheet.Cells[2, 12].Value = repForm.PassportNumber_DB;
                        worksheet.Cells[2, 13].Value = repForm.Type_DB;
                        worksheet.Cells[2, 14].Value = repForm.Radionuclids_DB;
                        worksheet.Cells[2, 15].Value = repForm.FactoryNumber_DB;
                        worksheet.Cells[2, 16].Value = repForm.Quantity_DB;
                        worksheet.Cells[2, 17].Value = repForm.Activity_DB;
                        worksheet.Cells[2, 18].Value = repForm.CreationDate_DB;
                        worksheet.Cells[2, 19].Value = repForm.StatusRAO_DB;
                        worksheet.Cells[2, 20].Value = repForm.DocumentVid_DB;
                        worksheet.Cells[2, 21].Value = repForm.DocumentNumber_DB;
                        worksheet.Cells[2, 22].Value = repForm.DocumentDate_DB;
                        worksheet.Cells[2, 23].Value = repForm.ProviderOrRecieverOKPO_DB;
                        worksheet.Cells[2, 24].Value = repForm.TransporterOKPO_DB;
                        worksheet.Cells[2, 25].Value = repForm.PackName_DB;
                        worksheet.Cells[2, 26].Value = repForm.PackType_DB;
                        worksheet.Cells[2, 27].Value = repForm.PackNumber_DB;
                        worksheet.Cells[2, 28].Value = repForm.StoragePlaceName_DB;
                        worksheet.Cells[2, 29].Value = repForm.StoragePlaceCode_DB;
                        worksheet.Cells[2, 30].Value = repForm.RefineOrSortRAOCode_DB;
                        worksheet.Cells[2, 31].Value = repForm.Subsidy_DB;
                        worksheet.Cells[2, 32].Value = repForm.FcpNumber_DB;
                        #endregion

                        lastRow++;
                        continue;
                    }
                    for (var currentRow = 2; currentRow <= lastRow + 1; currentRow++)
                    {
                        if (new CustomStringDateComparer(StringComparer.CurrentCulture).Compare(
                                repForm.OperationDate_DB, (string)worksheet.Cells[currentRow, 11].Value) < 0)
                        {
                            worksheet.InsertRow(currentRow, 1);

                            #region BindingCells
                            worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                            worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                            worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                            worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                            worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                            worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                            worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                            worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                            worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                            worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                            worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                            worksheet.Cells[currentRow, 12].Value = repForm.PassportNumber_DB;
                            worksheet.Cells[currentRow, 13].Value = repForm.Type_DB;
                            worksheet.Cells[currentRow, 14].Value = repForm.Radionuclids_DB;
                            worksheet.Cells[currentRow, 15].Value = repForm.FactoryNumber_DB;
                            worksheet.Cells[currentRow, 16].Value = repForm.Quantity_DB;
                            worksheet.Cells[currentRow, 17].Value = repForm.Activity_DB;
                            worksheet.Cells[currentRow, 18].Value = repForm.CreationDate_DB;
                            worksheet.Cells[currentRow, 19].Value = repForm.StatusRAO_DB;
                            worksheet.Cells[currentRow, 20].Value = repForm.DocumentVid_DB;
                            worksheet.Cells[currentRow, 21].Value = repForm.DocumentNumber_DB;
                            worksheet.Cells[currentRow, 22].Value = repForm.DocumentDate_DB;
                            worksheet.Cells[currentRow, 23].Value = repForm.ProviderOrRecieverOKPO_DB;
                            worksheet.Cells[currentRow, 24].Value = repForm.TransporterOKPO_DB;
                            worksheet.Cells[currentRow, 25].Value = repForm.PackName_DB;
                            worksheet.Cells[currentRow, 26].Value = repForm.PackType_DB;
                            worksheet.Cells[currentRow, 27].Value = repForm.PackNumber_DB;
                            worksheet.Cells[currentRow, 28].Value = repForm.StoragePlaceName_DB;
                            worksheet.Cells[currentRow, 29].Value = repForm.StoragePlaceCode_DB;
                            worksheet.Cells[currentRow, 30].Value = repForm.RefineOrSortRAOCode_DB;
                            worksheet.Cells[currentRow, 31].Value = repForm.Subsidy_DB;
                            worksheet.Cells[currentRow, 32].Value = repForm.FcpNumber_DB;
                            lastRow++;
                            #endregion

                            break;
                        }
                    }
                }
            }
        }

        if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
        {
            worksheet.Cells.AutoFitColumns();
        }
        headersCellsString = "A1:A" + worksheet.Dimension.End.Column;
        worksheet.Cells[headersCellsString].Style.WrapText = true;
        worksheet.Cells[headersCellsString].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        worksheet.Cells[headersCellsString].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        
        #endregion

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}