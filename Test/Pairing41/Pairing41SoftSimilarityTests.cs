using Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.Testing;
using Client_App.Commands.AsyncCommands.ExcelExport.Shared;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

namespace Test.Pairing41;

/// <summary>Unit-проверки soft-similarity closest для парности 41.</summary>
public sealed class Pairing41SoftSimilarityTests
{
    [Fact]
    public void OperationCode_41vs14_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.OperationCode,
            new Pairing41Row { Id = 1, OpCode = "41" },
            new Pairing41Row { Id = 2, OpCode = "14" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Passport_BackslashYear_IsExact()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PassportNumber,
            new Pairing41Row { Id = 1, PasNum = @"3197\25" },
            new Pairing41Row { Id = 2, PasNum = "3197/25" });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void Passport_BothEmpty_IsExact_EmptyVsFilled_IsMismatch()
    {
        var bothEmpty = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PassportNumber,
            new Pairing41Row { Id = 1, PasNum = "-", FacNum = "-" },
            new Pairing41Row { Id = 2, PasNum = "б.н.", FacNum = "-" });
        var emptyVsFilled = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PassportNumber,
            new Pairing41Row { Id = 1, PasNum = "-", FacNum = "F-1" },
            new Pairing41Row { Id = 2, PasNum = "P-1", FacNum = "F-1" });
        Assert.Equal(FieldMatchLevel.Exact, bothEmpty);
        Assert.Equal(FieldMatchLevel.Mismatch, emptyVsFilled);
    }

    [Fact]
    public void TransporterOkpo_BothDash_IsExact()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.TransporterOkpo,
            new Pairing41Row { Id = 1, TransporterOkpo = "-" },
            new Pairing41Row { Id = 2, TransporterOkpo = "-" });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void ProviderOkpo_BothDash_IsExact()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.ProviderOrRecieverOkpo,
            new Pairing41Row { Id = 1, ProviderOrRecieverOkpo = "-" },
            new Pairing41Row { Id = 2, ProviderOrRecieverOkpo = "-" });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void Radionuclids_OrderIndependent_IsExact()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Radionuclids,
            new Pairing41Row { Id = 1, Radionuclids = "Cs-137; Co-60" },
            new Pairing41Row { Id = 2, Radionuclids = "Co-60;Cs-137" });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void Radionuclids_OneTokenTypo_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Radionuclids,
            new Pairing41Row { Id = 1, Radionuclids = "кобальт-60" },
            new Pairing41Row { Id = 2, Radionuclids = "кобальт-61" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Factory_ManufacturerPrefix_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.FactoryNumber,
            new Pairing41Row { Id = 1, FacNum = "74041/22/0436" },
            new Pairing41Row { Id = 2, FacNum = "0436" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void PackNumber_SameTokensDifferentOrder_IsExact()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PackNumber,
            new Pairing41Row { Id = 1, PackNumber = "39 (38)" },
            new Pairing41Row { Id = 2, PackNumber = "38 (39)" });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void PackNumber_BothDash_IsExact()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PackNumber,
            new Pairing41Row { Id = 1, PackNumber = "-" },
            new Pairing41Row { Id = 2, PackNumber = "-" });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void Type_LookalikeZeroO_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Type,
            new Pairing41Row { Id = 1, Type = "6С0" },
            new Pairing41Row { Id = 2, Type = "6СО-326" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Activity_Within10Percent_IsExact_Outside_IsMismatchOrNear()
    {
        var within = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Activity,
            new Pairing41Row { Id = 1, Activity = "1.0e+06" },
            new Pairing41Row { Id = 2, Activity = "1.05e+06" });
        Assert.Equal(FieldMatchLevel.Exact, within);

        var outside = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Activity,
            new Pairing41Row { Id = 1, Activity = "1.0e+06" },
            new Pairing41Row { Id = 2, Activity = "2.0e+06" });
        Assert.NotEqual(FieldMatchLevel.Exact, outside);
    }

    [Fact]
    public void OperationDate_OneDayDiff_IsNear()
    {
        var level11 = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.OperationDate,
            new Pairing41Row { Id = 1, OpDate = "2024-06-15" },
            new Pairing41Row { Id = 2, OpDate = "2024-06-16" });
        var level12 = Pairing41TestAccess.SimilarityLevel12To16ForTests(
            Pairing12To16Field.OperationDate,
            new Pairing41Row { Id = 1, OpDate = "2024-06-15" },
            new Pairing41Row { Id = 2, OpDate = "2024-06-16" });
        Assert.Equal(FieldMatchLevel.Near, level11);
        Assert.Equal(FieldMatchLevel.Near, level12);
    }

    [Fact]
    public void OperationDate_OutsideWindow_IsMismatch()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.OperationDate,
            new Pairing41Row { Id = 1, OpDate = "2024-01-01" },
            new Pairing41Row { Id = 2, OpDate = "2024-03-01" });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }

    [Fact]
    public void Mass_WithinRelTolerance_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel12To16ForTests(
            Pairing12To16Field.Mass,
            new Pairing41Row { Id = 1, Mass = "1" },
            new Pairing41Row { Id = 2, Mass = "1.12" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void MainRadionuclids13_Typo_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel13To16ForTests(
            Pairing13To16Field.MainRadionuclids,
            new Pairing41Row { Id = 1, MainRadionuclids = "кобальт-60" },
            new Pairing41Row { Id = 2, MainRadionuclids = "кобальт-61" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void DocumentNumber_NormalizedMatch_IsExact()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.DocumentNumber,
            new Pairing41Row { Id = 1, DocumentNumber = " DOC-1 " },
            new Pairing41Row { Id = 2, DocumentNumber = "DOC-1" });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void Passport_SameYearDifferentCore_IsMismatch()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PassportNumber,
            new Pairing41Row { Id = 1, PasNum = "829/22" },
            new Pairing41Row { Id = 2, PasNum = "721/22" });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }

    [Fact]
    public void Type_LookalikeExact_3C0Variants()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Type,
            new Pairing41Row { Id = 1, Type = "3С0" },
            new Pairing41Row { Id = 2, Type = "3CO" });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void Type_OptionalTail_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Type,
            new Pairing41Row { Id = 1, Type = "2П9" },
            new Pairing41Row { Id = 2, Type = "2П9-254" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Activity_OrderOfMagnitude_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Activity,
            new Pairing41Row { Id = 1, Activity = "9.92E+10" },
            new Pairing41Row { Id = 2, Activity = "9.92E+09" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Mass12_OrderOfMagnitude_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel12To16ForTests(
            Pairing12To16Field.Mass,
            new Pairing41Row { Id = 1, Mass = "1.5" },
            new Pairing41Row { Id = 2, Mass = "15" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Volume14_WithinPairingTolerance_IsExact()
    {
        var level = Pairing41TestAccess.SimilarityLevel14To16ForTests(
            Pairing14To16Field.Volume,
            new Pairing41Row { Id = 1, Volume = "2" },
            new Pairing41Row { Id = 2, Volume = "2.2" });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void Volume14_OutsidePairingTolerance_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel14To16ForTests(
            Pairing14To16Field.Volume,
            new Pairing41Row { Id = 1, Volume = "2" },
            new Pairing41Row { Id = 2, Volume = "2.25" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Radionuclids_MissingNuclide_IsMismatch()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Radionuclids,
            new Pairing41Row { Id = 1, Radionuclids = "стронций-90; иттрий-90" },
            new Pairing41Row { Id = 2, Radionuclids = "стронций-90" });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }

    [Fact]
    public void PackNumber_67vs67_69_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PackNumber,
            new Pairing41Row { Id = 1, PackNumber = "67" },
            new Pairing41Row { Id = 2, PackNumber = "67(69)" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }
}
