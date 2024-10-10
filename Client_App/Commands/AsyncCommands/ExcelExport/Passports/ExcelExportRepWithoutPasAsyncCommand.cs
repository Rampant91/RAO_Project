using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.DTO;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Passports;

/// <summary>
/// Excel -> Паспорта -> Отчеты без паспортов.
/// </summary>
public class ExcelExportRepWithoutPasAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Отчеты_без_паспортов";
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(2, "Проверка списка файлов", "Выгрузка списка отчётов", ExportType);
        var files = await GetFilesFromPasDirectory(progressBar, cts);

        progressBarVM.SetProgressBar(7, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(10, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);

        progressBarVM.SetProgressBar(18, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);
        Worksheet = excelPackage.Workbook.Worksheets.Add("Список отчётов без файла паспорта");

        #region FillHeaders

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

        progressBarVM.SetProgressBar(20, "Формирование списка форм 1.1");
        List<string[]> pasUniqParam = [];
        var dtoList = await GetFilteredForms(tmpDbPath, files, pasUniqParam, cts);

        progressBarVM.SetProgressBar(40, "Поиск совпадений");
        ConcurrentBag<Form11ShortDTO> dtoToExcelThreadSafe = [];
        await FindFilesWithOutReport(pasUniqParam, dtoList, dtoToExcelThreadSafe, progressBarVM, cts);

        progressBarVM.SetProgressBar(60, "Загрузка совпавших форм");
        var matchedFormsList = await LoadMatchedForms(dtoToExcelThreadSafe.ToList(), tmpDbPath, progressBarVM, cts);

        progressBarVM.SetProgressBar(90, "Экспорт данных в .xlsx");
        await Task.Run(async () => await ExportToExcel(matchedFormsList), cts.Token);

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

    #region FindFilesWithOutReport

    /// <summary>
    /// Для каждой формы из списка проверяет соответствие уникальных параметров файла паспорта.
    /// Добавляет в выходной список те, для которых совпадение не найдено.
    /// </summary>
    /// <param name="pasUniqParam">Список массивов уникальных параметров паспортов, полученный из названий файлов паспортов.</param>
    /// <param name="dtoList">Список DTO'шек форм 1.1.</param>
    /// <param name="dtoToExcelThreadSafe">Потокобезопасный список отфильтрованных DTO'шек форм 1.1.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Обновление потокобезопасного списка.</returns>
    private static async Task FindFilesWithOutReport(List<string[]> pasUniqParam, List<Form11ShortDTO> dtoList, 
        ConcurrentBag<Form11ShortDTO> dtoToExcelThreadSafe, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        ParallelOptions parallelOptions = new()
        {
            CancellationToken = cts.Token,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };
        var count = 0;
        double progressBarDoubleValue = progressBarVM.ValueBar;
        await Parallel.ForEachAsync(dtoList, parallelOptions, (dto, token) =>
        {
            var findPasFile = pasUniqParam.Any(pasParam => ComparePasParam(
                ConvertPrimToDash(dto.CreatorOKPO) 
                + ConvertPrimToDash(dto.Type)
                + ConvertDateToYear(dto.CreationDate)
                + ConvertPrimToDash(dto.PassportNumber)
                + ConvertPrimToDash(dto.FactoryNumber), 
                pasParam[0] + pasParam[1] + pasParam[2] + pasParam[3] + pasParam[4]));
            if (!findPasFile)
            {
                dtoToExcelThreadSafe.Add(dto);
            }
            count++;
            progressBarDoubleValue += (double)20 / dtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Проверено {count} из {dtoList.Count} форм");
            return default;
        });
    }

    #endregion

    #region ExportToExcel

    /// <summary>
    /// Экспорт отсортированного списка форм в .xlsx.
    /// </summary>
    /// <param name="matchedFormsList">Неотсортированный список форм.</param>
    /// <returns></returns>
    private Task ExportToExcel(List<Form11ExtendedDTO> matchedFormsList)
    {
        var currentRow = 2;
        foreach (var dto in matchedFormsList
                     .OrderBy(x => x.RegNoRep)
                     .ThenBy(x => x.OkpoRep)
                     .ThenBy(x => x.FormNum)
                     .ThenBy(x => StringReverse(x.StartPeriod))
                     .ThenBy(x => StringReverse(x.EndPeriod))
                     .ThenBy(x => x.NumberInOrder))
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
        return Task.CompletedTask;
    }

    #endregion

    #region GetFilteredForms

    /// <summary>
    /// Формирование списка уникальных параметров паспортов и загрузка из БД отфильтрованного списка DTO'шек форм 1.1.
    /// </summary>
    /// <param name="dbReadOnlyPath">Полный путь до временного файла БД.</param>
    /// <param name="files">Список файлов паспортов.</param>
    /// <param name="pasUniqParam">Список массивов уникальных параметров паспортов, полученный из названий файлов паспортов.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Отфильтрованный массив DTO'шек форм 1.1.</returns>
    private static async Task<List<Form11ShortDTO>> GetFilteredForms(string dbReadOnlyPath, List<FileInfo> files, List<string[]> pasUniqParam, 
        CancellationTokenSource cts)
    {
        List<string> pasNames = [];
        pasNames.AddRange(files.Select(file => file.Name.Remove(file.Name.Length - 4)));
        pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);

        var form11ShortList = await dbReadOnly.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.1")
                .SelectMany(rep => rep.Rows11
                    .Where(x => (x.Category_DB == 1 || x.Category_DB == 2 || x.Category_DB == 3) 
                                && (x.OperationCode_DB == "11" || x.OperationCode_DB == "85"))
                    .Select(form11 => new Form11ShortDTO(
                        form11.Id, 
                        form11.Category_DB, 
                        form11.CreationDate_DB, 
                        form11.CreatorOKPO_DB, 
                        form11.FactoryNumber_DB, 
                        form11.OperationCode_DB, 
                        form11.PassportNumber_DB,
                        form11.Type_DB))))
            .ToListAsync(cts.Token);

        return form11ShortList
            .Select(x => new Form11ShortDTO(
                x.Id,
                x.Category,
                ReplaceRestrictedSymbols(x.CreationDate),
                ReplaceRestrictedSymbols(x.CreatorOKPO),
                ReplaceRestrictedSymbols(x.FactoryNumber),
                x.OperationCode,
                ReplaceRestrictedSymbols(x.PassportNumber),
                ReplaceRestrictedSymbols(x.Type)))
            .ToList();
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

    #region LoadMatchedForms

    /// <summary>
    /// Загрузка из БД полных строчек форм 1.1 вместе с данными организации.
    /// </summary>
    /// <param name="form11DtoList">Список dto'шек форм 1.1, для которых нужно загрузить полные строчки.</param>
    /// <param name="dbReadOnlyPath">Полный путь к временному файлу БД.</param>
    /// <param name="progressBarVM">ViewModel прогрессбара.</param>
    /// <returns>Список полных строчек форм 1.1 вместе с данными организации.</returns>
    private async Task<List<Form11ExtendedDTO>> LoadMatchedForms(List<Form11ShortDTO> form11DtoList, string dbReadOnlyPath, 
        AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
    {
        await using var dbReadOnly = new DBModel(dbReadOnlyPath);
        var form11ExtendedList = new List<Form11ExtendedDTO>();

        var count = 0;
        double progressBarDoubleValue = progressBarVM.ValueBar;
        foreach (var dto in form11DtoList)
        {
            var form11 = await dbReadOnly.form_11
                .AsSplitQuery()
                .Include(form11 => form11.Report).ThenInclude(rep => rep.Reports).ThenInclude(reps => reps.Master_DB).ThenInclude(x => x.Rows10)
                .Include(form11 => form11.Report).ThenInclude(rep => rep.Rows11)
                .AsQueryable()
                .FirstAsync(form11 => form11.Id == dto.Id, cts.Token);
            if (form11.Report?.Reports is null) continue;
            var rep = form11.Report;
            var reps = form11.Report.Reports;
            form11ExtendedList.Add(new Form11ExtendedDTO
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
            });
            count++;
            progressBarDoubleValue += (double)30 / form11DtoList.Count;
            progressBarVM.SetProgressBar((int)Math.Floor(progressBarDoubleValue),
                $"Загружено {count} из {form11DtoList.Count} форм");
        }
        return form11ExtendedList;
    }

    #endregion

    #region Form11ShortDTO

    private class Form11ShortDTO(int id, short? category, string creationDate, string creatorOkpo, string factoryNumber, string opCode, string passportNumber, string type)
    {
        public readonly int Id = id;

        public readonly short? Category = category;

        public readonly string CreationDate = creationDate;

        public readonly string CreatorOKPO = creatorOkpo;
        
        public readonly string FactoryNumber = factoryNumber;

        public readonly string OperationCode = opCode;

        public readonly string PassportNumber = passportNumber;

        public readonly string Type = type;
    }

    #endregion
}