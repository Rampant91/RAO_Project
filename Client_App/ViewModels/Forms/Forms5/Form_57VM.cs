using Client_App.Commands;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Calculator;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Generate;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.ViewModels.Controls;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms5
{
    public class Form_57VM : BaseFormVM
    {
        public override string FormType => "5.7";

        #region Constructors

        public Form_57VM() { }

        public Form_57VM(Report report) : base(report) { }

        public Form_57VM(in Reports reps)
        {
            var formNum = FormType;
            Report = new Report
            {
                FormNum_DB = formNum,
                Reports = reps
            };

            Reports = reps;

            base.InitializeUserControls();
        }

        #endregion

        #region Commands

        public ICommand CategoryCalculationFromReport => new CategoryCalculationFromReportAsyncCommand();
        public ICommand CopyPasName => new CopyPasNameAsyncCommand();
        public ICommand ExcelExportSourceMovementHistory => new ExcelExportSourceMovementHistoryAsyncCommand();
        public ICommand OpenPas => new OpenPasAsyncCommand();
        public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);
        public ICommand GenerateForm57 => new GenerateForm57AsyncCommand(this);

        #endregion

    }
}
