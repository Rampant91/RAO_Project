using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

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
    public void Passport_BothEmpty_IsNear_EmptyVsFilled_IsMismatch()
    {
        var bothEmpty = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = "-", FacNum = "-", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = "б.н.", FacNum = "-", IsTransfer = false });
        var emptyVsFilled = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.PassportNumber,
            new TransferReceiveRow { Id = 1, OpCode = "21", PasNum = "-", FacNum = "F-1", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", PasNum = "P-1", FacNum = "F-1", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, bothEmpty);
        Assert.Equal(FieldMatchLevel.Mismatch, emptyVsFilled);
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

    [Fact]
    public void CreationDate_OneDigitTypo_Beats_FartherCalendarDate()
    {
        // 26.07.2025 vs 26.04.2025 — 1 digit; vs 19.07.2025 — 2 digits in day.
        var oneDigit = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.CreationDate,
            new TransferReceiveRow { Id = 1, OpCode = "21", CreationDate = "26.07.2025", IsTransfer = true },
            new TransferReceiveRow { Id = 2, OpCode = "31", CreationDate = "26.04.2025", IsTransfer = false });
        var twoDigit = TransferReceiveTestAccess.SimilarityLevelForTests(
            TransferReceiveField.CreationDate,
            new TransferReceiveRow { Id = 1, OpCode = "21", CreationDate = "26.07.2025", IsTransfer = true },
            new TransferReceiveRow { Id = 3, OpCode = "31", CreationDate = "19.07.2025", IsTransfer = false });
        Assert.Equal(FieldMatchLevel.Near, oneDigit);
        Assert.Equal(FieldMatchLevel.Mismatch, twoDigit);
        Assert.True((int)oneDigit < (int)twoDigit);
    }
}
