using System;
using Models.Collections;
using Models.DBRealization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;

namespace Client_App.Commands.AsyncCommands.Delete;

//  Удалить выбранную организацию
internal class DeleteReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        #region MessageExcelExportComplete

        var answer = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" }
                },
                ContentTitle = "Уведомление",
                ContentHeader = "Уведомление",
                ContentMessage = "Вы действительно хотите удалить организацию?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow);

        #endregion

        if (answer is not "Да") return;

        var dbContext = StaticConfiguration.DBModel;
        if (parameter is IEnumerable param)
        {
            var iter = param.GetEnumerator();
            iter.MoveNext();
            var id = ((Reports)iter.Current).Id;
            var reports = dbContext.ReportsCollectionDbSet.FirstOrDefault(reps => reps.Id == id);
            switch (reports.Master_DB.FormNum_DB)
            {
                case "1.0":
                    dbContext.form_10.RemoveRange(reports.Master_DB.Rows10);
                    break;
                case "2.0":
                    dbContext.form_20.RemoveRange(reports.Master_DB.Rows20);
                    break;
            }
            dbContext.ReportsCollectionDbSet.Remove(reports);
            dbContext.ReportCollectionDbSet.RemoveRange(reports.Report_Collection);
            foreach (var key in reports.Report_Collection)
            {
                var rep = (Report)key;
                switch (rep.FormNum_DB)
                {
                    case "1.1":
                    {
                        dbContext.form_11.RemoveRange(rep.Rows11);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "1.2":
                    {
                        dbContext.form_12.RemoveRange(rep.Rows12);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "1.3":
                    {
                        dbContext.form_13.RemoveRange(rep.Rows13);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "1.4":
                    {
                        dbContext.form_14.RemoveRange(rep.Rows14);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "1.5":
                    {
                        dbContext.form_15.RemoveRange(rep.Rows15);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "1.6":
                    {
                        dbContext.form_16.RemoveRange(rep.Rows16);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "1.7":
                    {
                        dbContext.form_17.RemoveRange(rep.Rows17);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "1.8":
                    {
                        dbContext.form_18.RemoveRange(rep.Rows18);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "1.9":
                    {
                        dbContext.form_19.RemoveRange(rep.Rows19);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.1":
                    {
                        dbContext.form_21.RemoveRange(rep.Rows21);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.2":
                    {
                        dbContext.form_22.RemoveRange(rep.Rows22);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.3":
                    {
                        dbContext.form_23.RemoveRange(rep.Rows23);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.4":
                    {
                        dbContext.form_24.RemoveRange(rep.Rows24);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.5":
                    {
                        dbContext.form_25.RemoveRange(rep.Rows25);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.6":
                    {
                        dbContext.form_26.RemoveRange(rep.Rows26);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.7":
                    {
                        dbContext.form_27.RemoveRange(rep.Rows27);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.8":
                    {
                        dbContext.form_28.RemoveRange(rep.Rows28);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.9":
                    {
                        dbContext.form_29.RemoveRange(rep.Rows29);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.10":
                    {
                        dbContext.form_210.RemoveRange(rep.Rows210);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.11":
                    {
                        dbContext.form_211.RemoveRange(rep.Rows211);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                    case "2.12":
                    {
                        dbContext.form_212.RemoveRange(rep.Rows212);
                        dbContext.notes.RemoveRange(rep.Notes);
                        break;
                    }
                }
            }
        }
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}