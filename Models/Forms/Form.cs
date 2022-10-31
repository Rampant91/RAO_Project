using Models.Collections;
using System.Collections.Generic;
using Models.DataAccess;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using OfficeOpenXml;
using System;

namespace Models.Abstracts
{
    public abstract class Form : INotifyPropertyChanged, IKey, INumberInOrder, IDataGridColumn
    {
        public int Id { get; set; }
        [NotMapped]
        protected Dictionary<string, RamAccess> Dictionary { get; set; } = new();

        #region FormNum
        public string FormNum_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property(true,"Форма")]
        public RamAccess<string> FormNum
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(FormNum)))
                {
                    ((RamAccess<string>)Dictionary[nameof(FormNum)]).Value = FormNum_DB;
                    return (RamAccess<string>)Dictionary[nameof(FormNum)];
                }
                else
                {
                    var rm = new RamAccess<string>(FormNum_Validation, FormNum_DB);
                    rm.PropertyChanged += FormNumValueChanged;
                    Dictionary.Add(nameof(FormNum), rm);
                    return (RamAccess<string>)Dictionary[nameof(FormNum)];
                }
            }
            set
            {
                FormNum_DB = value.Value;
                OnPropertyChanged(nameof(FormNum));
            }
        }
        private void FormNumValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                FormNum_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool FormNum_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        [NotMapped]
        public long Order
        {
            get
            {
                return NumberInOrder_DB;
            }
        }

        public void SetOrder(long index) 
        {
            if (NumberInOrder_DB != (int)index)
            {
                NumberInOrder_DB = (int)index;
                OnPropertyChanged(nameof(NumberInOrder));
                OnPropertyChanged(nameof(Order));
            }
        }

        #region NumberInOrder
        public int NumberInOrder_DB { get; set; } = 0;

        [NotMapped]
        [Attributes.Form_Property(true, "null-1-1", "null-1","№ п/п","1")]
        public RamAccess<int> NumberInOrder
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(NumberInOrder)))
                {
                    ((RamAccess<int>)Dictionary[nameof(NumberInOrder)]).Value = NumberInOrder_DB;
                    return (RamAccess<int>)Dictionary[nameof(NumberInOrder)];
                }
                else
                {
                    var rm = new RamAccess<int>(NumberInOrder_Validation, NumberInOrder_DB);
                    rm.PropertyChanged += NumberInOrderValueChanged;
                    Dictionary.Add(nameof(NumberInOrder), rm);
                    return (RamAccess<int>)Dictionary[nameof(NumberInOrder)];
                }
            }
            set
            {
                NumberInOrder_DB = value.Value;
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private void NumberInOrderValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                NumberInOrder_DB = ((RamAccess<int>)Value).Value;
            }
        }
        private bool NumberInOrder_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region NumberOfFields
        public int NumberOfFields_DB { get; set; } = 0;
        [NotMapped]
        [Attributes.Form_Property(true,"Число полей")]
        public RamAccess<int> NumberOfFields
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(NumberOfFields)))
                {
                    ((RamAccess<int>)Dictionary[nameof(NumberOfFields)]).Value = NumberOfFields_DB;
                    return (RamAccess<int>)Dictionary[nameof(NumberOfFields)];
                }
                else
                {
                    var rm = new RamAccess<int>(NumberOfFields_Validation, NumberOfFields_DB);
                    rm.PropertyChanged += NumberOfFieldsValueChanged;
                    Dictionary.Add(nameof(NumberOfFields), rm);
                    return (RamAccess<int>)Dictionary[nameof(NumberOfFields)];
                }
            }
            set
            {
                NumberOfFields_DB = value.Value;
                OnPropertyChanged(nameof(NumberOfFields));
            }
        }
        private void NumberOfFieldsValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                NumberOfFields_DB = ((RamAccess<int>)Value).Value;
            }
        }
        private bool NumberOfFields_Validation(RamAccess<int> value)
        {
            value.ClearErrors();
            return true;

        }
        #endregion

        #region For_Validation
        public abstract bool Object_Validation();
        #endregion

        #region INotifyPropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region IExcel
        public abstract void ExcelGetRow(ExcelWorksheet worksheet, int Row);
        public abstract int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Tanspon = true, string SumNumber = "");
        public static int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            return 0;
        }
        #endregion

        #region IDataGridColumn
        public virtual DataGridColumns GetColumnStructure(string param)
        {
            return null;
        }
        #endregion
    }
}
