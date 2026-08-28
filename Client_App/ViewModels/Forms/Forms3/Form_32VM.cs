using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form3;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms3
{
    public class Form_32VM : BaseFormVM
    {
        public override string FormType
        {
            get
            {
                return "3.2";
            }
        }

        private Window owner 
        {
            get
            {
                return (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows.FirstOrDefault(w => w.Name == "3.2");
            }
        }

        #region Constructors
        public Form_32VM() { InitializeCommands(); }

        public Form_32VM(Report report) : base(report) { InitializeCommands(); }

        public Form_32VM(in Reports reps)
        {
            var formNum = FormType;
            Report = new Report
            {
                FormNum_DB = formNum,
                StartPeriod =
            {
                Value = reps.Report_Collection
                    .Where(x => x.FormNum_DB == formNum && DateOnly.TryParse(x.EndPeriod_DB, out _))
                    .OrderBy(x => DateOnly.Parse(x.EndPeriod_DB))
                    .Select(x => x.EndPeriod_DB)
                    .LastOrDefault() ?? ""
            },
                Reports = reps
            };

            InitializeUserControls();
            InitializeCommands();
            Reports = reps;

            StaticConfiguration.DBModel.ReportCollectionDbSet.Add(Report);

        }
        #endregion

        #region Properties
        Form32ExportedZriInfo _selectedZriInfo;
        public Form32ExportedZriInfo SelectedZriInfo
        {
            get => _selectedZriInfo;
            set
            {
                _selectedZriInfo = value;
                OnPropertyChanged();
            }
        }
        ObservableCollection<Form32ExportedZriInfo> _selectedZriInfoCollection = [];
        public ObservableCollection<Form32ExportedZriInfo> SelectedZriInfoCollection
        {
            get => _selectedZriInfoCollection;
            set
            {
                _selectedZriInfoCollection = value;
                OnPropertyChanged();
            }
        }

        Form32ContainerInfo _selectedContainerInfo;
        public Form32ContainerInfo SelectedContainerInfo
        {
            get => _selectedContainerInfo;
            set
            {
                _selectedContainerInfo = value;
                OnPropertyChanged();
            }
        }
        ObservableCollection<Form32ContainerInfo> _selectedContainerInfoCollection = [];
        public ObservableCollection<Form32ContainerInfo> SelectedContainerInfoCollection
        {
            get => _selectedContainerInfoCollection;
            set
            {
                _selectedContainerInfoCollection = value;
                OnPropertyChanged();
            }
        }


        Form32Identificator _selectedIdentificator;
        public Form32Identificator SelectedIdentificator
        {
            get => _selectedIdentificator;
            set
            {
                _selectedIdentificator = value;
                OnPropertyChanged();
            }
        }
        ObservableCollection<Form32Identificator> _selectedIdentificatorCollection = [];
        public ObservableCollection<Form32Identificator> SelectedIdentificatorCollection
        {
            get => _selectedIdentificatorCollection;
            set
            {
                _selectedIdentificatorCollection = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand AddExportedZriInfo { get; set; }
        public ICommand PasteExportedZriInfo { get; set; }
        public ICommand DeleteExportedZriInfo { get; set; }
        public ICommand AddContainerInfo { get; set; }
        public ICommand PasteContainerInfo { get; set; }
        public ICommand DeleteContainerInfo { get; set; }
        public ICommand AddIdentificator { get; set; }
        public ICommand PasteIdentificator { get; set; }
        public ICommand DeleteIdentificator { get; set; }
        #endregion

        #region InitializeCommands
        private void InitializeCommands()
        {
            AddExportedZriInfo = ReactiveCommand.Create(() =>
            {
                var item = new Form32ExportedZriInfo(Report.Rows32One);
                item.ValidateAll();
                Report.Rows32One.ExportedZriInfoCollection.Add(item);
            });
            AddContainerInfo = ReactiveCommand.Create(() =>
            {
                var item = new Form32ContainerInfo(Report.Rows32One);
                item.ValidateAll();
                Report.Rows32One.ContainersInfoCollection.Add(item);
            });
            AddIdentificator = ReactiveCommand.Create(() =>
            {
                var item = new Form32Identificator(Report.Rows32One);
                item.ValidateAll();
                Report.Rows32One.IdentificatorsCollection.Add(item);
            });

            PasteExportedZriInfo = new NewPasteRowsAsyncCommand(Report.Rows32One.ExportedZriInfoCollection);
            PasteContainerInfo = new NewPasteRowsAsyncCommand(Report.Rows32One.ContainersInfoCollection);
            PasteIdentificator = new NewPasteRowsAsyncCommand(Report.Rows32One.IdentificatorsCollection);

            DeleteExportedZriInfo = ReactiveCommand.Create<Form32ExportedZriInfo>( async info =>
            {
                if (info == null)
                {
                    await Dispatcher.UIThread.InvokeAsync(() =>
                        SelectedItemIsNullMessage.ShowDialog(owner));
                    return;
                }
                info.Form32?.ExportedZriInfoCollection.Remove(info);
            });
            DeleteContainerInfo = ReactiveCommand.Create<Form32ContainerInfo>(async container =>
            {
                if (container == null)
                {
                    await Dispatcher.UIThread.InvokeAsync(() =>
                        SelectedItemIsNullMessage.ShowDialog(owner));
                    return;
                }
                container.Form32?.ContainersInfoCollection.Remove(container);
            });
            DeleteIdentificator = ReactiveCommand.Create<Form32Identificator>(async identificator =>
            {
                if (identificator == null)
                {
                    #region infoIsNullMessage
                    await Dispatcher.UIThread.InvokeAsync(() =>
                        SelectedItemIsNullMessage.ShowDialog(owner));
                    #endregion
                    return;
                }

                identificator.Form32?.IdentificatorsCollection.Remove(identificator);
            });
        }
        #endregion

        #region Messages
        IMsBoxWindow<ButtonResult> SelectedItemIsNullMessage
        {
            get
            {
                var message = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Удаление",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Выберите строку, которую хотите удалить",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                });
                return message;
            }
        }
        #endregion
    }
}
