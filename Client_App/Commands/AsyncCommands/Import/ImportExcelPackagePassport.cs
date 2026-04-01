using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Controls.DataGrid;
using Client_App.ViewModels.Passports;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using Models.Passports;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Import
{
    public class ImportExcelPackagePassport(PassportsMenuWindowVM passportMenuVM) : ImportBaseAsyncCommand
    {
        PassportsMenuWindowVM _passportMenuVM => passportMenuVM;
        private Window owner;
        public override async Task AsyncExecute(object? parameter)
        {
            owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
            .FirstOrDefault(w => w.Name == "PassportMenu");

            var readAnyExcel = false;
            string[] extensions = ["xlsx", "XLSX"];
            var answer = await GetSelectedFilesFromDialog("Excel", extensions);
            if (answer is null) return;

            foreach (var res in answer) // Для каждого импортируемого файла
            {
                var impDateTime = DateTime.Now;

                ExcelImportNewReps = false;
                if (res is "") continue;
                SourceFile = new FileInfo(res);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                try
                {
                    using ExcelPackage excelPackageTry = new(SourceFile);
                }
                catch (Exception ex)
                {
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
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));
                    return;
                }
                ExcelPackage excelPackage = new(SourceFile);

                var worksheet = excelPackage.Workbook.Worksheets[0];

                var val = worksheet.Name == "Паспорт на упаковку"
                      && Convert.ToString(worksheet.Cells["J1"].Value)
                          is "П  А  С  П  О  Р  Т"
                      && Convert.ToString(worksheet.Cells["J2"].Value)
                          is "на упаковку твердых радиоактивных отходов";

                if (!val)
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
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(owner));

                    #endregion

                    continue;
                }

                var impPassport = new PackagePassport();

                //Header
                impPassport.PassportNum = worksheet.Cells["G3"].Text;
                impPassport.PassportDate = GetDateOnlyFromCell(worksheet.Cells["K3"])?? DateOnly.MinValue ;
                impPassport.PackageType = worksheet.Cells["H5"].Text;
                impPassport.CorrectionNumber = byte.TryParse(worksheet.Cells["H7"].Text, out var byteValue)? byteValue : (byte)0;
                impPassport.StatusRaoCode = worksheet.Cells["C9"].Text;
                impPassport.TechSpecification = worksheet.Cells["F9"].Text;
                impPassport.NameRao = worksheet.Cells["L9"].Text;
                impPassport.ClassRao = byte.TryParse(worksheet.Cells["N9"].Text, out byteValue) ? byteValue : (byte)0;
                impPassport.RaoDisposalNum = worksheet.Cells["G11"].Text;
                impPassport.RaoDisposalDate = GetDateOnlyFromCell(worksheet.Cells["J11"]) ?? DateOnly.MinValue;
                impPassport.PackageIdCode = worksheet.Cells["G12"].Text;
                impPassport.TypeAndIdPuod = worksheet.Cells["L12"].Text;
                impPassport.Owner = worksheet.Cells["G14"].Text;
                impPassport.OwnerOkpo = worksheet.Cells["N14"].Text;
                impPassport.Manufacturer = worksheet.Cells["G15"].Text;
                impPassport.ManufacturerOkpo = worksheet.Cells["N15"].Text;
                impPassport.CertificateConformityNum = worksheet.Cells["G16"].Text;
                impPassport.ManufactureDate = GetDateOnlyFromCell(worksheet.Cells["N16"]) ?? DateOnly.MinValue;
                impPassport.CertificateConformityStartPeriod = GetDateOnlyFromCell(worksheet.Cells["G17"]) ?? DateOnly.MinValue;
                impPassport.CertificateConformityEndPeriod = GetDateOnlyFromCell(worksheet.Cells["I17"]) ?? DateOnly.MinValue;
                impPassport.ServiceLife = uint.TryParse(worksheet.Cells["N9"].Text, out var intValue) ? intValue : 0;
                impPassport.TransferDate = GetDateOnlyFromCell(worksheet.Cells["N18"]) ?? DateOnly.MinValue;

                //Table1
                impPassport.DisposalMethod = worksheet.Cells["A24"].Text;
                //impPassport.PhysicochemicalForm = worksheet.Cells["B24"].Text;
                //impPassport.MorphologicalComposition = worksheet.Cells["C24"].Text;
                //impPassport.Flammability = worksheet.Cells["D24"].Text;
                impPassport.MatrixMaterialType = worksheet.Cells["E24"].Text;
                impPassport.FillingWasteDate = GetDateOnlyFromCell(worksheet.Cells["F24"]) ?? DateOnly.MinValue;
                impPassport.Diameter = uint.TryParse(worksheet.Cells["G24"].Text, out intValue) ? intValue : 0;
                impPassport.Height = uint.TryParse(worksheet.Cells["H24"].Text, out intValue) ? intValue : 0;
                impPassport.Length = uint.TryParse(worksheet.Cells["I24"].Text, out intValue) ? intValue : 0;
                impPassport.Width = uint.TryParse(worksheet.Cells["J24"].Text, out intValue) ? intValue : 0;
                impPassport.PackageMass = double.TryParse(worksheet.Cells["K24"].Text, out var doubleValue) ? doubleValue : 0;
                impPassport.RaoMass = double.TryParse(worksheet.Cells["L24"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.PackageVolume = double.TryParse(worksheet.Cells["M24"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.RaoVolume = double.TryParse(worksheet.Cells["N24"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.RadiationDoseRate10cm = double.TryParse(worksheet.Cells["O24"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.RadiationDoseRate1m = double.TryParse(worksheet.Cells["P24"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.LevelNonFixedPollutionAlpha = double.TryParse(worksheet.Cells["Q24"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.LevelNonFixedPollutionBetaGamma = double.TryParse(worksheet.Cells["R24"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.HeatOutput = double.TryParse(worksheet.Cells["W24"].Text, out  doubleValue) ? doubleValue : 0;


                //Table2
                var startTable2 = 30;
                var currentRow = startTable2;

                while (worksheet.Cells[$"A{currentRow}"].Value
                    is not null
                    and not "Примечания и пояснения:")
                {
                    var characteristic = new CharacteristicPrimaryPackage(impPassport);
                    impPassport.ContentCharacteristics.Add(characteristic);

                    //Переносим все данные кроме списка радионуклидов
                    //characteristic.PackageIdNum = worksheet.Cells[$"A{currentRow}"].Text;
                    characteristic.ClassRao = byte.TryParse(worksheet.Cells[$"B{currentRow}"].Text, out byteValue) ? byteValue : (byte)0;
                    characteristic.CodeRao = worksheet.Cells[$"C{currentRow}"].Text;
                    characteristic.PrimaryPackageQuantity = uint.TryParse(worksheet.Cells[$"D{currentRow}"].Text, out intValue) ? intValue : 0;
                    characteristic.PrimaryPackageVolume = double.TryParse(worksheet.Cells[$"E{currentRow}"].Text, out doubleValue) ? doubleValue : 0;
                    characteristic.PrimaryPackageMass = double.TryParse(worksheet.Cells[$"F{currentRow}"].Text, out doubleValue) ? doubleValue : 0;
                    characteristic.LongLivingActivity = double.TryParse(worksheet.Cells[$"I{currentRow}"].Text, out doubleValue) ? doubleValue : 0;
                    characteristic.TransuraniumActivity = double.TryParse(worksheet.Cells[$"J{currentRow}"].Text, out doubleValue) ? doubleValue : 0;
                    characteristic.AlphaActivity = double.TryParse(worksheet.Cells[$"K{currentRow}"].Text, out doubleValue) ? doubleValue : 0;
                    characteristic.BetaGammaActivity = double.TryParse(worksheet.Cells[$"L{currentRow}"].Text, out doubleValue) ? doubleValue : 0;
                    characteristic.TritiumActivity = double.TryParse(worksheet.Cells[$"M{currentRow}"].Text, out doubleValue) ? doubleValue : 0;
                    characteristic.TotalActivity = double.TryParse(worksheet.Cells[$"N{currentRow}"].Text, out doubleValue) ? doubleValue : 0;
                    characteristic.NuclearHazardousFissileNuclides = worksheet.Cells[$"O{currentRow}"].Text;


                    //Считаем кол-во объединенных строк, что будет равно кол-ву радионуклидов
                    var mergedRowsCount = 1;
                    var mergedRange = worksheet.MergedCells[currentRow, 1];
                    if (mergedRange != null)
                    {
                        // Получаем диапазон объединения
                        var range = worksheet.Cells[mergedRange];
                        // Количество строк = разница последней и первой строки + 1
                        mergedRowsCount = range.End.Row - range.Start.Row + 1;
                    }

                    //Переносим список радионуклидов
                    for (int i = 0; i < mergedRowsCount; i++)
                    {
                        var radionuclid = new Radionuclid(characteristic);
                        characteristic.RadionuclidsList.Add(radionuclid);

                        radionuclid.Name = worksheet.Cells[$"G{currentRow + i}"].Text;
                        radionuclid.Activity = double.TryParse(worksheet.Cells[$"H{currentRow}"].Text, out doubleValue) ? doubleValue : 0;
                    }

                    currentRow += mergedRowsCount;
                }
                var offset = currentRow - startTable2;
                //Footer 
                impPassport.Notes = worksheet.Cells[$"A{33 + offset}"].Text;
                impPassport.ResponsibleTransfer = worksheet.Cells[$"F{35 + offset}"].Text;
                impPassport.GradeAuthorizedPersonTransfer = worksheet.Cells[$"J{35 + offset}"].Text;
                impPassport.FioAuthorizedPersonTransfer = worksheet.Cells[$"M{35 + offset}"].Text;

                impPassport.ResponsibleReception = worksheet.Cells[$"F{36 + offset}"].Text;
                impPassport.GradeAuthorizedPersonReception = worksheet.Cells[$"J{36 + offset}"].Text;
                impPassport.FioAuthorizedPersonReception = worksheet.Cells[$"M{36 + offset}"].Text;

                StaticConfiguration.DBModel.package_passport.Add(impPassport);
                StaticConfiguration.DBModel.SaveChangesAsync();
                passportMenuVM.UpdatePassports();
            }

            
        }
        private DateOnly? GetDateOnlyFromCell(ExcelRange cell)
        {
            if (cell.Value is DateOnly dateOnly)
            {
                return dateOnly;
            }
            else if (cell.Value is DateTime dateTime)
            {
                return DateOnly.FromDateTime(dateTime);
            }
            else if (cell.Value is double OADate)
            {
                return DateOnly.FromDateTime(
                    DateTime.FromOADate(OADate));
            }
            else if (cell.Value is string dateStr)
            {
                dateStr = dateStr.Trim();
                dateStr = dateStr.Replace(',', '.');

                if (DateOnly.TryParse(dateStr, out var dateOnlyValue));
                    return dateOnlyValue;
            }
            return null;

        }
    }
}
