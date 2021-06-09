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
        public static Control GetControl(string type, int Row, Object Context)
        {
            switch (type)
            {
                case "0": return Get0();
                case "1": return Get1(Row, Context);
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
        static Control Get1Row(int starWidth, int Row,int Column, string Binding, Object Context)
        {
            var cell = new Controls.DataGrid.Cell(Context, Binding,false);
            cell.Width = starWidth * Wdth1;
            cell.Height= RowHeight1;
            cell.BorderBrush = new SolidColorBrush(border_color1);
            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }
        static Control Get1(int Row, Object Context)
        {
            DataGrid.Row stck = new DataGrid.Row();
            stck.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            stck.Orientation = Avalonia.Layout.Orientation.Horizontal;
            stck.Width = 24 * Wdth1;
            stck.Spacing = -1;

            stck.Children.Add(Get1Row(1, Row, 1, "NumberInOrder", Context));
            stck.Children.Add(Get1Row(2, Row, 2, "OperationCode", Context));
            stck.Children.Add(Get1Row(1, Row, 3, "OperationDate", Context));
            stck.Children.Add(Get1Row(1, Row, 4, "PassportNumber", Context));
            stck.Children.Add(Get1Row(1, Row, 5, "Type", Context));
            stck.Children.Add(Get1Row(1, Row, 6, "Radionuclids", Context));
            stck.Children.Add(Get1Row(1, Row, 7, "FactoryNumber", Context));
            stck.Children.Add(Get1Row(1, Row, 8, "Quantity", Context));
            stck.Children.Add(Get1Row(1, Row, 9, "Activity", Context));
            stck.Children.Add(Get1Row(1, Row, 10, "CreatorOKPO", Context));
            stck.Children.Add(Get1Row(1, Row, 11, "CreationDate", Context));
            stck.Children.Add(Get1Row(1, Row, 12, "Category", Context));
            stck.Children.Add(Get1Row(1, Row, 13, "SignedServicePeriod", Context));
            stck.Children.Add(Get1Row(1, Row, 14, "PropertyCode", Context));
            stck.Children.Add(Get1Row(1, Row, 15, "Owner", Context));
            stck.Children.Add(Get1Row(1, Row, 16, "DocumentVid", Context));
            stck.Children.Add(Get1Row(1, Row, 17, "DocumentNumber", Context));
            stck.Children.Add(Get1Row(1, Row, 18, "DocumentDate", Context));
            stck.Children.Add(Get1Row(1, Row, 19, "ProviderOrRecieverOKPO", Context));
            stck.Children.Add(Get1Row(1, Row, 20, "TransporterOKPO", Context));
            stck.Children.Add(Get1Row(1, Row, 21, "PackName", Context));
            stck.Children.Add(Get1Row(1, Row, 22, "PackType", Context));
            stck.Children.Add(Get1Row(1, Row, 23, "PackNumber", Context));

            //var bd = "StartPeriod";
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
