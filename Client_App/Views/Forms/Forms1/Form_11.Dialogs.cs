using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels.Forms.Forms1;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using System.Threading.Tasks;

namespace Client_App.Views.Forms.Forms1;

public partial class Form_11
{
    private Task<string?> ShowSaveChangesDialogAsync(Form_11VM vm) =>
        Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = Form_11UiText.Yes },
                    new ButtonDefinition { Name = Form_11UiText.No },
                    new ButtonDefinition { Name = Form_11UiText.Cancel }
                ],
                ContentTitle = Form_11UiText.SaveChangesTitle,
                ContentHeader = Form_11UiText.Notification,
                ContentMessage = Form_11UiText.SaveFormQuestion(vm.FormType),
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(this));

    private Task ShowIntersectionDialogAsync(
        string regNo, string okpo, string formNum, string start, string end, string repStart, string repEnd) =>
        Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentTitle = Form_11UiText.IntersectionTitle,
                ContentHeader = Form_11UiText.Notification,
                ContentMessage = Form_11UiText.IntersectionMessage(regNo, okpo, formNum, start, end, repStart, repEnd),
                MinWidth = 450,
                MinHeight = 170,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(this));

    private Task<string?> ShowRemoveEmptyRowsDialogAsync(Form_11VM vm) =>
        Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = Form_11UiText.Yes },
                    new ButtonDefinition { Name = Form_11UiText.No }
                ],
                ContentTitle = Form_11UiText.SaveChangesTitle,
                ContentHeader = Form_11UiText.Notification,
                ContentMessage = Form_11UiText.EmptyRowsMessage(vm.FormType),
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(this));
}
