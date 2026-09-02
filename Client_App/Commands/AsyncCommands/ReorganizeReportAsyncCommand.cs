using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;
using Models.Collections;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Avalonia.Controls;
using System.Threading.Tasks;
using Client_App.ViewModels.Forms.Forms3;

namespace Client_App.Commands.AsyncCommands;

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
        else if (vm is Form_20VM form20VM)
        {
            _vm = form20VM;
        }
        else if (vm is Form_30VM form30VM)
        {
            _vm = form30VM;
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
                // Предупреждение пользователя
                var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Продолжить" },
                            new ButtonDefinition { Name = "Отмена", IsCancel = true }
                        ],
                        CanResize = true,
                        ContentTitle = "Реорганизация",
                        ContentMessage = "После реорганизации поля территориального обособленного подразделения очистятся",
                        MinWidth = 300,
                        MinHeight = 125,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    }).Show());

                switch (answer)
                {
                    case "Продолжить":
                    {
                        //обработка пройдет дальше
                        break;
                    }
                    case null or "Отмена":
                    {
                        // обработка остановится
                        return;
                    }
                }
                //очищение содержимого полей
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
                //реорганизация формы
                    
            }
            form_10VM.IsSeparateDivision = !form_10VM.IsSeparateDivision;
        }
        else if (_vm is Form_20VM form_20VM)
        {

            //  Если подразделение реорганизуется в юр. лицо,
            // поля Территориального обособленного подразделения очищаются,
            // т.к. после реорганизации они не должны больше использоваться

            if (form_20VM.IsSeparateDivision)
            {
                // Предупреждение пользователя
                var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Продолжить" },
                            new ButtonDefinition { Name = "Отмена", IsCancel = true }
                        ],
                        CanResize = true,
                        ContentTitle = "Реорганизация",
                        ContentMessage = "После реорганизации поля территориального обособленного подразделения очистятся",
                        MinWidth = 300,
                        MinHeight = 125,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    }).Show());

                switch (answer)
                {
                    case "Продолжить":
                    {
                        //обработка пройдет дальше
                        break;
                    }
                    case null or "Отмена":
                    {
                        // обработка остановится
                        return;
                    }
                }
                //очищение содержимого полей
                Storage.Rows20[1].SubjectRF.Value = "";
                Storage.Rows20[1].JurLico.Value = "";
                Storage.Rows20[1].ShortJurLico.Value = "";
                Storage.Rows20[1].JurLicoAddress.Value = "";
                Storage.Rows20[1].JurLicoFactAddress.Value = "";
                Storage.Rows20[1].GradeFIO.Value = "";
                Storage.Rows20[1].Telephone.Value = "";
                Storage.Rows20[1].Fax.Value = "";
                Storage.Rows20[1].Email.Value = "";
                Storage.Rows20[1].Okpo.Value = "";
                Storage.Rows20[1].Okved.Value = "";
                Storage.Rows20[1].Okogu.Value = "";
                Storage.Rows20[1].Oktmo.Value = "";
                Storage.Rows20[1].Inn.Value = "";
                Storage.Rows20[1].Kpp.Value = "";
                Storage.Rows20[1].Okopf.Value = "";
                Storage.Rows20[1].Okfs.Value = "";
                //реорганизация формы
            }
            form_20VM.IsSeparateDivision = !form_20VM.IsSeparateDivision;
        }
        else if (_vm is Form_30VM form_30VM)
        {

            //  Если подразделение реорганизуется в юр. лицо,
            // поля Территориального обособленного подразделения очищаются,
            // т.к. после реорганизации они не должны больше использоваться

            if (form_30VM.IsSeparateDivision)
            {
                // Предупреждение пользователя
                var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Продолжить" },
                            new ButtonDefinition { Name = "Отмена", IsCancel = true }
                        ],
                        CanResize = true,
                        ContentTitle = "Реорганизация",
                        ContentMessage = "После реорганизации поля территориального обособленного подразделения очистятся",
                        MinWidth = 300,
                        MinHeight = 125,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    }).Show());

                switch (answer)
                {
                    case "Продолжить":
                        {
                            //обработка пройдет дальше
                            break;
                        }
                    case null or "Отмена":
                        {
                            // обработка остановится
                            return;
                        }
                }
                //очищение содержимого полей
                Storage.Rows30[1].SubjectRF.Value = "";
                Storage.Rows30[1].JurLico.Value = "";
                Storage.Rows30[1].ShortJurLico.Value = "";
                Storage.Rows30[1].JurLicoAddress.Value = "";
                Storage.Rows30[1].JurLicoFactAddress.Value = "";
                Storage.Rows30[1].GradeFIO.Value = "";
                Storage.Rows30[1].Telephone.Value = "";
                Storage.Rows30[1].Fax.Value = "";
                Storage.Rows30[1].Email.Value = "";
                Storage.Rows30[1].Okpo.Value = "";
                Storage.Rows30[1].Okved.Value = "";
                Storage.Rows30[1].Okogu.Value = "";
                Storage.Rows30[1].Oktmo.Value = "";
                Storage.Rows30[1].Inn.Value = "";
                Storage.Rows30[1].Kpp.Value = "";
                Storage.Rows30[1].Okopf.Value = "";
                Storage.Rows30[1].Okfs.Value = "";
                //реорганизация формы
            }
            form_30VM.IsSeparateDivision = !form_30VM.IsSeparateDivision;
        }
    }
}