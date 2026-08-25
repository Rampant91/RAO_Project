using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Controls.DataGrid;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.DBRealization;
using Models.Passports;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Passports
{
    public class ExcelExportPackagePassportPrikaz : ExcelBaseAsyncCommand
    {
        int offset;
        public override bool CanExecute(object? parameter) => true;

        /// <summary>
        /// Выгрузка паспорта на упаковку в Excel
        /// </summary>
        /// <param name="parameter">В качестве параметра допускается или сам паспорт на упаковку(PackagePassport), или его Id</param>
        /// <returns></returns>
        public override async Task AsyncExecute(object? parameter)
        {
            offset = 0;
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
                    .ThenInclude(characteristic => characteristic.RadionuclidsList)
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
            try
            {
                progressBarVM.SetProgressBar(70, "Инициализация Excel пакета");
                using var excelPackage = await InitializePassportExcelPackage(fullPath);

                progressBarVM.SetProgressBar(80, "Выгрузка данных");
                await FillHeader(excelPackage, passport);

                progressBarVM.SetProgressBar(82, "Выгрузка данных");
                await FillFooter(excelPackage, passport);

                progressBarVM.SetProgressBar(85, "Выгрузка данных");
                await FillTable1(excelPackage, passport);

                progressBarVM.SetProgressBar(88, "Выгрузка данных");
                await FillTable2(excelPackage, passport);

                SetRowsHeightInWorksheet(excelPackage.Workbook.Worksheets[0]);

                progressBarVM.SetProgressBar(90, "Сохранение");
                await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);

                progressBarVM.SetProgressBar(100, "Завершение выгрузки");
                GC.Collect();
                await progressBar.CloseAsync();
            }
            finally
            {
                TryDeleteTempDataBase(tmpDbPath);
            }
        }


        static double CalculateRowHeight(string text, ExcelFont excelFont, double columnWidthPixels)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;

            using (var bmp = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(bmp))
            {
                var font = new Font(excelFont.Name, excelFont.Size,
                GetFontStyle(excelFont.Bold, excelFont.Italic));

                var format = StringFormat.GenericTypographic;
                float totalHeight = 0;
                float lineHeight = g.MeasureString("A", font, 1000, format).Height; // высота одной строки

                // Разбиваем на слова
                string[] words = text.Split(' ');
                List<string> lines = new List<string>();
                string currentLine = "";

                foreach (var word in words)
                {
                    string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                    float width = g.MeasureString(testLine, font, 1000, format).Width;

                    if (width <= columnWidthPixels)
                    {
                        currentLine = testLine;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(currentLine))
                            lines.Add(currentLine);
                        currentLine = word;
                    }
                }
                if (!string.IsNullOrEmpty(currentLine))
                    lines.Add(currentLine);

                totalHeight = lines.Count * lineHeight;
                return totalHeight;
            }
        }
        private static FontStyle GetFontStyle(bool isBold, bool isItalic)
        {
            var style = FontStyle.Regular;
            if (isBold) style |= FontStyle.Bold;
            if (isItalic) style |= FontStyle.Italic;
            return style;
        }

        static void SetRowsHeightInWorksheet(ExcelWorksheet worksheet)
        {
            var rows = worksheet.Rows;
            var columns = worksheet.Columns;
            for (int i = rows.StartRow; i <= rows.EndRow || i<100; i++)
            {
                double maxRowHeight = 20;
                for (int j = columns.StartColumn; j <= columns.EndColumn; j++)
                {
                    var cell = worksheet.Cells[i, j];
                    var mergedRows = 1;
                    var mergedColumns = 1;
                    var mergedAddress = worksheet.MergedCells[i, j];

                    if (mergedAddress != null)
                    {
                        cell = worksheet.Cells[mergedAddress];
                        mergedRows = cell.End.Row - cell.Start.Row + 1;
                        mergedColumns = cell.End.Column - cell.Start.Column + 1;
                    }
                    else
                        ;
                    double width = 0;
                    for( int count = 0; count < mergedColumns; count++)
                    {
                        if (count > 0)
                            j++;
                        width += worksheet.Column(j).Width;
                    }

                    var pixels = width * 7 ;

                    double rowHeight = CalculateRowHeight(cell.Text, cell.Style.Font, pixels);

                    rowHeight = rowHeight / mergedRows;

                    if (maxRowHeight < rowHeight)
                    {
                        maxRowHeight = rowHeight;
                    }
                }
                worksheet.Row(i).CustomHeight = true;
                worksheet.Row(i).Height = maxRowHeight;
            }
        }

        #region FillHeader
        /// <summary>
        /// Заполняет .xlsx строчками данных.
        /// </summary>
        /// <param name="excelPackage">Пакет Excel.</param>
        /// <param name="rep">Отчёт.</param>
        /// <returns>Успешно выполненная Task.</returns>
        private Task FillHeader(ExcelPackage excelPackage, PackagePassport passport)
        {
            var worksheet = excelPackage.Workbook.Worksheets[0];

            worksheet.Cells["H3"].Value = passport.PassportNum;
            worksheet.Cells["H3"].Style.WrapText = true;
            worksheet.Cells["H3:J3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["L3"].Value = passport.PassportDate;
            worksheet.Cells["L3"].Style.Numberformat.Format = "dd.mm.yyyy";
            worksheet.Cells["L3"].Style.WrapText = true;
            worksheet.Cells["L3:M3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["I5"].Value = passport.PackageType;
            worksheet.Cells["I5"].Style.WrapText = true;
            worksheet.Cells["I5:M5"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["I7"].Value = passport.CorrectionNumber;
            worksheet.Cells["I7"].Style.WrapText = true;
            worksheet.Cells["I7:M7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["C9"].Value = passport.StatusRaoCode;
            worksheet.Cells["C9"].Style.WrapText = true;
            worksheet.Cells["C9:D9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["G9"].Value = passport.TechSpecification;
            worksheet.Cells["G9"].Style.WrapText = true;
            worksheet.Cells["G9:I9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["M9"].Value = passport.NameRao;
            worksheet.Cells["M9"].Style.WrapText = true;
            worksheet.Cells["O9"].Value = passport.ClassRao;
            worksheet.Cells["O9"].Style.WrapText = true;
            worksheet.Cells["M9:P9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["H11"].Value = passport.RaoDisposalNum;
            worksheet.Cells["H11"].Style.WrapText = true;
            worksheet.Cells["J11"].Value = passport.RaoDisposalDate;
            worksheet.Cells["J11"].Style.WrapText = true;
            worksheet.Cells["J11"].Style.Numberformat.Format = "dd.mm.yyyy";
            worksheet.Cells["H11:J11"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["H12"].Value = passport.PackageIdCode;
            worksheet.Cells["H12"].Style.WrapText = true;
            worksheet.Cells["H12:J12"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["M12"].Value = passport.TypeAndIdPuod;
            worksheet.Cells["M12"].Style.WrapText = true;
            worksheet.Cells["M12:P12"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["H14"].Value = passport.Owner;
            worksheet.Cells["H14"].Style.WrapText = true;
            worksheet.Cells["N14"].Value = passport.OwnerOkpo;
            worksheet.Cells["N14"].Style.WrapText = true;
            worksheet.Cells["H14:P14"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["H15"].Value = passport.Manufacturer;
            worksheet.Cells["H15"].Style.WrapText = true;
            worksheet.Cells["N15"].Value = passport.ManufacturerOkpo;
            worksheet.Cells["N15"].Style.WrapText = true;
            worksheet.Cells["H15:P15"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["H16"].Value = passport.CertificateConformityNum;
            worksheet.Cells["H16"].Style.WrapText = true;
            worksheet.Cells["H16:J16"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["O16"].Value = passport.ManufactureDate;
            worksheet.Cells["O16"].Style.Numberformat.Format = "dd.mm.yyyy";
            worksheet.Cells["O16"].Style.WrapText = true;
            worksheet.Cells["O16:P16"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["H17"].Value = passport.CertificateConformityStartPeriod; 
            worksheet.Cells["H17"].Style.Numberformat.Format = "dd.mm.yyyy";
            worksheet.Cells["H17"].Style.WrapText = true;
            worksheet.Cells["J17"].Value = passport.CertificateConformityEndPeriod;
            worksheet.Cells["J17"].Style.Numberformat.Format = "dd.mm.yyyy";
            worksheet.Cells["J17"].Style.WrapText = true;
            worksheet.Cells["H17:J17"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["H18"].Value = passport.ServiceLife;
            worksheet.Cells["H18"].Style.WrapText = true;
            worksheet.Cells["H18:J18"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["O18"].Value = passport.TransferDate;
            worksheet.Cells["O18"].Style.Numberformat.Format = "dd.mm.yyyy";
            worksheet.Cells["O18"].Style.WrapText = true;
            worksheet.Cells["O18:P18"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


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
        private Task FillTable1(ExcelPackage excelPackage, PackagePassport passport)
        {
            var worksheet = excelPackage.Workbook.Worksheets[0];

            var rowHeight = 2;

            

            //Заполняем подтаблицу количество и характеристик первичных упаковок

            // Начинаем с 1 индекса, так как 0 индекс во второй таблице выделен под общую упаковку
            // И только после идет описание первичных упаковок
            for (int i= 1; i< passport.ContentCharacteristics.Count; i++)
            {
                var primaryPackage = passport.ContentCharacteristics[i];

                if( i > 2)
                {
                    worksheet.InsertRow(25 + i - 1, 1);
                    rowHeight++;
                    offset++;
                }

                worksheet.Cells[$"B{25 + i - 1}"].Value = primaryPackage.PackageType;
                worksheet.Cells[$"B{25 + i - 1}"].Style.WrapText = true;

                worksheet.Cells[$"C{25 + i - 1}"].Value = primaryPackage.PackageNum;
                worksheet.Cells[$"C{25 + i - 1}"].Style.WrapText = true;

                worksheet.Cells[$"D{25 + i - 1}"].Value = primaryPackage.PrimaryPackageQuantity;
                worksheet.Cells[$"D{25 + i - 1}"].Style.WrapText = true;

                worksheet.Cells[$"E{25 + i - 1}"].Value = primaryPackage.PrimaryPackageVolume;
                worksheet.Cells[$"E{25 + i - 1}"].Style.WrapText = true;

                worksheet.Cells[$"F{25 + i - 1}"].Value = primaryPackage.PrimaryPackageMass;
                worksheet.Cells[$"F{25 + i - 1}"].Style.WrapText = true;

            }
            worksheet.Cells[$"D{25 + rowHeight}"].Value = passport.ContentCharacteristics.Sum(c => c.PrimaryPackageQuantity);
            worksheet.Cells[$"D{25 + rowHeight}"].Style.WrapText = true;

            worksheet.Cells[$"E{25 + rowHeight}"].Value = passport.ContentCharacteristics.Sum(c => c.PrimaryPackageVolume);
            worksheet.Cells[$"E{25 + rowHeight}"].Style.WrapText = true;

            worksheet.Cells[$"F{25 + rowHeight}"].Value = passport.ContentCharacteristics.Sum(c => c.PrimaryPackageMass);
            worksheet.Cells[$"F{25 + rowHeight}"].Style.WrapText = true;

            worksheet.Cells["A25"].Value = passport.DisposalMethod;
            worksheet.Cells[$"A25"].Style.WrapText = true;

            worksheet.Cells["G25"].Value = passport.MatrixMaterialType;
            worksheet.Cells["G25"].Style.WrapText = true;

            worksheet.Cells["H25"].Value = passport.FillingWasteDate;
            worksheet.Cells["H25"].Style.Numberformat.Format = "dd.mm.yyyy";
            worksheet.Cells[$"H25"].Style.WrapText = true;

            worksheet.Cells["I25"].Value = passport.Diameter;
            worksheet.Cells[$"I25"].Style.WrapText = true;

            worksheet.Cells["J25"].Value = passport.Height;
            worksheet.Cells[$"J25"].Style.WrapText = true;

            worksheet.Cells["K25"].Value = passport.Length;
            worksheet.Cells[$"K25"].Style.WrapText = true;

            worksheet.Cells["L25"].Value = passport.Width;
            worksheet.Cells[$"L25"].Style.WrapText = true;

            worksheet.Cells["M25"].Value = passport.PackageMass;
            worksheet.Cells[$"M25"].Style.WrapText = true;

            worksheet.Cells["N25"].Value = passport.RaoMass;
            worksheet.Cells[$"N25"].Style.WrapText = true;

            worksheet.Cells["M26"].Value = passport.PackageVolume;
            worksheet.Cells[$"M26"].Style.WrapText = true;

            worksheet.Cells["N26"].Value = passport.RaoVolume;
            worksheet.Cells[$"N26"].Style.WrapText = true;


            worksheet.Cells["O25"].Value = passport.RadiationDoseRate10cm;
            worksheet.Cells[$"O25"].Style.WrapText = true;

            worksheet.Cells["P25"].Value = passport.RadiationDoseRate1m;
            worksheet.Cells[$"P25"].Style.WrapText = true;

            worksheet.Cells["Q25"].Value = passport.LevelNonFixedPollutionAlpha;
            worksheet.Cells[$"Q25"].Style.WrapText = true;

            worksheet.Cells["R25"].Value = passport.LevelNonFixedPollutionBetaGamma;
            worksheet.Cells[$"R25"].Style.WrapText = true;

            worksheet.Cells["S25"].Value = passport.HeatOutput;
            worksheet.Cells[$"S25"].Style.WrapText = true;




            //Задаем стиль для ячеек с данными в таблице
            SetCellBorderStyle(worksheet.Cells[$"A25:A{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"G25:G{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"H25:H{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"I25:I{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"J25:J{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"K25:K{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"L25:L{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"M26:M{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"N26:N{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"O25:O{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"P25:P{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"Q25:Q{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"R25:R{25 + rowHeight - 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"S25:S{25 + rowHeight - 1}"]);

            var cells = worksheet.Cells[$"A20:S{25 + rowHeight}"];
            foreach (var cell in cells)
            {
                cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                cell.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }


            return Task.CompletedTask;
        }

        #endregion

        private void SetCellBorderStyle(ExcelRange cell)
        {
            cell.Merge = true;
            cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            cell.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            cell.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            cell.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }

        #region FillTable2

        /// <summary>
        /// Заполняет .xlsx строчками данных.
        /// </summary>
        /// <param name="excelPackage">Пакет Excel.</param>
        /// <param name="rep">Отчёт.</param>
        /// <returns>Успешно выполненная Task.</returns>
        private Task FillTable2(ExcelPackage excelPackage, PackagePassport passport)
        {
            var index = 30 + offset;
            var characteristics = passport.ContentCharacteristics;
            var worksheet = excelPackage.Workbook.Worksheets[0];

            SetCellBorderStyle(worksheet.Cells[$"A{index-2}:S{index - 2}"]);

            SetCellBorderStyle(worksheet.Cells[$"A{index-1}:C{index-1}"]);
            SetCellBorderStyle(worksheet.Cells[$"D{index-1}:E{index-1}"]);
            SetCellBorderStyle(worksheet.Cells[$"F{index-1}:G{index-1}"]);
            SetCellBorderStyle(worksheet.Cells[$"H{index-1}:J{index-1}"]);
            SetCellBorderStyle(worksheet.Cells[$"K{index-1}:L{index-1}"]);
            SetCellBorderStyle(worksheet.Cells[$"M{index-1}"]);
            SetCellBorderStyle(worksheet.Cells[$"N{index-1}"]);
            SetCellBorderStyle(worksheet.Cells[$"O{index-1}:Q{index-1}"]);
            SetCellBorderStyle(worksheet.Cells[$"R{index-1}"]);
            SetCellBorderStyle(worksheet.Cells[$"S{index-1}"]);

            SetCellBorderStyle(worksheet.Cells[$"A{index}:C{index}"]);
            SetCellBorderStyle(worksheet.Cells[$"D{index}:E{index}"]);
            SetCellBorderStyle(worksheet.Cells[$"F{index}:G{index}"]);
            SetCellBorderStyle(worksheet.Cells[$"H{index}:J{index}"]);
            SetCellBorderStyle(worksheet.Cells[$"K{index}:L{index}"]);
            SetCellBorderStyle(worksheet.Cells[$"M{index}"]);
            SetCellBorderStyle(worksheet.Cells[$"N{index}"]);
            SetCellBorderStyle(worksheet.Cells[$"O{index}:Q{index}"]);
            SetCellBorderStyle(worksheet.Cells[$"R{index}"]);
            SetCellBorderStyle(worksheet.Cells[$"S{index}"]);

            SetCellBorderStyle(worksheet.Cells[$"M{index+1}:P{index + 1}"]);
            SetCellBorderStyle(worksheet.Cells[$"Q{index + 1}:S{index + 1}"]);

            for (int i = 0; i < characteristics.Count; i++)
            {
                
                var start = index + 1;
                worksheet.InsertRow(start, 5);
                index +=5;
                worksheet.Cells[$"O{start}:P{start}"].Merge = true;
                worksheet.Cells[$"O{start}:P{start}"].Value = "долгоживущие";

                worksheet.Cells[$"O{start + 1}:P{start + 1}"].Merge = true;
                worksheet.Cells[$"O{start + 1}:P{start + 1}"].Value = "трансурановые";

                worksheet.Cells[$"O{start + 2}:P{start + 2}"].Merge = true;
                worksheet.Cells[$"O{start + 2}:P{start + 2}"].Value = "альфа-изл. (за искл. т/уран.)";

                worksheet.Cells[$"O{start + 3}:P{start + 3}"].Merge = true;
                worksheet.Cells[$"O{start + 3}:P{start + 3}"].Value = "бета/гамма- изл.";

                worksheet.Cells[$"O{start + 4}:P{start + 4}"].Merge = true;
                worksheet.Cells[$"O{start + 4}:P{start + 4}"].Value = "тритий";


                if (i == 0)
                {
                    worksheet.Cells[$"A{start + 1}:C{start + 1}"].Merge = true;
                    worksheet.Cells[$"A{start + 1}:C{start + 1}"].Value = $"{passport.PackageIdCode}";
                    worksheet.Cells[$"A{start + 1}:C{start + 1}"].Style.WrapText = true;

                    worksheet.Cells[$"A{start + 2}:C{start + 2}"].Merge = true;
                    worksheet.Cells[$"A{start + 2}:C{start + 2}"].Value = $"{passport.PackageType}";
                    worksheet.Cells[$"A{start + 2}:C{start + 2}"].Style.WrapText = true;

                    worksheet.Cells[$"A{start + 3}"].Value = "№";
                    worksheet.Cells[$"B{start + 3}:C{start + 3}"].Merge = true;
                    worksheet.Cells[$"A{start + 3}:C{start + 3}"].Style.WrapText = true;

                    if (passport.ContainerFactoryNum != null)
                        worksheet.Cells[$"B{start + 3}:C{start + 3}"].Value = $"{passport.ContainerFactoryNum}";
                }
                else if (i > 0)
                {
                    worksheet.Cells[$"A{start + 1}:C{start + 1}"].Merge = true;
                    worksheet.Cells[$"A{start + 1}:C{start + 1}"].Value = $"{characteristics[i].PackageType}";
                    worksheet.Cells[$"A{start + 1}:C{start + 1}"].Style.WrapText = true;

                    worksheet.Cells[$"A{start + 2}"].Value = "№";
                    worksheet.Cells[$"B{start + 2}:C{start + 2}"].Merge = true;
                    worksheet.Cells[$"A{start + 2}"].Style.WrapText = true;

                    worksheet.Cells[$"B{start + 2}:C{start + 2}"].Value = $"{characteristics[i].PackageNum}";
                    worksheet.Cells[$"B{start + 2}:C{start + 2}"].Style.WrapText = true;
                }

                var radionuclids = characteristics[i].RadionuclidsList;
                var radCounts = radionuclids.Count;
                for (int j = 0; j < radCounts; j++)
                {

                    if (j >= 5)
                    {
                        worksheet.InsertRow(index + 1, 1);
                        index++;
                    }

                    worksheet.Cells[$"M{start + j}"].Value = radionuclids[j].Name;
                    worksheet.Cells[$"M{start + j}"].Style.WrapText = true;

                    worksheet.Cells[$"N{start + j}"].Value = radionuclids[j].Activity;
                    worksheet.Cells[$"N{start + j}"].Style.WrapText = true;
                }

                worksheet.Cells[$"D{start}"].Value = characteristics[i].ClassRao;
                worksheet.Cells[$"D{start}"].Style.WrapText = true;
                worksheet.Cells[$"E{start}"].Value = "класс";

                worksheet.Cells[$"D{start + 1}:E{index}"].Merge = true;
                worksheet.Cells[$"D{start + 1}:E{index}"].Value = characteristics[i].CodeRao;
                worksheet.Cells[$"D{start + 1}:E{index}"].Style.WrapText = true;

                worksheet.Cells[$"F{start}:G{index}"].Merge = true;
                worksheet.Cells[$"F{start}:G{index}"].Value = characteristics[i].PhysicochemicalForm;
                worksheet.Cells[$"F{start}:G{index}"].Style.WrapText = true;

                worksheet.Cells[$"H{start}:J{index}"].Merge = true;
                worksheet.Cells[$"H{start}:J{index}"].Value = characteristics[i].MorphologicalComposition;
                worksheet.Cells[$"H{start}:J{index}"].Style.WrapText = true;

                worksheet.Cells[$"K{start}:L{index}"].Merge = true;
                worksheet.Cells[$"K{start}:L{index}"].Value = characteristics[i].Flammability;
                worksheet.Cells[$"K{start}:L{index}"].Style.WrapText = true;

                worksheet.Cells[$"Q{start}"].Value = characteristics[i].LongLivingActivity;
                worksheet.Cells[$"Q{start}"].Style.WrapText = true;

                worksheet.Cells[$"Q{start + 1}"].Value = characteristics[i].TransuraniumActivity;
                worksheet.Cells[$"Q{start + 1}"].Style.WrapText = true;

                worksheet.Cells[$"Q{start + 2}"].Value = characteristics[i].AlphaActivity;
                worksheet.Cells[$"Q{start + 2}"].Style.WrapText = true;

                worksheet.Cells[$"Q{start + 3}"].Value = characteristics[i].BetaGammaActivity;
                worksheet.Cells[$"Q{start + 3}"].Style.WrapText = true;

                worksheet.Cells[$"Q{start + 4}"].Value = characteristics[i].TritiumActivity;
                worksheet.Cells[$"Q{start + 4}"].Style.WrapText = true;


                worksheet.Cells[$"R{start}:R{index}"].Merge = true;
                worksheet.Cells[$"R{start}:R{index}"].Value = characteristics[i].TotalActivity;
                worksheet.Cells[$"R{start}:R{index}"].Style.WrapText = true;


                worksheet.Cells[$"S{start}:S{index}"].Merge = true;
                worksheet.Cells[$"S{start}:S{index}"].Value = characteristics[i].NuclearHazardousFissileNuclides;
                worksheet.Cells[$"S{start}:S{index}"].Style.WrapText = true;

                var cells = worksheet.Cells[$"D{start}:S{index}"];
                foreach (var cell in cells)
                {
                    var btm = cell.Style.Border.Bottom;
                    var lft = cell.Style.Border.Left;
                    var rgt = cell.Style.Border.Right;
                    var tp = cell.Style.Border.Top;
                    btm.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    btm.Color.SetColor(255, 0, 0, 0);
                    lft.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    lft.Color.SetColor(255, 0, 0, 0);
                    rgt.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    rgt.Color.SetColor(255, 0, 0, 0);
                    tp.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    tp.Color.SetColor(255, 0, 0, 0);
                }

                worksheet.Cells[$"A{start}:C{start}"].Merge = true;
                if( i ==0)
                    worksheet.Cells[$"A{start + 4}:C{index}"].Merge = true;
                else
                    worksheet.Cells[$"A{start+3}:C{index}"].Merge = true;

                {
                    cells = worksheet.Cells[$"A{start}:C{index}"];
                    var bottom = cells.Style.Border.Bottom;
                    var left = cells.Style.Border.Left;
                    var right = cells.Style.Border.Right;
                    var top = cells.Style.Border.Top;
                    bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                {
                    cells = worksheet.Cells[$"M{start}:M{index}"];
                    var bottom = cells.Style.Border.Bottom;
                    var left = cells.Style.Border.Left;
                    var right = cells.Style.Border.Right;
                    var top = cells.Style.Border.Top;
                    bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }
                {
                    cells = worksheet.Cells[$"N{start}:N{index}"];
                    var bottom = cells.Style.Border.Bottom;
                    var left = cells.Style.Border.Left;
                    var right = cells.Style.Border.Right;
                    var top = cells.Style.Border.Top;
                    bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }
            }


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
        private Task FillFooter(ExcelPackage excelPackage, PackagePassport passport)
        {
            var worksheet = excelPackage.Workbook.Worksheets[0];

            worksheet.Cells["A34"].Value = passport.Notes;
            worksheet.Cells["A34"].Style.WrapText = true;


            worksheet.Cells["F37"].Value = passport.ResponsibleTransfer;
            worksheet.Cells["F37"].Style.WrapText = true;
            worksheet.Cells["F37:I37"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["K37"].Value = passport.GradeAuthorizedPersonTransfer;
            worksheet.Cells["K37"].Style.WrapText = true;

            worksheet.Cells["N37"].Value = passport.FioAuthorizedPersonTransfer;
            worksheet.Cells["N37"].Style.WrapText = true;

            worksheet.Cells["K37:P37"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //подпись
            worksheet.Cells["R37:S37"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


            worksheet.Cells["F40"].Value = passport.ResponsibleReception;
            worksheet.Cells["F40"].Style.WrapText = true;
            worksheet.Cells["F40:I40"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            worksheet.Cells["K40"].Value = passport.GradeAuthorizedPersonReception;
            worksheet.Cells["K40"].Style.WrapText = true;

            worksheet.Cells["N40"].Value = passport.FioAuthorizedPersonReception;
            worksheet.Cells["N40"].Style.WrapText = true;

            worksheet.Cells["K40:P40"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //подпись
            worksheet.Cells["R40:S40"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


            return Task.CompletedTask;
        }

        #endregion

        #region GetFileName
        private async Task<string> GetFileName(PackagePassport passport, CancellationTokenSource cts,
            AnyTaskProgressBar? progressBar = null)
        {
            var fileName = $"PRIKAZ" +
                $"_{ExportType}" +
                $"_{passport.PassportNum?.Replace('/','-')}" +
                $"_{passport.PassportDate}" +
                $"_{passport.PackageType?.Replace('/', '-')}" +
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
        private protected Task<ExcelPackage> InitializePassportExcelPackage(string fullPath)
        {
#if DEBUG
            var appFolderPath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Excel", $"prikaz_package_passport.xlsx");
#else
            var appFolderPath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Excel", $"prikaz_package_passport.xlsx");
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
    }
}
