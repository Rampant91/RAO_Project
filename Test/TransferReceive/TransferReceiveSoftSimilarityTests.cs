using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>Unit-проверки soft-similarity closest без полного сценария.</summary>
public sealed class TransferReceiveSoftSimilarityTests
{
    [Fact]
    public void Passport_BackslashYear_IsExact()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = @"3197\25", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = "3197/25", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void ClosestConfidence_PerfectScore_CapsAt99()
    {
        Assert.Equal(99, WeightedClosestMatchEngine.ToConfidencePercent(10, 10));
        Assert.Equal(99, WeightedClosestMatchEngine.ToConfidencePercent(1, 1));
        Assert.Equal(50, WeightedClosestMatchEngine.ToConfidencePercent(1, 2));
        Assert.Equal(0, WeightedClosestMatchEngine.ToConfidencePercent(1, 0));
        Assert.Equal(WeightedClosestMatchEngine.MaxConfidencePercent, 99);
    }

    [Fact]
    public void Passport_BothEmpty_IsExact_EmptyVsFilled_IsMismatch()
    {
        var bothEmpty = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = "-", FacNum = "-", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = "б.н.", FacNum = "-", IsTransfer = false });
        var emptyVsFilled = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = "-", FacNum = "F-1", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = "P-1", FacNum = "F-1", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Exact, bothEmpty);
        Assert.Equal(FieldMatchLevel.Mismatch, emptyVsFilled);
    }

    [Fact]
    public void CreatorOkpo_BothDash_IsExact()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.CreatorOkpo,
            new TransferReceiveRow { Id = 1, OpCode = "21", CreatorOkpo = "-", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", CreatorOkpo = "-", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void Radionuclids_OrderIndependent_IsExact()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Radionuclids,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                Radionuclids = "Cs-137; Co-60",
                IsTransfer = true
            },
            new TransferReceiveRow
            {
                Id = 2,
                OpCode = "31",
                Radionuclids = "Co-60;Cs-137",
                IsTransfer = false
            });
        Assert.Equal(FieldMatchLevel.Exact, level);
    }

    [Fact]
    public void Radionuclids_OneTokenTypo_SameCount_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Radionuclids,
            new TransferReceiveRow { Id = 1, OpCode = "21", Radionuclids = "Cs-137", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Radionuclids = "Cs-138", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void CreatorOkpo_OneCharTypo_IsNear_FullMismatch_IsMismatch()
    {
        var oneChar = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.CreatorOkpo,
            new TransferReceiveRow { Id = 1, OpCode = "21", CreatorOkpo = "30000003", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", CreatorOkpo = "30000004", IsTransfer = false });
        var full = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.CreatorOkpo,
            new TransferReceiveRow { Id = 1, OpCode = "21", CreatorOkpo = "30000003", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", CreatorOkpo = "99999999", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, oneChar);
        Assert.Equal(FieldMatchLevel.Mismatch, full);
    }

    [Fact]
    public void Factory_ManufacturerPrefix_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", FacNum = "74041/22/0436", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", FacNum = "0436", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Passport_SameYearDifferentNumber_IsMismatch()
    {
        // Общий «/22» не делает номера похожими.
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = "829/22", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = "721/22", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }

    [Fact]
    public void Passport_LookalikeVariants_AreExact()
    {
        var a = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = "3C0", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = "3СО", IsTransfer = false });
        var b = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = "3C0", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = "ЗС0", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Exact, a);
        Assert.Equal(FieldMatchLevel.Exact, b);
    }

    [Fact]
    public void Type_ZeroVsLetterO_WithOptionalTail_IsNear()
    {
        // «6С0» ↔ «6СО-326»: 0≈О + обрезанный хвост.
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "6С0", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "6СО-326", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Type_4C0_vs_4CO_WithTail_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "4С0", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "4СО-534", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Type_Lookalike_3C0_3CO_ZC0_AreExact()
    {
        var a = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "3C0", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "3СО", IsTransfer = false });
        var b = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "3C0", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "ЗС0", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Exact, a);
        Assert.Equal(FieldMatchLevel.Exact, b);
    }

    [Fact]
    public void Type_OptionalTail_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "2П9-254", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "2П9", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Theory]
    [InlineData("3СО-213.89", "3СО-213")]
    [InlineData("3СО-806.89", "3СО-806")]
    [InlineData("3СО-806.90", "3СО-806")]
    [InlineData("3СО-213", "3СО-213.89")]
    [InlineData("3СО-806", "3СО-806.90")]
    public void Type_OptionalDecimalTail_WithZeroInNumber_IsNear(string left, string right)
    {
        // «.XX» — две цифры после точки (не обязательно год); умеренный Near.
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = right, IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);

        var a = SoftSimilarityCore.LightNormalizeId(left, mapDigitZeroToO: false);
        var b = SoftSimilarityCore.LightNormalizeId(right, mapDigitZeroToO: false);
        Assert.True(SoftSimilarityCore.MatchesOptionalDotTwoDigitSuffix(a, b));
        Assert.Equal(
            SoftSimilarityCore.TypeOptionalDotTwoDigitNearScore,
            SoftSimilarityCore.SimilarityType(left, right).Score,
            precision: 5);
    }

    [Theory]
    [InlineData("BNi3.C3.4", "BNi3.C3.4.R")]
    [InlineData("BNi3.C3.4.R", "BNi3.C3.4")]
    [InlineData("ABC.12", "ABC.12.X")]
    public void Type_OptionalDotLetterSuffix_IsNear(string left, string right)
    {
        Assert.Equal(FieldMatchLevel.Near, SoftSimilarityCore.SimilarityType(left, right).Level);
        Assert.True(SoftSimilarityCore.MatchesOptionalDotShortSuffix(
            SoftSimilarityCore.LightNormalizeId(left, mapDigitZeroToO: false),
            SoftSimilarityCore.LightNormalizeId(right, mapDigitZeroToO: false)));
        Assert.Equal(
            SoftSimilarityCore.TypeOptionalDotTwoDigitNearScore,
            SoftSimilarityCore.SimilarityType(left, right).Score,
            precision: 5);
    }

    [Theory]
    [InlineData("AB", "AB.R")]
    [InlineData("A", "A.B")]
    public void Type_OptionalDotLetterSuffix_ShortCore_IsMismatch(string left, string right)
    {
        Assert.False(SoftSimilarityCore.MatchesOptionalDotShortSuffix(
            SoftSimilarityCore.LightNormalizeId(left, mapDigitZeroToO: false),
            SoftSimilarityCore.LightNormalizeId(right, mapDigitZeroToO: false)));
        Assert.Equal(FieldMatchLevel.Mismatch, SoftSimilarityCore.SimilarityType(left, right).Level);
    }

    [Fact]
    public void Type_DifferentHyphenNumbers_IsMismatch()
    {
        // Не должны совпасть только из‑за общего префикса «3СО» после срезания хвоста.
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "3СО-213", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "3СО-806", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }

    [Fact]
    public void Type_SpaceVsHyphen_AndDigitOneVsLetterI_Long_IsNear()
    {
        // Пробел↔тире + «1»↔«I» в длинном типе: без снятия тире было бы distance≥2 → Mismatch.
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "ИБИРЗН-63 IIб-1", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "ИБИРЗН-63-IIб-I", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
        Assert.Equal(FieldMatchLevel.Near, SoftSimilarityCore.SimilarityType("ИБИРЗН-63 IIб-1", "ИБИРЗН-63-IIб-I").Level);
    }

    [Theory]
    [InlineData("1", "I")]
    [InlineData("I", "1")]
    [InlineData("A1", "AI")]
    [InlineData("AI", "A1")]
    public void Type_DigitOneVsLetterI_Short_IsMismatch(string left, string right)
    {
        // Короткие обозначения: одна «похожая» пара символов не смягчается до Near.
        Assert.Equal(FieldMatchLevel.Mismatch, SoftSimilarityCore.SimilarityType(left, right).Level);
    }

    [Fact]
    public void Type_MissingHyphenOnly_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "ИБИРЗН-63IIб", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "ИБИРЗН-63-IIб", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Theory]
    [InlineData("GM-232.02.000", "GM-232.02.000-SFC")]
    [InlineData("GM-232.02.000-SFC", "GM-232.02.000")]
    [InlineData("GM232.02-000", "GM-232.02.000-SFC")]
    [InlineData("GM232.02-000", "GM232.02-000-SFC")]
    [InlineData("GM232.02-000", "GM232.02-000 SFC")]
    public void Type_LongBase_OptionalLetterSuffix_IsNear(string left, string right)
    {
        var similarity = SoftSimilarityCore.SimilarityType(left, right);
        Assert.Equal(FieldMatchLevel.Near, similarity.Level);
        Assert.Equal(
            SoftSimilarityCore.TypeOptionalLetterSuffixNearScore,
            similarity.Score,
            precision: 5);
        Assert.True(SoftSimilarityCore.MatchesTypeOptionalLetterSuffix(
            SoftSimilarityCore.LightNormalizeTypeKeepingSeparators(left, mapDigitZeroToO: false),
            SoftSimilarityCore.LightNormalizeTypeKeepingSeparators(right, mapDigitZeroToO: false)));
    }

    [Theory]
    [InlineData("ABC", "ABC-SFC")]
    [InlineData("AB12", "AB12-SFC")]
    public void Type_ShortBase_OptionalLetterSuffix_IsMismatch(string left, string right)
    {
        Assert.False(SoftSimilarityCore.MatchesTypeOptionalLetterSuffix(
            SoftSimilarityCore.LightNormalizeTypeKeepingSeparators(left, mapDigitZeroToO: false),
            SoftSimilarityCore.LightNormalizeTypeKeepingSeparators(right, mapDigitZeroToO: false)));
        Assert.Equal(FieldMatchLevel.Mismatch, SoftSimilarityCore.SimilarityType(left, right).Level);
    }

    [Fact]
    public void Type_WithYearSuffix_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "1СО", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "1СО-326.24", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Type_OneCharTypo_LongerThanTwo_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "5П9-405", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "5П9-404", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Type_AdjacentCharSwap_Anywhere_IsNear()
    {
        // Пример из практики: перепутаны две соседние буквы в конце.
        var endSwap = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "АИП-ЭДГХ", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "АИП-ЭДХГ", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, endSwap);

        // Та же опечатка может быть в середине строки.
        var midSwap = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "АИПГХ-ЭД", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "АИПХГ-ЭД", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, midSwap);

        Assert.True(SoftSimilarityCore.IsAdjacentCharacterTransposition("АИП-ЭДГХ", "АИП-ЭДХГ"));
        Assert.False(SoftSimilarityCore.IsAdjacentCharacterTransposition("АИП-ЭДГХ", "АИП-ЭДГY"));
    }

    [Fact]
    public void TypeAndPackType_ParentheticalAlias_IsNear()
    {
        var typeLevel = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Type,
            new TransferReceiveRow { Id = 1, OpCode = "21", Type = "ИМН-Г-1 (ОИСН)", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Type = "ОИСН", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, typeLevel);

        var packLevel = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PackType,
            new TransferReceiveRow { Id = 1, OpCode = "21", PackType = "ИМН-Г-1 (ОИСН)", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PackType = "ОИСН", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, packLevel);

        Assert.True(SoftSimilarityCore.MatchesParentheticalAlias(
            SoftSimilarityCore.LightNormalizeId("ИМН-Г-1 (ОИСН)", mapDigitZeroToO: true),
            SoftSimilarityCore.LightNormalizeId("ОИСН", mapDigitZeroToO: true)));
        Assert.False(SoftSimilarityCore.MatchesParentheticalAlias(
            SoftSimilarityCore.LightNormalizeId("ИМН-Г-1 (ОИСН)", mapDigitZeroToO: true),
            SoftSimilarityCore.LightNormalizeId("ДРУГОЕ", mapDigitZeroToO: true)));
    }

    [Theory]
    [InlineData("Т-12", "Т12")]
    [InlineData("Т12", "Т-12")]
    [InlineData("Т-09", "Т09")]
    [InlineData("T-12", "T12")]
    [InlineData("P-100", "P100")]
    public void PassportFactory_OptionalHyphen_IsNear(string left, string right)
    {
        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", FacNum = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", FacNum = right, IsTransfer = false }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = right, IsTransfer = false }));

        Assert.True(SoftSimilarityCore.EqualIgnoringHyphens(
            SoftSimilarityCore.LightNormalizeId(left, mapDigitZeroToO: false),
            SoftSimilarityCore.LightNormalizeId(right, mapDigitZeroToO: false)));
    }

    [Theory]
    [InlineData("0378.09", "378")]
    [InlineData("378", "0378.09")]
    [InlineData("0378.09", "0378")]
    [InlineData("378.09", "378")]
    public void PassportFactory_LeadingZeroAndDotTwoDigit_IsNear(string left, string right)
    {
        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", FacNum = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", FacNum = right, IsTransfer = false }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = right, IsTransfer = false }));

        var score = SoftSimilarityCore.SimilarityPassportOrFactory(left, right, isFactory: true).Score;
        Assert.True(score is >= SoftSimilarityCore.CombinedSoftNearScore - 0.001
            and <= SoftSimilarityCore.OptionalDotTwoDigitNearScore + 0.001);
    }

    [Theory]
    [InlineData("001", "1")]
    [InlineData("0001", "001")]
    [InlineData("0001", "1")]
    [InlineData("P-001", "P-1")]
    public void PassportFactoryPack_LeadingZeros_AreNear(string left, string right)
    {
        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = right, IsTransfer = false }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", FacNum = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", FacNum = right, IsTransfer = false }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PackNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PackNumber = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PackNumber = right, IsTransfer = false }));

        Assert.True(SoftSimilarityCore.EqualIgnoringLeadingZerosInDigitRuns(
            SoftSimilarityCore.LightNormalizeId(left),
            SoftSimilarityCore.LightNormalizeId(right)));
    }

    [Fact]
    public void Passport_LeadingZeros_DifferentNumber_IsMismatch()
    {
        // После снятия нулей: «1» vs «100» — разные числа, не Near по ведущим нулям.
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = "001", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = "100", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
        Assert.False(SoftSimilarityCore.EqualIgnoringLeadingZerosInDigitRuns("001", "100"));
    }

    [Theory]
    [InlineData("4510", "4501")]
    [InlineData("AB12", "AB21")]
    public void PassportFactoryPack_AdjacentCharSwap_IsNear(string left, string right)
    {
        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = right, IsTransfer = false }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", FacNum = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", FacNum = right, IsTransfer = false }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PackNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PackNumber = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PackNumber = right, IsTransfer = false }));

        Assert.True(SoftSimilarityCore.IsAdjacentCharacterTransposition(
            SoftSimilarityCore.LightNormalizeId(left),
            SoftSimilarityCore.LightNormalizeId(right)));
    }

    [Theory]
    [InlineData("196 06.2015", "196")]
    [InlineData("513 11.2014", "513")]
    [InlineData("196,06.2015", "196")]
    [InlineData("513/11.2014", "513")]
    public void PassportFactoryPack_TrailingMonthYear_IsNear(string withDate, string bare)
    {
        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = withDate, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = bare, IsTransfer = false }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", FacNum = withDate, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", FacNum = bare, IsTransfer = false }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PackNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PackNumber = withDate, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PackNumber = bare, IsTransfer = false }));

        var left = SoftSimilarityCore.LightNormalizeId(withDate, mapDigitZeroToO: false);
        var right = SoftSimilarityCore.LightNormalizeId(bare, mapDigitZeroToO: false);
        Assert.True(SoftSimilarityCore.MatchesTrailingMonthYearAlias(left, right));
    }

    [Theory]
    [InlineData("2020-0264 FRD No 36", "2020-0264 FRD")]
    [InlineData("2020-0264 FRD", "2020-0264 FRD No 36")]
    [InlineData("2020-0264 FRD №36", "2020-0264 FRD")]
    [InlineData("2020-0264 FRD номер 36", "2020-0264 FRD")]
    [InlineData("2020-0264 FRD номера 36", "2020-0264 FRD")]
    [InlineData("2020-0264 FRD No.36", "2020-0264 FRD")]
    public void PassportFactory_TrailingSerialLabel_IsNear(string left, string right)
    {
        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", FacNum = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", FacNum = right, IsTransfer = false }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = right, IsTransfer = false }));

        var a = SoftSimilarityCore.LightNormalizeId(left, mapDigitZeroToO: false);
        var b = SoftSimilarityCore.LightNormalizeId(right, mapDigitZeroToO: false);
        Assert.True(SoftSimilarityCore.MatchesTrailingSerialLabelAlias(a, b));
    }

    [Fact]
    public void PassportFactory_TrailingSerialLabel_DifferentNumbers_IsMismatch()
    {
        // Разные хвосты No 36 / No 99: не алиас и не опечатка в 1 знак.
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", FacNum = "2020-0264 FRD No 36", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", FacNum = "2020-0264 FRD No 99", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Mismatch, level);

        var a = SoftSimilarityCore.LightNormalizeId("2020-0264 FRD No 36", mapDigitZeroToO: false);
        var b = SoftSimilarityCore.LightNormalizeId("2020-0264 FRD No 99", mapDigitZeroToO: false);
        Assert.False(SoftSimilarityCore.MatchesTrailingSerialLabelAlias(a, b));
    }

    [Fact]
    public void Pack_TrailingSerialLabel_DoesNotUsePassportFactoryRule()
    {
        // Без общего числового токена УКТ (иначе сработает subset частей).
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PackNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PackNumber = "ABC-FRD No 36", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PackNumber = "ABC-FRD", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Mismatch, level);

        var a = SoftSimilarityCore.LightNormalizeId("ABC-FRD No 36", mapDigitZeroToO: false);
        var b = SoftSimilarityCore.LightNormalizeId("ABC-FRD", mapDigitZeroToO: false);
        Assert.True(SoftSimilarityCore.MatchesTrailingSerialLabelAlias(a, b));
    }

    [Theory]
    [InlineData("Акт определения характеристик №10 от 20.09.2025", "Акт №10")]
    [InlineData("Акт №10", "Акт определения характеристик №10 от 20.09.2025")]
    [InlineData("Акт №010", "Акт №10")]
    [InlineData("Акт No 10", "Акт номер 10")]
    [InlineData("Акт №1 от 17.06.2025", "1")]
    [InlineData("1", "Акт №1 от 17.06.2025")]
    [InlineData("Акт №10 от 20.09.2025", "№10")]
    [InlineData("010", "Акт №10")]
    public void Passport_ActNumberAlias_IsNear(string left, string right)
    {
        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = right, IsTransfer = false }));
        Assert.True(SoftSimilarityCore.MatchesPassportActNumberAlias(left, right));
    }

    [Fact]
    public void Passport_ActNumberAlias_BareNumberDifferent_IsMismatch()
    {
        Assert.False(SoftSimilarityCore.MatchesPassportActNumberAlias("Акт №1 от 17.06.2025", "2"));
        Assert.False(SoftSimilarityCore.MatchesPassportActNumberAlias("Акт №1 от 17.06.2025", "17"));
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = "Акт №1 от 17.06.2025", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = "2", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }

    [Fact]
    public void Passport_ActNumberAlias_DifferentNumbers_IsMismatch()
    {
        Assert.False(SoftSimilarityCore.MatchesPassportActNumberAlias(
            "Акт №10", "Акт определения характеристик №11 от 20.09.2025"));
        // №10↔№99: не алиас и не опечатка в 1 знак.
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = "Акт №10", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = "Акт №99", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }

    [Fact]
    public void Factory_ActNumberAlias_DoesNotApply()
    {
        // Правило только для паспорта — зав.№ с «Акт» не подтягиваем тем же алиасом.
        Assert.True(SoftSimilarityCore.MatchesPassportActNumberAlias(
            "Акт №10", "Акт определения характеристик №10 от 20.09.2025"));
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                FacNum = "Акт определения характеристик №10 от 20.09.2025",
                IsTransfer = true
            },
            new TransferReceiveRow { Id = 2, OpCode = "31", FacNum = "Акт №10", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }

    [Theory]
    [InlineData("0376-0445", 70, "0376", 1)]
    [InlineData("0376-0445", 70, "0445", 1)]
    [InlineData("0376-0445", 70, "0400", 1)]
    [InlineData("0376–0445", 70, "376", 1)] // en-dash; ведущие нули у одиночного
    [InlineData("номера 112-137", 26, "112", 1)]
    [InlineData("номер 112-137", 26, "120", 1)]
    [InlineData("№112-137", 26, "137", 1)]
    public void Factory_NumericRange_WithMatchingQty_IsHighNear(
        string rangeFac, int rangeQty, string singleFac, int singleQty)
    {
        Assert.True(SoftSimilarityCore.TryFactoryNumericRangeAlias(
            rangeFac, singleFac, rangeQty, singleQty));
        // В обе стороны: одиночный ↔ перечисление.
        Assert.True(SoftSimilarityCore.TryFactoryNumericRangeAlias(
            singleFac, rangeFac, singleQty, rangeQty));

        var factoryLevel = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                FacNum = rangeFac,
                Quantity = rangeQty,
                IsTransfer = true
            },
            new TransferReceiveRow
            {
                Id = 2,
                OpCode = "31",
                FacNum = singleFac,
                Quantity = singleQty,
                IsTransfer = false
            });
        Assert.Equal(FieldMatchLevel.Near, factoryLevel);

        var quantityLevel = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Quantity,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                FacNum = rangeFac,
                Quantity = rangeQty,
                IsTransfer = true
            },
            new TransferReceiveRow
            {
                Id = 2,
                OpCode = "31",
                FacNum = singleFac,
                Quantity = singleQty,
                IsTransfer = false
            });
        Assert.Equal(FieldMatchLevel.Near, quantityLevel);

        var sim = SoftSimilarityCore.SimilarityPassportOrFactory(
            rangeFac, singleFac, isFactory: true, rangeQty, singleQty);
        Assert.Equal(FieldMatchLevel.Near, sim.Level);
        Assert.Equal(SoftSimilarityCore.FactoryNumericRangeNearScore, sim.Score);
        Assert.True(sim.Score > 0.7);

        var reverseFactory = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                FacNum = singleFac,
                Quantity = singleQty,
                IsTransfer = true
            },
            new TransferReceiveRow
            {
                Id = 2,
                OpCode = "31",
                FacNum = rangeFac,
                Quantity = rangeQty,
                IsTransfer = false
            });
        Assert.Equal(FieldMatchLevel.Near, reverseFactory);
    }

    [Fact]
    public void Factory_ComplexEnumeration_WithMatchingQty_IsHighNear_BothWays()
    {
        const string list =
            "503, 505, 506, 509-511, 513-518, 530-534, 543-545";
        const int qty = 20;

        Assert.True(SoftSimilarityCore.TryExpandFactoryNumericEnumeration(list, out var numbers));
        Assert.Equal(qty, numbers.Count);
        Assert.Contains(503L, numbers);
        Assert.Contains(511L, numbers);
        Assert.Contains(545L, numbers);
        Assert.DoesNotContain(504L, numbers);
        Assert.DoesNotContain(512L, numbers);

        Assert.True(SoftSimilarityCore.TryFactoryNumericRangeAlias(list, "510", qty, 1));
        Assert.True(SoftSimilarityCore.TryFactoryNumericRangeAlias("545", list, 1, qty));
        Assert.False(SoftSimilarityCore.TryFactoryNumericRangeAlias(list, "504", qty, 1));
        Assert.False(SoftSimilarityCore.TryFactoryNumericRangeAlias(list, "510", qty - 1, 1));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                FacNum = list,
                Quantity = qty,
                IsTransfer = true
            },
            new TransferReceiveRow
            {
                Id = 2,
                OpCode = "31",
                FacNum = "534",
                Quantity = 1,
                IsTransfer = false
            }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                FacNum = "503",
                Quantity = 1,
                IsTransfer = true
            },
            new TransferReceiveRow
            {
                Id = 2,
                OpCode = "31",
                FacNum = list,
                Quantity = qty,
                IsTransfer = false
            }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Quantity,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                FacNum = "518",
                Quantity = 1,
                IsTransfer = true
            },
            new TransferReceiveRow
            {
                Id = 2,
                OpCode = "31",
                FacNum = list,
                Quantity = qty,
                IsTransfer = false
            }));
    }

    [Fact]
    public void Activity_WithFactoryEnumeration_SumMatchesUnitTimesN_IsHighNear_BothWays()
    {
        // Список: сумма одинаковых вкладов 60×3e8 = 1.8e10 (частный случай суммы элементов).
        const string list = "663-692, 706-717, 723, 726-742";
        const int qty = 60;
        const string unitActivity = "300000000";
        const string sumActivity = "18000000000";

        var exact = SoftSimilarityCore.SimilarityActivityConsideringFactoryEnumeration(
            sumActivity, unitActivity, list, "723", qty, 1);
        Assert.Equal(FieldMatchLevel.Near, exact.Level);
        Assert.Equal(SoftSimilarityCore.FactoryNumericRangeNearScore, exact.Score);

        var exactReverse = SoftSimilarityCore.SimilarityActivityConsideringFactoryEnumeration(
            unitActivity, sumActivity, "723", list, 1, qty);
        Assert.Equal(FieldMatchLevel.Near, exactReverse.Level);
        Assert.Equal(SoftSimilarityCore.FactoryNumericRangeNearScore, exactReverse.Score);

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Activity,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                FacNum = list,
                Quantity = qty,
                Activity = sumActivity,
                IsTransfer = true
            },
            new TransferReceiveRow
            {
                Id = 2,
                OpCode = "31",
                FacNum = "742",
                Quantity = 1,
                Activity = unitActivity,
                IsTransfer = false
            }));

        // ±10% от ожидаемой суммы → Near чуть ниже.
        var withinTol = SoftSimilarityCore.SimilarityActivityConsideringFactoryEnumeration(
            "18900000000", unitActivity, list, "723", qty, 1); // 1.89e10 / 1.8e10 = +5%
        Assert.Equal(FieldMatchLevel.Near, withinTol.Level);
        Assert.Equal(SoftSimilarityCore.FactoryEnumerationActivityNearScoreWithinTolerance, withinTol.Score);

        // unit×N не бьётся, но одиночная активность < суммы группы → слабый Near (правдоподобный вклад).
        var plausiblePart = SoftSimilarityCore.SimilarityActivityConsideringFactoryEnumeration(
            "1200", "300", "101-103", "102", 3, 1);
        Assert.Equal(FieldMatchLevel.Near, plausiblePart.Level);
        Assert.Equal(SoftSimilarityCore.FactoryEnumerationActivityNearScorePlausiblePart, plausiblePart.Score);

        // Одиночная ≥ суммы группы — не может быть частью суммы.
        var notPart = SoftSimilarityCore.SimilarityActivityConsideringFactoryEnumeration(
            "1000", "2000", "101-103", "102", 3, 1);
        Assert.NotEqual(SoftSimilarityCore.FactoryEnumerationActivityNearScorePlausiblePart, notPart.Score);
        Assert.NotEqual(SoftSimilarityCore.FactoryNumericRangeNearScore, notPart.Score);
        Assert.NotEqual(SoftSimilarityCore.FactoryEnumerationActivityNearScoreWithinTolerance, notPart.Score);
    }

    [Fact]
    public void Factory_NumericRange_RejectsWrongQtyOutsideOrNonPure()
    {
        // Тире без совпадения qty с длиной ряда — не диапазон.
        Assert.False(SoftSimilarityCore.TryFactoryNumericRangeAlias("0376-0445", "0376", 69, 1));
        Assert.False(SoftSimilarityCore.TryFactoryNumericRangeAlias("12-15", "12", 1, 1));
        // Вне диапазона / не чистое число / qty одиночной ≠ 1.
        Assert.False(SoftSimilarityCore.TryFactoryNumericRangeAlias("0376-0445", "0500", 70, 1));
        Assert.False(SoftSimilarityCore.TryFactoryNumericRangeAlias("A-0376-0445", "0376", 70, 1));
        Assert.False(SoftSimilarityCore.TryFactoryNumericRangeAlias("0376-0445", "0400", 70, 5));
        // Обычный одиночный номер не считается «перечислением» (иначе 503↔503 дало бы Near вместо Exact).
        Assert.False(SoftSimilarityCore.TryFactoryNumericRangeAlias("503", "503", 1, 1));

        Assert.Equal(FieldMatchLevel.Mismatch, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.FactoryNumber,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                FacNum = "0376-0445",
                Quantity = 69,
                IsTransfer = true
            },
            new TransferReceiveRow
            {
                Id = 2,
                OpCode = "31",
                FacNum = "0376",
                Quantity = 1,
                IsTransfer = false
            }));
    }

    [Fact]
    public void Passport_NumericRangeLike_DoesNotUseFactoryRangeRule()
    {
        // Правило только для зав.№: паспорт «0376-0445»↔«0376» не становится Near через range+qty.
        Assert.NotEqual(
            SoftSimilarityCore.FactoryNumericRangeNearScore,
            SoftSimilarityCore.SimilarityPassportOrFactory(
                "0376-0445", "0376", isFactory: false, 70, 1).Score);

        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                PasNum = "0376-0445",
                Quantity = 70,
                IsTransfer = true
            },
            new TransferReceiveRow
            {
                Id = 2,
                OpCode = "31",
                PasNum = "0376",
                Quantity = 1,
                IsTransfer = false
            });
        Assert.NotEqual(FieldMatchLevel.Exact, level);
        // edit-distance / year-suffix — не high-range Near; главное — не Exact и не «зелёный».
        Assert.True(level is FieldMatchLevel.Near or FieldMatchLevel.Mismatch);
        if (level == FieldMatchLevel.Near)
        {
            var sim = SoftSimilarityCore.SimilarityPassportOrFactory(
                "0376-0445", "0376", isFactory: false, 70, 1);
            Assert.NotEqual(SoftSimilarityCore.FactoryNumericRangeNearScore, sim.Score);
        }
    }

    [Fact]
    public void Radionuclids_MissingNuclide_IsMismatch()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Radionuclids,
            new TransferReceiveRow
            {
                Id = 1,
                OpCode = "21",
                Radionuclids = "стронций-90; иттрий-90",
                IsTransfer = true
            },
            new TransferReceiveRow
            {
                Id = 2,
                OpCode = "31",
                Radionuclids = "стронций-90",
                IsTransfer = false
            });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }

    [Fact]
    public void Activity_OrderOfMagnitude_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Activity,
            new TransferReceiveRow { Id = 1, OpCode = "21", Activity = "9.92E+10", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Activity = "9.92E+09", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Mass_WithinTenPercent_IsExact_AndPairs()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Mass,
            new TransferReceiveRow { Id = 1, OpCode = "21", Mass = "1.50", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Mass = "1.60", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Exact, level);
        Assert.True(TransferReceiveTestAccess.MassMatchesForTests("1.50", "1.60"));
        Assert.False(TransferReceiveTestAccess.MassMatchesForTests("1.50", "2.00"));
    }

    [Fact]
    public void Mass_OrderOfMagnitude_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Mass,
            new TransferReceiveRow { Id = 1, OpCode = "21", Mass = "1.5", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Mass = "15", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Mass_KgVsTon_ThousandFold_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Mass,
            new TransferReceiveRow { Id = 1, OpCode = "21", Mass = "1.5", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Mass = "1500", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
        Assert.False(TransferReceiveTestAccess.MassMatchesForTests("1.5", "1500"));
    }

    [Fact]
    public void Mass_NonNumeric_FallsBackToNormalizedEquality()
    {
        var exact = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Mass,
            new TransferReceiveRow { Id = 1, OpCode = "21", Mass = "н/д", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Mass = "н/д", IsTransfer = false });
        var mismatch = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.Mass,
            new TransferReceiveRow { Id = 1, OpCode = "21", Mass = "н/д", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", Mass = "1.5", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Exact, exact);
        Assert.NotEqual(FieldMatchLevel.Exact, mismatch);
    }

    [Fact]
    public void OperationCode_UnpairedTransferReceive_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.OperationCode,
            new TransferReceiveRow { Id = 1, OpCode = "21", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "32", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void Pack_67_vs_67_69_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PackNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PackNumber = "67", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PackNumber = "67(69)", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Theory]
    [InlineData("-", "без номера")]
    [InlineData("б.н.", "-")]
    [InlineData("н/д", "без номера")]
    public void Pack_EmptyMarkers_AreExact(string left, string right)
    {
        Assert.Equal(FieldMatchLevel.Exact, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PackNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PackNumber = left, IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PackNumber = right, IsTransfer = false }));
    }

    [Fact]
    public void Pack_HyphenatedSerials_Different_IsMismatch()
    {
        // «1-692» и «1-346» — разные номера; общий «1» до тире не должен давать Near.
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PackNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PackNumber = "1-692", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PackNumber = "1-346", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Mismatch, level);
    }

    [Fact]
    public void Pack_HyphenatedSerial_SubsetOfList_IsNear()
    {
        var level = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PackNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PackNumber = "1-692", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PackNumber = "1-692 (1-346)", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, level);
    }

    [Fact]
    public void CreationDate_WithinFifteenDays_IsNear()
    {
        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.CreationDate,
            new TransferReceiveRow { Id = 1, OpCode = "21", CreationDate = "31.01.1975", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", CreationDate = "01.02.1975", IsTransfer = false }));

        Assert.Equal(FieldMatchLevel.Near, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.CreationDate,
            new TransferReceiveRow { Id = 1, OpCode = "21", CreationDate = "26.07.2025", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", CreationDate = "19.07.2025", IsTransfer = false }));
    }

    [Fact]
    public void CreationDate_OutsideFifteenDays_IsMismatch()
    {
        Assert.Equal(FieldMatchLevel.Mismatch, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.CreationDate,
            new TransferReceiveRow { Id = 1, OpCode = "21", CreationDate = "26.07.2025", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", CreationDate = "26.04.2025", IsTransfer = false }));
    }

    [Fact]
    public void CreationDate_Same_IsExact()
    {
        Assert.Equal(FieldMatchLevel.Exact, TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.CreationDate,
            new TransferReceiveRow { Id = 1, OpCode = "21", CreationDate = "01.08.1987", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", CreationDate = "01.08.1987", IsTransfer = false }));
    }
}
