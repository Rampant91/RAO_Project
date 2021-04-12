using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using Models;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Models
{
    public class Forms : INotifyPropertyChanged
    {
        public Forms()
        {
            _Filters = new Filter.Filters<Form>();
        }

        public ObservableCollection<Form> Forms_Collection
        {
            get
            {
                ObservableCollection<Form> tmp = new ObservableCollection<Form>();
                tmp.CollectionChanged += Update;
                var ids=AccessInterface.REDDatabase_Usage.REDDatabase_Methods.GetFormsIDS(MasterForm.FormNum,MasterForm.FormID);
                foreach(var item in ids)
                {
                    tmp.Add(new Form(item));
                }

                return tmp;
            }
        }
        public void Update(object sender,EventArgs args)
        {
            NotifyPropertyChanged("Forms_Collection");
            NotifyPropertyChanged("GetFilteredDictionary");
        }

        Form _MasterForm;
        public Form MasterForm
        {
            get
            {
                return _MasterForm;
            }
            set
            {
                if (value != _MasterForm)
                {
                    _MasterForm = value;
                    NotifyPropertyChanged("MasterForm");
                    Update(null,null);
                }
            }
        }

        Filter.Filters<Form> _Filters;
        public Filter.Filters<Form> Filters
        {
            get
            {
                return _Filters;
            }
            set
            {
                if (value.GetType() == _Filters.GetType())
                {
                    if (value != _Filters)
                    {
                        _Filters = value;
                        NotifyPropertyChanged("Filters");
                        Update(null,null);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<Form> GetFilteredDictionary
        {
            get
            {
                foreach (var item in Filters.CheckAndSort(Forms_Collection))
                {
                    yield return item;
                }
            }
        }
    }
}
