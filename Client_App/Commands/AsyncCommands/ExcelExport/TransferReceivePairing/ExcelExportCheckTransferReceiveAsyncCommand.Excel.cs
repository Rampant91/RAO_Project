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
    private static readonly Color PairingFieldNearFill = Color.FromArgb(255, 243, 160);
    private static readonly Color PairingFieldMismatchFill = Color.FromArgb(255, 205, 210);
    private static readonly Color PairingLegendTitleFill = Color.FromArgb(33, 78, 128);
    private static readonly Color PairingLegendSectionFill = Color.FromArgb(217, 226, 243);
    private static readonly Color PairingLegendBorder = Color.FromArgb(180, 180, 180);
    private static readonly Color SourceSectionFill = Color.FromArgb(217, 226, 243);
    private static readonly Color ClosestSectionFill = Color.FromArgb(255, 242, 204);
    private static readonly Color SeparatorFill = Color.FromArgb(89, 89, 89);

    /// <summary>
    /// Число колонок блока полей (левый/правый). Зависит от формы:
    /// 1.1/1.3 — 18, 1.2 — 17, 1.4 — 19.
    /// </summary>
    private static int SourceColCountFor(TransferReceiveSheetLayout layout) =>
        layout switch
        {
            TransferReceiveSheetLayout.Form12 => 17,
            TransferReceiveSheetLayout.Form14 => 19,
            _ => 18
        };

    private static int SeparatorColFor(TransferReceiveSheetLayout layout) =>
        SourceColCountFor(layout) + 1;

    private static int ConfidenceColFor(TransferReceiveSheetLayout layout) =>
        SourceColCountFor(layout) + 2;

    private static int ClosestStartColFor(TransferReceiveSheetLayout layout) =>
        SourceColCountFor(layout) + 3;

    private static int TotalColCountFor(TransferReceiveSheetLayout layout) =>
        ClosestStartColFor(layout) + SourceColCountFor(layout) - 1;

    /// <summary>Совместимость тестов: раскладка формы 1.1 (18 колонок в блоке данных).</summary>
    private static int SourceColCount => SourceColCountFor(TransferReceiveSheetLayout.Form11);

    private static int SeparatorCol => SeparatorColFor(TransferReceiveSheetLayout.Form11);

    private static int ConfidenceCol => ConfidenceColFor(TransferReceiveSheetLayout.Form11);

    private static int ClosestStartCol => ClosestStartColFor(TransferReceiveSheetLayout.Form11);

    private static int TotalColCount => TotalColCountFor(TransferReceiveSheetLayout.Form11);

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

        Section("Структура листов «Форма 1.1»–«Форма 1.4»");
        Body("Слева — непарная операция выбранной организации. Справа после тёмной разделительной колонки — наиболее похожая операция у контрагента (жёлтый заголовок «Ближайшее совпадение у контрагента»).");
        Bullet("Голубой заголовок слева — исходная (непарная) операция.");
        Bullet("Жёлтый заголовок справа — наиболее похожая операция у контрагента.");
        Bullet("Если в параметрах у формы сняты все поля («Выбрать все» = выкл.) — форма не загружается, не сопоставляется и не заполняется в Excel.");
        Bullet("Форма 1.2: наименование изделия, тип УКТ, масса обеднённого урана (кг); количество всегда считается равным 1.");
        Bullet("Форма 1.3: количество всегда считается равным 1.");
        Bullet("Форма 1.4: наименование, вид, объём, дата измерения активности, масса (кг), агрегатное состояние; без зав.№ и изготовителя; количество = 1.");
        Blank();

        Section("Что такое «ближайшее совпадение»");
        Body("«Ближайшее совпадение» — это не найденная пара (иначе строка не попала бы в отчёт), а подсказка: какая операция у контрагента больше всего похожа на непарную строку.");
        Bullet("Программа ищет среди операций контрагента противоположной стороны (передача и приём) с датой операции в пределах ±15 дней.");
        Bullet("Насколько строки похожи, оценивается по выбранным в параметрах полям. Важнее всего паспорт и заводской номер; код операции влияет слабее.");
        Bullet("Учитываются обычные различия в записи: опечатка в одном знаке, похожие на вид символы (например, 0 и буква О, цифра 3 и буква З), разные написания одного и того же номера или типа, варианты УКТ и т.п.");
        Bullet("Справа показывается самая похожая строка. Цвета ячеек: зелёный — совпало, жёлтый — небольшое отличие, красный — сильное отличие.");
        Bullet("Колонка «Схожесть, %» показывает, насколько правая строка близка к левой (от 0 до 100). Это ориентир для чтения отчёта, а не точная вероятность.");
        Body("Частый случай — у контрагента нет парной операции: справа окажется просто наиболее похожая из имеющихся («чужая» строка). Цвета тогда могут вводить в заблуждение: дело в отсутствии пары, а не в опечатках.");
        BoldBody("Важно: цвета и % — предположение программы о возможных расхождениях с наиболее похожей строкой, а не окончательный вывод. Сначала убедитесь, что справа ожидаемая парная операция контрагента; только после этого ориентируйтесь на цвета и %.");
        Blank();

        Section("Цвета ячеек полей");
        ColorRow(PairingFieldMatchFill, "Зелёный", "Поле совпало (с учётом обычной нормализации записи). Для кода операции — парные коды приёма и передачи; для активности — в допуске ±10%.");
        ColorRow(PairingFieldNearFill, "Жёлтый", "Небольшое отличие: опечатка в одном знаке, дата в пределах ±15 дней, непарный код приёма/передачи, похожие на вид символы, разные написания одного номера или типа, оба пустых паспорта/заводского номера и т.п.");
        ColorRow(PairingFieldMismatchFill, "Красный", "Сильное отличие: разные основные части номера, нет общего радионуклида в списке, несколько опечаток подряд, сильно разные ОКПО и т.п. (если справа подходящая строка, а не случайная похожая).");
        Body("Без заливки — по этому полю сравнение не делалось (нет правой строки, поле снято в параметрах или нечего сравнивать).");
        Blank();

        Section("Схожесть, %");
        ColorRow(PairingFieldMatchFill, "≥ 80%", "Строки очень похожи.");
        ColorRow(PairingFieldNearFill, "50–79%", "Средняя похожесть — смотрите жёлтые и красные поля.");
        ColorRow(PairingFieldMismatchFill, "< 50%", "Слабая похожесть — справа может быть «чужая» строка или данные сильно различаются.");
        Body("Чем важнее поле и чем оно ближе, тем выше %. Если паспорт и заводской номер совпали полностью (и не пустые), схожесть дополнительно повышается.");
        Blank();

        Section("Пустые паспорт и заводской номер");
        Body("Если паспорт и заводской номер пустые (или вместо них стоят «-», «б.н.» и т.п.), несколько строк могут относиться к одной партии: при поиске пары сравнивается суммарное количество. В правой части отчёта оба пустых номера дают жёлтый цвет, а «пусто напротив заполненного» — красный.");
        Bullet("Количество в правой части сравнивается по каждой строке отдельно (одинаковые числа — зелёные).");
        Blank();

        Section("Параметры сравнения");
        Body("Перед выгрузкой выбираются поля сопоставления отдельно для форм 1.1, 1.2, 1.3 и 1.4.");
        Bullet("Активность (1.1/1.3/1.4): допуск ±10% для пары и зелёной подсветки; отличие примерно на порядок обычно жёлтое.");
        Bullet("Масса (1.2/1.4) и объём (1.4): тот же допуск ±10%; путаница кг и тонн (ровно в 1000 раз) — жёлтый.");
        Bullet("Дата операции и дата измерения активности (1.4): для пары нужна одна и та же дата. Окно ±15 дней только ограничивает поиск похожих строк; отличие в несколько дней справа — жёлтый.");
        Bullet("Код операции: для пары нужны соответствующие коды приёма и передачи (например, 21 и 31). Непарный код из того же набора справа — жёлтый.");
        Bullet("Радионуклиды (1.1/1.3/1.4): порядок в списке не важен; если нуклида нет у одной из сторон — красный.");
        Bullet("Тип УКТ (1.2): сравнивается как обычное текстовое поле, но сильнее влияет на «Схожесть, %», чем большинство других полей.");
        Bullet("Агрегатное состояние (1.3/1.4) и вид (1.4): значения должны совпасть.");
        Blank();

        Section("Краткий порядок работы");
        Bullet("Сначала проверьте, что справа — ожидаемая парная операция контрагента, а не просто похожая чужая строка (смотрите % и ключевые поля).");
        Bullet("Если справа подходящая строка — смотрите жёлтые и красные ячейки как подсказку, чем записи отличаются.");
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
        else if (formNum == "1.2")
        {
            SetupForm12Headers();
        }
        else if (formNum == "1.3")
        {
            SetupForm13Headers();
        }
        else if (formNum == "1.4")
        {
            SetupForm14Headers();
        }
        else
        {
            Worksheet.Cells[1, 1].Value = "Сверка для этой формы будет добавлена позже.";
            Worksheet.Cells[1, 1].Style.Font.Italic = true;
            Worksheet.Cells[1, 1].Style.Font.Color.SetColor(Color.FromArgb(100, 100, 100));
        }
    }

    /// <summary>
    /// Дописывает непарные строки одной организации на листы включённых форм.
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
        var enabledForms = _currentParams?.EnabledForms ?? ImplementedFormDescriptors;
        var sheetSpan = enabledForms.Count == 0
            ? percentSpan
            : Math.Max(1, percentSpan / enabledForms.Count);

        string Stage(string sheetStage) =>
            FormatOrgExcelStage(orgIndex, orgCount, sheetStage, orgLabel);

        for (var i = 0; i < enabledForms.Count; i++)
        {
            var descriptor = enabledForms[i];
            var unpaired = export.GetUnpaired(descriptor.Id);
            if (unpaired.Count == 0)
            {
                continue;
            }

            var sheetBase = percentBase + i * sheetSpan;
            if (progressBarVM is not null && percentSpan > 0)
            {
                progressBarVM.SetProgressBar(sheetBase, Stage($"заполнение листа «{descriptor.SheetName}»"));
            }

            Worksheet = excelPackage.Workbook.Worksheets[descriptor.SheetName];
            CurrentRow = GetNextDataRow(Worksheet);
            WriteFormRowsFromDto(
                descriptor,
                unpaired,
                progressBarVM,
                sheetBase,
                sheetSpan,
                Stage);
        }
    }

    /// <summary>Следующая свободная строка данных на листе (после заголовков / уже записанных org).</summary>
    private static int GetNextDataRow(ExcelWorksheet sheet)
    {
        var lastRow = sheet.Dimension?.End.Row ?? HeaderRows;
        return lastRow < DataStartRow ? DataStartRow : lastRow + 1;
    }

    private void WriteFormRowsFromDto(
        TransferReceiveFormDescriptor descriptor,
        List<TransferReceiveDto> unpaired,
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
        progress.Report(0, total, formatStage($"заполнение листа «{descriptor.SheetName}»: 0 из {total}"));

        if (!_closestByForm.TryGetValue(descriptor.Id, out var closestMatches))
        {
            closestMatches = new Dictionary<int, ClosestMatchResult>();
        }

        var layout = descriptor.Layout;
        var sepCol = SeparatorColFor(layout);
        var confCol = ConfidenceColFor(layout);
        var closestStart = ClosestStartColFor(layout);

        foreach (var row in OrderForExport(unpaired))
        {
            WriteOperationBlock(row, startCol: 1, applyHighlight: true, isSource: true,
                closestMatches: closestMatches, layout: layout);
            Worksheet.Cells[CurrentRow, sepCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

            if (closestMatches.TryGetValue(row.Id, out var closest))
            {
                WriteConfidenceCell(confCol, closest.ConfidencePercent);
                WriteOperationBlock(closest.Candidate, startCol: closestStart, applyHighlight: true, isSource: false,
                    closestMatches: closestMatches,
                    layout: layout,
                    closest: closest);
            }

            CurrentRow++;
            done++;
            progress.Report(done, total, formatStage($"заполнение листа «{descriptor.SheetName}»: {done} из {total}"));
        }
    }

    private static void FinalizeWorkbookTables(ExcelPackage excelPackage)
    {
        foreach (var descriptor in ImplementedFormDescriptors)
        {
            FinalizeFormSheetTable(excelPackage.Workbook.Worksheets[descriptor.SheetName], descriptor.Layout);
        }
    }

    private static void FinalizeFormSheetTable(ExcelWorksheet sheet, TransferReceiveSheetLayout layout)
    {
        var lastRow = sheet.Dimension?.End.Row ?? HeaderRows;
        var totalCols = TotalColCountFor(layout);
        var sepCol = SeparatorColFor(layout);
        if (lastRow >= DataStartRow)
        {
            sheet.Cells[HeaderRows, 1, HeaderRows, totalCols].AutoFilter = true;
            ApplyThinGridBorders(sheet, DataStartRow, lastRow, totalCols, sepCol);
        }
    }

    /// <summary>
    /// Тонкая сетка по диапазонам слева/справа от разделителя (без поячеечного цикла).
    /// </summary>
    private static void ApplyThinGridBorders(
        ExcelWorksheet sheet,
        int firstRow,
        int lastRow,
        int totalColCount,
        int separatorCol)
    {
        var borderColor = Color.FromArgb(180, 180, 180);

        void ApplyBlock(int fromCol, int toCol)
        {
            if (toCol < fromCol || lastRow < firstRow)
            {
                return;
            }

            var range = sheet.Cells[firstRow, fromCol, lastRow, toCol];
            var border = range.Style.Border;
            border.Top.Style = ExcelBorderStyle.Thin;
            border.Bottom.Style = ExcelBorderStyle.Thin;
            border.Left.Style = ExcelBorderStyle.Thin;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Top.Color.SetColor(borderColor);
            border.Bottom.Color.SetColor(borderColor);
            border.Left.Color.SetColor(borderColor);
            border.Right.Color.SetColor(borderColor);
        }

        ApplyBlock(1, separatorCol - 1);
        ApplyBlock(separatorCol + 1, totalColCount);
    }

    #endregion

    #region Headers

    private void SetupForm11Headers()
    {
        SetupSharedFormHeaders(TransferReceiveSheetLayout.Form11);
    }

    private void SetupForm12Headers()
    {
        SetupSharedFormHeaders(TransferReceiveSheetLayout.Form12);
    }

    private void SetupForm13Headers()
    {
        SetupSharedFormHeaders(TransferReceiveSheetLayout.Form13);
    }

    private void SetupForm14Headers()
    {
        SetupSharedFormHeaders(TransferReceiveSheetLayout.Form14);
    }

    private void SetupSharedFormHeaders(TransferReceiveSheetLayout layout)
    {
        var sheet = Worksheet;
        var sourceCols = SourceColCountFor(layout);
        var sepCol = SeparatorColFor(layout);
        var confCol = ConfidenceColFor(layout);
        var closestStart = ClosestStartColFor(layout);
        var totalCols = TotalColCountFor(layout);

        sheet.Cells[1, 1, 1, sourceCols].Merge = true;
        sheet.Cells[1, 1].Value = "Непарная операция выбранной организации";
        sheet.Cells[1, 1].Style.Font.Bold = true;
        sheet.Cells[1, 1].Style.Font.Color.SetColor(Color.FromArgb(30, 30, 30));
        sheet.Cells[1, 1, 1, sourceCols].Style.Fill.SetBackground(SourceSectionFill, ExcelFillStyle.Solid);
        sheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        sheet.Cells[1, sepCol].Value = string.Empty;
        sheet.Column(sepCol).Width = 2.5;
        sheet.Cells[1, sepCol, 2, sepCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

        sheet.Cells[1, confCol].Value = string.Empty;
        sheet.Cells[1, confCol].Style.Fill.SetBackground(ClosestSectionFill, ExcelFillStyle.Solid);

        sheet.Cells[1, closestStart, 1, totalCols].Merge = true;
        sheet.Cells[1, closestStart].Value = "Ближайшее совпадение у контрагента";
        sheet.Cells[1, closestStart].Style.Font.Bold = true;
        sheet.Cells[1, closestStart, 1, totalCols].Style.Fill.SetBackground(ClosestSectionFill, ExcelFillStyle.Solid);
        sheet.Cells[1, closestStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        WriteFieldHeaders(2, startCol: 1, layout);
        WriteFieldHeaders(2, startCol: closestStart, layout);

        sheet.Cells[2, sepCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);
        sheet.Cells[2, confCol].Value = "Схожесть, %";
        sheet.Cells[2, confCol].Style.Font.Bold = true;
        sheet.Cells[2, confCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        sheet.Cells[2, confCol].Style.WrapText = true;

        ApplyForm11FieldHeaderStyle(sheet, 1, sourceCols);
        ApplyForm11FieldHeaderStyle(sheet, confCol, confCol);
        ApplyForm11FieldHeaderStyle(sheet, closestStart, totalCols);

        sheet.Columns[sepCol].Style.Border.Left.Style = ExcelBorderStyle.Medium;
        sheet.Columns[sepCol].Style.Border.Right.Style = ExcelBorderStyle.Medium;
        sheet.Columns[sepCol].Style.Border.Left.Color.SetColor(SeparatorFill);
        sheet.Columns[sepCol].Style.Border.Right.Color.SetColor(SeparatorFill);

        sheet.Row(1).Height = 22;
        sheet.Row(2).Height = ExcelHeaderRowMinHeight;
        ApplyFormColumnWidths(sheet, layout);
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
    /// Порядок совпадает с заголовками формы (без пустых слотов).
    /// </summary>
    private static void ApplyFormColumnWidths(ExcelWorksheet sheet, TransferReceiveSheetLayout layout)
    {
        int[] widthsPx = layout switch
        {
            TransferReceiveSheetLayout.Form12 =>
            [
                70, 90, 210, 110, 110, 50, 50, 110,
                140, // паспорт
                120, // наименование
                120, // зав.№
                130, // масса
                110, // ОКПО изг.
                110, // дата вып.
                130, // ОКПО пост/пол.
                100, // тип УКТ
                100  // номер УКТ
            ],
            TransferReceiveSheetLayout.Form13 =>
            [
                70, 90, 210, 110, 110, 50, 50, 110,
                140, 80, 130, 120,
                120, // активность
                110, // ОКПО изг.
                110, // дата вып.
                90,  // агр. состояние (после даты выпуска, как в форме)
                130, // ОКПО пост/пол.
                100  // номер УКТ
            ],
            TransferReceiveSheetLayout.Form14 =>
            [
                70, 90, 210, 110, 110, 50, 50, 110,
                140, // паспорт
                140, // наименование
                60,  // вид
                130, // радионуклиды
                120, // активность
                120, // дата изм. акт.
                100, // объём
                100, // масса
                90,  // агр. состояние
                130, // ОКПО пост/пол.
                100  // номер УКТ
            ],
            _ => // Form11
            [
                70, 90, 210, 110, 110, 50, 50, 110,
                140, 80, 130, 120, 80, 120,
                110, 110, 130,
                100  // номер УКТ (тип УКТ не выгружаем — не участвует в сверке)
            ]
        };

        var closestStart = ClosestStartColFor(layout);
        for (var i = 0; i < widthsPx.Length; i++)
        {
            var width = ExcelWidthFromPixels(widthsPx[i]);
            sheet.Column(1 + i).Width = width;
            sheet.Column(closestStart + i).Width = width;
        }

        sheet.Column(SeparatorColFor(layout)).Width = 2.5;
        sheet.Column(ConfidenceColFor(layout)).Width = ExcelWidthFromPixels(70);
    }

    private static double ExcelWidthFromPixels(int pixels) =>
        Math.Max(1.0, (pixels - 5) / 7.0);

    private void WriteFieldHeaders(int row, int startCol, TransferReceiveSheetLayout layout) =>
        WriteFieldHeadersToSheet(Worksheet, row, startCol, layout);

    private static void WriteFieldHeadersToSheet(
        ExcelWorksheet sheet,
        int row,
        int startCol,
        TransferReceiveSheetLayout layout)
    {
        // Порядок выгружаемых колонок = порядок в форме (пропуская невыгружаемые поля).
        string[] headers = layout switch
        {
            TransferReceiveSheetLayout.Form12 =>
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
                "Наименование",
                "Заводской номер",
                "Масса обеднённого урана, кг",
                "Код ОКПО изготовителя",
                "Дата выпуска",
                "ОКПО поставщика или получателя",
                "Тип УКТ",
                "Номер УКТ"
            ],
            TransferReceiveSheetLayout.Form13 =>
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
                "Суммарная активность",
                "Код ОКПО изготовителя",
                "Дата выпуска",
                "Агрегатное состояние",
                "ОКПО поставщика или получателя",
                "Номер УКТ"
            ],
            TransferReceiveSheetLayout.Form14 =>
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
                "Наименование",
                "Вид",
                "Радионуклиды",
                "Активность, Бк",
                "Дата измерения активности",
                "Объём, куб. м",
                "Масса, кг",
                "Агрегатное состояние",
                "ОКПО поставщика или получателя",
                "Номер УКТ"
            ],
            _ =>
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
                "Количество, шт",
                "Суммарная активность",
                "Код ОКПО изготовителя",
                "Дата выпуска",
                "ОКПО поставщика или получателя",
                "Номер УКТ"
            ]
        };

        for (var i = 0; i < headers.Length; i++)
        {
            sheet.Cells[row, startCol + i].Value = headers[i];
        }
    }

    #endregion

    #region Write form rows

    private void WriteConfidenceCell(int confidenceCol, int confidencePercent)
    {
        var cell = Worksheet.Cells[CurrentRow, confidenceCol];
        cell.Value = confidencePercent;
        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        cell.Style.Font.Bold = true;
        cell.Style.Fill.SetBackground(
            ConfidenceFill(confidencePercent),
            ExcelFillStyle.Solid);
    }

    private static Color ConfidenceFill(int percent) =>
        percent >= 80
            ? PairingFieldMatchFill
            : percent >= 50
                ? PairingFieldNearFill
                : PairingFieldMismatchFill;

    private void WriteOperationBlock(
        TransferReceiveDto op,
        int startCol,
        bool applyHighlight,
        bool isSource,
        System.Collections.Generic.Dictionary<int, ClosestMatchResult> closestMatches,
        TransferReceiveSheetLayout layout,
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

        if (layout == TransferReceiveSheetLayout.Form12)
        {
            // Форма 1.2: Наименование → зав.№ → масса → … → тип УКТ → номер УКТ (без пустых колонок).
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.Type);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.FacNum);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Mass);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.CreatorOkpo);
            WriteDate(c++, op.CreationDate);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.ProviderOrRecieverOkpo);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackType);
            Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelString(op.PackNumber);
        }
        else if (layout == TransferReceiveSheetLayout.Form13)
        {
            // Форма 1.3: … зав.№ → акт. → изг. → дата вып. → агр. → пост/пол. → номер УКТ.
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.Type);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.Radionuclids);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.FacNum);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Activity);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.CreatorOkpo);
            WriteDate(c++, op.CreationDate);
            Worksheet.Cells[CurrentRow, c++].Value = op.AggregateState is null ? "-" : op.AggregateState;
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.ProviderOrRecieverOkpo);
            Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelString(op.PackNumber);
        }
        else if (layout == TransferReceiveSheetLayout.Form14)
        {
            // Форма 1.4: наименование → вид → рад. → акт. → дата изм. → объём → масса → агр. → ОКПО → УКТ.
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.Type);
            Worksheet.Cells[CurrentRow, c++].Value = op.Sort is null ? "-" : op.Sort;
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.Radionuclids);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Activity);
            WriteDate(c++, op.ActivityMeasurementDate);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Volume);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Mass);
            Worksheet.Cells[CurrentRow, c++].Value = op.AggregateState is null ? "-" : op.AggregateState;
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.ProviderOrRecieverOkpo);
            Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelString(op.PackNumber);
        }
        else
        {
            // Форма 1.1: … зав.№ → кол-во → акт. → изг. → дата вып. → пост/пол. → номер УКТ.
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.Type);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.Radionuclids);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.FacNum);
            Worksheet.Cells[CurrentRow, c++].Value = op.Quantity is null ? "-" : op.Quantity;
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Activity);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.CreatorOkpo);
            WriteDate(c++, op.CreationDate);
            Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.ProviderOrRecieverOkpo);
            Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelString(op.PackNumber);
        }

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

        foreach (var (field, level) in highlightSource.FieldLevels)
        {
            if (GetComparableColumnOffset(field, layout) is int offset)
            {
                Worksheet.Cells[CurrentRow, startCol + offset].Style.Fill.SetBackground(
                    FillForMatchLevel(level),
                    ExcelFillStyle.Solid);
            }
        }
    }

    private static Color FillForMatchLevel(FieldMatchLevel level) =>
        level switch
        {
            FieldMatchLevel.Exact => PairingFieldMatchFill,
            FieldMatchLevel.Near => PairingFieldNearFill,
            _ => PairingFieldMismatchFill
        };

    /// <summary>
    /// Смещение колонки сравниваемого поля внутри блока (0-based).
    /// Совпадает с порядком колонок формы / WriteOperationBlock.
    /// </summary>
    private static int? GetComparableColumnOffset(
        TransferReceiveField field,
        TransferReceiveSheetLayout layout = TransferReceiveSheetLayout.Form11) =>
        layout switch
        {
            TransferReceiveSheetLayout.Form12 => field switch
            {
                TransferReceiveField.OperationCode => 6,
                TransferReceiveField.OperationDate => 7,
                TransferReceiveField.PassportNumber => 8,
                TransferReceiveField.Type => 9,
                TransferReceiveField.FactoryNumber => 10,
                TransferReceiveField.Mass => 11,
                TransferReceiveField.CreatorOkpo => 12,
                TransferReceiveField.CreationDate => 13,
                TransferReceiveField.ProviderOrRecieverOkpo => 14,
                TransferReceiveField.PackType => 15,
                TransferReceiveField.PackNumber => 16,
                _ => null
            },
            TransferReceiveSheetLayout.Form13 => field switch
            {
                TransferReceiveField.OperationCode => 6,
                TransferReceiveField.OperationDate => 7,
                TransferReceiveField.PassportNumber => 8,
                TransferReceiveField.Type => 9,
                TransferReceiveField.Radionuclids => 10,
                TransferReceiveField.FactoryNumber => 11,
                TransferReceiveField.Activity => 12,
                TransferReceiveField.CreatorOkpo => 13,
                TransferReceiveField.CreationDate => 14,
                TransferReceiveField.AggregateState => 15,
                TransferReceiveField.ProviderOrRecieverOkpo => 16,
                TransferReceiveField.PackNumber => 17,
                _ => null
            },
            TransferReceiveSheetLayout.Form14 => field switch
            {
                TransferReceiveField.OperationCode => 6,
                TransferReceiveField.OperationDate => 7,
                TransferReceiveField.PassportNumber => 8,
                TransferReceiveField.Type => 9,
                TransferReceiveField.Sort => 10,
                TransferReceiveField.Radionuclids => 11,
                TransferReceiveField.Activity => 12,
                TransferReceiveField.ActivityMeasurementDate => 13,
                TransferReceiveField.Volume => 14,
                TransferReceiveField.Mass => 15,
                TransferReceiveField.AggregateState => 16,
                TransferReceiveField.ProviderOrRecieverOkpo => 17,
                TransferReceiveField.PackNumber => 18,
                _ => null
            },
            _ => field switch
            {
                TransferReceiveField.OperationCode => 6,
                TransferReceiveField.OperationDate => 7,
                TransferReceiveField.PassportNumber => 8,
                TransferReceiveField.Type => 9,
                TransferReceiveField.Radionuclids => 10,
                TransferReceiveField.FactoryNumber => 11,
                TransferReceiveField.Quantity => 12,
                TransferReceiveField.Activity => 13,
                TransferReceiveField.CreatorOkpo => 14,
                TransferReceiveField.CreationDate => 15,
                TransferReceiveField.ProviderOrRecieverOkpo => 16,
                TransferReceiveField.PackNumber => 17,
                _ => null
            }
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
