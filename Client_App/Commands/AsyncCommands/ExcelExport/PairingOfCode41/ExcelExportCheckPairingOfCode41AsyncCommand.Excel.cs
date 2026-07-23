using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Client_App.ViewModels.ProgressBar;
using Models.Collections;
using Models.Forms.Form1;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    #region Workbook structure

    private static readonly Color PairingFieldMatchFill = Color.FromArgb(198, 239, 206);
    private static readonly Color PairingFieldMismatchFill = Color.FromArgb(255, 205, 210);

    /// <summary>
    /// Создаёт 6 листов с заголовками (без данных и без Excel-таблиц).
    /// </summary>
    private void InitializePairingWorkbook(ExcelPackage excelPackage)
    {
        CreateEmptyPairingSheet(excelPackage, "Форма 1.1", "1.1");
        CreateEmptyPairingSheet(excelPackage, "Форма 1.2", "1.2");
        CreateEmptyPairingSheet(excelPackage, "Форма 1.3", "1.3");
        CreateEmptyPairingSheet(excelPackage, "Форма 1.4", "1.4");
        CreateEmptyPairingSheet(excelPackage, "Форма 1.5", "1.5");
        CreateEmptyPairingSheet(excelPackage, "Форма 1.6", "1.6");
    }

    private void CreateEmptyPairingSheet(ExcelPackage excelPackage, string sheetName, string formNum)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add(sheetName);
        SetupHeaders(formNum);
    }

    /// <summary>
    /// Дописывает непарные строки одной организации на уже созданные листы.
    /// Подсветка closest-match берётся из полей экземпляра (пересобраны для этой org).
    /// </summary>
    private void AppendOrganizationToPairingWorkbook(
        ExcelPackage excelPackage,
        OrganizationPairingExport export,
        AnyTaskProgressBarVM? progressBarVM,
        int percentBase = 0,
        int percentSpan = 0)
    {
        SetExportActivitiesCache(export.UnpairedForm13, export.UnpairedForm14);

        void Progress(int step, string sheetTitle)
        {
            if (progressBarVM is null || percentSpan <= 0)
            {
                return;
            }

            var percent = percentBase + percentSpan * step / 6;
            progressBarVM.SetProgressBar(percent, $"Заполнение листа «{sheetTitle}»");
        }

        Progress(0, "Форма 1.1");
        AppendToPairingSheet(excelPackage, "Форма 1.1", export.Form11, WriteForm11Rows);
        Progress(1, "Форма 1.2");
        AppendToPairingSheet(excelPackage, "Форма 1.2", export.Form12, WriteForm12Rows);
        Progress(2, "Форма 1.3");
        AppendToPairingSheet(excelPackage, "Форма 1.3", export.Form13, WriteForm13Rows);
        Progress(3, "Форма 1.4");
        AppendToPairingSheet(excelPackage, "Форма 1.4", export.Form14, WriteForm14Rows);
        Progress(4, "Форма 1.5");
        AppendToPairingSheet(excelPackage, "Форма 1.5", export.Form15, WriteForm15Rows);
        Progress(5, "Форма 1.6");
        AppendToPairingSheet(excelPackage, "Форма 1.6", export.Form16, WriteForm16Rows);
    }

    private void AppendToPairingSheet(
        ExcelPackage excelPackage,
        string sheetName,
        Reports reports,
        Action writeRows)
    {
        Worksheet = excelPackage.Workbook.Worksheets[sheetName];
        CurrentReports = reports;
        CurrentRow = Worksheet.Dimension!.End.Row + 1;
        writeRows();
    }

    /// <summary>
    /// Создаёт Excel-таблицы по фактическому диапазону данных после всех org.
    /// </summary>
    private static void FinalizePairingWorkbookTables(ExcelPackage excelPackage)
    {
        AddPairingExcelTable(excelPackage.Workbook.Worksheets["Форма 1.1"], 29, "tbl_Pair41_Form11");
        AddPairingExcelTable(excelPackage.Workbook.Worksheets["Форма 1.2"], 30, "tbl_Pair41_Form12");
        AddPairingExcelTable(excelPackage.Workbook.Worksheets["Форма 1.3"], 31, "tbl_Pair41_Form13");
        AddPairingExcelTable(excelPackage.Workbook.Worksheets["Форма 1.4"], 33, "tbl_Pair41_Form14");
        AddPairingExcelTable(excelPackage.Workbook.Worksheets["Форма 1.5"], 32, "tbl_Pair41_Form15");
        AddPairingExcelTable(excelPackage.Workbook.Worksheets["Форма 1.6"], 30, "tbl_Pair41_Form16");
    }

    private static void AddPairingExcelTable(ExcelWorksheet sheet, int columnCount, string tableName)
    {
        var lastRow = sheet.Dimension?.End.Row ?? 1;
        if (lastRow < 2)
        {
            return;
        }

        var table = sheet.Tables.Add(sheet.Cells[1, 1, lastRow, columnCount], tableName);
        table.TableStyle = TableStyles.Medium2;
        table.ShowRowStripes = true;
    }

    #endregion

    #region Headers / column sizing

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
            "1.2" => 30,
            "1.3" => 31,
            "1.4" => 33,
            "1.5" => 32,
            "1.6" => 30,
            _ => 29
        };

        int[] dateColumns = formNum switch
        {
            "1.1" => [4, 5, 9, 17, 24],
            "1.2" => [4, 5, 9, 16, 22],
            "1.3" => [4, 5, 9, 17, 23],
            "1.4" => [4, 5, 9, 15, 22],
            "1.5" => [4, 5, 9, 16, 20],
            "1.6" => [4, 5, 9, 20, 23],
            _ => []
        };

        foreach (var colIndex in dateColumns)
        {
            Worksheet.Column(colIndex).Width *= 1.1;
        }

        Worksheet.Row(1).Height = formNum is "1.5" or "1.4" ? 52 : 44;
        Worksheet.Cells[1, 1, 1, lastCol].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        Worksheet.Cells[1, 1, 1, lastCol].Style.WrapText = true;

        // Длинный заголовок паспорта на 1.5 уже переносится — колонку держим уже.
        if (formNum == "1.5")
        {
            Worksheet.Column(10).Width *= 0.5;
        }
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
                Worksheet.Cells[1, 14].Value = "Масса для сопоставления, т";
                Worksheet.Cells[1, 15].Value = "Код ОКПО изготовителя";
                Worksheet.Cells[1, 16].Value = "Дата выпуска";
                Worksheet.Cells[1, 17].Value = "НСС, мес";
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
                Worksheet.Cells[1, 28].Value = "Бета-, гамма-активность, Бк";
                Worksheet.Cells[1, 29].Value = "Альфа-активность, Бк";
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
                Worksheet.Cells[1, 28].Value = "Активность трития, Бк";
                Worksheet.Cells[1, 29].Value = "Бета-, гамма-активность, Бк";
                Worksheet.Cells[1, 30].Value = "Альфа-активность, Бк";
                Worksheet.Cells[1, 31].Value = "Активность трансурановых, Бк";
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
                Worksheet.Cells[1, 18].Value = "Масса для сопоставления, т";
                Worksheet.Cells[1, 19].Value = "Агрегатное состояние";
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
                Worksheet.Cells[1, 30].Value = "Активность трития, Бк";
                Worksheet.Cells[1, 31].Value = "Бета-, гамма-активность, Бк";
                Worksheet.Cells[1, 32].Value = "Альфа-активность, Бк";
                Worksheet.Cells[1, 33].Value = "Активность трансурановых, Бк";
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
                Worksheet.Cells[1, 13].Value = "Масса, т";
                Worksheet.Cells[1, 14].Value = "Количество ОЗИИ, шт";
                Worksheet.Cells[1, 15].Value = "Основные радионуклиды";
                Worksheet.Cells[1, 16].Value = "Активность трития, Бк";
                Worksheet.Cells[1, 17].Value = "Бета-, гамма-активность, Бк";
                Worksheet.Cells[1, 18].Value = "Альфа-активность, Бк";
                Worksheet.Cells[1, 19].Value = "Активность трансурановых, Бк";
                Worksheet.Cells[1, 20].Value = "Дата измерения активности";
                Worksheet.Cells[1, 21].Value = "Вид документа";
                Worksheet.Cells[1, 22].Value = "Номер документа";
                Worksheet.Cells[1, 23].Value = "Дата документа";
                Worksheet.Cells[1, 24].Value = "ОКПО поставщика или получателя";
                Worksheet.Cells[1, 25].Value = "ОКПО перевозчика";
                Worksheet.Cells[1, 26].Value = "Наименование упаковки";
                Worksheet.Cells[1, 27].Value = "Тип УКТ";
                Worksheet.Cells[1, 28].Value = "Номер УКТ";
                Worksheet.Cells[1, 29].Value = "Наименование места хранения";
                Worksheet.Cells[1, 30].Value = "Текст статуса РАО";
                break;
        }
    }

    #endregion

    #region Write form rows

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
                ApplyForm11ClosestMatchHighlight(repForm.Id);
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
                var massTon = ToMassTon(repForm.Mass_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelDouble(massTon);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelString(repForm.CreatorOKPO_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDate(repForm.CreationDate_DB, Worksheet, CurrentRow, 16);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDouble(repForm.SignedServicePeriod_DB);
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
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelDouble(ComputeFromMass(massTon, 25_000_000_000d));
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelDouble(ComputeFromMass(massTon, 16_100_000_000d));
                ApplyForm12ClosestMatchHighlight(repForm.Id);
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
                var activities = ResolveActivitiesForExport(repForm.Id, repForm.Radionuclids_DB, repForm.Activity_DB);
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
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelDouble(activities["tritium"]);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelDouble(activities["beta"]);
                Worksheet.Cells[CurrentRow, 30].Value = ConvertToExcelDouble(activities["alpha"]);
                Worksheet.Cells[CurrentRow, 31].Value = ConvertToExcelDouble(activities["transuranium"]);
                ApplyForm13ClosestMatchHighlight(repForm.Id);
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
                var activities = ResolveActivitiesForExport(repForm.Id, repForm.Radionuclids_DB, repForm.Activity_DB);
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
                var massTon = ToMassTon(repForm.Mass_DB);
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelDouble(massTon);
                Worksheet.Cells[CurrentRow, 19].Value = repForm.AggregateState_DB is null ? "-" : repForm.AggregateState_DB;
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
                Worksheet.Cells[CurrentRow, 30].Value = ConvertToExcelDouble(activities["tritium"]);
                Worksheet.Cells[CurrentRow, 31].Value = ConvertToExcelDouble(activities["beta"]);
                Worksheet.Cells[CurrentRow, 32].Value = ConvertToExcelDouble(activities["alpha"]);
                Worksheet.Cells[CurrentRow, 33].Value = ConvertToExcelDouble(activities["transuranium"]);
                ApplyForm14ClosestMatchHighlight(repForm.Id);
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
                ApplyForm15ClosestMatchHighlight(repForm.Id);
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
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDouble(repForm.TritiumActivity_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDouble(repForm.BetaGammaActivity_DB);
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelDouble(repForm.AlphaActivity_DB);
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelDouble(repForm.TransuraniumActivity_DB);
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelDate(repForm.ActivityMeasurementDate_DB, Worksheet, CurrentRow, 20);
                Worksheet.Cells[CurrentRow, 21].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 23);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.PackNumber_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelString(repForm.StoragePlaceName_DB);
                Worksheet.Cells[CurrentRow, 30].Value = StatusRaoToDescription(repForm.StatusRAO_DB);
                ApplyForm16ClosestMatchHighlight(repForm.Id);
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

    #endregion

    #region Closest-match cell highlight

    private void ApplyForm11ClosestMatchHighlight(int formId)
    {
        if (!_form11ClosestMatchHighlights.TryGetValue(formId, out var highlight))
        {
            return;
        }

        foreach (var (field, matches) in highlight.FieldMatches)
        {
            if (GetForm11Column(field) is int column)
            {
                ApplyPairingComparisonCellFill(CurrentRow, column, matches);
            }
        }
    }

    private void ApplyForm15ClosestMatchHighlight(int formId)
    {
        if (!_form15ClosestMatchHighlights.TryGetValue(formId, out var highlight))
        {
            return;
        }

        foreach (var (field, matches) in highlight.FieldMatches)
        {
            if (GetForm15Column(field) is int column)
            {
                ApplyPairingComparisonCellFill(CurrentRow, column, matches);
            }
        }
    }

    private static void ApplyPairingComparisonCellFill(ExcelWorksheet worksheet, int row, int column, bool matches)
    {
        worksheet.Cells[row, column].Style.Fill.SetBackground(
            matches ? PairingFieldMatchFill : PairingFieldMismatchFill,
            ExcelFillStyle.Solid);
    }

    private void ApplyPairingComparisonCellFill(int row, int column, bool matches) =>
        ApplyPairingComparisonCellFill(Worksheet, row, column, matches);

    private static int? GetForm11Column(Pairing11To15Field field) =>
        field switch
        {
            Pairing11To15Field.OperationDate => 9,
            Pairing11To15Field.PassportNumber => 10,
            Pairing11To15Field.Type => 11,
            Pairing11To15Field.Radionuclids => 12,
            Pairing11To15Field.FactoryNumber => 13,
            Pairing11To15Field.Quantity => 14,
            Pairing11To15Field.Activity => 15,
            Pairing11To15Field.CreationDate => 17,
            Pairing11To15Field.DocumentVid => 22,
            Pairing11To15Field.DocumentNumber => 23,
            Pairing11To15Field.DocumentDate => 24,
            Pairing11To15Field.ProviderOrRecieverOkpo => 25,
            Pairing11To15Field.TransporterOkpo => 26,
            Pairing11To15Field.PackName => 27,
            Pairing11To15Field.PackType => 28,
            Pairing11To15Field.PackNumber => 29,
            _ => null
        };

    private static int? GetForm15Column(Pairing11To15Field field) =>
        field switch
        {
            Pairing11To15Field.OperationDate => 9,
            Pairing11To15Field.PassportNumber => 10,
            Pairing11To15Field.Type => 11,
            Pairing11To15Field.Radionuclids => 12,
            Pairing11To15Field.FactoryNumber => 13,
            Pairing11To15Field.Quantity => 14,
            Pairing11To15Field.Activity => 15,
            Pairing11To15Field.CreationDate => 16,
            Pairing11To15Field.DocumentVid => 18,
            Pairing11To15Field.DocumentNumber => 19,
            Pairing11To15Field.DocumentDate => 20,
            Pairing11To15Field.ProviderOrRecieverOkpo => 21,
            Pairing11To15Field.TransporterOkpo => 22,
            Pairing11To15Field.PackName => 23,
            Pairing11To15Field.PackType => 24,
            Pairing11To15Field.PackNumber => 25,
            _ => null
        };

    private void ApplyForm12ClosestMatchHighlight(int formId)
    {
        if (!_form12ClosestMatchHighlights.TryGetValue(formId, out var map)) return;
        foreach (var (field, matched) in map)
        {
            var col = field switch
            {
                Pairing12To16Field.OperationDate => 9,
                Pairing12To16Field.Mass => 14,
                Pairing12To16Field.BetaGammaActivity => 28,
                Pairing12To16Field.AlphaActivity => 29,
                // Для 1.2 дата измерения активности при переводе = дата операции.
                Pairing12To16Field.ActivityMeasurementDate => 9,
                Pairing12To16Field.DocumentVid => 20,
                Pairing12To16Field.DocumentNumber => 21,
                Pairing12To16Field.DocumentDate => 22,
                Pairing12To16Field.PackName => 25,
                Pairing12To16Field.PackType => 26,
                Pairing12To16Field.PackNumber => 27,
                _ => -1
            };
            if (col > 0) ApplyPairingComparisonCellFill(CurrentRow, col, matched);
        }
    }

    private void ApplyForm13ClosestMatchHighlight(int formId)
    {
        if (!_form13ClosestMatchHighlights.TryGetValue(formId, out var map)) return;
        foreach (var (field, matched) in map)
        {
            var col = field switch
            {
                Pairing13To16Field.OperationDate => 9,
                Pairing13To16Field.MainRadionuclids => 12,
                Pairing13To16Field.TritiumActivity => 28,
                Pairing13To16Field.BetaGammaActivity => 29,
                Pairing13To16Field.AlphaActivity => 30,
                Pairing13To16Field.TransuraniumActivity => 31,
                Pairing13To16Field.ActivityMeasurementDate => 16,
                Pairing13To16Field.DocumentVid => 20,
                Pairing13To16Field.DocumentNumber => 21,
                Pairing13To16Field.DocumentDate => 22,
                Pairing13To16Field.PackName => 25,
                Pairing13To16Field.PackType => 26,
                Pairing13To16Field.PackNumber => 27,
                _ => -1
            };
            if (col > 0) ApplyPairingComparisonCellFill(CurrentRow, col, matched);
        }
    }

    private void ApplyForm14ClosestMatchHighlight(int formId)
    {
        if (!_form14ClosestMatchHighlights.TryGetValue(formId, out var map)) return;
        foreach (var (field, matched) in map)
        {
            var col = field switch
            {
                Pairing14To16Field.OperationDate => 9,
                Pairing14To16Field.Volume => 16,
                Pairing14To16Field.Mass => 18,
                Pairing14To16Field.MainRadionuclids => 13,
                Pairing14To16Field.TritiumActivity => 30,
                Pairing14To16Field.BetaGammaActivity => 31,
                Pairing14To16Field.AlphaActivity => 32,
                Pairing14To16Field.TransuraniumActivity => 33,
                Pairing14To16Field.ActivityMeasurementDate => 15,
                Pairing14To16Field.DocumentVid => 22,
                Pairing14To16Field.DocumentNumber => 23,
                Pairing14To16Field.DocumentDate => 24,
                Pairing14To16Field.PackName => 27,
                Pairing14To16Field.PackType => 28,
                Pairing14To16Field.PackNumber => 29,
                _ => -1
            };
            if (col > 0) ApplyPairingComparisonCellFill(CurrentRow, col, matched);
        }
    }

    private void ApplyForm16ClosestMatchHighlight(int formId)
    {
        if (!_form16ClosestMatchHighlights.TryGetValue(formId, out var highlight))
        {
            return;
        }

        switch (highlight.Profile)
        {
            case Form16MatchProfile.Form12 when highlight.Matches12 is not null:
                foreach (var (field, matched) in highlight.Matches12)
                {
                    var col = field switch
                    {
                        Pairing12To16Field.OperationDate => 9,
                        Pairing12To16Field.Mass => 13,
                        Pairing12To16Field.BetaGammaActivity => 17,
                        Pairing12To16Field.AlphaActivity => 18,
                        Pairing12To16Field.ActivityMeasurementDate => 20,
                        Pairing12To16Field.DocumentVid => 21,
                        Pairing12To16Field.DocumentNumber => 22,
                        Pairing12To16Field.DocumentDate => 23,
                        Pairing12To16Field.PackName => 26,
                        Pairing12To16Field.PackType => 27,
                        Pairing12To16Field.PackNumber => 28,
                        _ => -1
                    };
                    if (col > 0) ApplyPairingComparisonCellFill(CurrentRow, col, matched);
                }
                break;

            case Form16MatchProfile.Form13 when highlight.Matches13 is not null:
                foreach (var (field, matched) in highlight.Matches13)
                {
                    var col = field switch
                    {
                        Pairing13To16Field.OperationDate => 9,
                        Pairing13To16Field.MainRadionuclids => 15,
                        Pairing13To16Field.TritiumActivity => 16,
                        Pairing13To16Field.BetaGammaActivity => 17,
                        Pairing13To16Field.AlphaActivity => 18,
                        Pairing13To16Field.TransuraniumActivity => 19,
                        Pairing13To16Field.ActivityMeasurementDate => 20,
                        Pairing13To16Field.DocumentVid => 21,
                        Pairing13To16Field.DocumentNumber => 22,
                        Pairing13To16Field.DocumentDate => 23,
                        Pairing13To16Field.PackName => 26,
                        Pairing13To16Field.PackType => 27,
                        Pairing13To16Field.PackNumber => 28,
                        _ => -1
                    };
                    if (col > 0) ApplyPairingComparisonCellFill(CurrentRow, col, matched);
                }
                break;

            case Form16MatchProfile.Form14 when highlight.Matches14 is not null:
                foreach (var (field, matched) in highlight.Matches14)
                {
                    var col = field switch
                    {
                        Pairing14To16Field.OperationDate => 9,
                        Pairing14To16Field.Volume => 12,
                        Pairing14To16Field.Mass => 13,
                        Pairing14To16Field.MainRadionuclids => 15,
                        Pairing14To16Field.TritiumActivity => 16,
                        Pairing14To16Field.BetaGammaActivity => 17,
                        Pairing14To16Field.AlphaActivity => 18,
                        Pairing14To16Field.TransuraniumActivity => 19,
                        Pairing14To16Field.ActivityMeasurementDate => 20,
                        Pairing14To16Field.DocumentVid => 21,
                        Pairing14To16Field.DocumentNumber => 22,
                        Pairing14To16Field.DocumentDate => 23,
                        Pairing14To16Field.PackName => 26,
                        Pairing14To16Field.PackType => 27,
                        Pairing14To16Field.PackNumber => 28,
                        _ => -1
                    };
                    if (col > 0) ApplyPairingComparisonCellFill(CurrentRow, col, matched);
                }
                break;
        }
    }

    #endregion
}
