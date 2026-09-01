using Client_App.ViewModels.Forms.Forms4.Items;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form4;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms4
{
    public class Form_40VM : BaseVM, INotifyPropertyChanged
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

        #region IsCanSaveReportEnabled

        private bool _isCanSaveReportEnabled;
        public bool IsCanSaveReportEnabled
        {
            get => _isCanSaveReportEnabled;
            set
            {
                if (_isCanSaveReportEnabled != value)
                {
                    _isCanSaveReportEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region FormType

        public string FormType { get; set; } = "4.0";

        #endregion

        public string WindowHeader => $"Форма {Storage.FormNum_DB}: Титульный лист организации";

        #region Properties

        public IReadOnlyList<SubjectRfCodeItem> SubjectRFItems { get; } =
            Spravochniks.DictionaryOfSubjectRF
                .OrderBy(x => x.Key)
                .Select(x => new SubjectRfCodeItem
                {
                    Code = FormatSubjectRfCode(x.Key),
                    Description = x.Value
                })
                .ToList();

        public ICollection<string> ValidSubjectRfCodes =>
            SubjectRFItems.Select(x => x.Code).ToList();

        public string SubjectRfCodePattern => @"^\d{0,2}$";

        public string? CodeOfSubjectRF
        {
            get
            {
                if (!TryParseSubjectRfCode(Storage.Rows40[0].CodeSubjectRF.Value, out int code)
                    || !Spravochniks.DictionaryOfSubjectRF.ContainsKey(code))
                {
                    return string.IsNullOrWhiteSpace(Storage.Rows40[0].CodeSubjectRF.Value)
                        ? null
                        : Storage.Rows40[0].CodeSubjectRF.Value;
                }

                return FormatSubjectRfCode(code);
            }
            set
            {
                var normalized = string.IsNullOrWhiteSpace(value)
                    ? string.Empty
                    : NormalizeSubjectRfCode(value);

                if (string.Equals(Storage.Rows40[0].CodeSubjectRF.Value, normalized, StringComparison.Ordinal))
                {
                    return;
                }

                Storage.Rows40[0].CodeSubjectRF.Value = normalized;
                ApplySubjectRfNameForCode(normalized);
                OnPropertyChanged();
                OnPropertyChanged(nameof(NameOfSubjectRF));
                OnPropertyChanged(nameof(IsSubjectRfNameEmpty));
            }
        }

        public string NameOfSubjectRF => Storage.Rows40[0].SubjectRF.Value;

        public bool IsSubjectRfNameEmpty => string.IsNullOrWhiteSpace(NameOfSubjectRF);

        private static string FormatSubjectRfCode(int code) =>
            code < 10 ? $"0{code}" : code.ToString();

        private static bool TryParseSubjectRfCode(string? code, out int parsed)
        {
            parsed = 0;
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            return int.TryParse(code.Trim(), out parsed);
        }

        private static string NormalizeSubjectRfCode(string code)
        {
            if (!TryParseSubjectRfCode(code, out int parsed))
            {
                return code.Trim();
            }

            return Spravochniks.DictionaryOfSubjectRF.ContainsKey(parsed)
                ? FormatSubjectRfCode(parsed)
                : code.Trim();
        }

        private void ApplySubjectRfNameForCode(string normalizedCode)
        {
            if (TryParseSubjectRfCode(normalizedCode, out int code)
                && Spravochniks.DictionaryOfSubjectRF.TryGetValue(code, out var name))
            {
                Storage.Rows40[0].SubjectRF.Value = name;
                return;
            }

            if (string.IsNullOrEmpty(normalizedCode))
            {
                Storage.Rows40[0].SubjectRF.Value = string.Empty;
            }
        }

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

        public Form_40VM()
        {
            InitializeEmptyStorage();
        }

        public Form_40VM(in DBObservable reps) : this()
        {
            DBO = reps;
        }

        public Form_40VM(string formNum, in Report rep)
        {
            if (formNum is "4.0")
            {
                Storage = rep;
            }

            FormType = formNum;
            try
            {
                StaticConfiguration.DBModel.SaveChanges();
            }
            catch (Exception ex)
            {
                ServiceExtension.LoggerManager.Error(
                    $"Form_40VM.SaveChanges: {ex.Message}{Environment.NewLine}{ex.StackTrace}",
                    ErrorCodeLogger.Application);
            }
        }

        #endregion

        private void InitializeEmptyStorage()
        {
            Storage = new Report { FormNum_DB = "4.0" };

            var ty1 = (Form40)FormCreator.Create("4.0");
            ty1.NumberInOrder_DB = 1;
            Storage.Rows40.Add(ty1);
        }

        #region OnPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
