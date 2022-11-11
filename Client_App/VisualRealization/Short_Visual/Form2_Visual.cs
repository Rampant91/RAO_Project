using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Models.Attributes;
using System;
using System.Linq;
using Client_App.Controls.DataGrid.DataGrids;
using Client_App.Views;
using Models.Collections;

namespace Client_App.Short_Visual
{
    public class Form2_Visual
    {
        public static void FormF_Visual(MainWindow v, in Panel pnl0, in Panel pnlx, in Panel pnlb)
        {
            INameScope? tp = pnl0.FindNameScope();
            DataGridReports grd1 = (DataGridReports)Form0_Visual(tp);
            pnl0.Children.Add(grd1);

            NameScope scp = new();
            scp.Register(grd1.Name, grd1);
            scp.Complete();
            DataGridReport grd2 = (DataGridReport)FormX_Visual(scp);
            pnlx.Children.Add(grd2);

            Binding bd = new()
            {
                Path = "SelectedItems",
                ElementName = "Form20AllDataGrid_",
                NameScope = new WeakReference<INameScope>(scp),
            };
            v.Bind(MainWindow.SelectedReportsProperty, bd);

            Panel? grd3 = FormB_Visual();
            pnlb.Children.Add(grd3);
        }

        //Форма 20
        private static Control Form0_Visual(INameScope scp)
        {
            DataGridReports grd = new()
            {
                Name = "Form20AllDataGrid_",
                ShowAllReport = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = Controls.DataGrid.MultilineMode.Single,
                ChooseMode = Controls.DataGrid.ChooseMode.Line,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                Search = true,
                PageSize = 8,
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
                Path = "DataContext.Local_Reports.Reports_Collection20",
                ElementName = "MainWindow",
                NameScope = new WeakReference<INameScope>(scp)
            };

            grd.Bind(Controls.DataGrid.DataGrid<Reports>.ItemsProperty, b);

            return grd;
        }

        //Форма 2X
        private static Control FormX_Visual(INameScope scp)
        {
            DataGridReport grd = new("Form2AllDataGrid_")
            {
                Name = "Form2AllDataGrid_",
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
            //((((grd.Content as Panel).Children[0] as StackPanel).Children[1] as Border).Child as ScrollViewer).MaxHeight = 175;
            Binding b = new()
            {
                Path = "SelectedItems",
                ElementName = "Form20AllDataGrid_",
                NameScope = new WeakReference<INameScope>(scp),
                Converter = new Converters.ReportsToReport_Converter()
            };

            grd.Bind(Controls.DataGrid.DataGrid<Report>.ItemsProperty, b);

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

            Button btn1 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form21,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn1.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn1.CommandParameter = "2.1";
            btn1.Height = 30;
            btn1.FontSize = 13;
            btn1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn1.Margin = Thickness.Parse("5,0,0,0");
            panel.Children.Add(btn1);

            Button btn2 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form22,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn2.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn2.CommandParameter = "2.2";
            btn2.Height = 30;
            btn2.FontSize = 13;
            btn2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn2.Margin = Thickness.Parse("5,35,0,0");
            panel.Children.Add(btn2);

            Button btn3 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form23,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn3.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn3.CommandParameter = "2.3";
            btn3.Height = 30;
            btn3.FontSize = 13;
            btn3.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn3.Margin = Thickness.Parse("5,70,0,0");
            panel.Children.Add(btn3);

            Button btn4 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form24,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn4.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn4.CommandParameter = "2.4";
            btn4.Height = 30;
            btn4.FontSize = 13;
            btn4.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn4.Margin = Thickness.Parse("5,105,0,0");
            panel.Children.Add(btn4);

            Button btn5 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form25,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn5.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn5.CommandParameter = "2.5";
            btn5.Height = 30;
            btn5.FontSize = 13;
            btn5.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn5.Margin = Thickness.Parse("5,140,0,0");
            panel.Children.Add(btn5);

            Button btn6 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form26,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn6.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn6.CommandParameter = "2.6";
            btn6.Height = 30;
            btn6.FontSize = 13;
            btn6.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn6.Margin = Thickness.Parse("5,175,0,0");
            panel.Children.Add(btn6);

            Button btn7 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form27,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn7.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn7.CommandParameter = "2.7";
            btn7.Height = 30;
            btn7.FontSize = 13;
            btn7.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn7.Margin = Thickness.Parse("5,210,0,0");
            panel.Children.Add(btn7);

            Button btn8 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form28,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn8.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn8.CommandParameter = "2.8";
            btn8.Height = 30;
            btn8.FontSize = 13;
            btn8.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn8.Margin = Thickness.Parse("5,245,0,0");
            panel.Children.Add(btn8);

            Button btn9 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form29,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn9.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn9.CommandParameter = "2.9";
            btn9.Height = 30;
            btn9.FontSize = 13;
            btn9.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn9.Margin = Thickness.Parse("5,280,0,0");
            panel.Children.Add(btn9);

            Button btn10 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form210,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn10.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn10.CommandParameter = "2.10";
            btn10.Height = 30;
            btn10.FontSize = 13;
            btn10.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn10.Margin = Thickness.Parse("5,315,0,0");
            panel.Children.Add(btn10);

            Button btn11 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form211,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn11.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn11.CommandParameter = "2.11";
            btn11.Height = 30;
            btn11.FontSize = 13;
            btn11.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn11.Margin = Thickness.Parse("5,350,0,0");
            panel.Children.Add(btn11);

            Button btn12 = new()
            {
                Content = ((Form_ClassAttribute)Type.GetType("Models.Form212,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name
            };
            btn12.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn12.CommandParameter = "2.12";
            btn12.Height = 30;
            btn12.FontSize = 13;
            btn12.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn12.Margin = Thickness.Parse("5,385,0,0");
            panel.Children.Add(btn12);

            return panel;
        }
    }
}
