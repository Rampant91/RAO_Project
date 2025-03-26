using CommunityToolkit.Mvvm.ComponentModel;

namespace Client_App.ViewModels
{
    public class CalculatorVM : ObservableObject
    {
        #region Properties

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        private string _windowTitleName;
        public string WindowTitleName
        {
            get => _windowTitleName;
            set => SetProperty(ref _windowTitleName, value);
        }

        #endregion

        private RadionuclideSelectionCalculatorVM RadionuclideSelectionVM { get; set; }

        public CalculatorVM()
        {
            WindowTitleName = "Калькулятор";

            RadionuclideSelectionVM = new RadionuclideSelectionCalculatorVM(this);

            CurrentView = RadionuclideSelectionVM;
        }
    }
}