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
    public class Form1
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
                case "6": return Get6();
                case "7": return Get7();
                case "8": return Get8();
                case "9": return Get9();
            }
            return null;
        }

        static int Wdth0 = 100;
        static Color border_color0 = Color.FromArgb(255, 0, 0, 0);

        static Control Get0()
        {
            return null;
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
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PassportNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Type").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("FactoryNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Quantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Activity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("CreatorOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("CreationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Category").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("SignedServicePeriod").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PropertyCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Owner").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PackNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
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
