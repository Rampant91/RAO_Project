using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Short_Visual
{
    public class Form2_Visual
    {
        public static void FormF_Visual(in Panel pnl0, in Panel pnlx, in Panel pnlb)
        {
            pnl0.Children.Add(Form0_Visual());
            pnlx.Children.Add(FormX_Visual());
            pnlb.Children.Add(FormB_Visual());
        }

        //Форма 10
        private static DataGrid Form0_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }

        //Форма 1X
        private static DataGrid FormX_Visual()
        {
            DataGrid grd = new DataGrid();

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
            //btn0.Content = ((Form_ClassAttribute)Type.GetType("Models.Form20,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            //btn0.Bind(Button.CommandProperty, new Binding("AddForm"));
            //btn0.CommandParameter = "2.0";
            //btn0.Height = 30;
            //btn0.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            //btn0.Margin = Thickness.Parse("5,0,0,0");
            //panel.Children.Add(btn0);

            Button btn1 = new Button();
            btn1.Content = ((Form_ClassAttribute)Type.GetType("Models.Form21,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn1.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn1.CommandParameter = "2.1";
            btn1.Height = 30;
            btn1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn1.Margin = Thickness.Parse("5,0,0,0");
            panel.Children.Add(btn1);

            Button btn2 = new Button();
            btn2.Content = ((Form_ClassAttribute)Type.GetType("Models.Form22,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn2.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn2.CommandParameter = "2.2";
            btn2.Height = 30;
            btn2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn2.Margin = Thickness.Parse("5,35,0,0");
            panel.Children.Add(btn2);

            Button btn3 = new Button();
            btn3.Content = ((Form_ClassAttribute)Type.GetType("Models.Form23,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn3.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn3.CommandParameter = "2.3";
            btn3.Height = 30;
            btn3.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn3.Margin = Thickness.Parse("5,70,0,0");
            panel.Children.Add(btn3);

            Button btn4 = new Button();
            btn4.Content = ((Form_ClassAttribute)Type.GetType("Models.Form24,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn4.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn4.CommandParameter = "2.4";
            btn4.Height = 30;
            btn4.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn4.Margin = Thickness.Parse("5,105,0,0");
            panel.Children.Add(btn4);

            Button btn5 = new Button();
            btn5.Content = ((Form_ClassAttribute)Type.GetType("Models.Form25,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn5.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn5.CommandParameter = "2.5";
            btn5.Height = 30;
            btn5.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn5.Margin = Thickness.Parse("5,140,0,0");
            panel.Children.Add(btn5);

            Button btn6 = new Button();
            btn6.Content = ((Form_ClassAttribute)Type.GetType("Models.Form26,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn6.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn6.CommandParameter = "2.6";
            btn6.Height = 30;
            btn6.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn6.Margin = Thickness.Parse("5,175,0,0");
            panel.Children.Add(btn6);

            Button btn7 = new Button();
            btn7.Content = ((Form_ClassAttribute)Type.GetType("Models.Form27,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn7.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn7.CommandParameter = "2.7";
            btn7.Height = 30;
            btn7.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn7.Margin = Thickness.Parse("5,210,0,0");
            panel.Children.Add(btn7);

            Button btn8 = new Button();
            btn8.Content = ((Form_ClassAttribute)Type.GetType("Models.Form28,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn8.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn8.CommandParameter = "2.8";
            btn8.Height = 30;
            btn8.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn8.Margin = Thickness.Parse("5,245,0,0");
            panel.Children.Add(btn8);

            Button btn9 = new Button();
            btn9.Content = ((Form_ClassAttribute)Type.GetType("Models.Form29,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn9.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn9.CommandParameter = "2.9";
            btn9.Height = 30;
            btn9.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn9.Margin = Thickness.Parse("5,280,0,0");
            panel.Children.Add(btn9);

            Button btn10 = new Button();
            btn10.Content = ((Form_ClassAttribute)Type.GetType("Models.Form210,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn10.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn10.CommandParameter = "2.10";
            btn10.Height = 30;
            btn10.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn10.Margin = Thickness.Parse("5,315,0,0");
            panel.Children.Add(btn10);

            Button btn11 = new Button();
            btn11.Content = ((Form_ClassAttribute)Type.GetType("Models.Form211,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn11.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn11.CommandParameter = "2.11";
            btn11.Height = 30;
            btn11.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn11.Margin = Thickness.Parse("5,350,0,0");
            panel.Children.Add(btn11);

            Button btn12 = new Button();
            btn12.Content = ((Form_ClassAttribute)Type.GetType("Models.Form212,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn12.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn12.CommandParameter = "2.12";
            btn12.Height = 30;
            btn12.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn12.Margin = Thickness.Parse("5,385,0,0");
            panel.Children.Add(btn12);

            return panel;
        }
    }
}
