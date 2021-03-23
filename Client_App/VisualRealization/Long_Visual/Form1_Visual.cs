using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Models.Storage;
using Avalonia.Data;
using Avalonia;
using Models.Attributes;
using Avalonia.Media;
using Avalonia.Controls.Templates;
using Models.Client_Model;
using Avalonia.Data.Converters;

namespace Client_App.Long_Visual
{
    public class Form1_Visual
    {
        //Форма 10
        public static DataGrid Form10_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static Grid Form11_Visual()
        {
            Grid maingrid = new Grid();
            var row = new RowDefinition();
            row.Height = new GridLength(0.5,GridUnitType.Star);
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition();
            row.Height = new GridLength(0.7, GridUnitType.Star);
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition();
            row.Height = new GridLength(5, GridUnitType.Star);
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition();
            row.Height = new GridLength(2, GridUnitType.Star);
            maingrid.RowDefinitions.Add(row);

            var topPnl1 = new Grid();
            var column = new ColumnDefinition();
            column.Width = new GridLength(0.3, GridUnitType.Star);
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            column.Width = new GridLength(1, GridUnitType.Star);
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            column.Width = new GridLength(1, GridUnitType.Star);
            topPnl1.ColumnDefinitions.Add(column);
            topPnl1.SetValue(Grid.RowProperty,0);
            topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl1.Children.Add(new TextBlock
            {
                Height = 30,
                Margin = Thickness.Parse("5,12,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Text = "Период:",
                [Grid.ColumnProperty] = 0,
            });
            topPnl1.Children.Add(new DatePicker
            {
                Height = 30,
                Margin = Thickness.Parse("5,0,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment=Avalonia.Layout.HorizontalAlignment.Center,
                [!DatePicker.SelectedDateProperty] = new Binding("StartPeriod", BindingMode.TwoWay),
                [Grid.ColumnProperty] = 1,

            });
            topPnl1.Children.Add(new DatePicker
            {
                Height = 30,
                Margin = Thickness.Parse("5,0,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                [!DatePicker.SelectedDateProperty] = new Binding("EndPeriod", BindingMode.TwoWay),
                [Grid.ColumnProperty] = 2,

            });
            maingrid.Children.Add(topPnl1);

            var topPnl2 = new Grid();
            column = new ColumnDefinition();
            column.Width = new GridLength(0.3, GridUnitType.Star);
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            column.Width = new GridLength(1, GridUnitType.Star);
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl2.SetValue(Grid.RowProperty, 1);
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

            topPnl2.Children.Add(new TextBlock
            {
                Height = 30,
                Margin = Thickness.Parse("5,12,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Text = "Номер корректировки:",
                [Grid.ColumnProperty] = 0,
            });

            topPnl2.Children.Add(new TextBox
            {
                Height = 30,
                Width=300,
                Margin = Thickness.Parse("5,12,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                [!TextBox.TextProperty] = new Binding("CorrectionNumber"),
                [Grid.ColumnProperty] = 1,
            }) ;

            maingrid.Children.Add(topPnl2);

            DataGrid grd = new DataGrid();
            maingrid.Children.Add(grd);
            grd.SetValue(Grid.RowProperty,2);
            grd.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            grd.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            grd.Bind(DataGrid.ItemsProperty, new Binding("Storage.GetFilteredStorage"));

            DataGridTemplateColumn clm0 = new DataGridTemplateColumn();
            clm0.Width = new DataGridLength(0.1, DataGridLengthUnitType.Star);
            clm0.Header = new TextBlock
            {

            };
            clm0.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBlock
                    {

                    });
            grd.Columns.Add(clm0);

            DataGridTemplateColumn clm1 = new DataGridTemplateColumn();
            clm1.Width = new DataGridLength(3, DataGridLengthUnitType.Star);
            clm1.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((FormVisualAttribute)Type.GetType("Models.Client_Model.Form11,Models").
                GetProperty("NumberInOrder").GetCustomAttributes(typeof(FormVisualAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "NumberInOrder"
            };
            clm1.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("NumberInOrder"),
                    }) ;
            grd.Columns.Add(clm1);

            DataGridTemplateColumn clm2 = new DataGridTemplateColumn();
            clm2.Width = new DataGridLength(3, DataGridLengthUnitType.Star);
            clm2.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((FormVisualAttribute)Type.GetType("Models.Client_Model.Form11,Models").
                GetProperty("OperationCode").GetCustomAttributes(typeof(FormVisualAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "OperationCode"
            };
            clm2.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("OperationCode"),
                    });
            grd.Columns.Add(clm2);

            DataGridTemplateColumn clm3 = new DataGridTemplateColumn();
            clm3.Width = new DataGridLength(3, DataGridLengthUnitType.Star);
            clm3.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((FormVisualAttribute)Type.GetType("Models.Client_Model.Form11,Models").
                GetProperty("OperationDate").GetCustomAttributes(typeof(FormVisualAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "OperationDate"
            };
            clm3.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new DatePicker
                    {
                        Height = 30,
                        Margin = Thickness.Parse("0,-4,0,0"),
                        [!DatePicker.SelectedDateProperty] = new Binding("OperationDate", BindingMode.TwoWay),

                    }) ;
            grd.Columns.Add(clm3);

            DataGridTemplateColumn clm4 = new DataGridTemplateColumn();
            clm4.Width = new DataGridLength(3, DataGridLengthUnitType.Star);
            clm4.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((FormVisualAttribute)Type.GetType("Models.Client_Model.Form11,Models").
                GetProperty("PassportNumber").GetCustomAttributes(typeof(FormVisualAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "PassportNumber"
            };
            clm4.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("PassportNumber"),

                    });
            grd.Columns.Add(clm4);

            DataGridTemplateColumn clm5 = new DataGridTemplateColumn();
            clm5.Width = new DataGridLength(3, DataGridLengthUnitType.Star);
            clm5.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((FormVisualAttribute)Type.GetType("Models.Client_Model.Form11,Models").
                GetProperty("Type").GetCustomAttributes(typeof(FormVisualAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "Type"
            };
            clm5.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("Type"),

                    });
            grd.Columns.Add(clm5);

            DataGrid bgrd = new DataGrid();
            bgrd.SetValue(Grid.RowProperty, 3);
            bgrd.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            bgrd.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            bgrd.Bind(DataGrid.ItemsProperty, new Binding("Storage.GetFilteredStorage"));
            bgrd.BeginEdit();
            maingrid.Children.Add(bgrd);

            DataGridTemplateColumn clm6 = new DataGridTemplateColumn();
            clm6.Width = new DataGridLength(3, DataGridLengthUnitType.Star);
            clm6.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((FormVisualAttribute)Type.GetType("Models.Client_Model.Report,Models").
                GetProperty("Notes").GetCustomAttributes(typeof(FormVisualAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "Type"
            };
            clm6.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("Type"),

                    });
            bgrd.Columns.Add(clm6);

            return maingrid;
        }
        public static DataGrid Form12_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form13_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form14_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form15_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form16_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form17_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form18_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form19_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
    }
}
