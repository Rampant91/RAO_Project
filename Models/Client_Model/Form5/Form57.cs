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
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 9;

        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return _correctionNumber;
                }
                else
                {
                    return _correctionNumber_Not_Valid;
                }
            }
            set
            {
                _correctionNumber_Not_Valid = value;
                if (CorrectionNumber_Validation())
                {
                    _correctionNumber = _correctionNumber_Not_Valid;
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private byte _correctionNumber = 255;
        private byte _correctionNumber_Not_Valid = 255;
        private bool CorrectionNumber_Validation()
        {
            return true;
            //ClearErrors(nameof(CorrectionNumber));
            ////Пример
            //if (value < 10)
            //    AddError(nameof(CorrectionNumber), "Значение должно быть больше 10.");
        }

        private int _numberInOrder = -1;
        [Attributes.FormVisual("№ п/п")]
        public int NumberInOrder
        {
            get { return _numberInOrder; }
            set
            {
                _numberInOrder = value;
                OnPropertyChanged("NumberInOrder");
            }
        }

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
