using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Messages;
using Models.Collections;
using Models.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить N строк в форму перед выбранной строкой.
/// </summary>
/// <param name="formVM">ViewModel отчёта.</param>
public class NewAddRowsInAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.Report;
    private string FormType => formVM.FormType;

    /// <param name="parameter">В качестве параметра принимает список выбранных форм</param>
    /// <returns></returns>
    public override async Task AsyncExecute(object? parameter)
    {
        var currentPageIsLastPage = formVM.CurrentPage == formVM.TotalPages;
        var collection = (IEnumerable<Form>)parameter;
        var item = collection.FirstOrDefault();
        var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
            .FirstOrDefault(w => w.IsActive);

        //Прекращение выполнения при получении некорректных данных
        if (owner == null) return;
        if (item == null) return;

        var numberCell = item.NumberInOrder_DB;
        var dialog = new AskIntMessageWindow(new AskIntMessageVM(
            $"Сколько добавить перед указанной строкой?\n" +
            $"Выбранная строка:   {numberCell}"));

        //Если пользователь отменил ввод числа, окно вернет null
        var rowCount = await dialog.ShowDialog<int?>(owner);
        //Если пользователь ничего не ввел, то прекращаем выполнение команды
        if (rowCount is null or <= 0) return;

        foreach (var key in Storage[item.FormNum_DB])
        {
            var it = (Form)key;
            if (it.NumberInOrder_DB > numberCell - 1)
            {
                it.NumberInOrder.Value = it.NumberInOrder_DB + (int)rowCount;
            }
        }
        List<Form> lst = [];
        for (var i = 0; i < rowCount; i++)
        {
            var frm = FormCreator.Create(FormType);
            frm.NumberInOrder_DB = numberCell;
            frm.Report = Storage;
            lst.Add(frm);
            numberCell++;
        }
        Storage[Storage.FormNum_DB].AddRange(lst);
        await Storage.SortAsync();

        formVM.UpdateFormList();
        formVM.UpdatePageInfo();
    }
}