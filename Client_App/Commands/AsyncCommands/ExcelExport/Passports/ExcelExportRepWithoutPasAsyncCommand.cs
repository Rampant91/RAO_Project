using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.DTO;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Passports;

/// <summary>
/// Excel -> Паспорта -> Отчеты без паспортов
/// </summary>
public class ExcelExportRepWithoutPasAsyncCommand : ExcelBaseAsyncCommand
{
    private AnyTaskProgressBar progressBar;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Отчеты_без_паспортов";
        DirectoryInfo directory = new(BaseVM.PasFolderPath);
        if (!directory.Exists)
        {
            #region MessageFailedToOpenPassportDirectory

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Ошибка",
                    ContentMessage =
                        "Не удалось открыть сетевое хранилище паспортов:" +
                        $"{Environment.NewLine}{directory.FullName}",
                    MinWidth = 400,
                    MinHeight = 170,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        await Dispatcher.UIThread.InvokeAsync(() => progressBar = new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM_DB;
        progressBarVM.ExportType = ExportType;
        progressBarVM.ExportName = "Выгрузка списка отчётов";
        progressBarVM.ValueBar = 2;
        var loadStatus = "Создание временной БД";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

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
            return;
        }

        loadStatus = "Определение списка файлов";
        progressBarVM.ValueBar = 5;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        Worksheet = excelPackage.Workbook.Worksheets.Add("Список отчётов без файла паспорта");

        #region ColumnHeaders

        Worksheet.Cells[1, 1].Value = "Рег. №";
        Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        Worksheet.Cells[1, 3].Value = "ОКПО";
        Worksheet.Cells[1, 4].Value = "Форма";
        Worksheet.Cells[1, 5].Value = "Дата начала периода";
        Worksheet.Cells[1, 6].Value = "Дата конца периода";
        Worksheet.Cells[1, 7].Value = "Номер корректировки";
        Worksheet.Cells[1, 8].Value = "Количество строк";
        Worksheet.Cells[1, 9].Value = "№ п/п";
        Worksheet.Cells[1, 10].Value = "код";
        Worksheet.Cells[1, 11].Value = "дата";
        Worksheet.Cells[1, 12].Value = "номер паспорта (сертификата)";
        Worksheet.Cells[1, 13].Value = "тип";
        Worksheet.Cells[1, 14].Value = "радионуклиды";
        Worksheet.Cells[1, 15].Value = "номер";
        Worksheet.Cells[1, 16].Value = "количество, шт";
        Worksheet.Cells[1, 17].Value = "суммарная активность, Бк";
        Worksheet.Cells[1, 18].Value = "код ОКПО изготовителя";
        Worksheet.Cells[1, 19].Value = "дата выпуска";
        Worksheet.Cells[1, 20].Value = "категория";
        Worksheet.Cells[1, 21].Value = "НСС, мес";
        Worksheet.Cells[1, 22].Value = "код формы собственности";
        Worksheet.Cells[1, 23].Value = "код ОКПО правообладателя";
        Worksheet.Cells[1, 24].Value = "вид";
        Worksheet.Cells[1, 25].Value = "номер";
        Worksheet.Cells[1, 26].Value = "дата";
        Worksheet.Cells[1, 27].Value = "поставщика или получателя";
        Worksheet.Cells[1, 28].Value = "перевозчика";
        Worksheet.Cells[1, 29].Value = "наименование";
        Worksheet.Cells[1, 30].Value = "тип";
        Worksheet.Cells[1, 31].Value = "номер";

        if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
        {
            Worksheet.Cells.AutoFitColumns();
        }
        Worksheet.View.FreezePanes(2, 1);

        #endregion

        List<string> pasNames = [];
        List<string[]> pasUniqParam = [];
        List<FileInfo> files = [];
        try
        {
            files.AddRange(directory.GetFiles("*#*#*#*#*.pdf", SearchOption.AllDirectories));
        }
        catch (Exception)
        {
            #region MessageFailedToOpenPassportDirectory

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Не удалось открыть сетевое хранилище паспортов:" +
                                     $"{Environment.NewLine}{directory.FullName}",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }

        pasNames.AddRange(files.Select(file => file.Name.Remove(file.Name.Length - 4)));
        pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));

        loadStatus = "Загрузка форм 1.1";
        progressBarVM.ValueBar = 10;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);

        var dtoList = dbReadOnly.ReportsCollectionDbSet //TODO
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
            .ToArray()
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.1")
                .SelectMany(rep => rep.Rows11
                    .Where(form11 => form11.OperationCode_DB is "11" or "85" && form11.Category_DB is 1 or 2 or 3)
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

        loadStatus = "Сравнение с паспортами";
        progressBarVM.ValueBar = 70;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 20 };
        ConcurrentBag<Form11DTO> dtoToExcelThreadSafe = [];
        await Parallel.ForEachAsync(dtoList, parallelOptions, (dto, token) =>
        {
            var findPasFile = pasUniqParam.Any(pasParam =>
                ComparePasParam(ConvertPrimToDash(dto.CreatorOKPO), pasParam[0])
                && ComparePasParam(ConvertPrimToDash(dto.Type), pasParam[1])
                && ComparePasParam(ConvertDateToYear(dto.CreationDate), pasParam[2])
                && ComparePasParam(ConvertPrimToDash(dto.PassportNumber), pasParam[3])
                && ComparePasParam(ConvertPrimToDash(dto.FactoryNumber), pasParam[4]));
            if (!findPasFile)
            {
                dtoToExcelThreadSafe.Add(dto);
            }
            return default;
        });

        var currentRow = 2;
        foreach (var dto in dtoToExcelThreadSafe)
        {
            #region BindingCells

            Worksheet.Cells[currentRow, 1].Value = dto.RegNoRep;
            Worksheet.Cells[currentRow, 2].Value = dto.ShortJurLico;
            Worksheet.Cells[currentRow, 3].Value = dto.OkpoRep;
            Worksheet.Cells[currentRow, 4].Value = dto.FormNum;
            Worksheet.Cells[currentRow, 5].Value = ConvertToExcelDate(dto.StartPeriod, Worksheet, currentRow, 5);
            Worksheet.Cells[currentRow, 6].Value = ConvertToExcelDate(dto.EndPeriod, Worksheet, currentRow, 6);
            Worksheet.Cells[currentRow, 7].Value = dto.CorrectionNumber;
            Worksheet.Cells[currentRow, 8].Value = dto.RowCount;
            Worksheet.Cells[currentRow, 9].Value = dto.NumberInOrder;
            Worksheet.Cells[currentRow, 10].Value = ConvertToExcelString(dto.OperationCode);
            Worksheet.Cells[currentRow, 11].Value = ConvertToExcelDate(dto.OperationDate, Worksheet, currentRow, 11);
            Worksheet.Cells[currentRow, 12].Value = ConvertToExcelString(dto.PassportNumber);
            Worksheet.Cells[currentRow, 13].Value = ConvertToExcelString(dto.Type);
            Worksheet.Cells[currentRow, 14].Value = ConvertToExcelString(dto.Radionuclids);
            Worksheet.Cells[currentRow, 15].Value = ConvertToExcelString(dto.FactoryNumber);
            Worksheet.Cells[currentRow, 16].Value = dto.Quantity is null ? "-" : dto.Quantity;
            Worksheet.Cells[currentRow, 17].Value = ConvertToExcelDouble(dto.Activity);
            Worksheet.Cells[currentRow, 18].Value = ConvertToExcelString(dto.CreatorOKPO);
            Worksheet.Cells[currentRow, 19].Value = ConvertToExcelDate(dto.CreationDate, Worksheet, currentRow, 19);
            Worksheet.Cells[currentRow, 20].Value = dto.Category is null ? "-" : dto.Category;
            Worksheet.Cells[currentRow, 21].Value = dto.SignedServicePeriod is null ? "-" : dto.SignedServicePeriod;
            Worksheet.Cells[currentRow, 22].Value = dto.PropertyCode is null ? "-" : dto.PropertyCode;
            Worksheet.Cells[currentRow, 23].Value = ConvertToExcelString(dto.Owner);
            Worksheet.Cells[currentRow, 24].Value = dto.DocumentVid is null ? "-" : dto.DocumentVid;
            Worksheet.Cells[currentRow, 25].Value = ConvertToExcelString(dto.DocumentNumber);
            Worksheet.Cells[currentRow, 26].Value = ConvertToExcelDate(dto.DocumentDate, Worksheet, currentRow, 26);
            Worksheet.Cells[currentRow, 27].Value = ConvertToExcelString(dto.ProviderOrRecieverOKPO);
            Worksheet.Cells[currentRow, 28].Value = ConvertToExcelString(dto.TransporterOKPO);
            Worksheet.Cells[currentRow, 29].Value = ConvertToExcelString(dto.PackName);
            Worksheet.Cells[currentRow, 30].Value = ConvertToExcelString(dto.PackType);
            Worksheet.Cells[currentRow, 31].Value = ConvertToExcelString(dto.PackNumber);

            #endregion

            currentRow++;
        }
        Worksheet.View.FreezePanes(2, 1);

        loadStatus = "Сохранение";
        progressBarVM.ValueBar = 95;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts);

        loadStatus = "Завершение выгрузки";
        progressBarVM.ValueBar = 100;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";
        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }
}