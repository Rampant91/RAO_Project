using Avalonia.Controls;
using Avalonia.Threading;
using Client_App;
using Client_App.Interfaces.Logger;
using Client_App.Resources.CustomComparers;
using Client_App.ViewModels;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.Views.Messages;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Forms.Form3;
using Models.Forms.Form4;
using Models.Forms.Form5;
using Models.Forms.Form5;
using OfficeOpenXml;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Tmds.DBus;
using static Client_App.ViewModels.Messages.SelectReportsMessageWindowVM;
using static Client_App.ViewModels.Messages.SelectReportsMessageWindowVM;

namespace Client_App.Commands.AsyncCommands.Import;

/// <summary>
/// Импорт -> Из Excel.
/// </summary>
public class ImportExcelAsyncCommand : ImportBaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlBaseVM;

    public ImportExcelAsyncCommand() { }

    public ImportExcelAsyncCommand(FormsTabControlBaseVM formsTabControlBaseVM)
    {
        _formsTabControlBaseVM = formsTabControlBaseVM;

        formsTabControlBaseVM.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(FormsTabControlBaseVM.SelectedReports))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) =>
        parameter switch
        {
            Reports => true,
            "Selected" => _formsTabControlBaseVM.SelectedReports is not null,
            "Auto" or "FromList" => true,
            _ => false
        };

    public override async Task AsyncExecute(object? parameter)
    {
        RepsWhereTitleFormCheckIsCancel.Clear();
        IsFirstLogLine = true;
        CurrentLogLine = 1;
        var readAnyExcel = false;
        string[] extensions = ["xlsx", "XLSX"];
        var answer = await GetSelectedFilesFromDialog("Excel", extensions);
        if (answer is null) return;
        ClearImportSummaryReports();
        var importSummaryShown = false;
        try
        {
            SkipNewOrg = false;
            SkipInter = false;
            SkipReplace = false;
            HasMultipleReport = false;
            AtLeastOneImportDone = false;

            var impReportsList = new List<Reports>();
            foreach (var res in answer) // Для каждого импортируемого файла
            {

                #region CheckExcelFile
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
                #endregion

                ExcelPackage excelPackage = new(SourceFile);
                var worksheet0 = excelPackage.Workbook.Worksheets[0];

                #region CheckFileIsValid
                // Проверка формата формы, записанного в Excel
                var patternIsValid =
                    (worksheet0.Name == "1.0" && Convert.ToString(worksheet0.Cells["A3"].Value)
                        is "ГОСУДАОСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"
                        or "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ")
                    || (worksheet0.Name == "2.0"
                        && Convert.ToString(worksheet0.Cells["A4"].Value)
                            is "ГОСУДАОСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"
                            or "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ")
                    || (worksheet0.Name == "Форма 3.0"
                        && (Convert.ToString(worksheet0.Cells["A7"].Value) 
                                is "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"))
                    || (worksheet0.Name == "Форма 4.0"
                        && (Convert.ToString(worksheet0.Cells["A7"].Value) //Старый шаблон
                                is "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ\n" +
                                   "Конфиденциальность гарантируется получателем информации"
                            || Convert.ToString(worksheet0.Cells["A6"].Value) //Новый шаблон
                                is "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ\n" +
                                   "Конфиденциальность гарантируется получателем информации"))
                    || (worksheet0.Name == "Форма 5.0"
                        && Convert.ToString(worksheet0.Cells["A7"].Value)
                            is "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ\n" +
                               "Конфиденциальность гарантируется получателем информации");


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
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow));

                    #endregion

                    continue;
                }
                readAnyExcel = true;
                #endregion

                #region TimeCreate
                var timeCreate = new List<string>
            {
                excelPackage.File.CreationTime.Day.ToString(),
                excelPackage.File.CreationTime.Month.ToString(),
                excelPackage.File.CreationTime.Year.ToString()
            };
                if (timeCreate[0].Length == 1)
                {
                    timeCreate[0] = $"0{timeCreate[0]}";
                }

                if (timeCreate[1].Length == 1)
                {
                    timeCreate[1] = $"0{timeCreate[1]}";
                }
                #endregion

                #region GetImportReps

                //Импортируем данные из титульника
                var impReps = GetImportReps(worksheet0);

                #endregion

                ExcelWorksheet? worksheet1 = null;

                if (excelPackage.Workbook.Worksheets.Count > 1)
                    worksheet1 = excelPackage.Workbook.Worksheets[1];

                Report? impRep;
                
                if (worksheet0.Name is "Форма 3.0")
                // Из-за уникальной структуры третьих форм, была написана отдельная фуекция для чтения их содержимого
                    impRep = GetReport3xFromExcel(worksheet0, worksheet1, timeCreate); 
                else
                    impRep = GetReportFromExcel(worksheet0, worksheet1, timeCreate);


                if (impRep is not null)
                {
                    ImpRepCorNum = impRep.CorrectionNumber_DB;
                    ImpRepEndPeriod = impRep.EndPeriod_DB;
                    ImpRepFormCount = impRep.Rows.Count;
                    ImpRepFormNum = impRep.FormNum_DB;
                    ImpRepStartPeriod = impRep.StartPeriod_DB;
                    ImpRepYear = impRep.Year_DB ?? "";

                    impReps.Report_Collection.Add(impRep);

                }


                HasMultipleReport = answer.Length > 1;


                #region FindBaseReports

                Reports? baseReps = null;


                var titleListNum = worksheet0.Name;
                // В некоторых шаблонах в наименовании листа Excel перед номером формы добавляется слово "Форма". Например "Форма 4.0"
                // а в других просто пишется номер формы. Например "1.0"
                if (titleListNum.ToLower().StartsWith("форма "))
                    titleListNum = titleListNum.Split(' ')[1];

                // В первую очередь записываем основные данные титульного листа (1.0, 2.0, 4.0, 5.0)
                // Для 1.0 и 2.0 основные данные - это рег.Номер и ОКПО
                // Для 4.0 основные данные - это код субъекта
                // У Формы 5.0 - полное наименование
                //switch (titleListNum)
                //{
                //    case "1.0" or "2.0":
                //        {
                //            #region 1.0 or 2.0

                var executeMode = parameter switch
                {
                    Reports => "Selected",
                    string mode => mode,
                    _ => null
                };

                switch (executeMode)
                {
                    case "Auto":
                        {
                            baseReps = GetBaseReps(worksheet0);
                            break;
                        }
                    case "Selected":
                        {
                            var selectedReports = parameter as Reports ?? _formsTabControlBaseVM.SelectedReports;
                            if (selectedReports is null) return;

                            if (selectedReports.Master_DB.FormNum_DB[0] != impReps.Master_DB.FormNum_DB[0])
                                continue;


                            baseReps = selectedReports;
                            break;
                        }
                    case "FromList":
                        {
                            var localRepsList = await GetReportsListFromDB(impReps.Master_DB.FormNum_DB);
                            var currentReportIndex = impReportsList.IndexOf(impReps) + 1;
                            var selectReportsMessageWindow = new SelectReportsMessageWindow(localRepsList, SourceFile!.Name, impReportsList.Count, currentReportIndex, impReps);
                            var selectedReports = await selectReportsMessageWindow.ShowDialog<OrganizationInfo>(Desktop.MainWindow);
                            if (selectedReports is null) return;

                            baseReps = StaticConfiguration.DBModel.ReportsCollectionDbSet
                                .Include(reps=>reps.Master_DB)
                                .FirstOrDefault(reps => reps.Id == selectedReports.ReportsId);
                            break;
                        }
                    default: return;
                }

                #endregion

                #region FindBaseReport

                //Report? baseRep = null;
                //if (baseReps != null)
                //{
                //    baseRep = StaticConfiguration.DBModel.ReportCollectionDbSet
                //        .Where(rep => rep.Reports.Id == baseReps.Id)
                //        .FirstOrDefault(rep => rep.FormNum_DB == impRep.FormNum_DB
                //        && ((rep.FormNum_DB[0] == '1'
                //        && rep.StartPeriod_DB == impRep.StartPeriod_DB
                //        && rep.EndPeriod_DB == impRep.EndPeriod_DB)
                //        || (rep.FormNum_DB[0] != '1'
                //        && rep.Year_DB == impRep.Year_DB)));
                //}

                #endregion

                if (baseReps is null)
                {

                    baseReps = StaticConfiguration.DBModel.ReportsCollectionDbSet.Add(impReps).Entity;
                    baseReps.DBObservableId = 1;

                }

                if (impRep is null)
                {
                    await CheckAnswer("Только титульный лист", baseReps, impReps, null, impRep);
                    continue;
                }

                // Проверяем есть ли в БД, импортируемые отчеты
                var impRepList = new List<Report> { impRep };
                if (!ExcelImportNewReps && impRepList.Count>0)
                {
                    switch (worksheet0.Name.ToLower())
                    {
                        case "1.0":
                            {
                                await ProcessIfHasReports11(baseReps, impReps, impRepList);
                                break;
                            }
                        case "2.0":
                            {
                                await ProcessIfHasReports21(baseReps, impReps, impRepList);
                                break;
                            }
                        case "форма 3.0":
                            {
                                await ProcessIfHasReports31(baseReps, impReps, impRepList);
                                break;
                            }
                        case "форма 4.0":
                            {
                                await ProcessIfHasReports41(baseReps, impReps, impRepList);
                                break;
                            }
                        case "форма 5.0":
                            {
                                await ProcessIfHasReports51(baseReps, impReps, impRepList);
                                break;
                            }

                    }
                }
                else
                {
                    await CheckAnswer("Добавить", baseReps, impReps, null, impRep);
                    
                }
            }

            
            //try
            //{
            //    var comparator = new CustomReportsComparer();
            //    var tmpReportsList = new List<Reports>(ReportsStorage.LocalReports.Reports_Collection);
            //    if (tmpReportsList.All(x => x.Master_DB.RegNoRep != null && x.Master_DB.OkpoRep != null))
            //    {
            //        var tmpReportsOrderedEnum = tmpReportsList
            //            .OrderBy(x => x.Master_DB?.RegNoRep?.Value, comparator)
            //            .ThenBy(x => x.Master_DB?.OkpoRep?.Value, comparator);

            //        ReportsStorage.LocalReports.Reports_Collection.Clear();
            //        ReportsStorage.LocalReports.Reports_Collection.AddRange(tmpReportsOrderedEnum);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    var msg = $"{Environment.NewLine}Message: {ex.Message}" +
            //              $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            //    ServiceExtension.LoggerManager.Warning(msg);
            //    return;
            //}


            #region SaveDbChanges
            try
            {
                await StaticConfiguration.DBModel.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var list = StaticConfiguration.DBModel.ChangeTracker.Entries<Report>()
                        .Where(e => e.State == EntityState.Added)
                        .Select(e => e.Entity)
                        .Where(rep => rep.FormNum_DB == "2.1")
                        .ToList();

                #region MessageImportError

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Импорт из .xlsx",
                        ContentHeader = "Уведомление",
                        ContentMessage = "При сохранении импортированных данных возникла ошибка.\n",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                return;
            }
            #endregion
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            importSummaryShown = await ShowImportSummaryMessageIfAnyAsync();
        }

        //if (impReportsList.All(x => x.Master_DB.FormNum_DB is "1.0" or "2.0"))
        //{
        //    await SetDataGridPage(impReportsList);
        //}

        if (AtLeastOneImportDone && readAnyExcel)
        {
            var mainWindowVM = Desktop.MainWindow.DataContext as MainWindowVM;
            mainWindowVM.UpdateReportsCollection();
        }
        else if (!importSummaryShown && readAnyExcel)
        {
            #region MessageImportCancel

            var suffix = answer.Length.ToString() is [.., '1'] && !answer.Length.ToString().EndsWith("11")
                ? "а"
                : "ов";

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Импорт из .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Импорт из файл{suffix} .xlsx был отменен.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion
        }
    }

    #region GetBaseReps

    /// <summary>
    /// 
    /// </summary>
    /// <param name="worksheet">Лист Excel.</param>
    /// <returns></returns>
    private static Reports? GetBaseReps(ExcelWorksheet worksheet0)
    {
        var dbm = StaticConfiguration.DBModel;

        switch(worksheet0.Name.ToLower())
        {
            case "1.0":
                {
                    var excelOkpo0 = Convert.ToString(worksheet0.Cells["B36"].Value);
                    var excelOkpo1 = Convert.ToString(worksheet0.Cells["B37"].Value);
                    var excelRegNo = Convert.ToString(worksheet0.Cells["F6"].Value);


                    //Собираем список из содержимого БД
                    //Сохраненные отчеты
                    var form10List = dbm.ReportsCollectionDbSet
                        .Include(reps => reps.Master_DB).ThenInclude(rep => rep.Rows10)
                        .Where(reps => reps.Master_DB.FormNum_DB == "1.0").ToList();

                    //Добавленные, но не сохраненные отчеты
                    form10List.AddRange(StaticConfiguration.DBModel.ChangeTracker.Entries<Reports>()
                        .Where(e => e.State == EntityState.Added)
                        .Select(e => e.Entity)
                        .Where(reps => reps.Master_DB.FormNum_DB == "1.0"));

                    return form10List
                        .FirstOrDefault(t =>

                             // обособленные пусты и в базе и в импорте, то сверяем головное
                             excelOkpo0 == t.Master_DB.Rows10[0].Okpo_DB
                             && excelRegNo == t.Master_DB.Rows10[0].RegNo_DB
                             && excelOkpo1 == ""
                             && t.Master_DB.Rows10[1].Okpo_DB == ""

                             // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                             || excelOkpo0 == t.Master_DB.Rows10[0].Okpo_DB 
                             && t.Master.Rows10[0].RegNo_DB == ""
                             && excelRegNo == t.Master_DB.Rows10[1].RegNo_DB
                             && excelOkpo1 == ""
                             && t.Master_DB.Rows10[1].Okpo_DB == ""

                             // обособленные не пусты, их и сверяем
                             || excelOkpo1 == t.Master_DB.Rows10[1].Okpo_DB
                             && excelRegNo == t.Master_DB.Rows10[1].RegNo_DB
                             && excelOkpo1 != ""

                             // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                             || excelOkpo1 == t.Master_DB.Rows10[1].Okpo_DB
                             && excelRegNo == t.Master_DB.Rows10[0].RegNo_DB
                             && excelOkpo1 != ""
                             && t.Master_DB.Rows10[1].RegNo_DB == "")
                        ?? form10List
                        .FirstOrDefault(t =>
                             // юр лицо в базе совпадает с обособленным в импорте
                             excelOkpo1 != ""
                             && t.Master_DB.Rows10[1].Okpo_DB == ""
                             && excelOkpo1 == t.Master_DB.Rows10[0].Okpo_DB
                             && excelRegNo == t.Master_DB.Rows10[0].RegNo_DB

                             // юр лицо в импорте совпадает с обособленным в базе
                             || excelOkpo1 == ""
                             && t.Master_DB.Rows10[1].Okpo_DB != ""
                             && excelOkpo0 == t.Master_DB.Rows10[1].Okpo_DB
                             && excelRegNo == t.Master_DB.Rows10[1].RegNo_DB);
                    break;
                }

            case "2.0":
                {
                    var excelOkpo0 = Convert.ToString(worksheet0.Cells["B36"].Value);
                    var excelOkpo1 = Convert.ToString(worksheet0.Cells["B37"].Value);
                    var excelRegNo = Convert.ToString(worksheet0.Cells["F6"].Value);

                    //Собираем список из содержимого БД
                    //Сохраненные отчеты
                    var form20List = dbm.ReportsCollectionDbSet
                        .Include(reps => reps.Master_DB).ThenInclude(rep => rep.Rows20)
                        .Where(reps => reps.Master_DB.FormNum_DB == "2.0").ToList();

                    //Добавленные, но не сохраненные отчеты
                    form20List.AddRange(StaticConfiguration.DBModel.ChangeTracker.Entries<Reports>()
                        .Where(e => e.State == EntityState.Added)
                        .Select(e => e.Entity)
                        .Where(reps => reps.Master_DB.FormNum_DB == "2.0"));

                    return form20List
                        .FirstOrDefault(t =>

                           // обособленные пусты и в базе и в импорте, то сверяем головное
                           excelOkpo0 == t.Master_DB.Rows20[0].Okpo_DB
                           && excelRegNo == t.Master_DB.Rows20[0].RegNo_DB
                           && excelOkpo1 == ""
                           && t.Master_DB.Rows20[1].Okpo_DB == ""

                           // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || excelOkpo0 == t.Master_DB.Rows20[0].Okpo_DB
                           && t.Master.Rows20[0].RegNo_DB == ""
                           && excelRegNo == t.Master_DB.Rows20[1].RegNo_DB
                           && excelOkpo1 == ""
                           && t.Master_DB.Rows20[1].Okpo_DB == ""

                           // обособленные не пусты, их и сверяем
                           || excelOkpo1 == t.Master_DB.Rows20[1].Okpo_DB
                           && excelRegNo == t.Master_DB.Rows20[1].RegNo_DB
                           && excelOkpo1 != ""

                           // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || excelOkpo1 == t.Master_DB.Rows20[1].Okpo_DB
                           && excelRegNo == t.Master_DB.Rows20[0].RegNo_DB
                           && excelOkpo1 != ""
                           && t.Master_DB.Rows20[1].RegNo_DB == "")

                   ?? form20List
                       .FirstOrDefault(t =>

                           // юр лицо в базе совпадает с обособленным в импорте
                           excelOkpo1 != ""
                           && t.Master_DB.Rows20[1].Okpo_DB == ""
                           && excelOkpo1 == t.Master_DB.Rows20[0].Okpo_DB
                           && excelRegNo == t.Master_DB.Rows20[0].RegNo_DB

                           // юр лицо в импорте совпадает с обособленным в базе
                           || excelOkpo1 == ""
                           && t.Master_DB.Rows20[1].Okpo_DB != ""
                           && excelOkpo0 == t.Master_DB.Rows20[1].Okpo_DB
                           && excelRegNo == t.Master_DB.Rows20[1].RegNo_DB);
                    break;
                }
            case "форма 3.0":
                {
                    var excelOkpo0 = Convert.ToString(worksheet0.Cells["B36"].Value);
                    var excelOkpo1 = Convert.ToString(worksheet0.Cells["B37"].Value);
                    var excelRegNo = Convert.ToString(worksheet0.Cells["B10"].Value);

                    //Собираем список из содержимого БД
                    //Сохраненные отчеты
                    var form30List = dbm.ReportsCollectionDbSet
                        .Include(reps => reps.Master_DB).ThenInclude(rep => rep.Rows30)
                        .Where(reps => reps.Master_DB.FormNum_DB == "3.0").ToList();

                    //Добавленные, но не сохраненные отчеты
                    form30List.AddRange(StaticConfiguration.DBModel.ChangeTracker.Entries<Reports>()
                        .Where(e => e.State == EntityState.Added)
                        .Select(e => e.Entity)
                        .Where(reps => reps.Master_DB.FormNum_DB == "3.0"));

                    return form30List
                        .FirstOrDefault(t =>

                           // обособленные пусты и в базе и в импорте, то сверяем головное
                           excelOkpo0 == t.Master_DB.Rows30[0].Okpo_DB
                           && excelRegNo == t.Master_DB.Rows30[0].RegNo_DB
                           && excelOkpo1 == ""
                           && t.Master_DB.Rows30[1].Okpo_DB == ""

                           // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || excelOkpo0 == t.Master_DB.Rows30[0].Okpo_DB 
                           && t.Master.Rows30[0].RegNo_DB == ""
                           && excelRegNo == t.Master_DB.Rows30[1].RegNo_DB
                           && excelOkpo1 == ""
                           && t.Master_DB.Rows30[1].Okpo_DB == ""

                           // обособленные не пусты, их и сверяем
                           || excelOkpo1 == t.Master_DB.Rows30[1].Okpo_DB
                           && excelRegNo == t.Master_DB.Rows30[1].RegNo_DB
                           && excelOkpo1 != ""

                           // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || excelOkpo1 == t.Master_DB.Rows30[1].Okpo_DB
                           && excelRegNo == t.Master_DB.Rows30[0].RegNo_DB
                           && excelOkpo1 != ""
                           && t.Master_DB.Rows30[1].RegNo_DB == "")

                   ?? form30List
                       .FirstOrDefault(t =>

                           // юр лицо в базе совпадает с обособленным в импорте
                           excelOkpo1 != ""
                           && t.Master_DB.Rows30[1].Okpo_DB == ""
                           && excelOkpo1 == t.Master_DB.Rows30[0].Okpo_DB
                           && excelRegNo == t.Master_DB.Rows30[0].RegNo_DB

                           // юр лицо в импорте совпадает с обособленным в базе
                           || excelOkpo1 == ""
                           && t.Master_DB.Rows30[1].Okpo_DB != ""
                           && excelOkpo0 == t.Master_DB.Rows30[1].Okpo_DB
                           && excelRegNo == t.Master_DB.Rows30[1].RegNo_DB);
                    break;
                }
            case "форма 4.0":
                {
                    //Собираем список из содержимого БД
                    //Сохраненные отчеты
                    var form40List = dbm.ReportsCollectionDbSet
                        .Include(reps => reps.Master_DB).ThenInclude(rep => rep.Rows40)
                        .Where(reps => reps.Master_DB.FormNum_DB == "4.0").ToList();

                    //Добавленные, но не сохраненные отчеты
                    form40List.AddRange(StaticConfiguration.DBModel.ChangeTracker.Entries<Reports>()
                        .Where(e => e.State == EntityState.Added)
                        .Select(e => e.Entity)
                        .Where(reps => reps.Master_DB.FormNum_DB == "4.0"));

                    var codeSubjectRF = Convert.ToString(worksheet0.Cells["B8"].Value);
                    var subjectRF = Convert.ToString(worksheet0.Cells["B9"].Value);
                    return form40List
                       .FirstOrDefault(t => t.Master_DB.Rows40[0].CodeSubjectRF_DB == codeSubjectRF
                       || t.Master_DB.Rows40[0].SubjectRF_DB == subjectRF);
                    break;
                }
            case "форма 5.0":
                {
                    //Собираем список из содержимого БД
                    //Сохраненные отчеты
                    var form50List = dbm.ReportsCollectionDbSet
                        .Include(reps => reps.Master_DB).ThenInclude(rep => rep.Rows50)
                        .Where(reps => reps.Master_DB.FormNum_DB == "5.0").ToList();

                    //Добавленные, но не сохраненные отчеты
                    form50List.AddRange(StaticConfiguration.DBModel.ChangeTracker.Entries<Reports>()
                        .Where(e => e.State == EntityState.Added)
                        .Select(e => e.Entity)
                        .Where(reps => reps.Master_DB.FormNum_DB == "5.0"));

                    var name = Convert.ToString(worksheet0.Cells["B20"].Value);

                    return form50List
                       .FirstOrDefault(t => t.Master_DB.Rows50[0].Name_DB == name);
                    break;
                }
            default: return null;
        }
    }

    #endregion

    #region GetDataFromRow

    private int NumberInOrder { get; set; } = 1;

    private void GetDataFromRow(string param1, ExcelWorksheet worksheet1, int start, Report repFromEx)
    {
        if (param1 is "2.1" or "2.2"
            && !int.TryParse(Convert.ToString(worksheet1.Cells[$"A{start}"].Value), out _)) return;
        dynamic form = FormCreator.Create(param1);
        form.ExcelGetRow(worksheet1, start);
        form.NumberInOrder_DB = NumberInOrder++;
        repFromEx.Rows.Add(form);
    }

    #endregion

    #region GetDataTitleReps

    private static void GetDataTitleReps(Reports newRepsFromExcel, ExcelWorksheet worksheet)
    {
        switch (worksheet.Name)
        {
            case "1.0":
                {
                    newRepsFromExcel.Master_DB.Rows10[0].RegNo_DB = Convert.ToString(worksheet.Cells["F6"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].OrganUprav_DB = Convert.ToString(worksheet.Cells["F15"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].SubjectRF_DB = Convert.ToString(worksheet.Cells["F16"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].JurLico_DB = Convert.ToString(worksheet.Cells["F17"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].ShortJurLico_DB = worksheet.Cells["F18"].Value == null
                        ? ""
                        : Convert.ToString(worksheet.Cells["F18"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].JurLicoAddress_DB = Convert.ToString(worksheet.Cells["F19"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].JurLicoFactAddress_DB = Convert.ToString(worksheet.Cells["F20"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].GradeFIO_DB = Convert.ToString(worksheet.Cells["F21"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].Telephone_DB = Convert.ToString(worksheet.Cells["F22"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].Fax_DB = Convert.ToString(worksheet.Cells["F23"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].Email_DB = Convert.ToString(worksheet.Cells["F24"].Value);

                    newRepsFromExcel.Master_DB.Rows10[1].SubjectRF_DB = Convert.ToString(worksheet.Cells["F25"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].JurLico_DB = Convert.ToString(worksheet.Cells["F26"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].ShortJurLico_DB = worksheet.Cells["F27"].Value == null
                        ? ""
                        : Convert.ToString(worksheet.Cells["F27"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].JurLicoAddress_DB = Convert.ToString(worksheet.Cells["F28"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].JurLicoFactAddress_DB = Convert.ToString(worksheet.Cells["F28"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].GradeFIO_DB = Convert.ToString(worksheet.Cells["F29"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].Telephone_DB = Convert.ToString(worksheet.Cells["F30"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].Fax_DB = Convert.ToString(worksheet.Cells["F31"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].Email_DB = Convert.ToString(worksheet.Cells["F32"].Value);

                    newRepsFromExcel.Master_DB.Rows10[0].Okpo_DB = worksheet.Cells["B36"].Value == null
                        ? ""
                        : Convert.ToString(worksheet.Cells["B36"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].Okved_DB = Convert.ToString(worksheet.Cells["C36"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].Okogu_DB = Convert.ToString(worksheet.Cells["D36"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].Oktmo_DB = Convert.ToString(worksheet.Cells["E36"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].Inn_DB = Convert.ToString(worksheet.Cells["F36"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].Kpp_DB = Convert.ToString(worksheet.Cells["G36"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].Okopf_DB = Convert.ToString(worksheet.Cells["H36"].Value);
                    newRepsFromExcel.Master_DB.Rows10[0].Okfs_DB = Convert.ToString(worksheet.Cells["I36"].Value);

                    newRepsFromExcel.Master_DB.Rows10[1].Okpo_DB = worksheet.Cells["B37"].Value == null
                        ? ""
                        : Convert.ToString(worksheet.Cells["B37"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].Okved_DB = Convert.ToString(worksheet.Cells["C37"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].Okogu_DB = Convert.ToString(worksheet.Cells["D37"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].Oktmo_DB = Convert.ToString(worksheet.Cells["E37"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].Inn_DB = Convert.ToString(worksheet.Cells["F37"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].Kpp_DB = Convert.ToString(worksheet.Cells["G37"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].Okopf_DB = Convert.ToString(worksheet.Cells["H37"].Value);
                    newRepsFromExcel.Master_DB.Rows10[1].Okfs_DB = Convert.ToString(worksheet.Cells["I37"].Value);
                    break;
                }
            case "2.0":
                {
                    newRepsFromExcel.Master_DB.Rows20[0].RegNo.Value = Convert.ToString(worksheet.Cells["F6"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].OrganUprav_DB = Convert.ToString(worksheet.Cells["F15"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].SubjectRF_DB = Convert.ToString(worksheet.Cells["F16"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].JurLico_DB = Convert.ToString(worksheet.Cells["F17"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].ShortJurLico_DB = Convert.ToString(worksheet.Cells["F18"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].JurLicoAddress_DB = Convert.ToString(worksheet.Cells["F19"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].JurLicoFactAddress_DB = Convert.ToString(worksheet.Cells["F20"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].GradeFIO_DB = Convert.ToString(worksheet.Cells["F21"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].Telephone_DB = Convert.ToString(worksheet.Cells["F22"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].Fax_DB = Convert.ToString(worksheet.Cells["F23"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].Email_DB = Convert.ToString(worksheet.Cells["F24"].Value);

                    newRepsFromExcel.Master_DB.Rows20[1].SubjectRF_DB = Convert.ToString(worksheet.Cells["F25"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].JurLico_DB = Convert.ToString(worksheet.Cells["F26"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].ShortJurLico_DB = Convert.ToString(worksheet.Cells["F27"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].JurLicoAddress_DB = Convert.ToString(worksheet.Cells["F28"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].JurLicoFactAddress_DB = Convert.ToString(worksheet.Cells["F28"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].GradeFIO_DB = Convert.ToString(worksheet.Cells["F29"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].Telephone_DB = Convert.ToString(worksheet.Cells["F30"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].Fax_DB = Convert.ToString(worksheet.Cells["F31"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].Email_DB = Convert.ToString(worksheet.Cells["F32"].Value);

                    newRepsFromExcel.Master_DB.Rows20[0].Okpo_DB = Convert.ToString(worksheet.Cells["B36"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].Okved_DB = Convert.ToString(worksheet.Cells["C36"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].Okogu_DB = Convert.ToString(worksheet.Cells["D36"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].Oktmo_DB = Convert.ToString(worksheet.Cells["E36"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].Inn_DB = Convert.ToString(worksheet.Cells["F36"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].Kpp_DB = Convert.ToString(worksheet.Cells["G36"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].Okopf_DB = Convert.ToString(worksheet.Cells["H36"].Value);
                    newRepsFromExcel.Master_DB.Rows20[0].Okfs_DB = Convert.ToString(worksheet.Cells["I36"].Value);

                    newRepsFromExcel.Master_DB.Rows20[1].Okpo_DB = Convert.ToString(worksheet.Cells["B37"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].Okved_DB = Convert.ToString(worksheet.Cells["C37"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].Okogu_DB = Convert.ToString(worksheet.Cells["D37"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].Oktmo_DB = Convert.ToString(worksheet.Cells["E37"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].Inn_DB = Convert.ToString(worksheet.Cells["F37"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].Kpp_DB = Convert.ToString(worksheet.Cells["G37"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].Okopf_DB = Convert.ToString(worksheet.Cells["H37"].Value);
                    newRepsFromExcel.Master_DB.Rows20[1].Okfs_DB = Convert.ToString(worksheet.Cells["I37"].Value);
                    break;
                }
            case "Форма 3.0":
                {

                    newRepsFromExcel.Master_DB.Rows30[0].RegNo.Value = Convert.ToString(worksheet.Cells["B10"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].OrganUprav_DB = Convert.ToString(worksheet.Cells["B15"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].SubjectRF_DB = Convert.ToString(worksheet.Cells["C16"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].JurLico_DB = Convert.ToString(worksheet.Cells["C17"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].ShortJurLico_DB = Convert.ToString(worksheet.Cells["C18"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].JurLicoAddress_DB = Convert.ToString(worksheet.Cells["C19"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].JurLicoFactAddress_DB = Convert.ToString(worksheet.Cells["C20"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].GradeFIO_DB = Convert.ToString(worksheet.Cells["C21"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].Telephone_DB = Convert.ToString(worksheet.Cells["C22"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].Fax_DB = Convert.ToString(worksheet.Cells["C23"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].Email_DB = Convert.ToString(worksheet.Cells["C24"].Value);

                    newRepsFromExcel.Master_DB.Rows30[1].SubjectRF_DB = Convert.ToString(worksheet.Cells["C25"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].JurLico_DB = Convert.ToString(worksheet.Cells["C26"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].ShortJurLico_DB = Convert.ToString(worksheet.Cells["C27"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].JurLicoAddress_DB = Convert.ToString(worksheet.Cells["C28"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].JurLicoFactAddress_DB = Convert.ToString(worksheet.Cells["C28"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].GradeFIO_DB = Convert.ToString(worksheet.Cells["C29"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].Telephone_DB = Convert.ToString(worksheet.Cells["C30"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].Fax_DB = Convert.ToString(worksheet.Cells["C31"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].Email_DB = Convert.ToString(worksheet.Cells["C32"].Value);

                    newRepsFromExcel.Master_DB.Rows30[0].Okpo_DB = Convert.ToString(worksheet.Cells["B36"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].Okved_DB = Convert.ToString(worksheet.Cells["C36"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].Okogu_DB = Convert.ToString(worksheet.Cells["D36"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].Oktmo_DB = Convert.ToString(worksheet.Cells["E36"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].Inn_DB = Convert.ToString(worksheet.Cells["F36"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].Kpp_DB = Convert.ToString(worksheet.Cells["G36"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].Okopf_DB = Convert.ToString(worksheet.Cells["H36"].Value);
                    newRepsFromExcel.Master_DB.Rows30[0].Okfs_DB = Convert.ToString(worksheet.Cells["I36"].Value);

                    newRepsFromExcel.Master_DB.Rows30[1].Okpo_DB = Convert.ToString(worksheet.Cells["B37"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].Okved_DB = Convert.ToString(worksheet.Cells["C37"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].Okogu_DB = Convert.ToString(worksheet.Cells["D37"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].Oktmo_DB = Convert.ToString(worksheet.Cells["E37"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].Inn_DB = Convert.ToString(worksheet.Cells["F37"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].Kpp_DB = Convert.ToString(worksheet.Cells["G37"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].Okopf_DB = Convert.ToString(worksheet.Cells["H37"].Value);
                    newRepsFromExcel.Master_DB.Rows30[1].Okfs_DB = Convert.ToString(worksheet.Cells["I37"].Value);
                    break;
                }
            case "Форма 4.0":
                {
                    var form40 = newRepsFromExcel.Master_DB.Rows40[0];

                    form40.CodeSubjectRF_DB = Truncate(Convert.ToString(worksheet.Cells["B8"].Value), 2);
                    form40.SubjectRF_DB = Truncate(Convert.ToString(worksheet.Cells["B9"].Value), 64);
                    form40.NameOrganUprav_DB = Truncate(Convert.ToString(worksheet.Cells["B19"].Value), 256);
                    form40.ShortNameOrganUprav_DB = Truncate(Convert.ToString(worksheet.Cells["B20"].Value), 256);
                    form40.AddressOrganUprav_DB = Truncate(Convert.ToString(worksheet.Cells["B21"].Value), 256);
                    form40.GradeFioDirectorOrganUprav_DB = Truncate(Convert.ToString(worksheet.Cells["B22"].Value), 256);
                    form40.GradeFioExecutorOrganUprav_DB = Truncate(Convert.ToString(worksheet.Cells["B23"].Value), 64);
                    form40.TelephoneOrganUprav_DB = Truncate(Convert.ToString(worksheet.Cells["B24"].Value), 64);
                    form40.FaxOrganUprav_DB = Truncate(Convert.ToString(worksheet.Cells["B25"].Value), 64);
                    form40.EmailOrganUprav_DB = Truncate(Convert.ToString(worksheet.Cells["B26"].Value), 256);

                    form40.NameRiac_DB = Truncate(Convert.ToString(worksheet.Cells["B28"].Value), 256);
                    form40.ShortNameRiac_DB = Truncate(Convert.ToString(worksheet.Cells["B29"].Value), 256);
                    form40.AddressRiac_DB = Truncate(Convert.ToString(worksheet.Cells["B30"].Value), 256);
                    form40.GradeFioDirectorRiac_DB = Truncate(Convert.ToString(worksheet.Cells["B31"].Value), 256);
                    form40.GradeFioExecutorRiac_DB = Truncate(Convert.ToString(worksheet.Cells["B32"].Value), 256);
                    form40.TelephoneRiac_DB = Truncate(Convert.ToString(worksheet.Cells["B33"].Value), 64);
                    form40.FaxRiac_DB = Truncate(Convert.ToString(worksheet.Cells["B34"].Value), 64);
                    form40.EmailRiac_DB = Truncate(Convert.ToString(worksheet.Cells["B35"].Value), 256);

                    break;
                }
            case "Форма 5.0":
                {
                    var form50 = newRepsFromExcel.Master_DB.Rows50[0];

                    form50.ExecutiveAuthority_DB = Truncate(Convert.ToString(worksheet.Cells["A9"].Value), 256);
                    form50.Name_DB = Truncate(Convert.ToString(worksheet.Cells["B20"].Value), 256);
                    form50.ShortName_DB = Truncate(Convert.ToString(worksheet.Cells["B21"].Value), 256);
                    form50.Address_DB = Truncate(Convert.ToString(worksheet.Cells["B22"].Value), 256);
                    form50.GradeFioDirector_DB = Truncate(Convert.ToString(worksheet.Cells["B23"].Value), 256);
                    form50.GradeFioExecutor_DB = Truncate(Convert.ToString(worksheet.Cells["B24"].Value), 64);
                    form50.Telephone_DB = Truncate(Convert.ToString(worksheet.Cells["B25"].Value), 64);
                    form50.Fax_DB = Truncate(Convert.ToString(worksheet.Cells["B26"].Value), 64);
                    form50.Email_DB = Truncate(Convert.ToString(worksheet.Cells["B27"].Value), 256);

                    break;
                }
        }
    }

    #endregion

    #region GetReport3xFromExcel
    private Report? GetReport3xFromExcel(ExcelWorksheet worksheet0, ExcelWorksheet worksheet1, List<string> timeCreate)
    {

        if (worksheet1 is null) return null;



        var formNumber = worksheet1.Name;
        // В некоторых шаблонах в наименовании листа Excel перед номером формы добавляется слово "Форма". Например "Форма 4.1"
        // а в других просто пишется номер формы. Например "1.1"
        if (formNumber.ToLower().StartsWith("форма "))
            formNumber = formNumber.Split(' ')[1];

        //Импортируем отчет
        var impRep = GetReportWithDataFromExcel(worksheet0, worksheet1, formNumber, timeCreate);
        impRep.ReportChangedDate = DateTime.Now;

        // Объявляем вспомогательные переменные, которые будем использовать далее
        DateOnly dateOnlyValue;
        double doubleValue;
        int intValue;
        string address;
        int index;

        switch (formNumber)
        {
            case "3.1":
                impRep.Rows31One.RecipientName_DB = worksheet1.Cells["C12"].Text;
                impRep.Rows31One.RecipientJurLicoAddress_DB = worksheet1.Cells["C13"].Text;
                impRep.Rows31One.RecipientWorkplaceAddress_DB = worksheet1.Cells["C14"].Text;
                impRep.Rows31One.LicenseNum_DB = worksheet1.Cells["C15"].Text;
                impRep.Rows31One.ValidityPeriod_DB = DateOnly.TryParse(worksheet1.Cells["C16"].Text, out dateOnlyValue)
                    ? dateOnlyValue
                    : null;
                impRep.Rows31One.ExpectedDecisionTimeframe_DB = DateOnly.TryParse(worksheet1.Cells["C17"].Text, out dateOnlyValue)
                    ? dateOnlyValue
                    : null;
                impRep.Rows31One.FinalUserName_DB = worksheet1.Cells["C19"].Text;
                impRep.Rows31One.FinalUserJurLicoAddress_DB = worksheet1.Cells["C20"].Text;
                impRep.Rows31One.FinalUserWorkplaceAddress_DB = worksheet1.Cells["C21"].Text;
                impRep.Rows31One.FinalUserTelephone_DB = worksheet1.Cells["C22"].Text;
                impRep.Rows31One.FinalUserEmail_DB = worksheet1.Cells["C23"].Text;
                impRep.Rows31One.ApplicationScope_DB = worksheet1.Cells["C24"].Text;

                impRep.Rows31One.ContractNum_DB = worksheet1.Cells["C26"].Text;
                impRep.Rows31One.ContractDate_DB = DateOnly.TryParse(worksheet1.Cells["C27"].Text, out dateOnlyValue)
                    ? dateOnlyValue
                    : null;
                impRep.Rows31One.ManufacturerOksm_DB = worksheet1.Cells["C28"].Text;


                #region Таблица Характеристики экспортируемых ЗРИ/ОЗИИИ:
                address = worksheet1.Cells.FirstOrDefault(cell => Convert.ToString(cell.Value).ToLower() == "характеристики экспортируемых зри/озиии:").LocalAddress;
                address = address.Remove(0, 1);
                int.TryParse(address, out index);
                index += 2;

                while (worksheet1.Cells[$"A{index}"].Text.Trim().ToLower() != "примечания"
                    && (!string.IsNullOrWhiteSpace(worksheet1.Cells[$"A{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"B{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"C{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"D{index}"].Text)))
                {
                    var a = worksheet1.Cells[$"A{index}"].Text;
                    var b = worksheet1.Cells[$"B{index}"].Text;
                    var c = worksheet1.Cells[$"C{index}"].Text;
                    var d = worksheet1.Cells[$"D{index}"].Text;

                    var flag = !string.IsNullOrWhiteSpace(a)
                    && !string.IsNullOrWhiteSpace(b)
                    && !string.IsNullOrWhiteSpace(c)
                    && !string.IsNullOrWhiteSpace(d);
                    impRep.Rows31One.ExportedZriOziiiInfoCollection.Add(
                        new Form31ExportedZriOziiiInfo()
                        {
                            RadionuclidComposition = worksheet1.Cells[$"B{index}"].Text,
                            Count = int.TryParse(worksheet1.Cells[$"C{index}"].Text, out intValue)
                                ? intValue
                                : null,
                            TotalActivity = double.TryParse(worksheet1.Cells[$"D{index}"].Text, out doubleValue)
                                ? doubleValue
                                : null,
                        });

                    index++;
                }
                #endregion

                break;
            case "3.2":

                impRep.Rows32One.AgreementIdNum_DB = worksheet1.Cells["C12"].Text;
                impRep.Rows32One.DeliveryDay_DB = DateOnly.TryParse(worksheet1.Cells["C13"].Text, out dateOnlyValue)
                    ? dateOnlyValue
                    : null;
                impRep.Rows32One.RecipientName_DB = worksheet1.Cells["C15"].Text;

                impRep.Rows32One.IsRvProduction_DB = !string.IsNullOrWhiteSpace(worksheet1.Cells["C17"].Text);
                impRep.Rows32One.IsRvTransportation_DB = !string.IsNullOrWhiteSpace(worksheet1.Cells["C18"].Text);
                impRep.Rows32One.IsRvExploitation_DB = !string.IsNullOrWhiteSpace(worksheet1.Cells["C19"].Text);
                impRep.Rows32One.IsRvStoring_DB = !string.IsNullOrWhiteSpace(worksheet1.Cells["C20"].Text);
                impRep.Rows32One.IsRvRecycling_DB = !string.IsNullOrWhiteSpace(worksheet1.Cells["C21"].Text);

                impRep.Rows32One.IsRaoTransportation_DB = !string.IsNullOrWhiteSpace(worksheet1.Cells["C23"].Text);
                impRep.Rows32One.IsRaoStoring_DB = !string.IsNullOrWhiteSpace(worksheet1.Cells["C24"].Text);
                impRep.Rows32One.IsRaoRecycling_DB = !string.IsNullOrWhiteSpace(worksheet1.Cells["C25"].Text);

                impRep.Rows32One.LicenseNumRv_DB = worksheet1.Cells["C27"].Text;
                impRep.Rows32One.LicenseNumRao_DB = worksheet1.Cells["C28"].Text;

                impRep.Rows32One.LicenseExpirationDateRv_DB = DateOnly.TryParse(worksheet1.Cells["C30"].Text, out dateOnlyValue)
                    ? dateOnlyValue
                    : null;
                impRep.Rows32One.LicenseExpirationDateRao_DB = DateOnly.TryParse(worksheet1.Cells["C31"].Text, out dateOnlyValue)
                    ? dateOnlyValue
                    : null;
                impRep.Rows32One.DeliveryAddress_DB = worksheet1.Cells["C32"].Text;
                impRep.Rows32One.RadionuclidCompositionZri_DB = worksheet1.Cells["C33"].Text;
                impRep.Rows32One.TotalActivity_DB = double.TryParse(worksheet1.Cells["C34"].Text, out doubleValue)
                    ? doubleValue
                    : null;
                impRep.Rows32One.TotalCount_DB = int.TryParse(worksheet1.Cells["C35"].Text, out intValue)
                    ? intValue
                    : null;

                #region Таблица Сведения о поставляемых ЗРИ
                address = worksheet1.Cells.FirstOrDefault(cell => cell.Text.ToLower() == "сведения о поставляемых зри").LocalAddress;
                address = address.Remove(0, 1);
                int.TryParse(address, out index);
                index += 3;

                while (worksheet1.Cells[$"A{index}"].Text.Trim().ToLower() != "9"
                    && (!string.IsNullOrWhiteSpace(worksheet1.Cells[$"A{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"B{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"C{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"D{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"E{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"F{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"G{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"H{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"I{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"J{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"K{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"L{index}"].Text)))
                {
                    impRep.Rows32One.ExportedZriInfoCollection.Add(
                        new Form32ExportedZriInfo()
                        {
                            PassportNum = worksheet1.Cells[$"B{index}"].Text,
                            Type = worksheet1.Cells[$"C{index}"].Text,
                            FactoryNum = worksheet1.Cells[$"D{index}"].Text,
                            RadionuclidComposition = worksheet1.Cells[$"E{index}"].Text,
                            ReleaseDate = DateOnly.TryParse(worksheet1.Cells[$"F{index}"].Text, out dateOnlyValue)
                                ? dateOnlyValue
                                : null,
                            ActivityOnRealeseDate = worksheet1.Cells[$"G{index}"].Text,
                            NuclearMaterials = worksheet1.Cells[$"H{index}"].Text,
                            Category = worksheet1.Cells[$"I{index}"].Text,
                            ManufacturerOksm = worksheet1.Cells[$"J{index}"].Text,
                            CertificateNum = worksheet1.Cells[$"K{index}"].Text,
                            CertificateExpirationDate = DateOnly.TryParse(worksheet1.Cells[$"L{index}"].Text, out dateOnlyValue)
                                ? dateOnlyValue
                                : null,
                        });

                    index++;
                }
                #endregion

                #region Таблица Сведения о контейнере, приборе, установке
                address = worksheet1.Cells.FirstOrDefault(cell => cell.Text.ToLower() == "сведения о контейнере, приборе, установке").LocalAddress;//
                address = address.Remove(0, 1);
                int.TryParse(address, out index);
                index += 3;

                while (worksheet1.Cells[$"A{index}"].Text.Trim().ToLower() != "10"
                    && (!string.IsNullOrWhiteSpace(worksheet1.Cells[$"A{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"B{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"C{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"D{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"E{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"F{index}"].Text)))
                {
                    impRep.Rows32One.ContainersInfoCollection.Add(
                        new Form32ContainerInfo()
                        {
                            Name = worksheet1.Cells[$"B{index}"].Text,
                            Type = worksheet1.Cells[$"C{index}"].Text,
                            IdNum = worksheet1.Cells[$"D{index}"].Text,
                            ReleaseYear = int.TryParse(worksheet1.Cells[$"E{index}"].Text, out intValue)
                                ? intValue
                                : null,
                            DepletedUraniumMass = double.TryParse(worksheet1.Cells[$"F{index}"].Text, out doubleValue)
                                ? doubleValue
                                : null,
                        });

                    index++;
                }
                #endregion

                #region Таблица Идентификаторы
                address = worksheet1.Cells.FirstOrDefault(cell => cell.Text.ToLower() == "идентификаторы").LocalAddress;
                address = address.Remove(0, 1);
                int.TryParse(address, out index);
                index += 3;

                while (worksheet1.Cells[$"A{index}"].Text.Trim().ToLower() != "примечания"
                    && (!string.IsNullOrWhiteSpace(worksheet1.Cells[$"A{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"B{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"C{index}"].Text)))
                {
                    impRep.Rows32One.IdentificatorsCollection.Add(
                        new Form32Identificator()
                        {
                            IdName = worksheet1.Cells[$"B{index}"].Text,
                            IdValue = worksheet1.Cells[$"C{index}"].Text,
                        });

                    index++;
                }
                #endregion

                break;

        }


        // Импортируем примечания
        #region Таблица Идентификаторы
        address = worksheet1.Cells.FirstOrDefault(cell => Convert.ToString(cell.Value).ToLower() == "примечания").LocalAddress;
        address = address.Remove(0, 1);
        int.TryParse(address, out index);
        index += 2;

        while (worksheet1.Cells[$"A{index}"].Text.Trim().ToLower() != "исполнитель"
                    && (!string.IsNullOrWhiteSpace(worksheet1.Cells[$"A{index}"].Text)
                    || !string.IsNullOrWhiteSpace(worksheet1.Cells[$"B{index}"].Text)))
        {
            impRep.Notes.Add(
                new Note()
                {
                    RowNumber_DB = worksheet1.Cells[$"A{index}"].Text,
                    Comment_DB = worksheet1.Cells[$"B{index}"].Text,
                });

            index++;
        }
        #endregion

        return impRep;

    }
    #endregion

    #region GetReportFromExcelFile
    private Report? GetReportFromExcel(ExcelWorksheet worksheet0, ExcelWorksheet worksheet1, List<string> timeCreate)
    {

        if (worksheet1 is null) return null;



        var formNumber = worksheet1.Name;
        // В некоторых шаблонах в наименовании листа Excel перед номером формы добавляется слово "Форма". Например "Форма 4.1"
        // а в других просто пишется номер формы. Например "1.1"
        if (formNumber.ToLower().StartsWith("форма "))
            formNumber = formNumber.Split(' ')[1];

        //Импортируем отчет
        var impRep = GetReportWithDataFromExcel(worksheet0, worksheet1, formNumber, timeCreate);
        impRep.ReportChangedDate = DateTime.Now;

        var start = formNumber switch
        {
            "2.8" => 14,
            "4.1" => 9,
            "5.1" or "5.2" or "5.3" or "5.4" or "5.5" or "5.6" or "5.7" => 12,
            _ => 11
        };

        var end = $"A{start}";
        var value = worksheet1.Cells[end].Value;

        while (value != null
               && Convert.ToString(value)?.ToLower() is not ("примечание:" or "примечания:" or "должность исполнителя"))
        {
            GetDataFromRow(formNumber, worksheet1, start, impRep);
            start++;
            end = $"A{start}";
            value = worksheet1.Cells[end].Value;
        }

        NumberInOrder = 1;

        while (value is null)
        {
            start += 1;
            end = $"A{start}";
            value = worksheet1.Cells[end].Value;
        }

        // Импортируем примечания
        // У форм 4.X нет примечаний

        if (formNumber[0] is '1' or '2' or '5' && formNumber is not "5.7")
        {
            if (Convert.ToString(value)?.ToLower() is "примечание:" or "примечания:")
            {
                start += 2;

                while (worksheet1.Cells[$"A{start}"].Value != null ||
                       worksheet1.Cells[$"B{start}"].Value != null ||
                       worksheet1.Cells[$"C{start}"].Value != null)
                {
                    Note newNote = new();
                    newNote.ExcelGetRow(worksheet1, start);
                    impRep.Notes.Add(newNote);
                    start++;
                }
            }
        }
        return impRep;

    }
    #endregion

    #region GetImportReps

    private static Reports GetImportReps(ExcelWorksheet worksheet)
    {
        var formNum = worksheet.Name;
        if (formNum.ToLower().StartsWith("форма "))
        {
            formNum = formNum.Split(' ')[1];
        }
        var newRepsFromExcel = new Reports
        {
            Master_DB = new Report
            {
                FormNum_DB = formNum
            }
        };
        switch (formNum)
        {
            case "1.0":
                {
                    var ty1 = (Form10)FormCreator.Create(formNum);
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form10)FormCreator.Create(formNum);
                    ty2.NumberInOrder_DB = 2;
                    newRepsFromExcel.Master_DB.Rows10.Add(ty1);
                    newRepsFromExcel.Master_DB.Rows10.Add(ty2);
                    break;
                }
            case "2.0":
                {
                    var ty1 = (Form20)FormCreator.Create(formNum);
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form20)FormCreator.Create(formNum);
                    ty2.NumberInOrder_DB = 2;
                    newRepsFromExcel.Master_DB.Rows20.Add(ty1);
                    newRepsFromExcel.Master_DB.Rows20.Add(ty2);
                    break;
                }
            case "3.0":
                {
                    var ty1 = (Form30)FormCreator.Create(formNum);
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form30)FormCreator.Create(formNum);
                    ty2.NumberInOrder_DB = 2;
                    newRepsFromExcel.Master_DB.Rows30.Add(ty1);
                    newRepsFromExcel.Master_DB.Rows30.Add(ty2);
                    break;
                }
            case "4.0":
                {
                    var row40 = (Form40)FormCreator.Create(formNum);
                    row40.NumberInOrder_DB = 1;

                    newRepsFromExcel.Master_DB.Rows40.Add(row40);
                    break;
                }
            case "5.0":
                {
                    var row50 = (Form50)FormCreator.Create(formNum);
                    row50.NumberInOrder_DB = 1;

                    newRepsFromExcel.Master_DB.Rows50.Add(row50);
                    break;
                }
        }
        GetDataTitleReps(newRepsFromExcel, worksheet);
        //ReportsStorage.LocalReports.Reports_Collection.Add(newRepsFromExcel);
        return newRepsFromExcel;
    }

    /// <summary>
    /// Обрезает строку до указанной длины
    /// </summary>
    /// <param name="value">Исходная строка</param>
    /// <param name="maxLength">Максимальная длина</param>
    /// <returns>Обрезанная строка или null/empty если входная строка была null/empty</returns>
    private static string? Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;

        return value.Length > maxLength ? value[..maxLength] : value;
    }

    #endregion


   


#region GetReportDataFromExcel

private static Report GetReportWithDataFromExcel(ExcelWorksheet worksheet0, ExcelWorksheet worksheet1, string formNumber, List<string> timeCreate)
    {
        var impRep = new Report
        {
            FormNum_DB = formNumber,
            ExportDate_DB = $"{timeCreate[0]}.{timeCreate[1]}.{timeCreate[2]}"
        };
        if (formNumber.Split('.')[0] == "1")
        {
            #region BindData_1.x

            impRep.StartPeriod_DB = Convert.ToString(worksheet1.Cells["G3"].Text).Replace("/", ".");
            impRep.EndPeriod_DB = Convert.ToString(worksheet1.Cells["G4"].Text).Replace("/", ".");
            impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G5"].Value);

            #endregion
        }
        else if (formNumber.Split('.')[0] == "2")
        {
            switch (formNumber)
            {
                case "2.6":
                    {
                        #region BindData_26

                        impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G4"].Value);
                        impRep.SourcesQuantity26_DB = Convert.ToInt32(worksheet1.Cells["G5"].Value);
                        impRep.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Value);

                        #endregion

                        break;
                    }
                case "2.7":
                    {
                        #region BindData_27

                        impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G3"].Value);
                        impRep.PermissionNumber27_DB = Convert.ToString(worksheet1.Cells["G4"].Value);
                        impRep.PermissionIssueDate27_DB = Convert.ToString(worksheet1.Cells["J4"].Value);
                        impRep.ValidBegin27_DB = Convert.ToString(worksheet1.Cells["G5"].Value);
                        impRep.ValidThru27_DB = Convert.ToString(worksheet1.Cells["J5"].Value);
                        impRep.PermissionDocumentName27_DB = Convert.ToString(worksheet1.Cells["G6"].Value);
                        impRep.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Value);

                        #endregion

                        break;
                    }
                case "2.8":
                    {
                        #region BindData_28

                        impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G3"].Value);
                        impRep.PermissionNumber_28_DB = Convert.ToString(worksheet1.Cells["G4"].Value);
                        impRep.PermissionIssueDate_28_DB = Convert.ToString(worksheet1.Cells["K5"].Value);
                        impRep.ValidBegin_28_DB = Convert.ToString(worksheet1.Cells["K4"].Value);
                        impRep.ValidThru_28_DB = Convert.ToString(worksheet1.Cells["N4"].Value);
                        impRep.PermissionDocumentName_28_DB = Convert.ToString(worksheet1.Cells["G5"].Value);

                        impRep.PermissionNumber1_28_DB = Convert.ToString(worksheet1.Cells["G6"].Value);
                        impRep.PermissionIssueDate1_28_DB = Convert.ToString(worksheet1.Cells["K7"].Value);
                        impRep.ValidBegin1_28_DB = Convert.ToString(worksheet1.Cells["K6"].Value);
                        impRep.ValidThru1_28_DB = Convert.ToString(worksheet1.Cells["N6"].Value);
                        impRep.PermissionDocumentName1_28_DB = Convert.ToString(worksheet1.Cells["G7"].Value);

                        impRep.ContractNumber_28_DB = Convert.ToString(worksheet1.Cells["G8"].Value);
                        impRep.ContractIssueDate2_28_DB = Convert.ToString(worksheet1.Cells["K9"].Value);
                        impRep.ValidBegin2_28_DB = Convert.ToString(worksheet1.Cells["K8"].Value);
                        impRep.ValidThru2_28_DB = Convert.ToString(worksheet1.Cells["N8"].Value);
                        impRep.OrganisationReciever_28_DB = Convert.ToString(worksheet1.Cells["G9"].Value);

                        impRep.GradeExecutor_DB = Convert.ToString(worksheet1.Cells["D21"].Value);
                        impRep.FIOexecutor_DB = Convert.ToString(worksheet1.Cells["F21"].Value);
                        impRep.ExecPhone_DB = Convert.ToString(worksheet1.Cells["I21"].Value);
                        impRep.ExecEmail_DB = Convert.ToString(worksheet1.Cells["K21"].Value);
                        impRep.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Value);

                        #endregion

                        break;
                    }
                default:
                    {
                        #region BindData_2.x

                        impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G4"].Value);
                        impRep.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Text);

                        #endregion

                        break;
                    }
            }
        }
        else if (formNumber.Split('.')[0] == "3")
        {
            if (formNumber is "3.1")
            {
                impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["B7"].Value);
                impRep.StartPeriod_DB = Convert.ToString(worksheet1.Cells["B9"].Text).Replace("/", ".");
            }
            else if (formNumber is "3.2")
            {
                impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["B8"].Value);
                impRep.StartPeriod_DB = Convert.ToString(worksheet1.Cells["B10"].Text).Replace("/", ".");
            }
        }
        else if (formNumber.Split('.')[0] == "4")
        {
            impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["B1"].Value);
            impRep.Year_DB = Convert.ToString(worksheet0.Cells["B15"].Text).Trim();
            //Отсекаем мусор из ячейки
            if (!impRep.Year_DB.All(c => char.IsDigit(c)))
            {
                var digits = "";
                foreach (var c in impRep.Year_DB)
                {
                    if (char.IsDigit(c))
                        digits += c;
                }
                impRep.Year_DB = digits;
            }
        }
        else if (formNumber.Split('.')[0] == "5")
        {
            impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["B7"].Value);
            impRep.Year_DB = Convert.ToString(worksheet0.Cells["B16"].Text).Trim();
            //Отсекаем мусор из ячейки
            if (!impRep.Year_DB.All(c => char.IsDigit(c)))
            {
                var digits = "";
                foreach (var c in impRep.Year_DB)
                {
                    if (char.IsDigit(c))
                        digits += c;
                }
                impRep.Year_DB = digits;
            }
        }

        #region BindCommonData

        if (formNumber.Split('.')[0] is "1" or "2")
        {
            impRep.GradeExecutor_DB = Convert.ToString(worksheet1.Cells[$"D{worksheet1.Dimension.Rows - 1}"].Value);
            impRep.FIOexecutor_DB = Convert.ToString(worksheet1.Cells[$"F{worksheet1.Dimension.Rows - 1}"].Value);
            impRep.ExecPhone_DB = Convert.ToString(worksheet1.Cells[$"I{worksheet1.Dimension.Rows - 1}"].Value);
            impRep.ExecEmail_DB = Convert.ToString(worksheet1.Cells[$"K{worksheet1.Dimension.Rows - 1}"].Value);
        }
        else if (formNumber.Split('.')[0] is "3")
        {
            var address = worksheet1.Cells.FirstOrDefault(cell => cell.Text.ToLower() == "должность").LocalAddress;
            address = address.Remove(0, 1);
            int.TryParse(address, out var index);

            impRep.GradeExecutor_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
            index++;
            impRep.FIOexecutor_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
            index++;
            impRep.ExecPhone_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
            index++;
            impRep.ExecEmail_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
        }
        else if (formNumber.Split('.')[0] is "4")
        {
            var address = worksheet1.Cells.FirstOrDefault(cell => cell.Text.ToLower() == "должность исполнителя").LocalAddress;
            address = address.Remove(0, 1);
            int.TryParse(address, out var index);

            impRep.GradeExecutor_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
            index++;
            impRep.FIOexecutor_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
            index++;
            impRep.ExecPhone_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
            index++;
            impRep.ExecEmail_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
        }
        else if (formNumber.Split('.')[0] is "5")
        {
            var address = worksheet1.Cells.FirstOrDefault(cell => cell.Text.ToLower() == "должность").LocalAddress;
            address = address.Remove(0, 1);
            int.TryParse(address, out var index);

            impRep.GradeExecutor_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
            index++;
            impRep.FIOexecutor_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
            index++;
            impRep.ExecPhone_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
            index++;
            impRep.ExecEmail_DB = Convert.ToString(worksheet1.Cells[$"B{index}"].Value);
        }

        #endregion

        return impRep;
    }

    #endregion
}