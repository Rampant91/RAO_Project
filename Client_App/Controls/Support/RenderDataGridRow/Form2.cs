using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using System.Globalization;
using Avalonia.Media;
using System;
using System.Reactive.Linq;
using Client_App.Controls.DataGrid;
using Models.DataAccess;

namespace Client_App.Controls.Support.RenderDataGridRow
{
    public class Form2
    {
        public static Control GetControl(string type, int Row, INameScope scp, string TopName)
        {
            switch (type)
            {
                case "0": return Get0(Row, scp, TopName);
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
                case "1*": return GetNotes(Row, scp, TopName);
                case "2*": return GetNotes(Row, scp, TopName);
                case "3*": return GetNotes(Row, scp, TopName);
                case "4*": return GetNotes(Row, scp, TopName);
                case "5*": return GetNotes(Row, scp, TopName);
                case "6*": return GetNotes(Row, scp, TopName);
                case "7*": return GetNotes(Row, scp, TopName);
                case "8*": return GetNotes(Row, scp, TopName);
                case "9*": return GetNotes(Row, scp, TopName);
                case "10*": return GetNotes(Row, scp, TopName);
                case "11*": return GetNotes(Row, scp, TopName);
                case "12*": return GetNotes(Row, scp, TopName);
            }
            return null;
        }

        #region Notes
        private static Control GetRowNotes(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
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

        private static Control GetNotes(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                Mode = BindingMode.OneTime,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(GetRowNotes(1, Row, 1, "RowNumber", scp, TopName));
            stck.Children.Add(GetRowNotes(1, Row, 2, "GraphNumber", scp, TopName));
            stck.Children.Add(GetRowNotes(6, Row, 3, "Comment", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.0
        private static Control Get0Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
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
        private static Control Get0(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get0Row(1, Row, 1, "SubjectRF", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 2, "JurLico", scp, TopName));
            stck.Children.Add(Get0Row(2, Row, 3, "ShortJurLico", scp, TopName));
            stck.Children.Add(Get0Row(2, Row, 4, "JurLicoAddress", scp, TopName));
            stck.Children.Add(Get0Row(2, Row, 5, "JurLicoFactAddress", scp, TopName));
            stck.Children.Add(Get0Row(2, Row, 6, "GradeFIO", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 7, "Telephone", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 8, "Fax", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 9, "Email", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 10, "RegNo", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 11, "Okpo", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 12, "Okved", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 13, "Okogu", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 14, "Oktmo", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 15, "Inn", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 16, "Kpp", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 17, "Okopf", scp, TopName));
            stck.Children.Add(Get0Row(1, Row, 18, "Okfs", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.1
        private static readonly int Wdth1 = 100;
        private static readonly int RowHeight1 = 30;
        private static readonly Color border_color1 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get1Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };
            cell.Bind(StyledElement.DataContextProperty, b);

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
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get1Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get1Row(2.5, Row, 2, "RefineMachineName", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 3, "MachineCode", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 4, "MachinePower", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 5, "NumberOfHoursPerYear", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "CodeRAOIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 7, "StatusRAOIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "VolumeIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "MassIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 10, "QuantityIn", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 11, "TritiumActivityIn", scp, TopName));
            stck.Children.Add(Get1Row(3.5, Row, 12, "BetaGammaActivityIn", scp, TopName));
            stck.Children.Add(Get1Row(3.6, Row, 13, "AlphaActivityIn", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 14, "TransuraniumActivityIn", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 15, "CodeRAOout", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 16, "StatusRAOout", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 17, "VolumeOut", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "MassOut", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 19, "QuantityOZIIIout", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 20, "TritiumActivityOut", scp, TopName));
            stck.Children.Add(Get1Row(3.5, Row, 21, "BetaGammaActivityOut", scp, TopName));
            stck.Children.Add(Get1Row(3.6, Row, 22, "AlphaActivityOut", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 23, "TransuraniumActivityOut", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.2
        private static readonly int Wdth2 = 100;
        private static readonly int RowHeight2 = 30;
        private static readonly Color border_color2 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get2Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };
            cell.Bind(StyledElement.DataContextProperty, b);

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }
        private static Control Get2(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get2Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 2, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 3, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 4, "PackName", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 5, "PackType", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 6, "PackQuantity", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 7, "CodeRAO", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 8, "StatusRAO", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 9, "VolumeOutOfPack", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 10, "VolumeInPack", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 11, "MassOutOfPack", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 12, "MassInPack", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 13, "QuantityOZIII", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 14, "TritiumActivity", scp, TopName));
            stck.Children.Add(Get2Row(3.5, Row, 15, "BetaGammaActivity", scp, TopName));
            stck.Children.Add(Get2Row(3.6, Row, 16, "AlphaActivity", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 17, "TransuraniumActivity", scp, TopName));
            stck.Children.Add(Get2Row(1.6, Row, 18, "MainRadionuclids", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 19, "Subsidy", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 20, "FcpNumber", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.3
        private static Control Get3Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };
            cell.Bind(StyledElement.DataContextProperty, b);

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }
        private static Control Get3(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get3Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 2, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 3, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 4, "ProjectVolume", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 5, "CodeRAO", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 6, "Volume", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 7, "Mass", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 8, "QuantityOZIII", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 9, "SummaryActivity", scp, TopName));
            stck.Children.Add(Get3Row(1.2, Row, 10, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 11, "DocumentDate", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 12, "ExpirationDate", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 13, "DocumentName", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.4
        private static Control Get4Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };
            cell.Bind(StyledElement.DataContextProperty, b);

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }
        private static Control Get4(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get4Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 2, "CodeOYAT", scp, TopName));
            stck.Children.Add(Get4Row(2, Row, 3, "FcpNumber", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 4, "MassCreated", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 5, "QuantityCreated", scp, TopName));
            stck.Children.Add(Get4Row(1.5, Row, 6, "MassFromAnothers", scp, TopName));
            stck.Children.Add(Get4Row(1.5, Row, 7, "QuantityFromAnothers", scp, TopName));
            stck.Children.Add(Get4Row(1.5, Row, 8, "MassFromAnothersImported", scp, TopName));
            stck.Children.Add(Get4Row(1.5, Row, 9, "QuantityFromAnothersImported", scp, TopName));
            stck.Children.Add(Get4Row(1.75, Row, 10, "MassAnotherReasons", scp, TopName));
            stck.Children.Add(Get4Row(1.75, Row, 11, "QuantityAnotherReasons", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 12, "MassTransferredToAnother", scp, TopName));
            stck.Children.Add(Get4Row(1.5, Row, 13, "QuantityTransferredToAnother", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 14, "MassRefined", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 15, "QuantityRefined", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 16, "MassRemovedFromAccount", scp, TopName));
            stck.Children.Add(Get4Row(1.33, Row, 17, "QuantityRemovedFromAccount", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.5
        private static Control Get5Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };
            cell.Bind(StyledElement.DataContextProperty, b);

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }
        private static Control Get5(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get5Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get5Row(2, Row, 2, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 3, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 4, "CodeOYAT", scp, TopName));
            stck.Children.Add(Get5Row(2, Row, 5, "FcpNumber", scp, TopName));
            stck.Children.Add(Get5Row(1.2, Row, 6, "FuelMass", scp, TopName));
            stck.Children.Add(Get5Row(3, Row, 7, "CellMass", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 8, "Quantity", scp, TopName));
            stck.Children.Add(Get5Row(3.5, Row, 9, "AlphaActivity", scp, TopName));
            stck.Children.Add(Get5Row(3.5, Row, 10, "BetaGammaActivity", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.6
        private static Control Get6Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };
            cell.Bind(StyledElement.DataContextProperty, b);

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }
        private static Control Get6(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get6Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get6Row(2.1, Row, 2, "ObservedSourceNumber", scp, TopName));
            stck.Children.Add(Get6Row(2, Row, 3, "ControlledAreaName", scp, TopName));
            stck.Children.Add(Get6Row(4, Row, 4, "SupposedWasteSource", scp, TopName));
            stck.Children.Add(Get6Row(5.5, Row, 5, "DistanceToWasteSource", scp, TopName));
            stck.Children.Add(Get6Row(2, Row, 6, "TestDepth", scp, TopName));
            stck.Children.Add(Get6Row(2, Row, 7, "RadionuclidName", scp, TopName));
            stck.Children.Add(Get6Row(3, Row, 8, "AverageYearConcentration", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.7
        private static Control Get7Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {

            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };
            cell.Bind(StyledElement.DataContextProperty, b);

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;

        }
        private static Control Get7(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get7Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get7Row(3, Row, 2, "ObservedSourceNumber", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 3, "RadionuclidName", scp, TopName));
            stck.Children.Add(Get7Row(1.7, Row, 4, "AllowedWasteValue", scp, TopName));
            stck.Children.Add(Get7Row(1.7, Row, 5, "FactedWasteValue", scp, TopName));
            stck.Children.Add(Get7Row(3.6, Row, 6, "WasteOutbreakPreviousYear", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.8
        private static Control Get8Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };
            cell.Bind(StyledElement.DataContextProperty, b);

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;

        }
        private static Control Get8(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get8Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get8Row(3, Row, 2, "WasteSourceName", scp, TopName));
            stck.Children.Add(Get8Row(3, Row, 3, "WasteRecieverName", scp, TopName));
            stck.Children.Add(Get8Row(2.5, Row, 4, "RecieverTypeCode", scp, TopName));
            stck.Children.Add(Get8Row(4, Row, 5, "PoolDistrictName", scp, TopName));
            stck.Children.Add(Get8Row(3.2, Row, 6, "AllowedWasteRemovalVolume", scp, TopName));
            stck.Children.Add(Get8Row(3, Row, 7, "RemovedWasteVolume", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.9
        private static Control Get9Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            if (Binding.Equals("AllowedActivity"))
            {
                Binding b = new()
                {
                    Path = "Items[" + (Row - 1) + "]." + Binding,
                    ElementName = TopName,
                    //Converter = new MyConverter(),
                    NameScope = new WeakReference<INameScope>(scp)
                };
                cell.Bind(StyledElement.DataContextProperty, b);
            }
            else
            {
                Binding b = new()
                {
                    Path = "Items[" + (Row - 1) + "]." + Binding,
                    ElementName = TopName,
                    NameScope = new WeakReference<INameScope>(scp)
                };
                cell.Bind(StyledElement.DataContextProperty, b);
            }

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }
        private static Control Get9(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get9Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get9Row(3, Row, 2, "WasteSourceName", scp, TopName));
            stck.Children.Add(Get9Row(2, Row, 3, "RadionuclidName", scp, TopName));
            stck.Children.Add(Get9Row(1, Row, 4, "AllowedActivity", scp, TopName));
            stck.Children.Add(Get9Row(1, Row, 5, "FactedActivity", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.10
        private static Control Get10Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };
            cell.Bind(StyledElement.DataContextProperty, b);

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;

        }
        private static Control Get10(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get10Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get10Row(2, Row, 2, "IndicatorName", scp, TopName));
            stck.Children.Add(Get10Row(2, Row, 3, "PlotName", scp, TopName));
            stck.Children.Add(Get10Row(2, Row, 4, "PlotKadastrNumber", scp, TopName));
            stck.Children.Add(Get10Row(1, Row, 5, "PlotCode", scp, TopName));
            stck.Children.Add(Get10Row(3, Row, 6, "InfectedArea", scp, TopName));
            stck.Children.Add(Get10Row(1.2, Row, 7, "AvgGammaRaysDosePower", scp, TopName));
            stck.Children.Add(Get10Row(1.6, Row, 8, "MaxGammaRaysDosePower", scp, TopName));
            stck.Children.Add(Get10Row(2.4, Row, 9, "WasteDensityAlpha", scp, TopName));
            stck.Children.Add(Get10Row(2.4, Row, 10, "WasteDensityBeta", scp, TopName));
            stck.Children.Add(Get10Row(2, Row, 11, "FcpNumber", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.11
        private static Control Get11Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {

            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };
            cell.Bind(StyledElement.DataContextProperty, b);

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;

        }
        private static Control Get11(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get11Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get11Row(2, Row, 2, "PlotName", scp, TopName));
            stck.Children.Add(Get11Row(2, Row, 3, "PlotKadastrNumber", scp, TopName));
            stck.Children.Add(Get11Row(1, Row, 4, "PlotCode", scp, TopName));
            stck.Children.Add(Get11Row(3, Row, 5, "InfectedArea", scp, TopName));
            stck.Children.Add(Get11Row(2, Row, 6, "Radionuclids", scp, TopName));
            stck.Children.Add(Get11Row(1.5, Row, 7, "SpecificActivityOfPlot", scp, TopName));
            stck.Children.Add(Get11Row(1, Row, 8, "SpecificActivityOfLiquidPart", scp, TopName));
            stck.Children.Add(Get11Row(1.5, Row, 9, "SpecificActivityOfDensePart", scp, TopName));

            return stck;
        }
        #endregion

        #region 2.12
        private static Control Get12Row(double starWidth, int Row, int Column, string Binding, INameScope scp, string TopName)
        {

            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };
            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]." + Binding,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };
            cell.Bind(StyledElement.DataContextProperty, b);

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;

        }
        private static Control Get12(int Row, INameScope scp, string TopName)
        {
            DataGrid.Row stck = new DataGrid.Row
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new Binding
            {
                Path = "Items[" + (Row - 1).ToString() + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StackPanel.DataContextProperty, b);

            stck.Children.Add(Get12Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get12Row(1, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get12Row(2, Row, 3, "ObjectTypeCode", scp, TopName));
            stck.Children.Add(Get12Row(1.1, Row, 4, "Radionuclids", scp, TopName));
            stck.Children.Add(Get12Row(1, Row, 5, "Activity", scp, TopName));
            stck.Children.Add(Get12Row(2, Row, 6, "ProviderOrRecieverOKPO", scp, TopName));

            return stck;
        }
        #endregion
    }
}
