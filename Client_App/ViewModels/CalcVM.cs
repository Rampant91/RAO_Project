using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Views;
using Models.Collections;
using Models.DBRealization;
using DynamicData;
using Models.Abstracts;
using Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ReactiveUI;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Collections;
using FirebirdSql.Data.FirebirdClient;
using Client_App.Long_Visual;
using RAO_Calculator_Library;
using Avalonia.Interactivity;

namespace Client_App.ViewModels
{
    public class CalcVM : BaseVM, INotifyPropertyChanged
    {
        Calculator calc = new Calculator();

        #region cod_RAO
        private string _cod_RAO = "***********";
        public string cod_RAO
        {
            get => _cod_RAO;
            set
            {
                if (_cod_RAO != value)
                {
                    _cod_RAO = value;
                    OnPropertyChanged("cod_RAO");
                }
            }
        }
        #endregion

        #region type_RAO
        private string _type_RAO = "_Выбрать тип РАО";
        public string type_RAO
        {
            get => _type_RAO;
            set
            {
                if (_type_RAO != value)
                {
                    _type_RAO = value;
                    OnPropertyChanged("type_RAO");
                }
            }
        }
        #endregion

        #region TRAO
        private bool _TRAO = true;
        public bool TRAO
        {
            get => _TRAO;
            set
            {
                if (_TRAO != value)
                {
                    _TRAO = value;
                    OnPropertyChanged("TRAO");
                }
            }
        }
        #endregion

        #region WRAO
        private bool _WRAO = false;
        public bool WRAO
        {
            get => _WRAO;
            set
            {
                if (_WRAO != value)
                {
                    _WRAO = value;
                    OnPropertyChanged("WRAO");
                }
            }
        }
        #endregion

        #region GRAO
        private bool _GRAO = false;
        public bool GRAO
        {
            get => _GRAO;
            set
            {
                if (_GRAO != value)
                {
                    _GRAO = value;
                    OnPropertyChanged("GRAO");
                }
            }
        }
        #endregion

        #region ListRad
        private IEnumerable<Radionuclide> _ListRad = null;
        public IEnumerable<Radionuclide> ListRad
        {
            get => _ListRad;
            set
            {
                if (_ListRad != value)
                {
                    _ListRad = value;
                    OnPropertyChanged("ListRad");
                }
            }
        }
        #endregion

        #region MyListRad
        private IEnumerable<Radionuclide> _MyListRad = null;
        public IEnumerable<Radionuclide> MyListRad
        {
            get => _MyListRad;
            set
            {
                if (_MyListRad != value)
                {
                    _MyListRad = value;
                    OnPropertyChanged("MyListRad");
                }
            }
        }
        #endregion

        #region newListRad
        private IEnumerable<Radionuclide> _newListRad = null;
        public IEnumerable<Radionuclide> newListRad
        {
            get => _newListRad;
            set
            {
                if (_newListRad != value)
                {
                    _newListRad = value;
                    OnPropertyChanged("newListRad");
                }
            }
        }
        #endregion

        #region SelectedItems
        private Radionuclide _SelectedItem;
        public Radionuclide SelectedItem
        {
            get =>_SelectedItem; 
            set 
            { 
                _SelectedItem = value; 
                OnPropertyChanged("SelectedItem"); 
            }
        }
        private Radionuclide _SelectedItemM;
        public Radionuclide SelectedItemM
        {
            get => _SelectedItemM;
            set
            {
                _SelectedItemM = value;
                OnPropertyChanged("SelectedItemM");
            }
        }
        #endregion

        #region AddRad
        public void CommandBinding_Add()
        {
            if (SelectedItem is Radionuclide)
            {
                if (TRAO) 
                {
                    SelectedItem.TWG = "t";
                }
                if (WRAO)
                {
                    SelectedItem.TWG = "w";
                }
                if (GRAO)
                {
                    SelectedItem.TWG = "g";
                }

                calc.Calc_List.Add(SelectedItem);
                MyListRad = calc.Calc_List;
                newListRad = MyListRad;
            }
        }
        #endregion

        #region DeleteRad
        public void CommandBinding_Delete()
        {
            if (SelectedItemM is Radionuclide)
            {
                calc.Calc_List.Remove(SelectedItemM);
                MyListRad = calc.Calc_List;
            }
        }

        #endregion

        #region Event
        public void Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).Content == "Жидкие РАО" && (bool)((RadioButton)sender).IsChecked) 
            {
                WRAO = true;
                TRAO = false;
                GRAO = false;
                calc.Calc_List.TWG = "w";
                calc.Lib_List.TWG = "w";
            }
            if (((RadioButton)sender).Content == "Твердые РАО" && (bool)((RadioButton)sender).IsChecked)
            {
                TRAO = true;
                WRAO = false;
                GRAO = false;
                calc.Calc_List.TWG = "t";
                calc.Lib_List.TWG = "t";
            }
            if (((RadioButton)sender).Content == "Газообразные РАО" && (bool)((RadioButton)sender).IsChecked)
            {
                GRAO = true;
                TRAO = false;
                WRAO = false;
                calc.Calc_List.TWG = "g";
                calc.Lib_List.TWG = "g";
            }
            cod_RAO = calc.Code;
            ListRad = calc.Lib_List.Filter();
            calc.Calc_List.Clear();
        }
        #endregion

        #region Culc
        public void CommandBinding_Culculate()
        {
            newListRad = MyListRad;
        }
        #endregion

        public void MenuItem_Click(string num)
        {

            //calc.num9 = ((string)(((MenuItem)sender).Header.ToString())).Split(' ')[0].Split('_')[1];
            calc.num9 = num;
            type_RAO = calc.num9;
            cod_RAO = calc.Code;
        }

        public CalcVM() 
        {
            Init();
        }

        #region Init
        public void Init()
        {
            calc.Lib_List.AddFromDB(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = data\newDB.mdb");
            ListRad = calc.Lib_List.Filter();
            calc.Calc_List.TWG = "t";
            cod_RAO=calc.Code;
        }
        #endregion

        #region INotifyPropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}