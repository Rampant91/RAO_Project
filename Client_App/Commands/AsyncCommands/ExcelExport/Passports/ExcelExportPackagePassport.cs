using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Passports;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Passports
{
    public class ExcelExportPackagePassport : ExcelBaseAsyncCommand
    {
        public override bool CanExecute(object? parameter) => true;

        /// <summary>
        /// Выгрузка паспорта на упаковку в Excel
        /// </summary>
        /// <param name="parameter">В качестве параметра допускается или сам паспорт на упаковку(PackagePassport), или его Id</param>
        /// <returns></returns>
        public override async Task AsyncExecute(object? parameter)
        {
            var dbm = StaticConfiguration.DBModel;
            PackagePassport passport;

            if (parameter is PackagePassport) 
            {
                passport = (PackagePassport)parameter;
            }
            else if (parameter is int passportId
                && dbm.package_passport.Any(pas => pas.Id == passportId))
            {
                passport = dbm.package_passport
                    .Include(passport => passport.ContentCharacteristics)
                    .FirstOrDefault(pas => pas.Id == passportId);
            }
            else return;

            var cts = new CancellationTokenSource();
            ExportType = "Для_печати";
            var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
            var progressBarVM = progressBar.AnyTaskProgressBarVM;

            progressBarVM.SetProgressBar(5, "Определение имени файла");
            var fileName = await GetFileName(passport, cts);

            progressBarVM.SetProgressBar(10, "Запрос пути сохранения");
            var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

            progressBarVM.SetProgressBar(15, "Создание временной БД", "Выгрузка отчёта для печати", ExportType);
            var tmpDbPath = await CreateTempDataBase(progressBar, cts);

            progressBarVM.SetProgressBar(70, "Инициализация Excel пакета");
            using var excelPackage = await InitializePassportExcelPackage(fullPath);

            progressBarVM.SetProgressBar(80, "Выгрузка данных");
            await FillHeader(excelPackage, passport);

            progressBarVM.SetProgressBar(90, "Сохранение");
            await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);

            progressBarVM.SetProgressBar(95, "Очистка временных данных");
            try
            {
                File.Delete(tmpDbPath);
            }
            catch
            {
                // ignored
            }

            progressBarVM.SetProgressBar(100, "Завершение выгрузки");
            GC.Collect();
            await progressBar.CloseAsync();
        }


        #region GetFileName
        private async Task<string> GetFileName(PackagePassport passport, CancellationTokenSource cts,
            AnyTaskProgressBar? progressBar = null)
        {
            var fileName = $"{ExportType}" +
                $"_{passport.PassportNum}" +
                $"_{passport.PassportDate}" +
                $"_{passport.PackageType}" +
                $"_{passport.CorrectionNumber}" +
                $"_{Assembly.GetExecutingAssembly().GetName().Version}";

            return fileName;
        }

        #endregion

        #region InitializePassportExcelPackage

        /// <summary>
        /// Инициализация Excel пакета.
        /// </summary>
        /// <param name="fullPath">Полный путь до .xlsx файла.</param>
        /// <returns>Пакет Excel.</returns>
        private protected static Task<ExcelPackage> InitializePassportExcelPackage(string fullPath)
        {
#if DEBUG
            var appFolderPath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Excel", $"package_passport.xlsx");
#else
        var appFolderPath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Excel", $"package_passport.xlsx");
#endif
            if (!File.Exists(appFolderPath))
            {
                throw new FileNotFoundException($"Шаблон Excel не найден: {appFolderPath}");
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new(new FileInfo(fullPath), new FileInfo(appFolderPath));
            
            var worksheet = excelPackage.Workbook.Worksheets[0];

            worksheet.Cells.Style.ShrinkToFit = true;
            return Task.FromResult(excelPackage);
        }

        #endregion
        #region FillExcel

        /// <summary>
        /// Заполняет .xlsx строчками данных.
        /// </summary>
        /// <param name="excelPackage">Пакет Excel.</param>
        /// <param name="rep">Отчёт.</param>
        /// <returns>Успешно выполненная Task.</returns>
        private static Task FillHeader(ExcelPackage excelPackage, PackagePassport passport)
        {
            ExcelWorksheet? worksheet = null;
            try
            {
                worksheet = excelPackage.Workbook.Worksheets[0];
            }
            catch (Exception ex) 
            { 
                throw ex; 
            }
            worksheet.Cells["G3"].Value = passport.PassportNum;
            worksheet.Cells["K3"].Value = passport.PassportDate;
            worksheet.Cells["H5"].Value = passport.PackageType;
            worksheet.Cells["H7"].Value = passport.CorrectionNumber;
            worksheet.Cells["C9"].Value = passport.StatusRaoCode;
            worksheet.Cells["F9"].Value = passport.TechSpecification;
            worksheet.Cells["L9"].Value = passport.NameRao;
            worksheet.Cells["N9"].Value = passport.ClassRao;
            worksheet.Cells["G11"].Value = passport.RaoDisposalNum;
            //worksheet.Cells[""].Value = passport.RaoDisposalDate;
            worksheet.Cells["G12"].Value = passport.PackageIdCode;
            worksheet.Cells["L12"].Value = passport.TypeAndIdPuod;
            worksheet.Cells["G14"].Value = passport.Owner;
            //worksheet.Cells[""].Value = passport.OwnerOkpo;
            worksheet.Cells["G15"].Value = passport.Manufacturer;
            //worksheet.Cells[""].Value = passport.ManufacturerOkpo;
            worksheet.Cells["G16"].Value = passport.CertificateConformityNum;
            worksheet.Cells["N16"].Value = passport.ManufactureDate;
            worksheet.Cells["G17"].Value = passport.CertificateConformityStartPeriod;
            //worksheet.Cells[""].Value = passport.CertificateConformityEndPeriod;
            worksheet.Cells["G18"].Value = passport.ServiceLife;
            worksheet.Cells["N18"].Value = passport.TransferDate;

            //ExcelPrintTitleExport(rep.FormNum_DB, worksheetTitle, rep, rep.Reports.Master);


            //ExcelPrintSubMainExport(rep.FormNum_DB, worksheetMain, rep);

            //if (worksheetTitle.Name is "1.0" or "2.0" or "Форма 5.0" && (worksheetMain.Name is not "Форма 5.7"))
            //    ExcelPrintNotesExport(rep.FormNum_DB, worksheetMain, rep);


            //ExcelPrintRowsExport(rep.FormNum_DB, worksheetMain, rep);

            return Task.CompletedTask;
        }

        #endregion
    }
}
