using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 5.5: Сведения о поступлении/передаче в подведомственные организации от сторонних организаций и переводе в РАО изделий из обедненного урана")]
    public class Form55 : Abstracts.Form5
    {
        public Form55(int RowID) : base(RowID)
        {
            FormNum = "55";
            NumberOfFields = 8;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //Name property
        [Attributes.Form_Property("Наименование")]
        public string Name
        {
            get
            {
                if (GetErrors(nameof(Name)) != null)
                {
                    return (string)_Name.Get();
                }
                else
                {
                    return _Name_Not_Valid;
                }
            }
            set
            {
                _Name_Not_Valid = value;
                if (GetErrors(nameof(Name)) != null)
                {
                    _Name.Set(_Name_Not_Valid);
                }
                OnPropertyChanged(nameof(Name));
            }
        }
        private IDataLoadEngine _Name;
        private string _Name_Not_Valid = "";
        private void Name_Validation(string value)//TODO
        {
            ClearErrors(nameof(Name));
        }
        //Name property

        //OperationCode property
        [Attributes.Form_Property("Код")]
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

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public int Quantity
        {
            get
            {
                if (GetErrors(nameof(Quantity)) != null)
                {
                    return (int)_Quantity.Get();
                }
                else
                {
                    return _Quantity_Not_Valid;
                }
            }
            set
            {
                _Quantity_Not_Valid = value;
                if (GetErrors(nameof(Quantity)) != null)
                {
                    _Quantity.Set(_Quantity_Not_Valid);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        private IDataLoadEngine _Quantity;  // positive int.
        private int _Quantity_Not_Valid = -1;
        private void Quantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value <= 0)
                AddError(nameof(Quantity), "Недопустимое значение");
        }
        //Quantity property

        //ProviderOrRecieverOKPO property
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
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

        //ProviderOrRecieverOKPONote property
        public string ProviderOrRecieverOKPONote
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) != null)
                {
                    return (string)_ProviderOrRecieverOKPONote.Get();
                }
                else
                {
                    return _ProviderOrRecieverOKPONote_Not_Valid;
                }
            }
            set
            {
                _ProviderOrRecieverOKPONote_Not_Valid = value;
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) != null)
                {
                    _ProviderOrRecieverOKPONote.Set(_ProviderOrRecieverOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }
        private IDataLoadEngine _ProviderOrRecieverOKPONote;
        private string _ProviderOrRecieverOKPONote_Not_Valid = "";
        private void ProviderOrRecieverOKPONote_Validation()
        {
            ClearErrors(nameof(ProviderOrRecieverOKPONote));
        }
        //ProviderOrRecieverOKPONote property

        //Mass Property
        [Attributes.Form_Property("Масса, кг")]
        public double Mass
        {
            get
            {
                if (GetErrors(nameof(Mass)) != null)
                {
                    return (double)_Mass.Get();
                }
                else
                {
                    return _Mass_Not_Valid;
                }
            }
            set
            {
                _Mass_Not_Valid = value;
                if (GetErrors(nameof(Mass)) != null)
                {
                    _Mass.Set(_Mass_Not_Valid);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }
        private IDataLoadEngine _Mass;
        private double _Mass_Not_Valid = -1;
        private void Mass_Validation()//TODO
        {
            ClearErrors(nameof(Mass));
        }
        //Mass Property
    }
}
