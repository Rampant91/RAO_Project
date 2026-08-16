using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.Testing;
using Client_App.Resources;
using OfficeOpenXml;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

namespace Test.Pairing41;

/// <summary>
/// Регрессии whole-DB Excel (дописывание строк) и org-границы / dual-block closest.
/// </summary>
public sealed class Pairing41WorkbookAppendTests
{
    [Fact]
    public void GetNextDataRow_AfterHeaders_ReturnsDataStartRow()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Форма 1.1");
        sheet.Cells[1, 1].Value = "header";
        sheet.Cells[2, 1].Value = "fields";

        var next = Pairing41TestAccess.GetNextDataRowForTests(sheet);
        Assert.Equal(3, next);
    }

    [Fact]
    public void GetNextDataRow_AfterData_ContinuesAfterLastRow()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Форма 1.1");
        sheet.Cells[1, 1].Value = "h1";
        sheet.Cells[2, 1].Value = "h2";
        sheet.Cells[3, 1].Value = "org-A";
        sheet.Cells[4, 1].Value = "org-A-2";

        var next = Pairing41TestAccess.GetNextDataRowForTests(sheet);
        Assert.Equal(5, next);
    }

    /// <summary>
    /// Регрессия: closest для непарной 1.1 берётся только из reference той же org (RepsId / OrgRegNo).
    /// </summary>
    [Fact]
    public void Closest11_Candidate_StaysWithinSameOrganization()
    {
        var form11 = new List<Pairing41Row>
        {
            MakeRow11(1, repsId: 94015, orgRegNo: "94015", documentNumber: "DOC-A")
        };

        var form15 = new List<Pairing41Row>
        {
            MakeRow11(101, repsId: 94015, orgRegNo: "94015", documentNumber: "DOC-OTHER"),
            MakeRow11(202, repsId: 93024, orgRegNo: "93024", documentNumber: "DOC-A")
        };

        var sameOrgOnly = form15.FindAll(r => r.RepsId == 94015);
        var keys = Pairing41TestAccess.GetClosest11CandidateOrgKeys(form11, sameOrgOnly);

        Assert.True(keys.ContainsKey(1));
        Assert.Equal(94015, keys[1].CandidateRepsId);
        Assert.Equal("94015", keys[1].CandidateOrgRegNo);
    }

    /// <summary>
    /// Регрессия whole-DB: org B дописывается ниже org A — правый блок A не затирается.
    /// </summary>
    [Fact]
    public void AppendSimulation_SecondOrgDoesNotOverwriteFirstOrgClosestBlock()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Форма 1.1");
        sheet.Cells[1, 1].Value = "Непарная";
        sheet.Cells[2, 1].Value = "поля";

        // Layout1115: SourceColCount = 7+17 = 24 → ClosestStartCol = 26
        const int closestStartCol = 26;
        var rowA = Pairing41TestAccess.GetNextDataRowForTests(sheet);
        Assert.Equal(3, rowA);
        sheet.Cells[rowA, 1].Value = "94015";
        sheet.Cells[rowA, closestStartCol].Value = "94015-closest";

        var rowB = Pairing41TestAccess.GetNextDataRowForTests(sheet);
        Assert.Equal(4, rowB);
        sheet.Cells[rowB, 1].Value = "93024";

        Assert.Equal("94015", sheet.Cells[3, 1].Value?.ToString());
        Assert.Equal("94015-closest", sheet.Cells[3, closestStartCol].Value?.ToString());
        Assert.Equal("93024", sheet.Cells[4, 1].Value?.ToString());
        Assert.True(string.IsNullOrEmpty(sheet.Cells[4, closestStartCol].Value?.ToString()));
    }

    /// <summary>
    /// Dual-block: кандидат и карта полей согласованы; слева и справа одни и те же true/false.
    /// </summary>
    [Fact]
    public void Closest11_FieldMatches_IsSingleMapForSourceAndCandidate()
    {
        var form11 = new List<Pairing41Row> { MakeRow11(1, documentNumber: "DOC-A") };
        var form15 = new List<Pairing41Row> { MakeRow11(101, documentNumber: "DOC-B") };

        var result = Pairing41ScenarioRunner.RunClosestMatches(new Pairing41TestCase
        {
            Name = "dual-map",
            Form11 = form11,
            Form15 = form15,
            ExpectedUnpaired11 = [1],
            ExpectedUnpaired15 = [101]
        });

        Assert.Equal(101, result.Closest11CandidateIds[1]);
        Assert.Equal(1, result.Closest15CandidateIds[101]);
        Assert.False(result.Closest11[1][Pairing11To15Field.DocumentNumber]);
        Assert.False(result.Closest15[101][Pairing11To15Field.DocumentNumber]);
        Assert.True(result.Closest11[1][Pairing11To15Field.Type]);
        Assert.True(result.Closest15[101][Pairing11To15Field.Type]);
    }

    private static Pairing41Row MakeRow11(
        int id,
        int repsId = 0,
        string orgRegNo = "",
        string documentNumber = "DOC-1",
        string type = "Тип-А") =>
        new()
        {
            Id = id,
            RepsId = repsId,
            OrgRegNo = orgRegNo,
            OpCode = "41",
            OpDate = "2024-01-15",
            PasNum = "P-1",
            FacNum = "F-1",
            Type = type,
            Radionuclids = "кобальт-60",
            Activity = "1e6",
            Quantity = 1,
            CreationDate = "2020-01-01",
            DocumentVid = 1,
            DocumentNumber = documentNumber,
            DocumentDate = "2024-01-10",
            ProviderOrRecieverOkpo = "111",
            TransporterOkpo = "222",
            PackName = "Уп",
            PackType = "Т",
            PackNumber = "УКТ-1"
        };
}

/// <summary>Unit-тесты <see cref="RaoCodeHelper"/>.</summary>
public sealed class RaoCodeHelperTests
{
    [Fact]
    public void Form12CodeRao_IsConstant()
    {
        Assert.Equal("22511300522", RaoCodeHelper.Form12CodeRao);
    }

    [Fact]
    public void AggregateStateMatchesCodeRao_True_WhenFirstDigitMatches()
    {
        Assert.True(RaoCodeHelper.AggregateStateMatchesCodeRao(1, "1_anything"));
    }

    [Fact]
    public void AggregateStateMatchesCodeRao_False_WhenDigitDiffers_OrEmpty()
    {
        Assert.False(RaoCodeHelper.AggregateStateMatchesCodeRao(2, "1_anything"));
        Assert.False(RaoCodeHelper.AggregateStateMatchesCodeRao(null, "1_anything"));
        Assert.False(RaoCodeHelper.AggregateStateMatchesCodeRao(1, ""));
        Assert.False(RaoCodeHelper.AggregateStateMatchesCodeRao(1, null));
    }

    [Fact]
    public void GetAggregateStateDigitFromCodeRao_ReturnsFirstChar()
    {
        Assert.Equal("2", RaoCodeHelper.GetAggregateStateDigitFromCodeRao("2_410084_"));
        Assert.Equal(string.Empty, RaoCodeHelper.GetAggregateStateDigitFromCodeRao(""));
        Assert.Equal(string.Empty, RaoCodeHelper.GetAggregateStateDigitFromCodeRao(null));
    }
}
