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
    public class Form1
    {
        public static Control GetControl(string type,string Name)
        {
            switch (type)
            {
                case "0": return Get0();
                case "1": return Get1(Name);
                case "2": return Get2();
                case "3": return Get3();
                case "4": return Get4();
                case "5": return Get5();
                case "6": return Get6();
                case "7": return Get7();
                case "8": return Get8();
                case "9": return Get9();
            }
            return null;
        }

        static Control Get0()
        {
            return null;
        }
        static int Wdth1 = 100;
        static int RowHeight1 = 30;
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

            TextBox txt = new TextBox();
            Text.Mode = BindingMode.TwoWay;
            
            txt.Bind(TextBlock.TextProperty, Text);
            //txt.TextAlignment = Avalonia.Media.TextAlignment.Center;
            txt.Background = new SolidColorBrush(new Color(0,0,0,0));
            //txt.Padding = Thickness.Parse("0,5,0,5");
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
            stck.Children.Add(Get1Row(2, Name + "_" + 2, new Binding("OperationCode")));
            stck.Children.Add(Get1Row(1, Name + "_" + 3, new Binding("OperationDate")));
            stck.Children.Add(Get1Row(1, Name + "_" + 4, new Binding("PassportNumber")));
            stck.Children.Add(Get1Row(1, Name + "_" + 5, new Binding("Type")));
            stck.Children.Add(Get1Row(1, Name + "_" + 6, new Binding("Radionuclids")));
            stck.Children.Add(Get1Row(1, Name + "_" + 7, new Binding("FactoryNumber")));
            stck.Children.Add(Get1Row(1, Name + "_" + 8, new Binding("Quantity")));
            stck.Children.Add(Get1Row(1, Name + "_" + 9, new Binding("Activity")));
            stck.Children.Add(Get1Row(1, Name + "_" + 10, new Binding("CreatorOKPO")));
            stck.Children.Add(Get1Row(1, Name + "_" + 11, new Binding("CreationDate")));
            stck.Children.Add(Get1Row(1, Name + "_" + 12, new Binding("Category")));
            stck.Children.Add(Get1Row(1, Name + "_" + 13, new Binding("SignedServicePeriod")));
            stck.Children.Add(Get1Row(1, Name + "_" + 14, new Binding("PropertyCode")));
            stck.Children.Add(Get1Row(1, Name + "_" + 15, new Binding("Owner")));
            stck.Children.Add(Get1Row(1, Name + "_" + 16, new Binding("DocumentVid")));
            stck.Children.Add(Get1Row(1, Name + "_" + 17, new Binding("DocumentNumber")));
            stck.Children.Add(Get1Row(1, Name + "_" + 18, new Binding("DocumentDate")));
            stck.Children.Add(Get1Row(1, Name + "_" + 19, new Binding("ProviderOrRecieverOKPO")));
            stck.Children.Add(Get1Row(1, Name + "_" + 20, new Binding("TransporterOKPO")));
            stck.Children.Add(Get1Row(1, Name + "_" + 21, new Binding("PackName")));
            stck.Children.Add(Get1Row(1, Name + "_" + 22, new Binding("PackType")));
            stck.Children.Add(Get1Row(1, Name + "_" + 23, new Binding("PackNumber")));

            //var bd = new Binding("StartPeriod");
            //bd.StringFormat = "{0:d}";

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
        static Control Get6()
        {
            return null;
        }
        static Control Get7()
        {
            return null;
        }
        static Control Get8()
        {
            return null;
        }
        static Control Get9()
        {
            return null;
        }
    }
}
