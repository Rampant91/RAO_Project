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
    [Form_Class(name: "Форма 5.3: Сведения о закрытых радионуклидных источниках, полученных/переданных  подведомственными организациями сторонним организациям переведенных в радиоактивные отходы")]
    [Table(name: "form_53")]
    public class Form53 : Form
    {
        #region Constructor

        public Form53()
        {
            var x = this;
            FormNum.Value = "5.3";
        }

        #endregion

        #region Properties

        #region OperationCode (2)

        public string OperationCode_DB { get; set; } = "";


        [MaxLength(2)]
        [Column(TypeName = "varchar(2)")]
        [NotMapped]
        public RamAccess<string> OperationCode
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(OperationCode), out var value))
                {
                    ((RamAccess<string>)value).Value = OperationCode_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(OperationCode_Validation, OperationCode_DB);
                rm.PropertyChanged += OperationCode_ValueChanged;
                Dictionary.Add(nameof(OperationCode), rm);
                return (RamAccess<string>)Dictionary[nameof(OperationCode)];
            }
            set
            {
                OperationCode_DB = ParseInnerText(value.Value);
                OnPropertyChanged();
            }
        }

        private void OperationCode_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            value1 = value1.Length > 2
                ? value1[..2]
                : value1;
            if (OperationCode_DB != value1)
            {
                OperationCode_DB = value1;
            }
        }

        private bool OperationCode_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region TypeORI (3)

        public string TypeORI_DB { get; set; } = "";


        [MaxLength(8)]
        [Column(TypeName = "varchar(8)")]
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
            value1 = value1.Length > 2
                ? value1[..2]
                : value1;
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

        #region VarietyORI (4)

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

        #region AggregateState (5)

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

        #region ProviderOrRecieverOKPO (6)

        [MaxLength(14)]
        [Column(TypeName = "varchar(14)")]
        public string ProviderOrRecieverOKPO_DB { get; set; } = "";

        [NotMapped]
        public RamAccess<string> ProviderOrRecieverOKPO
        {
            get
            {
                if (Dictionary.TryGetValue(nameof(ProviderOrRecieverOKPO), out var value))
                {
                    ((RamAccess<string>)value).Value = ProviderOrRecieverOKPO_DB;
                    return (RamAccess<string>)value;
                }
                var rm = new RamAccess<string>(ProviderOrRecieverOKPO_Validation, ProviderOrRecieverOKPO_DB);
                rm.PropertyChanged += ProviderOrRecieverOKPO_ValueChanged;
                Dictionary.Add(nameof(ProviderOrRecieverOKPO), rm);
                return (RamAccess<string>)Dictionary[nameof(ProviderOrRecieverOKPO)];
            }
            set
            {
                ProviderOrRecieverOKPO_DB = value.Value;
                OnPropertyChanged();
            }
        }

        private void ProviderOrRecieverOKPO_ValueChanged(object value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Value") return;
            var value1 = ((RamAccess<string>)value).Value;
            value1 = value1.Length > 14
                ? value1[..14]
                : value1;
            if (ProviderOrRecieverOKPO_DB != value1)
            {
                ProviderOrRecieverOKPO_DB = value1;
            }
        }

        private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region Radionuclids (7)

        [MaxLength(256)]
        [Column(TypeName = "varchar(256)")]
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
            value1 = value1.Length > 256
                ? value1[..256]
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

        #region Activity (8)

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

        #region Mass (9)

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
            value1 = value1.Length > 16
                ? value1[..16]
                : value1;
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

        #region Volume (10)

        [MaxLength(16)]
        [Column(TypeName = "varchar(16)")]
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
            value1 = value1.Length > 16
                ? value1[..16]
                : value1;
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

        #region Quantity (11)

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
            return !(OperationCode.HasErrors ||
                     TypeORI.HasErrors ||
                     VarietyORI.HasErrors ||
                     AggregateState.HasErrors ||
                     ProviderOrRecieverOKPO.HasErrors ||
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

            OperationCode_DB = Convert.ToString(worksheet.Cells[row, 2].Value).Trim();
            if (OperationCode_DB.Count() > 2)
                OperationCode_DB = OperationCode_DB[..2];

            TypeORI_DB = Convert.ToString(worksheet.Cells[row, 3].Value).Trim();
            if (TypeORI_DB.Count() > 8)
                TypeORI_DB = TypeORI_DB[..8];


            VarietyORI_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 4].Value), out var byteValue) ? byteValue : (byte)0;

            AggregateState_DB = byte.TryParse(Convert.ToString(worksheet.Cells[row, 5].Value), out byteValue) ? byteValue : (byte)0;

            ProviderOrRecieverOKPO_DB = Convert.ToString(worksheet.Cells[row, 6].Value).Trim();
            if (ProviderOrRecieverOKPO_DB.Count() > 14)
                ProviderOrRecieverOKPO_DB = ProviderOrRecieverOKPO_DB[..14];

            Radionuclids_DB = Convert.ToString(worksheet.Cells[row, 7].Value).Trim();
            if (Radionuclids_DB.Count() > 256)
                Radionuclids_DB = Radionuclids_DB[..256];

            Activity_DB = Convert.ToString(worksheet.Cells[row, 8].Value).Trim();
            if (Activity_DB.Count() > 16)
                Activity_DB = Activity_DB[..16];

            Mass_DB = Convert.ToString(worksheet.Cells[row, 9].Value).Trim();
            if (Mass_DB.Count() > 16)
                Mass_DB = Mass_DB[..16];

            Volume_DB = Convert.ToString(worksheet.Cells[row, 10].Value).Trim();
            if (Activity_DB.Count() > 16)
                Activity_DB = Activity_DB[..16];

            Quantity_DB = int.TryParse(Convert.ToString(worksheet.Cells[row, 11].Value), out intValue) ? intValue : 0;
        }

        public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
        {
            worksheet.Cells[row + 0, column + 0].Value = NumberInOrder_DB;
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ConvertToExcelString(OperationCode_DB);
            worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ConvertToExcelString(TypeORI_DB);
            worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = VarietyORI_DB == 0 ? "" : VarietyORI_DB;
            worksheet.Cells[row + (!transpose ? 4 : 0), column + (transpose ? 4 : 0)].Value = AggregateState_DB == 0 ? "" : AggregateState_DB;
            worksheet.Cells[row + (!transpose ? 5 : 0), column + (transpose ? 5 : 0)].Value = ConvertToExcelString(ProviderOrRecieverOKPO_DB);
            worksheet.Cells[row + (!transpose ? 6 : 0), column + (transpose ? 6 : 0)].Value = ConvertToExcelString(Radionuclids_DB);
            worksheet.Cells[row + (!transpose ? 7 : 0), column + (transpose ? 7 : 0)].Value = ConvertToExcelString(Activity_DB);
            worksheet.Cells[row + (!transpose ? 8 : 0), column + (transpose ? 8 : 0)].Value = ConvertToExcelString(Mass_DB);
            worksheet.Cells[row + (!transpose ? 9 : 0), column + (transpose ? 9 : 0)].Value = ConvertToExcelString(Volume_DB);
            worksheet.Cells[row + (!transpose ? 10 : 0), column + (transpose ? 10 : 0)].Value = Quantity_DB == 0 ? "" : Quantity_DB;

            return 11;
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
                $"{OperationCode.Value}\t" +
                $"{TypeORI.Value}\t" +
                $"{VarietyORI.Value}\t" +
                $"{AggregateState.Value}\t" +
                $"{ProviderOrRecieverOKPO.Value}\t" +
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
