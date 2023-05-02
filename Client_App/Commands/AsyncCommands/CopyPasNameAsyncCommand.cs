using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Client_App.Resources;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;

namespace Client_App.Commands.AsyncCommands;

//  Скопировать в буфер обмена уникальное имя паспорта
internal class CopyPasNameAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        ChangeOrCreateVM.PassportUniqParam(parameter, out var okpo, out var type, out var date, out var pasNum, out var factoryNum);
        var year = StaticStringMethods.ConvertDateToYear(date);
        if (okpo is null or ""
            || type is null or ""
            || year is null or ""
            || pasNum is null or ""
            || factoryNum is null or "")
        {
            #region MessageFailedToCopyPasName

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Копирование",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Имя паспорта не было скопировано, не заполнены все требуемые поля:"
                                     + Environment.NewLine + "- номер паспорта (сертификата)"
                                     + Environment.NewLine + "- тип"
                                     + Environment.NewLine + "- номер"
                                     + Environment.NewLine + "- код ОКПО изготовителя"
                                     + Environment.NewLine + "- дата выпуска",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                })
                .ShowDialog(Desktop.MainWindow);

            #endregion

            return;
        }
        var uniqPasName = $"{okpo}#{type}#{year}#{pasNum}#{factoryNum}";
        uniqPasName = Regex.Replace(uniqPasName, "[\\\\/:*?\"<>|]", "_");
        uniqPasName = Regex.Replace(uniqPasName, "\\s+", "");
        await Application.Current.Clipboard.SetTextAsync(uniqPasName);
    }
}