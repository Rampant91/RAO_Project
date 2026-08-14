using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Form_31VM() { }

        public Form_31VM(Report report) : base(report) { }

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
            Reports = reps;

        }

        public override int RowCount
        {
            get => _rowCount; 
            set
            { 
                _rowCount = value;
                OnPropertyChanged();
            }
        }
        public override int CurrentPage
        {
            get => _rowCount;
            set
            {
                _rowCount = value;
                OnPropertyChanged();
            }
        }
        public override int TotalPages
        {
            get => _rowCount;
        }
        public override int TotalRows
        {
            get => _rowCount;
        }


    }
}
