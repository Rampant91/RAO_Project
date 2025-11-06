using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Threading;
using Client_App.Behaviors;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Messages;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands
{
    public class GenerateForm41AsyncCommand : BaseAsyncCommand
    {
        private readonly BaseFormVM _formVM;
        private Report Report => _formVM.Report;
        private DBModel _dbModel = StaticConfiguration.DBModel;

        private List<Reports> _organizations10;
        private List<Reports> _organizations20;
        private string _codeSubjectRF;
        private int _year = 0;
        private Dictionary<string, Form41> _existingRowsDict;

        public GenerateForm41AsyncCommand(BaseFormVM formVM)
        {
            _formVM = formVM;
        }

        public override async Task AsyncExecute(object? parameter)
        {
            var owner = GetActiveWindow();
            if (owner == null) return;

            try
            {
                // Шаг 1: Подтверждение и настройка параметров
                if (!await ShowConfirmationMessage(owner)) return;

                var setupResult = await SetupGenerationParameters(owner);
                if (!setupResult.ShouldContinue) return;


                var organizationsData = await LoadOrganizationsDataBatch(_year);
                _organizations10 = organizationsData.Organizations10 ?? new List<Reports>();
                _organizations20 = organizationsData.Organizations20 ?? new List<Reports>();

                // Проверяем, есть ли данные для обработки
                if (!_organizations10.Any() && !_organizations20.Any())
                {
                    await ShowInfoMessage(owner, "Нет данных для формирования отчета");
                    return;
                }

                // Шаг 3: Фильтрация по субъекту РФ
                if (!string.IsNullOrEmpty(_codeSubjectRF))
                {
                    FilterDataBySubjectRF(_codeSubjectRF);

                    // Проверяем, остались ли данные после фильтрации
                    if (!_organizations10.Any() && !_organizations20.Any())
                    {
                        await ShowInfoMessage(owner, "Нет данных для выбранного субъекта РФ");
                        return;
                    }
                }

                // Шаг 4: Предварительные вычисления статистики
                var stats10 = await PrecomputeOrganization10Stats(_organizations10, _year);
                var stats20 = await PrecomputeOrganization20Stats(_organizations20, _year);

                // Шаг 5: Обработка данных
                await ProcessOrganizationsBatch(stats10, stats20);

                // Шаг 6: Финальная сортировка и обновление
                await FinalizeFormGeneration();

                await ShowSuccessMessage(owner);
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(owner, $"Произошла ошибка: {ex.Message}");
            }
        }

        #region Window Management
        private Window GetActiveWindow()
        {
            return (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?
                .Windows.FirstOrDefault(w => w.IsActive);
        }
        #endregion

        #region Setup and Configuration
        private async Task<SetupResult> SetupGenerationParameters(Window owner)
        {
            var result = new SetupResult { ShouldContinue = true };

            // Очистка текущих строк
            Report.Rows41.Clear();

            // Запрос о формировании на основе другого отчета
            if (await ShowAskDependOnReportOrNotMessage(owner))
            {
                var report = await ShowAskReportMessage(owner);
                if (report != null)
                {
                    CopyRowsFromReport(report);
                }
            }

            _formVM.UpdateFormList();
            _formVM.UpdatePageInfo();

            // Запрос о субъекте РФ
            if (await ShowAskAllOrOneSubjectRFMessage(owner))
            {
                _codeSubjectRF = await ShowAskSubjectRFMessage(owner);
            }

            // Определение года
            if (!int.TryParse(Report.Year.Value, out _year))
            {
                _year = await ShowAskYearMessage(owner);
                Report.Year.Value = _year.ToString();
            }

            // Инициализация словаря существующих строк
            _existingRowsDict = Report.Rows41.ToDictionary(
                x => x.RegNo_DB,
                x => x,
                StringComparer.OrdinalIgnoreCase);

            return result;
        }
        #endregion

        #region Data Loading
        private async Task<OrganizationsData> LoadOrganizationsDataBatch(int year)
        {
            var organizations10Task = GetOrganizationsList("1.0", year);
            var organizations20Task = GetOrganizationsList("2.0", year);

            await Task.WhenAll(organizations10Task, organizations20Task);

            return new OrganizationsData
            {
                Organizations10 = await organizations10Task,
                Organizations20 = await organizations20Task
            };
        }

        private async Task<List<Reports>> GetOrganizationsList(string formNum, int year)
        {
            try
            {
                return await _dbModel.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(reports => reports.Master_DB)
                    .Where(reports => reports.Master_DB.FormNum_DB == formNum)
                    .Include(reports => reports.Report_Collection
                        .Where(report => report.Year_DB == year.ToString()))
                    .ThenInclude(report => report.Rows11.Where(r => r.OperationCode_DB == "10"))
                    .Include(reports => reports.Report_Collection)
                    .ThenInclude(report => report.Rows12.Where(r => r.OperationCode_DB == "10"))
                    .Include(reports => reports.Report_Collection)
                    .ThenInclude(report => report.Rows13.Where(r => r.OperationCode_DB == "10"))
                    .Include(reports => reports.Report_Collection)
                    .ThenInclude(report => report.Rows14.Where(r => r.OperationCode_DB == "10"))
                    .Include(reports => reports.Report_Collection)
                    .ThenInclude(report => report.Rows212)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Логируем ошибку и возвращаем пустой список
                System.Diagnostics.Debug.WriteLine($"Ошибка при загрузке организаций для формы {formNum}: {ex.Message}");
                return new List<Reports>();
            }
        }
        #endregion

        #region Data Processing
        private async Task<Dictionary<string, Organization10Stats>> PrecomputeOrganization10Stats(
            List<Reports> organizations, int year)
        {
            var statsDict = new Dictionary<string, Organization10Stats>();

            // Защита от пустой коллекции
            if (organizations == null || !organizations.Any())
                return statsDict;

            var tasks = new List<Task>();

            foreach (var org in organizations)
            {
                var task = Task.Run(() =>
                {
                    var regNo = org.Master?.RegNoRep?.Value;
                    if (string.IsNullOrEmpty(regNo)) return;

                    var stats = new Organization10Stats
                    {
                        NumInventarizationForms = CountFormsWithInventarization(org, year),
                        NumWithoutInventarizationForms = CountFormsWithoutInventarization(org, year)
                    };

                    lock (statsDict)
                    {
                        statsDict[regNo] = stats;
                    }
                });
                tasks.Add(task);

                if (tasks.Count >= Environment.ProcessorCount)
                {
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                }
            }

            if (tasks.Any())
            {
                await Task.WhenAll(tasks);
            }

            return statsDict;
        }

        private async Task<Dictionary<string, int>> PrecomputeOrganization20Stats(
            List<Reports> organizations, int year)
        {
            var statsDict = new Dictionary<string, int>();

            // Защита от пустой коллекции
            if (organizations == null || !organizations.Any())
                return statsDict;

            var tasks = new List<Task>();

            foreach (var org in organizations)
            {
                var task = Task.Run(() =>
                {
                    var regNo = org.Master?.RegNoRep?.Value;
                    if (string.IsNullOrEmpty(regNo)) return;

                    var count = CountForm212(org, year);

                    lock (statsDict)
                    {
                        statsDict[regNo] = count;
                    }
                });
                tasks.Add(task);

                if (tasks.Count >= Environment.ProcessorCount)
                {
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                }
            }

            if (tasks.Any())
            {
                await Task.WhenAll(tasks);
            }

            return statsDict;
        }

        private async Task ProcessOrganizationsBatch(
            Dictionary<string, Organization10Stats> stats10,
            Dictionary<string, int> stats20)
        {
            // Объединяем ключи из обоих словарей и убираем дубликаты
            var allRegNumbers = new HashSet<string>();

            if (stats10 != null)
                foreach (var key in stats10.Keys) allRegNumbers.Add(key);

            if (stats20 != null)
                foreach (var key in stats20.Keys) allRegNumbers.Add(key);

            // Защита от пустого набора
            if (!allRegNumbers.Any())
                return;

            var tasks = new List<Task>();

            foreach (var regNo in allRegNumbers)
            {
                var task = ProcessOrganizationAsync(regNo, stats10, stats20);
                tasks.Add(task);

                if (tasks.Count >= 10) // Ограничиваем параллелизм
                {
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                }
            }

            if (tasks.Any())
            {
                await Task.WhenAll(tasks);
            }
        }

        private Task ProcessOrganizationAsync(
            string regNo,
            Dictionary<string, Organization10Stats> stats10,
            Dictionary<string, int> stats20)
        {
            return Task.Run(() =>
            {
                try
                {
                    Organization10Stats org10Stats = null;
                    int org20Count = 0;

                    // Безопасное извлечение данных из словарей
                    if (stats10?.ContainsKey(regNo) == true)
                        org10Stats = stats10[regNo];

                    if (stats20?.ContainsKey(regNo) == true)
                        org20Count = stats20[regNo];

                    // Безопасный поиск организаций с проверкой на null
                    var organization10 = _organizations10?.FirstOrDefault(o =>
                        o?.Master?.RegNoRep?.Value == regNo);
                    var organization20 = _organizations20?.FirstOrDefault(o =>
                        o?.Master?.RegNoRep?.Value == regNo);

                    // Определяем основную организацию для получения данных
                    var primaryOrganization = organization10 ?? organization20;
                    if (primaryOrganization?.Master == null) return;

                    if (_existingRowsDict?.ContainsKey(regNo) == true)
                    {
                        UpdateRow(primaryOrganization, org10Stats, org20Count);
                    }
                    else
                    {
                        CreateRow(primaryOrganization, org10Stats, org20Count);
                    }
                }
                catch (Exception ex)
                {
                    // Логируем ошибку, но не прерываем выполнение
                    System.Diagnostics.Debug.WriteLine($"Ошибка при обработке организации {regNo}: {ex.Message}");
                }
            });
        }
        #endregion

        #region Row Management
        private void UpdateRow(
            Reports organization,
            Organization10Stats org10Stats,
            int numForm212 = -1)
        {
            if (organization?.Master?.RegNoRep?.Value == null) return;

            if (!_existingRowsDict.TryGetValue(organization.Master.RegNoRep.Value, out var form41))
                return;

            // Безопасное обновление свойств
            form41.Okpo_DB = organization.Master.OkpoRep?.Value ?? "";
            form41.OrganizationName_DB = organization.Master.ShortJurLicoRep?.Value ?? "";

            if (org10Stats != null)
            {
                form41.NumOfFormsWithInventarizationInfo_DB = org10Stats.NumInventarizationForms;
                form41.NumOfFormsWithoutInventarizationInfo_DB = org10Stats.NumWithoutInventarizationForms;
            }

            if (numForm212 >= 0)
            {
                form41.NumOfForms212_DB = numForm212;
            }
        }

        private void CreateRow(
            Reports organization,
            Organization10Stats org10Stats,
            int numForm212 = -1)
        {
            if (organization?.Master == null) return;

            var form41 = new Form41()
            {
                RegNo_DB = organization.Master.RegNoRep?.Value ?? "",
                Okpo_DB = organization.Master.OkpoRep?.Value ?? "",
                OrganizationName_DB = organization.Master.ShortJurLicoRep?.Value ?? ""
            };

            if (org10Stats != null)
            {
                form41.NumOfFormsWithInventarizationInfo_DB = org10Stats.NumInventarizationForms;
                form41.NumOfFormsWithoutInventarizationInfo_DB = org10Stats.NumWithoutInventarizationForms;
            }

            if (numForm212 >= 0)
            {
                form41.NumOfForms212_DB = numForm212;
            }

            Report.Rows41.Add(form41);
            _existingRowsDict ??= new Dictionary<string, Form41>(StringComparer.OrdinalIgnoreCase);
            _existingRowsDict[form41.RegNo_DB] = form41;
        }
        #endregion

        #region Finalization
        private async Task FinalizeFormGeneration()
        {
            await Task.Run(() =>
            {
                // Проверяем, есть ли данные для сортировки
                if (!Report.Rows41.Any())
                    return;

                // Сортировка по Рег№
                var finalList = Report.Rows41
                    .Where(x => !string.IsNullOrEmpty(x.RegNo_DB))
                    .OrderBy(rep => rep.RegNo_DB)
                    .ToList();

                // Выставляем номера строк
                for (int i = 0; i < finalList.Count; i++)
                {
                    finalList[i].NumberInOrder_DB = i + 1;
                }

                Report.Rows41.Clear();
                Report.Rows41.AddRange(finalList);
            });

            // Обновляем таблицу
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                _formVM.UpdateFormList();
                _formVM.UpdatePageInfo();
            });
        }
        #endregion

        #region Filtering
        private void FilterDataBySubjectRF(string codeSubjectRF)
        {
            if (string.IsNullOrEmpty(codeSubjectRF)) return;

            try
            {
                // Фильтруем строки формы с защитой от null
                var filteredRows = Report.Rows41
                    .Where(form41 => form41?.RegNo_DB?.StartsWith(codeSubjectRF) == true)
                    .ToList();

                Report.Rows41.Clear();
                if (filteredRows.Any())
                {
                    Report.Rows41.AddRange(filteredRows);
                }

                // Обновляем словарь
                _existingRowsDict = Report.Rows41.ToDictionary(
                    x => x.RegNo_DB,
                    x => x,
                    StringComparer.OrdinalIgnoreCase);

                // Фильтруем организации с защитой от null
                if (_organizations10 != null)
                {
                    _organizations10 = _organizations10
                        .Where(reports => reports?.Master?.RegNoRep?.Value?.StartsWith(codeSubjectRF) == true)
                        .ToList();
                }
                else
                {
                    _organizations10 = new List<Reports>();
                }

                if (_organizations20 != null)
                {
                    _organizations20 = _organizations20
                        .Where(reports => reports?.Master?.RegNoRep?.Value?.StartsWith(codeSubjectRF) == true)
                        .ToList();
                }
                else
                {
                    _organizations20 = new List<Reports>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при фильтрации по субъекту РФ: {ex.Message}");
                // В случае ошибки оставляем исходные данные
            }
        }
        #endregion

        #region Counting Methods
        private int CountFormsWithInventarization(Reports organization10, int year)
        {
            if (organization10?.Report_Collection == null) return 0;

            return organization10.Report_Collection
                .Count(report =>
                    report?.Year_DB == year.ToString() &&
                    ((report.FormNum_DB == "1.1" && report.Rows11?.Any(x => x.OperationCode_DB == "10") == true) ||
                     (report.FormNum_DB == "1.2" && report.Rows12?.Any(x => x.OperationCode_DB == "10") == true) ||
                     (report.FormNum_DB == "1.3" && report.Rows13?.Any(x => x.OperationCode_DB == "10") == true) ||
                     (report.FormNum_DB == "1.4" && report.Rows14?.Any(x => x.OperationCode_DB == "10") == true)));
        }

        private int CountFormsWithoutInventarization(Reports organization10, int year)
        {
            if (organization10?.Report_Collection == null) return 0;

            return organization10.Report_Collection
                .Count(report =>
                    report?.Year_DB == year.ToString() &&
                    ((report.FormNum_DB == "1.1" && report.Rows11?.All(x => x.OperationCode_DB != "10") == true) ||
                     (report.FormNum_DB == "1.2" && report.Rows12?.All(x => x.OperationCode_DB != "10") == true) ||
                     (report.FormNum_DB == "1.3" && report.Rows13?.All(x => x.OperationCode_DB != "10") == true) ||
                     (report.FormNum_DB == "1.4" && report.Rows14?.All(x => x.OperationCode_DB != "10") == true)));
        }

        private int CountForm212(Reports organization20, int year)
        {
            if (organization20?.Report_Collection == null) return 0;

            return organization20.Report_Collection
                .Count(report =>
                    report?.Year_DB == year.ToString() &&
                    report.FormNum_DB == "2.12");
        }
        #endregion

        #region Dialog Messages
        private async Task<bool> ShowConfirmationMessage(Window owner)
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

            return answer == "Да";
        }

        private async Task<bool> ShowAskDependOnReportOrNotMessage(Window owner)
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
                    ContentMessage = "Вы хотите сформировать отчет на основе другого отчета?",
                    MinWidth = 300,
                    MinHeight = 125,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(owner));

            return answer == "Да";
        }

        private async Task<Report> ShowAskReportMessage(Window owner)
        {
            var dialog = new AskForm41Message(Report);
            return await dialog.ShowDialog<Report?>(owner);
        }

        private async Task<int> ShowAskYearMessage(Window owner)
        {
            var dialog = new AskIntMessageWindow(new AskIntMessageVM("Введите год, за который хотите сформировать отчет"));
            int? year = await dialog.ShowDialog<int>(owner);
            return year ?? DateTime.Now.Year;
        }

        private async Task<bool> ShowAskAllOrOneSubjectRFMessage(Window owner)
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
                    ContentMessage = "Вы хотите сформировать отчет по конкретному субъекту Российской Федерации?",
                    MinWidth = 300,
                    MinHeight = 125,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(owner));

            return answer == "Да";
        }

        private async Task<string?> ShowAskSubjectRFMessage(Window owner)
        {
            var dialog = new AskSubjectRFMessage();
            return await dialog.ShowDialog<string?>(owner);
        }

        private async Task ShowSuccessMessage(Window owner)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Успех", "Отчет успешно сформирован")
                .ShowDialog(owner));
        }

        private async Task ShowErrorMessage(Window owner, string message)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Ошибка", message)
                .ShowDialog(owner));
        }

        private async Task ShowInfoMessage(Window owner, string message)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Информация", message)
                .ShowDialog(owner));
        }
        #endregion

        #region Helper Methods
        private void CopyRowsFromReport(Report report)
        {
            if (report?.Rows41 == null) return;

            var copiedRows = report.Rows41.Select(original => new Form41()
            {
                NumberInOrder_DB = original.NumberInOrder_DB,
                RegNo_DB = original.RegNo_DB,
                Okpo_DB = original.Okpo_DB,
                OrganizationName_DB = original.OrganizationName_DB,
                LicenseOrRegistrationInfo_DB = original.LicenseOrRegistrationInfo_DB,
                Note_DB = original.Note_DB,
            }).ToList();

            Report.Rows41.AddRange(copiedRows);
        }
        #endregion

        #region Helper Classes
        private class SetupResult
        {
            public bool ShouldContinue { get; set; }
        }

        private class OrganizationsData
        {
            public List<Reports> Organizations10 { get; set; } = new List<Reports>();
            public List<Reports> Organizations20 { get; set; } = new List<Reports>();
        }

        private class Organization10Stats
        {
            public int NumInventarizationForms { get; set; }
            public int NumWithoutInventarizationForms { get; set; }
        }
        #endregion
    }
}