using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.ViewModels;
using Models.Collections;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Forms;
using Models.Interfaces;
using OfficeOpenXml;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Avalonia;
using Client_App.Views;

namespace Client_App.Commands.AsyncCommands.Excel
{
    public abstract class ExcelBaseAsyncCommand : AsyncBaseCommand
    {
        private protected ExcelWorksheet? Worksheet { get; set; }

        private protected ExcelWorksheet? WorksheetComment { get; set; }

        private protected static IClassicDesktopStyleApplicationLifetime? Desktop { get; set; }

        #region Constructor

        protected ExcelBaseAsyncCommand()
        {
            Desktop = Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        }

        #endregion

        #region ExcelExportNotes

        private int ExcelExportNotes(string param, int startRow, int startColumn, ExcelWorksheet worksheetPrim,
            List<Report> forms, bool printId = false)
        {
            foreach (var item in forms)
            {
                var findReports = MainWindowVM.LocalReports.Reports_Collection
                    .Where(t => t.Report_Collection.Contains(item));
                var reps = findReports.FirstOrDefault();
                if (reps == null) continue;
                var curRow = startRow;
                foreach (var i in item.Notes)
                {
                    var mstRep = reps.Master_DB;
                    i.ExcelRow(worksheetPrim, curRow, startColumn + 1);
                    var yu = printId
                        ? param.Split('.')[0] == "1"
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
                        : param.Split('.')[0] == "1"
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

        private protected async Task<(string fullPath, bool openTemp)> ExcelGetFullPath(string fileName, CancellationTokenSource cts)
        {
            #region MessageSaveOrOpenTemp

            var res = await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "Сохранить" },
                        new ButtonDefinition { Name = "Открыть временную копию" }
                    },
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Что бы вы хотели сделать" +
                                     $"{Environment.NewLine} с данной выгрузкой?",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow);

            #endregion

            var fullPath = "";
            var openTemp = res is "Открыть временную копию";

            switch (res)
            {
                case "Открыть временную копию":
                    {
                        DirectoryInfo tmpFolder = new(Path.Combine(Path.Combine(BaseVM.SystemDirectory, "RAO"), "temp"));
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
                        if (string.IsNullOrEmpty(fullPath))
                        {
                            cts.Cancel();
                            cts.Token.ThrowIfCancellationRequested();
                        }
                        if (fullPath!.EndsWith(".xlsx"))
                        {
                            fullPath += ".xlsx";
                        }

                        if (File.Exists(fullPath))
                        {
                            try
                            {
                                File.Delete(fullPath);
                            }
                            catch (Exception)
                            {
                                #region MessageFailedToSaveFile

                                await MessageBox.Avalonia.MessageBoxManager
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
                                    .ShowDialog(Desktop.MainWindow);

                                #endregion

                                cts.Cancel();
                                cts.Token.ThrowIfCancellationRequested();
                            }
                        }

                        break;
                    }
                default:
                    {
                        cts.Cancel();
                        cts.Token.ThrowIfCancellationRequested();
                        break;
                    }
            }

            return (fullPath, openTemp);
        }

        #endregion

        #region ExcelExportRows

        private int ExcelExportRows(string param, int startRow, int startColumn, ExcelWorksheet worksheet,
            List<Report> forms, bool id = false)
        {
            foreach (var item in forms)
            {
                var findReports = MainWindowVM.LocalReports.Reports_Collection
                    .Where(t => t.Report_Collection.Contains(item));
                var reps = findReports.FirstOrDefault();
                if (reps is null) continue;
                IEnumerable<IKey> t = null;
                switch (param)
                {
                    case "2.1":
                        {
                            t = item[param].ToList<IKey>().Where(x => ((Form21)x).Sum_DB || ((Form21)x).SumGroup_DB);
                            if (item[param].ToList<IKey>().Any() && !t.Any())
                            {
                                t = item[param].ToList<IKey>();
                            }

                            break;
                        }
                    case "2.2":
                        {
                            t = item[param].ToList<IKey>().Where(x => ((Form22)x).Sum_DB || ((Form22)x).SumGroup_DB);
                            if (item[param].ToList<IKey>().Any() && !t.Any())
                            {
                                t = item[param].ToList<IKey>();
                            }

                            break;
                        }
                }

                if (param != "2.1" && param != "2.2")
                {
                    t = item[param].ToList<IKey>();
                }

                var lst = t.Any()
                    ? item[param].ToList<IKey>().ToList()
                    : item[param].ToList<IKey>().OrderBy(x => ((Form)x).NumberInOrder_DB).ToList();
                if (lst.Count <= 0) continue;
                var count = startRow;
                startRow--;
                foreach (var it in lst.Where(it => it != null).OrderBy(x => x.Order))
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

                    var mstrep = reps.Master_DB;

                    var yu = id
                        ? param.Split('.')[0] == "1"
                            ? mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != ""
                                ? reps.Master_DB.Rows10[1]
                                    .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows10[1].Id.ToString()) + 1
                                : reps.Master_DB.Rows10[0]
                                    .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows10[0].Id.ToString()) + 1
                            : mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != ""
                                ? reps.Master_DB.Rows20[1]
                                    .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1
                                : reps.Master_DB.Rows20[0]
                                    .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows20[0].Id.ToString()) + 1
                        : param.Split('.')[0] == "1"
                            ? mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != ""
                                ? reps.Master_DB.Rows10[1].ExcelRow(worksheet, count, 1) + 1
                                : reps.Master_DB.Rows10[0].ExcelRow(worksheet, count, 1) + 1
                            : mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != ""
                                ? reps.Master_DB.Rows20[1].ExcelRow(worksheet, count, 1) + 1
                                : reps.Master_DB.Rows20[0].ExcelRow(worksheet, count, 1) + 1;

                    item.ExcelRow(worksheet, count, yu);
                    count++;
                }

                //if (param.Split('.')[0] == "2")
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

        private void ExcelPrintTitulExport(string param, ExcelWorksheet worksheet, Report form)
        {
            var findReports = MainWindowVM.LocalReports.Reports_Collection
                .Where(t => t.Report_Collection.Contains(form));
            var reps = findReports.FirstOrDefault();
            var master = reps.Master_DB;
            if (param.Split('.')[0] == "2")
            {
                var frmYur = master.Rows20[0];
                var frmObosob = master.Rows20[1];
                worksheet.Cells["G10"].Value = form.Year_DB;

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

        private void ExcelPrintSubMainExport(string param, ExcelWorksheet worksheet, Report form)
        {
            var findReports = MainWindowVM.LocalReports.Reports_Collection
                .Where(t => t.Report_Collection.Contains(form));
            var reps = findReports.FirstOrDefault();
            var master = reps.Master_DB;

            if (param.Split('.')[0] == "1")
            {
                worksheet.Cells["G3"].Value = form.StartPeriod_DB;
                worksheet.Cells["G4"].Value = form.EndPeriod_DB;
                worksheet.Cells["G5"].Value = form.CorrectionNumber_DB;
            }
            else
            {
                switch (param)
                {
                    case "2.6":
                        {
                            worksheet.Cells["G4"].Value = form.CorrectionNumber_DB;
                            worksheet.Cells["G5"].Value = form.SourcesQuantity26_DB;
                            break;
                        }
                    case "2.7":
                        {
                            worksheet.Cells["G3"].Value = form.CorrectionNumber_DB;
                            worksheet.Cells["G4"].Value = form.PermissionNumber27_DB;
                            worksheet.Cells["G5"].Value = form.ValidBegin27_DB;
                            worksheet.Cells["J5"].Value = form.ValidThru27_DB;
                            worksheet.Cells["G6"].Value = form.PermissionDocumentName27_DB;
                            break;
                        }
                    case "2.8":
                        {
                            worksheet.Cells["G3"].Value = form.CorrectionNumber_DB;
                            worksheet.Cells["G4"].Value = form.PermissionNumber_28_DB;
                            worksheet.Cells["K4"].Value = form.ValidBegin_28_DB;
                            worksheet.Cells["N4"].Value = form.ValidThru_28_DB;
                            worksheet.Cells["G5"].Value = form.PermissionDocumentName_28_DB;

                            worksheet.Cells["G6"].Value = form.PermissionNumber1_28_DB;
                            worksheet.Cells["K6"].Value = form.ValidBegin1_28_DB;
                            worksheet.Cells["N6"].Value = form.ValidThru1_28_DB;
                            worksheet.Cells["G7"].Value = form.PermissionDocumentName1_28_DB;

                            worksheet.Cells["G8"].Value = form.ContractNumber_28_DB;
                            worksheet.Cells["K8"].Value = form.ValidBegin2_28_DB;
                            worksheet.Cells["N8"].Value = form.ValidThru2_28_DB;
                            worksheet.Cells["G9"].Value = form.OrganisationReciever_28_DB;

                            worksheet.Cells["D21"].Value = form.GradeExecutor_DB;
                            worksheet.Cells["F21"].Value = form.FIOexecutor_DB;
                            worksheet.Cells["I21"].Value = form.ExecPhone_DB;
                            worksheet.Cells["K21"].Value = form.ExecEmail_DB;
                            return;
                        }
                    default:
                        {
                            worksheet.Cells["G4"].Value = form.CorrectionNumber_DB;
                            break;
                        }
                }
            }

            worksheet.Cells["D18"].Value = form.GradeExecutor_DB;
            worksheet.Cells["F18"].Value = form.FIOexecutor_DB;
            worksheet.Cells["I18"].Value = form.ExecPhone_DB;
            worksheet.Cells["K18"].Value = form.ExecEmail_DB;
        }

        #endregion

        #region ExcelPrintNotesExport

        private void ExcelPrintNotesExport(string param, ExcelWorksheet worksheet, Report form)
        {
            var start = param is "2.8"
                ? 18
                : 15;

            for (var i = 0; i < form.Notes.Count - 1; i++)
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
            foreach (var note in form.Notes)
            {
                note.ExcelRow(worksheet, count, 1);
                count++;
            }
        }

        #endregion

        #region ExcelPrintRowsExport

        private void ExcelPrintRowsExport(string param, ExcelWorksheet worksheet, Report form)
        {
            var start = param is "2.8"
                ? 14
                : 11;

            for (var i = 0; i < form[param].Count - 1; i++)
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
            foreach (var it in form[param])
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
            //var new_number = 1;
            //var row = 10;
            //while (worksheet.Cells[row, 1].Value != null)
            //{
            //    worksheet.Cells[row, 1].Value = new_number;
            //    new_number++;
            //    row++;
            //}
        }

        #endregion

        #region ExcelSaveAndOpen

        private protected static async Task ExcelSaveAndOpen(ExcelPackage excelPackage, string fullPath, bool openTemp)
        {
            try
            {
                excelPackage.Save();
            }
            catch (Exception)
            {
                #region MessageFailedToSaveFile

                await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в Excel",
                        ContentHeader = "Ошибка",
                        ContentMessage = "Не удалось сохранить файл по указанному пути:" +
                                         $"{Environment.NewLine}{fullPath}",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow);

                #endregion

                return;
            }

            if (openTemp)
            {
                Process.Start(new ProcessStartInfo { FileName = fullPath, UseShellExecute = true });
            }
            else
            {
                #region MessageExcelExportComplete

                var answer = await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions = new[]
                        {
                            new ButtonDefinition { Name = "Ок" },
                            new ButtonDefinition { Name = "Открыть выгрузку" }
                        },
                        ContentTitle = "Выгрузка в Excel",
                        ContentHeader = "Уведомление",
                        ContentMessage = "Выгрузка сохранена по пути:" +
                                         $"{Environment.NewLine}{fullPath}",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow);

                #endregion

                if (answer is "Открыть выгрузку")
                {
                    Process.Start(new ProcessStartInfo { FileName = fullPath, UseShellExecute = true });
                }
            }
        }

        #endregion

    }
}