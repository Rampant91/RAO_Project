using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Collections;
using Models.DBRealization;

namespace Client_App.Commands.AsyncCommands.Delete;

public class ClearDBAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        await using var db = StaticConfiguration.DBModel;

        var form11ToDelete = db.form_11.Where(x => x.Report == null);
        foreach (var form11 in form11ToDelete)
        {
            db.form_11.Remove(form11);
        }

        var form12ToDelete = db.form_12.Where(x => x.Report == null);
        foreach (var form12 in form12ToDelete)
        {
            db.form_12.Remove(form12);
        }

        var form13ToDelete = db.form_13.Where(x => x.Report == null);
        foreach (var form13 in form13ToDelete)
        {
            db.form_13.Remove(form13);
        }

        var form14ToDelete = db.form_14.Where(x => x.Report == null);
        foreach (var form14 in form14ToDelete)
        {
            db.form_14.Remove(form14);
        }

        var form15ToDelete = db.form_15.Where(x => x.Report == null);
        foreach (var form15 in form15ToDelete)
        {
            db.form_15.Remove(form15);
        }

        var form16ToDelete = db.form_16.Where(x => x.Report == null);
        foreach (var form16 in form16ToDelete)
        {
            db.form_16.Remove(form16);
        }

        var form17ToDelete = db.form_17.Where(x => x.Report == null);
        foreach (var form17 in form17ToDelete)
        {
            db.form_17.Remove(form17);
        }

        var form18ToDelete = db.form_18.Where(x => x.Report == null);
        foreach (var form18 in form18ToDelete)
        {
            db.form_18.Remove(form18);
        }

        var form19ToDelete = db.form_19.Where(x => x.Report == null);
        foreach (var form19 in form19ToDelete)
        {
            db.form_19.Remove(form19);
        }

        var notesToDelete = db.notes.Where(x => x.Report == null);
        foreach (var note in notesToDelete)
        {
            db.notes.Remove(note);
        }

        var repToDelete = db.ReportCollectionDbSet.Where(x => x.Reports == null);
        var a = repToDelete.Count();
        foreach (var rep in repToDelete)
        {
            var form_11 = db.form_11.Where(x => x.Report.Id == rep.Id);
            db.form_11.RemoveRange(form_11);   

            var form_12 = db.form_12.Where(x => x.Report.Id == rep.Id);
            db.form_12.RemoveRange(form_12);            
            
            var form_13 = db.form_13.Where(x => x.Report.Id == rep.Id);
            db.form_13.RemoveRange(form_13);           
            
            var form_14 = db.form_14.Where(x => x.Report.Id == rep.Id);
            db.form_14.RemoveRange(form_14);     
            
            var form_15 = db.form_15.Where(x => x.Report.Id == rep.Id);
            db.form_15.RemoveRange(form_15);      
            
            var form_16 = db.form_16.Where(x => x.Report.Id == rep.Id);
            db.form_16.RemoveRange(form_16);

            var form_17 = db.form_17.Where(x => x.Report.Id == rep.Id);
            db.form_17.RemoveRange(form_17);   
            
            var form_18 = db.form_18.Where(x => x.Report.Id == rep.Id);
            db.form_18.RemoveRange(form_18);      

            var form_19 = db.form_19.Where(x => x.Report.Id == rep.Id);
            db.form_19.RemoveRange(form_19);

            var form_21 = db.form_21.Where(x => x.Report.Id == rep.Id);
            db.form_21.RemoveRange(form_21);   

            var form_22 = db.form_22.Where(x => x.Report.Id == rep.Id);
            db.form_22.RemoveRange(form_22);            
            
            var form_23 = db.form_23.Where(x => x.Report.Id == rep.Id);
            db.form_23.RemoveRange(form_23);           
            
            var form_24 = db.form_24.Where(x => x.Report.Id == rep.Id);
            db.form_24.RemoveRange(form_24);     
            
            var form_25 = db.form_25.Where(x => x.Report.Id == rep.Id);
            db.form_25.RemoveRange(form_25);      
            
            var form_26 = db.form_26.Where(x => x.Report.Id == rep.Id);
            db.form_26.RemoveRange(form_26);

            var form_27 = db.form_27.Where(x => x.Report.Id == rep.Id);
            db.form_27.RemoveRange(form_27);   
            
            var form_28 = db.form_28.Where(x => x.Report.Id == rep.Id);
            db.form_28.RemoveRange(form_28);      

            var form_29 = db.form_29.Where(x => x.Report.Id == rep.Id);
            db.form_29.RemoveRange(form_29);   

            var form_210 = db.form_210.Where(x => x.Report.Id == rep.Id);
            db.form_210.RemoveRange(form_210);   
            
            var form_211 = db.form_211.Where(x => x.Report.Id == rep.Id);
            db.form_211.RemoveRange(form_211);      

            var form_212 = db.form_212.Where(x => x.Report.Id == rep.Id);
            db.form_212.RemoveRange(form_212);
        }
        db.ReportCollectionDbSet.RemoveRange(repToDelete);

        var repsToDelete = db.ReportsCollectionDbSet.Where(x => x.DBObservable == null);
        foreach (var reps in repsToDelete)
        {
            db.ReportsCollectionDbSet.Remove(reps);
        }

        try
        {
            await db.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {

        }
    }
}
