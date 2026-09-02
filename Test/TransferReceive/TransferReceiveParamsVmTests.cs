using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing;
using Client_App.ViewModels.Messages;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>
/// Диалог параметров: трёхсостояние CheckAll / CheckAll13 (все / ничего / частично).
/// </summary>
public class TransferReceiveParamsVmTests
{
    [Fact]
    public void CheckAll_DefaultsToTrue_AndUncheckingOneField_MakesIndeterminate()
    {
        var vm = new GetTransferReceiveParamsVM();
        Assert.True(vm.CheckAll);

        vm.CheckType = false;
        Assert.Null(vm.CheckAll);
    }

    [Fact]
    public void CheckAll_SetFalse_UnchecksAllFields_ThenSetTrue_Restores()
    {
        var vm = new GetTransferReceiveParamsVM();
        vm.CheckAll = false;
        Assert.False(vm.CheckOperationCode);
        Assert.False(vm.CheckPassportNumber);
        Assert.False(vm.CheckAll);

        vm.CheckAll = true;
        Assert.True(vm.CheckOperationCode);
        Assert.True(vm.CheckPassportNumber);
        Assert.True(vm.CheckAll);
    }

    [Fact]
    public void CheckAll13_DefaultsToTrue_AndUncheckingOneField_MakesIndeterminate()
    {
        var vm = new GetTransferReceiveParamsVM();
        Assert.True(vm.CheckAll13);
        Assert.True(vm.CheckAggregateState13);

        vm.CheckType13 = false;
        Assert.Null(vm.CheckAll13);
    }

    [Fact]
    public void CheckAll13_SetFalse_UnchecksAllFields_ThenSetTrue_Restores()
    {
        var vm = new GetTransferReceiveParamsVM();
        vm.CheckAll13 = false;
        Assert.False(vm.CheckOperationCode13);
        Assert.False(vm.CheckAggregateState13);
        Assert.False(vm.CheckAll13);

        vm.CheckAll13 = true;
        Assert.True(vm.CheckOperationCode13);
        Assert.True(vm.CheckAggregateState13);
        Assert.True(vm.CheckAll13);
    }

    [Fact]
    public void CheckAll_False_MeansFormDisabled()
    {
        var vm = new GetTransferReceiveParamsVM();
        vm.CheckAll = false;
        var form11 = new ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveFormParams(
            CheckOperationCode: vm.CheckOperationCode,
            CheckOperationDate: vm.CheckOperationDate,
            CheckPassportNumber: vm.CheckPassportNumber,
            CheckType: vm.CheckType,
            CheckRadionuclids: vm.CheckRadionuclids,
            CheckFactoryNumber: vm.CheckFactoryNumber,
            CheckQuantity: vm.CheckQuantity,
            CheckActivity: vm.CheckActivity,
            CheckCreatorOkpo: vm.CheckCreatorOkpo,
            CheckCreationDate: vm.CheckCreationDate,
            CheckProviderOrRecieverOkpo: vm.CheckProviderOrRecieverOkpo,
            CheckPackNumber: vm.CheckPackNumber);
        Assert.False(ExcelExportCheckTransferReceiveAsyncCommand.IsFormCheckEnabled(form11));
        Assert.True(ExcelExportCheckTransferReceiveAsyncCommand.IsFormCheckEnabled(
            ExcelExportCheckTransferReceiveAsyncCommand.DefaultForm13Params()));
    }

    [Fact]
    public void CheckAll12_DefaultsToTrue_AndUncheckingOneField_MakesIndeterminate()
    {
        var vm = new GetTransferReceiveParamsVM();
        Assert.True(vm.CheckAll12);
        Assert.True(vm.CheckMass12);
        Assert.True(vm.CheckPackType12);

        vm.CheckName12 = false;
        Assert.Null(vm.CheckAll12);
    }

    [Fact]
    public void CheckAll12_SetFalse_UnchecksAllFields_ThenSetTrue_Restores()
    {
        var vm = new GetTransferReceiveParamsVM();
        vm.CheckAll12 = false;
        Assert.False(vm.CheckOperationCode12);
        Assert.False(vm.CheckMass12);
        Assert.False(vm.CheckPackType12);
        Assert.False(vm.CheckAll12);

        vm.CheckAll12 = true;
        Assert.True(vm.CheckOperationCode12);
        Assert.True(vm.CheckMass12);
        Assert.True(vm.CheckPackType12);
        Assert.True(vm.CheckAll12);
    }

    [Fact]
    public void DefaultForm12Params_IsEnabled_AndDisablesQuantityActivityRadionuclides()
    {
        var p = ExcelExportCheckTransferReceiveAsyncCommand.DefaultForm12Params();
        Assert.True(ExcelExportCheckTransferReceiveAsyncCommand.IsFormCheckEnabled(p));
        Assert.False(p.CheckQuantity);
        Assert.False(p.CheckActivity);
        Assert.False(p.CheckRadionuclids);
        Assert.True(p.CheckMass);
        Assert.True(p.CheckPackType);
        Assert.True(p.CheckType);
    }

    [Fact]
    public void MapParamsFromDialogVm_MapsName12ToCheckType_AndForcesForm12QtyActivityOff()
    {
        var vm = new GetTransferReceiveParamsVM
        {
            CheckName12 = true,
            CheckMass12 = true,
            CheckPackType12 = false,
            CheckQuantity = true,
            CheckActivity = true,
            CheckAggregateState13 = true,
            CheckType13 = false
        };

        var set = TransferReceiveTestAccess.MapParamsFromDialogVmForTests(vm);

        Assert.True(set.Form12.CheckType);
        Assert.True(set.Form12.CheckMass);
        Assert.False(set.Form12.CheckPackType);
        Assert.False(set.Form12.CheckQuantity);
        Assert.False(set.Form12.CheckActivity);
        Assert.False(set.Form12.CheckRadionuclids);
        Assert.True(set.Form11.CheckQuantity);
        Assert.True(set.Form13.CheckAggregateState);
        Assert.False(set.Form13.CheckType);
        Assert.False(set.Form13.CheckQuantity);
        Assert.True(set.Form14.CheckSort);
        Assert.True(set.Form14.CheckVolume);
        Assert.True(set.Form14.CheckActivityMeasurementDate);
        Assert.True(set.Form14.CheckMass);
        Assert.False(set.Form14.CheckFactoryNumber);
        Assert.False(set.Form14.CheckQuantity);
        Assert.True(set.Form15.CheckQuantity);
        Assert.True(set.Form15.CheckActivity);
        Assert.False(set.Form15.CheckCreatorOkpo);
        Assert.True(set.Form15.CheckCreationDate);
    }

    [Fact]
    public void CheckAll14_DefaultsToTrue_AndUncheckingOneField_MakesIndeterminate()
    {
        var vm = new GetTransferReceiveParamsVM();
        Assert.True(vm.CheckAll14);
        Assert.True(vm.CheckSort14);
        Assert.True(vm.CheckVolume14);

        vm.CheckName14 = false;
        Assert.Null(vm.CheckAll14);
    }

    [Fact]
    public void DefaultForm14Params_IsEnabled_AndDisablesFactoryCreatorQuantity()
    {
        var p = ExcelExportCheckTransferReceiveAsyncCommand.DefaultForm14Params();
        Assert.True(ExcelExportCheckTransferReceiveAsyncCommand.IsFormCheckEnabled(p));
        Assert.False(p.CheckQuantity);
        Assert.False(p.CheckFactoryNumber);
        Assert.False(p.CheckCreatorOkpo);
        Assert.False(p.CheckCreationDate);
        Assert.True(p.CheckSort);
        Assert.True(p.CheckVolume);
        Assert.True(p.CheckActivityMeasurementDate);
        Assert.True(p.CheckMass);
        Assert.True(p.CheckAggregateState);
        Assert.True(p.CheckType);
    }

    [Fact]
    public void CheckAll15_DefaultsToTrue_AndUncheckingOneField_MakesIndeterminate()
    {
        var vm = new GetTransferReceiveParamsVM();
        Assert.True(vm.CheckAll15);
        Assert.True(vm.CheckQuantity15);

        vm.CheckType15 = false;
        Assert.Null(vm.CheckAll15);
    }

    [Fact]
    public void DefaultForm15Params_IsEnabled_AndDisablesCreatorOkpo()
    {
        var p = ExcelExportCheckTransferReceiveAsyncCommand.DefaultForm15Params();
        Assert.True(ExcelExportCheckTransferReceiveAsyncCommand.IsFormCheckEnabled(p));
        Assert.True(p.CheckQuantity);
        Assert.True(p.CheckActivity);
        Assert.True(p.CheckFactoryNumber);
        Assert.False(p.CheckCreatorOkpo);
        Assert.True(p.CheckCreationDate);
        Assert.True(p.CheckStatusRao);
        Assert.True(p.CheckPackName);
        Assert.True(p.CheckPackType);
        Assert.True(p.CheckPackNumber);
        Assert.True(p.CheckSubsidy);
        Assert.True(p.CheckFcpNumber);
    }

    [Fact]
    public void CheckAll16_DefaultsToTrue_AndUncheckingOneField_MakesIndeterminate()
    {
        var vm = new GetTransferReceiveParamsVM();
        Assert.True(vm.CheckAll16);
        Assert.True(vm.CheckCodeRao16);

        vm.CheckVolume16 = false;
        Assert.Null(vm.CheckAll16);
    }

    [Fact]
    public void DefaultForm16Params_IsEnabled_AndDisablesPassportTypeFactory()
    {
        var p = ExcelExportCheckTransferReceiveAsyncCommand.DefaultForm16Params();
        Assert.True(ExcelExportCheckTransferReceiveAsyncCommand.IsFormCheckEnabled(p));
        Assert.False(p.CheckPassportNumber);
        Assert.False(p.CheckType);
        Assert.False(p.CheckFactoryNumber);
        Assert.False(p.CheckActivity);
        Assert.False(p.CheckCreatorOkpo);
        Assert.False(p.CheckCreationDate);
        Assert.False(p.CheckPackName);
        Assert.True(p.CheckCodeRao);
        Assert.True(p.CheckTritiumActivity);
        Assert.True(p.CheckBetaGammaActivity);
        Assert.True(p.CheckAlphaActivity);
        Assert.True(p.CheckTransuraniumActivity);
        Assert.False(p.AllowEmptySerialQuantityDrain);
    }

    [Fact]
    public void OperationDateSearchToleranceDays_DefaultsTo15()
    {
        var vm = new GetTransferReceiveParamsVM();
        Assert.Equal(ExcelExportCheckTransferReceiveAsyncCommand.DefaultOperationDateSearchToleranceDays, vm.OperationDateSearchToleranceDays);
    }

    [Fact]
    public void OperationDateSearchToleranceDaysText_AcceptsDigitsOnly_AndCommitNormalizesEmpty()
    {
        var vm = new GetTransferReceiveParamsVM();
        vm.OperationDateSearchToleranceDaysText = "12a3";
        Assert.Equal("123", vm.OperationDateSearchToleranceDaysText);
        Assert.Equal(123, vm.OperationDateSearchToleranceDays);

        vm.OperationDateSearchToleranceDaysText = string.Empty;
        vm.CommitOperationDateSearchToleranceDays();
        Assert.Equal("15", vm.OperationDateSearchToleranceDaysText);
        Assert.Equal(15, vm.OperationDateSearchToleranceDays);
    }

    [Fact]
    public void MapParamsFromDialogVm_MapsOperationDateSearchToleranceDays_AndClamps()
    {
        var vm = new GetTransferReceiveParamsVM { OperationDateSearchToleranceDays = 30 };
        var set = TransferReceiveTestAccess.MapParamsFromDialogVmForTests(vm);
        Assert.Equal(30, set.OperationDateSearchToleranceDays);

        vm.OperationDateSearchToleranceDays = 999;
        set = TransferReceiveTestAccess.MapParamsFromDialogVmForTests(vm);
        Assert.Equal(ExcelExportCheckTransferReceiveAsyncCommand.TransferReceiveParamsSet.MaxOperationDateSearchToleranceDays, set.OperationDateSearchToleranceDays);
    }
}
