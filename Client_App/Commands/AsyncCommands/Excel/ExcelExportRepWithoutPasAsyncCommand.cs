using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using Models.Forms.Form1;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.ViewModels;
using Client_App.Resources;

namespace Client_App.Commands.AsyncCommands.Excel;

//  Excel -> Паспорта -> Отчеты без паспортов
public class ExcelExportRepWithoutPasAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Отчеты без паспортов";
        var fileName = $"{ExportType}_{BaseVM.DbFileName}_{BaseVM.Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

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
        DirectoryInfo directory = new(BaseVM.PasFolderPath);
        List<FileInfo> files = new();
        try
        {
            files.AddRange(directory.GetFiles("*#*#*#*#*.pdf", SearchOption.AllDirectories));
        }
        catch (Exception)
        {
            #region MessageFailedToOpenPassportDirectory

            await MessageBox.Avalonia.MessageBoxManager
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
                .ShowDialog(Desktop.MainWindow);

            #endregion

            return;
        }

        pasNames.AddRange(files.Select(file => file.Name.Remove(file.Name.Length - 4)));
        pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));
        var currentRow = 2;
        foreach (var key in MainWindowVM.LocalReports.Reports_Collection10)
        {
            var reps = (Reports)key;
            var form11 = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null)
                .OrderBy(x => StaticStringMethods.StringReverse(x.StartPeriod_DB))
                .ThenBy(x => x.NumberInOrder_DB);
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
                        if (StaticStringMethods.ComparePasParam(StaticStringMethods.ConvertPrimToDash(repForm.CreatorOKPO_DB), pasParam[0])
                            && StaticStringMethods.ComparePasParam(StaticStringMethods.ConvertPrimToDash(repForm.Type_DB), pasParam[1])
                            && StaticStringMethods.ComparePasParam(StaticStringMethods.ConvertDateToYear(repForm.CreationDate_DB), pasParam[2])
                            && StaticStringMethods.ComparePasParam(StaticStringMethods.ConvertPrimToDash(repForm.PassportNumber_DB), pasParam[3])
                            && StaticStringMethods.ComparePasParam(StaticStringMethods.ConvertPrimToDash(repForm.FactoryNumber_DB), pasParam[4]))
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
