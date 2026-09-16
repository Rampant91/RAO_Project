using Models.Forms;
using Models.Forms.Form1;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Маппинг строки аналитической выгрузки → Form 1.2–1.9 (обратное к ExportFormXXData).
/// Колонки «Текст статуса РАО» и аналоги не пишутся в модель.
/// </summary>
public static class FormsExcelRowMapperForm1x
{
    public static Form MapRow(string formNum, ExcelWorksheet worksheet, int row) =>
        formNum switch
        {
            "1.2" => Map12(worksheet, row),
            "1.3" => Map13(worksheet, row),
            "1.4" => Map14(worksheet, row),
            "1.5" => Map15(worksheet, row),
            "1.6" => Map16(worksheet, row),
            "1.7" => Map17(worksheet, row),
            "1.8" => Map18(worksheet, row),
            "1.9" => Map19(worksheet, row),
            "1.1" => FormsExcelExportParserForm11.MapForm11Row(worksheet, row),
            _ => throw new System.InvalidOperationException($"Нет маппера для формы {formNum}.")
        };

    private static Form12 Map12(ExcelWorksheet ws, int row)
    {
        var form = (Form12)FormCreator.Create("1.2");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 8].Value) ?? 0;
        form.OperationCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.OperationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 10].Value, ws.Cells[row, 10].Text);
        form.PassportNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.NameIOU_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        form.FactoryNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 13].Value);
        form.Mass_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 14].Value);
        form.CreatorOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 15].Value);
        form.CreationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 16].Value, ws.Cells[row, 16].Text);
        form.SignedServicePeriod_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 17].Value);
        form.PropertyCode_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 18].Value);
        form.Owner_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 19].Value);
        form.DocumentVid_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 20].Value);
        form.DocumentNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 21].Value);
        form.DocumentDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 22].Value, ws.Cells[row, 22].Text);
        form.ProviderOrRecieverOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 23].Value);
        form.TransporterOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 24].Value);
        form.PackName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 25].Value);
        form.PackType_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 26].Value);
        form.PackNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 27].Value);
        return form;
    }

    private static Form13 Map13(ExcelWorksheet ws, int row)
    {
        var form = (Form13)FormCreator.Create("1.3");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 8].Value) ?? 0;
        form.OperationCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.OperationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 10].Value, ws.Cells[row, 10].Text);
        form.PassportNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.Type_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        form.Radionuclids_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 13].Value);
        form.FactoryNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 14].Value);
        form.Activity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 15].Value);
        form.CreatorOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 16].Value);
        form.CreationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 17].Value, ws.Cells[row, 17].Text);
        form.AggregateState_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 18].Value);
        form.PropertyCode_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 19].Value);
        form.Owner_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 20].Value);
        form.DocumentVid_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 21].Value);
        form.DocumentNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 22].Value);
        form.DocumentDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 23].Value, ws.Cells[row, 23].Text);
        form.ProviderOrRecieverOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 24].Value);
        form.TransporterOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 25].Value);
        form.PackName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 26].Value);
        form.PackType_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 27].Value);
        form.PackNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 28].Value);
        return form;
    }

    private static Form14 Map14(ExcelWorksheet ws, int row)
    {
        var form = (Form14)FormCreator.Create("1.4");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 8].Value) ?? 0;
        form.OperationCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.OperationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 10].Value, ws.Cells[row, 10].Text);
        form.PassportNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.Name_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        form.Sort_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 13].Value);
        form.Radionuclids_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 14].Value);
        form.Activity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 15].Value);
        form.ActivityMeasurementDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 16].Value, ws.Cells[row, 16].Text);
        form.Volume_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 17].Value);
        form.Mass_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 18].Value);
        form.AggregateState_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 19].Value);
        form.PropertyCode_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 20].Value);
        form.Owner_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 21].Value);
        form.DocumentVid_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 22].Value);
        form.DocumentNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 23].Value);
        form.DocumentDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 24].Value, ws.Cells[row, 24].Text);
        form.ProviderOrRecieverOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 25].Value);
        form.TransporterOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 26].Value);
        form.PackName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 27].Value);
        form.PackType_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 28].Value);
        form.PackNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 29].Value);
        return form;
    }

    private static Form15 Map15(ExcelWorksheet ws, int row)
    {
        var form = (Form15)FormCreator.Create("1.5");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 8].Value) ?? 0;
        form.OperationCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.OperationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 10].Value, ws.Cells[row, 10].Text);
        form.PassportNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.Type_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        form.Radionuclids_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 13].Value);
        form.FactoryNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 14].Value);
        form.Quantity_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 15].Value);
        form.Activity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 16].Value);
        form.CreationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 17].Value, ws.Cells[row, 17].Text);
        form.StatusRAO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 18].Value);
        form.DocumentVid_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 19].Value);
        form.DocumentNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 20].Value);
        form.DocumentDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 21].Value, ws.Cells[row, 21].Text);
        form.ProviderOrRecieverOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 22].Value);
        form.TransporterOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 23].Value);
        form.PackName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 24].Value);
        form.PackType_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 25].Value);
        form.PackNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 26].Value);
        form.StoragePlaceName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 27].Value);
        form.StoragePlaceCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 28].Value);
        form.RefineOrSortRAOCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 29].Value);
        form.Subsidy_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 30].Value);
        form.FcpNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 31].Value);
        form.ContractNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 32].Value);
        // col 33 — Текст статуса РАО (вычисляемое, не маппится)
        return form;
    }

    private static Form16 Map16(ExcelWorksheet ws, int row)
    {
        var form = (Form16)FormCreator.Create("1.6");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 8].Value) ?? 0;
        form.OperationCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.OperationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 10].Value, ws.Cells[row, 10].Text);
        form.CodeRAO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.StatusRAO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        form.Volume_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 13].Value);
        form.Mass_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 14].Value);
        form.QuantityOZIII_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 15].Value);
        form.MainRadionuclids_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 16].Value);
        form.TritiumActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 17].Value);
        form.BetaGammaActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 18].Value);
        form.AlphaActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 19].Value);
        form.TransuraniumActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 20].Value);
        form.ActivityMeasurementDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 21].Value, ws.Cells[row, 21].Text);
        form.DocumentVid_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 22].Value);
        form.DocumentNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 23].Value);
        form.DocumentDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 24].Value, ws.Cells[row, 24].Text);
        form.ProviderOrRecieverOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 25].Value);
        form.TransporterOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 26].Value);
        form.StoragePlaceName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 27].Value);
        form.StoragePlaceCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 28].Value);
        form.RefineOrSortRAOCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 29].Value);
        form.PackName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 30].Value);
        form.PackType_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 31].Value);
        form.PackNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 32].Value);
        form.Subsidy_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 33].Value);
        form.FcpNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 34].Value);
        form.ContractNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 35].Value);
        return form;
    }

    private static Form17 Map17(ExcelWorksheet ws, int row)
    {
        var form = (Form17)FormCreator.Create("1.7");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 8].Value) ?? 0;
        form.OperationCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.OperationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 10].Value, ws.Cells[row, 10].Text);
        form.PackName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.PackType_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        form.PackFactoryNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 13].Value);
        form.PackNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 14].Value);
        form.FormingDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 15].Value, ws.Cells[row, 15].Text);
        form.PassportNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 16].Value);
        form.Volume_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 17].Value);
        form.Mass_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 18].Value);
        form.Radionuclids_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 19].Value);
        form.SpecificActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 20].Value);
        form.DocumentVid_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 21].Value);
        form.DocumentNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 22].Value);
        form.DocumentDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 23].Value, ws.Cells[row, 23].Text);
        form.ProviderOrRecieverOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 24].Value);
        form.TransporterOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 25].Value);
        form.StoragePlaceName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 26].Value);
        form.StoragePlaceCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 27].Value);
        form.CodeRAO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 28].Value);
        form.StatusRAO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 29].Value);
        form.VolumeOutOfPack_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 30].Value);
        form.MassOutOfPack_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 31].Value);
        form.Quantity_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 32].Value);
        form.TritiumActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 33].Value);
        form.BetaGammaActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 34].Value);
        form.AlphaActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 35].Value);
        form.TransuraniumActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 36].Value);
        form.RefineOrSortRAOCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 37].Value);
        form.Subsidy_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 38].Value);
        form.FcpNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 39].Value);
        form.ContractNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 40].Value);
        return form;
    }

    private static Form18 Map18(ExcelWorksheet ws, int row)
    {
        var form = (Form18)FormCreator.Create("1.8");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 8].Value) ?? 0;
        form.OperationCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.OperationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 10].Value, ws.Cells[row, 10].Text);
        form.IndividualNumberZHRO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 11].Value);
        form.PassportNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        form.Volume6_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 13].Value);
        form.Mass7_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 14].Value);
        form.SaltConcentration_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 15].Value);
        form.Radionuclids_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 16].Value);
        form.SpecificActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 17].Value);
        form.DocumentVid_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 18].Value);
        form.DocumentNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 19].Value);
        form.DocumentDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 20].Value, ws.Cells[row, 20].Text);
        form.ProviderOrRecieverOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 21].Value);
        form.TransporterOKPO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 22].Value);
        form.StoragePlaceName_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 23].Value);
        form.StoragePlaceCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 24].Value);
        form.CodeRAO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 25].Value);
        form.StatusRAO_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 26].Value);
        form.Volume20_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 27].Value);
        form.Mass21_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 28].Value);
        form.TritiumActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 29].Value);
        form.BetaGammaActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 30].Value);
        form.AlphaActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 31].Value);
        form.TransuraniumActivity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 32].Value);
        form.RefineOrSortRAOCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 33].Value);
        form.Subsidy_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 34].Value);
        form.FcpNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 35].Value);
        form.ContractNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 36].Value);
        return form;
    }

    private static Form19 Map19(ExcelWorksheet ws, int row)
    {
        var form = (Form19)FormCreator.Create("1.9");
        form.NumberInOrder_DB = FormsExcelCellConverters.ParseNullableInt(ws.Cells[row, 8].Value) ?? 0;
        form.OperationCode_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 9].Value);
        form.OperationDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 10].Value, ws.Cells[row, 10].Text);
        form.DocumentVid_DB = FormsExcelCellConverters.ParseNullableByte(ws.Cells[row, 11].Value);
        form.DocumentNumber_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 12].Value);
        form.DocumentDate_DB = FormsExcelCellConverters.NormalizeDate(ws.Cells[row, 13].Value, ws.Cells[row, 13].Text);
        form.CodeTypeAccObject_DB = FormsExcelCellConverters.ParseNullableShort(ws.Cells[row, 14].Value);
        form.Radionuclids_DB = FormsExcelCellConverters.CellStringOrDash(ws.Cells[row, 15].Value);
        form.Activity_DB = FormsExcelCellConverters.ParseActivity(ws.Cells[row, 16].Value);
        return form;
    }
}
