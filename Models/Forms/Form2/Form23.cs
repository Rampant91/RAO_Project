using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using Models.Collections;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.3: Разрешение на размещение РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form23 : Abstracts.Form2
    {
        public Form23() : base()
        {
            FormNum.Value = "2.3";
            //NumberOfFields.Value = 17;
            Validate_all();
        }
        private void Validate_all()
        {
            StoragePlaceName_Validation(StoragePlaceName);
            StoragePlaceCode_Validation(StoragePlaceCode);
            ProjectVolume_Validation(ProjectVolume);
            CodeRAO_Validation(CodeRAO);
            Volume_Validation(Volume);
            Mass_Validation(Mass);
            SummaryActivity_Validation(SummaryActivity);
            QuantityOZIII_Validation(QuantityOZIII);
            DocumentNumber_Validation(DocumentNumber);
            ExpirationDate_Validation(ExpirationDate);
            DocumentName_Validation(DocumentName);
            DocumentDate_Validation(DocumentDate);
        }

        [Attributes.Form_Property(true,"Форма")]
        public override bool Object_Validation()
        {
            return !(StoragePlaceName.HasErrors||
            StoragePlaceCode.HasErrors||
            ProjectVolume.HasErrors||
            CodeRAO.HasErrors||
            Volume.HasErrors||
            Mass.HasErrors||
            SummaryActivity.HasErrors||
            QuantityOZIII.HasErrors||
            DocumentNumber.HasErrors||
            ExpirationDate.HasErrors||
            DocumentName.HasErrors||
            DocumentDate.HasErrors);
        }

        //StoragePlaceName property
        #region  StoragePlaceName
        public string StoragePlaceName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Пункт хранения РАО","наименование","2")]
        public RamAccess<string> StoragePlaceName
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(StoragePlaceName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(StoragePlaceName)]).Value = StoragePlaceName_DB;
                    return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
                }
                else
                {
                    var rm = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB);
                    rm.PropertyChanged += StoragePlaceNameValueChanged;
                    Dictionary.Add(nameof(StoragePlaceName), rm);
                    return (RamAccess<string>)Dictionary[nameof(StoragePlaceName)];
                }
            }
            set
            {
                StoragePlaceName_DB = value.Value;
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation

        private void StoragePlaceNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                StoragePlaceName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            //List<string> spr = new List<string>();//here binds spr
            //if (!spr.Contains(value.Value))
            //{
            //    value.AddError("Недопустиое значение");
            //    return false;
            //}
            return true;
        }
        //StoragePlaceName property
        #endregion

        //StoragePlaceCode property
        #region  StoragePlaceCode
        public string StoragePlaceCode_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Пункт хранения РАО","код","3")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(StoragePlaceCode)))
                {
                    ((RamAccess<string>)Dictionary[nameof(StoragePlaceCode)]).Value = StoragePlaceCode_DB;
                    return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
                }
                else
                {
                    var rm = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB);
                    rm.PropertyChanged += StoragePlaceCodeValueChanged;
                    Dictionary.Add(nameof(StoragePlaceCode), rm);
                    return (RamAccess<string>)Dictionary[nameof(StoragePlaceCode)];
                }
            }
            set
            {
                StoragePlaceCode_DB = value.Value;
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation

        private void StoragePlaceCodeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                StoragePlaceCode_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("-"))
            {
                return true;
            }
            //List<string> spr = new List<string>();//here binds spr
            //if (!spr.Contains(value.Value))
            //{
            //    value.AddError("Недопустиое значение");
            //    return false;
            //}
            //return true;
            Regex a = new Regex("^[0-9]{8}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            var tmp = value.Value;
            if (tmp.Length == 8)
            {
                Regex a0 = new Regex("^[1-9]");
                if (!a0.IsMatch(tmp.Substring(0, 1)))
                {
                    value.AddError("Недопустимый вид пункта - " + tmp.Substring(0, 1));
                }
                Regex a1 = new Regex("^[1-3]");
                if (!a1.IsMatch(tmp.Substring(1, 1)))
                {
                    value.AddError("Недопустимое состояние пункта - " + tmp.Substring(1, 1));
                }
                Regex a2 = new Regex("^[1-2]");
                if (!a2.IsMatch(tmp.Substring(2, 1)))
                {
                    value.AddError("Недопустимая изоляция от окружающей среды - " + tmp.Substring(2, 1));
                }
                Regex a3 = new Regex("^[1-59]");
                if (!a3.IsMatch(tmp.Substring(3, 1)))
                {
                    value.AddError("Недопустимая зона нахождения пунтка - " + tmp.Substring(3, 1));
                }
                Regex a4 = new Regex("^[0-4]");
                if (!a4.IsMatch(tmp.Substring(4, 1)))
                {
                    value.AddError("Недопустимое значение пункта - " + tmp.Substring(4, 1));
                }
                Regex a5 = new Regex("^[1-49]");
                if (!a5.IsMatch(tmp.Substring(5, 1)))
                {
                    value.AddError("Недопустимое размещение пункта хранения относительно поверхности земли - " + tmp.Substring(5, 1));
                }
                Regex a67 = new Regex("^[1]{1}[1-9]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1]{1}|^[4]{1}[1-49]{1}|^[5]{1}[1-69]{1}|^[6]{1}[1]{1}|^[7]{1}[1349]{1}|^[8]{1}[1-69]{1}|^[9]{1}[9]{1}");
                if (!a67.IsMatch(tmp.Substring(6, 2)))
                {
                    value.AddError("Недопустимоый код типа РАО - " + tmp.Substring(6, 2));
                }
                if (value.HasErrors)
                {
                    return false;
                }
            }
            return true;
        }
        //StoragePlaceCode property
        #endregion

        //ProjectVolume property
        #region  ProjectVolume
        public string ProjectVolume_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Пункт хранения РАО","проектный объем, куб. м","4")]
        public RamAccess<string> ProjectVolume
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ProjectVolume)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ProjectVolume)]).Value = ProjectVolume_DB;
                    return (RamAccess<string>)Dictionary[nameof(ProjectVolume)];
                }
                else
                {
                    var rm = new RamAccess<string>(ProjectVolume_Validation, ProjectVolume_DB);
                    rm.PropertyChanged += ProjectVolumeValueChanged;
                    Dictionary.Add(nameof(ProjectVolume), rm);
                    return (RamAccess<string>)Dictionary[nameof(ProjectVolume)];
                }
            }
            set
            {
                ProjectVolume_DB = value.Value;
                OnPropertyChanged(nameof(ProjectVolume));
            }
        }


        private void ProjectVolumeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        ProjectVolume_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
                }
                ProjectVolume_DB = value1;
            }
        }
        private bool ProjectVolume_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //ProjectVolume property
        #endregion

        //CodeRAO property
        #region  CodeRAO
        public string CodeRAO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Разрешено к размещению", "код РАО","5")]
        public RamAccess<string> CodeRAO
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(CodeRAO)))
                {
                    ((RamAccess<string>)Dictionary[nameof(CodeRAO)]).Value = CodeRAO_DB;
                    return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
                }
                else
                {
                    var rm = new RamAccess<string>(CodeRAO_Validation, CodeRAO_DB);
                    rm.PropertyChanged += CodeRAOValueChanged;
                    Dictionary.Add(nameof(CodeRAO), rm);
                    return (RamAccess<string>)Dictionary[nameof(CodeRAO)];
                }
            }
            set
            {
                CodeRAO_DB = value.Value;
                OnPropertyChanged(nameof(CodeRAO));
            }
        }


        private void CodeRAOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value.ToLower();
                tmp = tmp.Replace("х", "x");
                CodeRAO_DB = tmp;
            }
        }
        private bool CodeRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var tmp = value.Value.ToLower();
            tmp = tmp.Replace("х", "x");
            Regex a = new Regex("^[0-9x+]{11}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            if (tmp.Length == 11)
            {
                Regex a0 = new Regex("^[1-3x+]");
                if (!a0.IsMatch(tmp.Substring(0, 1)))
                {
                    value.AddError("Недопустимое агрегатное состояние - " + tmp.Substring(0, 1));
                }
                Regex a1 = new Regex("^[0-49x+]");
                if (!a1.IsMatch(tmp.Substring(1, 1)))
                {
                    value.AddError("Недопустимое категория РАО - " + tmp.Substring(1, 1));
                }
                Regex a2 = new Regex("^[0-6x+]");
                if (!a2.IsMatch(tmp.Substring(2, 1)))
                {
                    value.AddError("Недопустимый радионуклидный состав РАО - " + tmp.Substring(2, 1));
                }
                Regex a3 = new Regex("^[12x+]");
                if (!a3.IsMatch(tmp.Substring(3, 1)))
                {
                    value.AddError("Недопустимое содержание ядерных материалов - " + tmp.Substring(3, 1));
                }
                Regex a4 = new Regex("^[12x+]");
                if (!a4.IsMatch(tmp.Substring(4, 1)))
                {
                    value.AddError("Недопустимоый период полураспада - " + tmp.Substring(4, 1));
                }
                Regex a5 = new Regex("^[0-3x+]");
                if (!a5.IsMatch(tmp.Substring(5, 1)))
                {
                    value.AddError("Недопустимоый период потенциальной опасности РАО - " + tmp.Substring(5, 1));
                }
                Regex a6 = new Regex("^[0-49x+]");
                if (!a6.IsMatch(tmp.Substring(6, 1)))
                {
                    value.AddError("Недопустимоый способ переработки - " + tmp.Substring(6, 1));
                }
                Regex a7 = new Regex("^[0-79x+]");
                if (!a7.IsMatch(tmp.Substring(7, 1)))
                {
                    value.AddError("Недопустимоый класс РАО - " + tmp.Substring(7, 1));
                }
                Regex a89 = new Regex("^[1]{1}[1-9]{1}|^[0]{1}[1]{1}|^[2]{1}[1-69]{1}|^[3]{1}[1-9]{1}|^[4]{1}[1-6]{1}|^[5]{1}[1-9]{1}|^[6]{1}[1-9]{1}|^[7]{1}[1-9]{1}|^[8]{1}[1-9]{1}|^[9]{1}[1-9]{1}");
                if (!a89.IsMatch(tmp.Substring(8, 2)))
                {
                    value.AddError("Недопустимоый код типа РАО - " + tmp.Substring(8, 2));
                }
                Regex a10 = new Regex("^[12x+]");
                if (!a7.IsMatch(tmp.Substring(10, 1)))
                {
                    value.AddError("Недопустимая горючесть - " + tmp.Substring(10, 1));
                }
                if (value.HasErrors)
                {
                    return false;
                }
            }
            return true;
        }
        //CodeRAO property
        #endregion

        //Volume property
        #region  Volume
        public string Volume_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property(true,"Разрешено к размещению", "объем, куб. м","6")]
        public RamAccess<string> Volume
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(Volume)))
                {
                    ((RamAccess<string>)Dictionary[nameof(Volume)]).Value = Volume_DB;
                    return (RamAccess<string>)Dictionary[nameof(Volume)];
                }
                else
                {
                    var rm = new RamAccess<string>(Volume_Validation, Volume_DB);
                    rm.PropertyChanged += VolumeValueChanged;
                    Dictionary.Add(nameof(Volume), rm);
                    return (RamAccess<string>)Dictionary[nameof(Volume)];
                }
            }
            set
            {
                Volume_DB = value.Value;
                OnPropertyChanged(nameof(Volume));
            }
        }


        private void VolumeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        Volume_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
                }
                Volume_DB = value1;
            }
        }
        private bool Volume_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Volume property
        #endregion

        //Mass Property
        #region  Mass
        public string Mass_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property(true,"Разрешено к размещению","масса, т","7")]
        public RamAccess<string> Mass
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(Mass)))
                {
                    ((RamAccess<string>)Dictionary[nameof(Mass)]).Value = Mass_DB;
                    return (RamAccess<string>)Dictionary[nameof(Mass)];
                }
                else
                {
                    var rm = new RamAccess<string>(Mass_Validation, Mass_DB);
                    rm.PropertyChanged += MassValueChanged;
                    Dictionary.Add(nameof(Mass), rm);
                    return (RamAccess<string>)Dictionary[nameof(Mass)];
                }
            }
            set
            {
                Mass_DB = value.Value;
                OnPropertyChanged(nameof(Mass));
            }
        }


        private void MassValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        Mass_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
                }
                Mass_DB = value1;
            }
        }
        private bool Mass_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                return true;
            }
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Mass Property
        #endregion

        //QuantityOZIII property
        #region  QuantityOZIII
        public string QuantityOZIII_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property(true,"Разрешено к размещению","количество ОЗИИИ, шт","8")]
        public RamAccess<string> QuantityOZIII
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(QuantityOZIII)))
                {
                    ((RamAccess<string>)Dictionary[nameof(QuantityOZIII)]).Value = QuantityOZIII_DB;
                    return (RamAccess<string>)Dictionary[nameof(QuantityOZIII)];
                }
                else
                {
                    var rm = new RamAccess<string>(QuantityOZIII_Validation, QuantityOZIII_DB);
                    rm.PropertyChanged += QuantityOZIIIValueChanged;
                    Dictionary.Add(nameof(QuantityOZIII), rm);
                    return (RamAccess<string>)Dictionary[nameof(QuantityOZIII)];
                }
            }
            set
            {
                QuantityOZIII_DB = value.Value;
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        // positive int.

        private void QuantityOZIIIValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                QuantityOZIII_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool QuantityOZIII_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                if (int.Parse(value.Value) <= 0)
                {
                    value.AddError("Число должно быть больше нуля");
                    return false;
                }
            }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityOZIII property
        #endregion

        //SummaryActivity property
        #region  SummaryActivity
        public string SummaryActivity_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property(true,"Разрешено к размещению","суммарная активность, Бк","9")]
        public RamAccess<string> SummaryActivity
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(SummaryActivity)))
                {
                    ((RamAccess<string>)Dictionary[nameof(SummaryActivity)]).Value = SummaryActivity_DB;
                    return (RamAccess<string>)Dictionary[nameof(SummaryActivity)];
                }
                else
                {
                    var rm = new RamAccess<string>(SummaryActivity_Validation, SummaryActivity_DB);
                    rm.PropertyChanged += SummaryActivityValueChanged;
                    Dictionary.Add(nameof(SummaryActivity), rm);
                    return (RamAccess<string>)Dictionary[nameof(SummaryActivity)];
                }
            }
            set
            {
                SummaryActivity_DB = value.Value;
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }


        private void SummaryActivityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                if (value1 != null)
                {
                    value1 = value1.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                    if (value1.Equals("-"))
                    {
                        SummaryActivity_DB = value1;
                        return;
                    }
                    if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                    {
                        value1 = value1.Replace("+", "e+").Replace("-", "e-");
                    }
                    try
                    {
                        var value2 = Convert.ToDouble(value1);
                        value1 = String.Format("{0:0.######################################################e+00}", value2);
                    }
                    catch (Exception ex)
                    { }
                }
                SummaryActivity_DB = value1;
            }
        }
        private bool SummaryActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if (value1.Equals("-"))
            {
                return true;
            }
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value1, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //SummaryActivity property
        #endregion

        //DocumentNumber property
        #region  DocumentNumber
        public string DocumentNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Наименование и реквизиты документа на размещение РАО","номер","10")]
        public RamAccess<string> DocumentNumber
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(DocumentNumber)))
                {
                    ((RamAccess<string>)Dictionary[nameof(DocumentNumber)]).Value = DocumentNumber_DB;
                    return (RamAccess<string>)Dictionary[nameof(DocumentNumber)];
                }
                else
                {
                    var rm = new RamAccess<string>(DocumentNumber_Validation, DocumentNumber_DB);
                    rm.PropertyChanged += DocumentNumberValueChanged;
                    Dictionary.Add(nameof(DocumentNumber), rm);
                    return (RamAccess<string>)Dictionary[nameof(DocumentNumber)];
                }
            }
            set
            {
                DocumentNumber_DB = value.Value;
                OnPropertyChanged(nameof(DocumentNumber));
            }
        }


        private void DocumentNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                DocumentNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool DocumentNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //DocumentNumber property
        #endregion

        //DocumentDate property
        #region DocumentDate 
        public string DocumentDate_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Наименование и реквизиты документа на размещение РАО","дата","11")]
        public RamAccess<string> DocumentDate
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(DocumentDate)))
                {
                    ((RamAccess<string>)Dictionary[nameof(DocumentDate)]).Value = DocumentDate_DB;
                    return (RamAccess<string>)Dictionary[nameof(DocumentDate)];
                }
                else
                {
                    var rm = new RamAccess<string>(DocumentDate_Validation, DocumentDate_DB);
                    rm.PropertyChanged += DocumentDateValueChanged;
                    Dictionary.Add(nameof(DocumentDate), rm);
                    return (RamAccess<string>)Dictionary[nameof(DocumentDate)];
                }
            }
            set
            {
                DocumentDate_DB = value.Value;
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
        //if change this change validation

        private void DocumentDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b1 = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b1.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                DocumentDate_DB = tmp;
            }
        }
        private bool DocumentDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var tmp = value.Value;
            Regex b1 = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b1.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //DocumentDate property
        #endregion

        //ExpirationDate property
        #region  ExpirationDate
        public string ExpirationDate_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Наименование и реквизиты документа на размещение РАО","срок действия","12")]
        public RamAccess<string> ExpirationDate
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(ExpirationDate)))
                {
                    ((RamAccess<string>)Dictionary[nameof(ExpirationDate)]).Value = ExpirationDate_DB;
                    return (RamAccess<string>)Dictionary[nameof(ExpirationDate)];
                }
                else
                {
                    var rm = new RamAccess<string>(ExpirationDate_Validation, ExpirationDate_DB);
                    rm.PropertyChanged += ExpirationDateValueChanged;
                    Dictionary.Add(nameof(ExpirationDate), rm);
                    return (RamAccess<string>)Dictionary[nameof(ExpirationDate)];
                }
            }
            set
            {
                ExpirationDate_DB = value.Value;
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }


        private void ExpirationDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b1 = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b1.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                ExpirationDate_DB = tmp;
            }
        }
        private bool ExpirationDate_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var tmp = value.Value;
            Regex b1 = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b1.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ExpirationDate property
        #endregion

        //DocumentName property
        #region  DocumentName
        public string DocumentName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Наименование и реквизиты документа на размещение РАО","наименование документа","13")]
        public RamAccess<string> DocumentName
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(DocumentName)))
                {
                    ((RamAccess<string>)Dictionary[nameof(DocumentName)]).Value = DocumentName_DB;
                    return (RamAccess<string>)Dictionary[nameof(DocumentName)];
                }
                else
                {
                    var rm = new RamAccess<string>(DocumentName_Validation, DocumentName_DB);
                    rm.PropertyChanged += DocumentNameValueChanged;
                    Dictionary.Add(nameof(DocumentName), rm);
                    return (RamAccess<string>)Dictionary[nameof(DocumentName)];
                }
            }
            set
            {
                DocumentName_DB = value.Value;
                OnPropertyChanged(nameof(DocumentName));
            }
        }


        private void DocumentNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                DocumentName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool DocumentName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //DocumentName property
        #endregion

        #region IExcel
        public override void ExcelGetRow(ExcelWorksheet worksheet, int Row)
        {
            throw new NotImplementedException();
        }
        public int ExcelRow(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = base.ExcelRow(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);
            double val;
            int valInt;

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = StoragePlaceName_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = StoragePlaceCode_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ProjectVolume_DB== "" || ProjectVolume_DB == "-" || ProjectVolume_DB == null ? 0  : double.TryParse(ProjectVolume_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : ProjectVolume_DB;
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = CodeRAO_DB;
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = Volume_DB== "" || Volume_DB == "-" || Volume_DB == null ? 0  : double.TryParse(Volume_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : Volume_DB;
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = Mass_DB== "" || Mass_DB == "-" || Mass_DB == null ? 0  : double.TryParse(Mass_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : Mass_DB;
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = QuantityOZIII_DB== "" || QuantityOZIII_DB == "-" || QuantityOZIII_DB == null ? 0  : int.TryParse(QuantityOZIII_DB.Replace("(", "").Replace(")", "").Replace(".", ","), out valInt) ? valInt : QuantityOZIII_DB;
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = SummaryActivity_DB== "" || SummaryActivity_DB == "-" || SummaryActivity_DB == null ? 0  : double.TryParse(SummaryActivity_DB.Replace("е", "E").Replace("(", "").Replace(")", "").Replace("Е", "E").Replace(".", ","), out val) ? val : SummaryActivity_DB;
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = DocumentNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = DocumentDate_DB;
            worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = ExpirationDate_DB;
            worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11 : 0)].Value = DocumentName_DB;
            return 12;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            var cnt = Form2.ExcelHeader(worksheet, Row, Column, Transpon);
            Column = Column + (Transpon == true ? cnt : 0);
            Row = Row + (Transpon == false ? cnt : 0);

            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(StoragePlaceName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(StoragePlaceCode)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(ProjectVolume)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 3 : 0), Column + (Transpon == true ? 3 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(CodeRAO)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 4 : 0), Column + (Transpon == true ? 4 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(Volume)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 5 : 0), Column + (Transpon == true ? 5 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(Mass)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 6 : 0), Column + (Transpon == true ? 6 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(QuantityOZIII)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 7 : 0), Column + (Transpon == true ? 7 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(SummaryActivity)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 8 : 0), Column + (Transpon == true ? 8 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(DocumentNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 9 : 0), Column + (Transpon == true ? 9 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(DocumentDate)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 10 : 0), Column + (Transpon == true ? 10 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(ExpirationDate)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            worksheet.Cells[Row + (Transpon == false ? 11 : 0), Column + (Transpon == true ? 11: 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form23,Models").GetProperty(nameof(DocumentName)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Names[1];
            return 12;
        }
        #endregion
        #region IDataGridColumn
        private static DataGridColumns _DataGridColumns { get; set; } = null;
        public override DataGridColumns GetColumnStructure(string param = "")
        {
            if (_DataGridColumns == null)
            {
                #region NumberInOrder (1)
                DataGridColumns NumberInOrderR = ((Attributes.Form_PropertyAttribute)typeof(Form).GetProperty(nameof(Form.NumberInOrder)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
                NumberInOrderR.SetSizeColToAllLevels(50);
                NumberInOrderR.Binding = nameof(Form.NumberInOrder);
                NumberInOrderR.Blocked = true;
                NumberInOrderR.ChooseLine = true;
                #endregion
                #region StoragePlaceName (2)
                DataGridColumns StoragePlaceNameR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.StoragePlaceName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                StoragePlaceNameR.SetSizeColToAllLevels(163);
                StoragePlaceNameR.Binding = nameof(Form23.StoragePlaceName);
                NumberInOrderR += StoragePlaceNameR;
                #endregion
                #region StoragePlaceCode (3)
                DataGridColumns StoragePlaceCodeR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.StoragePlaceCode)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                StoragePlaceCodeR.SetSizeColToAllLevels(88);
                StoragePlaceCodeR.Binding = nameof(Form23.StoragePlaceCode);
                NumberInOrderR += StoragePlaceCodeR;
                #endregion
                #region ProjectVolume (4)
                DataGridColumns ProjectVolumeR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.ProjectVolume)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                ProjectVolumeR.SetSizeColToAllLevels(133);
                ProjectVolumeR.Binding = nameof(Form23.ProjectVolume);
                NumberInOrderR += ProjectVolumeR;
                #endregion
                #region CodeRAO (5)
                DataGridColumns CodeRAOR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.CodeRAO)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                CodeRAOR.SetSizeColToAllLevels(88);
                CodeRAOR.Binding = nameof(Form23.CodeRAO);
                NumberInOrderR += CodeRAOR;
                #endregion
                #region Volume (6)
                DataGridColumns VolumeR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.Volume)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                VolumeR.SetSizeColToAllLevels(163);
                VolumeR.Binding = nameof(Form23.Volume);
                NumberInOrderR += VolumeR;
                #endregion
                #region Mass (7)
                DataGridColumns MassR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.Mass)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                MassR.SetSizeColToAllLevels(123);
                MassR.Binding = nameof(Form23.Mass);
                NumberInOrderR += MassR;
                #endregion
                #region QuantityOZIII (8)
                DataGridColumns QuantityOZIIIR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.QuantityOZIII)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                QuantityOZIIIR.SetSizeColToAllLevels(123);
                QuantityOZIIIR.Binding = nameof(Form23.QuantityOZIII);
                NumberInOrderR += QuantityOZIIIR;
                #endregion
                #region SummaryActivity (9)
                DataGridColumns SummaryActivityR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.SummaryActivity)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                SummaryActivityR.SetSizeColToAllLevels(163);
                SummaryActivityR.Binding = nameof(Form23.SummaryActivity);
                NumberInOrderR += SummaryActivityR;
                #endregion
                #region DocumentNumber (10)
                DataGridColumns DocumentNumberR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.DocumentNumber)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                DocumentNumberR.SetSizeColToAllLevels(103);
                DocumentNumberR.Binding = nameof(Form23.DocumentNumber);
                NumberInOrderR += DocumentNumberR;
                #endregion
                #region DocumentDate (11)
                DataGridColumns DocumentDateR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.DocumentDate)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                DocumentDateR.SetSizeColToAllLevels(88);
                DocumentDateR.Binding = nameof(Form23.DocumentDate);
                NumberInOrderR += DocumentDateR;
                #endregion
                #region ExpirationDate (12)
                DataGridColumns ExpirationDateR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.ExpirationDate)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                ExpirationDateR.SetSizeColToAllLevels(163);
                ExpirationDateR.Binding = nameof(Form23.ExpirationDate);
                NumberInOrderR += ExpirationDateR;
                #endregion
                #region DocumentName (13)
                DataGridColumns DocumentNameR = ((Attributes.Form_PropertyAttribute)typeof(Form23).GetProperty(nameof(Form23.DocumentName)).GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD(NumberInOrderR);
                DocumentNameR.SetSizeColToAllLevels(163);
                DocumentNameR.Binding = nameof(Form23.DocumentName);
                NumberInOrderR += DocumentNameR;
                #endregion
                _DataGridColumns = NumberInOrderR;
            }
            return _DataGridColumns;
        }
        #endregion
    }
}
