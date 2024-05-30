using Avalonia.Threading;
using Client_App.ViewModels;
using Models.Collections;
using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using Models.Forms.Form1;
using System.Globalization;
using Models.DBRealization;
using Models.Classes;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.SourceTransmission;

// Перевод всех источников в форме из РВ в РАО
public class SourceTransmissionAllAsyncCommand : SourceTransmissionBaseAsyncCommand
{
    public SourceTransmissionAllAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        ChangeOrCreateViewModel = changeOrCreateViewModel;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        var formsWithCode41 = SelectedReport[SelectedReport.FormNum_DB].ToList<Form1>()
            .Where(x => string.Equals(x.OperationCode_DB.Trim(), "41", StringComparison.Ordinal))
            .ToList();
        var linesWithCorruptOpDate = formsWithCode41
            .Where(x => !DateOnly.TryParse(x.OperationDate_DB, out _))
            .Select(x => x.NumberInOrder_DB)
            .ToArray();
        if (linesWithCorruptOpDate.Length > 0)
        {
            #region MessageSourceTransmissionFailed

            var suffix1 = linesWithCorruptOpDate.Length == 1 ? "чке" : "ках";
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Перевод источников",
                    ContentHeader = "Ошибка",
                    ContentMessage = $"Некорректно введена дата операции в стро{suffix1} {string.Join(", ", linesWithCorruptOpDate)}.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }

        foreach (var form in formsWithCode41)
        {
            var opDate = DateOnly.Parse(form.OperationDate_DB);
            var repInRange = SelectedReports.Report_Collection
                .Where(rep => (form.FormNum_DB == "1.1" && rep.FormNum_DB == "1.5"
                               || form.FormNum_DB == "1.2" && rep.FormNum_DB == "1.6"
                               || form.FormNum_DB == "1.3" && rep.FormNum_DB == "1.6"
                               || form.FormNum_DB == "1.4" && rep.FormNum_DB == "1.6")
                              && (DateOnly.TryParse(rep.StartPeriod_DB, out var repStartDate)
                                  && DateOnly.TryParse(rep.EndPeriod_DB, out var repEndDate)
                                  && opDate > repStartDate && opDate <= repEndDate
                                  || DateOnly.TryParse(rep.StartPeriod_DB, out repStartDate)
                                      && !DateOnly.TryParse(rep.EndPeriod_DB, out _)
                                      && opDate > repStartDate))
                .OrderBy(x => x.EndPeriod_DB)
                .ToList();
            if (repInRange.Count == 2
                && !DateOnly.TryParse(repInRange[0].EndPeriod_DB, out _)
                && DateOnly.TryParse(repInRange[1].EndPeriod_DB, out _))
            {
                repInRange.Remove(repInRange[0]);
            }

            if (repInRange.Count > 1)
            {
                #region MessageSourceTransmissionFailed

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Перевод источника в РАО",
                        ContentHeader = "Ошибка",
                        ContentMessage =
                            "У выбранной организации присутствуют отчёты по форме 1.5 с пересекающимися периодами. " +
                            $"{Environment.NewLine}Устраните данное несоответствие перед операцией перевода источника в РАО.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                return;
            }
        }
        foreach (var form in formsWithCode41)
        {
            var opDate = DateOnly.Parse(form.OperationDate_DB);
            var repInRange = SelectedReports.Report_Collection
                .Where(rep => (form.FormNum_DB == "1.1" && rep.FormNum_DB == "1.5"
                               || form.FormNum_DB == "1.2" && rep.FormNum_DB == "1.6"
                               || form.FormNum_DB == "1.3" && rep.FormNum_DB == "1.6"
                               || form.FormNum_DB == "1.4" && rep.FormNum_DB == "1.6")
                              && (DateOnly.TryParse(rep.StartPeriod_DB, out var repStartDate)
                                  && DateOnly.TryParse(rep.EndPeriod_DB, out var repEndDate)
                                  && opDate > repStartDate && opDate <= repEndDate
                                  || DateOnly.TryParse(rep.StartPeriod_DB, out repStartDate)
                                      && !DateOnly.TryParse(rep.EndPeriod_DB, out _)
                                      && opDate > repStartDate))
                .OrderBy(x => x.EndPeriod_DB)
                .ToList();
            if (repInRange.Count == 2
                && !DateOnly.TryParse(repInRange[0].EndPeriod_DB, out _)
                && DateOnly.TryParse(repInRange[1].EndPeriod_DB, out _))
            {
                repInRange.Remove(repInRange[0]);
            }
            await using var db = new DBModel(StaticConfiguration.DBPath);
            Report repToOpen = new();
            switch (repInRange.Count)
            {
                case 1:   // Если есть подходящий отчет, то добавляем форму в него
                    {
                        var rep = repInRange.First();
                        switch (form.FormNum_DB)
                        {
                            case "1.1":
                                {
                                    var form11 = (Form11)form;
                                    var numberInOrder = await db.ReportCollectionDbSet
                                        .AsNoTracking()
                                        .AsSplitQuery()
                                        .AsQueryable()
                                        .Where(x => x.Id == rep.Id)
                                        .Include(x => x.Rows15)
                                        .SelectMany(x => x.Rows15)
                                        .CountAsync() + 1;
                                    var newForm15 = new Form15
                                    {
                                        #region BindingData

                                        ReportId = rep.Id,
                                        NumberInOrder_DB = numberInOrder,
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
                                        ? $"{massDoubleValue / 1000:0.######################################################e+00}"
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
                                    R_Populate_From_File();
                                    var form13 = (Form13)form;
                                    var numberInOrder = await db.ReportCollectionDbSet
                                        .AsNoTracking()
                                        .AsSplitQuery()
                                        .AsQueryable()
                                        .Where(x => x.Id == rep.Id)
                                        .Include(x => x.Rows16)
                                        .SelectMany(x => x.Rows16)
                                        .CountAsync() + 1;
                                    var nuclidsArray = form13.Radionuclids_DB
                                        .Replace(" ", string.Empty)
                                        .ToLower()
                                        .Replace(',', ';')
                                        .Split(';');
                                    var nuclidTypeArray = R
                                        .Where(x => nuclidsArray.Contains(x["name"]))
                                        .Select(x => x["code"])
                                        .ToArray();

                                    var codeRao = GetCodeRao(form13, nuclidsArray, nuclidTypeArray);
                                    var activitiesDictionary = GetActivities(form13, nuclidTypeArray);

                                    var newForm16 = new Form16
                                    {
                                        #region BindingData

                                        ReportId = rep.Id,
                                        NumberInOrder_DB = numberInOrder,
                                        OperationCode_DB = form13.OperationCode_DB,
                                        OperationDate_DB = form13.OperationDate_DB,
                                        CodeRAO_DB = codeRao,
                                        StatusRAO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                                        QuantityOZIII_DB = "-",
                                        MainRadionuclids_DB = form13.Radionuclids_DB,
                                        TritiumActivity_DB = activitiesDictionary["tritium"],
                                        BetaGammaActivity_DB = activitiesDictionary["beta"],
                                        AlphaActivity_DB = activitiesDictionary["alpha"],
                                        TransuraniumActivity_DB = activitiesDictionary["transuranium"],
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
                                    R_Populate_From_File();
                                    var form14 = (Form14)form;
                                    var numberInOrder = await db.ReportCollectionDbSet
                                        .AsNoTracking()
                                        .AsSplitQuery()
                                        .AsQueryable()
                                        .Where(x => x.Id == rep.Id)
                                        .Include(x => x.Rows16)
                                        .SelectMany(x => x.Rows16)
                                        .CountAsync() + 1;
                                    var nuclidsArray = form14.Radionuclids_DB
                                        .Replace(" ", string.Empty)
                                        .ToLower()
                                        .Replace(',', ';')
                                        .Split(';');
                                    var nuclidTypeArray = R
                                        .Where(x => nuclidsArray.Contains(x["name"]))
                                        .Select(x => x["code"])
                                        .ToArray();
                                    var massTmp = (form14.Mass_DB ?? "")
                                        .ToLower()
                                        .Replace(".", ",")
                                        .Replace("(", "")
                                        .Replace(")", "")
                                        .Replace('е', 'e')
                                        .Trim();
                                    var massTon = double.TryParse(massTmp,
                                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                                        CultureInfo.CreateSpecificCulture("ru-RU"),
                                        out var massDoubleValue)
                                        ? $"{massDoubleValue / 1000:0.######################################################e+00}"
                                        : "";

                                    var codeRao = GetCodeRao(form14, nuclidsArray, nuclidTypeArray);
                                    var activitiesDictionary = GetActivities(form14, nuclidTypeArray);

                                    var newForm16 = new Form16
                                    {
                                        #region BindingData

                                        ReportId = rep.Id,
                                        NumberInOrder_DB = numberInOrder,
                                        OperationCode_DB = form14.OperationCode_DB,
                                        OperationDate_DB = form14.OperationDate_DB,
                                        CodeRAO_DB = codeRao,
                                        Volume_DB = form14.Volume_DB,
                                        Mass_DB = massTon,
                                        QuantityOZIII_DB = "-",
                                        MainRadionuclids_DB = form14.Radionuclids_DB,
                                        TritiumActivity_DB = activitiesDictionary["tritium"],
                                        BetaGammaActivity_DB = activitiesDictionary["beta"],
                                        AlphaActivity_DB = activitiesDictionary["alpha"],
                                        TransuraniumActivity_DB = activitiesDictionary["transuranium"],
                                        ActivityMeasurementDate_DB = form14.ActivityMeasurementDate_DB,
                                        DocumentVid_DB = form14.DocumentVid_DB,
                                        DocumentNumber_DB = form14.DocumentNumber_DB,
                                        DocumentDate_DB = form14.DocumentDate_DB,
                                        ProviderOrRecieverOKPO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                                        TransporterOKPO_DB = "-",
                                        RefineOrSortRAOCode_DB = "-",
                                        PackName_DB = form14.PackName_DB,
                                        PackType_DB = form14.PackType_DB,
                                        PackNumber_DB = form14.PackNumber_DB,
                                        Subsidy_DB = "-",
                                        FcpNumber_DB = "-"

                                        #endregion
                                    };
                                    db.form_16.Add(newForm16);
                                    break;
                                }
                        }
                        var report = await ReportsStorage.GetReportAsync(rep.Id);
                        await db.SaveChangesAsync();
                        repToOpen = report;
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
                                    var newRep15 = GetNewReport(opDate, form11.FormNum_DB);
                                    var entityEntry = db.ReportCollectionDbSet.Add(newRep15);
                                    await db.SaveChangesAsync();
                                    repId = entityEntry.Entity.Id;    //id обновляется после сохранения БД.
                                    var numberInOrder = await db.ReportCollectionDbSet
                                        .AsNoTracking()
                                        .AsSplitQuery()
                                        .AsQueryable()
                                        .Where(x => x.Id == repId)
                                        .Include(x => x.Rows15)
                                        .SelectMany(x => x.Rows15)
                                        .CountAsync() + 1;
                                    var newForm15 = new Form15
                                    {
                                        #region BindingData

                                        ReportId = repId,
                                        NumberInOrder_DB = numberInOrder,
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
                                    var newRep16 = GetNewReport(opDate, form12.FormNum_DB);
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
                                        ? $"{massDoubleValue / 1000:0.######################################################e+00}"
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
                                    R_Populate_From_File();
                                    var form13 = (Form13)form;
                                    var newRep16 = GetNewReport(opDate, form13.FormNum_DB);
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
                                    var nuclidsArray = form13.Radionuclids_DB
                                        .Replace(" ", string.Empty)
                                        .ToLower()
                                        .Replace(',', ';')
                                        .Split(';');
                                    var nuclidTypeArray = R
                                        .Where(x => nuclidsArray.Contains(x["name"]))
                                        .Select(x => x["code"])
                                        .ToArray();

                                    var codeRao = GetCodeRao(form13, nuclidsArray, nuclidTypeArray);
                                    var activitiesDictionary = GetActivities(form13, nuclidTypeArray);

                                    var newForm16 = new Form16
                                    {
                                        #region BindingData

                                        ReportId = repId,
                                        NumberInOrder_DB = numberInOrder,
                                        OperationCode_DB = form13.OperationCode_DB,
                                        OperationDate_DB = form13.OperationDate_DB,
                                        CodeRAO_DB = codeRao,
                                        StatusRAO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                                        QuantityOZIII_DB = "-",
                                        MainRadionuclids_DB = form13.Radionuclids_DB,
                                        TritiumActivity_DB = activitiesDictionary["tritium"],
                                        BetaGammaActivity_DB = activitiesDictionary["beta"],
                                        AlphaActivity_DB = activitiesDictionary["alpha"],
                                        TransuraniumActivity_DB = activitiesDictionary["transuranium"],
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
                                    R_Populate_From_File();
                                    var form14 = (Form14)form;
                                    var newRep16 = GetNewReport(opDate, form14.FormNum_DB);
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
                                    var nuclidsArray = form14.Radionuclids_DB
                                        .Replace(" ", string.Empty)
                                        .ToLower()
                                        .Replace(',', ';')
                                        .Split(';');
                                    var nuclidTypeArray = R
                                        .Where(x => nuclidsArray.Contains(x["name"]))
                                        .Select(x => x["code"])
                                        .ToArray();
                                    var massTmp = (form14.Mass_DB ?? "")
                                        .Replace(".", ",")
                                        .Replace("(", "")
                                        .Replace(")", "");
                                    var massTon = double.TryParse(massTmp,
                                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                                        CultureInfo.CreateSpecificCulture("ru-RU"),
                                        out var massDoubleValue)
                                        ? $"{massDoubleValue / 1000:0.######################################################e+00}"
                                        : "";

                                    var codeRao = GetCodeRao(form14, nuclidsArray, nuclidTypeArray);
                                    var activitiesDictionary = GetActivities(form14, nuclidTypeArray);

                                    var newForm16 = new Form16
                                    {
                                        #region BindingData

                                        ReportId = repId,
                                        NumberInOrder_DB = numberInOrder,
                                        OperationCode_DB = form14.OperationCode_DB,
                                        OperationDate_DB = form14.OperationDate_DB,
                                        CodeRAO_DB = codeRao,
                                        Volume_DB = form14.Volume_DB,
                                        Mass_DB = massTon,
                                        QuantityOZIII_DB = "-",
                                        MainRadionuclids_DB = form14.Radionuclids_DB,
                                        TritiumActivity_DB = activitiesDictionary["tritium"],
                                        BetaGammaActivity_DB = activitiesDictionary["beta"],
                                        AlphaActivity_DB = activitiesDictionary["alpha"],
                                        TransuraniumActivity_DB = activitiesDictionary["transuranium"],
                                        ActivityMeasurementDate_DB = form14.ActivityMeasurementDate_DB,
                                        DocumentVid_DB = form14.DocumentVid_DB,
                                        DocumentNumber_DB = form14.DocumentNumber_DB,
                                        DocumentDate_DB = form14.DocumentDate_DB,
                                        ProviderOrRecieverOKPO_DB = SelectedReports.Master_DB.OkpoRep.Value,
                                        TransporterOKPO_DB = "-",
                                        RefineOrSortRAOCode_DB = "-",
                                        PackName_DB = form14.PackName_DB,
                                        PackType_DB = form14.PackType_DB,
                                        PackNumber_DB = form14.PackNumber_DB,
                                        Subsidy_DB = "-",
                                        FcpNumber_DB = "-"

                                        #endregion
                                    };
                                    db.form_16.Add(newForm16);
                                    break;
                                }
                        }
                        await db.SaveChangesAsync();
                        var report = await ReportsStorage.Api.GetAsync(repId);
                        SelectedReports.Report_Collection.Add(report);
                        repToOpen = report;
                        break;
                    }
            }
            await CloseWindowAndOpenNew(repToOpen);
        }
    }

    #region CloseWindowAndOpenNew

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

    #endregion
}