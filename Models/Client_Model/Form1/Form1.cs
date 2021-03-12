using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Client_Model
{
    public abstract class Form1:Form
    {
        //IsCorrection
        [Attributes.FormVisual("Корректирующий отчет")]
        public bool IsCorrection
        {
            get
            {
                if (GetErrors(nameof(EndPeriod)) != null)
                {
                    return _IsCorrection;
                }
                else
                {
                    return _IsCorrection_Not_Valid;
                }
            }
            set
            {
                _IsCorrection_Not_Valid = value;
                if (IsCorrection_Validation())
                {
                    _IsCorrection = _IsCorrection_Not_Valid;
                }
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private bool _IsCorrection = false;
        private bool _IsCorrection_Not_Valid = false;
        private bool IsCorrection_Validation()
        {
            return true;
        }
        //IsCorrection

        //Comments
        [Attributes.FormVisual("Комментарий")]
        public string Comments
        {
            get
            {
                if (GetErrors(nameof(EndPeriod)) != null)
                {
                    return _Comments;
                }
                else
                {
                    return _Comments_Not_Valid;
                }
            }
            set
            {
                _Comments_Not_Valid = value;
                if (Comments_Validation())
                {
                    _Comments = _Comments_Not_Valid;
                }
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private string _Comments = "";
        private string _Comments_Not_Valid = "";
        private bool Comments_Validation()
        {
            return true;
        }
        //IsCorrection

        //StartPeriod (пример идеальной реализации свойства)
        [Attributes.FormVisual("Начало")]
        public DateTime StartPeriod
        {
            get
            {
                if (GetErrors(nameof(StartPeriod)) != null)
                {
                    return _StartPeriod;
                }
                else
                {
                    return _StartPeriod_Not_Valid;
                }
            }
            set
            {
                _StartPeriod_Not_Valid = value;
                if (StartPeriod_Validation())
                {
                    _StartPeriod = _StartPeriod_Not_Valid;
                }
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private DateTime _StartPeriod = DateTime.MinValue;
        private DateTime _StartPeriod_Not_Valid = DateTime.MinValue;
        private bool StartPeriod_Validation()
        {
            ClearErrors(nameof(StartPeriod));
            if (_StartPeriod_Not_Valid.Equals(DateTime.MinValue))
                AddError(nameof(StartPeriod), "Не заполнено начало периода");
            else
                if (!EndPeriod.Equals(DateTime.MinValue))
                if (_StartPeriod_Not_Valid.CompareTo(_EndPeriod_Not_Valid) > 0)
                    AddError(nameof(StartPeriod), "Начало периода не может быть позже его конца");

            if (GetErrors(nameof(StartPeriod)) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //StartPeriod

        //EndPeriod
        [Attributes.FormVisual("Конец")]
        public DateTime EndPeriod
        {
            get
            {
                if (GetErrors(nameof(EndPeriod)) != null)
                {
                    return _EndPeriod;
                }
                else
                {
                    return _EndPeriod_Not_Valid;
                }
            }
            set
            {
                _EndPeriod_Not_Valid = value;
                if (EndPeriod_Validation())
                {
                    _EndPeriod = _EndPeriod_Not_Valid;
                }
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private DateTime _EndPeriod = DateTime.MinValue;
        private DateTime _EndPeriod_Not_Valid = DateTime.MinValue;
        private bool EndPeriod_Validation()
        {
            ClearErrors(nameof(EndPeriod));
            if (_EndPeriod_Not_Valid.Equals(DateTime.MinValue))
                AddError(nameof(EndPeriod), "Не заполнено начало периода");
            else
                if (!EndPeriod.Equals(DateTime.MinValue))
                if (_StartPeriod_Not_Valid.CompareTo(_EndPeriod_Not_Valid) > 0)
                    AddError(nameof(EndPeriod), "Начало периода не может быть позже его конца");

            if (GetErrors(nameof(EndPeriod)) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //EndPeriod

        //ExportDate
        [Attributes.FormVisual("Дата выгрузки")]
        public DateTime ExportDate
        {
            get
            {
                if (GetErrors(nameof(EndPeriod)) != null)
                {
                    return _ExportDate;
                }
                else
                {
                    return _ExportDate_Not_Valid;
                }
            }
            set
            {
                _ExportDate_Not_Valid = value;
                if (ExportDate_Validation())
                {
                    _ExportDate = _ExportDate_Not_Valid;
                }
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private DateTime _ExportDate = DateTime.MinValue;
        private DateTime _ExportDate_Not_Valid = DateTime.MinValue;
        private bool ExportDate_Validation()
        {
            return true;
        }
        //ExportDate
    }
}
