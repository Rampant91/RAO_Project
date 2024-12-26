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
        #region Properties
        private string _radionuclideName;
        private string _radionuclideType;
        private string _radionuclideHalfLife;
        private string _radionuclideUnit;
        private bool _visibleResult = false;
        private bool _visibleBorderOne = false;
        private bool _visibleBorderTwo = false;
        private DateTimeOffset? _startDate;
        private DateTimeOffset? _endDate;

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
        public bool VisibleResult
        {
            get => _visibleResult;
            set => SetProperty(ref _visibleResult, value);
        }
        public bool VisibleBorderOne
        {
            get => _visibleBorderOne;
            set => SetProperty(ref _visibleBorderOne, value);
        }
        public bool VisibleBorderTwo
        {
            get => _visibleBorderTwo;
            set => SetProperty(ref _visibleBorderTwo, value);
        }
        public DateTimeOffset? StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }
        public DateTimeOffset? EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public ObservableCollection<TimeUnit> TimeUnits { get; } =
        [
            new TimeUnit { DisplayName = "минуты", Tag = "мин" },
            new TimeUnit { DisplayName = "часы", Tag = "час" },
            new TimeUnit { DisplayName = "сутки", Tag = "сут" },
            new TimeUnit { DisplayName = "года", Tag = "лет" }
        ];
        #endregion

        #region RelayCommand
        public RelayCommand CalculateOneCommand { get; }
        public RelayCommand CalculateTwoCommand { get; }
        public RelayCommand BorderOneVisibleCommand { get; set; }
        public RelayCommand BorderTwoVisibleCommand { get; set; }
        #endregion

        public RadionuclideCalculationVM(string radionuclideName, string codeNumber, string halfLife, string unit)
        {
            RadionuclideName = radionuclideName;
            RadionuclideType = codeNumber;
            RadionuclideHalfLife = halfLife;
            RadionuclideUnit = unit;

            CalculateOneCommand = new RelayCommand(CalculateActivity);
            CalculateTwoCommand = new RelayCommand(CalculateActivityByDates);
            BorderOneVisibleCommand = new RelayCommand(() =>
            {
                VisibleBorderOne = true;
                VisibleBorderTwo = false;
            });
            BorderTwoVisibleCommand = new RelayCommand(() =>
            {
                VisibleBorderOne = false;
                VisibleBorderTwo = true;
            });

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
                    VisibleResult = true;
                }
                else
                {
                    var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", "Пожалуйста, укажите корректное время");
                    msBox.Show();
                }
            }
            catch (FormatException)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", "Ошибка формата данных. Проверьте введенные значения.");
                msBox.Show();
            }
            catch (OverflowException)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", "Число слишком велико или мало для обработки. Проверьте введенные значения.");
                msBox.Show();
            }
            catch (ArgumentNullException ex)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", $"Отсутствуют необходимые данные: {ex.Message}");
                msBox.Show();
            }
            catch (ArgumentException ex)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", $"Ошибка аргумента: {ex.Message}");
                msBox.Show();
            }
            catch (Exception ex)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", $"Возникла непредвиденная ошибка: {ex.Message}");
                msBox.Show();
            }
        }
        private void CalculateActivityByDates()
        {
            try
            {
                if (StartDate == null || EndDate == null || StartDate > EndDate)
                {
                    var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", "Пожалуйста, укажите корректные даты");
                    msBox.Show();
                    return;
                }

                // Преобразуем период полураспада в дни
                double halfLifeInDays = Convert.ToString(RadionuclideUnit.ToLower().Trim(), new CultureInfo("ru-RU")) switch
                {
                    "мин" => double.Parse(RadionuclideHalfLife, new CultureInfo("ru-RU")) / (24 * 60),
                    "час" => double.Parse(RadionuclideHalfLife, new CultureInfo("ru-RU")) / 24,
                    "сут" => double.Parse(RadionuclideHalfLife, new CultureInfo("ru-RU")),
                    "лет" => double.Parse(RadionuclideHalfLife, new CultureInfo("ru-RU")) * 365,
                    _ => throw new ArgumentException("Недопустимые единицы периода полураспада")
                };

                // Рассчитываем разницу в днях между датами
                double elapsedTimeInDays = (EndDate - StartDate).Value.TotalDays;

                // Рассчитываем оставшуюся активность
                double decayConstant = Math.Log(2) / halfLifeInDays;
                double remainingActivity = InitialActivity * Math.Exp(-decayConstant * elapsedTimeInDays);

                var culture = new CultureInfo("ru-RU") { NumberFormat = { NumberDecimalDigits = 3 } };
                Result = $"{remainingActivity.ToString("0.000e+00", culture)} Bq";
                VisibleResult = true;
            }
            catch (FormatException)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", "Ошибка формата данных. Проверьте введенные значения.");
                msBox.Show();
            }
            catch (OverflowException)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", "Число слишком велико или мало для обработки. Проверьте введенные значения.");
                msBox.Show();
            }
            catch (ArgumentNullException ex)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", $"Отсутствуют необходимые данные: {ex.Message}");
                msBox.Show();
            }
            catch (ArgumentException ex)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", $"Ошибка аргумента: {ex.Message}");
                msBox.Show();
            }
            catch (Exception ex)
            {
                var msBox = MessageBoxManager.GetMessageBoxStandardWindow("Калькулятор", $"Возникла непредвиденная ошибка: {ex.Message}");
                msBox.Show();
            }
        }
    }

    public class TimeUnit //По какой-то причине Combobox не хочет мне возвращать выбранное значение корректно, поэтому заполняю его через DTO
    {
        public string? DisplayName { get; set; }
        public string? Tag { get; set; }
    }
}