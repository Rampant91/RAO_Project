using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.SyncCommands;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms.Forms1;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Views.Forms.Forms1;

public partial class Form_11
{
    private async void OnStandardClosing(object? sender, CancelEventArgs args)
    {
        args.Cancel = true;

        _isCloseConfirmed = true;
        if (DataContext is not Form_11VM vm) return;

        try
        {
            await CheckPeriod(vm);
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }

        var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!;
        try
        {
            var db = StaticConfiguration.DBModel;

            var modifiedEntities = db.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged);

            if (modifiedEntities.All(x => x.Entity is Report rep && rep.FormNum_DB != vm.FormType)
                || !db.ChangeTracker.HasChanges() || vm.SkipChangeTacking)
            {
                if (vm.SkipChangeTacking) vm.SkipChangeTacking = false;
                desktop.MainWindow.WindowState = OwnerPrevState;

                if (_isCloseConfirmed)
                {
                    Closing -= OnStandardClosing;
                    Close();
                }

                return;
            }
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }

        args.Cancel = true;

        var res = await ShowSaveChangesDialogAsync(vm);

        var dbm = StaticConfiguration.DBModel;
        switch (res)
        {
            case Form_11UiText.Yes:
            {
                _isCloseConfirmed = true;

                try
                {
                    await RemoveEmptyForms(vm);
                }
                catch (Exception ex)
                {
                    var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                              $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
                    ServiceExtension.LoggerManager.Error(msg);
                }

                await dbm.SaveChangesAsync();
                await new SaveReportAsyncCommand(vm).AsyncExecute(null);

                if (desktop.Windows.Count == 1)
                {
                    desktop.MainWindow.WindowState = OwnerPrevState;
                    break;
                }

                args.Cancel = false;
                break;
            }
            case Form_11UiText.No:
            {
                _isCloseConfirmed = true;
                dbm.Restore();
                new NewSortFormSyncCommand(vm).Execute(null);
                await dbm.SaveChangesAsync();

                foreach (var key in vm.Report[vm.FormType])
                {
                    var item = (Form)key;
                    if (item.Id == 0)
                        vm.Report[vm.Report.FormNum_DB].Remove(item);
                }

                foreach (var item in vm.Report.Notes.ToList<Note>().Where(item => item.Id == 0))
                    vm.Report.Notes.Remove(item);

                if (vm.FormType is not "1.0" and not "2.0")
                {
                    if (vm.FormType.Split('.')[0] == "1")
                    {
                        vm.Report.OnPropertyChanged(nameof(vm.Report.StartPeriod));
                        vm.Report.OnPropertyChanged(nameof(vm.Report.EndPeriod));
                        vm.Report.OnPropertyChanged(nameof(vm.Report.CorrectionNumber));
                    }
                    else if (vm.FormType.Split('.')[0] == "2")
                    {
                        vm.Report.OnPropertyChanged(nameof(vm.Report.Year));
                        vm.Report.OnPropertyChanged(nameof(vm.Report.CorrectionNumber));
                    }
                }
                else
                {
                    vm.Report.OnPropertyChanged(nameof(vm.Report.RegNoRep));
                    vm.Report.OnPropertyChanged(nameof(vm.Report.ShortJurLicoRep));
                    vm.Report.OnPropertyChanged(nameof(vm.Report.OkpoRep));
                }
                break;
            }
            case Form_11UiText.Cancel or null:
                _isCloseConfirmed = false;
                return;
        }

        desktop.MainWindow.WindowState = OwnerPrevState;

        if (_isCloseConfirmed)
        {
            Closing -= OnStandardClosing;
            Close();
        }
    }

    private async Task CheckPeriod(Form_11VM vm)
    {
        if (vm.Report.FormNum_DB is "1.0" or "2.0") return;
        var reps = vm.Reports;
        var reportCollection = reps.Report_Collection;
        var rep = vm.Report;
        if (DateOnly.TryParse(rep.StartPeriod_DB, out var startPeriod)
            && DateOnly.TryParse(rep.EndPeriod_DB, out var endPeriod))
        {
            foreach (var currentReport in reportCollection.Where(x => x.FormNum_DB == rep.FormNum_DB && x.Id != rep.Id))
            {
                if (DateOnly.TryParse(currentReport.StartPeriod_DB, out var currentRepStartPeriod)
                    && DateOnly.TryParse(currentReport.EndPeriod_DB, out var currentRepEndPeriod)
                    && startPeriod < currentRepEndPeriod && endPeriod > currentRepStartPeriod)
                {
                    await ShowIntersectionDialogAsync(
                        reps.Master_DB.RegNoRep.Value,
                        reps.Master_DB.OkpoRep.Value,
                        currentReport.FormNum_DB,
                        currentReport.StartPeriod_DB,
                        currentReport.EndPeriod_DB,
                        rep.StartPeriod_DB,
                        rep.EndPeriod_DB);
                    return;
                }
            }
        }
    }

    private async Task RemoveEmptyForms(Form_11VM vm)
    {
        List<Form> formToDeleteList = [];
        foreach (var form in vm.Report[vm.FormType].ToList<Form11>())
        {
            if (string.IsNullOrWhiteSpace(form.OperationCode_DB)
                && string.IsNullOrWhiteSpace(form.OperationDate_DB)
                && string.IsNullOrWhiteSpace(form.PassportNumber_DB)
                && string.IsNullOrWhiteSpace(form.Type_DB)
                && string.IsNullOrWhiteSpace(form.Radionuclids_DB)
                && string.IsNullOrWhiteSpace(form.FactoryNumber_DB)
                && form.Quantity_DB is null
                && string.IsNullOrWhiteSpace(form.Activity_DB)
                && string.IsNullOrWhiteSpace(form.CreatorOKPO_DB)
                && string.IsNullOrWhiteSpace(form.CreationDate_DB)
                && form.Category_DB is null
                && form.SignedServicePeriod_DB is null
                && form.PropertyCode_DB is null
                && string.IsNullOrWhiteSpace(form.Owner_DB)
                && form.DocumentVid_DB is null
                && string.IsNullOrWhiteSpace(form.DocumentNumber_DB)
                && string.IsNullOrWhiteSpace(form.DocumentDate_DB)
                && string.IsNullOrWhiteSpace(form.ProviderOrRecieverOKPO_DB)
                && string.IsNullOrWhiteSpace(form.TransporterOKPO_DB)
                && string.IsNullOrWhiteSpace(form.PackName_DB)
                && string.IsNullOrWhiteSpace(form.PackType_DB)
                && string.IsNullOrWhiteSpace(form.PackNumber_DB))
            {
                formToDeleteList.Add(form);
            }
        }

        if (formToDeleteList.Count == 0)
            return;

        var res = await ShowRemoveEmptyRowsDialogAsync(vm);
        if (res is not Form_11UiText.Yes)
            return;

        await using var db = new DBModel(StaticConfiguration.DBPath);
        foreach (var form in formToDeleteList)
            vm.Report.Rows.Remove(form);

        var minItem = formToDeleteList.Min(x => x.Order);
        await vm.Report.SortAsync();
        foreach (var form in vm.Report.Rows
                     .GetEnumerable()
                     .Where(x => x.Order > minItem)
                     .Select(x => (Form)x))
        {
            form.NumberInOrder_DB = (int)minItem;
            form.NumberInOrder.OnPropertyChanged();
            minItem++;
        }
        await db.SaveChangesAsync();
    }
}
