using Client_App.ViewModels;
using Models.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.Save;
using Models.Forms;
using Models.Interfaces;
using ReactiveUI;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить N строк в форму.
/// </summary>
/// <param name="changeOrCreateViewModel">ViewModel отчёта.</param>
public class AddRowsAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;
    private string FormType => changeOrCreateViewModel.FormType;
    private Interaction<object, int> ShowDialog => changeOrCreateViewModel.ShowDialog;

    public override async Task AsyncExecute(object? parameter)
    {
        var t = await ShowDialog.Handle(Desktop.MainWindow);
        if (t > 0)
        {
            var number = GetNumberInOrder(Storage.Rows);
            var lst = new List<Form?>();
            for (var i = 0; i < t; i++)
            {
                var frm = FormCreator.Create(FormType);
                frm.NumberInOrder_DB = number;
                lst.Add(frm);
                number++;
            }
            var formContainRowAtStart = Storage.Rows.Count > 0;
            Storage.Rows.AddRange(lst);
            if (!formContainRowAtStart)
            {
                await new SaveReportAsyncCommand(changeOrCreateViewModel).AsyncExecute(null);
            }
        }
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