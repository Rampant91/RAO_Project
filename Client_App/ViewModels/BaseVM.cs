using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using OfficeOpenXml;
using System.IO;
using System.Threading;
using Models.Collections;
using Models.Forms.Form1;

namespace Client_App.ViewModels;

public class BaseVM
{
    private protected static string PasFolderPath = @"Y:\!!! Поручения\Паспорта ЗРИ 2022\Хранилище паспортов ЗРИ";

    internal const string Version = @"1.2.2.11";

    internal static string DbFileName = "Local_0";

    internal static string SystemDirectory = "";

    private protected static string RaoDirectory = "";

    private protected static string TmpDirectory = "";

    private protected static bool ComparePasParam(string? nameDb, string? namePas)
    {
        if (nameDb == null || namePas == null)
        {
            return nameDb == null && namePas == null;
        }
        nameDb = Regex.Replace(nameDb, "[\\\\/:*?\"<>|]", "");
        nameDb = Regex.Replace(nameDb, "\\s+", "");
        namePas = Regex.Replace(namePas, "[\\\\/:*?\"<>|]", "");
        namePas = Regex.Replace(namePas, "\\s+", "");
        return nameDb.Equals(namePas, StringComparison.OrdinalIgnoreCase)
               || TranslateToEng(nameDb).Equals(TranslateToEng(namePas), StringComparison.OrdinalIgnoreCase)
               || TranslateToRus(nameDb).Equals(TranslateToRus(namePas), StringComparison.OrdinalIgnoreCase);
    }

    private protected static string ConvertPrimToDash(string? num)
    {
        if (num == null)
        {
            return "";
        }
        if (num.Contains("прим", StringComparison.OrdinalIgnoreCase)
            || num.Equals("бн", StringComparison.OrdinalIgnoreCase)
            || num.Equals("бп", StringComparison.OrdinalIgnoreCase)
            || num.Contains("без", StringComparison.OrdinalIgnoreCase)
            || num.Contains("нет", StringComparison.OrdinalIgnoreCase)
            || num.Contains("отсут", StringComparison.OrdinalIgnoreCase))
        {
            return "-";
        }
        return num;
    }

    private protected static string ConvertDateToYear(string? date)
    {
        return DateTime.TryParse(date, out var dateTime) 
            ? dateTime.Year.ToString()
            : "0000";
    }

    internal static string InventoryCheck(Report? rep)
    {
        if (rep is null)
        {
            return "";
        }

        var countCode10 = 0;
        foreach (var key in rep.Rows)
        {
            if (key is Form1 { OperationCode_DB: "10" })
            {
                countCode10++;
            }
        }

        return countCode10 == rep.Rows.Count
            ? " (ИНВ)"
            : countCode10 > 0
                ? " (инв)"
                : "";
    }

    private protected static string RemoveForbiddenChars(string str)
    {
        str = str.Replace(Environment.NewLine, "").Trim();
        str = Regex.Replace(str, "[\\\\/:*?\"<>|]", "");
        return str;
    }

    internal static string StringReverse(string str)
    {
        var charArray = str.Replace("_", "0").Replace("/", ".").Split(".");
        Array.Reverse(charArray);
        return string.Join("", charArray);
    }

    private protected static string TranslateToEng(string pasName)
    {
        Dictionary<string, string> dictRusToEng = new()
        {
            {"а", "a"},
            {"А", "A"},
            {"е", "e"},
            {"Е", "E"},
            {"к", "k"},
            {"К", "K"},
            {"м", "m"},
            {"М", "M"},
            {"о", "o"},
            {"О", "O"},
            {"р", "p"},
            {"Р", "P"},
            {"с", "c"},
            {"С", "C"},
            {"Т", "T"},
            {"у", "y"},
            {"У", "Y"},
            {"х", "x"},
            {"Х", "X"},
        };
        var newPasName = "";
        foreach (var ch in pasName)
        {
            if (dictRusToEng.TryGetValue(ch.ToString(), out var ss)) newPasName += ss;
            else newPasName += ch;
        }
        return newPasName;
    }

    private protected static string TranslateToRus(string pasName)
    {
        Dictionary<string, string> dictEngToRus = new()
        {
            {"a", "а"},
            {"A", "А"},
            {"e", "е"},
            {"E", "Е"},
            {"k", "к"},
            {"K", "К"},
            {"m", "м"},
            {"M", "М"},
            {"o", "о"},
            {"O", "О"},
            {"p", "р"},
            {"P", "Р"},
            {"c", "с"},
            {"C", "С"},
            {"T", "Т"},
            {"y", "у"},
            {"Y", "У"},
            {"x", "х"},
            {"X", "Х"},
        };
        var newPasName = "";
        foreach (var ch in pasName)
        {
            if (dictEngToRus.TryGetValue(ch.ToString(), out var ss)) newPasName += ss;
            else newPasName += ch;
        }
        return newPasName;
    }

    private protected async Task<string?> RunCommandInBush(string command)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };
        process.Start();
        await process.StandardInput.WriteLineAsync(command);
        return await process.StandardOutput.ReadLineAsync();
    }

    #region ExcelGetFullPath

    private protected async Task<(string fullPath, bool openTemp)> ExcelGetFullPath(string fileName, CancellationTokenSource cts)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            cts.Cancel();
            cts.Token.ThrowIfCancellationRequested();
            return ("", false);
        }

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
            .ShowDialog(desktop.MainWindow);

        #endregion

        var fullPath = "";
        var openTemp = res is "Открыть временную копию";

        switch (res)
        {
            case "Открыть временную копию":
                {
                    DirectoryInfo tmpFolder = new(Path.Combine(Path.Combine(SystemDirectory, "RAO"), "temp"));
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
                    fullPath = await dial.ShowAsync(desktop.MainWindow);
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
                                .ShowDialog(desktop.MainWindow);

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

    #region ExcelSaveAndOpen

    private protected static async Task ExcelSaveAndOpen(ExcelPackage excelPackage, string fullPath, bool openTemp)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
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
                .ShowDialog(desktop.MainWindow);

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
                .ShowDialog(desktop.MainWindow);

            #endregion

            if (answer is "Открыть выгрузку")
            {
                Process.Start(new ProcessStartInfo { FileName = fullPath, UseShellExecute = true });
            }
        }
    }

    #endregion

    
}