using Avalonia.Controls;
using Avalonia.Media;
using Models.Attributes;
using System;
using System.Linq;

namespace Client_App.Controls.Support.RenderDataGridHeader
{
    public class Form1
    {
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
                case "6": return Get6();
                case "7": return Get7();
                case "8": return Get8();
                case "9": return Get9();
                case "1*": return GetNotes();
            }
            return null;
        }

        private static Control Get0()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(2, 1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("OrganUprav").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("JurLico").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(2, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("ShortJurLico").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("JurLicoAddress").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(2, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("JurLicoFactAddress").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(2, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("GradeFIO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Telephone").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Fax").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Email").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("RegNo").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Okpo").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Okved").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Okogu").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Oktmo").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Inn").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Kpp").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Okopf").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form10,Models").
                    GetProperty("Okfs").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            //stck.Children.Add(Get1Header(1, 24,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("DocumentNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));

            return stck;
        }

        private static readonly int Wdth0 = 100;
        private static readonly int RowHeight0 = 30;
        private static readonly Color border_color0 = Color.FromArgb(255, 0, 0, 0);

        private static Control Get1Header(int starWidth, int Column, string Text)
        {
            Models.DataAccess.RamAccess<string>? ram = new Models.DataAccess.RamAccess<string>(null, Text);
            DataGrid.Cell? cell = new Controls.DataGrid.Cell(ram, "", true)
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

        private static Control Get1()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(1, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PassportNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Type").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("FactoryNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Quantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Activity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("CreatorOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("CreationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Category").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("SignedServicePeriod").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PropertyCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("Owner").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 19,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 20,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(2, 21,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 22,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 23,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
                    GetProperty("PackNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            //stck.Children.Add(Get1Header(1, 24,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("DocumentNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));

            return stck;
        }

        private static Control GetNotes()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Note,Models").
                    GetProperty("RowNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Note,Models").
                    GetProperty("GraphNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Note,Models").
                    GetProperty("Comment").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            ////stck.Children.Add(Get1Header(1, 2,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 3,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 4,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("PassportNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 5,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("Type").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 6,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 7,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("FactoryNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 8,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("Quantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 9,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("Activity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 10,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("CreatorOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 11,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("CreationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 12,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("Category").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 13,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("SignedServicePeriod").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 14,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("PropertyCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 15,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("Owner").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 16,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 17,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 18,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 19,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 20,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(2, 21,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 22,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            ////stck.Children.Add(Get1Header(1, 23,
            ////        ((Form_PropertyAttribute)Type.GetType("Models.Form11,Models").
            ////        GetProperty("PackNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            ////    ));
            //stck.Children.Add(Get1Header(1, 24,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("DocumentNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));

            return stck;
        }

        private static Control Get2()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(1, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("PassportNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(2, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("NameIOU").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("FactoryNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("Mass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("CreatorOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("CreationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("SignedServicePeriod").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("PropertyCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("Owner").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(2, 18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 19,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 20,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form12,Models").
                    GetProperty("PackNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            //stck.Children.Add(Get1Header(1, 21,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("DocumentNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 22,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("FactoryNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 23,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("PassportNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 24,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("PackTypeRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 25,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("PackNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));

            return stck;
        }

        private static Control Get3()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("PassportNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("Type").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("FactoryNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("Activity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("CreatorOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("CreationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("AggregateState").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("PropertyCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("Owner").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 19,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 20,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 21,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
                    GetProperty("PackNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            //stck.Children.Add(Get1Header(1, 22,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("PassportNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 23,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("TypeRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 24,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("FactoryNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 25,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("PackTypeRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 26,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("PackNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 27,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form13,Models").
            //        GetProperty("DocumentNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));

            return stck;
        }

        private static Control Get4()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("PassportNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("Name").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("Sort").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("Activity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("ActivityMeasurementDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("Volume").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("Mass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("AggregateState").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("PropertyCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("Owner").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 19,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 20,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 21,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 22,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
                    GetProperty("PackNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            //stck.Children.Add(Get1Header(1, 23,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
            //        GetProperty("PassportNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 24,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
            //        GetProperty("PackTypeRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 25,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
            //        GetProperty("PackNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 26,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form14,Models").
            //        GetProperty("DocumentNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));

            return stck;
        }

        private static Control Get5()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("PassportNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("Type").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("FactoryNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("Quantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("Activity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("CreationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("StatusRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 19,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("PackNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 20,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("StoragePlaceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 21,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("StoragePlaceCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 22,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("RefineOrSortRAOCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 23,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("Subsidy").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 24,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
                    GetProperty("FcpNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            //stck.Children.Add(Get1Header(1, 25,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
            //        GetProperty("PassportNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 26,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
            //        GetProperty("TypeRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 27,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
            //        GetProperty("FactoryNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 28,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
            //        GetProperty("PackNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 29,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
            //        GetProperty("PackTypeRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 30,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form15,Models").
            //        GetProperty("DocumentNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            return stck;
        }

        private static Control Get6()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("CodeRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("StatusRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("Volume").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("Mass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("QuantityOZIII").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("MainRadionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("TritiumActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("BetaGammaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("AlphaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("TransuraniumActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("ActivityMeasurementDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 19,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 20,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("StoragePlaceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 21,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("StoragePlaceCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 22,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("RefineOrSortRAOCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 23,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 24,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 25,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("PackNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 26,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("Subsidy").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 27,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form16,Models").
                    GetProperty("FcpNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            return stck;
        }

        private static Control Get7()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("PackFactoryNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("PackNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("FormingDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("PassportNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("Volume").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("Mass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("SpecificActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 19,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("StoragePlaceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 20,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("StoragePlaceCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 21,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("CodeRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 22,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("StatusRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 23,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("VolumeOutOfPack").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 24,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("MassOutOfPack").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 25,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("Quantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 26,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("TritiumActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 27,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("BetaGammaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 28,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("AlphaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 29,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("TransuraniumActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 30,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("RefineOrSortRAOCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 31,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("Subsidy").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 32,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
                    GetProperty("FcpNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            //stck.Children.Add(Get1Header(1, 33,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
            //        GetProperty("PackTypeRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 34,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
            //        GetProperty("PackNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 35,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form17,Models").
            //        GetProperty("DocumentNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));

            return stck;
        }

        private static Control Get8()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("IndividualNumberZHRO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("PassportNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("Volume6").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("Mass7").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("SaltConcentration").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("SpecificActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("ProviderOrRecieverOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("TransporterOKPO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("StoragePlaceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("StoragePlaceCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("CodeRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 19,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("StatusRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 20,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("Volume20").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 21,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("Mass21").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 22,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("TritiumActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 23,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("BetaGammaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 24,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("AlphaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 25,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("TransuraniumActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 26,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("RefineOrSortRAOCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 27,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("Subsidy").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 28,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
                    GetProperty("FcpNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            //stck.Children.Add(Get1Header(1, 29,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
            //        GetProperty("PassportNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 30,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
            //        GetProperty("IndividualNumberZHROrecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));
            //stck.Children.Add(Get1Header(1, 31,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form18,Models").
            //        GetProperty("DocumentNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));

            return stck;
        }

        private static Control Get9()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 1,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form19,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form19,Models").
                    GetProperty("OperationCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form19,Models").
                    GetProperty("OperationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form19,Models").
                    GetProperty("DocumentVid").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form19,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form19,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form19,Models").
                    GetProperty("CodeTypeAccObject").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form19,Models").
                    GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form19,Models").
                    GetProperty("Quantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form19,Models").
                    GetProperty("Activity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            //stck.Children.Add(Get1Header(1, 10,
            //        ((Form_PropertyAttribute)Type.GetType("Models.Form19,Models").
            //        GetProperty("DocumentNumberRecoded").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
            //    ));

            return stck;
        }
    }
}
