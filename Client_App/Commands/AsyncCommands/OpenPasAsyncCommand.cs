using Avalonia.Controls;
using Client_App.Resources;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

//  Найти и открыть соответствующий файл паспорта в сетевом хранилище
public class OpenPasAsyncCommand : BaseAsyncCommand
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

            await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Уведомление",
                        "Паспорт не может быть открыт, поскольку не заполнены или заполнены некорректно все требуемые поля:"
                        + Environment.NewLine + "- номер паспорта (сертификата);"
                        + Environment.NewLine + "- тип;"
                        + Environment.NewLine + "- номер;"
                        + Environment.NewLine + "- код ОКПО изготовителя;"
                        + Environment.NewLine + "- дата выпуска;")
                    .ShowDialog(Desktop.MainWindow);

            #endregion

            return;
        }
        
        var uniqPasName = $"{okpo}#{type}#{year}#{pasNum}#{factoryNum}.pdf";
        uniqPasName = Regex.Replace(uniqPasName, "[\\\\/:*?\"<>|]", "_");
        uniqPasName = Regex.Replace(uniqPasName, @"\s+", "");

        var pasFullPath = Directory.EnumerateFiles(BaseVM.PasFolderPath, uniqPasName, SearchOption.AllDirectories).FirstOrDefault() is not null
            ? Directory.EnumerateFiles(BaseVM.PasFolderPath, uniqPasName, SearchOption.AllDirectories).FirstOrDefault()
            : Directory.EnumerateFiles(BaseVM.PasFolderPath, StaticStringMethods.TranslateToEng(uniqPasName), SearchOption.AllDirectories).FirstOrDefault() is not null
                ? Directory.EnumerateFiles(BaseVM.PasFolderPath, StaticStringMethods.TranslateToEng(uniqPasName), SearchOption.AllDirectories).FirstOrDefault()
                : Directory.EnumerateFiles(BaseVM.PasFolderPath, StaticStringMethods.TranslateToRus(uniqPasName)).FirstOrDefault();

        if (pasFullPath is not null)
        {
            Process.Start(new ProcessStartInfo { FileName = pasFullPath, UseShellExecute = true });
        }
        else
        {
            #region MessagePasportFileMissing

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Поиск файла паспорта",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Паспорт {uniqPasName}" +
                                     $"{Environment.NewLine}отсутствует в сетевом хранилище:" +
                                     $"{Environment.NewLine}{BaseVM.PasFolderPath}",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(Desktop.MainWindow);

            #endregion
        }
    }
}