using System;
using System.Collections.Generic;
using System.Linq;
using Models.Collections;
using Models.Forms.Form1;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    private void FillPairingExcel(
        ExcelPackage excelPackage,
        Reports reportsForForm11,
        Reports reportsForForm12,
        Reports reportsForForm13,
        Reports reportsForForm14,
        Reports reportsForForm15,
        Reports reportsForForm16)
    {
        AddPairingSheet(excelPackage, "Форма 1.1", "1.1", reportsForForm11, 29, "tbl_Pair41_Form11", WriteForm11Rows);
        AddPairingSheet(excelPackage, "Форма 1.2", "1.2", reportsForForm12, 27, "tbl_Pair41_Form12", WriteForm12Rows);
        AddPairingSheet(excelPackage, "Форма 1.3", "1.3", reportsForForm13, 27, "tbl_Pair41_Form13", WriteForm13Rows);
        AddPairingSheet(excelPackage, "Форма 1.4", "1.4", reportsForForm14, 28, "tbl_Pair41_Form14", WriteForm14Rows);
        AddPairingSheet(excelPackage, "Форма 1.5", "1.5", reportsForForm15, 32, "tbl_Pair41_Form15", WriteForm15Rows);
        AddPairingSheet(excelPackage, "Форма 1.6", "1.6", reportsForForm16, 26, "tbl_Pair41_Form16", WriteForm16Rows);
    }

    private void AddPairingSheet(
        ExcelPackage excelPackage,
        string sheetName,
        string formNum,
        Reports reports,
        int columnCount,
        string tableName,
        Action writeRows)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add(sheetName);
        SetupHeaders(formNum);
        CurrentReports = reports;
        CurrentRow = Worksheet.Dimension!.End.Row + 1;
        writeRows();
        AddPairingExcelTable(Worksheet, CurrentRow - 1, columnCount, tableName);
    }

    private static void AddPairingExcelTable(ExcelWorksheet sheet, int lastRow, int columnCount, string tableName)
    {
        if (lastRow < 2)
        {
            return;
        }

        var table = sheet.Tables.Add(sheet.Cells[1, 1, lastRow, columnCount], tableName);
        table.TableStyle = TableStyles.Medium2;
        table.ShowRowStripes = true;
    }

    private void SetupHeaders(string formNum)
    {
        FillFormHeadersWithoutNotes(formNum);

        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns();
        }

        ApplyPairingSheetColumnSizing(formNum);

        Worksheet.View.FreezePanes(2, 1);
    }

    private void ApplyPairingSheetColumnSizing(string formNum)
    {
        var lastCol = formNum switch
        {
            "1.1" => 29,
            "1.2" => 27,
            "1.3" => 27,
            "1.4" => 28,
            "1.5" => 32,
            "1.6" => 26,
            _ => 29
        };

        int[] dateColumns = formNum switch
        {
            "1.1" => [4, 5, 9, 17, 24],
            "1.2" => [4, 5, 9, 15, 21],
            "1.3" => [4, 5, 9, 17, 23],
            "1.4" => [4, 5, 9, 15, 21],
            "1.5" => [4, 5, 9, 16, 20],
            "1.6" => [4, 5, 9, 16, 19],
            _ => []
        };

        foreach (var colIndex in dateColumns)
        {
            Worksheet.Column(colIndex).Width *= 1.1;
        }

        Worksheet.Row(1).Height = formNum is "1.5" or "1.4" ? 52 : 44;
        Worksheet.Cells[1, 1, 1, lastCol].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        Worksheet.Cells[1, 1, 1, lastCol].Style.WrapText = true;
    }

    private void FillFormHeadersWithoutNotes(string formNum)
    {
        switch (formNum)
        {
            case "1.1":
                Worksheet.Cells[1, 1].Value = "Рег.№";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Дата начала периода";
                Worksheet.Cells[1, 5].Value = "Дата конца периода";
                Worksheet.Cells[1, 6].Value = "Номер корректировки";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Код";
                Worksheet.Cells[1, 9].Value = "Дата";
                Worksheet.Cells[1, 10].Value = "Номер паспорта (сертификата)";
                Worksheet.Cells[1, 11].Value = "Тип";
                Worksheet.Cells[1, 12].Value = "Радионуклиды";
                Worksheet.Cells[1, 13].Value = "Заводской номер";
                Worksheet.Cells[1, 14].Value = "Количество, шт";
                Worksheet.Cells[1, 15].Value = "Суммарная активность, Бк";
                Worksheet.Cells[1, 16].Value = "Код ОКПО изготовителя";
                Worksheet.Cells[1, 17].Value = "Дата выпуска";
                Worksheet.Cells[1, 18].Value = "Категория";
                Worksheet.Cells[1, 19].Value = "НСС, мес";
                Worksheet.Cells[1, 20].Value = "Код формы собственности";
                Worksheet.Cells[1, 21].Value = "Код ОКПО правообладателя";
                Worksheet.Cells[1, 22].Value = "Вид документа";
                Worksheet.Cells[1, 23].Value = "Номер документа";
                Worksheet.Cells[1, 24].Value = "Дата документа";
                Worksheet.Cells[1, 25].Value = "ОКПО поставщика или получателя";
                Worksheet.Cells[1, 26].Value = "ОКПО перевозчика";
                Worksheet.Cells[1, 27].Value = "Наименование упаковки";
                Worksheet.Cells[1, 28].Value = "Тип УКТ";
                Worksheet.Cells[1, 29].Value = "Номер УКТ";
                break;

            case "1.2":
                Worksheet.Cells[1, 1].Value = "Рег.№";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Дата начала периода";
                Worksheet.Cells[1, 5].Value = "Дата конца периода";
                Worksheet.Cells[1, 6].Value = "Номер корректировки";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Код";
                Worksheet.Cells[1, 9].Value = "Дата";
                Worksheet.Cells[1, 10].Value = "Номер паспорта (сертификата)";
                Worksheet.Cells[1, 11].Value = "Наименование ИОУ";
                Worksheet.Cells[1, 12].Value = "Заводской номер";
                Worksheet.Cells[1, 13].Value = "Масса, кг";
                Worksheet.Cells[1, 14].Value = "Код ОКПО изготовителя";
                Worksheet.Cells[1, 15].Value = "Дата выпуска";
                Worksheet.Cells[1, 16].Value = "НСС, мес";
                Worksheet.Cells[1, 17].Value = "Код формы собственности";
                Worksheet.Cells[1, 18].Value = "Код ОКПО правообладателя";
                Worksheet.Cells[1, 19].Value = "Вид документа";
                Worksheet.Cells[1, 20].Value = "Номер документа";
                Worksheet.Cells[1, 21].Value = "Дата документа";
                Worksheet.Cells[1, 22].Value = "ОКПО поставщика или получателя";
                Worksheet.Cells[1, 23].Value = "ОКПО перевозчика";
                Worksheet.Cells[1, 24].Value = "Наименование упаковки";
                Worksheet.Cells[1, 25].Value = "Тип УКТ";
                Worksheet.Cells[1, 26].Value = "Номер УКТ";
                break;

            case "1.3":
                Worksheet.Cells[1, 1].Value = "Рег.№";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Дата начала периода";
                Worksheet.Cells[1, 5].Value = "Дата конца периода";
                Worksheet.Cells[1, 6].Value = "Номер корректировки";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Код";
                Worksheet.Cells[1, 9].Value = "Дата";
                Worksheet.Cells[1, 10].Value = "Номер паспорта (сертификата)";
                Worksheet.Cells[1, 11].Value = "Тип";
                Worksheet.Cells[1, 12].Value = "Радионуклиды";
                Worksheet.Cells[1, 13].Value = "Заводской номер";
                Worksheet.Cells[1, 14].Value = "Суммарная активность, Бк";
                Worksheet.Cells[1, 15].Value = "Код ОКПО изготовителя";
                Worksheet.Cells[1, 16].Value = "Дата выпуска";
                Worksheet.Cells[1, 17].Value = "Агрегатное состояние";
                Worksheet.Cells[1, 18].Value = "Код формы собственности";
                Worksheet.Cells[1, 19].Value = "Код ОКПО правообладателя";
                Worksheet.Cells[1, 20].Value = "Вид документа";
                Worksheet.Cells[1, 21].Value = "Номер документа";
                Worksheet.Cells[1, 22].Value = "Дата документа";
                Worksheet.Cells[1, 23].Value = "ОКПО поставщика или получателя";
                Worksheet.Cells[1, 24].Value = "ОКПО перевозчика";
                Worksheet.Cells[1, 25].Value = "Наименование упаковки";
                Worksheet.Cells[1, 26].Value = "Тип УКТ";
                Worksheet.Cells[1, 27].Value = "Номер УКТ";
                break;

            case "1.4":
                Worksheet.Cells[1, 1].Value = "Рег.№";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Дата начала периода";
                Worksheet.Cells[1, 5].Value = "Дата конца периода";
                Worksheet.Cells[1, 6].Value = "Номер корректировки";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Код";
                Worksheet.Cells[1, 9].Value = "Дата";
                Worksheet.Cells[1, 10].Value = "Номер паспорта (сертификата)";
                Worksheet.Cells[1, 11].Value = "Наименование";
                Worksheet.Cells[1, 12].Value = "Сорт";
                Worksheet.Cells[1, 13].Value = "Радионуклиды";
                Worksheet.Cells[1, 14].Value = "Суммарная активность, Бк";
                Worksheet.Cells[1, 15].Value = "Дата измерения активности";
                Worksheet.Cells[1, 16].Value = "Объём, м³";
                Worksheet.Cells[1, 17].Value = "Масса, кг";
                Worksheet.Cells[1, 18].Value = "Агрегатное состояние";
                Worksheet.Cells[1, 19].Value = "Код формы собственности";
                Worksheet.Cells[1, 20].Value = "Код ОКПО правообладателя";
                Worksheet.Cells[1, 21].Value = "Вид документа";
                Worksheet.Cells[1, 22].Value = "Номер документа";
                Worksheet.Cells[1, 23].Value = "Дата документа";
                Worksheet.Cells[1, 24].Value = "ОКПО поставщика или получателя";
                Worksheet.Cells[1, 25].Value = "ОКПО перевозчика";
                Worksheet.Cells[1, 26].Value = "Наименование упаковки";
                Worksheet.Cells[1, 27].Value = "Тип УКТ";
                Worksheet.Cells[1, 28].Value = "Номер УКТ";
                break;

            case "1.5":
                Worksheet.Cells[1, 1].Value = "Рег.№";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Дата начала периода";
                Worksheet.Cells[1, 5].Value = "Дата конца периода";
                Worksheet.Cells[1, 6].Value = "Номер корректировки";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Код";
                Worksheet.Cells[1, 9].Value = "Дата";
                Worksheet.Cells[1, 10].Value = "Номер паспорта (сертификата) ЗРИ, акта определения характеристик ОЗИИ";
                Worksheet.Cells[1, 11].Value = "Тип";
                Worksheet.Cells[1, 12].Value = "Радионуклиды";
                Worksheet.Cells[1, 13].Value = "Заводской номер";
                Worksheet.Cells[1, 14].Value = "Количество, шт";
                Worksheet.Cells[1, 15].Value = "Суммарная активность, Бк";
                Worksheet.Cells[1, 16].Value = "Дата выпуска";
                Worksheet.Cells[1, 17].Value = "Статус РАО";
                Worksheet.Cells[1, 18].Value = "Вид документа";
                Worksheet.Cells[1, 19].Value = "Номер документа";
                Worksheet.Cells[1, 20].Value = "Дата документа";
                Worksheet.Cells[1, 21].Value = "ОКПО поставщика или получателя";
                Worksheet.Cells[1, 22].Value = "ОКПО перевозчика";
                Worksheet.Cells[1, 23].Value = "Наименование упаковки";
                Worksheet.Cells[1, 24].Value = "Тип УКТ";
                Worksheet.Cells[1, 25].Value = "Номер УКТ";
                Worksheet.Cells[1, 26].Value = "Наименование места хранения";
                Worksheet.Cells[1, 27].Value = "Код места хранения";
                Worksheet.Cells[1, 28].Value = "Код переработки / сортировки РАО";
                Worksheet.Cells[1, 29].Value = "Субсидия, %";
                Worksheet.Cells[1, 30].Value = "Номер мероприятия ФЦП";
                Worksheet.Cells[1, 31].Value = "Номер договора";
                Worksheet.Cells[1, 32].Value = "Текст статуса РАО";
                break;

            case "1.6":
                Worksheet.Cells[1, 1].Value = "Рег.№";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Дата начала периода";
                Worksheet.Cells[1, 5].Value = "Дата конца периода";
                Worksheet.Cells[1, 6].Value = "Номер корректировки";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Код";
                Worksheet.Cells[1, 9].Value = "Дата";
                Worksheet.Cells[1, 10].Value = "Код РАО";
                Worksheet.Cells[1, 11].Value = "Статус РАО";
                Worksheet.Cells[1, 12].Value = "Объём, м³";
                Worksheet.Cells[1, 13].Value = "Масса, кг";
                Worksheet.Cells[1, 14].Value = "Количество ОЗИИ, шт";
                Worksheet.Cells[1, 15].Value = "Основные радионуклиды";
                Worksheet.Cells[1, 16].Value = "Дата измерения активности";
                Worksheet.Cells[1, 17].Value = "Вид документа";
                Worksheet.Cells[1, 18].Value = "Номер документа";
                Worksheet.Cells[1, 19].Value = "Дата документа";
                Worksheet.Cells[1, 20].Value = "ОКПО поставщика или получателя";
                Worksheet.Cells[1, 21].Value = "ОКПО перевозчика";
                Worksheet.Cells[1, 22].Value = "Наименование упаковки";
                Worksheet.Cells[1, 23].Value = "Тип УКТ";
                Worksheet.Cells[1, 24].Value = "Номер УКТ";
                Worksheet.Cells[1, 25].Value = "Наименование места хранения";
                Worksheet.Cells[1, 26].Value = "Текст статуса РАО";
                break;
        }
    }

    private void WriteOrgReportColumns(Report rep, int rowOffset)
    {
        Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.RegNoRep.Value;
        Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
        Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
        Worksheet.Cells[CurrentRow, 4].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 4);
        Worksheet.Cells[CurrentRow, 5].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 5);
        Worksheet.Cells[CurrentRow, 6].Value = rep.CorrectionNumber_DB;
        Worksheet.Cells[CurrentRow, 7].Value = rowOffset;
    }

    private void WriteForm11Rows()
    {
        foreach (var rep in OrderedReports("1.1", rep => rep.Rows11))
        {
            foreach (var repForm in rep.Rows11.OrderBy(form => form.NumberInOrder_DB))
            {
                WriteOrgReportColumns(rep, repForm.NumberInOrder_DB);
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 9);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.Type_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.FactoryNumber_DB);
                Worksheet.Cells[CurrentRow, 14].Value = repForm.Quantity_DB is null ? "-" : repForm.Quantity_DB;
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.Activity_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelString(repForm.CreatorOKPO_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDate(repForm.CreationDate_DB, Worksheet, CurrentRow, 17);
                Worksheet.Cells[CurrentRow, 18].Value = repForm.Category_DB is null ? "-" : repForm.Category_DB;
                Worksheet.Cells[CurrentRow, 19].Value = repForm.SignedServicePeriod_DB is null ? "-" : repForm.SignedServicePeriod_DB;
                Worksheet.Cells[CurrentRow, 20].Value = repForm.PropertyCode_DB is null ? "-" : repForm.PropertyCode_DB;
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelString(repForm.Owner_DB);
                Worksheet.Cells[CurrentRow, 22].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 24);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelString(repForm.PackNumber_DB);
                CurrentRow++;
            }
        }
    }

    private void WriteForm12Rows()
    {
        foreach (var rep in OrderedReports("1.2", rep => rep.Rows12))
        {
            foreach (var repForm in rep.Rows12.OrderBy(form => form.NumberInOrder_DB))
            {
                WriteOrgReportColumns(rep, repForm.NumberInOrder_DB);
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 9);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.NameIOU_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.FactoryNumber_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelDouble(repForm.Mass_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelString(repForm.CreatorOKPO_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDate(repForm.CreationDate_DB, Worksheet, CurrentRow, 15);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDouble(repForm.SignedServicePeriod_DB);
                Worksheet.Cells[CurrentRow, 17].Value = repForm.PropertyCode_DB is null ? "-" : repForm.PropertyCode_DB;
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelString(repForm.Owner_DB);
                Worksheet.Cells[CurrentRow, 19].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 21);
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.PackNumber_DB);
                CurrentRow++;
            }
        }
    }

    private void WriteForm13Rows()
    {
        foreach (var rep in OrderedReports("1.3", rep => rep.Rows13))
        {
            foreach (var repForm in rep.Rows13.OrderBy(form => form.NumberInOrder_DB))
            {
                WriteOrgReportColumns(rep, repForm.NumberInOrder_DB);
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 9);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.Type_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.FactoryNumber_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelDouble(repForm.Activity_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelString(repForm.CreatorOKPO_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDate(repForm.CreationDate_DB, Worksheet, CurrentRow, 16);
                Worksheet.Cells[CurrentRow, 17].Value = repForm.AggregateState_DB is null ? "-" : repForm.AggregateState_DB;
                Worksheet.Cells[CurrentRow, 18].Value = repForm.PropertyCode_DB is null ? "-" : repForm.PropertyCode_DB;
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelString(repForm.Owner_DB);
                Worksheet.Cells[CurrentRow, 20].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 22);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.PackNumber_DB);
                CurrentRow++;
            }
        }
    }

    private void WriteForm14Rows()
    {
        foreach (var rep in OrderedReports("1.4", rep => rep.Rows14))
        {
            foreach (var repForm in rep.Rows14.OrderBy(form => form.NumberInOrder_DB))
            {
                WriteOrgReportColumns(rep, repForm.NumberInOrder_DB);
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 9);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.Name_DB);
                Worksheet.Cells[CurrentRow, 12].Value = repForm.Sort_DB is null ? "-" : repForm.Sort_DB;
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelDouble(repForm.Activity_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDate(repForm.ActivityMeasurementDate_DB, Worksheet, CurrentRow, 15);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDouble(repForm.Volume_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDouble(repForm.Mass_DB);
                Worksheet.Cells[CurrentRow, 18].Value = repForm.AggregateState_DB is null ? "-" : repForm.AggregateState_DB;
                Worksheet.Cells[CurrentRow, 19].Value = repForm.PropertyCode_DB is null ? "-" : repForm.PropertyCode_DB;
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelString(repForm.Owner_DB);
                Worksheet.Cells[CurrentRow, 21].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 23);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.PackNumber_DB);
                CurrentRow++;
            }
        }
    }

    private void WriteForm15Rows()
    {
        foreach (var rep in OrderedReports("1.5", rep => rep.Rows15))
        {
            foreach (var repForm in rep.Rows15.OrderBy(form => form.NumberInOrder_DB))
            {
                WriteOrgReportColumns(rep, repForm.NumberInOrder_DB);
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 9);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.Type_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.FactoryNumber_DB);
                Worksheet.Cells[CurrentRow, 14].Value = repForm.Quantity_DB is null ? "-" : repForm.Quantity_DB;
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.Activity_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDate(repForm.CreationDate_DB, Worksheet, CurrentRow, 16);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelString(repForm.StatusRAO_DB);
                Worksheet.Cells[CurrentRow, 18].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 20);
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.PackNumber_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.StoragePlaceName_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.StoragePlaceCode_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.RefineOrSortRAOCode_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelString(repForm.Subsidy_DB);
                Worksheet.Cells[CurrentRow, 30].Value = ConvertToExcelString(repForm.FcpNumber_DB);
                Worksheet.Cells[CurrentRow, 31].Value = ConvertToExcelString(repForm.ContractNumber_DB);
                Worksheet.Cells[CurrentRow, 32].Value = StatusRaoToDescription(repForm.StatusRAO_DB);
                CurrentRow++;
            }
        }
    }

    private void WriteForm16Rows()
    {
        foreach (var rep in OrderedReports("1.6", rep => rep.Rows16))
        {
            foreach (var repForm in rep.Rows16.OrderBy(form => form.NumberInOrder_DB))
            {
                WriteOrgReportColumns(rep, repForm.NumberInOrder_DB);
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 9);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.CodeRAO_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.StatusRAO_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelDouble(repForm.Volume_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelDouble(repForm.Mass_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelInt(repForm.QuantityOZIII_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelString(repForm.MainRadionuclids_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDate(repForm.ActivityMeasurementDate_DB, Worksheet, CurrentRow, 16);
                Worksheet.Cells[CurrentRow, 17].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 19);
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.PackNumber_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.StoragePlaceName_DB);
                Worksheet.Cells[CurrentRow, 26].Value = StatusRaoToDescription(repForm.StatusRAO_DB);
                CurrentRow++;
            }
        }
    }

    private IEnumerable<Report> OrderedReports<T>(string formNum, Func<Report, ICollection<T>?> getRows)
        where T : Form1 =>
        CurrentReports.Report_Collection
            .Where(rep => rep.FormNum_DB == formNum && getRows(rep) is { Count: > 0 })
            .OrderBy(rep => DateOnly.TryParse(rep.StartPeriod_DB, out var startDate) ? startDate : DateOnly.MaxValue)
            .ThenBy(rep => DateOnly.TryParse(rep.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue);

    private static string StatusRaoToDescription(string? status)
    {
        var tmp = status?.Trim();
        return tmp switch
        {
            "1" => "накопленные",
            "2" => "федеральные",
            "3" => "собственность субъекта РФ",
            "4" => "муниципальная собственность",
            "6" => "бесхозяйные",
            "9" => "прочая собственность",
            "-" or "" or null => "-",
            _ => "вновь образованные"
        };
    }
}
