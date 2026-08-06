using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>
/// Отключение формы (все Check* = false): unpaired не строится даже при ненулевых ops.
/// </summary>
public sealed class TransferReceiveFormEnableTests
{
    private const string OurOkpo = "10000001";
    private const string CounterpartOkpo = "20000002";
    private const string OpDate = "2024-06-15";

    [Fact]
    public void Form11Disabled_SkipsUnpairedEvenWhenOpsPresent_Form13StillRuns()
    {
        var form11Off = DisabledFormParams();
        var pairing = TransferReceiveParamsSet.Form11And13(form11Off, DefaultForm13Params());

        var our11 = new List<TransferReceiveRow>
        {
            new()
            {
                Id = 1,
                OpCode = "21",
                OpDate = OpDate,
                PasNum = "P-1",
                FacNum = "F-1",
                Type = "T",
                Radionuclids = "Cs-137",
                PackNumber = "U",
                ProviderOrRecieverOkpo = CounterpartOkpo,
                Activity = "1e6",
                CreatorOkpo = "30000003",
                CreationDate = OpDate,
                Quantity = 1,
                IsTransfer = true
            }
        };
        var cp11 = new List<TransferReceiveRow>();

        var our13 = new List<TransferReceiveRow>
        {
            new()
            {
                Id = 13,
                OpCode = "21",
                OpDate = OpDate,
                PasNum = "P-13",
                FacNum = "F-13",
                Type = "T",
                Radionuclids = "Cs-137",
                PackNumber = "U",
                ProviderOrRecieverOkpo = CounterpartOkpo,
                Activity = "1e6",
                CreatorOkpo = "30000003",
                CreationDate = OpDate,
                AggregateState = 1,
                IsTransfer = true
            }
        };
        var cp13 = new List<TransferReceiveRow>();

        var (unpaired11, unpaired12, unpaired13) = TransferReceiveTestAccess.AnalyzeEnabledFormsUnpairedForTests(
            OurOkpo, our11, [], our13, cp11, [], cp13, pairing);

        Assert.Empty(unpaired11);
        Assert.Empty(unpaired12);
        Assert.Equal([13], unpaired13);
    }

    [Fact]
    public void Form12Disabled_SkipsUnpairedEvenWhenOpsPresent_Form11StillRuns()
    {
        var pairing = new TransferReceiveParamsSet(
            new TransferReceiveFormParams(),
            DisabledFormParams(),
            DefaultForm13Params());

        var our11 = new List<TransferReceiveRow>
        {
            new()
            {
                Id = 1,
                OpCode = "21",
                OpDate = OpDate,
                PasNum = "P-1",
                FacNum = "F-1",
                Type = "T",
                Radionuclids = "Cs-137",
                PackNumber = "U",
                ProviderOrRecieverOkpo = CounterpartOkpo,
                Activity = "1e6",
                CreatorOkpo = "30000003",
                CreationDate = OpDate,
                Quantity = 1,
                IsTransfer = true
            }
        };

        var our12 = new List<TransferReceiveRow>
        {
            new()
            {
                Id = 12,
                OpCode = "21",
                OpDate = OpDate,
                PasNum = "P-12",
                FacNum = "F-12",
                Type = "Изделие",
                PackType = "УКТ-1",
                PackNumber = "N-1",
                ProviderOrRecieverOkpo = CounterpartOkpo,
                Mass = "1.5",
                CreatorOkpo = "30000003",
                CreationDate = OpDate,
                Quantity = 1,
                IsTransfer = true
            }
        };

        var (unpaired11, unpaired12, _) = TransferReceiveTestAccess.AnalyzeEnabledFormsUnpairedForTests(
            OurOkpo, our11, our12, [], [], [], [], pairing);

        Assert.Equal([1], unpaired11);
        Assert.Empty(unpaired12);
    }

    [Fact]
    public void Form12Enabled_WithSameOps_ReportsUnpaired()
    {
        var pairing = new TransferReceiveParamsSet(
            DisabledFormParams(),
            DefaultForm12Params(),
            DisabledFormParams());

        var our12 = new List<TransferReceiveRow>
        {
            new()
            {
                Id = 12,
                OpCode = "21",
                OpDate = OpDate,
                PasNum = "P-12",
                FacNum = "F-12",
                Type = "Изделие",
                PackType = "УКТ-1",
                PackNumber = "N-1",
                ProviderOrRecieverOkpo = CounterpartOkpo,
                Mass = "1.5",
                CreatorOkpo = "30000003",
                CreationDate = OpDate,
                Quantity = 1,
                IsTransfer = true
            }
        };

        var (_, unpaired12, _) = TransferReceiveTestAccess.AnalyzeEnabledFormsUnpairedForTests(
            OurOkpo, [], our12, [], [], [], [], pairing);

        Assert.Equal([12], unpaired12);
    }

    [Fact]
    public void Form11Enabled_WithSameOps_ReportsUnpaired()
    {
        var pairing = TransferReceiveParamsSet.Form11And13(
            new TransferReceiveFormParams(), DefaultForm13Params());
        var our11 = new List<TransferReceiveRow>
        {
            new()
            {
                Id = 1,
                OpCode = "21",
                OpDate = OpDate,
                PasNum = "P-1",
                FacNum = "F-1",
                Type = "T",
                Radionuclids = "Cs-137",
                PackNumber = "U",
                ProviderOrRecieverOkpo = CounterpartOkpo,
                Activity = "1e6",
                CreatorOkpo = "30000003",
                CreationDate = OpDate,
                Quantity = 1,
                IsTransfer = true
            }
        };

        var (unpaired11, _, _) = TransferReceiveTestAccess.AnalyzeEnabledFormsUnpairedForTests(
            OurOkpo, our11, [], [], [], [], [], pairing);

        Assert.Equal([1], unpaired11);
    }
}
