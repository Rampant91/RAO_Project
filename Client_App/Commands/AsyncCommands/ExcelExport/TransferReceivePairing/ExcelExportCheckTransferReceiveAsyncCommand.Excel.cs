using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Client_App.Resources.CustomComparers;
using Client_App.ViewModels.ProgressBar;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Workbook structure

    private static readonly Color PairingFieldMatchFill = Color.FromArgb(198, 239, 206);
    private static readonly Color PairingFieldMismatchFill = Color.FromArgb(255, 205, 210);
    private static readonly Color PairingLegendTitleFill = Color.FromArgb(33, 78, 128);
    private static readonly Color PairingLegendSectionFill = Color.FromArgb(217, 226, 243);
    private static readonly Color PairingLegendBorder = Color.FromArgb(180, 180, 180);
    private static readonly Color SourceSectionFill = Color.FromArgb(217, 226, 243);
    private static readonly Color ClosestSectionFill = Color.FromArgb(255, 242, 204);
    private static readonly Color SeparatorFill = Color.FromArgb(89, 89, 89);

    private const int SourceColCount = 18;
    private const int SeparatorCol = 19;
    private const int ClosestStartCol = 20;
    private const int TotalColCount = 37;
    private const int HeaderRows = 2;
    private const int DataStartRow = 3;

    private void InitializeWorkbook(ExcelPackage excelPackage)
    {
        CreateLegendSheet(excelPackage);
        CreateEmptyFormSheet(excelPackage, "Форма 1.1", "1.1");
        CreateEmptyFormSheet(excelPackage, "Форма 1.2", "1.2");
        CreateEmptyFormSheet(excelPackage, "Форма 1.3", "1.3");
        CreateEmptyFormSheet(excelPackage, "Форма 1.4", "1.4");
        CreateEmptyFormSheet(excelPackage, "Форма 1.5", "1.5");
        CreateEmptyFormSheet(excelPackage, "Форма 1.6", "1.6");
        CreateEmptyFormSheet(excelPackage, "Форма 1.7", "1.7");
        CreateEmptyFormSheet(excelPackage, "Форма 1.8", "1.8");
    }

    private static void CreateLegendSheet(ExcelPackage excelPackage)
    {
        var sheet = excelPackage.Workbook.Worksheets.Add("Легенда");
        excelPackage.Workbook.Worksheets.MoveToStart("Легенда");

        sheet.Cells.Style.Font.Name = "Calibri";
        sheet.Cells.Style.Font.Size = 11;
        sheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        sheet.Column(1).Width = 22;
        sheet.Column(2).Width = 78;
        sheet.View.ShowGridLines = false;

        var row = 1;

        void Title(string text)
        {
            sheet.Cells[row, 1, row, 2].Merge = true;
            var cell = sheet.Cells[row, 1];
            cell.Value = text;
            cell.Style.Font.Size = 16;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Color.SetColor(Color.White);
            cell.Style.Fill.SetBackground(PairingLegendTitleFill, ExcelFillStyle.Solid);
            sheet.Row(row).Height = 28;
            row++;
        }

        void Blank()
        {
            sheet.Row(row).Height = 8;
            row++;
        }

        void Section(string text)
        {
            sheet.Cells[row, 1, row, 2].Merge = true;
            var cell = sheet.Cells[row, 1];
            cell.Value = text;
            cell.Style.Font.Size = 12;
            cell.Style.Font.Bold = true;
            cell.Style.Fill.SetBackground(PairingLegendSectionFill, ExcelFillStyle.Solid);
            sheet.Row(row).Height = 22;
            row++;
        }

        void Body(string text)
        {
            sheet.Cells[row, 1, row, 2].Merge = true;
            var cell = sheet.Cells[row, 1];
            cell.Value = text;
            cell.Style.WrapText = true;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            sheet.Row(row).Height = EstimateWrappedRowHeight(text, 100);
            row++;
        }

        void BoldBody(string text)
        {
            sheet.Cells[row, 1, row, 2].Merge = true;
            var cell = sheet.Cells[row, 1];
            cell.Value = text;
            cell.Style.WrapText = true;
            cell.Style.Font.Bold = true;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            sheet.Row(row).Height = EstimateWrappedRowHeight(text, 100);
            row++;
        }

        void Bullet(string text) => Body("•  " + text);

        void ColorRow(Color fill, string label, string explanation)
        {
            var sample = sheet.Cells[row, 1];
            sample.Value = label;
            sample.Style.Fill.SetBackground(fill, ExcelFillStyle.Solid);
            sample.Style.Font.Bold = true;
            sample.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sample.Style.Border.BorderAround(ExcelBorderStyle.Thin, PairingLegendBorder);
            sheet.Cells[row, 2].Value = explanation;
            sheet.Cells[row, 2].Style.WrapText = true;
            sheet.Row(row).Height = EstimateWrappedRowHeight(explanation, 78);
            row++;
        }

        Title("Проверка операций приёма-передачи — как читать отчёт");
        Blank();

        Section("Структура листов «Форма 1.1» и «Форма 1.3»");
        Body("Слева — непарная операция выбранной организации. Справа после тёмной разделительной колонки (жёлтый заголовок «Ближайшее совпадение у контрагента») — наиболее похожая операция у контрагента. Формы сверяются только сами с собой (1.1↔1.1, 1.3↔1.3).");
        Bullet("Голубой заголовок слева — исходная (непарная) операция.");
        Bullet("Жёлтый заголовок справа — ближайшее совпадение.");
        Bullet("Форма 1.3: вместо количества — агрегатное состояние (1/2/3); количество всегда считается равным 1.");
        Blank();

        Section("Что такое «ближайшее совпадение»");
        Body("«Ближайшее совпадение» — это не найденная пара (иначе строка не попала бы в отчёт), а подсказка: какая операция у контрагента больше всего похожа на непарную строку.");
        Bullet("Программа сравнивает непарную строку с операциями контрагента противоположной стороны (передача↔приём) в окне ±15 дней по дате операции.");
        Bullet("Для каждого кандидата считается, сколько включённых полей совпало (галочки в окне параметров перед выгрузкой).");
        Bullet("В правый блок попадает кандидат с наибольшим числом совпавших полей. Зелёные и красные ячейки показывают, что совпало, а что нет (одинаковая подсветка слева и справа).");
        Body("Частый случай — у контрагента нет парной операции: справа окажется наиболее похожая из имеющихся («чужая» строка). Красные ячейки тогда могут указывать на ложные расхождения: проблема в отсутствии пары, а не в опечатках.");
        Body("Другой случай — настоящая парная строка есть, но в ней много ошибок, а рядом лежит почти идентичная «похожая чужая» (например, отличается один символ в заводском номере). Программа выберет её, потому что совпавших полей больше; подсветка снова может вводить в заблуждение.");
        BoldBody("Важно: зелёная и красная подсветка — это предположение программы о возможных расхождениях с наиболее похожей строкой, а не точный диагноз с гарантией 100%. Сначала убедитесь, что справа ожидаемая парная операция контрагента (а не пропуск ввода и не «похожая чужая» строка); только после этого ориентируйтесь на красные ячейки.");
        Blank();

        Section("Цвета ячеек");
        ColorRow(PairingFieldMatchFill, "Зелёный", "Поле совпало с ближайшим совпадением (если справа действительно ожидаемая пара). Код — по таблице парности; дата — только точное совпадение; активность — ±10%; агрегатное состояние — точное совпадение.");
        ColorRow(PairingFieldMismatchFill, "Красный", "Поле не совпало с ближайшим совпадением. Подсказка, где смотреть — но только если справа подходящая строка, а не случайный похожий кандидат.");
        Body("Без заливки — сравнение по полю не выполнялось. Так бывает, если ближайшего совпадения нет (пустой ОКПО, нет контрагента, нет операций противоположной стороны, нет кандидатов в окне ±15 дней) либо галочка поля снята в параметрах.");
        Blank();

        Section("Пустые паспорт и заводской номер");
        Body("Если паспорт и заводской номер пустые (или заглушки вроде «-», «б.н.»), несколько строк могут описывать одну партию: сравнивается суммарное количество при совпадении остальных ключевых полей, включая номер упаковки.");
        Bullet("В подсветке «ближайшего совпадения» количество сравнивается построчно (одинаковые числа — зелёные). Красное количество значит, что у этой пары строк числа разные (например 8 и 5), а не «всегда ошибка» для безсерийных.");
        Blank();

        Section("Параметры сравнения");
        Body("Перед выгрузкой выбираются поля сопоставления отдельно для форм 1.1 и 1.3. Рег.№, ОКПО организации, наименование, период и № п/п всегда только для наглядности и в сравнении не участвуют.");
        Bullet("Активность: допуск ±10%.");
        Bullet("Дата операции: для признания пары и зелёной подсветки нужно точное совпадение. Окно ±15 дней только сужает поиск кандидатов (ускорение), кандидаты вне окна не рассматриваются.");
        Bullet("Код операции: для пары нужны коды из таблицы 21↔31, 22↔32 и т.д. (на формах 1.1/1.3; пара 26↔36 относится к формам 1.5–1.8).");
        Bullet("Агрегатное состояние (форма 1.3): точное совпадение значений 1/2/3.");
        Blank();

        Section("Краткий порядок работы");
        Bullet("Сначала проверьте, что справа — ожидаемая парная операция контрагента, а не просто похожая чужая строка.");
        Bullet("Если справа подходящий кандидат — смотрите красные ячейки как подсказку по расхождениям.");
        Bullet("Если парной операции нет — ищите пропущенный ввод у контрагента, а не правьте данные только по цветам.");
        Bullet("Проверьте ОКПО контрагента (кол. 19) и код операции.");

        sheet.View.FreezePanes(3, 1);
    }

    private static double EstimateWrappedRowHeight(string text, double approxCharsPerLine)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 18;
        }

        var lines = Math.Max(1, (int)Math.Ceiling(text.Length / Math.Max(1.0, approxCharsPerLine)));
        return Math.Min(72, 16 + lines * 14);
    }

    private void CreateEmptyFormSheet(ExcelPackage excelPackage, string sheetName, string formNum)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add(sheetName);
        if (formNum == "1.1")
        {
            SetupForm11Headers();
        }
        else if (formNum == "1.3")
        {
            SetupForm13Headers();
        }
        else
        {
            Worksheet.Cells[1, 1].Value = "Сверка для этой формы будет добавлена позже.";
            Worksheet.Cells[1, 1].Style.Font.Italic = true;
            Worksheet.Cells[1, 1].Style.Font.Color.SetColor(Color.FromArgb(100, 100, 100));
        }
    }

    /// <summary>
    /// Дописывает непарные строки одной организации на листы 1.1 и 1.3.
    /// CurrentRow берётся через <see cref="GetNextDataRow"/> — в режиме «вся БД»
    /// организации не перезаписывают друг друга.
    /// <paramref name="orgIndex"/>/<paramref name="orgCount"/> &gt; 0 — префикс «Организация i из N» в прогрессе
    /// (для multi-org всегда передавайте <paramref name="progressBarVM"/>, не null).
    /// </summary>
    private void AppendOrganizationToWorkbook(
        ExcelPackage excelPackage,
        OrganizationTransferReceiveExport export,
        AnyTaskProgressBarVM? progressBarVM,
        int percentBase = 0,
        int percentSpan = 0,
        int orgIndex = 0,
        int orgCount = 0,
        string? orgLabel = null)
    {
        var halfSpan = Math.Max(0, percentSpan / 2);

        string Stage(string sheetStage) =>
            FormatOrgExcelStage(orgIndex, orgCount, sheetStage, orgLabel);

        if (progressBarVM is not null && percentSpan > 0)
        {
            progressBarVM.SetProgressBar(percentBase, Stage("заполнение листа «Форма 1.1»"));
        }

        Worksheet = excelPackage.Workbook.Worksheets["Форма 1.1"];
        CurrentRow = GetNextDataRow(Worksheet);
        WriteForm11RowsFromDto(export.UnpairedForm11, progressBarVM, percentBase, halfSpan, Stage);

        if (progressBarVM is not null && percentSpan > 0)
        {
            progressBarVM.SetProgressBar(percentBase + halfSpan, Stage("заполнение листа «Форма 1.3»"));
        }

        Worksheet = excelPackage.Workbook.Worksheets["Форма 1.3"];
        CurrentRow = GetNextDataRow(Worksheet);
        WriteForm13RowsFromDto(export.UnpairedForm13, progressBarVM, percentBase + halfSpan, percentSpan - halfSpan, Stage);
    }

    /// <summary>Следующая свободная строка данных на листе (после заголовков / уже записанных org).</summary>
    private static int GetNextDataRow(ExcelWorksheet sheet)
    {
        var lastRow = sheet.Dimension?.End.Row ?? HeaderRows;
        return lastRow < DataStartRow ? DataStartRow : lastRow + 1;
    }

    private void WriteForm11RowsFromDto(
        System.Collections.Generic.List<TransferReceiveDto> unpaired,
        AnyTaskProgressBarVM? progressBarVM,
        int percentBase,
        int percentSpan,
        Func<string, string>? formatStage = null)
    {
        formatStage ??= static s => s;
        Action<int, string>? report = progressBarVM is null
            ? null
            : (percent, text) => progressBarVM.SetProgressBar(percent, text);
        var progress = new ProgressReporter(report, percentBase, percentBase + Math.Max(0, percentSpan));
        var total = unpaired.Count;
        var done = 0;
        progress.Report(0, total, formatStage($"заполнение листа «Форма 1.1»: 0 из {total}"));

        foreach (var row in OrderForExport(unpaired))
        {
            WriteOperationBlock(row, startCol: 1, applyHighlight: true, isSource: true,
                closestMatches: _form11ClosestMatches, writeAggregateState: false);
            Worksheet.Cells[CurrentRow, SeparatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

            if (_form11ClosestMatches.TryGetValue(row.Id, out var closest))
            {
                WriteOperationBlock(closest.Candidate, startCol: ClosestStartCol, applyHighlight: true, isSource: false,
                    closestMatches: _form11ClosestMatches, writeAggregateState: false, closest: closest);
            }

            CurrentRow++;
            done++;
            progress.Report(done, total, formatStage($"заполнение листа «Форма 1.1»: {done} из {total}"));
        }
    }

    private void WriteForm13RowsFromDto(
        System.Collections.Generic.List<TransferReceiveDto> unpaired,
        AnyTaskProgressBarVM? progressBarVM,
        int percentBase,
        int percentSpan,
        Func<string, string>? formatStage = null)
    {
        formatStage ??= static s => s;
        Action<int, string>? report = progressBarVM is null
            ? null
            : (percent, text) => progressBarVM.SetProgressBar(percent, text);
        var progress = new ProgressReporter(report, percentBase, percentBase + Math.Max(0, percentSpan));
        var total = unpaired.Count;
        var done = 0;
        progress.Report(0, total, formatStage($"заполнение листа «Форма 1.3»: 0 из {total}"));

        foreach (var row in OrderForExport(unpaired))
        {
            WriteOperationBlock(row, startCol: 1, applyHighlight: true, isSource: true,
                closestMatches: _form13ClosestMatches, writeAggregateState: true);
            Worksheet.Cells[CurrentRow, SeparatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

            if (_form13ClosestMatches.TryGetValue(row.Id, out var closest))
            {
                WriteOperationBlock(closest.Candidate, startCol: ClosestStartCol, applyHighlight: true, isSource: false,
                    closestMatches: _form13ClosestMatches, writeAggregateState: true, closest: closest);
            }

            CurrentRow++;
            done++;
            progress.Report(done, total, formatStage($"заполнение листа «Форма 1.3»: {done} из {total}"));
        }
    }

    private static void FinalizeWorkbookTables(ExcelPackage excelPackage)
    {
        FinalizeFormSheetTable(excelPackage.Workbook.Worksheets["Форма 1.1"]);
        FinalizeFormSheetTable(excelPackage.Workbook.Worksheets["Форма 1.3"]);
    }

    private static void FinalizeFormSheetTable(ExcelWorksheet sheet)
    {
        var lastRow = sheet.Dimension?.End.Row ?? HeaderRows;
        if (lastRow >= DataStartRow)
        {
            sheet.Cells[HeaderRows, 1, HeaderRows, TotalColCount].AutoFilter = true;
            ApplyThinGridBorders(sheet, DataStartRow, lastRow);
        }
    }

    /// <summary>Тонкая сетка по ячейкам данных (без затирания заливки подсветки).</summary>
    private static void ApplyThinGridBorders(ExcelWorksheet sheet, int firstRow, int lastRow)
    {
        var borderColor = Color.FromArgb(180, 180, 180);
        for (var row = firstRow; row <= lastRow; row++)
        {
            for (var col = 1; col <= TotalColCount; col++)
            {
                if (col == SeparatorCol)
                {
                    continue;
                }

                var border = sheet.Cells[row, col].Style.Border;
                border.Top.Style = ExcelBorderStyle.Thin;
                border.Bottom.Style = ExcelBorderStyle.Thin;
                border.Left.Style = ExcelBorderStyle.Thin;
                border.Right.Style = ExcelBorderStyle.Thin;
                border.Top.Color.SetColor(borderColor);
                border.Bottom.Color.SetColor(borderColor);
                border.Left.Color.SetColor(borderColor);
                border.Right.Color.SetColor(borderColor);
            }
        }
    }

    #endregion

    #region Headers

    private void SetupForm11Headers()
    {
        SetupSharedFormHeaders(quantityOrAggregateHeader: "Количество, шт");
    }

    private void SetupForm13Headers()
    {
        SetupSharedFormHeaders(quantityOrAggregateHeader: "Агрегатное состояние");
    }

    private void SetupSharedFormHeaders(string quantityOrAggregateHeader)
    {
        var sheet = Worksheet;

        sheet.Cells[1, 1, 1, SourceColCount].Merge = true;
        sheet.Cells[1, 1].Value = "Непарная операция выбранной организации";
        sheet.Cells[1, 1].Style.Font.Bold = true;
        sheet.Cells[1, 1].Style.Font.Color.SetColor(Color.FromArgb(30, 30, 30));
        sheet.Cells[1, 1, 1, SourceColCount].Style.Fill.SetBackground(SourceSectionFill, ExcelFillStyle.Solid);
        sheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        sheet.Cells[1, SeparatorCol].Value = string.Empty;
        sheet.Column(SeparatorCol).Width = 2.5;
        sheet.Cells[1, SeparatorCol, 2, SeparatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

        sheet.Cells[1, ClosestStartCol, 1, TotalColCount].Merge = true;
        sheet.Cells[1, ClosestStartCol].Value = "Ближайшее совпадение у контрагента";
        sheet.Cells[1, ClosestStartCol].Style.Font.Bold = true;
        sheet.Cells[1, ClosestStartCol, 1, TotalColCount].Style.Fill.SetBackground(ClosestSectionFill, ExcelFillStyle.Solid);
        sheet.Cells[1, ClosestStartCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        WriteFieldHeaders(2, startCol: 1, quantityOrAggregateHeader);
        WriteFieldHeaders(2, startCol: ClosestStartCol, quantityOrAggregateHeader);

        sheet.Cells[2, SeparatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

        ApplyForm11FieldHeaderStyle(sheet, 1, SourceColCount);
        ApplyForm11FieldHeaderStyle(sheet, ClosestStartCol, TotalColCount);

        sheet.Columns[SeparatorCol].Style.Border.Left.Style = ExcelBorderStyle.Medium;
        sheet.Columns[SeparatorCol].Style.Border.Right.Style = ExcelBorderStyle.Medium;
        sheet.Columns[SeparatorCol].Style.Border.Left.Color.SetColor(SeparatorFill);
        sheet.Columns[SeparatorCol].Style.Border.Right.Color.SetColor(SeparatorFill);

        sheet.Row(1).Height = 22;
        sheet.Row(2).Height = ExcelHeaderRowMinHeight;
        ApplyForm11ColumnWidths(sheet);
        sheet.View.FreezePanes(DataStartRow, 1);
    }

    /// <summary>Синяя шапка наименований колонок — как в остальных Excel-выгрузках.</summary>
    private static void ApplyForm11FieldHeaderStyle(ExcelWorksheet sheet, int firstCol, int lastCol)
    {
        var headerRange = sheet.Cells[HeaderRows, firstCol, HeaderRows, lastCol];
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(ExcelHeaderFillColor);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Font.Size = ExcelHeaderFontSize;
        headerRange.Style.Font.Color.SetColor(ExcelHeaderFontColor);
        headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        headerRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        headerRange.Style.WrapText = true;

        for (var col = firstCol; col <= lastCol; col++)
        {
            var border = sheet.Cells[HeaderRows, col].Style.Border;
            border.Top.Style = ExcelBorderStyle.Thin;
            border.Bottom.Style = ExcelBorderStyle.Thin;
            border.Left.Style = ExcelBorderStyle.Thin;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Top.Color.SetColor(ExcelHeaderBorderColor);
            border.Bottom.Color.SetColor(ExcelHeaderBorderColor);
            border.Left.Color.SetColor(ExcelHeaderBorderColor);
            border.Right.Color.SetColor(ExcelHeaderBorderColor);
        }
    }

    /// <summary>
    /// Ширины колонок в «пикселях» Excel (перевод в единицы EPPlus: (px − 5) / 7).
    /// </summary>
    private static void ApplyForm11ColumnWidths(ExcelWorksheet sheet)
    {
        // Рег.№, ОКПО, наим., нач.пер., кон.пер., №п/п, код, дата, паспорт, тип,
        // рад., зав.№, кол-во, акт., ОКПО изг., дата вып., ОКПО пост/пол., УКТ
        int[] widthsPx =
        [
            70,
            90,
            210, // сокращённое наименование ~в 1.5 раза шире типичного autofit заголовка
            110,
            110,
            50,
            50,
            110,
            140,
            80,
            130,
            120,
            80,
            120,
            110,
            110,
            130,
            100
        ];

        for (var i = 0; i < widthsPx.Length; i++)
        {
            var width = ExcelWidthFromPixels(widthsPx[i]);
            sheet.Column(1 + i).Width = width;
            sheet.Column(ClosestStartCol + i).Width = width;
        }

        sheet.Column(SeparatorCol).Width = 2.5;
    }

    private static double ExcelWidthFromPixels(int pixels) =>
        Math.Max(1.0, (pixels - 5) / 7.0);

    private void WriteFieldHeaders(int row, int startCol, string quantityOrAggregateHeader = "Количество, шт")
    {
        string[] headers =
        [
            "Рег.№",
            "ОКПО",
            "Сокращенное наименование",
            "Дата начала периода",
            "Дата конца периода",
            "№ п/п",
            "Код",
            "Дата",
            "Номер паспорта (сертификата)",
            "Тип",
            "Радионуклиды",
            "Заводской номер",
            quantityOrAggregateHeader,
            "Суммарная активность",
            "Код ОКПО изготовителя",
            "Дата выпуска",
            "ОКПО поставщика или получателя",
            "Номер УКТ"
        ];

        for (var i = 0; i < headers.Length; i++)
        {
            Worksheet.Cells[row, startCol + i].Value = headers[i];
        }
    }

    #endregion

    #region Write form rows

    private void WriteOperationBlock(
        TransferReceiveDto op,
        int startCol,
        bool applyHighlight,
        bool isSource,
        System.Collections.Generic.Dictionary<int, ClosestMatchResult> closestMatches,
        bool writeAggregateState,
        ClosestMatchResult? closest = null)
    {
        void WriteDate(int col, string? value) =>
            Worksheet.Cells[CurrentRow, col].Value = ConvertToExcelDate(value, Worksheet, CurrentRow, col);

        var c = startCol;
        Worksheet.Cells[CurrentRow, c++].Value = op.OrgRegNo;
        Worksheet.Cells[CurrentRow, c++].Value = op.OrgOkpo;
        Worksheet.Cells[CurrentRow, c++].Value = op.OrgShortName;
        WriteDate(c++, op.StartPeriod);
        WriteDate(c++, op.EndPeriod);
        Worksheet.Cells[CurrentRow, c++].Value = op.NumberInOrder;
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.OpCode);
        WriteDate(c++, op.OpDate);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PasNum);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.Type);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.Radionuclids);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.FacNum);
        if (writeAggregateState)
        {
            Worksheet.Cells[CurrentRow, c++].Value = op.AggregateState is null ? "-" : op.AggregateState;
        }
        else
        {
            Worksheet.Cells[CurrentRow, c++].Value = op.Quantity is null ? "-" : op.Quantity;
        }

        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Activity);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.CreatorOkpo);
        WriteDate(c++, op.CreationDate);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.ProviderOrRecieverOkpo);
        Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelString(op.PackNumber);

        if (!applyHighlight)
        {
            return;
        }

        ClosestMatchResult? highlightSource = closest;
        if (isSource)
        {
            closestMatches.TryGetValue(op.Id, out highlightSource);
        }

        if (highlightSource is null)
        {
            return;
        }

        foreach (var (field, matches) in highlightSource.FieldMatches)
        {
            if (GetComparableColumnOffset(field) is int offset)
            {
                Worksheet.Cells[CurrentRow, startCol + offset].Style.Fill.SetBackground(
                    matches ? PairingFieldMatchFill : PairingFieldMismatchFill,
                    ExcelFillStyle.Solid);
            }
        }
    }

    /// <summary>Смещение колонки сравниваемого поля внутри блока из 18 колонок (0-based index from start of block).</summary>
    private static int? GetComparableColumnOffset(TransferReceiveField field) =>
        field switch
        {
            TransferReceiveField.OperationCode => 6,
            TransferReceiveField.OperationDate => 7,
            TransferReceiveField.PassportNumber => 8,
            TransferReceiveField.Type => 9,
            TransferReceiveField.Radionuclids => 10,
            TransferReceiveField.FactoryNumber => 11,
            TransferReceiveField.Quantity => 12,
            TransferReceiveField.AggregateState => 12,
            TransferReceiveField.Activity => 13,
            TransferReceiveField.CreatorOkpo => 14,
            TransferReceiveField.CreationDate => 15,
            TransferReceiveField.ProviderOrRecieverOkpo => 16,
            TransferReceiveField.PackNumber => 17,
            _ => null
        };

    #endregion

    #region Common helpers

    private static readonly CustomReportsComparer OrgRegNoComparer = new();

    /// <summary>
    /// Статус прогресса при записи Excel. При orgCount/orgIndex &gt; 0 — префикс «Организация i из N».
    /// Режим «вся БД» должен всегда передавать progressBarVM и эти индексы (не null).
    /// </summary>
    private static string FormatOrgExcelStage(int orgIndex, int orgCount, string stage, string? orgLabel = null)
    {
        if (orgCount <= 0 || orgIndex <= 0)
        {
            return stage;
        }

        var label = string.IsNullOrWhiteSpace(orgLabel) ? string.Empty : $", {orgLabel}";
        return $"Организация {orgIndex} из {orgCount}{label}: {stage}";
    }

    /// <summary>
    /// Порядок строк в Excel: рег.№ (как в списке организаций), начало/конец периода, № п/п, Id.
    /// </summary>
    private static IOrderedEnumerable<TransferReceiveDto> OrderForExport(List<TransferReceiveDto> unpaired) =>
        unpaired
            .OrderBy(op => op.OrgRegNo, OrgRegNoComparer)
            .ThenBy(op => DateOnly.TryParse(op.StartPeriod, out var start) ? start : DateOnly.MaxValue)
            .ThenBy(op => DateOnly.TryParse(op.EndPeriod, out var end) ? end : DateOnly.MaxValue)
            .ThenBy(op => op.NumberInOrder)
            .ThenBy(op => op.Id);

    #endregion
}
