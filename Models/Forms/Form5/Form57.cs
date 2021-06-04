using Models.DataAccess;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 5.7: Перечень подведомственных организаций ведомственного информационно-аналитического центра федерального органа исполнительной власти")]
    public class Form57 : Abstracts.Form5
    {
        public Form57() : base()
        {
            FormNum.Value = "57";
            NumberOfFields.Value = 9;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //RegNo property
        [Attributes.Form_Property("Регистрационный номер")]
        public RamAccess<string> RegNo
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(RegNo));
            }
            set
            {
                    _dataAccess.Set(nameof(RegNo), value);
                OnPropertyChanged(nameof(RegNo));
            }
        }


        //RegNo property

        //Okpo property
        [Attributes.Form_Property("ОКПО")]
        public RamAccess<string> Okpo
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(Okpo));
            }
            set
            {
                    _dataAccess.Set(nameof(Okpo), value);
                OnPropertyChanged(nameof(Okpo));
            }
        }

        private void Okpo_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
                value.AddError( "Недопустимое значение");
            
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //Okpo property

        //OrgName property
        [Attributes.Form_Property("Наименование организации")]
        public RamAccess<string> OrgName
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(OrgName));
            }
            set
            {
                    _dataAccess.Set(nameof(OrgName), value);
                OnPropertyChanged(nameof(OrgName));
            }
        }


        //OrgName property

        //DocumentNameNumber property
        [Attributes.Form_Property("Наименование и номер докумета о признании")]
        public RamAccess<string> DocumentNameNumber
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(DocumentNameNumber));
            }
            set
            {
                    _dataAccess.Set(nameof(DocumentNameNumber), value);
                OnPropertyChanged(nameof(DocumentNameNumber));
            }
        }


        //DocumentNameNumber property

        //PermissionNameNumber property
        [Attributes.Form_Property("Наименование и номер разрешительного докумета")]
        public RamAccess<string> PermissionNameNumber
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(PermissionNameNumber));
            }
            set
            {
                    _dataAccess.Set(nameof(PermissionNameNumber), value);
                OnPropertyChanged(nameof(PermissionNameNumber));
            }
        }


        //PermissionNameNumber property

        //AllowedActivity property
        [Attributes.Form_Property("Разрешенный вид деятельности")]
        public RamAccess<string> AllowedActivity
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(AllowedActivity));
            }
            set
            {
                    _dataAccess.Set(nameof(AllowedActivity), value);
                OnPropertyChanged(nameof(AllowedActivity));
            }
        }


        private void AllowedActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || (value.Value.Equals("")))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            if (value.Value != "прим.")
            {
                var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                   NumberStyles.AllowExponent;
                try
                {
                    if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                        value.AddError( "Число должно быть больше нуля");
                }
                catch
                {
                    value.AddError( "Недопустимое значение");
                }
            }
        }
        //AllowedActivity property

        //Note property
        [Attributes.Form_Property("Примечание")]
        public RamAccess<string> Note
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(Note));
            }
            set
            {
                    _dataAccess.Set(nameof(Note), value);
                OnPropertyChanged(nameof(Note));
            }
        }


        //Note property
    }
}
