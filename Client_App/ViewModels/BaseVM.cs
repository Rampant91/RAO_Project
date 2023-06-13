using System;
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

namespace Client_App.ViewModels;

public class BaseVM
{
    internal static string PasFolderPath = @"Y:\!!! Поручения\Паспорта ЗРИ 2022\Хранилище паспортов ЗРИ";

    internal const string Version = @"1.2.2.11";

    internal static string DbFileName = "Local_0";

    internal static string LogsDirectory = "";

    internal static string SystemDirectory = "";

    internal static string RaoDirectory = "";

    internal static string TmpDirectory = "";

    private bool _IsBusy;
    public bool IsBusy { get; set; }

    //  Запускает баш скрипт с введенной командой
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
                    DirectoryInfo tmpFolder = new(Path.Combine(SystemDirectory, "RAO", "temp"));
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