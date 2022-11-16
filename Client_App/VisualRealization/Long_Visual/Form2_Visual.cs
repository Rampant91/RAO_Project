using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using System;
using System.Linq;
using Avalonia.Layout;
using Client_App.Controls.DataGrid;
using Avalonia.Media;
using Models.Attributes;
using Client_App.Converters;
using Avalonia.Controls.Primitives;
using Client_App.Controls.DataGrid.DataGrids;
using Client_App.ViewModels;

namespace Client_App.Long_Visual;

public class Form2_Visual
{
    public static ChangeOrCreateVM tmpVM { get; set; }
    public static Button CreateButton(string content, string thickness, int height, string commProp)
    {
        return new Button
        {
            Height = height,
            Margin = Thickness.Parse(thickness),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Content = content,
            [!Button.CommandProperty] = new Binding(commProp)
        };
    }

    public static ToggleSwitch CreateToggleSwitch(string content, string thickness, int height, string commProp)
    {
        var a = new ToggleSwitch
        {
            Height = height,
            Margin = Thickness.Parse(thickness),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            IsChecked = tmpVM.isSum
        };
        a.Checked += tmpVM._SumRow;
        a.Unchecked += tmpVM._CancelSumRow;
        return a;
    }

    public static Cell CreateTextBox(string thickness, int height, string textProp, double width, INameScope scp, string _flag = "")
    {
        Cell textCell = new()
        {
            Width = width,
            Margin = Thickness.Parse(thickness),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        Binding b = new()
        {
            Path = textProp,
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        if (_flag == "")
        {
            textCell.Control = new TextBox()
            {
                [!StyledElement.DataContextProperty] = b,
                [!TextBox.TextProperty] = new Binding("Value")
            };
        }
        if (_flag == "phone")
        {
            textCell.Control = new MaskedTextBox()
            {
                [!StyledElement.DataContextProperty] = b,
                [!TextBox.TextProperty] = new Binding("Value"),
            };
            ((MaskedTextBox)textCell.Control).Mask = "+7 (000) 000-00-00";
        }
        if (_flag == "year")
        {
            textCell.Control = new TextBox()
            {
                [!StyledElement.DataContextProperty] = b,
                [!TextBox.TextProperty] = new Binding("Value"),
            };
            //textCell.Control = new MaskedTextBox()
            //{
            //    [!MaskedTextBox.DataContextProperty] = b,
            //    [!MaskedTextBox.TextProperty] = new Binding("Value"),
            //};
            //((MaskedTextBox)textCell.Control).Mask = "0000";
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
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = text
            };
        }
        else
        {
            tmp = new TextBlock
            {
                Height = height,
                Margin = Thickness.Parse(margin),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = text
            };
        }
        return tmp;
    }

    static Grid Create20Item(string Property, string BindingPrefix, INameScope scp, int index)
    {
        Grid itemStackPanel = new();
        itemStackPanel.ColumnDefinitions.Add(new ColumnDefinition {Width=GridLength.Parse("1*")});
        itemStackPanel.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Parse("1*")});

        var tmp1 = CreateTextBlock("5,0,0,0", 30, ((Form_PropertyAttribute)Type.GetType("Models.Form20,Models").GetProperty(Property).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[index], 0);
        tmp1.SetValue(Grid.ColumnProperty,0);
        var tmp2 = CreateTextBox("5,0,0,0", 30, $"{BindingPrefix}[{index}].{Property}", 400, scp);
        tmp2.SetValue(Grid.ColumnProperty, 1);
        itemStackPanel.Children.Add(tmp1);
        itemStackPanel.Children.Add(tmp2);
        return itemStackPanel;
    }

    public static Control Form20_Visual(INameScope scp)
    {
        var BindingPrefix = "DataContext.Storage.Rows20";

        ScrollViewer vw = new()
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Visible
        };

        #region Main
        Panel mainPanel = new();
        var mainCanvas = new Canvas
        {
            Height = 1223
        };
        mainPanel.Children.Add(mainCanvas);
        vw.Content = mainPanel;

        #endregion

        #region Header
        Panel headerPanel = new() { Width = 735 };
        Binding b = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginTop_Converter()
        };
        Border brdH = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            ZIndex = 999,
            Background = new SolidColorBrush(Color.Parse("White")),
            [!Layoutable.MarginProperty] = b,
            Child = headerPanel
        };
        mainCanvas.Children.Add(brdH);

        StackPanel headerStackPanel = new()
        {
            Orientation = Orientation.Vertical
        };
        headerPanel.Children.Add(headerStackPanel);

        Grid headerOrganUprav = new();
        headerOrganUprav.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });
        headerOrganUprav.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });

        headerStackPanel.Children.Add(headerOrganUprav);
        var tmp1 = CreateTextBlock("5,0,0,0", 30,
            ((Form_PropertyAttribute)Type.GetType("Models.Form20,Models").GetProperty("OrganUprav")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[0]
            , 0);
        tmp1.SetValue(Grid.ColumnProperty, 0);
        headerOrganUprav.Children.Add(tmp1);

        var tmp2 = CreateTextBox("5,0,0,0", 30, $"{BindingPrefix}[0].OrganUprav", 400, scp);
        tmp2.SetValue(Grid.ColumnProperty, 1);
        headerOrganUprav.Children.Add(tmp2);

        Grid headerRegNo = new();
        headerRegNo.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });
        headerRegNo.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });

        headerStackPanel.Children.Add(headerRegNo);

        tmp1 = CreateTextBlock("5,0,0,0", 30,
            ((Form_PropertyAttribute)Type.GetType("Models.Form20,Models").GetProperty("RegNo")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[0]
            , 0);
        tmp1.SetValue(Grid.ColumnProperty, 0);
        headerRegNo.Children.Add(tmp1);

        tmp2 = CreateTextBox("5,0,0,0", 30, $"{BindingPrefix}[0].RegNo", 400, scp);
        tmp2.SetValue(Grid.ColumnProperty, 1);
        headerRegNo.Children.Add(tmp2);

        StackPanel headerButtons = new()
        {
            Orientation = Orientation.Horizontal
        };
        headerStackPanel.Children.Add(headerButtons);
        headerButtons.Children.Add(CreateButton("Поменять местами", "5,5,0,0", 30, "ChangeReportOrder"));
        headerButtons.Children.Add(CreateButton("Проверить", "5,5,0,0", 30, "CheckReport"));
        headerButtons.Children.Add(CreateButton("Сохранить", "5,5,0,0", 30, "SaveReport"));
        #endregion

        StackPanel centerStackPanel = new();
        centerStackPanel.SetValue(Canvas.TopProperty, 105);
        centerStackPanel.Orientation = Orientation.Vertical;
        mainCanvas.Children.Add(centerStackPanel);

        #region UrLico
        Panel urLicoPanel = new();
        Border brdUr = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = urLicoPanel
        };
        centerStackPanel.Children.Add(brdUr);

        StackPanel urLicoStackPanel = new()
        {
            Orientation = Orientation.Vertical
        };
        urLicoPanel.Children.Add(urLicoStackPanel);

        urLicoStackPanel.Children.Add(new TextBlock
        {
            Height = 30,
            Margin = Thickness.Parse("0,0,0,0"),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            TextAlignment = TextAlignment.Center,
            FontWeight = FontWeight.Bold,
            FontSize = 16,
            Text = "Юридическое лицо",
        });
        urLicoStackPanel.Children.Add(Create20Item("SubjectRF", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("JurLico", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("ShortJurLico", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("JurLicoAddress", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("JurLicoFactAddress", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("GradeFIO", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("Telephone", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("Fax", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("Email", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("Okpo", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("Okved", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("Okogu", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("Oktmo", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("Inn", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("Kpp", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("Okopf", BindingPrefix, scp, 0));
        urLicoStackPanel.Children.Add(Create20Item("Okfs", BindingPrefix, scp, 0));
        #endregion

        #region ObosobPodrazd
        Panel obosobPodrazdPanel = new();
        Border brdOb = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = obosobPodrazdPanel
        };
        centerStackPanel.Children.Add(brdOb);

        StackPanel obosobPodrazdStackPanel = new()
        {
            Orientation = Orientation.Vertical
        };
        obosobPodrazdPanel.Children.Add(obosobPodrazdStackPanel);

        obosobPodrazdStackPanel.Children.Add(new TextBlock
        {
            Height = 30,
            Margin = Thickness.Parse("0,0,0,0"),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            TextAlignment = TextAlignment.Center,
            FontWeight = FontWeight.Bold,
            FontSize = 16,
            Text = "Обособленное подразделение",
        });
        obosobPodrazdStackPanel.Children.Add(Create20Item("SubjectRF", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("JurLico", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("ShortJurLico", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("JurLicoAddress", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("JurLicoFactAddress", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("GradeFIO", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("Telephone", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("Fax", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("Email", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("Okpo", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("Okved", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("Okogu", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("Oktmo", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("Inn", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("Kpp", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("Okopf", BindingPrefix, scp, 1));
        obosobPodrazdStackPanel.Children.Add(Create20Item("Okfs", BindingPrefix, scp, 1));
        #endregion

        return vw;
    }

    public static Control Form21_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            [!Layoutable.MarginProperty] = ind
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,20,0,5", 30, "Суммирование:"));
        var testS = CreateToggleSwitch("Суммировать", "65,0,0,15", 30, "SumRow");
        content.Children.Add(testS);
        leftStPT.Children.Add(content);
        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm21 grd = new()
        {
            Name = "Form21Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm21.FixedContentProperty] = ind,
            [!DataGridForm21.IsReadableSumProperty] = testS[!ToggleButton.IsCheckedProperty]
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm21.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows21",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm21.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }

    public static Control Form22_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            [!Layoutable.MarginProperty] = ind
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,20,0,5", 30, "Суммирование:"));
        var testS = CreateToggleSwitch("Суммировать", "65,0,0,15", 30, "SumRow");
        content.Children.Add(testS);
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm22 grd = new()
        {
            Name = "Form22Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm22.FixedContentProperty] = ind,
            [!DataGridForm22.IsReadableSumProperty] = testS[!ToggleButton.IsCheckedProperty]
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm22.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows22",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm22.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }

    public static Control Form23_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            [!Layoutable.MarginProperty] = ind
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm23 grd = new()
        {
            Name = "Form23Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm23.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm23.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows23",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm23.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }

    public static Control Form24_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            [!Layoutable.MarginProperty] = ind
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm24 grd = new()
        {
            Name = "Form24Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm24.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm24.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows24",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm24.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }

    public static Control Form25_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            [!Layoutable.MarginProperty] = ind
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm25 grd = new()
        {
            Name = "Form25Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm25.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm25.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows25",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm25.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }

    public static Control Form26_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            [!Layoutable.MarginProperty] = ind
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Количество наблюдательных скважин, принадлежащих организации:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.SourcesQuantity26", 100, scp));

        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm26 grd = new()
        {
            Name = "Form26Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm26.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm26.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows26",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm26.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }

    public static Control Form27_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Разрешение на допустимые выбросы радионуклидов в атмосферу №"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.PermissionNumber27", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "от"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.PermissionIssueDate27", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, ". Срок действия с"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ValidBegin27", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "по"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ValidThru27", 100, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Наименование разрешительного документа на допустимые выбросы радионуклидов в атмосферу:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.PermissionDocumentName27", 600, scp));
        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm27 grd = new()
        {
            Name = "Form27Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm27.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm27.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows27",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm27.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }

    public static Control Form28_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Bottom
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Разрешение на сброс радионуклидов в водные объекты №", 350));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.PermissionNumber_28", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "от"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.PermissionIssueDate_28", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, ". Срок действия с"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ValidBegin_28", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "по"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ValidThru_28", 100, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Наименование разрешительного документа на сброс:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.PermissionDocumentName_28", 600, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Разрешение на сброс радионуклидов на рельеф местности №", 350));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.PermissionNumber1_28", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "от"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.PermissionIssueDate1_28", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, ". Срок действия с"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ValidBegin1_28", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "по"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ValidThru1_28", 100, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Наименование разрешительного документа на сброс:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.PermissionDocumentName1_28", 600, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Договор на передачу сточных вод в сети канализации №", 350));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ContractNumber_28", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "от"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ContractIssueDate2_28", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, ". Срок действия с"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ValidBegin2_28", 100, scp));
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "по"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ValidThru2_28", 100, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Организация, осуществляющая прием сточных вод:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.OrganisationReciever_28", 100, scp));
        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm28 grd = new()
        {
            Name = "Form28Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm28.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm28.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows28",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm28.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }

    public static Control Form29_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            [!Layoutable.MarginProperty] = ind
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm29 grd = new()
        {
            Name = "Form29Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm29.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm29.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows29",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm29.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }

    public static Control Form210_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            [!Layoutable.MarginProperty] = ind
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm210 grd = new()
        {
            Name = "Form210Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm210.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm210.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows210",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm210.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }

    public static Control Form211_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            [!Layoutable.MarginProperty] = ind
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm211 grd = new()
        {
            Name = "Form211Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm211.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm211.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows211",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm211.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }

    public static Control Form212_Visual(INameScope scp)
    {
        ScrollViewer vw = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };
        StackPanel maingrid = new();
        vw.Content = maingrid;
        Binding ind = new()
        {
            Source = vw,
            Path = "Offset",
            Converter = new VectorToMarginLeft_Converter()
        };

        #region Top

        StackPanel? topPnl1 = new()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            [!Layoutable.MarginProperty] = ind
        };


        #region Left

        StackPanel leftStPT = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")

        };
        StackPanel? content = new()
        {
            Orientation = Orientation.Horizontal
        };

        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Отчетный год:               "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.Year", 70, scp, "year"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Номер корректировки:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.CorrectionNumber", 70, scp));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Проверить", "5,5,0,5", 30, "CheckReport"));
        content.Children.Add(CreateButton("Сохранить", "5,5,0,5", 30, "SaveReport"));

        leftStPT.Children.Add(content);

        Border brdC = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            [Grid.RowProperty] = 1,
            Margin = Thickness.Parse("5,5,5,5"),
            Child = leftStPT
        };

        #endregion

        #region Right
        StackPanel rigthStP = new()
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse("0,12,0,0")
        };

        Border brdR = new()
        {
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(4),
            Margin = Thickness.Parse("5,5,5,5"),
            Child = rigthStP
        };

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "ФИО исполнителя:"));
        content.Children.Add(CreateTextBox("10,0,0,0", 30, "DataContext.Storage.FIOexecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Должность:"));
        content.Children.Add(CreateTextBox("49,0,0,0", 30, "DataContext.Storage.GradeExecutor", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Телефон:"));
        content.Children.Add(CreateTextBox("64,0,0,0", 30, "DataContext.Storage.ExecPhone", 180, scp, ""));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Электронная почта:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.ExecEmail", 180, scp));
        rigthStP.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateButton("Скопировать данные предыдущей формы", "5,0,0,3", 30, "CopyExecutorData"));
        rigthStP.Children.Add(content);
        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        maingrid.Children.Add(topPnl1);
        #endregion

        #region Centre
        DataGridForm212 grd = new()
        {
            Name = "Form212Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm212.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm212.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows212",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm212.ItemsProperty, b);

        maingrid.Children.Add(grd);

        Grid? topPnl22 = new()
        {
            Margin = Thickness.Parse("5,0,0,0")
        };
        var column = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };
        topPnl22.ColumnDefinitions.Add(column);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;
        topPnl22.SetValue(Grid.RowProperty, 3);
        topPnl22.HorizontalAlignment = HorizontalAlignment.Left;
        topPnl22.VerticalAlignment = VerticalAlignment.Top;

        maingrid.Children.Add(topPnl22);
        #endregion

        #region Bot
        Panel prt = new()
        {
            [Grid.ColumnProperty] = 4,
            [!Layoutable.MarginProperty] = ind
        };
        DataGridNote grd1 = new()
        {
            Name = "Form21Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,5,0,0")

        };

        prt.Children.Add(grd1);

        Binding b1 = new()
        {
            Path = "DataContext.Storage.Notes",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd1.Bind(DataGridNote.ItemsProperty, b1);

        maingrid.Children.Add(prt);
        #endregion

        return vw;
    }
}