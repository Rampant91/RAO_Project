using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Client_App.Properties.ColumnWidthSettings;
using Client_App.ViewModels.Forms;
using DynamicData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Behaviors
{
    public class DataGridColumnWidthLoadBehavior : Behavior<DataGrid>
    {
        public static readonly AttachedProperty<string?> FormNumProperty =
        AvaloniaProperty.RegisterAttached<DataGridColumnWidthLoadBehavior, DataGrid, string?>("FormNum");

        public string? FormNum
        {
            get => GetValue(FormNumProperty);
            set => SetValue(FormNumProperty, value);
        }

        private List<double> columnWidths = new();

        protected override void OnAttached()
        {

            base.OnAttached();
            AssociatedObject.Initialized += AssociatedObject_Initialized;
        }

        private void AssociatedObject_Initialized(object? sender, EventArgs e)
        {
            // Восстановление ширины колонок
            columnWidths = ColumnSettingsManager.LoadSettings(FormNum);

            var columns = AssociatedObject.Columns;

            for (int i = 0; i < columnWidths.Count || i<columnWidths.Count; i++)
            {
                columns[i].Width = new DataGridLength(columnWidths[i]);
            }
        }

        protected override void OnDetaching()
        {

            OnClosing();

            AssociatedObject.Initialized -= AssociatedObject_Initialized;
            base.OnDetaching();
        }

        private void OnClosing()
        {
            var columns = AssociatedObject.Columns;
            // Сохранение ширины колонок
            columnWidths.Clear();
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].Width.IsAbsolute)
                    columnWidths.Add(columns[i].Width.Value);
                else
                    columnWidths.Add(AssociatedObject.ColumnWidth.Value);
            }
            ColumnSettingsManager.SaveSettings(columnWidths, FormNum);
        }
    }
}
