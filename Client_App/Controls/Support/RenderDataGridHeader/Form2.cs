using Avalonia.Controls;
using Avalonia.Media;
using Models.Attributes;
using System;
using System.Linq;

namespace Client_App.Controls.Support.RenderDataGridHeader
{
    public class Form2
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
                case "10": return Get10();
                case "11": return Get11();
                case "12": return Get12();
            }
            return null;
        }

        private static Control Get0()
        {
            return null;
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
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("MachinePower").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("MachineCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("RefineMachineName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("NumberOfHoursPerYear").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("CodeRAOIn").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("StatusRAOIn").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("VolumeIn").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("MassIn").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("QuantityIn").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("TritiumActivityIn").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("TritiumActivityOut").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("BetaGammaActivityIn").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("BetaGammaActivityOut").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("TransuraniumActivityIn").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("TransuraniumActivityOut").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("AlphaActivityIn").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("AlphaActivityOut").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 19,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("VolumeOut").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 20,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("MassOut").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 21,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("QuantityOZIIIout").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 22,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("CodeRAOout").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 23,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form21,Models").
                    GetProperty("StatusRAOout").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

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
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("StoragePlaceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("StoragePlaceCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("PackName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("PackType").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("CodeRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("StatusRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("VolumeOutOfPack").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("MassInPack").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("QuantityOZIII").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("TritiumActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("BetaGammaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("TransuraniumActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 14,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("AlphaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 15,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("VolumeInPack").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 16,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("MassOutOfPack").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 17,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("MainRadionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 18,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("Subsidy").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 19,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("FcpNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 20,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form22,Models").
                    GetProperty("PackQuantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

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
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("StoragePlaceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("StoragePlaceCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("ProjectVolume").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("CodeRAO").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("Volume").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("Mass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("SummaryActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("QuantityOZIII").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("DocumentNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("ExpirationDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("DocumentName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form23,Models").
                    GetProperty("DocumentDate").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

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
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            stck.Children.Add(Get1Header(2, 2,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("CodeOYAT").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 3,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("FcpNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 4,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("QuantityFromAnothers").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 5,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("QuantityFromAnothersImported").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 6,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("QuantityCreated").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 7,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("QuantityRemovedFromAccount").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 8,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("MassCreated").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("MassFromAnothers").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("MassFromAnothersImported").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("MassRemovedFromAccount").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("QuantityTransferredToAnother").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("MassAnotherReasons").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("MassTransferredToAnother").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("QuantityAnotherReasons").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("QuantityRefined").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form24,Models").
                    GetProperty("MassRefined").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            return stck;
        }

        private static Control Get5()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").
                    GetProperty("CodeOYAT").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").
                    GetProperty("FcpNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").
                    GetProperty("StoragePlaceCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").
                    GetProperty("StoragePlaceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").
                    GetProperty("FuelMass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").
                    GetProperty("CellMass").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").
                    GetProperty("Quantity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").
                    GetProperty("BetaGammaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form25,Models").
                    GetProperty("AlphaActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            return stck;
        }

        private static Control Get6()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").
                    GetProperty("ObservedSourceNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").
                    GetProperty("ControlledAreaName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").
                    GetProperty("SupposedWasteSource").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").
                    GetProperty("DistanceToWasteSource").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").
                    GetProperty("TestDepth").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").
                    GetProperty("RadionuclidName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form26,Models").
                    GetProperty("AverageYearConcentration").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
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

            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form27,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form27,Models").
                    GetProperty("ObservedSourceNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form27,Models").
                    GetProperty("RadionuclidName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form27,Models").
                    GetProperty("AllowedWasteValue").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form27,Models").
                    GetProperty("FactedWasteValue").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form27,Models").
                    GetProperty("WasteOutbreakPreviousYear").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            return stck;
        }

        private static Control Get8()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form28,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form28,Models").
                    GetProperty("WasteSourceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form28,Models").
                    GetProperty("WasteRecieverName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form28,Models").
                    GetProperty("RecieverTypeCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form28,Models").
                    GetProperty("AllowedWasteRemovalVolume").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form28,Models").
                    GetProperty("RemovedWasteVolume").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form28,Models").
                    GetProperty("PoolDistrictName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            return stck;
        }

        private static Control Get9()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form29,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form29,Models").
                    GetProperty("WasteSourceName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form29,Models").
                    GetProperty("RadionuclidName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form29,Models").
                    GetProperty("AllowedActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form29,Models").
                    GetProperty("FactedActivity").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            return stck;
        }

        private static Control Get10()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").
                    GetProperty("IndicatorName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").
                    GetProperty("PlotName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").
                    GetProperty("PlotKadastrNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").
                    GetProperty("PlotCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").
                    GetProperty("InfectedArea").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").
                    GetProperty("AvgGammaRaysDosePower").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").
                    GetProperty("MaxGammaRaysDosePower").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").
                    GetProperty("WasteDensityAlpha").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").
                    GetProperty("WasteDensityBeta").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form210,Models").
                    GetProperty("FcpNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            return stck;
        }

        private static Control Get11()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").
                    GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").
                    GetProperty("PlotName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").
                    GetProperty("PlotKadastrNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").
                    GetProperty("PlotCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").
                    GetProperty("InfectedArea").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").
                    GetProperty("SpecificActivityOfPlot").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").
                    GetProperty("SpecificActivityOfLiquidPart").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form211,Models").
                    GetProperty("SpecificActivityOfDensePart").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            return stck;
        }

        private static Control Get12()
        {
            StackPanel stck = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = -1
            };

            stck.Children.Add(Get1Header(1, 9,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form212,Models").
                    GetProperty("NumberInOrder").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 10,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form212,Models").
                    GetProperty("Radionuclids").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 11,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form212,Models").
                    GetProperty("PlotName").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 12,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form212,Models").
                    GetProperty("PlotKadastrNumber").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));
            stck.Children.Add(Get1Header(1, 13,
                    ((Form_PropertyAttribute)Type.GetType("Models.Form212,Models").
                    GetProperty("PlotCode").GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name
                ));

            return stck;
        }
    }
}
