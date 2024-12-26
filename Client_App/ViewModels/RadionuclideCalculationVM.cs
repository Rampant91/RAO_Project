using CommunityToolkit.Mvvm.ComponentModel;

namespace Client_App.ViewModels
{
    public class RadionuclideCalculationVM : ObservableObject
    {
        private string _radionuclideName;
        private string _radionuclideType;
        private string _radionuclideHalfLife;
        private string _radionuclideUnit;

        private int _initialActivity;
        private int _initialDateTime;
        private string _result;

        public string RadionuclideName
        {
            get => _radionuclideName;
            set => SetProperty(ref _radionuclideName, value);
        }
        public string RadionuclideType
        {
            get => _radionuclideType;
            set => SetProperty(ref _radionuclideType, value);
        }
        public string RadionuclideHalfLife
        {
            get => _radionuclideHalfLife;
            set => SetProperty(ref _radionuclideHalfLife, value);
        }
        public string RadionuclideUnit
        {
            get => _radionuclideUnit;
            set => SetProperty(ref _radionuclideUnit, value);
        }
        public int InitialActivity
        {
            get => _initialActivity;
            set => SetProperty(ref _initialActivity, value);
        }
        public int InitialDateTime
        {
            get => _initialDateTime;
            set => SetProperty(ref _initialDateTime, value);
        }
        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public RadionuclideCalculationVM(string radionuclideName, string codeNumber, string halfLife, string unit)
        {
            RadionuclideName = radionuclideName;
            RadionuclideType = codeNumber;
            RadionuclideHalfLife = halfLife;
            RadionuclideUnit = unit;
        }
    }
}