using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.DTO;
using Models.Forms.Form1;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Passports;

//  Excel -> Паспорта -> Отчеты без паспортов
public class ExcelExportRepWithoutPasAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
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
        
        await using var dbReadOnly = new DBModel(dbReadOnlyPath);
        try
        {
            var reports = dbReadOnly.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Report_Collection).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
                .Select(reps => reps)
                .SelectMany(reps => reps.Report_Collection
                    .Where(rep => rep.FormNum_DB == "1.1")
                    .SelectMany(rep => rep.Rows11
                        .Where(form11 => (form11.OperationCode_DB == "11" || form11.OperationCode_DB == "85")
                                         && (form11.Category_DB == 1 || form11.Category_DB == 2 ||
                                             form11.Category_DB == 3))
                        .Select(form11 => new PasWithoutRepDTO
                        {
                            //RegNoRep = reps.Master.RegNoRep.Value,
                            //ShortJurLico = reps.Master.ShortJurLicoRep.Value,
                            //OkpoRep = reps.Master.OkpoRep.Value,
                            //FormNum = rep.FormNum_DB,
                            //StartPeriod = rep.StartPeriod_DB,
                            //EndPeriod = rep.EndPeriod_DB,
                            //CorrectionNumber = rep.CorrectionNumber_DB,
                            //RowCount = rep.Rows11.Count,
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
                        })))
                .ToList();
        }
        catch (Exception ex)
        {

        }

        DirectoryInfo directory = new(BaseVM.PasFolderPath);
        if (!directory.Exists)
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
        //var cts = new CancellationTokenSource();
        ExportType = "Отчеты без паспортов";
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{BaseVM.Version}";
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

        //var dbReadOnlyPath = Path.Combine(BaseVM.TmpDirectory, BaseVM.DbFileName + ".RAODB");
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

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        Worksheet = excelPackage.Workbook.Worksheets.Add("Список отчётов без файла паспорта");

        #region ColumnHeaders

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

        List<string> pasNames = new();
        List<string[]> pasUniqParam = new();
        List<FileInfo> files = new();
        try
        {
            files.AddRange(directory.GetFiles("*#*#*#*#*.pdf", SearchOption.AllDirectories));
        }
        catch (Exception)
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

        pasNames.AddRange(files.Select(file => file.Name.Remove(file.Name.Length - 4)));
        pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));

        //await using var dbReadOnly = new DBModel(dbReadOnlyPath);
        //var forms11 = await dbReadOnly.ReportCollectionDbSet
        //    .AsNoTracking()
        //    .AsSplitQuery()
        //    .AsQueryable()
        //    .Where(x => x.FormNum_DB == "1.1")
        //    .Include(x => x.Rows11)
        //    .SelectMany(x => x.Rows11
        //        .Where(y => (y.OperationCode_DB == "11" || y.OperationCode_DB == "85")
        //                    && (y.Category_DB == 1 || y.Category_DB == 2 || y.Category_DB == 3))
        //        .Select(form11 => new Form11DTO
        //        {
        //            Id = form11.Id,
        //            CreatorOKPO = form11.CreatorOKPO_DB,
        //            Type = form11.Type_DB,
        //            CreationDate = form11.CreationDate_DB,
        //            PassportNumber = form11.PassportNumber_DB,
        //            FactoryNumber = form11.FactoryNumber_DB
        //        }))
        //    .ToListAsync(cancellationToken: cts.Token);

        //var reports = dbReadOnly.ReportsCollectionDbSet
        //    .AsNoTracking()
        //    .AsSplitQuery()
        //    .AsQueryable()
        //    .Include(x => x.Master)
        //    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows10)
        //    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
        //    .SelectMany(reps => reps.Report_Collection
        //        .Where(rep => rep.FormNum_DB == "1.1")
        //        .SelectMany(rep => rep.Rows11
        //            .Where(form11 => (form11.OperationCode_DB == "11" || form11.OperationCode_DB  == "85")
        //                             && (form11.Category_DB == 1 || form11.Category_DB == 2 || form11.Category_DB == 3)))
        //            .Select(form11 => new Form11DTO
        //            {
        //                Id = form11.Id,
        //                CreatorOKPO = form11.CreatorOKPO_DB,
        //                Type = form11.Type_DB,
        //                CreationDate = form11.CreationDate_DB,
        //                PassportNumber = form11.PassportNumber_DB,
        //                FactoryNumber = form11.FactoryNumber_DB
        //            }));

        var currentRow = 2;
        //foreach (var pasParam in pasUniqParam)
        //{
        //    var form = forms11.FirstOrDefault(form11 =>
        //        ComparePasParam(ConvertPrimToDash(form11.CreatorOKPO), pasParam[0]) 
        //        && ComparePasParam(ConvertPrimToDash(form11.Type), pasParam[1])
        //        && ComparePasParam(ConvertDateToYear(form11.CreationDate), pasParam[2])
        //        && ComparePasParam(ConvertPrimToDash(form11.PassportNumber), pasParam[3])
        //        && ComparePasParam(ConvertPrimToDash(form11.FactoryNumber), pasParam[4]));
        //    if (form is not null)
        //    {

        //    }
            
        //}
        //foreach (var form11 in forms11)
        //{
        //    var findPasFile = false;
        //    foreach (var pasParam in pasUniqParam)
        //    {
        //        if (ComparePasParam(ConvertPrimToDash(form11.CreatorOKPO), pasParam[0])
        //            && ComparePasParam(ConvertPrimToDash(form11.Type), pasParam[1])
        //            && ComparePasParam(ConvertDateToYear(form11.CreationDate), pasParam[2])
        //            && ComparePasParam(ConvertPrimToDash(form11.PassportNumber), pasParam[3])
        //            && ComparePasParam(ConvertPrimToDash(form11.FactoryNumber), pasParam[4]))
        //        {
        //            findPasFile = true;
        //            break;
        //        }
        //    }

        //    if (!findPasFile)
        //    {

        //        #region BindingCells

        //        Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
        //        Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
        //        Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
        //        Worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
        //        Worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
        //        Worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
        //        Worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
        //        Worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
        //        Worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
        //        Worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
        //        Worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
        //        Worksheet.Cells[currentRow, 12].Value = repForm.PassportNumber_DB;
        //        Worksheet.Cells[currentRow, 13].Value = repForm.Type_DB;
        //        Worksheet.Cells[currentRow, 14].Value = repForm.Radionuclids_DB;
        //        Worksheet.Cells[currentRow, 15].Value = repForm.FactoryNumber_DB;
        //        Worksheet.Cells[currentRow, 16].Value = repForm.Quantity_DB;
        //        Worksheet.Cells[currentRow, 17].Value = repForm.Activity_DB is null or "" or "-"
        //            ? "-"
        //            : double.TryParse(repForm.Activity_DB.Replace("е", "E")
        //                .Replace("Е", "E").Replace("e", "E")
        //                .Replace("(", "").Replace(")", "")
        //                .Replace(".", ","), out var doubleValue)
        //                ? doubleValue
        //                : repForm.Activity_DB;
        //        Worksheet.Cells[currentRow, 18].Value = repForm.CreatorOKPO_DB;
        //        Worksheet.Cells[currentRow, 19].Value = repForm.CreationDate_DB;
        //        Worksheet.Cells[currentRow, 20].Value = repForm.Category_DB;
        //        Worksheet.Cells[currentRow, 21].Value = repForm.SignedServicePeriod_DB;
        //        Worksheet.Cells[currentRow, 22].Value = repForm.PropertyCode_DB;
        //        Worksheet.Cells[currentRow, 23].Value = repForm.Owner_DB;
        //        Worksheet.Cells[currentRow, 24].Value = repForm.DocumentVid_DB;
        //        Worksheet.Cells[currentRow, 25].Value = repForm.DocumentNumber_DB;
        //        Worksheet.Cells[currentRow, 26].Value = repForm.DocumentDate_DB;
        //        Worksheet.Cells[currentRow, 27].Value = repForm.ProviderOrRecieverOKPO_DB;
        //        Worksheet.Cells[currentRow, 28].Value = repForm.TransporterOKPO_DB;
        //        Worksheet.Cells[currentRow, 29].Value = repForm.PackName_DB;
        //        Worksheet.Cells[currentRow, 30].Value = repForm.PackType_DB;
        //        Worksheet.Cells[currentRow, 31].Value = repForm.PackNumber_DB;

        //        #endregion

        //        currentRow++;
        //    }
        //}


        foreach (var key in ReportsStorage.LocalReports.Reports_Collection10)
        {
            var reps = (Reports)key;
            var form11 = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null)
                .OrderBy(x => StringReverse(x.StartPeriod_DB))
                .ThenBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var rep in form11)
            {
                List<Form11> repPas = rep.Rows11
                    .Where(x => x.OperationCode_DB is "11" or "85" && x.Category_DB is 1 or 2 or 3)
                    .ToList();
                foreach (var repForm in repPas)
                {
                    var findPasFile = false;
                    foreach (var pasParam in pasUniqParam)
                    {
                        if (ComparePasParam(ConvertPrimToDash(repForm.CreatorOKPO_DB), pasParam[0])
                            && ComparePasParam(ConvertPrimToDash(repForm.Type_DB), pasParam[1])
                            && ComparePasParam(ConvertDateToYear(repForm.CreationDate_DB), pasParam[2])
                            && ComparePasParam(ConvertPrimToDash(repForm.PassportNumber_DB), pasParam[3])
                            && ComparePasParam(ConvertPrimToDash(repForm.FactoryNumber_DB), pasParam[4]))
                        {
                            findPasFile = true;
                            break;
                        }
                    }

                    if (!findPasFile)
                    {
                        #region BindingCells

                        Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                        Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                        Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                        Worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                        Worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                        Worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                        Worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                        Worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                        Worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                        Worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                        Worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                        Worksheet.Cells[currentRow, 12].Value = repForm.PassportNumber_DB;
                        Worksheet.Cells[currentRow, 13].Value = repForm.Type_DB;
                        Worksheet.Cells[currentRow, 14].Value = repForm.Radionuclids_DB;
                        Worksheet.Cells[currentRow, 15].Value = repForm.FactoryNumber_DB;
                        Worksheet.Cells[currentRow, 16].Value = repForm.Quantity_DB;
                        Worksheet.Cells[currentRow, 17].Value = repForm.Activity_DB is null or "" or "-"
                            ? "-"
                            : double.TryParse(repForm.Activity_DB.Replace("е", "E")
                                .Replace("Е", "E").Replace("e", "E")
                                .Replace("(", "").Replace(")", "")
                                .Replace(".", ","), out var doubleValue)
                                ? doubleValue
                                : repForm.Activity_DB;
                        Worksheet.Cells[currentRow, 18].Value = repForm.CreatorOKPO_DB;
                        Worksheet.Cells[currentRow, 19].Value = repForm.CreationDate_DB;
                        Worksheet.Cells[currentRow, 20].Value = repForm.Category_DB;
                        Worksheet.Cells[currentRow, 21].Value = repForm.SignedServicePeriod_DB;
                        Worksheet.Cells[currentRow, 22].Value = repForm.PropertyCode_DB;
                        Worksheet.Cells[currentRow, 23].Value = repForm.Owner_DB;
                        Worksheet.Cells[currentRow, 24].Value = repForm.DocumentVid_DB;
                        Worksheet.Cells[currentRow, 25].Value = repForm.DocumentNumber_DB;
                        Worksheet.Cells[currentRow, 26].Value = repForm.DocumentDate_DB;
                        Worksheet.Cells[currentRow, 27].Value = repForm.ProviderOrRecieverOKPO_DB;
                        Worksheet.Cells[currentRow, 28].Value = repForm.TransporterOKPO_DB;
                        Worksheet.Cells[currentRow, 29].Value = repForm.PackName_DB;
                        Worksheet.Cells[currentRow, 30].Value = repForm.PackType_DB;
                        Worksheet.Cells[currentRow, 31].Value = repForm.PackNumber_DB;

                        #endregion

                        currentRow++;
                    }
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}