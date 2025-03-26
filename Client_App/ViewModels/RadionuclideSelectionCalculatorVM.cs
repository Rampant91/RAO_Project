using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.ViewModels;

public class RadionuclideSelectionCalculatorVM : ObservableObject
{
    #region Properties

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

    private ObservableCollection<RadionuclidDTO> FilteredRadionuclidList
    {
        get => _filteredRadionuclidList;
        set => SetProperty(ref _filteredRadionuclidList, value);
    }

    #endregion

    public AsyncRelayCommand<object> SelectRadionuclidCommand { get; set; }

    public RadionuclideSelectionCalculatorVM(){}

    public RadionuclideSelectionCalculatorVM(CalculatorVM calculator)
    {
        _calculator = calculator;

        _allRadionuclidList = [];
        FilteredRadionuclidList = [];

        SelectRadionuclidCommand = new AsyncRelayCommand<object>(SelectRadionuclid);

        LoadFromExcel();
    }

    #region LoadFromExcel
    
    /// <summary>
    /// Загрузка данных из excel и заполнение коллекции
    /// </summary>
    private void LoadFromExcel()
    {
#if DEBUG
        var relativePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx");
#else
        var relativePath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"R.xlsx");
#endif

        var parsedData = ParseRadionuclides(relativePath);
        _allRadionuclidList = new ObservableCollection<RadionuclidDTO>(parsedData);
        UpdateFilteredRadionuclides();
    }

    #endregion

    #region ExcelParser

    /// <summary>
    /// Парсит данные о радионуклидах из указанного Excel-файла
    /// </summary>
    /// <param name="filePath">Путь к Excel-файлу</param>
    private static List<RadionuclidDTO> ParseRadionuclides(string filePath)
    {
        var radionuclides = new List<RadionuclidDTO>();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage(new FileInfo(filePath));
        var worksheet = package.Workbook.Worksheets[0]; // Получение первого листа
        if (worksheet?.Dimension == null) return radionuclides; // Если лист пустой, возвращаем пустой список

        // Создаем список для хранения данных текущего листа
        var localList = new List<RadionuclidDTO>();

        // Обработка строк на листе (начиная со второй строки, так как первая — заголовки)
        for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            var name = worksheet.Cells[row, 1].Text?.Trim(); // Чтение имени радионуклида из первой колонки
            var codeNumber = worksheet.Cells[row, 4].Text?.Trim(); // Чтение кодового номера из четвертой колонки

            // Пропускаем строки с пустыми значениями
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(codeNumber))
                continue;

            var halfLife = worksheet.Cells[row, 5].Text?.Trim(); // Чтение периода полураспада из пятой колонки
            var unit = worksheet.Cells[row, 6].Text?.Trim(); // Чтение единицы измерения из шестой колонки

            // Создание объекта RadionuclidDTO и добавление в локальный список
            var radionuclid = new RadionuclidDTO(name, codeNumber, halfLife, unit);

            localList.Add(radionuclid);
        }

        // Добавляем все элементы из временного списка в основной
        radionuclides.AddRange(localList);

        return radionuclides;
    }

    #endregion

    #region SelectRadionuclid

    /// <summary>
    /// Проверка, является ли выбранный элемент RadionuclidDTO.
    /// Если да, то передаем его и устанавливаем в Calculator window свойство CurrentView на новый UserControl.
    /// </summary>
    /// <param name="parameter">DTO радионуклида.</param>
    /// <returns>CompletedTask.</returns>
    private Task SelectRadionuclid(object parameter)
    {
        if (parameter is RadionuclidDTO currentRadionuclid)
        {
            _calculator.CurrentView = new RadionuclideCalculationVM(
                currentRadionuclid.Name, currentRadionuclid.CodeNumber, currentRadionuclid.HalfLife!, currentRadionuclid.Unit!);
        }
        return Task.CompletedTask;
    }

    #endregion

    #region UpdateFilteredRadionuclides

    /// <summary>
    /// Фильтрация заполненной коллекции по определённым параметрам.
    /// </summary>
    private void UpdateFilteredRadionuclides()
    {
        if (string.IsNullOrEmpty(SearchText))
        {
            FilteredRadionuclidList = [.. _allRadionuclidList];
        }
        else
        {
            var filtered = _allRadionuclidList
                .Where(r => r.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                            || r.CodeNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            FilteredRadionuclidList = new ObservableCollection<RadionuclidDTO>(filtered);
        }
    }

    #endregion

    #region DTO

    private class RadionuclidDTO(string name, string codeNumber, string? halfLife, string? unit)
    {
        public string Name { get; } = name;
        public string CodeNumber { get; } = codeNumber;
        public string? HalfLife { get; } = halfLife;
        public string? Unit { get; } = unit;
    }

    #endregion
}