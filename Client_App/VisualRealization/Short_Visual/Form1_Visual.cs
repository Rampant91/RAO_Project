using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Client_App.Controls.DataGrid.DataGrids;
using Client_App.Views;
using Client_App.VisualRealization.Converters;
using Models.Attributes;

namespace Client_App.VisualRealization.Short_Visual;

public class Form1_Visual
{
    //Полный вывод
    public static void FormF_Visual(MainWindow v,in Panel pnl0, in Panel pnlx, in Panel pnlb)
    {
        var tp = pnl0.FindNameScope();
        var grd1 = Form0_Visual(tp);
        pnl0.Children.Add(grd1);

        NameScope scp = new();
        scp.Register(grd1.Name, grd1);
        scp.Complete();
        var grd2 = FormX_Visual(scp);
        pnlx.Children.Add(grd2);

        Binding bd = new()
        {
            Path = "SelectedItems",
            ElementName = "Form10AllDataGrid_",
            NameScope = new WeakReference<INameScope>(scp),
        };
        v.Bind(MainWindow.SelectedReportsProperty, bd);


        var grd3 = FormB_Visual();
        pnlb.Children.Add(grd3);
    }

    //Форма 10
    private static DataGridReports Form0_Visual(INameScope scp)
    {
        DataGridReports grd = new()
        {
            Name= "Form10AllDataGrid_",
            ShowAllReport=true,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            MultilineMode = Controls.DataGrid.MultilineMode.Single,
            ChooseMode = Controls.DataGrid.ChooseMode.Line,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            Search = true,
            PageSize = 5,
            IsReadable = true,
            IsAutoSizable = true
        };
        //if (grd.Search == true)
        //{
        //    (((((grd.Content as Panel).Children[0] as StackPanel).Children[2] as Border).Child as ScrollViewer).Content as Panel).Height = 200;
        //}
        //else 
        //{
        //    (((((grd.Content as Panel).Children[0] as StackPanel).Children[1] as Border).Child as ScrollViewer).Content as Panel).Height = 200;
        //}

        Binding b = new()
        {
            Path = "DataContext.LocalReports.Reports_Collection10",
            ElementName = "MainWindow",
            NameScope = new WeakReference<INameScope>(scp)
        };

        grd.Bind(DataGridReports.ItemsProperty, b);

        return grd;
    }

    //Форма 1X
    private static DataGridReport FormX_Visual(INameScope scp)
    {
        DataGridReport grd = new("Form1AllDataGrid_")
        {
            Name = "Form1AllDataGrid_",
            CommentСhangeable = true,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            MultilineMode = Controls.DataGrid.MultilineMode.Single,
            ChooseMode = Controls.DataGrid.ChooseMode.Line,
            ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
            PageSize = 10,
            IsReadable = true,
            IsAutoSizable = true
        };
        //((((grd.Content as Panel).Children[0] as StackPanel).Children[1] as Border).Child as ScrollViewer).MaxHeight = 261;
        Binding b = new()
        {
            Path = "SelectedItems",
            ElementName = "Form10AllDataGrid_",
            NameScope = new WeakReference<INameScope>(scp),
            Converter = new ReportsToReport_Converter(),
        };
        grd.Bind(DataGridReport.ItemsProperty, b);

        return grd;
    }


    //Кнопки создания или изменения формы
    private static Panel FormB_Visual()
    {
        Panel panel = new()
        {
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
        };

        const double h = 33;

        Button btn1 = new()
        {
            Content = ((Form_ClassAttribute)Type.GetType("Models.Forms.Form1.Form11,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
        };
        btn1.Bind(Button.CommandProperty, new Binding("AddForm"));
        btn1.CommandParameter = "1.1";
        btn1.Height = h;
        btn1.FontSize = 13;
        btn1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        btn1.Margin = Thickness.Parse("5,0,0,0");
        panel.Children.Add(btn1);

        Button btn2 = new()
        {
            Content = ((Form_ClassAttribute)Type.GetType("Models.Forms.Form1.Form12,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
        };
        btn2.Bind(Button.CommandProperty, new Binding("AddForm"));
        btn2.CommandParameter = "1.2";
        btn2.Height = h;
        btn2.FontSize = 13;
        btn2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        btn2.Margin = Thickness.Parse("5,37,0,0");
        panel.Children.Add(btn2);

        Button btn3 = new()
        {
            Content = ((Form_ClassAttribute)Type.GetType("Models.Forms.Form1.Form13,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
        };
        btn3.Bind(Button.CommandProperty, new Binding("AddForm"));
        btn3.CommandParameter = "1.3";
        btn3.Height = h;
        btn3.FontSize = 13;
        btn3.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        btn3.Margin = Thickness.Parse("5,74,0,0");
        panel.Children.Add(btn3);

        Button btn4 = new()
        {
            Content = ((Form_ClassAttribute)Type.GetType("Models.Forms.Form1.Form14,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
        };
        btn4.Bind(Button.CommandProperty, new Binding("AddForm"));
        btn4.CommandParameter = "1.4";
        btn4.Height = h;
        btn4.FontSize = 13;
        btn4.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        btn4.Margin = Thickness.Parse("5,111,0,0");
        panel.Children.Add(btn4);

        Button btn5 = new()
        {
            Content = ((Form_ClassAttribute)Type.GetType("Models.Forms.Form1.Form15,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
        };
        btn5.Bind(Button.CommandProperty, new Binding("AddForm"));
        btn5.CommandParameter = "1.5";
        btn5.Height = h;
        btn5.FontSize = 12;
        btn5.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        btn5.Margin = Thickness.Parse("5,148,0,0");
        panel.Children.Add(btn5);

        Button btn6 = new()
        {
            Content = ((Form_ClassAttribute)Type.GetType("Models.Forms.Form1.Form16,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
        };
        btn6.Bind(Button.CommandProperty, new Binding("AddForm"));
        btn6.CommandParameter = "1.6";
        btn6.Height = h;
        btn6.FontSize = 13;
        btn6.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        btn6.Margin = Thickness.Parse("5,185,0,0");
        panel.Children.Add(btn6);

        Button btn7 = new()
        {
            Content = ((Form_ClassAttribute)Type.GetType("Models.Forms.Form1.Form17,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
        };
        btn7.Bind(Button.CommandProperty, new Binding("AddForm"));
        btn7.CommandParameter = "1.7";
        btn7.Height = h;
        btn7.FontSize = 13;
        btn7.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        btn7.Margin = Thickness.Parse("5,222,0,0");
        panel.Children.Add(btn7);

        Button btn8 = new()
        {
            Content = ((Form_ClassAttribute)Type.GetType("Models.Forms.Form1.Form18,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
        };
        btn8.Bind(Button.CommandProperty, new Binding("AddForm"));
        btn8.CommandParameter = "1.8";
        btn8.Height = h;
        btn8.FontSize = 13;
        btn8.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        btn8.Margin = Thickness.Parse("5,259,0,0");
        panel.Children.Add(btn8);

        Button btn9 = new()
        {
            Content = ((Form_ClassAttribute)Type.GetType("Models.Forms.Form1.Form19,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
        };
        btn9.Bind(Button.CommandProperty, new Binding("AddForm"));
        btn9.CommandParameter = "1.9";
        btn9.Height = h;
        btn9.FontSize = 13;
        btn9.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
        btn9.Margin = Thickness.Parse("5,296,0,0");
        panel.Children.Add(btn9);

        return panel;
    }
}