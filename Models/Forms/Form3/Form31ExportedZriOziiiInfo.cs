using Models.Comparers.FormContent;
using Models.Forms.DataAccess;
using Models.Interfaces;
using Models.Passports;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Models.Forms.Form3
{
    [Serializable]
    [Table(name: "form_31_table")]
    public class Form31ExportedZriOziiiInfo : INotifyPropertyChanged, INotifyDataErrorInfo, ICopiable
    {
        #region Constructor
        public Form31ExportedZriOziiiInfo()
        {
        }
        public Form31ExportedZriOziiiInfo(Form31 form31)
        {
            _form31 = form31;
        }
        #endregion

        [Key]
        public int Id { get; set; }

        #region ForeignKey Form31Id
        public int Form31Id { get; set; }

        private Form31 _form31;

        [ForeignKey(nameof(Form31Id))]
        public Form31 Form31
        {
            get
            {
                return _form31;
            }
            private set
            {
                _form31 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Properties

        #region RadionuclidComposition
        private string _radionuclidComposition;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string RadionuclidComposition
        {
            get => _radionuclidComposition;
            set
            {
                _radionuclidComposition = value ?? string.Empty;
                ValidateString(nameof(RadionuclidComposition), _radionuclidComposition);
                OnPropertyChanged();
            }
        }
        #endregion

        #region Count
        private int? _count = null;
        public int? Count
        {
            get => _count;
            set
            {
                _count = value;
                ValidateNullableInt(nameof(Count), _count);
                OnPropertyChanged();
            }
        }
        #endregion

        #region TotalActivity
        private double? _totalActivity = null;
        public double? TotalActivity
        {
            get => _totalActivity;
            set
            {
                _totalActivity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #endregion

        #region ICopiable
        #region ConvertToTSVstring
        /// <summary>
        /// </summary>
        /// <returns>Возвращает строку с записанными данными в формате TSV(Tab-Separated Values) </returns>
        public string ConvertToTSVstring()
        {
            var str =
            $"{RadionuclidComposition}\t" +
            $"{Count}\t" +
            $"{TotalActivity}";
            return str;
        }
        #endregion
        #region PasteParsedTSVstring
        public void PasteParsedTSVstring(string[] parsedTSVstring)
        {
            if (parsedTSVstring.Length is not 3) return;


            RadionuclidComposition = parsedTSVstring[0];
            Count = FormStringHelper.ConvertStringToInt(parsedTSVstring[1]);
            TotalActivity = FormStringHelper.ConvertStringToFloat(parsedTSVstring[2]);
        }
        #endregion
        #endregion

        #region Validation
        public void ValidateAll()
        {
            ValidateString(nameof(RadionuclidComposition), _radionuclidComposition);
            ValidateNullableInt(nameof(Count), _count);
        }

        private void ValidateString(string propertyName, string str)
        {
            ClearErrors(propertyName);
            if (string.IsNullOrEmpty(str))
                AddError(propertyName, "Поле не заполнено");
        }
        private void ValidateNullableInt(string propertyName, int? str)
        {
            ClearErrors(propertyName);
            if (str is null)
                AddError(propertyName, "Поле не заполнено");
        }
        #endregion

        #region NotifyDataError
        private readonly Dictionary<string, List<string>> _errors = new();

        // Добавление ошибки
        private void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        // Очистка ошибок свойства
        private void ClearErrors(string propertyName)
        {
            if (_errors.Remove(propertyName))
                OnErrorsChanged(propertyName);
        }

        // INotifyDataErrorInfo
        public bool HasErrors => _errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return _errors.SelectMany(x => x.Value);

            return _errors.TryGetValue(propertyName, out var errors) ? errors : Enumerable.Empty<string>();
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

