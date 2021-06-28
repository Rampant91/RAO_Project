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
            get => DataAccess.Get<string>(nameof(RegNo));
            set
            {
                DataAccess.Set(nameof(RegNo), value);
                OnPropertyChanged(nameof(RegNo));
            }
        }


        //RegNo property

        //Okpo property
        [Attributes.Form_Property("ОКПО")]
        public RamAccess<string> Okpo
        {
            get => DataAccess.Get<string>(nameof(Okpo));
            set
            {
                DataAccess.Set(nameof(Okpo), value);
                OnPropertyChanged(nameof(Okpo));
            }
        }

        private bool Okpo_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Okpo property

        //OrgName property
        [Attributes.Form_Property("Наименование организации")]
        public RamAccess<string> OrgName
        {
            get => DataAccess.Get<string>(nameof(OrgName));
            set
            {
                DataAccess.Set(nameof(OrgName), value);
                OnPropertyChanged(nameof(OrgName));
            }
        }


        //OrgName property

        //DocumentNameNumber property
        [Attributes.Form_Property("Наименование и номер докумета о признании")]
        public RamAccess<string> DocumentNameNumber
        {
            get => DataAccess.Get<string>(nameof(DocumentNameNumber));
            set
            {
                DataAccess.Set(nameof(DocumentNameNumber), value);
                OnPropertyChanged(nameof(DocumentNameNumber));
            }
        }


        //DocumentNameNumber property

        //PermissionNameNumber property
        [Attributes.Form_Property("Наименование и номер разрешительного докумета")]
        public RamAccess<string> PermissionNameNumber
        {
            get => DataAccess.Get<string>(nameof(PermissionNameNumber));
            set
            {
                DataAccess.Set(nameof(PermissionNameNumber), value);
                OnPropertyChanged(nameof(PermissionNameNumber));
            }
        }


        //PermissionNameNumber property

        //AllowedActivity property
        [Attributes.Form_Property("Разрешенный вид деятельности")]
        public RamAccess<string> AllowedActivity
        {
            get => DataAccess.Get<string>(nameof(AllowedActivity));
            set
            {
                DataAccess.Set(nameof(AllowedActivity), value);
                OnPropertyChanged(nameof(AllowedActivity));
            }
        }


        private bool AllowedActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || (value.Value.Equals("")))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            if (value.Value != "прим.")
            {
                NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                   NumberStyles.AllowExponent;
                try
                {
                    if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    {
                        value.AddError("Число должно быть больше нуля");
                        return false;
                    }
                }
                catch
                {
                    value.AddError("Недопустимое значение");
                    return false;
                }
            }
            return true;
        }
        //AllowedActivity property

        //Note property
        [Attributes.Form_Property("Примечание")]
        public RamAccess<string> Note
        {
            get => DataAccess.Get<string>(nameof(Note));
            set
            {
                DataAccess.Set(nameof(Note), value);
                OnPropertyChanged(nameof(Note));
            }
        }


        //Note property
    }
}
