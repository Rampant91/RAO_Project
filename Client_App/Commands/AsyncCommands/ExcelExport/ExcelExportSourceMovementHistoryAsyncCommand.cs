using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.DTO;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Выгрузка в Excel истории движения источника
internal class ExcelExportSourceMovementHistoryAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        var cts = new CancellationTokenSource();
        ExportType = "История движения источника";
        StaticMethods.PassportUniqParam(parameter, out _, out _, out _, out var pasNum, out var factoryNum);
        if (string.IsNullOrEmpty(pasNum) || string.IsNullOrEmpty(factoryNum) || pasNum is "-" && factoryNum is "-")
        {
            #region MessageFailedToLoadPassportUniqParam

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
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
                    .ShowDialog(Desktop.MainWindow));

                #endregion

            return;
        }

        var fileName = $"{ExportType}_{RemoveForbiddenChars(pasNum)}_{RemoveForbiddenChars(factoryNum)}_{BaseVM.Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            cts.Dispose();
            return;
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        var dbReadOnlyPath = Path.Combine(BaseVM.TmpDirectory, BaseVM.DbFileName + ".RAODB");
        try
        {
            if (!StaticConfiguration.IsFileLocked(dbReadOnlyPath))
            {
                File.Delete(dbReadOnlyPath);
                File.Copy(Path.Combine(BaseVM.RaoDirectory, BaseVM.DbFileName + ".RAODB"), dbReadOnlyPath);
            }
        }
        catch
        {
            cts.Dispose();
            return;
        }

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

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);
        var dto11List = dbReadOnly.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
            .ToArray()
            .SelectMany(reps => reps.Report_Collection
                    .Where(rep => rep.FormNum_DB == "1.1")
                    .SelectMany(rep => rep.Rows11
                        .Where(form11 => ComparePasParam(form11.PassportNumber_DB, pasNum)
                                         && ComparePasParam(form11.FactoryNumber_DB, factoryNum))
                        .Select(form11 => new Form11DTO
                        {
                            RegNoRep = reps.Master.RegNoRep.Value,
                            ShortJurLico = reps.Master.ShortJurLicoRep.Value,
                            OkpoRep = reps.Master.OkpoRep.Value,
                            FormNum = rep.FormNum_DB,
                            StartPeriod = rep.StartPeriod_DB,
                            EndPeriod = rep.EndPeriod_DB,
                            CorrectionNumber = rep.CorrectionNumber_DB,
                            RowCount = rep.Rows11.Count,
                            NumberInOrder = form11.NumberInOrder_DB,
                            OperationCode = form11.OperationCode_DB,
                            OperationDate = form11.OperationDate_DB,
                            PassportNumber = form11.PassportNumber_DB,
                            Type = form11.Type_DB,
                            Radionuclids = form11.Radionuclids_DB,
                            FactoryNumber = form11.FactoryNumber_DB,
                            Activity = form11.Activity_DB,
                            Quantity = form11.Quantity_DB,
                            CreatorOKPO = form11.CreatorOKPO_DB,
                            CreationDate = form11.CreationDate_DB,
                            Category = form11.Category_DB,
                            SignedServicePeriod = form11.SignedServicePeriod_DB,
                            PropertyCode = form11.PropertyCode_DB,
                            Owner = form11.Owner_DB,
                            DocumentVid = form11.DocumentVid_DB,
                            DocumentNumber = form11.DocumentNumber_DB,
                            DocumentDate = form11.DocumentDate_DB,
                            ProviderOrRecieverOKPO = form11.ProviderOrRecieverOKPO_DB,
                            TransporterOKPO = form11.TransporterOKPO_DB,
                            PackName = form11.PackName_DB,
                            PackType = form11.PackType_DB,
                            PackNumber = form11.PackNumber_DB
                        })))
                .ToList();

        var lastRow = 1;
        foreach (var dto in dto11List)
        {
            if (lastRow == 1)
            {
                #region BindingCells

                Worksheet.Cells[2, 1].Value = dto.RegNoRep;
                Worksheet.Cells[2, 2].Value = dto.ShortJurLico;
                Worksheet.Cells[2, 3].Value = dto.OkpoRep;
                Worksheet.Cells[2, 4].Value = dto.FormNum;
                Worksheet.Cells[2, 5].Value = dto.StartPeriod;
                Worksheet.Cells[2, 6].Value = dto.EndPeriod;
                Worksheet.Cells[2, 7].Value = dto.CorrectionNumber;
                Worksheet.Cells[2, 8].Value = dto.RowCount;
                Worksheet.Cells[2, 9].Value = dto.NumberInOrder;
                Worksheet.Cells[2, 10].Value = dto.OperationCode;
                Worksheet.Cells[2, 11].Value = dto.OperationDate;
                Worksheet.Cells[2, 12].Value = dto.PassportNumber;
                Worksheet.Cells[2, 13].Value = dto.Type;
                Worksheet.Cells[2, 14].Value = dto.Radionuclids;
                Worksheet.Cells[2, 15].Value = dto.FactoryNumber;
                Worksheet.Cells[2, 16].Value = dto.Quantity;
                Worksheet.Cells[2, 17].Value = dto.Activity;
                Worksheet.Cells[2, 18].Value = dto.CreatorOKPO;
                Worksheet.Cells[2, 19].Value = dto.CreationDate;
                Worksheet.Cells[2, 20].Value = dto.Category;
                Worksheet.Cells[2, 21].Value = dto.SignedServicePeriod;
                Worksheet.Cells[2, 22].Value = dto.PropertyCode;
                Worksheet.Cells[2, 23].Value = dto.Owner;
                Worksheet.Cells[2, 24].Value = dto.DocumentVid;
                Worksheet.Cells[2, 25].Value = dto.DocumentNumber;
                Worksheet.Cells[2, 26].Value = dto.DocumentDate;
                Worksheet.Cells[2, 27].Value = dto.ProviderOrRecieverOKPO;
                Worksheet.Cells[2, 28].Value = dto.TransporterOKPO;
                Worksheet.Cells[2, 29].Value = dto.PackName;
                Worksheet.Cells[2, 30].Value = dto.PackType;
                Worksheet.Cells[2, 31].Value = dto.PackNumber;

                #endregion

                lastRow++;
                continue;
            }
            for (var currentRow = 2; currentRow <= lastRow + 1; currentRow++)
            {
                if (new CustomStringDateComparer(StringComparer.CurrentCulture)
                        .Compare(dto.OperationDate, (string)worksheet.Cells[currentRow, 11].Value) >= 0) continue;

                worksheet.InsertRow(currentRow, 1);

                #region BindingCells

                worksheet.Cells[currentRow, 1].Value = dto.RegNoRep;
                worksheet.Cells[currentRow, 2].Value = dto.ShortJurLico;
                worksheet.Cells[currentRow, 3].Value = dto.OkpoRep;
                worksheet.Cells[currentRow, 4].Value = dto.FormNum;
                worksheet.Cells[currentRow, 5].Value = dto.StartPeriod;
                worksheet.Cells[currentRow, 6].Value = dto.EndPeriod;
                worksheet.Cells[currentRow, 7].Value = dto.CorrectionNumber;
                worksheet.Cells[currentRow, 8].Value = dto.RowCount;
                worksheet.Cells[currentRow, 9].Value = dto.NumberInOrder;
                worksheet.Cells[currentRow, 10].Value = dto.OperationCode;
                worksheet.Cells[currentRow, 11].Value = dto.OperationDate;
                worksheet.Cells[currentRow, 12].Value = dto.PassportNumber;
                worksheet.Cells[currentRow, 13].Value = dto.Type;
                worksheet.Cells[currentRow, 14].Value = dto.Radionuclids;
                worksheet.Cells[currentRow, 15].Value = dto.FactoryNumber;
                worksheet.Cells[currentRow, 16].Value = dto.Quantity;
                worksheet.Cells[currentRow, 17].Value = dto.Activity;
                worksheet.Cells[currentRow, 18].Value = dto.CreatorOKPO;
                worksheet.Cells[currentRow, 19].Value = dto.CreationDate;
                worksheet.Cells[currentRow, 20].Value = dto.Category;
                worksheet.Cells[currentRow, 21].Value = dto.SignedServicePeriod;
                worksheet.Cells[currentRow, 22].Value = dto.PropertyCode;
                worksheet.Cells[currentRow, 23].Value = dto.Owner;
                worksheet.Cells[currentRow, 24].Value = dto.DocumentVid;
                worksheet.Cells[currentRow, 25].Value = dto.DocumentNumber;
                worksheet.Cells[currentRow, 26].Value = dto.DocumentDate;
                worksheet.Cells[currentRow, 27].Value = dto.ProviderOrRecieverOKPO;
                worksheet.Cells[currentRow, 28].Value = dto.TransporterOKPO;
                worksheet.Cells[currentRow, 29].Value = dto.PackName;
                worksheet.Cells[currentRow, 30].Value = dto.PackType;
                worksheet.Cells[currentRow, 31].Value = dto.PackNumber;

                #endregion

                lastRow++;
                break;
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

        var dto15List = dbReadOnly.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows15)
            .ToArray()
            .SelectMany(reps => reps.Report_Collection
                    .Where(rep => rep.FormNum_DB == "1.5")
                    .SelectMany(rep => rep.Rows15
                        .Where(form15 => ComparePasParam(form15.PassportNumber_DB, pasNum)
                                         && ComparePasParam(form15.FactoryNumber_DB, factoryNum))
                        .Select(form15 => new Form15DTO
                        {
                            RegNoRep = reps.Master.RegNoRep.Value,
                            ShortJurLico = reps.Master.ShortJurLicoRep.Value,
                            OkpoRep = reps.Master.OkpoRep.Value,
                            FormNum = rep.FormNum_DB,
                            StartPeriod = rep.StartPeriod_DB,
                            EndPeriod = rep.EndPeriod_DB,
                            CorrectionNumber = rep.CorrectionNumber_DB,
                            RowCount = rep.Rows11.Count,
                            NumberInOrder = form15.NumberInOrder_DB,
                            OperationCode = form15.OperationCode_DB,
                            OperationDate = form15.OperationDate_DB,
                            PassportNumber = form15.PassportNumber_DB,
                            Type = form15.Type_DB,
                            Radionuclids = form15.Radionuclids_DB,
                            FactoryNumber = form15.FactoryNumber_DB,
                            Activity = form15.Activity_DB,
                            Quantity = form15.Quantity_DB,
                            CreationDate = form15.CreationDate_DB,
                            StatusRAO = form15.StatusRAO_DB,
                            DocumentVid = form15.DocumentVid_DB,
                            DocumentNumber = form15.DocumentNumber_DB,
                            DocumentDate = form15.DocumentDate_DB,
                            ProviderOrRecieverOKPO = form15.ProviderOrRecieverOKPO_DB,
                            TransporterOKPO = form15.TransporterOKPO_DB,
                            PackName = form15.PackName_DB,
                            PackType = form15.PackType_DB,
                            PackNumber = form15.PackNumber_DB,
                            StoragePlaceName = form15.StoragePlaceName_DB,
                            StoragePlaceCode = form15.StoragePlaceCode_DB,
                            RefineOrSortRAOCode = form15.RefineOrSortRAOCode_DB,
                            Subsidy = form15.Subsidy_DB,
                            FcpNumber = form15.FcpNumber_DB
                        })))
                .ToList();

        lastRow = 1;
        foreach (var dto in dto15List)
        {
            if (lastRow == 1)
            {
                #region BindingCells

                Worksheet.Cells[2, 1].Value = dto.RegNoRep;
                Worksheet.Cells[2, 2].Value = dto.ShortJurLico;
                Worksheet.Cells[2, 3].Value = dto.OkpoRep;
                Worksheet.Cells[2, 4].Value = dto.FormNum;
                Worksheet.Cells[2, 5].Value = dto.StartPeriod;
                Worksheet.Cells[2, 6].Value = dto.EndPeriod;
                Worksheet.Cells[2, 7].Value = dto.CorrectionNumber;
                Worksheet.Cells[2, 8].Value = dto.RowCount;
                Worksheet.Cells[2, 9].Value = dto.NumberInOrder;
                Worksheet.Cells[2, 10].Value = dto.OperationCode;
                Worksheet.Cells[2, 11].Value = dto.OperationDate;
                Worksheet.Cells[2, 12].Value = dto.PassportNumber;
                Worksheet.Cells[2, 13].Value = dto.Type;
                Worksheet.Cells[2, 14].Value = dto.Radionuclids;
                Worksheet.Cells[2, 15].Value = dto.FactoryNumber;
                Worksheet.Cells[2, 16].Value = dto.Quantity;
                Worksheet.Cells[2, 17].Value = dto.Activity;
                Worksheet.Cells[2, 18].Value = dto.CreationDate;
                Worksheet.Cells[2, 19].Value = dto.StatusRAO;
                Worksheet.Cells[2, 20].Value = dto.DocumentVid;
                Worksheet.Cells[2, 21].Value = dto.DocumentNumber;
                Worksheet.Cells[2, 22].Value = dto.DocumentDate;
                Worksheet.Cells[2, 23].Value = dto.ProviderOrRecieverOKPO;
                Worksheet.Cells[2, 24].Value = dto.TransporterOKPO;
                Worksheet.Cells[2, 25].Value = dto.PackName;
                Worksheet.Cells[2, 26].Value = dto.PackType;
                Worksheet.Cells[2, 27].Value = dto.PackNumber;
                Worksheet.Cells[2, 28].Value = dto.StoragePlaceName;
                Worksheet.Cells[2, 29].Value = dto.StoragePlaceCode;
                Worksheet.Cells[2, 30].Value = dto.RefineOrSortRAOCode;
                Worksheet.Cells[2, 31].Value = dto.Subsidy;
                Worksheet.Cells[2, 32].Value = dto.FcpNumber;

                #endregion

                lastRow++;
                continue;
            }
            for (var currentRow = 2; currentRow <= lastRow + 1; currentRow++)
            {
                if (new CustomStringDateComparer(StringComparer.CurrentCulture)
                        .Compare(dto.OperationDate, (string)worksheet.Cells[currentRow, 11].Value) >= 0) continue;

                worksheet.InsertRow(currentRow, 1);

                #region BindingCells

                worksheet.Cells[currentRow, 1].Value = dto.RegNoRep;
                worksheet.Cells[currentRow, 2].Value = dto.ShortJurLico;
                worksheet.Cells[currentRow, 3].Value = dto.OkpoRep;
                worksheet.Cells[currentRow, 4].Value = dto.FormNum;
                worksheet.Cells[currentRow, 5].Value = dto.StartPeriod;
                worksheet.Cells[currentRow, 6].Value = dto.EndPeriod;
                worksheet.Cells[currentRow, 7].Value = dto.CorrectionNumber;
                worksheet.Cells[currentRow, 8].Value = dto.RowCount;
                worksheet.Cells[currentRow, 9].Value = dto.NumberInOrder;
                worksheet.Cells[currentRow, 10].Value = dto.OperationCode;
                worksheet.Cells[currentRow, 11].Value = dto.OperationDate;
                worksheet.Cells[currentRow, 12].Value = dto.PassportNumber;
                worksheet.Cells[currentRow, 13].Value = dto.Type;
                worksheet.Cells[currentRow, 14].Value = dto.Radionuclids;
                worksheet.Cells[currentRow, 15].Value = dto.FactoryNumber;
                worksheet.Cells[currentRow, 16].Value = dto.Quantity;
                worksheet.Cells[currentRow, 17].Value = dto.Activity;
                worksheet.Cells[currentRow, 18].Value = dto.CreationDate;
                worksheet.Cells[currentRow, 19].Value = dto.StatusRAO;
                worksheet.Cells[currentRow, 20].Value = dto.DocumentVid;
                worksheet.Cells[currentRow, 21].Value = dto.DocumentNumber;
                worksheet.Cells[currentRow, 22].Value = dto.DocumentDate;
                worksheet.Cells[currentRow, 23].Value = dto.ProviderOrRecieverOKPO;
                worksheet.Cells[currentRow, 24].Value = dto.TransporterOKPO;
                worksheet.Cells[currentRow, 25].Value = dto.PackName;
                worksheet.Cells[currentRow, 26].Value = dto.PackType;
                worksheet.Cells[currentRow, 27].Value = dto.PackNumber;
                worksheet.Cells[currentRow, 28].Value = dto.StoragePlaceName;
                worksheet.Cells[currentRow, 29].Value = dto.StoragePlaceCode;
                worksheet.Cells[currentRow, 30].Value = dto.RefineOrSortRAOCode;
                worksheet.Cells[currentRow, 31].Value = dto.Subsidy;
                worksheet.Cells[currentRow, 32].Value = dto.FcpNumber;

                #endregion

                lastRow++;
                break;
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