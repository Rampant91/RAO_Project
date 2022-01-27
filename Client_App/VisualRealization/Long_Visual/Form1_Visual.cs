using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Layout;
using Avalonia.Media;
using Client_App.Controls.DataGrid;
using Models.Attributes;
using Models;
using Avalonia.Controls.Primitives;
using Client_App.Converters;

namespace Client_App.Long_Visual
{
    public class Form1_Visual
    {
        public static Button CreateButton(string content, string thickness, int height, string commProp)
        {
            return new Button()
            {
                Height = height,
                Margin = Thickness.Parse(thickness),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = content,
                [!Button.CommandProperty] = new Binding(commProp)
            };
        }

        public static Cell CreateTextBox(string thickness, int height, string textProp, double width, INameScope scp, string watermark = "", bool _flag = false)
        {
            Cell textCell = new Cell() {
                Width = width,
                Margin = Thickness.Parse(thickness),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            Binding b = new Binding
            {
                Path = textProp,
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            textCell.Control = new TextBox()
            {
                [!TextBox.DataContextProperty] = b,
                [!TextBox.TextProperty] = new Binding("Value"),
            };
            return textCell;
        }

        public static TextBlock CreateTextBlock(string margin, int height, string text,double width=0)
        {
            TextBlock tmp = null;
            if (width != 0)
            {
                tmp = new TextBlock
                {
                    Width = width,
                    Height = height,
                    Margin = Thickness.Parse(margin),
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                    Text = text
                };
            }
            tmp = new TextBlock
            {
                Height = height,
                Margin = Thickness.Parse(margin),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Text = text
            };
            return tmp;
        }

        static StackPanel Create10Item(string margine, string Property, string BindingPrefix, INameScope scp, int index)
        {
            StackPanel itemStackPanel = new StackPanel();
            itemStackPanel.Orientation= Orientation.Horizontal;
            itemStackPanel.Children.Add(CreateTextBlock("5,0,0,0", 30, ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").GetProperty(Property).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[index], 0));
            itemStackPanel.Children.Add(CreateTextBox(margine, 30, BindingPrefix + "[" + index + "]." + Property, 400, scp));
            return itemStackPanel;
        }

        public static Control Form10_Visual(INameScope scp)
        {
            string BindingPrefix = "DataContext.Storage.Rows10";

            ScrollViewer vw = new ScrollViewer();
            vw.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

            #region Main
            Panel mainPanel = new Panel();
            var mainCanvas = new Canvas();
            mainCanvas.Height = 1220;
            mainPanel.Children.Add(mainCanvas);
            vw.Content = mainPanel;

            #endregion

            #region Header
            Panel headerPanel = new Panel();
            Binding b = new Binding() { 
                Source=vw,
                Path="Offset",
                Converter=new VectorToMarginTop_Converter()
            };
            Border brdH = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                Margin = Thickness.Parse("5,5,5,5"),
                ZIndex = 999,
                Background = new SolidColorBrush(Color.Parse("White"))
            };
            brdH[!Border.MarginProperty] = b;
            brdH.Child = headerPanel;
            mainCanvas.Children.Add(brdH);

            StackPanel headerStackPanel = new StackPanel();
            headerStackPanel.Orientation = Orientation.Vertical;
            headerPanel.Children.Add(headerStackPanel);

            StackPanel headerOrganUprav = new StackPanel();
            headerOrganUprav.Orientation = Orientation.Horizontal;
            headerStackPanel.Children.Add(headerOrganUprav);
            headerOrganUprav.Children.Add(CreateTextBlock("5,10,0,0", 30,
                ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").GetProperty("OrganUprav")
                 .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[0]
                , 295));

            headerOrganUprav.Children.Add(CreateTextBox("28,0,10,0", 30, BindingPrefix + "[0]." + "OrganUprav", 400, scp));

            StackPanel headerRegNo = new StackPanel();
            headerRegNo.Orientation = Orientation.Horizontal;
            headerStackPanel.Children.Add(headerRegNo);
            headerRegNo.Children.Add(CreateTextBlock("5,0,0,0", 30,
                ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").GetProperty("RegNo")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[0]
                , 295));

            headerRegNo.Children.Add(CreateTextBox("286,0,10,20", 30, BindingPrefix + "[0]." + "RegNo", 400, scp));

            StackPanel headerButtons = new StackPanel();
            headerButtons.Orientation = Orientation.Horizontal;
            headerStackPanel.Children.Add(headerButtons);
            headerButtons.Children.Add(CreateButton("Проверить", "5,5,0,0", 30, "CheckReport"));
            headerButtons.Children.Add(CreateButton("Сохранить", "5,5,0,0", 30, "SaveReport"));
            #endregion

            StackPanel centerStackPanel = new StackPanel();
            centerStackPanel.SetValue(Canvas.TopProperty,140);
            centerStackPanel.Orientation = Orientation.Vertical;
            mainCanvas.Children.Add(centerStackPanel);

            #region UrLico
            Panel urLicoPanel = new Panel();
            Border brdUr = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdUr.Child = urLicoPanel;
            centerStackPanel.Children.Add(brdUr);

            StackPanel urLicoStackPanel = new StackPanel();
            urLicoStackPanel.Orientation = Orientation.Vertical;
            urLicoPanel.Children.Add(urLicoStackPanel);

            urLicoStackPanel.Children.Add(new TextBlock
            {
                Height = 30,
                Margin = Thickness.Parse("0,0,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeight.Bold,
                FontSize = 16,
                Text = "Юридическое лицо",
            });
            urLicoStackPanel.Children.Add(Create10Item("103,0,10,0", "SubjectRF", BindingPrefix, scp,0));
            urLicoStackPanel.Children.Add(Create10Item("83,0,10,0", "JurLico", BindingPrefix, scp,0));
            urLicoStackPanel.Children.Add(Create10Item("119,0,10,0", "ShortJurLico", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("28,0,10,0", "JurLicoAddress", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("121,0,10,0", "JurLicoFactAddress", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("108,0,10,0", "GradeFIO", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("158,0,10,0", "Telephone", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("179,0,10,0", "Fax", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("154,0,10,0", "Email", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("246,0,10,0", "Okpo", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("241,0,10,0", "Okved", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("242,0,10,0", "Okogu", BindingPrefix, scp, 0)); 
            urLicoStackPanel.Children.Add(Create10Item("238,0,10,0", "Oktmo", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("254,0,10,0", "Inn", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("256,0,10,0", "Kpp", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("238,0,10,0", "Okopf", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("248,0,10,0", "Okfs", BindingPrefix, scp, 0));
            #endregion

            #region ObosobPodrazd
            Panel obosobPodrazdPanel = new Panel();
            Border brdOb = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdOb.Child = obosobPodrazdPanel;
            centerStackPanel.Children.Add(brdOb);

            StackPanel obosobPodrazdStackPanel = new StackPanel();
            obosobPodrazdStackPanel.Orientation = Orientation.Vertical;
            obosobPodrazdPanel.Children.Add(obosobPodrazdStackPanel);

            obosobPodrazdStackPanel.Children.Add(new TextBlock
            {
                Height = 30,
                Margin = Thickness.Parse("0,0,0,0"),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeight.Bold,
                FontSize = 16,
                Text = "Обособленное подразделение",
            });
            obosobPodrazdStackPanel.Children.Add(Create10Item("139,0,10,0", "SubjectRF", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("56,0,10,0", "JurLico", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("155,0,10,0", "ShortJurLico", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("1,0,10,0", "JurLicoAddress", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("33,0,10,0", "JurLicoFactAddress", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("144,0,10,0", "GradeFIO", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("194,0,10,0", "Telephone", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("215,0,10,0", "Fax", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("190,0,10,0", "Email", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("282,0,10,0", "Okpo", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("277,0,10,0", "Okved", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("278,0,10,0", "Okogu", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("274,0,10,0", "Oktmo", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("290,0,10,0", "Inn", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("292,0,10,0", "Kpp", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("274,0,10,0", "Okopf", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("284,0,10,0", "Okfs", BindingPrefix, scp, 1));
            #endregion

            return vw;
        }

        public static Control Form11_Visual(INameScope scp)
        {
            ScrollViewer vw = new ScrollViewer();
            vw.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
            StackPanel maingrid = new StackPanel();
            vw.Content = maingrid;
            Binding ind = new Binding()
            {
                Source = vw,
                Path = "Offset",
                Converter = new VectorToMarginLeft_Converter()
            };

            #region Header

            StackPanel? topPnl1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                [!StackPanel.MarginProperty]=ind
            };


            #region Left
            StackPanel leftStP = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Spacing = 45
            };

            StackPanel leftStPT = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = Thickness.Parse("0,12,0,0")

            };
            StackPanel? content = new StackPanel()
            {
                Orientation= Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:"));
            content.Children.Add(CreateTextBox("17,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment=VerticalAlignment.Bottom
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
            content.Children.Add(CreateButton("Проверить", "85,0,0,15", 30, "CheckReport"));
            content.Children.Add(CreateButton("Сохранить", "5,0,0,15", 30, "SaveReport"));

            leftStP.Children.Add(leftStPT);
            leftStP.Children.Add(content);

            Border brdC = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                [Grid.RowProperty] = 1,
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdC.Child = leftStP;
            #endregion

            #region Right
            StackPanel rigthStP = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = Thickness.Parse("0,12,0,0")
            };

            Border brdR = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdR.Child = rigthStP;

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
            content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
            rigthStP.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
            content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
            rigthStP.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
            content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp));
            rigthStP.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
            rigthStP.Children.Add(content);
            #endregion

            topPnl1.Children.Add(brdC);
            topPnl1.Children.Add(brdR);
            maingrid.Children.Add(topPnl1);
            #endregion

            Controls.DataGrid.DataGridForm11 grd = new Controls.DataGrid.DataGridForm11()
            {
                Name = "Form11Data_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,0,0,0")
            };
            grd.SetValue(Grid.RowProperty, 2);

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows11",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGridForm11.ItemsProperty, b);

            maingrid.Children.Add(grd);

            Grid? topPnl22 = new Grid() 
            {
                Margin = Thickness.Parse("5,0,0,0")
            };
            var column = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };
            topPnl22.ColumnDefinitions.Add(column);
            topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            topPnl22.SetValue(Grid.RowProperty, 3);
            topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

            topPnl22.Children.Add(CreateTextBlock("5,13,0,0", 30, "Примечания:"));
            maingrid.Children.Add(topPnl22);

            Panel prt = new Panel()
            {
                [Grid.ColumnProperty] = 4,
                [!Control.MarginProperty] = ind
            };
            Controls.DataGrid.DataGridNote grd1 = new Controls.DataGrid.DataGridNote()
            {
                Name = "Form11Notes_",
                Focusable = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,0,0,0")

            };

            prt.Children.Add(grd1);

            Binding b1 = new Binding
            {
                Path = "DataContext.Storage.Notes",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd1.Bind(Controls.DataGrid.DataGridNote.ItemsProperty, b1);

            maingrid.Children.Add(prt);

            return vw;
        }
        //public static Control Form12_Visual(INameScope scp)
        //{
        //    ScrollViewer vw = new ScrollViewer();
        //    vw.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
        //    StackPanel maingrid = new StackPanel();
        //    vw.Content = maingrid;

        //    Grid? topPnl1 = new Grid();
        //    ColumnDefinition? column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    topPnl1.SetValue(Grid.RowProperty, 0);
        //    topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 0, 30, "Дата окончания предыдущего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 0, 30, "DataContext.Storage.StartPeriod", 150, scp));
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 2, 0, 30, "Дата окончания настоящего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 3, 0, 30, "DataContext.Storage.EndPeriod", 150, scp));
        //    maingrid.Children.Add(topPnl1);

        //    Grid? topPnl2 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition();
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl2.SetValue(Grid.RowProperty, 1);
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 0, 30, "Номер корректировки:"));
        //    topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 0, 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        //    topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 0, 0, 30, "CheckReport"));
        //    topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 0, 0, 30, "SaveReport"));

        //    maingrid.Children.Add(topPnl2);

        //    Controls.DataGrid.DataGridForm12 grd = new Controls.DataGrid.DataGridForm12()
        //    {
        //        Name = "Form12Data_",
        //        Focusable = true,
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd.SetValue(Grid.RowProperty, 2);

        //    Binding b = new Binding
        //    {
        //        Path = "DataContext.Storage.Rows12",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd.Bind(Controls.DataGrid.DataGridForm12.ItemsProperty, b);


        //    ContextMenu? cntx = new ContextMenu();
        //    List<MenuItem> itms = new List<MenuItem>
        //    {
        //        new MenuItem
        //        {
        //            Header = "Добавить строку",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRow"),
        //        },
        //                        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateRowsx1"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк перед",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+I"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRowIn"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx.Items = itms;

        //    grd.ContextMenu = cntx;

        //    maingrid.Children.Add(grd);

        //    Grid? topPnl22 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl22.ColumnDefinitions.Add(column);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl22.SetValue(Grid.RowProperty, 3);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl22.Children.Add(CreateTextBlock("5,13,0,0", 0, 0, 30, "Примечания:"));
        //    maingrid.Children.Add(topPnl22);

        //    Controls.DataGrid.DataGridNote grd1 = new Controls.DataGrid.DataGridNote()
        //    {
        //        Name = "Form11Notes_",
        //        Focusable = true,
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd1.SetValue(Grid.RowProperty, 4);

        //    Binding b1 = new Binding
        //    {
        //        Path = "DataContext.Storage.Notes",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd1.Bind(Controls.DataGrid.DataGridNote.ItemsProperty, b1);


        //    ContextMenu? cntx1 = new ContextMenu();
        //    var mn = new MenuItem
        //    {
        //        Header = "Добавить строку",
        //        InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //        [!MenuItem.CommandProperty] = new Binding("AddNote"),
        //        CommandParameter = "1.1*"
        //    };
        //    List<MenuItem> itms1 = new List<MenuItem>
        //    {
        //        mn,
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateNotes"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx1.Items = itms1;

        //    grd1.ContextMenu = cntx1;

        //    maingrid.Children.Add(grd1);

        //    Grid? topPnl23 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.5, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.SetValue(Grid.RowProperty, 5);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 2, 0, 30, "ФИО исполнителя:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 3, 30, "DataContext.Storage.FIOexecutor", 180, scp,"ФИО исполнителя...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Должность:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.GradeExecutor", 95, scp, "Должность...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 4, 30, "Телефон:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 5, 30, "DataContext.Storage.ExecPhone", 95, scp,"Телефон...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 6, 30, "Электронная почта:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 7, 30, "DataContext.Storage.ExecEmail", 130, scp, "Электронная почта...", true));
        //    maingrid.Children.Add(topPnl23);

        //    return vw;
        //}
        //public static Control Form13_Visual(INameScope scp)
        //{
        //    ScrollViewer vw = new ScrollViewer(); vw.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
        //    StackPanel maingrid = new StackPanel();
        //    vw.Content = maingrid;

        //    Grid? topPnl1 = new Grid();
        //    ColumnDefinition? column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    topPnl1.SetValue(Grid.RowProperty, 0);
        //    topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 0, 30, "Дата окончания предыдущего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 0, 30, "DataContext.Storage.StartPeriod", 150, scp));
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 2, 0, 30, "Дата окончания настоящего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 3, 0, 30, "DataContext.Storage.EndPeriod", 150, scp));
        //    maingrid.Children.Add(topPnl1);

        //    Grid? topPnl2 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition();
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl2.SetValue(Grid.RowProperty, 1);
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
        //    topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        //    topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
        //    topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

        //    maingrid.Children.Add(topPnl2);

        //    Controls.DataGrid.DataGrid<Form13> grd = new Controls.DataGrid.DataGrid<Form13>
        //    {
        //        Name = "Form13Data_",
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd.SetValue(Grid.RowProperty, 2);

        //    Binding b = new Binding
        //    {
        //        Path = "DataContext.Storage.Rows13",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd.Bind(Controls.DataGrid.DataGrid<Form13>.ItemsProperty, b);


        //    ContextMenu? cntx = new ContextMenu();
        //    List<MenuItem> itms = new List<MenuItem>
        //    {
        //        new MenuItem
        //        {
        //            Header = "Добавить строку",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRow"),
        //        },
        //                        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateRowsx1"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк перед",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+I"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRowIn"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx.Items = itms;

        //    grd.ContextMenu = cntx;

        //    maingrid.Children.Add(grd);
        //    Grid? topPnl22 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl22.ColumnDefinitions.Add(column);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl22.SetValue(Grid.RowProperty, 3);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl22.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Примечания:"));
        //    maingrid.Children.Add(topPnl22);

        //    Controls.DataGrid.DataGrid<Note> grd1 = new Controls.DataGrid.DataGrid<Note>()
        //    {
        //        Name = "Form11Notes_",
        //        Focusable = true,
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd1.SetValue(Grid.RowProperty, 4);

        //    Binding b1 = new Binding
        //    {
        //        Path = "DataContext.Storage.Notes",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd1.Bind(Controls.DataGrid.DataGrid<Note>.ItemsProperty, b1);

        //    ContextMenu? cntx1 = new ContextMenu();
        //    var mn = new MenuItem
        //    {
        //        Header = "Добавить строку",
        //        InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //        [!MenuItem.CommandProperty] = new Binding("AddNote"),
        //        CommandParameter = "1.1*"
        //    };
        //    List<MenuItem> itms1 = new List<MenuItem>
        //    {
        //        mn,
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateNotes"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx1.Items = itms1;

        //    grd1.ContextMenu = cntx1;

        //    maingrid.Children.Add(grd1);

        //    Grid? topPnl23 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.5, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.SetValue(Grid.RowProperty, 5);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "ФИО исполнителя:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 3, 30, "DataContext.Storage.FIOexecutor", 180, scp, "ФИО исполнителя...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Должность:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.GradeExecutor", 95, scp, "Должность...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 4, 30, "Телефон:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 5, 30, "DataContext.Storage.ExecPhone", 95, scp, "Телефон...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 6, 30, "Электронная почта:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 7, 30, "DataContext.Storage.ExecEmail", 130, scp, "Электронная почта...", true));
        //    maingrid.Children.Add(topPnl23);
        //    return vw;
        //}
        //public static Control Form14_Visual(INameScope scp)
        //{
        //    ScrollViewer vw = new ScrollViewer(); vw.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
        //    StackPanel maingrid = new StackPanel();
        //    vw.Content = maingrid;

        //    Grid? topPnl1 = new Grid();
        //    ColumnDefinition? column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    topPnl1.SetValue(Grid.RowProperty, 0);
        //    topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Дата окончания предыдущего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "DataContext.Storage.StartPeriod", 150, scp));
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "Дата окончания настоящего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 3, 30, "DataContext.Storage.EndPeriod", 150, scp));
        //    maingrid.Children.Add(topPnl1);

        //    Grid? topPnl2 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition();
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl2.SetValue(Grid.RowProperty, 1);
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
        //    topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        //    topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
        //    topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

        //    maingrid.Children.Add(topPnl2);

        //    Controls.DataGrid.DataGrid<Form14> grd = new Controls.DataGrid.DataGrid<Form14>
        //    {
        //        Name = "Form14Data_",
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd.SetValue(Grid.RowProperty, 2);

        //    Binding b = new Binding
        //    {
        //        Path = "DataContext.Storage.Rows14",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd.Bind(Controls.DataGrid.DataGrid<Form14>.ItemsProperty, b);


        //    ContextMenu? cntx = new ContextMenu();
        //    List<MenuItem> itms = new List<MenuItem>
        //    {
        //        new MenuItem
        //        {
        //            Header = "Добавить строку",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRow"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateRowsx1"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк перед",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+I"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRowIn"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx.Items = itms;

        //    grd.ContextMenu = cntx;

        //    maingrid.Children.Add(grd);
        //    Grid? topPnl22 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl22.ColumnDefinitions.Add(column);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl22.SetValue(Grid.RowProperty, 3);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl22.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Примечания:"));
        //    maingrid.Children.Add(topPnl22);
        //    Controls.DataGrid.DataGrid<Note> grd1 = new Controls.DataGrid.DataGrid<Note>()
        //    {
        //        Name = "Form11Notes_",
        //        Focusable = true,
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd1.SetValue(Grid.RowProperty, 4);

        //    Binding b1 = new Binding
        //    {
        //        Path = "DataContext.Storage.Notes",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd1.Bind(Controls.DataGrid.DataGrid<Note>.ItemsProperty, b1);


        //    ContextMenu? cntx1 = new ContextMenu();
        //    var mn = new MenuItem
        //    {
        //        Header = "Добавить строку",
        //        InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //        [!MenuItem.CommandProperty] = new Binding("AddNote"),
        //        CommandParameter = "1.1*"
        //    };
        //    List<MenuItem> itms1 = new List<MenuItem>
        //    {
        //        mn,
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateNotes"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx1.Items = itms1;

        //    grd1.ContextMenu = cntx1;

        //    maingrid.Children.Add(grd1);

        //    Grid? topPnl23 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.5, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.SetValue(Grid.RowProperty, 5);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "ФИО исполнителя:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 3, 30, "DataContext.Storage.FIOexecutor", 180, scp, "ФИО исполнителя...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Должность:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.GradeExecutor", 95, scp, "Должность...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 4, 30, "Телефон:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 5, 30, "DataContext.Storage.ExecPhone", 95, scp, "Телефон...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 6, 30, "Электронная почта:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 7, 30, "DataContext.Storage.ExecEmail", 130, scp, "Электронная почта...", true));
        //    maingrid.Children.Add(topPnl23);
        //    return vw;
        //}
        //public static Control Form15_Visual(INameScope scp)
        //{
        //    ScrollViewer vw = new ScrollViewer(); vw.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
        //    StackPanel maingrid = new StackPanel();
        //    vw.Content = maingrid;

        //    Grid? topPnl1 = new Grid();
        //    ColumnDefinition? column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    topPnl1.SetValue(Grid.RowProperty, 0);
        //    topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Дата окончания предыдущего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "DataContext.Storage.StartPeriod", 150, scp));
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "Дата окончания настоящего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 3, 30, "DataContext.Storage.EndPeriod", 150, scp));
        //    maingrid.Children.Add(topPnl1);

        //    Grid? topPnl2 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition();
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl2.SetValue(Grid.RowProperty, 1);
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
        //    topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        //    topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
        //    topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

        //    maingrid.Children.Add(topPnl2);

        //    Controls.DataGrid.DataGrid<Form15> grd = new Controls.DataGrid.DataGrid<Form15>
        //    {
        //        Name = "Form15Data_",
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd.SetValue(Grid.RowProperty, 2);

        //    Binding b = new Binding
        //    {
        //        Path = "DataContext.Storage.Rows15",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd.Bind(Controls.DataGrid.DataGrid<Form15>.ItemsProperty, b);


        //    ContextMenu? cntx = new ContextMenu();
        //    List<MenuItem> itms = new List<MenuItem>
        //    {
        //        new MenuItem
        //        {
        //            Header = "Добавить строку",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRow"),
        //        },
        //                        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateRowsx1"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк перед",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+I"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRowIn"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx.Items = itms;

        //    grd.ContextMenu = cntx;

        //    maingrid.Children.Add(grd);
        //    Grid? topPnl22 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl22.ColumnDefinitions.Add(column);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl22.SetValue(Grid.RowProperty, 3);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl22.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Примечания:"));
        //    maingrid.Children.Add(topPnl22);
        //    Controls.DataGrid.DataGrid<Note> grd1 = new Controls.DataGrid.DataGrid<Note>()
        //    {
        //        Name = "Form11Notes_",
        //        Focusable = true,
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd1.SetValue(Grid.RowProperty, 4);

        //    Binding b1 = new Binding
        //    {
        //        Path = "DataContext.Storage.Notes",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd1.Bind(Controls.DataGrid.DataGrid<Note>.ItemsProperty, b1);


        //    ContextMenu? cntx1 = new ContextMenu();
        //    var mn = new MenuItem
        //    {
        //        Header = "Добавить строку",
        //        InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //        [!MenuItem.CommandProperty] = new Binding("AddNote"),
        //        CommandParameter = "1.1*"
        //    };
        //    List<MenuItem> itms1 = new List<MenuItem>
        //    {
        //        mn,
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateNotes"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx1.Items = itms1;

        //    grd1.ContextMenu = cntx1;

        //    maingrid.Children.Add(grd1);

        //    Grid? topPnl23 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.5, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.SetValue(Grid.RowProperty, 5);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "ФИО исполнителя:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 3, 30, "DataContext.Storage.FIOexecutor", 180, scp, "ФИО исполнителя...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Должность:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.GradeExecutor", 95, scp, "Должность...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 4, 30, "Телефон:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 5, 30, "DataContext.Storage.ExecPhone", 95, scp, "Телефон...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 6, 30, "Электронная почта:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 7, 30, "DataContext.Storage.ExecEmail", 130, scp, "Электронная почта...", true));
        //    maingrid.Children.Add(topPnl23);

        //    return vw;
        //}
        //public static Control Form16_Visual(INameScope scp)
        //{
        //    ScrollViewer vw = new ScrollViewer(); vw.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
        //    StackPanel maingrid = new StackPanel();
        //    vw.Content = maingrid;

        //    Grid? topPnl1 = new Grid();
        //    ColumnDefinition? column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    topPnl1.SetValue(Grid.RowProperty, 0);
        //    topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Дата окончания предыдущего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "DataContext.Storage.StartPeriod", 150, scp));
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "Дата окончания настоящего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 3, 30, "DataContext.Storage.EndPeriod", 150, scp));
        //    maingrid.Children.Add(topPnl1);

        //    Grid? topPnl2 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition();
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl2.SetValue(Grid.RowProperty, 1);
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
        //    topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        //    topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
        //    topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

        //    maingrid.Children.Add(topPnl2);

        //    Controls.DataGrid.DataGrid<Form16> grd = new Controls.DataGrid.DataGrid<Form16>
        //    {
        //        Name = "Form16Data_",
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd.SetValue(Grid.RowProperty, 2);

        //    Binding b = new Binding
        //    {
        //        Path = "DataContext.Storage.Rows16",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd.Bind(Controls.DataGrid.DataGrid<Form16>.ItemsProperty, b);


        //    ContextMenu? cntx = new ContextMenu();
        //    List<MenuItem> itms = new List<MenuItem>
        //    {
        //        new MenuItem
        //        {
        //            Header = "Добавить строку",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRow"),
        //        },
        //                        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateRowsx1"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк перед",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+I"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRowIn"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx.Items = itms;

        //    grd.ContextMenu = cntx;

        //    maingrid.Children.Add(grd);
        //    Grid? topPnl22 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl22.ColumnDefinitions.Add(column);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl22.SetValue(Grid.RowProperty, 3);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl22.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Примечания:"));
        //    maingrid.Children.Add(topPnl22);
        //    Controls.DataGrid.DataGrid<Note> grd1 = new Controls.DataGrid.DataGrid<Note>()
        //    {
        //        Name = "Form11Notes_",
        //        Focusable = true,
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd1.SetValue(Grid.RowProperty, 4);

        //    Binding b1 = new Binding
        //    {
        //        Path = "DataContext.Storage.Notes",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd1.Bind(Controls.DataGrid.DataGrid<Note>.ItemsProperty, b1);


        //    ContextMenu? cntx1 = new ContextMenu();
        //    var mn = new MenuItem
        //    {
        //        Header = "Добавить строку",
        //        InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //        [!MenuItem.CommandProperty] = new Binding("AddNote"),
        //        CommandParameter = "1.1*"
        //    };
        //    List<MenuItem> itms1 = new List<MenuItem>
        //    {
        //        mn,
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateNotes"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx1.Items = itms1;

        //    grd1.ContextMenu = cntx1;

        //    maingrid.Children.Add(grd1);

        //    Grid? topPnl23 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.5, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.SetValue(Grid.RowProperty, 5);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "ФИО исполнителя:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 3, 30, "DataContext.Storage.FIOexecutor", 180, scp, "ФИО исполнителя...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Должность:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.GradeExecutor", 95, scp, "Должность...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 4, 30, "Телефон:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 5, 30, "DataContext.Storage.ExecPhone", 95, scp, "Телефон...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 6, 30, "Электронная почта:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 7, 30, "DataContext.Storage.ExecEmail", 130, scp, "Электронная почта...", true));
        //    maingrid.Children.Add(topPnl23);

        //    return vw;
        //}
        //public static Control Form17_Visual(INameScope scp)
        //{
        //    ScrollViewer vw = new ScrollViewer(); vw.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
        //    StackPanel maingrid = new StackPanel();
        //    vw.Content = maingrid;

        //    Grid? topPnl1 = new Grid();
        //    ColumnDefinition? column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    topPnl1.SetValue(Grid.RowProperty, 0);
        //    topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Дата окончания предыдущего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "DataContext.Storage.StartPeriod", 150, scp));
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "Дата окончания настоящего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 3, 30, "DataContext.Storage.EndPeriod", 150, scp));
        //    maingrid.Children.Add(topPnl1);

        //    Grid? topPnl2 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition();
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl2.SetValue(Grid.RowProperty, 1);
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
        //    topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        //    topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
        //    topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

        //    maingrid.Children.Add(topPnl2);

        //    Controls.DataGrid.DataGrid<Form17> grd = new Controls.DataGrid.DataGrid<Form17>
        //    {
        //        Name = "Form17Data_",
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd.SetValue(Grid.RowProperty, 2);

        //    Binding b = new Binding
        //    {
        //        Path = "DataContext.Storage.Rows17",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd.Bind(Controls.DataGrid.DataGrid<Form17>.ItemsProperty, b);


        //    ContextMenu? cntx = new ContextMenu();
        //    List<MenuItem> itms = new List<MenuItem>
        //    {
        //        new MenuItem
        //        {
        //            Header = "Добавить строку",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRow"),
        //        },
        //                        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateRowsx1"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк перед",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+I"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRowIn"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx.Items = itms;

        //    grd.ContextMenu = cntx;

        //    maingrid.Children.Add(grd);
        //    Grid? topPnl22 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl22.ColumnDefinitions.Add(column);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl22.SetValue(Grid.RowProperty, 3);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl22.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Примечания:"));
        //    maingrid.Children.Add(topPnl22);
        //    Controls.DataGrid.DataGrid<Note> grd1 = new Controls.DataGrid.DataGrid<Note>()
        //    {
        //        Name = "Form11Notes_",
        //        Focusable = true,
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd1.SetValue(Grid.RowProperty, 4);

        //    Binding b1 = new Binding
        //    {
        //        Path = "DataContext.Storage.Notes",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd1.Bind(Controls.DataGrid.DataGrid<Note>.ItemsProperty, b1);


        //    ContextMenu? cntx1 = new ContextMenu();
        //    var mn = new MenuItem
        //    {
        //        Header = "Добавить строку",
        //        InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //        [!MenuItem.CommandProperty] = new Binding("AddNote"),
        //        CommandParameter = "1.1*"
        //    };
        //    List<MenuItem> itms1 = new List<MenuItem>
        //    {
        //        mn,
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateNotes"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx1.Items = itms1;

        //    grd1.ContextMenu = cntx1;

        //    maingrid.Children.Add(grd1);

        //    Grid? topPnl23 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.5, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.SetValue(Grid.RowProperty, 5);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "ФИО исполнителя:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 3, 30, "DataContext.Storage.FIOexecutor", 180, scp,"ФИО исполнителя...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Должность:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.GradeExecutor", 95, scp, "Должность...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 4, 30, "Телефон:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 5, 30, "DataContext.Storage.ExecPhone", 95, scp, "Телефон...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 6, 30, "Электронная почта:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 7, 30, "DataContext.Storage.ExecEmail", 130, scp,  "Электронная почта...", true));

        //    return vw;
        //}
        //public static Control Form18_Visual(INameScope scp)
        //{
        //    ScrollViewer vw = new ScrollViewer(); vw.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
        //    StackPanel maingrid = new StackPanel();
        //    vw.Content = maingrid;

        //    Grid? topPnl1 = new Grid();
        //    ColumnDefinition? column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    topPnl1.SetValue(Grid.RowProperty, 0);
        //    topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Дата окончания предыдущего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "DataContext.Storage.StartPeriod", 150, scp));
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "Дата окончания настоящего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 3, 30, "DataContext.Storage.EndPeriod", 150, scp));
        //    maingrid.Children.Add(topPnl1);

        //    Grid? topPnl2 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition();
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl2.SetValue(Grid.RowProperty, 1);
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
        //    topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        //    topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
        //    topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

        //    maingrid.Children.Add(topPnl2);

        //    Controls.DataGrid.DataGrid<Form18> grd = new Controls.DataGrid.DataGrid<Form18>
        //    {
        //        Name = "Form18Data_",
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd.SetValue(Grid.RowProperty, 2);

        //    Binding b = new Binding
        //    {
        //        Path = "DataContext.Storage.Rows18",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd.Bind(Controls.DataGrid.DataGrid<Form18>.ItemsProperty, b);


        //    ContextMenu? cntx = new ContextMenu();
        //    List<MenuItem> itms = new List<MenuItem>
        //    {
        //        new MenuItem
        //        {
        //            Header = "Добавить строку",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRow"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateRowsx1"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк перед",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+I"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRowIn"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx.Items = itms;

        //    grd.ContextMenu = cntx;

        //    maingrid.Children.Add(grd);
        //    Grid? topPnl22 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl22.ColumnDefinitions.Add(column);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl22.SetValue(Grid.RowProperty, 3);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl22.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Примечания:"));
        //    maingrid.Children.Add(topPnl22);
        //    Controls.DataGrid.DataGrid<Note> grd1 = new Controls.DataGrid.DataGrid<Note>()
        //    {
        //        Name = "Form11Notes_",
        //        Focusable = true,
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd1.SetValue(Grid.RowProperty, 4);

        //    Binding b1 = new Binding
        //    {
        //        Path = "DataContext.Storage.Notes",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd1.Bind(Controls.DataGrid.DataGrid<Note>.ItemsProperty, b1);


        //    ContextMenu? cntx1 = new ContextMenu();
        //    var mn = new MenuItem
        //    {
        //        Header = "Добавить строку",
        //        InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //        [!MenuItem.CommandProperty] = new Binding("AddNote"),
        //        CommandParameter = "1.1*"
        //    };
        //    List<MenuItem> itms1 = new List<MenuItem>
        //    {
        //        mn,
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateNotes"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx1.Items = itms1;

        //    grd1.ContextMenu = cntx1;

        //    maingrid.Children.Add(grd1);

        //    Grid? topPnl23 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.5, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.SetValue(Grid.RowProperty, 5);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "ФИО исполнителя:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 3, 30, "DataContext.Storage.FIOexecutor", 180, scp, "ФИО исполнителя...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Должность:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.GradeExecutor", 95, scp, "Должность...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 4, 30, "Телефон:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 5, 30, "DataContext.Storage.ExecPhone", 95, scp, "Телефон...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 6, 30, "Электронная почта:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 7, 30, "DataContext.Storage.ExecEmail", 130, scp, "Электронная почта...", true));
        //    maingrid.Children.Add(topPnl23);

        //    return vw;
        //}
        //public static Control Form19_Visual(INameScope scp)
        //{
        //    ScrollViewer vw = new ScrollViewer(); vw.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
        //    StackPanel maingrid = new StackPanel();
        //    vw.Content = maingrid;

        //    Grid? topPnl1 = new Grid();
        //    ColumnDefinition? column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl1.ColumnDefinitions.Add(column);
        //    topPnl1.SetValue(Grid.RowProperty, 0);
        //    topPnl1.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Дата окончания предыдущего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 1, 30, "DataContext.Storage.StartPeriod", 150, scp));
        //    topPnl1.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "Дата окончания настоящего отчетного периода:"));
        //    topPnl1.Children.Add(CreateTextBox("5,0,0,0", 3, 30, "DataContext.Storage.EndPeriod", 150, scp));
        //    maingrid.Children.Add(topPnl1);

        //    Grid? topPnl2 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl2.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition();
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl2.SetValue(Grid.RowProperty, 1);
        //    topPnl2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl2.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Номер корректировки:"));
        //    topPnl2.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        //    topPnl2.Children.Add(CreateButton("Проверить", "5,12,0,0", 2, 30, "CheckReport"));
        //    topPnl2.Children.Add(CreateButton("Сохранить", "5,12,0,0", 3, 30, "SaveReport"));

        //    maingrid.Children.Add(topPnl2);

        //    Controls.DataGrid.DataGrid<Form19> grd = new Controls.DataGrid.DataGrid<Form19>
        //    {
        //        Name = "Form19Data_",
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd.SetValue(Grid.RowProperty, 2);

        //    Binding b = new Binding
        //    {
        //        Path = "DataContext.Storage.Rows19",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd.Bind(Controls.DataGrid.DataGrid<Form19>.ItemsProperty, b);


        //    ContextMenu? cntx = new ContextMenu();
        //    List<MenuItem> itms = new List<MenuItem>
        //    {
        //        new MenuItem
        //        {
        //            Header = "Добавить строку",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRow"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateRowsx1"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк перед",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+I"),
        //            [!MenuItem.CommandProperty] = new Binding("AddRowIn"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteRow"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx.Items = itms;

        //    grd.ContextMenu = cntx;

        //    maingrid.Children.Add(grd);
        //    Grid? topPnl22 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl22.ColumnDefinitions.Add(column);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl22.SetValue(Grid.RowProperty, 3);
        //    topPnl22.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl22.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;

        //    topPnl22.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Примечания:"));
        //    maingrid.Children.Add(topPnl22);

        //    Controls.DataGrid.DataGrid<Note> grd1 = new Controls.DataGrid.DataGrid<Note>()
        //    {
        //        Name = "Form11Notes_",
        //        Focusable = true,
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        MultilineMode = MultilineMode.Multi,
        //        ChooseMode = ChooseMode.Cell,
        //        ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
        //        MaxHeight = 700
        //    };
        //    grd1.SetValue(Grid.RowProperty, 4);

        //    Binding b1 = new Binding
        //    {
        //        Path = "DataContext.Storage.Notes",
        //        ElementName = "ChangingPanel",
        //        NameScope = new WeakReference<INameScope>(scp)
        //    };
        //    grd1.Bind(Controls.DataGrid.DataGrid<Note>.ItemsProperty, b1);


        //    ContextMenu? cntx1 = new ContextMenu();
        //    var mn = new MenuItem
        //    {
        //        Header = "Добавить строку",
        //        InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+A"),
        //        [!MenuItem.CommandProperty] = new Binding("AddNote"),
        //        CommandParameter = "1.1*"
        //    };
        //    mn.SetValue(MenuItem.CommandParameterProperty, "1.1*");
        //    List<MenuItem> itms1 = new List<MenuItem>
        //    {
        //        mn,
        //        new MenuItem
        //        {
        //            Header = "Добавить N строк",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+N"),
        //            [!MenuItem.CommandProperty] = new Binding("DuplicateNotes"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Копировать",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+C"),
        //            [!MenuItem.CommandProperty] = new Binding("CopyRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Вставить",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+V"),
        //            [!MenuItem.CommandProperty] = new Binding("PasteRows"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedCells"),
        //        },
        //        new MenuItem
        //        {
        //            Header = "Удалить строки",
        //            InputGesture = Avalonia.Input.KeyGesture.Parse("Ctrl+D"),
        //            [!MenuItem.CommandProperty] = new Binding("DeleteNote"),
        //            [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
        //        }
        //    };
        //    cntx1.Items = itms1;

        //    grd1.ContextMenu = cntx1;

        //    maingrid.Children.Add(grd1);

        //    Grid? topPnl23 = new Grid();
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(2, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.5, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    column = new ColumnDefinition
        //    {
        //        Width = new GridLength(1.4, GridUnitType.Star)
        //    };
        //    topPnl23.ColumnDefinitions.Add(column);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.SetValue(Grid.RowProperty, 5);
        //    topPnl23.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        //    topPnl23.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 2, 30, "ФИО исполнителя:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 3, 30, "DataContext.Storage.FIOexecutor", 180, scp, "ФИО исполнителя...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 0, 30, "Должность:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 1, 30, "DataContext.Storage.GradeExecutor", 95, scp, "Должность...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 4, 30, "Телефон:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 5, 30, "DataContext.Storage.ExecPhone", 95, scp, "Телефон...", true));
        //    topPnl23.Children.Add(CreateTextBlock("5,13,0,0", 6, 30, "Электронная почта:"));
        //    topPnl23.Children.Add(CreateTextBox("5,12,0,0", 7, 30, "DataContext.Storage.ExecEmail", 130, scp , "Электронная почта...", true));
        //    maingrid.Children.Add(topPnl23);

        //    return vw;
        //}
    }
}
