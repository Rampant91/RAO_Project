using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.Commands.AsyncCommands;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client_App;

public partial class Form_12 : BaseWindow<Form_12VM>
{

    //private Form_12VM _vm = null!;

    public Form_12() 
    {
        InitializeComponent();
        Show();
    }

    public Form_12(Form_12VM vm)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = vm;
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new Form_12VM();
    }

    //Временное узкоспециализированное решение
    private void CopyExecutorData_Click(object sender, RoutedEventArgs e)
    {
        var command = new NewCopyExecutorDataAsyncCommand((Form_12VM)DataContext);
        if (command.CanExecute(null))
        {
            command.Execute(null);
        }
    }

    #region PaginationTextBoxValidation
    // Валидация для контроля паджинации

    private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
    private static bool IsTextAllowed(string text)
    {
        return !_regex.IsMatch(text);
    }
    private void TextBox_TextInput(object sender, TextInputEventArgs e)
    {
        e.Handled = !IsTextAllowed(e.Text);
    }
    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Разрешаем служебные клавиши (Backspace, Delete, стрелки и т.д.)
        if (IsControlKey(e))
        {
            return;
        }

        // Разрешаем цифры с основной клавиатуры и цифрового блока
        if (IsDigitKey(e))
        {
            return;
        }

        // Блокируем все остальные клавиши
        e.Handled = true;
    }

    private bool IsControlKey(KeyEventArgs e)
    {
        return e.Key == Key.Back ||
               e.Key == Key.Delete ||
               e.Key == Key.Left ||
               e.Key == Key.Right ||
               e.Key == Key.Home ||
               e.Key == Key.End ||
               e.Key == Key.Tab ||
               e.Key == Key.Enter ||
               e.Key == Key.Escape ||
               e.Key == Key.CapsLock ||
               e.Key == Key.PageUp ||
               e.Key == Key.PageDown ||
               e.KeyModifiers.HasFlag(KeyModifiers.Control); // Разрешаем Ctrl+C, Ctrl+V и т.д.
    }

    private bool IsDigitKey(KeyEventArgs e)
    {
        // Цифры на основной клавиатуре (0-9)
        if (e.Key >= Key.D0 && e.Key <= Key.D9)
            return true;

        // Цифры на цифровом блоке (NumPad0-NumPad9)
        if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            return true;

        return false;
    }
    #endregion
}