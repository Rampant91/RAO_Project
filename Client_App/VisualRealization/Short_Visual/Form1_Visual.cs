using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Models;
using Avalonia.Data;
using Avalonia;
using Models.Attributes;
using Avalonia.Media;
using Avalonia.Controls.Templates;
using Avalonia.Data.Converters;

namespace Client_App.Short_Visual
{
    public class Form1_Visual
    { 
        //Полный вывод
        public static void FormF_Visual(in Panel pnl0, in Panel pnlx, in Panel pnlb)
        {
            pnl0.Children.Add(Form0_Visual());
            pnlx.Children.Add(FormX_Visual());
            pnlb.Children.Add(FormB_Visual());
        }

        //Форма 10
        static DataGrid Form0_Visual()
        {
            DataGrid grd = new DataGrid();
            grd.IsReadOnly = true;
            grd.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            grd.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            grd.Bind(DataGrid.ItemsProperty, new Binding("FormModel_Local.Dictionary.GetFilteredStorage"));

            var cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>();
            itms.Add(new MenuItem
            {
                Header = "Добавить форму",
                [!MenuItem.CommandProperty] = new Binding("AddForm"),
                CommandParameter = "10",
            });
            itms.Add(new MenuItem
            {
                Header = "Изменить форму",
                [!MenuItem.CommandProperty] = new Binding("ChangeForm"),
                [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItem"),
            });
            itms.Add(new MenuItem
            {
                Header = "Удалить форму",
                [!MenuItem.CommandProperty] = new Binding("DeleteForm"),
                [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItem"),
            });
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            DataGridTemplateColumn clm1 = new DataGridTemplateColumn();
            clm1.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            clm1.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                GetProperty("RegNo").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter= "10/RegNo"
            };
            clm1.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding("RegNo"),
                    });
            grd.Columns.Add(clm1);

            DataGridTemplateColumn clm2 = new DataGridTemplateColumn();
            clm2.Width = new DataGridLength(3, DataGridLengthUnitType.Star);
            clm2.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                GetProperty("ShortJurLico").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "10/ShortJurLico"
            };
            clm2.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBlock
                    {
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        [!TextBlock.TextProperty] = new Binding("ShortJurLico"),
                    });
            grd.Columns.Add(clm2);

            DataGridTemplateColumn clm3 = new DataGridTemplateColumn();
            clm3.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            clm3.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                GetProperty("Okpo").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "10/Okpo"
            };
            clm3.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding("Okpo"),
                    });
            grd.Columns.Add(clm3);
            return grd;
        }
        
        //Форма 1X
        static DataGrid FormX_Visual()
        {
            DataGrid grd = new DataGrid();
            grd.IsReadOnly = true;
            grd.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            grd.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            grd.Bind(DataGrid.ItemsProperty, new Binding("FormModel_Local.GetFilteredDictionary"));

            var cntx = new ContextMenu();
            List<MenuItem> itms = new List<MenuItem>();
            itms.Add(new MenuItem
            {
                Header = "Изменить форму",
                [!MenuItem.CommandProperty] = new Binding("ChangeForm"),
                [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItem"),
            });
            itms.Add(new MenuItem
            {
                Header = "Удалить форму",
                [!MenuItem.CommandProperty] = new Binding("DeleteForm"),
                [!MenuItem.CommandParameterProperty] = new Binding("$parent[2].SelectedItem"),
            });
            cntx.Items = itms;

            grd.ContextMenu = cntx;

            DataGridTemplateColumn clm1 = new DataGridTemplateColumn();
            clm1.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            clm1.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Abstracts.Form1,Models").
                GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "1/NumberInOrder"
            };
            clm1.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding("NumberInOrder"),
                    });
            grd.Columns.Add(clm1);

            DataGridTemplateColumn clm2 = new DataGridTemplateColumn();
            clm2.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            clm2.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Abstracts.Form1,Models").
                GetProperty("FormNum").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "1/FormNum"
            };
            clm2.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding("FormNum"),
                    });
            grd.Columns.Add(clm2);

            DataGridTemplateColumn clm3 = new DataGridTemplateColumn();
            clm3.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            clm3.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form,Models").
                GetProperty("StartPeriod").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "_/StartPeriod"
            };
            Binding bnd = new Binding("StartPeriod");
            bnd.StringFormat = "{0:d}";
            clm3.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = bnd,
                    });
            grd.Columns.Add(clm3);

            DataGridTemplateColumn clm4 = new DataGridTemplateColumn();
            clm4.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            clm4.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form,Models").
                GetProperty("EndPeriod").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "_/EndPeriod"
            };
            bnd = new Binding("EndPeriod");
            bnd.StringFormat = "{0:d}";
            clm4.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = bnd,
                    });
            grd.Columns.Add(clm4);

            DataGridTemplateColumn clm5 = new DataGridTemplateColumn();
            clm5.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            clm5.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form,Models").
                GetProperty("ExportDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "_/ExportDate"
            };
            bnd = new Binding("ExportDate");
            bnd.StringFormat = "{0:d}";
            clm5.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = bnd,
                    });
            grd.Columns.Add(clm5);

            DataGridTemplateColumn clm6 = new DataGridTemplateColumn();
            clm6.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            clm6.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form,Models").
                GetProperty("IsCorrection").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "_/IsCorrection"
            };
            clm6.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding("IsCorrection"),
                    });
            grd.Columns.Add(clm6);
            DataGridTemplateColumn clm7 = new DataGridTemplateColumn();
            clm7.Width = new DataGridLength(2, DataGridLengthUnitType.Star);
            clm7.Header = new Button
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Content = ((Form_PropertyAttribute)Type.GetType("Models.Form,Models").
                GetProperty("Comments").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name,
                [!Button.CommandProperty] = new Binding("AddSort"),
                CommandParameter = "_/Comments"
            };
            clm7.CellTemplate = new FuncDataTemplate<Form>((x, e) =>
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding("Comments"),
                    });
            grd.Columns.Add(clm7);
            return grd;
        }

        //Кнопки создания или изменения формы
        static Panel FormB_Visual()
        {
            Panel panel = new Panel();
            panel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            panel.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;

            Button btn1 = new Button();
            btn1.Content = ((Form_ClassAttribute)Type.GetType("Models.Form11,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn1.Bind(Button.CommandProperty,new Binding("AddForm"));
            btn1.CommandParameter = "11";
            btn1.Height = 30;
            btn1.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn1.Margin = Thickness.Parse("5,0,0,0");
            panel.Children.Add(btn1);

            Button btn2 = new Button();
            btn2.Content = ((Form_ClassAttribute)Type.GetType("Models.Form12,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn2.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn2.CommandParameter = "12";
            btn2.Height = 30;
            btn2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn2.Margin = Thickness.Parse("5,35,0,0");
            panel.Children.Add(btn2);

            Button btn3 = new Button();
            btn3.Content = ((Form_ClassAttribute)Type.GetType("Models.Form13,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn3.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn3.CommandParameter = "13";
            btn3.Height = 30;
            btn3.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn3.Margin = Thickness.Parse("5,70,0,0");
            panel.Children.Add(btn3);

            Button btn4 = new Button();
            btn4.Content = ((Form_ClassAttribute)Type.GetType("Models.Form14,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn4.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn4.CommandParameter = "14";
            btn4.Height = 30;
            btn4.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn4.Margin = Thickness.Parse("5,105,0,0");
            panel.Children.Add(btn4);

            Button btn5 = new Button();
            btn5.Content = ((Form_ClassAttribute)Type.GetType("Models.Form15,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn5.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn5.CommandParameter = "15";
            btn5.Height = 30;
            btn5.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn5.Margin = Thickness.Parse("5,140,0,0");
            panel.Children.Add(btn5);

            Button btn6 = new Button();
            btn6.Content = ((Form_ClassAttribute)Type.GetType("Models.Form16,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn6.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn6.CommandParameter = "16";
            btn6.Height = 30;
            btn6.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn6.Margin = Thickness.Parse("5,175,0,0");
            panel.Children.Add(btn6);

            Button btn7 = new Button();
            btn7.Content = ((Form_ClassAttribute)Type.GetType("Models.Form17,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn7.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn7.CommandParameter = "17";
            btn7.Height = 30;
            btn7.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn7.Margin = Thickness.Parse("5,210,0,0");
            panel.Children.Add(btn7);

            Button btn8 = new Button();
            btn8.Content = ((Form_ClassAttribute)Type.GetType("Models.Form18,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn8.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn8.CommandParameter = "18";
            btn8.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn8.Margin = Thickness.Parse("5,245,0,0");
            panel.Children.Add(btn8);

            Button btn9 = new Button();
            btn9.Content = ((Form_ClassAttribute)Type.GetType("Models.Form19,Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            btn9.Bind(Button.CommandProperty, new Binding("AddForm"));
            btn9.CommandParameter = "19";
            btn9.Height = 30;
            btn9.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            btn9.Margin = Thickness.Parse("5,275,0,0");
            panel.Children.Add(btn9);

            return panel;
        }
    }
}
