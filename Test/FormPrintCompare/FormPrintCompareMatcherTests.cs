using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;
using Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare.Testing;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Xunit;

namespace Test.FormPrintCompare;

/// <summary>
/// Сценарии сопоставления строк: identical, reorder, add/delete, typos, counts.
/// </summary>
public class FormPrintCompareMatcherTests
{
    private static CompareReportDto Left(params CompareRowDto[] rows) =>
        FormPrintCompareTestAccess.CreateReport11("01.01.2024", "31.12.2024", 0, rows);

    private static CompareReportDto Right(params CompareRowDto[] rows) =>
        FormPrintCompareTestAccess.CreateReport11("01.01.2024", "31.12.2024", 1, rows);

    private static CompareRowDto R(
        int id,
        int index,
        int npp,
        string passport,
        string factory = "F1",
        string activity = "1.0e+3",
        string type = "ИИИ") =>
        FormPrintCompareTestAccess.Row11(
            id, index, npp, "11", "15.03.2024", passport, type, factory, activity);

    [Fact]
    public void Identical_Reports_AreIdentical_AndDisplayEmpty()
    {
        var left = Left(R(1, 0, 1, "PAS1"), R(2, 1, 2, "PAS2"));
        var right = Right(R(10, 0, 1, "PAS1"), R(20, 1, 2, "PAS2"));

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.True(result.IsIdentical);
        Assert.Empty(result.DisplayLines);
        Assert.Equal(2, result.UnchangedCount);
        Assert.Equal(100, result.Lines[0].ConfidencePercent);
        Assert.Contains("полностью совпадает", result.Message);
    }

    [Fact]
    public void LookalikePassport_MatchesAsSameRow()
    {
        var left = Left(R(1, 0, 1, "СОС-01"));
        var right = Right(R(2, 0, 1, "COC-01"));

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.True(result.IsIdentical);
    }

    [Fact]
    public void ReorderOnly_IsMoved_NotChanged()
    {
        var left = Left(
            R(1, 0, 1, "PAS1"),
            R(2, 1, 2, "PAS2"));
        var right = Right(
            R(20, 0, 1, "PAS2"),
            R(10, 1, 2, "PAS1"));

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.True(result.OrderOnlyChanged);
        Assert.False(result.IsIdentical);
        Assert.Equal(0, result.ChangedCount);
        Assert.Equal(2, result.MovedCount);
        Assert.All(result.DisplayLines, l => Assert.Equal(DiffRowStatus.Moved, l.Status));
        Assert.Contains("№ п/п: 1 → 2", result.DisplayLines.Single(l => l.Left!.Id == 1).StatusText);
        Assert.Contains("отличается только № п/п", result.Message);
    }

    [Fact]
    public void DuplicateNppOnRight_IsFlagged_AndMessageExplainsNumberingBreak()
    {
        // Исходник: 50, 51. Сравнение: обе строки с №50 (сбой нумерации), содержимое то же.
        var left = Left(
            R(1, 0, 50, "PAS_A"),
            R(2, 1, 51, "PAS_B"));
        var right = Right(
            R(10, 0, 50, "PAS_A"),
            R(20, 1, 50, "PAS_B"));

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.False(result.IsIdentical);
        Assert.Equal(new[] { 50 }, result.RightDuplicateNpps.ToArray());
        Assert.Empty(result.LeftDuplicateNpps);
        Assert.Contains("Содержимое строк совпало", result.Message);
        Assert.Contains("сбита нумерация", result.Message);
        Assert.Contains("50", result.Message);

        var moved = result.DisplayLines.Single(l => l.Left!.Id == 2);
        Assert.Equal(DiffRowStatus.Moved, moved.Status);
        Assert.True(moved.RightNppDuplicate);
        Assert.Equal("№ п/п: 51 → 50", moved.StatusText);

        var kept = result.DisplayLines.Single(l => l.Left!.Id == 1);
        Assert.Equal(DiffRowStatus.Unchanged, kept.Status);
        Assert.Equal("Без изменений", kept.StatusText);
    }

    [Fact]
    public void SameNppDifferentSourceIndex_IsUnchanged_NotMoved()
    {
        // Содержимое совпало, № п/п тот же, но индекс в списке другой (сдвиг из‑за чужих строк).
        var left = Left(R(1, 5, 50, "PAS1"));
        var right = Right(R(2, 3, 50, "PAS1"));

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.True(result.IsIdentical);
        Assert.Equal(0, result.MovedCount);
        Assert.Equal(1, result.UnchangedCount);
        Assert.Empty(result.DisplayLines); // identical → листа нет
    }

    [Fact]
    public void DeletedAndAdded_AreClassified()
    {
        var left = Left(
            R(1, 0, 1, "KEEP"),
            R(2, 1, 2, "GONE"));
        var right = Right(
            R(10, 0, 1, "KEEP"),
            R(30, 1, 2, "NEW"));

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.Equal(1, result.DeletedCount);
        Assert.Equal(1, result.AddedCount);
        Assert.Equal(1, result.UnchangedCount);
        Assert.Contains(result.DisplayLines, l => l.Status == DiffRowStatus.Deleted && l.Left!.Values[3] == "GONE");
        Assert.Contains(result.DisplayLines, l => l.Status == DiffRowStatus.Added && l.Right!.Values[3] == "NEW");
        Assert.Contains(result.DisplayLines, l => l.Status == DiffRowStatus.Unchanged);
        Assert.Equal(100, result.DisplayLines.Single(l => l.Status == DiffRowStatus.Unchanged).ConfidencePercent);
    }

    [Fact]
    public void ActivityChange_IsChanged_WithMismatchOrNearOnActivity()
    {
        var left = Left(R(1, 0, 1, "PAS1", activity: "1.0e+3"));
        var right = Right(R(2, 0, 1, "PAS1", activity: "2.0e+3"));

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.Equal(1, result.ChangedCount);
        var line = Assert.Single(result.DisplayLines);
        Assert.Equal(DiffRowStatus.Changed, line.Status);
        Assert.InRange(line.ConfidencePercent, 0, 99);
        Assert.NotNull(line.FieldLevels);
        Assert.Equal(
            FieldMatchLevel.Mismatch,
            line.FieldLevels![FormPrintCompareTestAccess.ActivityColumnIndex]);
    }

    [Fact]
    public void SoftPairs_WhenPassportExact_DespiteTypeTypo()
    {
        var left = Left(R(1, 0, 1, "PAS1", type: "ИИИ-ABC"));
        var right = Right(R(2, 0, 1, "PAS1", type: "ИИИ-ABX")); // typo in type, same passport

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.Equal(0, result.DeletedCount);
        Assert.Equal(0, result.AddedCount);
        Assert.True(result.ChangedCount + result.UnchangedCount + result.MovedCount >= 1);
    }

    [Fact]
    public void MoreRowsOnRight_ExtraAreAdded()
    {
        var left = Left(R(1, 0, 1, "A"));
        var right = Right(R(10, 0, 1, "A"), R(20, 1, 2, "B"), R(30, 2, 3, "C"));

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.Equal(2, result.AddedCount);
        Assert.Equal(0, result.DeletedCount);
        Assert.Equal(2, result.DisplayLines.Count(l => l.Status == DiffRowStatus.Added));
    }

    [Fact]
    public void MoreRowsOnLeft_ExtraAreDeleted()
    {
        var left = Left(R(1, 0, 1, "A"), R(2, 1, 2, "B"), R(3, 2, 3, "C"));
        var right = Right(R(10, 0, 1, "A"));

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.Equal(2, result.DeletedCount);
        Assert.Equal(0, result.AddedCount);
    }

    [Fact]
    public void Mixed_AddDeleteReorderChange()
    {
        var left = Left(
            R(1, 0, 1, "STABLE"),
            R(2, 1, 2, "MOVE_ME"),
            R(3, 2, 3, "DELETE_ME"),
            R(4, 3, 4, "CHANGE_ME", activity: "1.0e+3"));
        var right = Right(
            R(10, 0, 1, "MOVE_ME"),
            R(20, 1, 2, "STABLE"),
            R(30, 2, 3, "CHANGE_ME", activity: "3.0e+3"),
            R(40, 3, 4, "BRAND_NEW"));

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.Equal(1, result.DeletedCount);
        Assert.Equal(1, result.AddedCount);
        Assert.True(result.MovedCount >= 1);
        Assert.Equal(1, result.ChangedCount);
        Assert.Contains(result.DisplayLines, l => l.Status == DiffRowStatus.Deleted && l.Left!.Values[3] == "DELETE_ME");
        Assert.Contains(result.DisplayLines, l => l.Status == DiffRowStatus.Added && l.Right!.Values[3] == "BRAND_NEW");
        Assert.Contains(result.DisplayLines, l =>
            l.Status == DiffRowStatus.Changed && l.Left!.Values[3] == "CHANGE_ME");
    }

    [Fact]
    public void MissingRightReport_MessageAndEmptyLines()
    {
        var left = Left(R(1, 0, 1, "PAS1"));
        var result = FormPrintCompareTestAccess.Compare(left, null);

        Assert.False(result.HasRight);
        Assert.Empty(result.DisplayLines);
        Assert.Contains("отсутствует отчёт для сверки", result.Message);
    }

    [Fact]
    public void EmptyBothSides_Identical()
    {
        var left = Left();
        var right = Right();
        var result = FormPrintCompareTestAccess.Compare(left, right);
        Assert.True(result.IsIdentical);
    }

    [Fact]
    public void SoftDoesNotPair_WhenNoFingerprintAnchor()
    {
        // Completely different keys → delete + add, not soft-changed.
        var left = Left(R(1, 0, 1, "AAAA", factory: "F-A"));
        var right = Right(R(2, 0, 1, "BBBB", factory: "F-B"));

        var result = FormPrintCompareTestAccess.Compare(left, right);

        Assert.Equal(1, result.DeletedCount);
        Assert.Equal(1, result.AddedCount);
        Assert.Equal(0, result.ChangedCount);
    }
}
