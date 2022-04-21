using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Avalonia.Data;
using Models.Collections;
using ReactiveUI;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Views;
using Models.DBRealization;
using DynamicData;
using Models.Abstracts;
using Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Spravochniki;
using System.IO;
using System.Reactive.Linq;
using System.Reactive;
using System.Threading;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Collections;
using FirebirdSql.Data.FirebirdClient;
using RAO_Calculator_Library;
using Avalonia.Interactivity;

namespace Client_App.ViewModels
{
    public class RAOCodeCalcWindowVM : BaseVM, INotifyPropertyChanged
    {

        Calculator calc = new Calculator();

        #region typesRAOProps
        private List<string> _typesRAOProps = new List<string>() { "11 - воды загрязненные подземные, в т.ч. грунтовые и дренажных систем",
        "12 - воды охлаждения реакторного производства, контурные воды, воды цистерн биологической защиты",
        "13 - растворы хвостовые",
        "14 - конденсаты после упаривания, газоочистки, конденсаты технологические прочие",
        "15 - растворы регенерационные (сливы), растворы промывки экстрагентов, сорбентов",
        "16 - воды лабораторий, трапные, обмывочные воды, растворы после дезактивации, включая воды санпропускников и спецпрачечных, сточные воды прочие",
        "17 - воды бассейнов-хранилищ",
        "18 - воды промысловые",
        "19 - жидкости неорганические прочие",
        "21 - экстрагенты",
        "22 - масла",
        "23 - эмульсии смазочно-охлаждающих жидкостей (СОЖ)",
        "24 - моющие средства (детергенты) за исключением спиртов",
        "25 - спирты",
        "26 - сцинцилляторы органические жидкостные",
        "29 - жидкости органические прочие",
        "31 - хвосты гидрометаллургического, химико-металлургического, разделительного и сублиматного производств, пульпы, образующиеся после нейтрализации",
        "32 - пульпы отработавших ионообменных смол и др. сорбентов органических",
        "33 - шламы и пульпы отработавших сорбентов неорганических, фильтрующих материалов",
        "34 - солевой остаток с солесодержанием до 250 г/л, кубовый остаток",
        "35 - кубовый остаток после упаривания с солесодержанием более 250 г/л, концентрат солевой",
        "36 - взвеси, содержащие продукты коррозии",
        "37 - илы, иловые осадки водоемов-накопителей, осадки, кеки",
        "38 - шламы после очистки трапных вод",
        "39 - прочие пульпы, шламы технологические",
        "94 - жидкие РАО, подготовленные к передаче национальному оператору",
        "99 - прочие типы РАО"};
        public List<string> TypesRAOProps
        {
            get => _typesRAOProps;
            set
            {
                _typesRAOProps = value;
                OnPropertyChanged("typesRAOProps");
            }
        }
        #endregion

        #region SelectidItems
        private string p_SelectedItem;
        public string SelectedItem
        {
            get { return p_SelectedItem; }
            set { p_SelectedItem = value; OnPropertyChanged("SelectedItem"); }
        }
        #endregion

        #region liquidProps
        private bool _liquidProps = false;
        public bool liquidProps
        {
            get => _liquidProps;
            set
            {
                if (_liquidProps != value)
                {
                    _liquidProps = value;
                    OnPropertyChanged("liquidProps");
                }
            }
        }
        #endregion

        public RAOCodeCalcWindowVM()
        {
            calc.Lib_List.AddFromDB(@"Provider = Microsoft.Jet.OLEDB.6.0; Data Source = data\DataBase.mdb");
            var x = 1;
        }

        private void CommandBinding_Add(object sender, RoutedEventArgs e)
        {
            calc.Calc_List.Add((Radionuclide)e.Source);
        }

        private void CommandBinding_Delete(object sender, RoutedEventArgs e)
        {
 
            calc.Calc_List.Remove((Radionuclide)e.Source);
            //if (tmp < calc.Calc_List.Count)
            //{
            //    Radionuclide_List_My.SelectedIndex = tmp;
            //}
            //else
            //{
            //    Radionuclide_List_My.SelectedIndex = calc.Calc_List.Count - 1;
            //}
        }

        private void CommandBinding_Culculate(object sender, EventArgs e)
        {
            //foreach (var item in calc.Calc_List)
            //{
            //    if ((bool)TRAO.IsChecked)
            //    {
            //        item.TWG = "t";
            //    }
            //    if ((bool)WRAO.IsChecked)
            //    {
            //        item.TWG = "w";
            //    }
            //    if ((bool)GRAO.IsChecked)
            //    {
            //        item.TWG = "g";
            //    }
            //}
            //if (calc.Calc_List.Count > 0)
            //{
            //    bool flag = true;
            //    foreach (var item in calc.Calc_List)
            //    {
            //        if (item.UDA <= 0)
            //        {
            //            flag = false;
            //        }
            //    }
            //    if (flag)
            //    {
            //        Calculations culc = new Calculations(calc);
            //        culc.ShowDialog();
            //    }
            //}
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            //if (TRAO != null && (bool)TRAO.IsChecked)
            //{
            //    calc.Calc_List.TWG = "t";
            //    calc.Lib_List.TWG = "t";
            //}
            //if (WRAO != null && (bool)WRAO.IsChecked)
            //{
            //    calc.Calc_List.TWG = "w";
            //    calc.Lib_List.TWG = "w";
            //}
            //if (GRAO != null && (bool)GRAO.IsChecked)
            //{
            //    calc.Calc_List.TWG = "g";
            //    calc.Lib_List.TWG = "g";
            //}
            //Radionuclide_List.ItemsSource = calc.Lib_List.Filter();
            //calc.Calc_List.Clear();
        }



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
