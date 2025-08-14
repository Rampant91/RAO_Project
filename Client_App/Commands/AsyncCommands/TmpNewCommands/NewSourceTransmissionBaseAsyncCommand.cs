using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.TmpNewCommands;

public abstract class NewSourceTransmissionBaseAsyncCommand : BaseAsyncCommand
{
    private protected Reports Reports
    {
        get => Report.Reports;
        set => Report.Reports = value;
    }

    private protected Report Report
    {
        get => VM.CurrentReport;
        set => VM.CurrentReport = value;
    }

    private protected dynamic VM = null!;

    #region AddNewFormToExistingReport

    private protected async Task<bool> AddNewFormToExistingReport(Report rep, Form1 form, DBModel db)
    {
        var formIsAdded = false;
        switch (form.FormNum_DB)
        {
            #region 1.1

            case "1.1":
            {
                var form11 = (Form11)form;
                var numberInOrder = await db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                    .Include(x => x.Rows15)
                    .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.Id == rep.Id)
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
                    StatusRAO_DB = Reports.Master_DB.OkpoRep.Value,
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
                var comparator = new CustomForm11ToForm15Comparer();
                var isDuplicate = rep.Rows15
                    .Any(currentForm => comparator.Compare(newForm15, currentForm) == 0);
                if (!isDuplicate)
                {
                    db.form_15.Add(newForm15);
                    formIsAdded = true;
                }
                break;
            }

            #endregion

            #region 1.2

            case "1.2":
            {
                var form12 = (Form12)form;
                var numberInOrder = await db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                    .Include(x => x.Rows16)
                    .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.Id == rep.Id)
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
                    StatusRAO_DB = Reports.Master_DB.OkpoRep.Value,
                    Mass_DB = massTon,
                    QuantityOZIII_DB = "-",
                    MainRadionuclids_DB = "уран-238; торий-234; протактиний-234м; уран-234",
                    TritiumActivity_DB = "-",
                    BetaGammaActivity_DB = betaGammaActivity,
                    AlphaActivity_DB = alphaActivity,
                    TransuraniumActivity_DB = "-",
                    ActivityMeasurementDate_DB = form12.OperationDate_DB,
                    DocumentVid_DB = form12.DocumentVid_DB,
                    DocumentNumber_DB = form12.DocumentNumber_DB,
                    DocumentDate_DB = form12.DocumentDate_DB,
                    ProviderOrRecieverOKPO_DB = Reports.Master_DB.OkpoRep.Value,
                    TransporterOKPO_DB = "-",
                    RefineOrSortRAOCode_DB = "-",
                    PackName_DB = form12.PackName_DB,
                    PackType_DB = form12.PackType_DB,
                    PackNumber_DB = form12.PackNumber_DB,
                    Subsidy_DB = "-",
                    FcpNumber_DB = "-"

                    #endregion
                };
                var comparator = new CustomForm12And13ToForm16Comparer();
                var isDuplicate = rep.Rows16
                    .Any(currentForm => comparator.Compare(newForm16, currentForm) == 0);
                if (!isDuplicate)
                {
                    db.form_16.Add(newForm16);
                    formIsAdded = true;
                }
                break;
            }

            #endregion

            #region 1.3

            case "1.3":
            {
                R_Populate_From_File();
                var form13 = (Form13)form;
                var numberInOrder = await db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                    .Include(x => x.Rows16)
                    .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.Id == rep.Id)
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
                    StatusRAO_DB = Reports.Master_DB.OkpoRep.Value,
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
                    ProviderOrRecieverOKPO_DB = Reports.Master_DB.OkpoRep.Value,
                    TransporterOKPO_DB = "-",
                    RefineOrSortRAOCode_DB = "-",
                    PackName_DB = form13.PackName_DB,
                    PackType_DB = form13.PackType_DB,
                    PackNumber_DB = form13.PackNumber_DB,
                    Subsidy_DB = "-",
                    FcpNumber_DB = "-"

                    #endregion
                };
                var comparator = new CustomForm12And13ToForm16Comparer();
                var isDuplicate = rep.Rows16
                    .Any(currentForm => comparator.Compare(newForm16, currentForm) == 0);
                if (!isDuplicate)
                {
                    db.form_16.Add(newForm16);
                    formIsAdded = true;
                }
                break;
            }

            #endregion

            #region 1.4

            case "1.4":
            {
                R_Populate_From_File();
                var form14 = (Form14)form;
                var numberInOrder = await db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                    .Include(x => x.Rows16)
                    .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.Id == rep.Id)
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
                    StatusRAO_DB = Reports.Master_DB.OkpoRep.Value,
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
                    ProviderOrRecieverOKPO_DB = Reports.Master_DB.OkpoRep.Value,
                    TransporterOKPO_DB = "-",
                    RefineOrSortRAOCode_DB = "-",
                    PackName_DB = form14.PackName_DB,
                    PackType_DB = form14.PackType_DB,
                    PackNumber_DB = form14.PackNumber_DB,
                    Subsidy_DB = "-",
                    FcpNumber_DB = "-"

                    #endregion
                };
                var comparator = new CustomForm14ToForm16Comparer();
                var isDuplicate = rep.Rows16
                    .Any(currentForm => comparator.Compare(newForm16, currentForm) == 0);
                if (!isDuplicate)
                {
                    db.form_16.Add(newForm16);
                    formIsAdded = true;
                }
                break;
            }

            #endregion
        }
        return formIsAdded;
    }

    #endregion

    #region CreateReportAndAddNewForm

    private protected async Task<Report> CreateReportAndAddNewForm(DBModel db, Form1 form, DateOnly opDate)
    {
        var repId = 0;
        Report newRep = new();
        switch (form.FormNum_DB)
        {
            #region 1.1

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
                    .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                    .Include(x => x.Rows15)
                    .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.Id == repId)
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
                    StatusRAO_DB = Reports.Master_DB.OkpoRep.Value,
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
                newRep = newRep15;
                break;
            }

            #endregion

            #region 1.2

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
                    .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                    .Include(x => x.Rows16)
                    .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.Id == repId)
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
                    StatusRAO_DB = Reports.Master_DB.OkpoRep.Value,
                    Mass_DB = massTon,
                    QuantityOZIII_DB = "-",
                    MainRadionuclids_DB = "уран-238; торий-234; протактиний-234м; уран-234",
                    TritiumActivity_DB = "-",
                    BetaGammaActivity_DB = betaGammaActivity,
                    AlphaActivity_DB = alphaActivity,
                    TransuraniumActivity_DB = "-",
                    ActivityMeasurementDate_DB = form12.OperationDate_DB,
                    DocumentVid_DB = form12.DocumentVid_DB,
                    DocumentNumber_DB = form12.DocumentNumber_DB,
                    DocumentDate_DB = form12.DocumentDate_DB,
                    ProviderOrRecieverOKPO_DB = Reports.Master_DB.OkpoRep.Value,
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
                newRep = newRep16;
                break;
            }

            #endregion

            #region 1.3

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
                    .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                    .Include(x => x.Rows16)
                    .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.Id == repId)
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
                    StatusRAO_DB = Reports.Master_DB.OkpoRep.Value,
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
                    ProviderOrRecieverOKPO_DB = Reports.Master_DB.OkpoRep.Value,
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
                newRep = newRep16;
                break;
            }

            #endregion

            #region 1.4

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
                    .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                    .Include(x => x.Rows16)
                    .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.Id == repId)
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
                    StatusRAO_DB = Reports.Master_DB.OkpoRep.Value,
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
                    ProviderOrRecieverOKPO_DB = Reports.Master_DB.OkpoRep.Value,
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
                newRep = newRep16;
                break;
            }

            #endregion
        }
        return newRep;
    }

    #endregion

    #region CustomComparators

    #region CustomNullStringWithTrimComparer

    private class CustomNullStringWithTrimComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            var strA = (x ?? string.Empty).Trim();
            var strB = (y ?? string.Empty).Trim();
            return string.CompareOrdinal(strA, strB);
        }
    }

    #endregion

    #region CustomForm11ToForm15Comparer

    private class CustomForm11ToForm15Comparer : IComparer<Form15>
    {
        public int Compare(Form15? x, Form15? y)
        {
            if (x is null || y is null) return 1;
            var comparator = new CustomNullStringWithTrimComparer();
            var isDuplicate = comparator.Compare(x.OperationCode_DB, y.OperationCode_DB) == 0
                              && comparator.Compare(x.OperationDate_DB, y.OperationDate_DB) == 0
                              && comparator.Compare(x.PassportNumber_DB, y.PassportNumber_DB) == 0
                              && comparator.Compare(x.Type_DB, y.Type_DB) == 0
                              && comparator.Compare(x.Radionuclids_DB, y.Radionuclids_DB) == 0
                              && comparator.Compare(x.FactoryNumber_DB, y.FactoryNumber_DB) == 0
                              && comparator.Compare(x.Activity_DB, y.Activity_DB) == 0
                              && x.Quantity_DB == y.Quantity_DB
                              && comparator.Compare(x.CreationDate_DB, y.CreationDate_DB) == 0
                              && comparator.Compare(x.StatusRAO_DB, y.StatusRAO_DB) == 0
                              && x.DocumentVid_DB == y.DocumentVid_DB
                              && comparator.Compare(x.DocumentNumber_DB, y.DocumentNumber_DB) == 0
                              && comparator.Compare(x.DocumentDate_DB, y.DocumentDate_DB) == 0
                              && comparator.Compare(x.ProviderOrRecieverOKPO_DB, y.ProviderOrRecieverOKPO_DB) == 0
                              && comparator.Compare(x.TransporterOKPO_DB, y.TransporterOKPO_DB) == 0
                              && comparator.Compare(x.PackName_DB, y.PackName_DB) == 0
                              && comparator.Compare(x.PackType_DB, y.PackType_DB) == 0
                              && comparator.Compare(x.PackNumber_DB, y.PackNumber_DB) == 0
                              && comparator.Compare(x.RefineOrSortRAOCode_DB, y.RefineOrSortRAOCode_DB) == 0
                              && comparator.Compare(x.Subsidy_DB, y.Subsidy_DB) == 0
                              && comparator.Compare(x.FcpNumber_DB, y.FcpNumber_DB) == 0;
            return isDuplicate ? 0 : 1;
        }
    }

    #endregion

    #region CustomForm12And13ToForm16Comparer

    private class CustomForm12And13ToForm16Comparer : IComparer<Form16>
    {
        public int Compare(Form16? x, Form16? y)
        {
            if (x is null || y is null) return 1;
            var comparator = new CustomNullStringWithTrimComparer();
            var isDuplicate = comparator.Compare(x.OperationCode_DB, y.OperationCode_DB) == 0
                              && comparator.Compare(x.OperationDate_DB, y.OperationDate_DB) == 0
                              && comparator.Compare(x.CodeRAO_DB, y.CodeRAO_DB) == 0
                              && comparator.Compare(x.StatusRAO_DB, y.StatusRAO_DB) == 0
                              && comparator.Compare(x.Mass_DB, y.Mass_DB) == 0
                              && comparator.Compare(x.QuantityOZIII_DB, y.QuantityOZIII_DB) == 0
                              && comparator.Compare(x.MainRadionuclids_DB, y.MainRadionuclids_DB) == 0
                              && comparator.Compare(x.TritiumActivity_DB, y.TritiumActivity_DB) == 0
                              && comparator.Compare(x.BetaGammaActivity_DB, y.BetaGammaActivity_DB) == 0
                              && comparator.Compare(x.AlphaActivity_DB, y.AlphaActivity_DB) == 0
                              && comparator.Compare(x.TransuraniumActivity_DB, y.TransuraniumActivity_DB) == 0
                              && comparator.Compare(x.ActivityMeasurementDate_DB, y.ActivityMeasurementDate_DB) == 0
                              && x.DocumentVid_DB == y.DocumentVid_DB
                              && comparator.Compare(x.DocumentNumber_DB, y.DocumentNumber_DB) == 0
                              && comparator.Compare(x.DocumentDate_DB, y.DocumentDate_DB) == 0
                              && comparator.Compare(x.ProviderOrRecieverOKPO_DB, y.ProviderOrRecieverOKPO_DB) == 0
                              && comparator.Compare(x.TransporterOKPO_DB, y.TransporterOKPO_DB) == 0
                              && comparator.Compare(x.RefineOrSortRAOCode_DB, y.RefineOrSortRAOCode_DB) == 0
                              && comparator.Compare(x.PackName_DB, y.PackName_DB) == 0
                              && comparator.Compare(x.PackType_DB, y.PackType_DB) == 0
                              && comparator.Compare(x.PackNumber_DB, y.PackNumber_DB) == 0
                              && comparator.Compare(x.Subsidy_DB, y.Subsidy_DB) == 0
                              && comparator.Compare(x.FcpNumber_DB, y.FcpNumber_DB) == 0;
            return isDuplicate ? 0 : 1;
        }
    }

    #endregion

    #region CustomForm14ToForm16Comparer

    private class CustomForm14ToForm16Comparer : IComparer<Form16>
    {
        public int Compare(Form16? x, Form16? y)
        {
            if (x is null || y is null) return 1;
            var comparator = new CustomNullStringWithTrimComparer();
            var isDuplicate = comparator.Compare(x.OperationCode_DB, y.OperationCode_DB) == 0
                              && comparator.Compare(x.OperationDate_DB, y.OperationDate_DB) == 0
                              && comparator.Compare(x.CodeRAO_DB, y.CodeRAO_DB) == 0
                              && comparator.Compare(x.Volume_DB, y.Volume_DB) == 0
                              && comparator.Compare(x.Mass_DB, y.Mass_DB) == 0
                              && comparator.Compare(x.QuantityOZIII_DB, y.QuantityOZIII_DB) == 0
                              && comparator.Compare(x.MainRadionuclids_DB, y.MainRadionuclids_DB) == 0
                              && comparator.Compare(x.TritiumActivity_DB, y.TritiumActivity_DB) == 0
                              && comparator.Compare(x.BetaGammaActivity_DB, y.BetaGammaActivity_DB) == 0
                              && comparator.Compare(x.AlphaActivity_DB, y.AlphaActivity_DB) == 0
                              && comparator.Compare(x.TransuraniumActivity_DB, y.TransuraniumActivity_DB) == 0
                              && comparator.Compare(x.ActivityMeasurementDate_DB, y.ActivityMeasurementDate_DB) == 0
                              && x.DocumentVid_DB == y.DocumentVid_DB
                              && comparator.Compare(x.DocumentNumber_DB, y.DocumentNumber_DB) == 0
                              && comparator.Compare(x.DocumentDate_DB, y.DocumentDate_DB) == 0
                              && comparator.Compare(x.ProviderOrRecieverOKPO_DB, y.ProviderOrRecieverOKPO_DB) == 0
                              && comparator.Compare(x.TransporterOKPO_DB, y.TransporterOKPO_DB) == 0
                              && comparator.Compare(x.RefineOrSortRAOCode_DB, y.RefineOrSortRAOCode_DB) == 0
                              && comparator.Compare(x.PackName_DB, y.PackName_DB) == 0
                              && comparator.Compare(x.PackType_DB, y.PackType_DB) == 0
                              && comparator.Compare(x.PackNumber_DB, y.PackNumber_DB) == 0
                              && comparator.Compare(x.Subsidy_DB, y.Subsidy_DB) == 0
                              && comparator.Compare(x.FcpNumber_DB, y.FcpNumber_DB) == 0;
            return isDuplicate ? 0 : 1;
        }
    }

    #endregion

    #endregion

    #region GetActivities

    private static Dictionary<string, string> GetActivities(Form1 form1, string[] nuclidTypeArray)
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

        if (nuclidTypeArray.Length == 1
            || nuclidTypeArray.Length > 1
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

    private static string GetCodeRao(Form1 form, IEnumerable<string> nuclidsArray, string[] nuclidTypeArray)
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

    private Report GetNewReport(DateOnly opDate, string formNum)
    {
        var relevantFormNum = formNum == "1.1"
            ? "1.5"
            : "1.6";

        #region GetDates

        var startDateList = Reports.Report_Collection
            .Where(x => x.FormNum_DB == relevantFormNum)
            .Select(x => DateOnly.TryParse(x.StartPeriod_DB, out var startDate)
                ? startDate
                : DateOnly.MinValue)
            .ToList();
        var endDateList = Reports.Report_Collection
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
        var firstRepStartDate = Reports.Report_Collection
            .Any(x => x.FormNum_DB == relevantFormNum)
            ? Reports.Report_Collection
                .Where(x => x.FormNum_DB == relevantFormNum)
                .Select(x => DateOnly.TryParse(x.StartPeriod_DB, out var startDate)
                    ? startDate
                    : DateOnly.MaxValue)
                .Min()
            : DateOnly.MaxValue;
        var lastRepEndDate = Reports.Report_Collection
            .Any(x => x.FormNum_DB == relevantFormNum)
            ? Reports.Report_Collection
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
            ExecEmail_DB = Report.ExecEmail_DB,
            ExecPhone_DB = Report.ExecPhone_DB,
            GradeExecutor_DB = Report.GradeExecutor_DB,
            FIOexecutor_DB = Report.FIOexecutor_DB
        };
        if (Reports.Report_Collection.All(x => x.FormNum_DB != relevantFormNum)
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

    private static List<Dictionary<string, string>> R = [];

    private static void R_Populate_From_File()
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