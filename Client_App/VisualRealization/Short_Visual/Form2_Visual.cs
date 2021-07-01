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

            Button btn1 = new Button();
            btn1.Content = ((Form_ClassAttribute)Type.GetType("Models.Form21,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn1.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn1.CommandParameter = "2/1";
            btn1.Height = 30;
            btn1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn1.Margin = Thickness.Parse("5,0,0,0");
            panel.Children.Add(btn1);

            Button btn2 = new Button();
            btn2.Content = ((Form_ClassAttribute)Type.GetType("Models.Form22,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn2.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn2.CommandParameter = "2/2";
            btn2.Height = 30;
            btn2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn2.Margin = Thickness.Parse("5,35,0,0");
            panel.Children.Add(btn2);

            Button btn3 = new Button();
            btn3.Content = ((Form_ClassAttribute)Type.GetType("Models.Form23,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn3.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn3.CommandParameter = "2/3";
            btn3.Height = 30;
            btn3.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn3.Margin = Thickness.Parse("5,70,0,0");
            panel.Children.Add(btn3);

            Button btn4 = new Button();
            btn4.Content = ((Form_ClassAttribute)Type.GetType("Models.Form24,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn4.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn4.CommandParameter = "2/4";
            btn4.Height = 30;
            btn4.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn4.Margin = Thickness.Parse("5,105,0,0");
            panel.Children.Add(btn4);

            return panel;
        }
    }
}
