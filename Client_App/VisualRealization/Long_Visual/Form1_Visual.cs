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

        public static Cell CreateTextBox(string thickness, int height, string textProp, double width, INameScope scp, string _flag = "")
        {
            Cell textCell = new Cell()
            {
                Width = width,
                Margin = Thickness.Parse(thickness),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right
            };
            Binding b = new Binding
            {
                Path = textProp,
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            if (_flag == "")
            {
                textCell.Control = new TextBox()
                {
                    [!TextBox.DataContextProperty] = b,
                    [!TextBox.TextProperty] = new Binding("Value")
                };
            }
            if(_flag == "phone")
            { 
                textCell.Control = new MaskedTextBox()
                {
                    [!MaskedTextBox.DataContextProperty] = b,
                    [!MaskedTextBox.TextProperty] = new Binding("Value"),
                };
                ((MaskedTextBox)textCell.Control).Mask = "+7 (000) 000-00-00";
            }
            if (_flag == "date")
            {
                textCell.Control = new MaskedTextBox()
                {
                    [!MaskedTextBox.DataContextProperty] = b,
                    [!MaskedTextBox.TextProperty] = new Binding("Value"),
                };
                ((MaskedTextBox)textCell.Control).Mask = "99/99/9999";
            }
            return textCell;
        }

        public static TextBlock CreateTextBlock(string margin, int height, string text, double width = 0)
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
            else
            {
                tmp = new TextBlock
                {
                    Height = height,
                    Margin = Thickness.Parse(margin),
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                    Text = text
                };
            }
            return tmp;
        }

        static Grid Create10Item(string Property, string BindingPrefix, INameScope scp, int index)
        {
            Grid itemStackPanel = new Grid();
            itemStackPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });
            itemStackPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });

            var tmp1 = CreateTextBlock("5,0,0,0", 30, ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").GetProperty(Property).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[index], 0);
            tmp1.SetValue(Grid.ColumnProperty, 0);
            var tmp2 = CreateTextBox("5,0,0,0", 30, BindingPrefix + "[" + index + "]." + Property, 400, scp);
            tmp2.SetValue(Grid.ColumnProperty, 1);
            itemStackPanel.Children.Add(tmp1);
            itemStackPanel.Children.Add(tmp2);
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
            mainCanvas.Height = 1223;
            mainPanel.Children.Add(mainCanvas);
            vw.Content = mainPanel;
            #endregion

            #region Header
            Panel headerPanel = new Panel() {Width=735};
            Binding b = new Binding()
            {
                Source = vw,
                Path = "Offset",
                Converter = new VectorToMarginTop_Converter()
            };
            Border brdH = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                ZIndex = 999,
                Background = new SolidColorBrush(Color.Parse("White"))
            };
            brdH[!Border.MarginProperty] = b;
            brdH.Child = headerPanel;
            mainCanvas.Children.Add(brdH);

            StackPanel headerStackPanel = new StackPanel();
            headerStackPanel.Orientation = Orientation.Vertical;
            headerPanel.Children.Add(headerStackPanel);

            Grid headerOrganUprav = new Grid();
            headerOrganUprav.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });
            headerOrganUprav.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });

            headerStackPanel.Children.Add(headerOrganUprav);
            var tmp1 = CreateTextBlock("5,0,0,0", 30,
                ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").GetProperty("OrganUprav")
                 .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[0]
                , 0);
            tmp1.SetValue(Grid.ColumnProperty, 0);
            headerOrganUprav.Children.Add(tmp1);

            var tmp2 = CreateTextBox("5,0,0,0", 30, BindingPrefix + "[0]." + "OrganUprav", 400, scp);
            tmp2.SetValue(Grid.ColumnProperty, 1);
            headerOrganUprav.Children.Add(tmp2);

            Grid headerRegNo = new Grid();
            headerRegNo.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });
            headerRegNo.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });

            headerStackPanel.Children.Add(headerRegNo);

            tmp1 = CreateTextBlock("5,0,0,0", 30,
                ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").GetProperty("RegNo")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[0]
                , 0);
            tmp1.SetValue(Grid.ColumnProperty, 0);
            headerRegNo.Children.Add(tmp1);

            tmp2 = CreateTextBox("5,0,0,0", 30, BindingPrefix + "[0]." + "RegNo", 400, scp);
            tmp2.SetValue(Grid.ColumnProperty, 1);
            headerRegNo.Children.Add(tmp2);

            StackPanel headerButtons = new StackPanel();
            headerButtons.Orientation = Orientation.Horizontal;
            headerStackPanel.Children.Add(headerButtons);
            headerButtons.Children.Add(CreateButton("Поменять местами", "5,5,0,0", 30, "ChangeReportOrder"));
            headerButtons.Children.Add(CreateButton("Проверить", "5,5,0,0", 30, "CheckReport"));
            headerButtons.Children.Add(CreateButton("Сохранить", "5,5,0,0", 30, "SaveReport"));
            #endregion

            StackPanel centerStackPanel = new StackPanel();
            centerStackPanel.SetValue(Canvas.TopProperty, 105);
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
            urLicoStackPanel.Children.Add(Create10Item("SubjectRF", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("JurLico", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("ShortJurLico", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("JurLicoAddress", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("JurLicoFactAddress", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("GradeFIO", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("Telephone", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("Fax", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("Email", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("Okpo", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("Okved", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("Okogu", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("Oktmo", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("Inn", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("Kpp", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("Okopf", BindingPrefix, scp, 0));
            urLicoStackPanel.Children.Add(Create10Item("Okfs", BindingPrefix, scp, 0));
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
            obosobPodrazdStackPanel.Children.Add(Create10Item("SubjectRF", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("JurLico", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("ShortJurLico", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("JurLicoAddress", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("JurLicoFactAddress", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("GradeFIO", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("Telephone", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("Fax", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("Email", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("Okpo", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("Okved", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("Okogu", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("Oktmo", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("Inn", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("Kpp", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("Okopf", BindingPrefix, scp, 1));
            obosobPodrazdStackPanel.Children.Add(Create10Item("Okfs", BindingPrefix, scp, 1));
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

            #region Top

            StackPanel? topPnl1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                [!StackPanel.MarginProperty] = ind
            };


            #region Left
            StackPanel leftStPT = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = Thickness.Parse("0,12,0,0")

            };
            StackPanel? content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
            content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));
            leftStPT.Children.Add(content);

            Border brdC = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                [Grid.RowProperty] = 1,
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdC.Child = leftStPT;
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
            content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, "phone"));
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

            #region Centre
            Controls.DataGrid.DataGridForm11 grd = new Controls.DataGrid.DataGridForm11()
            {
                Name = "Form11Data_",
                Focusable = true,
                Sum = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,0,0,0"),
                [!DataGridForm11.FixedContentProperty] = ind
            };
            grd.SetValue(Grid.RowProperty, 2);

            vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm11.ScrollLeftRightProperty];

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

            maingrid.Children.Add(topPnl22);
            #endregion

            #region Bot
            Panel prt = new Panel()
            {
                [Grid.ColumnProperty] = 4,
                [!Control.MarginProperty] = ind
            };
            Controls.DataGrid.DataGridNote grd1 = new Controls.DataGrid.DataGridNote()
            {
                Name = "Form11Notes_",
                Focusable = true,
                Comment = "Комментарии",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,5,0,0")

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
            #endregion

            return vw;
        }

        public static Control Form12_Visual(INameScope scp)
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

            #region Top

            StackPanel? topPnl1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                [!StackPanel.MarginProperty] = ind
            };


            #region Left

            StackPanel leftStPT = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = Thickness.Parse("0,12,0,0")

            };
            StackPanel? content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
            content.Children.Add(CreateButton("Сохранить", "5,0,0,0", 30, "SaveReport"));
            leftStPT.Children.Add(content);

            Border brdC = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                [Grid.RowProperty] = 1,
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdC.Child = leftStPT;
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
            content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, "phone"));
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

            #region Centre
            Controls.DataGrid.DataGridForm12 grd = new Controls.DataGrid.DataGridForm12()
            {
                Name = "Form12Data_",
                Focusable = true,
                Sum = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,0,0,0"),
                [!DataGridForm12.FixedContentProperty] = ind
            };
            grd.SetValue(Grid.RowProperty, 2);

            vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm12.ScrollLeftRightProperty];

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows12",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGridForm12.ItemsProperty, b);

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

            maingrid.Children.Add(topPnl22);
            #endregion

            #region Bot
            Panel prt = new Panel()
            {
                [Grid.ColumnProperty] = 4,
                [!Control.MarginProperty] = ind
            };
            Controls.DataGrid.DataGridNote grd1 = new Controls.DataGrid.DataGridNote()
            {
                Name = "Form11Notes_",
                Focusable = true,
                Comment = "Комментарии",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,5,0,0")

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
            #endregion

            return vw;
        }

        public static Control Form13_Visual(INameScope scp)
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

            #region Top

            StackPanel? topPnl1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                [!StackPanel.MarginProperty] = ind
            };


            #region Left

            StackPanel leftStPT = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = Thickness.Parse("0,12,0,0")

            };
            StackPanel? content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
            content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

            leftStPT.Children.Add(content);

            Border brdC = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                [Grid.RowProperty] = 1,
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdC.Child = leftStPT;
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
            content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, "phone"));
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

            #region Centre
            Controls.DataGrid.DataGridForm13 grd = new Controls.DataGrid.DataGridForm13()
            {
                Name = "Form13Data_",
                Focusable = true,
                Sum = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,0,0,0"),
                [!DataGridForm13.FixedContentProperty] = ind
            };
            grd.SetValue(Grid.RowProperty, 2);

            vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm13.ScrollLeftRightProperty];

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows13",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGridForm13.ItemsProperty, b);

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

            maingrid.Children.Add(topPnl22);
            #endregion

            #region Bot
            Panel prt = new Panel()
            {
                [Grid.ColumnProperty] = 4,
                [!Control.MarginProperty] = ind
            };
            Controls.DataGrid.DataGridNote grd1 = new Controls.DataGrid.DataGridNote()
            {
                Name = "Form11Notes_",
                Focusable = true,
                Comment = "Комментарии",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,5,0,0")

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
            #endregion

            return vw;
        }

        public static Control Form14_Visual(INameScope scp)
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

            #region Top

            StackPanel? topPnl1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                [!StackPanel.MarginProperty] = ind
            };


            #region Left

            StackPanel leftStPT = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = Thickness.Parse("0,12,0,0")

            };
            StackPanel? content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
            content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

            leftStPT.Children.Add(content);

            Border brdC = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                [Grid.RowProperty] = 1,
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdC.Child = leftStPT;
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
            content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, "phone"));
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

            #region Centre
            Controls.DataGrid.DataGridForm14 grd = new Controls.DataGrid.DataGridForm14()
            {
                Name = "Form14Data_",
                Focusable = true,
                Sum = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,0,0,0"),
                [!DataGridForm14.FixedContentProperty] = ind
            };
            grd.SetValue(Grid.RowProperty, 2);

            vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm14.ScrollLeftRightProperty];

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows14",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGridForm14.ItemsProperty, b);

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

            maingrid.Children.Add(topPnl22);
            #endregion

            #region Bot
            Panel prt = new Panel()
            {
                [Grid.ColumnProperty] = 4,
                [!Control.MarginProperty] = ind
            };
            Controls.DataGrid.DataGridNote grd1 = new Controls.DataGrid.DataGridNote()
            {
                Name = "Form11Notes_",
                Focusable = true,
                Comment = "Комментарии",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,5,0,0")

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
            #endregion

            return vw;
        }

        public static Control Form15_Visual(INameScope scp)
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

            #region Top

            StackPanel? topPnl1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                [!StackPanel.MarginProperty] = ind
            };


            #region Left

            StackPanel leftStPT = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = Thickness.Parse("0,12,0,0")

            };
            StackPanel? content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
            content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

            leftStPT.Children.Add(content);

            Border brdC = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                [Grid.RowProperty] = 1,
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdC.Child = leftStPT;
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
            content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, "phone"));
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

            #region Centre
            Controls.DataGrid.DataGridForm15 grd = new Controls.DataGrid.DataGridForm15()
            {
                Name = "Form15Data_",
                Focusable = true,
                Sum = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,0,0,0"),
                [!DataGridForm15.FixedContentProperty] = ind
            };
            grd.SetValue(Grid.RowProperty, 2);

            vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm15.ScrollLeftRightProperty];

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows15",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGridForm15.ItemsProperty, b);

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

            maingrid.Children.Add(topPnl22);
            #endregion

            #region Bot
            Panel prt = new Panel()
            {
                [Grid.ColumnProperty] = 4,
                [!Control.MarginProperty] = ind
            };
            Controls.DataGrid.DataGridNote grd1 = new Controls.DataGrid.DataGridNote()
            {
                Name = "Form11Notes_",
                Focusable = true,
                Comment = "Комментарии",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,5,0,0")

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
            #endregion

            return vw;
        }

        public static Control Form16_Visual(INameScope scp)
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

            #region Top

            StackPanel? topPnl1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                [!StackPanel.MarginProperty] = ind
            };


            #region Left

            StackPanel leftStPT = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = Thickness.Parse("0,12,0,0")

            };
            StackPanel? content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
            content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

            leftStPT.Children.Add(content);

            Border brdC = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                [Grid.RowProperty] = 1,
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdC.Child = leftStPT;
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
            content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, "phone"));
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

            #region Centre
            Controls.DataGrid.DataGridForm16 grd = new Controls.DataGrid.DataGridForm16()
            {
                Name = "Form16Data_",
                Focusable = true,
                Sum = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,0,0,0"),
                [!DataGridForm16.FixedContentProperty] = ind
            };
            grd.SetValue(Grid.RowProperty, 2);

            vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm16.ScrollLeftRightProperty];

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows16",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGridForm16.ItemsProperty, b);

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

            maingrid.Children.Add(topPnl22);
            #endregion

            #region Bot
            Panel prt = new Panel()
            {
                [Grid.ColumnProperty] = 4,
                [!Control.MarginProperty] = ind
            };
            Controls.DataGrid.DataGridNote grd1 = new Controls.DataGrid.DataGridNote()
            {
                Name = "Form11Notes_",
                Focusable = true,
                Comment = "Комментарии",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,5,0,0")

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
            #endregion


            return vw;
        }

        public static Control Form17_Visual(INameScope scp)
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

            #region Top

            StackPanel? topPnl1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                [!StackPanel.MarginProperty] = ind
            };


            #region Left

            StackPanel leftStPT = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = Thickness.Parse("0,12,0,0")

            };
            StackPanel? content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
            content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

            leftStPT.Children.Add(content);

            Border brdC = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                [Grid.RowProperty] = 1,
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdC.Child = leftStPT;
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
            content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, "phone"));
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

            #region Centre
            Controls.DataGrid.DataGridForm17 grd = new Controls.DataGrid.DataGridForm17()
            {
                Name = "Form17Data_",
                Focusable = true,
                Sum = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,0,0,0"),
                [!DataGridForm17.FixedContentProperty] = ind
            };
            grd.SetValue(Grid.RowProperty, 2);

            vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm17.ScrollLeftRightProperty];

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows17",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGridForm17.ItemsProperty, b);

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

            maingrid.Children.Add(topPnl22);
            #endregion

            #region Bot
            Panel prt = new Panel()
            {
                [Grid.ColumnProperty] = 4,
                [!Control.MarginProperty] = ind
            };
            Controls.DataGrid.DataGridNote grd1 = new Controls.DataGrid.DataGridNote()
            {
                Name = "Form11Notes_",
                Focusable = true,
                Comment = "Комментарии",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,5,0,0")

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
            #endregion

            return vw;
        }

        public static Control Form18_Visual(INameScope scp)
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

            #region Top

            StackPanel? topPnl1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                [!StackPanel.MarginProperty] = ind
            };


            #region Left

            StackPanel leftStPT = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = Thickness.Parse("0,12,0,0")

            };
            StackPanel? content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
            content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

            leftStPT.Children.Add(content);

            Border brdC = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                [Grid.RowProperty] = 1,
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdC.Child = leftStPT;
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
            content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, "phone"));
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

            #region Centre
            Controls.DataGrid.DataGridForm18 grd = new Controls.DataGrid.DataGridForm18()
            {
                Name = "Form18Data_",
                Focusable = true,
                Sum = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,0,0,0"),
                [!DataGridForm18.FixedContentProperty] = ind
            };
            grd.SetValue(Grid.RowProperty, 2);

            vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm18.ScrollLeftRightProperty];

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows18",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGridForm18.ItemsProperty, b);

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

            maingrid.Children.Add(topPnl22);
            #endregion

            #region Bot
            Panel prt = new Panel()
            {
                [Grid.ColumnProperty] = 4,
                [!Control.MarginProperty] = ind
            };
            Controls.DataGrid.DataGridNote grd1 = new Controls.DataGrid.DataGridNote()
            {
                Name = "Form11Notes_",
                Focusable = true,
                Comment = "Комментарии",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,5,0,0")

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
            #endregion


            return vw;
        }

        public static Control Form19_Visual(INameScope scp)
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

            #region Top

            StackPanel? topPnl1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                [!StackPanel.MarginProperty] = ind
            };


            #region Left

            StackPanel leftStPT = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Margin = Thickness.Parse("0,12,0,0")

            };
            StackPanel? content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
            content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
            leftStPT.Children.Add(content);

            content = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
            content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

            leftStPT.Children.Add(content);

            Border brdC = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4),
                [Grid.RowProperty] = 1,
                Margin = Thickness.Parse("5,5,5,5")
            };
            brdC.Child = leftStPT;
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
            content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, "phone"));
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

            #region Centre
            Controls.DataGrid.DataGridForm19 grd = new Controls.DataGrid.DataGridForm19()
            {
                Name = "Form12Data_",
                Focusable = true,
                Sum = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,0,0,0"),
                [!DataGridForm19.FixedContentProperty] = ind
            };
            grd.SetValue(Grid.RowProperty, 2);

            vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm19.ScrollLeftRightProperty];

            Binding b = new Binding
            {
                Path = "DataContext.Storage.Rows19",
                ElementName = "ChangingPanel",
                NameScope = new WeakReference<INameScope>(scp)
            };
            grd.Bind(Controls.DataGrid.DataGridForm19.ItemsProperty, b);

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

            maingrid.Children.Add(topPnl22);
            #endregion

            #region Bot
            Panel prt = new Panel()
            {
                [Grid.ColumnProperty] = 4,
                [!Control.MarginProperty] = ind
            };
            Controls.DataGrid.DataGridNote grd1 = new Controls.DataGrid.DataGridNote()
            {
                Name = "Form11Notes_",
                Focusable = true,
                Comment = "Комментарии",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = MultilineMode.Multi,
                ChooseMode = ChooseMode.Cell,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                MaxHeight = 700,
                Margin = Thickness.Parse("5,5,0,0")

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
            #endregion

            return vw;
        }
    }
}
