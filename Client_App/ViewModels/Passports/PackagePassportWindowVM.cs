using MsBox.Avalonia;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.ExcelExport.Passports;
using DynamicData;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.DBRealization;
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
                var result = new ObservableCollection<string>(LatinRusNamePairs.Select(r => r.Item1));

                return result;
            }
        }
        private List<Tuple<string, string>> _latinRusNames = new List<Tuple<string, string>>();
        // Синонимы для RaoNamesCollection
        private List<Tuple<string, string>> LatinRusNamePairs
        {
            get
            {
                return _latinRusNames;
            }
        }

        // Свойство-фильтр нужного типа
        public AutoCompleteFilterPredicate<object> RadionuclidFilter { 
            get
            {
                return RadionuclidFilterPredicate;
            }
        }
        public bool RadionuclidFilterPredicate(string searchText, object item)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;

            if ( item is not string radionuclidLatinName) return false;

            var pair = LatinRusNamePairs.FirstOrDefault(p => p.Item1 == radionuclidLatinName);

            if (string.IsNullOrWhiteSpace(pair.Item1)
                && string.IsNullOrWhiteSpace(pair.Item2)) return false;

            return pair.Item1.ToLower().Contains(searchText.ToLower())
                || pair.Item2.ToLower().Contains(searchText.ToLower());
        }

        #region SkipChangeTracking

        private bool _skipChangeTracking;
        public bool SkipChangeTracking
        {
            get => _skipChangeTracking;
            set
            {
                if (_skipChangeTracking != value)
                {
                    _skipChangeTracking = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #endregion

        #region Constructor
        public PackagePassportWindowVM()
        {
            InitializeCommands();

            _passport = new PackagePassport();
            

        }
        public PackagePassportWindowVM(int passportId)
        {
            InitializeCommands();
            

            _passport = StaticConfiguration.DBModel.package_passport
                .Include(pas => pas.ContentCharacteristics)
                .ThenInclude(c => c.RadionuclidsList)
                .FirstOrDefault(pas => pas.Id == passportId);

            foreach (var charcteristic in _passport.ContentCharacteristics)
            {
                foreach (var radionuclid in charcteristic.RadionuclidsList)
                {
                    radionuclid.GetGroupCode();
                    radionuclid.GetIsLongLivingActivity();
                }
                charcteristic.UpdateLongLivingActivity();
                charcteristic.UpdateAlphaActivity();
                charcteristic.UpdateBetaGammaActivity();
                charcteristic.UpdateTransuraniumActivity();
                charcteristic.UpdateTritiumActivity();
                charcteristic.UpdateTotalActivity();
            }

        }
        private void InitializeCommands()
        {

            { // Справочник радионуклидов
                List<Tuple<string, string>> result = new List<Tuple<string, string>>();
                var latinRusNamePairs = Spravochniks.SprRadionuclids.Select(r => (r.latinName, r.rusName));
                foreach (var pair in latinRusNamePairs)
                {
                    result.Add(new Tuple<string, string>(pair.latinName, pair.rusName));
                }

                _latinRusNames = result;
            }

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
                if (Passport.ContentCharacteristics.Count <= 1) return;
                
                Passport.ContentCharacteristics.Remove(characteristic);
            });

            SavePassport = ReactiveCommand.Create(async () =>
            {
                try
                {
                    await StaticConfiguration.DBModel.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Dispatcher.UIThread.InvokeAsync(async () => await MessageBoxManager
                    .GetMessageBoxCustom(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Ок" },
                        ],
                        ContentTitle = "Сохранение изменений",
                        ContentHeader = "Ошибка",
                        ContentMessage = $"Произошла ошибка во время попытки сохранения:\n" +
                            $"{ex.Message}",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    }).ShowWindowDialogAsync(owner));
                }
            });

            ExcelExportPackagePassport = new ExcelExportPackagePassportPrikaz();
        }

        #endregion

        #region Commands

        public ICommand AddRadionuclid { get; set; }

        public ICommand DeleteRadionuclid { get; set; }
        public ICommand SelectAllPrimaryPackages { get; set; }
        public ICommand AddPrimaryPackage { get; set; }
        public ICommand DeletePrimaryPackage { get; set; }
        public ICommand SavePassport { get; set; }
        public ICommand ExcelExportPackagePassport { get; set; }

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
