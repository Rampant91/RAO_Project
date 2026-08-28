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

namespace Models.Forms.Form3
{
    [Serializable]
    [Table(name: "form_32_table_3")]
    public class Form32Identificator : INotifyPropertyChanged, INotifyDataErrorInfo, ICopiable
    {
        #region Constructor
        public Form32Identificator()
        {
        }
        public Form32Identificator(Form32 form32)
        {
            _form32 = form32;
        }
        #endregion
        [Key]
        public int Id { get; set; }

        #region ForeignKey Form32Id
        public int Form32Id { get; set; }

        private Form32 _form32;

        [ForeignKey(nameof(Form32Id))]
        public Form32 Form32
        {
            get
            {
                return _form32;
            }
            private set
            {
                _form32 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region IdName (10.1)
        private string _idName;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string IdName
        {
            get => _idName;
            set
            {
                _idName = value ?? string.Empty;
                ValidateString(nameof(IdName), _idName);
                OnPropertyChanged();
            }
        }
        #endregion

        #region IdValue (10.2)
        private string _idValue;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string IdValue
        {
            get => _idValue;
            set
            {
                _idValue = value ?? string.Empty;
                ValidateString(nameof(IdValue), _idValue);
                OnPropertyChanged();
            }
        }
        #endregion

        #region ICopiable
        #region ConvertToTSVstring

        /// <summary>
        /// </summary>
        /// <returns>Возвращает строку с записанными данными в формате TSV(Tab-Separated Values) </returns>
        public string ConvertToTSVstring()
        {
            var str =
            $"{IdName}\t" +
            $"{IdValue}";
            return str;
        }
        #endregion
        #region PasteParsedTSVstring
        public void PasteParsedTSVstring(string[] parsedTSVstring)
        {
            if (parsedTSVstring.Length is not 2) return;


            IdName = parsedTSVstring[0];
            IdValue = parsedTSVstring[1];
        }
        #endregion
        #endregion

        #region IsContentEqual
        public bool IsContentEqual(Form32Identificator other)
        {
            return FormTextEquality.Equals(IdName, other.IdName)
                && FormTextEquality.Equals(IdValue, other.IdValue);
        }
        #endregion

        #region Validation
        public void ValidateAll()
        {
            ValidateString(nameof(IdName), _idName);
            ValidateString(nameof(IdValue), _idValue);
        }
        private void ValidateString(string propertyName, string str)
        {
            ClearErrors(propertyName);
            if (string.IsNullOrEmpty(str))
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
