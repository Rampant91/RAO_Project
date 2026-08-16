using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Messages;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Views.Messages;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Добавить N строк в форму.
/// </summary>
public class NewAddRowsAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Storage => formVM.Report;
    private string FormType => formVM.FormType;

    public override async Task AsyncExecute(object? parameter)
    {
        bool currentPageIsLastPage = formVM.CurrentPage == formVM.TotalPages || formVM.TotalPages == 0;
        var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                .FirstOrDefault(w => w.IsActive);

        if (owner == null) return;

        var dialog = new AskIntMessageWindow(new AskIntMessageVM("Введите количество строк"));
        var rowCount = await dialog.ShowDialog<int?>(owner);

        if (rowCount > 0)
        {
            var number = await ResolveNextNumberInOrderAsync();
            var lst = new List<Form?>();
            for (var i = 0; i < rowCount; i++)
            {
                var frm = FormCreator.Create(FormType);
                frm.NumberInOrder_DB = number;
                frm.Report = Storage;
                frm.ReportId = Storage.Id;
                lst.Add(frm);
                number++;
            }
            var formContainRowAtStart = Storage.Rows.Count > 0;
            formVM.Report.Rows.AddRange(lst);
            foreach (var frm in lst)
            {
                if (frm != null)
                    FormRowsPageLoader.TrackNewFormRow(StaticConfiguration.DBModel, Storage, frm);
            }

            if (!formContainRowAtStart)
            {
                await new SaveReportAsyncCommand(formVM).AsyncExecute(null);
            }
            else if (formVM.UseDbPaging)
            {
                formVM.DbTotalRows = (formVM.DbTotalRows ?? 0) + rowCount.Value;
                formVM.IsCanSaveReportEnabled = true;
            }

            if (currentPageIsLastPage)
            {
                formVM.UpdateFormList();
            }
            formVM.UpdatePageInfo();
        }
    }

    private async Task<int> ResolveNextNumberInOrderAsync()
    {
        if (formVM.UseDbPaging && FormRowsPageLoader.SupportsDbPaging(FormType) && Storage.Id > 0)
        {
            var dbMax = await FormRowsPageLoader.GetMaxNumberInOrderAsync(
                StaticConfiguration.DBModel, Storage.Id, FormType);
            var localMax = 0;
            foreach (var item in Storage.Rows)
            {
                if (item is INumberInOrder n && n.Order > localMax)
                    localMax = (int)n.Order;
            }

            return Math.Max(dbMax, localMax) + 1;
        }

        return GetNumberInOrder(Storage.Rows);
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
