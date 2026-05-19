using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.SyncCommands;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms.Forms1;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Collections;

namespace Client_App.Views.Forms.Forms1;

public partial class Form_11 : BaseWindow<Form_11VM>
{
    private readonly Form_11VM _vm = null!;
    public Form_11VM? VM => DataContext as Form_11VM;

    private bool _isCloseConfirmed;
    protected override bool IsFullScreenWindow => true;

    private bool _isCtrlPressed;
    private bool _cKeyPressed;
    private bool _vKeyPressed;
    private bool _aKeyPressed;

    public Form_11()
    {
        InitializeComponent();
        _vm = new Form_11VM();
        DataContext = _vm;
        Show();
    }

    public Form_11(Form_11VM vm)
    {
        InitializeComponent();
        DataContext = vm;
        _vm = vm;
        Closing += OnStandardClosing;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void CopyExecutorData_Click(object sender, RoutedEventArgs e)
    {
        var command = new NewCopyExecutorDataAsyncCommand((Form_11VM)DataContext);
        if (command.CanExecute(null))
        {
            command.Execute(null);
        }
    }

    #region DataGrid_KeyDown

    private void DataGrid_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.F9 && DataContext is Form_11VM vm)
        {
            vm.ShowScrollPerfOverlay = !vm.ShowScrollPerfOverlay;
            e.Handled = true;
            return;
        }

        switch (e.Key)
        {
            case Key.LeftCtrl:
            case Key.RightCtrl:
                _isCtrlPressed = true;
                break;
            case Key.C:
                _cKeyPressed = true;
                break;
            case Key.V:
                _vKeyPressed = true;
                break;
            case Key.A:
                _aKeyPressed = true;
                break;
        }
    }

    #endregion

    #region DataGrid_KeyUp

    private void DataGrid_KeyUp(object? sender, KeyEventArgs e)
    {
        if (DataContext is not Form_11VM vm) return;

        if (e.Key == Key.F9)
        {
            vm.ShowScrollPerfOverlay = !vm.ShowScrollPerfOverlay;
            e.Handled = true;
            return;
        }

        var dataGrid = this.FindControl<DataGrid>("dataGrid");
        var dataContext = dataGrid?.DataContext;
        if (dataContext is null || dataGrid is null) return;

        var selectedForms = vm.SelectedForms;

        if (!dataGrid.IsPointerOver || !_isCtrlPressed) return;

        if (!vm.DataGridIsEditing)
        {
            if (_cKeyPressed || e.Key is Key.C)
            {
                _isCtrlPressed = false;
                _cKeyPressed = false;
                if (selectedForms is { Count: > 0 })
                {
                    vm.CopyRows.Execute(selectedForms);
                    e.Handled = true;
                }
                return;
            }
            else if (_vKeyPressed || e.Key is Key.V)
            {
                _isCtrlPressed = false;
                _vKeyPressed = false;
                if (selectedForms is { Count: > 0 })
                {
                    vm.PasteRows.Execute(selectedForms);
                    e.Handled = true;
                }
                return;
            }
            else if (_aKeyPressed || e.Key is Key.A)
            {
                _isCtrlPressed = false;
                _aKeyPressed = false;
                vm.SelectAll.Execute(null);
                e.Handled = true;
                return;
            }

        }

        switch (e.Key)
        {
            case Key.LeftCtrl:
            case Key.RightCtrl:
            {
                _isCtrlPressed = false;
                break;
            }
            case Key.T: // Add Row
            {
                vm.AddRow.Execute(null);
                e.Handled = true;

                break;
            }
            case Key.N: // Add N Rows
            {
                vm.AddRows.Execute(null);
                e.Handled = true;

                break;
            }
            case Key.I: // Add N Rows Before
            {
                if (selectedForms is { Count: > 0 })
                {
                    vm.AddRowsIn.Execute(selectedForms);
                    e.Handled = true;
                }

                break;
            }
            case Key.D: // Delete Selected Rows
            {
                if (selectedForms is { Count: > 0 })
                {
                    vm.DeleteRows.Execute(selectedForms);
                    e.Handled = true;
                }

                break;
            }
            case Key.O: // Set Number Order
            {
                vm.SetNumberOrder.Execute(null);
                e.Handled = true;

                break;
            }
            case Key.K: // Copy Pas Name
            {
                vm.CopyPasName.Execute(selectedForms);
                e.Handled = true;

                break;
            }
            case Key.P: // Open Pas
            {
                vm.OpenPas.Execute(selectedForms);
                e.Handled = true;

                break;
            }
            case Key.E: // Export Movement History
            {
                vm.ExcelExportSourceMovementHistory.Execute(selectedForms);
                e.Handled = true;

                break;
            }
            case Key.Y: // Calculate Category
            {
                vm.CategoryCalculationFromReport.Execute(selectedForms);
                e.Handled = true;

                break;
            }
            case Key.U: // Clear Rows
            {
                if (selectedForms is { Count: > 0 })
                {
                    vm.DeleteDataInRows.Execute(selectedForms);
                    e.Handled = true;
                }

                break;
            }
            case Key.J: // Source Transmission to RAO
            {
                if (vm.SelectedForm is not null)
                {
                    vm.SourceTransmission.Execute(vm.SelectedForm);
                    e.Handled = true;
                }

                break;
            }
        }
    }

    #endregion

}
