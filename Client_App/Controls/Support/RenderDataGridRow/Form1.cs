using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using System;

namespace Client_App.Controls.Support.RenderDataGridRow
{
    public class Form1
    {
        public static Control GetControl(string type, int Row, INameScope scp, string TopName)
        {
            switch (type)
            {
                case "0": return Get0();
                case "1": return Get1(Row, scp, TopName);
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

        private static Control Get0()
        {
            return null;
        }

        private static readonly int Wdth1 = 100;
        private static readonly int RowHeight1 = 30;
        private static readonly Color border_color1 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get1Row(int starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {
            DataGrid.Cell? cell = new Controls.DataGrid.Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            cell.Bind(DataGrid.Cell.DataContextProperty, b);

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }

        private static Control Get1(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = -1
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get1Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "OperationDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "PassportNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "Type", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "Radionuclids", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "FactoryNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "Quantity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "Activity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "CreatorOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "CreationDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "Category", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "SignedServicePeriod", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 14, "PropertyCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "Owner", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "DocumentVid", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "DocumentDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 20, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 21, "PackName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 22, "PackType", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 23, "PackNumber", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get2()
        {
            return null;
        }

        private static Control Get3()
        {
            return null;
        }

        private static Control Get4()
        {
            return null;
        }

        private static Control Get5()
        {
            return null;
        }

        private static Control Get6()
        {
            return null;
        }

        private static Control Get7()
        {
            return null;
        }

        private static Control Get8()
        {
            return null;
        }

        private static Control Get9()
        {
            return null;
        }
    }
}
