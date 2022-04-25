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

        #region is_Ckecked
        private bool _is_Ckecked1 = true;
        public bool is_Ckecked1
        {
            get => _is_Ckecked1;
            set
            {
                if (_is_Ckecked1 != value)
                {
                    _is_Ckecked1 = value;
                    OnPropertyChanged("is_Ckecked1");
                }
            }
        }
        private bool _is_Ckecked2 = true;
        public bool is_Ckecked2
        {
            get => _is_Ckecked2;
            set
            {
                if (_is_Ckecked2 != value)
                {
                    _is_Ckecked2 = value;
                    OnPropertyChanged("is_Ckecked2");
                }
            }
        }
        #endregion

        #region ozri_V
        private bool _ozri_V = true;
        public bool ozri_V
        {
            get => _ozri_V;
            set
            {
                if (_ozri_V != value)
                {
                    _ozri_V = value;
                    OnPropertyChanged("ozri_V");
                }
            }
        }
        #endregion

        #region sec_R
        private bool _sec_R = true;
        public bool sec_R
        {
            get => _sec_R;
            set
            {
                if (_sec_R != value)
                {
                    _sec_R = value;
                    OnPropertyChanged("sec_R");
                }
            }
        }
        #endregion

        #region thr_R1
        private bool _thr_R1 = false;
        public bool thr_R1
        {
            get => _thr_R1;
            set
            {
                if (_thr_R1 != value)
                {
                    _thr_R1 = value;
                    OnPropertyChanged("thr_R1");
                }
            }
        }
        #endregion

        #region thr_R2
        private bool _thr_R2 = true;
        public bool thr_R2
        {
            get => _thr_R2;
            set
            {
                if (_thr_R2 != value)
                {
                    _thr_R2 = value;
                    OnPropertyChanged("thr_R2");
                }
            }
        }
        #endregion

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
                cod_RAO = calc.Code;
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
            cod_RAO = calc.Code;
        }

        #endregion

        #region Event
        public void Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).Content == "Жидкие РАО" && (bool)((RadioButton)sender).IsChecked) 
            {
                WRAO = true;

                ozri_V = false;
                sec_R = false;
                thr_R1 = true;
                thr_R2 = true;
                is_Ckecked1 = true;
                is_Ckecked2 = true;

                TRAO = false;
                GRAO = false;
                calc.Calc_List.TWG = "w";
                calc.Lib_List.TWG = "w";
            }
            if (((RadioButton)sender).Content == "Твердые РАО" && (bool)((RadioButton)sender).IsChecked)
            {
                TRAO = true;

                ozri_V = true;
                sec_R = true;
                thr_R1 = false;
                thr_R2 = true;
                is_Ckecked1 = true;
                is_Ckecked2 = true;

                WRAO = false;
                GRAO = false;
                calc.Calc_List.TWG = "t";
                calc.Lib_List.TWG = "t";
            }
            if (((RadioButton)sender).Content == "Газообразные РАО" && (bool)((RadioButton)sender).IsChecked)
            {
                GRAO = true;

                ozri_V = false;
                sec_R = false;
                thr_R1 = false;
                thr_R2 = false;
                is_Ckecked1 = true;
                is_Ckecked2 = true;

                TRAO = false;
                WRAO = false;
                calc.Calc_List.TWG = "g";
                calc.Lib_List.TWG = "g";
            }
            calc.num7 = 0;
            calc.num8 = 0;
            cod_RAO = calc.Code;
            ListRad = calc.Lib_List.Filter();
            calc.Calc_List.Clear();
        }

        public void Radio_Checked2(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).Content == "1 - не содержащие ядерные материалы" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num4 = 1;
            }
            if (((RadioButton)sender).Content == "2 - содержащие ядерные материалы" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num4 = 2;
            }
            
            cod_RAO = calc.Code;
        }

        public void Radio_Checked3(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).Content == "0 - удаляемые, класс которых не установлен" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num8 = 0;
            }
            if (((RadioButton)sender).Content == "1 - удаляемые 1 - го класса" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num8 = 1;
            }
            if (((RadioButton)sender).Content == "2 - удаляемые 2 - го класcа" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num8 = 2;
            }
            if (((RadioButton)sender).Content == "3 - удаляемые 3 - го класса" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num8 = 3;
            }
            if (((RadioButton)sender).Content == "4 - удаляемые 4 - го класса" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num8 = 4;
            }
            if (((RadioButton)sender).Content == "5 - удаляемые 5 - го класса" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num8 = 5;
            }
            if (((RadioButton)sender).Content == "6 - удаляемые 6 - го класса" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num8 = 6;
            }
            if (((RadioButton)sender).Content == "7 - особые РАО" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num8 = 7;
            }
            if (((RadioButton)sender).Content == "9 - прочие РАО" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num8 = 9;
            }
            
            cod_RAO = calc.Code;
        }

        public void Radio_Checked4(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).Content == "0 - не подтвергавшиеся переработке способами, перечисленными ниже" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num7 = 0;
            }
            if (((RadioButton)sender).Content == "1 - спрессованные (компактированные)" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num7 = 1;
            }
            if (((RadioButton)sender).Content == "2 - битумированные" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num7 = 2;
            }
            if (((RadioButton)sender).Content == "3 - цементированные" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num7 = 3;
            }
            if (((RadioButton)sender).Content == "4 - остеклованные" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num7 = 4;
            }
            if (((RadioButton)sender).Content == "99 - омоноличенные (отвержденные) другим способом" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num7 = 9;
            }

            cod_RAO = calc.Code;
        }

        public void Radio_Checked5(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).Content == "1 - горючие" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num10 = 1;
            }
            if (((RadioButton)sender).Content == "2 - негорючие" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num10 = 2;
            }

            cod_RAO = calc.Code;
        }

        public void Radio_Checked6(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).Content == "1 - Да" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num2 = 4;
            }
            if (((RadioButton)sender).Content == "2 - Нет" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.num2 = -1;
            }

            cod_RAO = calc.Code;
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
            calc.num10 = 1;
            calc.num4 = 1;
            calc.num7 = 0;
            calc.num8 = 0;
            cod_RAO =calc.Code;
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