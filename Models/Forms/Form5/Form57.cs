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
    [Form_Class(name: "Форма 5.7: Сведения о закрытых радионуклидных источниках, полученных/переданных  подведомственными организациями сторонним организациям переведенных в радиоактивные отходы")]
    [Table(name: "form_57")]
    public class Form57 : Form
    {
        #region Constructor

        public Form57()
        {
            var x = this;
            FormNum.Value = "5.7";
        }

        #endregion

        #region Properties

        #region RegNo (2)

        [MaxLength(5)]
        [Column(TypeName = "varchar(5)")]
        public string RegNo_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> RegNo
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(RegNo), out var value))
                {
                    ((RamAccess<string>)value).Value = RegNo_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(RegNo_Validation, RegNo_DB);
                rm.PropertyChanged += RegNo_ValueChanged;
                Dictionary.Add(nameof(RegNo), rm);
                return (RamAccess<string>)Dictionary[nameof(RegNo)];
            }
            set
            {
                RegNo_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void RegNo_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            value1 = value1.Length > 5
                ? value1[..5]
                : value1;
            if (RegNo_DB != value1)
            {
                RegNo_DB = value1;
            }
        }

        private bool RegNo_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region OKPO (3)

        [MaxLength(14)]
        [Column(TypeName = "varchar(14)")]
        public string OKPO_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> OKPO
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(OKPO), out var value))
                {
                    ((RamAccess<string>)value).Value = OKPO_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(OKPO_Validation, OKPO_DB);
                rm.PropertyChanged += OKPO_ValueChanged;
                Dictionary.Add(nameof(OKPO), rm);
                return (RamAccess<string>)Dictionary[nameof(OKPO)];
            }
            set
            {
                OKPO_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void OKPO_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            value1 = value1.Length > 14
                ? value1[..14]
                : value1;
            if (OKPO_DB != value1)
            {
                OKPO_DB = value1;
            }
        }

        private bool OKPO_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region Name (4)

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
            value1 = value1.Length > 64
                ? value1[..64]
                : value1;
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

        #region Recognizance (5)

        [MaxLength(256)]
        [Column(TypeName = "varchar(256)")]
        public string Recognizance_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> Recognizance
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Recognizance), out var value))
                {
                    ((RamAccess<string>)value).Value = Recognizance_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Recognizance_Validation, Recognizance_DB);
                rm.PropertyChanged += Recognizance_ValueChanged;
                Dictionary.Add(nameof(Recognizance), rm);
                return (RamAccess<string>)Dictionary[nameof(Recognizance)];
            }
            set
            {
                Recognizance_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void Recognizance_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            value1 = value1.Length > 256
                ? value1[..256]
                : value1;
            if (Recognizance_DB != value1)
            {
                Recognizance_DB = value1;
            }
        }

        private bool Recognizance_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region License (6)

        [MaxLength(256)]
        [Column(TypeName = "varchar(256)")]
        public string License_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> License
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(License), out var value))
                {
                    ((RamAccess<string>)value).Value = License_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(License_Validation, License_DB);
                rm.PropertyChanged += License_ValueChanged;
                Dictionary.Add(nameof(License), rm);
                return (RamAccess<string>)Dictionary[nameof(License)];
            }
            set
            {
                License_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void License_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            value1 = value1.Length > 256
                ? value1[..256]
                : value1;
            if (License_DB != value1)
            {
                License_DB = value1;
            }
        }

        private bool License_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region Practice (7)

        public string Practice_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> Practice
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Practice), out var value))
                {
                    ((RamAccess<string>)value).Value = Practice_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Practice_Validation, Practice_DB);
                rm.PropertyChanged += Practice_ValueChanged;
                Dictionary.Add(nameof(Practice), rm);
                return (RamAccess<string>)Dictionary[nameof(Practice)];
            }
            set
            {
                Practice_DB = ParseInnerText(value.Value);
                OnPropertyChanged();
            }
        }

        private void Practice_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            if (Practice_DB != value1)
            {
                Practice_DB = value1;
                OnPropertyChanged(nameof(RowColor));
            }
        }

        private bool Practice_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region Note (8)

        public string Note_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> Note
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Note), out var value))
                {
                    ((RamAccess<string>)value).Value = Note_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Note_Validation, Note_DB);
                rm.PropertyChanged += Note_ValueChanged;
                Dictionary.Add(nameof(Note), rm);
                return (RamAccess<string>)Dictionary[nameof(Note)];
            }
            set
            {
                Note_DB = ParseInnerText(value.Value);
                OnPropertyChanged();
            }
        }

        private void Note_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            if (Note_DB != value1)
            {
                Note_DB = value1;
                OnPropertyChanged(nameof(RowColor));
            }
        }

        private bool Note_Validation(RamAccess<string> value)
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
            return !(RegNo.HasErrors || 
                     OKPO.HasErrors ||
                     Name.HasErrors ||
                     Recognizance.HasErrors ||
                     License.HasErrors ||
                     Practice.HasErrors ||
                     Note.HasErrors);
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

            RegNo_DB = Convert.ToString(worksheet.Cells[row, 2].Value).Trim();
            if (RegNo_DB.Count() > 5)
                RegNo_DB = RegNo_DB[..5];

            OKPO_DB = Convert.ToString(worksheet.Cells[row, 2].Value).Trim();
            if (OKPO_DB.Count() > 14)
                OKPO_DB = OKPO_DB[..14];

            Name_DB = Convert.ToString(worksheet.Cells[row, 2].Value).Trim();
            if (Name_DB.Count() > 64)
                Name_DB = Name_DB[..64];

            Recognizance_DB = Convert.ToString(worksheet.Cells[row, 2].Value).Trim();
            if (Recognizance_DB.Count() > 256)
                Recognizance_DB = Recognizance_DB[..256];

            License_DB = Convert.ToString(worksheet.Cells[row, 2].Value).Trim();
            if (License_DB.Count() > 256)
                License_DB = License_DB[..256];

            Practice_DB = Convert.ToString(worksheet.Cells[row, 2].Value).Trim();

            Note_DB = Convert.ToString(worksheet.Cells[row, 2].Value).Trim();

        }

        public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
        {
            worksheet.Cells[row + 0, column + 0].Value = NumberInOrder_DB;
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(RegNo_DB);
            worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(OKPO_DB);
            worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = ConvertToExcelString(Name_DB);
            worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelString(Recognizance_DB);
            worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelString(License_DB);
            worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelString(Practice_DB);
            worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelString(Note_DB);

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
                $"{RegNo.Value}\t" +
                $"{OKPO.Value}\t" +
                $"{Name.Value}\t" +
                $"{Recognizance.Value}\t" +
                $"{License.Value}\t" +
                $"{Practice.Value}\t" +
                $"{Note.Value}";
            return str;
        }

        #endregion
    }
}
