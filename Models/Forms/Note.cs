using System;
using System.Collections.Generic;
using System.Text;
using DBRealization;

namespace Models
{
    public class Note:Abstracts.Form
    {
        public Note(IDataAccess Access) : base(Access)
        {

        }

        //RowNumber property
        [Attributes.Form_Property("Номер строки")]
        public int RowNumber
        {
            get
            {
                if (GetErrors(nameof(RowNumber)) == null)
                {
                    return (int)_dataAccess.Get(nameof(RowNumber));
                }
                else
                {
                    return _RowNumber_Not_Valid;
                }
            }
            set
            {
                _RowNumber_Not_Valid = value;
                if (GetErrors(nameof(RowNumber)) == null)
                {
                    _dataAccess.Set(nameof(RowNumber), _RowNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(RowNumber));
            }
        }
        private int _RowNumber_Not_Valid;
        private void RowNumber_Validation()
        {
            ClearErrors(nameof(RowNumber));
        }
        //RowNumber property

        //GraphNumber property
        [Attributes.Form_Property("Номер графы")]
        public int GraphNumber
        {
            get
            {
                if (GetErrors(nameof(GraphNumber)) == null)
                {
                    return (int)_dataAccess.Get(nameof(GraphNumber));
                }
                else
                {
                    return _GraphNumber_Not_Valid;
                }
            }
            set
            {
                _GraphNumber_Not_Valid = value;
                if (GetErrors(nameof(GraphNumber)) == null)
                {
                    _dataAccess.Set(nameof(GraphNumber), _GraphNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(GraphNumber));
            }
        }
        private int _GraphNumber_Not_Valid;
        private void GraphNumber_Validation()
        {
            ClearErrors(nameof(RowNumber));
        }
        //GraphNumber property

        //Comment property
        [Attributes.Form_Property("Комментарий")]
        public string Comment
        {
            get
            {
                if (GetErrors(nameof(Comment)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Comment));
                }
                else
                {
                    return _Comment_Not_Valid;
                }
            }
            set
            {
                _Comment_Not_Valid = value;
                if (GetErrors(nameof(Comment)) == null)
                {
                    _dataAccess.Set(nameof(Comment), _Comment_Not_Valid);
                }
                OnPropertyChanged(nameof(Comment));
            }
        }
        private string _Comment_Not_Valid;
        private void Comment_Validation()
        {
            ClearErrors(nameof(RowNumber));
        }
        //Comment property

        public override bool Object_Validation()
        {
            return false;
        }
    }
}
