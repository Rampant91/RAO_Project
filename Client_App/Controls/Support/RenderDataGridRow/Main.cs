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
using Avalonia.Input;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Interactivity;

namespace Client_App.Controls.Support.RenderDataGridRow
{
    public class Main
    {
        public static Control GetControl(string type,string Name)
        {
            switch (type)
            {
                case "0": return Get0(Name);
                case "1": return Get1(Name);
                case "2": return Get2();
                case "3": return Get3();
                case "4": return Get4();
                case "5": return Get5();
            }
            return null;
        }

        static int Wdth0 = 100;
        static int RowHeight0 = 25;
        static Color border_color0 = Color.FromArgb(255, 0, 0, 0);

        static Control Get0Row(int starWidth, string Name, Binding Text)
        {
            Border brd = new Border()
            {
                BorderThickness = Thickness.Parse("1"),
                BorderBrush = new SolidColorBrush(border_color0)
            };

            Panel pnl = new Panel();
            pnl.Name = Name;
            pnl.Width = starWidth * Wdth0;
            pnl.Height = RowHeight0;

            TextBlock txt = new TextBlock();
            txt.Bind(TextBlock.TextProperty, Text);
            txt.TextAlignment = Avalonia.Media.TextAlignment.Center;
            txt.Padding = Thickness.Parse("0,5,0,5");
            txt.Width = starWidth * Wdth0;

            brd.Child = pnl;
            pnl.Children.Add(txt);

            return brd;
        }
        static Control Get0(string Name)
        {
            StackPanel stck = new StackPanel();
            stck.Orientation = Avalonia.Layout.Orientation.Horizontal;
            stck.Spacing = -1;

            stck.Children.Add(Get1Row(1, Name + "_" + 1, new Binding("Master.RegNo")));
            stck.Children.Add(Get1Row(2, Name + "_" + 2, new Binding("Master.ShortJurLico")));

            stck.Children.Add(Get1Row(1, Name + "_" + 6, new Binding("Master.Okpo")));

            return stck;
        }

        static int Wdth1 = 100;
        static int RowHeight1 = 25;
        static Color border_color1 = Color.FromArgb(255, 0, 0, 0);
        static Control Get1Row(int starWidth, string Name, Binding Text)
        {
            Border brd = new Border()
            {
                BorderThickness = Thickness.Parse("1"),
                BorderBrush = new SolidColorBrush(border_color1)
            }; 

            Panel pnl = new Panel();
            pnl.Name = Name;
            pnl.Width = starWidth * Wdth1;
            pnl.Height = RowHeight1;

            TextBlock txt = new TextBlock();
            txt.Bind(TextBlock.TextProperty, Text);
            txt.TextAlignment = Avalonia.Media.TextAlignment.Center;
            txt.Padding = Thickness.Parse("0,5,0,5");
            txt.Width = starWidth * Wdth1;

            brd.Child = pnl;
            pnl.Children.Add(txt);

            return brd;
        }
        static Control Get1(string Name)
        {
            StackPanel stck = new StackPanel();
            stck.Orientation = Avalonia.Layout.Orientation.Horizontal;
            stck.Spacing = -1;

            stck.Children.Add(Get1Row(1, Name + "_" + 1, new Binding("NumberInOrder")));
            stck.Children.Add(Get1Row(1, Name + "_" + 2, new Binding("FormNum")));

            var bd = new Binding("StartPeriod");
            bd.StringFormat = "{0:d}";
            stck.Children.Add(Get1Row(1, Name + "_" + 3, bd));
            bd = new Binding("EndPeriod");
            bd.StringFormat = "{0:d}";
            stck.Children.Add(Get1Row(1, Name + "_" + 4, bd));
            bd = new Binding("ExportDate");
            bd.StringFormat = "{0:d}";
            stck.Children.Add(Get1Row(1, Name + "_" + 5, bd));

            stck.Children.Add(Get1Row(2, Name + "_" + 6, new Binding("IsCorrection")));
            stck.Children.Add(Get1Row(1, Name + "_" + 7, new Binding("Comments")));

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
