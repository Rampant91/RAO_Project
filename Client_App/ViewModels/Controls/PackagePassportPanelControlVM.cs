using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Change;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.ViewModels.Passports;
using Models.Passports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Controls
{
    public class PackagePassportPanelControlVM : INotifyPropertyChanged
    {
        public PackagePassportPanelControlVM()
        {
            //
        }
        #region Properties

        PassportsMenuWindowVM _passportsMenuWindowVM = new();
        public PassportsMenuWindowVM? PassportsMenuWindowVM
        {
            get
            {
                return _passportsMenuWindowVM;
            }
            set
            {
                _passportsMenuWindowVM = value;
                OnPropertyChanged();
            }
        }

        PackagePassport? _selectedPassport;
        public PackagePassport? SelectedPassport
        {
            get
            {
                return _selectedPassport;
            }
            set
            {
                _selectedPassport = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PassportIsExist));
            }
        }
        public bool PassportIsExist
        {
            get
            {
                return SelectedPassport is not null;
            }
        }

        #endregion

        #region Functions
        public void SelectPassport(string num, string type)
        {
            SelectedPassport = PassportsMenuWindowVM.Passports.FirstOrDefault(p =>
            p.PassportNum == num 
            && p.PackageType == type);
        }
        #endregion

        #region Commands

        public ICommand OpenPassportMenu => new OpenPassportMenuWindowAsyncCommand();

        public ICommand ChangePackagePassport => new ChangePackagePassportAsyncCommand();
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
