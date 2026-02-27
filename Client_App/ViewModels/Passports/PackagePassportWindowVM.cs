using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Passports;
using ReactiveUI;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Passports
{
    public class PackagePassportWindowVM : BaseVM, INotifyPropertyChanged
    {
        #region Properties

        private PackagePassport _passport;
        public PackagePassport Passport
        {
            get
            {
                return _passport;
            }
            set
            {
                _passport = value;
                OnPropertyChanged();
            }
        }

        private CharacteristicPrimaryPackage _selectedCharacteristic;
        public CharacteristicPrimaryPackage SelectedCharacteristic
        {
            get
            {
                return _selectedCharacteristic;
            }
            set
            {
                _selectedCharacteristic = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> RaoNamesCollection
        {
            get
            {
                return new ObservableCollection<string>(Spravochniks.SprRadionuclids.Select(r => r.latinName));
            }
        }
        #endregion

        #region Constructor
        public PackagePassportWindowVM()
        {

            var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                .FirstOrDefault(w => w.IsActive);

            AddRadionuclid = ReactiveCommand.Create<CharacteristicPrimaryPackage>(async characteristic =>
            {
                characteristic.RadionuclidsList.Add(new Radionuclid(characteristic));
            });

            DeleteRadionuclid = ReactiveCommand.Create<Radionuclid>(radionuclid =>
            {
                radionuclid.Characteristic?.RadionuclidsList.Remove(radionuclid);
            });

            AddPrimaryPackage = ReactiveCommand.Create(() =>
            {
                Passport.ContentCharacteristics.Add(new CharacteristicPrimaryPackage(Passport));
            });
            DeletePrimaryPackage = ReactiveCommand.Create<CharacteristicPrimaryPackage>(async characteristic =>
            {
                int index = Passport.ContentCharacteristics.IndexOf(characteristic);
                if (index > 0)
                    Passport.ContentCharacteristics.RemoveAt(index);
            });


            _passport = new PackagePassport();

            


            Dispatcher.UIThread.InvokeAsync(() => Passport.ContentCharacteristics.Add(new CharacteristicPrimaryPackage(Passport)));

            
        }

        #endregion

        #region Commands

        public ICommand AddRadionuclid { get; }

        public ICommand DeleteRadionuclid { get; }
        public ICommand SelectAllPrimaryPackages { get; }
        public ICommand AddPrimaryPackage { get; }
        public ICommand DeletePrimaryPackage { get; }

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
