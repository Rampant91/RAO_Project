using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Client_App.Controls.DataGrid;
using Models.DataAccess;

namespace Client_App.Controls.Support.RenderDataGridRow
{
    public class Form1
    {
        public static Control GetControl(string type, int Row, INameScope scp, string TopName)
        {
            switch (type)
            {
                case "1": return Get1(Row, scp, TopName);
                case "2": return Get2(Row, scp, TopName);
                case "3": return Get3(Row, scp, TopName);
                case "4": return Get4(Row, scp, TopName);
                case "5": return Get5(Row, scp, TopName);
                case "6": return Get6(Row, scp, TopName);
                case "7": return Get7(Row, scp, TopName);
                case "8": return Get8(Row, scp, TopName);
                case "9": return Get9(Row, scp, TopName);
                case "1*": return GetNotes(Row, scp, TopName);
            }

            return null;
        }

        private static Control Get3Row(double starWidth, int Row, int Column, string Binding, INameScope scp,
            string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };

            if (Column != 1)
            {
                Binding b = new()
                {
                    Path = "Items[" + (Row - 1) + "]." + Binding,
                    ElementName = TopName,
                    NameScope = new WeakReference<INameScope>(scp)
                };
                cell.Bind(StyledElement.DataContextProperty, b);
            }
            else
            {
                var sub = cell.GetSubject(Cell.CellRowProperty);

                cell.Bind(StyledElement.DataContextProperty, sub.Select(x =>
                {
                    var obj = new RamAccess<int>(null, x);
                    return obj;
                }));
            }

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }

        private static Control Get4Row(double starWidth, int Row, int Column, string Binding, INameScope scp,
            string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };

            if (Column != 1)
            {
                Binding b = new()
                {
                    Path = "Items[" + (Row - 1) + "]." + Binding,
                    ElementName = TopName,
                    NameScope = new WeakReference<INameScope>(scp)
                };
                cell.Bind(StyledElement.DataContextProperty, b);
            }
            else
            {
                var sub = cell.GetSubject(Cell.CellRowProperty);

                cell.Bind(StyledElement.DataContextProperty, sub.Select(x =>
                {
                    var obj = new RamAccess<int>(null, x);
                    return obj;
                }));
            }

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }

        private static Control Get5Row(double starWidth, int Row, int Column, string Binding, INameScope scp,
            string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };

            if (Column != 1)
            {
                Binding b = new()
                {
                    Path = "Items[" + (Row - 1) + "]." + Binding,
                    ElementName = TopName,
                    NameScope = new WeakReference<INameScope>(scp)
                };
                cell.Bind(StyledElement.DataContextProperty, b);
            }
            else
            {
                var sub = cell.GetSubject(Cell.CellRowProperty);

                cell.Bind(StyledElement.DataContextProperty, sub.Select(x =>
                {
                    var obj = new RamAccess<int>(null, x);
                    return obj;
                }));
            }

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }

        private static Control Get6Row(double starWidth, int Row, int Column, string Binding, INameScope scp,
            string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };

            if (Column != 1)
            {
                Binding b = new()
                {
                    Path = "Items[" + (Row - 1) + "]." + Binding,
                    ElementName = TopName,
                    NameScope = new WeakReference<INameScope>(scp)
                };
                cell.Bind(StyledElement.DataContextProperty, b);
            }
            else
            {
                var sub = cell.GetSubject(Cell.CellRowProperty);

                cell.Bind(StyledElement.DataContextProperty, sub.Select(x =>
                {
                    var obj = new RamAccess<int>(null, x);
                    return obj;
                }));
            }

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }

        private static Control Get7Row(double starWidth, int Row, int Column, string Binding, INameScope scp,
            string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };

            if (Column != 1)
            {
                Binding b = new()
                {
                    Path = "Items[" + (Row - 1) + "]." + Binding,
                    ElementName = TopName,
                    NameScope = new WeakReference<INameScope>(scp)
                };
                cell.Bind(StyledElement.DataContextProperty, b);
            }
            else
            {
                var sub = cell.GetSubject(Cell.CellRowProperty);

                cell.Bind(StyledElement.DataContextProperty, sub.Select(x =>
                {
                    var obj = new RamAccess<int>(null, x);
                    return obj;
                }));
            }

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }

        private static Control Get8Row(double starWidth, int Row, int Column, string Binding, INameScope scp,
            string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };

            if (Column != 1)
            {
                Binding b = new()
                {
                    Path = "Items[" + (Row - 1) + "]." + Binding,
                    ElementName = TopName,
                    NameScope = new WeakReference<INameScope>(scp)
                };
                cell.Bind(StyledElement.DataContextProperty, b);
            }
            else
            {
                var sub = cell.GetSubject(Cell.CellRowProperty);

                cell.Bind(StyledElement.DataContextProperty, sub.Select(x =>
                {
                    var obj = new RamAccess<int>(null, x);
                    return obj;
                }));
            }

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }

        private static Control Get9Row(double starWidth, int Row, int Column, string Binding, INameScope scp,
            string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1)
            };

            if (Column != 1)
            {
                Binding b = new()
                {
                    Path = "Items[" + (Row - 1) + "]." + Binding,
                    ElementName = TopName,
                    NameScope = new WeakReference<INameScope>(scp)
                };
                cell.Bind(StyledElement.DataContextProperty, b);
            }
            else
            {
                var sub = cell.GetSubject(Cell.CellRowProperty);

                cell.Bind(StyledElement.DataContextProperty, sub.Select(x =>
                {
                    var obj = new RamAccess<int>(null, x);
                    return obj;
                }));
            }

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }

        private static Control Get3(int Row, INameScope scp, string TopName)
        {
            Row stck = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StyledElement.DataContextProperty, b);

            stck.Children.Add(Get3Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 3, "OperationDate", scp, TopName));
            stck.Children.Add(Get3Row(1.1, Row, 4, "PassportNumber", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 5, "Type", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 6, "Radionuclids", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 7, "FactoryNumber", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 8, "Activity", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 9, "CreatorOKPO", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 10, "CreationDate", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 11, "AggregateState", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 12, "PropertyCode", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 13, "Owner", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 14, "DocumentVid", scp, TopName));
            stck.Children.Add(Get3Row(1.2, Row, 15, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 16, "DocumentDate", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 17, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 18, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 19, "PackName", scp, TopName));
            stck.Children.Add(Get3Row(1, Row, 20, "PackType", scp, TopName));
            stck.Children.Add(Get3Row(2, Row, 21, "PackNumber", scp, TopName));

            return stck;
        }

        private static Control Get4(int Row, INameScope scp, string TopName)
        {
            Row stck = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StyledElement.DataContextProperty, b);

            stck.Children.Add(Get4Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 3, "OperationDate", scp, TopName));
            stck.Children.Add(Get4Row(1.1, Row, 4, "PassportNumber", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 5, "Name", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 6, "Sort", scp, TopName));
            stck.Children.Add(Get4Row(2, Row, 7, "Radionuclids", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 8, "Activity", scp, TopName));
            stck.Children.Add(Get4Row(2, Row, 9, "ActivityMeasurementDate", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 10, "Volume", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 11, "Mass", scp, TopName));
            stck.Children.Add(Get4Row(2, Row, 12, "AggregateState", scp, TopName));
            stck.Children.Add(Get4Row(2, Row, 13, "PropertyCode", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 14, "Owner", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 15, "DocumentVid", scp, TopName));
            stck.Children.Add(Get4Row(1.2, Row, 16, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 17, "DocumentDate", scp, TopName));
            stck.Children.Add(Get4Row(2, Row, 18, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get4Row(2, Row, 19, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get4Row(2, Row, 20, "PackName", scp, TopName));
            stck.Children.Add(Get4Row(1, Row, 21, "PackType", scp, TopName));
            stck.Children.Add(Get4Row(2, Row, 22, "PackNumber", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 23, "PassportNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 24, "PackTypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 25, "PackNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 26, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get5(int Row, INameScope scp, string TopName)
        {
            Row stck = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StyledElement.DataContextProperty, b);

            stck.Children.Add(Get5Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 3, "OperationDate", scp, TopName));
            stck.Children.Add(Get5Row(1.1, Row, 4, "PassportNumber", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 5, "Type", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 6, "Radionuclids", scp, TopName));
            stck.Children.Add(Get5Row(2, Row, 7, "FactoryNumber", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 8, "Quantity", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 9, "Activity", scp, TopName));
            stck.Children.Add(Get5Row(2, Row, 10, "CreationDate", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 11, "StatusRAO", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 12, "DocumentVid", scp, TopName));
            stck.Children.Add(Get5Row(1.2, Row, 13, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 14, "DocumentDate", scp, TopName));
            stck.Children.Add(Get5Row(2, Row, 15, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get5Row(2, Row, 16, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get5Row(2, Row, 17, "PackName", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 18, "PackType", scp, TopName));
            stck.Children.Add(Get5Row(2, Row, 19, "PackNumber", scp, TopName));
            stck.Children.Add(Get5Row(1.2, Row, 20, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 21, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get5Row(2.1, Row, 22, "RefineOrSortRAOCode", scp, TopName));
            stck.Children.Add(Get5Row(1, Row, 23, "Subsidy", scp, TopName));
            stck.Children.Add(Get5Row(2, Row, 24, "FcpNumber", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 25, "PassportNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 26, "TypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 27, "FactoryNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 28, "PackNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 29, "PackTypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 30, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get6(int Row, INameScope scp, string TopName)
        {
            Row stck = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StyledElement.DataContextProperty, b);

            stck.Children.Add(Get6Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 3, "OperationDate", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 4, "CodeRAO", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 5, "StatusRAO", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 6, "Volume", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 7, "Mass", scp, TopName));
            stck.Children.Add(Get6Row(1.5, Row, 8, "QuantityOZIII", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 9, "MainRadionuclids", scp, TopName));
            stck.Children.Add(Get6Row(2, Row, 10, "TritiumActivity", scp, TopName));
            stck.Children.Add(Get6Row(3.5, Row, 11, "BetaGammaActivity", scp, TopName));
            stck.Children.Add(Get6Row(3.5, Row, 12, "AlphaActivity", scp, TopName));
            stck.Children.Add(Get6Row(2, Row, 13, "TransuraniumActivity", scp, TopName));
            stck.Children.Add(Get6Row(2, Row, 14, "ActivityMeasurementDate", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 15, "DocumentVid", scp, TopName));
            stck.Children.Add(Get6Row(1.2, Row, 16, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 17, "DocumentDate", scp, TopName));
            stck.Children.Add(Get6Row(2, Row, 18, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get6Row(2, Row, 19, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get6Row(1.2, Row, 20, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 21, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get6Row(2.1, Row, 22, "RefineOrSortRAOCode", scp, TopName));
            stck.Children.Add(Get6Row(2, Row, 23, "PackName", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 24, "PackType", scp, TopName));
            stck.Children.Add(Get6Row(2, Row, 25, "PackNumber", scp, TopName));
            stck.Children.Add(Get6Row(1, Row, 26, "Subsidy", scp, TopName));
            stck.Children.Add(Get6Row(2, Row, 27, "FcpNumber", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 28, "PackTypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 29, "PackNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 30, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get7(int Row, INameScope scp, string TopName)
        {
            Row stck = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StyledElement.DataContextProperty, b);

            stck.Children.Add(Get7Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get7Row(1, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get7Row(1, Row, 3, "OperationDate", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 4, "PackName", scp, TopName));
            stck.Children.Add(Get7Row(1, Row, 5, "PackType", scp, TopName));
            stck.Children.Add(Get7Row(3, Row, 6, "PackFactoryNumber", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 7, "PackNumber", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 8, "FormingDate", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 9, "PassportNumber", scp, TopName));
            stck.Children.Add(Get7Row(1, Row, 10, "Volume", scp, TopName));
            stck.Children.Add(Get7Row(1, Row, 11, "Mass", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 12, "Radionuclids", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 13, "SpecificActivity", scp, TopName));
            stck.Children.Add(Get7Row(1.2, Row, 14, "DocumentVid", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 15, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get7Row(1, Row, 16, "DocumentDate", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 17, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 18, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get7Row(1.2, Row, 19, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get7Row(1, Row, 20, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get7Row(1, Row, 21, "CodeRAO", scp, TopName));
            stck.Children.Add(Get7Row(1, Row, 22, "StatusRAO", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 23, "VolumeOutOfPack", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 24, "MassOutOfPack", scp, TopName));
            stck.Children.Add(Get7Row(1, Row, 25, "Quantity", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 26, "TritiumActivity", scp, TopName));
            stck.Children.Add(Get7Row(3.5, Row, 27, "BetaGammaActivity", scp, TopName));
            stck.Children.Add(Get7Row(3.5, Row, 28, "AlphaActivity", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 29, "TransuraniumActivity", scp, TopName));
            stck.Children.Add(Get7Row(2.1, Row, 30, "RefineOrSortRAOCode", scp, TopName));
            stck.Children.Add(Get7Row(1, Row, 31, "Subsidy", scp, TopName));
            stck.Children.Add(Get7Row(2, Row, 32, "FcpNumber", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 33, "PackTypeRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 34, "PackNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 35, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get8(int Row, INameScope scp, string TopName)
        {
            Row stck = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StyledElement.DataContextProperty, b);

            stck.Children.Add(Get8Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 3, "OperationDate", scp, TopName));
            stck.Children.Add(Get8Row(2, Row, 4, "IndividualNumberZHRO", scp, TopName));
            stck.Children.Add(Get8Row(1.1, Row, 5, "PassportNumber", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 6, "Volume6", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 7, "Mass7", scp, TopName));
            stck.Children.Add(Get8Row(1.5, Row, 8, "SaltConcentration", scp, TopName));
            stck.Children.Add(Get8Row(2, Row, 9, "Radionuclids", scp, TopName));
            stck.Children.Add(Get8Row(2, Row, 10, "SpecificActivity", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 11, "DocumentVid", scp, TopName));
            stck.Children.Add(Get8Row(1.2, Row, 12, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 13, "DocumentDate", scp, TopName));
            stck.Children.Add(Get8Row(2, Row, 14, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get8Row(2, Row, 15, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get8Row(1.2, Row, 16, "StoragePlaceName", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 17, "StoragePlaceCode", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 18, "CodeRAO", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 19, "StatusRAO", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 20, "Volume20", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 21, "Mass21", scp, TopName));
            stck.Children.Add(Get8Row(2, Row, 22, "TritiumActivity", scp, TopName));
            stck.Children.Add(Get8Row(3.5, Row, 23, "BetaGammaActivity", scp, TopName));
            stck.Children.Add(Get8Row(3.5, Row, 24, "AlphaActivity", scp, TopName));
            stck.Children.Add(Get8Row(2, Row, 25, "TransuraniumActivity", scp, TopName));
            stck.Children.Add(Get8Row(2.1, Row, 26, "RefineOrSortRAOCode", scp, TopName));
            stck.Children.Add(Get8Row(1, Row, 27, "Subsidy", scp, TopName));
            stck.Children.Add(Get8Row(2, Row, 28, "FcpNumber", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 29, "PassportNumberRecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 30, "IndividualNumberZHROrecoded", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 31, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control Get9(int Row, INameScope scp, string TopName)
        {
            Row stck = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StyledElement.DataContextProperty, b);

            stck.Children.Add(Get9Row(1, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get9Row(1, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get9Row(1, Row, 3, "OperationDate", scp, TopName));
            stck.Children.Add(Get9Row(1, Row, 4, "DocumentVid", scp, TopName));
            stck.Children.Add(Get9Row(1.2, Row, 5, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get9Row(1, Row, 6, "DocumentDate", scp, TopName));
            stck.Children.Add(Get9Row(2, Row, 7, "CodeTypeAccObject", scp, TopName));
            stck.Children.Add(Get9Row(1, Row, 8, "Radionuclids", scp, TopName));
            //stck.Children.Add(Get9Row(1, Row, 9, "Quantity", scp, TopName));
            stck.Children.Add(Get9Row(1, Row, 9, "Activity", scp, TopName));
            //stck.Children.Add(Get1Row(1, Row, 10, "DocumentNumberRecoded", scp, TopName));

            //var bd = "StartPeriod";
            //bd.StringFormat = "{0:d}";

            return stck;
        }

        private static Control GetRowNotes(double starWidth, int Row, int Column, string Binding, INameScope scp,
            string TopName)
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

        private static Control GetNotes(int Row, INameScope scp, string TopName)
        {
            Row stck = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]",
                Mode = BindingMode.OneTime,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StyledElement.DataContextProperty, b);

            stck.Children.Add(GetRowNotes(1, Row, 1, "RowNumber", scp, TopName));
            stck.Children.Add(GetRowNotes(1, Row, 2, "GraphNumber", scp, TopName));
            stck.Children.Add(GetRowNotes(1, Row, 3, "Comment", scp, TopName));

            return stck;
        }

        #region 1.1

        private static readonly int Wdth1 = 100;
        private static readonly int RowHeight1 = 30;
        private static readonly Color border_color1 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get1Row(double starWidth, int Row, int Column, string Binding, INameScope scp,
            string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                ZIndex = 10000
            };
            if (Column != 1)
            {
                Binding b = new()
                {
                    Path = "Items[" + (Row - 1) + "]." + Binding,
                    ElementName = TopName,
                    NameScope = new WeakReference<INameScope>(scp)
                };
                cell.Bind(StyledElement.DataContextProperty, b);
            }
            else
            {
                var sub = cell.GetSubject(Cell.CellRowProperty);

                cell.Bind(StyledElement.DataContextProperty, sub.Select(x =>
                {
                    var obj = new RamAccess<int>(null, x);
                    return obj;
                }));
            }

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }

        private static Control Get1(int Row, INameScope scp, string TopName)
        {
            Row stck = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]",
                Mode = BindingMode.OneTime,
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StyledElement.DataContextProperty, b);

            stck.Children.Add(Get1Row(0.5, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 3, "OperationDate", scp, TopName));
            stck.Children.Add(Get1Row(1.5, Row, 4, "PassportNumber", scp, TopName));
            stck.Children.Add(Get1Row(0.5, Row, 5, "Type", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 6, "Radionuclids", scp, TopName));
            stck.Children.Add(Get1Row(1.5, Row, 7, "FactoryNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 8, "Quantity", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 9, "Activity", scp, TopName));
            stck.Children.Add(Get1Row(1.5, Row, 10, "CreatorOKPO", scp, TopName));
            stck.Children.Add(Get1Row(1.5, Row, 11, "CreationDate", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 12, "Category", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 13, "SignedServicePeriod", scp, TopName));
            stck.Children.Add(Get1Row(1.5, Row, 14, "PropertyCode", scp, TopName));
            stck.Children.Add(Get1Row(1.5, Row, 15, "Owner", scp, TopName));
            stck.Children.Add(Get1Row(1.5, Row, 16, "DocumentVid", scp, TopName));
            stck.Children.Add(Get1Row(1.5, Row, 17, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 18, "DocumentDate", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 19, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 20, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get1Row(2, Row, 21, "PackName", scp, TopName));
            stck.Children.Add(Get1Row(1, Row, 22, "PackType", scp, TopName));
            stck.Children.Add(Get1Row(1.5, Row, 23, "PackNumber", scp, TopName));

            return stck;
        }

        #endregion

        #region 1.2

        private static readonly int Wdth2 = 100;
        private static readonly int RowHeight2 = 30;
        private static readonly Color border_color2 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get2Row(double starWidth, int Row, int Column, string Binding, INameScope scp,
            string TopName)
        {
            var cell = new Cell(Binding, false)
            {
                Width = starWidth * Wdth2,
                Height = RowHeight2,
                BorderBrush = new SolidColorBrush(border_color2)
            };

            if (Column != 1)
            {
                Binding b = new()
                {
                    Path = "Items[" + (Row - 1) + "]." + Binding,
                    ElementName = TopName,
                    NameScope = new WeakReference<INameScope>(scp)
                };
                cell.Bind(StyledElement.DataContextProperty, b);
            }
            else
            {
                var sub = cell.GetSubject(Cell.CellRowProperty);

                cell.Bind(StyledElement.DataContextProperty, sub.Select(x =>
                {
                    var obj = new RamAccess<int>(null, x);
                    return obj;
                }));
            }

            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }

        private static Control Get2(int Row, INameScope scp, string TopName)
        {
            Row stck = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Width = 24 * Wdth1,
                Spacing = 0,
                SRow = Row
            };

            Binding b = new()
            {
                Path = "Items[" + (Row - 1) + "]",
                ElementName = TopName,
                NameScope = new WeakReference<INameScope>(scp)
            };

            stck.Bind(StyledElement.DataContextProperty, b);

            stck.Children.Add(Get2Row(0.5, Row, 1, "NumberInOrder", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 2, "OperationCode", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 3, "OperationDate", scp, TopName));
            stck.Children.Add(Get2Row(1.5, Row, 4, "PassportNumber", scp, TopName));
            stck.Children.Add(Get2Row(1.5, Row, 5, "NameIOU", scp, TopName));
            stck.Children.Add(Get2Row(1.5, Row, 6, "FactoryNumber", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 7, "Mass", scp, TopName));
            stck.Children.Add(Get2Row(1.5, Row, 8, "CreatorOKPO", scp, TopName));
            stck.Children.Add(Get2Row(1.5, Row, 9, "CreationDate", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 10, "SignedServicePeriod", scp, TopName));
            stck.Children.Add(Get2Row(1.5, Row, 11, "PropertyCode", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 12, "Owner", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 13, "DocumentVid", scp, TopName));
            stck.Children.Add(Get2Row(1.5, Row, 14, "DocumentNumber", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 15, "DocumentDate", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 16, "ProviderOrRecieverOKPO", scp, TopName));
            stck.Children.Add(Get2Row(1.5, Row, 17, "TransporterOKPO", scp, TopName));
            stck.Children.Add(Get2Row(2, Row, 18, "PackName", scp, TopName));
            stck.Children.Add(Get2Row(1, Row, 19, "PackType", scp, TopName));
            stck.Children.Add(Get2Row(1.5, Row, 20, "PackNumber", scp, TopName));

            return stck;
        }

        #endregion
    }
}