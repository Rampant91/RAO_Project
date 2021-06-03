using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Data;
using Avalonia.Media;
using Models.Attributes;

namespace Client_App.Controls.Support.RenderDataGridHeader
{
    public class Main
    {
        public static Control GetControl(string type)
        {
            switch (type)
            {
                case "0": return Get0();
                case "1": return Get1();
                case "2": return Get2();
                case "3": return Get3();
                case "4": return Get4();
                case "5": return Get5();
            }
            return null;
        }

        static int Wdth0 = 100;
        static Color border_color0 = Color.FromArgb(255, 0, 0, 0);

        static Control Get0Header(int starWidth,string Text)
        {
            Border brd = new Border()
            {
                BorderThickness = Thickness.Parse("1"),
                BorderBrush = new SolidColorBrush(border_color0)
            };
            Panel pnl = new Panel();
            pnl.Width = starWidth * Wdth0;
            pnl.Background = new SolidColorBrush(Color.Parse("LightGray"));
            TextBlock txt = new TextBlock();
            txt.FontWeight = FontWeight.Bold;
            txt.Text = Text;
            txt.TextAlignment = Avalonia.Media.TextAlignment.Center;
            txt.Padding = Thickness.Parse("0,5,0,5");

            brd.Child = pnl;
            pnl.Children.Add(txt);

            return brd;
        }
        static Control Get0()
        {
            StackPanel stck = new StackPanel();
            stck.Orientation = Avalonia.Layout.Orientation.Horizontal;
            stck.Spacing = -1;

            stck.Children.Add(Get0Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("RegNo").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get0Header(2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("ShortJurLico").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get0Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Okpo").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            return stck;
        }

        static int Wdth1 = 100;
        static Color border_color1 = Color.FromArgb(255, 0, 0, 0);
        static Control Get1Header(int starWidth, string Text)
        {
            Border brd = new Border()
            {
                BorderThickness = Thickness.Parse("1"),
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Panel pnl = new Panel();
            pnl.Width = starWidth * Wdth1;
            pnl.Background = new SolidColorBrush(Color.Parse("LightGray"));
            TextBlock txt = new TextBlock();
            txt.FontWeight = FontWeight.Bold;
            txt.Text = Text;
            txt.TextAlignment = Avalonia.Media.TextAlignment.Center;
            txt.Padding = Thickness.Parse("0,5,0,5");

            brd.Child = pnl;
            pnl.Children.Add(txt);

            return brd;
        }
        static Control Get1()
        {
            StackPanel stck = new StackPanel();
            stck.Orientation = Avalonia.Layout.Orientation.Horizontal;
            stck.Spacing = -1;

            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Abstracts.Form1,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Abstracts.Form1,Models").
                    GetProperty("FormNum").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Collections.Report,Models").
                    GetProperty("StartPeriod").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Collections.Report,Models").
                    GetProperty("EndPeriod").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Collections.Report,Models").
                    GetProperty("ExportDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(2,
                    ((Form_PropertyAttribute)Type.GetType("Collections.Report,Models").
                    GetProperty("IsCorrection").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Collections.Report,Models").
                    GetProperty("Comments").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }
        static Control Get2()
        {
            return null;
        }
        static Control Get3()
        {
            return null;
        }
        static Control Get4()
        {
            return null;
        }
        static Control Get5()
        {
            return null;
        }
    }
}
