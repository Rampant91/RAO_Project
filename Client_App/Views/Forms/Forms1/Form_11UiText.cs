namespace Client_App.Views.Forms.Forms1;

/// <summary>
/// Тексты UI формы 1.1 (только ASCII + \u — устойчиво к кодировке файла).
/// </summary>
internal static class Form_11UiText
{
    public const string Yes = "\u0414\u0430";
    public const string No = "\u041d\u0435\u0442";
    public const string Cancel = "\u041e\u0442\u043c\u0435\u043d\u0430";

    public const string SaveChangesTitle = "\u0421\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u0438\u0435 \u0438\u0437\u043c\u0435\u043d\u0435\u043d\u0438\u0439";
    public const string Notification = "\u0423\u0432\u0435\u0434\u043e\u043c\u043b\u0435\u043d\u0438\u0435";

    public static string SaveFormQuestion(string formType) =>
        $"\u0421\u043e\u0445\u0440\u0430\u043d\u0438\u0442\u044c \u0444\u043e\u0440\u043c\u0443 {formType}?";

    public const string IntersectionTitle = "\u041f\u0435\u0440\u0435\u0441\u0435\u0447\u0435\u043d\u0438\u0435";

    public static string IntersectionMessage(
        string regNo, string okpo, string formNum, string start, string end, string repStart, string repEnd) =>
        $"\u0423 \u043e\u0440\u0433\u0430\u043d\u0438\u0437\u0430\u0446\u0438\u0438 {regNo}_{okpo} " +
        $"{System.Environment.NewLine}\u043f\u0440\u0438\u0441\u0443\u0442\u0441\u0442\u0432\u0443\u0435\u0442 \u043e\u0442\u0447\u0451\u0442 \u043f\u043e \u0444\u043e\u0440\u043c\u0435 " +
        $"{formNum} {start}-{end}" +
        $"{System.Environment.NewLine}\u043f\u0435\u0440\u0435\u0441\u0435\u043a\u0430\u044e\u0449\u0438\u0439\u0441\u044f \u0441 \u0432\u0432\u0435\u0434\u0451\u043d\u043d\u044b\u043c \u043f\u0435\u0440\u0438\u043e\u0434\u043e\u043c " +
        $"{repStart}-{repEnd}.";

    public static string EmptyRowsMessage(string formType) =>
        $"\u0412 \u0444\u043e\u0440\u043c\u0435 {formType} \u043f\u0440\u0438\u0441\u0443\u0442\u0441\u0442\u0432\u0443\u044e\u0442 \u043f\u0443\u0441\u0442\u044b\u0435 \u0441\u0442\u0440\u043e\u0447\u043a\u0438." +
        $"{System.Environment.NewLine}\u0412\u044b \u0445\u043e\u0442\u0438\u0442\u0435 \u0438\u0445 \u0443\u0434\u0430\u043b\u0438\u0442\u044c?";
}
