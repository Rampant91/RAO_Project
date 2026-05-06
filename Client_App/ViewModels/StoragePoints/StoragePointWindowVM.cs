using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.ExcelExport.Passports;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using Models.Passports;
using Models.StoragePoints;
using ReactiveUI;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.StoragePoints
{
    public class StoragePointWindowVM : BaseVM, INotifyPropertyChanged
    {
        public StoragePointWindowVM()
        {

            InitializeCommands();
            _storagePoint = new StoragePoint();


        }
        public StoragePointWindowVM(int storagePointId)
        {

            InitializeCommands();
            _storagePoint = StaticConfiguration.DBModel.storage_point
                .FirstOrDefault(sp => sp.Id == storagePointId);
        }

        private void InitializeCommands()
        {

            var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                .FirstOrDefault(w => w.IsActive);


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

        }

        private StoragePoint _storagePoint;

        public StoragePoint StoragePoint
        {
            get
            {
                return _storagePoint;
            }
            set
            {
                _storagePoint = value;
                OnPropertyChanged();
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

        #region Commands

        public ICommand SavePassport { get; set; }

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
