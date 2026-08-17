using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Client_App.Resources.CustomComparers;
using Client_App.ViewModels.ProgressBar;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
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

    private const int HeaderRows = 2;
    private const int DataStartRow = 3;

    /// <summary>Число общих «информационных» колонок в начале каждого блока (Рег.№ … № п/п).</summary>
    private const int InfoColCount = 7;

    private static readonly string[] InfoHeaders =
    [
        "Рег.№",
        "ОКПО",
        "Сокр. наименование",
        "№ формы",
        "Дата начала периода",
        "Дата конца периода",
        "№ п/п"
    ];

    /// <summary>Разметка одного листа: сколько колонок в блоке, где разделитель, confidence и второй блок.</summary>
    private readonly record struct SheetLayout(int SourceColCount)
    {
        public int SeparatorCol => SourceColCount + 1;
        public int ConfidenceCol => SourceColCount + 2;
        public int ClosestStartCol => SourceColCount + 3;
        public int TotalColCount => SourceColCount * 2 + 2;
    }

    private static readonly SheetLayout Layout1115 = new(InfoColCount + 17);
    private static readonly SheetLayout Layout12 = new(InfoColCount + 12);
    private static readonly SheetLayout Layout13 = new(InfoColCount + 15);
    private static readonly SheetLayout Layout14 = new(InfoColCount + 17);
    private static readonly SheetLayout Layout16 = new(InfoColCount + 16);

    private static readonly string[] Form1115DataHeaders =
    [
        "Код",
        "Дата",
        "Номер паспорта (сертификата)",
        "Тип",
        "Радионуклиды",
        "Заводской номер",
        "Количество, шт",
        "Суммарная активность, Бк",
        "Дата выпуска",
        "Вид документа",
        "Номер документа",
        "Дата документа",
        "ОКПО поставщика или получателя",
        "ОКПО перевозчика",
        "Наименование упаковки",
        "Тип УКТ",
        "Номер УКТ"
    ];

    private static readonly string[] Form12DataHeaders =
    [
        "Дата",
        "Масса, т",
        "Бета-, гамма-активность, Бк",
        "Альфа-активность, Бк",
        "Дата измерения активности",
        "Вид документа",
        "Номер документа",
        "Дата документа",
        "Наименование упаковки",
        "Тип УКТ",
        "Номер УКТ",
        "Код РАО"
    ];

    private static readonly string[] Form13DataHeaders =
    [
        "Дата",
        "Основные радионуклиды",
        "Активность трития, Бк",
        "Бета-, гамма-активность, Бк",
        "Альфа-активность, Бк",
        "Активность трансурановых, Бк",
        "Дата измерения активности",
        "Агрегатное состояние",
        "Вид документа",
        "Номер документа",
        "Дата документа",
        "Наименование упаковки",
        "Тип УКТ",
        "Номер УКТ",
        "Код РАО"
    ];

    private static readonly string[] Form14DataHeaders =
    [
        "Дата",
        "Объём, м³",
        "Масса, т",
        "Основные радионуклиды",
        "Активность трития, Бк",
        "Бета-, гамма-активность, Бк",
        "Альфа-активность, Бк",
        "Активность трансурановых, Бк",
        "Дата измерения активности",
        "Агрегатное состояние",
        "Вид документа",
        "Номер документа",
        "Дата документа",
        "Наименование упаковки",
        "Тип УКТ",
        "Номер УКТ",
        "Код РАО"
    ];

    private static readonly string[] Form16DataHeaders =
    [
        "Дата",
        "Код РАО",
        "Объём, м³",
        "Масса, т",
        "Основные радионуклиды",
        "Активность трития, Бк",
        "Бета-, гамма-активность, Бк",
        "Альфа-активность, Бк",
        "Активность трансурановых, Бк",
        "Дата измерения активности",
        "Вид документа",
        "Номер документа",
        "Дата документа",
        "Наименование упаковки",
        "Тип УКТ",
        "Номер УКТ"
    ];

    /// <summary>Создаёт лист «Легенда» и 6 листов форм с заголовками (без данных).</summary>
    private void InitializePairingWorkbook(ExcelPackage excelPackage)
    {
        CreatePairingLegendSheet(excelPackage);
        CreateEmptyPairingSheet(excelPackage, "Форма 1.1", Layout1115, Form1115DataHeaders);
        CreateEmptyPairingSheet(excelPackage, "Форма 1.2", Layout12, Form12DataHeaders);
        CreateEmptyPairingSheet(excelPackage, "Форма 1.3", Layout13, Form13DataHeaders);
        CreateEmptyPairingSheet(excelPackage, "Форма 1.4", Layout14, Form14DataHeaders);
        CreateEmptyPairingSheet(excelPackage, "Форма 1.5", Layout1115, Form1115DataHeaders);
        CreateEmptyPairingSheet(excelPackage, "Форма 1.6", Layout16, Form16DataHeaders);
    }

    /// <summary>Первый лист книги: пояснения для пользователя (структура блоков, цвета, пары форм, допуски).</summary>
    private static void CreatePairingLegendSheet(ExcelPackage excelPackage)
    {
        var sheet = excelPackage.Workbook.Worksheets.Add("Легенда");
        excelPackage.Workbook.Worksheets.MoveToStart("Легенда");

        sheet.Cells.Style.Font.Name = "Calibri";
        sheet.Cells.Style.Font.Size = 11;
        sheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        sheet.Column(1).Width = 22;
        sheet.Column(2).Width = 78;
        sheet.Column(3).Width = 4;
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
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            sheet.Row(row).Height = 28;
            row++;
        }

        void Subtitle(string text)
        {
            sheet.Cells[row, 1, row, 2].Merge = true;
            var cell = sheet.Cells[row, 1];
            cell.Value = text;
            cell.Style.Font.Size = 11;
            cell.Style.Font.Italic = true;
            cell.Style.Font.Color.SetColor(Color.FromArgb(70, 70, 70));
            sheet.Row(row).Height = 20;
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

            var text = sheet.Cells[row, 2];
            text.Value = explanation;
            text.Style.WrapText = true;
            sheet.Row(row).Height = EstimateWrappedRowHeight(explanation, 78);
            row++;
        }

        void PairRow(string leftForm, string rightForm)
        {
            sheet.Cells[row, 1].Value = leftForm;
            sheet.Cells[row, 1].Style.Font.Bold = true;
            sheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, PairingLegendBorder);
            sheet.Cells[row, 2].Value = rightForm;
            sheet.Cells[row, 2].Style.WrapText = true;
            sheet.Row(row).Height = 18;
            row++;
        }

        Title("Непарные операции с кодом 41 — как читать отчёт");
        Subtitle("Ниже — кратко о том, что означают блоки, строки и цвета на листах форм.");
        Blank();

        Section("Зачем этот файл");
        Body("В отчёт попадают строки операций с кодом 41, для которых не нашлась парная запись при переводе сведений из форм учёта РВ в формы учёта РАО. Файл помогает найти расхождения и восстановить картину перевода.");
        Bullet("Листы «Форма 1.1» … «Форма 1.6» — сами непарные строки.");
        Bullet("Этот лист «Легенда» — пояснения; данные организаций на нём не выводятся.");
        Blank();

        Section("Структура листов форм");
        Body("Каждая строка листа — одна непарная операция. Слева (голубой заголовок) — сама непарная операция. Справа, после тёмной разделительной колонки (жёлтый заголовок «Ближайшее совпадение») — наиболее похожая операция с парной формы. Что именно значит «ближайшее» — в следующем разделе.");
        Bullet("Если ближайшего совпадения нет (парная форма пуста или не нашлось ни одного кандидата) — правый блок остаётся пустым.");
        Bullet("Колонки Рег.№, ОКПО, наименование, № формы, период и № п/п — только для наглядности, в сравнении не участвуют.");
        Blank();

        Section("Что такое «ближайшее совпадение»");
        Body("«Ближайшее совпадение» — это не найденная пара (иначе строка не попала бы в отчёт), а подсказка: какая операция на парной форме больше всего похожа на непарную строку.");
        Bullet("Программа сравнивает непарную строку со всеми операциями той же организации на парной форме.");
        Bullet("Насколько строки похожи, оценивается по полям с галочками в окне параметров (взвешенная оценка: паспорт и зав.№ важнее большинства прочих полей).");
        Bullet("В правый блок попадает наиболее похожий кандидат. Зелёные, жёлтые и красные ячейки показывают степень совпадения (одинаковая подсветка слева и справа).");
        Bullet("Колонка «Схожесть, %» — насколько правая строка близка к левой (0–99). Это ориентир, а не точная вероятность; 100 не используется — справа ближайшее совпадение, а не подтверждённая пара.");
        Bullet("На листе 1.6 кандидат ищется среди форм 1.2, 1.3 и 1.4; при равной оценке предпочтение у формы 1.2.");
        Bullet("Если на парной форме нечего сравнивать — правый блок пустой, подсветки нет. Это нормально и не означает ошибку выгрузки.");
        Body("Частый случай — парную строку не ввели: для операции с кодом 41 на соответствующей форме нет записи. Тогда справа окажется не «настоящая» пара, а просто наиболее похожая из уже имеющихся — другая операция, другой источник или другой период. Красные ячейки сравнивают непарную строку с «чужой» записью и могут указывать на ложные расхождения: проблема не в опечатках, а в том, что парной строки нет вовсе.");
        Body("Другой случай — настоящая парная строка есть, но в ней много опечаток и ошибок (не совпадает сразу несколько полей). Рядом в отчёте может лежать другая, почти идентичная строка, где отличается, например, только один символ в заводском номере, а остальные поля совпадают. Программа выберет именно её как «ближайшую», потому что оценка выше. Подсветка тогда сравнивает не с ожидаемой парой, а с «похожей чужой» строкой и тоже может вводить в заблуждение.");
        BoldBody("Важно: зелёная, жёлтая и красная подсветка — это предположение программы о возможных расхождениях с наиболее похожей строкой, а не точный диагноз с гарантией 100%. Сначала убедитесь, что справа действительно ожидаемая парная операция (а не пропуск ввода и не «похожая чужая» строка); только после этого ориентируйтесь на цвета ячеек.");
        Blank();

        Section("Цвета ячеек");
        Body("На листах форм часть ячеек подкрашена. Цвет показывает сравнение непарной строки с ближайшим совпадением; заливкой отмечены соответствующие ячейки в ОБОИХ блоках.");
        ColorRow(
            PairingFieldMatchFill,
            "Зелёный",
            "Значение совпало с ближайшим совпадением (в т.ч. оба прочерка или пусто, с учётом нормализации и допуска ±10% для активностей/массы/объёма).");
        ColorRow(
            PairingFieldNearFill,
            "Жёлтый",
            "Похоже, но не точное совпадение: опечатка (в т.ч. перестановка соседних знаков), ведущие нули, пропущенное тире в номере, хвост даты у номера, тип с уточнением в скобках, дата в пределах ±15 дней, код 41↔14, диапазон зав.№ напротив одиночного номера из ряда и т.п.");
        ColorRow(
            PairingFieldMismatchFill,
            "Красный",
            "Сильное отличие. Подсказка, где смотреть — но только если справа подходящая строка, а не случайный похожий кандидат.");
        Blank();
        Section("Схожесть, %");
        ColorRow(PairingFieldMatchFill, "≥ 80%", "Строки очень похожи.");
        ColorRow(PairingFieldNearFill, "50–79%", "Средняя похожесть — смотрите жёлтые и красные поля.");
        ColorRow(PairingFieldMismatchFill, "< 50%", "Слабая похожесть — справа может быть «чужая» строка.");
        Body("Без заливки — сравнение по этому полю не выполнялось. Так бывает в трёх случаях:");
        Bullet("на парной форме у организации нет ни одной операции с кодом 41, с которой можно сравнить строку (для 1.1 парная форма — 1.5; для 1.2, 1.3 и 1.4 — 1.6; для 1.5 — 1.1; для 1.6 — 1.2, 1.3 и 1.4);");
        Bullet("перед выгрузкой в окне параметров вы сняли галочку с этого поля — оно не участвует в сравнении и не подсвечивается;");
        Bullet("на листе «Форма 1.6» поле не входит в набор сравнения с формой ближайшего совпадения (1.2, 1.3 или 1.4) — см. раздел ниже.");
        Blank();

        Section("Типичные жёлтые отличия номеров и типа");
        Bullet("Паспорт, зав.№, номер упаковки: ведущие нули в начале номера, пропущенное тире в номере, перестановка двух соседних знаков, дата в конце номера, дописанный хвост №/No/номер с цифрами, две цифры через точку после номера (в т.ч. вместе с ведущими нулями) — жёлтый.");
        Bullet("Паспорт: «Акт №…» и развёрнутое название того же акта, либо акт с одной стороны и только его номер с другой — жёлтый (очень близко).");
        Bullet("Дата выпуска / дата документа / дата измерения: отличие в пределах ±15 дней — жёлтый (чем дальше, тем слабее совпадение).");
        Bullet("Тип / тип упаковки: одно и то же наименование, но у одной стороны добавлено уточнение в скобках — жёлтый.");
        Bullet("Тип / тип упаковки: пропущенное или лишнее тире (пробел вместо тире), а также одна опечатка в достаточно длинном обозначении — жёлтый; короткие пары вроде одной-двух букв так не смягчаются.");
        Bullet("Похожие на вид символы (0 и О, 1 и I/l, 3 и З и т.п.) в типе, номере и упаковке считаются одним и тем же знаком.");
        Bullet("Тип: к совпадающему обозначению после точки дописаны одна-две буквы или цифры (модификатор в конце) — жёлтый, совпадение не гарантировано.");
        Bullet("Тип: у достаточно длинного обозначения дописан короткий буквенный хвост через тире, точку или пробел — жёлтый (слабее обычного «почти совпало»).");
        Bullet("Зав.№ (не паспорт): если с одной стороны перечислены несколько номеров (список или диапазон через тире), а с другой — один номер из этого перечисления, и количество совпадает с числом номеров в перечислении — жёлтый у зав.№ и количества. Обычное тире в номере без такого совпадения количества перечислением не считается.");
        Bullet("Активность при таком перечислении: если у списка указана суммарная активность партии, а у одиночной строки — активность одного изделия, ячейка активности может быть жёлтой (чем лучше сходится «сумма ↔ одно изделие × число штук», тем ближе совпадение).");
        Blank();

        Section("Агрегатное состояние и код РАО");
        Body("На листах 1.3 и 1.4 колонка «Агрегатное состояние» подсвечивается отдельно: зелёным, если её значение совпадает с первой цифрой кода РАО ближайшего совпадения на 1.6, красным — если не совпадает. На листе 1.6, если ближайшее совпадение найдено среди 1.3/1.4, этим же правилом подсвечивается колонка «Код РАО».");
        Blank();

        Section("Какие формы сравниваются");
        PairRow("Форма", "С чем сравнивается");
        sheet.Cells[row - 1, 1].Style.Fill.SetBackground(PairingLegendSectionFill, ExcelFillStyle.Solid);
        sheet.Cells[row - 1, 2].Style.Fill.SetBackground(PairingLegendSectionFill, ExcelFillStyle.Solid);
        sheet.Cells[row - 1, 1].Style.Font.Bold = true;
        sheet.Cells[row - 1, 2].Style.Font.Bold = true;
        PairRow("1.1", "1.5");
        PairRow("1.2", "1.6");
        PairRow("1.3", "1.6");
        PairRow("1.4", "1.6");
        PairRow("1.5", "1.1 (только операции с кодом 41)");
        PairRow("1.6", "1.2, затем 1.3, затем 1.4");
        Blank();

        Section("Лист «Форма 1.6»: зелёные, красные и белые ячейки");
        Body("На листе 1.6 колонки одни и те же для всех строк, а набор сравниваемых полей зависит от того, с какой формой РВ найдено ближайшее совпадение. Смотрите колонку «№ формы» в правом блоке: там будет 1.2, 1.3 или 1.4. Белая ячейка при включённых галочках — не ошибка выгрузки: по этому полю сравнение с выбранным кандидатом просто не делается.");
        Bullet("Ближайшее совпадение с формы 1.2: подсвечиваются масса, β/γ- и α-активности, код РАО, даты, документ и упаковка. Объём, основные радионуклиды, активность трития и трансурановых остаются белыми — в форме 1.2 этих реквизитов нет.");
        Bullet("Ближайшее совпадение с формы 1.3: подсвечиваются основные радионуклиды, четыре вида активностей, код РАО, даты, документ и упаковка. Объём и масса остаются белыми — в ключе парности 1.3↔1.6 их нет.");
        Bullet("Ближайшее совпадение с формы 1.4: подсвечиваются объём, масса, основные радионуклиды, четыре вида активностей, код РАО, даты, документ и упаковка — полный набор колонок данных 1.6.");
        Blank();

        Section("Особый случай: код 14 на форме 1.5");
        Body("Иногда на форме 1.5 вместо кода 41 указывают код 14. Программа учитывает такие строки при поиске пары для формы 1.1 (код 41), но:");
        Bullet("строка формы 1.5 с кодом 14 в этот отчёт не выводится;");
        Bullet("если на 1.1 осталась непарная строка, а ближайшее совпадение на 1.5 имеет код 14, ячейка «Код» на листе 1.1 будет жёлтой — коды 41 и 14 близки, но не совпали.");
        Blank();

        Section("Пустые паспорт, заводской номер и номер упаковки");
        Body("Если паспорт и заводской номер пустые (или стоят заглушки вроде «-», «б.н.», «без номера», «н.д.», «н/д», «нет данных»), несколько строк могут описывать одну партию: одна строка с количеством N или несколько строк, сумма количеств которых равна N. Такие записи считаются одной партией при совпадении остальных ключевых полей, включая номер упаковки.");
        Bullet("В подсветке «ближайшего совпадения» количество сравнивается построчно (одинаковые числа — зелёные), кроме случая диапазона зав.№ выше. Красное количество значит, что у этой пары строк числа разные. Одинаковые прочерки и заглушки («-», «без номера», «б.н.» и т.п.) у паспорта, зав.№ и номера упаковки — зелёные; пусто напротив заполненного — красные.");
        Blank();

        Section("Допуск ±10%");
        Body("Для массы, объёма и активностей допускается расхождение до ±10%.");
        Blank();

        Section("Параметры сравнения (галочки перед выгрузкой)");
        Body("Перед сохранением файла открывается окно, где можно выбрать, по каким полям сравнивать строки. Снятая галочка означает: поле не участвует в решении «парная / непарная» и не подкрашивается на листах форм. Само значение поля в Excel всё равно может быть выведено.");
        Blank();

        Section("Краткий порядок работы");
        Bullet("Откройте нужный лист формы.");
        Bullet("Сначала проверьте, что справа — ожидаемая парная операция, а не просто похожая чужая строка (в том числе когда у настоящей пары много ошибок, а у «соседа» почти всё совпало).");
        Bullet("Если справа подходящий кандидат — смотрите красные ячейки как подсказку по расхождениям.");
        Bullet("Если парной строки нет вовсе — ищите пропущенный ввод на парной форме, а не правьте данные только по цветам.");
        Bullet("При необходимости откройте исходные отчёты организации в программе и исправьте данные, внеся корректировку в отчёт.");

        sheet.View.FreezePanes(3, 1);
        sheet.PrinterSettings.FitToPage = true;
        sheet.PrinterSettings.FitToWidth = 1;
        sheet.PrinterSettings.FitToHeight = 0;
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

    private void CreateEmptyPairingSheet(ExcelPackage excelPackage, string sheetName, SheetLayout layout, string[] dataHeaders)
    {
        Worksheet = excelPackage.Workbook.Worksheets.Add(sheetName);
        SetupPairingFormHeaders(layout, dataHeaders);
    }

    /// <summary>
    /// Дописывает непарные строки одной организации на уже созданные листы.
    /// Подсветка closest-match берётся из полей экземпляра (пересобраны для этой org).
    /// Важно: CurrentRow продолжается с конца листа (не с DataStartRow) — иначе при режиме
    /// «вся БД» организации перезаписывают друг друга, а справа остаётся «чужой» closest.
    /// </summary>
    private void AppendOrganizationToPairingWorkbook(
        ExcelPackage excelPackage,
        OrganizationPairingExport export,
        AnyTaskProgressBarVM? progressBarVM,
        int percentBase = 0,
        int percentSpan = 0)
    {
        var step = percentSpan > 0 ? Math.Max(1, percentSpan / 6) : 0;

        Worksheet = excelPackage.Workbook.Worksheets["Форма 1.1"];
        CurrentRow = GetNextDataRow(Worksheet);
        WriteForm1115Rows(export.UnpairedForm11, _form11ClosestMatchHighlights, Layout1115, "Форма 1.1", progressBarVM, percentBase, step);

        Worksheet = excelPackage.Workbook.Worksheets["Форма 1.2"];
        CurrentRow = GetNextDataRow(Worksheet);
        WriteForm12Rows(export.UnpairedForm12, progressBarVM, percentBase + step, step);

        Worksheet = excelPackage.Workbook.Worksheets["Форма 1.3"];
        CurrentRow = GetNextDataRow(Worksheet);
        WriteForm13Rows(export.UnpairedForm13, progressBarVM, percentBase + step * 2, step);

        Worksheet = excelPackage.Workbook.Worksheets["Форма 1.4"];
        CurrentRow = GetNextDataRow(Worksheet);
        WriteForm14Rows(export.UnpairedForm14, progressBarVM, percentBase + step * 3, step);

        Worksheet = excelPackage.Workbook.Worksheets["Форма 1.5"];
        CurrentRow = GetNextDataRow(Worksheet);
        WriteForm1115Rows(export.UnpairedForm15, _form15ClosestMatchHighlights, Layout1115, "Форма 1.5", progressBarVM, percentBase + step * 4, step);

        Worksheet = excelPackage.Workbook.Worksheets["Форма 1.6"];
        CurrentRow = GetNextDataRow(Worksheet);
        WriteForm16Rows(export.UnpairedForm16, progressBarVM, percentBase + step * 5, step);
    }

    /// <summary>Следующая свободная строка данных на листе (после заголовков / уже записанных org).</summary>
    private static int GetNextDataRow(ExcelWorksheet sheet)
    {
        var lastRow = sheet.Dimension?.End.Row ?? HeaderRows;
        return lastRow < DataStartRow ? DataStartRow : lastRow + 1;
    }

    /// <summary>Автофильтр по строке заголовков + тонкая сетка по данным (без затирания заливки подсветки).</summary
    private static void FinalizePairingWorkbookTables(ExcelPackage excelPackage)
    {
        FinalizePairingSheetTable(excelPackage.Workbook.Worksheets["Форма 1.1"], Layout1115);
        FinalizePairingSheetTable(excelPackage.Workbook.Worksheets["Форма 1.2"], Layout12);
        FinalizePairingSheetTable(excelPackage.Workbook.Worksheets["Форма 1.3"], Layout13);
        FinalizePairingSheetTable(excelPackage.Workbook.Worksheets["Форма 1.4"], Layout14);
        FinalizePairingSheetTable(excelPackage.Workbook.Worksheets["Форма 1.5"], Layout1115);
        FinalizePairingSheetTable(excelPackage.Workbook.Worksheets["Форма 1.6"], Layout16);
    }

    private static void FinalizePairingSheetTable(ExcelWorksheet sheet, SheetLayout layout)
    {
        var lastRow = sheet.Dimension?.End.Row ?? HeaderRows;
        if (lastRow < DataStartRow)
        {
            return;
        }

        sheet.Cells[HeaderRows, 1, HeaderRows, layout.TotalColCount].AutoFilter = true;
        ApplyThinGridBorders(sheet, DataStartRow, lastRow, layout);
    }

    private static void ApplyThinGridBorders(ExcelWorksheet sheet, int firstRow, int lastRow, SheetLayout layout)
    {
        var borderColor = Color.FromArgb(180, 180, 180);
        for (var row = firstRow; row <= lastRow; row++)
        {
            for (var col = 1; col <= layout.TotalColCount; col++)
            {
                if (col == layout.SeparatorCol)
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

    #region Headers / column sizing

    private void SetupPairingFormHeaders(SheetLayout layout, string[] dataHeaders)
    {
        var sheet = Worksheet;

        sheet.Cells[1, 1, 1, layout.SourceColCount].Merge = true;
        sheet.Cells[1, 1].Value = "Непарная операция";
        sheet.Cells[1, 1].Style.Font.Bold = true;
        sheet.Cells[1, 1].Style.Font.Color.SetColor(Color.FromArgb(30, 30, 30));
        sheet.Cells[1, 1, 1, layout.SourceColCount].Style.Fill.SetBackground(SourceSectionFill, ExcelFillStyle.Solid);
        sheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        sheet.Cells[1, layout.SeparatorCol].Value = string.Empty;
        sheet.Column(layout.SeparatorCol).Width = 2.5;
        sheet.Cells[1, layout.SeparatorCol, 2, layout.SeparatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

        sheet.Cells[1, layout.ConfidenceCol].Value = string.Empty;
        sheet.Cells[1, layout.ConfidenceCol].Style.Fill.SetBackground(ClosestSectionFill, ExcelFillStyle.Solid);

        sheet.Cells[1, layout.ClosestStartCol, 1, layout.TotalColCount].Merge = true;
        sheet.Cells[1, layout.ClosestStartCol].Value = "Ближайшее совпадение";
        sheet.Cells[1, layout.ClosestStartCol].Style.Font.Bold = true;
        sheet.Cells[1, layout.ClosestStartCol, 1, layout.TotalColCount].Style.Fill.SetBackground(ClosestSectionFill, ExcelFillStyle.Solid);
        sheet.Cells[1, layout.ClosestStartCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        WriteFieldHeaders(2, 1, dataHeaders);
        WriteFieldHeaders(2, layout.ClosestStartCol, dataHeaders);

        sheet.Cells[2, layout.SeparatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);
        sheet.Cells[2, layout.ConfidenceCol].Value = "Схожесть, %";
        sheet.Cells[2, layout.ConfidenceCol].Style.Font.Bold = true;
        sheet.Cells[2, layout.ConfidenceCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        sheet.Cells[2, layout.ConfidenceCol].Style.WrapText = true;

        ApplyPairingFieldHeaderStyle(sheet, 1, layout.SourceColCount);
        ApplyPairingFieldHeaderStyle(sheet, layout.ConfidenceCol, layout.ConfidenceCol);
        ApplyPairingFieldHeaderStyle(sheet, layout.ClosestStartCol, layout.TotalColCount);

        sheet.Columns[layout.SeparatorCol].Style.Border.Left.Style = ExcelBorderStyle.Medium;
        sheet.Columns[layout.SeparatorCol].Style.Border.Right.Style = ExcelBorderStyle.Medium;
        sheet.Columns[layout.SeparatorCol].Style.Border.Left.Color.SetColor(SeparatorFill);
        sheet.Columns[layout.SeparatorCol].Style.Border.Right.Color.SetColor(SeparatorFill);

        sheet.Row(1).Height = 22;
        sheet.Row(2).Height = ExcelHeaderRowMinHeight;
        ApplyPairingColumnWidths(sheet, layout);
        sheet.View.FreezePanes(DataStartRow, 1);
    }

    /// <summary>Синяя шапка наименований колонок — как в остальных Excel-выгрузках.</summary>
    private static void ApplyPairingFieldHeaderStyle(ExcelWorksheet sheet, int firstCol, int lastCol)
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

    /// <summary>Ширины колонок в «пикселях» Excel (перевод в единицы EPPlus: (px − 5) / 7).</summary>
    private static void ApplyPairingColumnWidths(ExcelWorksheet sheet, SheetLayout layout)
    {
        int[] infoWidthsPx = [70, 90, 210, 60, 110, 110, 50];
        var dataColCount = layout.SourceColCount - InfoColCount;
        var dataWidthPx = dataColCount switch
        {
            <= 12 => 110,
            <= 15 => 100,
            _ => 95
        };

        for (var i = 0; i < infoWidthsPx.Length; i++)
        {
            var width = ExcelWidthFromPixels(infoWidthsPx[i]);
            sheet.Column(1 + i).Width = width;
            sheet.Column(layout.ClosestStartCol + i).Width = width;
        }

        for (var i = InfoColCount; i < layout.SourceColCount; i++)
        {
            var width = ExcelWidthFromPixels(dataWidthPx);
            sheet.Column(1 + i).Width = width;
            sheet.Column(layout.ClosestStartCol + i).Width = width;
        }

        sheet.Column(layout.SeparatorCol).Width = 2.5;
        sheet.Column(layout.ConfidenceCol).Width = ExcelWidthFromPixels(70);
    }

    private static double ExcelWidthFromPixels(int pixels) =>
        Math.Max(1.0, (pixels - 5) / 7.0);

    private void WriteFieldHeaders(int row, int startCol, string[] dataHeaders)
    {
        for (var i = 0; i < InfoHeaders.Length; i++)
        {
            Worksheet.Cells[row, startCol + i].Value = InfoHeaders[i];
        }

        for (var i = 0; i < dataHeaders.Length; i++)
        {
            Worksheet.Cells[row, startCol + InfoColCount + i].Value = dataHeaders[i];
        }
    }

    #endregion

    #region Write blocks — common info

    /// <summary>Пишет общие информационные колонки (Рег.№ … № п/п). Возвращает индекс первой колонки данных.</summary>
    private int WriteInfoBlock(Operation41PairingDto op, int startCol)
    {
        var c = startCol;
        Worksheet.Cells[CurrentRow, c++].Value = op.OrgRegNo;
        Worksheet.Cells[CurrentRow, c++].Value = op.OrgOkpo;
        Worksheet.Cells[CurrentRow, c++].Value = op.OrgShortName;
        Worksheet.Cells[CurrentRow, c++].Value = op.FormNum;
        Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelDate(op.StartPeriod, Worksheet, CurrentRow, c);
        c++;
        Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelDate(op.EndPeriod, Worksheet, CurrentRow, c);
        c++;
        Worksheet.Cells[CurrentRow, c++].Value = op.NumberInOrder;
        return c;
    }

    #endregion

    #region Write blocks — 1.1 / 1.5

    private void WriteForm1115Block(Operation41PairingDto op, int startCol, IReadOnlyDictionary<Pairing11To15Field, Shared.FieldMatchLevel>? fieldLevels)
    {
        var c = WriteInfoBlock(op, startCol);
        void WriteDate(int col, string? value) => Worksheet.Cells[CurrentRow, col].Value = ConvertToExcelDate(value, Worksheet, CurrentRow, col);

        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.OpCode);
        WriteDate(c++, op.OpDate);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PasNum);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.Type);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.Radionuclids);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.FacNum);
        Worksheet.Cells[CurrentRow, c++].Value = op.Quantity is null ? "-" : op.Quantity;
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Activity);
        WriteDate(c++, op.CreationDate);
        Worksheet.Cells[CurrentRow, c++].Value = op.DocumentVid is null ? "-" : op.DocumentVid;
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.DocumentNumber);
        WriteDate(c++, op.DocumentDate);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.ProviderOrRecieverOkpo);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.TransporterOkpo);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackName);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackType);
        Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelString(op.PackNumber);

        if (fieldLevels is null)
        {
            return;
        }

        foreach (var (field, level) in fieldLevels)
        {
            if (GetForm1115FieldOffset(field) is int offset)
            {
                ApplyPairingComparisonCellFill(CurrentRow, startCol + offset, level);
            }
        }
    }

    private static int? GetForm1115FieldOffset(Pairing11To15Field field) =>
        field switch
        {
            Pairing11To15Field.OperationCode => InfoColCount + 0,
            Pairing11To15Field.OperationDate => InfoColCount + 1,
            Pairing11To15Field.PassportNumber => InfoColCount + 2,
            Pairing11To15Field.Type => InfoColCount + 3,
            Pairing11To15Field.Radionuclids => InfoColCount + 4,
            Pairing11To15Field.FactoryNumber => InfoColCount + 5,
            Pairing11To15Field.Quantity => InfoColCount + 6,
            Pairing11To15Field.Activity => InfoColCount + 7,
            Pairing11To15Field.CreationDate => InfoColCount + 8,
            Pairing11To15Field.DocumentVid => InfoColCount + 9,
            Pairing11To15Field.DocumentNumber => InfoColCount + 10,
            Pairing11To15Field.DocumentDate => InfoColCount + 11,
            Pairing11To15Field.ProviderOrRecieverOkpo => InfoColCount + 12,
            Pairing11To15Field.TransporterOkpo => InfoColCount + 13,
            Pairing11To15Field.PackName => InfoColCount + 14,
            Pairing11To15Field.PackType => InfoColCount + 15,
            Pairing11To15Field.PackNumber => InfoColCount + 16,
            _ => null
        };

    private void WriteForm1115Rows(
        List<Operation41PairingDto> unpaired,
        Dictionary<int, ClosestMatchHighlight> closestMatches,
        SheetLayout layout,
        string sheetLabel,
        AnyTaskProgressBarVM? progressBarVM,
        int percentBase,
        int percentSpan)
    {
        Action<int, string>? report = progressBarVM is null ? null : (percent, text) => progressBarVM.SetProgressBar(percent, text);
        var progress = new ProgressReporter(report, percentBase, percentBase + Math.Max(0, percentSpan));
        var total = unpaired.Count;
        var done = 0;
        progress.Report(0, total, $"заполнение листа «{sheetLabel}»: 0 из {total}");

        foreach (var row in OrderForExport(unpaired))
        {
            closestMatches.TryGetValue(row.Id, out var closest);
            WriteForm1115Block(row, 1, closest?.FieldLevels);
            Worksheet.Cells[CurrentRow, layout.SeparatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

            if (closest is not null)
            {
                WriteConfidenceCell(layout.ConfidenceCol, closest.ConfidencePercent);
                WriteForm1115Block(closest.Candidate, layout.ClosestStartCol, closest.FieldLevels);
            }

            CurrentRow++;
            done++;
            progress.Report(done, total, $"заполнение листа «{sheetLabel}»: {done} из {total}");
        }
    }

    #endregion

    #region Write blocks — 1.2

    private void WriteForm12Block(Operation41PairingDto op, int startCol, IReadOnlyDictionary<Pairing12To16Field, Shared.FieldMatchLevel>? fieldLevels)
    {
        var c = WriteInfoBlock(op, startCol);
        void WriteDate(int col, string? value) => Worksheet.Cells[CurrentRow, col].Value = ConvertToExcelDate(value, Worksheet, CurrentRow, col);

        WriteDate(c++, op.OpDate);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Mass);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.BetaGammaActivity);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.AlphaActivity);
        WriteDate(c++, op.ActivityMeasurementDate);
        Worksheet.Cells[CurrentRow, c++].Value = op.DocumentVid is null ? "-" : op.DocumentVid;
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.DocumentNumber);
        WriteDate(c++, op.DocumentDate);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackName);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackType);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackNumber);
        Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelString(op.CodeRao);

        if (fieldLevels is null)
        {
            return;
        }

        foreach (var (field, level) in fieldLevels)
        {
            if (GetForm12FieldOffset(field) is int offset)
            {
                ApplyPairingComparisonCellFill(CurrentRow, startCol + offset, level);
            }
        }
    }

    private static int? GetForm12FieldOffset(Pairing12To16Field field) =>
        field switch
        {
            Pairing12To16Field.OperationDate => InfoColCount + 0,
            Pairing12To16Field.Mass => InfoColCount + 1,
            Pairing12To16Field.BetaGammaActivity => InfoColCount + 2,
            Pairing12To16Field.AlphaActivity => InfoColCount + 3,
            Pairing12To16Field.ActivityMeasurementDate => InfoColCount + 4,
            Pairing12To16Field.DocumentVid => InfoColCount + 5,
            Pairing12To16Field.DocumentNumber => InfoColCount + 6,
            Pairing12To16Field.DocumentDate => InfoColCount + 7,
            Pairing12To16Field.PackName => InfoColCount + 8,
            Pairing12To16Field.PackType => InfoColCount + 9,
            Pairing12To16Field.PackNumber => InfoColCount + 10,
            Pairing12To16Field.CodeRao => InfoColCount + 11,
            _ => null
        };

    private void WriteForm12Rows(
        List<Operation41PairingDto> unpaired,
        AnyTaskProgressBarVM? progressBarVM,
        int percentBase,
        int percentSpan)
    {
        Action<int, string>? report = progressBarVM is null ? null : (percent, text) => progressBarVM.SetProgressBar(percent, text);
        var progress = new ProgressReporter(report, percentBase, percentBase + Math.Max(0, percentSpan));
        var total = unpaired.Count;
        var done = 0;
        progress.Report(0, total, $"заполнение листа «Форма 1.2»: 0 из {total}");

        foreach (var row in OrderForExport(unpaired))
        {
            _form12ClosestMatchHighlights.TryGetValue(row.Id, out var closest);
            WriteForm12Block(row, 1, closest?.FieldLevels);
            Worksheet.Cells[CurrentRow, Layout12.SeparatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

            if (closest is not null)
            {
                WriteConfidenceCell(Layout12.ConfidenceCol, closest.ConfidencePercent);
                WriteForm12Block(closest.Candidate, Layout12.ClosestStartCol, closest.FieldLevels);
            }

            CurrentRow++;
            done++;
            progress.Report(done, total, $"заполнение листа «Форма 1.2»: {done} из {total}");
        }
    }

    #endregion

    #region Write blocks — 1.3

    private static Shared.FieldMatchLevel? ToAggregateStateHighlightLevel(bool? matchesCodeRao) =>
        matchesCodeRao switch
        {
            true => Shared.FieldMatchLevel.Exact,
            false => Shared.FieldMatchLevel.Mismatch,
            null => null
        };

    private void WriteForm13Block(
        Operation41PairingDto op,
        int startCol,
        IReadOnlyDictionary<Pairing13To16Field, Shared.FieldMatchLevel>? fieldLevels,
        Shared.FieldMatchLevel? aggregateStateLevel)
    {
        var c = WriteInfoBlock(op, startCol);
        void WriteDate(int col, string? value) => Worksheet.Cells[CurrentRow, col].Value = ConvertToExcelDate(value, Worksheet, CurrentRow, col);

        WriteDate(c++, op.OpDate);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.MainRadionuclids);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.TritiumActivity);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.BetaGammaActivity);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.AlphaActivity);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.TransuraniumActivity);
        WriteDate(c++, op.ActivityMeasurementDate);
        var aggregateStateCol = c;
        Worksheet.Cells[CurrentRow, c++].Value = op.AggregateState is null ? "-" : op.AggregateState;
        Worksheet.Cells[CurrentRow, c++].Value = op.DocumentVid is null ? "-" : op.DocumentVid;
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.DocumentNumber);
        WriteDate(c++, op.DocumentDate);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackName);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackType);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackNumber);
        Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelString(op.CodeRao);

        if (aggregateStateLevel is Shared.FieldMatchLevel aggLevel)
        {
            ApplyPairingComparisonCellFill(CurrentRow, aggregateStateCol, aggLevel);
        }

        if (fieldLevels is null)
        {
            return;
        }

        foreach (var (field, level) in fieldLevels)
        {
            if (GetForm13FieldOffset(field) is int offset)
            {
                ApplyPairingComparisonCellFill(CurrentRow, startCol + offset, level);
            }
        }
    }

    private static int? GetForm13FieldOffset(Pairing13To16Field field) =>
        field switch
        {
            Pairing13To16Field.OperationDate => InfoColCount + 0,
            Pairing13To16Field.MainRadionuclids => InfoColCount + 1,
            Pairing13To16Field.TritiumActivity => InfoColCount + 2,
            Pairing13To16Field.BetaGammaActivity => InfoColCount + 3,
            Pairing13To16Field.AlphaActivity => InfoColCount + 4,
            Pairing13To16Field.TransuraniumActivity => InfoColCount + 5,
            Pairing13To16Field.ActivityMeasurementDate => InfoColCount + 6,
            Pairing13To16Field.DocumentVid => InfoColCount + 8,
            Pairing13To16Field.DocumentNumber => InfoColCount + 9,
            Pairing13To16Field.DocumentDate => InfoColCount + 10,
            Pairing13To16Field.PackName => InfoColCount + 11,
            Pairing13To16Field.PackType => InfoColCount + 12,
            Pairing13To16Field.PackNumber => InfoColCount + 13,
            Pairing13To16Field.CodeRao => InfoColCount + 14,
            _ => null
        };

    private void WriteForm13Rows(
        List<Operation41PairingDto> unpaired,
        AnyTaskProgressBarVM? progressBarVM,
        int percentBase,
        int percentSpan)
    {
        Action<int, string>? report = progressBarVM is null ? null : (percent, text) => progressBarVM.SetProgressBar(percent, text);
        var progress = new ProgressReporter(report, percentBase, percentBase + Math.Max(0, percentSpan));
        var total = unpaired.Count;
        var done = 0;
        progress.Report(0, total, $"заполнение листа «Форма 1.3»: 0 из {total}");

        foreach (var row in OrderForExport(unpaired))
        {
            _form13ClosestMatchHighlights.TryGetValue(row.Id, out var closest);
            WriteForm13Block(row, 1, closest?.FieldLevels, ToAggregateStateHighlightLevel(closest?.AggregateStateMatchesCodeRao));
            Worksheet.Cells[CurrentRow, Layout13.SeparatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

            if (closest is not null)
            {
                WriteConfidenceCell(Layout13.ConfidenceCol, closest.ConfidencePercent);
                WriteForm13Block(closest.Candidate, Layout13.ClosestStartCol, closest.FieldLevels,
                    ToAggregateStateHighlightLevel(closest.AggregateStateMatchesCodeRao));
            }

            CurrentRow++;
            done++;
            progress.Report(done, total, $"заполнение листа «Форма 1.3»: {done} из {total}");
        }
    }

    #endregion

    #region Write blocks — 1.4

    private void WriteForm14Block(
        Operation41PairingDto op,
        int startCol,
        IReadOnlyDictionary<Pairing14To16Field, Shared.FieldMatchLevel>? fieldLevels,
        Shared.FieldMatchLevel? aggregateStateLevel)
    {
        var c = WriteInfoBlock(op, startCol);
        void WriteDate(int col, string? value) => Worksheet.Cells[CurrentRow, col].Value = ConvertToExcelDate(value, Worksheet, CurrentRow, col);

        WriteDate(c++, op.OpDate);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Volume);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Mass);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.MainRadionuclids);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.TritiumActivity);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.BetaGammaActivity);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.AlphaActivity);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.TransuraniumActivity);
        WriteDate(c++, op.ActivityMeasurementDate);
        var aggregateStateCol = c;
        Worksheet.Cells[CurrentRow, c++].Value = op.AggregateState is null ? "-" : op.AggregateState;
        Worksheet.Cells[CurrentRow, c++].Value = op.DocumentVid is null ? "-" : op.DocumentVid;
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.DocumentNumber);
        WriteDate(c++, op.DocumentDate);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackName);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackType);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackNumber);
        Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelString(op.CodeRao);

        if (aggregateStateLevel is Shared.FieldMatchLevel aggLevel)
        {
            ApplyPairingComparisonCellFill(CurrentRow, aggregateStateCol, aggLevel);
        }

        if (fieldLevels is null)
        {
            return;
        }

        foreach (var (field, level) in fieldLevels)
        {
            if (GetForm14FieldOffset(field) is int offset)
            {
                ApplyPairingComparisonCellFill(CurrentRow, startCol + offset, level);
            }
        }
    }

    private static int? GetForm14FieldOffset(Pairing14To16Field field) =>
        field switch
        {
            Pairing14To16Field.OperationDate => InfoColCount + 0,
            Pairing14To16Field.Volume => InfoColCount + 1,
            Pairing14To16Field.Mass => InfoColCount + 2,
            Pairing14To16Field.MainRadionuclids => InfoColCount + 3,
            Pairing14To16Field.TritiumActivity => InfoColCount + 4,
            Pairing14To16Field.BetaGammaActivity => InfoColCount + 5,
            Pairing14To16Field.AlphaActivity => InfoColCount + 6,
            Pairing14To16Field.TransuraniumActivity => InfoColCount + 7,
            Pairing14To16Field.ActivityMeasurementDate => InfoColCount + 8,
            Pairing14To16Field.DocumentVid => InfoColCount + 10,
            Pairing14To16Field.DocumentNumber => InfoColCount + 11,
            Pairing14To16Field.DocumentDate => InfoColCount + 12,
            Pairing14To16Field.PackName => InfoColCount + 13,
            Pairing14To16Field.PackType => InfoColCount + 14,
            Pairing14To16Field.PackNumber => InfoColCount + 15,
            Pairing14To16Field.CodeRao => InfoColCount + 16,
            _ => null
        };

    private void WriteForm14Rows(
        List<Operation41PairingDto> unpaired,
        AnyTaskProgressBarVM? progressBarVM,
        int percentBase,
        int percentSpan)
    {
        Action<int, string>? report = progressBarVM is null ? null : (percent, text) => progressBarVM.SetProgressBar(percent, text);
        var progress = new ProgressReporter(report, percentBase, percentBase + Math.Max(0, percentSpan));
        var total = unpaired.Count;
        var done = 0;
        progress.Report(0, total, $"заполнение листа «Форма 1.4»: 0 из {total}");

        foreach (var row in OrderForExport(unpaired))
        {
            _form14ClosestMatchHighlights.TryGetValue(row.Id, out var closest);
            WriteForm14Block(row, 1, closest?.FieldLevels, ToAggregateStateHighlightLevel(closest?.AggregateStateMatchesCodeRao));
            Worksheet.Cells[CurrentRow, Layout14.SeparatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

            if (closest is not null)
            {
                WriteConfidenceCell(Layout14.ConfidenceCol, closest.ConfidencePercent);
                WriteForm14Block(closest.Candidate, Layout14.ClosestStartCol, closest.FieldLevels,
                    ToAggregateStateHighlightLevel(closest.AggregateStateMatchesCodeRao));
            }

            CurrentRow++;
            done++;
            progress.Report(done, total, $"заполнение листа «Форма 1.4»: {done} из {total}");
        }
    }

    #endregion

    #region Write blocks — 1.6

    private void WriteForm16Block(Operation41PairingDto op, int startCol, Form16ClosestMatchHighlight? highlight)
    {
        var c = WriteInfoBlock(op, startCol);
        void WriteDate(int col, string? value) => Worksheet.Cells[CurrentRow, col].Value = ConvertToExcelDate(value, Worksheet, CurrentRow, col);

        WriteDate(c++, op.OpDate);
        var codeRaoCol = c;
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.CodeRao);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Volume);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.Mass);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.MainRadionuclids);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.TritiumActivity);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.BetaGammaActivity);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.AlphaActivity);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelDouble(op.TransuraniumActivity);
        WriteDate(c++, op.ActivityMeasurementDate);
        Worksheet.Cells[CurrentRow, c++].Value = op.DocumentVid is null ? "-" : op.DocumentVid;
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.DocumentNumber);
        WriteDate(c++, op.DocumentDate);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackName);
        Worksheet.Cells[CurrentRow, c++].Value = ConvertToExcelString(op.PackType);
        Worksheet.Cells[CurrentRow, c].Value = ConvertToExcelString(op.PackNumber);

        if (highlight is null)
        {
            return;
        }

        switch (highlight.Profile)
        {
            case Form16MatchProfile.Form12 when highlight.Levels12 is not null:
                foreach (var (field, level) in highlight.Levels12)
                {
                    if (GetForm16Form12FieldOffset(field) is int offset)
                    {
                        ApplyPairingComparisonCellFill(CurrentRow, startCol + offset, level);
                    }
                }

                break;

            case Form16MatchProfile.Form13 when highlight.Levels13 is not null:
                foreach (var (field, level) in highlight.Levels13)
                {
                    if (GetForm16Form13FieldOffset(field) is int offset)
                    {
                        ApplyPairingComparisonCellFill(CurrentRow, startCol + offset, level);
                    }
                }

                break;

            case Form16MatchProfile.Form14 when highlight.Levels14 is not null:
                foreach (var (field, level) in highlight.Levels14)
                {
                    if (GetForm16Form14FieldOffset(field) is int offset)
                    {
                        ApplyPairingComparisonCellFill(CurrentRow, startCol + offset, level);
                    }
                }

                break;
        }

        // Для профилей 1.3/1.4 совпадение агрегатного состояния важнее формального совпадения кода РАО.
        if (ToAggregateStateHighlightLevel(highlight.AggregateStateMatchesCodeRao) is { } codeRaoLevel)
        {
            ApplyPairingComparisonCellFill(CurrentRow, codeRaoCol, codeRaoLevel);
        }
    }

    private static int? GetForm16Form12FieldOffset(Pairing12To16Field field) =>
        field switch
        {
            Pairing12To16Field.OperationDate => InfoColCount + 0,
            Pairing12To16Field.CodeRao => InfoColCount + 1,
            Pairing12To16Field.Mass => InfoColCount + 3,
            Pairing12To16Field.BetaGammaActivity => InfoColCount + 6,
            Pairing12To16Field.AlphaActivity => InfoColCount + 7,
            Pairing12To16Field.ActivityMeasurementDate => InfoColCount + 9,
            Pairing12To16Field.DocumentVid => InfoColCount + 10,
            Pairing12To16Field.DocumentNumber => InfoColCount + 11,
            Pairing12To16Field.DocumentDate => InfoColCount + 12,
            Pairing12To16Field.PackName => InfoColCount + 13,
            Pairing12To16Field.PackType => InfoColCount + 14,
            Pairing12To16Field.PackNumber => InfoColCount + 15,
            _ => null
        };

    private static int? GetForm16Form13FieldOffset(Pairing13To16Field field) =>
        field switch
        {
            Pairing13To16Field.OperationDate => InfoColCount + 0,
            Pairing13To16Field.CodeRao => InfoColCount + 1,
            Pairing13To16Field.MainRadionuclids => InfoColCount + 4,
            Pairing13To16Field.TritiumActivity => InfoColCount + 5,
            Pairing13To16Field.BetaGammaActivity => InfoColCount + 6,
            Pairing13To16Field.AlphaActivity => InfoColCount + 7,
            Pairing13To16Field.TransuraniumActivity => InfoColCount + 8,
            Pairing13To16Field.ActivityMeasurementDate => InfoColCount + 9,
            Pairing13To16Field.DocumentVid => InfoColCount + 10,
            Pairing13To16Field.DocumentNumber => InfoColCount + 11,
            Pairing13To16Field.DocumentDate => InfoColCount + 12,
            Pairing13To16Field.PackName => InfoColCount + 13,
            Pairing13To16Field.PackType => InfoColCount + 14,
            Pairing13To16Field.PackNumber => InfoColCount + 15,
            _ => null
        };

    private static int? GetForm16Form14FieldOffset(Pairing14To16Field field) =>
        field switch
        {
            Pairing14To16Field.OperationDate => InfoColCount + 0,
            Pairing14To16Field.CodeRao => InfoColCount + 1,
            Pairing14To16Field.Volume => InfoColCount + 2,
            Pairing14To16Field.Mass => InfoColCount + 3,
            Pairing14To16Field.MainRadionuclids => InfoColCount + 4,
            Pairing14To16Field.TritiumActivity => InfoColCount + 5,
            Pairing14To16Field.BetaGammaActivity => InfoColCount + 6,
            Pairing14To16Field.AlphaActivity => InfoColCount + 7,
            Pairing14To16Field.TransuraniumActivity => InfoColCount + 8,
            Pairing14To16Field.ActivityMeasurementDate => InfoColCount + 9,
            Pairing14To16Field.DocumentVid => InfoColCount + 10,
            Pairing14To16Field.DocumentNumber => InfoColCount + 11,
            Pairing14To16Field.DocumentDate => InfoColCount + 12,
            Pairing14To16Field.PackName => InfoColCount + 13,
            Pairing14To16Field.PackType => InfoColCount + 14,
            Pairing14To16Field.PackNumber => InfoColCount + 15,
            _ => null
        };

    private void WriteForm16Rows(
        List<Operation41PairingDto> unpaired,
        AnyTaskProgressBarVM? progressBarVM,
        int percentBase,
        int percentSpan)
    {
        Action<int, string>? report = progressBarVM is null ? null : (percent, text) => progressBarVM.SetProgressBar(percent, text);
        var progress = new ProgressReporter(report, percentBase, percentBase + Math.Max(0, percentSpan));
        var total = unpaired.Count;
        var done = 0;
        progress.Report(0, total, $"заполнение листа «Форма 1.6»: 0 из {total}");

        foreach (var row in OrderForExport(unpaired))
        {
            _form16ClosestMatchHighlights.TryGetValue(row.Id, out var highlight);
            WriteForm16Block(row, 1, highlight);
            Worksheet.Cells[CurrentRow, Layout16.SeparatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

            if (highlight is not null)
            {
                WriteConfidenceCell(Layout16.ConfidenceCol, highlight.ConfidencePercent);
                WriteForm16Block(highlight.Candidate, Layout16.ClosestStartCol, highlight);
            }

            CurrentRow++;
            done++;
            progress.Report(done, total, $"заполнение листа «Форма 1.6»: {done} из {total}");
        }
    }

    #endregion

    #region Common helpers

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

    private static readonly CustomReportsComparer OrgRegNoComparer = new();

    /// <summary>
    /// Порядок строк в Excel: рег.№ (как в списке организаций), начало/конец периода, № п/п, Id.
    /// Непарсящиеся даты периода уходят в конец (DateOnly.MaxValue).
    /// </summary>
    private static IOrderedEnumerable<Operation41PairingDto> OrderForExport(List<Operation41PairingDto> unpaired) =>
        unpaired
            .OrderBy(op => op.OrgRegNo, OrgRegNoComparer)
            .ThenBy(op => DateOnly.TryParse(op.StartPeriod, out var start) ? start : DateOnly.MaxValue)
            .ThenBy(op => DateOnly.TryParse(op.EndPeriod, out var end) ? end : DateOnly.MaxValue)
            .ThenBy(op => op.NumberInOrder)
            .ThenBy(op => op.Id);

    private static void ApplyPairingComparisonCellFill(ExcelWorksheet worksheet, int row, int column, Shared.FieldMatchLevel level) =>
        worksheet.Cells[row, column].Style.Fill.SetBackground(
            FillForMatchLevel(level),
            ExcelFillStyle.Solid);

    private void ApplyPairingComparisonCellFill(int row, int column, Shared.FieldMatchLevel level) =>
        ApplyPairingComparisonCellFill(Worksheet, row, column, level);

    private static void ApplyPairingComparisonCellFill(ExcelWorksheet worksheet, int row, int column, bool matches) =>
        ApplyPairingComparisonCellFill(worksheet, row, column,
            matches ? Shared.FieldMatchLevel.Exact : Shared.FieldMatchLevel.Mismatch);

    private void ApplyPairingComparisonCellFill(int row, int column, bool matches) =>
        ApplyPairingComparisonCellFill(Worksheet, row, column, matches);

    private static Color FillForMatchLevel(Shared.FieldMatchLevel level) =>
        level switch
        {
            Shared.FieldMatchLevel.Exact => PairingFieldMatchFill,
            Shared.FieldMatchLevel.Near => PairingFieldNearFill,
            _ => PairingFieldMismatchFill
        };

    #endregion
}
