using Models.Forms;
using Models.Forms.Form2;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Маппинг строки аналитической выгрузки → Form 2.1–2.12 (обратное к ExportForm2xData).
/// Колонки «Текст статуса РАО» и аналоги не пишутся в модель.
/// Колонки 1–6 — метаданные организации/периода; № п/п — колонка 7.
/// </summary>
public static class FormsExcelRowMapperForm2x
{
    public static Form MapRow(string formNum, ExcelWorksheet worksheet, int row) =>
        formNum switch
        {
            "2.1" => Map21(worksheet, row),
            "2.2" => Map22(worksheet, row),
            "2.3" => Map23(worksheet, row),
            "2.4" => Map24(worksheet, row),
            "2.5" => Map25(worksheet, row),
            "2.6" => Map26(worksheet, row),
            "2.7" => Map27(worksheet, row),
            "2.8" => Map28(worksheet, row),
            "2.9" => Map29(worksheet, row),
            "2.10" => Map210(worksheet, row),
            "2.11" => Map211(worksheet, row),
            "2.12" => Map212(worksheet, row),
            _ => throw new System.InvalidOperationException($"Нет маппера для формы {formNum}.")
        };

    private static Form21 Map21(ExcelWorksheet ws, int row)
    {
        var form = (Form21)FormCreator.Create("2.1");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.RefineMachineName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 8].Value);
        form.MachineCode_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 9].Value);
        form.MachinePower_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 10].Value);
        form.NumberOfHoursPerYear_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 11].Value);
        form.CodeRAOIn_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        form.StatusRAOIn_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 13].Value);
        form.VolumeIn_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 14].Value);
        form.MassIn_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 15].Value);
        form.QuantityIn_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 16].Value);
        form.TritiumActivityIn_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 17].Value);
        form.BetaGammaActivityIn_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 18].Value);
        form.AlphaActivityIn_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 19].Value);
        form.TransuraniumActivityIn_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 20].Value);
        form.CodeRAOout_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 21].Value);
        form.StatusRAOout_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 22].Value);
        form.VolumeOut_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 23].Value);
        form.MassOut_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 24].Value);
        form.QuantityOZIIIout_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 25].Value);
        form.TritiumActivityOut_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 26].Value);
        form.BetaGammaActivityOut_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 27].Value);
        form.AlphaActivityOut_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 28].Value);
        form.TransuraniumActivityOut_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 29].Value);
        // cols 30–31 — Текст статуса РАО (вычисляемое, не маппится)
        return form;
    }

    private static Form22 Map22(ExcelWorksheet ws, int row)
    {
        var form = (Form22)FormCreator.Create("2.2");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.StoragePlaceName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 8].Value);
        form.StoragePlaceCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.PackName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 10].Value);
        form.PackType_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.PackQuantity_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        form.CodeRAO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 13].Value);
        form.StatusRAO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 14].Value);
        form.VolumeOutOfPack_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 15].Value);
        form.VolumeInPack_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 16].Value);
        form.MassOutOfPack_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 17].Value);
        form.MassInPack_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 18].Value);
        form.QuantityOZIII_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 19].Value);
        form.TritiumActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 20].Value);
        form.BetaGammaActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 21].Value);
        form.AlphaActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 22].Value);
        form.TransuraniumActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 23].Value);
        form.MainRadionuclids_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 24].Value);
        form.Subsidy_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 25].Value);
        form.FcpNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 26].Value);
        // col 27 — Текст статуса РАО (вычисляемое, не маппится)
        return form;
    }

    private static Form23 Map23(ExcelWorksheet ws, int row)
    {
        var form = (Form23)FormCreator.Create("2.3");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.StoragePlaceName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 8].Value);
        form.StoragePlaceCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.ProjectVolume_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 10].Value);
        form.CodeRAO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.Volume_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 12].Value);
        form.Mass_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 13].Value);
        form.QuantityOZIII_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 14].Value);
        form.SummaryActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 15].Value);
        form.DocumentNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 16].Value);
        form.DocumentDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 17].Value, ws.Cells[row, 17].Text);
        form.ExpirationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 18].Value, ws.Cells[row, 18].Text);
        form.DocumentName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 19].Value);
        return form;
    }

    private static Form24 Map24(ExcelWorksheet ws, int row)
    {
        var form = (Form24)FormCreator.Create("2.4");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.CodeOYAT_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 8].Value);
        form.FcpNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.MassCreated_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 10].Value);
        form.QuantityCreated_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.MassFromAnothers_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 12].Value);
        form.QuantityFromAnothers_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 13].Value);
        form.MassFromAnothersImported_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 14].Value);
        form.QuantityFromAnothersImported_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 15].Value);
        form.MassAnotherReasons_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 16].Value);
        form.QuantityAnotherReasons_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 17].Value);
        form.MassTransferredToAnother_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 18].Value);
        form.QuantityTransferredToAnother_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 19].Value);
        form.MassRefined_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 20].Value);
        form.QuantityRefined_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 21].Value);
        form.MassRemovedFromAccount_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 22].Value);
        form.QuantityRemovedFromAccount_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 23].Value);
        return form;
    }

    private static Form25 Map25(ExcelWorksheet ws, int row)
    {
        var form = (Form25)FormCreator.Create("2.5");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.StoragePlaceName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 8].Value);
        form.StoragePlaceCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.CodeOYAT_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 10].Value);
        form.FcpNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.FuelMass_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 12].Value);
        form.CellMass_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 13].Value);
        form.Quantity_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 14].Value);
        form.AlphaActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 15].Value);
        form.BetaGammaActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 16].Value);
        return form;
    }

    private static Form26 Map26(ExcelWorksheet ws, int row)
    {
        var form = (Form26)FormCreator.Create("2.6");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.ObservedSourceNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 8].Value);
        form.ControlledAreaName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.SupposedWasteSource_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 10].Value);
        form.DistanceToWasteSource_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 11].Value);
        form.TestDepth_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 12].Value);
        form.RadionuclidName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 13].Value);
        form.AverageYearConcentration_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 14].Value);
        return form;
    }

    private static Form27 Map27(ExcelWorksheet ws, int row)
    {
        var form = (Form27)FormCreator.Create("2.7");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.ObservedSourceNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 8].Value);
        form.RadionuclidName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.AllowedWasteValue_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 10].Value);
        form.FactedWasteValue_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 11].Value);
        form.WasteOutbreakPreviousYear_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 12].Value);
        return form;
    }

    private static Form28 Map28(ExcelWorksheet ws, int row)
    {
        var form = (Form28)FormCreator.Create("2.8");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.WasteSourceName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 8].Value);
        form.WasteRecieverName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.RecieverTypeCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 10].Value);
        form.PoolDistrictName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.AllowedWasteRemovalVolume_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 12].Value);
        form.RemovedWasteVolume_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 13].Value);
        return form;
    }

    private static Form29 Map29(ExcelWorksheet ws, int row)
    {
        var form = (Form29)FormCreator.Create("2.9");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.WasteSourceName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 8].Value);
        form.RadionuclidName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.AllowedActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 10].Value);
        form.FactedActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 11].Value);
        return form;
    }

    private static Form210 Map210(ExcelWorksheet ws, int row)
    {
        var form = (Form210)FormCreator.Create("2.10");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.IndicatorName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 8].Value);
        form.PlotName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.PlotKadastrNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 10].Value);
        form.PlotCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.InfectedArea_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 12].Value);
        form.AvgGammaRaysDosePower_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 13].Value);
        form.MaxGammaRaysDosePower_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 14].Value);
        form.WasteDensityAlpha_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 15].Value);
        form.WasteDensityBeta_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 16].Value);
        form.FcpNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 17].Value);
        return form;
    }

    private static Form211 Map211(ExcelWorksheet ws, int row)
    {
        var form = (Form211)FormCreator.Create("2.11");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.PlotName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 8].Value);
        form.PlotKadastrNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.PlotCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 10].Value);
        form.InfectedArea_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 11].Value);
        form.Radionuclids_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        form.SpecificActivityOfPlot_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 13].Value);
        form.SpecificActivityOfLiquidPart_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 14].Value);
        form.SpecificActivityOfDensePart_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 15].Value);
        return form;
    }

    private static Form212 Map212(ExcelWorksheet ws, int row)
    {
        var form = (Form212)FormCreator.Create("2.12");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 7].Value) ?? 0;
        form.OperationCode_DB = FormsExcelCellConverters.ParseNullableShort(ws.Cells[row, 8].Value);
        form.ObjectTypeCode_DB = FormsExcelCellConverters.ParseNullableShort(ws.Cells[row, 9].Value);
        form.Radionuclids_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 10].Value);
        form.Activity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 11].Value);
        form.ProviderOrRecieverOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        return form;
    }
}
