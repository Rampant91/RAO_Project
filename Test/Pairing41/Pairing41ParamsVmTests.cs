using Client_App.ViewModels.Messages;
using Xunit;

namespace Test.Pairing41;

/// <summary>
/// Диалог параметров: трёхсостояние CheckAll / CheckAll12–14
/// (все / ничего / частично).
/// </summary>
public class Pairing41ParamsVmTests
{
    /// <summary>По умолчанию все CheckAll = true; снятие одного поля → indeterminate.</summary>
    [Fact]
    public void CheckAll11_DefaultsToTrue_AndUncheckingOneField_MakesIndeterminate()
    {
        var vm = new GetPairingCode41ParamsVM();
        Assert.True(vm.CheckAll);
        Assert.True(vm.CheckAll12To16);
        Assert.True(vm.CheckAll13To16);
        Assert.True(vm.CheckAll14To16);

        vm.CheckType = false;
        Assert.Null(vm.CheckAll);
        Assert.True(vm.CheckAll12To16);
    }

    [Fact]
    public void CheckAll12_SetFalse_UnchecksAll12Fields_ThenSetTrue_Restores()
    {
        var vm = new GetPairingCode41ParamsVM();
        vm.CheckAll12To16 = false;
        Assert.False(vm.CheckMass12To16);
        Assert.False(vm.CheckDocumentNumber12To16);
        Assert.False(vm.CheckAll12To16);

        vm.CheckAll12To16 = true;
        Assert.True(vm.CheckMass12To16);
        Assert.True(vm.CheckDocumentNumber12To16);
        Assert.True(vm.CheckAll12To16);
    }

    [Fact]
    public void CheckAll13_And14_Indeterminate_WhenMixed()
    {
        var vm = new GetPairingCode41ParamsVM();
        vm.CheckMainRadionuclids13To16 = false;
        Assert.Null(vm.CheckAll13To16);

        vm.CheckVolume14To16 = false;
        Assert.Null(vm.CheckAll14To16);
        Assert.True(vm.CheckMass14To16);
    }
}
