using Avalonia;
using Avalonia.Input;
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter">В качестве параметра принимает список выбранных форм</param>
    /// <returns></returns>
    public override async Task AsyncExecute(object? parameter)
    {
        var forms = (IEnumerable<Form12>)parameter;
        if (forms == null || !forms.Any()) return;

        // Создаем текстовое представление (TSV - tab-separated values)
        var plainText = new StringBuilder();


        // Данные
        foreach (var form in forms)
        {
            plainText.AppendLine(
                $"{form.NumberInOrder.Value}\t" +
                $"{form.OperationCode.Value}\t" +
                $"{form.OperationDate.Value}\t" +
                $"{form.PassportNumber.Value}\t" +
                $"{form.NameIOU.Value}\t" +
                $"{form.FactoryNumber.Value}\t" +
                $"{form.Mass.Value}\t" +
                $"{form.CreatorOKPO.Value}\t" +
                $"{form.CreationDate.Value}\t" +
                $"{form.SignedServicePeriod.Value}\t" +
                $"{form.PropertyCode.Value}\t" +
                $"{form.Owner.Value}\t" +
                $"{form.DocumentVid.Value}\t" +
                $"{form.DocumentNumber.Value}\t" +
                $"{form.DocumentDate.Value}\t" +
                $"{form.ProviderOrRecieverOKPO.Value}\t" +
                $"{form.TransporterOKPO.Value}\t" +
                $"{form.PackName.Value}\t" +
                $"{form.PackType.Value}\t" +
                $"{form.PackNumber.Value}");
        }
        var clipboard = Application.Current.Clipboard;

        // Очищаем буфер обмена
        //await clipboard.ClearAsync();

        await clipboard.SetTextAsync(plainText.ToString());
    }
}