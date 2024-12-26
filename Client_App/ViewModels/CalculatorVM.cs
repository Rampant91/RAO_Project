using CommunityToolkit.Mvvm.ComponentModel;

namespace Client_App.ViewModels
{
    public class CalculatorVM : ObservableObject
    {
        private object _currentView;
        private string _windowTitleName;

        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }
        public string WindowTitleName
        {
            get => _windowTitleName;
            set => SetProperty(ref _windowTitleName, value);
        }
        
        private RadionuclideSelectionCalculatorVM RadionuclideSelectionVM { get; set; }

        public CalculatorVM()
        {
            WindowTitleName = "Калькулятор";

            RadionuclideSelectionVM = new RadionuclideSelectionCalculatorVM(this);

            CurrentView = RadionuclideSelectionVM;
        }
    }
}