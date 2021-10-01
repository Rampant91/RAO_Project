using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Client_App.Controls.DataGrid;
using Models.Attributes;
using Models.DataAccess;

namespace Client_App.Controls.Support.RenderDataGridHeader
{
    public class Form2
    {
        public static Control GetControl(string type)
        {
            switch (type)
            {
                case "1": return Get1();
                case "2": return Get2();
                case "3": return Get3();
                case "4": return Get4();
                case "5": return Get5();
                case "6": return Get6();
                case "7": return Get7();
                case "8": return Get8();
                case "9": return Get9();
                case "10": return Get10();
                case "11": return Get11();
                case "12": return Get12();
                case "1*": return GetNotes();
            }

            return null;
        }

        #region 2.1

        private static readonly int Wdth1 = 100;
        private static readonly int RowHeight1 = 30;
        private static readonly Color border_color1 = Color.FromArgb(255, 0, 0, 0);
        private static Control Get1Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = 0,
                    CellColumn = Column
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = 0,
                    CellColumn = Column
                };

                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = 0,
                    CellColumn = Column
                };
                var stckPnl = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                stckPnl.Children.Add(cell0);
                stckPnl.Children.Add(cell);
                stckPnl.Children.Add(cell1);
                return stckPnl;
            }
            else
            {
                int len = Text.Length;
                var headers = new RamAccess<string>[len];
                var cells = new Cell[len];
                var stckPnl = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (int k = 0; k < len - 1; k++)
                {
                    headers[k] = new RamAccess<string>(null, Text[k]);
                    cells[k] = new Cell(headers[k], "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth1,
                        Height = RowHeight1,
                        BorderBrush = new SolidColorBrush(border_color1),
                        CellRow = 0,
                        CellColumn = Column
                    };
                    stckPnl.Children.Add(cells[k]);
                }
                var stckPnl1 = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                var ram0 = new RamAccess<string>(null, Text[len - 1]);
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = 0,
                    CellColumn = Column
                };
                var stckPnl2 = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (int k = 0; k < len - 1; k++)
                {
                    headers[k] = new RamAccess<string>(null, (offset + k + 1).ToString());
                    cells[k] = new Cell(headers[k], "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth1,
                        Height = RowHeight1,
                        BorderBrush = new SolidColorBrush(border_color1),
                        CellRow = 0,
                        CellColumn = Column
                    };
                    stckPnl2.Children.Add(cells[k]);
                }
                stckPnl1.Children.Add(cell0);
                stckPnl1.Children.Add(stckPnl);
                stckPnl1.Children.Add(stckPnl2);
                return stckPnl1;
            }
        }

        private static Control Get1()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            //stck.Children.Add(Get1Header(1, 1,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("NumberInOrder")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get1Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("NumberInOrder")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,0));

            //stck.Children.Add(Get1Header(2, 2,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("RefineMachineName")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(2, 3,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("MachineCode")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(2, 4,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("MachinePower")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(2, 5,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("NumberOfHoursPerYear")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get1Header(new double[5] { 2.5,2,2,2,8.5 }, 2, new string[5] {
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("RefineMachineName")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("MachineCode")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("MachinePower")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("NumberOfHoursPerYear")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            "Установки переработки"
            }, true,1));

            //stck.Children.Add(Get1Header(1, 6,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("CodeRAOIn")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(1, 7,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("StatusRAOIn")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(1, 8,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("VolumeIn")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(1, 9,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("MassIn")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(1, 10,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("QuantityIn")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(2, 11,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("TritiumActivityIn")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(2, 12,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("BetaGammaActivityIn")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(2, 13,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("AlphaActivityIn")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(2, 14,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("TransuraniumActivityIn")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get1Header(new double[10] { 1, 1, 1, 1, 1.5, 2, 3.5, 3.5, 2, 16.5 }, 3, new string[10] {
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("CodeRAOIn")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("StatusRAOIn")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("VolumeIn")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("MassIn")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("QuantityIn")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("TritiumActivityIn")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("BetaGammaActivityIn")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("AlphaActivityIn")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("TransuraniumActivityIn")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            "Поступило РАО на переработку, кондиционирование"
            }, true,5));

            //stck.Children.Add(Get1Header(1, 15,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("CodeRAOout")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(1, 16,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("StatusRAOout")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(1, 17,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("VolumeOut")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(1, 18,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("MassOut")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(1, 19,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("QuantityOZIIIout")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(2, 20,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("TritiumActivityOut")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(2, 21,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("BetaGammaActivityOut")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(2, 22,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("AlphaActivityOut")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get1Header(2, 23,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("TransuraniumActivityOut")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get1Header(new double[10] { 1, 1, 1, 1, 1.5, 2, 3.5, 3.5, 2, 16.5 }, 3, new string[10] {
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("CodeRAOout")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("StatusRAOout")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("VolumeOut")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("MassOut")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("QuantityOZIIIout")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("TritiumActivityOut")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("BetaGammaActivityOut")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("AlphaActivityOut")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("TransuraniumActivityOut")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            "Образовалось РАО после переработки, кондиционирования"
            }, true,14));
            return stck;
        }

        #endregion

        #region 2.2

        private static readonly int Wdth2 = 100;
        private static readonly int RowHeight2 = 30;
        private static readonly Color border_color2 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get2Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth2,
                    Height = RowHeight2,
                    BorderBrush = new SolidColorBrush(border_color2),
                    CellRow = 0,
                    CellColumn = Column
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth2,
                    Height = RowHeight2,
                    BorderBrush = new SolidColorBrush(border_color2),
                    CellRow = 0,
                    CellColumn = Column
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth2,
                    Height = RowHeight2,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = 0,
                    CellColumn = Column
                };
                var stckPnl = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                stckPnl.Children.Add(cell0);
                stckPnl.Children.Add(cell);
                stckPnl.Children.Add(cell1);
                return stckPnl;
            }
            else
            {
                int len = Text.Length;
                var headers = new RamAccess<string>[len];
                var cells = new Cell[len];
                var stckPnl = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (int k = 0; k < len - 1; k++)
                {
                    headers[k] = new RamAccess<string>(null, Text[k]);
                    cells[k] = new Cell(headers[k], "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth2,
                        Height = RowHeight2,
                        BorderBrush = new SolidColorBrush(border_color2),
                        CellRow = 0,
                        CellColumn = Column
                    };
                    stckPnl.Children.Add(cells[k]);
                }
                var stckPnl1 = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                var ram0 = new RamAccess<string>(null, Text[len - 1]);
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth2,
                    Height = RowHeight2,
                    BorderBrush = new SolidColorBrush(border_color2),
                    CellRow = 0,
                    CellColumn = Column
                };
                var stckPnl2 = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (int k = 0; k < len - 1; k++)
                {
                    headers[k] = new RamAccess<string>(null, (offset + k + 1).ToString());
                    cells[k] = new Cell(headers[k], "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth2,
                        Height = RowHeight2,
                        BorderBrush = new SolidColorBrush(border_color2),
                        CellRow = 0,
                        CellColumn = Column
                    };
                    stckPnl2.Children.Add(cells[k]);
                }
                stckPnl1.Children.Add(cell0);
                stckPnl1.Children.Add(stckPnl);
                stckPnl1.Children.Add(stckPnl2);
                return stckPnl1;
            }
        }

        private static Control Get2()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            //stck.Children.Add(Get2Header(1, 1,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("NumberInOrder")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("NumberInOrder")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,0));

            //stck.Children.Add(Get2Header(2, 2,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("StoragePlaceName")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get2Header(1, 3,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("StoragePlaceCode")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[3] { 2,1,3 }, 2, new string[3] {
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("StoragePlaceName")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("StoragePlaceCode")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                "Пункт хранения"
            }, true,1));

            //stck.Children.Add(Get2Header(2, 4,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("PackName")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get2Header(1, 5,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("PackType")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get2Header(2, 6,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("PackQuantity")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[4] { 2, 1, 2,5 }, 3, new string[4] {
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("PackName")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("PackType")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("PackQuantity")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                "УКТ, упаковка ли иная учетная единица"
            }, true,3));

            //stck.Children.Add(Get2Header(1, 7,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("CodeRAO")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[1] { 1 }, 4, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("CodeRAO")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,6));

            //stck.Children.Add(Get2Header(1, 8,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("StatusRAO")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[1] { 1 }, 5, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("StatusRAO")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,7));

            //stck.Children.Add(Get2Header(2, 9,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("VolumeOutOfPack")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get2Header(2, 10,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("VolumeInPack")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[3] { 2, 2, 4 }, 6, new string[3] {
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("VolumeOutOfPack")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("VolumeInPack")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                "Объем, куб. м"
            }, true,8));

            //stck.Children.Add(Get2Header(2, 11,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("MassOutOfPack")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get2Header(2, 12,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("MassInPack")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[3] { 2, 2, 4 }, 7, new string[3] {
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("MassOutOfPack")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("MassInPack")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                "Масса, т"
            }, true,10));

            //stck.Children.Add(Get2Header(1, 13,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("QuantityOZIII")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[1] { 2 }, 8, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("QuantityOZIII")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,12));

            //stck.Children.Add(Get2Header(2, 14,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("TritiumActivity")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get2Header(2, 15,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("BetaGammaActivity")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get2Header(2, 16,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("AlphaActivity")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get2Header(2, 17,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("TransuraniumActivity")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[5] { 2, 3.5, 3.5, 2,11 }, 9, new string[5] {
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("TritiumActivity")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("BetaGammaActivity")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("AlphaActivity")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("TransuraniumActivity")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                "Суммарная активность, Бк"
            }, true,13));

            //stck.Children.Add(Get2Header(1, 18,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("MainRadionuclids")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[1] { 1 }, 10, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("MainRadionuclids")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,17));

            //stck.Children.Add(Get2Header(1, 19,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("Subsidy")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[1] { 1 }, 11, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("Subsidy")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,18));

            //stck.Children.Add(Get2Header(2, 20,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("FcpNumber")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get2Header(new double[1] { 2 }, 12, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("FcpNumber")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,19));
            return stck;
        }

        #endregion

        #region 2.3

        private static readonly int Wdth3 = 100;
        private static readonly int RowHeight3 = 30;
        private static readonly Color border_color3 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get3Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = 0,
                    CellColumn = Column
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = 0,
                    CellColumn = Column
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = 0,
                    CellColumn = Column
                };
                var stckPnl = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                stckPnl.Children.Add(cell0);
                stckPnl.Children.Add(cell);
                stckPnl.Children.Add(cell1);
                return stckPnl;
            }
            else
            {
                int len = Text.Length;
                var headers = new RamAccess<string>[len];
                var cells = new Cell[len];
                var stckPnl = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (int k = 0; k < len - 1; k++)
                {
                    headers[k] = new RamAccess<string>(null, Text[k]);
                    cells[k] = new Cell(headers[k], "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth3,
                        Height = RowHeight3,
                        BorderBrush = new SolidColorBrush(border_color3),
                        CellRow = 0,
                        CellColumn = Column
                    };
                    stckPnl.Children.Add(cells[k]);
                }
                var stckPnl1 = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                var ram0 = new RamAccess<string>(null, Text[len - 1]);
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = 0,
                    CellColumn = Column
                };
                var stckPnl2 = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (int k = 0; k < len - 1; k++)
                {
                    headers[k] = new RamAccess<string>(null, (offset + k + 1).ToString());
                    cells[k] = new Cell(headers[k], "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth3,
                        Height = RowHeight3,
                        BorderBrush = new SolidColorBrush(border_color3),
                        CellRow = 0,
                        CellColumn = Column
                    };
                    stckPnl2.Children.Add(cells[k]);
                }
                stckPnl1.Children.Add(cell0);
                stckPnl1.Children.Add(stckPnl);
                stckPnl1.Children.Add(stckPnl2);
                return stckPnl1;
            }
        }

            private static Control Get3()
            {
                StackPanel stck = new()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };

                //stck.Children.Add(Get3Header(1, 1,
                //    ((Form_PropertyAttribute) Type.GetType("Models.Form23,Models").GetProperty("NumberInOrder")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                stck.Children.Add(Get3Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form23,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,0));

                //stck.Children.Add(Get3Header(2, 2,
                //    ((Form_PropertyAttribute) Type.GetType("Models.Form23,Models").GetProperty("StoragePlaceName")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                //stck.Children.Add(Get3Header(1, 3,
                //    ((Form_PropertyAttribute) Type.GetType("Models.Form23,Models").GetProperty("StoragePlaceCode")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                //stck.Children.Add(Get3Header(2, 4,
                //    ((Form_PropertyAttribute) Type.GetType("Models.Form23,Models").GetProperty("ProjectVolume")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                stck.Children.Add(Get3Header(new double[4] { 2, 1, 2, 5 }, 2, new string[4] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("StoragePlaceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("StoragePlaceCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("ProjectVolume").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    "Пункт хранения РАО"
                }, true,1));

                //stck.Children.Add(Get3Header(1, 5,
                //    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("CodeRAO")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                //stck.Children.Add(Get3Header(2, 6,
                //    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("Volume")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                //stck.Children.Add(Get3Header(2, 7,
                //    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("Mass")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                //stck.Children.Add(Get3Header(2, 8,
                //    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("QuantityOZIII")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                //stck.Children.Add(Get3Header(2, 9,
                //    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("SummaryActivity")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                stck.Children.Add(Get3Header(new double[6] { 1, 2, 2, 2, 2 ,9}, 3, new string[6] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("CodeRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("Volume").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("Mass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("QuantityOZIII").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("SummaryActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    "Разрешено к размещению"
                }, true,4));

                //stck.Children.Add(Get3Header(1, 10,
                //    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("DocumentNumber")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                //stck.Children.Add(Get3Header(1, 11,
                //    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("DocumentDate")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                //stck.Children.Add(Get3Header(2, 12,
                //    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("ExpirationDate")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                //stck.Children.Add(Get3Header(2, 13,
                //    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("DocumentName")
                //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                //));
                stck.Children.Add(Get3Header(new double[5] { 1.2, 1, 2, 2, 6.2}, 4, new string[5] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("ExpirationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("DocumentName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    "Наименование и реквизиты документа на размещение РАО"
                }, true,9));
                return stck;
            }

        #endregion

        #region 2.4

        private static readonly int Wdth4 = 100;
        private static readonly int RowHeight4 = 30;
        private static readonly Color border_color4 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get4Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth4,
                    Height = RowHeight4,
                    BorderBrush = new SolidColorBrush(border_color4),
                    CellRow = 0,
                    CellColumn = Column
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth4,
                    Height = RowHeight4,
                    BorderBrush = new SolidColorBrush(border_color4),
                    CellRow = 0,
                    CellColumn = Column
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth4,
                    Height = RowHeight4,
                    BorderBrush = new SolidColorBrush(border_color4),
                    CellRow = 0,
                    CellColumn = Column
                };
                var stckPnl = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                stckPnl.Children.Add(cell0);
                stckPnl.Children.Add(cell);
                stckPnl.Children.Add(cell1);
                return stckPnl;
            }
            else
            {
                int len = Text.Length;
                var headers = new RamAccess<string>[len];
                var cells = new Cell[len];
                var stckPnl = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (int k = 0; k < len-1; k++)
                {
                    headers[k] = new RamAccess<string>(null, Text[k]);
                    cells[k] = new Cell(headers[k], "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth4,
                        Height = RowHeight4,
                        BorderBrush = new SolidColorBrush(border_color4),
                        CellRow = 0,
                        CellColumn = Column
                    };
                    stckPnl.Children.Add(cells[k]);
                }
                var stckPnl1 = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                var ram0 = new RamAccess<string>(null, Text[len-1]);
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len-1] * Wdth4,
                    Height = RowHeight4,
                    BorderBrush = new SolidColorBrush(border_color4),
                    CellRow = 0,
                    CellColumn = Column
                };
                var stckPnl2 = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (int k = 0; k < len - 1; k++)
                {
                    headers[k] = new RamAccess<string>(null, (offset + k + 1).ToString());
                    cells[k] = new Cell(headers[k], "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth4,
                        Height = RowHeight4,
                        BorderBrush = new SolidColorBrush(border_color4),
                        CellRow = 0,
                        CellColumn = Column
                    };
                    stckPnl2.Children.Add(cells[k]);
                }
                stckPnl1.Children.Add(cell0);
                stckPnl1.Children.Add(stckPnl);
                stckPnl1.Children.Add(stckPnl2);
                return stckPnl1;
            }
        }

        private static Control Get4()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            //stck.Children.Add(Get4Header(1, 1,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("NumberInOrder")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get4Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("NumberInOrder")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,0));

            //stck.Children.Add(Get4Header(1, 2,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("CodeOYAT")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get4Header(new double[1] { 1 }, 2, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("CodeOYAT")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,1));

            //stck.Children.Add(Get4Header(2, 3,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("FcpNumber")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get4Header(new double[1] { 2 }, 3, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("FcpNumber")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,2));

            //stck.Children.Add(Get4Header(2, 4,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassCreated")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 5,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityCreated")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 6,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassFromAnothers")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 7,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityFromAnothers")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 8,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassFromAnothersImported")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 9,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models")
            //        .GetProperty("QuantityFromAnothersImported")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 10,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassAnotherReasons")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 11,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityAnotherReasons")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get4Header(new double[9] { 2,2,3,3,3,3,3.5,3.5,23 }, 4, new string[9] {
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassCreated")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityCreated")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassFromAnothers")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityFromAnothers")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassFromAnothersImported")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityFromAnothersImported")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassAnotherReasons")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityAnotherReasons")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            "Поставлено на учет в организации"}, true,3));

            //stck.Children.Add(Get4Header(2, 12,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassTransferredToAnother")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 13,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models")
            //        .GetProperty("QuantityTransferredToAnother")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 14,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassRefined")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 15,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityRefined")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 16,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassRemovedFromAccount")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get4Header(2, 17,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityRemovedFromAccount")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get4Header(new double[7] { 2, 3, 2, 2, 2, 2, 13 }, 5, new string[7] {
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassTransferredToAnother")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityTransferredToAnother")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassRefined")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityRefined")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("MassRemovedFromAccount")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("QuantityRemovedFromAccount")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            "Снято с учета в организации"}, true,11));
            return stck;
        }

        #endregion

        #region 2.5

        private static readonly int Wdth5 = 100;
        private static readonly int RowHeight5 = 30;
        private static readonly Color border_color5 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get5Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth5,
                    Height = RowHeight5,
                    BorderBrush = new SolidColorBrush(border_color5),
                    CellRow = 0,
                    CellColumn = Column
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth5,
                    Height = RowHeight5,
                    BorderBrush = new SolidColorBrush(border_color5),
                    CellRow = 0,
                    CellColumn = Column
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth5,
                    Height = RowHeight5,
                    BorderBrush = new SolidColorBrush(border_color5),
                    CellRow = 0,
                    CellColumn = Column
                };
                var stckPnl = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                stckPnl.Children.Add(cell0);
                stckPnl.Children.Add(cell);
                stckPnl.Children.Add(cell1);
                return stckPnl;
            }
            else
            {
                int len = Text.Length;
                var headers = new RamAccess<string>[len];
                var cells = new Cell[len];
                var stckPnl = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (int k = 0; k < len - 1; k++)
                {
                    headers[k] = new RamAccess<string>(null, Text[k]);
                    cells[k] = new Cell(headers[k], "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth5,
                        Height = RowHeight5,
                        BorderBrush = new SolidColorBrush(border_color5),
                        CellRow = 0,
                        CellColumn = Column
                    };
                    stckPnl.Children.Add(cells[k]);
                }
                var stckPnl1 = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                var ram0 = new RamAccess<string>(null, Text[len - 1]);
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth5,
                    Height = RowHeight5,
                    BorderBrush = new SolidColorBrush(border_color5),
                    CellRow = 0,
                    CellColumn = Column
                };
                var stckPnl2 = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                for (int k = 0; k < len - 1; k++)
                {
                    headers[k] = new RamAccess<string>(null, (offset + k + 1).ToString());
                    cells[k] = new Cell(headers[k], "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth5,
                        Height = RowHeight5,
                        BorderBrush = new SolidColorBrush(border_color5),
                        CellRow = 0,
                        CellColumn = Column
                    };
                    stckPnl2.Children.Add(cells[k]);
                }
                stckPnl1.Children.Add(cell0);
                stckPnl1.Children.Add(stckPnl);
                stckPnl1.Children.Add(stckPnl2);
                return stckPnl1;
            }
        }

        private static Control Get5()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            //stck.Children.Add(Get5Header(1, 1,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("NumberInOrder")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get5Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,0));

            //stck.Children.Add(Get5Header(2, 2,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("StoragePlaceName")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get5Header(1, 3,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("StoragePlaceCode")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get5Header(new double[3] { 2, 1, 3}, 2, new string[3] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("StoragePlaceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("StoragePlaceCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    "Пункт хранения ОЯТ"
                }, true,1));

            //stck.Children.Add(Get5Header(1, 4,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("CodeOYAT")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get5Header(2, 5,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("FcpNumber")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get5Header(1, 6,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("FuelMass")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get5Header(1, 7,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("CellMass")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get5Header(1, 8,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("Quantity")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get5Header(2, 9,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("AlphaActivity")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            //stck.Children.Add(Get5Header(2, 10,
            //    ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("BetaGammaActivity")
            //        .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //));
            stck.Children.Add(Get5Header(new double[8] { 1, 2, 1.2, 3, 1, 3.5,3.5,15.2 }, 3, new string[8] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("CodeOYAT").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("FcpNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("FuelMass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("CellMass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("Quantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("AlphaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("BetaGammaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    "Наличие на конец отчетного года"
                }, true,3));

            return stck;
        }

        #endregion

        #region 2.6

        private static readonly int Wdth6 = 100;
        private static readonly int RowHeight6 = 30;
        private static readonly Color border_color6 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get6Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth6,
                Height = RowHeight6,
                BorderBrush = new SolidColorBrush(border_color6),
                CellRow = 0,
                CellColumn = Column
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth6,
                Height = RowHeight6,
                BorderBrush = new SolidColorBrush(border_color6),
                CellRow = 0,
                CellColumn = Column
            };
            var stckPnl = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            stckPnl.Children.Add(cell);
            stckPnl.Children.Add(cell1);

            return stckPnl;
        }

        private static Control Get6()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            stck.Children.Add(Get6Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form26,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));
            stck.Children.Add(Get6Header(2.1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form26,Models").GetProperty("ObservedSourceNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Children.Add(Get6Header(2, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form26,Models").GetProperty("ControlledAreaName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Children.Add(Get6Header(4, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form26,Models").GetProperty("SupposedWasteSource")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Children.Add(Get6Header(5.5, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form26,Models").GetProperty("DistanceToWasteSource")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Children.Add(Get6Header(2, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form26,Models").GetProperty("TestDepth")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Children.Add(Get6Header(1, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form26,Models").GetProperty("RadionuclidName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Children.Add(Get6Header(3, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form26,Models").GetProperty("AverageYearConcentration")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));

            return stck;
        }

        #endregion

        #region 2.7

        private static readonly int Wdth7 = 100;
        private static readonly int RowHeight7 = 30;
        private static readonly Color border_color7 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get7Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth7,
                Height = RowHeight7,
                BorderBrush = new SolidColorBrush(border_color7),
                CellRow = 0,
                CellColumn = Column
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth7,
                Height = RowHeight7,
                BorderBrush = new SolidColorBrush(border_color7),
                CellRow = 0,
                CellColumn = Column
            };
            var stckPnl = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            stckPnl.Children.Add(cell);
            stckPnl.Children.Add(cell1);

            return stckPnl;
        }

        private static Control Get7()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            stck.Children.Add(Get7Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));
            stck.Children.Add(Get7Header(2, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("ObservedSourceNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Children.Add(Get7Header(2, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("RadionuclidName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Children.Add(Get7Header(4, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("AllowedWasteValue")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Children.Add(Get7Header(4, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("FactedWasteValue")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Children.Add(Get7Header(4.2, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("WasteOutbreakPreviousYear")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));

            return stck;
        }

        #endregion

        #region 2.8

        private static readonly int Wdth8 = 100;
        private static readonly int RowHeight8 = 30;
        private static readonly Color border_color8 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get8Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth8,
                Height = RowHeight8,
                BorderBrush = new SolidColorBrush(border_color8),
                CellRow = 0,
                CellColumn = Column
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth8,
                Height = RowHeight8,
                BorderBrush = new SolidColorBrush(border_color8),
                CellRow = 0,
                CellColumn = Column
            };
            var stckPnl = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            stckPnl.Children.Add(cell);
            stckPnl.Children.Add(cell1);

            return stckPnl;
        }

        private static Control Get8()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            stck.Children.Add(Get8Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form28,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));
            stck.Children.Add(Get8Header(3, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form28,Models").GetProperty("WasteSourceName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Children.Add(Get8Header(3, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form28,Models").GetProperty("WasteRecieverName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Children.Add(Get8Header(2.5, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form28,Models").GetProperty("RecieverTypeCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Children.Add(Get8Header(4, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form28,Models").GetProperty("PoolDistrictName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Children.Add(Get8Header(3.2, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form28,Models").GetProperty("AllowedWasteRemovalVolume")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Children.Add(Get8Header(3, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form28,Models").GetProperty("RemovedWasteVolume")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));

            return stck;
        }

        #endregion

        #region 2.9

        private static readonly int Wdth9 = 100;
        private static readonly int RowHeight9 = 30;
        private static readonly Color border_color9 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get9Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth9,
                Height = RowHeight9,
                BorderBrush = new SolidColorBrush(border_color9),
                CellRow = 0,
                CellColumn = Column
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth9,
                Height = RowHeight9,
                BorderBrush = new SolidColorBrush(border_color9),
                CellRow = 0,
                CellColumn = Column
            };
            var stckPnl = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            stckPnl.Children.Add(cell);
            stckPnl.Children.Add(cell1);

            return stckPnl;
        }

        private static Control Get9()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            stck.Children.Add(Get9Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));
            stck.Children.Add(Get9Header(3, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty("WasteSourceName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Children.Add(Get9Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty("RadionuclidName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Children.Add(Get9Header(3, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty("AllowedActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Children.Add(Get9Header(3, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty("FactedActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));

            return stck;
        }

        #endregion

        #region 2.10

        private static readonly int Wdth10 = 100;
        private static readonly int RowHeight10 = 30;
        private static readonly Color border_color10 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get10Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth10,
                Height = RowHeight10,
                BorderBrush = new SolidColorBrush(border_color10),
                CellRow = 0,
                CellColumn = Column
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth10,
                Height = RowHeight10,
                BorderBrush = new SolidColorBrush(border_color10),
                CellRow = 0,
                CellColumn = Column
            };
            var stckPnl = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            stckPnl.Children.Add(cell);
            stckPnl.Children.Add(cell1);

            return stckPnl;
        }

        private static Control Get10()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            stck.Children.Add(Get10Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));
            stck.Children.Add(Get10Header(2, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("IndicatorName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Children.Add(Get10Header(2, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("PlotName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Children.Add(Get10Header(2, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("PlotKadastrNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Children.Add(Get10Header(1, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("PlotCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Children.Add(Get10Header(3, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("InfectedArea")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Children.Add(Get10Header(3.1, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("AvgGammaRaysDosePower")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Children.Add(Get10Header(4, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("MaxGammaRaysDosePower")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));
            stck.Children.Add(Get10Header(4.7, 9,
                ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("WasteDensityAlpha")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,8
            ));
            stck.Children.Add(Get10Header(4.6, 10,
                ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("WasteDensityBeta")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,9
            ));
            stck.Children.Add(Get10Header(2, 11,
                ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("FcpNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,10
            ));

            return stck;
        }

        #endregion

        #region 2.11

        private static readonly int Wdth11 = 100;
        private static readonly int RowHeight11 = 30;
        private static readonly Color border_color11 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get11Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth11,
                Height = RowHeight11,
                BorderBrush = new SolidColorBrush(border_color11),
                CellRow = 0,
                CellColumn = Column
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth11,
                Height = RowHeight11,
                BorderBrush = new SolidColorBrush(border_color11),
                CellRow = 0,
                CellColumn = Column
            };
            var stckPnl = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            stckPnl.Children.Add(cell);
            stckPnl.Children.Add(cell1);

            return stckPnl;
        }

        private static Control Get11()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            stck.Children.Add(Get11Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form211,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));
            stck.Children.Add(Get11Header(2, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form211,Models").GetProperty("PlotName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Children.Add(Get11Header(2, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form211,Models").GetProperty("PlotKadastrNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Children.Add(Get11Header(1, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form211,Models").GetProperty("PlotCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Children.Add(Get11Header(3, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form211,Models").GetProperty("InfectedArea")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Children.Add(Get11Header(2, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form211,Models").GetProperty("Radionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Children.Add(Get11Header(2, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form211,Models").GetProperty("SpecificActivityOfPlot")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Children.Add(Get11Header(3, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form211,Models")
                    .GetProperty("SpecificActivityOfLiquidPart")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));
            stck.Children.Add(Get11Header(3, 9,
                ((Form_PropertyAttribute) Type.GetType("Models.Form211,Models")
                    .GetProperty("SpecificActivityOfDensePart")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,8
            ));

            return stck;
        }

        #endregion

        #region 2.12

        private static readonly int Wdth12 = 100;
        private static readonly int RowHeight12 = 30;
        private static readonly Color border_color12 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get12Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth12,
                Height = RowHeight12,
                BorderBrush = new SolidColorBrush(border_color12),
                CellRow = 0,
                CellColumn = Column
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth12,
                Height = RowHeight12,
                BorderBrush = new SolidColorBrush(border_color12),
                CellRow = 0,
                CellColumn = Column
            };
            var stckPnl = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            stckPnl.Children.Add(cell);
            stckPnl.Children.Add(cell1);

            return stckPnl;
        }

        private static Control Get12()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            stck.Children.Add(Get12Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));
            stck.Children.Add(Get12Header(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("OperationCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Children.Add(Get12Header(2, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("ObjectTypeCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Children.Add(Get12Header(2, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("Radionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Children.Add(Get12Header(1, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("Activity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Children.Add(Get12Header(2, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("ProviderOrRecieverOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));


            return stck;
        }

        #endregion

        #region Notes

        private static readonly int WdthNotes = 100;
        private static readonly int RowHeightNotes = 30;
        private static readonly Color border_colorNotes = Color.FromArgb(255, 0, 0, 0);

        private static Control GetNotesHeader(double starWidth, int Column, string Text)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * WdthNotes,
                Height = RowHeightNotes,
                BorderBrush = new SolidColorBrush(border_colorNotes),
                CellRow = 0,
                CellColumn = Column
            };


            return cell;
        }

        private static Control GetNotes()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };

            stck.Children.Add(GetNotesHeader(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Note,Models").GetProperty("RowNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(GetNotesHeader(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Note,Models").GetProperty("GraphNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(GetNotesHeader(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Note,Models").GetProperty("Comment")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }

        #endregion
    }
}