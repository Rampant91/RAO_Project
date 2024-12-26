using Avalonia.Controls;
using Client_App.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia;
using Models.DTO;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.ViewModels
{
    public class RadionuclideSelectionCalculatorVM : ObservableObject
    {
        private readonly CalculatorVM _calculator;

        private ObservableCollection<RadionuclidDTO> _allRadionuclidList;
        private ObservableCollection<RadionuclidDTO> _filteredRadionuclidList;
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    UpdateFilteredRadionuclides();
                }
            }
        }
        public ObservableCollection<RadionuclidDTO> FilteredRadionuclidList
        {
            get => _filteredRadionuclidList;
            private set => SetProperty(ref _filteredRadionuclidList, value);
        }
        public AsyncRelayCommand<object> SelectRadionuclidCommand { get; set; }

        public RadionuclideSelectionCalculatorVM(CalculatorVM calculator)
        {
            _calculator = calculator;

            _allRadionuclidList = [];
            FilteredRadionuclidList = [];

            SelectRadionuclidCommand = new AsyncRelayCommand<object>(SelectRadionuclid);

            LoadFromExcel();
        }

        private void LoadFromExcel()
        {
#if DEBUG
            string relativePath = @"C:\Users\shaih\source\repos\RAO_Project\data\Spravochniki\R.xlsx";
#else
string baseDirectory = AppContext.BaseDirectory;
string relativePath = Path.Combine(baseDirectory, "data", "Spravochniki", "R.xlsx");
#endif

            var parsedData = ExcelParser.ParseRadionuclides(relativePath);
            _allRadionuclidList = new ObservableCollection<RadionuclidDTO>(parsedData);
            UpdateFilteredRadionuclides();
        }
        private void UpdateFilteredRadionuclides()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                FilteredRadionuclidList = new ObservableCollection<RadionuclidDTO>(_allRadionuclidList);
            }
            else
            {
                var filtered = _allRadionuclidList.Where(r => r.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                                r.CodeNumber.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)).ToList();

                FilteredRadionuclidList = new ObservableCollection<RadionuclidDTO>(filtered);
            }
        }
        private async Task SelectRadionuclid(object parameter)
        {
            if(parameter is RadionuclidDTO currentRadionuclid)
            {
                _calculator.CurrentView = new RadionuclideCalculationVM(currentRadionuclid.Name, currentRadionuclid.CodeNumber, currentRadionuclid.HalfLife!, currentRadionuclid.Unit!);
            }
        }
    }
}