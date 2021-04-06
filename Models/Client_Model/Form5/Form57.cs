using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 5.7: Перечень подведомственных организаций ведомственного информационно-аналитического центра федерального органа исполнительной власти")]
    public class Form57: Form5
    {
        public Form57() : base()
        {
            FormNum = "57";
            NumberOfFields = 9;
        }

        [Attributes.FormVisual("Форма")]
        public override void Object_Validation()
        {

        }

        //RegNo property
        [Attributes.FormVisual("Регистрационный номер")]
        public string RegNo
        {
            get
            {
                if (GetErrors(nameof(RegNo)) != null)
                {
                    return (string)_RegNo.Get();
                }
                else
                {
                    return _RegNo_Not_Valid;
                }
            }
            set
            {
                _RegNo_Not_Valid = value;
                if (GetErrors(nameof(RegNo)) != null)
                {
                    _RegNo.Set(_RegNo_Not_Valid);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }
        private IDataLoadEngine _RegNo;
        private string _RegNo_Not_Valid = "";
        //RegNo property

        //Okpo property
        [Attributes.FormVisual("ОКПО")]
        public string Okpo
        {
            get
            {
                if (GetErrors(nameof(Okpo)) != null)
                {
                    return (string)_Okpo.Get();
                }
                else
                {
                    return _Okpo_Not_Valid;
                }
            }
            set
            {
                _Okpo_Not_Valid = value;
                if (GetErrors(nameof(Okpo)) != null)
                {
                    _Okpo.Set(_Okpo_Not_Valid);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }
        private IDataLoadEngine _Okpo;
        private string _Okpo_Not_Valid = "";
        //Okpo property

        //OrgName property
        [Attributes.FormVisual("Наименование организации")]
        public string OrgName
        {
            get
            {
                if (GetErrors(nameof(OrgName)) != null)
                {
                    return (string)_OrgName.Get();
                }
                else
                {
                    return _OrgName_Not_Valid;
                }
            }
            set
            {
                _OrgName_Not_Valid = value;
                if (GetErrors(nameof(OrgName)) != null)
                {
                    _OrgName.Set(_OrgName_Not_Valid);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }
        private IDataLoadEngine _OrgName;
        private string _OrgName_Not_Valid = "";
        //OrgName property

        //DocumentNameNumber property
        [Attributes.FormVisual("Наименование и номер докумета о признании")]
        public string DocumentNameNumber
        {
            get
            {
                if (GetErrors(nameof(DocumentNameNumber)) != null)
                {
                    return (string)_DocumentNameNumber.Get();
                }
                else
                {
                    return _DocumentNameNumber_Not_Valid;
                }
            }
            set
            {
                _DocumentNameNumber_Not_Valid = value;
                if (GetErrors(nameof(DocumentNameNumber)) != null)
                {
                    _DocumentNameNumber.Set(_DocumentNameNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNameNumber));
            }
        }
        private IDataLoadEngine _DocumentNameNumber;
        private string _DocumentNameNumber_Not_Valid = "";
        //DocumentNameNumber property

        //PermissionNameNumber property
        [Attributes.FormVisual("Наименование и номер разрешительного докумета")]
        public string PermissionNameNumber
        {
            get
            {
                if (GetErrors(nameof(PermissionNameNumber)) != null)
                {
                    return (string)_PermissionNameNumber.Get();
                }
                else
                {
                    return _PermissionNameNumber_Not_Valid;
                }
            }
            set
            {
                _PermissionNameNumber_Not_Valid = value;
                if (GetErrors(nameof(PermissionNameNumber)) != null)
                {
                    _PermissionNameNumber.Set(_PermissionNameNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionNameNumber));
            }
        }
        private IDataLoadEngine _PermissionNameNumber;
        private string _PermissionNameNumber_Not_Valid = "";
        //PermissionNameNumber property

        //AllowedActivity property
        [Attributes.FormVisual("Разрешенный вид деятельности")]
        public string AllowedActivity
        {
            get
            {
                if (GetErrors(nameof(AllowedActivity)) != null)
                {
                    return (string)_AllowedActivity.Get();
                }
                else
                {
                    return _AllowedActivity_Not_Valid;
                }
            }
            set
            {
                _AllowedActivity_Not_Valid = value;
                if (GetErrors(nameof(AllowedActivity)) != null)
                {
                    _AllowedActivity.Set(_AllowedActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedActivity));
            }
        }
        private IDataLoadEngine _AllowedActivity;
        private string _AllowedActivity_Not_Valid = "";
        private void AllowedActivity_Validation(string value)//Ready
        {
            ClearErrors(nameof(AllowedActivity));
        }
        //AllowedActivity property

        //Note property
        [Attributes.FormVisual("Примечание")]
        public string Note
        {
            get
            {
                if (GetErrors(nameof(Note)) != null)
                {
                    return (string)_Note.Get();
                }
                else
                {
                    return _Note_Not_Valid;
                }
            }
            set
            {
                _Note_Not_Valid = value;
                if (GetErrors(nameof(Note)) != null)
                {
                    _Note.Set(_Note_Not_Valid);
                }
                OnPropertyChanged(nameof(Note));
            }
        }
        private IDataLoadEngine _Note;
        private string _Note_Not_Valid = "";
        //Note property
    }
}
