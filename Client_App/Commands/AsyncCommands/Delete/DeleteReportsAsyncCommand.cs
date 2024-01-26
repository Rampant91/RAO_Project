using System;
using System.Collections;
using System.Linq;
using Models.Collections;
using Models.DBRealization;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Client_App.Views;
using Microsoft.EntityFrameworkCore;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Commands.AsyncCommands.Delete;

//  Удалить выбранную организацию
internal class DeleteReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        #region MessageDeleteReports

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
        
        if (parameter is IEnumerable param2)
        {
            foreach (var item in param2)
            {
                var rep = item as Reports;
                rep?.Report_Collection.Clear();
                ReportsStorage.LocalReports.Reports_Collection.Remove((Reports)item);
            }
        }
        await StaticConfiguration.DBModel.SaveChangesAsync().ConfigureAwait(false);

        await using var db = new DBModel(StaticConfiguration.DBPath);
        Reports selectedReports = new();
        if (parameter is IEnumerable param)
        {
            foreach (Reports rep in param)
            {
                selectedReports = rep;
                break;
            }
        }

        if (selectedReports is null) return;

        foreach (var key in selectedReports.Report_Collection)
        {
            var rep = (Report)key;

            #region RemoveForms

            switch (rep.FormNum_DB)
            {
                #region 1.1

                case "1.1":
                    var forms11IdToDelete = db.form_11
                        .AsNoTracking()
                        .Where(form11 => form11.ReportId == rep.Id)
                        .Select(form11 => form11.Id)
                        .ToList();
                    foreach (var form11 in forms11IdToDelete.Select(formId => new Form11 { Id = formId }))
                    {
                        db.form_11.Remove(form11);
                    }
                    break;

                #endregion

                #region 1.2

                case "1.2":
                    var forms12IdToDelete = db.form_12
                        .AsNoTracking()
                        .Where(form12 => form12.ReportId == rep.Id)
                        .Select(form12 => form12.Id)
                        .ToList();
                    foreach (var form12 in forms12IdToDelete.Select(formId => new Form12 { Id = formId }))
                    {
                        db.form_12.Remove(form12);
                    }
                    break;

                #endregion

                #region 1.3

                case "1.3":
                    var forms13IdToDelete = db.form_13
                        .AsNoTracking()
                        .Where(form13 => form13.ReportId == rep.Id)
                        .Select(form13 => form13.Id)
                        .ToList();
                    foreach (var form13 in forms13IdToDelete.Select(formId => new Form13 { Id = formId }))
                    {
                        db.form_13.Remove(form13);
                    }
                    break;

                #endregion

                #region 1.4

                case "1.4":
                    var forms14IdToDelete = db.form_14
                        .AsNoTracking()
                        .Where(form14 => form14.ReportId == rep.Id)
                        .Select(form14 => form14.Id)
                        .ToList();
                    foreach (var form14 in forms14IdToDelete.Select(formId => new Form14 { Id = formId }))
                    {
                        db.form_14.Remove(form14);
                    }
                    break;

                #endregion

                #region 1.5

                case "1.5":
                    var forms15IdToDelete = db.form_15
                        .AsNoTracking()
                        .Where(form15 => form15.ReportId == rep.Id)
                        .Select(form15 => form15.Id)
                        .ToList();
                    foreach (var form15 in forms15IdToDelete.Select(formId => new Form15 { Id = formId }))
                    {
                        db.form_15.Remove(form15);
                    }
                    break;

                #endregion

                #region 1.6

                case "1.6":
                    var forms16IdToDelete = db.form_16
                        .AsNoTracking()
                        .Where(form16 => form16.ReportId == rep.Id)
                        .Select(form16 => form16.Id)
                        .ToList();
                    foreach (var form16 in forms16IdToDelete.Select(formId => new Form16 { Id = formId }))
                    {
                        db.form_16.Remove(form16);
                    }
                    break;

                #endregion

                #region 1.7

                case "1.7":
                    var forms17IdToDelete = db.form_17
                        .AsNoTracking()
                        .Where(form17 => form17.ReportId == rep.Id)
                        .Select(form17 => form17.Id)
                        .ToList();
                    foreach (var form17 in forms17IdToDelete.Select(formId => new Form17 { Id = formId }))
                    {
                        db.form_17.Remove(form17);
                    }
                    break;

                #endregion

                #region 1.8

                case "1.8":
                    var forms18IdToDelete = db.form_18
                        .AsNoTracking()
                        .Where(form18 => form18.ReportId == rep.Id)
                        .Select(form18 => form18.Id)
                        .ToList();
                    foreach (var form18 in forms18IdToDelete.Select(formId => new Form18 { Id = formId }))
                    {
                        db.form_18.Remove(form18);
                    }
                    break;

                #endregion

                #region 1.9

                case "1.9":
                    var forms19IdToDelete = db.form_19
                        .AsNoTracking()
                        .Where(form19 => form19.ReportId == rep.Id)
                        .Select(form19 => form19.Id)
                        .ToList();
                    foreach (var form19 in forms19IdToDelete.Select(formId => new Form19 { Id = formId }))
                    {
                        db.form_19.Remove(form19);
                    }
                    break;

                #endregion

                #region 2.1

                case "2.1":
                    var forms21IdToDelete = db.form_21
                        .AsNoTracking()
                        .Where(form21 => form21.ReportId == rep.Id)
                        .Select(form21 => form21.Id)
                        .ToList();
                    foreach (var form21 in forms21IdToDelete.Select(formId => new Form21 { Id = formId }))
                    {
                        db.form_21.Remove(form21);
                    }
                    break;

                #endregion

                #region 2.2

                case "2.2":
                    var forms22IdToDelete = db.form_22
                        .AsNoTracking()
                        .Where(form22 => form22.ReportId == rep.Id)
                        .Select(form22 => form22.Id)
                        .ToList();
                    foreach (var form22 in forms22IdToDelete.Select(formId => new Form22 { Id = formId }))
                    {
                        db.form_22.Remove(form22);
                    }
                    break;

                #endregion

                #region 2.3

                case "2.3":
                    var forms23IdToDelete = db.form_23
                        .AsNoTracking()
                        .Where(form23 => form23.ReportId == rep.Id)
                        .Select(form23 => form23.Id)
                        .ToList();
                    foreach (var form23 in forms23IdToDelete.Select(formId => new Form23 { Id = formId }))
                    {
                        db.form_23.Remove(form23);
                    }
                    break;

                #endregion

                #region 2.4

                case "2.4":
                    var forms24IdToDelete = db.form_24
                        .AsNoTracking()
                        .Where(form24 => form24.ReportId == rep.Id)
                        .Select(form24 => form24.Id)
                        .ToList();
                    foreach (var form24 in forms24IdToDelete.Select(formId => new Form24 { Id = formId }))
                    {
                        db.form_24.Remove(form24);
                    }
                    break;

                #endregion

                #region 2.5

                case "2.5":
                    var forms25IdToDelete = db.form_25
                        .AsNoTracking()
                        .Where(form25 => form25.ReportId == rep.Id)
                        .Select(form25 => form25.Id)
                        .ToList();
                    foreach (var form25 in forms25IdToDelete.Select(formId => new Form25 { Id = formId }))
                    {
                        db.form_25.Remove(form25);
                    }
                    break;

                #endregion

                #region 2.6

                case "2.6":
                    var forms26IdToDelete = db.form_26
                        .AsNoTracking()
                        .Where(form26 => form26.ReportId == rep.Id)
                        .Select(form26 => form26.Id)
                        .ToList();
                    foreach (var form26 in forms26IdToDelete.Select(formId => new Form26 { Id = formId }))
                    {
                        db.form_26.Remove(form26);
                    }
                    break;

                #endregion

                #region 2.7

                case "2.7":
                    var forms27IdToDelete = db.form_27
                        .AsNoTracking()
                        .Where(form27 => form27.ReportId == rep.Id)
                        .Select(form27 => form27.Id)
                        .ToList();
                    foreach (var form27 in forms27IdToDelete.Select(formId => new Form27 { Id = formId }))
                    {
                        db.form_27.Remove(form27);
                    }
                    break;

                #endregion

                #region 2.8

                case "2.8":
                    var forms28IdToDelete = db.form_28
                        .AsNoTracking()
                        .Where(form28 => form28.ReportId == rep.Id)
                        .Select(form28 => form28.Id)
                        .ToList();
                    foreach (var form28 in forms28IdToDelete.Select(formId => new Form28 { Id = formId }))
                    {
                        db.form_28.Remove(form28);
                    }
                    break;

                #endregion

                #region 2.9

                case "2.9":
                    var forms29IdToDelete = db.form_29
                        .AsNoTracking()
                        .Where(form29 => form29.ReportId == rep.Id)
                        .Select(form29 => form29.Id)
                        .ToList();
                    foreach (var form29 in forms29IdToDelete.Select(formId => new Form29 { Id = formId }))
                    {
                        db.form_29.Remove(form29);
                    }
                    break;

                #endregion

                #region 2.10

                case "2.10":
                    var forms210IdToDelete = db.form_210
                        .AsNoTracking()
                        .Where(form210 => form210.ReportId == rep.Id)
                        .Select(form210 => form210.Id)
                        .ToList();
                    foreach (var form210 in forms210IdToDelete.Select(formId => new Form210 { Id = formId }))
                    {
                        db.form_210.Remove(form210);
                    }
                    break;

                #endregion

                #region 2.11

                case "2.11":
                    var forms211IdToDelete = db.form_211
                        .AsNoTracking()
                        .Where(form211 => form211.ReportId == rep.Id)
                        .Select(form211 => form211.Id)
                        .ToList();
                    foreach (var form211 in forms211IdToDelete.Select(formId => new Form211 { Id = formId }))
                    {
                        db.form_211.Remove(form211);
                    }
                    break;

                #endregion

                #region 2.12

                case "2.12":
                    var forms212IdToDelete = db.form_212
                        .AsNoTracking()
                        .Where(form212 => form212.ReportId == rep.Id)
                        .Select(form212 => form212.Id)
                        .ToList();
                    foreach (var form212 in forms212IdToDelete.Select(formId => new Form212 { Id = formId }))
                    {
                        db.form_212.Remove(form212);
                    }
                    break;

                #endregion

                default: return;
            }
            await db.SaveChangesAsync();

            #endregion

            #region RemoveNotes

            var notesIdToDelete = db.notes
                .AsNoTracking()
                .Where(note => note.ReportId == rep.Id)
                .Select(note => note.Id)
                .ToList();
            foreach (var note in notesIdToDelete.Select(noteId => new Note { Id = noteId }))
            {
                db.notes.Remove(note);
            }

            await db.SaveChangesAsync();

            #endregion

            db.ReportCollectionDbSet.Remove(rep);

            await db.SaveChangesAsync();
        }

        #region RemoveForms10,20

        try
        {
            var form10IdToDelete = selectedReports.Master_DB.Rows10.Select(x => x.Id).ToList();
            foreach (var form10Id in form10IdToDelete.Select(formId => new Form10 { Id = formId }))
            {
                db.form_10.Remove(form10Id);
            }

            var form20IdToDelete = selectedReports.Master_DB.Rows20.Select(x => x.Id).ToList();
            foreach (var form20Id in form20IdToDelete.Select(formId => new Form20 { Id = formId }))
            {
                db.form_20.Remove(form20Id);
            }

            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {

        }

        #endregion

        #region RemoveReports

        db.ReportCollectionDbSet.Remove(new Report { Id = selectedReports.Master_DB.Id });

        await StaticConfiguration.DBModel.SaveChangesAsync();


        db.ReportsCollectionDbSet.Remove(new Reports { Id = selectedReports.Id });

        await StaticConfiguration.DBModel.SaveChangesAsync();

        #endregion
    }
}