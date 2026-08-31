using Client_App.Interfaces;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.Commands.AsyncCommands.SwitchReport;
using Client_App.Commands.SyncCommands;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.Controls;
using Models.Collections;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Models.Attributes;
using Models.DBRealization;
using Microsoft.EntityFrameworkCore;
using Avalonia.Threading;

namespace Client_App.ViewModels.Forms;

public abstract class BaseFormVM : BaseVM, INotifyPropertyChanged, IFormContentLoadingHost
{
    #region Properties

    public abstract string FormType { get; }

    public string WindowTitle
    {
        get
        {
            var formNum = FormType.Replace(".", "");
            var typeName = $"Models.Forms.Form{formNum[0]}.Form{formNum},Models";
            var formName = ((Form_ClassAttribute)Type.GetType(typeName)!
                .GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;

            var master = Reports?.Master_DB;
            string reg = "", shortName = "", okpo = "";
            try
            {
                if (master != null)
                {
                    reg = master.RegNoRep?.Value ?? "";
                    shortName = master.ShortJurLicoRep?.Value ?? "";
                    okpo = master.OkpoRep?.Value ?? "";
                }
            }
            catch
            {
                // Rows10/20 могут быть ещё не загружены
            }

            return $"{formName} {reg}  {shortName}  {okpo}";
        }
    }

    private ObservableCollection<Form> _formList = [];
    public ObservableCollection<Form> FormList
    {
        get => _formList;
        set
        {
            _formList = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Note> _noteList = [];
    public ObservableCollection<Note> NoteList
    {
        get => _noteList;
        set
        {
            _noteList = value;
            OnPropertyChanged();
        }
    }

    #region Report

    private Report _report;
    public Report Report
    {
        get => _report;
        set
        {
            _report = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Reports

    private Reports _reports;
    public Reports Reports
    {
        get => _reports;
        set
        {
            if (_reports != value)
            {
                _reports = value;
                OnPropertyChanged();
            }
        }
    }
    #endregion

    #region DBO

    private DBObservable _DBO;
    public DBObservable DBO
    {
        get => _DBO;
        set
        {
            if (_DBO != value)
            {
                _DBO = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region IsCanSaveReportEnabled

    private bool _isCanSaveReportEnabled;
    public bool IsCanSaveReportEnabled
    {
        get => _isCanSaveReportEnabled;
        set
        {
            if (_isCanSaveReportEnabled != value)
            {
                _isCanSaveReportEnabled = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion


    private Form? _selectedForm;
    public Form? SelectedForm
    {
        get => _selectedForm;
        set
        {
            _selectedForm = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Form> _selectedForms = [];
    public ObservableCollection<Form> SelectedForms
    {
        get => _selectedForms;
        set
        {
            if (_selectedForms != value)
            {
                UnsubscribeSelectedForms(_selectedForms);
                _selectedForms = value;
                SubscribeSelectedForms(_selectedForms);
                OnPropertyChanged();
                OnPropertyChanged(nameof(AnyRowSelected));
                OnPropertyChanged(nameof(OnlyOneRowSelected));
            }
        }
    }

    #region AnyRowSelected

    private bool _anyRowSelected;
    public bool AnyRowSelected
    {
        get => SelectedForms.Any();
        set
        {
            if (_anyRowSelected != value)
            {
                _anyRowSelected = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    private bool _dataGridIsEditing;
    public bool DataGridIsEditing
    {
        get => _dataGridIsEditing;
        set
        {
            if (_dataGridIsEditing != value)
            {
                _dataGridIsEditing = value;
                OnPropertyChanged();
            }
        }
    }

    #region OnlyOneRowSelected

    private bool _onlyOneRowSelected;
    public bool OnlyOneRowSelected
    {
        get => SelectedForms.Count == 1;
        set
        {
            if (_onlyOneRowSelected != value)
            {
                _onlyOneRowSelected = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region SkipChangeTacking

    private bool _skipChangeTacking;
    public bool SkipChangeTacking
    {
        get => _skipChangeTacking;
        set
        {
            if (_skipChangeTacking != value)
            {
                _skipChangeTacking = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    private Note _selectedNote;
    public Note SelectedNote
    {
        get => _selectedNote;
        set
        {
            _selectedNote = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Note> _selectedNotes = [];
    public ObservableCollection<Note> SelectedNotes
    {
        get => _selectedNotes;
        set
        {
            if (_selectedNotes != value)
            {
                _selectedNotes = value;
                OnPropertyChanged();
            }
        }
    }

    private int _rowCount = 30;
    public int RowCount
    {
        get => _rowCount;
        set
        {
            var result = value;
            if (value <= 0)
                result = 1;
            else if (value > 50) //Хардкод максимального кол-ва загруженных строк
                result = 50;

            if (_rowCount != result)
            {
                _rowCount = result;
                OnPropertyChanged();
                SchedulePagingRefresh();
            }
        }
    }

    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            var result = value;
            if (value <= 0)
                result = 1;
            else if (value > TotalPages && TotalPages > 0)
                result = TotalPages;

            if (result == _currentPage)
                return;

            _currentPage = result;
            OnPropertyChanged();
            // Debounce typed CurrentPage/RowCount (e.g. "66" / "30") like org SearchText — 300ms.
            SchedulePagingRefresh();
        }
    }

    private CancellationTokenSource? _pagingDebounceCts;
    private const int PagingDebounceMs = 300;

    /// <summary>
    /// Cancel pending typed CurrentPage/RowCount debounce and refresh now (◀/▶).
    /// </summary>
    public void FlushPendingPagingRefresh()
    {
        CancelPagingDebounce();
        _ = RefreshVisibleRowsAsync();
    }

    private void CancelPagingDebounce()
    {
        try { _pagingDebounceCts?.Cancel(); } catch { /* ignore */ }
        _pagingDebounceCts = null;
    }

    private void SchedulePagingRefresh()
    {
        if (!UseDbPaging)
        {
            CancelPagingDebounce();
            _ = RefreshVisibleRowsAsync();
            return;
        }

        CancelPagingDebounce();
        var cts = new CancellationTokenSource();
        _pagingDebounceCts = cts;
        _ = DebouncedPagingRefreshAsync(cts);
    }

    private async Task DebouncedPagingRefreshAsync(CancellationTokenSource cts)
    {
        try
        {
            await Task.Delay(PagingDebounceMs, cts.Token).ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        if (cts.IsCancellationRequested || !ReferenceEquals(_pagingDebounceCts, cts))
            return;

        await RefreshVisibleRowsAsync().ConfigureAwait(true);
    }

    /// <summary>
    /// true — в Report.Rows только текущая страница из БД; TotalRows берётся из DbTotalRows.
    /// </summary>
    public bool UseDbPaging { get; set; }

    public int? DbTotalRows { get; set; }

    /// <summary>
    /// В памяти полный набор строк (2/4/5 или черновик 1.x). Для paging 1.x не используется.
    /// </summary>
    private bool _rowSessionActive;

    /// <summary>Индикатор загрузки содержимого окна (смена отчёта, страница строк, догрузка).</summary>
    public bool IsContentLoading
    {
        get => _isContentLoading;
        private set
        {
            if (_isContentLoading == value)
                return;
            _isContentLoading = value;
            OnPropertyChanged();
        }
    }

    private bool _isContentLoading;
    private string _contentLoadingMessage = "Загрузка…";
    public string ContentLoadingMessage
    {
        get => _contentLoadingMessage;
        set
        {
            if (_contentLoadingMessage == value)
                return;
            _contentLoadingMessage = value;
            OnPropertyChanged();
        }
    }

    private int _contentLoadGeneration;

    /// <summary>
    /// Soft-lock формы: сразу очистить видимые строки формы, отдать кадр UI, затем работа.
    /// Overlay — лёгкая карточка; бегунок крутится на DispatcherTimer.
    /// Примечания не трогаем — догрузка строк к ним не относится.
    /// </summary>
    public void BeginContentLoading(string? message = null)
    {
        if (_isContentLoading)
            return;

        ContentLoadingMessage = string.IsNullOrEmpty(message) ? "\u0417\u0430\u0433\u0440\u0443\u0437\u043a\u0430\u2026" : message;
        IsContentLoading = true;
    }

    public async Task WithContentLoadingAsync(Func<Task> work, bool clearVisibleRows = true, string? message = null)
    {
        ArgumentNullException.ThrowIfNull(work);
        var generation = Interlocked.Increment(ref _contentLoadGeneration);
        ContentLoadingMessage = string.IsNullOrEmpty(message) ? "Загрузка…" : message;
        IsContentLoading = true;

        if (clearVisibleRows && _formList.Count > 0)
            _formList.Clear();

        try
        {
            await Avalonia.Threading.Dispatcher.UIThread
                .InvokeAsync(static () => { }, Avalonia.Threading.DispatcherPriority.Render);

            if (generation != _contentLoadGeneration)
                return;

            await work().ConfigureAwait(true);
        }
        finally
        {
            if (generation == _contentLoadGeneration)
                IsContentLoading = false;
        }
    }

    public int TotalPages
    {
        get
        {
            var total = TotalRows;
            if (total <= 0 || RowCount <= 0) return 0;
            var result = total / RowCount;
            if (total % RowCount != 0)
                result++;
            return result;
        }
    }

    public int TotalRows
    {
        get
        {
            if (DbTotalRows is int dbCount && Report?.Id > 0)
            {
                var delta = FormRowMutationService.GetPendingDelta(
                    StaticConfiguration.DBModel, Report.Id, FormType);
                return Math.Max(0, dbCount + delta);
            }

            return CountLiveInMemoryRows();
        }
    }


    private bool _isAutoReplaceEnabled = true;
    public bool IsAutoReplaceEnabled
    {
        get => _isAutoReplaceEnabled;
        set
        {
            if (_isAutoReplaceEnabled != value)
            {
                _isAutoReplaceEnabled = value;
                Report.AutoReplace = value;
                OnPropertyChanged();
            }
        }
    }

    private int _selectedYear = DateTime.Now.Year;
    public int SelectedYear
    {
        get => _selectedYear;
        set
        {
            _selectedYear = value;
            OnPropertyChanged();
        }
    }

    private ExecutorDataControlVM _executorDataControlVM;
    public ExecutorDataControlVM ExecutorDataControlVM
    {
        get
        {
            return _executorDataControlVM;
        }
    }

    private SelectReportPopupVM _selectReportVM;
    public SelectReportPopupVM SelectReportPopupVM
    {
        get
        {
            return _selectReportVM;
        }
        set
        {
            _selectReportVM = value;
        }
    }
    #endregion

    #region Constructors

    public BaseFormVM() 
    { 
        SubscribeSelectedForms(_selectedForms);
    }

    /// <summary>
    /// Редактирование ранее созданного отчёта.
    /// </summary>
    /// <param name="report">Отчёт.</param>
    public BaseFormVM(Report report)
    {
        _report = report;
        _reports = report.Reports;
        InitializeUserControls();
        Avalonia.Threading.Dispatcher.UIThread.Post(
            () => _ = CompleteInitialLoadAsync(),
            Avalonia.Threading.DispatcherPriority.Loaded);
    }

    private async Task CompleteInitialLoadAsync()
    {
        await WithContentLoadingAsync(async () =>
        {
            await Avalonia.Threading.Dispatcher.UIThread
                .InvokeAsync(static () => { }, Avalonia.Threading.DispatcherPriority.Render);

            UpdateFormList();
            UpdatePageInfo();

            Report.Notes = new ObservableCollectionWithItemPropertyChanged<Note>(
                StaticConfiguration.DBModel.notes
                    .Where(note => note.ReportId == Report.Id));
            NoteList = Report.Notes;

            SubscribeSelectedForms(_selectedForms);
        }, clearVisibleRows: true, message: "\u041e\u0442\u043a\u0440\u044b\u0442\u0438\u0435 \u043e\u0442\u0447\u0451\u0442\u0430\u2026");
    }

    public void InitializeUserControls()
    {
        _selectReportVM = new SelectReportPopupVM(this);
        _executorDataControlVM = new ExecutorDataControlVM(this.Report);
    }

    /// <summary>
    /// Сменить отчёт в том же окне (↑/↓ / «Выбрать» при той же FormNum).
    /// </summary>
    /// <param name="dbTotalRowsHint">Count строк 1.x, уже посчитанный в фоне (избежать повторного COUNT на UI).</param>
    public async Task ReloadFromReportAsync(Report loadedReport, int? dbTotalRowsHint = null)
    {
        Report = loadedReport;
        Reports = loadedReport.Reports;
        FormRowOrderedIdsCache.Invalidate();

        var formNum = FormType;
        var isPaged = formNum is "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6" or "1.7" or "1.8" or "1.9";
        _rowSessionActive = false;
        if (isPaged)
        {
            UseDbPaging = true;
            DbTotalRows = dbTotalRowsHint
                ?? await FormRowsPageLoader.CountAsync(StaticConfiguration.DBModel, loadedReport.Id, formNum);
            _currentPage = 1;
            OnPropertyChanged(nameof(CurrentPage));
        }
        else
        {
            UseDbPaging = false;
            DbTotalRows = null;
            _currentPage = 1;
            OnPropertyChanged(nameof(CurrentPage));
        }

        if (loadedReport.Notes != null)
        {
            Report.Notes = loadedReport.Notes is ObservableCollectionWithItemPropertyChanged<Note> notes
                ? notes
                : new ObservableCollectionWithItemPropertyChanged<Note>(loadedReport.Notes);
        }
        else
        {
            Report.Notes = new ObservableCollectionWithItemPropertyChanged<Note>(
                StaticConfiguration.DBModel.notes.Where(note => note.ReportId == Report.Id));
        }

        NoteList = Report.Notes;
        SelectedForm = null;
        SelectedForms = [];
        SelectedNote = null!;
        SelectedNotes = [];

        UpdateFormList();
        UpdatePageInfo();
        InitializeUserControls();
        OnPropertyChanged(nameof(WindowTitle));
        OnPropertyChanged(nameof(TotalPages));
        OnPropertyChanged(nameof(TotalRows));
        if (isPaged)
            WarmVisiblePageCache();
    }
    #endregion

    #region Commands

    public ICommand CheckForm => new NewCheckFormAsyncCommand(this);    //  Кнопка "Проверить"
    //public ICommand CopyExecutorDate => new NewCopyExecutorDataAsyncCommand(this); //После привязки кнопка неактивна
    public ICommand SourceTransmissionAll => new SourceTransmissionAllAsyncCommand(this);    //  Кнопка "Перевести данные предыдущей формы"
    public ICommand AddRow => new NewAddRowAsyncCommand(this);
    public ICommand AddRows => new NewAddRowsAsyncCommand(this);
    public ICommand AddRowsIn => new NewAddRowsInAsyncCommand(this);
    public ICommand DeleteDataInRows => new NewDeleteDataInRowsAsyncCommand();
    private ICommand? _deleteRows;
    public ICommand DeleteRows => _deleteRows ??= new NewDeleteRowsAsyncCommand(this);
    public ICommand SortForm => new NewSortFormSyncCommand(this);
    public ICommand SetNumberOrder => new NewSetNumberOrderSyncCommand(this);
    public ICommand SortForms => new NewSortFormSyncCommand(this);
    public ICommand CopyRows => new NewCopyRowsAsyncCommand();
    public ICommand PasteRows => new NewPasteRowsAsyncCommand(this);
    public ICommand SelectAll => new SelectAllRowsAsyncCommand(this);
    public ICommand SaveReport => new SaveReportAsyncCommand(this);
    public ICommand AddNote => new NewAddNoteAsyncCommand(this);
    public ICommand AddNotes => new NewAddNotesAsyncCommand(this);
    public ICommand CopyNotes => new NewCopyNotesAsyncCommand();
    public ICommand PasteNotes => new NewPasteNotesAsyncCommand(this);
    public ICommand DeleteNotes => new NewDeleteNoteAsyncCommand(this);
    public ICommand SetDefaultColumnWidth => new SetDefaultColumnWidthAsyncCommand();
    #endregion

    #region SelectionChangeWiring

    /// <summary>
    /// Обновление списка выделенных строчек.
    /// </summary>
    /// <param name="collection"></param>
    private void SubscribeSelectedForms(ObservableCollection<Form> collection)
    {
        if (collection is INotifyCollectionChanged notify)
        {
            notify.CollectionChanged += SelectedForms_CollectionChanged;
        }
    }

    private void UnsubscribeSelectedForms(ObservableCollection<Form> collection)
    {
        if (collection is INotifyCollectionChanged notify)
        {
            notify.CollectionChanged -= SelectedForms_CollectionChanged;
        }
    }

    private void SelectedForms_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(AnyRowSelected));
        OnPropertyChanged(nameof(OnlyOneRowSelected));
    }

    #endregion

    #region  UpdateNoteList

    public void UpdateNoteList()
    {
        NoteList = new ObservableCollection<Note>( Report.Notes.ToList<Note>());
    }

    #endregion

    #region UpdateFormList

    /// <summary>
    /// Обновляет отображение ячеек DataGrid'а.
    /// Коллекцию не подменяем: новая ObservableCollection рвёт SelectedItems у Avalonia DataGrid,
    /// второе удаление тогда уходит с пустым/устаревшим параметром.
    /// № п/п в гриде — виртуальный, NumberInOrder_DB не трогаем.
    /// </summary>
    public void UpdateFormList()
    {
        IEnumerable<Form> page = UseDbPaging
            ? Report.Rows.ToList<Form>()
            : Report.Rows
                .ToList<Form>()
                .Skip((CurrentPage - 1) * RowCount)
                .Take(RowCount);

        var list = page.ToList();
        for (var i = 0; i < list.Count; i++)
            list[i].SetDisplayOrder(FormRowNumberCompact.DisplayNumber(CurrentPage, RowCount, i));

        _formList.Clear();
        foreach (var form in list)
            _formList.Add(form);
    }

    /// <summary>Сбросить выделение строк после мутации, чтобы команда не повторно удаляла мёртвые Id.</summary>
    public void ClearFormRowSelection()
    {
        SelectedForm = null;
        if (SelectedForms.Count > 0)
            SelectedForms.Clear();
    }

    /// <summary>
    /// Исторический вход «догрузить все строки». Paging+pending больше не грузит 20k — no-op.
    /// </summary>
    public Task EnsureAllRowsForMutationAsync() => Task.CompletedTask;

    /// <summary>
    /// Синхронный no-op: Sort/№ п/п не грузят полный набор (виртуальный npp + compact на Save).
    /// </summary>
    public void EnsureAllRowsForMutationBlocking()
    {
    }

    /// <summary>
    /// Полный набор в UI больше не открываем на мутациях 1.x.
    /// </summary>
    public Task EnsureFullRowSessionAsync() => Task.CompletedTask;

    public void EnsureFullRowSessionBlocking()
    {
    }

    /// <summary>
    /// Сквозная нумерация только на экране. В БД номера едут compact'ом на Save.
    /// </summary>
    public void RenumberFormRowsFromOne()
    {
        UpdateFormList();
    }

    /// <summary>Следующий № п/п = живой count + 1 (не Max текущей страницы).</summary>
    public int GetNextNumberInOrder() => TotalRows + 1;

    private int CountLiveInMemoryRows()
    {
        var db = StaticConfiguration.DBModel;
        var count = 0;
        foreach (var key in Report.Rows)
        {
            if (key is not Form form)
                continue;
            if (db.Entry(form).State == EntityState.Deleted)
                continue;
            count++;
        }

        return count;
    }

    /// <summary>Инвалидация prefetch-кэша после add/delete.</summary>
    public void NotifyRowMutation()
    {
        ClearFormPageCache();
        IsCanSaveReportEnabled = true;
    }

    /// <summary>Показать последнюю страницу без обращения к БД (in-memory) или с merge (paging).</summary>
    public void RevealLastPage()
    {
        var last = Math.Max(1, TotalPages);
        if (_currentPage != last)
        {
            _currentPage = last;
            OnPropertyChanged(nameof(CurrentPage));
        }

        _ = RefreshVisibleRowsAfterMutationAsync();
    }

    public async Task RevealLastPageAsync()
    {
        var last = Math.Max(1, TotalPages);
        if (_currentPage != last)
        {
            _currentPage = last;
            OnPropertyChanged(nameof(CurrentPage));
        }

        await RefreshVisibleRowsAfterMutationAsync().ConfigureAwait(true);
    }

    /// <summary>
    /// После add/delete: перечитать текущую страницу с merge, без overlay и без очистки грида.
    /// </summary>
    public async Task RefreshVisibleRowsAfterMutationAsync()
    {
        if (!UseDbPaging || Report is not { Id: > 0 } report ||
            !FormRowsPageLoader.SupportsDbPaging(FormType))
        {
            UpdateFormList();
            UpdatePageInfo();
            return;
        }

        var skip = Math.Max(0, (CurrentPage - 1) * RowCount);
        var items = await FormRowsPageLoader
            .LoadMergedVisiblePageAsync(StaticConfiguration.DBModel, report.Id, FormType, skip, RowCount)
            .ConfigureAwait(true);
        FormRowsPageLoader.ApplyPageToReport(report, FormType, items);
        CacheCurrentFormPage();
        UpdateFormList();
        UpdatePageInfo();
        ScheduleFormPagePrefetch();
    }

    /// <summary>
    /// После успешного Save: recount, paging on, одна страница из БД.
    /// </summary>
    public async Task OnRowsSavedAsync()
    {
        ClearFormPageCache();
        FormRowOrderedIdsCache.Invalidate();
        _rowSessionActive = false;
        if (Report?.Id > 0 && FormRowsPageLoader.SupportsDbPaging(FormType))
        {
            UseDbPaging = true;
            DbTotalRows = await FormRowsPageLoader
                .CountAsync(StaticConfiguration.DBModel, Report.Id, FormType)
                .ConfigureAwait(true);
            var last = Math.Max(1, TotalPages);
            if (_currentPage > last)
            {
                _currentPage = last;
                OnPropertyChanged(nameof(CurrentPage));
            }

            var skip = Math.Max(0, (CurrentPage - 1) * RowCount);
            var items = await FormRowsPageLoader
                .LoadMergedVisiblePageAsync(StaticConfiguration.DBModel, Report.Id, FormType, skip, RowCount)
                .ConfigureAwait(true);
            FormRowsPageLoader.ApplyPageToReport(Report, FormType, items);
        }

        UpdateFormList();
        UpdatePageInfo();
        if (UseDbPaging)
            WarmVisiblePageCache();
    }

    /// <summary>
    /// Откат к последнему сохранённому состоянию (закрытие «Нет» / смена отчёта без сохранения).
    /// Черновик Id==0 — не оставлять отчёт в БД. Существующий / после 41 — Restore + страница.
    /// </summary>
    public async Task DiscardUnsavedChangesAsync()
    {
        var db = StaticConfiguration.DBModel;
        var isDraft = Report == null || Report.Id <= 0;
        db.Restore();
        _rowSessionActive = false;
        ClearFormPageCache();
        FormRowOrderedIdsCache.Invalidate();

        if (isDraft)
        {
            UseDbPaging = false;
            DbTotalRows = null;
            if (Report != null)
            {
                foreach (var key in Report[FormType].GetEnumerable().ToList())
                {
                    if (key is Form form)
                        Report[FormType].Remove(form);
                }

                foreach (var note in Report.Notes.Where(n => n.Id == 0).ToList())
                    Report.Notes.Remove(note);
            }

            UpdateFormList();
            UpdateNoteList();
            UpdatePageInfo();
            IsCanSaveReportEnabled = false;
            return;
        }

        var report = Report;
        await FormRowsEnsureService.ReloadNotesFromDbAsync(report)
            .ConfigureAwait(true);

        if (FormRowsPageLoader.SupportsDbPaging(FormType))
        {
            UseDbPaging = true;
            DbTotalRows = await FormRowsPageLoader
                .CountAsync(db, report.Id, FormType)
                .ConfigureAwait(true);

            var skip = Math.Max(0, (CurrentPage - 1) * RowCount);
            var page = await FormRowsPageLoader
                .LoadMergedVisiblePageAsync(db, report.Id, FormType, skip, RowCount)
                .ConfigureAwait(true);
            FormRowsPageLoader.ApplyPageToReport(report, FormType, page);
        }
        else
        {
            UseDbPaging = false;
            DbTotalRows = null;
            await FormRowsEnsureService.ForceReplaceRowsAndNotesFromDbAsync(report)
                .ConfigureAwait(true);
        }

        foreach (var key in report[FormType].GetEnumerable().ToList())
        {
            if (key is Form { Id: 0 } form)
                report[FormType].Remove(form);
        }

        foreach (var note in report.Notes.Where(n => n.Id == 0).ToList())
            report.Notes.Remove(note);

        UpdateFormList();
        UpdateNoteList();
        UpdatePageInfo();
        IsCanSaveReportEnabled = false;
        if (UseDbPaging)
            WarmVisiblePageCache();
    }

    private readonly object _formPageCacheGate = new();
    private readonly Dictionary<(int Page, int RowCount), List<Form>> _formPageCache = new();
    private CancellationTokenSource? _formPrefetchCts;

    public void WarmVisiblePageCache()
    {
        if (!UseDbPaging || Report?.Id <= 0 || !FormRowsPageLoader.SupportsDbPaging(FormType))
            return;
        CacheCurrentFormPage();
        ScheduleFormPagePrefetch();
    }

    private void ClearFormPageCache()
    {
        Interlocked.Increment(ref _contentLoadGeneration);
        CancelPagingDebounce();
        IsContentLoading = false;
        try { _formPrefetchCts?.Cancel(); } catch { /* ignore */ }
        _formPrefetchCts = null;
        lock (_formPageCacheGate)
            _formPageCache.Clear();
    }

    private async Task RefreshVisibleRowsAsync()
    {
        if (!UseDbPaging)
        {
            UpdateFormList();
            UpdatePageInfo();
            return;
        }

        await RefreshFormListForPagingAsync().ConfigureAwait(true);
    }

    /// <summary>
    /// Смена страницы: очистить видимый FormList, overlay, затем Apply/UpdateFormList.
    /// </summary>
    private async Task RefreshFormListForPagingAsync()
    {
        if (!UseDbPaging || !FormRowsPageLoader.SupportsDbPaging(FormType) || Report?.Id <= 0)
        {
            UpdateFormList();
            UpdatePageInfo();
            return;
        }

        var generation = Interlocked.Increment(ref _contentLoadGeneration);
        ContentLoadingMessage = "Загрузка…";
        IsContentLoading = true;
        if (_formList.Count > 0)
            _formList.Clear();

        try
        {
            await Dispatcher.UIThread
                .InvokeAsync(static () => { }, DispatcherPriority.Render);

            if (generation != _contentLoadGeneration || Report?.Id <= 0)
                return;

            var page = CurrentPage;
            var rowCount = RowCount;
            var key = (page, rowCount);
            var reportId = Report.Id;
            var formType = FormType;
            var skip = Math.Max(0, (page - 1) * rowCount);

            List<Form>? cached;
            lock (_formPageCacheGate)
                _formPageCache.TryGetValue(key, out cached);

            List<Form>? items;
            if (cached != null)
            {
                items = cached;
            }
            else
            {
                var pending = FormRowsPageLoader.CapturePending(
                    StaticConfiguration.DBModel, reportId, formType);
                items = await Task.Run(async () =>
                        await FormRowsPageLoader
                            .LoadMergedVisiblePageAsync(pending, reportId, formType, skip, rowCount)
                            .ConfigureAwait(false))
                    .ConfigureAwait(true);

                if (generation != _contentLoadGeneration)
                    return;
                if (items == null || Report?.Id != reportId)
                    return;
            }

            FormRowsPageLoader.ApplyPageToReport(Report, FormType, items);
            if (cached == null)
                PutFormPageCache(key, items);
            UpdateFormList();
            UpdatePageInfo();
            ScheduleFormPagePrefetch();
        }
        finally
        {
            if (generation == _contentLoadGeneration)
                IsContentLoading = false;
        }
    }

    private void PutFormPageCache((int Page, int RowCount) key, List<Form> items)
    {
        lock (_formPageCacheGate)
        {
            _formPageCache[key] = items;
            TrimFormPageCache_NoLock();
        }
    }

    private void CacheCurrentFormPage()
    {
        if (Report?.Rows == null) return;
        PutFormPageCache((CurrentPage, RowCount), Report.Rows.ToList<Form>());
    }

    private void TrimFormPageCache_NoLock()
    {
        var current = CurrentPage;
        var rowCount = RowCount;
        if (_formPageCache.Count <= 9) return;
        var keep = new HashSet<(int, int)>
        {
            (current, rowCount),
            (current - 1, rowCount),
            (current + 1, rowCount),
            (current - 2, rowCount),
            (current + 2, rowCount)
        };
        foreach (var k in _formPageCache.Keys.Where(k => !keep.Contains(k)).ToList())
            _formPageCache.Remove(k);
    }

    private static IEnumerable<int> NeighborFormPages(int current, int radius)
    {
        for (var d = 1; d <= radius; d++)
        {
            yield return current - d;
            yield return current + d;
        }
    }

    private void ScheduleFormPagePrefetch()
    {
        if (!UseDbPaging || Report?.Id <= 0) return;
        if (!FormRowsPageLoader.SupportsDbPaging(FormType)) return;
        try { _formPrefetchCts?.Cancel(); } catch { /* ignore */ }
        var cts = new CancellationTokenSource();
        _formPrefetchCts = cts;
        var reportId = Report.Id;
        var formType = FormType;
        var rowCount = RowCount;
        var totalPages = TotalPages;
        var current = CurrentPage;
        var pending = FormRowsPageLoader.CapturePending(StaticConfiguration.DBModel, reportId, formType);
        var gate = _formPageCacheGate;
        var cache = _formPageCache;

        _ = Task.Run(async () =>
        {
            try
            {
                foreach (var p in NeighborFormPages(current, radius: 2))
                {
                    if (cts.IsCancellationRequested || p < 1 || p > totalPages) continue;
                    var key = (p, rowCount);
                    lock (gate)
                    {
                        if (cache.ContainsKey(key)) continue;
                    }

                    var skip = (p - 1) * rowCount;
                    var items = await FormRowsPageLoader.LoadMergedVisiblePageAsync(
                        pending, reportId, formType, skip, rowCount, cts.Token)
                        .ConfigureAwait(false);
                    if (cts.IsCancellationRequested) continue;
                    lock (gate)
                    {
                        if (!cache.ContainsKey(key))
                            cache[key] = items;
                    }
                }
            }
            catch
            {
                // best-effort
            }
        }, cts.Token);
    }

    #endregion

    #region UpdatePageInfo

    /// <summary>
    /// Обновляет отображаемое количество страниц и строчек.
    /// </summary>
    public void UpdatePageInfo()
    {
        OnPropertyChanged(nameof(TotalPages));
        OnPropertyChanged(nameof(TotalRows));
    }

    #endregion

    #region OnPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}