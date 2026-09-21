using System.Linq;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Helpers.MasterTitleRows;

/// <summary>
/// Безопасное добавление пустых шаблонов титула: только если в БД/Local
/// по ReportId реально нет строк (не путать с пустой navigation-коллекцией).
/// </summary>
public static class MasterTitleRowsFillEmptyGuard
{
    /// <summary>
    /// Для Form 1.0: привязать строки из Local при необходимости;
    /// добавить пару Ord=1/2 только если строк нет ни в коллекции, ни в Local.
    /// </summary>
    public static void EnsureForm10Templates(DataContext dbm, Report master)
    {
        if (master is null || master.FormNum_DB != "1.0")
        {
            return;
        }

        AttachLocalForm10(dbm, master);
        if (master.Rows10.Count > 0)
        {
            return;
        }

        if (HasLocalForm10(dbm, master.Id))
        {
            AttachLocalForm10(dbm, master);
            if (master.Rows10.Count > 0)
            {
                return;
            }
        }

        var ty1 = (Form10)FormCreator.Create("1.0");
        ty1.NumberInOrder_DB = 1;
        var ty2 = (Form10)FormCreator.Create("1.0");
        ty2.NumberInOrder_DB = 2;
        master.Rows10.Add(ty1);
        master.Rows10.Add(ty2);
    }

    /// <summary>
    /// Для Form 2.0 — зеркало EnsureForm10Templates.
    /// </summary>
    public static void EnsureForm20Templates(DataContext dbm, Report master)
    {
        if (master is null || master.FormNum_DB != "2.0")
        {
            return;
        }

        AttachLocalForm20(dbm, master);
        if (master.Rows20.Count > 0)
        {
            return;
        }

        if (HasLocalForm20(dbm, master.Id))
        {
            AttachLocalForm20(dbm, master);
            if (master.Rows20.Count > 0)
            {
                return;
            }
        }

        var ty1 = (Form20)FormCreator.Create("2.0");
        ty1.NumberInOrder_DB = 1;
        var ty2 = (Form20)FormCreator.Create("2.0");
        ty2.NumberInOrder_DB = 2;
        master.Rows20.Add(ty1);
        master.Rows20.Add(ty2);
    }

    /// <summary>
    /// Вариант без проверки FormNum (как в DeleteReports): не плодить, если Local уже содержит строки.
    /// </summary>
    public static void EnsureForm10TemplatesIfCollectionEmpty(DataContext dbm, Report master)
    {
        if (master is null)
        {
            return;
        }

        AttachLocalForm10(dbm, master);
        if (master.Rows10.Count > 0)
        {
            return;
        }

        if (HasLocalForm10(dbm, master.Id))
        {
            AttachLocalForm10(dbm, master);
            if (master.Rows10.Count > 0)
            {
                return;
            }
        }

        var ty1 = (Form10)FormCreator.Create("1.0");
        ty1.NumberInOrder_DB = 1;
        var ty2 = (Form10)FormCreator.Create("1.0");
        ty2.NumberInOrder_DB = 2;
        master.Rows10.Add(ty1);
        master.Rows10.Add(ty2);
    }

    public static void EnsureForm20TemplatesIfCollectionEmpty(DataContext dbm, Report master)
    {
        if (master is null)
        {
            return;
        }

        AttachLocalForm20(dbm, master);
        if (master.Rows20.Count > 0)
        {
            return;
        }

        if (HasLocalForm20(dbm, master.Id))
        {
            AttachLocalForm20(dbm, master);
            if (master.Rows20.Count > 0)
            {
                return;
            }
        }

        var ty1 = (Form20)FormCreator.Create("2.0");
        ty1.NumberInOrder_DB = 1;
        var ty2 = (Form20)FormCreator.Create("2.0");
        ty2.NumberInOrder_DB = 2;
        master.Rows20.Add(ty1);
        master.Rows20.Add(ty2);
    }

    private static bool HasLocalForm10(DataContext dbm, int reportId) =>
        reportId > 0 && dbm.form_10.Local.Any(f => f.ReportId == reportId);

    private static bool HasLocalForm20(DataContext dbm, int reportId) =>
        reportId > 0 && dbm.form_20.Local.Any(f => f.ReportId == reportId);

    private static void AttachLocalForm10(DataContext dbm, Report master)
    {
        if (master.Id <= 0 || master.Rows10.Count > 0)
        {
            return;
        }

        foreach (var row in dbm.form_10.Local.Where(f => f.ReportId == master.Id).ToList())
        {
            if (!master.Rows10.Contains(row))
            {
                master.Rows10.Add(row);
            }
        }
    }

    private static void AttachLocalForm20(DataContext dbm, Report master)
    {
        if (master.Id <= 0 || master.Rows20.Count > 0)
        {
            return;
        }

        foreach (var row in dbm.form_20.Local.Where(f => f.ReportId == master.Id).ToList())
        {
            if (!master.Rows20.Contains(row))
            {
                master.Rows20.Add(row);
            }
        }
    }
}
