﻿using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Properties;
using Client_App.Resources;
using MessageBox.Avalonia.DTO;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Найти и открыть соответствующий файл паспорта в сетевом хранилище.
/// </summary>
public partial class OpenPasAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        StaticMethods.PassportUniqParam(parameter, out var okpo, out var type, out var date, out var pasNum, out var factoryNum);
        var year = StaticStringMethods.ConvertDateToYear(date);
        if (okpo is null or ""
            || type is null or ""
            || year is null or ""
            || pasNum is null or ""
            || factoryNum is null or "")
        {
            #region MessageUnableToOpenPassport

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Уведомление",
                        "Паспорт не может быть открыт, поскольку не заполнены или заполнены некорректно все требуемые поля:"
                        + Environment.NewLine + "- номер паспорта (сертификата);"
                        + Environment.NewLine + "- тип;"
                        + Environment.NewLine + "- номер;"
                        + Environment.NewLine + "- код ОКПО изготовителя;"
                        + Environment.NewLine + "- дата выпуска;")
                    .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }
        
        var uniqPasName = $"{okpo}#{type}#{year}#{pasNum}#{factoryNum}.pdf";
        uniqPasName = SpecialCharactersRegex().Replace(uniqPasName, "_").Replace(" ", "");

        var pasFolderPath = Settings.Default.PasFolderDefaultPath;
        if (!Path.Exists(pasFolderPath))
        {
            #region MessagePasportFileMissing

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Поиск файла паспорта",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Сетевое хранилище недоступно:" +
                                     $"{Environment.NewLine}{pasFolderPath}" +
                                     $"{Environment.NewLine}Для изменения пути по умолчанию, воспользуйтесь кнопкой " +
                                     $"{Environment.NewLine}\"Excel -> Паспорта -> Изменить расположение паспортов по умолчанию\".",
                    MinWidth = 475,
                    MinHeight = 175,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(Desktop.MainWindow));

            #endregion
        }

        var pasFullPath = Directory.EnumerateFiles(pasFolderPath, uniqPasName, SearchOption.AllDirectories).FirstOrDefault() is not null
            ? Directory.EnumerateFiles(pasFolderPath, uniqPasName, SearchOption.AllDirectories).FirstOrDefault()
            : Directory.EnumerateFiles(pasFolderPath, StaticStringMethods.TranslateToEng(uniqPasName), SearchOption.AllDirectories).FirstOrDefault() is not null
                ? Directory.EnumerateFiles(pasFolderPath, StaticStringMethods.TranslateToEng(uniqPasName), SearchOption.AllDirectories).FirstOrDefault()
                : Directory.EnumerateFiles(pasFolderPath, StaticStringMethods.TranslateToRus(uniqPasName)).FirstOrDefault();

        if (pasFullPath is not null)
        {
            Process.Start(new ProcessStartInfo { FileName = pasFullPath, UseShellExecute = true });
        }
        else
        {
            #region MessagePasportFileMissing

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Поиск файла паспорта",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Паспорт {uniqPasName}" +
                                     $"{Environment.NewLine}отсутствует в сетевом хранилище:" +
                                     $"{Environment.NewLine}{Settings.Default.PasFolderDefaultPath}",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(Desktop.MainWindow));

            #endregion
        }
    }

    [GeneratedRegex("[\\\\/:*?\"<>|]")]
    private static partial Regex SpecialCharactersRegex();
}