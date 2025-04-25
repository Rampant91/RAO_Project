using Client_App.Commands.AsyncCommands.Calculator;
using Models.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client_App.ViewModels.Calculator;

public class CategoryCalculatorVM : BaseCalculatorVM
{
    public CategoryCalculatorVM() { }

    public CategoryCalculatorVM(List<CalculatorRadionuclidDTO> radionuclids)
    {
        Radionuclids = new ObservableCollection<CalculatorRadionuclidDTO>(radionuclids);
        RadionuclidsFullList = [.. Radionuclids];

        FilterCommand = new CalculatorFilterAsyncCommand(this);
    }
}