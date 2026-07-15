using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Views.ProgressBar;
using FirebirdSql.Data.FirebirdClient;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.RaodbExport.tempExportCommands
{
    public class ExportAllFormsFromListAsyncCommand : ExportRaodbBaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            var cts = new CancellationTokenSource();
            string answer;

            #region ProgressBarInitialization

            await Dispatcher.UIThread.InvokeAsync(() => ProgressBar = new AnyTaskProgressBar(cts));
            var progressBar = ProgressBar;
            var progressBarVM = progressBar.AnyTaskProgressBarVM;

            progressBarVM.ExportType = "Экспорт_RAODB";
            progressBarVM.ExportName = "Выгрузка организаций в отдельный файл";
            progressBarVM.ValueBar = 5;
            var loadStatus = "Создание временной БД";
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            #endregion

            var dbReadOnlyPath = await CreateTempDataBase(progressBar, cts);

            #region Progress = 10

            loadStatus = "Создание новой БД";
            progressBarVM.ValueBar = 10;
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            #endregion
            await using var dbReadOnly = new DBModel(dbReadOnlyPath);

            var newDbFolder = await new OpenFolderDialog().ShowAsync(Desktop.MainWindow);
            if (string.IsNullOrEmpty(newDbFolder)) return;
            var newDbPath = Path.Combine(newDbFolder, "Local_0.RAODB");

            await using var db = new DBModel(newDbPath);
            await db.Database.MigrateAsync(cancellationToken: cts.Token);


            #region Progress = 20

            loadStatus = "Загрузка данных организаций";
            progressBarVM.ValueBar = 20;
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            #endregion

            var reportInfoList = await SelectFileWithReportInfoList();
            if (reportInfoList is null) return;


            var orgsGroup = reportInfoList.GroupBy(repInfo => new { repInfo.regNo, repInfo.okpo });
            var orgsCount = orgsGroup.Count();
            var iteration = 0;
            double progressBarInc = (double)(80 - 20) / orgsCount;
            double barValue = 20;
            foreach (var org in orgsGroup)
            {

                #region Progress = [20:90]
                loadStatus = $"Загрузка данных организаций ({iteration}/{orgsCount})";
                progressBarVM.ValueBar = (int)barValue;
                progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";
                barValue += progressBarInc;
                iteration++;
                #endregion

                var regNo = org.Key.regNo;
                var okpo = org.Key.okpo;

                var masterList = StaticConfiguration.DBModel.form_10
                    .AsNoTracking()
                    .Include(f10 => f10.Report)
                    .ThenInclude(rep => rep.Rows10)
                    .Where(form10 =>
                    form10.RegNo_DB == regNo
                    || form10.Okpo_DB == okpo)
                    .Select(f10 => f10.Report)
                    .ToList()
                    ;

                var clonedReports = new Reports();
                if (masterList.Count() > 1)
                {
                    var master = masterList.FirstOrDefault(rep => rep.RegNoRep.Value == regNo && rep.OkpoRep.Value == okpo);
                    clonedReports.Master_DB = master;
                }
                else if (masterList.Count() == 1)
                    clonedReports.Master_DB = masterList[0];
                else
                    continue;

                var formNumGroups = org.GroupBy(o => o.formNum);
                foreach (var formNumGroup in formNumGroups)
                {
                    switch (formNumGroup.Key)
                    {
                        case "1. 1":
                            {
                                try
                                {
                                    var reportList11 = StaticConfiguration.DBModel.ReportCollectionDbSet
                                   .AsNoTracking()
                                   .Include(rep => rep.Rows11)
                                   .Include(rep => rep.Reports)
                                   .Where(rep => rep.FormNum_DB == "1.1")
                                   .Where(rep => rep.Reports.Master_DBId == clonedReports.Master_DB.Id)
                                   .ToList();
                                
                                    clonedReports.Report_Collection.AddRange(reportList11
                                   .Where(rep =>
                                   formNumGroup.Any(repInfo =>
                                        repInfo.startPeriod == rep.StartPeriod_DB
                                        && repInfo.endPeriod == rep.EndPeriod_DB
                                        && repInfo.corNum == rep.CorrectionNumber_DB)));
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                break;
                            }
                        case "1. 5":
                            try
                            {
                                var reportList15 = StaticConfiguration.DBModel.ReportCollectionDbSet
                                   .AsNoTracking()
                                   .Include(rep => rep.Rows15)
                                   .Include(rep => rep.Reports)
                                   .Where(rep => rep.FormNum_DB == "1.5")
                                   .Where(rep => rep.Reports.Master_DBId == clonedReports.Master_DB.Id)
                                   .ToList();
                           
                                clonedReports.Report_Collection.AddRange(reportList15
                                   .Where(rep =>
                                   formNumGroup.Any(repInfo =>
                                        repInfo.startPeriod == rep.StartPeriod_DB
                                        && repInfo.endPeriod == rep.EndPeriod_DB
                                        && repInfo.corNum == rep.CorrectionNumber_DB)));
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            break;
                        case "1. 6":
                            try
                            {
                                var reportList16 = StaticConfiguration.DBModel.ReportCollectionDbSet
                                   .AsNoTracking()
                                   .Include(rep => rep.Rows16)
                                   .Include(rep => rep.Reports)
                                   .Where(rep => rep.FormNum_DB == "1.6")
                                   .Where(rep => rep.Reports.Master_DBId == clonedReports.Master_DB.Id)
                                   .ToList();
                            
                                clonedReports.Report_Collection.AddRange(reportList16
                                   .Where(rep =>
                                   formNumGroup.Any(repInfo =>
                                        repInfo.startPeriod == rep.StartPeriod_DB
                                        && repInfo.endPeriod == rep.EndPeriod_DB
                                        && repInfo.corNum == rep.CorrectionNumber_DB)));
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            break;
                        case "новые":
                            try
                            {
                                var reportList = StaticConfiguration.DBModel.ReportCollectionDbSet
                                   .AsNoTracking()
                                   .Include(rep => rep.Rows11)
                                   .Include(rep => rep.Rows12)
                                   .Include(rep => rep.Rows13)
                                   .Include(rep => rep.Rows14)
                                   .Include(rep => rep.Rows15)
                                   .Include(rep => rep.Rows16)
                                   .Include(rep => rep.Rows17)
                                   .Include(rep => rep.Rows18)
                                   .Include(rep => rep.Rows19)
                                   .Include(rep => rep.Reports)
                                   .Where(rep => rep.Reports.Master_DBId == clonedReports.Master_DB.Id)
                                   .ToList();
                            
                                clonedReports.Report_Collection.AddRange(reportList);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            break;
                    }
                }

                await db.ReportsCollectionDbSet.AddAsync(clonedReports, cts.Token);
            }

            #region Progress = 90

            loadStatus = "Сохраненение данных";
            progressBarVM.ValueBar = 90;
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            #endregion
;
            if (!db.DBObservableDbSet.Any())
            {
                db.DBObservableDbSet.Add(new DBObservable());
                db.DBObservableDbSet.Local.First().Reports_Collection.AddRange(db.ReportsCollectionDbSet.Local);
            }

            await db.SaveChangesAsync(cts.Token);

            var t = db.Database.GetDbConnection() as FbConnection;
            await t.CloseAsync();
            await t.DisposeAsync();
            await db.Database.CloseConnectionAsync();


            #region Progress = 100

            loadStatus = "Завершение выгрузки";
            progressBarVM.ValueBar = 100;
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            #endregion

            if (!cts.IsCancellationRequested)
            {
                #region ExportDoneMessage

                answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Ок", IsDefault = true },
                        new ButtonDefinition { Name = "Открыть расположение файла" }
                        ],
                        ContentTitle = "Выгрузка",
                        ContentHeader = "Уведомление",
                        ContentMessage = "Выгрузка всех организаций в отдельный" +
                                         $"{Environment.NewLine}файл .raodb завершена.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                if (answer is "Открыть расположение файлов")
                {
                    Process.Start("explorer", newDbFolder);
                }
            }
            await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
        }


        private async Task<List<ReportInfoDTO>?> SelectFileWithReportInfoList()
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
                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Ошибка",
                    ContentHeader = $"Произошла ошибка при импорте файла {SourceFile.Name}",
                    ContentMessage = $"Описание:\n" +
                                     $"{ex.Message}",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                })
                .ShowDialog(Desktop.MainWindow));
                #endregion
                return null;
            }
            ExcelPackage excelPackage = new(SourceFile);

            var worksheet = excelPackage.Workbook.Worksheets[0];
            var patternIsValid = worksheet.Cells["A1"].Text.ToLower() is "регно"
                && worksheet.Cells["B1"].Text.ToLower() is "окпо"
                && worksheet.Cells["C1"].Text.ToLower() is "форма"
                && worksheet.Cells["D1"].Text.ToLower() is "нач"
                && worksheet.Cells["E1"].Text.ToLower() is "окн"
                && worksheet.Cells["F1"].Text.ToLower() is "кор";

            if (!patternIsValid)
            {
                #region InvalidDataFormatMessage

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
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
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion
                return null;
            }
            var reportInfoDTOList = new List<ReportInfoDTO>();
            var row = 1;

            while (worksheet.Cells[$"A{row + 1}"].Text is not ""
                || worksheet.Cells[$"B{row + 1}"].Text is not "")
            {
                try
                {
                    row++;

                    reportInfoDTOList.Add(new ReportInfoDTO
                    {
                        regNo = worksheet.Cells[$"A{row}"].Text,
                        okpo = worksheet.Cells[$"B{row}"].Text,
                        formNum = worksheet.Cells[$"C{row}"].Text,
                        startPeriod = worksheet.Cells[$"D{row}"].Text,
                        endPeriod = worksheet.Cells[$"E{row}"].Text,
                        corNum = int.TryParse(worksheet.Cells[$"F{row}"].Text, out var intValue) ? intValue : 0,
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return reportInfoDTOList;
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

    public class ReportInfoDTO()
    {
        public string regNo;
        public string okpo;
        public string formNum;
        public string startPeriod;
        public string endPeriod;
        public int corNum;
    }
}
