using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.Change;
using Client_App.Commands.AsyncCommands.Import;
using Models.DBRealization;
using Models.StoragePoints;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.StoragePoints
{
    public class StoragePointsMenuWindowVM : BaseVM, INotifyPropertyChanged
    {

        #region Properties

        DBModel dbm => StaticConfiguration.DBModel;

        public ObservableCollection<StoragePoint> StoragePoints
        {
            get
            {
                return new ObservableCollection<StoragePoint>(dbm.storage_point);
            }
        }
        private StoragePoint _selectedStoragePoint;
        public StoragePoint SelectedStoragePoint
        {
            get => _selectedStoragePoint;
            set
            {
                _selectedStoragePoint = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands

        public ICommand AddStoragePoint { get; set; }
        public ICommand ChangeStoragePoint { get; set; }
        public ICommand DeleteStoragePoint { get; set; }
        #endregion


        #region Constructor
        public StoragePointsMenuWindowVM ()
        {
            AddStoragePoint = new AddStoragePointAsyncCommand(this);
            ChangeStoragePoint = new ChangeStoragePointAsyncCommand();
            DeleteStoragePoint = new DeleteStoragePointAsyncCommand(this);
        }
        #endregion

        #region Functions
        public void UpdateStoragePoints ()
        {
            OnPropertyChanged(nameof(StoragePoints));
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
