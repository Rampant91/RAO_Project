using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using AvaloniaEdit.Utils;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Forms.Form5;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Generate.GenerateForm5
{
    public static class ViacManager
    {
       
        public static string FilePath 
        {
            get
            {
                return "rosatom.xlsx";
            }
        }

        private static List<ViacOrganization> _organizationList = new List<ViacOrganization>();
        public static List<ViacOrganization> OrganizationList 
        {
            get
            {
                if (_organizationList == null || _organizationList.Count <= 0)
                    LoadViacListFromExcel();

                return _organizationList;
            }
        }
        public static List<string> ViacList 
        { 
            get
            {
                if (_organizationList == null || _organizationList.Count<=0)
                    LoadViacListFromExcel();

                return OrganizationList
                    .GroupBy(x => x.Viac)
                    .Select(g => g.Key)
                    .ToList();
            }
        }

        public static async Task<List<ViacOrganization>> GetAllOrganizationsFromVIAC(Window owner)
        {
            string viac = await ShowChooseViacMessage(owner);
            return OrganizationList.FindAll(org => org.Viac == viac);
        }

        private static async Task LoadViacListFromExcel()
        {
            var Desktop = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            //Открываем Excel-файл со списком 

            var configDirectory = BaseVM.ConfigDirectory;

            FileInfo SourceFile = new FileInfo(Path.Combine(configDirectory, FilePath));

            using ExcelPackage excelPackage = new(SourceFile);

            var worksheet = excelPackage.Workbook.Worksheets[0];

            var val =
                Convert.ToString(worksheet.Cells["A1"].Value) is "Рег.№"
                && Convert.ToString(worksheet.Cells["B1"].Value) is "ОКПО"
                && Convert.ToString(worksheet.Cells["C1"].Value) is "Сокращенное наименование"
                && Convert.ToString(worksheet.Cells["D1"].Value) is "ВИАЦ";

            if (!val)
            {
                #region InvalidDataFormatMessage
                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true }
                        ],
                        ContentTitle = "Импорт из .xlsx",
                        ContentHeader = "Уведомление",
                        ContentMessage = $"Не удалось импортировать данные из {SourceFile.FullName}." +
                                         $"{Environment.NewLine}Не соответствует формат данных!",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));
                #endregion
                return ;
            }


            //Загружаем список всех организаций входящих в состав Росатома

            int rowIndex = 2;
            bool rowIsEmpty =
                string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"A{rowIndex}"].Value))
                && string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"B{rowIndex}"].Value))
                && string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"C{rowIndex}"].Value))
                && string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"D{rowIndex}"].Value));

            while (!rowIsEmpty)
            {
                bool invalidRow =
                string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"A{rowIndex}"].Value))
                || string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"B{rowIndex}"].Value))
                || string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"C{rowIndex}"].Value))
                || string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"D{rowIndex}"].Value));

                if (!invalidRow)
                {
                    _organizationList.Add(new ViacOrganization()
                    {
                        RegNo = Convert.ToString(worksheet.Cells[$"A{rowIndex}"].Value),
                        OKPO = Convert.ToString(worksheet.Cells[$"B{rowIndex}"].Value),
                        ShortName = Convert.ToString(worksheet.Cells[$"C{rowIndex}"].Value),
                        Viac = Convert.ToString(worksheet.Cells[$"D{rowIndex}"].Value)
                    });



                }

                rowIndex++;
                rowIsEmpty =
                string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"A{rowIndex}"].Value))
                && string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"B{rowIndex}"].Value))
                && string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"C{rowIndex}"].Value))
                && string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[$"D{rowIndex}"].Value));
            }
        }

        public static async Task<string> ShowChooseViacMessage(Window owner)
        {
            var observableCollection = new ObservableCollection<string>();
            observableCollection.AddRange(ViacList);

            return await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var dialog = new AskViacMessage(observableCollection);
                return await dialog.ShowDialog<string?>(owner);
            });

        }
    }

    public class ViacOrganization
    {
        public string RegNo { get; set; }
        public string OKPO { get; set; }
        public string ShortName { get; set; }
        public string Viac { get; set; }
    }
}
