using Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public interface ICopiable
    {

        /// <summary>
        /// </summary>
        /// <returns>Возвращает строку с записанными данными в формате TSV(Tab-Separated Values) </returns>
        public string ConvertToTSVstring();
        public void PasteParsedTSVstring(string[] parsedTSVstring);

        public static string[][] ParseTSVstring(string pastedString)
        {
            // Универсальное разделение для Windows и Linux
            string[] rows;
            if (pastedString.Contains("\r\n"))
                rows = pastedString.Split("\r\n", StringSplitOptions.None);
            else if (pastedString.Contains('\n'))
                rows = pastedString.Split('\n', StringSplitOptions.None);
            else
                rows = [pastedString]; // Одна строка без переносов

            //Последняя строка пустая, поэтому выделяем память на одну ячейку меньше
            var parsedRows = new string[rows.Length - 1][];
            for (var i = 0; i < parsedRows.Length; i++)
            {
                parsedRows[i] = rows[i].Split('\t');
            }

            for (var i = 0; i < parsedRows.Length; i++)
            {
                for (var j = 0; j < parsedRows[i].Length; j++)
                {
                    var cell = parsedRows[i][j];
                    //Тримим каждую ячейку для проверки на кавычки
                    cell = cell.Trim();

                    // Убираем все пустые символы что были после кавычек
                    cell = cell.Trim();
                    parsedRows[i][j] = cell;

                }
            }
            return parsedRows;
        }
        
    }
}
