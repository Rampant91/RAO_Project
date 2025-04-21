using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client_App.ViewModels.Calculator;

namespace Client_App.Commands.AsyncCommands.Calculator;

public class CalculatorFilterAsyncCommand : BaseAsyncCommand
{
    private readonly ActivityCalculatorVM _activityCalculatorVM;

    public CalculatorFilterAsyncCommand(ActivityCalculatorVM activityCalculatorVM)
    {
        _activityCalculatorVM = activityCalculatorVM;
        _activityCalculatorVM.PropertyChanged += ActivityCalculatorVMPropertyChanged;
    }

    private void ActivityCalculatorVMPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ActivityCalculatorVM.Filter))
        {
            OnCanExecuteChanged();
        }
    }

    public override Task AsyncExecute(object? parameter)
    {
        if (_activityCalculatorVM.RadionuclidsFullList is not null)
        {
            _activityCalculatorVM.Radionuclids = new ObservableCollection<Radionuclid>(_activityCalculatorVM.RadionuclidsFullList
                .Where(x => 
                    x.Name.Contains(_activityCalculatorVM.Filter, StringComparison.OrdinalIgnoreCase) 
                    || x.Abbreviation.Contains(_activityCalculatorVM.Filter, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.CompletedTask;
    }
}