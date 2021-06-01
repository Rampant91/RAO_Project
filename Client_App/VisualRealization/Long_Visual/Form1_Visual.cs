using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media;
using Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

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
            row.Height = new GridLength(0.5, GridUnitType.Star);
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
            topPnl1.SetValue(Grid.RowProperty, 0);
            topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl1.Children.Add(new TextBlock
            {
                Height = 30,
                Margin = Thickness.Parse("5,13,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Text = "Дата конца периода:",
                [Grid.ColumnProperty] = 0,
            });
            topPnl1.Children.Add(new DatePicker
            {
                Height = 30,
                Margin = Thickness.Parse("5,0,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                [!DatePicker.SelectedDateProperty] = new Binding("Storage.EndPeriod", BindingMode.TwoWay),
                [Grid.ColumnProperty] = 2,

            });
            maingrid.Children.Add(topPnl1);

            var topPnl2 = new Grid();
            column = new ColumnDefinition();
            column.Width = new GridLength(1, GridUnitType.Star);
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            column.Width = new GridLength(1, GridUnitType.Star);
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            column.Width = new GridLength(1, GridUnitType.Star);
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            column.Width = new GridLength(1, GridUnitType.Star);
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
                Margin = Thickness.Parse("5,15,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Text = "Номер корректировки:",
                [Grid.ColumnProperty] = 0,
            });

            topPnl2.Children.Add(new TextBox
            {
                Height = 30,
                Width = 70,
                Margin = Thickness.Parse("5,12,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                [!TextBox.TextProperty] = new Binding("Storage.CorrectionNumber"),
                [Grid.ColumnProperty] = 1,
            });

            topPnl2.Children.Add(new Button
            {
                Height = 30,
                Margin = Thickness.Parse("5,12,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = "Проверить",
                [!Button.CommandProperty] = new Binding("CheckReport"),
                [Grid.ColumnProperty] = 2,
            });
            topPnl2.Children.Add(new Button
            {
                Height = 30,
                Margin = Thickness.Parse("5,12,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = "Сохранить",
                [!Button.CommandProperty] = new Binding("SaveReport"),
                [Grid.ColumnProperty] = 3,
            });

            maingrid.Children.Add(topPnl2);

            DataGrid grd = new DataGrid();
            grd.Name = "ReportGrid";
            grd.CanUserResizeColumns = true;
            grd.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            grd.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            grd.Bind(DataGrid.ItemsProperty, new Binding("Storage.Rows11"));

            var cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>();
            itms.Add(new MenuItem
            {
                Header = "Добавить строку",
                [!MenuItem.CommandProperty] = new Binding("AddRow"),
            });
            itms.Add(new MenuItem
            {
                Header = "Вставить из буфера",
                [!MenuItem.CommandProperty] = new Binding("PasteRows"),
            });
            itms.Add(new MenuItem
            {
                Header = "Удалить строки",
                [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
                [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
            });
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            DataGridTemplateColumn clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new TextBlock
            {

            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBlock
                    {

                    });
            grd.Columns.Add(clm);

            int FontSizeGlobal = 11;

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "NumberInOrder"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("NumberInOrder"),
                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "OperationCode"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        FontSize = FontSizeGlobal,
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("OperationCode"),
                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "OperationDate"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        FontSize = FontSizeGlobal,
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("OperationDate"),
                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("PassportNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "PassportNumber"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        FontSize = FontSizeGlobal,
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("PassportNumber"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("Type").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "Type"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        FontSize = FontSizeGlobal,
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("Type"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "Radionuclids"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("Radionuclids"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("FactoryNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "FactoryNumber"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("FactoryNumber"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("Quantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "Quantity"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("Quantity"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("Activity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "Activity"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("Activity"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("CreatorOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "CreatorOKPO"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("CreatorOKPO"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("CreationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "CreationDate"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("CreationDate"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("Category").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "Category"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("Category"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("SignedServicePeriod").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "SignedServicePeriod"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("SignedServicePeriod"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("PropertyCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "PropertyCode"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("PropertyCode"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("Owner").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "Owner"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("Owner"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "DocumentVid"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("DocumentVid"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "DocumentNumber"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("DocumentNumber"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "DocumentDate"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("DocumentDate"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "ProviderOrRecieverOKPO"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("ProviderOrRecieverOKPO"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "TransporterOKPO"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("TransporterOKPO"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "PackName"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("PackName"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "PackType"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("PackType"),

                    });
            grd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                FontSize = FontSizeGlobal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                GetProperty("PackNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "PackNumber"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("PackNumber"),

                    });
            grd.Columns.Add(clm);

            grd.SetValue(Grid.RowProperty, 2);
            grd.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;

            maingrid.Children.Add(grd);

            DataGrid bgrd = new DataGrid();
            bgrd.SetValue(Grid.RowProperty, 3);
            bgrd.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            bgrd.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            bgrd.Bind(DataGrid.ItemsProperty, new Binding("Storage.Notes"));
            bgrd.CanUserResizeColumns = true;
            maingrid.Children.Add(bgrd);

            cntx = new ContextMenu();
            itms = new List<MenuItem>();
            itms.Add(new MenuItem
            {
                Header = "Добавить строку",
                [!MenuItem.CommandProperty] = new Binding("AddRowN"),
            });
            //itms.Add(new MenuItem
            //{
            //    Header = "Удалить строку",
            //    [!MenuItem.CommandProperty] = new Binding("DeleteRowN"),
            //    [!MenuItem.CommandParameterProperty] = new Binding("#parent[2].SelectedItem"),
            //});
            cntx.Items = itms;

            bgrd.ContextMenu = cntx;

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Note,Models").
                GetProperty("RowNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "RowNumber"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("RowNumber"),

                    });
            bgrd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = DataGridLength.SizeToHeader;
            clm.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Note,Models").
                GetProperty("GraphNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "GraphNumber"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("GraphNumber"),

                    });
            bgrd.Columns.Add(clm);

            clm = new DataGridTemplateColumn();
            clm.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            clm.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Note,Models").
                GetProperty("Comment").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "Comment"
            };
            clm.CellTemplate = new FuncDataTemplate<Models.Abstracts.Form>((x, e) =>
                    new TextBox
                    {
                        Foreground = new SolidColorBrush(Color.Parse("Black")),
                        [!TextBox.TextProperty] = new Binding("Comment"),

                    });
            bgrd.Columns.Add(clm);

            return maingrid;
        }
        public static Grid Form12_Visual()
        {
            Grid maingrid = new Grid();

            return maingrid;
        }
        public static Grid Form13_Visual()
        {
            Grid maingrid = new Grid();

            return maingrid;
        }
        public static Grid Form14_Visual()
        {
            Grid maingrid = new Grid();
            return maingrid;
        }
        public static Grid Form15_Visual()
        {
            Grid maingrid = new Grid();

            return maingrid;
        }
        public static Grid Form16_Visual()
        {
            Grid maingrid = new Grid();

            return maingrid;
        }
        public static Grid Form17_Visual()
        {
            Grid maingrid = new Grid();

            return maingrid;
        }
        public static Grid Form18_Visual()
        {
            Grid maingrid = new Grid();

            return maingrid;
        }
        public static Grid Form19_Visual()
        {
            Grid maingrid = new Grid();

            return maingrid;
        }
    }
}
