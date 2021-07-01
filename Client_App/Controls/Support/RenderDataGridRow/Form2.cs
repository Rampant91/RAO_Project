using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using System;

namespace Client_App.Controls.Support.RenderDataGridRow
{
    public class Form2
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
                case "10": return Get10(Row, scp, TopName);
                case "11": return Get11(Row, scp, TopName);
                case "12": return Get12(Row, scp, TopName);
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
            stck.Children.Add(Get1Row(2, Row, 2, "MachinePower", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "MachineCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "RefineMachineName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "NumberOfHoursPerYear", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "CodeRAOIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "StatusRAOIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "VolumeIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "MassIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "QuantityIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "TritiumActivityIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "TritiumActivityOut", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "BetaGammaActivityIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 14, "BetaGammaActivityOut", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "TransuraniumActivityIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "TransuraniumActivityOut", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "AlphaActivityIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "AlphaActivityOut", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "VolumeOut", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 20, "MassOut", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 21, "QuantityOZIIIout", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 22, "CodeRAOout", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 23, "StatusRAOout", scp, TopName));

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
            stck.Children.Add(Get1Row(2, Row, 2, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "PackName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "PackType", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "CodeRAO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "StatusRAO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "VolumeOutOfPack", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "MassInPack", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "QuantityOZIII", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "TritiumActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "BetaGammaActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "TransuraniumActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 14, "AlphaActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "VolumeInPack", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "MassOutOfPack", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "MainRadionuclids", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "Subsidy", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "FcpNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 20, "PackQuantity", scp, TopName));

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
            stck.Children.Add(Get1Row(2, Row, 2, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "ProjectVolume", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "CodeRAO", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "Volume", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "Mass", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "SummaryActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "QuantityOZIII", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "ExpirationDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "DocumentName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "DocumentDate", scp, TopName));

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
            stck.Children.Add(Get1Row(2, Row, 2, "CodeOYAT", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "FcpNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "QuantityFromAnothers", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "QuantityFromAnothersImported", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "QuantityCreated", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "QuantityRemovedFromAccount", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "MassCreated", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "MassFromAnothers", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "MassFromAnothersImported", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "MassRemovedFromAccount", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "QuantityTransferredToAnother", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "MassAnotherReasons", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 14, "MassTransferredToAnother", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "QuantityAnotherReasons", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "QuantityRefined", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "MassRefined", scp, TopName));

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
            stck.Children.Add(Get1Row(2, Row, 2, "CodeOYAT", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "FcpNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "FuelMass", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "CellMass", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "Quantity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "BetaGammaActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "AlphaActivity", scp, TopName));

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
            stck.Children.Add(Get1Row(2, Row, 2, "ObservedSourceNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "ControlledAreaName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "SupposedWasteSource", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "DistanceToWasteSource", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "TestDepth", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "RadionuclidName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "AverageYearConcentration", scp, TopName));

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
            stck.Children.Add(Get1Row(2, Row, 2, "ObservedSourceNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "RadionuclidName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "AllowedWasteValue", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "FactedWasteValue", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "WasteOutbreakPreviousYear", scp, TopName));

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
            stck.Children.Add(Get1Row(2, Row, 2, "WasteSourceName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "WasteRecieverName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "RecieverTypeCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "AllowedWasteRemovalVolume", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "RemovedWasteVolume", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "PoolDistrictName", scp, TopName));

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
            stck.Children.Add(Get1Row(2, Row, 2, "WasteSourceName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "RadionuclidName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "AllowedActivity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "FactedActivity", scp, TopName));

            return stck;
        }

        private static Control Get10(int Row, INameScope scp, string TopName)
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
            stck.Children.Add(Get1Row(2, Row, 2, "IndicatorName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "PlotName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "PlotKadastrNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "PlotCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "InfectedArea", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "AvgGammaRaysDosePower", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "MaxGammaRaysDosePower", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "WasteDensityAlpha", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "WasteDensityBeta", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 11, "FcpNumber", scp, TopName));

            return stck;
        }

        private static Control Get11(int Row, INameScope scp, string TopName)
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
            stck.Children.Add(Get1Row(2, Row, 2, "Radionuclids", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "PlotName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "PlotKadastrNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "PlotCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "InfectedArea", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "SpecificActivityOfPlot", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "SpecificActivityOfLiquidPart", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "SpecificActivityOfDensePart", scp, TopName));

            return stck;
        }

        private static Control Get12(int Row, INameScope scp, string TopName)
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

            stck.Children.Add(Get1Row(1, Row, 1, "Radionuclids", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "ObjectTypeCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 4, "Activity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 5, "ProviderOrRecieverOKPO", scp, TopName));

            return stck;
        }
    }
}
