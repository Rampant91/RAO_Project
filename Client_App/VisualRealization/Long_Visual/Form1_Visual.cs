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
using Avalonia.Data.Converters;

namespace Client_App.Long_Visual
{
    public class Form1_Visual
    {
        //Форма 10
        public static DataGrid Form10_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form11_Visual()
        {
            DataGrid grd = new DataGrid();
            grd.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            grd.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            grd.Bind(DataGrid.ItemsProperty, new Binding("Storage.GetFilteredStorage"));

            DataGridTemplateColumn clm1 = new DataGridTemplateColumn();
            clm1.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            clm1.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((FormVisualAttribute)Type.GetType("Models.Client_Model.Form11,Models").
                GetProperty("NumberInOrder").GetCustomAttributes(typeof(FormVisualAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "NumberInOrder"
            };
            clm1.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBox
                    {
                        [!TextBox.TextProperty] = new Binding("NumberInOrder"),
                    });
            grd.Columns.Add(clm1);

            DataGridTemplateColumn clm2 = new DataGridTemplateColumn();
            clm2.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            clm2.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((FormVisualAttribute)Type.GetType("Models.Client_Model.Form11,Models").
                GetProperty("OperationCode").GetCustomAttributes(typeof(FormVisualAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "OperationCode"
            };
            clm2.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBox
                    {
                        [!TextBox.TextProperty] = new Binding("OperationCode"),
                    });
            grd.Columns.Add(clm2);

            DataGridTemplateColumn clm3 = new DataGridTemplateColumn();
            clm3.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            clm3.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((FormVisualAttribute)Type.GetType("Models.Client_Model.Form11,Models").
                GetProperty("OperationDate").GetCustomAttributes(typeof(FormVisualAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "OperationDate"
            };
            clm3.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new DatePicker
                    {
                        [!DatePicker.SelectedDateProperty] = new Binding("OperationDate",BindingMode.TwoWay),

                    }) ;
            grd.Columns.Add(clm3);

            DataGridTemplateColumn clm4 = new DataGridTemplateColumn();
            clm4.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            clm4.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((FormVisualAttribute)Type.GetType("Models.Client_Model.Form11,Models").
                GetProperty("PassportNumber").GetCustomAttributes(typeof(FormVisualAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "PassportNumber"
            };
            clm4.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBox
                    {
                        [!TextBox.TextProperty] = new Binding("PassportNumber"),

                    });
            grd.Columns.Add(clm4);

            DataGridTemplateColumn clm5 = new DataGridTemplateColumn();
            clm5.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            clm5.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((FormVisualAttribute)Type.GetType("Models.Client_Model.Form11,Models").
                GetProperty("Type").GetCustomAttributes(typeof(FormVisualAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "Type"
            };
            clm5.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBox
                    {
                        [!TextBox.TextProperty] = new Binding("Type"),

                    });
            grd.Columns.Add(clm5);
            return grd;
        }
        public static DataGrid Form12_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form13_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form14_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form15_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form16_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form17_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form18_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
        public static DataGrid Form19_Visual()
        {
            DataGrid grd = new DataGrid();

            return grd;
        }
    }
}
