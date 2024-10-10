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
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.DTO;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Passports;

/// <summary>
/// Excel -> Паспорта -> Паспорта без отчетов
/// </summary>
public class ExcelExportPasWithoutRepAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Паспорта_без_отчетов";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(2, "Формирование списка файлов", "Выгрузка списка паспортов", ExportType);
        var files = await GetFilesFromPasDirectory(progressBar, cts);

        progressBarVM.SetProgressBar(5, "Запрос списка категорий");
        var categories = await GetCategories(progressBar, cts);

        progressBarVM.SetProgressBar(8, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(10, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);

        progressBarVM.SetProgressBar(18, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);
        Worksheet = excelPackage.Workbook.Worksheets.Add("Список паспортов без отчетов");

        #region FillHeaders

        Worksheet.Cells[1, 1].Value = "Путь до папки";
        Worksheet.Cells[1, 2].Value = "Имя файла";
        Worksheet.Cells[1, 3].Value = "Код ОКПО изготовителя";
        Worksheet.Cells[1, 4].Value = "Тип";
        Worksheet.Cells[1, 5].Value = "Год выпуска";
        Worksheet.Cells[1, 6].Value = "Номер паспорта";
        Worksheet.Cells[1, 7].Value = "Номер";

        #endregion

        progressBarVM.SetProgressBar(20, "Формирование списка форм 1.1");
        var filteredForm11DtoArray = await GetFilteredForms(tmpDbPath, categories, cts);

        progressBarVM.SetProgressBar(50, "Поиск совпадений");
        await FindFilesWithOutReport(files, filteredForm11DtoArray, progressBarVM, cts);

        progressBarVM.SetProgressBar(90, "Экспорт данных в .xlsx");
        await FillRows(files);

        progressBarVM.SetProgressBar(95, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);

        progressBarVM.SetProgressBar(98, "Очистка временных данных");
        try
        {
            File.Delete(tmpDbPath);
        }
        catch
        {
            // ignored
        }

        progressBarVM.SetProgressBar(100, "Завершение выгрузки");
        await progressBar.CloseAsync();
    }

    #region FillRows

    /// <summary>
    /// Заполняет выгрузку в Excel данными.
    /// </summary>
    /// <param name="files">Список имён паспортов без отчетов.</param>
    /// <returns></returns>
    private Task FillRows(List<FileInfo> files)
    {
        var currentRow = 2;
        foreach (var file in files)
        {
            var pasName = file.Name.TrimEnd(".pdf".ToCharArray());

            #region FillRows

            Worksheet.Cells[currentRow, 1].Value = file.DirectoryName;
            Worksheet.Cells[currentRow, 2].Value = pasName;
            Worksheet.Cells[currentRow, 3].Value = ConvertToExcelString(pasName.Split('#')[0]);
            Worksheet.Cells[currentRow, 4].Value = ConvertToExcelString(pasName.Split('#')[1]);
            Worksheet.Cells[currentRow, 5].Value = ConvertToExcelDate(pasName.Split('#')[2], Worksheet, currentRow, 5);
            Worksheet.Cells[currentRow, 6].Value = ConvertToExcelString(pasName.Split('#')[3]);
            Worksheet.Cells[currentRow, 7].Value = ConvertToExcelString(pasName.Split('#')[4]);

            #endregion

            currentRow++;
        }
        if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
        {
            Worksheet.Cells.AutoFitColumns();
        }
        Worksheet.View.FreezePanes(2, 1);

        return Task.CompletedTask;
    }

    #endregion

    #region FindFilesWithOutReport

    /// <summary>
    /// Для каждого файла из списка проверяет наличие отчёта в БД.
    /// Удаляет из списка файлов те, для которых совпадение найдено.
    /// </summary>
    /// <param name="files">Список файлов паспортов.</param>
    /// <param name="filteredForm11DtoArray">Отфильтрованный массив DTO'шек форм 1.1.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns></returns>
    private static async Task FindFilesWithOutReport(List<FileInfo> files, Form11ShortDTO[] filteredForm11DtoArray, 
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        List<string> pasNames = [];
        List<string[]> pasUniqParam = [];
        pasNames.AddRange(files.Select(file => file.Name.Remove(file.Name.Length - 4)));
        pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));

        ConcurrentBag<FileInfo> filesToRemove = [];
        ParallelOptions parallelOptions = new()
        {
            CancellationToken = cts.Token,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };
        var count = 0;
        double progressBarDoubleValue = progressBarVM.ValueBar;
        await Parallel.ForEachAsync(pasUniqParam, parallelOptions, (pasParam, token) =>
        {
            if (filteredForm11DtoArray.Any(form11 => ComparePasParam(
                        ConvertPrimToDash(form11.CreatorOKPO) 
                        + ConvertPrimToDash(form11.Type) 
                        + ConvertDateToYear(form11.CreationDate) 
                        + ConvertPrimToDash(form11.PassportNumber) 
                        + ConvertPrimToDash(form11.FactoryNumber),
                        pasParam[0] + pasParam[1] + pasParam[2] + pasParam[3] + pasParam[4])))
            {
                filesToRemove.Add(files.First(file =>
                    file.Name.Remove(file.Name.Length - 4) ==
                    $"{pasParam[0]}#{pasParam[1]}#{pasParam[2]}#{pasParam[3]}#{pasParam[4]}"));
            }
            count++;
            progressBarDoubleValue += (double)40 / pasUniqParam.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Проверено {count} из {pasUniqParam.Count} паспортов");
            return default;
        });

        foreach (var fileToRemove in filesToRemove.ToArray())
        {
            files.Remove(fileToRemove);
        }
    }

    #endregion

    #region GetCategories

    /// <summary>
    /// Выводит сообщение с запросом списка категорий, парсит входные данные и возвращает HashSet.
    /// </summary>
    /// ///
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>HashSet категорий.</returns>
    private async Task<HashSet<short?>> GetCategories(AnyTaskProgressBar progressBar, CancellationTokenSource cts)
    {
        HashSet<short?> categories;

        #region MessageInputCategoryNums

        var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отмена", IsCancel = true }
                ],
                CanResize = true,
                ContentTitle = "Выбор категории",
                ContentMessage = "Введите через запятую номера категорий " +
                                 $"{Environment.NewLine}(допускается несколько значений)",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(progressBar ?? Desktop.MainWindow));

        #endregion

        if (res.Button is null or "Отмена")
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        var categoryArray = (res.Message ?? string.Empty)
            .Replace(" ", string.Empty)
            .Split(',');

        if (!categoryArray.All(category => short.TryParse(category, out _)))
        {
            categories = [1, 2, 3, 4, 5];

            #region MessageInvalidCategoryNums

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Номера категорий не были введены, либо были введены некорректно. " +
                        $"{Environment.NewLine}Выгрузка будет осуществлена по всем категориям (1-5).",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion
        }
        else
        {
            categories = categoryArray
                .Select(short.Parse)
                .Cast<short?>()
                .ToHashSet();
        }
        return categories;
    }

    #endregion

    #region GetFilteredForms

    /// <summary>
    /// Загрузка из БД отфильтрованного списка DTO'шек форм 1.1.
    /// </summary>
    /// <param name="dbReadOnlyPath">Полный путь до временного файла БД.</param>
    /// <param name="categories">HashSet категорий.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Отфильтрованный массив DTO'шек форм 1.1.</returns>
    private static async Task<Form11ShortDTO[]> GetFilteredForms(string dbReadOnlyPath, HashSet<short?> categories, 
        CancellationTokenSource cts)
    {
        await using var dbReadOnly = new DBModel(dbReadOnlyPath);
        var filteredForms = await dbReadOnly.ReportCollectionDbSet
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
            .ToArrayAsync(cts.Token);

        return filteredForms.Select(x => new Form11ShortDTO
            {
                CreatorOKPO = ReplaceRestrictedSymbols(x.CreatorOKPO),
                Type = ReplaceRestrictedSymbols(x.Type),
                CreationDate = ReplaceRestrictedSymbols(x.CreationDate),
                PassportNumber = ReplaceRestrictedSymbols(x.PassportNumber),
                FactoryNumber = ReplaceRestrictedSymbols(x.FactoryNumber)
            })
            .ToArray();
    }

    /// <summary>
    /// Заменяет в строчке запрещённые символы на "_".
    /// </summary>
    /// <param name="str">Входная строчка.</param>
    /// <returns>Строчка, в которой заменены запрещённые символы.</returns>
    private static string ReplaceRestrictedSymbols(string str)
    {
        return new Regex("[\\\\/:*?\"<>|]").Replace(str, "_");
    }

    #endregion
}