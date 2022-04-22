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

        #region ListRad
        private IEnumerable<Radionuclide> _ListRad;
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

        private void CommandBinding_Add(object sender, RoutedEventArgs e)
        {
            calc.Calc_List.Add((Radionuclide)e.Source);
        }

        private void CommandBinding_Delete(object sender, RoutedEventArgs e)
        {
            var tmp = Radionuclide_List_My.SelectedIndex;
            calc.Calc_List.Remove((Radionuclide)e.Source);
            if (tmp < calc.Calc_List.Count)
            {
                Radionuclide_List_My.SelectedIndex = tmp;
            }
            else
            {
                Radionuclide_List_My.SelectedIndex = calc.Calc_List.Count - 1;
            }
        }

        public void Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).Content == "Жидкие РАО" && (bool)((RadioButton)sender).IsChecked) 
            {
                calc.Calc_List.TWG = "w";
                calc.Lib_List.TWG = "w";
            }
            if (((RadioButton)sender).Content == "Твердые РАО" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.Calc_List.TWG = "t";
                calc.Lib_List.TWG = "t";
            }
            if (((RadioButton)sender).Content == "Газообразные РАО" && (bool)((RadioButton)sender).IsChecked)
            {
                calc.Calc_List.TWG = "g";
                calc.Lib_List.TWG = "g";
            }
            ListRad = calc.Lib_List.Filter();
            calc.Calc_List.Clear();
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