using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Client_App.ViewModels;
using Models.Collections;
using Models.Forms.Form1;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.SourceTransmission;

public abstract class SourceTransmissionBaseAsyncCommand : BaseAsyncCommand
{
    private protected Reports SelectedReports => ChangeOrCreateViewModel.Storages;
    private protected Report SelectedReport => ChangeOrCreateViewModel.Storage;

    public required ChangeOrCreateVM ChangeOrCreateViewModel;

    #region GetActivities

    private protected static Dictionary<string, string> GetActivities(Form1 form1, IReadOnlyList<string> nuclidTypeArray)
    {
        var tritiumActivity = "-";
        var betaGammaActivity = "-";
        var alphaActivity = "-";
        var transuraniumActivity = "-";

        var activityTmp = form1 switch
        {
            Form13 form13 => form13.Activity_DB ?? "",
            Form14 form14 => form14.Activity_DB ?? "",
            _ => "-"
        };
        activityTmp = activityTmp
            .Replace(".", ",")
            .Replace("(", "")
            .Replace(")", "");

        var activity = double.TryParse(activityTmp,
            NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
            CultureInfo.CreateSpecificCulture("ru-RU"),
            out var activityDoubleValue)
            ? $"{activityDoubleValue:0.######################################################e+00}"
            : activityTmp;

        if (nuclidTypeArray.Count == 1
            || nuclidTypeArray.Count > 1
            && nuclidTypeArray
                .Skip(1)
                .All(x => string.Equals(nuclidTypeArray[0], x)))
        {
            switch (nuclidTypeArray[0])
            {
                case "а":
                    alphaActivity = activity;
                    break;
                case "б":
                    betaGammaActivity = activity;
                    break;
                case "т":
                    tritiumActivity = activity;
                    break;
                case "у":
                    transuraniumActivity = activity;
                    break;
            }
        }
        return new Dictionary<string, string>
        {
            { "alpha", alphaActivity },
            { "beta", betaGammaActivity },
            { "tritium", tritiumActivity },
            { "transuranium", transuraniumActivity }
        };
    }

    #endregion

    #region GetCodeRao

    private protected static string GetCodeRao(Form1 form, IEnumerable<string> nuclidsArray, string[] nuclidTypeArray)
    {
        var thirdSymbolCodeRao = GetThirdSymbolCodeRao(nuclidTypeArray);
        var fifthSymbolCodeRao = GetFifthSymbolCodeRao(nuclidsArray);
        var ninthTenthSymbols = "__";
        var agrState = "_";
        switch (form)
        {
            case Form13 form13:
                {
                    agrState = form13.AggregateState_DB != null
                        ? form13.AggregateState_DB.ToString()![..1]
                        : "";
                    ninthTenthSymbols = "84";
                    break;
                }
            case Form14 form14:
                {
                    agrState = form14.AggregateState_DB != null
                        ? form14.AggregateState_DB.ToString()![..1]
                        : "";
                    break;
                }
        }
        return $"{agrState}_{thirdSymbolCodeRao}1{fifthSymbolCodeRao}_00{ninthTenthSymbols}_";
    }

    #region GetThirdSymbolCodeRao

    private static string GetThirdSymbolCodeRao(string[] nuclidTypeArray)
    {
        var thirdSymbolCodeRao = "0";
        if (nuclidTypeArray.Contains("а")
            && (nuclidTypeArray.Contains("б") || nuclidTypeArray.Contains("т"))
            && nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "6";
        }
        else if (nuclidTypeArray.Contains("а")
                 && (nuclidTypeArray.Contains("б") || nuclidTypeArray.Contains("т"))
                 && !nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "5";
        }
        else if (!nuclidTypeArray.Contains("а")
                 && (nuclidTypeArray.Contains("б") || nuclidTypeArray.Contains("т"))
                 && !nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "4";
        }
        else if (nuclidTypeArray.Contains("а")
                 && !nuclidTypeArray.Contains("б") && !nuclidTypeArray.Contains("т")
                 && nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "3";
        }
        else if (nuclidTypeArray.Contains("а")
                 && !nuclidTypeArray.Contains("б") && !nuclidTypeArray.Contains("т")
                 && !nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "2";
        }
        else if (!nuclidTypeArray.Contains("а")
                 && !nuclidTypeArray.Contains("б") && !nuclidTypeArray.Contains("т")
                 && nuclidTypeArray.Contains("у"))
        {
            thirdSymbolCodeRao = "1";
        }
        return thirdSymbolCodeRao;
    }

    #endregion

    #region GetFifthSymbolCodeRao

    private static string GetFifthSymbolCodeRao(IEnumerable<string> nuclidsArray)
    {
        double maxPeriod = 0;
        foreach (var nuclidName in nuclidsArray)
        {
            var nuclidDictionary = R.FirstOrDefault(x => x["name"] == nuclidName);
            if (nuclidDictionary == null) continue;

            var unit = nuclidDictionary["periodUnit"];
            var periodValue = nuclidDictionary["periodValue"].Replace('.', ',');
            if (!double.TryParse(periodValue,
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                    CultureInfo.CreateSpecificCulture("ru-RU"),
                    out var halfLife
                )) continue;
            switch (unit)
            {
                case "лет":
                    break;
                case "сут":
                    halfLife /= 365;
                    break;
                case "час":
                    halfLife /= 8760;   //365*24
                    break;
                case "мин":
                    halfLife /= 525_600;   //365*24*60
                    break;
                default: continue;
            }
            if (halfLife > maxPeriod)
            {
                maxPeriod = halfLife;
            }
        }
        return maxPeriod > 31
            ? "1"
            : "2";
    }

    #endregion

    #endregion

    #region GetNewReport

    private protected Report GetNewReport(DateOnly opDate, string formNum)
    {
        var relevantFormNum = formNum == "1.1"
            ? "1.5"
            : "1.6";

        #region GetDates

        var startDateList = SelectedReports.Report_Collection
            .Where(x => x.FormNum_DB == relevantFormNum)
            .Select(x => DateOnly.TryParse(x.StartPeriod_DB, out var startDate)
                ? startDate
                : DateOnly.MinValue)
            .ToList();
        var endDateList = SelectedReports.Report_Collection
            .Where(x => x.FormNum_DB == relevantFormNum)
            .Select(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate)
                ? endDate
                : DateOnly.MinValue)
            .ToList();
        var closestStartDate = startDateList.Any(x => x >= opDate)
            ? startDateList
                .Where(x => x >= opDate)
                .Min()
            : DateOnly.MaxValue;
        var closestEndDate = endDateList.Any(x => x < opDate)
            ? endDateList
                .Where(x => x < opDate)
                .Max()
            : DateOnly.MinValue;
        var firstRepStartDate = SelectedReports.Report_Collection
            .Any(x => x.FormNum_DB == relevantFormNum)
            ? SelectedReports.Report_Collection
                .Where(x => x.FormNum_DB == relevantFormNum)
                .Select(x => DateOnly.TryParse(x.StartPeriod_DB, out var startDate)
                    ? startDate
                    : DateOnly.MaxValue)
                .Min()
            : DateOnly.MaxValue;
        var lastRepEndDate = SelectedReports.Report_Collection
            .Any(x => x.FormNum_DB == relevantFormNum)
            ? SelectedReports.Report_Collection
                .Where(x => x.FormNum_DB == relevantFormNum)
                .Select(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate)
                    ? endDate
                    : DateOnly.MinValue)
                .Max()
            : DateOnly.MinValue;

        #endregion

        var newRep = new Report()
        {
            FormNum_DB = relevantFormNum,
            CorrectionNumber_DB = 0,
            ExecEmail_DB = SelectedReport.ExecEmail_DB,
            ExecPhone_DB = SelectedReport.ExecPhone_DB,
            GradeExecutor_DB = SelectedReport.GradeExecutor_DB,
            FIOexecutor_DB = SelectedReport.FIOexecutor_DB
        };
        if (SelectedReports.Report_Collection.All(x => x.FormNum_DB != relevantFormNum)
            || firstRepStartDate == DateOnly.MaxValue)  //Форм нет или не парсится ни одна дата начала
        {
            newRep.StartPeriod_DB = $"01.01.{opDate.Year}";
            newRep.EndPeriod_DB = DateTime.Now.ToShortDateString();
        }
        else if (firstRepStartDate > opDate)    //Самая ранняя форма начинается позднее даты операции
        {
            newRep.StartPeriod_DB = $"01.01.{opDate.Year}";
            newRep.EndPeriod_DB = firstRepStartDate.ToShortDateString();
        }
        else if (lastRepEndDate < opDate)   //Самая поздняя форма заканчивается ранее даты операции
        {
            newRep.StartPeriod_DB = lastRepEndDate.ToShortDateString();
            newRep.EndPeriod_DB = DateTime.Now.ToShortDateString();
        }
        else   //Дата операции в разрыве между формами
        {
            newRep.StartPeriod_DB = closestEndDate.ToShortDateString();
            newRep.EndPeriod_DB = closestStartDate.ToShortDateString();
        }
        return newRep;
    }

    #endregion

    #region RFromFile

    private protected static List<Dictionary<string, string>> R = new();

    private protected static void R_Populate_From_File()
    {
        if (R.Count != 0) return;
        var filePath = string.Empty;
#if DEBUG
        filePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx");
#else
        filePath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"R.xlsx");
#endif
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(filePath)) return;
        FileInfo excelImportFile = new(filePath);
        var xls = new ExcelPackage(excelImportFile);
        var worksheet = xls.Workbook.Worksheets["Лист1"];
        var i = 2;
        R.Clear();
        while (worksheet.Cells[i, 1].Text != string.Empty)
        {
            R.Add(new Dictionary<string, string>
            {
                {"name", worksheet.Cells[i, 1].Text},
                {"periodValue", worksheet.Cells[i, 5].Text},
                {"periodUnit", worksheet.Cells[i, 6].Text},
                {"code", worksheet.Cells[i, 8].Text}
            });
            i++;
        }
    }

    #endregion
}