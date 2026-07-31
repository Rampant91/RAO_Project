using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;
using Client_App.ViewModels.Messages;
using Xunit;

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
            vm.CheckOperationCode,
            vm.CheckOperationDate,
            vm.CheckPassportNumber,
            vm.CheckType,
            vm.CheckRadionuclids,
            vm.CheckFactoryNumber,
            vm.CheckQuantity,
            vm.CheckActivity,
            vm.CheckCreatorOkpo,
            vm.CheckCreationDate,
            vm.CheckProviderOrRecieverOkpo,
            vm.CheckPackNumber);
        Assert.False(ExcelExportCheckTransferReceiveAsyncCommand.IsFormCheckEnabled(form11));
        Assert.True(ExcelExportCheckTransferReceiveAsyncCommand.IsFormCheckEnabled(
            ExcelExportCheckTransferReceiveAsyncCommand.DefaultForm13Params()));
    }
}
