using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Form_32VM() { }

        public Form_32VM(Report report) : base(report) { }

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
            Reports = reps;

        }


    }
}
