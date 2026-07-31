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
        var form11Off = new TransferReceiveFormParams(
            CheckOperationCode: false,
            CheckOperationDate: false,
            CheckPassportNumber: false,
            CheckType: false,
            CheckRadionuclids: false,
            CheckFactoryNumber: false,
            CheckQuantity: false,
            CheckActivity: false,
            CheckCreatorOkpo: false,
            CheckCreationDate: false,
            CheckProviderOrRecieverOkpo: false,
            CheckPackNumber: false);

        var pairing = new TransferReceiveParamsSet(form11Off, DefaultForm13Params());

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
        var cp11 = new List<TransferReceiveRow>(); // нет пары → были бы непарные, если форма включена

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

        var (unpaired11, unpaired13) = TransferReceiveTestAccess.AnalyzeEnabledFormsUnpairedForTests(
            OurOkpo, our11, our13, cp11, cp13, pairing);

        Assert.Empty(unpaired11);
        Assert.Equal([13], unpaired13);
    }

    [Fact]
    public void Form11Enabled_WithSameOps_ReportsUnpaired()
    {
        var pairing = new TransferReceiveParamsSet(new TransferReceiveFormParams(), DefaultForm13Params());
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

        var (unpaired11, _) = TransferReceiveTestAccess.AnalyzeEnabledFormsUnpairedForTests(
            OurOkpo, our11, [], [], [], pairing);

        Assert.Equal([1], unpaired11);
    }
}
