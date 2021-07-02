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
                case "2": return Get2(Row, scp, TopName);
                case "3": return Get3(Row, scp, TopName);
                case "4": return Get4(Row, scp, TopName);
                case "5": return Get5(Row, scp, TopName);
                case "6": return Get6(Row, scp, TopName);
                case "7": return Get7(Row, scp, TopName);
                case "8": return Get8(Row, scp, TopName);
                case "9": return Get9(Row, scp, TopName);
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
            //stck.Children.Add(Get1Row(1, Row, 24, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get2(int Row, INameScope scp, string TopName)
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
            stck.Children.Add(Get1Row(1, Row, 5, "NameIOU", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "FactoryNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "Mass", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "CreatorOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9,  "CreationDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "PropertyCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "Owner", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "DocumentVid", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 14, "DocumentDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "PackName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "PackType", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "PackNumber", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 20, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get3(int Row, INameScope scp, string TopName)
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
            stck.Children.Add(Get1Row(1, Row, 6, "FactoryNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "Activity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "CreatorOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "CreationDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "PropertyCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "Owner", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "Radionuclids", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "AggregateState", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 14, "DocumentVid", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "DocumentDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "PackName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 20, "PackType", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 21, "PackNumber", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 22, "PassportNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 23, "TypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 24, "FactoryNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 25, "PackTypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 26, "PackNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 27, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get4(int Row, INameScope scp, string TopName)
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
            stck.Children.Add(Get1Row(1, Row, 5, "Sort", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "Volume", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "Mass", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "Activity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "ActivityMeasurementDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "PropertyCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "Owner", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "Radionuclids", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "AggregateState", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 14, "DocumentVid", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "DocumentDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "PackName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 20, "PackType", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 21, "PackNumber", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 22, "PassportNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 23, "FactoryNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 24, "PackTypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 25, "PackNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 26, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get5(int Row, INameScope scp, string TopName)
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
            stck.Children.Add(Get1Row(1, Row, 6, "FactoryNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "Activity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "Quantity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "CreationDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "StatusRAO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "Radionuclids", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "Subsidy", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "DocumentVid", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 14, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "DocumentDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "PackName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "PackType", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 20, "RefineOrSortRAOCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 21, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 22, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 23, "PackNumber", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 24, "PassportNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 25, "TypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 26, "FactoryNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 27, "PackNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 28, "PackTypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 29, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get6(int Row, INameScope scp, string TopName)
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
            stck.Children.Add(Get1Row(1, Row, 4, "CodeRAO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "Mass", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "MainRadionuclids", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "PackNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "QuantityOZIII", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "ActivityMeasurementDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "TritiumActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "BetaGammaActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "AlphaActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "TransuraniumActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 14, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "DocumentVid", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "DocumentDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "Subsidy", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 20, "FcpNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 21, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 22, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 23, "PackName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 24, "PackType", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 25, "RefineOrSortRAOCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 26, "StatusRAO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 27, "Volume", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 28, "PackTypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 29, "PackNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 30, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get7(int Row, INameScope scp, string TopName)
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
            stck.Children.Add(Get1Row(1, Row, 4, "CodeRAO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "Mass", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "Radionuclids", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "FormingDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "Quantity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "PassportNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "RefineOrSortRAOCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "StatusRAO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "VolumeOutOfPack", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "MassOutOfPack", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 14, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "DocumentVid", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "DocumentDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 20, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 21, "PackName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 22, "PackType", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 23, "PackNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 23, "FcpNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "Subsidy", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "TritiumActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "TransuraniumActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "BetaGammaActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "AlphaActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "SpecificActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 23, "PackFactoryNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 23, "Volume", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 28, "PackTypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 29, "PackNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 29, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get8(int Row, INameScope scp, string TopName)
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
            stck.Children.Add(Get1Row(1, Row, 4, "CodeRAO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "IndividualNumberZHRO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "Radionuclids", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "SpecificActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "SaltConcentration", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "Subsidy", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "FcpNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 14, "StatusRAO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "Volume6", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "DocumentVid", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "DocumentDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 20, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 21, "Volume20", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 22, "Mass21", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "TritiumActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "TransuraniumActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "BetaGammaActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "AlphaActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "PassportNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "RefineOrSortRAOCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "Mass7", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 24, "PassportNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 23, "IndividualNumberZHROrecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 29, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get9(int Row, INameScope scp, string TopName)
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
            stck.Children.Add(Get1Row(1, Row, 4, "Quantity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "CodeTypeAccObject", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "DocumentVid", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "DocumentDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "Activity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 20, "Radionuclids", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 29, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }
    }
}
