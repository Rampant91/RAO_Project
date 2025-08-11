using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Messages;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Collections;
using Models.Forms;
using ReactiveUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить N строк в форму перед выбранной строкой.
/// </summary>
/// <param name="formVM">ViewModel отчёта.</param>
public class NewAddRowsInAsyncCommand(Form_12VM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.CurrentReport;
    private string FormType => formVM.FormType;

    public override async Task AsyncExecute(object? parameter)
    {
        var owner = (Window)parameter;
        if (owner == null) return;

        var param = (IEnumerable)parameter;
        var item = param.Cast<Form>().ToList().FirstOrDefault();
        if (item != null)
        {
            var numberCell = item.NumberInOrder_DB;
            var dialog = new AskIntMessageWindow(new AskIntMessageVM("Введите количество строк"));

            //Если пользователь отменил ввод числа, окно вернет null
            int? rowCount = await dialog.ShowDialog<int?>(owner);
            
            //Если пользователь ничего не ввел, то прекращаем выполнение команды
            if ((rowCount == null)||(rowCount == 0)) return;

            if (rowCount > 0)
            {
                foreach (var key in Storage[item.FormNum_DB])
                {
                    var it = (Form)key;
                    if (it.NumberInOrder_DB > numberCell - 1)
                    {
                        it.NumberInOrder.Value = it.NumberInOrder_DB + (int)rowCount;
                    }
                }
                List<Form> lst = new();
                for (var i = 0; i < rowCount; i++)
                {
                    var frm = FormCreator.Create(FormType);
                    frm.NumberInOrder_DB = numberCell;
                    lst.Add(frm);
                    numberCell++;
                }
                Storage[Storage.FormNum_DB].AddRange(lst);
                await Storage.SortAsync();
            }
        }
    }
}