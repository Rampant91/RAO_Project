using Avalonia.Controls;
using DynamicData;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.Forms.Form1;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.ViewModels;
using Client_App.Resources;

namespace Client_App.Commands.AsyncCommands.Excel;

//  Excel -> Паспорта -> Паспорта без отчетов
public class ExcelExportPasWithoutRepAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        ExportType = "Паспорта без отчетов";
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

        #region MessageInputCategoryNums

        var res = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отмена", IsCancel = true }
                },
                ContentTitle = "Выбор категории",
                ContentMessage = "Введите через запятую номера категорий (допускается несколько значений)",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow);

        #endregion

        List<short?> categories = new() { 1, 2, 3, 4, 5 };
        if (res.Button is null or "Отмена") return;
        try
        {
            categories = Regex.Replace(res.Message, "[^\\d,]", "")
                .Split(',').Select(short.Parse).Cast<short?>().ToList();
        }
        catch (Exception)
        {
            #region MessageInvalidCategoryNums

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Номера категорий не были введены, либо были введены некорректно" +
                        $"{Environment.NewLine}Выгрузка будет осуществлена по всем категориям (1-5)",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow);

            #endregion
        }

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
        Worksheet = excelPackage.Workbook.Worksheets.Add($"Список паспортов без отчетов");

        #region Headers

        Worksheet.Cells[1, 1].Value = "Путь до папки";
        Worksheet.Cells[1, 2].Value = "Имя файла";
        Worksheet.Cells[1, 3].Value = "Код ОКПО изготовителя";
        Worksheet.Cells[1, 4].Value = "Тип";
        Worksheet.Cells[1, 5].Value = "Год выпуска";
        Worksheet.Cells[1, 6].Value = "Номер паспорта";
        Worksheet.Cells[1, 7].Value = "Номер";

        #endregion

        

        pasNames.AddRange(files.Select(file => file.Name.Remove(file.Name.Length - 4)));
        pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));
        foreach (var key in MainWindowVM.LocalReports.Reports_Collection10)
        {
            var reps = (Reports)key;
            var form11 = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null);
            foreach (var rep in form11)
            {
                List<Form11> repPas = rep.Rows11
                    .Where(x => x.OperationCode_DB is ("11" or "85") && categories.Contains(x.Category_DB))
                    .ToList();
                foreach (var repForm in repPas)
                {
                    foreach (var pasParam in pasUniqParam.Where(pasParam =>
                                 StaticStringMethods.ComparePasParam(StaticStringMethods.ConvertPrimToDash(repForm.CreatorOKPO_DB), pasParam[0])
                                 && StaticStringMethods.ComparePasParam(StaticStringMethods.ConvertPrimToDash(repForm.Type_DB), pasParam[1])
                                 && StaticStringMethods.ComparePasParam(StaticStringMethods.ConvertDateToYear(repForm.CreationDate_DB), pasParam[2])
                                 && StaticStringMethods.ComparePasParam(StaticStringMethods.ConvertPrimToDash(repForm.PassportNumber_DB), pasParam[3])
                                 && StaticStringMethods.ComparePasParam(StaticStringMethods.ConvertPrimToDash(repForm.FactoryNumber_DB), pasParam[4])))
                    {
                        files.RemoveMany(files.Where(file => file.Name.Remove(file.Name.Length - 4) == $"{pasParam[0]}#{pasParam[1]}#{pasParam[2]}#{pasParam[3]}#{pasParam[4]}"));
                        break;
                    }
                }
            }
        }

        var currentRow = 2;
        foreach (var file in files)
        {
            var pasName = file.Name.TrimEnd(".pdf".ToCharArray());
            Worksheet.Cells[currentRow, 1].Value = file.DirectoryName;
            Worksheet.Cells[currentRow, 2].Value = pasName;
            Worksheet.Cells[currentRow, 3].Value = pasName.Split('#')[0];
            Worksheet.Cells[currentRow, 4].Value = pasName.Split('#')[1];
            Worksheet.Cells[currentRow, 5].Value = pasName.Split('#')[2];
            Worksheet.Cells[currentRow, 6].Value = pasName.Split('#')[3];
            Worksheet.Cells[currentRow, 7].Value = pasName.Split('#')[4];
            currentRow++;
        }

        if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
        {
            Worksheet.Cells.AutoFitColumns();
        }
        Worksheet.View.FreezePanes(2, 1);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }
}