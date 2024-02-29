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

namespace Client_App.Commands.AsyncCommands.Import;

//  Импорт -> Из Excel
internal class ImportExcelAsyncCommand : ImportBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        IsFirstLogLine = true;
        CurrentLogLine = 1;
        string[] extensions = ["xlsx", "XLSX"];
        var answer = await GetSelectedFilesFromDialog("Excel", extensions);
        if (answer is null) return;
        foreach (var res in answer) // Для каждого импортируемого файла
        {
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

                await MessageBox.Avalonia.MessageBoxManager
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
                    .ShowDialog(Desktop.MainWindow);

                #endregion

                continue;
            }

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
            baseReps ??= impReps;

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

            SkipNewOrg = SkipInter = SkipLess = SkipNew = SkipReplace = AtLeastOneImportDone = false;
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

                        an = await MessageBox.Avalonia.MessageBoxManager
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
                            .ShowDialog(Desktop.MainWindow);

                        #endregion

                        if (an is "Да для всех") SkipNewOrg = true;
                    }
                    else
                    {
                        #region MessageNewOrg

                        an = await MessageBox.Avalonia.MessageBoxManager
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
                            .ShowDialog(Desktop.MainWindow);

                        #endregion
                    }
                }

                await CheckAnswer(an, baseReps, impReps, null, impRep);

                #endregion
            }
        }

        await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync();
        await StaticConfiguration.DBModel.SaveChangesAsync();

        var suffix = answer.Length.ToString().EndsWith('1') && !answer.Length.ToString().EndsWith("11")
                ? "а"
                : "ов";
        if (AtLeastOneImportDone)
        {
            #region MessageImportDone

            await MessageBox.Avalonia.MessageBoxManager
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
                .ShowDialog(Desktop.MainWindow);

            #endregion
        }
        else
        {
            #region MessageImportCancel

            await MessageBox.Avalonia.MessageBoxManager
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
                .ShowDialog(Desktop.MainWindow);

            #endregion
        }
    }

    #region GetBaseReps

    private static Reports? GetBaseReps(ExcelWorksheet worksheet0)
    {
        return worksheet0.Name switch
        {
            "1.0" => ReportsStorage.LocalReports.Reports_Collection10.FirstOrDefault(t => 
                    (Convert.ToString(worksheet0.Cells["B36"].Value) == t.Master.Rows10[0].Okpo_DB
                     && Convert.ToString(worksheet0.Cells["F6"].Value) == t.Master.Rows10[0].RegNo_DB)
                    || (Convert.ToString(worksheet0.Cells["B36"].Value) == t.Master.Rows10[1].Okpo_DB
                        && Convert.ToString(worksheet0.Cells["F6"].Value) == t.Master.Rows10[1].RegNo_DB)),
            "2.0" => ReportsStorage.LocalReports.Reports_Collection20.FirstOrDefault(t =>
                (Convert.ToString(worksheet0.Cells["B36"].Value) == t.Master.Rows20[0].Okpo_DB
                 && Convert.ToString(worksheet0.Cells["F6"].Value) == t.Master.Rows20[0].RegNo_DB)
                || (Convert.ToString(worksheet0.Cells["B36"].Value) == t.Master.Rows20[1].Okpo_DB
                    && Convert.ToString(worksheet0.Cells["F6"].Value) == t.Master.Rows20[1].RegNo_DB)),
            _ => null
        };
    }

    #endregion

    #region GetDataFromRow

    private static void GetDataFromRow(string param1, ExcelWorksheet worksheet1, int start, Report repFromEx)
    {
        switch (param1)
        {
            case "1.1":
            {
                Form11 form11 = new();
                form11.ExcelGetRow(worksheet1, start);
                repFromEx.Rows11.Add(form11);
                break;
            }
            case "1.2":
            {
                Form12 form12 = new();
                form12.ExcelGetRow(worksheet1, start);
                repFromEx.Rows12.Add(form12);
                break;
            }
            case "1.3":
            {
                Form13 form13 = new();
                form13.ExcelGetRow(worksheet1, start);
                repFromEx.Rows13.Add(form13);
                break;
            }
            case "1.4":
            {
                Form14 form14 = new();
                form14.ExcelGetRow(worksheet1, start);
                repFromEx.Rows14.Add(form14);
                break;
            }
            case "1.5":
            {
                Form15 form15 = new();
                form15.ExcelGetRow(worksheet1, start);
                repFromEx.Rows15.Add(form15);
                break;
            }
            case "1.6":
            {
                Form16 form16 = new();
                form16.ExcelGetRow(worksheet1, start);
                repFromEx.Rows16.Add(form16);
                break;
            }
            case "1.7":
            {
                Form17 form17 = new();
                form17.ExcelGetRow(worksheet1, start);
                repFromEx.Rows17.Add(form17);
                break;
            }
            case "1.8":
            {
                Form18 form18 = new();
                form18.ExcelGetRow(worksheet1, start);
                repFromEx.Rows18.Add(form18);
                break;
            }
            case "1.9":
            {
                Form19 form19 = new();
                form19.ExcelGetRow(worksheet1, start);
                repFromEx.Rows19.Add(form19);
                break;
            }
            case "2.1":
            {
                if (!int.TryParse(Convert.ToString(worksheet1.Cells[$"A{start}"].Value), out _)) break;
                Form21 form21 = new();
                form21.ExcelGetRow(worksheet1, start);
                repFromEx.Rows21.Add(form21);
                break;
            }
            case "2.2":
            {
                if (!int.TryParse(Convert.ToString(worksheet1.Cells[$"A{start}"].Value), out _)) break;
                Form22 form22 = new();
                form22.ExcelGetRow(worksheet1, start);
                repFromEx.Rows22.Add(form22);
                break;
            }
            case "2.3":
            {
                Form23 form23 = new();
                form23.ExcelGetRow(worksheet1, start);
                repFromEx.Rows23.Add(form23);
                break;
            }
            case "2.4":
            {
                Form24 form24 = new();
                form24.ExcelGetRow(worksheet1, start);
                repFromEx.Rows24.Add(form24);
                break;
            }
            case "2.5":
            {
                Form25 form25 = new();
                form25.ExcelGetRow(worksheet1, start);
                repFromEx.Rows25.Add(form25);
                break;
            }
            case "2.6":
            {
                Form26 form26 = new();
                form26.ExcelGetRow(worksheet1, start);
                repFromEx.Rows26.Add(form26);
                break;
            }
            case "2.7":
            {
                Form27 form27 = new();
                form27.ExcelGetRow(worksheet1, start);
                repFromEx.Rows27.Add(form27);
                break;
            }
            case "2.8":
            {
                Form28 form28 = new();
                form28.ExcelGetRow(worksheet1, start);
                repFromEx.Rows28.Add(form28);
                break;
            }
            case "2.9":
            {
                Form29 form29 = new();
                form29.ExcelGetRow(worksheet1, start);
                repFromEx.Rows29.Add(form29);
                break;
            }
            case "2.10":
            {
                Form210 form210 = new();
                form210.ExcelGetRow(worksheet1, start);
                repFromEx.Rows210.Add(form210);
                break;
            }
            case "2.11":
            {
                Form211 form211 = new();
                form211.ExcelGetRow(worksheet1, start);
                repFromEx.Rows211.Add(form211);
                break;
            }
            case "2.12":
            {
                Form212 form212 = new();
                form212.ExcelGetRow(worksheet1, start);
                repFromEx.Rows212.Add(form212);
                break;
            }
        }
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

    private static Report GetReportWithDataFromExcel(ExcelWorksheet worksheet0, ExcelWorksheet worksheet1, string formNumber, IReadOnlyList<string> timeCreate)
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