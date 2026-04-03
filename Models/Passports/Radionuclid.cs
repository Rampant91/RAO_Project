using Models.Collections;
using Spravochniki;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Models.Passports
{

    [Serializable]
    [Table(name: "radionuclid")]
    public class Radionuclid : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private void ValidatePostiveDouble(string propertyName, double? value)
        {
            ClearErrors(propertyName);
            if (value == null) return;

            if (value < 0)
                AddError(propertyName, "Число должно быть положительным");
        }
        public Radionuclid()
        {
            _characteristic = new CharacteristicPrimaryPackage();
        }
        public Radionuclid(CharacteristicPrimaryPackage characteristic)
        {
            _characteristic = characteristic;
        }

        [NotMapped]
        private CharacteristicPrimaryPackage _characteristic;
        public CharacteristicPrimaryPackage Characteristic
        {
            get
            {
                return _characteristic;
            }
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Characteristic))]
        public int? CharacteristicId { get; set; }

        [NotMapped]
        string _name;

        [MaxLength(8)]
        //Как в справочнике
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }

        }
        [NotMapped]
        double _activity;

        //Экспоненциальный вид
        public double Activity
        {
            get
            {
                return _activity;
            }
            set
            {
                _activity = value;
                ValidatePostiveDouble(nameof(Activity), _activity);
                OnPropertyChanged();
            }
        }

        GroupCode _activityType;

        [NotMapped]
        //Экспоненциальный вид
        public GroupCode ActivityType
        {
            get
            {
                return _activityType;
            }
            set
            {
                _activityType = value;
                OnPropertyChanged();
            }
        }

        bool _isLongLivingActivity;

        [NotMapped]
        //Экспоненциальный вид
        public bool IsLongLivingActivity
        {
            get
            {
                return _isLongLivingActivity;
            }
            set
            {
                _isLongLivingActivity = value;
                OnPropertyChanged();
            }
        }

        // Используется для определения типа удельной активности
        public void GetGroupCode()
        {
            if (Spravochniks.SprRadionuclids.Any(rad => rad.latinName == this.Name))
                ActivityType = Spravochniks.SprRadionuclids
                    .First(r => r.latinName == this.Name)
                    .groupCode;
            else
                ActivityType = GroupCode.NotDefined;
        }
        public void GetIsLongLivingActivity()
        {
            if (Spravochniks.SprRadionuclids.Any(rad => rad.latinName == this.Name))
                IsLongLivingActivity = Spravochniks.SprRadionuclids
                    .First(r => r.latinName == this.Name)
                    .isLongLiving;
            else
                IsLongLivingActivity = false;
        }
        

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
