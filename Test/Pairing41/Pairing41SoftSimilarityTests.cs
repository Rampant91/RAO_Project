using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.Testing;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

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

    [Theory]
    [InlineData("-", "без номера")]
    [InlineData("без номера", "-")]
    [InlineData("б.н.", "без номера")]
    [InlineData("н.д.", "-")]
    [InlineData("н/д", "нет данных")]
    public void PackNumber_EmptyMarkers_AreExact(string left, string right)
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PackNumber,
            new Pairing41Row { Id = 1, PackNumber = left },
            new Pairing41Row { Id = 2, PackNumber = right });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void PackNumber_EmptyVsFilled_IsMismatch()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PackNumber,
            new Pairing41Row { Id = 1, PackNumber = "-" },
            new Pairing41Row { Id = 2, PackNumber = "17425" });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
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
    public void Type_AdjacentCharSwap_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Type,
            new Pairing41Row { Id = 1, Type = "АИП-ЭДГХ" },
            new Pairing41Row { Id = 2, Type = "АИП-ЭДХГ" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void TypeAndPackType_ParentheticalAlias_IsNear()
    {
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Type,
            new Pairing41Row { Id = 1, Type = "ИМН-Г-1 (ОИСН)" },
            new Pairing41Row { Id = 2, Type = "ОИСН" }));
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PackType,
            new Pairing41Row { Id = 1, PackType = "ИМН-Г-1 (ОИСН)" },
            new Pairing41Row { Id = 2, PackType = "ОИСН" }));
    }

    [Fact]
    public void PassportFactoryPack_LeadingZeros_AreNear()
    {
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PassportNumber,
            new Pairing41Row { Id = 1, PasNum = "001" },
            new Pairing41Row { Id = 2, PasNum = "1" }));
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.FactoryNumber,
            new Pairing41Row { Id = 1, FacNum = "0001" },
            new Pairing41Row { Id = 2, FacNum = "001" }));
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PackNumber,
            new Pairing41Row { Id = 1, PackNumber = "001" },
            new Pairing41Row { Id = 2, PackNumber = "1" }));
    }

    [Fact]
    public void PassportFactoryPack_AdjacentCharSwap_IsNear()
    {
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PassportNumber,
            new Pairing41Row { Id = 1, PasNum = "4510" },
            new Pairing41Row { Id = 2, PasNum = "4501" }));
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.FactoryNumber,
            new Pairing41Row { Id = 1, FacNum = "4510" },
            new Pairing41Row { Id = 2, FacNum = "4501" }));
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PackNumber,
            new Pairing41Row { Id = 1, PackNumber = "4510" },
            new Pairing41Row { Id = 2, PackNumber = "4501" }));
    }

    [Fact]
    public void PassportFactoryPack_TrailingMonthYear_IsNear()
    {
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PassportNumber,
            new Pairing41Row { Id = 1, PasNum = "196 06.2015" },
            new Pairing41Row { Id = 2, PasNum = "196" }));
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.FactoryNumber,
            new Pairing41Row { Id = 1, FacNum = "513 11.2014" },
            new Pairing41Row { Id = 2, FacNum = "513" }));
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PackNumber,
            new Pairing41Row { Id = 1, PackNumber = "196,06.2015" },
            new Pairing41Row { Id = 2, PackNumber = "196" }));
    }

    [Fact]
    public void FactoryAndQuantity_NumericRange_AreHighNear()
    {
        // Проводка Code41: диапазон/список зав.№ + qty ↔ одиночный номер (логика в SoftSimilarityCore).
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.FactoryNumber,
            new Pairing41Row { Id = 1, FacNum = "0376-0445", Quantity = 70 },
            new Pairing41Row { Id = 2, FacNum = "0400", Quantity = 1 }));
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Quantity,
            new Pairing41Row { Id = 1, FacNum = "0376-0445", Quantity = 70 },
            new Pairing41Row { Id = 2, FacNum = "0400", Quantity = 1 }));

        const string list = "503, 505, 506, 509-511, 513-518, 530-534, 543-545";
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.FactoryNumber,
            new Pairing41Row { Id = 1, FacNum = "510", Quantity = 1 },
            new Pairing41Row { Id = 2, FacNum = list, Quantity = 20 }));
        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Quantity,
            new Pairing41Row { Id = 1, FacNum = list, Quantity = 20 },
            new Pairing41Row { Id = 2, FacNum = "545", Quantity = 1 }));

        Assert.Equal(FieldMatchLevel.Near, Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Activity,
            new Pairing41Row
            {
                Id = 1,
                FacNum = "663-692, 706-717, 723, 726-742",
                Quantity = 60,
                Activity = "18000000000"
            },
            new Pairing41Row { Id = 2, FacNum = "723", Quantity = 1, Activity = "300000000" }));
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
    public void Type_SpaceVsHyphen_AndDigitOneVsLetterI_Long_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Type,
            new Pairing41Row { Id = 1, Type = "ИБИРЗН-63 IIб-1" },
            new Pairing41Row { Id = 2, Type = "ИБИРЗН-63-IIб-I" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Type_OptionalDotLetterSuffix_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Type,
            new Pairing41Row { Id = 1, Type = "BNi3.C3.4" },
            new Pairing41Row { Id = 2, Type = "BNi3.C3.4.R" });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Type_LongBase_OptionalLetterSuffix_IsNear()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.Type,
            new Pairing41Row { Id = 1, Type = "GM-232.02.000" },
            new Pairing41Row { Id = 2, Type = "GM-232.02.000-SFC" });
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

    [Fact]
    public void PackNumber_HyphenatedSerials_Different_IsMismatch()
    {
        var level = Pairing41TestAccess.SimilarityLevel11To15ForTests(
            Pairing11To15Field.PackNumber,
            new Pairing41Row { Id = 1, PackNumber = "1-692" },
            new Pairing41Row { Id = 2, PackNumber = "1-346" });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }
}
