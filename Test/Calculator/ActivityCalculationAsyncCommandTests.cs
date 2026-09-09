using System.Collections.Generic;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.Calculator;
using Client_App.ViewModels.Calculator;
using Models.DTO;
using Xunit;

namespace Test.Calculator;

public class ActivityCalculationAsyncCommandTests
{
    [Fact]
    public async Task DateRange_WithoutNuclid_DoesNotThrow_AndClearsResult()
    {
        var vm = CreateVm();
        vm.IsDateRange = true;
        vm.InitialActivity = "1e+3";
        vm.InitialActivityDate = "01.01.2020";
        vm.ResidualActivityDate = "01.01.2021";
        vm.ResidualActivity = "stale";

        var cmd = new ActivityCalculationAsyncCommand(vm);
        await cmd.AsyncExecute(null);

        Assert.Equal(string.Empty, vm.ResidualActivity);
        Assert.False(vm.IsDateRangeTextVisible);
    }

    [Fact]
    public async Task DateRange_InvertedDates_ShowsWarning_WithoutNuclid()
    {
        var vm = CreateVm();
        vm.IsDateRange = true;
        vm.InitialActivity = "1e+3";
        vm.InitialActivityDate = "01.01.2022";
        vm.ResidualActivityDate = "01.01.2021";

        var cmd = new ActivityCalculationAsyncCommand(vm);
        await cmd.AsyncExecute(null);

        Assert.True(vm.IsDateRangeTextVisible);
        Assert.Equal(string.Empty, vm.ResidualActivity);
    }

    [Fact]
    public async Task DateRange_CompleteInputs_ComputesResidual()
    {
        var vm = CreateVm();
        vm.IsDateRange = true;
        vm.SelectedDictionaryNuclid = SampleNuclid(halflifeDays: 365);
        vm.InitialActivity = "1000";
        vm.InitialActivityDate = "01.01.2020";
        vm.ResidualActivityDate = "01.01.2021";

        var cmd = new ActivityCalculationAsyncCommand(vm);
        await cmd.AsyncExecute(null);

        Assert.False(string.IsNullOrWhiteSpace(vm.ResidualActivity));
        Assert.False(vm.IsDateRangeTextVisible);
    }

    [Fact]
    public async Task TimePeriod_WithoutNuclid_DoesNotThrow()
    {
        var vm = CreateVm();
        vm.IsDateRange = false;
        vm.InitialActivity = "1e+3";
        vm.TimePeriodDouble = "10";
        vm.SelectedTimeUnit = "сут";
        vm.ResidualActivity = "stale";

        var cmd = new ActivityCalculationAsyncCommand(vm);
        await cmd.AsyncExecute(null);

        Assert.Equal(string.Empty, vm.ResidualActivity);
    }

    private static ActivityCalculatorVM CreateVm() =>
        new(new List<CalculatorRadionuclidDTO>());

    private static CalculatorRadionuclidDTO SampleNuclid(double halflifeDays) =>
        new()
        {
            Name = "Test",
            Abbreviation = "T",
            Halflife = halflifeDays,
            Unit = "сут",
            D = "1",
            Mza = "1"
        };
}
