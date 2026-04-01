using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Threading;
using Client_App.Controls.DataGrid;
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

            progressBarVM.SetProgressBar(82, "Выгрузка данных");
            await FillTable1(excelPackage, passport);

            progressBarVM.SetProgressBar(85, "Выгрузка данных");
            await FillFooter(excelPackage, passport);

            progressBarVM.SetProgressBar(88, "Выгрузка данных");
            await FillTable2(excelPackage, passport);

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

        #region FillHeader

        /// <summary>
        /// Заполняет .xlsx строчками данных.
        /// </summary>
        /// <param name="excelPackage">Пакет Excel.</param>
        /// <param name="rep">Отчёт.</param>
        /// <returns>Успешно выполненная Task.</returns>
        private static Task FillHeader(ExcelPackage excelPackage, PackagePassport passport)
        {
            var worksheet = excelPackage.Workbook.Worksheets[0];

            worksheet.Cells["G3"].Value = passport.PassportNum;

            worksheet.Cells["K3"].Value = passport.PassportDate;
            worksheet.Cells["K3"].Style.Numberformat.Format = "dd.mm.yyyy";

            worksheet.Cells["H5"].Value = passport.PackageType;
            worksheet.Cells["H7"].Value = passport.CorrectionNumber;
            worksheet.Cells["C9"].Value = passport.StatusRaoCode;
            worksheet.Cells["F9"].Value = passport.TechSpecification;
            worksheet.Cells["L9"].Value = passport.NameRao;
            worksheet.Cells["N9"].Value = passport.ClassRao;
            worksheet.Cells["G11"].Value = passport.RaoDisposalNum;

            worksheet.Cells["J11"].Value = passport.RaoDisposalDate;
            worksheet.Cells["J11"].Style.Numberformat.Format = "dd.mm.yyyy";

            worksheet.Cells["G12"].Value = passport.PackageIdCode;
            worksheet.Cells["L12"].Value = passport.TypeAndIdPuod;
            worksheet.Cells["G14"].Value = passport.Owner;
            worksheet.Cells["N14"].Value = passport.OwnerOkpo;
            worksheet.Cells["G15"].Value = passport.Manufacturer;
            worksheet.Cells["N15"].Value = passport.ManufacturerOkpo;
            worksheet.Cells["G16"].Value = passport.CertificateConformityNum;

            worksheet.Cells["N16"].Value = passport.ManufactureDate;
            worksheet.Cells["N16"].Style.Numberformat.Format = "dd.mm.yyyy";

            worksheet.Cells["G17"].Value = passport.CertificateConformityStartPeriod;
            worksheet.Cells["G17"].Style.Numberformat.Format = "dd.mm.yyyy";

            worksheet.Cells["I17"].Value = passport.CertificateConformityEndPeriod;
            worksheet.Cells["I17"].Style.Numberformat.Format = "dd.mm.yyyy";

            worksheet.Cells["G18"].Value = passport.ServiceLife;

            worksheet.Cells["N18"].Value = passport.TransferDate;
            worksheet.Cells["N18"].Style.Numberformat.Format = "dd.mm.yyyy";

            //ExcelPrintTitleExport(rep.FormNum_DB, worksheetTitle, rep, rep.Reports.Master);


            //ExcelPrintSubMainExport(rep.FormNum_DB, worksheetMain, rep);

            //if (worksheetTitle.Name is "1.0" or "2.0" or "Форма 5.0" && (worksheetMain.Name is not "Форма 5.7"))
            //    ExcelPrintNotesExport(rep.FormNum_DB, worksheetMain, rep);


            //ExcelPrintRowsExport(rep.FormNum_DB, worksheetMain, rep);

            return Task.CompletedTask;
        }

        #endregion

        #region FillTable1

        /// <summary>
        /// Заполняет .xlsx строчками данных.
        /// </summary>
        /// <param name="excelPackage">Пакет Excel.</param>
        /// <param name="rep">Отчёт.</param>
        /// <returns>Успешно выполненная Task.</returns>
        private static Task FillTable1(ExcelPackage excelPackage, PackagePassport passport)
        {
            var worksheet = excelPackage.Workbook.Worksheets[0];

            worksheet.Cells["A24"].Value = passport.DisposalMethod;

            //worksheet.Cells["B24"].Value = passport.PhysicochemicalForm;
            //worksheet.Cells["B24"].Style.WrapText = true;

            //worksheet.Cells["C24"].Value = passport.MorphologicalComposition;
            //worksheet.Cells["C24"].Style.WrapText = true;

            //worksheet.Cells["D24"].Value = passport.Flammability;
            //worksheet.Cells["D24"].Style.WrapText = true;

            worksheet.Cells["E24"].Value = passport.MatrixMaterialType;
            worksheet.Cells["E24"].Style.WrapText = true;


            worksheet.Cells["F24"].Value = passport.FillingWasteDate;
            worksheet.Cells["F24"].Style.Numberformat.Format = "dd.mm.yyyy";

            worksheet.Cells["G24"].Value = passport.Diameter;
            worksheet.Cells["H24"].Value = passport.Height;
            worksheet.Cells["I24"].Value = passport.Length;
            worksheet.Cells["J24"].Value = passport.Width;
            worksheet.Cells["K24"].Value = passport.PackageMass;
            worksheet.Cells["L24"].Value = passport.RaoMass;
            worksheet.Cells["M24"].Value = passport.PackageVolume;
            worksheet.Cells["N24"].Value = passport.RaoVolume;
            worksheet.Cells["O24"].Value = passport.RadiationDoseRate10cm;
            worksheet.Cells["P24"].Value = passport.RadiationDoseRate1m;
            worksheet.Cells["Q24"].Value = passport.LevelNonFixedPollutionAlpha;
            worksheet.Cells["R24"].Value = passport.LevelNonFixedPollutionBetaGamma;
            worksheet.Cells["W24"].Value = passport.HeatOutput;

            //ExcelPrintTitleExport(rep.FormNum_DB, worksheetTitle, rep, rep.Reports.Master);


            //ExcelPrintSubMainExport(rep.FormNum_DB, worksheetMain, rep);

            //if (worksheetTitle.Name is "1.0" or "2.0" or "Форма 5.0" && (worksheetMain.Name is not "Форма 5.7"))
            //    ExcelPrintNotesExport(rep.FormNum_DB, worksheetMain, rep);


            //ExcelPrintRowsExport(rep.FormNum_DB, worksheetMain, rep);

            return Task.CompletedTask;
        }

        #endregion

        #region FillFooter

        /// <summary>
        /// Заполняет .xlsx строчками данных.
        /// </summary>
        /// <param name="excelPackage">Пакет Excel.</param>
        /// <param name="rep">Отчёт.</param>
        /// <returns>Успешно выполненная Task.</returns>
        private static Task FillFooter(ExcelPackage excelPackage, PackagePassport passport)
        {
            var worksheet = excelPackage.Workbook.Worksheets[0];

            worksheet.Cells["A33"].Value = passport.Notes;
            worksheet.Cells["A33"].Style.WrapText = true;


            worksheet.Cells["F35"].Value = passport.ResponsibleTransfer;
            worksheet.Cells["J35"].Value = passport.GradeAuthorizedPersonTransfer;
            worksheet.Cells["M35"].Value = passport.FioAuthorizedPersonTransfer;

            worksheet.Cells["F36"].Value = passport.ResponsibleReception;
            worksheet.Cells["J36"].Value = passport.GradeAuthorizedPersonReception;
            worksheet.Cells["M36"].Value = passport.FioAuthorizedPersonReception;



            return Task.CompletedTask;
        }

        #endregion

        #region FillTable1

        /// <summary>
        /// Заполняет .xlsx строчками данных.
        /// </summary>
        /// <param name="excelPackage">Пакет Excel.</param>
        /// <param name="rep">Отчёт.</param>
        /// <returns>Успешно выполненная Task.</returns>
        private static Task FillTable2(ExcelPackage excelPackage, PackagePassport passport)
        {
            var offset = 29;
            var characteristics = passport.ContentCharacteristics;
            var worksheet = excelPackage.Workbook.Worksheets[0];

            for ( int i=0; i<characteristics.Count; i++)
            {
                worksheet.InsertRow(offset + 1, 1);
                offset++;
                var start = offset;

                var radionuclids = characteristics[i].RadionuclidsList;
                var radCounts = radionuclids.Count;
                for (int j = 0; j < radCounts; j++)
                {
                    worksheet.Cells[$"G{offset}"].Value = radionuclids[j].Name;
                    worksheet.Cells[$"H{offset}"].Value = radionuclids[j].Activity;

                    if (j != radCounts - 1)
                    {
                        worksheet.InsertRow(offset + 1, 1);
                        offset++;
                    }
                }

                //Если нет радионуклидов, то все равно нужно вписать какое то значения,
                //иначе границы ячеек не отрисуются
                if (radCounts == 0)
                {
                    worksheet.Cells[$"G{offset}"].Value = "";
                    worksheet.Cells[$"H{offset}"].Value = "";
                }


                //worksheet.Cells[$"A{start}:A{offset}"].Merge = true;
                //worksheet.Cells[$"A{start}:A{offset}"].Value = characteristics[i].PackageIdNum;

                worksheet.Cells[$"B{start}:B{offset}"].Merge = true;
                worksheet.Cells[$"B{start}:B{offset}"].Value = characteristics[i].ClassRao;

                worksheet.Cells[$"C{start}:C{offset}"].Merge = true;
                worksheet.Cells[$"C{start}:C{offset}"].Value = characteristics[i].CodeRao;

                worksheet.Cells[$"D{start}:D{offset}"].Merge = true;
                worksheet.Cells[$"D{start}:D{offset}"].Value = characteristics[i].PrimaryPackageQuantity;

                worksheet.Cells[$"E{start}:E{offset}"].Merge = true;
                worksheet.Cells[$"E{start}:E{offset}"].Value = characteristics[i].PrimaryPackageVolume;

                worksheet.Cells[$"F{start}:F{offset}"].Merge = true;
                worksheet.Cells[$"F{start}:F{offset}"].Value = characteristics[i].PrimaryPackageMass;

                worksheet.Cells[$"I{start}:I{offset}"].Merge = true;
                worksheet.Cells[$"I{start}:I{offset}"].Value = characteristics[i].LongLivingActivity;

                worksheet.Cells[$"J{start}:J{offset}"].Merge = true;
                worksheet.Cells[$"J{start}:J{offset}"].Value = characteristics[i].TransuraniumActivity;

                worksheet.Cells[$"K{start}:K{offset}"].Merge = true;
                worksheet.Cells[$"K{start}:K{offset}"].Value = characteristics[i].AlphaActivity;

                worksheet.Cells[$"L{start}:L{offset}"].Merge = true;
                worksheet.Cells[$"L{start}:L{offset}"].Value = characteristics[i].BetaGammaActivity;

                worksheet.Cells[$"M{start}:M{offset}"].Merge = true;
                worksheet.Cells[$"M{start}:M{offset}"].Value = characteristics[i].TritiumActivity;

                worksheet.Cells[$"N{start}:N{offset}"].Merge = true;
                worksheet.Cells[$"N{start}:N{offset}"].Value = characteristics[i].TotalActivity;

                worksheet.Cells[$"O{start}:O{offset}"].Merge = true;
                worksheet.Cells[$"O{start}:O{offset}"].Value = characteristics[i].NuclearHazardousFissileNuclides;

                var cells = worksheet.Cells[$"A{start}:O{offset}"];
                foreach (var cell in cells)
                {
                    var btm = cell.Style.Border.Bottom;
                    var lft = cell.Style.Border.Left;
                    var rgt = cell.Style.Border.Right;
                    var top = cell.Style.Border.Top;
                    btm.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    btm.Color.SetColor(255, 0, 0, 0);
                    lft.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    lft.Color.SetColor(255, 0, 0, 0);
                    rgt.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rgt.Color.SetColor(255, 0, 0, 0);
                    top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    top.Color.SetColor(255, 0, 0, 0);
                }
            }


            return Task.CompletedTask;
        }

        #endregion


    }
}
