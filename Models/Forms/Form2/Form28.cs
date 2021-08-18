using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Globalization;
using Spravochniki;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.8: Отведение сточных вод, содержащих радионуклиды")]
    public class Form28 : Abstracts.Form2
    {
        public Form28() : base()
        {
            FormNum.Value = "2.8";
            //NumberOfFields.Value = 24;
            Validate_all();
        }

        private void Validate_all()
        {
            PermissionNumber2_Validation(PermissionNumber2);
            PermissionIssueDate2_Validation(PermissionIssueDate2);
            ValidBegin2_Validation(ValidBegin2);
            ValidThru2_Validation(ValidThru2);
            PermissionDocumentName2_Validation(PermissionDocumentName2);
            PermissionNumber_Validation(PermissionNumber);
            PermissionIssueDate_Validation(PermissionIssueDate);
            ValidBegin_Validation(ValidBegin);
            ValidThru_Validation(ValidThru);
            PermissionDocumentName_Validation(PermissionDocumentName);
            PermissionNumber1_Validation(PermissionNumber1);
            PermissionIssueDate1_Validation(PermissionIssueDate1);
            ValidBegin1_Validation(ValidBegin1);
            ValidThru1_Validation(ValidThru1);
            PermissionDocumentName1_Validation(PermissionDocumentName1);
            WasteSourceName_Validation(WasteSourceName);
            WasteRecieverName_Validation(WasteRecieverName);
            RecieverTypeCode_Validation(RecieverTypeCode);
            AllowedWasteRemovalVolume_Validation(AllowedWasteRemovalVolume);
            RemovedWasteVolume_Validation(RemovedWasteVolume);
            PoolDistrictName_Validation(PoolDistrictName);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PermissionNumber property
#region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PermissionNumber_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PermissionNumber_DB = value.Value;
                }
                OnPropertyChanged(nameof(PermissionNumber));
            }
        }


        private bool PermissionNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        //PermissionNumber property
        #endregion

        //PermissionIssueDate property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PermissionIssueDate_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PermissionIssueDate_DB = value.Value;
                }
                OnPropertyChanged(nameof(PermissionIssueDate));
            }
        }


        private bool PermissionIssueDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionIssueDate property
        #endregion

        //PermissionDocumentName property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PermissionDocumentName_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PermissionDocumentName_DB = value.Value;
                }
                OnPropertyChanged(nameof(PermissionDocumentName));
            }
        }


        private bool PermissionDocumentName_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionDocumentName property
        #endregion

        //ValidBegin property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Действует с")]
        public RamAccess<string> ValidBegin
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(ValidBegin_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    ValidBegin_DB = value.Value;
                }
                OnPropertyChanged(nameof(ValidBegin));
            }
        }


        private bool ValidBegin_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidBegin property
        #endregion

        //ValidThru property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Действует по")]
        public RamAccess<string> ValidThru
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(ValidThru_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    ValidThru_DB = value.Value;
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }


        private bool ValidThru_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidThru property
        #endregion

        //PermissionNumber1 property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber1
        {
            get => new RamAccess<string>(PermissionNumber1_Validation, _DB);
            set
            {
                PermissionNumber1_DB = value.Value;
                OnPropertyChanged(nameof(PermissionNumber1));
            }
        }


        private bool PermissionNumber1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionNumber1 property
        #endregion

        //PermissionIssueDate1 property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate1
        {
            get
            {
                {
                    var tmp = new RamAccess<string>(PermissionIssueDate1_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {
                PermissionIssueDate1_DB = value.Value;
                OnPropertyChanged(nameof(PermissionIssueDate1));
            }
        }


        private bool PermissionIssueDate1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionIssueDate1 property
        #endregion

        //PermissionDocumentName1 property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName1
        {
            get
            {
                {
                    var tmp = new RamAccess<string>(PermissionDocumentName1_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {
                PermissionDocumentName1_DB = value.Value;
                OnPropertyChanged(nameof(PermissionDocumentName1));
            }
        }


        private bool PermissionDocumentName1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionDocumentName1 property
        #endregion

        //ValidBegin1 property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Действует с")]
        public RamAccess<string> ValidBegin1
        {
            get
            {
                {
                    var tmp = new RamAccess<string>(ValidBegin1_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {
                ValidBegin1_DB = value.Value;
                OnPropertyChanged(nameof(ValidBegin1));
            }
        }


        private bool ValidBegin1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidBegin1 property
        #endregion

        //ValidThru1 property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Действует по")]
        public RamAccess<string> ValidThru1
        {
            get => new RamAccess<string>(ValidThru1_Validation, _DB);
            set
            {
                ValidThru1_DB = value.Value;
                OnPropertyChanged(nameof(ValidThru1));
            }
        }


        private bool ValidThru1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidThru1 property
        #endregion

        //PermissionNumber2 property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber2
        {
            get => new RamAccess<string>(PermissionNumber2_Validation, _DB);
            set
            {
                PermissionNumber2_DB = value.Value;
                OnPropertyChanged(nameof(PermissionNumber2));
            }
        }


        private bool PermissionNumber2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionNumber2 property
        #endregion

        //PermissionIssueDate2 property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate2
        {
            get
            {
                {
                    var tmp = new RamAccess<string>(PermissionIssueDate2_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {
                PermissionIssueDate2_DB = value.Value;
                OnPropertyChanged(nameof(PermissionIssueDate2));
            }
        }


        private bool PermissionIssueDate2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionIssueDate property
        #endregion

        //PermissionDocumentName2 property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName2
        {
            get
            {
                {
                    var tmp = new RamAccess<string>(PermissionDocumentName2_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PermissionDocumentName2_DB = value.Value;
                }
                OnPropertyChanged(nameof(PermissionDocumentName2));
            }
        }


        private bool PermissionDocumentName2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionDocumentName2 property
        #endregion

        //ValidBegin2 property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Действует с")]
        public RamAccess<string> ValidBegin2
        {
            get => new RamAccess<string>(ValidBegin2_Validation, _DB);
            set
            {
                ValidBegin2_DB = value.Value;
                OnPropertyChanged(nameof(ValidBegin2));
            }
        }


        private bool ValidBegin2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidBegin2 property
        #endregion

        //ValidThru2 property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Действует по")]
        public RamAccess<string> ValidThru2
        {
            get
            {
                {
                    var tmp = new RamAccess<string>(ValidThru2_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {
                ValidThru2_DB = value.Value;
                OnPropertyChanged(nameof(ValidThru2));
            }
        }


        private bool ValidThru2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidThru2 property
        #endregion

        //WasteSourceName property
        #region WasteSourceName
        public string WasteSourceName_DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Наименование, номер выпуска сточных вод")]
        public RamAccess<string> WasteSourceName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(WasteSourceName_Validation, WasteSourceName_DB);
                    tmp.PropertyChanged += WasteSourceNameValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    WasteSourceName_DB = value.Value;
                }
                OnPropertyChanged(nameof(WasteSourceName));
            }
        }


        private bool WasteSourceName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //WasteSourceName property
        #endregion

        //WasteRecieverName property
        #region WasteRecieverName
        public string WasteRecieverName_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Наименование приемника отведенных вод")]
        public RamAccess<string> WasteRecieverName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(WasteRecieverName_Validation, WasteRecieverName_DB);
                    tmp.PropertyChanged += WasteRecieverNameValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    WasteRecieverName_DB = value.Value;
                }
                OnPropertyChanged(nameof(WasteRecieverName));
            }
        }


        private bool WasteRecieverName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //WasteRecieverName property
        #endregion

        //RecieverTypeCode property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Код типа приемника отведенных вод")]
        public RamAccess<string> RecieverTypeCode
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(RecieverTypeCode_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    RecieverTypeCode_DB = value.Value;
                }
                OnPropertyChanged(nameof(RecieverTypeCode));
            }
        }


        private bool RecieverTypeCode_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (Spravochniks.SprRecieverTypeCode.Contains(value.Value))
            {
                return true;
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //RecieverTypeCode property
        #endregion

        //PoolDistrictName property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Наименование бассейнового округа приемника отведенных вод")]
        public RamAccess<string> PoolDistrictName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PoolDistrictName_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PoolDistrictName_DB = value.Value;
                }
                OnPropertyChanged(nameof(PoolDistrictName));
            }
        }


        private bool PoolDistrictName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<string> spr = new List<string>();
            if (spr.Contains(value.Value))
            {
                return true;
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //PoolDistrictName property
        #endregion

        //AllowedWasteRemovalVolume property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Допустимый объем водоотведения за год, тыс. куб. м")]
        public RamAccess<string> AllowedWasteRemovalVolume
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(AllowedWasteRemovalVolume_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    AllowedWasteRemovalVolume_DB = value.Value;
                }
                OnPropertyChanged(nameof(AllowedWasteRemovalVolume));
            }
        }


        private bool AllowedWasteRemovalVolume_Validation(RamAccess<string> value)
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
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //AllowedWasteRemovalVolume property
        #endregion

        //RemovedWasteVolume property
        #region NumberInOrder 
public int NumberInOrder_DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Отведено за отчетный период, тыс. куб. м")]
        public RamAccess<string> RemovedWasteVolume
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(RemovedWasteVolume_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    RemovedWasteVolume_DB = value.Value;
                }
                OnPropertyChanged(nameof(RemovedWasteVolume));
            }
        }


        private bool RemovedWasteVolume_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //RemovedWasteVolume property
        #endregion

        ////RemovedWasteVolumeNote property
        //public RamAccess<double> RemovedWasteVolumeNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<double>(RemovedWasteVolumeNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            RemovedWasteVolumeNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(RemovedWasteVolumeNote));
        //    }
        //}


        //private bool RemovedWasteVolumeNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////RemovedWasteVolumeNote property
    }
}
