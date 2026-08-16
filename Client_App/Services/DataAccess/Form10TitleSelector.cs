namespace Client_App.Services.DataAccess;

/// <summary>
/// Выбор отображаемых полей титула 1.0/2.0 из двух строк Rows10/Rows20
/// (та же логика, что <c>Report.RegNoRep</c> / <c>OkpoRep</c> / <c>ShortJurLicoRep</c>).
/// </summary>
public static class Form10TitleSelector
{
    public readonly record struct TitleFields(string RegNo, string Okpo, string ShortJurLico);

    /// <summary>
    /// rows упорядочены по NumberInOrder: [0] юрлицо, [1] обособленное.
    /// Если вторая строка заполнена (ОКПО не пустой и не «-» для ОКПО/краткого) —
    /// для рег.№ берётся вторая при выполнении условий RegNoRep.
    /// </summary>
    public static TitleFields Pick(
        string? regNo0, string? okpo0, string? short0,
        string? regNo1, string? okpo1, string? short1)
    {
        var r0 = regNo0 ?? "";
        var o0 = okpo0 ?? "";
        var s0 = short0 ?? "";
        var r1 = regNo1 ?? "";
        var o1 = okpo1 ?? "";
        var s1 = short1 ?? "";

        // RegNoRep: вторая строка, если (RegNo1 не пуст ИЛИ Okpo1 == "-") И Okpo1 не пуст
        var useRow1RegNo = (!string.IsNullOrEmpty(r1) || o1 == "-") && !string.IsNullOrEmpty(o1);

        // OkpoRep / ShortJurLicoRep: вторая строка, только если Okpo1 не "" и не "-"
        var useRow1OkpoAndShort = o1 is not ("" or "-");

        return new TitleFields(
            RegNo: useRow1RegNo ? r1 : r0,
            Okpo: useRow1OkpoAndShort ? o1 : o0,
            ShortJurLico: useRow1OkpoAndShort && !string.IsNullOrWhiteSpace(s1)
                ? s1
                : !string.IsNullOrWhiteSpace(s0) ? s0 : s1);
    }

    public static TitleFields PickFromOrderedRows(
        (string? RegNo, string? Okpo, string? Short) row0,
        (string? RegNo, string? Okpo, string? Short) row1) =>
        Pick(row0.RegNo, row0.Okpo, row0.Short, row1.RegNo, row1.Okpo, row1.Short);
}
