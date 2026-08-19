using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form3;
using Models.Interfaces;
using Models.Passports;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms3
{
    public class Form_31VM : BaseFormVM
    {
        public override string FormType
        {
            get
            {
                return "3.1";
            }
        }
        #region Constructors
        public Form_31VM()
        {
            InitializeCommands();
        }

        public Form_31VM(Report report) : base(report)
        {
            InitializeCommands();
        }

        public Form_31VM(in Reports reps)
        {
            var formNum = FormType;
            Report = new Report
            {
                FormNum_DB = formNum,
                StartPeriod_DB = reps.Report_Collection
                        .Where(x => x.FormNum_DB == formNum && DateOnly.TryParse(x.EndPeriod_DB, out _))
                        .OrderBy(x => DateOnly.Parse(x.EndPeriod_DB))
                        .Select(x => x.EndPeriod_DB)
                        .LastOrDefault() ?? "",
                Reports = reps
            };

            InitializeUserControls();
            InitializeCommands();
            Reports = reps;

            StaticConfiguration.DBModel.ReportCollectionDbSet.Add(Report);
        }
        #endregion

        #region Properties

        Form31ExportedZriOziiiInfo _selectedInfo;
        public Form31ExportedZriOziiiInfo SelectedInfo
        {
            get => _selectedInfo;
            set
            {
                _selectedInfo = value;
                OnPropertyChanged();
            }
        }
        ObservableCollection<Form31ExportedZriOziiiInfo> _selectedInfoCollection;
        public ObservableCollection<Form31ExportedZriOziiiInfo> SelectedInfoCollection
        {
            get => _selectedInfoCollection;
            set
            {
                _selectedInfoCollection = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Commands
        public ICommand AddExportedZriOziiiInfo { get; set; }
        public ICommand DeleteExportedZriOziiiInfo { get; set; }

        public ICommand PasteRows { get; set; }
        #endregion

        #region InitializeCommands
        private void InitializeCommands()
        {
            AddExportedZriOziiiInfo = ReactiveCommand.Create(() =>
            {
                Report.Rows31One.ExportedZriOziiiInfoCollection.Add(new Form31ExportedZriOziiiInfo(Report.Rows31One));
            });
            DeleteExportedZriOziiiInfo = ReactiveCommand.Create<Form31ExportedZriOziiiInfo>(info =>
            {
                info.Form31?.ExportedZriOziiiInfoCollection.Remove(info);
            });
        }
        #endregion
    }
}
