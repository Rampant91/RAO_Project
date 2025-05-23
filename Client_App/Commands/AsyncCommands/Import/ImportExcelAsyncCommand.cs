using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Avalonia.Threading;
using Client_App.Resources.CustomComparers;

namespace Client_App.Commands.AsyncCommands.Import;

/// <summary>
/// Импорт -> Из Excel.
/// </summary>
internal class ImportExcelAsyncCommand : ImportBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        RepsWhereTitleFormCheckIsCancel.Clear();
        IsFirstLogLine = true;
        CurrentLogLine = 1;
        var readAnyExcel = false;
        string[] extensions = ["xlsx", "XLSX"];
        var answer = await GetSelectedFilesFromDialog("Excel", extensions);
        if (answer is null) return;

        SkipNewOrg = false;
        SkipInter = false;
        SkipLess = false;
        SkipNew = false;
        SkipReplace = false;
        HasMultipleReport = false;
        AtLeastOneImportDone = false;

        var impReportsList = new List<Reports>();
        foreach (var res in answer) // Для каждого импортируемого файла
        {
            ExcelImportNewReps = false;
            if (res is "") continue;
            SourceFile = new FileInfo(res);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage excelPackage = new(SourceFile);
            var worksheet0 = excelPackage.Workbook.Worksheets[0];
            var val = worksheet0.Name == "1.0"
                      && Convert.ToString(worksheet0.Cells["A3"].Value)
                          is "ГОСУДАОСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"
                          or "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"
                      || worksheet0.Name == "2.0"
                      && Convert.ToString(worksheet0.Cells["A4"].Value)
                          is "ГОСУДАОСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"
                          or "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ";
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
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                continue;
            }
            readAnyExcel = true;

            var timeCreate = new List<string>()
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

            var baseReps = GetBaseReps(worksheet0);
            var impReps = GetImportReps(worksheet0);
            impReportsList.Add(impReps);
            if (baseReps is null)
            {
                ExcelImportNewReps = true;
                baseReps = impReps;
            }
            
            BaseRepsOkpo = baseReps.Master.OkpoRep.Value;
            BaseRepsRegNum = baseReps.Master.RegNoRep.Value;
            BaseRepsShortName = baseReps.Master.ShortJurLicoRep.Value;

            var worksheet1 = excelPackage.Workbook.Worksheets[1];
            var repNumber = worksheet0.Name;
            var formNumber = worksheet1.Name;

            var impRep = GetReportWithDataFromExcel(worksheet0, worksheet1, formNumber, timeCreate);

            var start = formNumber is "2.8"
                ? 14
                : 11;
            var end = $"A{start}";
            var value = worksheet1.Cells[end].Value;

            while (value != null && Convert.ToString(value)?.ToLower() is not ("примечание:" or "примечания:"))
            {
                GetDataFromRow(formNumber, worksheet1, start, impRep);
                start++;
                end = $"A{start}";
                value = worksheet1.Cells[end].Value;
            }

            if (value is null)
                start += 3;
            else if (Convert.ToString(value)?.ToLower() is "примечание:" or "примечания:")
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

            ImpRepCorNum = impRep.CorrectionNumber_DB;
            ImpRepEndPeriod = impRep.EndPeriod_DB;
            ImpRepFormCount = impRep.Rows.Count;
            ImpRepFormNum = impRep.FormNum_DB;
            ImpRepStartPeriod = impRep.StartPeriod_DB;
            ImpRepYear = impRep.Year_DB ?? "";

            //SkipNewOrg = SkipInter = SkipLess = SkipNew = SkipReplace = AtLeastOneImportDone = false;
            HasMultipleReport = answer.Length > 1;

            var impRepList = new List<Report> { impRep };
            if (baseReps.Report_Collection.Count != 0)
            {
                switch (worksheet0.Name)
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
                }
            }
            else
            {
                #region AddNewOrg

                var an = "Добавить";
                if (!SkipNewOrg)
                {
                    if (answer.Length > 1)
                    {
                        #region MessageNewOrg

                        an = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions =
                                [
                                    new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                    new ButtonDefinition { Name = "Да для всех" },
                                    new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                ],
                                ContentTitle = "Импорт из .xlsx",
                                ContentHeader = "Уведомление",
                                ContentMessage =
                                    $"Будет добавлена новая организация ({repNumber}), содержащая отчет по форме {ImpRepFormNum}." +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                    $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Кнопка \"Да для всех\" позволяет без уведомлений " +
                                    $"{Environment.NewLine}импортировать все новые организации.",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(Desktop.MainWindow));

                        #endregion

                        if (an is "Да для всех") SkipNewOrg = true;
                    }
                    else
                    {
                        #region MessageNewOrg

                        an = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions =
                                [
                                    new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                    new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                ],
                                ContentTitle = "Импорт из .xlsx",
                                ContentHeader = "Уведомление",
                                ContentMessage = $"Будет добавлена новая организация ({repNumber})." +
                                                 $"{Environment.NewLine}" +
                                                 $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                                 $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                                 $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(Desktop.MainWindow));

                        #endregion
                    }
                }

                await CheckAnswer(an, baseReps, impReps, null, impRep);

                #endregion
            }
        }

        var comparator = new CustomReportsComparer();
        var tmpReportsList = new List<Reports>(ReportsStorage.LocalReports.Reports_Collection);
        ReportsStorage.LocalReports.Reports_Collection.Clear();
        ReportsStorage.LocalReports.Reports_Collection
            .AddRange(tmpReportsList
                .OrderBy(x => x.Master_DB.RegNoRep.Value, comparator)
                .ThenBy(x => x.Master_DB.OkpoRep.Value, comparator));

        //await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync();

        await StaticConfiguration.DBModel.SaveChangesAsync();

        await SetDataGridPage(impReportsList);

        var suffix = answer.Length.ToString().EndsWith('1') && !answer.Length.ToString().EndsWith("11")
                ? "а"
                : "ов";
        if (AtLeastOneImportDone && readAnyExcel)
        {
            #region MessageImportDone

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Импорт из .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Импорт из файл{suffix} .xlsx успешно завершен.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion
        }
        else
        {
            #region MessageImportCancel

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
    /// <param name="worksheet0">Лист Excel.</param>
    /// <returns></returns>
    private static Reports? GetBaseReps(ExcelWorksheet worksheet0)
    {
        var excelOkpo0 = Convert.ToString(worksheet0.Cells["B36"].Value);
        var excelOkpo1= Convert.ToString(worksheet0.Cells["B37"].Value);

        var excelRegNo = Convert.ToString(worksheet0.Cells["F6"].Value);

        return worksheet0.Name switch
        {
            "1.0" => ReportsStorage.LocalReports.Reports_Collection10
                         .FirstOrDefault(t =>
                         
                             // обособленные пусты и в базе и в импорте, то сверяем головное
                             excelOkpo0 == t.Master.Rows10[0].Okpo_DB
                             && excelRegNo == t.Master.Rows10[0].RegNo_DB
                             && excelOkpo1 == ""
                             && t.Master.Rows10[1].Okpo_DB == ""

                             // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                             || excelOkpo0 == t.Master.Rows10[0].Okpo_DB
                             && excelRegNo == t.Master.Rows10[1].RegNo_DB
                             && excelOkpo1 == ""
                             && t.Master.Rows10[1].Okpo_DB == ""

                             // обособленные не пусты, их и сверяем
                             || excelOkpo1 == t.Master.Rows10[1].Okpo_DB
                             && excelRegNo == t.Master.Rows10[1].RegNo_DB
                             && excelOkpo1 != ""

                             // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                             || excelOkpo1 == t.Master.Rows10[1].Okpo_DB
                             && excelRegNo == t.Master.Rows10[0].RegNo_DB
                             && excelOkpo1 != ""
                             && t.Master.Rows10[1].RegNo_DB == "")

                     ?? ReportsStorage.LocalReports
                         .Reports_Collection10 // если null, то ищем сбитый окпо (совпадение юр лица с обособленным)
                         .FirstOrDefault(t =>

                             // юр лицо в базе совпадает с обособленным в импорте
                             excelOkpo1 != ""
                             && t.Master.Rows10[1].Okpo_DB == ""
                             && excelOkpo1 == t.Master.Rows10[0].Okpo_DB
                             && excelRegNo == t.Master.Rows10[0].RegNo_DB

                             // юр лицо в импорте совпадает с обособленным в базе
                             || excelOkpo1 == ""
                             && t.Master.Rows10[1].Okpo_DB != ""
                             && excelOkpo0 == t.Master.Rows10[1].Okpo_DB
                             && excelRegNo == t.Master.Rows10[1].RegNo_DB),

            "2.0" => ReportsStorage.LocalReports.Reports_Collection20
                       .FirstOrDefault(t =>

                           // обособленные пусты и в базе и в импорте, то сверяем головное
                           excelOkpo0 == t.Master.Rows20[0].Okpo_DB
                           && excelRegNo == t.Master.Rows20[0].RegNo_DB
                           && excelOkpo1 == ""
                           && t.Master.Rows20[1].Okpo_DB == ""

                           // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || excelOkpo0 == t.Master.Rows20[0].Okpo_DB
                           && excelRegNo == t.Master.Rows20[1].RegNo_DB
                           && excelOkpo1 == ""
                           && t.Master.Rows20[1].Okpo_DB == ""

                           // обособленные не пусты, их и сверяем
                           || excelOkpo1 == t.Master.Rows20[1].Okpo_DB
                           && excelRegNo == t.Master.Rows20[1].RegNo_DB
                           && excelOkpo1 != ""

                           // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || excelOkpo1 == t.Master.Rows20[1].Okpo_DB
                           && excelRegNo == t.Master.Rows20[0].RegNo_DB
                           && excelOkpo1 != ""
                           && t.Master.Rows20[1].RegNo_DB == "")

                   ?? ReportsStorage.LocalReports.Reports_Collection20 // если null, то ищем сбитый окпо (совпадение юр лица с обособленным)
                       .FirstOrDefault(t =>

                           // юр лицо в базе совпадает с обособленным в импорте
                           excelOkpo1 != ""
                           && t.Master.Rows20[1].Okpo_DB == ""
                           && excelOkpo1 == t.Master.Rows20[0].Okpo_DB
                           && excelRegNo == t.Master.Rows20[0].RegNo_DB

                           // юр лицо в импорте совпадает с обособленным в базе
                           || excelOkpo1 == ""
                           && t.Master.Rows20[1].Okpo_DB != ""
                           && excelOkpo0 == t.Master.Rows20[1].Okpo_DB
                           && excelRegNo == t.Master.Rows20[1].RegNo_DB),

            _ => null
        };
    }

    #endregion

    #region GetDataFromRow

    private static void GetDataFromRow(string param1, ExcelWorksheet worksheet1, int start, Report repFromEx)
    {
        if (param1 is "2.1" or "2.2" && !int.TryParse(Convert.ToString(worksheet1.Cells[$"A{start}"].Value), out _)) return;
        dynamic form = FormCreator.Create(param1);
        form.ExcelGetRow(worksheet1, start);
        repFromEx.Rows.Add(form);
    }

    #endregion

    #region GetDataTitleReps

    private static void GetDataTitleReps(Reports newRepsFromExcel, ExcelWorksheet worksheet0)
    {
        switch (worksheet0.Name)
        {
            case "1.0":
                newRepsFromExcel.Master_DB.Rows10[0].RegNo_DB = Convert.ToString(worksheet0.Cells["F6"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].OrganUprav_DB = Convert.ToString(worksheet0.Cells["F15"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].SubjectRF_DB = Convert.ToString(worksheet0.Cells["F16"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].JurLico_DB = Convert.ToString(worksheet0.Cells["F17"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].ShortJurLico_DB = worksheet0.Cells["F18"].Value == null
                    ? ""
                    : Convert.ToString(worksheet0.Cells["F18"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].JurLicoAddress_DB = Convert.ToString(worksheet0.Cells["F19"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].JurLicoFactAddress_DB = Convert.ToString(worksheet0.Cells["F20"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].GradeFIO_DB = Convert.ToString(worksheet0.Cells["F21"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Telephone_DB = Convert.ToString(worksheet0.Cells["F22"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Fax_DB = Convert.ToString(worksheet0.Cells["F23"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Email_DB = Convert.ToString(worksheet0.Cells["F24"].Value);

                newRepsFromExcel.Master_DB.Rows10[1].SubjectRF_DB = Convert.ToString(worksheet0.Cells["F25"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].JurLico_DB = Convert.ToString(worksheet0.Cells["F26"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].ShortJurLico_DB = worksheet0.Cells["F27"].Value == null
                    ? ""
                    : Convert.ToString(worksheet0.Cells["F27"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].JurLicoAddress_DB = Convert.ToString(worksheet0.Cells["F28"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].JurLicoFactAddress_DB = Convert.ToString(worksheet0.Cells["F28"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].GradeFIO_DB = Convert.ToString(worksheet0.Cells["F29"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Telephone_DB = Convert.ToString(worksheet0.Cells["F30"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Fax_DB = Convert.ToString(worksheet0.Cells["F31"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Email_DB = Convert.ToString(worksheet0.Cells["F32"].Value);

                newRepsFromExcel.Master_DB.Rows10[0].Okpo_DB = worksheet0.Cells["B36"].Value == null
                    ? ""
                    : Convert.ToString(worksheet0.Cells["B36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okved_DB = Convert.ToString(worksheet0.Cells["C36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okogu_DB = Convert.ToString(worksheet0.Cells["D36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Oktmo_DB = Convert.ToString(worksheet0.Cells["E36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Inn_DB = Convert.ToString(worksheet0.Cells["F36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Kpp_DB = Convert.ToString(worksheet0.Cells["G36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okopf_DB = Convert.ToString(worksheet0.Cells["H36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okfs_DB = Convert.ToString(worksheet0.Cells["I36"].Value);

                newRepsFromExcel.Master_DB.Rows10[1].Okpo_DB = worksheet0.Cells["B37"].Value == null
                    ? ""
                    : Convert.ToString(worksheet0.Cells["B37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okved_DB = Convert.ToString(worksheet0.Cells["C37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okogu_DB = Convert.ToString(worksheet0.Cells["D37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Oktmo_DB = Convert.ToString(worksheet0.Cells["E37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Inn_DB = Convert.ToString(worksheet0.Cells["F37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Kpp_DB = Convert.ToString(worksheet0.Cells["G37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okopf_DB = Convert.ToString(worksheet0.Cells["H37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okfs_DB = Convert.ToString(worksheet0.Cells["I37"].Value);
                break;
            case "2.0":
                newRepsFromExcel.Master_DB.Rows20[0].RegNo.Value = Convert.ToString(worksheet0.Cells["F6"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].OrganUprav_DB = Convert.ToString(worksheet0.Cells["F15"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].SubjectRF_DB = Convert.ToString(worksheet0.Cells["F16"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].JurLico_DB = Convert.ToString(worksheet0.Cells["F17"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].ShortJurLico_DB = Convert.ToString(worksheet0.Cells["F18"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].JurLicoAddress_DB = Convert.ToString(worksheet0.Cells["F19"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].JurLicoFactAddress_DB = Convert.ToString(worksheet0.Cells["F20"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].GradeFIO_DB = Convert.ToString(worksheet0.Cells["F21"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Telephone_DB = Convert.ToString(worksheet0.Cells["F22"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Fax_DB = Convert.ToString(worksheet0.Cells["F23"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Email_DB = Convert.ToString(worksheet0.Cells["F24"].Value);

                newRepsFromExcel.Master_DB.Rows20[1].SubjectRF_DB = Convert.ToString(worksheet0.Cells["F25"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].JurLico_DB = Convert.ToString(worksheet0.Cells["F26"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].ShortJurLico_DB = Convert.ToString(worksheet0.Cells["F27"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].JurLicoAddress_DB = Convert.ToString(worksheet0.Cells["F28"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].JurLicoFactAddress_DB = Convert.ToString(worksheet0.Cells["F28"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].GradeFIO_DB = Convert.ToString(worksheet0.Cells["F29"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Telephone_DB = Convert.ToString(worksheet0.Cells["F30"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Fax_DB = Convert.ToString(worksheet0.Cells["F31"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Email_DB = Convert.ToString(worksheet0.Cells["F32"].Value);

                newRepsFromExcel.Master_DB.Rows20[0].Okpo_DB = Convert.ToString(worksheet0.Cells["B36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Okved_DB = Convert.ToString(worksheet0.Cells["C36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Okogu_DB = Convert.ToString(worksheet0.Cells["D36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Oktmo_DB = Convert.ToString(worksheet0.Cells["E36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Inn_DB = Convert.ToString(worksheet0.Cells["F36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Kpp_DB = Convert.ToString(worksheet0.Cells["G36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Okopf_DB = Convert.ToString(worksheet0.Cells["H36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Okfs_DB = Convert.ToString(worksheet0.Cells["I36"].Value);

                newRepsFromExcel.Master_DB.Rows20[1].Okpo_DB = Convert.ToString(worksheet0.Cells["B37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Okved_DB = Convert.ToString(worksheet0.Cells["C37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Okogu_DB = Convert.ToString(worksheet0.Cells["D37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Oktmo_DB = Convert.ToString(worksheet0.Cells["E37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Inn_DB = Convert.ToString(worksheet0.Cells["F37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Kpp_DB = Convert.ToString(worksheet0.Cells["G37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Okopf_DB = Convert.ToString(worksheet0.Cells["H37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Okfs_DB = Convert.ToString(worksheet0.Cells["I37"].Value);
                break;
        }
    }

    #endregion

    #region GetImportReps

    private static Reports GetImportReps(ExcelWorksheet worksheet0)
    {
        var param0 = worksheet0.Name;
        var newRepsFromExcel = new Reports
        {
            Master_DB = new Report
            {
                FormNum_DB = param0
            }
        };
        switch (param0)
        {
            case "1.0":
            {
                var ty1 = (Form10)FormCreator.Create(param0);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form10)FormCreator.Create(param0);
                ty2.NumberInOrder_DB = 2;
                newRepsFromExcel.Master_DB.Rows10.Add(ty1);
                newRepsFromExcel.Master_DB.Rows10.Add(ty2);
                break;
            }
            case "2.0":
            {
                var ty1 = (Form20)FormCreator.Create(param0);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form20)FormCreator.Create(param0);
                ty2.NumberInOrder_DB = 2;
                newRepsFromExcel.Master_DB.Rows20.Add(ty1);
                newRepsFromExcel.Master_DB.Rows20.Add(ty2);
                break;
            }
        }
        GetDataTitleReps(newRepsFromExcel, worksheet0);
        //ReportsStorage.LocalReports.Reports_Collection.Add(newRepsFromExcel);
        return newRepsFromExcel;
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
        else
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
                    impRep.ValidBegin_28_DB = Convert.ToString(worksheet1.Cells["K4"].Value);
                    impRep.ValidThru_28_DB = Convert.ToString(worksheet1.Cells["N4"].Value);
                    impRep.PermissionDocumentName_28_DB = Convert.ToString(worksheet1.Cells["G5"].Value);
                    impRep.PermissionNumber1_28_DB = Convert.ToString(worksheet1.Cells["G6"].Value);
                    impRep.ValidBegin1_28_DB = Convert.ToString(worksheet1.Cells["K6"].Value);
                    impRep.ValidThru1_28_DB = Convert.ToString(worksheet1.Cells["N6"].Value);
                    impRep.PermissionDocumentName1_28_DB = Convert.ToString(worksheet1.Cells["G7"].Value);
                    impRep.ContractNumber_28_DB = Convert.ToString(worksheet1.Cells["G8"].Value);
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

        #region BindCommonData
        
        impRep.GradeExecutor_DB = Convert.ToString(worksheet1.Cells[$"D{worksheet1.Dimension.Rows - 1}"].Value);
        impRep.FIOexecutor_DB = Convert.ToString(worksheet1.Cells[$"F{worksheet1.Dimension.Rows - 1}"].Value);
        impRep.ExecPhone_DB = Convert.ToString(worksheet1.Cells[$"I{worksheet1.Dimension.Rows - 1}"].Value);
        impRep.ExecEmail_DB = Convert.ToString(worksheet1.Cells[$"K{worksheet1.Dimension.Rows - 1}"].Value);

        #endregion

        return impRep;
    }

    #endregion
}