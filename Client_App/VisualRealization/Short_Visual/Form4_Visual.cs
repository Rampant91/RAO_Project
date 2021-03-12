using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Models.LocalStorage;
using Avalonia.Data;
using Avalonia;
using Models.Attributes;
using Avalonia.Media;
using Avalonia.Controls.Templates;
using Models.Client_Model;

namespace Client_App.Short_Visual
{
    public class Form4_Visual
    {
        public static Grid FormF_Visual()
        {
            Grid grd = new Grid();

            //1
            grd.RowDefinitions.Add(new RowDefinition());
            Grid gd = new Grid();
            gd.SetValue(Grid.RowProperty, 0);

            //1.1
            gd.ColumnDefinitions.Add(new ColumnDefinition());
            var item1 = Form0_Visual();
            item1.SetValue(Grid.ColumnProperty, 0);
            gd.Children.Add(item1);

            //1.2
            gd.ColumnDefinitions.Add(new ColumnDefinition());
            var item2 = FormB_Visual();
            item2.SetValue(Grid.ColumnProperty, 0);
            gd.Children.Add(item2);

            //1
            grd.Children.Add(gd);

            //2
            var item3 = FormX_Visual();
            item3.SetValue(Grid.RowProperty, 1);
            grd.Children.Add(item3);

            return grd;
        }

        //Форма 10
        static DataGrid Form0_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }

        //Форма 1X
        static DataGrid FormX_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }

        //Кнопки создания или изменения формы
        static Panel FormB_Visual()
        {
            Panel grd = new Panel();

            return grd;
        }
    }
}
