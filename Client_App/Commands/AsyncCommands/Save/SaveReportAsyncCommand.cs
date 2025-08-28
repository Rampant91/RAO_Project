using System;
using Client_App.ViewModels;
using Models.Collections;
using Models.DBRealization;
using System.Threading.Tasks;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;

namespace Client_App.Commands.AsyncCommands.Save;

//  Сохранить отчет
public class SaveReportAsyncCommand : BaseAsyncCommand
{
    private dynamic VM 
    {
        get 
        { 
            switch (_formType)
            {
                case "1.0":
                        return _form10VM;
                case "2.0":
                        return _form20VM;
                case "1.1":
                    return _formVM;
                case "1.2":
                    return _formVM;
                case "1.3":
                    return _formVM;
                case "1.4":
                    return _formVM;
                case "1.5":
                    return _formVM;
                case "1.6":
                    return _formVM;
                case "1.7":
                    return _formVM;
                case "1.8":
                    return _formVM;
                case "1.9":
                    return _formVM;
                default:
                    return _changeOrCreateVM;
            }
        }
    }

    private readonly ChangeOrCreateVM _changeOrCreateVM = null!;

    private readonly BaseFormVM _formVM = null!;
    private readonly Form_10VM _form10VM = null!;
    private readonly Form_20VM _form20VM = null!;

    private readonly string _formType = null!;
    private Report Storage => VM is BaseFormVM ? VM.Report : VM.Storage;
    private Reports? Storages => VM is BaseFormVM ? VM.Reports : VM.Storages;

    public SaveReportAsyncCommand(BaseVM vm)
    {
        switch (vm)
        {
            case Form_10VM form10VM:
            {
                _formType = form10VM.FormType;
                _form10VM = form10VM;
                break;
            }
            case Form_20VM form20VM:
            {
                _formType = form20VM.FormType;
                _form20VM = form20VM;
                break;
            }
            case BaseFormVM formVM:
            {
                _formType = formVM.FormType;
                _formVM = formVM;
                break;
            }
            case ChangeOrCreateVM changeOrCreateVM:
            {
                _formType = changeOrCreateVM.FormType;
                _changeOrCreateVM = changeOrCreateVM;
                break;
            }
        }
    }

    public SaveReportAsyncCommand(Form_10VM formViewModel)
    {
        _formType = formViewModel.FormType;
        _form10VM = formViewModel;
    }
    public SaveReportAsyncCommand(Form_20VM formViewModel)
    {
        _formType = formViewModel.FormType;
        _form20VM = formViewModel;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        if (VM.DBO != null)
        {
            var tmp = new Reports { Master = Storage };
            if (tmp.Master.Rows10.Count != 0)
            {
                tmp.Master.Rows10[1].OrganUprav.Value = tmp.Master.Rows10[0].OrganUprav.Value;
                tmp.Master.Rows10[1].RegNo.Value = tmp.Master.Rows10[0].RegNo.Value;
            }
            if (tmp.Master.Rows20.Count != 0)
            {
                tmp.Master.Rows20[1].OrganUprav.Value = tmp.Master.Rows20[0].OrganUprav.Value;
                tmp.Master.Rows20[1].RegNo.Value = tmp.Master.Rows20[0].RegNo.Value;
            }
            VM.DBO.Reports_Collection.Add(tmp);
            VM.DBO = null;
        }
        else if (Storages != null && _formType is not ("1.0" or "2.0") && !Storages.Report_Collection.Contains(Storage))
        {
            Storages.Report_Collection.Add(Storage);
        }

        if (Storages != null)
        {
            if (Storages.Master.Rows10.Count != 0)
            {
                Storages.Master.Rows10[1].OrganUprav.Value = Storages.Master.Rows10[0].OrganUprav.Value;
                Storages.Master.Rows10[1].RegNo.Value = Storages.Master.Rows10[0].RegNo.Value;
            }
            if (Storages.Master.Rows20.Count != 0)
            {
                Storages.Master.Rows20[1].OrganUprav.Value = Storages.Master.Rows20[0].OrganUprav.Value;
                Storages.Master.Rows20[1].RegNo.Value = Storages.Master.Rows20[0].RegNo.Value;
            }
            Storages.Report_Collection.Sorted = false;
            await Storages.Report_Collection.QuickSortAsync();
        }
        var db = StaticConfiguration.DBModel;
        try
        {
            Storage.ReportChangedDate = DateTime.Now;
            await db.SaveChangesAsync();
            VM.IsCanSaveReportEnabled = false;
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
        }
    }
}