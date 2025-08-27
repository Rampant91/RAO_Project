using Avalonia;
using Avalonia.Input;
using Models.Forms;
using Models.Forms.Form1; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Скопировать в буфер обмена выделенную строку/ячейки.
/// </summary>
public class NewCopyRowsAsyncCommand : BaseAsyncCommand
{
    //После обновления версии Авалонии нужно будет добавить копирование в формате html

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter">В качестве параметра принимает список выбранных форм</param>
    /// <returns></returns>
    public override async Task AsyncExecute(object? parameter)
    {
        var forms = (IEnumerable<Form>)parameter;
        if (forms == null || !forms.Any()) return;

        var plainText = new StringBuilder();

        foreach (var form in forms)
        {
            plainText.AppendLine(form.ConvertToTSVstring());
        }

        var clipboard = Application.Current.Clipboard;

        await clipboard.SetTextAsync(plainText.ToString());
    }
}