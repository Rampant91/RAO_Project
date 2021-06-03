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
        public IDataAccess<string> RegNo
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RegNo));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RegNo), _RegNo_Not_Valid);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }


        //RegNo property

        //Okpo property
        [Attributes.Form_Property("ОКПО")]
        public IDataAccess<string> Okpo
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okpo));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okpo), _Okpo_Not_Valid);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }

        private void Okpo_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Length != 8) && (value.Length != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //Okpo property

        //OrgName property
        [Attributes.Form_Property("Наименование организации")]
        public IDataAccess<string> OrgName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(OrgName));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(OrgName), _OrgName_Not_Valid);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }


        //OrgName property

        //DocumentNameNumber property
        [Attributes.Form_Property("Наименование и номер докумета о признании")]
        public IDataAccess<string> DocumentNameNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(DocumentNameNumber));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(DocumentNameNumber), _DocumentNameNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNameNumber));
            }
        }


        //DocumentNameNumber property

        //PermissionNameNumber property
        [Attributes.Form_Property("Наименование и номер разрешительного докумета")]
        public IDataAccess<string> PermissionNameNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PermissionNameNumber));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PermissionNameNumber), _PermissionNameNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionNameNumber));
            }
        }


        //PermissionNameNumber property

        //AllowedActivity property
        [Attributes.Form_Property("Разрешенный вид деятельности")]
        public IDataAccess<string> AllowedActivity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AllowedActivity));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AllowedActivity), _AllowedActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedActivity));
            }
        }


        private void AllowedActivity_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || (value.Equals("")))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            if (value != "прим.")
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
        public IDataAccess<string> Note
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Note));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Note), _Note_Not_Valid);
                }
                OnPropertyChanged(nameof(Note));
            }
        }


        //Note property
    }
}
