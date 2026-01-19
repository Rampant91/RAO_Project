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
    [Form_Class(name: "Форма 5.2: Сведения о закрытых радионуклидных источниках, полученных/переданных  подведомственными организациями сторонним организациям переведенных в радиоактивные отходы")]
    [Table(name: "form_52")]
    public class Form52 : Form
    {
        #region Constructor

        public Form52()
        {
            var x = this;
            FormNum.Value = "5.2";
        }

        #endregion

        #region Properties

        
        #region Category (2)

        public short? Category_DB { get; set; }

        [NotMapped]
        public RamAccess<short?> Category
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Category), out var value))
                {
                    ((RamAccess<short?>)value).Value = Category_DB;
                    return (RamAccess<short?>)value;
                }
                var rm = new RamAccess<short?>(Category_Validation, Category_DB);
                rm.PropertyChanged += Category_ValueChanged;
                Dictionary.Add(nameof(Category), rm);
                return (RamAccess<short?>)Dictionary[nameof(Category)];
            }//OK
            set
            {
                Category_DB = value.Value;
                OnPropertyChanged(nameof(Category));
            }
        }

        private void Category_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<short?>)value).Value;
            if (Category_DB != value1)
            {
                Category_DB = value1;
            }
        }

        private bool Category_Validation(RamAccess<short?> value)//TODO
        {
            value.ClearErrors();
            switch (value.Value)
            {
                case null:
                    value.AddError("Поле не заполнено");
                    return false;
                case < 1 or > 5:
                    value.AddError("Недопустимое значение");
                    return false;
                default:
                    return true;
            }
        }
        #endregion

        #region Radionuclids (3)

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string Radionuclids_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> Radionuclids
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Radionuclids), out var value))
                {
                    ((RamAccess<string>)value).Value = Radionuclids_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Radionuclids_Validation, Radionuclids_DB);
                rm.PropertyChanged += Radionuclids_ValueChanged;
                Dictionary.Add(nameof(Radionuclids), rm);
                return (RamAccess<string>)Dictionary[nameof(Radionuclids)];
            }
            set
            {
                Radionuclids_DB = ParseInnerText(value.Value);
                OnPropertyChanged();
            }
        }

        private void Radionuclids_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            value1 = value1.Length > 64
                ? value1[..64]
                : value1;
            if (Radionuclids_DB != value1)
            {
                Radionuclids_DB = value1;
            }
        }

        private bool Radionuclids_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region Quantity (4)

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

        #region Activity (5)

        [MaxLength(16)]
        [Column(TypeName = "varchar(16)")]
        public string Activity_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> Activity
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Activity), out var value))
                {
                    ((RamAccess<string>)value).Value = Activity_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Activity_Validation, Activity_DB);
                rm.PropertyChanged += Activity_ValueChanged;
                Dictionary.Add(nameof(Activity), rm);
                return (RamAccess<string>)Dictionary[nameof(Activity)];
            }
            set
            {
                Activity_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void Activity_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            value1 = value1.Length > 16
                ? value1[..16]
                : value1;
            if (Activity_DB != value1)
            {
                Activity_DB = value1;
            }
        }

        private bool Activity_Validation(RamAccess<string> value)
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
            return !(Category.HasErrors ||
                     Radionuclids.HasErrors ||
                     Quantity.HasErrors ||
                     Activity.HasErrors);
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

            Category_DB = short.TryParse(Convert.ToString(worksheet.Cells[row, 2].Value), out var shortValue) ? shortValue : null;

            Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 3].Value).Trim();
            if (Radionuclids_DB.Count() > 64)
                Radionuclids_DB = Radionuclids_DB[..64];

            Quantity_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 4].Value), out intValue) ? intValue : 0;

            Activity_DB = Convert.ToString(worksheet.Cells[row, 5].Value).Trim();
            if (Activity_DB.Count() > 16)
                Activity_DB = Activity_DB[..16];
        }

        public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
        {
            worksheet.Cells[row + 0, column + 0].Value = NumberInOrder_DB;
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = Category_DB == 0 ? "" : Category_DB;
            worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
            worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = Quantity_DB == 0 ? "" : Quantity_DB;
            worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelString(Activity_DB);

            return 5;
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
                $"{Category.Value}\t" +
                $"{Radionuclids.Value}\t" +
                $"{Quantity.Value}\t" +
                $"{Activity.Value}";
            return str;
        }

        #endregion
    }
}
