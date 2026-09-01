using Client_App.ViewModels.Forms.Forms4;
using System.Linq;
using Xunit;

namespace Test.ViewModels;

public class Form40SubjectRfBindingTests
{
    [Fact]
    public void CodeOfSubjectRF_SetValidCode_FillsSubjectNameFromDictionary()
    {
        var vm = new Form_40VM();

        vm.CodeOfSubjectRF = "77";

        Assert.Equal("77", vm.Storage.Rows40[0].CodeSubjectRF.Value);
        Assert.Equal("\u041c\u043e\u0441\u043a\u0432\u0430", vm.NameOfSubjectRF);
    }

    [Fact]
    public void CodeOfSubjectRF_SetSingleDigitCode_NormalizesAndFillsSubjectName()
    {
        var vm = new Form_40VM();

        vm.CodeOfSubjectRF = "1";

        Assert.Equal("01", vm.Storage.Rows40[0].CodeSubjectRF.Value);
        Assert.Equal("\u0420\u0435\u0441\u043f\u0443\u0431\u043b\u0438\u043a\u0430 \u0410\u0434\u044b\u0433\u0435\u044f (\u0410\u0434\u044b\u0433\u0435\u044f)", vm.NameOfSubjectRF);
    }

    [Fact]
    public void CodeOfSubjectRF_ClearCode_ClearsSubjectName()
    {
        var vm = new Form_40VM();
        vm.CodeOfSubjectRF = "77";

        vm.CodeOfSubjectRF = null;

        Assert.Equal(string.Empty, vm.Storage.Rows40[0].CodeSubjectRF.Value);
        Assert.Equal(string.Empty, vm.NameOfSubjectRF);
    }

    [Fact]
    public void SubjectRFItems_ContainCodeAndDescription()
    {
        var vm = new Form_40VM();

        var moscow = vm.SubjectRFItems.FirstOrDefault(x => x.Code == "77");
        Assert.NotNull(moscow);
        Assert.Equal("\u041c\u043e\u0441\u043a\u0432\u0430", moscow!.Description);
        Assert.Contains("77", moscow.ToString());
    }
}
