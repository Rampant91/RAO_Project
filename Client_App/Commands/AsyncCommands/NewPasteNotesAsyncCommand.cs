using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels.Forms;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using System.Threading.Tasks;
using Models.Forms;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Вставить значения из буфера обмена
/// </summary>
public class NewPasteNotesAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    //После обновления версии Авалонии нужно будет добавить вставку в формате html
    private Report Storage => formVM.Report;
    private Note SelectedNote => formVM.SelectedNote;
    public override async Task AsyncExecute(object? parameter)
    {
        var clipboard = Application.Current.Clipboard;

        var pastedString = await clipboard.GetTextAsync();
        if ((pastedString == null) || (pastedString == "")) return;

        var rows = pastedString.Split("\r\n");

        //Последняя строка пустая, поэтому выделяем память на одну ячейку меньше
        var parsedRows = new string[rows.Length-1][];
        for(var i =0; i< parsedRows.Length;i++)
        {
            parsedRows[i] = rows[i].Split('\t');
        }

        var start = formVM.Report.Notes.IndexOf(SelectedNote);

        if (start + parsedRows.Length > Storage.Notes.Count)
        {
            #region NotEnoughSpaceInTheTableMessage

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                       .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                       {
                           ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                           ContentTitle = $"Вставка данных из буфера обмена",
                           ContentHeader = "Внимание",
                           ContentMessage =
                               "В таблице не хватает места для некоторых строк, которые вы хотите вставить",
                           MinWidth = 400,
                           MinHeight = 150,
                           WindowStartupLocation = WindowStartupLocation.CenterOwner
                       })
                       .ShowDialog(Desktop.MainWindow)); 
            
            #endregion
        }

        for (var i = 0; i < parsedRows.Length && i+start<Storage.Notes.Count; i++)
        {
            var note = Storage.Notes.Get<Note>(i+start);

            note.RowNumber.Value = parsedRows[i][0];
            note.GraphNumber.Value = parsedRows[i][1];
            note.Comment.Value = parsedRows[i][2];
        }
    }
}