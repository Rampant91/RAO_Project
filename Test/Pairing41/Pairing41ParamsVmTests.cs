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

    /// <summary>CheckCodeRao12To16 по умолчанию true и участвует в трёхсостоянии CheckAll12To16.</summary>
    [Fact]
    public void CheckCodeRao12To16_DefaultsToTrue_AndUncheckingIt_MakesCheckAllIndeterminate()
    {
        var vm = new GetPairingCode41ParamsVM();
        Assert.True(vm.CheckCodeRao12To16);
        Assert.True(vm.CheckAll12To16);

        vm.CheckCodeRao12To16 = false;
        Assert.Null(vm.CheckAll12To16);

        vm.CheckAll12To16 = true;
        Assert.True(vm.CheckCodeRao12To16);
        Assert.True(vm.CheckAll12To16);
    }

    /// <summary>CheckCodeRao13To16 по умолчанию true и участвует в трёхсостоянии CheckAll13To16.</summary>
    [Fact]
    public void CheckCodeRao13To16_DefaultsToTrue_AndUncheckingIt_MakesCheckAllIndeterminate()
    {
        var vm = new GetPairingCode41ParamsVM();
        Assert.True(vm.CheckCodeRao13To16);
        Assert.True(vm.CheckAll13To16);

        vm.CheckCodeRao13To16 = false;
        Assert.Null(vm.CheckAll13To16);

        vm.CheckAll13To16 = false;
        Assert.False(vm.CheckCodeRao13To16);
        Assert.False(vm.CheckMainRadionuclids13To16);
        Assert.False(vm.CheckAll13To16);
    }

    /// <summary>CheckCodeRao14To16 по умолчанию true и участвует в трёхсостоянии CheckAll14To16.</summary>
    [Fact]
    public void CheckCodeRao14To16_DefaultsToTrue_AndUncheckingIt_MakesCheckAllIndeterminate()
    {
        var vm = new GetPairingCode41ParamsVM();
        Assert.True(vm.CheckCodeRao14To16);
        Assert.True(vm.CheckAll14To16);

        vm.CheckCodeRao14To16 = false;
        Assert.Null(vm.CheckAll14To16);

        vm.CheckAll14To16 = true;
        Assert.True(vm.CheckCodeRao14To16);
        Assert.True(vm.CheckAll14To16);
    }

    /// <summary>Снятие всех остальных полей 12To16, кроме CheckCodeRao — CheckAll12To16 остаётся indeterminate, а не false.</summary>
    [Fact]
    public void CheckAll12To16_Indeterminate_WhenOnlyCodeRaoRemainsChecked()
    {
        var vm = new GetPairingCode41ParamsVM
        {
            CheckOperationDate12To16 = false,
            CheckMass12To16 = false,
            CheckBetaGammaActivity12To16 = false,
            CheckAlphaActivity12To16 = false,
            CheckActivityMeasurementDate12To16 = false,
            CheckDocumentVid12To16 = false,
            CheckDocumentNumber12To16 = false,
            CheckDocumentDate12To16 = false,
            CheckPackName12To16 = false,
            CheckPackType12To16 = false,
            CheckPackNumber12To16 = false
        };

        Assert.True(vm.CheckCodeRao12To16);
        Assert.Null(vm.CheckAll12To16);
    }
}
