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
    [Form_Class(name: "Форма 5.4: Сведения о закрытых радионуклидных источниках, полученных/переданных  подведомственными организациями сторонним организациям переведенных в радиоактивные отходы")]
    [Table(name: "form_54")]
    public class Form54 : Form
    {
        #region Constructor

        public Form54()
        {
            var x = this;
            FormNum.Value = "5.4";
        }

        #endregion

        #region Properties

        #region TypeORI (2)

        public string TypeORI_DB { get; set; } = "";


        [NotMapped]
        public RamAccess<string> TypeORI
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(TypeORI), out var value))
                {
                    ((RamAccess<string>)value).Value = TypeORI_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(TypeORI_Validation, TypeORI_DB);
                rm.PropertyChanged += TypeORI_ValueChanged;
                Dictionary.Add(nameof(TypeORI), rm);
                return (RamAccess<string>)Dictionary[nameof(TypeORI)];
            }
            set
            {
                TypeORI_DB = ParseInnerText(value.Value);
                OnPropertyChanged();
            }
        }

        private void TypeORI_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;

            if (TypeORI_DB != value1)
            {
                TypeORI_DB = value1;
            }
        }

        private bool TypeORI_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region VarietyORI (3)

        public byte? VarietyORI_DB { get; set; }

        [NotMapped]
        public RamAccess<byte?> VarietyORI
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(VarietyORI), out var value))
                {
                    ((RamAccess<byte?>)value).Value = VarietyORI_DB;
                    return (RamAccess<byte?>)value;
                }
                var rm = new RamAccess<byte?>(VarietyORI_Validation, VarietyORI_DB);
                rm.PropertyChanged += VarietyORI_ValueChanged;
                Dictionary.Add(nameof(VarietyORI), rm);
                return (RamAccess<byte?>)Dictionary[nameof(VarietyORI)];
            }//OK
            set
            {
                VarietyORI_DB = value.Value;
                OnPropertyChanged(nameof(VarietyORI));
            }
        }

        private void VarietyORI_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;

            var value1 = ((RamAccess<byte?>)value).Value;
            if (VarietyORI_DB != value1)
            {
                VarietyORI_DB = value1;
            }
        }

        private bool VarietyORI_Validation(RamAccess<byte?> value)//TODO
        {
            value.ClearErrors();
            switch (value.Value)
            {
                case null:
                    value.AddError("Поле не заполнено");
                    return false;
                case < 4 or > 12:
                    value.AddError("Недопустимое значение");
                    return false;
                default:
                    return true;
            }
        }
        #endregion

        #region AggregateState (4)

        public byte? AggregateState_DB { get; set; }

        [NotMapped]
        [FormProperty(true, "Сведения из паспорта на открытый радионуклидный источник", "агрегатное состояние", "12")]
        public RamAccess<byte?> AggregateState//1 2 3
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(AggregateState), out var value))
                {
                    ((RamAccess<byte?>)value).Value = AggregateState_DB;
                    return (RamAccess<byte?>)value;
                }
                var rm = new RamAccess<byte?>(AggregateState_Validation, AggregateState_DB);
                rm.PropertyChanged += AggregateState_ValueChanged;
                Dictionary.Add(nameof(AggregateState), rm);
                return (RamAccess<byte?>)Dictionary[nameof(AggregateState)];
            }
            set
            {
                AggregateState_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void AggregateState_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            AggregateState_DB = ((RamAccess<byte?>)value).Value;
        }

        private static bool AggregateState_Validation(RamAccess<byte?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value is not (1 or 2 or 3))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }

        #endregion

        #region Radionuclids (5)

        [MaxLength(1024)]
        [Column(TypeName = "varchar(1024)")]
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

            if (value1 != null)
            {
                value1 = value1.Length > 1024
                    ? value1[..1024]
                    : value1;
            }

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

        #region Activity (6)

        [MaxLength(32)]
        [Column(TypeName = "varchar(32)")]
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
            if (value1 != null)
            {
                Activity_DB = ExponentialString_ValueChanged(value1);
            }
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

        #region Mass (7)

        [MaxLength(32)]
        [Column(TypeName = "varchar(32)")]
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
                Mass_DB = ExponentialString_ValueChanged(value1);
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

        #region Volume (8)

        [MaxLength(32)]
        [Column(TypeName = "varchar(32)")]
        public string Volume_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> Volume
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(Volume), out var value))
                {
                    ((RamAccess<string>)value).Value = Volume_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(Volume_Validation, Volume_DB);
                rm.PropertyChanged += Volume_ValueChanged;
                Dictionary.Add(nameof(Volume), rm);
                return (RamAccess<string>)Dictionary[nameof(Volume)];
            }
            set
            {
                Volume_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void Volume_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            if (value1 != null)
            {
                Volume_DB = ExponentialString_ValueChanged(value1);
            }
            if (Volume_DB != value1)
            {
                Volume_DB = value1;
            }
        }

        private bool Volume_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region Quantity (9)

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
            return !(TypeORI.HasErrors ||
                     VarietyORI.HasErrors ||
                     AggregateState.HasErrors ||
                     Radionuclids.HasErrors ||
                     Activity.HasErrors ||
                     Mass.HasErrors ||
                     Volume.HasErrors ||
                     Quantity.HasErrors);
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

            TypeORI_DB = Convert.ToString(worksheet.Cells[row, 2].Value).Trim();


            VarietyORI_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 3].Value), out var byteValue) ? byteValue : (byte)0;

            AggregateState_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 4].Value), out byteValue) ? byteValue : (byte)0;

            Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 5].Value).Trim();
            if (Radionuclids_DB.Count() > 1024)
                Radionuclids_DB = Radionuclids_DB[..1024];

            Activity_DB = ConvertFromExcelDouble(worksheet.Cells[row, 6].Value);
            if (Activity_DB.Count() > 32)
                Activity_DB = Activity_DB[..32];

            Mass_DB = ConvertFromExcelDouble(worksheet.Cells[row, 7].Value);
            if (Mass_DB.Count() > 32)
                Mass_DB = Mass_DB[..32];

            Volume_DB = ConvertFromExcelDouble(worksheet.Cells[row, 8].Value);
            if (Volume_DB.Count() > 32)
                Volume_DB = Volume_DB[..32];

            Quantity_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 9].Value), out intValue) ? intValue : 0;
        }

        public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
        {
            worksheet.Cells[row + 0, column + 0].Value = NumberInOrder_DB;
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(TypeORI_DB);
            worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = VarietyORI_DB == 0 ? "" : VarietyORI_DB;
            worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = AggregateState_DB == 0 ? "" : AggregateState_DB;
            worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
            worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelDouble(Activity_DB);
            worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelDouble(Mass_DB);
            worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelDouble(Volume_DB);
            worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = Quantity_DB == 0 ? "" : Quantity_DB;

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
                $"{TypeORI.Value}\t" +
                $"{VarietyORI.Value}\t" +
                $"{AggregateState.Value}\t" +
                $"{Radionuclids.Value}\t" +
                $"{Activity.Value}\t" +
                $"{Mass.Value}\t" +
                $"{Volume.Value}\t" +
                $"{Quantity.Value}";
            return str;
        }

        #endregion
    }
}
