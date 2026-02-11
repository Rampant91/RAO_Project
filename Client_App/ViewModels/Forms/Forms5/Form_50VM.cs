using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Save;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form5;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms5
{
    public class Form_50VM : BaseVM, INotifyPropertyChanged
    {
        #region Storages

        private Reports? _reports;
        public Reports? Storages
        {
            get => _reports;
            set
            {
                if (_reports != value)
                {
                    _reports = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Storage

        private Report _storage;
        public Report Storage
        {
            get => _storage;
            set
            {
                if (_storage != value)
                {
                    _storage = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region DBO

        private DBObservable _DBO;
        public DBObservable DBO
        {
            get => _DBO;
            set
            {
                if (_DBO != value)
                {
                    _DBO = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region FormType

        public string FormType { get; set; } = "5.0";

        #endregion

        #region FederalExecutiveAuthorityList

        public ICollection<string> FederalExecutiveAuthorityList
        {
            get
            {
                return new List<string>()
                {
                    "Министерство здравоохранения Российской Федерации",
                    "Министерство промышленности и торговли Российской Федерации",
                    "Министерство энергетики Российской Федерации",
                    "Министерство науки и высшего образования Российской Федерации",
                    "Федеральное агентство по недропользованию",
                    "Федеральное агентство по техническому регулированию и метрологии",
                    "Федеральное агентство морского и речного транспорта",
                    "Федеральное медико-биологическое агентство"
                };
            }
        }

        #endregion

        public string WindowHeader => $"Форма {Storage.FormNum_DB}: Титульный лист организации";



        #region Properties

        #region SelectedReports

        private Reports _selectedReports;
        public Reports SelectedReports
        {
            get => _selectedReports;
            set
            {
                if (_selectedReports == value) return;
                _selectedReports = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        public ICommand SaveReport => new SaveReportAsyncCommand(this);             //  Сохранить отчет
        public ICommand ChangeReportOrder => new ChangeReportOrderAsyncCommand(this);       //  Поменять местами юр. лицо и обособленное подразделение

        #endregion

        #region Constructor

        public Form_50VM() { }

        public Form_50VM(in DBObservable reps)
        {
            Storage = new Report { FormNum_DB = "5.0" };

            var ty1 = (Form50)FormCreator.Create("5.0");
            ty1.NumberInOrder_DB = 1;
            Storage.Rows50.Add(ty1);
            DBO = reps;
        }

        public Form_50VM(string formNum, in Report rep)
        {
            if (rep.FormNum_DB is "5.0")
            {
                Storage = rep;
            }

            FormType = formNum;
            StaticConfiguration.DBModel.SaveChanges();
        }

        #endregion

        #region OnPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
