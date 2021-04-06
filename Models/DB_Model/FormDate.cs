using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DB_Model
{
    class FormDate
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        private DateTimeOffset _date = DateTimeOffset.MinValue;
        public DateTimeOffset Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }
        public FormDate() { }
        public FormDate(int id, DateTimeOffset date)
        {
            Id = id;
            Date = date;
        }
    }
}
