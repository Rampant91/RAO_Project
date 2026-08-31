using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MsBox.Avalonia.Dto;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Forms.Form4;
using Models.Forms.Form5;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Client_App.ViewModels.ProgressBar;
using FirebirdSql.Data.FirebirdClient;

using MsBox.Avalonia.Enums;
namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Конвертация файлов .xlsx в отдельные файлы .RAODB (без изменения активной БД).
/// </summary>
public class ConvertExcelToRaodbAsyncCommand : BaseAsyncCommand
{
    private int NumberInOrder { get; set; } = 1;

    public override async Task AsyncExecute(object? parameter)
    {
        string[] extensions = ["xlsx", "XLSX"];
        var answer = await GetSelectedFilesFromDialog("Excel", extensions);
        if (answer is null) return;

        var cts = new CancellationTokenSource();
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        // Настройка прогрессбара
        progressBarVM.ExportType = "Конвертация .xls в .RAODB";

        var exportedCount = 0;
        var totalFiles = answer.Length;
        var errorMessages = new List<string>();

        try
        {
            await Task.Run(async () =>
            {
                for (var i = 0; i < totalFiles; i++)
                {
                    var filePath = answer[i];
                    if (string.IsNullOrEmpty(filePath)) continue;

                    var progressPercent = totalFiles == 1 ? 50 : 5 + (i * 85 / totalFiles);
                    await Dispatcher.UIThread.InvokeAsync(() =>
                        progressBarVM.SetProgressBar(progressPercent, $"Обработка файла {i + 1} из {totalFiles}"));

                    try
                    {
                        await ProcessOneFile(filePath, cts, progressBarVM);
                        exportedCount++;
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add($"Файл {Path.GetFileName(filePath)}: {ex.Message}");
                    }
                }

                await Dispatcher.UIThread.InvokeAsync(() => progressBarVM.SetProgressBar(95, "Завершение"));
            }, cts.Token);

            await progressBar.CloseAsync();

            // Показываем ошибки если есть
            if (errorMessages.Count > 0)
            {
                await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                    .GetMessageBoxStandard(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Импорт из .xlsx в .RAODB",
                        ContentHeader = "Ошибки при обработке",
                        ContentMessage = string.Join(Environment.NewLine, errorMessages),
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    }).ShowWindowDialogAsync(Desktop.MainWindow));
            }

            var suffix = exportedCount.ToString() is [.., '1'] && !exportedCount.ToString().EndsWith("11")
                ? "а"
                : "ов";
            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Импорт из .xlsx в .RAODB",
                    ContentHeader = "Уведомление",
                    ContentMessage = exportedCount > 0
                        ? $"Успешно преобразовано {exportedCount} файл{suffix} .xlsx в .RAODB."
                        : "Ни один файл не был преобразован.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                }).ShowWindowDialogAsync(Desktop.MainWindow));
        }
        catch (OperationCanceledException)
        {
            // Отмена
        }
        catch (Exception ex)
        {
            Interfaces.Logger.ServiceExtension.LoggerManager.Error($"Ошибка импорта .xlsx в .RAODB: {ex}");
        }
        finally
        {
            await progressBar.CloseAsync();
        }
    }

    private async Task ProcessOneFile(string filePath, CancellationTokenSource cts, AnyTaskProgressBarVM progressBarVM)
    {
        var sourceFile = new FileInfo(filePath);
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var excelPackage = new ExcelPackage(sourceFile);
        var worksheet0 = excelPackage.Workbook.Worksheets[0];
        var worksheet1 = excelPackage.Workbook.Worksheets[1];

        // Проверка формата
        var patternIsValid =
            (worksheet0.Name == "1.0" && Convert.ToString(worksheet0.Cells["A3"].Value)
                is "ГОСУДАОСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"
                or "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ")
            || (worksheet0.Name == "2.0"
                && Convert.ToString(worksheet0.Cells["A4"].Value)
                    is "ГОСУДАОСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"
                    or "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ")
            || (worksheet0.Name == "Форма 4.0"
                && (Convert.ToString(worksheet0.Cells["A7"].Value)
                        is "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ\n" +
                           "Конфиденциальность гарантируется получателем информации"
                    || Convert.ToString(worksheet0.Cells["A6"].Value)
                        is "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ\n" +
                           "Конфиденциальность гарантируется получателем информации"))
            || (worksheet0.Name == "Форма 5.0"
                && Convert.ToString(worksheet0.Cells["A7"].Value)
                    is "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ\n" +
                       "Конфиденциальность гарантируется получателем информации");

        if (!patternIsValid)
            throw new InvalidDataException("Не соответствует формат данных.");

        var impDateTime = DateTime.Now;
        var timeCreate = new List<string>
        {
            excelPackage.File.CreationTime.Day.ToString(),
            excelPackage.File.CreationTime.Month.ToString(),
            excelPackage.File.CreationTime.Year.ToString()
        };
        if (timeCreate[0].Length == 1) timeCreate[0] = $"0{timeCreate[0]}";
        if (timeCreate[1].Length == 1) timeCreate[1] = $"0{timeCreate[1]}";

        // Импортируем данные из титульника
        var impReps = GetImportReps(worksheet0);
        impReps.Master_DB.ReportChangedDate = impDateTime;

        // Определяем номер формы титульника
        var repNumber = worksheet0.Name;
        if (repNumber.ToLower().StartsWith("форма "))
            repNumber = repNumber.Split(' ')[1];

        // Определяем номер формы отчёта
        var formNumber = worksheet1.Name;
        if (formNumber.ToLower().StartsWith("форма "))
            formNumber = formNumber.Split(' ')[1];

        // Импортируем отчёт
        var impRep = GetReportWithDataFromExcel(worksheet0, worksheet1, formNumber, timeCreate);
        impRep.ReportChangedDate = impDateTime;

        var start = formNumber switch
        {
            "2.8" => 14,
            "4.1" => 9,
            "5.1" or "5.2" or "5.3" or "5.4" or "5.5" or "5.6" or "5.7" => 12,
            _ => 11
        };

        var end = $"A{start}";
        var value = worksheet1.Cells[end].Value;
        NumberInOrder = 1;

        while (value != null
               && Convert.ToString(value)?.ToLower() is not ("примечание:" or "примечания:" or "должность исполнителя"))
        {
            GetDataFromRow(formNumber, worksheet1, start, impRep);
            start++;
            end = $"A{start}";
            value = worksheet1.Cells[end].Value;
        }

        // Ищем примечания
        while (value is null)
        {
            start += 1;
            end = $"A{start}";
            value = worksheet1.Cells[end].Value;
        }

        if (repNumber is "1.0" or "2.0" or "5.0" && formNumber is not "5.7")
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

        // Формируем организацию с одним отчётом
        var orgWithExpForm = new Reports
        {
            Master = impReps.Master,
            Report_Collection = new ObservableCollectionWithItemPropertyChanged<Report>([impRep])
        };
        impRep.Reports = orgWithExpForm;

        // Формируем заголовок прогрессбара
        string exportName;
        var formNum = impRep.FormNum_DB;
        var formPrefix = formNum.Split('.')[0];

        if (formPrefix == "1")
        {
            exportName = $"{orgWithExpForm.Master.RegNoRep.Value}_{orgWithExpForm.Master.OkpoRep.Value}_{formNum}_{impRep.StartPeriod_DB}_{impRep.EndPeriod_DB}";
        }
        else if (formPrefix == "2")
        {
            exportName = $"{orgWithExpForm.Master.RegNoRep.Value}_{orgWithExpForm.Master.OkpoRep.Value}_{formNum}_{impRep.Year_DB}";
        }
        else if (formPrefix == "4")
        {
            exportName = $"{orgWithExpForm.Master.Rows40.OrderBy(r => r.NumberInOrder_DB).ToList()[0].CodeSubjectRF_DB}_{formNum}_{impRep.Year_DB}";
        }
        else if (formPrefix == "5")
        {
            exportName = $"{orgWithExpForm.Master.Rows50[0].Name_DB}_{formNum}_{impRep.Year_DB}";
        }
        else
        {
            exportName = $"{formNum}_{impRep.Year_DB}";
        }

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            progressBarVM.ExportName = exportName;
        });

        // Обновляем дату выгрузки
        var dt = DateTime.Now;
        var dtDay = dt.Day.ToString();
        var dtMonth = dt.Month.ToString();
        if (dtDay.Length < 2) dtDay = $"0{dtDay}";
        if (dtMonth.Length < 2) dtMonth = $"0{dtMonth}";
        impRep.ExportDate.Value = $"{dtDay}.{dtMonth}.{dt.Year}";

        // Формируем имя файла
        var filename = orgWithExpForm.Master_DB.FormNum_DB switch
        {
            "1.0" =>
                StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.RegNoRep.Value) +
                $"_{StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.OkpoRep.Value)}" +
                $"_{impRep.FormNum_DB}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(impRep.StartPeriod_DB)}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(impRep.EndPeriod_DB)}" +
                $"_{impRep.CorrectionNumber_DB}" +
                $"_{Assembly.GetExecutingAssembly().GetName().Version}",

            "2.0" when orgWithExpForm.Master.Rows20.Count > 0 =>
                StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.RegNoRep.Value) +
                $"_{StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.OkpoRep.Value)}" +
                $"_{impRep.FormNum_DB}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(impRep.Year_DB?.ToString())}" +
                $"_{impRep.CorrectionNumber_DB}" +
                $"_{Assembly.GetExecutingAssembly().GetName().Version}",

            "4.0" when orgWithExpForm.Master.Rows40.Count > 0 =>
                $"{orgWithExpForm.Master.Rows40.OrderBy(r => r.NumberInOrder_DB).ToList()[0].CodeSubjectRF_DB}" +
                $"_{impRep.FormNum_DB}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(impRep.Year_DB?.ToString())}" +
                $"_{impRep.CorrectionNumber_DB}" +
                $"_{Assembly.GetExecutingAssembly().GetName().Version}",

            "5.0" when orgWithExpForm.Master.Rows50.Count > 0 =>
                $"{impRep.FormNum_DB}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(impRep.Year_DB?.ToString())}" +
                $"_{impRep.CorrectionNumber_DB}" +
                $"_{Assembly.GetExecutingAssembly().GetName().Version}",

            _ => Path.GetFileNameWithoutExtension(filePath)
        };

        // Путь к .RAODB — в той же папке, что и исходный .xlsx
        var sourceFolder = Path.GetDirectoryName(filePath) ?? "";
        var fullPath = Path.Combine(sourceFolder, $"{filename}.RAODB");

        // Удаляем существующий файл
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        // Создаём временную БД
        var tmpDbPath = Path.Combine(BaseVM.TmpDirectory, $"{Guid.NewGuid()}.RAODB");
        await using var tempDb = new DBModel(tmpDbPath);
        await tempDb.MigrateDatabaseAsync(cts.Token);

        await tempDb.ReportsCollectionDbSet.AddAsync(orgWithExpForm, cts.Token);
        if (!tempDb.DBObservableDbSet.Any())
        {
            tempDb.DBObservableDbSet.Add(new DBObservable());
            tempDb.DBObservableDbSet.Local.First().Reports_Collection.AddRange(tempDb.ReportsCollectionDbSet.Local);
        }

        await tempDb.SaveChangesAsync(cts.Token);

        var conn = tempDb.Database.GetDbConnection() as FbConnection;
        await conn.CloseAsync();
        await conn.DisposeAsync();
        await tempDb.Database.CloseConnectionAsync();

        // Копируем в целевую папку
        File.Copy(tmpDbPath, fullPath);

        // Удаляем временный файл
        try { File.Delete(tmpDbPath); } catch { /* ignored */ }
    }

    #region GetImportReps

    private static Reports GetImportReps(ExcelWorksheet worksheet)
    {
        var name = worksheet.Name;
        if (name.ToLower().StartsWith("форма "))
            name = name.Split(' ')[1];

        var newRepsFromExcel = new Reports
        {
            Master_DB = new Report
            {
                FormNum_DB = name
            }
        };

        switch (name)
        {
            case "1.0":
            {
                var ty1 = (Form10)FormCreator.Create(name);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form10)FormCreator.Create(name);
                ty2.NumberInOrder_DB = 2;
                newRepsFromExcel.Master_DB.Rows10.Add(ty1);
                newRepsFromExcel.Master_DB.Rows10.Add(ty2);
                break;
            }
            case "2.0":
            {
                var ty1 = (Form20)FormCreator.Create(name);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form20)FormCreator.Create(name);
                ty2.NumberInOrder_DB = 2;
                newRepsFromExcel.Master_DB.Rows20.Add(ty1);
                newRepsFromExcel.Master_DB.Rows20.Add(ty2);
                break;
            }
            case "4.0":
            {
                var row40 = (Form40)FormCreator.Create(name);
                row40.NumberInOrder_DB = 1;
                newRepsFromExcel.Master_DB.Rows40.Add(row40);
                break;
            }
            case "5.0":
            {
                var row50 = (Form50)FormCreator.Create(name);
                row50.NumberInOrder_DB = 1;
                newRepsFromExcel.Master_DB.Rows50.Add(row50);
                break;
            }
        }

        GetDataTitleReps(newRepsFromExcel, worksheet);
        return newRepsFromExcel;
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

    private static string? Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length > maxLength ? value[..maxLength] : value;
    }

    #endregion

    #region GetReportWithDataFromExcel

    private static Report GetReportWithDataFromExcel(ExcelWorksheet worksheet, ExcelWorksheet worksheet1, string formNumber, List<string> timeCreate)
    {
        var impRep = new Report
        {
            FormNum_DB = formNumber,
            ExportDate_DB = $"{timeCreate[0]}.{timeCreate[1]}.{timeCreate[2]}"
        };

        if (formNumber.Split('.')[0] == "1")
        {
            impRep.StartPeriod_DB = Convert.ToString(worksheet1.Cells["G3"].Text).Replace("/", ".");
            impRep.EndPeriod_DB = Convert.ToString(worksheet1.Cells["G4"].Text).Replace("/", ".");
            impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G5"].Value);
        }
        else if (formNumber.Split('.')[0] == "2")
        {
            switch (formNumber)
            {
                case "2.6":
                    impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G4"].Value);
                    impRep.SourcesQuantity26_DB = Convert.ToInt32(worksheet1.Cells["G5"].Value);
                    impRep.Year_DB = Report.ParseYearFromImport(worksheet.Cells["G10"].Value);
                    break;
                case "2.7":
                    impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G3"].Value);
                    impRep.PermissionNumber27_DB = Convert.ToString(worksheet1.Cells["G4"].Value);
                    impRep.PermissionIssueDate27_DB = Convert.ToString(worksheet1.Cells["J4"].Value);
                    impRep.ValidBegin27_DB = Convert.ToString(worksheet1.Cells["G5"].Value);
                    impRep.ValidThru27_DB = Convert.ToString(worksheet1.Cells["J5"].Value);
                    impRep.PermissionDocumentName27_DB = Convert.ToString(worksheet1.Cells["G6"].Value);
                    impRep.Year_DB = Report.ParseYearFromImport(worksheet.Cells["G10"].Value);
                    break;
                case "2.8":
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
                    impRep.Year_DB = Report.ParseYearFromImport(worksheet.Cells["G10"].Value);
                    break;
                default:
                    impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G4"].Value);
                    impRep.Year_DB = Report.ParseYearFromImport(worksheet.Cells["G10"].Text);
                    break;
            }
        }
        else if (formNumber.Split('.')[0] == "4")
        {
            impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["B1"].Value);
            impRep.Year_DB = Report.ParseYearFromText(Convert.ToString(worksheet.Cells["B15"].Text).Trim());
        }
        else if (formNumber.Split('.')[0] == "5")
        {
            impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["B7"].Value);
            impRep.Year_DB = Report.ParseYearFromText(Convert.ToString(worksheet.Cells["B16"].Text).Trim());
        }

        // Общие данные исполнителя
        if (formNumber.Split('.')[0] is "1" or "2")
        {
            impRep.GradeExecutor_DB = Convert.ToString(worksheet1.Cells[$"D{worksheet1.Dimension.Rows - 1}"].Value);
            impRep.FIOexecutor_DB = Convert.ToString(worksheet1.Cells[$"F{worksheet1.Dimension.Rows - 1}"].Value);
            impRep.ExecPhone_DB = Convert.ToString(worksheet1.Cells[$"I{worksheet1.Dimension.Rows - 1}"].Value);
            impRep.ExecEmail_DB = Convert.ToString(worksheet1.Cells[$"K{worksheet1.Dimension.Rows - 1}"].Value);
        }
        else if (formNumber.Split('.')[0] is "4")
        {
            var address = worksheet1.Cells.FirstOrDefault(cell => Convert.ToString(cell.Value).ToLower() == "должность исполнителя").LocalAddress;
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
            var address = worksheet1.Cells.FirstOrDefault(cell => Convert.ToString(cell.Value).ToLower() == "должность").LocalAddress;
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

        return impRep;
    }

    #endregion

    #region GetDataFromRow

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

    #region GetSelectedFilesFromDialog

    private static async Task<string[]?> GetSelectedFilesFromDialog(string name, params string[] extensions)
    {
        OpenFileDialog dial = new() { AllowMultiple = true };
        var filter = new FileDialogFilter
        {
            Name = name,
            Extensions = [..extensions]
        };
        dial.Filters = [filter];
        return await dial.ShowAsync(Desktop.MainWindow);
    }

    #endregion
}