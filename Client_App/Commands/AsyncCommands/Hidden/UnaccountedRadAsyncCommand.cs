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

        #region GetRadsHashSets

        await using var db = new DBModel(StaticConfiguration.DBPath);
        var radsFromDictionaryHashSet = RadsFromFile();

        var dto11List = db.form_11
            .Where(x => x.Report != null && x.Report.Reports != null && x.Report.Reports.Master_DB != null)
            .Include(x => x.Report.Reports.Master_DB).ThenInclude(x => x.Rows10)
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => new UnaccountedRadsDTO
            {
                RegNo = x.Report.Reports.Master_DB.RegNoRep.Value,
                OKPO = x.Report.Reports.Master_DB.OkpoRep.Value,
                StartPeriod = x.Report.StartPeriod_DB,
                EndPeriod = x.Report.EndPeriod_DB,
                FormNum = x.FormNum_DB,
                Rad = x.Radionuclids_DB
            })
            .ToArray()
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad)
            .Select(dto => new
            {
                dto,
                radList = dto.Rad
                    .Replace(" ", string.Empty)
                    .ToLower()
                    .Replace(',', ';')
                    .Split(';')
            })
            .SelectMany(x => x.radList,
                (x, rad) => new UnaccountedRadsDTO
                {
                    RegNo = x.dto.RegNo,
                    OKPO = x.dto.OKPO,
                    StartPeriod = x.dto.StartPeriod,
                    EndPeriod = x.dto.EndPeriod,
                    FormNum = x.dto.FormNum,
                    Rad = rad
                })
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad);

        var dto13List = db.form_13
            .Where(x => x.Report != null && x.Report.Reports != null && x.Report.Reports.Master_DB != null)
            .Include(x => x.Report.Reports.Master_DB).ThenInclude(x => x.Rows10)
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => new UnaccountedRadsDTO
            {
                RegNo = x.Report.Reports.Master_DB.RegNoRep.Value,
                OKPO = x.Report.Reports.Master_DB.OkpoRep.Value,
                StartPeriod = x.Report.StartPeriod_DB,
                EndPeriod = x.Report.EndPeriod_DB,
                FormNum = x.FormNum_DB,
                Rad = x.Radionuclids_DB
            })
            .ToArray()
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad)
            .Select(dto => new
            {
                dto,
                radList = dto.Rad
                    .Replace(" ", string.Empty)
                    .ToLower()
                    .Replace(',', ';')
                    .Split(';')
            })
            .SelectMany(x => x.radList,
                (x, rad) => new UnaccountedRadsDTO
                {
                    RegNo = x.dto.RegNo,
                    OKPO = x.dto.OKPO,
                    StartPeriod = x.dto.StartPeriod,
                    EndPeriod = x.dto.EndPeriod,
                    FormNum = x.dto.FormNum,
                    Rad = rad
                })
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad);

        var dto14List = db.form_14
            .Where(x => x.Report != null && x.Report.Reports != null && x.Report.Reports.Master_DB != null)
            .Include(x => x.Report.Reports.Master_DB).ThenInclude(x => x.Rows10)
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => new UnaccountedRadsDTO
            {
                RegNo = x.Report.Reports.Master_DB.RegNoRep.Value,
                OKPO = x.Report.Reports.Master_DB.OkpoRep.Value,
                StartPeriod = x.Report.StartPeriod_DB,
                EndPeriod = x.Report.EndPeriod_DB,
                FormNum = x.FormNum_DB,
                Rad = x.Radionuclids_DB
            })
            .ToArray()
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad)
            .Select(dto => new
            {
                dto,
                radList = dto.Rad
                    .Replace(" ", string.Empty)
                    .ToLower()
                    .Replace(',', ';')
                    .Split(';')
            })
            .SelectMany(x => x.radList,
                (x, rad) => new UnaccountedRadsDTO
                {
                    RegNo = x.dto.RegNo,
                    OKPO = x.dto.OKPO,
                    StartPeriod = x.dto.StartPeriod,
                    EndPeriod = x.dto.EndPeriod,
                    FormNum = x.dto.FormNum,
                    Rad = rad
                })
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad);

        var dto15List = db.form_15
            .Where(x => x.Report != null && x.Report.Reports != null && x.Report.Reports.Master_DB != null)
            .Include(x => x.Report.Reports.Master_DB).ThenInclude(x => x.Rows10)
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => new UnaccountedRadsDTO
            {
                RegNo = x.Report.Reports.Master_DB.RegNoRep.Value,
                OKPO = x.Report.Reports.Master_DB.OkpoRep.Value,
                StartPeriod = x.Report.StartPeriod_DB,
                EndPeriod = x.Report.EndPeriod_DB,
                FormNum = x.FormNum_DB,
                Rad = x.Radionuclids_DB
            })
            .ToArray()
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad)
            .Select(dto => new
            {
                dto,
                radList = dto.Rad
                    .Replace(" ", string.Empty)
                    .ToLower()
                    .Replace(',', ';')
                    .Split(';')
            })
            .SelectMany(x => x.radList,
                (x, rad) => new UnaccountedRadsDTO
                {
                    RegNo = x.dto.RegNo,
                    OKPO = x.dto.OKPO,
                    StartPeriod = x.dto.StartPeriod,
                    EndPeriod = x.dto.EndPeriod,
                    FormNum = x.dto.FormNum,
                    Rad = rad
                })
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad);

        var dto16List = db.form_16
            .Where(x => x.Report != null && x.Report.Reports != null && x.Report.Reports.Master_DB != null)
            .Include(x => x.Report.Reports.Master_DB).ThenInclude(x => x.Rows10)
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => new UnaccountedRadsDTO
            {
                RegNo = x.Report.Reports.Master_DB.RegNoRep.Value,
                OKPO = x.Report.Reports.Master_DB.OkpoRep.Value,
                StartPeriod = x.Report.StartPeriod_DB,
                EndPeriod = x.Report.EndPeriod_DB,
                FormNum = x.FormNum_DB,
                Rad = x.MainRadionuclids_DB
            })
            .ToArray()
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad)
            .Select(dto => new
            {
                dto,
                radList = dto.Rad
                    .Replace(" ", string.Empty)
                    .ToLower()
                    .Replace(',', ';')
                    .Split(';')
            })
            .SelectMany(x => x.radList,
                (x, rad) => new UnaccountedRadsDTO
                {
                    RegNo = x.dto.RegNo,
                    OKPO = x.dto.OKPO,
                    StartPeriod = x.dto.StartPeriod,
                    EndPeriod = x.dto.EndPeriod,
                    FormNum = x.dto.FormNum,
                    Rad = rad
                })
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad);

        var dto17List = db.form_17
            .Where(x => x.Report != null && x.Report.Reports != null && x.Report.Reports.Master_DB != null)
            .Include(x => x.Report.Reports.Master_DB).ThenInclude(x => x.Rows10)
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => new UnaccountedRadsDTO
            {
                RegNo = x.Report.Reports.Master_DB.RegNoRep.Value,
                OKPO = x.Report.Reports.Master_DB.OkpoRep.Value,
                StartPeriod = x.Report.StartPeriod_DB,
                EndPeriod = x.Report.EndPeriod_DB,
                FormNum = x.FormNum_DB,
                Rad = x.Radionuclids_DB
            })
            .ToArray()
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad)
            .Select(dto => new
            {
                dto,
                radList = dto.Rad
                    .Replace(" ", string.Empty)
                    .ToLower()
                    .Replace(',', ';')
                    .Split(';')
            })
            .SelectMany(x => x.radList,
                (x, rad) => new UnaccountedRadsDTO
                {
                    RegNo = x.dto.RegNo,
                    OKPO = x.dto.OKPO,
                    StartPeriod = x.dto.StartPeriod,
                    EndPeriod = x.dto.EndPeriod,
                    FormNum = x.dto.FormNum,
                    Rad = rad
                })
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad);

        var dto18List = db.form_18
            .Where(x => x.Report != null && x.Report.Reports != null && x.Report.Reports.Master_DB != null)
            .Include(x => x.Report.Reports.Master_DB).ThenInclude(x => x.Rows10)
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => new UnaccountedRadsDTO
            {
                RegNo = x.Report.Reports.Master_DB.RegNoRep.Value,
                OKPO = x.Report.Reports.Master_DB.OkpoRep.Value,
                StartPeriod = x.Report.StartPeriod_DB,
                EndPeriod = x.Report.EndPeriod_DB,
                FormNum = x.FormNum_DB,
                Rad = x.Radionuclids_DB
            })
            .ToArray()
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad)
            .Select(dto => new
            {
                dto,
                radList = dto.Rad
                    .Replace(" ", string.Empty)
                    .ToLower()
                    .Replace(',', ';')
                    .Split(';')
            })
            .SelectMany(x => x.radList,
                (x, rad) => new UnaccountedRadsDTO
                {
                    RegNo = x.dto.RegNo,
                    OKPO = x.dto.OKPO,
                    StartPeriod = x.dto.StartPeriod,
                    EndPeriod = x.dto.EndPeriod,
                    FormNum = x.dto.FormNum,
                    Rad = rad
                })
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad);

        var dto19List = db.form_19
            .Where(x => x.Report != null && x.Report.Reports != null && x.Report.Reports.Master_DB != null)
            .Include(x => x.Report.Reports.Master_DB).ThenInclude(x => x.Rows10)
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Select(x => new UnaccountedRadsDTO
            {
                RegNo = x.Report.Reports.Master_DB.RegNoRep.Value,
                OKPO = x.Report.Reports.Master_DB.OkpoRep.Value,
                StartPeriod = x.Report.StartPeriod_DB,
                EndPeriod = x.Report.EndPeriod_DB,
                FormNum = x.FormNum_DB,
                Rad = x.Radionuclids_DB
            })
            .ToArray()
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad)
            .Select(dto => new
            {
                dto,
                radList = dto.Rad
                    .Replace(" ", string.Empty)
                    .ToLower()
                    .Replace(',', ';')
                    .Split(';')
            })
            .SelectMany(x => x.radList,
                (x, rad) => new UnaccountedRadsDTO
                {
                    RegNo = x.dto.RegNo,
                    OKPO = x.dto.OKPO,
                    StartPeriod = x.dto.StartPeriod,
                    EndPeriod = x.dto.EndPeriod,
                    FormNum = x.dto.FormNum,
                    Rad = rad
                })
            .DistinctBy(x => x.RegNo + x.OKPO + x.StartPeriod + x.EndPeriod + x.FormNum + x.Rad);

        var dtoList = new List<UnaccountedRadsDTO>();
        dtoList.AddRange(dto11List);
        dtoList.AddRange(dto13List);
        dtoList.AddRange(dto14List);
        dtoList.AddRange(dto15List);
        dtoList.AddRange(dto16List);
        dtoList.AddRange(dto17List);
        dtoList.AddRange(dto18List);
        dtoList.AddRange(dto19List);
        var b = dtoList.GroupBy(x => x.Rad);

        //var form11Rads = db.form_11
        //    .AsNoTracking()
        //    .AsSplitQuery()
        //    .AsQueryable()
        //    .Select(x => x.Radionuclids_DB)
        //    .ToArray()
        //    .SelectMany(x => x
        //        .Replace(" ", string.Empty)
        //        .ToLower()
        //        .Replace(',', ';')
        //        .Split(';'))
        //    .ToHashSet();
        //var form13Rads = db.form_13
        //    .AsNoTracking()
        //    .AsSplitQuery()
        //    .AsQueryable()
        //    .Select(x => x.Radionuclids_DB)
        //    .ToArray()
        //    .SelectMany(x => x
        //        .Replace(" ", string.Empty)
        //        .ToLower()
        //        .Replace(',', ';')
        //        .Split(';'))
        //    .ToHashSet();
        //var form14Rads = db.form_14
        //    .AsNoTracking()
        //    .AsSplitQuery()
        //    .AsQueryable()
        //    .Select(x => x.Radionuclids_DB)
        //    .ToArray()
        //    .SelectMany(x => x
        //        .Replace(" ", string.Empty)
        //        .ToLower()
        //        .Replace(',', ';')
        //        .Split(';'))
        //    .ToHashSet();
        //var form15Rads = db.form_15
        //    .AsNoTracking()
        //    .AsSplitQuery()
        //    .AsQueryable()
        //    .Select(x => x.Radionuclids_DB)
        //    .ToArray()
        //    .SelectMany(x => x
        //        .Replace(" ", string.Empty)
        //        .ToLower()
        //        .Replace(',', ';')
        //        .Split(';'))
        //    .ToHashSet();
        //var form16Rads = db.form_16
        //    .AsNoTracking()
        //    .AsSplitQuery()
        //    .AsQueryable()
        //    .Select(x => x.MainRadionuclids_DB)
        //    .ToArray()
        //    .SelectMany(x => x
        //        .Replace(" ", string.Empty)
        //        .ToLower()
        //        .Replace(',', ';')
        //        .Split(';'))
        //.ToHashSet();
        //var form17Rads = db.form_17
        //    .AsNoTracking()
        //    .AsSplitQuery()
        //    .AsQueryable()
        //    .Select(x => x.Radionuclids_DB)
        //    .ToArray()
        //    .SelectMany(x => x
        //        .Replace(" ", string.Empty)
        //        .ToLower()
        //        .Replace(',', ';')
        //        .Split(';'))
        //    .ToHashSet();
        //var form18Rads = db.form_18
        //    .AsNoTracking()
        //    .AsSplitQuery()
        //    .AsQueryable()
        //    .Select(x => x.Radionuclids_DB)
        //    .ToArray()
        //    .SelectMany(x => x
        //        .Replace(" ", string.Empty)
        //        .ToLower()
        //        .Replace(',', ';')
        //        .Split(';'))
        //    .ToHashSet();
        //var form19Rads = db.form_19
        //    .AsNoTracking()
        //    .AsSplitQuery()
        //    .AsQueryable()
        //    .Select(x => x.Radionuclids_DB)
        //    .ToArray()
        //    .SelectMany(x => x
        //        .Replace(" ", string.Empty)
        //        .ToLower()
        //        .Replace(',', ';')
        //        .Split(';'))
        //    .ToHashSet();
        //var formsRads = form11Rads
        //    .Union(form13Rads)
        //    .Union(form14Rads)
        //    .Union(form15Rads)
        //    .Union(form16Rads)
        //    .Union(form17Rads)
        //    .Union(form18Rads)
        //    .Union(form19Rads);
        //var uniqRads = formsRads
        //    .Where(x => !radsFromDictionaryHashSet.Contains(x))
        //    .ToArray();

        #endregion

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        Worksheet = excelPackage.Workbook.Worksheets.Add("Отсутствующие радионуклиды");

        var line = 2;
        Worksheet.Cells[1, 1].Value = "Наименование";
        Worksheet.Cells[1, 2].Value = "Рег.№";
        Worksheet.Cells[1, 3].Value = "ОКПО";
        Worksheet.Cells[1, 4].Value = "Начало периода";
        Worksheet.Cells[1, 5].Value = "Конец периода";
        Worksheet.Cells[1, 6].Value = "Номер формы";
        foreach (var group in b)
        {
            foreach (var dto in group)
            {
                    Worksheet.Cells[line, 1].Value = dto.Rad;
                    Worksheet.Cells[line, 2].Value = dto.RegNo;
                    Worksheet.Cells[line, 3].Value = dto.OKPO;
                    Worksheet.Cells[line, 4].Value = dto.StartPeriod;
                    Worksheet.Cells[line, 5].Value = dto.EndPeriod;
                    Worksheet.Cells[line, 6].Value = dto.FormNum;
                    line++;
            }
            
            //var regNoList = db.ReportsCollectionDbSet
            //    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            //    .AsNoTracking()
            //    .AsSplitQuery()
            //    .AsQueryable()
            //    .Where(reps => reps.Report_Collection
            //        .Any(rep => rep.Rows11.Any(form11 => form11.Radionuclids_DB == rad)
            //                    || rep.Rows13.Any(form13 => form13.Radionuclids_DB == rad)
            //                    || rep.Rows14.Any(form14 => form14.Radionuclids_DB == rad)
            //                    || rep.Rows15.Any(form15 => form15.Radionuclids_DB == rad)
            //                    || rep.Rows16.Any(form16 => form16.MainRadionuclids_DB == rad)
            //                    || rep.Rows17.Any(form17 => form17.Radionuclids_DB == rad)
            //                    || rep.Rows18.Any(form18 => form18.Radionuclids_DB == rad)
            //                    || rep.Rows19.Any(form19 => form19.Radionuclids_DB == rad)))
            //    .Select(x => x.Master_DB.RegNoRep.Value)
            //    .ToList();
            //Worksheet.Cells[line, 2].Value = string.Join(", ", regNoList);
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

    private class UnaccountedRadsDTO
    {
        public string Rad;

        public string RegNo;

        public string OKPO;

        public string FormNum;

        public string StartPeriod;

        public string EndPeriod;
    }
}