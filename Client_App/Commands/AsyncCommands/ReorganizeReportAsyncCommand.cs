using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands
{
    public class ReorganizeReportAsyncCommand : BaseAsyncCommand
    {
        private readonly dynamic _vm;

        public ReorganizeReportAsyncCommand(BaseVM vm)
        {
            if (vm is ChangeOrCreateVM changeOrCreateVM)
            {
                _vm = changeOrCreateVM;
            }
            else if (vm is Form_10VM form10VM)
            {
                _vm = form10VM;
            }
        }

        private Report Storage => _vm.Storage;

        public override async Task AsyncExecute(object? parameter)
        {
            if (_vm is Form_10VM form_10VM)
            {
                //  Если подразделение реорганизуется в юр. лицо,
                // поля Территориального обособленного подразделения очищаются,
                // т.к. после реорганизации они не должны больше использоваться
                if (form_10VM.IsSeparateDivision) 
                {
                    Storage.Rows10[1].SubjectRF.Value = "";
                    Storage.Rows10[1].JurLico.Value = "";
                    Storage.Rows10[1].ShortJurLico.Value = "";
                    Storage.Rows10[1].JurLicoAddress.Value = "";
                    Storage.Rows10[1].JurLicoFactAddress.Value = "";
                    Storage.Rows10[1].GradeFIO.Value = "";
                    Storage.Rows10[1].Telephone.Value = "";
                    Storage.Rows10[1].Fax.Value = "";
                    Storage.Rows10[1].Email.Value = "";
                    Storage.Rows10[1].Okpo.Value = "";
                    Storage.Rows10[1].Okved.Value = "";
                    Storage.Rows10[1].Okogu.Value = "";
                    Storage.Rows10[1].Oktmo.Value = "";
                    Storage.Rows10[1].Inn.Value = "";
                    Storage.Rows10[1].Kpp.Value = "";
                    Storage.Rows10[1].Okopf.Value = "";
                    Storage.Rows10[1].Okfs.Value = "";
                }
                //реорганизация формы
                form_10VM.IsSeparateDivision = !form_10VM.IsSeparateDivision;
            }
        }
    }
}
