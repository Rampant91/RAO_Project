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
public class NewCopyRowsAsyncCommand() : BaseAsyncCommand
{
    //После обновления версии Авалонии нужно будет добавить копирование в формате html

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter">В качестве параметра принимает список выбранных форм</param>
    /// <returns></returns>
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        var forms = (IEnumerable<Form>)parameter;
        if (forms == null || !forms.Any()) return;

        var plainText = new StringBuilder();

        foreach (var form in forms)
        {
            plainText.AppendLine(form.ConvertToTSVstring());
        }

        var clipboard = Application.Current!.Clipboard;
        if (clipboard != null) 
        {
            await clipboard.SetTextAsync(plainText.ToString());
        }
    }
}