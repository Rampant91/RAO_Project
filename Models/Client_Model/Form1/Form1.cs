using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Client_Model
{
    public abstract class Form1:Form
    {
        [Attributes.FormVisual("Форма")]
        public abstract string FormNum { get;}

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
    }
}
