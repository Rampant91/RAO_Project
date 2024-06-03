using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Views;
using Models.DBRealization;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Client_App.Commands.AsyncCommands.Hidden;

public class UnaccountedRadAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var mainWindow = Desktop.MainWindow as MainWindow;
        var fileName = $"Отсутствующие_радионуклиды_{Assembly.GetExecutingAssembly().GetName().Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            cts.Dispose();
            return;
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        await using var db = new DBModel(StaticConfiguration.DBPath);

        var radsFromDictionaryHashSet = RadsFromFile();
        var form11Rads = db.form_11
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => x.Radionuclids_DB)
            .ToArray()
            .SelectMany(x => x
                .Replace(" ", string.Empty)
                .ToLower()
                .Replace(',', ';')
                .Split(';'))
            .ToHashSet();
        var form13Rads = db.form_13
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => x.Radionuclids_DB)
            .ToArray()
            .SelectMany(x => x
                .Replace(" ", string.Empty)
                .ToLower()
                .Replace(',', ';')
                .Split(';'))
            .ToHashSet();
        var form14Rads = db.form_14
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => x.Radionuclids_DB)
            .ToArray()
            .SelectMany(x => x
                .Replace(" ", string.Empty)
                .ToLower()
                .Replace(',', ';')
                .Split(';'))
            .ToHashSet();
        var form15Rads = db.form_15
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => x.Radionuclids_DB)
            .ToArray()
            .SelectMany(x => x
                .Replace(" ", string.Empty)
                .ToLower()
                .Replace(',', ';')
                .Split(';'))
            .ToHashSet();
        var form16Rads = db.form_16
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => x.MainRadionuclids_DB)
            .ToArray()
            .SelectMany(x => x
                .Replace(" ", string.Empty)
                .ToLower()
                .Replace(',', ';')
                .Split(';'))
        .ToHashSet();
        var form17Rads = db.form_17
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => x.Radionuclids_DB)
            .ToArray()
            .SelectMany(x => x
                .Replace(" ", string.Empty)
                .ToLower()
                .Replace(',', ';')
                .Split(';'))
            .ToHashSet();
        var form18Rads = db.form_18
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => x.Radionuclids_DB)
            .ToArray()
            .SelectMany(x => x
                .Replace(" ", string.Empty)
                .ToLower()
                .Replace(',', ';')
                .Split(';'))
            .ToHashSet();
        var form19Rads = db.form_19
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => x.Radionuclids_DB)
            .ToArray()
            .SelectMany(x => x
                .Replace(" ", string.Empty)
                .ToLower()
                .Replace(',', ';')
                .Split(';'))
            .ToHashSet();
        var formsRads = form11Rads
            .Union(form13Rads)
            .Union(form14Rads)
            .Union(form15Rads)
            .Union(form16Rads)
            .Union(form17Rads)
            .Union(form18Rads)
            .Union(form19Rads);
        var uniqRads = formsRads
            .Where(x => !radsFromDictionaryHashSet.Contains(x))
            .ToArray();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        Worksheet = excelPackage.Workbook.Worksheets.Add("Отсутствующие радионуклиды");

        var line = 2;
        Worksheet.Cells[1, 1].Value = "Наименование";
        foreach (var rad in uniqRads)
        {
            Worksheet.Cells[line, 1].Value = rad;
            line++;
        }
        Worksheet.View.FreezePanes(2, 1);
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #region RFromFile

    private HashSet<string> RadsFromFile()
    {
        string filePath;
        HashSet<string> radsFromDictionaryHashSet = [];
#if DEBUG
        filePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx");
#else
        filePath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"R.xlsx");
#endif
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(filePath)) return radsFromDictionaryHashSet;
        FileInfo excelImportFile = new(filePath);
        var xls = new ExcelPackage(excelImportFile);
        var worksheet = xls.Workbook.Worksheets["Лист1"];
        var i = 2;
        while (worksheet.Cells[i, 1].Text != string.Empty)
        {
            radsFromDictionaryHashSet.Add(worksheet.Cells[i, 1].Text);
            i++;
        }
        return radsFromDictionaryHashSet;
    }

    #endregion
}
