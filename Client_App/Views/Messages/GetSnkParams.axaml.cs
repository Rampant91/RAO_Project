using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using Client_App.ViewModels.Messages;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia.Controls.Templates;

namespace Client_App.Views.Messages;

public partial class GetSnkParams : BaseWindow<GetSnkParamsVM>
{
    public GetSnkParamsVM _vm = null!;

    #region InitializeComponent

    public GetSnkParams()
    {
        InitializeComponent();

        var regionBox = this.FindControl<TextBox>("RegionBox");
        regionBox?.AddHandler(TextInputEvent, (_, e) =>
        {
            if (e.Text != null && !char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }, RoutingStrategies.Tunnel);

        // Ищем TextBox в CalendarDatePicker после открытия окна
        Opened += (_, _) =>
        {
            var dateCalendar = this.FindControl<CalendarDatePicker>("DateCalendar");
            if (dateCalendar == null) return;

            // Принудительно применяем шаблон
            dateCalendar.ApplyTemplate();
                
            // Ищем TextBox с задержкой
            var timer = new Avalonia.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
                
            timer.Tick += (_, _) =>
            {
                // Ищем TextBox в шаблоне CalendarDatePicker
                var textBox = dateCalendar.GetTemplateChildren()
                    .OfType<TextBox>()
                    .FirstOrDefault();
                    
                if (textBox != null)
                {
                    // Удаляем старые обработчики если есть
                    textBox.RemoveHandler(TextInputEvent, OnDateTextInput);
                    textBox.RemoveHandler(KeyDownEvent, OnDateKeyDown);
                        
                    // Добавляем новые обработчики
                    textBox.AddHandler(TextInputEvent, OnDateTextInput, RoutingStrategies.Tunnel);
                    textBox.AddHandler(KeyDownEvent, OnDateKeyDown, RoutingStrategies.Tunnel);
                        
                    timer.Stop();
                }
            };
                
            timer.Start();
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new GetSnkParamsVM();
        _vm = (DataContext as GetSnkParamsVM)!;
    }

    #endregion

    #region ButtonsAndCheckBoxes

    private void OkButtonClick(object? sender, RoutedEventArgs e)
    {
        _vm.Ok = true;
        Close();
    }

    private void CancelButtonClick(object? sender, RoutedEventArgs e)
    {
        _vm.Ok = false;  // Явно устанавливаем false при отмене
        Close();
    }

    private void AllCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckAll = _vm.CheckPasNum = _vm.CheckType = _vm.CheckRadionuclids = _vm.CheckFacNum = _vm.CheckPackNumber = true;
        }
    }

    private void AllCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        if (_vm != null)
        {
            _vm.CheckPasNum = _vm.CheckType = _vm.CheckRadionuclids = _vm.CheckFacNum = _vm.CheckPackNumber = false;
        }
    }

    private void AnyCheckBox_Clicked(object sender, RoutedEventArgs e)
    {
        var allCheckBox = (sender as Control).FindNameScope().Find("All") as CheckBox;
        allCheckBox!.IsThreeState = true;
        _vm.CheckAll = _vm switch
        {
            { CheckPasNum: true, CheckType: true, CheckRadionuclids: true, CheckFacNum: true, CheckPackNumber: true } => true,
            { CheckPasNum: false, CheckType: false, CheckRadionuclids: false, CheckFacNum: false, CheckPackNumber: false } => false,
            _ => allCheckBox.IsChecked = null
        };
    }

    #endregion

    #region DateInputHandlers

    private static void OnDateTextInput(object? sender, TextInputEventArgs e)
    {
        if (e.Text == null) return;

        // Разрешаем только цифры и точки
        if (!char.IsDigit(e.Text, 0) && e.Text != ".")
        {
            e.Handled = true;
            return;
        }

        // Проверяем корректность формата
        if (sender is not TextBox textBox) return;

        var currentText = textBox.Text ?? "";
        var selectionStart = textBox.SelectionStart;
        var selectionEnd = textBox.SelectionEnd;
            
        string newText;
            
        // Если есть выделение, заменяем его новым символом
        if (selectionStart != selectionEnd)
        {
            // Исправляем порядок выделения (справа налево или слева направо)
            var start = Math.Min(selectionStart, selectionEnd);
            var end = Math.Max(selectionStart, selectionEnd);
                
            newText = currentText[..start] + e.Text + currentText[end..];
        }
        else
        {
            newText = currentText.Insert(selectionStart, e.Text);
        }
                
        if (!IsValidDateInput(newText))
        {
            e.Handled = true;
        }
    }

    private static void OnDateKeyDown(object? sender, KeyEventArgs e)
    {
        // Разрешаем Backspace, Delete, Tab, Enter, стрелки
        if (e.Key is Key.Back or Key.Delete or Key.Tab or Key.Enter or Key.Left or Key.Right or Key.Home or Key.End)
            return;

        // Разрешаем точку (пусть TextInput обрабатывает английскую)
        if (e.Key is Key.OemPeriod or Key.Oem2)
            return;

        // Разрешаем только цифры
        if (e.Key is >= Key.D0 and <= Key.D9 or >= Key.NumPad0 and <= Key.NumPad9)
            return;

        e.Handled = true;
    }

    private static bool IsValidDateInput(string input)
    {
        if (string.IsNullOrEmpty(input)) return true;

        // Проверяем, что строка соответствует формату дд.мм.гггг
        if (!DateRegex().IsMatch(input)) return false;

        // Проверяем диапазоны значений
        var parts = input.Split('.');
        
        // День - проверяем только если введено полное значение (2 цифры)
        if (parts is [{ Length: 2 }, ..])
        {
            if (int.TryParse(parts[0], out var day) && day is < 1 or > 31)
                return false;
        }
        
        // Месяц - проверяем только если введено полное значение (2 цифры)
        if (parts is [_, { Length: 2 }, ..])
        {
            if (int.TryParse(parts[1], out var month) && month is < 1 or > 12)
                return false;
        }
        
        // Год - проверяем только если введено полное значение (4 цифры)
        if (parts is [_, _, { Length: 4 }, ..])
        {
            if (int.TryParse(parts[2], out var year) && year is < 1900 or > 2100)
                return false;
        }

        return true;
    }

    [GeneratedRegex(@"^[0-9]{0,2}(\.[0-9]{0,2}(\.[0-9]{0,4})?)?$")]
    private static partial Regex DateRegex();

    #endregion
}