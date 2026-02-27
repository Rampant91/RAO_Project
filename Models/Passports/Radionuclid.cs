using Models.Collections;
using System;
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
    public class Radionuclid : INotifyPropertyChanged
    {
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
                OnPropertyChanged();
            }
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
