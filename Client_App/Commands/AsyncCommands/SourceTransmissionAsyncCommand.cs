using Client_App.ViewModels;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.Classes;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Interfaces;
using OfficeOpenXml;
using System.IO;
using DynamicData;

namespace Client_App.Commands.AsyncCommands;

// Перевод источника из РВ в РАО
public class SourceTransmissionAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Reports SelectedReports => changeOrCreateViewModel.Storages;
    private Report SelectedReport => changeOrCreateViewModel.Storage;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        var form = ((IKeyCollection)parameter).Get<Form1>(0);
        if (!string.Equals(form.OperationCode_DB.Trim(), "41", StringComparison.Ordinal))
        {
            #region MessageSourceTransmissionFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Передача источника",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Перевод источника в РАО осуществляется кодом операции 41",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }
        if (!DateOnly.TryParse(form.OperationDate_DB, out var opDate))
        {
            #region MessageSourceTransmissionFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Передача источника",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Некорректно введена дата операции",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }

        var repInRange = SelectedReports.Report_Collection
            .Where(rep => (form.FormNum_DB == "1.1" && rep.FormNum_DB == "1.5" 
                           || form.FormNum_DB == "1.2" && rep.FormNum_DB == "1.6"
                           || form.FormNum_DB == "1.3" && rep.FormNum_DB == "1.6"
                           || form.FormNum_DB == "1.4" && rep.FormNum_DB == "1.6")
                          && (DateOnly.TryParse(rep.StartPeriod_DB, out var repStartDate)
                              && DateOnly.TryParse(rep.EndPeriod_DB, out var repEndDate)
                              && opDate >= repStartDate && opDate <= repEndDate
                              || (DateOnly.TryParse(rep.StartPeriod_DB, out repStartDate) 
                                  && !DateOnly.TryParse(rep.EndPeriod_DB, out _)
                                  && opDate >= repStartDate)))
            .OrderBy(x => x.EndPeriod_DB)
            .ToList();
        if (repInRange.Count == 2 
            && !DateOnly.TryParse(repInRange[0].EndPeriod_DB, out _) 
            && DateOnly.TryParse(repInRange[1].EndPeriod_DB, out _))
        {
            repInRange.Remove(repInRange[0]);
        }

        await using var db = new DBModel(StaticConfiguration.DBPath);
        switch (repInRange.Count)
        {
            case > 1:   // У организации по ошибке есть несколько отчётов с нужным периодом
            {
                #region MessageSourceTransmissionFailed

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Перевод источника в РАО",
                        ContentHeader = "Ошибка",
                        ContentMessage = "У выбранной организации присутствуют отчёты по форме 1.5 с пересекающимися периодами. " +
                                         $"{Environment.NewLine}Устраните данное несоответствие перед операцией перевода источника в РАО.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                return;
            }
            case 1:   // Если есть подходящий отчет, то добавляем форму в него
            {
                var rep = repInRange.First();
                switch (form.FormNum_DB)
                {
                    case "1.1":
                    {
                        var form11 = (Form11)form;
                        var newForm15 = new Form15
                        {
                            #region BindingData
                
                            ReportId = rep.Id,
                            NumberInOrder_DB = await db.ReportCollectionDbSet
                                .AsNoTracking()
                                .AsSplitQuery()
                                .AsQueryable()
                                .Where(x => x.Id == rep.Id)
                                .Include(x => x.Rows15)
                                .SelectMany(x => x.Rows15)
                                .CountAsync() + 1,
                            OperationCode_DB = form11.OperationCode_DB,
                            OperationDate_DB = form11.OperationDate_DB,
                            PassportNumber_DB = form11.PassportNumber_DB,
                            Type_DB = form11.Type_DB,
                            Radionuclids_DB = form11.Radionuclids_DB,
                            FactoryNumber_DB = form11.FactoryNumber_DB,
                            Activity_DB = form11.Activity_DB,
                            Quantity_DB = form11.Quantity_DB,
                            CreationDate_DB = form11.CreationDate_DB,
                            StatusRAO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                            DocumentVid_DB = form11.DocumentVid_DB,
                            DocumentNumber_DB = form11.DocumentNumber_DB,
                            DocumentDate_DB = form11.DocumentDate_DB,
                            ProviderOrRecieverOKPO_DB = form11.ProviderOrRecieverOKPO_DB,
                            TransporterOKPO_DB = form11.TransporterOKPO_DB,
                            PackName_DB = form11.PackName_DB,
                            PackType_DB = form11.PackType_DB,
                            PackNumber_DB = form11.PackNumber_DB,
                            RefineOrSortRAOCode_DB = "-",
                            Subsidy_DB = "-",
                            FcpNumber_DB = "-"

                            #endregion
                        };
                        db.form_15.Add(newForm15);
                        break;
                    }
                    case "1.2":
                    {
                        var form12 = (Form12)form;
                        var numberInOrder = await db.ReportCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == rep.Id)
                            .Include(x => x.Rows16)
                            .SelectMany(x => x.Rows16)
                            .CountAsync() + 1;
                        var massTmp = (form12.Mass_DB ?? "")
                            .Replace(".", ",")
                            .Replace("(", "")
                            .Replace(")", "");
                        var massTon = double.TryParse(massTmp, 
                            NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                            CultureInfo.CreateSpecificCulture("ru-RU"),
                            out var massDoubleValue)
                            ? $"{massDoubleValue / 1000 :0.######################################################e+00}"
                            : "";
                        var betaGammaActivity = double.TryParse(massTon,
                            NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent,
                            CultureInfo.CreateSpecificCulture("ru-RU"),
                            out var betaActivityDoubleValue)
                            ? $"{betaActivityDoubleValue * 25_000_000_000:0.######################################################e+00}"
                            : "";
                        var alphaActivity = double.TryParse(massTon,
                            NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent,
                            CultureInfo.CreateSpecificCulture("ru-RU"),
                            out var alphaActivityDoubleValue)
                            ? $"{alphaActivityDoubleValue * 16_100_000_000:0.######################################################e+00}"
                            : "";
                        var newForm16 = new Form16
                        {
                            #region BindingData

                            ReportId = rep.Id,
                            NumberInOrder_DB = numberInOrder,
                            OperationCode_DB = form12.OperationCode_DB,
                            OperationDate_DB = form12.OperationDate_DB,
                            CodeRAO_DB = "22511300522",
                            StatusRAO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                            Mass_DB = massTon,
                            QuantityOZIII_DB = "-",
                            MainRadionuclids_DB = "уран-238; торий-234; протактиний-234м; уран-234",
                            TritiumActivity_DB = "-",
                            BetaGammaActivity_DB = betaGammaActivity,
                            AlphaActivity_DB = alphaActivity,
                            TransuraniumActivity_DB = "-",
                            ActivityMeasurementDate_DB = $"({DateTime.Now.ToShortDateString()})",
                            DocumentVid_DB = form12.DocumentVid_DB,
                            DocumentNumber_DB = form12.DocumentNumber_DB,
                            DocumentDate_DB = form12.DocumentDate_DB,
                            ProviderOrRecieverOKPO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                            TransporterOKPO_DB = "-",
                            RefineOrSortRAOCode_DB = "-",
                            PackName_DB = form12.PackName_DB,
                            PackType_DB = form12.PackType_DB,
                            PackNumber_DB = form12.PackNumber_DB,
                            Subsidy_DB = "-",
                            FcpNumber_DB = "-"

                            #endregion
                        };
                        db.form_16.Add(newForm16);
                        break;
                    }
                    case "1.3":
                    {
                        var form13 = (Form13)form;
                        if (R.Count == 0)
                        {
#if DEBUG
                            R_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx"));
#else
                            R_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"R.xlsx"));
#endif
                        }
                        var nuclidsArray = form13.Radionuclids_DB
                            .Replace(" ", string.Empty)
                            .ToLower()
                            .Replace(',', ';')
                            .Split(';');
                        foreach (var tatata in R)
                        {
                            tatata.
                        }
                        var numberInOrder = await db.ReportCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == rep.Id)
                            .Include(x => x.Rows16)
                            .SelectMany(x => x.Rows16)
                            .CountAsync() + 1;
                        var agrState = form13.AggregateState_DB != null 
                            ? form13.AggregateState_DB.ToString()![..1] 
                            : "";
                        var codeRao = $"{agrState}__{1}__{0}{0}{84}_";

                        var newForm16 = new Form16
                        {
                            #region BindingData

                            ReportId = rep.Id,
                            NumberInOrder_DB = numberInOrder,
                            OperationCode_DB = form13.OperationCode_DB,
                            OperationDate_DB = form13.OperationDate_DB,
                            CodeRAO_DB = codeRao,
                            QuantityOZIII_DB = "-",
                            MainRadionuclids_DB = form13.Radionuclids_DB,
                            TritiumActivity_DB = "",
                            BetaGammaActivity_DB = "",
                            AlphaActivity_DB = "",
                            TransuraniumActivity_DB = "",
                            ActivityMeasurementDate_DB = form13.CreationDate_DB,
                            DocumentVid_DB = form13.DocumentVid_DB,
                            DocumentNumber_DB = form13.DocumentNumber_DB,
                            DocumentDate_DB = form13.DocumentDate_DB,
                            ProviderOrRecieverOKPO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                            TransporterOKPO_DB = "-",
                            RefineOrSortRAOCode_DB = "-",
                            PackName_DB = form13.PackName_DB,
                            PackType_DB = form13.PackType_DB,
                            PackNumber_DB = form13.PackNumber_DB,
                            Subsidy_DB = "-",
                            FcpNumber_DB = "-"

                            #endregion
                        };
                        db.form_16.Add(newForm16);
                        break;
                    }
                    case "1.4":
                    {
                        var form14 = (Form14)form;
                        var newForm16 = new Form16
                        {
                            #region BindingData

                            ReportId = rep.Id,
                            NumberInOrder_DB = await db.ReportCollectionDbSet
                                .AsNoTracking()
                                .AsSplitQuery()
                                .AsQueryable()
                                .Where(x => x.Id == rep.Id)
                                .Include(x => x.Rows16)
                                .SelectMany(x => x.Rows16)
                                .CountAsync() + 1,
                            OperationCode_DB = form14.OperationCode_DB,
                            OperationDate_DB = form14.OperationDate_DB,

                            #endregion
                        };
                        db.form_16.Add(newForm16);
                        break;
                    }
                }
                var report = await ReportsStorage.GetReportAsync(rep.Id);
                await db.SaveChangesAsync();
                await CloseWindowAndOpenNew(report);
                break;
            }
            default:    // Если отчета с подходящим периодом нет, создаём новый отчёт и добавляем в него форму 
            {
                var repId = 0;
                switch (form.FormNum_DB)
                {
                    case "1.1":
                    {
                        var form11 = (Form11)form;
                        Report newRep15;
                        if (SelectedReports.Report_Collection.All(x => x.FormNum_DB != "1.5"))
                        {
                            newRep15 = new Report
                            {
                                FormNum_DB = "1.5",
                                StartPeriod_DB = $"01.01.{DateTime.Now.Year}",
                                CorrectionNumber_DB = 0
                            };
                        }
                        else
                        {
                            var lastRep15 = SelectedReports.Report_Collection
                                .Where(x => x.FormNum_DB == "1.5")
                                .OrderByDescending(x => DateOnly.TryParse(x.StartPeriod_DB, out var startDateTo) 
                                    ? StaticStringMethods.StringDateReverse(startDateTo.ToShortDateString()) 
                                    : x.StartPeriod_DB)
                                .First();
                            newRep15 = new Report
                            {
                                FormNum_DB = "1.5",
                                StartPeriod_DB = lastRep15.EndPeriod_DB,
                                CorrectionNumber_DB = 0
                            };
                        }
                        var entityEntry = db.ReportCollectionDbSet.Add(newRep15);
                        await db.SaveChangesAsync();
                        repId = entityEntry.Entity.Id;    //id обновляется после сохранения БД.
                        var newForm15 = new Form15
                        {
                            #region BindingData
                
                            ReportId = repId,
                            NumberInOrder_DB = 1,
                            OperationCode_DB = form11.OperationCode_DB,
                            OperationDate_DB = form11.OperationDate_DB,
                            PassportNumber_DB = form11.PassportNumber_DB,
                            Type_DB = form11.Type_DB,
                            Radionuclids_DB = form11.Radionuclids_DB,
                            FactoryNumber_DB = form11.FactoryNumber_DB,
                            Activity_DB = form11.Activity_DB,
                            Quantity_DB = form11.Quantity_DB,
                            CreationDate_DB = form11.CreationDate_DB,
                            StatusRAO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                            DocumentVid_DB = form11.DocumentVid_DB,
                            DocumentNumber_DB = form11.DocumentNumber_DB,
                            DocumentDate_DB = form11.DocumentDate_DB,
                            ProviderOrRecieverOKPO_DB = form11.ProviderOrRecieverOKPO_DB,
                            TransporterOKPO_DB = form11.TransporterOKPO_DB,
                            PackName_DB = form11.PackName_DB,
                            PackType_DB = form11.PackType_DB,
                            PackNumber_DB = form11.PackNumber_DB,
                            RefineOrSortRAOCode_DB = "-",
                            Subsidy_DB = "-",
                            FcpNumber_DB = "-"

                            #endregion
                        };
                        db.form_15.Add(newForm15);
                        break;
                    }
                    case "1.2":
                    {
                        var form12 = (Form12)form;
                        Report newRep16;
                        if (SelectedReports.Report_Collection.All(x => x.FormNum_DB != "1.6"))
                        {
                            newRep16 = new Report
                            {
                                FormNum_DB = "1.6",
                                StartPeriod_DB = $"01.01.{DateTime.Now.Year}",
                                CorrectionNumber_DB = 0
                            };
                        }
                        else
                        {
                            var lastRep16 = SelectedReports.Report_Collection
                                .Where(x => x.FormNum_DB == "1.6")
                                .OrderByDescending(x => DateOnly.TryParse(x.StartPeriod_DB, out var startDateTo) 
                                    ? StaticStringMethods.StringDateReverse(startDateTo.ToShortDateString()) 
                                    : x.StartPeriod_DB)
                                .First();
                            newRep16 = new Report
                            {
                                FormNum_DB = "1.6",
                                StartPeriod_DB = lastRep16.EndPeriod_DB,
                                CorrectionNumber_DB = 0
                            };
                        }
                        var entityEntry = db.ReportCollectionDbSet.Add(newRep16);
                        await db.SaveChangesAsync();
                        repId = entityEntry.Entity.Id;    //id обновляется после сохранения БД.

                        var numberInOrder = await db.ReportCollectionDbSet
                            .AsNoTracking()
                            .AsSplitQuery()
                            .AsQueryable()
                            .Where(x => x.Id == repId)
                            .Include(x => x.Rows16)
                            .SelectMany(x => x.Rows16)
                            .CountAsync() + 1;
                        var massTmp = (form12.Mass_DB ?? "")
                            .Replace(".", ",")
                            .Replace("(", "")
                            .Replace(")", "");
                        var massTon = double.TryParse(massTmp, 
                            NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                            CultureInfo.CreateSpecificCulture("ru-RU"),
                            out var massDoubleValue)
                            ? $"{massDoubleValue / 1000 :0.######################################################e+00}"
                            : "";
                        var betaGammaActivity = double.TryParse(massTon,
                            NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent,
                            CultureInfo.CreateSpecificCulture("ru-RU"),
                            out var betaActivityDoubleValue)
                            ? $"{betaActivityDoubleValue * 25_000_000_000:0.######################################################e+00}"
                            : "";
                        var alphaActivity = double.TryParse(massTon,
                            NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent,
                            CultureInfo.CreateSpecificCulture("ru-RU"),
                            out var alphaActivityDoubleValue)
                            ? $"{alphaActivityDoubleValue * 16_100_000_000:0.######################################################e+00}"
                            : "";
                        var newForm16 = new Form16
                        {
                            #region BindingData

                            ReportId = repId,
                            NumberInOrder_DB = numberInOrder,
                            OperationCode_DB = form12.OperationCode_DB,
                            OperationDate_DB = form12.OperationDate_DB,
                            CodeRAO_DB = "22511300522",
                            StatusRAO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                            Mass_DB = massTon,
                            QuantityOZIII_DB = "-",
                            MainRadionuclids_DB = "уран-238; торий-234; протактиний-234м; уран-234",
                            TritiumActivity_DB = "-",
                            BetaGammaActivity_DB = betaGammaActivity,
                            AlphaActivity_DB = alphaActivity,
                            TransuraniumActivity_DB = "-",
                            ActivityMeasurementDate_DB = $"({DateTime.Now.ToShortDateString()})",
                            DocumentVid_DB = form12.DocumentVid_DB,
                            DocumentNumber_DB = form12.DocumentNumber_DB,
                            DocumentDate_DB = form12.DocumentDate_DB,
                            ProviderOrRecieverOKPO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                            TransporterOKPO_DB = "-",
                            RefineOrSortRAOCode_DB = "-",
                            PackName_DB = form12.PackName_DB,
                            PackType_DB = form12.PackType_DB,
                            PackNumber_DB = form12.PackNumber_DB,
                            Subsidy_DB = "-",
                            FcpNumber_DB = "-"

                            #endregion
                        };
                        db.form_16.Add(newForm16);
                        break;
                    }
                    case "1.3":
                    {
                        var form13 = (Form13)form;
                        var newRep16 = new Report
                        {
                            FormNum_DB = "1.6",
                            StartPeriod_DB = SelectedReport.EndPeriod_DB,
                            CorrectionNumber_DB = 0
                        };
                        var entityEntry = db.ReportCollectionDbSet.Add(newRep16);
                        await db.SaveChangesAsync();
                        repId = entityEntry.Entity.Id;    //id обновляется после сохранения БД.
                        var newForm16 = new Form16
                        {
                            #region BindingData

                            ReportId = repId,
                            NumberInOrder_DB = 1,
                            OperationCode_DB = form13.OperationCode_DB,
                            OperationDate_DB = form13.OperationDate_DB,

                            #endregion
                        };
                        db.form_16.Add(newForm16);
                        break;
                    }
                    case "1.4":
                    {
                        var form14 = (Form14)form;
                        var newRep16 = new Report
                        {
                            FormNum_DB = "1.6",
                            StartPeriod_DB = SelectedReport.EndPeriod_DB,
                            CorrectionNumber_DB = 0
                        };
                        var entityEntry = db.ReportCollectionDbSet.Add(newRep16);
                        await db.SaveChangesAsync();
                        repId = entityEntry.Entity.Id;    //id обновляется после сохранения БД.
                        var newForm16 = new Form16
                        {
                            #region BindingData

                            ReportId = repId,
                            NumberInOrder_DB = 1,
                            OperationCode_DB = form14.OperationCode_DB,
                            OperationDate_DB = form14.OperationDate_DB,

                            #endregion
                        };
                        db.form_16.Add(newForm16);
                        break;
                    }
                }
                await db.SaveChangesAsync();
                var report = await ReportsStorage.Api.GetAsync(repId);
                SelectedReports.Report_Collection.Add(report);
                await CloseWindowAndOpenNew(report);
                break;
            }
        }
    }

    private static async Task CloseWindowAndOpenNew(Report rep)
    {
        var window = Desktop.Windows.First(x => x.Name is "1.1" or "1.2" or "1.3" or "1.4");
        var windowParam = new FormParameter()
        {
            Parameter = new ObservableCollectionWithItemPropertyChanged<IKey>(new List<Report> { rep }),
            Window = window
        };
        await new ChangeFormAsyncCommand(windowParam).AsyncExecute(null).ConfigureAwait(false);
    }

    #region RFromFile

    private static List<Dictionary<string, string>> R = new();

    private static void R_Populate_From_File(string filePath)
    {
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
                {"code", worksheet.Cells[i, 8].Text},
                {"Num", worksheet.Cells[i, 9].Text}
            });
            i++;
        }
    }

    #endregion
}