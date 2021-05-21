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
            FormNum = "57";
            NumberOfFields = 9;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //RegNo property
        [Attributes.Form_Property("Регистрационный номер")]
        public string RegNo
        {
            get
            {
                if (GetErrors(nameof(RegNo)) == null)
                {
                    return (string)_dataAccess.Get(nameof(RegNo));
                }
                else
                {
                    return _RegNo_Not_Valid;
                }
            }
            set
            {
                _RegNo_Not_Valid = value;
                if (GetErrors(nameof(RegNo)) == null)
                {
                    _dataAccess.Set(nameof(RegNo), _RegNo_Not_Valid);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }

        private string _RegNo_Not_Valid = "";
        //RegNo property

        //Okpo property
        [Attributes.Form_Property("ОКПО")]
        public string Okpo
        {
            get
            {
                if (GetErrors(nameof(Okpo)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Okpo));
                }
                else
                {
                    return _Okpo_Not_Valid;
                }
            }
            set
            {
                _Okpo_Not_Valid = value;
                if (GetErrors(nameof(Okpo)) == null)
                {
                    _dataAccess.Set(nameof(Okpo), _Okpo_Not_Valid);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }
        private string _Okpo_Not_Valid = "";
        private void Okpo_Validation(string value)
        {
            ClearErrors(nameof(Okpo));
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(Okpo), "Недопустимое значение");
            else
            {
                var mask = new Regex("[0123456789_]*");
                if (!mask.IsMatch(value))
                    AddError(nameof(Okpo), "Недопустимое значение");
            }
        }
        //Okpo property

        //OrgName property
        [Attributes.Form_Property("Наименование организации")]
        public string OrgName
        {
            get
            {
                if (GetErrors(nameof(OrgName)) == null)
                {
                    return (string)_dataAccess.Get(nameof(OrgName));
                }
                else
                {
                    return _OrgName_Not_Valid;
                }
            }
            set
            {
                _OrgName_Not_Valid = value;
                if (GetErrors(nameof(OrgName)) == null)
                {
                    _dataAccess.Set(nameof(OrgName), _OrgName_Not_Valid);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }

        private string _OrgName_Not_Valid = "";
        //OrgName property

        //DocumentNameNumber property
        [Attributes.Form_Property("Наименование и номер докумета о признании")]
        public string DocumentNameNumber
        {
            get
            {
                if (GetErrors(nameof(DocumentNameNumber)) == null)
                {
                    return (string)_dataAccess.Get(nameof(DocumentNameNumber));
                }
                else
                {
                    return _DocumentNameNumber_Not_Valid;
                }
            }
            set
            {
                _DocumentNameNumber_Not_Valid = value;
                if (GetErrors(nameof(DocumentNameNumber)) == null)
                {
                    _dataAccess.Set(nameof(DocumentNameNumber), _DocumentNameNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNameNumber));
            }
        }

        private string _DocumentNameNumber_Not_Valid = "";
        //DocumentNameNumber property

        //PermissionNameNumber property
        [Attributes.Form_Property("Наименование и номер разрешительного докумета")]
        public string PermissionNameNumber
        {
            get
            {
                if (GetErrors(nameof(PermissionNameNumber)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PermissionNameNumber));
                }
                else
                {
                    return _PermissionNameNumber_Not_Valid;
                }
            }
            set
            {
                _PermissionNameNumber_Not_Valid = value;
                if (GetErrors(nameof(PermissionNameNumber)) == null)
                {
                    _dataAccess.Set(nameof(PermissionNameNumber), _PermissionNameNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionNameNumber));
            }
        }

        private string _PermissionNameNumber_Not_Valid = "";
        //PermissionNameNumber property

        //AllowedActivity property
        [Attributes.Form_Property("Разрешенный вид деятельности")]
        public string AllowedActivity
        {
            get
            {
                if (GetErrors(nameof(AllowedActivity)) == null)
                {
                    return (string)_dataAccess.Get(nameof(AllowedActivity));
                }
                else
                {
                    return _AllowedActivity_Not_Valid;
                }
            }
            set
            {
                _AllowedActivity_Not_Valid = value;
                if (GetErrors(nameof(AllowedActivity)) == null)
                {
                    _dataAccess.Set(nameof(AllowedActivity), _AllowedActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedActivity));
            }
        }

        private string _AllowedActivity_Not_Valid = "";
        private void AllowedActivity_Validation(string value)//Ready
        {
            ClearErrors(nameof(AllowedActivity));
            if ((value == null) || (value.Equals("")))
            {
                AddError(nameof(AllowedActivity), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e')))
            {
                AddError(nameof(AllowedActivity), "Недопустимое значение");
                return;
            }
            if (value != "прим.")
            {
                var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                   NumberStyles.AllowExponent;
                try
                {
                    if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                        AddError(nameof(AllowedActivity), "Число должно быть больше нуля");
                }
                catch
                {
                    AddError(nameof(AllowedActivity), "Недопустимое значение");
                }
            }
        }
        //AllowedActivity property

        //Note property
        [Attributes.Form_Property("Примечание")]
        public string Note
        {
            get
            {
                if (GetErrors(nameof(Note)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Note));
                }
                else
                {
                    return _Note_Not_Valid;
                }
            }
            set
            {
                _Note_Not_Valid = value;
                if (GetErrors(nameof(Note)) == null)
                {
                    _dataAccess.Set(nameof(Note), _Note_Not_Valid);
                }
                OnPropertyChanged(nameof(Note));
            }
        }

        private string _Note_Not_Valid = "";
        //Note property
    }
}
