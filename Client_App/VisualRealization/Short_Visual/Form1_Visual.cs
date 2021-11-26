using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Views;

namespace Client_App.Short_Visual
{
    public class Form1_Visual
    {
        //Полный вывод
        public static void FormF_Visual(MainWindow v,in Panel pnl0, in Panel pnlx, in Panel pnlb)
        {
            INameScope? tp = pnl0.FindNameScope();
            Controls.DataGrid.DataGrid? grd1 = (Controls.DataGrid.DataGrid)Form0_Visual(tp);
            pnl0.Children.Add(grd1);

            NameScope scp = new NameScope();
            scp.Register(grd1.Name, grd1);
            scp.Complete();
            Controls.DataGrid.DataGrid? grd2 = (Controls.DataGrid.DataGrid)FormX_Visual(scp);
            pnlx.Children.Add(grd2);

            Binding bd = new Binding
            {
                Path = "SelectedItems",
                ElementName = "Form10AllDataGrid_",
                NameScope = new WeakReference<INameScope>(scp),
            };
            v.Bind(MainWindow.SelectedReportsProperty, bd);


            Panel? grd3 = FormB_Visual();
            pnlb.Children.Add(grd3);
        }

        //Форма 10
        private static Control Form0_Visual(INameScope scp)
        {
            Controls.DataGrid.DataGrid grd = new Controls.DataGrid.DataGrid
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = Controls.DataGrid.MultilineMode.Single,
                ChooseMode = Controls.DataGrid.ChooseMode.Line,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255)),
                Type = "0.0"
            };

            grd.Name = "Form10AllDataGrid_";

            Binding b = new Binding
            {
                Path = "DataContext.Local_Reports.Reports_Collection10",
                ElementName = "MainWindow",
                NameScope = new WeakReference<INameScope>(scp)
            };

            grd.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b);

            ContextMenu? cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>
            {
                new MenuItem
                {
                    Header = "Добавить форму",
                    [!MenuItem.CommandProperty] = new Binding("AddReport"),
                    CommandParameter = "1.0",
                },
                new MenuItem
                {
                    Header = "Изменить форму",
                    [!MenuItem.CommandProperty] = new Binding("ChangeReport"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                },
                new MenuItem
                {
                    Header = "Удалить форму",
                    [!MenuItem.CommandProperty] = new Binding("DeleteReport"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            return grd;
        }

        //Форма 1X
        private static Control FormX_Visual(INameScope scp)
        {
            Controls.DataGrid.DataGrid grd = new Controls.DataGrid.DataGrid
            {
                Type = "0.1",
                Name = "Form1AllDataGrid_",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                MultilineMode = Controls.DataGrid.MultilineMode.Single,
                ChooseMode = Controls.DataGrid.ChooseMode.Line,
                ChooseColor = new SolidColorBrush(new Color(150, 135, 209, 255))
            };

            Binding b = new Binding
            {
                Path = "SelectedItems",
                ElementName = "Form10AllDataGrid_",
                NameScope = new WeakReference<INameScope>(scp),
                Converter = new Converters.ReportsToReport_Converter()
            };

            grd.Bind(Controls.DataGrid.DataGrid.ItemsProperty, b);

            ContextMenu? cntx = new ContextMenu();
            List<MenuItem> excelitems = new List<MenuItem>
            {
                new MenuItem
                {
                    Header = "Для печати",
                    IsEnabled=false,
                    //[!MenuItem.CommandProperty] = new Binding("_Print_Excel_Export"),
                    //[!MenuItem.CommandParameterProperty] = new Binding("$parent[3].SelectedItems"),
                },
                new MenuItem
                {
                    Header = "Для выгрузки",
                    [!MenuItem.CommandProperty] = new Binding("_Excel_Export"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[3].SelectedItems"),
                }
            };

            List<MenuItem> itms = new List<MenuItem>
            {
                new MenuItem
                {
                    Items=excelitems,
                    Header = "Экспорт Excel",
                },
                new MenuItem
                {
                    Header = "Экспорт",
                    [!MenuItem.CommandProperty] = new Binding("ExportForm"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                },
                new MenuItem
                {
                    Header = "Изменить форму",
                    [!MenuItem.CommandProperty] = new Binding("ChangeForm"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                },
                new MenuItem
                {
                    Header = "Удалить форму",
                    [!MenuItem.CommandProperty] = new Binding("DeleteForm"),
                    [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItems"),
                }
            };
            cntx.Items = itms;

            grd.ContextMenu = cntx;
            return grd;
        }

        //Кнопки создания или изменения формы
        private static Panel FormB_Visual()
        {
            Panel panel = new Panel
            {
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
            };

            //Button btn0 = new Button();
            //btn0.Content = ((Form_ClassAttribute)Type.GetType("Models.Form10,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            //btn0.Bind(Button.CommandProperty, new Binding("AddForm"));
            //btn0.CommandParameter = "1.0";
            //btn0.Height = 30;
            //btn0.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            //btn0.Margin = Thickness.Parse("5,0,0,0");
            //panel.Children.Add(btn0);

            Button btn1 = new Button();
            btn1.Content = ((Form_ClassAttribute)Type.GetType("Models.Form11,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn1.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn1.CommandParameter = "1.1";
            btn1.Height = 30;
            btn1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn1.Margin = Thickness.Parse("5,0,0,0");
            panel.Children.Add(btn1);

            Button btn2 = new Button();
            btn2.Content = ((Form_ClassAttribute)Type.GetType("Models.Form12,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn2.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn2.CommandParameter = "1.2";
            btn2.Height = 30;
            btn2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn2.Margin = Thickness.Parse("5,35,0,0");
            panel.Children.Add(btn2);

            Button btn3 = new Button();
            btn3.Content = ((Form_ClassAttribute)Type.GetType("Models.Form13,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn3.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn3.CommandParameter = "1.3";
            btn3.Height = 30;
            btn3.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn3.Margin = Thickness.Parse("5,70,0,0");
            panel.Children.Add(btn3);

            Button btn4 = new Button();
            btn4.Content = ((Form_ClassAttribute)Type.GetType("Models.Form14,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn4.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn4.CommandParameter = "1.4";
            btn4.Height = 30;
            btn4.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn4.Margin = Thickness.Parse("5,105,0,0");
            panel.Children.Add(btn4);

            Button btn5 = new Button();
            btn5.Content = ((Form_ClassAttribute)Type.GetType("Models.Form15,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn5.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn5.CommandParameter = "1.5";
            btn5.Height = 30;
            btn5.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn5.Margin = Thickness.Parse("5,140,0,0");
            panel.Children.Add(btn5);

            Button btn6 = new Button();
            btn6.Content = ((Form_ClassAttribute)Type.GetType("Models.Form16,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn6.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn6.CommandParameter = "1.6";
            btn6.Height = 30;
            btn6.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn6.Margin = Thickness.Parse("5,175,0,0");
            panel.Children.Add(btn6);

            Button btn7 = new Button();
            btn7.Content = ((Form_ClassAttribute)Type.GetType("Models.Form17,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn7.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn7.CommandParameter = "1.7";
            btn7.Height = 30;
            btn7.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn7.Margin = Thickness.Parse("5,210,0,0");
            panel.Children.Add(btn7);

            Button btn8 = new Button();
            btn8.Content = ((Form_ClassAttribute)Type.GetType("Models.Form18,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn8.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn8.CommandParameter = "1.8";
            btn8.Height = 30;
            btn8.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn8.Margin = Thickness.Parse("5,245,0,0");
            panel.Children.Add(btn8);

            Button btn9 = new Button();
            btn9.Content = ((Form_ClassAttribute)Type.GetType("Models.Form19,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn9.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn9.CommandParameter = "1.9";
            btn9.Height = 30;
            btn9.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn9.Margin = Thickness.Parse("5,280,0,0");
            panel.Children.Add(btn9);

            return panel;
        }
    }
}
