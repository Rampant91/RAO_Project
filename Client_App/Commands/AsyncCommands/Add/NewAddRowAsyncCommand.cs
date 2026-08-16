using Client_App.Commands.AsyncCommands.Save;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.Forms;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить строку в форму.
/// </summary>
/// <param name="formVM">ViewModel отчёта.</param>
public class NewAddRowAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.Report;
    private string FormType => formVM.FormType;

    public override async Task AsyncExecute(object? parameter)
    {
        var currentPageIsLastPage = formVM.CurrentPage == formVM.TotalPages || formVM.TotalPages == 0;
        var frm = FormCreator.Create(FormType);
        frm.NumberInOrder_DB = await ResolveNextNumberInOrderAsync();
        frm.Report = Storage;
        frm.ReportId = Storage.Id;

        var formContainRowAtStart = Storage.Rows.Count > 0;
        Storage[Storage.FormNum_DB].Add(frm);

        // Paging грузит страницы AsNoTracking — без явного Add SaveChanges строку не увидит.
        FormRowsPageLoader.TrackNewFormRow(StaticConfiguration.DBModel, Storage, frm);

        await Storage.SortAsync();
        if (!formContainRowAtStart)
        {
            await new SaveReportAsyncCommand(formVM).AsyncExecute(null);
        }
        else if (formVM.UseDbPaging)
        {
            formVM.DbTotalRows = (formVM.DbTotalRows ?? 0) + 1;
            formVM.IsCanSaveReportEnabled = true;
        }

        if (currentPageIsLastPage)
        {
            formVM.UpdateFormList();
        }
        formVM.UpdatePageInfo();
        Debug.WriteLine(this);
    }

    private async Task<int> ResolveNextNumberInOrderAsync()
    {
        if (formVM.UseDbPaging && FormRowsPageLoader.SupportsDbPaging(FormType) && Storage.Id > 0)
        {
            var dbMax = await FormRowsPageLoader.GetMaxNumberInOrderAsync(
                StaticConfiguration.DBModel, Storage.Id, FormType);
            var localMax = 0;
            foreach (var item in Storage[Storage.FormNum_DB])
            {
                if (item is INumberInOrder n && n.Order > localMax)
                    localMax = (int)n.Order;
            }

            return Math.Max(dbMax, localMax) + 1;
        }

        return GetNumberInOrder(Storage[Storage.FormNum_DB]);
    }

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
