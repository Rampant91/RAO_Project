using System.Collections;
using Client_App.Views;
using Models.Collections;
using Models.DBRealization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Forms.Form1;

namespace Client_App.Commands.AsyncCommands.Delete;

//  Удалить выбранную форму у выбранной организации
internal class DeleteFormAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        #region MessageDeleteReport

        var answer = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Да", IsDefault = true },
                    new ButtonDefinition { Name = "Нет", IsCancel = false }
                },
                ContentTitle = "Уведомление",
                ContentHeader = "Уведомление",
                ContentMessage = "Вы действительно хотите удалить отчет?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow);

        #endregion

        if (answer is "Да")
        {
            var mainWindow = Desktop.MainWindow as MainWindow;
            Report selectedReport = new();
            if (parameter is IEnumerable param)
            {
                foreach (Report rep in param)
                {
                    selectedReport = rep;
                    break;
                }
            }
            if (selectedReport is null) return;
            await using var db = new DBModel(StaticConfiguration.DBPath);

            var forms11IdToDelete = db.form_11.Where(x => x.ReportId == selectedReport.Id).Select(x => x.Id).ToList();
            foreach (var form11 in forms11IdToDelete.Select(formId => new Form11 { Id = formId }))
            {
                db.form_11.Remove(form11);
            }
            var forms12IdToDelete = db.form_11.Where(x => x.ReportId == selectedReport.Id).Select(x => x.Id).ToList();
            foreach (var form12 in forms12IdToDelete.Select(formId => new Form12 { Id = formId }))
            {
                db.form_12.Remove(form12);
            }
            var forms13IdToDelete = db.form_11.Where(x => x.ReportId == selectedReport.Id).Select(x => x.Id).ToList();
            foreach (var form13 in forms13IdToDelete.Select(formId => new Form13 { Id = formId }))
            {
                db.form_13.Remove(form13);
            }
            var forms14IdToDelete = db.form_11.Where(x => x.ReportId == selectedReport.Id).Select(x => x.Id).ToList();
            foreach (var form14 in forms14IdToDelete.Select(formId => new Form14 { Id = formId }))
            {
                db.form_14.Remove(form14);
            }
            var forms15IdToDelete = db.form_11.Where(x => x.ReportId == selectedReport.Id).Select(x => x.Id).ToList();
            foreach (var form15 in forms15IdToDelete.Select(formId => new Form15 { Id = formId }))
            {
                db.form_15.Remove(form15);
            }
            var forms16IdToDelete = db.form_11.Where(x => x.ReportId == selectedReport.Id).Select(x => x.Id).ToList();
            foreach (var form16 in forms16IdToDelete.Select(formId => new Form16 { Id = formId }))
            {
                db.form_16.Remove(form16);
            }
            var forms17IdToDelete = db.form_11.Where(x => x.ReportId == selectedReport.Id).Select(x => x.Id).ToList();
            foreach (var form17 in forms17IdToDelete.Select(formId => new Form17 { Id = formId }))
            {
                db.form_17.Remove(form17);
            }
            var forms18IdToDelete = db.form_11.Where(x => x.ReportId == selectedReport.Id).Select(x => x.Id).ToList();
            foreach (var form18 in forms18IdToDelete.Select(formId => new Form18 { Id = formId }))
            {
                db.form_18.Remove(form18);
            }
            var forms19IdToDelete = db.form_11.Where(x => x.ReportId == selectedReport.Id).Select(x => x.Id).ToList();
            foreach (var form19 in forms19IdToDelete.Select(formId => new Form19 { Id = formId }))
            {
                db.form_19.Remove(form19);
            }

            db.ReportCollectionDbSet.Remove(selectedReport);
            var selectedReports = mainWindow!.SelectedReports.First() as Reports;
            selectedReports!.Report_Collection.Remove(selectedReport);
            await db.SaveChangesAsync();
        }
    }
}