using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Client_App.Controls.DataGrid;
using Models.Attributes;
using Models.DataAccess;
using System.Collections.Generic;

namespace Client_App.Controls.Support.RenderDataGridHeader
{
    public class Main
    {
        public static List<Row> GetControl(string type)
        {
            switch (type)
            {
                case "0": return Get0();
                case "1": return Get1();
                case "2": return Get2();
                case "3": return Get3();
            }

            return null;
        }

        #region 0
        private static readonly int Wdth0 = 100;
        private static readonly int RowHeight0 = 30;
        private static readonly Color border_color0 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get0Header(double starWidth, int Column, string Text)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth0,
                Height = RowHeight0,
                BorderBrush = new SolidColorBrush(border_color0),
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

        private static List<Row> Get0()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get0Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form10,Models").GetProperty("RegNo")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            stck.Add(Get0Header(2.5, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form10,Models").GetProperty("ShortJurLico")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(Get0Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form10,Models").GetProperty("Okpo")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }
        #endregion

        #region 1
        private static readonly int Wdth1 = 100;
        private static readonly int RowHeight1 = 30;
        private static readonly Color border_color1 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get1Header(double starWidth, int Column, string Text)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
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

        private static List<Row> Get1()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get1Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Abstracts.Form1,Models").GetProperty("FormNum")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(Get1Header(1.6, 2,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("StartPeriod")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(Get1Header(1.6, 3,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("EndPeriod")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(Get1Header(1, 4,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("ExportDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(Get1Header(2, 5,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("IsCorrection")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(Get1Header(1, 6,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("Comments")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }
        #endregion

        #region 2
        private static readonly int Wdth2 = 100;
        private static readonly int RowHeight2 = 30;
        private static readonly Color border_color2 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get2Header(double starWidth, int Column, string Text)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth2,
                Height = RowHeight2,
                BorderBrush = new SolidColorBrush(border_color2),
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

        private static List<Row> Get2()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get2Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form20,Models").GetProperty("RegNo")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            stck.Add(Get2Header(2.5, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form20,Models").GetProperty("ShortJurLico")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(Get2Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form20,Models").GetProperty("Okpo")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }
        #endregion

        #region 3
        private static readonly int Wdth3 = 100;
        private static readonly int RowHeight3 = 30;
        private static readonly Color border_color3 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get3Header(int starWidth, int Column, string Text)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth3,
                Height = RowHeight3,
                BorderBrush = new SolidColorBrush(border_color3),
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
        private static List<Row> Get3()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get3Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Abstracts.Form2,Models").GetProperty("FormNum")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(Get3Header(2, 2,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("Year")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(Get3Header(1, 4,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("ExportDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(Get3Header(2, 5,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("IsCorrection")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Add(Get1Header(1, 6,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("Comments")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }
        #endregion
    }
}