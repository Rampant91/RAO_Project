namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Эталон заголовков аналитической выгрузки формы 1.1
/// (зеркало <c>ExcelExportBaseAllAsyncCommand.FillHeaders("1.1")</c> / NotesHeaders1).
/// </summary>
public static class FormsExcelExportHeadersForm11
{
    public const string ReportsSheetName = "Отчеты 1.1";
    public const string NotesSheetName = "Примечания 1.1";
    public const string FormNum = "1.1";

    public static readonly string[] ReportColumns =
    [
        "ID",
        "ОКПО",
        "Сокращенное наименование",
        "Рег.№",
        "Номер корректировки",
        "Дата начала периода",
        "Дата конца периода",
        "№ п/п",
        "код",
        "дата",
        "номер паспорта (сертификата)",
        "тип",
        "радионуклиды",
        "номер",
        "количество, шт",
        "суммарная активность, Бк",
        "код ОКПО изготовителя",
        "дата выпуска",
        "категория",
        "НСС, мес",
        "код формы собственности",
        "код ОКПО правообладателя",
        "вид",
        "номер",
        "дата",
        "поставщика или получателя",
        "перевозчика",
        "наименование",
        "тип",
        "номер"
    ];

    /// <summary>
    /// Заголовки листа примечаний. «Рег. №» — с пробелом (как в выгрузке).
    /// </summary>
    public static readonly string[] NotesColumns =
    [
        "ОКПО",
        "Сокращенное наименование",
        "Рег. №",
        "Номер корректировки",
        "Дата начала периода",
        "Дата конца периода",
        "№ строки",
        "№ графы",
        "Пояснение"
    ];
}
