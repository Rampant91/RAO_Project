using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.X11;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using DynamicData;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

using MsBox.Avalonia.Enums;
namespace Client_App.Commands.AsyncCommands
{
    public class CalculateVolumesRaoProcessingByPeriodAsyncCommand : ExcelBaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            var cts = new CancellationTokenSource();

            var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
            var progressBarVM = progressBar.AnyTaskProgressBarVM;

            //Выбираем файл со списком предприятий
            var companyList = await SelectFileWithCompanyList();
            if (companyList is null) return;


            var period = await SelectDatePeriod();
            if (period == null) return;

            DateOnly startPeriod = period.Value.startPeriod;
            DateOnly endPeriod = period.Value.endPeriod;

            progressBarVM.SetProgressBar(10, "Загружаем отчеты организаций");

            var result = await GetVolumeProccessingInfoFromDB(companyList, startPeriod, endPeriod, progressBarVM , cts);

            var fileName = $"Сведения о переработке РАО за период {startPeriod}-{endPeriod}";
            await CreateAndFillExcelFile(fileName, startPeriod, endPeriod, result, progressBar, cts);
        }

        private async Task<List<Tuple<string, string>>?> SelectFileWithCompanyList()
        {
            string[] extensions = ["xlsx", "XLSX"];
            var answer = await GetSelectedFilesFromDialog("Excel", extensions);
            if (answer is null) return null;

            if (answer[0] is "") return null;

            var SourceFile = new FileInfo(answer[0]);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                using ExcelPackage excelPackageTry = new(SourceFile);
            }
            catch (Exception ex)
            {
                #region ReadFileError
                await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Ошибка",
                    ContentHeader = $"Произошла ошибка при импорте файла {SourceFile.Name}",
                    ContentMessage = $"Описание:\n" +
                                     $"{ex.Message}",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow));
                #endregion
                return null;
            }
            ExcelPackage excelPackage = new(SourceFile);

            var worksheet = excelPackage.Workbook.Worksheets[0];
            var patternIsValid = worksheet.Cells["A1"].Text.ToLower() is "регномер"
                && worksheet.Cells["B1"].Text.ToLower() is "окпо";

            if (!patternIsValid)
            {
                #region InvalidDataFormatMessage

                await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                    .GetMessageBoxCustom(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true }
                        ],
                        ContentTitle = "Импорт из .xlsx",
                        ContentHeader = "Уведомление",
                        ContentMessage = $"Не удалось импортировать данные из {SourceFile.FullName}." +
                                         $"{Environment.NewLine}Не соответствует формат данных!",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    }).ShowWindowDialogAsync(Desktop.MainWindow));

                #endregion
                return null;
            }
            var companyList = new List<Tuple<string, string>>();
            var row = 1;
            while (worksheet.Cells[$"A{row}"].Text is not ""
                || worksheet.Cells[$"B{row}"].Text is not "")
            {
                try
                {
                    row++;

                    if (worksheet.Cells[$"A{row}"].Text is ""
                        || worksheet.Cells[$"B{row}"].Text is "")
                        continue;

                    companyList.Add(new Tuple<string, string>(worksheet.Cells[$"A{row}"].Text, worksheet.Cells[$"B{row}"].Text));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return companyList;
        }

        private async Task<(DateOnly startPeriod, DateOnly endPeriod)?> SelectDatePeriod()
        {
            try
            {
                //указываем период дат
                var period = await Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    var window = new AskDatePeriodMessageWindow();
                    return await window.ShowDialog<(string command, DateOnly initialDate, DateOnly residualDate)>(Desktop.MainWindow);
                });


                if (period.command is not "Ок") return null;

                return  (period.initialDate,
                        period.residualDate);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<VolumeProccessingInfoDTO>> GetVolumeProccessingInfoFromDB(
            List<Tuple<string, string>> companyList,
            DateOnly startPeriod,
            DateOnly endPeriod,
            AnyTaskProgressBarVM progressBarVM,
            CancellationTokenSource cts)
        {
            var result = new List<VolumeProccessingInfoDTO>();

            var allowedStatuses = companyList.Select(c => c.Item2).ToHashSet();

            var rows16 = StaticConfiguration.DBModel.form_16
            .AsNoTracking()
            .Where(row16 => row16.OperationCode_DB == "44" && allowedStatuses.Contains(row16.StatusRAO_DB))
            .Select(row16 => new
            {
                row16.OperationDate_DB,
                row16.StatusRAO_DB,
                row16.Volume_DB,
                row16.CodeRAO_DB,
                row16.ReportId
            })
            .ToList();

            var progress = 20.0;
            var inc = (90.0 - 20.0) / rows16.Count;
            progressBarVM.SetProgressBar((int)progress, "Собираем сведения об перарботке РАО");
            // Для каждой организации собираем все отчеты за указанный период
            foreach (var row16 in rows16)
            {
                if (!DateOnly.TryParse(row16.OperationDate_DB, out var date)
                    || !(startPeriod <= date && date <= endPeriod))
                {
                    progress += inc;
                    progressBarVM.SetProgressBar((int)progress, "Собираем сведения об перарботке РАО");
                    continue;
                }

                string companyName = "";
                bool IsLiquid = row16.CodeRAO_DB.Length > 0 && row16.CodeRAO_DB[0] == '1';
                bool IsSolid = row16.CodeRAO_DB.Length > 0 && row16.CodeRAO_DB[0] == '2';
                double.TryParse(
                    row16.Volume_DB
                    .Replace('.', ',')
                    .Replace('е', 'E')
                    .Replace('Е', 'E')
                    .Replace('e', 'E'),
                   out var volume);
                if ((IsLiquid || IsSolid) && volume > 0)
                {
                    if (result.Any(r => r.reportIdList.Any(id => id == row16.ReportId)))
                    {
                        companyName = result.First(r => r.reportIdList.Any(id => id == row16.ReportId)).name;

                    }
                    else
                    {
                        var master = StaticConfiguration.DBModel.ReportCollectionDbSet
                            .Where(rep => rep.Id == row16.ReportId)
                            .Select(rep => new
                            {
                                rep.Reports.Master_DB.Rows10
                            })
                            .FirstOrDefault();

                        if (!string.IsNullOrWhiteSpace(master.Rows10[1].ShortJurLico_DB))
                            companyName = master.Rows10[1].ShortJurLico_DB;
                        else if (!string.IsNullOrWhiteSpace(master.Rows10[1].JurLico_DB)
                            && master.Rows10[1].JurLico_DB != master.Rows10[0].JurLico_DB)
                            companyName = master.Rows10[1].JurLico_DB;
                        else if (!string.IsNullOrWhiteSpace(master.Rows10[0].ShortJurLico_DB))
                            companyName = master.Rows10[0].ShortJurLico_DB;
                        else
                            companyName = master.Rows10[0].JurLico_DB;
                    }

                    if (result.Any(r =>
                        r.status == row16.StatusRAO_DB
                        && r.name == companyName))
                    {
                        var match = result.FirstOrDefault(r =>
                            r.status == row16.StatusRAO_DB
                            && r.name == companyName);

                        match.liquid += IsLiquid ? volume : 0;
                        match.solid += IsSolid ? volume : 0;
                        if (!match.reportIdList.Any(id => id == (int)row16.ReportId))
                            match.reportIdList.Add((int)row16.ReportId);
                    }
                    else
                    {
                        var raoOwner = StaticConfiguration.DBModel.form_10.First(row10 => row10.Okpo_DB == row16.StatusRAO_DB);

                        var raoOwnerName = !string.IsNullOrEmpty(raoOwner.ShortJurLico_DB) ? raoOwner.ShortJurLico_DB : raoOwner.JurLico_DB;
                        result.Add(new()
                        {
                            name = companyName,
                            status = row16.StatusRAO_DB,
                            ownerRao = raoOwnerName,
                            liquid = IsLiquid ? volume : 0,
                            solid = IsSolid ? volume : 0,
                            reportIdList = new List<int>()
                        });
                        result.Last().reportIdList.Add(row16.ReportId ?? 0);
                    }
                }
                progress += inc;
                progressBarVM.SetProgressBar((int)progress, "Собираем сведения об перарботке РАО");
            }
            return result.OrderBy(r => r.name).ToList();

        }
        private async Task CreateAndFillExcelFile(string fileName,
            DateOnly startPeriod,
            DateOnly endPeriod,
            List<VolumeProccessingInfoDTO> result,
            AnyTaskProgressBar progressBar,
            CancellationTokenSource cts)
        {

            var progressBarVM = progressBar.AnyTaskProgressBarVM;

            var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

            progressBarVM.SetProgressBar(90, "Инициализация Excel пакета");
            using var excelPackage = await InitializeExcelPackage(fullPath);

            excelPackage.Workbook.Worksheets.Add("Справка");

            try
            {
                Worksheet = excelPackage.Workbook.Worksheets[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Worksheet.Cells["A1:F1"].Merge = true;
            Worksheet.Cells["A3:F3"].Merge = true;

            Worksheet.Cells["A1:F1"].Value = "Справка";
            Worksheet.Cells["A3:F3"].Value = $"Сведения о переработке РАО предприятиями ГК \"Росатом\" за период {startPeriod}-{endPeriod}, " +
                $"имеющиеся в БД ЦИАЦ СГУК РВ и РАО на {DateTime.Today.ToString("dd.MM.yyyy")} ";

            Worksheet.Cells["A5"].Value = "Название организации";
            Worksheet.Cells["B5"].Value = "Статус";
            Worksheet.Cells["C5"].Value = "Наименование владельца РАО";
            Worksheet.Cells["D5"].Value = "ЖРО";
            Worksheet.Cells["E5"].Value = "ТРО";
            Worksheet.Cells["F5"].Value = "Всего";

            for (int i=0; i<result.Count; i++)
            {
                Worksheet.Cells[$"A{6 + i}"].Value = result[i].name;
                Worksheet.Cells[$"B{6 + i}"].Value = result[i].status;
                Worksheet.Cells[$"C{6 + i}"].Value = result[i].ownerRao;
                Worksheet.Cells[$"D{6 + i}"].Value = result[i].liquid;
                Worksheet.Cells[$"E{6 + i}"].Value = result[i].solid;
                Worksheet.Cells[$"F{6 + i}"].Value = result[i].liquid + result[i].solid;
            }
            Worksheet.Cells[$"A{6 + result.Count}"].Value = "Всего";
            Worksheet.Cells[$"B{6 + result.Count}"].Value = "";
            Worksheet.Cells[$"C{6 + result.Count}"].Value = "";
            Worksheet.Cells[$"D{6 + result.Count}"].Value = result.Sum(row => row.liquid);
            Worksheet.Cells[$"E{6 + result.Count}"].Value = result.Sum(row => row.solid);
            Worksheet.Cells[$"F{6 + result.Count}"].Value = result.Sum(row => row.liquid + row.solid);

            SetWorksheetStyle(result.Count);

            progressBarVM.SetProgressBar(95, "Сохранение");
            await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);

            await progressBar.CloseAsync();
        }
        private void SetWorksheetStyle(int tableLength)
        {
            try 
            {
                Worksheet.Columns[1].Width = 35;
                Worksheet.Columns[3].Width = 35;

                Worksheet.Rows[3].Height = 30;

                Worksheet.Cells["A1:F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Worksheet.Cells["A1:F1"].Style.Font.Bold = true;


                Worksheet.Cells["A3:F3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Worksheet.Cells["A3:F3"].Style.Font.Bold = true;
                Worksheet.Cells["A3:F3"].Style.WrapText = true;

                Worksheet.Cells["A5:F5"].Style.Font.Bold = true;

                Worksheet.Cells[$"A6:A{6 + tableLength}"].Style.WrapText = true;
                Worksheet.Cells[$"C6:C{6 + tableLength}"].Style.WrapText = true;

                Worksheet.Cells[$"A5:F{6 + tableLength}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                Worksheet.Cells[$"A5:F{6 + tableLength}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                Worksheet.Cells[$"A5:F{6 + tableLength}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                Worksheet.Cells[$"A5:F{6 + tableLength}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;


                Worksheet.Cells[$"A{6 + tableLength}:F{6 + tableLength}"].Style.Font.Bold = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region GetSelectedFilesFromDialog

        /// <summary>
        /// Открыть окно выбора файлов с соответствующим фильтром.
        /// </summary>
        /// <param name="name">Имя фильтра.</param>
        /// <param name="extensions">Массив расширений файлов.</param>
        /// <returns>Список файлов.</returns>
        private protected static async Task<string[]?> GetSelectedFilesFromDialog(string name, params string[] extensions)
        {
            OpenFileDialog dial = new() { AllowMultiple = true };
            var filter = new FileDialogFilter
            {
                Name = name,
                Extensions = [.. extensions]
            };
            dial.Filters = [filter];
            return await dial.ShowAsync(Desktop.MainWindow);
        }

        #endregion
    }

    public class VolumeProccessingInfoDTO()
    {
        public string name;
        public string status;
        public string ownerRao;
        public double liquid;
        public double solid;
        public List<int> reportIdList;
    }

}
