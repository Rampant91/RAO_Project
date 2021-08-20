using Models.DataAccess;
using System;

namespace Models
{
    //[Serializable]
    //[Attributes.Form_Class("Форма 5.6: Сведения о наличии в подведомственных организациях изделий из обедненного урана")]
    //public class Form56 : Abstracts.Form5
    //{
    //    public Form56() : base()
    //    {
    //        //FormNum.Value = "56";
    //        //NumberOfFields.Value = 5;
    //    }

    //    [Attributes.Form_Property("Форма")]
    //    public override bool Object_Validation()
    //    {
    //        return false;
    //    }

    //    //NameIOU property
    //    public int? NameIOUId { get; set; }
    //    [Attributes.Form_Property("Наименование ИОУ")]
    //    public virtual RamAccess<string> NameIOU
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<string>(nameof(NameIOU));//OK

    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(NameIOU), value);
    //            }
    //            OnPropertyChanged(nameof(NameIOU));
    //        }
    //    }


    //    private bool NameIOU_Validation(RamAccess<string> value)//TODO
    //    {
    //        value.ClearErrors(); return true;
    //    }
    //    //NameIOU property

    //    //Quantity property
    //    public int? QuantityId { get; set; }
    //    [Attributes.Form_Property("Количество, шт.")]
    //    public virtual RamAccess<int> Quantity
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<int>(nameof(Quantity));//OK

    //            }

    //            {

    //            }
    //        }
    //        set
    //        {




    //            {
    //                DataAccess.Set(nameof(Quantity), value);
    //            }
    //            OnPropertyChanged(nameof(Quantity));
    //        }
    //    }
    //    // positive int.

    //    private bool Quantity_Validation(RamAccess<int> value)//Ready
    //    {
    //        value.ClearErrors();
    //        if (value.Value <= 0)
    //        {
    //            value.AddError("Недопустимое значение");
    //            return false;
    //        }
    //        return true;
    //    }
    //    //Quantity property

    //    //Mass Property
    //    public int? MassId { get; set; }
    //    [Attributes.Form_Property("Масса, кг")]
    //    public virtual RamAccess<double> Mass
    //    {
    //        get
    //        {

    //            {
    //                return DataAccess.Get<double>(nameof(Mass));
    //            }

    //            {

    //            }
    //        }
    //        set
    //        {


    //            {
    //                DataAccess.Set(nameof(Mass), value);
    //            }
    //            OnPropertyChanged(nameof(Mass));
    //        }
    //    }


    //    private bool Mass_Validation(RamAccess<double> value)//TODO
    //    {
    //        value.ClearErrors();
    //        if (value.Value <= 0)
    //        {
    //            value.AddError("Недопустимое значение");
    //            return false;
    //        }
    //        return true;
    //    }
    //    //Mass Property
    //}
}
