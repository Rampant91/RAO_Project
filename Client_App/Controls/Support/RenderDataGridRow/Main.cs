using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Data;
using Avalonia.Media;
using Models.Attributes;
using Avalonia.Input;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Interactivity;

namespace Client_App.Controls.Support.RenderDataGridRow
{
    public class Main
    {
        public static Control GetControl(string type,int Row, Object Context)
        {
            switch (type)
            {
                case "0": return Get0(Row, Context);
                case "1": return Get1(Row, Context);
                case "2": return Get2();
                case "3": return Get3();
                case "4": return Get4();
                case "5": return Get5();
            }
            return null;
        }

        static int Wdth0 = 100;
        static int RowHeight0 = 30;
        static Color border_color0 = Color.FromArgb(255, 0, 0, 0);

        static Control Get0Row(int starWidth, int Row,int Column, string Binding, Object Context)
        {
            var cell = new Controls.DataGrid.Cell(Context, Binding, true);
            cell.Width = starWidth * Wdth0;
            cell.Height = RowHeight0;
            cell.BorderBrush = new SolidColorBrush(border_color0);
            cell.CellRow = Row;
            cell.CellColumn = Column;


            return cell;
        }
        static Control Get0(int Row, Object Context)
        {
            DataGrid.Row stck = new DataGrid.Row();
            stck.Orientation = Avalonia.Layout.Orientation.Horizontal;
            stck.Width = 4 * Wdth0;
            stck.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            stck.Spacing = -1;

            stck.Children.Add(Get0Row(1, Row, 1, "Master.RegNo", Context));
            stck.Children.Add(Get0Row(2, Row, 2, "Master.ShortJurLico", Context));
            stck.Children.Add(Get0Row(1, Row, 3, "Master.Okpo", Context));

            return stck;
        }

        static int Wdth1 = 100;
        static int RowHeight1 = 30;
        static Color border_color1 = Color.FromArgb(255, 0, 0, 0);
        static Control Get1Row(int starWidth, int Row, int Column, string Binding, Object Context)
        {
            var cell = new Controls.DataGrid.Cell(Context, Binding, true);
            cell.Width = starWidth * Wdth1;
            cell.Height = RowHeight1;
            cell.BorderBrush = new SolidColorBrush(border_color1);
            cell.CellRow = Row;
            cell.CellColumn = Column;

            return cell;
        }
        static Control Get1(int Row, object Context)
        {
            DataGrid.Row stck = new DataGrid.Row();
            stck.Width = 8 * Wdth1;
            stck.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            stck.Orientation = Avalonia.Layout.Orientation.Horizontal;
            stck.Spacing = -1;

            stck.Children.Add(Get1Row(1, Row, 1, "NumberInOrder",Context));
            stck.Children.Add(Get1Row(1, Row, 2, "FormNum",Context));

            var str = "{0:d}";
            stck.Children.Add(Get1Row(1, Row, 3, "StartPeriod", Context));
            stck.Children.Add(Get1Row(1, Row, 4, "EndPeriod", Context));
            stck.Children.Add(Get1Row(1, Row, 5, "ExportDate", Context));
            stck.Children.Add(Get1Row(2, Row, 6, "IsCorrection", Context));
            stck.Children.Add(Get1Row(1, Row, 7, "Comments", Context));

            return stck;
        }
        static Control Get2()
        {
            return null;
        }
        static Control Get3()
        {
            return null;
        }
        static Control Get4()
        {
            return null;
        }
        static Control Get5()
        {
            return null;
        }
    }
}
