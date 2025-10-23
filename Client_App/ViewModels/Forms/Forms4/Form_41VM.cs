using Client_App.Commands;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Calculator;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms4
{
    public class Form_41VM : BaseFormVM
    {
        public override string FormType => "4.1";

        #region Constructors

        public Form_41VM() { }

        public Form_41VM(Report report) : base(report) { }

        public Form_41VM(in Reports reps)
        {
            var formNum = FormType;
            Report = new Report
            {
                FormNum_DB = formNum,
                Reports = reps
            };

            Reports = reps;
        }

        #endregion

        #region Commands

        public ICommand CategoryCalculationFromReport => new CategoryCalculationFromReportAsyncCommand();
        public ICommand CopyPasName => new CopyPasNameAsyncCommand();
        public ICommand ExcelExportSourceMovementHistory => new ExcelExportSourceMovementHistoryAsyncCommand();
        public ICommand OpenPas => new OpenPasAsyncCommand();
        public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);
        public ICommand GenerateForm41 => new GenerateForm41AsyncCommand(this);

        #endregion

    }
}
