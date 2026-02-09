using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels.Messages;
using Client_App.Views.Messages;
using CommunityToolkit.Mvvm.DependencyInjection;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Generate.GenerateForm5
{
    public class BaseGenerateForm5 : BaseAsyncCommand
    {
        protected Window owner;
        public override async void Execute(object? parameter)
        {
        }
        public override async Task AsyncExecute(object? parameter)
        {
        }

        protected string SummarizeExponentionalStrings(string str, string incStr)
        {
            if (double.TryParse(str,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var value)
                && double.TryParse(incStr,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var increment))
            {
                var sum = value + increment;
                return sum.ToString("e5", CultureInfo.CreateSpecificCulture("ru-RU"));
            }
            else
                return str;
        }

        protected string SubtractExponentionalStrings(string str, string incStr)
        {
            if (double.TryParse(str,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var value)
                && double.TryParse(incStr,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var decrement))
            {
                var difference  = value - decrement;
                return difference.ToString("e5", CultureInfo.CreateSpecificCulture("ru-RU"));
            }
            else
                return str;
        }
        #region AskMessages
        protected async Task<bool> ShowConfirmationMessage(Window owner)
        {
            string answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" },
                    ],
                    CanResize = true,
                    ContentTitle = "Формирование нового отчета",
                    ContentMessage = "Все строки будут перезаписаны!\n" +
                    "Вы уверены, что хотите продолжить?",
                    MinWidth = 300,
                    MinHeight = 125,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(owner));

            if (answer == "Да")
                return true;
            else
                return false;
        }
        protected async Task<int> ShowAskYearMessage(Window owner)
        {
            var dialog = new AskIntMessageWindow(new AskIntMessageVM("Введите год, за который хотите сформировать отчет"));

            int? year = await dialog.ShowDialog<int>(owner);

            if (year == null)
                return 0;

            return (int)year;
        }
        #endregion

        #region Requests
        protected async Task<List<int>> GetOrganizations10List(DBModel DB, CancellationToken cancellationToken, List<ViacOrganization> loadedList = null)
        {
            try
            {

                cancellationToken.ThrowIfCancellationRequested();

                if (loadedList == null || loadedList.Count == 0)
                    return await DB.ReportsCollectionDbSet
                        .AsSplitQuery()
                        .AsNoTracking()
                        .Include(reports => reports.Master_DB).ThenInclude(reports => reports.Rows10)
                        .Where(reports => reports.Master_DB.FormNum_DB == "1.0")
                        .Select(reports => reports.Id)
                        .ToListAsync(cancellationToken);
                else
                    return DB.ReportsCollectionDbSet
                        .AsSplitQuery()
                        .AsNoTracking()
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Where(x => x.Master_DB.FormNum_DB == "1.0")
                        .AsEnumerable()
                        .Where(reports => loadedList.Any(x =>
                        x.RegNo == reports.Master_DB.Rows10[0].RegNo_DB
                        && (x.OKPO == reports.Master_DB.Rows10[1].Okpo_DB
                        ||
                        (string.IsNullOrEmpty(reports.Master_DB.Rows10[1].Okpo_DB)
                        && x.OKPO == reports.Master_DB.Rows10[0].Okpo_DB))))
                        .Select(reports => reports.Id)
                        .ToList();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                   .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                   {
                       ButtonDefinitions =
                       [
                           new ButtonDefinition { Name = "Ок" },
                       ],
                       CanResize = true,
                       ContentTitle = "Формирование нового отчета",
                       ContentMessage = "Произошла ошибка:\n" +
                       $"{ex.Message}",
                       MinWidth = 300,
                       MinHeight = 125,
                       WindowStartupLocation = WindowStartupLocation.CenterOwner
                   })
                   .ShowDialog(owner));
                return new List<int>();
            }
        }
        #endregion

    }
}