using Models.DTO;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace Client_App.Service
{
    public class ExcelParser
    {
        /// <summary>
        /// Парсит данные о радионуклидах из указанного Excel-файла
        /// </summary>
        /// <param name="filePath">Путь к Excel-файлу</param>
        public static List<RadionuclidDTO> ParseRadionuclides(string filePath)
        {
            var radionuclides = new List<RadionuclidDTO>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Получение первого листа
                if (worksheet?.Dimension == null) return radionuclides; // Если лист пустой, возвращаем пустой список

                // Создаем список для хранения данных текущего листа
                var localList = new List<RadionuclidDTO>();

                // Обработка строк на листе (начиная со второй строки, так как первая — заголовки)
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var name = worksheet.Cells[row, 1].Text?.Trim(); // Чтение имени радионуклида из первой колонки
                    var codeNumber = worksheet.Cells[row, 4].Text?.Trim(); // Чтение кодового номера из четвертой колонки

                    // Пропускаем строки с пустыми значениями
                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(codeNumber))
                        continue;

                    var halflife = worksheet.Cells[row, 5].Text?.Trim(); // Чтение периода полураспада из пятой колонки
                    var unit = worksheet.Cells[row, 6].Text?.Trim(); // Чтение единицы измерения из шестой колонки

                    // Создание объекта RadionuclidDTO и добавление в локальный список
                    var radionuclid = new RadionuclidDTO
                    {
                        Name = name,
                        CodeNumber = codeNumber,
                        HalfLife = halflife,
                        Unit = unit
                    };

                    localList.Add(radionuclid);
                }

                // Добавляем все элементы из временного списка в основной
                radionuclides.AddRange(localList);
            }

            return radionuclides;
        }
    }
}