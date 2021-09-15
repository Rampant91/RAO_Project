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
    public class Main
    {
        private static readonly int Wdth0 = 100;
        private static readonly int RowHeight0 = 30;
        private static readonly Color border_color0 = Color.FromArgb(255, 0, 0, 0);

        private static readonly int Wdth1 = 100;
        private static readonly int RowHeight1 = 30;
        private static readonly Color border_color1 = Color.FromArgb(255, 0, 0, 0);

        public static Control GetControl(string type)
        {
            switch (type)
            {
                case "0": return Get0();
                case "1": return Get1();
                case "2": return Get2();
                case "3": return Get3();
                case "4": return Get4();
                case "5": return Get5();
            }

            return null;
        }

        private static Control Get0Header(int starWidth, int Column, string Text)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth0,
                Height = RowHeight0,
                BorderBrush = new SolidColorBrush(border_color0),
                CellRow = 0,
                CellColumn = Column
            };


            return cell;
        }

        private static Control Get0()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get0Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form10,Models").GetProperty("RegNo")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            stck.Children.Add(Get0Header(2, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form10,Models").GetProperty("ShortJurLico")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get0Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form10,Models").GetProperty("Okpo")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }

        private static Control Get1Header(int starWidth, int Column, string Text)
        {
            var ram = new RamAccess<string>(null, Text);
            var cell = new Cell(ram, "", true)
            {
                Background = new SolidColorBrush(Color.Parse("LightGray")),
                Width = starWidth * Wdth1,
                Height = RowHeight1,
                BorderBrush = new SolidColorBrush(border_color1),
                CellRow = 0,
                CellColumn = Column
            };


            return cell;
        }

        private static Control Get1()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = -1
            };

            //stck.Children.Add(Get1Header(1, 1,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Abstracts.Form1,Models").
            //        GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));

            stck.Children.Add(Get1Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Abstracts.Form1,Models").GetProperty("FormNum")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(1, 2,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("StartPeriod")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("EndPeriod")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(1, 4,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("ExportDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(2, 5,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("IsCorrection")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(1, 6,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("Comments")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }

        private static Control Get2()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get0Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Form20,Models").GetProperty("RegNo")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            stck.Children.Add(Get0Header(2, 2,
                ((Form_PropertyAttribute) Type.GetType("Models.Form20,Models").GetProperty("ShortJurLico")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get0Header(1, 3,
                ((Form_PropertyAttribute) Type.GetType("Models.Form20,Models").GetProperty("Okpo")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }

        private static Control Get3()
        {
            StackPanel stck = new()
            {
                Orientation = Orientation.Horizontal,
                Spacing = -1
            };

            //stck.Children.Add(Get1Header(1, 1,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Abstracts.Form2,Models").
            //        GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));

            stck.Children.Add(Get1Header(1, 1,
                ((Form_PropertyAttribute) Type.GetType("Models.Abstracts.Form2,Models").GetProperty("FormNum")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(2, 2,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("Year")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(1, 4,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("ExportDate")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(2, 5,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("IsCorrection")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));
            stck.Children.Add(Get1Header(1, 6,
                ((Form_PropertyAttribute) Type.GetType("Collections.Report,Models").GetProperty("Comments")
                    .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ));

            return stck;
        }

        private static Control Get4()
        {
            return null;
        }

        private static Control Get5()
        {
            return null;
        }
    }
}