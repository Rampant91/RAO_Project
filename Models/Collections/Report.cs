using Models.DataAccess;
using Models.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;

namespace Collections
{
    public class Report : INotifyPropertyChanged
    {
        IDataAccessCollection _dataAccess { get; set; }

        public Report(IDataAccessCollection Access)
        {
            _dataAccess = Access;
        }

        public Report()
        {
            _dataAccess = new DataAccessCollection();
        }

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows10));
            OnPropertyChanged(nameof(Rows11));
            OnPropertyChanged(nameof(Rows12));
            OnPropertyChanged(nameof(Rows13));
            OnPropertyChanged(nameof(Rows14));
            OnPropertyChanged(nameof(Rows15));
            OnPropertyChanged(nameof(Rows16));
            OnPropertyChanged(nameof(Rows17));
            OnPropertyChanged(nameof(Rows18));
            OnPropertyChanged(nameof(Rows19));

            //OnPropertyChanged(nameof(Rows20));
            //OnPropertyChanged(nameof(Rows21));
            //OnPropertyChanged(nameof(Rows22));
            //OnPropertyChanged(nameof(Rows23));
            //OnPropertyChanged(nameof(Rows24));
            //OnPropertyChanged(nameof(Rows25));
            //OnPropertyChanged(nameof(Rows26));
            //OnPropertyChanged(nameof(Rows27));
            //OnPropertyChanged(nameof(Rows28));
            //OnPropertyChanged(nameof(Rows29));
            //OnPropertyChanged(nameof(Rows210));
            //OnPropertyChanged(nameof(Rows211));
            //OnPropertyChanged(nameof(Rows212));

            //OnPropertyChanged(nameof(Rows30));
            //OnPropertyChanged(nameof(Rows31));
            //OnPropertyChanged(nameof(Rows31_1));
            //OnPropertyChanged(nameof(Rows32));
            //OnPropertyChanged(nameof(Rows32_1));
            //OnPropertyChanged(nameof(Rows32_2));
            //OnPropertyChanged(nameof(Rows32_3));

            //OnPropertyChanged(nameof(Rows40));
            //OnPropertyChanged(nameof(Rows41));

            //OnPropertyChanged(nameof(Rows50));
            //OnPropertyChanged(nameof(Rows51));
            //OnPropertyChanged(nameof(Rows52));
            //OnPropertyChanged(nameof(Rows53));
            //OnPropertyChanged(nameof(Rows54));
            //OnPropertyChanged(nameof(Rows55));
            //OnPropertyChanged(nameof(Rows56));
            //OnPropertyChanged(nameof(Rows57));
        }

        [Key]
        public IDataAccess<int> ReportId
        {
            get
            {
                return _dataAccess.Get<int>(nameof(ReportId));
            }
            set
            {
                _dataAccess.Set(nameof(ReportId), value);
                OnPropertyChanged(nameof(ReportId));
            }
        }

        public virtual IDataAccess<ObservableCollection<Models.Form10>> Rows10
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form10>>(nameof(Rows10));
            }
            set
            {
                _dataAccess.Set(nameof(Rows10), value);
                OnPropertyChanged(nameof(Rows10));
            }
        }
        private bool Rows10_Validation(IDataAccess<ObservableCollection<Models.Form10>> value)
        {
            return true;
        }


        public virtual IDataAccess<ObservableCollection<Models.Form11>> Rows11
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form11>>(nameof(Rows11));
            }
            set
            {
                _dataAccess.Set(nameof(Rows11), value);
                OnPropertyChanged(nameof(Rows11));
            }
        }
        private bool Rows11_Validation(IDataAccess<ObservableCollection<Models.Form11>> value)
        {
            return true;
        }


        public virtual IDataAccess<ObservableCollection<Models.Form12>> Rows12
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form12>>(nameof(Rows12));
            }
            set
            {
                _dataAccess.Set(nameof(Rows12), value);
                OnPropertyChanged(nameof(Rows12));
            }
        }
        private bool Rows12_Validation(IDataAccess<ObservableCollection<Models.Form12>> value)
        {
            return true;
        }

        public virtual IDataAccess<ObservableCollection<Models.Form13>> Rows13
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form13>>(nameof(Rows13));
            }
            set
            {
                _dataAccess.Set(nameof(Rows13), value);
                OnPropertyChanged(nameof(Rows13));
            }
        }
        private bool Rows13_Validation(IDataAccess<ObservableCollection<Models.Form13>> value)
        {
            return true;
        }

        public virtual IDataAccess<ObservableCollection<Models.Form14>> Rows14
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form14>>(nameof(Rows14));
            }
            set
            {
                _dataAccess.Set(nameof(Rows14), value);
                OnPropertyChanged(nameof(Rows14));
            }
        }
        private bool Rows14_Validation(IDataAccess<ObservableCollection<Models.Form14>> value)
        {
            return true;
        }

        public virtual IDataAccess<ObservableCollection<Models.Form15>> Rows15
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form15>>(nameof(Rows15));
            }
            set
            {
                _dataAccess.Set(nameof(Rows15), value);
                OnPropertyChanged(nameof(Rows15));
            }
        }
        private bool Rows15_Validation(IDataAccess<ObservableCollection<Models.Form15>> value)
        {
            return true;
        }

        public virtual IDataAccess<ObservableCollection<Models.Form16>> Rows16
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form16>>(nameof(Rows16));
            }
            set
            {
                _dataAccess.Set(nameof(Rows16), value);
                OnPropertyChanged(nameof(Rows16));
            }
        }
        private bool Rows16_Validation(IDataAccess<ObservableCollection<Models.Form16>> value)
        {
            return true;
        }

        public virtual IDataAccess<ObservableCollection<Models.Form17>> Rows17
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form17>>(nameof(Rows17));
            }
            set
            {
                _dataAccess.Set(nameof(Rows17), value);
                OnPropertyChanged(nameof(Rows17));
            }
        }
        private bool Rows17_Validation(IDataAccess<ObservableCollection<Models.Form17>> value)
        {
            return true;
        }

        public virtual IDataAccess<ObservableCollection<Models.Form18>> Rows18
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form18>>(nameof(Rows18));
            }
            set
            {
                _dataAccess.Set(nameof(Rows18), value);
                OnPropertyChanged(nameof(Rows18));
            }
        }
        private bool Rows18_Validation(IDataAccess<ObservableCollection<Models.Form18>> value)
        {
            return true;
        }

        public virtual IDataAccess<ObservableCollection<Models.Form19>> Rows19
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form19>>(nameof(Rows19));
            }
            set
            {
                _dataAccess.Set(nameof(Rows19), value);
                OnPropertyChanged(nameof(Rows19));
            }
        }
        private bool Rows19_Validation(IDataAccess<ObservableCollection<Models.Form19>> value)
        {
            return true;
        }

        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}


        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}


        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}


        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}


        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}


        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}


        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}


        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}


        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}


        //public virtual IDataAccess<ObservableCollection<Models.Form20>> Rows20
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows20), value);
        //        OnPropertyChanged(nameof(Rows20));
        //    }
        //}
        //private bool Rows20_Validation(IDataAccess<ObservableCollection<Models.Form20>> value)
        //{
        //    return true;
        //}


        //public virtual IDataAccess<ObservableCollection<Models.Form30>> Rows30
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form30>>(nameof(Rows30));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows30), value);
        //        OnPropertyChanged(nameof(Rows30));
        //    }
        //}
        //private bool Rows30_Validation(IDataAccess<ObservableCollection<Models.Form30>> value)
        //{
        //    return true;
        //}


        //public virtual IDataAccess<ObservableCollection<Models.Form30>> Rows30
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form30>>(nameof(Rows30));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows30), value);
        //        OnPropertyChanged(nameof(Rows30));
        //    }
        //}
        //private bool Rows30_Validation(IDataAccess<ObservableCollection<Models.Form30>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form30>> Rows30
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form30>>(nameof(Rows30));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows30), value);
        //        OnPropertyChanged(nameof(Rows30));
        //    }
        //}
        //private bool Rows30_Validation(IDataAccess<ObservableCollection<Models.Form30>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form30>> Rows30
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form30>>(nameof(Rows30));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows30), value);
        //        OnPropertyChanged(nameof(Rows30));
        //    }
        //}
        //private bool Rows30_Validation(IDataAccess<ObservableCollection<Models.Form30>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form30>> Rows30
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form30>>(nameof(Rows30));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows30), value);
        //        OnPropertyChanged(nameof(Rows30));
        //    }
        //}
        //private bool Rows30_Validation(IDataAccess<ObservableCollection<Models.Form30>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form30>> Rows30
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form30>>(nameof(Rows30));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows30), value);
        //        OnPropertyChanged(nameof(Rows30));
        //    }
        //}
        //private bool Rows30_Validation(IDataAccess<ObservableCollection<Models.Form30>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form30>> Rows30
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form30>>(nameof(Rows30));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows30), value);
        //        OnPropertyChanged(nameof(Rows30));
        //    }
        //}
        //private bool Rows30_Validation(IDataAccess<ObservableCollection<Models.Form30>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form40>> Rows40
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form40>>(nameof(Rows40));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows40), value);
        //        OnPropertyChanged(nameof(Rows40));
        //    }
        //}
        //private bool Rows40_Validation(IDataAccess<ObservableCollection<Models.Form40>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form40>> Rows40
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form40>>(nameof(Rows40));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows40), value);
        //        OnPropertyChanged(nameof(Rows40));
        //    }
        //}
        //private bool Rows40_Validation(IDataAccess<ObservableCollection<Models.Form40>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form50>> Rows50
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form50>>(nameof(Rows50));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows50), value);
        //        OnPropertyChanged(nameof(Rows50));
        //    }
        //}
        //private bool Rows50_Validation(IDataAccess<ObservableCollection<Models.Form50>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form50>> Rows50
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form50>>(nameof(Rows50));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows50), value);
        //        OnPropertyChanged(nameof(Rows50));
        //    }
        //}
        //private bool Rows50_Validation(IDataAccess<ObservableCollection<Models.Form50>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form50>> Rows50
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form50>>(nameof(Rows50));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows50), value);
        //        OnPropertyChanged(nameof(Rows50));
        //    }
        //}
        //private bool Rows50_Validation(IDataAccess<ObservableCollection<Models.Form50>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form50>> Rows50
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form50>>(nameof(Rows50));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows50), value);
        //        OnPropertyChanged(nameof(Rows50));
        //    }
        //}
        //private bool Rows50_Validation(IDataAccess<ObservableCollection<Models.Form50>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form50>> Rows50
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form50>>(nameof(Rows50));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows50), value);
        //        OnPropertyChanged(nameof(Rows50));
        //    }
        //}
        //private bool Rows50_Validation(IDataAccess<ObservableCollection<Models.Form50>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form50>> Rows50
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form50>>(nameof(Rows50));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows50), value);
        //        OnPropertyChanged(nameof(Rows50));
        //    }
        //}
        //private bool Rows50_Validation(IDataAccess<ObservableCollection<Models.Form50>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form50>> Rows50
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form50>>(nameof(Rows50));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows50), value);
        //        OnPropertyChanged(nameof(Rows50));
        //    }
        //}
        //private bool Rows50_Validation(IDataAccess<ObservableCollection<Models.Form50>> value)
        //{
        //    return true;
        //}

        //public virtual IDataAccess<ObservableCollection<Models.Form50>> Rows50
        //{
        //    get
        //    {
        //        return _dataAccess.Get<ObservableCollection<Models.Form50>>(nameof(Rows50));
        //    }
        //    set
        //    {
        //        _dataAccess.Set(nameof(Rows50), value);
        //        OnPropertyChanged(nameof(Rows50));
        //    }
        //}
        //private bool Rows50_Validation(IDataAccess<ObservableCollection<Models.Form50>> value)
        //{
        //    return true;
        //}

        [Form_Property("Форма")]
        public IDataAccess<string> FormNum
        {
            get
            {
                return _dataAccess.Get<string>(nameof(FormNum));
            }
            set
            {
                _dataAccess.Set(nameof(FormNum), value);
                OnPropertyChanged(nameof(FormNum));
            }
        }

        //IsCorrection 
        [Form_Property("Корректирующий отчет")]
        public IDataAccess<bool> IsCorrection
        {
            get
            {
                return _dataAccess.Get<bool>(nameof(IsCorrection));
            }
            set
            {
                _dataAccess.Set(nameof(IsCorrection), value);
                OnPropertyChanged(nameof(IsCorrection));
            }
        }
        private bool IsCorrection_Validation(IDataAccess<bool> value)
        {
            return true;
        }
        //IsCorrection

        //CorrectionNumber property
        [Form_Property("Номер корректировки")]
        public IDataAccess<byte> CorrectionNumber
        {
            get
            {
                return _dataAccess.Get<byte>(nameof(CorrectionNumber));
            }
            set
            {
                _dataAccess.Set(nameof(CorrectionNumber), value);
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private void CorrectionNumber_Validation(IDataAccess<byte> value)
        {
            value.ClearErrors();
        }
        //CorrectionNumber property

        //NumberInOrder property
        [Form_Property("Номер")]
        public IDataAccess<string> NumberInOrder
        {
            get
            {
                return _dataAccess.Get<string>(nameof(NumberInOrder));
            }
            set
            {
                _dataAccess.Set(nameof(NumberInOrder), value);
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private bool NumberInOrder_Validation(IDataAccess<string> value)
        {
            return true;
        }
        //NumberInOrder property

        //Comments property
        [Form_Property("Комментарий")]
        public IDataAccess<string> Comments
        {
            get
            {
                return _dataAccess.Get<string>(nameof(Comments));
            }
            set
            {
                _dataAccess.Set(nameof(Comments), value);
                OnPropertyChanged(nameof(Comments));
            }
        }
        private bool Comments_Validation(IDataAccess<string> value)
        {
            return true;
        }
        //Comments property

        //Notes property
        [Form_Property("Примечания")]
        public virtual IDataAccess<ObservableCollection<Models.Note>> Notes
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Note>>(nameof(Notes));
            }
            set
            {
                _dataAccess.Set(nameof(Notes), value);
                OnPropertyChanged(nameof(Notes));
            }
        }
        private bool Notes_Validation(IDataAccess<ObservableCollection<Models.Note>> value)
        {
            return true;
        }
        //Notes property

        //StartPeriod
        [Form_Property("Начало")]
        public IDataAccess<string> StartPeriod
        {
            get
            {
                return _dataAccess.Get<string>(nameof(StartPeriod));
            }
            set
            {
                _dataAccess.Set(nameof(StartPeriod), value);
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private void StartPeriod_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //StartPeriod

        //EndPeriod
        [Form_Property("Конец")]
        public IDataAccess<string> EndPeriod
        {
            get
            {
                return _dataAccess.Get<string>(nameof(EndPeriod));
            }
            set
            {
                _dataAccess.Set(nameof(EndPeriod), value);
                OnPropertyChanged(nameof(EndPeriod));
            }
        }
        private void EndPeriod_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //EndPeriod

        //ExportDate
        [Form_Property("Дата выгрузки")]
        public IDataAccess<string> ExportDate
        {
            get
            {
                return _dataAccess.Get<string>(nameof(ExportDate));
            }
            set
            {
                _dataAccess.Set(nameof(ExportDate), value);
                OnPropertyChanged(nameof(ExportDate));
            }
        }
        private bool ExportDate_Validation(IDataAccess<string> value)
        {
            return true;
        }
        //ExportDate

        //Property Changed
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed
    }
}
