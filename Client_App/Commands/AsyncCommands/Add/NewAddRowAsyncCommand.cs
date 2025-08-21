using Client_App.Commands.AsyncCommands.Save;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms1;
using Models.Collections;
using Models.Forms;
using Models.Interfaces;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить строку в форму.
/// </summary>
/// <param name="changeOrCreateViewModel">ViewModel отчёта.</param>
public class NewAddRowAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.CurrentReport;
    private string FormType => formVM.FormType;

    public override async Task AsyncExecute(object? parameter)
    {
        bool currentPageIsLastPage = formVM.CurrentPage == formVM.TotalPages;
        var frm = FormCreator.Create(FormType);
        frm.NumberInOrder_DB = GetNumberInOrder(Storage[Storage.FormNum_DB]);
        var formContainRowAtStart = Storage.Rows.Count > 0;
        Storage[Storage.FormNum_DB].Add(frm);
        await Storage.SortAsync();
        if (!formContainRowAtStart)
        {
            await new SaveReportAsyncCommand(formVM).AsyncExecute(null);
        }

        //Обновление DataGrid для отображения новых строк
        if(currentPageIsLastPage)
        {
            formVM.UpdateFormList();
        }
        formVM.UpdatePageInfo();
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