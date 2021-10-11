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
    public class Form1
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
                case "1*": return GetNotes();
            }

            return null;
        }

        #region 1.1

        private static readonly int Wdth1 = 100;
        private static readonly int RowHeight1 = 30;
        private static readonly Color border_color1 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get1Header(double starWidth, int Column, string Text, int offset)
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
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
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

        private static List<Row> Get1()
        {
            List<Row> stck = new List<Row>();
            stck.Add(Get1Header(0.5, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));
            stck.Add(Get1Header(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("OperationCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Add(Get1Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("OperationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Add(Get1Header(1.5, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("PassportNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Add(Get1Header(0.5, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("Type")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Add(Get1Header(1, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("Radionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Add(Get1Header(1.5, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("FactoryNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Add(Get1Header(1, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("Quantity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));
            stck.Add(Get1Header(1, 9,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("Activity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,8
            ));
            stck.Add(Get1Header(1.5, 10,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("CreatorOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,9
            ));
            stck.Add(Get1Header(1.5, 11,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("CreationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,10
            ));
            stck.Add(Get1Header(1, 12,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("Category")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,11
            ));
            stck.Add(Get1Header(1, 13,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("SignedServicePeriod")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,12
            ));
            stck.Add(Get1Header(1.5, 14,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("PropertyCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,13
            ));
            stck.Add(Get1Header(1.5, 15,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("Owner")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,14
            ));
            stck.Add(Get1Header(1.5, 16,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("DocumentVid")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,15
            ));
            stck.Add(Get1Header(1.5, 17,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("DocumentNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,16
            ));
            stck.Add(Get1Header(1, 18,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("DocumentDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,17
            ));
            stck.Add(Get1Header(2, 19,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("ProviderOrRecieverOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,18
            ));
            stck.Add(Get1Header(2, 20,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("TransporterOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,19
            ));
            stck.Add(Get1Header(2, 21,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("PackName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,20
            ));
            stck.Add(Get1Header(1, 22,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("PackType")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,21
            ));
            stck.Add(Get1Header(1.5, 23,
                ((Form_PropertyAttribute) Type.GetType("Models.Form11,Models").GetProperty("PackNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,22
            ));

            return stck;
        }

        #endregion

        #region 1.2

        private static readonly int Wdth2 = 100;
        private static readonly int RowHeight2 = 30;
        private static readonly Color border_color2 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get2Header(double starWidth, int Column, string Text, int offset)
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
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth2,
                Height = RowHeight2,
                BorderBrush = new SolidColorBrush(border_color2),
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

        private static List<Row> Get2()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get2Header(0.5, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));
            stck.Add(Get2Header(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("OperationCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Add(Get2Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("OperationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Add(Get2Header(1.5, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("PassportNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Add(Get2Header(1.5, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("NameIOU")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Add(Get2Header(1.5, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("FactoryNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Add(Get2Header(1, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("Mass")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Add(Get2Header(1.5, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("CreatorOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));
            stck.Add(Get2Header(1.5, 9,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("CreationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,8
            ));
            stck.Add(Get2Header(1, 10,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("SignedServicePeriod")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,9
            ));
            stck.Add(Get2Header(1.5, 11,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("PropertyCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,10
            ));
            stck.Add(Get2Header(1, 12,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("Owner")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,11
            ));
            stck.Add(Get2Header(1, 13,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("DocumentVid")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,12
            ));
            stck.Add(Get2Header(1.5, 14,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("DocumentNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,13
            ));
            stck.Add(Get2Header(1, 15,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("DocumentDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,14
            ));
            stck.Add(Get2Header(2, 16,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("ProviderOrRecieverOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,15
            ));
            stck.Add(Get2Header(1.5, 17,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("TransporterOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,16
            ));
            stck.Add(Get2Header(2, 18,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("PackName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,17
            ));
            stck.Add(Get2Header(1, 19,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("PackType")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,18
            ));
            stck.Add(Get2Header(1.5, 20,
                ((Form_PropertyAttribute) Type.GetType("Models.Form12,Models").GetProperty("PackNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,19
            ));

            return stck;
        }

        #endregion

        #region 1.3

        private static readonly int Wdth3 = 100;
        private static readonly int RowHeight3 = 30;
        private static readonly Color border_color3 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get3Header(double starWidth, int Column, string Text, int offset)
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
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth3,
                Height = RowHeight3,
                BorderBrush = new SolidColorBrush(border_color3),
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

        private static List<Row> Get3()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get3Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));

            stck.Add(Get3Header(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("OperationCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Add(Get3Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("OperationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Add(Get3Header(1.1, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("PassportNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Add(Get3Header(1, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("Type")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Add(Get3Header(1, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("Radionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Add(Get3Header(2, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("FactoryNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Add(Get3Header(1, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("Activity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));
            stck.Add(Get3Header(2, 9,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("CreatorOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,8
            ));
            stck.Add(Get3Header(2, 10,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("CreationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,9
            ));
            stck.Add(Get3Header(2, 11,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("AggregateState")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,10
            ));
            stck.Add(Get3Header(2, 12,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("PropertyCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,11
            ));
            stck.Add(Get3Header(1, 13,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("Owner")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,12
            ));
            stck.Add(Get3Header(1, 14,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("DocumentVid")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,13
            ));
            stck.Add(Get3Header(1.2, 15,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("DocumentNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,14
            ));
            stck.Add(Get3Header(1, 16,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("DocumentDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,15
            ));
            stck.Add(Get3Header(2, 17,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("ProviderOrRecieverOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,16
            ));
            stck.Add(Get3Header(2, 18,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("TransporterOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,17
            ));
            stck.Add(Get3Header(2, 19,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("PackName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,18
            ));
            stck.Add(Get3Header(1, 20,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("PackType")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,19
            ));
            stck.Add(Get3Header(2, 21,
                ((Form_PropertyAttribute) Type.GetType("Models.Form13,Models").GetProperty("PackNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,20
            ));

            return stck;
        }

        #endregion

        #region 1.4

        private static readonly int Wdth4 = 100;
        private static readonly int RowHeight4 = 30;
        private static readonly Color border_color4 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get4Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth4,
                Height = RowHeight4,
                BorderBrush = new SolidColorBrush(border_color4),
                CellRow = Column,
                CellColumn = 0
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth4,
                Height = RowHeight4,
                BorderBrush = new SolidColorBrush(border_color4),
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

        private static List<Row> Get4()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get4Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));

            stck.Add(Get4Header(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("OperationCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Add(Get4Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("OperationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Add(Get4Header(1.1, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("PassportNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Add(Get4Header(1, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("Name")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Add(Get4Header(1, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("Sort")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Add(Get4Header(2, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("Radionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Add(Get4Header(1, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("Activity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));
            stck.Add(Get4Header(2, 9,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("ActivityMeasurementDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,8
            ));
            stck.Add(Get4Header(1, 10,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("Volume")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,9
            ));
            stck.Add(Get4Header(1, 11,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("Mass")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,10
            ));
            stck.Add(Get4Header(2, 12,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("AggregateState")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,11
            ));
            stck.Add(Get4Header(2, 13,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("PropertyCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,12
            ));
            stck.Add(Get4Header(1, 14,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("Owner")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,13
            ));
            stck.Add(Get4Header(1, 15,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("DocumentVid")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,14
            ));
            stck.Add(Get4Header(1.2, 16,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("DocumentNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,15
            ));
            stck.Add(Get4Header(1, 17,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("DocumentDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,16
            ));
            stck.Add(Get4Header(2, 18,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("ProviderOrRecieverOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,17
            ));
            stck.Add(Get4Header(2, 19,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("TransporterOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,18
            ));
            stck.Add(Get4Header(2, 20,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("PackName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,19
            ));
            stck.Add(Get4Header(1, 21,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("PackType")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,20
            ));
            stck.Add(Get4Header(2, 22,
                ((Form_PropertyAttribute) Type.GetType("Models.Form14,Models").GetProperty("PackNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,21
            ));

            return stck;
        }

        #endregion

        #region 1.5

        private static readonly int Wdth5 = 100;
        private static readonly int RowHeight5 = 30;
        private static readonly Color border_color5 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get5Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth5,
                Height = RowHeight5,
                BorderBrush = new SolidColorBrush(border_color5),
                CellRow = Column,
                CellColumn = 0
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth5,
                Height = RowHeight5,
                BorderBrush = new SolidColorBrush(border_color5),
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

        private static List<Row> Get5()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get5Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));

            stck.Add(Get5Header(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("OperationCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Add(Get5Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("OperationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Add(Get5Header(1.1, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("PassportNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Add(Get5Header(1, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("Type")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Add(Get5Header(1, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("Radionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Add(Get5Header(2, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("FactoryNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Add(Get5Header(1, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("Quantity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));
            stck.Add(Get5Header(1, 9,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("Activity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,8
            ));
            stck.Add(Get5Header(2, 10,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("CreationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,9
            ));
            stck.Add(Get5Header(1, 11,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("StatusRAO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,10
            ));
            stck.Add(Get5Header(1, 12,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("DocumentVid")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,11
            ));
            stck.Add(Get5Header(1.2, 13,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("DocumentNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,12
            ));
            stck.Add(Get5Header(1, 14,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("DocumentDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,13
            ));
            stck.Add(Get5Header(2, 15,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("ProviderOrRecieverOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,14
            ));
            stck.Add(Get5Header(2, 16,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("TransporterOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,15
            ));
            stck.Add(Get5Header(2, 17,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("PackName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,16
            ));
            stck.Add(Get5Header(1, 18,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("PackType")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,17
            ));
            stck.Add(Get5Header(2, 19,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("PackNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,18
            ));
            stck.Add(Get5Header(1.2, 20,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("StoragePlaceName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,19
            ));
            stck.Add(Get5Header(1, 21,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("StoragePlaceCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,20
            ));
            stck.Add(Get5Header(2.1, 22,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("RefineOrSortRAOCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,21
            ));
            stck.Add(Get5Header(1, 23,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("Subsidy")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,22
            ));
            stck.Add(Get5Header(2, 24,
                ((Form_PropertyAttribute) Type.GetType("Models.Form15,Models").GetProperty("FcpNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,23
            ));
            return stck;
        }

        #endregion

        #region 1.6

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
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));

            stck.Add(Get6Header(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("OperationCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Add(Get6Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("OperationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Add(Get6Header(1, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("CodeRAO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Add(Get6Header(1, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("StatusRAO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Add(Get6Header(1, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("Volume")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Add(Get6Header(1, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("Mass")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Add(Get6Header(1.5, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("QuantityOZIII")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));
            stck.Add(Get6Header(1, 9,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("MainRadionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,8
            ));
            stck.Add(Get6Header(2, 10,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("TritiumActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,9
            ));
            stck.Add(Get6Header(3.5, 11,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("BetaGammaActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,10
            ));
            stck.Add(Get6Header(3.5, 12,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("AlphaActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,11
            ));
            stck.Add(Get6Header(2, 13,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("TransuraniumActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,12
            ));
            stck.Add(Get6Header(2, 14,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("ActivityMeasurementDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,13
            ));
            stck.Add(Get6Header(1, 15,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("DocumentVid")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,14
            ));
            stck.Add(Get6Header(1.2, 16,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("DocumentNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,15
            ));
            stck.Add(Get6Header(1, 17,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("DocumentDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,16
            ));
            stck.Add(Get6Header(2, 18,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("ProviderOrRecieverOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,17
            ));
            stck.Add(Get6Header(2, 19,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("TransporterOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,18
            ));
            stck.Add(Get6Header(1.2, 20,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("StoragePlaceName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,19
            ));
            stck.Add(Get6Header(1, 21,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("StoragePlaceCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,20
            ));
            stck.Add(Get6Header(2.1, 22,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("RefineOrSortRAOCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,21
            ));
            stck.Add(Get6Header(2, 23,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("PackName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,22
            ));
            stck.Add(Get6Header(1, 24,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("PackType")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,23
            ));
            stck.Add(Get6Header(2, 25,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("PackNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,24
            ));
            stck.Add(Get6Header(1, 26,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("Subsidy")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,25
            ));
            stck.Add(Get6Header(2, 27,
                ((Form_PropertyAttribute) Type.GetType("Models.Form16,Models").GetProperty("FcpNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,26
            ));

            return stck;
        }

        #endregion

        #region 1.7

        private static readonly int Wdth7 = 100;
        private static readonly int RowHeight7 = 30;
        private static readonly Color border_color7 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get7Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth7,
                Height = RowHeight7,
                BorderBrush = new SolidColorBrush(border_color7),
                CellRow = Column,
                CellColumn = 0
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth7,
                Height = RowHeight7,
                BorderBrush = new SolidColorBrush(border_color7),
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

        private static List<Row> Get7()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get7Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));

            stck.Add(Get7Header(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("OperationCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Add(Get7Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("OperationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Add(Get7Header(2, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("PackName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Add(Get7Header(1, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("PackType")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Add(Get7Header(3, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("PackFactoryNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Add(Get7Header(2, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("PackNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Add(Get7Header(2, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("FormingDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));
            stck.Add(Get7Header(2, 9,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("PassportNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,8
            ));
            stck.Add(Get7Header(1, 10,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("Volume")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,9
            ));
            stck.Add(Get7Header(1, 11,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("Mass")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,10
            ));
            stck.Add(Get7Header(2, 12,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("Radionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,11
            ));
            stck.Add(Get7Header(2, 13,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("SpecificActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,12
            ));
            stck.Add(Get7Header(1.2, 14,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("DocumentVid")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,13
            ));
            stck.Add(Get7Header(2, 15,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("DocumentNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,14
            ));
            stck.Add(Get7Header(1, 16,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("DocumentDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,15
            ));
            stck.Add(Get7Header(2, 17,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("ProviderOrRecieverOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,16
            ));
            stck.Add(Get7Header(2, 18,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("TransporterOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,17
            ));
            stck.Add(Get7Header(1.2, 19,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("StoragePlaceName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,18
            ));
            stck.Add(Get7Header(1, 20,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("StoragePlaceCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,19
            ));
            stck.Add(Get7Header(1, 21,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("CodeRAO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,20
            ));
            stck.Add(Get7Header(1, 22,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("StatusRAO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,21
            ));
            stck.Add(Get7Header(2, 23,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("VolumeOutOfPack")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,22
            ));
            stck.Add(Get7Header(2, 24,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("MassOutOfPack")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,23
            ));
            stck.Add(Get7Header(1, 25,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("Quantity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,24
            ));
            stck.Add(Get7Header(2, 26,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("TritiumActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,25
            ));
            stck.Add(Get7Header(3.5, 27,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("BetaGammaActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,26
            ));
            stck.Add(Get7Header(3.5, 28,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("AlphaActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,27
            ));
            stck.Add(Get7Header(2, 29,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("TransuraniumActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,28
            ));
            stck.Add(Get7Header(2.1, 30,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("RefineOrSortRAOCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,29
            ));
            stck.Add(Get7Header(1, 31,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("Subsidy")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,30
            ));
            stck.Add(Get7Header(2, 32,
                ((Form_PropertyAttribute) Type.GetType("Models.Form17,Models").GetProperty("FcpNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,31
            ));

            return stck;
        }

        #endregion

        #region 1.8

        private static readonly int Wdth8 = 100;
        private static readonly int RowHeight8 = 30;
        private static readonly Color border_color8 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get8Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth8,
                Height = RowHeight8,
                BorderBrush = new SolidColorBrush(border_color8),
                CellRow = Column,
                CellColumn = 0
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth8,
                Height = RowHeight8,
                BorderBrush = new SolidColorBrush(border_color8),
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

        private static List<Row> Get8()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get8Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));

            stck.Add(Get8Header(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("OperationCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Add(Get8Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("OperationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Add(Get8Header(2, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("IndividualNumberZHRO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Add(Get8Header(1.1, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("PassportNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Add(Get8Header(1, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("Volume6")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Add(Get8Header(1, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("Mass7")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Add(Get8Header(1.5, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("SaltConcentration")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));
            stck.Add(Get8Header(2, 9,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("Radionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,8
            ));
            stck.Add(Get8Header(2, 10,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("SpecificActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,9
            ));
            stck.Add(Get8Header(1, 11,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("DocumentVid")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,10
            ));
            stck.Add(Get8Header(1.2, 12,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("DocumentNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,11
            ));
            stck.Add(Get8Header(1, 13,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("DocumentDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,12
            ));
            stck.Add(Get8Header(2, 14,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("ProviderOrRecieverOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,13
            ));
            stck.Add(Get8Header(2, 15,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("TransporterOKPO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,14
            ));
            stck.Add(Get8Header(1.2, 16,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("StoragePlaceName")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,15
            ));
            stck.Add(Get8Header(1, 17,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("StoragePlaceCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,16
            ));
            stck.Add(Get8Header(1, 18,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("CodeRAO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,17
            ));
            stck.Add(Get8Header(1, 19,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("StatusRAO")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,18
            ));
            stck.Add(Get8Header(1, 20,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("Volume20")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,19
            ));
            stck.Add(Get8Header(1, 21,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("Mass21")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,20
            ));
            stck.Add(Get8Header(2, 22,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("TritiumActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,21
            ));
            stck.Add(Get8Header(3.5, 23,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("BetaGammaActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,22
            ));
            stck.Add(Get8Header(3.5, 24,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("AlphaActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,23
            ));
            stck.Add(Get8Header(2, 25,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("TransuraniumActivity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,24
            ));
            stck.Add(Get8Header(2.1, 26,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("RefineOrSortRAOCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,25
            ));
            stck.Add(Get8Header(1, 27,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("Subsidy")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,26
            ));
            stck.Add(Get8Header(2, 28,
                ((Form_PropertyAttribute) Type.GetType("Models.Form18,Models").GetProperty("FcpNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,27
            ));

            return stck;
        }

        #endregion

        #region 1.9

        private static readonly int Wdth9 = 100;
        private static readonly int RowHeight9 = 30;
        private static readonly Color border_color9 = Color.FromArgb(255, 0, 0, 0);

        private static Row Get9Header(double starWidth, int Column, string Text, int offset)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth9,
                Height = RowHeight9,
                BorderBrush = new SolidColorBrush(border_color9),
                CellRow = Column,
                CellColumn = 0
            };
            var ram1 = new RamAccess<string>(null, (offset + 1).ToString());
            var cell1 = new Cell(ram1, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth9,
                Height = RowHeight9,
                BorderBrush = new SolidColorBrush(border_color9),
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

        private static List<Row> Get9()
        {
            List<Row> stck = new List<Row>();

            stck.Add(Get9Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty("NumberInOrder")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,0
            ));

            stck.Add(Get9Header(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty("OperationCode")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,1
            ));
            stck.Add(Get9Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty("OperationDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,2
            ));
            stck.Add(Get9Header(1, 4,
                ((Form_PropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty("DocumentVid")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,3
            ));
            stck.Add(Get9Header(1.2, 5,
                ((Form_PropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty("DocumentNumber")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,4
            ));
            stck.Add(Get9Header(1, 6,
                ((Form_PropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty("DocumentDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,5
            ));
            stck.Add(Get9Header(2, 7,
                ((Form_PropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty("CodeTypeAccObject")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,6
            ));
            stck.Add(Get9Header(1, 8,
                ((Form_PropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty("Radionuclids")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,7
            ));
            stck.Add(Get9Header(1, 9,
                ((Form_PropertyAttribute) Type.GetType("Models.Form19,Models").GetProperty("Activity")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,8
            ));

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
            stck.Add(GetNotesHeader(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Note,Models").GetProperty("Comment")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }

        #endregion
    }
}