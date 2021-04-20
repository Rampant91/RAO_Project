using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DBRealization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 5.6: Сведения о наличии в подведомственных организациях изделий из обедненного урана")]
    public class Form56 : Abstracts.Form5
    {
        public static string SQLCommandParams()
        {
            return
                Abstracts.Form5.SQLCommandParamsBase() +
            nameof(NameIOU) + SQLconsts.strNotNullDeclaration +
            nameof(Mass) + SQLconsts.doubleNotNullDeclaration +
            nameof(Quantity) + " int not null";
        }
        public Form56(IDataAccess Access) : base(Access)
        {
            FormNum = "56";
            NumberOfFields = 5;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //NameIOU property
        [Attributes.Form_Property("Наименование ИОУ")]
        public string NameIOU
        {
            get
            {
                if (GetErrors(nameof(NameIOU)) != null)
                {
                    return (string)_dataAccess.Get(nameof(NameIOU));
                }
                else
                {
                    return _NameIOU_Not_Valid;
                }
            }
            set
            {
                _NameIOU_Not_Valid = value;
                if (GetErrors(nameof(NameIOU)) != null)
                {
                    _dataAccess.Set(nameof(NameIOU), _NameIOU_Not_Valid);
                }
                OnPropertyChanged(nameof(NameIOU));
            }
        }
        
        private string _NameIOU_Not_Valid = "";
        private void NameIOU_Validation(string value)//TODO
        {
            ClearErrors(nameof(NameIOU));
        }
        //NameIOU property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public int Quantity
        {
            get
            {
                if (GetErrors(nameof(Quantity)) != null)
                {
                    return (int)_dataAccess.Get(nameof(Quantity));
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
                    _dataAccess.Set(nameof(Quantity), _Quantity_Not_Valid);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
          // positive int.
        private int _Quantity_Not_Valid = -1;
        private void Quantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value <= 0)
                AddError(nameof(Quantity), "Недопустимое значение");
        }
        //Quantity property

        //Mass Property
        [Attributes.Form_Property("Масса, кг")]
        public double Mass
        {
            get
            {
                if (GetErrors(nameof(Mass)) != null)
                {
                    return (double)_dataAccess.Get(nameof(Mass));
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
                    _dataAccess.Set(nameof(Mass), _Mass_Not_Valid);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }
        
        private double _Mass_Not_Valid = -1;
        private void Mass_Validation()//TODO
        {
            ClearErrors(nameof(Mass));
        }
        //Mass Property
    }
}
