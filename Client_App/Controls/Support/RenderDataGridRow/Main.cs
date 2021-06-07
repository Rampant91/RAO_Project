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
        public static Control GetControl(string type,string Name, object Context)
        {
            switch (type)
            {
                case "0": return Get0(Name, Context);
                case "1": return Get1(Name, Context);
                case "2": return Get2();
                case "3": return Get3();
                case "4": return Get4();
                case "5": return Get5();
            }
            return null;
        }

        static int Wdth0 = 100;
        static int RowHeight0 = 30;
        static Color border_color0 = Color.FromArgb(255, 0, 0, 0);

        static Control Get0Row(int starWidth, string Name, string Binding, object Context)
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

            var txt = new Controls.DataGrid.Cell(Context, Binding,true);
            txt.Background = new SolidColorBrush(new Color(0, 0, 0, 0));
            txt.IsReadOnly = true;
            txt.Width = starWidth * Wdth0;

            brd.Child = pnl;
            pnl.Children.Add(txt);

            return brd;
        }
        static Control Get0(string Name, object Context)
        {
            StackPanel stck = new StackPanel();
            stck.Orientation = Avalonia.Layout.Orientation.Horizontal;
            stck.Spacing = -1;

            stck.Children.Add(Get0Row(1, Name + "_" + 1, "Master.RegNo",Context));
            stck.Children.Add(Get0Row(2, Name + "_" + 2, "Master.ShortJurLico", Context));

            stck.Children.Add(Get0Row(1, Name + "_" + 6, "Master.Okpo", Context));

            return stck;
        }

        static int Wdth1 = 100;
        static int RowHeight1 = 30;
        static Color border_color1 = Color.FromArgb(255, 0, 0, 0);
        static Control Get1Row(int starWidth, string Name, string Binding, object Context)
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

            var txt = new Controls.DataGrid.Cell(Context, Binding,true);
            txt.Background = new SolidColorBrush(new Color(0, 0, 0, 0));
            txt.Width = starWidth * Wdth1;

            brd.Child = pnl;
            pnl.Children.Add(txt);

            return brd;
        }
        static Control Get1(string Name, object Context)
        {
            StackPanel stck = new StackPanel();
            stck.Orientation = Avalonia.Layout.Orientation.Horizontal;
            stck.Spacing = -1;

            stck.Children.Add(Get1Row(1, Name + "_" + 1, "NumberInOrder",Context));
            stck.Children.Add(Get1Row(1, Name + "_" + 2, "FormNum",Context));

            var str = "{0:d}";
            stck.Children.Add(Get1Row(1, Name + "_" + 3, "StartPeriod", Context));
            stck.Children.Add(Get1Row(1, Name + "_" + 4, "EndPeriod", Context));
            stck.Children.Add(Get1Row(1, Name + "_" + 5, "ExportDate", Context));

            stck.Children.Add(Get1Row(2, Name + "_" + 6, "IsCorrection", Context));
            stck.Children.Add(Get1Row(1, Name + "_" + 7, "Comments", Context));

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
