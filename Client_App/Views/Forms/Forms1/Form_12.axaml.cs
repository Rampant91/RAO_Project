using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.Commands.AsyncCommands.TmpNewCommands;
using Client_App.ViewModels.Forms.Forms1;
using Models.Forms.Form1;
using Avalonia.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace Client_App.Views.Forms.Forms1;

public partial class Form_12 : BaseWindow<Form_12VM>
{
    private Form_12VM _vm = null!;

    public Form_12() 
    {
        InitializeComponent();
        Show();
    }

    public Form_12(Form_12VM vm)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = vm;
        _vm = vm;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new Form_12VM();
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is Form_12VM vm && sender is DataGrid dataGrid)
        {
            var selectedItems = new List<Form12>();
            if (dataGrid.SelectedItems != null)
            {
                foreach (var item in dataGrid.SelectedItems)
                {
                    if (item is Form12 form12)
                    {
                        selectedItems.Add(form12);
                    }
                }
            }
            vm.SelectedForms = new ObservableCollection<Form12>(selectedItems);
        }
    }

    #region PaginationTextBoxValidation
    // ��������� ��� �������� ����������

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
        // ��������� ��������� ������� (Backspace, Delete, ������� � �.�.)
        if (IsControlKey(e))
        {
            return;
        }

        // ��������� ����� � �������� ���������� � ��������� �����
        if (IsDigitKey(e))
        {
            return;
        }

        // ��������� ��� ��������� �������
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
               e.KeyModifiers.HasFlag(KeyModifiers.Control); // ��������� Ctrl+C, Ctrl+V � �.�.
    }

    private bool IsDigitKey(KeyEventArgs e)
    {
        // ����� �� �������� ���������� (0-9)
        if (e.Key >= Key.D0 && e.Key <= Key.D9)
            return true;

        // ����� �� �������� ����� (NumPad0-NumPad9)
        if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            return true;

        return false;
    }
    #endregion
}