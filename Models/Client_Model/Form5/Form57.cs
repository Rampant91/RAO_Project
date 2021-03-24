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
        public override string FormNum { get { return "5.7"; } }
        public override void Object_Validation()
        {

        }
        public override int NumberOfFields { get; } = 9;

        private string _regNo = "";
        [Attributes.FormVisual("Регистрационный номер")]
        public string RegNo
        {
            get { return _regNo; }
            set
            {
                _regNo = value;
                OnPropertyChanged("RegNo");
            }
        }

        private string _okpo = "";
        [Attributes.FormVisual("Код ОКПО")]
        public string Okpo
        {
            get { return _okpo; }
            set
            {
                _okpo = value;
                OnPropertyChanged("Okpo");
            }
        }

        private string _orgName = "";
        [Attributes.FormVisual("Наименование организации")]
        public string OrgName
        {
            get { return _orgName; }
            set
            {
                _orgName = value;
                OnPropertyChanged("OrgName");
            }
        }

        private string _documentNameNumber = "";
        [Attributes.FormVisual("Наименование и номер докумета о признании")]
        public string DocumentNameNumber
        {
            get { return _documentNameNumber; }
            set
            {
                _documentNameNumber = value;
                OnPropertyChanged("DocumentNameNumber");
            }
        }

        private string _permissionNameNumber = "";
        [Attributes.FormVisual("Наименование и номер разрешительного докумета")]
        public string PermissionNameNumber
        {
            get { return _permissionNameNumber; }
            set
            {
                _permissionNameNumber = value;
                OnPropertyChanged("PermissionNameNumber");
            }
        }

        private string _allowedActivity = "";
        [Attributes.FormVisual("Разрешенный вид деятельности")]
        public string AllowedActivity
        {
            get { return _allowedActivity; }
            set
            {
                _allowedActivity = value;
                OnPropertyChanged("AllowedActivity");
            }
        }

        private string _note = "";
        [Attributes.FormVisual("Примечание")]
        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
                OnPropertyChanged("Note");
            }
        }
    }
}
