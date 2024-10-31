using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Views;
using Client_App.Views.ProgressBar;
using Models.Collections;
using Models.DBRealization;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

public class ExcelExportSnkAsyncCommand : ExcelBaseAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        var formNum = parameter as string;

        var mainWindow = Desktop.MainWindow as MainWindow;
        var selectedReports = mainWindow!.SelectedReports.First() as Reports;
        if (selectedReports is null)
        {
            #region MessageExcelExportFail

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = "Выгрузка не выполнена, поскольку не выбрана организация.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        var regNum = selectedReports!.Master_DB.RegNoRep.Value;
        var okpo = selectedReports.Master_DB.OkpoRep.Value;
        ExportType = $"СНК_{formNum}_{regNum}_{okpo}";

        progressBarVM.SetProgressBar(9, "Создание временной БД", $"Выгрузка СНК {formNum} {regNum}_{okpo}", ExportType);
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(7, "Запрос пути сохранения");
        var fileName = $"{ExportType}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts, progressBar);

        progressBarVM.SetProgressBar(11, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);

        var forms11DtoList = await GetForms11DtoList(db, selectedReports.Id, cts);

    }

    private static async Task<IEnumerable<ShortForm11DTO>> GetForms11DtoList(DBModel db, int id, CancellationTokenSource cts)
    {
        var tatata = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
            .Where(reps => reps.Id == id)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.1"))
            .Where(rep => rep.Rows11
                .Any(form => form.OperationCode_DB == "10"))
            .ToListAsync(cts.Token);

        var tatata2 = tatata
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .Take(1)
            .First();


        return await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.Master_DB).ThenInclude(rep => rep.Rows10)
            .Include(reps => reps.Report_Collection).ThenInclude(rep => rep.Rows11)
            .Where(reps => reps.Id == id)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.1")
                .SelectMany(rep => rep.Rows11
                    .Select(form11 =>
                        new ShortForm11DTO(form11.Id,
                            form11.FactoryNumber_DB,
                            form11.PassportNumber_DB,
                            form11.Type_DB))))
            .ToListAsync(cts.Token);
    }

    //private static Task<IEnumerable<ShortForm11DTO>> GetUniqFormsList(List<ShortForm11DTO> dtoList)
    //{
    //    var uniqFormsList = new List<string>();

    //}

    private class ShortForm11DTO(int id, string facNum, string pasNum, string type)
    {
        public readonly int Id = id;

        public readonly string FacNum = facNum;

        public readonly string PasNum = pasNum;

        public readonly string Type = type;
    }
}