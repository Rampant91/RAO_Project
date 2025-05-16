using System;
using System.Linq;
using System.Threading.Tasks;
using Client_App.ViewModels.Calculator;

namespace Client_App.Commands.AsyncCommands.Calculator;

public class CalculatorFilterAsyncCommand : BaseAsyncCommand
{
    private readonly BaseCalculatorVM _baseCalculatorVM;

    public CalculatorFilterAsyncCommand(BaseCalculatorVM baseCalculatorVM)
    {
        _baseCalculatorVM = baseCalculatorVM;
        _baseCalculatorVM.PropertyChanged += BaseCalculatorVMPropertyChanged;
    }

    private void BaseCalculatorVMPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ActivityCalculatorVM.Filter))
        {
            OnCanExecuteChanged();
        }
    }

    public override Task AsyncExecute(object? parameter)
    {
        if (_baseCalculatorVM.RadionuclidsFullList is not null)
        {
            _baseCalculatorVM.RadionuclidDictionary = [.. _baseCalculatorVM.RadionuclidsFullList
                .Where(x => _baseCalculatorVM.Filter != null 
                            && (x.Name.Contains(_baseCalculatorVM.Filter, StringComparison.OrdinalIgnoreCase) 
                                || x.Abbreviation.Contains(_baseCalculatorVM.Filter, StringComparison.OrdinalIgnoreCase)))];
        }
        return Task.CompletedTask;
    }
}