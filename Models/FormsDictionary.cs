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
using System.Reflection;

namespace Models
{
    public class FormsDictionary : INotifyPropertyChanged
    {
        string _PathToDB;
        public string PathToDB
        {
            get
            {
                return _PathToDB;
            }
            set
            {
                if (value != _PathToDB)
                {
                    _PathToDB = value;
                    NotifyPropertyChanged("PathToDB");
                    Update();
                }
            }
        }
        public FormsDictionary(string PathToDB)
        {
            this.PathToDB = PathToDB;
        }

        int _ChooseID;
        public int ChooseID
        {
            get
            {
                return _ChooseID;
            }
            set
            {
                if (value != _ChooseID)
                {
                    _ChooseID = value;
                    NotifyPropertyChanged("ChooseID");
                    Update();
                }
            }
        }

        public Forms Dictionary
        {
            get
            {
                Forms frms = new Forms();
                Form frm = new Form(_ChooseID);
                frms.MasterForm = frm;

                return frms;
            }
        }

        public void Update()
        {
            NotifyPropertyChanged("Dictionary");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}  