using Avalonia;
using Models.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Скопировать в буфер обмена выделенную строку/ячейки.
/// </summary>
public class NewCopyNotesAsyncCommand : BaseAsyncCommand
{
    //После обновления версии Авалонии нужно будет добавить копирование в формате html

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter">В качестве параметра принимает список выбранных форм</param>
    /// <returns></returns>
    public override async Task AsyncExecute(object? parameter)
    {
        var notes = (IEnumerable<Note>)parameter;
        if (notes == null || !notes.Any()) return;

        var plainText = new StringBuilder();

        foreach (var note in notes)
        {
            plainText.AppendLine(note.ConvertToTSVstring());
        }

        var clipboard = Application.Current.Clipboard;

        await clipboard.SetTextAsync(plainText.ToString());
    }
}