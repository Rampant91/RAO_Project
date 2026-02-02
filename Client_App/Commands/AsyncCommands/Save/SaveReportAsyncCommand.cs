using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Client_App.ViewModels.Forms.Forms5;

namespace Client_App.Commands.AsyncCommands.Save;

/// <summary>
/// Сохранение отчёта
/// </summary>
public class SaveReportAsyncCommand : BaseAsyncCommand
{
    private dynamic VM
    {
        get
        {
            if (_formVM != null)
                return _formVM;
            else if (_changeOrCreateVM != null)
                return _changeOrCreateVM;
            else if (_form10VM != null)
                return _form10VM;
            else if (_form20VM != null)
                return _form20VM;
            else if (_form40VM != null)
                return _form40VM;
            else if (_form50VM != null)
                return _form50VM;
            else
                return null;
        }
    }

    private readonly ChangeOrCreateVM _changeOrCreateVM = null!;

    private readonly BaseFormVM _formVM = null!;
    private readonly Form_10VM _form10VM = null!;
    private readonly Form_20VM _form20VM = null!;
    private readonly Form_40VM _form40VM = null!;
    private readonly Form_50VM _form50VM = null!;

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
            case Form_40VM form40VM:
            {
                _formType = form40VM.FormType;
                _form40VM = form40VM;
                break;
                }
            case Form_50VM form50VM:
                {
                    _formType = form50VM.FormType;
                    _form50VM = form50VM;
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
    public SaveReportAsyncCommand(Form_40VM formViewModel)
    {
        _formType = formViewModel.FormType;
        _form40VM = formViewModel;
    }
    public SaveReportAsyncCommand(Form_50VM formViewModel)
    {
        _formType = formViewModel.FormType;
        _form50VM = formViewModel;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        //Если это титульная форма, то разрешаем сохранение только если организаций с такими ОКПО + рег.№ отсутствуют в базе.
        if (_formType is "1.0" or "2.0")
        {
            var dbm = StaticConfiguration.DBModel;
            var window = Desktop.Windows.FirstOrDefault(x => x.Name == _formType);
            try
            {
                var query = dbm.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.DBObservable)
                    .Include(reps => reps.Master_DB).ThenInclude(report => report.Rows10)
                    .Include(reps => reps.Master_DB).ThenInclude(report => report.Rows20)
                    .Where(reps => reps.DBObservable != null);

                var regNum = _formType switch
                {
                    "1.0" => _form10VM.Storage.RegNoRep.Value,
                    "2.0" => _form20VM.Storage.RegNoRep.Value,
                    _ => ""
                };

                var okpo = _formType switch
                {
                    "1.0" => _form10VM.Storage.OkpoRep.Value,
                    "2.0" => _form20VM.Storage.OkpoRep.Value,
                    _ => ""
                };
                bool reportsAlreadyExist;

                if (string.IsNullOrWhiteSpace(regNum)
                    && string.IsNullOrWhiteSpace(okpo))
                {
                    reportsAlreadyExist = false;
                }
                else
                {
                    reportsAlreadyExist = _formType switch
                    {
                        "1.0" => query
                            .ToList()
                            .Any(x => x.Master_DB.FormNum_DB == _formType
                                      && x.Master_DB.RegNoRep.Value == regNum
                                      && x.Master_DB.OkpoRep.Value == okpo
                                      && x.Master_DB.Id != _form10VM.Storage.Id),

                        "2.0" => query
                            .ToList()
                            .Any(x => x.Master_DB.FormNum_DB == _formType
                                      && x.Master_DB.RegNoRep.Value == regNum
                                      && x.Master_DB.OkpoRep.Value == okpo
                                      && x.Master_DB.Id != _form20VM.Storage.Id),

                        _ => false
                    };
                }

                if (reportsAlreadyExist)
                {
                    await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                            ContentTitle = "Ошибка при сохранении титульного листа организации",
                            ContentHeader = "Ошибка",
                            ContentMessage =
                                $"Не удалось сохранить изменения в титульном листе организации, " +
                                $"поскольку организация с данными ОКПО и рег.№ уже существует в базе данных. " +
                                $"Убедитесь в правильности заполнения ОКПО и рег.№.",
                            MinWidth = 400,
                            MaxWidth = 600,
                            MinHeight = 150,
                            MaxHeight = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(window ?? Desktop.MainWindow));

                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }

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
        else if (Storages != null && _formType is not ("1.0" or "2.0" or "4.0" or "5.0") && !Storages.Report_Collection.Contains(Storage))
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

            var mainWindow = Desktop.MainWindow as MainWindow;
            var mainWindowVM = await Dispatcher.UIThread.InvokeAsync(() => mainWindow.DataContext as MainWindowVM);
            mainWindowVM.UpdateReportsCollection();
    }
}