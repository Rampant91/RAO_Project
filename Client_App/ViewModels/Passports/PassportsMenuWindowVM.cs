using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.Change;
using Client_App.Commands.AsyncCommands.ExcelExport.Passports;
using Models.DBRealization;
using Models.Passports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Passports
{
    public class PassportsMenuWindowVM : BaseVM, INotifyPropertyChanged
    {

        #region Properties

        DBModel dbm => StaticConfiguration.DBModel;

        public ObservableCollection<PackagePassport> Passports
        {
            get
            {
                return new ObservableCollection<PackagePassport>(
                    dbm.package_passport);
            }
        }
        private PackagePassport _selectedPassport;
        public PackagePassport SelectedPassport
        {
            get => _selectedPassport;
            set
            {
                _selectedPassport = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands

        public ICommand AddPackagePassport { get; set; }
        public ICommand ChangePackagePassport { get; set; }
        public ICommand ExcelExportPackagePassport { get; set; }
        public ICommand DeletePackagePassport { get; set; }
        #endregion

        #region Constructor
        public PassportsMenuWindowVM ()
        {
            AddPackagePassport = new AddPackagePassportAsyncCommand(this);
            ChangePackagePassport = new ChangePackagePassportAsyncCommand();
            ExcelExportPackagePassport = new ExcelExportPackagePassport();
            DeletePackagePassport = new DeletePackagePassportAsyncCommand(this);
        }
        #endregion

        #region Functions
        public void UpdatePassports ()
        {
            OnPropertyChanged(nameof(Passports));
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
