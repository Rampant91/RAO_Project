using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Properties;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Interfaces;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Базовый класс выгрузки данных в .xlsx.
/// </summary>
public abstract class ExcelBaseAsyncCommand : BaseAsyncCommand
{
    private readonly CancellationTokenSource _cts = new();

    private protected ExcelWorksheet Worksheet { get; set; }

    private protected ExcelWorksheet WorksheetPrim { get; set; }

    private protected string ExportType;

    private protected AnyTaskProgressBar? ProgressBar;

    public override async void Execute(object? parameter)
    {
        IsExecute = true;
        try
        {
            await Task.Run(() => AsyncExecute(parameter));
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }
        finally
        {
            if (ProgressBar is not null) await ProgressBar.CloseAsync();
            IsExecute = false;
        }
    }

    public abstract override Task AsyncExecute(object? parameter);

    private protected static async Task CancelCommandAndCloseProgressBarWindow(CancellationTokenSource cts, AnyTaskProgressBar? progressBar = null)
    {
        await cts.CancelAsync();
        if (progressBar is not null) await progressBar.CloseAsync();
        cts.Token.ThrowIfCancellationRequested();
    }

    #region ExcelExportNotes

    /// <summary>
    /// Выгрузка примечаний в .xlsx.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="startRow">Номер начального ряда.</param>
    /// <param name="startColumn">Номер начальной колонки.</param>
    /// <param name="worksheetPrim">Лист Excel с примечаниями.</param>
    /// <param name="forms">Список отчётов.</param>
    /// <param name="printId"></param>
    /// <returns></returns>
    private protected static int ExcelExportNotes(string formNum, int startRow, int startColumn, ExcelWorksheet worksheetPrim,
        List<Report> forms, bool printId = false)
    {
        foreach (var item in forms)
        {
            var findReports = ReportsStorage.LocalReports.Reports_Collection
                .Where(t => t.Report_Collection.Contains(item));
            var reps = findReports.FirstOrDefault();
            if (reps == null) continue;
            var curRow = startRow;
            foreach (var i in item.Notes)
            {
                var mstRep = reps.Master_DB;
                i.ExcelRow(worksheetPrim, curRow, startColumn + 1);
                var yu = printId
                    ? formNum.Split('.')[0] == "1"
                        ? mstRep.Rows10[1].RegNo_DB != "" && mstRep.Rows10[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows10[1]
                                .ExcelRow(worksheetPrim, curRow, 1, sumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1
                            : reps.Master_DB.Rows10[0]
                                .ExcelRow(worksheetPrim, curRow, 1, sumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1
                        : mstRep.Rows20[1].RegNo_DB != "" && mstRep.Rows20[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows20[1]
                                .ExcelRow(worksheetPrim, curRow, 1, sumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1
                            : reps.Master_DB.Rows20[0]
                                .ExcelRow(worksheetPrim, curRow, 1, sumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1
                    : formNum.Split('.')[0] == "1"
                        ? mstRep.Rows10[1].RegNo_DB != "" && mstRep.Rows10[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows10[1].ExcelRow(worksheetPrim, curRow, 1) + 1
                            : reps.Master_DB.Rows10[0].ExcelRow(worksheetPrim, curRow, 1) + 1
                        : mstRep.Rows20[1].RegNo_DB != "" && mstRep.Rows20[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows20[1].ExcelRow(worksheetPrim, curRow, 1) + 1
                            : reps.Master_DB.Rows20[0].ExcelRow(worksheetPrim, curRow, 1) + 1;

                item.ExcelRow(worksheetPrim, curRow, yu);
                curRow++;
            }

            startRow = curRow;
        }

        return startRow;
    }

    #endregion

    #region ExcelGetFullPath

    /// <summary>
    /// Выводит сообщение, дающее выбор, открывать временную копию или сохранить файл.
    /// </summary>
    /// <param name="fileName">Имя файла.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <returns>Полный путь до файла и флаг, нужно ли открывать временную копию.</returns>
    private protected static async Task<(string fullPath, bool openTemp)> ExcelGetFullPath(string fileName, CancellationTokenSource cts, 
        AnyTaskProgressBar? progressBar = null)
    {
        #region MessageSaveOrOpenTemp

        var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams 
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Сохранить" },
                    new ButtonDefinition { Name = "Открыть временную копию" }
                ],
                ContentTitle = "Выгрузка в Excel",
                ContentHeader = "Уведомление",
                ContentMessage = "Что бы вы хотели сделать" +
                                 $"{Environment.NewLine} с данной выгрузкой?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        var fullPath = "";
        var openTemp = res is "Открыть временную копию";

        switch (res)
        {
            case "Открыть временную копию":
            {
                DirectoryInfo tmpFolder = new(Path.Combine(BaseVM.SystemDirectory, "RAO", "temp"));
                var count = 0;
                do
                {
                    fullPath = Path.Combine(tmpFolder.FullName, fileName + $"_{++count}.xlsx");
                } while (File.Exists(fullPath));

                break;
            }
            case "Сохранить":
            {
                SaveFileDialog dial = new();
                var filter = new FileDialogFilter
                {
                    Name = "Excel",
                    Extensions = { "xlsx" }
                };
                dial.Filters.Add(filter);
                dial.InitialFileName = fileName;
                fullPath = await dial.ShowAsync(Desktop.MainWindow);
                if (string.IsNullOrEmpty(fullPath)) await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                if (!fullPath.EndsWith(".xlsx")) fullPath += ".xlsx"; //В проводнике Linux в имя файла не подставляется расширение из фильтра, добавляю руками если его нет
                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath!);
                    }
                    catch
                    {
                        #region MessageFailedToSaveFile

                        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                            {
                                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                                ContentTitle = "Выгрузка в Excel",
                                ContentHeader = "Ошибка",
                                ContentMessage =
                                    $"Не удалось сохранить файл по пути: {fullPath}" +
                                    $"{Environment.NewLine}Файл с таким именем уже существует в этом расположении" +
                                    $"{Environment.NewLine}и используется другим процессом.",
                                MinWidth = 400,
                                MinHeight = 150,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(Desktop.MainWindow));

                            #endregion

                        await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                    }
                }

                break;
            }
            default:
            {
                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                break;
            }
        }
        return (fullPath, openTemp);
    }

    #endregion

    #region ExcelExportRows

    /// <summary>
    /// Выгрузка строчек форм в .xlsx.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="startRow">Номер начального ряда.</param>
    /// <param name="startColumn">Номер начальной колонки.</param>
    /// <param name="worksheet">Лист Excel.</param>
    /// <param name="forms">Список отчётов.</param>
    /// <param name="id"></param>
    /// <returns></returns>
    private protected static int ExcelExportRows(string formNum, int startRow, int startColumn, ExcelWorksheet worksheet,
        List<Report> forms, bool id = false)
    {
        foreach (var item in forms)
        {
            var reps = ReportsStorage.LocalReports.Reports_Collection
                .FirstOrDefault(t => t.Report_Collection.Any(x => x.Id == item.Id));
                    //t.Report_Collection.Contains(item));
            if (reps is null) continue;
            IEnumerable<IKey> t;
            switch (formNum)
            {
                case "2.1":
                    t = item[formNum].ToList<IKey>().Where(x => ((Form21)x).Sum_DB || ((Form21)x).SumGroup_DB);
                    if (item[formNum].ToList<IKey>().Any() && !t.Any())
                    {
                        t = item[formNum].ToList<IKey>();
                    }
                    break;
                case "2.2":
                    t = item[formNum].ToList<IKey>().Where(x => ((Form22)x).Sum_DB || ((Form22)x).SumGroup_DB);
                    if (item[formNum].ToList<IKey>().Any() && !t.Any())
                    {
                        t = item[formNum].ToList<IKey>();
                    }
                    break;
                default:
                    t = item[formNum].ToList<IKey>();
                    break;
            }

            var lst = t.Any()
                ? item[formNum].ToList<IKey>().ToList()
                : item[formNum].ToList<IKey>().OrderBy(x => ((Form)x).NumberInOrder_DB).ToList();
            if (lst.Count <= 0) continue;
            var count = startRow;
            startRow--;
            lst = lst
                .Where(it => it != null)
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var it in lst)
            {
                switch (it)
                {
                    case Form11 form11:
                        form11.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form12 form12:
                        form12.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form13 form13:
                        form13.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form14 form14:
                        form14.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form15 form15:
                        form15.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form16 form16:
                        form16.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form17 form17:
                        form17.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form18 form18:
                        form18.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form19 form19:
                        form19.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form21 form21:
                        form21.ExcelRow(worksheet, count, startColumn + 1, sumNumber: form21.NumberInOrderSum_DB);
                        break;
                    case Form22 form22:
                        form22.ExcelRow(worksheet, count, startColumn + 1, sumNumber: form22.NumberInOrderSum_DB);
                        break;
                    case Form23 form23:
                        form23.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form24 form24:
                        form24.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form25 form25:
                        form25.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form26 form26:
                        form26.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form27 form27:
                        form27.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form28 form28:
                        form28.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form29 form29:
                        form29.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form210 form210:
                        form210.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form211 form211:
                        form211.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form212 form212:
                        form212.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                }

                var masterRep = reps.Master_DB;

                var yu = id
                    ? formNum.Split('.')[0] == "1"
                        ? masterRep.Rows10[1].RegNo_DB != "" && masterRep.Rows10[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows10[1]
                                .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows10[1].Id.ToString()) + 1
                            : reps.Master_DB.Rows10[0]
                                .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows10[0].Id.ToString()) + 1
                        : masterRep.Rows20[1].RegNo_DB != "" && masterRep.Rows20[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows20[1]
                                .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1
                            : reps.Master_DB.Rows20[0]
                                .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows20[0].Id.ToString()) + 1
                    : formNum.Split('.')[0] == "1"
                        ? masterRep.Rows10[1].RegNo_DB != "" && masterRep.Rows10[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows10[1].ExcelRow(worksheet, count, 1) + 1
                            : reps.Master_DB.Rows10[0].ExcelRow(worksheet, count, 1) + 1
                        : masterRep.Rows20[1].RegNo_DB != "" && masterRep.Rows20[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows20[1].ExcelRow(worksheet, count, 1) + 1
                            : reps.Master_DB.Rows20[0].ExcelRow(worksheet, count, 1) + 1;

                item.ExcelRow(worksheet, count, yu);
                count++;
            }

            //if (formNum.Split('.')[0] == "2")
            //{
            //    var new_number = 2;
            //    while (worksheet.Cells[new_number, 6].Value != null)
            //    {
            //        worksheet.Cells[new_number, 6].Value = new_number - 1;
            //        new_number++;
            //    }
            //}
            startRow = count;
        }

        return startRow;
    }

    #endregion

    #region ExcelPrintTitulExport

    /// <summary>
    /// Выгрузка данных титульного листа в .xlsx.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="worksheet">Лист Excel.</param>
    /// <param name="rep">Отчёт.</param>
    /// <param name="master">Головной отчёт организации.</param>
    private protected static void ExcelPrintTitleExport(string formNum, ExcelWorksheet worksheet, Report rep, Report master)
    {
        if (formNum.Split('.')[0] == "2")
        {
            var frmYur = master.Rows20[0];
            var frmObosob = master.Rows20[1];
            worksheet.Cells["G10"].Value = rep.Year_DB;

            worksheet.Cells["F6"].Value = frmYur.RegNo_DB;
            worksheet.Cells["F15"].Value = frmYur.OrganUprav_DB;
            worksheet.Cells["F16"].Value = frmYur.SubjectRF_DB;
            worksheet.Cells["F17"].Value = frmYur.JurLico_DB;
            worksheet.Cells["F18"].Value = frmYur.ShortJurLico_DB;
            worksheet.Cells["F19"].Value = frmYur.JurLicoAddress_DB;
            worksheet.Cells["F20"].Value = frmYur.JurLicoFactAddress_DB;
            worksheet.Cells["F21"].Value = frmYur.GradeFIO_DB;
            worksheet.Cells["F22"].Value = frmYur.Telephone_DB;
            worksheet.Cells["F23"].Value = frmYur.Fax_DB;
            worksheet.Cells["F24"].Value = frmYur.Email_DB;

            worksheet.Cells["F25"].Value = frmObosob.SubjectRF_DB;
            worksheet.Cells["F26"].Value = frmObosob.JurLico_DB;
            worksheet.Cells["F27"].Value = frmObosob.ShortJurLico_DB;
            worksheet.Cells["F28"].Value = frmObosob.JurLicoAddress_DB;
            worksheet.Cells["F29"].Value = frmObosob.GradeFIO_DB;
            worksheet.Cells["F30"].Value = frmObosob.Telephone_DB;
            worksheet.Cells["F31"].Value = frmObosob.Fax_DB;
            worksheet.Cells["F32"].Value = frmObosob.Email_DB;

            worksheet.Cells["B36"].Value = frmYur.Okpo_DB;
            worksheet.Cells["C36"].Value = frmYur.Okved_DB;
            worksheet.Cells["D36"].Value = frmYur.Okogu_DB;
            worksheet.Cells["E36"].Value = frmYur.Oktmo_DB;
            worksheet.Cells["F36"].Value = frmYur.Inn_DB;
            worksheet.Cells["G36"].Value = frmYur.Kpp_DB;
            worksheet.Cells["H36"].Value = frmYur.Okopf_DB;
            worksheet.Cells["I36"].Value = frmYur.Okfs_DB;

            worksheet.Cells["B37"].Value = frmObosob.Okpo_DB;
            worksheet.Cells["C37"].Value = frmObosob.Okved_DB;
            worksheet.Cells["D37"].Value = frmObosob.Okogu_DB;
            worksheet.Cells["E37"].Value = frmObosob.Oktmo_DB;
            worksheet.Cells["F37"].Value = frmObosob.Inn_DB;
            worksheet.Cells["G37"].Value = frmObosob.Kpp_DB;
            worksheet.Cells["H37"].Value = frmObosob.Okopf_DB;
            worksheet.Cells["I37"].Value = frmObosob.Okfs_DB;
        }
        else
        {
            var frmYur = master.Rows10[0];
            var frmObosob = master.Rows10[1];

            worksheet.Cells["F6"].Value = frmYur.RegNo_DB;
            worksheet.Cells["F15"].Value = frmYur.OrganUprav_DB;
            worksheet.Cells["F16"].Value = frmYur.SubjectRF_DB;
            worksheet.Cells["F17"].Value = frmYur.JurLico_DB;
            worksheet.Cells["F18"].Value = frmYur.ShortJurLico_DB;
            worksheet.Cells["F19"].Value = frmYur.JurLicoAddress_DB;
            worksheet.Cells["F20"].Value = frmYur.JurLicoFactAddress_DB;
            worksheet.Cells["F21"].Value = frmYur.GradeFIO_DB;
            worksheet.Cells["F22"].Value = frmYur.Telephone_DB;
            worksheet.Cells["F23"].Value = frmYur.Fax_DB;
            worksheet.Cells["F24"].Value = frmYur.Email_DB;

            worksheet.Cells["F25"].Value = frmObosob.SubjectRF_DB;
            worksheet.Cells["F26"].Value = frmObosob.JurLico_DB;
            worksheet.Cells["F27"].Value = frmObosob.ShortJurLico_DB;
            worksheet.Cells["F28"].Value = frmObosob.JurLicoAddress_DB;
            worksheet.Cells["F29"].Value = frmObosob.GradeFIO_DB;
            worksheet.Cells["F30"].Value = frmObosob.Telephone_DB;
            worksheet.Cells["F31"].Value = frmObosob.Fax_DB;
            worksheet.Cells["F32"].Value = frmObosob.Email_DB;

            worksheet.Cells["B36"].Value = frmYur.Okpo_DB;
            worksheet.Cells["C36"].Value = frmYur.Okved_DB;
            worksheet.Cells["D36"].Value = frmYur.Okogu_DB;
            worksheet.Cells["E36"].Value = frmYur.Oktmo_DB;
            worksheet.Cells["F36"].Value = frmYur.Inn_DB;
            worksheet.Cells["G36"].Value = frmYur.Kpp_DB;
            worksheet.Cells["H36"].Value = frmYur.Okopf_DB;
            worksheet.Cells["I36"].Value = frmYur.Okfs_DB;

            worksheet.Cells["B37"].Value = frmObosob.Okpo_DB;
            worksheet.Cells["C37"].Value = frmObosob.Okved_DB;
            worksheet.Cells["D37"].Value = frmObosob.Okogu_DB;
            worksheet.Cells["E37"].Value = frmObosob.Oktmo_DB;
            worksheet.Cells["F37"].Value = frmObosob.Inn_DB;
            worksheet.Cells["G37"].Value = frmObosob.Kpp_DB;
            worksheet.Cells["H37"].Value = frmObosob.Okopf_DB;
            worksheet.Cells["I37"].Value = frmObosob.Okfs_DB;
        }
    }

    #endregion

    #region ExcelPrintSubMainExport

    /// <summary>
    /// Выгрузка дополнительных данных титульного листа в .xlsx.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="worksheet">Лист Excel.</param>
    /// <param name="rep">Отчёт.</param>
    private protected static void ExcelPrintSubMainExport(string formNum, ExcelWorksheet worksheet, Report rep)
    {
        if (formNum.Split('.')[0] == "1")
        {
            worksheet.Cells["G3"].Value = rep.StartPeriod_DB;
            worksheet.Cells["G4"].Value = rep.EndPeriod_DB;
            worksheet.Cells["G5"].Value = rep.CorrectionNumber_DB;
        }
        else
        {
            switch (formNum)
            {
                case "2.6":
                {
                    worksheet.Cells["G4"].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells["G5"].Value = rep.SourcesQuantity26_DB;
                    break;
                }
                case "2.7":
                {
                    worksheet.Cells["G3"].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells["G4"].Value = rep.PermissionNumber27_DB;
                    worksheet.Cells["G5"].Value = rep.ValidBegin27_DB;
                    worksheet.Cells["J5"].Value = rep.ValidThru27_DB;
                    worksheet.Cells["G6"].Value = rep.PermissionDocumentName27_DB;
                    break;
                }
                case "2.8":
                {
                    worksheet.Cells["G3"].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells["G4"].Value = rep.PermissionNumber_28_DB;
                    worksheet.Cells["K4"].Value = rep.ValidBegin_28_DB;
                    worksheet.Cells["N4"].Value = rep.ValidThru_28_DB;
                    worksheet.Cells["G5"].Value = rep.PermissionDocumentName_28_DB;

                    worksheet.Cells["G6"].Value = rep.PermissionNumber1_28_DB;
                    worksheet.Cells["K6"].Value = rep.ValidBegin1_28_DB;
                    worksheet.Cells["N6"].Value = rep.ValidThru1_28_DB;
                    worksheet.Cells["G7"].Value = rep.PermissionDocumentName1_28_DB;

                    worksheet.Cells["G8"].Value = rep.ContractNumber_28_DB;
                    worksheet.Cells["K8"].Value = rep.ValidBegin2_28_DB;
                    worksheet.Cells["N8"].Value = rep.ValidThru2_28_DB;
                    worksheet.Cells["G9"].Value = rep.OrganisationReciever_28_DB;

                    worksheet.Cells["D21"].Value = rep.GradeExecutor_DB;
                    worksheet.Cells["F21"].Value = rep.FIOexecutor_DB;
                    worksheet.Cells["I21"].Value = rep.ExecPhone_DB;
                    worksheet.Cells["K21"].Value = rep.ExecEmail_DB;
                    return;
                }
                default:
                {
                    worksheet.Cells["G4"].Value = rep.CorrectionNumber_DB;
                    break;
                }
            }
        }

        worksheet.Cells["D18"].Value = rep.GradeExecutor_DB;
        worksheet.Cells["F18"].Value = rep.FIOexecutor_DB;
        worksheet.Cells["I18"].Value = rep.ExecPhone_DB;
        worksheet.Cells["K18"].Value = rep.ExecEmail_DB;
    }

    #endregion

    #region ExcelPrintNotesExport

    /// <summary>
    /// Выгрузка примечаний в шаблон для печати .xlsx.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="worksheet">Лист Excel.</param>
    /// <param name="rep">Отчёт.</param>
    private protected static void ExcelPrintNotesExport(string formNum, ExcelWorksheet worksheet, Report rep)
    {
        var start = formNum is "2.8"
            ? 18
            : 15;

        for (var i = 0; i < rep.Notes.Count - 1; i++)
        {
            worksheet.InsertRow(start + 1, 1, start);
            var cells = worksheet.Cells[$"A{start + 1}:B{start + 1}"];
            foreach (var cell in cells)
            {
                var btm = cell.Style.Border.Bottom;
                var lft = cell.Style.Border.Left;
                var rgt = cell.Style.Border.Right;
                var top = cell.Style.Border.Top;
                btm.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                btm.Color.SetColor(255, 0, 0, 0);
                lft.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                lft.Color.SetColor(255, 0, 0, 0);
                rgt.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                rgt.Color.SetColor(255, 0, 0, 0);
                top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                top.Color.SetColor(255, 0, 0, 0);
            }

            var cellCL = worksheet.Cells[$"C{start + 1}:L{start + 1}"];
            cellCL.Merge = true;
            var btmCL = cellCL.Style.Border.Bottom;
            var lftCL = cellCL.Style.Border.Left;
            var rgtCL = cellCL.Style.Border.Right;
            var topCL = cellCL.Style.Border.Top;
            btmCL.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            btmCL.Color.SetColor(255, 0, 0, 0);
            lftCL.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            lftCL.Color.SetColor(255, 0, 0, 0);
            rgtCL.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            rgtCL.Color.SetColor(255, 0, 0, 0);
            topCL.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            topCL.Color.SetColor(255, 0, 0, 0);
        }

        var count = start;
        foreach (var note in rep.Notes)
        {
            note.ExcelRow(worksheet, count, 1);
            count++;
        }
    }

    #endregion

    #region ExcelPrintRowsExport

    /// <summary>
    /// Выгрузка строчек в шаблон для печати .xlsx.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    /// <param name="worksheet">Лист Excel.</param>
    /// <param name="rep">Отчёт.</param>
    private protected static void ExcelPrintRowsExport(string formNum, ExcelWorksheet worksheet, Report rep)
    {
        var start = formNum is "2.8"
            ? 14
            : 11;

        for (var i = 0; i < rep[formNum].Count - 1; i++)
        {
            worksheet.InsertRow(start + 1, 1, start);
            var cells = worksheet.Cells[$"A{start + 1}:B{start + 1}"];
            foreach (var cell in cells)
            {
                var btm = cell.Style.Border.Bottom;
                var lft = cell.Style.Border.Left;
                var rgt = cell.Style.Border.Right;
                var top = cell.Style.Border.Top;
                btm.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                btm.Color.SetColor(255, 0, 0, 0);
                lft.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                lft.Color.SetColor(255, 0, 0, 0);
                rgt.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                rgt.Color.SetColor(255, 0, 0, 0);
                top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                top.Color.SetColor(255, 0, 0, 0);
            }
        }

        var count = start;

        #region 2.1 with Sum

        if (formNum is "2.1" && rep[formNum].ToList<Form21>().Any(form21 => form21.Sum_DB))
        {
            var forms21 = rep[formNum]
                .ToList<Form21>()
                .GroupBy(x => x.RefineMachineName_DB
                              + x.MachineCode_DB
                              + x.MachinePower_DB
                              + x.NumberOfHoursPerYear_DB)
                .OrderBy(x => x.Count())
                .Select(group => group
                    .OrderBy(x => x.Order))
                .SelectMany(x => x.ToArray())
                .ToArray();
            foreach (var formWithSum in forms21)
            {
                formWithSum.ExcelRow(worksheet, count, 1, sumNumber: formWithSum.NumberInOrderSum_DB);
                count++;
            }
        }

        #endregion

        #region 2.2 with Sum

        if (formNum is "2.2" && rep[formNum].ToList<Form22>().Any(form22 => form22.Sum_DB))
        {
            var forms22 = rep[formNum]
                .ToList<Form22>()
                .GroupBy(x => x.StoragePlaceName_DB
                              + x.StoragePlaceCode_DB
                              + x.PackName_DB
                              + x.PackType_DB)
                .OrderBy(x => x.Count())
                .Select(group => group
                    .OrderBy(x => x.Order))
                .SelectMany(x => x.ToArray())
                .ToArray();
            foreach (var formWithSum in forms22)
            {
                formWithSum.ExcelRow(worksheet, count, 1, sumNumber: formWithSum.NumberInOrderSum_DB);
                count++;
            }
        }

        #endregion

        else
        {
            foreach (var it in rep[formNum])
            {
                switch (it)
                {
                    case Form11 form11:
                        form11.ExcelRow(worksheet, count, 1);
                        break;
                    case Form12 form12:
                        form12.ExcelRow(worksheet, count, 1);
                        break;
                    case Form13 form13:
                        form13.ExcelRow(worksheet, count, 1);
                        break;
                    case Form14 form14:
                        form14.ExcelRow(worksheet, count, 1);
                        break;
                    case Form15 form15:
                        form15.ExcelRow(worksheet, count, 1);
                        break;
                    case Form16 form16:
                        form16.ExcelRow(worksheet, count, 1);
                        break;
                    case Form17 form17:
                        form17.ExcelRow(worksheet, count, 1);
                        break;
                    case Form18 form18:
                        form18.ExcelRow(worksheet, count, 1);
                        break;
                    case Form19 form19:
                        form19.ExcelRow(worksheet, count, 1);
                        break;
                    case Form21 form21:
                        form21.ExcelRow(worksheet, count, 1);
                        break;
                    case Form22 form22:
                        form22.ExcelRow(worksheet, count, 1);
                        break;
                    case Form23 form23:
                        form23.ExcelRow(worksheet, count, 1);
                        break;
                    case Form24 form24:
                        form24.ExcelRow(worksheet, count, 1);
                        break;
                    case Form25 form25:
                        form25.ExcelRow(worksheet, count, 1);
                        break;
                    case Form26 form26:
                        form26.ExcelRow(worksheet, count, 1);
                        break;
                    case Form27 form27:
                        form27.ExcelRow(worksheet, count, 1);
                        break;
                    case Form28 form28:
                        form28.ExcelRow(worksheet, count, 1);
                        break;
                    case Form29 form29:
                        form29.ExcelRow(worksheet, count, 1);
                        break;
                    case Form210 form210:
                        form210.ExcelRow(worksheet, count, 1);
                        break;
                    case Form211 form211:
                        form211.ExcelRow(worksheet, count, 1);
                        break;
                    case Form212 form212:
                        form212.ExcelRow(worksheet, count, 1);
                        break;
                }

                count++;
            }
        }
    }

    #endregion

    #region ExcelSaveAndOpen

    /// <summary>
    /// Сохранить изменения в .xlsx и открыть временную копию при необходимости.
    /// </summary>
    /// <param name="excelPackage">Пакет данных .xlsx.</param>
    /// <param name="fullPath">Полный путь к файлу .xlsx.</param>
    /// <param name="openTemp">Флаг, открывать ли временную копию.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Открывает файл выгрузки в .xlsx.</returns>
    private protected static async Task ExcelSaveAndOpen(ExcelPackage excelPackage, string fullPath, bool openTemp, 
        CancellationTokenSource cts)
    {
        try
        {
            await excelPackage.SaveAsync(cancellationToken: cts.Token);
        }
        catch (ObjectDisposedException ex)
        {
            return;
        }
        catch (Exception ex)
        {
            #region MessageFailedToSaveFile

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Не удалось сохранить файл по указанному пути:" +
                                     $"{Environment.NewLine}{fullPath}",
                    MinWidth = 400,
                    MinHeight = 175,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Warning(msg);

            return;
        }

        if (openTemp)
        {
            Process.Start(new ProcessStartInfo { FileName = fullPath, UseShellExecute = true });
        }
        else
        {
            #region MessageExcelExportComplete

            var answer =
                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Ок" },
                        new ButtonDefinition { Name = "Открыть выгрузку" }
                    ],
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Выгрузка сохранена по пути:" +
                                     $"{Environment.NewLine}{fullPath}",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            if (answer is "Открыть выгрузку")
            {
                Process.Start(new ProcessStartInfo { FileName = fullPath, UseShellExecute = true });
            }
        }
    }

    #endregion

    #region CreateTempDb

    /// <summary>
    /// Создание временной копии БД.
    /// </summary>
    /// <returns>Полный путь до файла временной копии БД.</returns>
    private protected static Task<string> CreateTempDb()
    {
        var tmpFolder = new DirectoryInfo(Path.Combine(BaseVM.SystemDirectory, "RAO", "temp"));
        var tmpDbPath = Path.Combine(tmpFolder.FullName, BaseVM.DbFileName + ".RAODB");

        //var count = 0;
        //string tmpDbPath;
        //do
        //{
        //    tmpDbPath = Path.Combine(tmpFolder.FullName, BaseVM.DbFileName + $"_{++count}.RAODB");
        //} 
        //while (File.Exists(tmpDbPath));

        if (!File.Exists(tmpDbPath))
        {
            File.Copy(Path.Combine(BaseVM.RaoDirectory, BaseVM.DbFileName + ".RAODB"), tmpDbPath);
        }

        return Task.FromResult(tmpDbPath);
    }

    #endregion

    #region GetFilesFromPasDirectory

    /// <summary>
    /// Получение списка файлов из папки хранилища паспортов.
    /// </summary>
    /// <returns>Список файлов из папки хранилища паспортов.</returns>
    private protected async Task<List<FileInfo>> GetFilesFromPasDirectory()
    {
        var pasFolderDirectory = new DirectoryInfo(Settings.Default.PasFolderDefaultPath);
        List<FileInfo> files = [];
        try
        {
            files.AddRange(pasFolderDirectory.GetFiles("*#*#*#*#*.pdf", SearchOption.AllDirectories));
        }
        catch
        {
            #region MessageFailedToOpenPassportDirectory

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Ошибка",
                    ContentMessage = $"Не удалось открыть сетевое хранилище паспортов:" +
                                     $"{Environment.NewLine}{pasFolderDirectory.FullName}",
                    MinWidth = 400,
                    MinHeight = 170,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(ProgressBar));

            #endregion

            await _cts.CancelAsync();
            _cts.Token.ThrowIfCancellationRequested();
        }
        return files;
    }

    #endregion

    #region InitializeExcelPackage

    /// <summary>
    /// Инициализация Excel пакета.
    /// </summary>
    /// <param name="fullPath">Полный путь до .xlsx файла.</param>
    /// <returns>Пакет Excel.</returns>
    private protected static Task<ExcelPackage> InitializeExcelPackage(string fullPath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        return Task.FromResult(excelPackage);
    }

    #endregion

    #region InventoryCheck

    /// <summary>
    /// Проверка, является ли отчёт инвентаризационным. Если все строчки с кодом операции 10 - добавляет " (ИНВ)",
    /// если хотя бы одна - добавляет " (инв)".
    /// </summary>
    /// <param name="repRowsCount">Количество строчек в отчёте.</param>
    /// <param name="countCode10">Количество строчек с кодом операции 10.</param>
    /// <returns>Строчка, информирующая о том, является ли отчёт инвентаризационным.</returns>
    private protected static string InventoryCheck(int repRowsCount, int countCode10)
    {
        return countCode10 == repRowsCount && repRowsCount > 0
            ? " (ИНВ)"
            : countCode10 > 0
                ? " (инв)"
                : "";
    }

    #endregion
}