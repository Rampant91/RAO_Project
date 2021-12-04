using System;
using System.Linq;
using System.Collections.Generic;
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
        public static List<Row> GetControl(string type)
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
        private static Row Get1Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var rama = new RamAccess<string>(null, "");
                var cella = new Cell(rama, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 0
                };
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 1
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 2
                };

                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 3
                };
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };
                stckPnl.Children.Add(cella);
                stckPnl.Children.Add(cell0);
                stckPnl.Children.Add(cell);
                stckPnl.Children.Add(cell1);
                return stckPnl;
            }
            else
            {
                int len = Text.Length;
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };
                var cella = new Cell(new RamAccess<string>(null, ""), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 0
                };
                stckPnl.Children.Add(cella);
                var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 1
                };
                stckPnl.Children.Add(cell0);
                var stck = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };
                var stck1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };

                for (int k = 0; k < len - 1; k++)
                {
                    stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth1,
                        Height = RowHeight1,
                        BorderBrush = new SolidColorBrush(border_color1),
                    });
                }
                var cell1 = new CustomCell(stck, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 2
                };
                for (int i = 0; i < len - 1; i++)
                {
                    stck1.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[i] * Wdth1,
                        Height = RowHeight1,
                        BorderBrush = new SolidColorBrush(border_color1),
                    });
                }
                var cell2 = new CustomCell(stck1, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 3
                };
                stckPnl.Children.Add(cell1);
                stckPnl.Children.Add(cell2);
                return stckPnl;
            }
        }

        private static Row Get1HeaderTripleLevel(double[] starWidthThirdLevel, double[] starWidth, int Column, string[] TextBottom, string[] Text, int offset)
        {
            int len = Text.Length;
            int len1 = TextBottom.Length;
            var stckPnl = new Row
            {
                Orientation = Orientation.Vertical
            };

            var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 0
            };
            stckPnl.Children.Add(cell0);
            var stck = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck2 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            for (int k = 0; k < len - 1; k++)
            {
                stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[k] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell1 = new CustomCell(stck, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 1
            };
            for (int i = 0; i < len1; i++)
            {
                stck1.Children.Add(new Cell(new RamAccess<string>(null, (TextBottom[i]).ToString()), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthThirdLevel[i] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell2 = new CustomCell(stck1, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 2
            };
            for (int i = 0; i < len1; i++)
            {
                stck2.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthThirdLevel[i] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell3 = new CustomCell(stck2, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 3
            };
            stckPnl.Children.Add(cell1);
            stckPnl.Children.Add(cell2);
            stckPnl.Children.Add(cell3);
            return stckPnl;
        }

        private static List<Row> Get1()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get1Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("NumberInOrder")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 0));

            stck.Add(Get1Header(new double[5] { 2.5, 2, 2, 2, 8.5 }, 2, new string[5] {
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("RefineMachineName")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("MachineCode")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("MachinePower")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form21,Models").GetProperty("NumberOfHoursPerYear")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            "Установки переработки"
            }, true, 1));

            stck.Add(Get1HeaderTripleLevel(new double[10] { 1, 1, 1, 1, 1, 2, 3.5, 3.6, 2, 16.1 }, new double[5] { 1, 1, 3, 11.1, 16.1 }, 3, new string[9]
            {
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
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            }, new string[5] { "", "", "количество", "суммарная активность, Бк", "Поступило РАО на переработку, кондиционирование" }, 5));
            stck.Add(Get1HeaderTripleLevel(new double[10] { 1, 1, 1, 1, 1, 2, 3.5, 3.6, 2, 16.1 }, new double[5] { 1, 1, 3, 11.1, 16.1 }, 4, new string[9]
            {
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
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            }, new string[5] { "", "", "количество", "суммарная активность, Бк", "Образовалось РАО после переработки, кондиционирования" }, 14));
            return stck;
        }

        #endregion

        #region 2.2

        private static readonly int Wdth2 = 100;
        private static readonly int RowHeight2 = 30;
        private static readonly Color border_color2 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get2Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
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
                    CellRow = Column,
                    CellColumn = 0
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth2,
                    Height = RowHeight2,
                    BorderBrush = new SolidColorBrush(border_color2),
                    CellRow = Column,
                    CellColumn = 1
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth2,
                    Height = RowHeight2,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 2
                };
                var stckPnl = new Row
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
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };

                var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth2,
                    Height = RowHeight2,
                    BorderBrush = new SolidColorBrush(border_color2),
                    CellRow = Column,
                    CellColumn = 0
                };
                stckPnl.Children.Add(cell0);
                var stck = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };
                var stck1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };

                for (int k = 0; k < len - 1; k++)
                {
                    stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth2,
                        Height = RowHeight2,
                        BorderBrush = new SolidColorBrush(border_color2),
                    });
                }
                var cell1 = new CustomCell(stck, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth2,
                    Height = RowHeight2,
                    BorderBrush = new SolidColorBrush(border_color2),
                    CellRow = Column,
                    CellColumn = 1
                };
                for (int i = 0; i < len - 1; i++)
                {
                    stck1.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[i] * Wdth2,
                        Height = RowHeight2,
                        BorderBrush = new SolidColorBrush(border_color2),
                    });
                }
                var cell2 = new CustomCell(stck1, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth2,
                    Height = RowHeight2,
                    BorderBrush = new SolidColorBrush(border_color2),
                    CellRow = Column,
                    CellColumn = 2
                };
                stckPnl.Children.Add(cell1);
                stckPnl.Children.Add(cell2);
                return stckPnl;
            }
        }

        private static List<Row> Get2()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get2Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("NumberInOrder")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 0));

            stck.Add(Get2Header(new double[3] { 2, 1, 3 }, 2, new string[3] {
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("StoragePlaceName")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("StoragePlaceCode")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                "Пункт хранения"
            }, true, 1));

            stck.Add(Get2Header(new double[4] { 2, 1, 2, 5 }, 3, new string[4] {
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("PackName")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("PackType")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("PackQuantity")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                "УКТ, упаковка ли иная учетная единица"
            }, true, 3));

            stck.Add(Get2Header(new double[1] { 1 }, 4, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("CodeRAO")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 6));

            stck.Add(Get2Header(new double[1] { 1 }, 5, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("StatusRAO")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 7));

            stck.Add(Get2Header(new double[3] { 2, 2, 4 }, 6, new string[3] {
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("VolumeOutOfPack")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("VolumeInPack")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                "Объем, куб. м"
            }, true, 8));

            stck.Add(Get2Header(new double[3] { 2, 2, 4 }, 7, new string[3] {
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("MassOutOfPack")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("MassInPack")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                "Масса, т"
            }, true, 10));

            stck.Add(Get2Header(new double[1] { 2 }, 8, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("QuantityOZIII")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 12));

            stck.Add(Get2Header(new double[5] { 2, 3.5, 3.6, 2, 11.1 }, 9, new string[5] {
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("TritiumActivity")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("BetaGammaActivity")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("AlphaActivity")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("TransuraniumActivity")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                "Суммарная активность, Бк"
            }, true, 13));

            stck.Add(Get2Header(new double[1] { 1.6 }, 10, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("MainRadionuclids")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 17));

            stck.Add(Get2Header(new double[1] { 1 }, 11, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("Subsidy")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 18));

            stck.Add(Get2Header(new double[1] { 2 }, 12, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form22,Models").GetProperty("FcpNumber")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 19));
            return stck;
        }

        #endregion

        #region 2.3

        private static readonly int Wdth3 = 100;
        private static readonly int RowHeight3 = 30;
        private static readonly Color border_color3 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get3HeaderTripleLevel(double[] starWidthThirdLevel, double[] starWidth, int Column, string[] TextBottom, string[] Text, int offset)
        {
            int len = Text.Length;
            int len1 = TextBottom.Length;
            var stckPnl = new Row
            {
                Orientation = Orientation.Vertical
            };

            var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 0
            };
            stckPnl.Children.Add(cell0);
            var stck = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck2 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            for (int k = 0; k < len - 1; k++)
            {
                stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[k] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell1 = new CustomCell(stck, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 1
            };
            for (int i = 0; i < len1; i++)
            {
                stck1.Children.Add(new Cell(new RamAccess<string>(null, (TextBottom[i]).ToString()), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthThirdLevel[i] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell2 = new CustomCell(stck1, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 2
            };
            for (int i = 0; i < len1; i++)
            {
                stck2.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthThirdLevel[i] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell3 = new CustomCell(stck2, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 3
            };
            stckPnl.Children.Add(cell1);
            stckPnl.Children.Add(cell2);
            stckPnl.Children.Add(cell3);
            return stckPnl;
        }

        private static Row Get3Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var cella = new Cell(new RamAccess<string>(null, ""), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 0
                };
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 1
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 2
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 3
                };
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };

                stckPnl.Children.Add(cella);
                stckPnl.Children.Add(cell0);
                stckPnl.Children.Add(cell);
                stckPnl.Children.Add(cell1);
                return stckPnl;
            }
            else
            {
                int len = Text.Length;
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };
                var cella = new Cell(new RamAccess<string>(null, ""), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 0
                };
                stckPnl.Children.Add(cella);
                var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 1
                };
                stckPnl.Children.Add(cell0);
                var stck = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };
                var stck1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };

                for (int k = 0; k < len - 1; k++)
                {
                    stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth3,
                        Height = RowHeight3,
                        BorderBrush = new SolidColorBrush(border_color3),
                    });
                }
                var cell1 = new CustomCell(stck, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 2
                };
                for (int i = 0; i < len - 1; i++)
                {
                    stck1.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[i] * Wdth3,
                        Height = RowHeight3,
                        BorderBrush = new SolidColorBrush(border_color3),
                    });
                }
                var cell2 = new CustomCell(stck1, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 3
                };
                stckPnl.Children.Add(cell1);
                stckPnl.Children.Add(cell2);
                return stckPnl;
            }
        }

        private static List<Row> Get3()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get3Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form23,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 0));

            stck.Add(Get3Header(new double[4] { 2, 1, 2, 5 }, 2, new string[4] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("StoragePlaceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("StoragePlaceCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("ProjectVolume").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    "Пункт хранения РАО"
                }, true, 1));

            stck.Add(Get3HeaderTripleLevel(new double[6] { 1, 2, 2, 2, 2, 9 }, new double[5] { 1, 4, 2, 2, 9 }, 3, new string[5] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("CodeRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("Volume").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("Mass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("QuantityOZIII").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("SummaryActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                }, new string[5] { "", "количество РАО", "", "", "Разрешено к размещению" }, 4));

            stck.Add(Get3Header(new double[5] { 1.2, 1, 2, 2, 6.2 }, 4, new string[5] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("ExpirationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").GetProperty("DocumentName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    "Наименование и реквизиты документа на размещение РАО"
                }, true, 9));
            return stck;
        }

        #endregion

        #region 2.4

        private static readonly int Wdth4 = 100;
        private static readonly int RowHeight4 = 30;
        private static readonly Color border_color4 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get4HeaderFourthLevel(double[] starWidthFourthLevel, double[] starWidthThirdLevel, double[] starWidth, int Column, string[] TextFourthLevel, string[] TextThirdLevel, string[] Text, int offset)
        {
            int len = Text.Length;
            int len1 = TextFourthLevel.Length;
            int len2 = TextThirdLevel.Length;
            var stckPnl = new Row
            {
                Orientation = Orientation.Vertical
            };

            var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 0
            };
            stckPnl.Children.Add(cell0);
            var stck = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck2 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck3 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            for (int k = 0; k < len - 1; k++)
            {
                stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[k] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell1 = new CustomCell(stck, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 1
            };
            for (int i = 0; i < len2; i++)
            {
                stck1.Children.Add(new Cell(new RamAccess<string>(null, TextThirdLevel[i]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthThirdLevel[i] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell2 = new CustomCell(stck1, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 2
            };
            for (int i = 0; i < len1; i++)
            {
                stck2.Children.Add(new Cell(new RamAccess<string>(null, TextFourthLevel[i]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthFourthLevel[i] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell3 = new CustomCell(stck2, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 3
            };
            for (int i = 0; i < len1; i++)
            {
                stck3.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthFourthLevel[i] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell4 = new CustomCell(stck3, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 4
            };
            stckPnl.Children.Add(cell1);
            stckPnl.Children.Add(cell2);
            stckPnl.Children.Add(cell3);
            stckPnl.Children.Add(cell4);
            return stckPnl;
        }

        private static Row Get4Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var cellb = new Cell(new RamAccess<string>(null, ""), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 0
                };
                var cella = new Cell(new RamAccess<string>(null, ""), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 1
                };
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 2
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 3
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 4
                };
                                                                                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };
                stckPnl.Children.Add(cellb);
                stckPnl.Children.Add(cella);
                stckPnl.Children.Add(cell0);
                stckPnl.Children.Add(cell);
                stckPnl.Children.Add(cell1);
                return stckPnl;
            }
            else
            {
                int len = Text.Length;
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };
                var cellb = new Cell(new RamAccess<string>(null, ""), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len-1] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 0
                }; stckPnl.Children.Add(cellb);
                var cella = new Cell(new RamAccess<string>(null, ""), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 1
                };
                stckPnl.Children.Add(cella);
                var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 2
                };
                stckPnl.Children.Add(cell0);
                var stck = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };
                var stck1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };

                for (int k = 0; k < len - 1; k++)
                {
                    stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth3,
                        Height = RowHeight3,
                        BorderBrush = new SolidColorBrush(border_color3),
                    });
                }
                var cell1 = new CustomCell(stck, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 3
                };
                for (int i = 0; i < len - 1; i++)
                {
                    stck1.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[i] * Wdth3,
                        Height = RowHeight3,
                        BorderBrush = new SolidColorBrush(border_color3),
                    });
                }
                var cell2 = new CustomCell(stck1, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 4
                };
                stckPnl.Children.Add(cell1);
                stckPnl.Children.Add(cell2);
                return stckPnl;
            }
        }
        private static Row Get4HeaderTripleLevel(double[] starWidthThirdLevel, double[] starWidth, int Column, string[] TextBottom, string[] Text, int offset)
        {
            int len = Text.Length;
            int len1 = TextBottom.Length;
            var stckPnl = new Row
            {
                Orientation = Orientation.Vertical
            };

            var cella = new Cell(new RamAccess<string>(null, ""), "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 0
            };
            stckPnl.Children.Add(cella);
            var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 1
            };
            stckPnl.Children.Add(cell0);
            var stck = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck2 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            for (int k = 0; k < len - 1; k++)
            {
                stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[k] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell1 = new CustomCell(stck, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 2
            };
            for (int i = 0; i < len1; i++)
            {
                stck1.Children.Add(new Cell(new RamAccess<string>(null, TextBottom[i]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthThirdLevel[i] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell2 = new CustomCell(stck1, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 3
            };
            for (int i = 0; i < len1; i++)
            {
                stck2.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthThirdLevel[i] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell3 = new CustomCell(stck2, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 4
            };
            stckPnl.Children.Add(cell1);
            stckPnl.Children.Add(cell2);
            stckPnl.Children.Add(cell3);
            return stckPnl;
        }
        private static List<Row> Get4()
        {
            List<Row> stck = new List<Row>();

                                                            stck.Add(Get4Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("NumberInOrder")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,0));

                                                            stck.Add(Get4Header(new double[1] { 1 }, 2, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("CodeOYAT")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,1));

                                                            stck.Add(Get4Header(new double[1] { 2 }, 3, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form24,Models").GetProperty("FcpNumber")
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false,2));

                                                                                                                                                                                                                                                                                                                                                                                                                        stck.Add(Get4HeaderFourthLevel(new double[9] { 1,1,1.5,1.5,1.5,1.5,1.75,1.75,11.5 },new double[5] { 2,3,3,3.5,11.5},new double[4] {2,6,3.5,11.5 }, 4, new string[8] {
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
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name},
                new string[4] {"образовалось","всего","в том числе по импорту","поставлено на учет по другим причинам" },new string[4] { "", "поступило от сторонних организаций", "", "Поставлено на учет в организации" },3));

                                                                                                                                                                                                                                                                                                                        stck.Add(Get4HeaderTripleLevel(new double[7] { 1, 1.5, 1, 1, 1, 1.33, 6.83 },new double[4] { 2.5,2,2.33,6.83}, 5, new string[6] {
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
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name},
                new string[4] { "передано сторонним организациям","переработано","снято с учета по другим причинам","Снято с учета в организации"},11));
            return stck;
        }

        #endregion

        #region 2.5

        private static readonly int Wdth5 = 100;
        private static readonly int RowHeight5 = 30;
        private static readonly Color border_color5 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get5HeaderTripleLevel(double[] starWidthThirdLevel, double[] starWidth, int Column, string[] TextBottom, string[] Text, int offset)
        {
            int len = Text.Length;
            int len1 = TextBottom.Length;
            var stckPnl = new Row
            {
                Orientation = Orientation.Vertical
            };

            var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 0
            };
            stckPnl.Children.Add(cell0);
            var stck = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck2 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            for (int k = 0; k < len - 1; k++)
            {
                stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[k] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell1 = new CustomCell(stck, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 1
            };
            for (int i = 0; i < len1; i++)
            {
                stck1.Children.Add(new Cell(new RamAccess<string>(null, TextBottom[i]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthThirdLevel[i] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell2 = new CustomCell(stck1, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 2
            };
            for (int i = 0; i < len1; i++)
            {
                stck2.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthThirdLevel[i] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                });
            }
            var cell3 = new CustomCell(stck2, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = Column,
                CellColumn = 3
            };
            stckPnl.Children.Add(cell1);
            stckPnl.Children.Add(cell2);
            stckPnl.Children.Add(cell3);
            return stckPnl;
        }

        private static Row Get5Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var cella = new Cell(new RamAccess<string>(null, ""), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 0
                };
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 1
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 2
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 3
                };
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };

                stckPnl.Children.Add(cella);
                stckPnl.Children.Add(cell0);
                stckPnl.Children.Add(cell);
                stckPnl.Children.Add(cell1);
                return stckPnl;
            }
            else
            {
                int len = Text.Length;
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };
                var cella = new Cell(new RamAccess<string>(null, ""), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth1,
                    Height = RowHeight1,
                    BorderBrush = new SolidColorBrush(border_color1),
                    CellRow = Column,
                    CellColumn = 0
                };
                stckPnl.Children.Add(cella);
                var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 1
                };
                stckPnl.Children.Add(cell0);
                var stck = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };
                var stck1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };

                for (int k = 0; k < len - 1; k++)
                {
                    stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth3,
                        Height = RowHeight3,
                        BorderBrush = new SolidColorBrush(border_color3),
                    });
                }
                var cell1 = new CustomCell(stck, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 2
                };
                for (int i = 0; i < len - 1; i++)
                {
                    stck1.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[i] * Wdth3,
                        Height = RowHeight3,
                        BorderBrush = new SolidColorBrush(border_color3),
                    });
                }
                var cell2 = new CustomCell(stck1, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth3,
                    Height = RowHeight3,
                    BorderBrush = new SolidColorBrush(border_color3),
                    CellRow = Column,
                    CellColumn = 3
                };
                stckPnl.Children.Add(cell1);
                stckPnl.Children.Add(cell2);
                return stckPnl;
            }
        }

        private static List<Row> Get5()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get5Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form25,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 0));

            stck.Add(Get5Header(new double[3] { 2, 1, 3 }, 2, new string[3] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("StoragePlaceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("StoragePlaceCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    "Пункт хранения ОЯТ"
                }, true, 1));

            stck.Add(Get5HeaderTripleLevel(new double[8] { 1, 2, 1.2, 3, 1, 3.5, 3.5, 15.2 }, new double[6] { 1, 2, 4.2, 1, 7, 15.2 }, 3, new string[7] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("CodeOYAT").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("FcpNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("FuelMass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("CellMass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("Quantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("AlphaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").GetProperty("BetaGammaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                }, new string[6] { "", "", "масса ОЯТ, т", "", "суммарная активность, Бк", "Наличие на конец отчетного года" }, 3));

            return stck;
        }

        #endregion

        #region 2.6

        private static readonly int Wdth6 = 100;
        private static readonly int RowHeight6 = 30;
        private static readonly Color border_color6 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get6Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth6,
                Height = RowHeight6,
                BorderBrush = new SolidColorBrush(border_color6),
                CellRow = Column,
                CellColumn = 0
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth6,
                Height = RowHeight6,
                BorderBrush = new SolidColorBrush(border_color6),
                CellRow = Column,
                CellColumn = 1
            };
            var stckPnl = new Row
            {
                Orientation = Orientation.Vertical
            };
            stckPnl.Children.Add(cell);
            stckPnl.Children.Add(cell1);

            return stckPnl;
        }

        private static List<Row> Get6()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get6Header(1, 1,
                ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 0
            ));
            stck.Add(Get6Header(2.1, 2,
                ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").GetProperty("ObservedSourceNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 1
            ));
            stck.Add(Get6Header(2, 3,
                ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").GetProperty("ControlledAreaName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 2
            ));
            stck.Add(Get6Header(4, 4,
                ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").GetProperty("SupposedWasteSource")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 3
            ));
            stck.Add(Get6Header(5.5, 5,
                ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").GetProperty("DistanceToWasteSource")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 4
            ));
            stck.Add(Get6Header(2, 6,
                ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").GetProperty("TestDepth")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 5
            ));
            stck.Add(Get6Header(2, 7,
                ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").GetProperty("RadionuclidName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 6
            ));
            stck.Add(Get6Header(3, 8,
                ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").GetProperty("AverageYearConcentration")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 7
            ));

            return stck;
        }

        #endregion

        #region 2.7

        private static readonly int Wdth7 = 100;
        private static readonly int RowHeight7 = 30;
        private static readonly Color border_color7 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get7Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth7,
                    Height = RowHeight7,
                    BorderBrush = new SolidColorBrush(border_color7),
                    CellRow = Column,
                    CellColumn = 0
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth7,
                    Height = RowHeight7,
                    BorderBrush = new SolidColorBrush(border_color7),
                    CellRow = Column,
                    CellColumn = 1
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth7,
                    Height = RowHeight7,
                    BorderBrush = new SolidColorBrush(border_color7),
                    CellRow = Column,
                    CellColumn = 2
                };
                var stckPnl = new Row
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
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };

                var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth7,
                    Height = RowHeight7,
                    BorderBrush = new SolidColorBrush(border_color7),
                    CellRow = Column,
                    CellColumn = 0
                };
                stckPnl.Children.Add(cell0);
                var stck = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };
                var stck1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };

                for (int k = 0; k < len - 1; k++)
                {
                    stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth7,
                        Height = RowHeight7,
                        BorderBrush = new SolidColorBrush(border_color7),
                    });
                }
                var cell1 = new CustomCell(stck, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth7,
                    Height = RowHeight7,
                    BorderBrush = new SolidColorBrush(border_color7),
                    CellRow = Column,
                    CellColumn = 1
                };
                for (int i = 0; i < len - 1; i++)
                {
                    stck1.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[i] * Wdth7,
                        Height = RowHeight7,
                        BorderBrush = new SolidColorBrush(border_color7),
                    });
                }
                var cell2 = new CustomCell(stck1, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth7,
                    Height = RowHeight7,
                    BorderBrush = new SolidColorBrush(border_color7),
                    CellRow = Column,
                    CellColumn = 2
                };
                stckPnl.Children.Add(cell1);
                stckPnl.Children.Add(cell2);
                return stckPnl;
            }
        }

        private static List<Row> Get7()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get7Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 0));
            stck.Add(Get7Header(new double[1] { 3 }, 2, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("ObservedSourceNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 1));
            stck.Add(Get7Header(new double[1] { 2 }, 3, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("RadionuclidName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 2));
            stck.Add(Get7Header(new double[3] { 1.7, 1.7, 3.4 }, 4, new string[3] {
            ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("AllowedWasteValue")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("FactedWasteValue")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            "Выброс радионуклида в атмосферу за отчетный год, Бк"}, true, 3));
            stck.Add(Get7Header(new double[2] { 3.6, 3.6 }, 5, new string[2] {
            ((Form_PropertyAttribute) Type.GetType("Models.Form27,Models").GetProperty("WasteOutbreakPreviousYear")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            "Выброс радионуклида в атмосферу за предыдущий год, Бк"}, true, 5));

            return stck;
        }

        #endregion

        #region 2.8

        private static readonly int Wdth8 = 100;
        private static readonly int RowHeight8 = 30;
        private static readonly Color border_color8 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get8Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth8,
                    Height = RowHeight8,
                    BorderBrush = new SolidColorBrush(border_color8),
                    CellRow = Column,
                    CellColumn = 0
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth8,
                    Height = RowHeight8,
                    BorderBrush = new SolidColorBrush(border_color8),
                    CellRow = Column,
                    CellColumn = 1
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth8,
                    Height = RowHeight8,
                    BorderBrush = new SolidColorBrush(border_color8),
                    CellRow = Column,
                    CellColumn = 2
                };
                var stckPnl = new Row
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
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };

                var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth8,
                    Height = RowHeight8,
                    BorderBrush = new SolidColorBrush(border_color8),
                    CellRow = Column,
                    CellColumn = 0
                };
                stckPnl.Children.Add(cell0);
                var stck = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };
                var stck1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };

                for (int k = 0; k < len - 1; k++)
                {
                    stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth8,
                        Height = RowHeight8,
                        BorderBrush = new SolidColorBrush(border_color8),
                    });
                }
                var cell1 = new CustomCell(stck, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth8,
                    Height = RowHeight8,
                    BorderBrush = new SolidColorBrush(border_color8),
                    CellRow = Column,
                    CellColumn = 1
                };
                for (int i = 0; i < len - 1; i++)
                {
                    stck1.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[i] * Wdth8,
                        Height = RowHeight8,
                        BorderBrush = new SolidColorBrush(border_color8),
                    });
                }
                var cell2 = new CustomCell(stck1, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth8,
                    Height = RowHeight8,
                    BorderBrush = new SolidColorBrush(border_color8),
                    CellRow = Column,
                    CellColumn = 2
                };
                stckPnl.Children.Add(cell1);
                stckPnl.Children.Add(cell2);
                return stckPnl;
            }
        }

        private static List<Row> Get8()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get8Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form28,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 0));
            stck.Add(Get8Header(new double[1] { 3 }, 2, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form28,Models").GetProperty("WasteSourceName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 1));
            stck.Add(Get8Header(new double[4] { 3, 2.5, 4, 9.5 }, 3, new string[4] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form28,Models").GetProperty("WasteRecieverName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form28,Models").GetProperty("RecieverTypeCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form28,Models").GetProperty("PoolDistrictName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    "Приемник отведенных вод"
                }, true, 2));
            stck.Add(Get8Header(new double[1] { 3.2 }, 4, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form28,Models").GetProperty("AllowedWasteRemovalVolume")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 5));
            stck.Add(Get8Header(new double[1] { 3 }, 5, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form28,Models").GetProperty("RemovedWasteVolume")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 6));

            return stck;
        }

        #endregion

        #region 2.9

        private static readonly int Wdth9 = 100;
        private static readonly int RowHeight9 = 30;
        private static readonly Color border_color9 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get9Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth9,
                    Height = RowHeight9,
                    BorderBrush = new SolidColorBrush(border_color9),
                    CellRow = Column,
                    CellColumn = 0
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth9,
                    Height = RowHeight9,
                    BorderBrush = new SolidColorBrush(border_color9),
                    CellRow = Column,
                    CellColumn = 1
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth9,
                    Height = RowHeight9,
                    BorderBrush = new SolidColorBrush(border_color9),
                    CellRow = Column,
                    CellColumn = 2
                };
                var stckPnl = new Row
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
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };

                var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth9,
                    Height = RowHeight9,
                    BorderBrush = new SolidColorBrush(border_color9),
                    CellRow = Column,
                    CellColumn = 0
                };
                stckPnl.Children.Add(cell0);
                var stck = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };
                var stck1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };

                for (int k = 0; k < len - 1; k++)
                {
                    stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth9,
                        Height = RowHeight9,
                        BorderBrush = new SolidColorBrush(border_color9),
                    });
                }
                var cell1 = new CustomCell(stck, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth9,
                    Height = RowHeight9,
                    BorderBrush = new SolidColorBrush(border_color9),
                    CellRow = Column,
                    CellColumn = 1
                };
                for (int i = 0; i < len - 1; i++)
                {
                    stck1.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[i] * Wdth9,
                        Height = RowHeight9,
                        BorderBrush = new SolidColorBrush(border_color9),
                    });
                }
                var cell2 = new CustomCell(stck1, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth9,
                    Height = RowHeight9,
                    BorderBrush = new SolidColorBrush(border_color9),
                    CellRow = Column,
                    CellColumn = 2
                };
                stckPnl.Children.Add(cell1);
                stckPnl.Children.Add(cell2);
                return stckPnl;
            }
        }

        private static List<Row> Get9()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get9Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 0));
            stck.Add(Get9Header(new double[1] { 3 }, 2, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty("WasteSourceName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 1));
            stck.Add(Get9Header(new double[1] { 2 }, 3, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty("RadionuclidName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 2));
            stck.Add(Get9Header(new double[3] { 1, 1, 2 }, 4, new string[3] {
            ((Form_PropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty("AllowedActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form29,Models").GetProperty("FactedActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            "Активность радионуклида, Бк"}, true, 3));

            return stck;
        }

        #endregion

        #region 2.10

        private static readonly int Wdth10 = 100;
        private static readonly int RowHeight10 = 30;
        private static readonly Color border_color10 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get10Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth10,
                    Height = RowHeight10,
                    BorderBrush = new SolidColorBrush(border_color10),
                    CellRow = Column,
                    CellColumn = 0
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth10,
                    Height = RowHeight10,
                    BorderBrush = new SolidColorBrush(border_color10),
                    CellRow = Column,
                    CellColumn = 1
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth10,
                    Height = RowHeight10,
                    BorderBrush = new SolidColorBrush(border_color10),
                    CellRow = Column,
                    CellColumn = 2
                };
                var stckPnl = new Row
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
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };

                var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth10,
                    Height = RowHeight10,
                    BorderBrush = new SolidColorBrush(border_color10),
                    CellRow = Column,
                    CellColumn = 0
                };
                stckPnl.Children.Add(cell0);
                var stck = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };
                var stck1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };

                for (int k = 0; k < len - 1; k++)
                {
                    stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth10,
                        Height = RowHeight10,
                        BorderBrush = new SolidColorBrush(border_color10),
                    });
                }
                var cell1 = new CustomCell(stck, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth10,
                    Height = RowHeight10,
                    BorderBrush = new SolidColorBrush(border_color10),
                    CellRow = Column,
                    CellColumn = 1
                };
                for (int i = 0; i < len - 1; i++)
                {
                    stck1.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[i] * Wdth10,
                        Height = RowHeight10,
                        BorderBrush = new SolidColorBrush(border_color10)
                    });
                }
                var cell2 = new CustomCell(stck1, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth10,
                    Height = RowHeight10,
                    BorderBrush = new SolidColorBrush(border_color10),
                    CellRow = Column,
                    CellColumn = 2
                };
                stckPnl.Children.Add(cell1);
                stckPnl.Children.Add(cell2);
                return stckPnl;
            }
        }

        private static List<Row> Get10()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get10Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 0));
            stck.Add(Get10Header(new double[1] { 2 }, 2, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("IndicatorName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 1));
            stck.Add(Get10Header(new double[1] { 2 }, 3, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("PlotName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 2));
            stck.Add(Get10Header(new double[1] { 2 }, 4, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("PlotKadastrNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 3));
            stck.Add(Get10Header(new double[1] { 1 }, 5, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("PlotCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 4));
            stck.Add(Get10Header(new double[1] { 3 }, 6, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("InfectedArea")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 5));
            stck.Add(Get10Header(new double[3] { 1.2, 1.6, 2.8 }, 7, new string[3] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").GetProperty("AvgGammaRaysDosePower").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").GetProperty("MaxGammaRaysDosePower").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    "Мощность дозы гамма-излучения, мкЗв/час"
                }, true, 6));
            stck.Add(Get10Header(new double[3] { 2.4, 2.4, 4.8 }, 8, new string[3] {
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").GetProperty("WasteDensityAlpha").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").GetProperty("WasteDensityBeta").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                    "Плотность загрязнения (средняя), Бк/кв.м"
                }, true, 8));
            stck.Add(Get10Header(new double[1] { 2 }, 9, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form210,Models").GetProperty("FcpNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 10));
            return stck;
        }

        #endregion

        #region 2.11

        private static readonly int Wdth11 = 100;
        private static readonly int RowHeight11 = 30;
        private static readonly Color border_color11 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get11HeaderTripleLevel(double[] starWidthThirdLevel, double[] starWidth, int Column, string[] TextBottom, string[] Text, int offset)
        {
            int len = Text.Length;
            int len1 = TextBottom.Length;
            var stckPnl = new Row
            {
                Orientation = Orientation.Vertical
            };

            var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth7,
                Height = RowHeight7,
                BorderBrush = new SolidColorBrush(border_color7),
                CellRow = Column,
                CellColumn = 0
            };
            stckPnl.Children.Add(cell0);
            var stck = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck1 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            var stck2 = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 0
            };
            for (int k = 0; k < len - 1; k++)
            {
                stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[k] * Wdth7,
                    Height = RowHeight7,
                    BorderBrush = new SolidColorBrush(border_color7),
                });
            }
            var cell1 = new CustomCell(stck, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth7,
                Height = RowHeight7,
                BorderBrush = new SolidColorBrush(border_color7),
                CellRow = Column,
                CellColumn = 1
            };
            for (int i = 0; i < len1; i++)
            {
                stck1.Children.Add(new Cell(new RamAccess<string>(null, (TextBottom[i]).ToString()), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthThirdLevel[i] * Wdth7,
                    Height = RowHeight7,
                    BorderBrush = new SolidColorBrush(border_color7),
                });
            }
            var cell2 = new CustomCell(stck1, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth7,
                Height = RowHeight7,
                BorderBrush = new SolidColorBrush(border_color7),
                CellRow = Column,
                CellColumn = 2
            };
            for (int i = 0; i < len1; i++)
            {
                stck2.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidthThirdLevel[i] * Wdth7,
                    Height = RowHeight7,
                    BorderBrush = new SolidColorBrush(border_color7),
                });
            }
            var cell3 = new CustomCell(stck2, null, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth[len - 1] * Wdth7,
                Height = RowHeight7,
                BorderBrush = new SolidColorBrush(border_color7),
                CellRow = Column,
                CellColumn = 3
            };
            stckPnl.Children.Add(cell1);
            stckPnl.Children.Add(cell2);
            stckPnl.Children.Add(cell3);
            return stckPnl;
        }
        private static Row Get11Header(double starWidth, int Column, string Text, int offset)
        {
            var ramb = new RamAccess<string>(null, "");
            var cellb = new Cell(ramb, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth11,
                Height = RowHeight11,
                BorderBrush = new SolidColorBrush(border_color11),
                CellRow = Column,
                CellColumn = 0
            };
            var rama = new RamAccess<string>(null, "");
            var cella = new Cell(rama, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth11,
                Height = RowHeight11,
                BorderBrush = new SolidColorBrush(border_color11),
                CellRow = Column,
                CellColumn = 1
            };
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth11,
                Height = RowHeight11,
                BorderBrush = new SolidColorBrush(border_color11),
                CellRow = Column,
                CellColumn = 2
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth11,
                Height = RowHeight11,
                BorderBrush = new SolidColorBrush(border_color11),
                CellRow = Column,
                CellColumn = 3
            };
            var stckPnl = new Row
            {
                Orientation = Orientation.Vertical
            };
            stckPnl.Children.Add(cellb);
            stckPnl.Children.Add(cella);
            stckPnl.Children.Add(cell);
            stckPnl.Children.Add(cell1);

            return stckPnl;
        }

        private static List<Row> Get11()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get11Header(1, 1,
                ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 0
            ));
            stck.Add(Get11Header(2, 2,
                ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").GetProperty("PlotName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 1
            ));
            stck.Add(Get11Header(2, 3,
                ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").GetProperty("PlotKadastrNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 2
            ));
            stck.Add(Get11Header(1, 4,
                ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").GetProperty("PlotCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 3
            ));
            stck.Add(Get11Header(3, 5,
                ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").GetProperty("InfectedArea")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 4
            ));
            stck.Add(Get11Header(2, 6,
                ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").GetProperty("Radionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name, 5
            ));
            stck.Add(Get11HeaderTripleLevel(new double[4] { 1.5, 1, 1.5, 4 }, new double[3] { 1.5, 2.5, 4 }, 7, new string[3] { "земельный участок", "жидкая фаза", "донные отложения" },
            new string[3] { "", "водный объект", "Удельная активность, Бк/г" }, 6));
            return stck;
        }

        #endregion

        #region 2.12

        private static readonly int Wdth12 = 100;
        private static readonly int RowHeight12 = 30;
        private static readonly Color border_color12 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get12Header(double[] starWidth, int Column, string[] Text, bool OneToMany, int offset)
        {
            if (!OneToMany)
            {
                var ram0 = new RamAccess<string>(null, "");
                var cell0 = new Cell(ram0, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth12,
                    Height = RowHeight12,
                    BorderBrush = new SolidColorBrush(border_color12),
                    CellRow = Column,
                    CellColumn = 0
                };

                var ram = new RamAccess<string>(null, Text[0]);
                var cell = new Cell(ram, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth12,
                    Height = RowHeight12,
                    BorderBrush = new SolidColorBrush(border_color12),
                    CellRow = Column,
                    CellColumn = 1
                };
                var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
                var cell1 = new Cell(ram1, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[0] * Wdth12,
                    Height = RowHeight12,
                    BorderBrush = new SolidColorBrush(border_color12),
                    CellRow = Column,
                    CellColumn = 2
                };
                var stckPnl = new Row
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
                var stckPnl = new Row
                {
                    Orientation = Orientation.Vertical
                };

                var cell0 = new Cell(new RamAccess<string>(null, Text[len - 1]), "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth12,
                    Height = RowHeight12,
                    BorderBrush = new SolidColorBrush(border_color12),
                    CellRow = Column,
                    CellColumn = 0
                };
                stckPnl.Children.Add(cell0);
                var stck = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };
                var stck1 = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 0
                };

                for (int k = 0; k < len - 1; k++)
                {
                    stck.Children.Add(new Cell(new RamAccess<string>(null, Text[k]), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[k] * Wdth12,
                        Height = RowHeight12,
                        BorderBrush = new SolidColorBrush(border_color12),
                    });
                }
                var cell1 = new CustomCell(stck, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth12,
                    Height = RowHeight12,
                    BorderBrush = new SolidColorBrush(border_color12),
                    CellRow = Column,
                    CellColumn = 1
                };
                for (int i = 0; i < len - 1; i++)
                {
                    stck1.Children.Add(new Cell(new RamAccess<string>(null, (offset + i + 1).ToString()), "", true)
                    {
                        Background = new SolidColorBrush(Color.Parse("LightGray")),
                        Width = starWidth[i] * Wdth12,
                        Height = RowHeight12,
                        BorderBrush = new SolidColorBrush(border_color12),
                    });
                }
                var cell2 = new CustomCell(stck1, null, "", true)
                {
                    Background = new SolidColorBrush(Color.Parse("LightGray")),
                    Width = starWidth[len - 1] * Wdth12,
                    Height = RowHeight12,
                    BorderBrush = new SolidColorBrush(border_color12),
                    CellRow = Column,
                    CellColumn = 2
                };
                stckPnl.Children.Add(cell1);
                stckPnl.Children.Add(cell2);
                return stckPnl;
            }
        }

        private static List<Row> Get12()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get12Header(new double[1] { 1 }, 1, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 0));
            stck.Add(Get12Header(new double[1] { 1 }, 2, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("OperationCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 1));
            stck.Add(Get12Header(new double[1] { 2 }, 3, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("ObjectTypeCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 2));
            stck.Add(Get12Header(new double[3] { 1.1, 1, 2.1 }, 4, new string[3] {
            ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("Radionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("Activity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
            "Сведения о радионуклидных источниках"}, true, 3));
            stck.Add(Get12Header(new double[1] { 2 }, 5, new string[1] { ((Form_PropertyAttribute) Type.GetType("Models.Form212,Models").GetProperty("ProviderOrRecieverOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name }, false, 5));


            return stck;
        }

        #endregion

        #region Notes

        private static readonly int WdthNotes = 100;
        private static readonly int RowHeightNotes = 30;
        private static readonly Color border_colorNotes = Color.FromArgb(255, 0, 0, 0);

        private static Row GetNotesHeader(double starWidth, int Column, string Text)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * WdthNotes,
                Height = RowHeightNotes,
                BorderBrush = new SolidColorBrush(border_colorNotes),
                CellRow = Column,
                CellColumn = 0
            };

            var stckPnl = new Row
            {
                Orientation = Orientation.Vertical
            };
            stckPnl.Children.Add(cell);

            return stckPnl;
        }

        private static List<Row> GetNotes()
        {
            List<Row> stck = new List<Row>();

            stck.Add(GetNotesHeader(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Note,Models").GetProperty("RowNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(GetNotesHeader(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Note,Models").GetProperty("GraphNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(GetNotesHeader(6, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Note,Models").GetProperty("Comment")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }

        #endregion
    }
}