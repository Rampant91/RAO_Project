using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

//  Excel -> Все формы и Excel -> Выбранная организация -> Все формы
public class ExcelExportAllAsyncCommand : ExcelBaseAsyncCommand
{
    private Reports CurrentReports;
    private int _currentRow;
    private int _currentPrimRow;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var isSelectedOrg = parameter is "SelectedOrg";
        string fileName;
        var mainWindow = Desktop.MainWindow as MainWindow;
        if (isSelectedOrg)
        {
            var selectedReports = (Reports?)mainWindow?.SelectedReports.FirstOrDefault();
            if (selectedReports is null || !selectedReports.Report_Collection.Any())
            {
                #region MessageExcelExportFail

                var msg = "Выгрузка не выполнена, поскольку ";
                msg += selectedReports is null
                    ? "не выбрана организация."
                    : "у выбранной организации" +
                       $"{Environment.NewLine}отсутствуют формы отчетности.";
                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в Excel",
                        ContentHeader = "Уведомление",
                        ContentMessage = msg,
                        MinHeight = 150,
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(mainWindow));

                #endregion

                return;
            }
            CurrentReports = selectedReports;
            ExportType = "Выбранная организация_Все формы";
            var regNum = RemoveForbiddenChars(CurrentReports.Master.RegNoRep.Value);
            var okpo = RemoveForbiddenChars(CurrentReports.Master.OkpoRep.Value);
            fileName = $"{ExportType}_{regNum}_{okpo}_{BaseVM.DbFileName}_{BaseVM.Version}";
        }
        else
        {
            ExportType = "Все формы";
            fileName = $"{ExportType}_{BaseVM.DbFileName}_{BaseVM.Version}";
        }
        
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

        var repsList = new List<Reports>();
        var orderedReportsCollection = MainWindowVM.LocalReports.Reports_Collection
                .OrderBy(x => x.Master.RegNoRep.Value)
                .ToList();
        if (isSelectedOrg)
        {
            repsList.Add(CurrentReports);
        }
        else
        {
            repsList.AddRange(orderedReportsCollection);
        }
        HashSet<string> formNums = new();

        foreach (var rep in repsList
                     .SelectMany(reps => reps.Report_Collection)
                     .OrderBy(x => byte.Parse(x.FormNum_DB.Split('.')[0]))
                     .ThenBy(x => byte.Parse(x.FormNum_DB.Split('.')[1]))
                     .ToList())
        {
            formNums.Add(rep.FormNum_DB);
        }

        _currentRow = 2;
        _currentPrimRow = 2;

        foreach (var formNum in formNums)
        {
            Worksheet = excelPackage.Workbook.Worksheets.Add($"Форма {formNum}");
            WorksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {formNum}");
            if (OperatingSystem.IsWindows())
            {
                Worksheet.Cells.AutoFitColumns(); // Под Astra Linux эта команда крашит программу без GDI дров
                WorksheetPrim.Cells.AutoFitColumns();
            }
            FillHeaders(formNum);
            foreach (var reps in repsList)
            {
                CurrentReports = reps;
                FillExportForms(formNum);
            }
        }
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #region Headers

    #region FillExportForms

    private void FillExportForms(string formNum)
    {
        switch (formNum)
        {
            case "1.1":
                ExportForm11Data();
                break;
            case "1.2":
                ExportForm12Data();
                break;
            case "1.3":
                ExportForm13Data();
                break;
            case "1.4":
                ExportForm14Data();
                break;
            case "1.5":
                ExportForm15Data();
                break;
            case "1.6":
                ExportForm16Data();
                break;
            case "1.7":
                ExportForm17Data();
                break;
            case "1.8":
                ExportForm18Data();
                break;
            case "1.9":
                ExportForm19Data();
                break;
            case "2.1":
                ExportForm21Data();
                break;
            case "2.2":
                ExportForm22Data();
                break;
            case "2.3":
                ExportForm23Data();
                break;
            case "2.4":
                ExportForm24Data();
                break;
            case "2.5":
                ExportForm25Data();
                break;
            case "2.6":
                ExportForm26Data();
                break;
            case "2.7":
                ExportForm27Data();
                break;
            case "2.8":
                ExportForm28Data();
                break;
            case "2.9":
                ExportForm29Data();
                break;
            case "2.10":
                ExportForm210Data();
                break;
            case "2.11":
                ExportForm211Data();
                break;
            case "2.12":
                ExportForm212Data();
                break;
        }
    }

    #endregion

    #region FillHeaders

    private void FillHeaders(string formNum)
    {
        switch (formNum)
        {
            case "1.1":
            {
                #region Headers

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
                NotesHeaders1();

                #endregion

                break;
            }
            case "1.2":
            {
                #region Headers

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
                Worksheet.Cells[1, 12].Value = "номер паспорта";
                Worksheet.Cells[1, 13].Value = "наименование";
                Worksheet.Cells[1, 14].Value = "номер";
                Worksheet.Cells[1, 15].Value = "масса объединенного урана, кг";
                Worksheet.Cells[1, 16].Value = "код ОКПО изготовителя";
                Worksheet.Cells[1, 17].Value = "дата выпуска";
                Worksheet.Cells[1, 18].Value = "НСС, мес";
                Worksheet.Cells[1, 19].Value = "код формы собственности";
                Worksheet.Cells[1, 20].Value = "код ОКПО правообладателя";
                Worksheet.Cells[1, 21].Value = "вид";
                Worksheet.Cells[1, 22].Value = "номер";
                Worksheet.Cells[1, 23].Value = "дата";
                Worksheet.Cells[1, 24].Value = "поставщика или получателя";
                Worksheet.Cells[1, 25].Value = "перевозчика";
                Worksheet.Cells[1, 26].Value = "наименование";
                Worksheet.Cells[1, 27].Value = "тип";
                Worksheet.Cells[1, 28].Value = "номер";
                NotesHeaders1();

                #endregion

                break;
            }
            case "1.3":
            {
                #region Headers

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
                Worksheet.Cells[1, 12].Value = "номер паспорта";
                Worksheet.Cells[1, 13].Value = "тип";
                Worksheet.Cells[1, 14].Value = "радионуклиды";
                Worksheet.Cells[1, 15].Value = "номер";
                Worksheet.Cells[1, 16].Value = "активность, Бк";
                Worksheet.Cells[1, 17].Value = "код ОКПО изготовителя";
                Worksheet.Cells[1, 18].Value = "дата выпуска";
                Worksheet.Cells[1, 19].Value = "агрегатное состояние";
                Worksheet.Cells[1, 20].Value = "код формы собственности";
                Worksheet.Cells[1, 21].Value = "код ОКПО правообладателя";
                Worksheet.Cells[1, 22].Value = "вид";
                Worksheet.Cells[1, 23].Value = "номер";
                Worksheet.Cells[1, 24].Value = "дата";
                Worksheet.Cells[1, 25].Value = "поставщика или получателя";
                Worksheet.Cells[1, 26].Value = "перевозчика";
                Worksheet.Cells[1, 27].Value = "наименование";
                Worksheet.Cells[1, 28].Value = "тип";
                Worksheet.Cells[1, 29].Value = "номер";
                NotesHeaders1();

                #endregion

                break;
            }
            case "1.4":
            {
                #region Headers
        
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
                Worksheet.Cells[1, 12].Value = "номер паспорта";
                Worksheet.Cells[1, 13].Value = "наименование";
                Worksheet.Cells[1, 14].Value = "вид";
                Worksheet.Cells[1, 15].Value = "радионуклиды";
                Worksheet.Cells[1, 16].Value = "активность, Бк";
                Worksheet.Cells[1, 17].Value = "дата измерения активности";
                Worksheet.Cells[1, 18].Value = "объем, куб.м";
                Worksheet.Cells[1, 19].Value = "масса, кг";
                Worksheet.Cells[1, 20].Value = "агрегатное состояние";
                Worksheet.Cells[1, 21].Value = "код формы собственности";
                Worksheet.Cells[1, 22].Value = "код ОКПО правообладателя";
                Worksheet.Cells[1, 23].Value = "вид";
                Worksheet.Cells[1, 24].Value = "номер";
                Worksheet.Cells[1, 25].Value = "дата";
                Worksheet.Cells[1, 26].Value = "поставщика или получателя";
                Worksheet.Cells[1, 27].Value = "перевозчика";
                Worksheet.Cells[1, 28].Value = "наименование";
                Worksheet.Cells[1, 29].Value = "тип";
                Worksheet.Cells[1, 30].Value = "номер";
                NotesHeaders1(); 
        
                #endregion

                break;
            }
            case "1.5":
            {
                #region Headers
        
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
                Worksheet.Cells[1, 12].Value = "номер паспорта (сертификата) Эри, акта определения характеристик ОЗИИ";
                Worksheet.Cells[1, 13].Value = "тип";
                Worksheet.Cells[1, 14].Value = "радионуклиды";
                Worksheet.Cells[1, 15].Value = "номер";
                Worksheet.Cells[1, 16].Value = "количество, шт";
                Worksheet.Cells[1, 17].Value = "суммарная активность, Бк";
                Worksheet.Cells[1, 18].Value = "дата выпуска";
                Worksheet.Cells[1, 19].Value = "статус РАО";
                Worksheet.Cells[1, 20].Value = "вид";
                Worksheet.Cells[1, 21].Value = "номер";
                Worksheet.Cells[1, 22].Value = "дата";
                Worksheet.Cells[1, 23].Value = "поставщика или получателя";
                Worksheet.Cells[1, 24].Value = "перевозчика";
                Worksheet.Cells[1, 25].Value = "наименование";
                Worksheet.Cells[1, 26].Value = "тип";
                Worksheet.Cells[1, 27].Value = "заводской номер";
                Worksheet.Cells[1, 28].Value = "наименование";
                Worksheet.Cells[1, 29].Value = "код";
                Worksheet.Cells[1, 30].Value = "Код переработки / сортировки РАО";
                Worksheet.Cells[1, 31].Value = "Субсидия, %";
                Worksheet.Cells[1, 32].Value = "Номер мероприятия ФЦП";
                NotesHeaders1(); 
        
                #endregion
                
                break;
            }
            case "1.6":
            {
                #region Headers
        
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
                Worksheet.Cells[1, 12].Value = "Код РАО";
                Worksheet.Cells[1, 13].Value = "Статус РАО";
                Worksheet.Cells[1, 14].Value = "объем без упаковки, куб.";
                Worksheet.Cells[1, 15].Value = "масса без упаковки";
                Worksheet.Cells[1, 16].Value = "количество ОЗИИИ";
                Worksheet.Cells[1, 17].Value = "Основные радионуклиды";
                Worksheet.Cells[1, 18].Value = "тритий";
                Worksheet.Cells[1, 19].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 20].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 21].Value = "трансурановые радионуклиды";
                Worksheet.Cells[1, 22].Value = "Дата измерения активности";
                Worksheet.Cells[1, 23].Value = "вид";
                Worksheet.Cells[1, 24].Value = "номер";
                Worksheet.Cells[1, 25].Value = "дата";
                Worksheet.Cells[1, 26].Value = "поставщика или получателя";
                Worksheet.Cells[1, 27].Value = "перевозчика";
                Worksheet.Cells[1, 28].Value = "наименование";
                Worksheet.Cells[1, 29].Value = "код";
                Worksheet.Cells[1, 30].Value = "Код переработки /";
                Worksheet.Cells[1, 31].Value = "наименование";
                Worksheet.Cells[1, 32].Value = "тип";
                Worksheet.Cells[1, 33].Value = "номер упаковки";
                Worksheet.Cells[1, 34].Value = "Субсидия, %";
                Worksheet.Cells[1, 35].Value = "Номер мероприятия ФЦП";
                NotesHeaders1(); 
                
                #endregion
                
                break;
            }
            case "1.7":
            {
                #region Headers
        
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
                Worksheet.Cells[1, 12].Value = "наименование";
                Worksheet.Cells[1, 13].Value = "тип";
                Worksheet.Cells[1, 14].Value = "заводской номер";
                Worksheet.Cells[1, 15].Value = "номер упаковки (идентификационный код)";
                Worksheet.Cells[1, 16].Value = "дата формирования";
                Worksheet.Cells[1, 17].Value = "номер паспорта";
                Worksheet.Cells[1, 18].Value = "объем, куб.м";
                Worksheet.Cells[1, 19].Value = "масса брутто, т";
                Worksheet.Cells[1, 20].Value = "наименования радионуклида";
                Worksheet.Cells[1, 21].Value = "удельная активность, Бк/г";
                Worksheet.Cells[1, 22].Value = "вид";
                Worksheet.Cells[1, 23].Value = "номер";
                Worksheet.Cells[1, 24].Value = "дата";
                Worksheet.Cells[1, 25].Value = "поставщика или получателя";
                Worksheet.Cells[1, 26].Value = "перевозчика";
                Worksheet.Cells[1, 27].Value = "наименование";
                Worksheet.Cells[1, 28].Value = "код";
                Worksheet.Cells[1, 29].Value = "код";
                Worksheet.Cells[1, 30].Value = "статус";
                Worksheet.Cells[1, 31].Value = "объем без упаковки, куб.м";
                Worksheet.Cells[1, 32].Value = "масса без упаковки (нетто), т";
                Worksheet.Cells[1, 33].Value = "количество ОЗИИИ, шт";
                Worksheet.Cells[1, 34].Value = "тритий";
                Worksheet.Cells[1, 35].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 36].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 37].Value = "трансурановые радионуклиды";
                Worksheet.Cells[1, 38].Value = "Код переработки/сортировки РАО";
                Worksheet.Cells[1, 39].Value = "Субсидия, %";
                Worksheet.Cells[1, 40].Value = "Номер мероприятия ФЦП";
                NotesHeaders1(); 
                
                #endregion
                
                break;
            }
            case "1.8":
            {
                #region Headers
        
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
                Worksheet.Cells[1, 12].Value = "индивидуальный номер (идентификационный код) партии ЖРО";
                Worksheet.Cells[1, 13].Value = "номер паспорта";
                Worksheet.Cells[1, 14].Value = "объем, куб.м";
                Worksheet.Cells[1, 15].Value = "масса, т";
                Worksheet.Cells[1, 16].Value = "солесодержание, г/л";
                Worksheet.Cells[1, 17].Value = "наименование радионуклида";
                Worksheet.Cells[1, 18].Value = "удельная активность, Бк/г";
                Worksheet.Cells[1, 19].Value = "вид";
                Worksheet.Cells[1, 20].Value = "номер";
                Worksheet.Cells[1, 21].Value = "дата";
                Worksheet.Cells[1, 22].Value = "поставщика или получателя";
                Worksheet.Cells[1, 23].Value = "перевозчика";
                Worksheet.Cells[1, 24].Value = "наименование";
                Worksheet.Cells[1, 25].Value = "код";
                Worksheet.Cells[1, 26].Value = "код";
                Worksheet.Cells[1, 27].Value = "статус";
                Worksheet.Cells[1, 28].Value = "объем, куб.м";
                Worksheet.Cells[1, 29].Value = "масса, т";
                Worksheet.Cells[1, 30].Value = "тритий";
                Worksheet.Cells[1, 31].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 32].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 33].Value = "трансурановые радионуклиды";
                Worksheet.Cells[1, 34].Value = "Код переработки/сортировки РАО";
                Worksheet.Cells[1, 35].Value = "Субсидия, %";
                Worksheet.Cells[1, 36].Value = "Номер мероприятия ФЦП";
                NotesHeaders1(); 
                
                #endregion

                break;
            }
            case "1.9":
            {
                #region Headers

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
                Worksheet.Cells[1, 12].Value = "вид";
                Worksheet.Cells[1, 13].Value = "номер";
                Worksheet.Cells[1, 14].Value = "дата";
                Worksheet.Cells[1, 15].Value = "Код типа объектов учета";
                Worksheet.Cells[1, 16].Value = "радионуклиды";
                Worksheet.Cells[1, 17].Value = "активность, Бк"; 
                NotesHeaders1();

                #endregion
                
                break;
            }
            case "2.1":
            {
                #region Headers

                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "наименование";
                Worksheet.Cells[1, 8].Value = "код";
                Worksheet.Cells[1, 9].Value = "мощность куб.м/год";
                Worksheet.Cells[1, 10].Value = "количество часов работы за год";
                Worksheet.Cells[1, 11].Value = "код РАО";
                Worksheet.Cells[1, 12].Value = "статус РАО";
                Worksheet.Cells[1, 13].Value = "куб.м";
                Worksheet.Cells[1, 14].Value = "т";
                Worksheet.Cells[1, 15].Value = "ОЗИИИ, шт";
                Worksheet.Cells[1, 16].Value = "тритий";
                Worksheet.Cells[1, 17].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 18].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 19].Value = "трансурановые радионуклиды";
                Worksheet.Cells[1, 20].Value = "код РАО";
                Worksheet.Cells[1, 21].Value = "статус РАО";
                Worksheet.Cells[1, 22].Value = "куб.м";
                Worksheet.Cells[1, 23].Value = "т";
                Worksheet.Cells[1, 24].Value = "ОЗИИИ, шт";
                Worksheet.Cells[1, 25].Value = "тритий";
                Worksheet.Cells[1, 26].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 27].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 28].Value = "трансурановые радионуклиды";
                NotesHeaders2(); 

                #endregion

                break;
            }
            case "2.2":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "наименование";
                Worksheet.Cells[1, 8].Value = "код";
                Worksheet.Cells[1, 9].Value = "наименование";
                Worksheet.Cells[1, 10].Value = "тип";
                Worksheet.Cells[1, 11].Value = "количество, шт";
                Worksheet.Cells[1, 12].Value = "код РАО";
                Worksheet.Cells[1, 13].Value = "статус РАО";
                Worksheet.Cells[1, 14].Value = "РАО без упаковки";
                Worksheet.Cells[1, 15].Value = "РАО с упаковкой";
                Worksheet.Cells[1, 16].Value = "РАО без упаковки (нетто)";
                Worksheet.Cells[1, 17].Value = "РАО с упаковкой (брутто)";
                Worksheet.Cells[1, 18].Value = "Количество ОЗИИИ, шт";
                Worksheet.Cells[1, 19].Value = "тритий";
                Worksheet.Cells[1, 20].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 21].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 22].Value = "трансурановые радионуклиды";
                Worksheet.Cells[1, 23].Value = "Основные радионуклиды";
                Worksheet.Cells[1, 24].Value = "Субсидия, %";
                Worksheet.Cells[1, 25].Value = "Номер мероприятия ФЦП";
                NotesHeaders2(); 
        
                #endregion
                
                break;
            }
            case "2.3":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "наименование";
                Worksheet.Cells[1, 8].Value = "код";
                Worksheet.Cells[1, 9].Value = "проектный объем, куб.м";
                Worksheet.Cells[1, 10].Value = "код РАО";
                Worksheet.Cells[1, 11].Value = "объем, куб.м";
                Worksheet.Cells[1, 12].Value = "масса, т";
                Worksheet.Cells[1, 13].Value = "количество ОЗИИИ, шт";
                Worksheet.Cells[1, 14].Value = "суммарная активность, Бк";
                Worksheet.Cells[1, 15].Value = "номер";
                Worksheet.Cells[1, 16].Value = "дата";
                Worksheet.Cells[1, 17].Value = "срок действия";
                Worksheet.Cells[1, 18].Value = "наименование документа";
                NotesHeaders2(); 
        
                #endregion
                
                break;
            }
            case "2.4":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "Код ОЯТ";
                Worksheet.Cells[1, 8].Value = "Номер мероприятия ФЦП";
                Worksheet.Cells[1, 9].Value = "масса образованного, т";
                Worksheet.Cells[1, 10].Value = "количество образованного, шт";
                Worksheet.Cells[1, 11].Value = "масса поступивших от сторонних, т";
                Worksheet.Cells[1, 12].Value = "количество поступивших от сторонних, шт";
                Worksheet.Cells[1, 13].Value = "масса импортированных от сторонних, т";
                Worksheet.Cells[1, 14].Value = "количество импортированных от сторонних, шт";
                Worksheet.Cells[1, 15].Value = "масса учтенных по другим причинам, т";
                Worksheet.Cells[1, 16].Value = "количество учтенных по другим причинам, шт";
                Worksheet.Cells[1, 17].Value = "масса переданных сторонним, т";
                Worksheet.Cells[1, 18].Value = "количество переданных сторонним, шт";
                Worksheet.Cells[1, 19].Value = "масса переработанных, т";
                Worksheet.Cells[1, 20].Value = "количество переработанных, шт";
                Worksheet.Cells[1, 21].Value = "масса снятия с учета, т";
                Worksheet.Cells[1, 22].Value = "количество снятых с учета, шт";
                NotesHeaders2();
        
                #endregion
                
                break;
            }
            case "2.5":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "наименование, номер";
                Worksheet.Cells[1, 8].Value = "код";
                Worksheet.Cells[1, 9].Value = "код ОЯТ";
                Worksheet.Cells[1, 10].Value = "номер мероприятия ФЦП";
                Worksheet.Cells[1, 11].Value = "топливо (нетто)";
                Worksheet.Cells[1, 12].Value = "ОТВС(ТВЭЛ, выемной части реактора) брутто";
                Worksheet.Cells[1, 13].Value = "количество, шт";
                Worksheet.Cells[1, 14].Value = "альфа-излучающих нуклидов";
                Worksheet.Cells[1, 15].Value = "бета-, гамма-излучающих нуклидов";
                NotesHeaders2(); 
        
                #endregion

                break;
            }
            case "2.6":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "Номер наблюдательной скважины";
                Worksheet.Cells[1, 8].Value = "Наименование зоны контроля";
                Worksheet.Cells[1, 9].Value = "Предполагаемый источник поступления радиоактивных веществ";
                Worksheet.Cells[1, 10].Value =
                    "Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м";
                Worksheet.Cells[1, 11].Value = "Глубина отбора проб, м";
                Worksheet.Cells[1, 12].Value = "Наименование радионуклида";
                Worksheet.Cells[1, 13].Value = "Среднегодовое содержание радионуклида, Бк/кг";
                NotesHeaders2();

                #endregion
                
                break;
            }
            case "2.7":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "Наименование, номер источника выбросов";
                Worksheet.Cells[1, 8].Value = "Наименование радионуклида";
                Worksheet.Cells[1, 9].Value = "разрешенный выброс за отчетный год";
                Worksheet.Cells[1, 10].Value = "фактический выброс за отчетный год";
                Worksheet.Cells[1, 11].Value = "фактический выброс за предыдущий год";
                NotesHeaders2(); 
        
                #endregion

                break;
            }
            case "2.8":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "Наименование, номер выпуска сточных вод";
                Worksheet.Cells[1, 8].Value = "наименование";
                Worksheet.Cells[1, 9].Value = "код типа документа";
                Worksheet.Cells[1, 10].Value = "Наименование бассейнового округа";
                Worksheet.Cells[1, 11].Value = "Допустимый объем водоотведения за год, тыс.куб.м";
                Worksheet.Cells[1, 12].Value = "Отведено за отчетный период, тыс.куб.м";
                NotesHeaders2();

                #endregion
                
                break;
            }
            case "2.9":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "Наименование, номер выпуска сточных вод";
                Worksheet.Cells[1, 8].Value = "Наименование радионуклида";
                Worksheet.Cells[1, 9].Value = "допустимая";
                Worksheet.Cells[1, 10].Value = "фактическая";
                NotesHeaders2();

                #endregion
                
                break;
            }
            case "2.10":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "Наименование показателя";
                Worksheet.Cells[1, 8].Value = "Наименование участка";
                Worksheet.Cells[1, 9].Value = "Кадастровый номер участка";
                Worksheet.Cells[1, 10].Value = "Код участка";
                Worksheet.Cells[1, 11].Value = "Площадь загрязненной территории, кв.м";
                Worksheet.Cells[1, 12].Value = "средняя";
                Worksheet.Cells[1, 13].Value = "максимальная";
                Worksheet.Cells[1, 14].Value = "альфа-излучающие радионуклиды";
                Worksheet.Cells[1, 15].Value = "бета-излучающие радионуклиды";
                Worksheet.Cells[1, 16].Value = "Номер мероприятия ФЦП";
                NotesHeaders2(); 
        
                #endregion
                
                break;
            }
            case "2.11":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "Наименование участка";
                Worksheet.Cells[1, 8].Value = "Кадастровый номер участка";
                Worksheet.Cells[1, 9].Value = "Код участка";
                Worksheet.Cells[1, 10].Value = "Площадь загрязненной территории, кв.м";
                Worksheet.Cells[1, 11].Value = "Наименование радионуклидов";
                Worksheet.Cells[1, 12].Value = "земельный участок";
                Worksheet.Cells[1, 13].Value = "жидкая фаза";
                Worksheet.Cells[1, 14].Value = "донные отложения";
                NotesHeaders2();

                #endregion
                
                break;
            }
            case "2.12":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "Рег. №";
                Worksheet.Cells[1, 2].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 3].Value = "ОКПО";
                Worksheet.Cells[1, 4].Value = "Номер корректировки";
                Worksheet.Cells[1, 5].Value = "отчетный год";
                Worksheet.Cells[1, 6].Value = "№ п/п";
                Worksheet.Cells[1, 7].Value = "Код операции";
                Worksheet.Cells[1, 8].Value = "Код типа объектов учета";
                Worksheet.Cells[1, 9].Value = "радионуклиды";
                Worksheet.Cells[1, 10].Value = "активность, Бк";
                Worksheet.Cells[1, 11].Value = "ОКПО поставщика/получателя";
                NotesHeaders2();

                #endregion
                
                break;
            }
        }
    }

    #endregion

    #region NotesHeader

    private void NotesHeaders1()
    {
        WorksheetPrim.Cells[1, 1].Value = "ОКПО";
        WorksheetPrim.Cells[1, 2].Value = "Сокращенное наименование";
        WorksheetPrim.Cells[1, 3].Value = "Рег. №";
        WorksheetPrim.Cells[1, 4].Value = "Номер корректировки";
        WorksheetPrim.Cells[1, 5].Value = "Дата начала периода";
        WorksheetPrim.Cells[1, 6].Value = "Дата конца периода";
        WorksheetPrim.Cells[1, 7].Value = "№ строки";
        WorksheetPrim.Cells[1, 8].Value = "№ графы";
        WorksheetPrim.Cells[1, 9].Value = "Пояснение";
        WorksheetPrim.View.FreezePanes(2, 1);
    }

    private void NotesHeaders2()
    {
        WorksheetPrim.Cells[1, 1].Value = "ОКПО";
        WorksheetPrim.Cells[1, 2].Value = "Сокращенное наименование";
        WorksheetPrim.Cells[1, 3].Value = "Рег. №";
        WorksheetPrim.Cells[1, 4].Value = "Номер корректировки";
        WorksheetPrim.Cells[1, 5].Value = "Отчетный год";
        WorksheetPrim.Cells[1, 6].Value = "№ строки";
        WorksheetPrim.Cells[1, 7].Value = "№ графы";
        WorksheetPrim.Cells[1, 8].Value = "Пояснение";
        WorksheetPrim.View.FreezePanes(2, 1);
    }

    #endregion

    #endregion

    #region ExportForms

    #region ExportForm_11

    private void ExportForm11Data()
    {
        var reps = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null)
            .OrderBy(x => StringReverse(x.StartPeriod_DB))
            .ToList();
        foreach (var rep in reps)
        {
            var forms = rep.Rows11
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[_currentRow, 1].Value = CurrentReports.Master.RegNoRep.Value;
                Worksheet.Cells[_currentRow, 2].Value = CurrentReports.Master.Rows10[0].ShortJurLico_DB;
                Worksheet.Cells[_currentRow, 3].Value = CurrentReports.Master.OkpoRep.Value;
                Worksheet.Cells[_currentRow, 4].Value = rep.FormNum_DB;
                Worksheet.Cells[_currentRow, 5].Value = rep.StartPeriod_DB;
                Worksheet.Cells[_currentRow, 6].Value = rep.EndPeriod_DB;
                Worksheet.Cells[_currentRow, 7].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[_currentRow, 8].Value = rep.Rows.Count;
                Worksheet.Cells[_currentRow, 9].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[_currentRow, 10].Value = repForm.OperationCode_DB;
                Worksheet.Cells[_currentRow, 11].Value = repForm.OperationDate_DB;
                Worksheet.Cells[_currentRow, 12].Value = repForm.PassportNumber_DB;
                Worksheet.Cells[_currentRow, 13].Value = repForm.Type_DB;
                Worksheet.Cells[_currentRow, 14].Value = repForm.Radionuclids_DB;
                Worksheet.Cells[_currentRow, 15].Value = repForm.FactoryNumber_DB;
                Worksheet.Cells[_currentRow, 16].Value = repForm.Quantity_DB;
                Worksheet.Cells[_currentRow, 17].Value = repForm.Activity_DB;
                Worksheet.Cells[_currentRow, 18].Value = repForm.CreatorOKPO_DB;
                Worksheet.Cells[_currentRow, 19].Value = repForm.CreationDate_DB;
                Worksheet.Cells[_currentRow, 20].Value = repForm.Category_DB;
                Worksheet.Cells[_currentRow, 21].Value = repForm.SignedServicePeriod_DB;
                Worksheet.Cells[_currentRow, 22].Value = repForm.PropertyCode_DB;
                Worksheet.Cells[_currentRow, 23].Value = repForm.Owner_DB;
                Worksheet.Cells[_currentRow, 24].Value = repForm.DocumentVid_DB;
                Worksheet.Cells[_currentRow, 25].Value = repForm.DocumentNumber_DB;
                Worksheet.Cells[_currentRow, 26].Value = repForm.DocumentDate_DB;
                Worksheet.Cells[_currentRow, 27].Value = repForm.ProviderOrRecieverOKPO_DB;
                Worksheet.Cells[_currentRow, 28].Value = repForm.TransporterOKPO_DB;
                Worksheet.Cells[_currentRow, 29].Value = repForm.PackName_DB;
                Worksheet.Cells[_currentRow, 30].Value = repForm.PackType_DB;
                Worksheet.Cells[_currentRow, 31].Value = repForm.PackNumber_DB;

                #endregion

                _currentRow++;
            }

            var notes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in notes)
            {
                #region Binding

                WorksheetPrim.Cells[_currentPrimRow, 1].Value = CurrentReports.Master.OkpoRep.Value;
                WorksheetPrim.Cells[_currentPrimRow, 2].Value = CurrentReports.Master.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[_currentPrimRow, 3].Value = CurrentReports.Master.RegNoRep.Value;
                WorksheetPrim.Cells[_currentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[_currentPrimRow, 5].Value = rep.StartPeriod_DB;
                WorksheetPrim.Cells[_currentPrimRow, 6].Value = rep.EndPeriod_DB;
                WorksheetPrim.Cells[_currentPrimRow, 7].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[_currentPrimRow, 8].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[_currentPrimRow, 9].Value = comment.Comment_DB;

                #endregion

                _currentPrimRow++;
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_12

    private void ExportForm12Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.2") && x.Rows12 != null)
                .OrderBy(x => x.StartPeriod_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows12
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding

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
                    Worksheet.Cells[currentRow, 13].Value = repForm.NameIOU_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.FactoryNumber_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.Mass_DB;
                    Worksheet.Cells[currentRow, 16].Value = repForm.CreatorOKPO_DB;
                    Worksheet.Cells[currentRow, 17].Value = repForm.CreationDate_DB;
                    Worksheet.Cells[currentRow, 18].Value = repForm.SignedServicePeriod_DB;
                    Worksheet.Cells[currentRow, 19].Value = repForm.PropertyCode_DB;
                    Worksheet.Cells[currentRow, 20].Value = repForm.Owner_DB;
                    Worksheet.Cells[currentRow, 21].Value = repForm.DocumentVid_DB;
                    Worksheet.Cells[currentRow, 22].Value = repForm.DocumentNumber_DB;
                    Worksheet.Cells[currentRow, 23].Value = repForm.DocumentDate_DB;
                    Worksheet.Cells[currentRow, 24].Value = repForm.ProviderOrRecieverOKPO_DB;
                    Worksheet.Cells[currentRow, 25].Value = repForm.TransporterOKPO_DB;
                    Worksheet.Cells[currentRow, 26].Value = repForm.PackName_DB;
                    Worksheet.Cells[currentRow, 27].Value = repForm.PackType_DB;
                    Worksheet.Cells[currentRow, 28].Value = repForm.PackNumber_DB; 
                    
                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding

                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 9].Value = comment.Comment_DB; 
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_13

    private void ExportForm13Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.3") && x.Rows13 != null)
                .OrderBy(x => x.StartPeriod_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows13
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
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
                    Worksheet.Cells[currentRow, 16].Value = repForm.Activity_DB;
                    Worksheet.Cells[currentRow, 17].Value = repForm.CreatorOKPO_DB;
                    Worksheet.Cells[currentRow, 18].Value = repForm.CreationDate_DB;
                    Worksheet.Cells[currentRow, 19].Value = repForm.AggregateState_DB;
                    Worksheet.Cells[currentRow, 20].Value = repForm.PropertyCode_DB;
                    Worksheet.Cells[currentRow, 21].Value = repForm.Owner_DB;
                    Worksheet.Cells[currentRow, 22].Value = repForm.DocumentVid_DB;
                    Worksheet.Cells[currentRow, 23].Value = repForm.DocumentNumber_DB;
                    Worksheet.Cells[currentRow, 24].Value = repForm.DocumentDate_DB;
                    Worksheet.Cells[currentRow, 25].Value = repForm.ProviderOrRecieverOKPO_DB;
                    Worksheet.Cells[currentRow, 26].Value = repForm.TransporterOKPO_DB;
                    Worksheet.Cells[currentRow, 27].Value = repForm.PackName_DB;
                    Worksheet.Cells[currentRow, 28].Value = repForm.PackType_DB;
                    Worksheet.Cells[currentRow, 29].Value = repForm.PackNumber_DB; 
                    
                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 9].Value = comment.Comment_DB; 
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_14

    private void ExportForm14Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.4") && x.Rows14 != null)
                .OrderBy(x => x.StartPeriod_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows14
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
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
                    Worksheet.Cells[currentRow, 13].Value = repForm.Name_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.Sort_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.Radionuclids_DB;
                    Worksheet.Cells[currentRow, 16].Value = repForm.Activity_DB;
                    Worksheet.Cells[currentRow, 17].Value = repForm.ActivityMeasurementDate_DB;
                    Worksheet.Cells[currentRow, 18].Value = repForm.Volume_DB;
                    Worksheet.Cells[currentRow, 19].Value = repForm.Mass_DB;
                    Worksheet.Cells[currentRow, 20].Value = repForm.AggregateState_DB;
                    Worksheet.Cells[currentRow, 21].Value = repForm.PropertyCode_DB;
                    Worksheet.Cells[currentRow, 22].Value = repForm.Owner_DB;
                    Worksheet.Cells[currentRow, 23].Value = repForm.DocumentVid_DB;
                    Worksheet.Cells[currentRow, 24].Value = repForm.DocumentNumber_DB;
                    Worksheet.Cells[currentRow, 25].Value = repForm.DocumentDate_DB;
                    Worksheet.Cells[currentRow, 26].Value = repForm.ProviderOrRecieverOKPO_DB;
                    Worksheet.Cells[currentRow, 27].Value = repForm.TransporterOKPO_DB;
                    Worksheet.Cells[currentRow, 28].Value = repForm.PackName_DB;
                    Worksheet.Cells[currentRow, 29].Value = repForm.PackType_DB;
                    Worksheet.Cells[currentRow, 30].Value = repForm.PackNumber_DB; 

                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 9].Value = comment.Comment_DB; 
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_15

    private void ExportForm15Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.5") && x.Rows15 != null)
                .OrderBy(x => x.StartPeriod_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows15
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
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
                    Worksheet.Cells[currentRow, 17].Value = repForm.Activity_DB;
                    Worksheet.Cells[currentRow, 18].Value = repForm.CreationDate_DB;
                    Worksheet.Cells[currentRow, 19].Value = repForm.StatusRAO_DB;
                    Worksheet.Cells[currentRow, 20].Value = repForm.DocumentVid_DB;
                    Worksheet.Cells[currentRow, 21].Value = repForm.DocumentNumber_DB;
                    Worksheet.Cells[currentRow, 22].Value = repForm.DocumentDate_DB;
                    Worksheet.Cells[currentRow, 23].Value = repForm.ProviderOrRecieverOKPO_DB;
                    Worksheet.Cells[currentRow, 24].Value = repForm.TransporterOKPO_DB;
                    Worksheet.Cells[currentRow, 25].Value = repForm.PackName_DB;
                    Worksheet.Cells[currentRow, 26].Value = repForm.PackType_DB;
                    Worksheet.Cells[currentRow, 27].Value = repForm.PackNumber_DB;
                    Worksheet.Cells[currentRow, 28].Value = repForm.StoragePlaceName_DB;
                    Worksheet.Cells[currentRow, 29].Value = repForm.StoragePlaceCode_DB;
                    Worksheet.Cells[currentRow, 30].Value = repForm.RefineOrSortRAOCode_DB;
                    Worksheet.Cells[currentRow, 31].Value = repForm.Subsidy_DB;
                    Worksheet.Cells[currentRow, 32].Value = repForm.FcpNumber_DB;

                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 9].Value = comment.Comment_DB; 
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_16

    private void ExportForm16Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.6") && x.Rows16 != null)
                .OrderBy(x => x.StartPeriod_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows16
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
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
                    Worksheet.Cells[currentRow, 12].Value = repForm.CodeRAO_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.StatusRAO_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.Volume_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.Mass_DB;
                    Worksheet.Cells[currentRow, 16].Value = repForm.QuantityOZIII_DB;
                    Worksheet.Cells[currentRow, 17].Value = repForm.MainRadionuclids_DB;
                    Worksheet.Cells[currentRow, 18].Value = repForm.TritiumActivity_DB;
                    Worksheet.Cells[currentRow, 19].Value = repForm.BetaGammaActivity_DB;
                    Worksheet.Cells[currentRow, 20].Value = repForm.AlphaActivity_DB;
                    Worksheet.Cells[currentRow, 21].Value = repForm.TransuraniumActivity_DB;
                    Worksheet.Cells[currentRow, 22].Value = repForm.ActivityMeasurementDate_DB;
                    Worksheet.Cells[currentRow, 23].Value = repForm.DocumentVid_DB;
                    Worksheet.Cells[currentRow, 24].Value = repForm.DocumentNumber_DB;
                    Worksheet.Cells[currentRow, 25].Value = repForm.DocumentDate_DB;
                    Worksheet.Cells[currentRow, 26].Value = repForm.ProviderOrRecieverOKPO_DB;
                    Worksheet.Cells[currentRow, 27].Value = repForm.TransporterOKPO_DB;
                    Worksheet.Cells[currentRow, 28].Value = repForm.StoragePlaceName_DB;
                    Worksheet.Cells[currentRow, 29].Value = repForm.StoragePlaceCode_DB;
                    Worksheet.Cells[currentRow, 30].Value = repForm.RefineOrSortRAOCode_DB;
                    Worksheet.Cells[currentRow, 31].Value = repForm.PackName_DB;
                    Worksheet.Cells[currentRow, 32].Value = repForm.PackType_DB;
                    Worksheet.Cells[currentRow, 33].Value = repForm.PackNumber_DB;
                    Worksheet.Cells[currentRow, 34].Value = repForm.Subsidy_DB;
                    Worksheet.Cells[currentRow, 35].Value = repForm.FcpNumber_DB; 
                    
                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    
                    WorksheetPrim.Cells[currentRow, 9].Value = comment.Comment_DB; 
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_17

    private void ExportForm17Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.7") && x.Rows17 != null)
                .OrderBy(x => x.StartPeriod_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows17
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
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
                    Worksheet.Cells[currentRow, 12].Value = repForm.PackName_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.PackType_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.PackFactoryNumber_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.PackFactoryNumber_DB;
                    Worksheet.Cells[currentRow, 16].Value = repForm.FormingDate_DB;
                    Worksheet.Cells[currentRow, 17].Value = repForm.PassportNumber_DB;
                    Worksheet.Cells[currentRow, 18].Value = repForm.Volume_DB;
                    Worksheet.Cells[currentRow, 19].Value = repForm.Mass_DB;
                    Worksheet.Cells[currentRow, 20].Value = repForm.Radionuclids_DB;
                    Worksheet.Cells[currentRow, 21].Value = repForm.SpecificActivity_DB;
                    Worksheet.Cells[currentRow, 22].Value = repForm.DocumentVid_DB;
                    Worksheet.Cells[currentRow, 23].Value = repForm.DocumentNumber_DB;
                    Worksheet.Cells[currentRow, 24].Value = repForm.DocumentDate_DB;
                    Worksheet.Cells[currentRow, 25].Value = repForm.ProviderOrRecieverOKPO_DB;
                    Worksheet.Cells[currentRow, 26].Value = repForm.TransporterOKPO_DB;
                    Worksheet.Cells[currentRow, 27].Value = repForm.StoragePlaceName_DB;
                    Worksheet.Cells[currentRow, 28].Value = repForm.StoragePlaceCode_DB;
                    Worksheet.Cells[currentRow, 29].Value = repForm.CodeRAO_DB;
                    Worksheet.Cells[currentRow, 30].Value = repForm.StatusRAO_DB;
                    Worksheet.Cells[currentRow, 31].Value = repForm.VolumeOutOfPack_DB;
                    Worksheet.Cells[currentRow, 32].Value = repForm.MassOutOfPack_DB;
                    Worksheet.Cells[currentRow, 33].Value = repForm.Quantity_DB;
                    Worksheet.Cells[currentRow, 34].Value = repForm.TritiumActivity_DB;
                    Worksheet.Cells[currentRow, 35].Value = repForm.BetaGammaActivity_DB;
                    Worksheet.Cells[currentRow, 36].Value = repForm.AlphaActivity_DB;
                    Worksheet.Cells[currentRow, 37].Value = repForm.TransuraniumActivity_DB;
                    Worksheet.Cells[currentRow, 38].Value = repForm.RefineOrSortRAOCode_DB;
                    Worksheet.Cells[currentRow, 39].Value = repForm.Subsidy_DB;
                    Worksheet.Cells[currentRow, 40].Value = repForm.FcpNumber_DB; 
                    
                    #endregion

                    currentRow++;
                }
                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 9].Value = comment.Comment_DB; 
                    
                    #endregion
                    
                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_18

    private void ExportForm18Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.8") && x.Rows18 != null)
                .OrderBy(x => x.StartPeriod_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows18
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
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
                    Worksheet.Cells[currentRow, 12].Value = repForm.IndividualNumberZHRO_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.PassportNumber_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.Volume6_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.Mass7_DB;
                    Worksheet.Cells[currentRow, 16].Value = repForm.SaltConcentration_DB;
                    Worksheet.Cells[currentRow, 17].Value = repForm.Radionuclids_DB;
                    Worksheet.Cells[currentRow, 18].Value = repForm.SpecificActivity_DB;
                    Worksheet.Cells[currentRow, 19].Value = repForm.DocumentVid_DB;
                    Worksheet.Cells[currentRow, 20].Value = repForm.DocumentNumber_DB;
                    Worksheet.Cells[currentRow, 21].Value = repForm.DocumentDate_DB;
                    Worksheet.Cells[currentRow, 22].Value = repForm.ProviderOrRecieverOKPO_DB;
                    Worksheet.Cells[currentRow, 23].Value = repForm.TransporterOKPO_DB;
                    Worksheet.Cells[currentRow, 24].Value = repForm.StoragePlaceName_DB;
                    Worksheet.Cells[currentRow, 25].Value = repForm.StoragePlaceCode_DB;
                    Worksheet.Cells[currentRow, 26].Value = repForm.CodeRAO_DB;
                    Worksheet.Cells[currentRow, 27].Value = repForm.StatusRAO_DB;
                    Worksheet.Cells[currentRow, 28].Value = repForm.Volume20_DB;
                    Worksheet.Cells[currentRow, 29].Value = repForm.Mass21_DB;
                    Worksheet.Cells[currentRow, 30].Value = repForm.TritiumActivity_DB;
                    Worksheet.Cells[currentRow, 31].Value = repForm.BetaGammaActivity_DB;
                    Worksheet.Cells[currentRow, 32].Value = repForm.AlphaActivity_DB;
                    Worksheet.Cells[currentRow, 33].Value = repForm.TransuraniumActivity_DB;
                    Worksheet.Cells[currentRow, 34].Value = repForm.RefineOrSortRAOCode_DB;
                    Worksheet.Cells[currentRow, 35].Value = repForm.Subsidy_DB;
                    Worksheet.Cells[currentRow, 36].Value = repForm.FcpNumber_DB; 
                   
                    #endregion
                    
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 9].Value = comment.Comment_DB; 
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_19

    private void ExportForm19Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.9") && x.Rows19 != null)
                .OrderBy(x => StringReverse(x.StartPeriod_DB))
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows19
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding

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
                    Worksheet.Cells[currentRow, 12].Value = repForm.DocumentVid_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.DocumentNumber_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.DocumentDate_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.CodeTypeAccObject_DB;
                    Worksheet.Cells[currentRow, 16].Value = repForm.Radionuclids_DB;
                    Worksheet.Cells[currentRow, 17].Value = repForm.Activity_DB;

                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding

                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 9].Value = comment.Comment_DB;

                    #endregion

                    currentRow++;
                }
            }
        }

        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_21

    private void ExportForm21Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.1") && x.Rows21 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows21
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding

                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.RefineMachineName_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.MachineCode_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.MachinePower_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.NumberOfHoursPerYear_DB;
                    Worksheet.Cells[currentRow, 11].Value = repForm.CodeRAOIn_DB;
                    Worksheet.Cells[currentRow, 12].Value = repForm.StatusRAOIn_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.VolumeIn_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.MassIn_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.QuantityIn_DB;
                    Worksheet.Cells[currentRow, 16].Value = repForm.TritiumActivityIn_DB;
                    Worksheet.Cells[currentRow, 17].Value = repForm.BetaGammaActivityIn_DB;
                    Worksheet.Cells[currentRow, 18].Value = repForm.AlphaActivityIn_DB;
                    Worksheet.Cells[currentRow, 19].Value = repForm.TransuraniumActivityIn_DB;
                    Worksheet.Cells[currentRow, 20].Value = repForm.CodeRAOout_DB;
                    Worksheet.Cells[currentRow, 21].Value = repForm.StatusRAOout_DB;
                    Worksheet.Cells[currentRow, 22].Value = repForm.VolumeOut_DB;
                    Worksheet.Cells[currentRow, 23].Value = repForm.MassOut_DB;
                    Worksheet.Cells[currentRow, 24].Value = repForm.QuantityOZIIIout_DB;
                    Worksheet.Cells[currentRow, 25].Value = repForm.TritiumActivityOut_DB;
                    Worksheet.Cells[currentRow, 26].Value = repForm.BetaGammaActivityOut_DB;
                    Worksheet.Cells[currentRow, 27].Value = repForm.AlphaActivityOut_DB;
                    Worksheet.Cells[currentRow, 28].Value = repForm.TransuraniumActivityOut_DB; 

                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding

                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB;

                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_22

    private void ExportForm22Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.2") && x.Rows22 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows22
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.StoragePlaceName_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.StoragePlaceCode_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.PackName_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.PackType_DB;
                    Worksheet.Cells[currentRow, 11].Value = repForm.PackQuantity_DB;
                    Worksheet.Cells[currentRow, 12].Value = repForm.CodeRAO_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.StatusRAO_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.VolumeOutOfPack_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.VolumeInPack_DB;
                    Worksheet.Cells[currentRow, 16].Value = repForm.MassOutOfPack_DB;
                    Worksheet.Cells[currentRow, 17].Value = repForm.MassInPack_DB;
                    Worksheet.Cells[currentRow, 18].Value = repForm.QuantityOZIII_DB;
                    Worksheet.Cells[currentRow, 19].Value = repForm.TritiumActivity_DB;
                    Worksheet.Cells[currentRow, 20].Value = repForm.BetaGammaActivity_DB;
                    Worksheet.Cells[currentRow, 21].Value = repForm.AlphaActivity_DB;
                    Worksheet.Cells[currentRow, 22].Value = repForm.TransuraniumActivity_DB;
                    Worksheet.Cells[currentRow, 23].Value = repForm.MainRadionuclids_DB;
                    Worksheet.Cells[currentRow, 24].Value = repForm.Subsidy_DB;
                    Worksheet.Cells[currentRow, 25].Value = repForm.FcpNumber_DB; 
                    
                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB; 
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_23

    private void ExportForm23Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.3") && x.Rows23 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows23
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.StoragePlaceName_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.StoragePlaceCode_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.ProjectVolume_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.CodeRAO_DB;
                    Worksheet.Cells[currentRow, 11].Value = repForm.Volume_DB;
                    Worksheet.Cells[currentRow, 12].Value = repForm.Mass_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.QuantityOZIII_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.SummaryActivity_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.DocumentNumber_DB;
                    Worksheet.Cells[currentRow, 16].Value = repForm.DocumentDate_DB;
                    Worksheet.Cells[currentRow, 17].Value = repForm.ExpirationDate_DB;
                    Worksheet.Cells[currentRow, 18].Value = repForm.DocumentName_DB; 
                    
                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB;

                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_24

    private void ExportForm24Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.4") && x.Rows24 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows24
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.CodeOYAT_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.FcpNumber_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.MassCreated_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.QuantityCreated_DB;
                    Worksheet.Cells[currentRow, 11].Value = repForm.MassFromAnothers_DB;
                    Worksheet.Cells[currentRow, 12].Value = repForm.QuantityFromAnothers_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.MassFromAnothersImported_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.QuantityFromAnothersImported_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.MassAnotherReasons_DB;
                    Worksheet.Cells[currentRow, 16].Value = repForm.QuantityAnotherReasons_DB;
                    Worksheet.Cells[currentRow, 17].Value = repForm.MassTransferredToAnother_DB;
                    Worksheet.Cells[currentRow, 18].Value = repForm.QuantityTransferredToAnother_DB;
                    Worksheet.Cells[currentRow, 19].Value = repForm.MassRefined_DB;
                    Worksheet.Cells[currentRow, 20].Value = repForm.QuantityRefined_DB;
                    Worksheet.Cells[currentRow, 21].Value = repForm.MassRemovedFromAccount_DB;
                    Worksheet.Cells[currentRow, 22].Value = repForm.QuantityRemovedFromAccount_DB; 
                    
                    #endregion
                    
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB;

                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_25

    private void ExportForm25Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.5") && x.Rows25 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows25
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.StoragePlaceName_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.StoragePlaceCode_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.CodeOYAT_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.FcpNumber_DB;
                    Worksheet.Cells[currentRow, 11].Value = repForm.FuelMass_DB;
                    Worksheet.Cells[currentRow, 12].Value = repForm.CellMass_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.Quantity_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.AlphaActivity_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.BetaGammaActivity_DB;

                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB;
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_26

    private void ExportForm26Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.6") && x.Rows26 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows26
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.ObservedSourceNumber_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.ControlledAreaName_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.SupposedWasteSource_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.DistanceToWasteSource_DB;
                    Worksheet.Cells[currentRow, 11].Value = repForm.TestDepth_DB;
                    Worksheet.Cells[currentRow, 12].Value = repForm.RadionuclidName_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.AverageYearConcentration_DB;

                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes.OrderBy(x => x.Order).ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB; 
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_27

    private void ExportForm27Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.7") && x.Rows27 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows27
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.ObservedSourceNumber_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.RadionuclidName_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.AllowedWasteValue_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.FactedWasteValue_DB;
                    Worksheet.Cells[currentRow, 11].Value = repForm.WasteOutbreakPreviousYear_DB; 
                    
                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB;

                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_28

    private void ExportForm28Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.8") && x.Rows28 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows28
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.WasteSourceName_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.WasteRecieverName_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.RecieverTypeCode_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.PoolDistrictName_DB;
                    Worksheet.Cells[currentRow, 11].Value = repForm.AllowedWasteRemovalVolume_DB;
                    Worksheet.Cells[currentRow, 12].Value = repForm.RemovedWasteVolume_DB; 
                    
                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB; 
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_29

    private void ExportForm29Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.9") && x.Rows29 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows29
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.WasteSourceName_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.RadionuclidName_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.AllowedActivity_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.FactedActivity_DB; 
                    
                    #endregion
                    
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB; 
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_210

    private void ExportForm210Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.10") && x.Rows210 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows210
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.IndicatorName_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.PlotName_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.PlotKadastrNumber_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.PlotCode_DB;
                    Worksheet.Cells[currentRow, 11].Value = repForm.InfectedArea_DB;
                    Worksheet.Cells[currentRow, 12].Value = repForm.AvgGammaRaysDosePower_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.MaxGammaRaysDosePower_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.WasteDensityAlpha_DB;
                    Worksheet.Cells[currentRow, 15].Value = repForm.WasteDensityBeta_DB;
                    Worksheet.Cells[currentRow, 16].Value = repForm.FcpNumber_DB; 
                    
                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes.OrderBy(x => x.Order).ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB; 
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_211

    private void ExportForm211Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.11") && x.Rows211 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows211
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.PlotName_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.PlotKadastrNumber_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.PlotCode_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.InfectedArea_DB;
                    Worksheet.Cells[currentRow, 11].Value = repForm.Radionuclids_DB;
                    Worksheet.Cells[currentRow, 12].Value = repForm.SpecificActivityOfPlot_DB;
                    Worksheet.Cells[currentRow, 13].Value = repForm.SpecificActivityOfLiquidPart_DB;
                    Worksheet.Cells[currentRow, 14].Value = repForm.SpecificActivityOfDensePart_DB; 
                    
                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes
                    .OrderBy(x => x.Order)
                    .ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB;

                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_212

    private void ExportForm212Data()
    {
        var tmp = 2;
        List<Reports> repList = new() { CurrentReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("2.12") && x.Rows212 != null)
                .OrderBy(x => x.Year_DB)
                .ToList();
            foreach (var rep in form)
            {
                var currentRow = tmp;
                var repSort = rep.Rows212
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var repForm in repSort)
                {
                    #region Binding
                    
                    Worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    Worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    Worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    Worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    Worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    Worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    Worksheet.Cells[currentRow, 7].Value = repForm.OperationCode_DB;
                    Worksheet.Cells[currentRow, 8].Value = repForm.ObjectTypeCode_DB;
                    Worksheet.Cells[currentRow, 9].Value = repForm.Radionuclids_DB;
                    Worksheet.Cells[currentRow, 10].Value = repForm.Activity_DB;
                    Worksheet.Cells[currentRow, 11].Value = repForm.ProviderOrRecieverOKPO_DB;

                    #endregion

                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                var repNotes = rep.Notes.OrderBy(x => x.Order).ToList();
                foreach (var comment in repNotes)
                {
                    #region Binding
                    
                    WorksheetPrim.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    WorksheetPrim.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    WorksheetPrim.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    WorksheetPrim.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    WorksheetPrim.Cells[currentRow, 5].Value = rep.Year_DB;
                    WorksheetPrim.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    WorksheetPrim.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    WorksheetPrim.Cells[currentRow, 8].Value = comment.Comment_DB; 
                    
                    #endregion

                    currentRow++;
                }
            }
        }
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #endregion
}