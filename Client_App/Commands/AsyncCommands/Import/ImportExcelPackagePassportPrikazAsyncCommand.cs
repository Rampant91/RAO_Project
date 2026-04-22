using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Controls.DataGrid;
using Client_App.ViewModels.Passports;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using Models.JSON;
using Models.Passports;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Import
{
    public class ImportExcelPackagePassportPrikazAsyncCommand(PassportsMenuWindowVM passportMenuVM) : ImportBaseAsyncCommand
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
                      && Convert.ToString(worksheet.Cells["K1"].Value)
                          is "П  А  С  П  О  Р  Т"
                      && Convert.ToString(worksheet.Cells["K2"].Value)
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
                impPassport.PassportNum = worksheet.Cells["H3"].Text;
                impPassport.PassportDate = GetDateOnlyFromCell(worksheet.Cells["L3"])?? DateOnly.MinValue ;
                impPassport.PackageType = worksheet.Cells["I5"].Text;
                impPassport.CorrectionNumber = byte.TryParse(worksheet.Cells["I7"].Text, out var byteValue)? byteValue : (byte)0;
                impPassport.StatusRaoCode = worksheet.Cells["C9"].Text;
                impPassport.TechSpecification = worksheet.Cells["G9"].Text;
                impPassport.NameRao = worksheet.Cells["M9"].Text;
                impPassport.ClassRao = byte.TryParse(worksheet.Cells["O9"].Text, out byteValue) ? byteValue : (byte)0;
                impPassport.RaoDisposalNum = worksheet.Cells["H11"].Text;
                impPassport.RaoDisposalDate = GetDateOnlyFromCell(worksheet.Cells["J11"]) ?? DateOnly.MinValue;
                impPassport.PackageIdCode = worksheet.Cells["H12"].Text;
                impPassport.TypeAndIdPuod = worksheet.Cells["M12"].Text;
                impPassport.Owner = worksheet.Cells["H14"].Text;
                impPassport.OwnerOkpo = worksheet.Cells["N14"].Text;
                impPassport.Manufacturer = worksheet.Cells["H15"].Text;
                impPassport.ManufacturerOkpo = worksheet.Cells["N15"].Text;
                impPassport.CertificateConformityNum = worksheet.Cells["H16"].Text;
                impPassport.ManufactureDate = GetDateOnlyFromCell(worksheet.Cells["O16"]) ?? DateOnly.MinValue;
                impPassport.CertificateConformityStartPeriod = GetDateOnlyFromCell(worksheet.Cells["H17"]) ?? DateOnly.MinValue;
                impPassport.CertificateConformityEndPeriod = GetDateOnlyFromCell(worksheet.Cells["J17"]) ?? DateOnly.MinValue;
                impPassport.ServiceLife = uint.TryParse(worksheet.Cells["H18"].Text, out var intValue) ? intValue : 0;
                impPassport.TransferDate = GetDateOnlyFromCell(worksheet.Cells["O18"]) ?? DateOnly.MinValue;

                //Table1
                impPassport.DisposalMethod = worksheet.Cells["A25"].Text;
                //impPassport.PhysicochemicalForm = worksheet.Cells["B24"].Text;
                //impPassport.MorphologicalComposition = worksheet.Cells["C24"].Text;
                //impPassport.Flammability = worksheet.Cells["D24"].Text;
                impPassport.MatrixMaterialType = worksheet.Cells["G25"].Text;
                impPassport.FillingWasteDate = GetDateOnlyFromCell(worksheet.Cells["H25"]) ?? DateOnly.MinValue;
                impPassport.Diameter = uint.TryParse(worksheet.Cells["I25"].Text, out intValue) ? intValue : 0;
                impPassport.Height = uint.TryParse(worksheet.Cells["J25"].Text, out intValue) ? intValue : 0;
                impPassport.Length = uint.TryParse(worksheet.Cells["K25"].Text, out intValue) ? intValue : 0;
                impPassport.Width = uint.TryParse(worksheet.Cells["L25"].Text, out intValue) ? intValue : 0;
                impPassport.PackageMass = double.TryParse(worksheet.Cells["M25"].Text, out var doubleValue) ? doubleValue : 0;
                impPassport.RaoMass = double.TryParse(worksheet.Cells["N25"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.PackageVolume = double.TryParse(worksheet.Cells["M26"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.RaoVolume = double.TryParse(worksheet.Cells["N26"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.RadiationDoseRate10cm = double.TryParse(worksheet.Cells["O25"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.RadiationDoseRate1m = double.TryParse(worksheet.Cells["P25"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.LevelNonFixedPollutionAlpha = double.TryParse(worksheet.Cells["Q25"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.LevelNonFixedPollutionBetaGamma = double.TryParse(worksheet.Cells["R25"].Text, out  doubleValue) ? doubleValue : 0;
                impPassport.HeatOutput = double.TryParse(worksheet.Cells["S25"].Text, out  doubleValue) ? doubleValue : 0;


                //Table2
                var currentRow = 25;
                var offset = 0;


                //Первая запись выделяется под общую характеристику упаковки
                impPassport.ContentCharacteristics.Add(new CharacteristicPrimaryPackage(impPassport));

                //Инициализируем характеристику первичных упаковок
                var count = 0;
                while (worksheet.Cells[$"A{currentRow + count}"].Text != "ВСЕГО:")
                {
                    CharacteristicPrimaryPackage characteristic = new CharacteristicPrimaryPackage(impPassport);

                    if (!string.IsNullOrWhiteSpace(worksheet.Cells[$"B{currentRow + count}"].Text)
                        || !string.IsNullOrWhiteSpace(worksheet.Cells[$"C{currentRow + count}"].Text))
                    {
                        characteristic.PackageType = worksheet.Cells[$"B{currentRow + count}"].Text;
                        characteristic.PackageNum = worksheet.Cells[$"C{currentRow + count}"].Text;
                        characteristic.PrimaryPackageQuantity = uint.TryParse(worksheet.Cells[$"D{currentRow + count}"].Text, out intValue) ? intValue : 0;
                        characteristic.PrimaryPackageVolume = double.TryParse(worksheet.Cells[$"E{currentRow + count}"].Text, out doubleValue) ? doubleValue : 0;
                        characteristic.PrimaryPackageMass = double.TryParse(worksheet.Cells[$"F{currentRow + count}"].Text, out doubleValue) ? doubleValue : 0;

                        impPassport.ContentCharacteristics.Add(characteristic);
                    }

                    count++;
                    if (count > 2)
                        offset++;
                }


                currentRow = 31 + offset;
                var tableRowCount = 0;

                while (worksheet.Cells[$"M{currentRow}:P{currentRow}"].Value is not "активность приведена на"
                    && impPassport.ContentCharacteristics.Count > tableRowCount)
                {
                    CharacteristicPrimaryPackage characteristic;
                    characteristic = impPassport.ContentCharacteristics[tableRowCount];


                    //Считаем кол-во строк в текущем кортеже
                    var rowHeight = 0;
                    if (worksheet.Cells[$"O{currentRow}:P{currentRow}"].Text is "долгоживущие")
                    {
                        rowHeight++;
                        while (worksheet.Cells[$"O{currentRow + rowHeight}:P{currentRow + rowHeight}"].Text is not "долгоживущие"
                            && worksheet.Cells[$"M{currentRow + rowHeight}"].Text is not "активность приведена на")
                        {
                            rowHeight++;
                        }
                    }

                    if(tableRowCount == 0)
                    {
                        impPassport.ContainerFactoryNum = worksheet.Cells[$"B{currentRow + 3}"].Text;
                    }

                    //Переносим все данные кроме списка радионуклидов
                    characteristic.ClassRao = byte.TryParse(worksheet.Cells[$"D{currentRow}"].Text, out byteValue) ? byteValue : (byte)0;
                    characteristic.CodeRao = worksheet.Cells[$"D{currentRow + 1}"].Text;
                    characteristic.PhysicochemicalForm = worksheet.Cells[$"F{currentRow}"].Text;
                    characteristic.MorphologicalComposition = worksheet.Cells[$"H{currentRow}"].Text;
                    characteristic.Flammability = worksheet.Cells[$"K{currentRow}"].Text;
                    characteristic.NuclearHazardousFissileNuclides = worksheet.Cells[$"S{currentRow}"].Text;

                    //Переносим список радионуклидов
                    for(int i = 0; i < rowHeight; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(worksheet.Cells[$"M{currentRow + i}"].Text)
                            || !string.IsNullOrWhiteSpace(worksheet.Cells[$"N{currentRow + i}"].Text))
                        {
                            var radionuclid = new Radionuclid();
                            radionuclid.Name = worksheet.Cells[$"M{currentRow + i}"].Text;
                            radionuclid.Activity = double.TryParse(worksheet.Cells[$"N{currentRow + i}"].Text, out doubleValue) ? doubleValue : 0;

                            characteristic.RadionuclidsList.Add(radionuclid);
                        }
                    }

                    currentRow += rowHeight;
                    offset += rowHeight;
                    tableRowCount++;
                }

                
                //Footer 
                impPassport.Notes = worksheet.Cells[$"A{34 + offset}"].Text;
                impPassport.ResponsibleTransfer = worksheet.Cells[$"F{37 + offset}"].Text;
                impPassport.GradeAuthorizedPersonTransfer = worksheet.Cells[$"K{37 + offset}"].Text;
                impPassport.FioAuthorizedPersonTransfer = worksheet.Cells[$"N{37 + offset}"].Text;

                impPassport.ResponsibleReception = worksheet.Cells[$"F{40 + offset}"].Text;
                impPassport.GradeAuthorizedPersonReception = worksheet.Cells[$"K{40 + offset}"].Text;
                impPassport.FioAuthorizedPersonReception = worksheet.Cells[$"N{40 + offset}"].Text;



                StaticConfiguration.DBModel.package_passport.Add(impPassport);
                await StaticConfiguration.DBModel.SaveChangesAsync();
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
