using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Layout;
using Avalonia.Media;
using Client_App.Controls.DataGrid;
using Client_App.ViewModels;
using Models.Attributes;

namespace Client_App.Long_Visual
{
    public class Form1_Visual
    {
        public static Button CreateButton(string content, string thickness, int columnProp, int height, string commProp)
        {
            return new Button()
            {
                Height = height,
                Margin = Thickness.Parse(thickness),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = content,
                [!Button.CommandProperty] = new Binding(commProp),
                [Grid.ColumnProperty] = columnProp
            };
        }

        public static Cell CreateTextBox(string thickness, int columnProp, int height, string textProp, double width)
        {
            return new Cell(textProp,false)
            {
                Height = height,
                Width = width,
                Margin = Thickness.Parse(thickness),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                [!Cell.DataContextProperty] = new Binding(textProp, BindingMode.TwoWay),
                [Grid.ColumnProperty] = columnProp
            };
        }

        public static TextBlock CreateTextBlock(string margin, int columnProp, int height, string text)
        {
            return new TextBlock
            {
                Height = height,
                Margin = Thickness.Parse(margin),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Text = text,
                [Grid.ColumnProperty] = columnProp
            };
        }

        static StackPanel Create10Row(string Property, string BindingPrefix)
        {
            StackPanel pnl = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 30
            };

            Grid grd = new Grid()
            {
                Width = 400
            };
            grd.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(2, GridUnitType.Star)});
            grd.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)});

            grd.Children.Add(CreateTextBlock("5,0,0,0", 0, 30,
                ((Form_PropertyAttribute) Type.GetType("Models.Form10,Models").GetProperty(Property)
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            grd.Children.Add(CreateTextBox("5,0,10,0", 1, 30, BindingPrefix + "[0]." + Property, 200));

            Grid grd2 = new Grid()
            {
                Width = 400
            };
            grd2.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
            grd2.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            grd2.Children.Add(CreateTextBlock("5,0,0,0", 0, 30,
                ((Form_PropertyAttribute) Type.GetType("Models.Form10,Models").GetProperty(Property)
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            grd2.Children.Add(CreateTextBox("5,0,10,0", 1, 30, BindingPrefix + "[1]." + Property, 200));

            pnl.Children.Add(grd);
            pnl.Children.Add(grd2);
            return pnl;
        }

        public static Grid Form10_Visual(INameScope scp)
        {
            Grid maingrid = new Grid();
            
            RowDefinition? row = new RowDefinition
            {
                Height = new GridLength(0.07, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.07, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.86, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);

            Grid? topPnl2 = new Grid();
            ColumnDefinition? column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl2.SetValue(Grid.RowProperty, 0);

            topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 0, 30, "CheckReport"));
            topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 1, 30, "SaveReport"));

            maingrid.Children.Add(topPnl2);

            var topPnl3 = new StackPanel();
            topPnl3.Orientation = Orientation.Horizontal;
            topPnl3.Spacing = 30;
            //ColumnDefinition? column3 = new ColumnDefinition();
            //topPnl3.ColumnDefinitions.Add(column3);
            //column3 = new ColumnDefinition();
            //topPnl3.ColumnDefinitions.Add(column3);

            topPnl3.SetValue(Grid.RowProperty, 1);

            var pnl1 = new Panel();
            pnl1.Width = 400;
            pnl1.Children.Add(
            new TextBlock
            {
                Height = 30,
                Margin = Thickness.Parse("0,0,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeight.Bold,
                FontSize = 16,
                Text = "Юридическое лицо",
                //[Grid.ColumnProperty] = 0
            });
            topPnl3.Children.Add(pnl1);

            var pnl2 = new Panel();
            pnl2.Width = 400;
            pnl2.Children.Add(
            new TextBlock
            {
                Height = 30,
                Margin = Thickness.Parse("0,0,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeight.Bold,
                FontSize = 16,
                Text = "Обособленное подразделение",
                //[Grid.ColumnProperty] = 1
            });
            topPnl3.Children.Add(pnl2);

            maingrid.Children.Add(topPnl3);

            StackPanel pnl = new StackPanel()
            {
                [Grid.ColumnProperty] = 0,
                [Grid.RowProperty] = 2
            };
            maingrid.Children.Add(pnl);

            string BindingPrefix = "Storage.Rows10";

            pnl.Children.Add(Create10Row("OrganUprav", BindingPrefix));
            pnl.Children.Add(Create10Row("JurLico", BindingPrefix));
            pnl.Children.Add(Create10Row("ShortJurLico", BindingPrefix));
            pnl.Children.Add(Create10Row("JurLicoAddress", BindingPrefix));
            pnl.Children.Add(Create10Row("JurLicoFactAddress", BindingPrefix));
            pnl.Children.Add(Create10Row("GradeFIO", BindingPrefix));
            pnl.Children.Add(Create10Row("Telephone", BindingPrefix));
            pnl.Children.Add(Create10Row("Fax", BindingPrefix));
            pnl.Children.Add(Create10Row("Email", BindingPrefix));
            pnl.Children.Add(Create10Row("RegNo", BindingPrefix));
            pnl.Children.Add(Create10Row("Okpo", BindingPrefix));
            pnl.Children.Add(Create10Row("Okved", BindingPrefix));
            pnl.Children.Add(Create10Row("Okogu", BindingPrefix)); 
            pnl.Children.Add(Create10Row("Oktmo", BindingPrefix));
            pnl.Children.Add(Create10Row("Inn", BindingPrefix));
            pnl.Children.Add(Create10Row("Kpp", BindingPrefix));
            pnl.Children.Add(Create10Row("Okopf", BindingPrefix));
            pnl.Children.Add(Create10Row("Okfs", BindingPrefix));

            return maingrid;
        }

        public static Grid Form11_Visual(INameScope scp)
        {
            Grid maingrid = new Grid();
            RowDefinition? row = new RowDefinition
            {
                Height = new GridLength(0.5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(2, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);

            Grid? topPnl1 = new Grid();
            ColumnDefinition? column = new ColumnDefinition
            {
                Width = new GridLength(0.3, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            topPnl1.SetValue(Grid.RowProperty, 0);
            topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Период:"));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "Storage.StartPeriod", 150));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 2, 30, "Storage.EndPeriod", 150));
            maingrid.Children.Add(topPnl1);

            Grid? topPnl2 = new Grid();
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl2.SetValue(Grid.RowProperty, 1);
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

            topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
            topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.CorrectionNumber", 70));
            topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
            topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

            maingrid.Children.Add(topPnl2);

            Controls.DataGrid.DataGrid grd = new Controls.DataGrid.DataGrid()
            {
                Type = "1.1",
                Name = "Form11Data_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd.SetValue(Grid.RowProperty, 2);

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows11",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b);


            ContextMenu? cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>
            {
                new MenuItem
                {
                    Header = "Добавить строку",
                    [!MenuItem.CommandProperty] = new Binding("AddRow"),
                },
                new MenuItem
                {
                    Header = "Копировать",
                    [!MenuItem.CommandProperty] = new Binding("CopyRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Вставить",
                    [!MenuItem.CommandProperty] = new Binding("PasteRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            maingrid.Children.Add(grd);

            Controls.DataGrid.DataGrid grd1 = new Controls.DataGrid.DataGrid()
            {
                Type = "1.1*",
                Name = "Form11Notes_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd1.SetValue(Grid.RowProperty, 3);

            Binding b1 = new Binding
            {
                Path = "DataContext.Storage.Notes",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd1.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b1);


            ContextMenu? cntx1 = new ContextMenu();
            var mn = new MenuItem
            {
                Header = "Добавить строку",
                [!MenuItem.CommandProperty] = new Binding("AddNote")
            };
            mn.SetValue(MenuItem.CommandParameterProperty, "1.1*");
            List<MenuItem> itms1 = new List<MenuItem>
            {
                mn,
                new MenuItem
                {
                    Header = "Вставить из буфера",
                    [!MenuItem.CommandProperty] = new Binding("PasteNotes"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx1.Items = itms1;

            grd1.ContextMenu = cntx1;

            maingrid.Children.Add(grd1);

            var pnl1 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                [Grid.RowProperty] = 4,
                Spacing = 0
            };
            pnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "ФИО исполнителя:"));
            pnl1.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.FIOexecutor", 400));
            maingrid.Children.Add(pnl1);

            return maingrid;
        }
        public static Grid Form12_Visual(INameScope scp)
        {
            Grid maingrid = new Grid();
            RowDefinition? row = new RowDefinition
            {
                Height = new GridLength(0.5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(2, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);

            Grid? topPnl1 = new Grid();
            ColumnDefinition? column = new ColumnDefinition
            {
                Width = new GridLength(0.3, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            topPnl1.SetValue(Grid.RowProperty, 0);
            topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Период:"));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "Storage.StartPeriod", 150));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 2, 30, "Storage.EndPeriod", 150));
            maingrid.Children.Add(topPnl1);

            Grid? topPnl2 = new Grid();
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl2.SetValue(Grid.RowProperty, 1);
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

            topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
            topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.CorrectionNumber", 70));
            topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
            topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

            maingrid.Children.Add(topPnl2);

            Controls.DataGrid.DataGrid grd = new Controls.DataGrid.DataGrid
            {
                Name = "Form12Data_",
                Type = "1.2",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd.SetValue(Grid.RowProperty, 2);

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows12",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b);


            ContextMenu? cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>
            {
                new MenuItem
                {
                    Header = "Добавить строку",
                    [!MenuItem.CommandProperty] = new Binding("AddRow"),
                },
                new MenuItem
                {
                    Header = "Копировать",
                    [!MenuItem.CommandProperty] = new Binding("CopyRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Вставить",
                    [!MenuItem.CommandProperty] = new Binding("PasteRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            maingrid.Children.Add(grd);

            Controls.DataGrid.DataGrid grd1 = new Controls.DataGrid.DataGrid()
            {
                Type = "1.1*",
                Name = "Form11Notes_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd1.SetValue(Grid.RowProperty, 3);

            Binding b1 = new Binding
            {
                Path = "DataContext.Storage.Notes",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd1.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b1);


            ContextMenu? cntx1 = new ContextMenu();
            var mn = new MenuItem
            {
                Header = "Добавить строку",
                [!MenuItem.CommandProperty] = new Binding("AddNote")
            };
            mn.SetValue(MenuItem.CommandParameterProperty, "1.1*");
            List<MenuItem> itms1 = new List<MenuItem>
            {
                mn,
                new MenuItem
                {
                    Header = "Вставить из буфера",
                    [!MenuItem.CommandProperty] = new Binding("PasteNotes"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx1.Items = itms1;

            grd1.ContextMenu = cntx1;

            maingrid.Children.Add(grd1);

            var pnl1 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                [Grid.RowProperty] = 4,
                Spacing = 0
            };
            pnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "ФИО исполнителя:"));
            pnl1.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.FIOexecutor", 400));
            maingrid.Children.Add(pnl1);

            return maingrid;
        }
        public static Grid Form13_Visual(INameScope scp)
        {
            Grid maingrid = new Grid();
            RowDefinition? row = new RowDefinition
            {
                Height = new GridLength(0.5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(2, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);

            Grid? topPnl1 = new Grid();
            ColumnDefinition? column = new ColumnDefinition
            {
                Width = new GridLength(0.3, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            topPnl1.SetValue(Grid.RowProperty, 0);
            topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Период:"));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "Storage.StartPeriod", 150));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 2, 30, "Storage.EndPeriod", 150));
            maingrid.Children.Add(topPnl1);

            Grid? topPnl2 = new Grid();
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl2.SetValue(Grid.RowProperty, 1);
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

            topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
            topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.CorrectionNumber", 70));
            topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
            topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

            maingrid.Children.Add(topPnl2);

            Controls.DataGrid.DataGrid grd = new Controls.DataGrid.DataGrid
            {
                Name = "Form13Data_",
                Type = "1.3",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd.SetValue(Grid.RowProperty, 2);

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows13",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b);


            ContextMenu? cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>
            {
                new MenuItem
                {
                    Header = "Добавить строку",
                    [!MenuItem.CommandProperty] = new Binding("AddRow"),
                },
                new MenuItem
                {
                    Header = "Копировать",
                    [!MenuItem.CommandProperty] = new Binding("CopyRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Вставить",
                    [!MenuItem.CommandProperty] = new Binding("PasteRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            maingrid.Children.Add(grd);

            Controls.DataGrid.DataGrid grd1 = new Controls.DataGrid.DataGrid()
            {
                Type = "1.1*",
                Name = "Form11Notes_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd1.SetValue(Grid.RowProperty, 3);

            Binding b1 = new Binding
            {
                Path = "DataContext.Storage.Notes",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd1.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b1);

            ContextMenu? cntx1 = new ContextMenu();
            var mn = new MenuItem
            {
                Header = "Добавить строку",
                [!MenuItem.CommandProperty] = new Binding("AddNote")
            };
            mn.SetValue(MenuItem.CommandParameterProperty, "1.1*");
            List<MenuItem> itms1 = new List<MenuItem>
            {
                mn,
                new MenuItem
                {
                    Header = "Вставить из буфера",
                    [!MenuItem.CommandProperty] = new Binding("PasteNotes"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx1.Items = itms1;

            grd1.ContextMenu = cntx1;

            maingrid.Children.Add(grd1);

            var pnl1 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                [Grid.RowProperty] = 4,
                Spacing = 0
            };
            pnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "ФИО исполнителя:"));
            pnl1.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.FIOexecutor", 400));
            maingrid.Children.Add(pnl1);

            return maingrid;
        }
        public static Grid Form14_Visual(INameScope scp)
        {
            Grid maingrid = new Grid();
            RowDefinition? row = new RowDefinition
            {
                Height = new GridLength(0.5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(2, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);

            Grid? topPnl1 = new Grid();
            ColumnDefinition? column = new ColumnDefinition
            {
                Width = new GridLength(0.3, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            topPnl1.SetValue(Grid.RowProperty, 0);
            topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Период:"));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "Storage.StartPeriod", 150));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 2, 30, "Storage.EndPeriod", 150));
            maingrid.Children.Add(topPnl1);

            Grid? topPnl2 = new Grid();
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl2.SetValue(Grid.RowProperty, 1);
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

            topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
            topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.CorrectionNumber", 70));
            topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
            topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

            maingrid.Children.Add(topPnl2);

            Controls.DataGrid.DataGrid grd = new Controls.DataGrid.DataGrid
            {
                Name = "Form14Data_",
                Type = "1.4",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd.SetValue(Grid.RowProperty, 2);

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows14",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b);


            ContextMenu? cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>
            {
                new MenuItem
                {
                    Header = "Добавить строку",
                    [!MenuItem.CommandProperty] = new Binding("AddRow"),
                },
                new MenuItem
                {
                    Header = "Копировать",
                    [!MenuItem.CommandProperty] = new Binding("CopyRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Вставить",
                    [!MenuItem.CommandProperty] = new Binding("PasteRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            maingrid.Children.Add(grd);

            Controls.DataGrid.DataGrid grd1 = new Controls.DataGrid.DataGrid()
            {
                Type = "1.1*",
                Name = "Form11Notes_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd1.SetValue(Grid.RowProperty, 3);

            Binding b1 = new Binding
            {
                Path = "DataContext.Storage.Notes",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd1.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b1);


            ContextMenu? cntx1 = new ContextMenu();
            var mn = new MenuItem
            {
                Header = "Добавить строку",
                [!MenuItem.CommandProperty] = new Binding("AddNote")
            };
            mn.SetValue(MenuItem.CommandParameterProperty, "1.1*");
            List<MenuItem> itms1 = new List<MenuItem>
            {
                mn,
                new MenuItem
                {
                    Header = "Вставить из буфера",
                    [!MenuItem.CommandProperty] = new Binding("PasteNotes"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx1.Items = itms1;

            grd1.ContextMenu = cntx1;

            maingrid.Children.Add(grd1);

            var pnl1 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                [Grid.RowProperty] = 4,
                Spacing = 0
            };
            pnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "ФИО исполнителя:"));
            pnl1.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.FIOexecutor", 400));
            maingrid.Children.Add(pnl1);

            return maingrid;
        }
        public static Grid Form15_Visual(INameScope scp)
        {
            Grid maingrid = new Grid();
            RowDefinition? row = new RowDefinition
            {
                Height = new GridLength(0.5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(2, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);

            Grid? topPnl1 = new Grid();
            ColumnDefinition? column = new ColumnDefinition
            {
                Width = new GridLength(0.3, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            topPnl1.SetValue(Grid.RowProperty, 0);
            topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Период:"));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "Storage.StartPeriod", 150));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 2, 30, "Storage.EndPeriod", 150));
            maingrid.Children.Add(topPnl1);

            Grid? topPnl2 = new Grid();
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl2.SetValue(Grid.RowProperty, 1);
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

            topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
            topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.CorrectionNumber", 70));
            topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
            topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

            maingrid.Children.Add(topPnl2);

            Controls.DataGrid.DataGrid grd = new Controls.DataGrid.DataGrid
            {
                Name = "Form15Data_",
                Type = "1.5",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd.SetValue(Grid.RowProperty, 2);

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows15",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b);


            ContextMenu? cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>
            {
                new MenuItem
                {
                    Header = "Добавить строку",
                    [!MenuItem.CommandProperty] = new Binding("AddRow"),
                },
                new MenuItem
                {
                    Header = "Копировать",
                    [!MenuItem.CommandProperty] = new Binding("CopyRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Вставить",
                    [!MenuItem.CommandProperty] = new Binding("PasteRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            maingrid.Children.Add(grd);

            Controls.DataGrid.DataGrid grd1 = new Controls.DataGrid.DataGrid()
            {
                Type = "1.1*",
                Name = "Form11Notes_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd1.SetValue(Grid.RowProperty, 3);

            Binding b1 = new Binding
            {
                Path = "DataContext.Storage.Notes",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd1.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b1);


            ContextMenu? cntx1 = new ContextMenu();
            var mn = new MenuItem
            {
                Header = "Добавить строку",
                [!MenuItem.CommandProperty] = new Binding("AddNote")
            };
            mn.SetValue(MenuItem.CommandParameterProperty, "1.1*");
            List<MenuItem> itms1 = new List<MenuItem>
            {
                mn,
                new MenuItem
                {
                    Header = "Вставить из буфера",
                    [!MenuItem.CommandProperty] = new Binding("PasteNotes"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx1.Items = itms1;

            grd1.ContextMenu = cntx1;

            maingrid.Children.Add(grd1);

            var pnl1 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                [Grid.RowProperty] = 4,
                Spacing = 0
            };
            pnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "ФИО исполнителя:"));
            pnl1.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.FIOexecutor", 400));
            maingrid.Children.Add(pnl1);

            return maingrid;
        }
        public static Grid Form16_Visual(INameScope scp)
        {
            Grid maingrid = new Grid();
            RowDefinition? row = new RowDefinition
            {
                Height = new GridLength(0.5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(2, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);

            Grid? topPnl1 = new Grid();
            ColumnDefinition? column = new ColumnDefinition
            {
                Width = new GridLength(0.3, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            topPnl1.SetValue(Grid.RowProperty, 0);
            topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Период:"));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "Storage.StartPeriod", 150));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 2, 30, "Storage.EndPeriod", 150));
            maingrid.Children.Add(topPnl1);

            Grid? topPnl2 = new Grid();
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl2.SetValue(Grid.RowProperty, 1);
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

            topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
            topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.CorrectionNumber", 70));
            topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
            topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

            maingrid.Children.Add(topPnl2);

            Controls.DataGrid.DataGrid grd = new Controls.DataGrid.DataGrid
            {
                Name = "Form16Data_",
                Type = "1.6",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd.SetValue(Grid.RowProperty, 2);

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows16",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b);


            ContextMenu? cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>
            {
                new MenuItem
                {
                    Header = "Добавить строку",
                    [!MenuItem.CommandProperty] = new Binding("AddRow"),
                },
                new MenuItem
                {
                    Header = "Копировать",
                    [!MenuItem.CommandProperty] = new Binding("CopyRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Вставить",
                    [!MenuItem.CommandProperty] = new Binding("PasteRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            maingrid.Children.Add(grd);

            Controls.DataGrid.DataGrid grd1 = new Controls.DataGrid.DataGrid()
            {
                Type = "1.1*",
                Name = "Form11Notes_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd1.SetValue(Grid.RowProperty, 3);

            Binding b1 = new Binding
            {
                Path = "DataContext.Storage.Notes",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd1.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b1);


            ContextMenu? cntx1 = new ContextMenu();
            var mn = new MenuItem
            {
                Header = "Добавить строку",
                [!MenuItem.CommandProperty] = new Binding("AddNote")
            };
            mn.SetValue(MenuItem.CommandParameterProperty, "1.1*");
            List<MenuItem> itms1 = new List<MenuItem>
            {
                mn,
                new MenuItem
                {
                    Header = "Вставить из буфера",
                    [!MenuItem.CommandProperty] = new Binding("PasteNotes"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx1.Items = itms1;

            grd1.ContextMenu = cntx1;

            maingrid.Children.Add(grd1);

            var pnl1 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                [Grid.RowProperty] = 4,
                Spacing = 0
            };
            pnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "ФИО исполнителя:"));
            pnl1.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.FIOexecutor", 400));
            maingrid.Children.Add(pnl1);

            return maingrid;
        }
        public static Grid Form17_Visual(INameScope scp)
        {
            Grid maingrid = new Grid();
            RowDefinition? row = new RowDefinition
            {
                Height = new GridLength(0.5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(2, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);

            Grid? topPnl1 = new Grid();
            ColumnDefinition? column = new ColumnDefinition
            {
                Width = new GridLength(0.3, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            topPnl1.SetValue(Grid.RowProperty, 0);
            topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Период:"));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "Storage.StartPeriod", 150));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 2, 30, "Storage.EndPeriod", 150));
            maingrid.Children.Add(topPnl1);

            Grid? topPnl2 = new Grid();
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl2.SetValue(Grid.RowProperty, 1);
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

            topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
            topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.CorrectionNumber", 70));
            topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
            topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

            maingrid.Children.Add(topPnl2);

            Controls.DataGrid.DataGrid grd = new Controls.DataGrid.DataGrid
            {
                Name = "Form17Data_",
                Type = "1.7",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd.SetValue(Grid.RowProperty, 2);

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows17",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b);


            ContextMenu? cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>
            {
                new MenuItem
                {
                    Header = "Добавить строку",
                    [!MenuItem.CommandProperty] = new Binding("AddRow"),
                },
                new MenuItem
                {
                    Header = "Копировать",
                    [!MenuItem.CommandProperty] = new Binding("CopyRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Вставить",
                    [!MenuItem.CommandProperty] = new Binding("PasteRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            maingrid.Children.Add(grd);

            Controls.DataGrid.DataGrid grd1 = new Controls.DataGrid.DataGrid()
            {
                Type = "1.1*",
                Name = "Form11Notes_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd1.SetValue(Grid.RowProperty, 3);

            Binding b1 = new Binding
            {
                Path = "DataContext.Storage.Notes",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd1.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b1);


            ContextMenu? cntx1 = new ContextMenu();
            var mn = new MenuItem
            {
                Header = "Добавить строку",
                [!MenuItem.CommandProperty] = new Binding("AddNote")
            };
            mn.SetValue(MenuItem.CommandParameterProperty, "1.1*");
            List<MenuItem> itms1 = new List<MenuItem>
            {
                mn,
                new MenuItem
                {
                    Header = "Вставить из буфера",
                    [!MenuItem.CommandProperty] = new Binding("PasteNotes"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx1.Items = itms1;

            grd1.ContextMenu = cntx1;

            maingrid.Children.Add(grd1);

            var pnl1 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                [Grid.RowProperty] = 4,
                Spacing = 0
            };
            pnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "ФИО исполнителя:"));
            pnl1.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.FIOexecutor", 400));
            maingrid.Children.Add(pnl1);

            return maingrid;
        }
        public static Grid Form18_Visual(INameScope scp)
        {
            Grid maingrid = new Grid();
            RowDefinition? row = new RowDefinition
            {
                Height = new GridLength(0.5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(2, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);

            Grid? topPnl1 = new Grid();
            ColumnDefinition? column = new ColumnDefinition
            {
                Width = new GridLength(0.3, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            topPnl1.SetValue(Grid.RowProperty, 0);
            topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Период:"));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "Storage.StartPeriod", 150));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 2, 30, "Storage.EndPeriod", 150));
            maingrid.Children.Add(topPnl1);

            Grid? topPnl2 = new Grid();
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl2.SetValue(Grid.RowProperty, 1);
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

            topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
            topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.CorrectionNumber", 70));
            topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
            topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

            maingrid.Children.Add(topPnl2);

            Controls.DataGrid.DataGrid grd = new Controls.DataGrid.DataGrid
            {
                Name = "Form18Data_",
                Type = "1.8",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd.SetValue(Grid.RowProperty, 2);

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows18",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b);


            ContextMenu? cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>
            {
                new MenuItem
                {
                    Header = "Добавить строку",
                    [!MenuItem.CommandProperty] = new Binding("AddRow"),
                },
                new MenuItem
                {
                    Header = "Копировать",
                    [!MenuItem.CommandProperty] = new Binding("CopyRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Вставить",
                    [!MenuItem.CommandProperty] = new Binding("PasteRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            maingrid.Children.Add(grd);

            Controls.DataGrid.DataGrid grd1 = new Controls.DataGrid.DataGrid()
            {
                Type = "1.1*",
                Name = "Form11Notes_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd1.SetValue(Grid.RowProperty, 3);

            Binding b1 = new Binding
            {
                Path = "DataContext.Storage.Notes",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd1.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b1);


            ContextMenu? cntx1 = new ContextMenu();
            var mn = new MenuItem
            {
                Header = "Добавить строку",
                [!MenuItem.CommandProperty] = new Binding("AddNote")
            };
            mn.SetValue(MenuItem.CommandParameterProperty, "1.1*");
            List<MenuItem> itms1 = new List<MenuItem>
            {
                mn,
                new MenuItem
                {
                    Header = "Вставить из буфера",
                    [!MenuItem.CommandProperty] = new Binding("PasteNotes"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx1.Items = itms1;

            grd1.ContextMenu = cntx1;

            maingrid.Children.Add(grd1);

            var pnl1 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                [Grid.RowProperty] = 4,
                Spacing = 0
            };
            pnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "ФИО исполнителя:"));
            pnl1.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.FIOexecutor", 400));
            maingrid.Children.Add(pnl1);

            return maingrid;
        }
        public static Grid Form19_Visual(INameScope scp)
        {
            Grid maingrid = new Grid();
            RowDefinition? row = new RowDefinition
            {
                Height = new GridLength(0.5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(5, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(2, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);
            row = new RowDefinition
            {
                Height = new GridLength(0.7, GridUnitType.Star)
            };
            maingrid.RowDefinitions.Add(row);

            Grid? topPnl1 = new Grid();
            ColumnDefinition? column = new ColumnDefinition
            {
                Width = new GridLength(0.3, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl1.ColumnDefinitions.Add(column);
            topPnl1.SetValue(Grid.RowProperty, 0);
            topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Период:"));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "Storage.StartPeriod", 150));
            topPnl1.Children.Add(CreateTextBox("5,0,0,0", 2, 30, "Storage.EndPeriod", 150));
            maingrid.Children.Add(topPnl1);

            Grid? topPnl2 = new Grid();
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl2.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl2.SetValue(Grid.RowProperty, 1);
            topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

            topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
            topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.CorrectionNumber", 70));
            topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
            topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

            maingrid.Children.Add(topPnl2);

            Controls.DataGrid.DataGrid grd = new Controls.DataGrid.DataGrid
            {
                Name = "Form19Data_",
                Type = "1.9",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd.SetValue(Grid.RowProperty, 2);

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows19",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b);


            ContextMenu? cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>
            {
                new MenuItem
                {
                    Header = "Добавить строку",
                    [!MenuItem.CommandProperty] = new Binding("AddRow"),
                },
                new MenuItem
                {
                    Header = "Копировать",
                    [!MenuItem.CommandProperty] = new Binding("CopyRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Вставить",
                    [!MenuItem.CommandProperty] = new Binding("PasteRows"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            maingrid.Children.Add(grd);

            Controls.DataGrid.DataGrid grd1 = new Controls.DataGrid.DataGrid()
            {
                Type = "1.1*",
                Name = "Form11Notes_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };
            grd1.SetValue(Grid.RowProperty, 3);

            Binding b1 = new Binding
            {
                Path = "DataContext.Storage.Notes",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd1.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b1);


            ContextMenu? cntx1 = new ContextMenu();
            var mn = new MenuItem
            {
                Header = "Добавить строку",
                [!MenuItem.CommandProperty] = new Binding("AddNote")
            };
            mn.SetValue(MenuItem.CommandParameterProperty,"1.1*");
            List<MenuItem> itms1 = new List<MenuItem>
            {
                mn,
                new MenuItem
                {
                    Header = "Вставить из буфера",
                    [!MenuItem.CommandProperty] = new Binding("PasteNotes"),
                },
                new MenuItem
                {
                    Header = "Удалить строки",
                    [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx1.Items = itms1;

            grd1.ContextMenu = cntx1;

            maingrid.Children.Add(grd1);

            var pnl1 = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                [Grid.RowProperty] = 4,
                Spacing = 0
            };
            pnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "ФИО исполнителя:"));
            pnl1.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "Storage.FIOexecutor", 400));
            maingrid.Children.Add(pnl1);

            return maingrid;
        }
    }
}
