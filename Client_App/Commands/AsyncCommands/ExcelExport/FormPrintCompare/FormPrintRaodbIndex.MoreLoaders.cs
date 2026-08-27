using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;

internal static partial class FormPrintRaodbIndex
{
    private static async Task<List<CompareRowDto>> LoadRows12Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_12.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            f.OperationDate_DB,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.OperationCode_DB), Cell(f.OperationDate_DB),
                Cell(f.PassportNumber_DB), Cell(f.NameIOU_DB), Cell(f.FactoryNumber_DB),
                Cell(f.Mass_DB), Cell(f.CreatorOKPO_DB), Cell(f.CreationDate_DB),
                Cell(f.SignedServicePeriod_DB), Cell(f.PropertyCode_DB), Cell(f.Owner_DB),
                Cell(f.DocumentVid_DB), Cell(f.DocumentNumber_DB), Cell(f.DocumentDate_DB),
                Cell(f.ProviderOrRecieverOKPO_DB), Cell(f.TransporterOKPO_DB),
                Cell(f.PackName_DB), Cell(f.PackType_DB), Cell(f.PackNumber_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows15Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_15.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            f.OperationDate_DB,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.OperationCode_DB), Cell(f.OperationDate_DB),
                Cell(f.PassportNumber_DB), Cell(f.Type_DB), Cell(f.Radionuclids_DB),
                Cell(f.FactoryNumber_DB), Cell(f.Quantity_DB), Cell(f.Activity_DB),
                Cell(f.CreationDate_DB), Cell(f.StatusRAO_DB),
                Cell(f.DocumentVid_DB), Cell(f.DocumentNumber_DB), Cell(f.DocumentDate_DB),
                Cell(f.ProviderOrRecieverOKPO_DB), Cell(f.TransporterOKPO_DB),
                Cell(f.PackName_DB), Cell(f.PackType_DB), Cell(f.PackNumber_DB),
                Cell(f.StoragePlaceName_DB), Cell(f.StoragePlaceCode_DB),
                Cell(f.RefineOrSortRAOCode_DB), Cell(f.Subsidy_DB), Cell(f.FcpNumber_DB),
                Cell(f.ContractNumber_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows16Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_16.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            f.OperationDate_DB,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.OperationCode_DB), Cell(f.OperationDate_DB),
                Cell(f.CodeRAO_DB), Cell(f.StatusRAO_DB), Cell(f.Volume_DB), Cell(f.Mass_DB),
                Cell(f.QuantityOZIII_DB), Cell(f.MainRadionuclids_DB),
                Cell(f.TritiumActivity_DB), Cell(f.BetaGammaActivity_DB),
                Cell(f.AlphaActivity_DB), Cell(f.TransuraniumActivity_DB),
                Cell(f.ActivityMeasurementDate_DB),
                Cell(f.DocumentVid_DB), Cell(f.DocumentNumber_DB), Cell(f.DocumentDate_DB),
                Cell(f.ProviderOrRecieverOKPO_DB), Cell(f.TransporterOKPO_DB),
                Cell(f.StoragePlaceName_DB), Cell(f.StoragePlaceCode_DB),
                Cell(f.RefineOrSortRAOCode_DB),
                Cell(f.PackName_DB), Cell(f.PackType_DB), Cell(f.PackNumber_DB),
                Cell(f.Subsidy_DB), Cell(f.FcpNumber_DB), Cell(f.ContractNumber_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows17Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_17.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            f.OperationDate_DB,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.OperationCode_DB), Cell(f.OperationDate_DB),
                Cell(f.PackName_DB), Cell(f.PackType_DB), Cell(f.PackFactoryNumber_DB),
                Cell(f.PackNumber_DB), Cell(f.FormingDate_DB), Cell(f.PassportNumber_DB),
                Cell(f.Volume_DB), Cell(f.Mass_DB), Cell(f.Radionuclids_DB),
                Cell(f.SpecificActivity_DB),
                Cell(f.DocumentVid_DB), Cell(f.DocumentNumber_DB), Cell(f.DocumentDate_DB),
                Cell(f.ProviderOrRecieverOKPO_DB), Cell(f.TransporterOKPO_DB),
                Cell(f.StoragePlaceName_DB), Cell(f.StoragePlaceCode_DB),
                Cell(f.CodeRAO_DB), Cell(f.StatusRAO_DB),
                Cell(f.VolumeOutOfPack_DB), Cell(f.MassOutOfPack_DB), Cell(f.Quantity_DB),
                Cell(f.TritiumActivity_DB), Cell(f.BetaGammaActivity_DB),
                Cell(f.AlphaActivity_DB), Cell(f.TransuraniumActivity_DB),
                Cell(f.RefineOrSortRAOCode_DB), Cell(f.Subsidy_DB), Cell(f.FcpNumber_DB),
                Cell(f.ContractNumber_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows18Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_18.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            f.OperationDate_DB,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.OperationCode_DB), Cell(f.OperationDate_DB),
                Cell(f.IndividualNumberZHRO_DB), Cell(f.PassportNumber_DB),
                Cell(f.Volume6_DB), Cell(f.Mass7_DB), Cell(f.SaltConcentration_DB),
                Cell(f.Radionuclids_DB), Cell(f.SpecificActivity_DB),
                Cell(f.DocumentVid_DB), Cell(f.DocumentNumber_DB), Cell(f.DocumentDate_DB),
                Cell(f.ProviderOrRecieverOKPO_DB), Cell(f.TransporterOKPO_DB),
                Cell(f.StoragePlaceName_DB), Cell(f.StoragePlaceCode_DB),
                Cell(f.CodeRAO_DB), Cell(f.StatusRAO_DB),
                Cell(f.Volume20_DB), Cell(f.Mass21_DB),
                Cell(f.TritiumActivity_DB), Cell(f.BetaGammaActivity_DB),
                Cell(f.AlphaActivity_DB), Cell(f.TransuraniumActivity_DB),
                Cell(f.RefineOrSortRAOCode_DB), Cell(f.Subsidy_DB), Cell(f.FcpNumber_DB),
                Cell(f.ContractNumber_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows19Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_19.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            f.OperationDate_DB,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.OperationCode_DB), Cell(f.OperationDate_DB),
                Cell(f.DocumentVid_DB), Cell(f.DocumentNumber_DB), Cell(f.DocumentDate_DB),
                Cell(f.CodeTypeAccObject_DB), Cell(f.Radionuclids_DB), Cell(f.Activity_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows21Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_21.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.RefineMachineName_DB), Cell(f.MachineCode_DB),
                Cell(f.MachinePower_DB), Cell(f.NumberOfHoursPerYear_DB),
                Cell(f.CodeRAOIn_DB), Cell(f.StatusRAOIn_DB),
                Cell(f.VolumeIn_DB), Cell(f.MassIn_DB), Cell(f.QuantityIn_DB),
                Cell(f.TritiumActivityIn_DB), Cell(f.BetaGammaActivityIn_DB),
                Cell(f.AlphaActivityIn_DB), Cell(f.TransuraniumActivityIn_DB),
                Cell(f.CodeRAOout_DB), Cell(f.StatusRAOout_DB),
                Cell(f.VolumeOut_DB), Cell(f.MassOut_DB), Cell(f.QuantityOZIIIout_DB),
                Cell(f.TritiumActivityOut_DB), Cell(f.BetaGammaActivityOut_DB),
                Cell(f.AlphaActivityOut_DB), Cell(f.TransuraniumActivityOut_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows22Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_22.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.StoragePlaceName_DB), Cell(f.StoragePlaceCode_DB),
                Cell(f.PackName_DB), Cell(f.PackType_DB), Cell(f.PackQuantity_DB),
                Cell(f.CodeRAO_DB), Cell(f.StatusRAO_DB),
                Cell(f.VolumeOutOfPack_DB), Cell(f.VolumeInPack_DB),
                Cell(f.MassOutOfPack_DB), Cell(f.MassInPack_DB), Cell(f.QuantityOZIII_DB),
                Cell(f.TritiumActivity_DB), Cell(f.BetaGammaActivity_DB),
                Cell(f.AlphaActivity_DB), Cell(f.TransuraniumActivity_DB),
                Cell(f.MainRadionuclids_DB), Cell(f.Subsidy_DB), Cell(f.FcpNumber_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows23Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_23.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.StoragePlaceName_DB), Cell(f.StoragePlaceCode_DB),
                Cell(f.ProjectVolume_DB), Cell(f.CodeRAO_DB), Cell(f.Volume_DB), Cell(f.Mass_DB),
                Cell(f.QuantityOZIII_DB), Cell(f.SummaryActivity_DB),
                Cell(f.DocumentNumber_DB), Cell(f.DocumentDate_DB), Cell(f.ExpirationDate_DB),
                Cell(f.DocumentName_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows24Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_24.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.CodeOYAT_DB), Cell(f.FcpNumber_DB),
                Cell(f.MassCreated_DB), Cell(f.QuantityCreated_DB),
                Cell(f.MassFromAnothers_DB), Cell(f.QuantityFromAnothers_DB),
                Cell(f.MassFromAnothersImported_DB), Cell(f.QuantityFromAnothersImported_DB),
                Cell(f.MassAnotherReasons_DB), Cell(f.QuantityAnotherReasons_DB),
                Cell(f.MassTransferredToAnother_DB), Cell(f.QuantityTransferredToAnother_DB),
                Cell(f.MassRefined_DB), Cell(f.QuantityRefined_DB),
                Cell(f.MassRemovedFromAccount_DB), Cell(f.QuantityRemovedFromAccount_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows25Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_25.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.StoragePlaceName_DB), Cell(f.StoragePlaceCode_DB),
                Cell(f.CodeOYAT_DB), Cell(f.FcpNumber_DB),
                Cell(f.FuelMass_DB), Cell(f.CellMass_DB), Cell(f.Quantity_DB),
                Cell(f.AlphaActivity_DB), Cell(f.BetaGammaActivity_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows26Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_26.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.ObservedSourceNumber_DB), Cell(f.ControlledAreaName_DB),
                Cell(f.SupposedWasteSource_DB), Cell(f.DistanceToWasteSource_DB), Cell(f.TestDepth_DB),
                Cell(f.RadionuclidName_DB), Cell(f.AverageYearConcentration_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows27Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_27.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.ObservedSourceNumber_DB), Cell(f.RadionuclidName_DB),
                Cell(f.AllowedWasteValue_DB), Cell(f.FactedWasteValue_DB),
                Cell(f.WasteOutbreakPreviousYear_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows28Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_28.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.WasteSourceName_DB), Cell(f.WasteRecieverName_DB),
                Cell(f.RecieverTypeCode_DB), Cell(f.PoolDistrictName_DB),
                Cell(f.AllowedWasteRemovalVolume_DB), Cell(f.RemovedWasteVolume_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows29Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_29.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.WasteSourceName_DB), Cell(f.RadionuclidName_DB),
                Cell(f.AllowedActivity_DB), Cell(f.FactedActivity_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows210Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_210.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.IndicatorName_DB), Cell(f.PlotName_DB),
                Cell(f.PlotKadastrNumber_DB), Cell(f.PlotCode_DB), Cell(f.InfectedArea_DB),
                Cell(f.AvgGammaRaysDosePower_DB), Cell(f.MaxGammaRaysDosePower_DB),
                Cell(f.WasteDensityAlpha_DB), Cell(f.WasteDensityBeta_DB), Cell(f.FcpNumber_DB)
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows211Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_211.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                Cell(f.NumberInOrder_DB), Cell(f.PlotName_DB), Cell(f.PlotKadastrNumber_DB),
                Cell(f.PlotCode_DB), Cell(f.InfectedArea_DB), Cell(f.Radionuclids_DB),
                Cell(f.SpecificActivityOfPlot_DB), Cell(f.SpecificActivityOfLiquidPart_DB),
                Cell(f.SpecificActivityOfDensePart_DB)
            }
        ));
    }
}
