using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessageBox.Avalonia;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Client_App.ViewModels
{
    public class RadionuclideCalculationVM : ObservableObject
    {
        private string _radionuclideName;
        private string _radionuclideType;
        private string _radionuclideHalfLife;
        private string _radionuclideUnit;

        private double _initialActivity;
        private double _elapsedTime;
        private string _result;
        private TimeUnit _selectedTimeUnit;

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
        public double InitialActivity
        {
            get => _initialActivity;
            set => SetProperty(ref _initialActivity, value);
        }
        public double ElapsedTime
        {
            get => _elapsedTime;
            set => SetProperty(ref _elapsedTime, value);
        }
        public TimeUnit SelectedTimeUnit
        {
            get => _selectedTimeUnit;
            set => SetProperty(ref _selectedTimeUnit, value);
        }
        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }
        public ObservableCollection<TimeUnit> TimeUnits { get; } =
        [
            new TimeUnit { DisplayName = "минуты", Tag = "мин" },
            new TimeUnit { DisplayName = "часы", Tag = "час" },
            new TimeUnit { DisplayName = "сутки", Tag = "сут" },
            new TimeUnit { DisplayName = "года", Tag = "лет" }
        ];

        public RelayCommand CalculateCommand { get; }

        public RadionuclideCalculationVM(string radionuclideName, string codeNumber, string halfLife, string unit)
        {
            RadionuclideName = radionuclideName;
            RadionuclideType = codeNumber;
            RadionuclideHalfLife = halfLife;
            RadionuclideUnit = unit;

            CalculateCommand = new RelayCommand(CalculateActivity);
        }

        private void CalculateActivity()
        {
            try
            {
                if (SelectedTimeUnit != null || TimeUnits != null)
                {
                    // Преобразуем период полураспада в сутки
                    double halfLifeInDays = Convert.ToString(RadionuclideUnit.ToLower().Trim(), new CultureInfo("ru-RU")) switch
                    {
                        "мин" => double.Parse(RadionuclideHalfLife, new CultureInfo("ru-RU")) / (24 * 60),
                        "час" => double.Parse(RadionuclideHalfLife, new CultureInfo("ru-RU")) / 24,
                        "сут" => double.Parse(RadionuclideHalfLife, new CultureInfo("ru-RU")),
                        "лет" => double.Parse(RadionuclideHalfLife, new CultureInfo("ru-RU")) * 365,
                        _ => throw new ArgumentException("Недопустимые единицы периода полураспада")
                    };


                    // Преобразуем прошедшее время в дни
                    double elapsedTimeInDays = SelectedTimeUnit?.Tag?.ToLower().Trim() switch
                    {
                        "мин" => ElapsedTime / (24 * 60),
                        "час" => ElapsedTime / 24,
                        "сут" => ElapsedTime,
                        "лет" => ElapsedTime * 365,
                        _ => throw new ArgumentException("Недопустимые единицы времени")
                    };

                    // Рассчитываем оставшуюся активность
                    double decayConstant = Math.Log(2) / halfLifeInDays;
                    double remainingActivity = InitialActivity * Math.Exp(-decayConstant * elapsedTimeInDays);

                    var culture = new CultureInfo("ru-RU") { NumberFormat = { NumberDecimalDigits = 3 } };
                    Result = $"{remainingActivity.ToString("0.000e+00", culture)} Bq";
                }
                else
                {
                    var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", "Пожалуйста, укажите корректное время");
                    msBox.Show();
                }
            }
            catch (Exception ex)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", $"Возникла ошибка: {ex.Message}");
                msBox.Show();
            }
        }
    }

    public class TimeUnit
    {
        public string? DisplayName { get; set; }
        public string? Tag { get; set; }
    }
}