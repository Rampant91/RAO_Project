using Client_App.ViewModels;
using Models.Collections;
using Models.Forms;
using Models.Interfaces;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить примечание в отчёт.
/// </summary>
/// <param name="changeOrCreateViewModel">ViewModel отчёта.</param>
public class AddNoteAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;

    public override async Task AsyncExecute(object? parameter)
    {
        Note nt = new() { Order = GetNumberInOrder(Storage.Notes) };
        Storage.Notes.Add(nt);
        await Storage.SortAsync();
    }

    /// <summary>
    /// Получить порядковый номер
    /// </summary>
    /// <param name="lst">Список элементов</param>
    /// <returns>Порядковый номер</returns>
    private static int GetNumberInOrder(IKeyCollection lst)
    {
        var maxNum = 0;
        foreach (var item in lst)
        {
            var frm = (INumberInOrder)item;
            if (frm.Order >= maxNum)
            {
                maxNum++;
            }
        }
        return maxNum + 1;
    }
}