using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Forms.Form5
{
    [Serializable]
    [Form_Class(name: "Форма 5.6: Сведения о закрытых радионуклидных источниках, полученных/переданных  подведомственными организациями сторонним организациям переведенных в радиоактивные отходы")]
    [Table(name: "form_56")]
    public class Form56 : Form
    {
        #region Constructor

        public Form56()
        {
            var x = this;
            FormNum.Value = "5.6";
        }

        #endregion

        #region Properties

        #region Name (2)

        public string Name_DB { get; set; } = "";


        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        [NotMapped]
        public RamAccess<string> Name
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Name), out var value))
                {
                    ((RamAccess<string>)value).Value = Name_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Name_Validation, Name_DB);
                rm.PropertyChanged += Name_ValueChanged;
                Dictionary.Add(nameof(Name), rm);
                return (RamAccess<string>)Dictionary[nameof(Name)];
            }
            set
            {
                Name_DB = ParseInnerText(value.Value);
                OnPropertyChanged();
            }
        }

        private void Name_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            if (value1 != null)
            {
                value1 = value1.Length > 64
                ? value1[..64]
                : value1;
            }
            if (Name_DB != value1)
            {
                Name_DB = value1;
            }
        }

        private bool Name_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region Quantity (3)

        public int? Quantity_DB { get; set; } = 0;

        [NotMapped]
        public RamAccess<int?> Quantity
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Quantity), out var value))
                {
                    ((RamAccess<int?>)value).Value = Quantity_DB;
                    return (RamAccess<int?>)value;
                }
                var rm = new RamAccess<int?>(Quantity_Validation, Quantity_DB);
                rm.PropertyChanged += Quantity_ValueChanged;
                Dictionary.Add(nameof(Quantity), rm);
                return (RamAccess<int?>)Dictionary[nameof(Quantity)];
            }
            set
            {
                Quantity_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void Quantity_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<int?>)value).Value;
            if (Quantity_DB != value1)
            {
                Quantity_DB = value1;
            }
        }

        private bool Quantity_Validation(RamAccess<int?> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region Mass (4)

        [MaxLength(16)]
        [Column(TypeName = "varchar(16)")]
        public string Mass_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> Mass
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Mass), out var value))
                {
                    ((RamAccess<string>)value).Value = Mass_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Mass_Validation, Mass_DB);
                rm.PropertyChanged += Mass_ValueChanged;
                Dictionary.Add(nameof(Mass), rm);
                return (RamAccess<string>)Dictionary[nameof(Mass)];
            }
            set
            {
                Mass_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void Mass_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            if (value1 != null)
            {
                value1 = value1.Length > 16
                ? value1[..16]
                : value1;
            }
            if (Mass_DB != value1)
            {
                Mass_DB = value1;
            }
        }

        private bool Mass_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region RowColor

        [NotMapped]
        public Color RowColor
        {
            get
            {
                return Color.FromArgb(0, 255, 255, 255); // Значение по умолчанию
            }
        }

        [NotMapped]
        private string _toolTipText = "";

        [NotMapped]
        public string ToolTipText
        {
            get
            {
                return _toolTipText;
            }
            set
            {
                _toolTipText = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion

        #region Validation

        public override bool Object_Validation()
        {
            return !(Name.HasErrors ||
                     Quantity.HasErrors ||
                     Mass.HasErrors);
        }

        #endregion

        #region ParseInnerText
        private static string ParseInnerText(string text)
        {
            return text.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
        }
        #endregion

        #region IExcel

        public override void ExcelGetRow(ExcelWorksheet worksheet, int row)
        {
            NumberInOrder_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 1].Value), out var intValue)
                ? intValue
                : 0;

            Name_DB = Convert.ToString(worksheet.Cells[row, 2].Value).Trim();
            if (Name_DB.Count() > 64)
                Name_DB = Name_DB[..64];

            Quantity_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 3].Value), out intValue) ? intValue : 0;

            Mass_DB = Convert.ToString(worksheet.Cells[row, 4].Value).Trim();
            if (Mass_DB.Count() > 16)
                Mass_DB = Mass_DB[..16];
        }

        public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
        {
            worksheet.Cells[row + 0, column + 0].Value = NumberInOrder_DB;
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(Name_DB);
            worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = Quantity_DB == 0 ? "" : Quantity_DB;
            worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelString(Mass_DB);

            return 9;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string id = "")
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDataGridColumn

        // Заглушка
        public override DataGridColumns GetColumnStructure(string param)
        {
            return null;
        }

        #endregion

        #region ConvertToTSVstring

        /// <summary>
        /// </summary>
        /// <returns>Возвращает строку с записанными данными в формате TSV(Tab-Separated Values) </returns>
        public override string ConvertToTSVstring()
        {
            // Создаем текстовое представление (TSV - tab-separated values)
            var str =
                $"{NumberInOrder.Value}\t" +
                $"{Name.Value}\t" +
                $"{Quantity.Value}\t" +
                $"{Mass.Value}";
            return str;
        }

        #endregion
    }
}
