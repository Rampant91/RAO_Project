using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.DTO;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Passports;

//  Excel -> Паспорта -> Паспорта без отчетов
public class ExcelExportPasWithoutRepAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Паспорта без отчетов";
        List<string> pasNames = new();
        List<string[]> pasUniqParam = new();
        DirectoryInfo directory = new(BaseVM.PasFolderPath);
        List<FileInfo> files = new();
        try
        {
            files.AddRange(directory.GetFiles("*#*#*#*#*.pdf", SearchOption.AllDirectories));
        }
        catch
        {
            #region MessageFailedToOpenPassportDirectory

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Ошибка",
                    ContentMessage =
                        "Не удалось открыть сетевое хранилище паспортов:" +
                        $"{Environment.NewLine}{directory.FullName}",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }

        #region MessageInputCategoryNums

        var res =
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отмена", IsCancel = true }
                },
                ContentTitle = "Выбор категории",
                ContentMessage = "Введите через запятую номера категорий (допускается несколько значений)",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        List<short?> categories = new() { 1, 2, 3, 4, 5 };
        if (res.Button is null or "Отмена") return;
        try
        {
            categories = Regex.Replace(res.Message, "[^\\d,]", "")
                .Split(',').Select(short.Parse).Cast<short?>().ToList();
        }
        catch
        {
            #region MessageInvalidCategoryNums

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Номера категорий не были введены, либо были введены некорректно" +
                        $"{Environment.NewLine}Выгрузка будет осуществлена по всем категориям (1-5)",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion
        }

        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
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

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        Worksheet = excelPackage.Workbook.Worksheets.Add($"Список паспортов без отчетов");

        #region Headers

        Worksheet.Cells[1, 1].Value = "Путь до папки";
        Worksheet.Cells[1, 2].Value = "Имя файла";
        Worksheet.Cells[1, 3].Value = "Код ОКПО изготовителя";
        Worksheet.Cells[1, 4].Value = "Тип";
        Worksheet.Cells[1, 5].Value = "Год выпуска";
        Worksheet.Cells[1, 6].Value = "Номер паспорта";
        Worksheet.Cells[1, 7].Value = "Номер";

        #endregion
        
        pasNames.AddRange(files.Select(file => file.Name.Remove(file.Name.Length - 4)));
        pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);
        var forms11 = await dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Where(x => x.FormNum_DB == "1.1")
            .Include(x => x.Rows11)
            .SelectMany(x => x.Rows11
                .Where(y => (y.OperationCode_DB == "11" || y.OperationCode_DB == "85")
                            && categories.Contains(y.Category_DB))
                .Select(form11 => new Form11ShortDTO
                {
                    CreatorOKPO = form11.CreatorOKPO_DB,
                    Type = form11.Type_DB,
                    CreationDate = form11.CreationDate_DB,
                    PassportNumber = form11.PassportNumber_DB,
                    FactoryNumber = form11.FactoryNumber_DB
                }))
            .ToListAsync(cancellationToken: cts.Token);

        var i = 0;
        ConcurrentBag<FileInfo> filesToRemove = new();
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 20 };
        try
        {
            await Parallel.ForEachAsync(pasUniqParam, parallelOptions, (pasParam, token) =>
            {
                if (forms11.Any(form11 =>
                        ComparePasParam(ConvertPrimToDash(form11.CreatorOKPO), pasParam[0])
                        && ComparePasParam(ConvertPrimToDash(form11.Type), pasParam[1])
                        && ComparePasParam(ConvertDateToYear(form11.CreationDate), pasParam[2])
                        && ComparePasParam(ConvertPrimToDash(form11.PassportNumber), pasParam[3])
                        && ComparePasParam(ConvertPrimToDash(form11.FactoryNumber), pasParam[4])))
                {
                    filesToRemove.Add(files.First(file =>
                        file.Name.Remove(file.Name.Length - 4) ==
                        $"{pasParam[0]}#{pasParam[1]}#{pasParam[2]}#{pasParam[3]}#{pasParam[4]}"));
                }
                i++;
                return default;
            });
        }
        catch
        {
            cts.Dispose();
            return;
        }
        
        foreach (var fileToRemove in filesToRemove.ToArray())
        {
            files.Remove(fileToRemove);
        }

        var currentRow = 2;
        foreach (var file in files)
        {
            var pasName = file.Name.TrimEnd(".pdf".ToCharArray());
            Worksheet.Cells[currentRow, 1].Value = file.DirectoryName;
            Worksheet.Cells[currentRow, 2].Value = pasName;
            Worksheet.Cells[currentRow, 3].Value = ConvertToExcelString(pasName.Split('#')[0]);
            Worksheet.Cells[currentRow, 4].Value = ConvertToExcelString(pasName.Split('#')[1]);
            Worksheet.Cells[currentRow, 5].Value = ConvertToExcelDate(pasName.Split('#')[2], Worksheet, currentRow, 5);
            Worksheet.Cells[currentRow, 6].Value = ConvertToExcelString(pasName.Split('#')[3]);
            Worksheet.Cells[currentRow, 7].Value = ConvertToExcelString(pasName.Split('#')[4]);
            currentRow++;
        }

        if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
        {
            Worksheet.Cells.AutoFitColumns();
        }
        Worksheet.View.FreezePanes(2, 1);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}