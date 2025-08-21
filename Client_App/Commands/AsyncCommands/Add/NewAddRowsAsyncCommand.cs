using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Messages;
using Models.Collections;
using Models.Forms;
using Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить N строк в форму.
/// </summary>
/// <param name="changeOrCreateViewModel">ViewModel отчёта.</param>
public class NewAddRowsAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.CurrentReport;
    private string FormType => formVM.FormType;

    public override async Task AsyncExecute(object? parameter)
    {
        bool currentPageIsLastPage = formVM.CurrentPage == formVM.TotalPages;
        // Если не получили окно напрямую, попробуем найти активное окно
        var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                .FirstOrDefault(w => w.IsActive);

        if (owner == null) return;

        var dialog = new AskIntMessageWindow(new AskIntMessageVM("Введите количество строк"));
        int? rowCount = await dialog.ShowDialog<int?>(owner); 


        if (rowCount == null) return;
        if (rowCount > 0)
        {
            var number = GetNumberInOrder(Storage.Rows);
            var lst = new List<Form?>();
            for (var i = 0; i < rowCount; i++)
            {
                var frm = FormCreator.Create(FormType);
                frm.NumberInOrder_DB = number;
                lst.Add(frm);
                number++;
            }
            var formContainRowAtStart = Storage.Rows.Count > 0;
            formVM.CurrentReport.Rows.AddRange(lst);
            if (!formContainRowAtStart)
            {
                await new SaveReportAsyncCommand(formVM).AsyncExecute(null);
            }

            if (currentPageIsLastPage)
            {
                formVM.UpdateFormList();
            }
            formVM.UpdatePageInfo();
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