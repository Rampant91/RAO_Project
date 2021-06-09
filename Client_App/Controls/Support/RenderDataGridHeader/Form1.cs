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
        static int RowHeight0 = 30;
        static Color border_color1 = Color.FromArgb(255, 0, 0, 0);
        static Control Get1Header(int starWidth, int Column, string Text)
        {
            var ram = new Models.DataAccess.RamAccess<string>(null, Text);
            var cell = new Controls.DataGrid.Cell(ram, "", true);
            cell.Background = new SolidColorBrush(Color.Parse("LightGray"));
            cell.Width = starWidth * Wdth0;
            cell.Height = RowHeight0;
            cell.BorderBrush = new SolidColorBrush(border_color0);
            cell.CellRow = 0;
            cell.CellColumn = Column;


            return cell;
        }
        static Control Get1()
        {
            StackPanel stck = new StackPanel();
            stck.Orientation = Avalonia.Layout.Orientation.Horizontal;
            stck.Spacing = -1;

            stck.Children.Add(Get1Header(1,1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PassportNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Type").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("FactoryNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Quantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Activity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("CreatorOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("CreationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Category").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("SignedServicePeriod").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PropertyCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Owner").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,19,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,20,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,21,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,22,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1,23,
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
