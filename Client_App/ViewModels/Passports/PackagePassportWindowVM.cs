using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.ExcelExport.Passports;
using DynamicData;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
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
                return new ObservableCollection<string>(Spravochniks.SprRadionuclids.Select(r => r.latinName));
            }
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

        }
        private void InitializeCommands()
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
                    Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
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
                    })
                    .ShowDialog(owner));
                }
            });

            ExcelExportPackagePassport = new ExcelExportPackagePassport();
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
