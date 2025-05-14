using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Client_App.Controls.DataGrid;
using Client_App.Controls.DataGrid.DataGrids;
using Client_App.Controls.MaskedTextBox;
using Client_App.VisualRealization.Converters;
using Models.Attributes;
using Models.DBRealization;

namespace Client_App.VisualRealization.Long_Visual;

public class Form1_Visual
{
    #region CreateButton
    
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

    #endregion

    #region CreateTextBox
    
    public static Cell CreateTextBox(string thickness, int height, string textProp, double width, INameScope scp, string _flag = "")
    {
        Cell textCell = new()
        {
            Width = width,
            Margin = Thickness.Parse(thickness),
            VerticalAlignment = VerticalAlignment.Top,
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
        if (_flag == "date")
        {
            textCell.Control = new MaskedTextBoxForDate()
            {
                [!StyledElement.DataContextProperty] = b,
                [!TextBox.TextProperty] = new Binding("Value"),
            };
            ((MaskedTextBox)textCell.Control).Mask = "99/99/9999";
        }
        return textCell;
    }

    #endregion

    #region CreateTextBlock
    
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

    #endregion

    #region Create10Item
    
    private static Grid Create10Item(string Property, string BindingPrefix, INameScope scp, int index)
    {
        Grid itemStackPanel = new();
        itemStackPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });
        itemStackPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });

        var tmp1 = CreateTextBlock("5,0,0,0", 30, ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form10,Models").GetProperty(Property).GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[index], 0);
        tmp1.SetValue(Grid.ColumnProperty, 0);
        var tmp2 = CreateTextBox("5,0,0,0", 30, $"{BindingPrefix}[{index}].{Property}", 400, scp);
        if (BindingPrefix == "DataContext.Storage.Rows10")
        {
            ((TextBox)tmp2.Control).HorizontalContentAlignment = HorizontalAlignment.Right;
        }
        tmp2.SetValue(Grid.ColumnProperty, 1);
        itemStackPanel.Children.Add(tmp1);
        itemStackPanel.Children.Add(tmp2);
        return itemStackPanel;
    }

    #endregion

    #region Forms1.x_Visual

    #region Form10_Visual

    public static Control Form10_Visual(INameScope scp)
    {
        var BindingPrefix = "DataContext.Storage.Rows10";

        ScrollViewer vw = new()
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
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
            ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form10,Models").GetProperty("OrganUprav")
                .GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[0], 0);
        tmp1.SetValue(Grid.ColumnProperty, 0);
        headerOrganUprav.Children.Add(tmp1);

        var tmp2 = CreateTextBox("5,0,0,0", 30, $"{BindingPrefix}[0].OrganUprav", 400, scp);
        ((TextBox)tmp2.Control).HorizontalContentAlignment = HorizontalAlignment.Right;
        tmp2.SetValue(Grid.ColumnProperty, 1);
        headerOrganUprav.Children.Add(tmp2);

        Grid headerRegNo = new();
        headerRegNo.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });
        headerRegNo.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Parse("1*") });

        headerStackPanel.Children.Add(headerRegNo);

        tmp1 = CreateTextBlock("5,0,0,0", 30,
            ((FormPropertyAttribute)Type.GetType("Models.Forms.Form1.Form10,Models").GetProperty("RegNo")
                .GetCustomAttributes(typeof(FormPropertyAttribute), false).First()).Names[0], 0);
        tmp1.SetValue(Grid.ColumnProperty, 0);
        headerRegNo.Children.Add(tmp1);

        tmp2 = CreateTextBox("5,0,0,0", 30, $"{BindingPrefix}[0].RegNo", 400, scp);
        ((TextBox)tmp2.Control).HorizontalContentAlignment = HorizontalAlignment.Right;
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
            Text = "Территориальное обособленное подразделение",
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

    #endregion

    #region Form11_Visual
    
    public static Control Form11_Visual(INameScope scp)
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
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
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

        #region Right2

        StackPanel rightStP2 = new()
        {
            Orientation = Orientation.Vertical,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = Thickness.Parse("5,5,5,10")
        };
        rightStP2.Children.Add(CreateButton("Перевести все РВ в РАО", "5,0,0,3", 30, "SourceTransmissionAll"));

        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        topPnl1.Children.Add(rightStP2);
        maingrid.Children.Add(topPnl1);

        #endregion

        #region Center

        DataGridForm11 grd = new()
        {
            Name = "Form11Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm11.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm11.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows11",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm11.ItemsProperty, b);
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
            Name = "Form11Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MinHeight = 175,
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
    
    #endregion

    #region Form12_Visual

    public static Control Form12_Visual(INameScope scp)
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
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
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
        content.Children.Add(CreateButton("Сохранить", "5,0,0,0", 30, "SaveReport"));
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

        #region Right2

        StackPanel rightStP2 = new()
        {
            Orientation = Orientation.Vertical,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = Thickness.Parse("5,5,5,10")
        };
        rightStP2.Children.Add(CreateButton("Перевести все РВ в РАО", "5,0,0,3", 30, "SourceTransmissionAll"));

        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        topPnl1.Children.Add(rightStP2);
        maingrid.Children.Add(topPnl1);

        #endregion

        #region Centre
        DataGridForm12 grd = new()
        {
            Name = "Form12Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm12.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm12.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows12",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm12.ItemsProperty, b);

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
            Name = "Form12Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            MinHeight = 175,
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

    #endregion

    #region Form13_Visual

    public static Control Form13_Visual(INameScope scp)
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
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
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

        #region Right2

        StackPanel rightStP2 = new()
        {
            Orientation = Orientation.Vertical,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = Thickness.Parse("5,5,5,10")
        };
        rightStP2.Children.Add(CreateButton("Перевести все РВ в РАО", "5,0,0,3", 30, "SourceTransmissionAll"));

        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        topPnl1.Children.Add(rightStP2);
        maingrid.Children.Add(topPnl1);

        #endregion

        #region Centre

        DataGridForm13 grd = new()
        {
            Name = "Form13Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm13.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm13.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows13",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm13.ItemsProperty, b);

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
            Name = "Form13Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            MinHeight = 175,
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

    #endregion

    #region Form14_Visual

    public static Control Form14_Visual(INameScope scp)
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
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
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

        #region Right2

        StackPanel rightStP2 = new()
        {
            Orientation = Orientation.Vertical,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = Thickness.Parse("5,5,5,10")
        };
        rightStP2.Children.Add(CreateButton("Перевести все РВ в РАО", "5,0,0,3", 30, "SourceTransmissionAll"));

        #endregion

        topPnl1.Children.Add(brdC);
        topPnl1.Children.Add(brdR);
        topPnl1.Children.Add(rightStP2);
        maingrid.Children.Add(topPnl1);

        #endregion

        #region Centre

        DataGridForm14 grd = new()
        {
            Name = "Form14Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm14.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm14.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows14",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm14.ItemsProperty, b);

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
            Name = "Form14Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            MinHeight = 175,
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

    #endregion

    #region Form15_Visual

    public static Control Form15_Visual(INameScope scp)
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
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
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

        DataGridForm15 grd = new()
        {
            Name = "Form15Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm15.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm15.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows15",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm15.ItemsProperty, b);

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
            Name = "Form15Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            MinHeight = 175,
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

    #endregion

    #region Form16_Visual

    public static Control Form16_Visual(INameScope scp)
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
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
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

        DataGridForm16 grd = new()
        {
            Name = "Form16Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm16.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm16.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows16",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm16.ItemsProperty, b);

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
            Name = "Form16Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            MinHeight = 175,
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

    #endregion

    #region Form17_Visual

    public static Control Form17_Visual(INameScope scp)
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
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
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

        DataGridForm17 grd = new()
        {
            Name = "Form17Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm17.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm17.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows17",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm17.ItemsProperty, b);

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
            Name = "Form17Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            MinHeight = 175,
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

    #endregion

    #region Form18_Visual

    public static Control Form18_Visual(INameScope scp)
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
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
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

        DataGridForm18 grd = new()
        {
            Name = "Form18Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm18.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm18.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows18",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm18.ItemsProperty, b);

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
            Name = "Form18Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            MinHeight = 175,
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
    
    #endregion

    #region Form19_Visual

    public static Control Form19_Visual(INameScope scp)
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
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания предыдущего отчетного периода:"));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.StartPeriod", 150, scp, "date"));
        leftStPT.Children.Add(content);

        content = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        content.Children.Add(CreateTextBlock("5,5,0,5", 30, "Дата окончания настоящего отчетного периода:   "));
        content.Children.Add(CreateTextBox("5,0,0,0", 30, "DataContext.Storage.EndPeriod", 150, scp, "date"));
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

        DataGridForm19 grd = new()
        {
            Name = "Form19Data_",
            Focusable = true,
            Sum = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            Margin = Thickness.Parse("5,0,0,0"),
            [!DataGridForm19.FixedContentProperty] = ind
        };
        grd.SetValue(Grid.RowProperty, 2);

        vw[!ScrollViewer.HorizontalScrollBarValueProperty] = grd[!DataGridForm19.ScrollLeftRightProperty];

        Binding b = new()
        {
            Path = "DataContext.Storage.Rows19",
            ElementName = "ChangingPanel",
            NameScope = new WeakReference<INameScope>(scp)
        };
        grd.Bind(DataGridForm19.ItemsProperty, b);

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
            Name = "Form19Notes_",
            Focusable = true,
            Comment = "Примечания",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Stretch,
            MultilineMode = MultilineMode.Multi,
            ChooseMode = ChooseMode.Cell,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            MaxHeight = 700,
            MinHeight = 175,
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

    #endregion

    #endregion
}