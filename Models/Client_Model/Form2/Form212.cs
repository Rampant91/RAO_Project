using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.12: Суммарные сведения о РВ не в составе ЗРИ")]
    public class Form212 : Form2

    {
        public override string FormNum { get { return "2.12"; } }
        public override void Object_Validation()
        {

        }
        public override int NumberOfFields { get; } = 8;

        //OperationCode property
        [Attributes.FormVisual("Код")]
        public short OperationCode
        {
            get
            {
                if (GetErrors(nameof(OperationCode)) != null)
                {
                    return (short)_OperationCode.Get();
                }
                else
                {
                    return _OperationCode_Not_Valid;
                }
            }
            set
            {
                _OperationCode_Not_Valid = value;
                if (GetErrors(nameof(OperationCode)) != null)
                {
                    _OperationCode.Set(_OperationCode_Not_Valid);
                }
                OnPropertyChanged(nameof(OperationCode));
            }
        }
        private IDataLoadEngine _OperationCode;
        private short _OperationCode_Not_Valid = -1;
        private void OperationCode_Validation()
        {
            ClearErrors(nameof(OperationCode));
        }
        //OperationCode property
        
        //ObjectTypeCode property
        [Attributes.FormVisual("Код типа объектов учета")]
        public string ObjectTypeCode
        {
            get
            {
                if (GetErrors(nameof(ObjectTypeCode)) != null)
                {
                    return (string)_ObjectTypeCode.Get();
                }
                else
                {
                    return _ObjectTypeCode_Not_Valid;
                }
            }
            set
            {
                _ObjectTypeCode_Not_Valid = value;
                if (GetErrors(nameof(ObjectTypeCode)) != null)
                {
                    _ObjectTypeCode.Set(_ObjectTypeCode_Not_Valid);
                }
                OnPropertyChanged(nameof(ObjectTypeCode));
            }
        }
        private IDataLoadEngine _ObjectTypeCode; //2 digit code
        private string _ObjectTypeCode_Not_Valid = ""; //2 digit code
        private void ObjectTypeCode_Validation(string value)//TODO
        {
            ClearErrors(ObjectTypeCode);
        }
        //ObjectTypeCode property

        //Radionuclids property
        [Attributes.FormVisual("Радионуклиды")]
        public string Radionuclids
        {
            get
            {
                if (GetErrors(nameof(Radionuclids)) != null)
                {
                    return (string)_Radionuclids.Get();
                }
                else
                {
                    return _Radionuclids_Not_Valid;
                }
            }
            set
            {
                _Radionuclids_Not_Valid = value;
                if (GetErrors(nameof(Radionuclids)) != null)
                {
                    _Radionuclids.Set(_Radionuclids_Not_Valid);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        private IDataLoadEngine _Radionuclids;//If change this change validation
        private string _Radionuclids_Not_Valid = "";
        private void Radionuclids_Validation()//TODO
        {
            ClearErrors(nameof(Radionuclids));
        }
        //Radionuclids property

        //Activity property
        [Attributes.FormVisual("Активность, Бк")]
        public double Activity
        {
            get
            {
                if (GetErrors(nameof(Activity)) != null)
                {
                    return (double)_Activity.Get();
                }
                else
                {
                    return _Activity_Not_Valid;
                }
            }
            set
            {
                _Activity_Not_Valid = value;
                if (GetErrors(nameof(Activity)) != null)
                {
                    _Activity.Set(_Activity_Not_Valid);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }
        private IDataLoadEngine _Activity;
        private double _Activity_Not_Valid = -1;
        private void Activity_Validation(double value)//Ready
        {
            ClearErrors(nameof(Activity));
            if (!(value > 0))
                AddError(nameof(Activity), "Число должно быть больше нуля");
        }
        //Activity property

        //ProviderOrRecieverOKPO property
        [Attributes.FormVisual("ОКПО поставщика/получателя")]
        public string ProviderOrRecieverOKPO
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) != null)
                {
                    return (string)_ProviderOrRecieverOKPO.Get();
                }
                else
                {
                    return _ProviderOrRecieverOKPO_Not_Valid;
                }
            }
            set
            {
                _ProviderOrRecieverOKPO_Not_Valid = value;
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) != null)
                {
                    _ProviderOrRecieverOKPO.Set(_ProviderOrRecieverOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }
        private IDataLoadEngine _ProviderOrRecieverOKPO;
        private string _ProviderOrRecieverOKPO_Not_Valid = "";
        private void ProviderOrRecieverOKPO_Validation()//TODO
        {
            ClearErrors(nameof(ProviderOrRecieverOKPO));
        }
        //ProviderOrRecieverOKPO property

        private string _providerOrRecieverOKPONote = "";
        public string ProviderOrRecieverOKPONote
        {
            get { return _providerOrRecieverOKPONote; }
            set
            {
                _providerOrRecieverOKPONote = value;
                OnPropertyChanged("ProviderOrRecieverOKPONote");
            }
        }
    }
}
