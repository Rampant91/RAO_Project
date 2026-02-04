using System;
using CommunityToolkit.Mvvm.Input;
using Models.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Client_App.ViewModels.Messages;

public sealed class SelectReportsMessageWindowVM : INotifyPropertyChanged
{
    #region Fields

    private OrganizationInfo _selectedOrganization;
    private bool _isLoading;
    private readonly IRelayCommand _okCommand;
    private readonly string _fileName;
    private readonly int _totalReports;
    private readonly string _formNum;
    private readonly int _currentReportIndex;

    #endregion

    #region Events

    /// <summary>
    /// Событие запроса закрытия окна с результатом
    /// </summary>
    public event Action<OrganizationInfo>? OkRequested;

    /// <summary>
    /// Событие запроса закрытия окна без результата
    /// </summary>
    public event Action? CancelRequested;

    #endregion

    #region Properties

    /// <summary>
    /// Список организаций для выбора
    /// </summary>
    public ObservableCollection<OrganizationInfo> Organizations { get; }

    /// <summary>
    /// Выбранная организация
    /// </summary>
    public OrganizationInfo SelectedOrganization
    {
        get => _selectedOrganization;
        set
        {
            if (SetProperty(ref _selectedOrganization, value))
            {
                // Уведомляем об изменении возможности выполнения команды Ok
                if (_okCommand is RelayCommand relayCommand)
                {
                    relayCommand.NotifyCanExecuteChanged();
                }
            }
        }
    }

    /// <summary>
    /// Флаг загрузки данных
    /// </summary>
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    /// <summary>
    /// Информация об импортируемом файле
    /// </summary>
    public string ImportInfo => $"Импортируемый файл \"{_fileName}\" содержит {_totalReports} отчётов.";

    /// <summary>
    /// Информация о текущем импорте
    /// </summary>
    public static string CurrentImportInfo => "Импорт из .RAODB.";

    /// <summary>
    /// Информация о форме и периоде
    /// </summary>
    public string FormInfo { get; }

    #endregion

    #region Commands

    /// <summary>
    /// Команда подтверждения выбора
    /// </summary>
    public IRelayCommand OkCommand => _okCommand;

    private void OkAsync()
    {
        // Закрытие окна с результатом
        if (SelectedOrganization != null)
        {
            OkRequested?.Invoke(SelectedOrganization);
        }
    }

    private bool CanOk()
    {
        return SelectedOrganization != null;
    }

    /// <summary>
    /// Команда отмены выбора
    /// </summary>
    public IRelayCommand CancelCommand => new RelayCommand(CancelAsync);

    private void CancelAsync()
    {
        // Закрытие окна без результата
        CancelRequested?.Invoke();
    }

    /// <summary>
    /// Команда загрузки списка организаций
    /// </summary>
    public IAsyncRelayCommand LoadOrganizationsCommand => new AsyncRelayCommand(LoadOrganizationsAsync);

    private async Task LoadOrganizationsAsync()
    {
        try
        {
            IsLoading = true;
            Organizations.Clear();

            // TODO: Здесь будет ваша команда для загрузки организаций
            // Пример:
            // var organizations = await yourLoadCommand.ExecuteAsync();
            // foreach (var org in organizations)
            // {
            //     Organizations.Add(org);
            // }

            // Временные данные для демонстрации
            await Task.Delay(500); // Имитация загрузки
            Organizations.Add(new OrganizationInfo { RegNum = "1234567890", ShortName = "ООО 'Ромашка'", Okpo = "12345678" });
            Organizations.Add(new OrganizationInfo { RegNum = "0987654321", ShortName = "АО 'Солнце'", Okpo = "87654321" });
            Organizations.Add(new OrganizationInfo { RegNum = "1122334455", ShortName = "ИП Иванов И.И.", Okpo = "55443322" });
        }
        finally
        {
            IsLoading = false;
        }
    }

    #endregion

    #region Constructor

    public SelectReportsMessageWindowVM() 
    {
        Organizations = new ObservableCollection<OrganizationInfo>();
        _okCommand = new RelayCommand(OkAsync, CanOk);
    }

    public SelectReportsMessageWindowVM(List<Reports> repsList)
    {
        var repsDtoList = repsList
            .Select(reps => new OrganizationInfo 
            {
                Okpo = reps.Master.OkpoRep.Value, 
                ShortName = reps.Master.ShortJurLicoRep.Value, 
                RegNum = reps.Master.RegNoRep.Value
            })
            .ToList();
        Organizations = new ObservableCollection<OrganizationInfo>(repsDtoList);
        _okCommand = new RelayCommand(OkAsync, CanOk);
    }

    public SelectReportsMessageWindowVM(List<Reports> repsList, string fileName, int totalReports, int currentReportIndex, Reports impReps)
    {
        _fileName = fileName;
        _totalReports = totalReports;
        _currentReportIndex = currentReportIndex;
        _formNum = impReps.Master_DB.FormNum_DB;

        FormInfo = $"Импортируются отчёты организации {impReps.Master_DB.RegNoRep.Value}_{impReps.Master.OkpoRep.Value} по форме {_formNum}";

        // Сортируем организации: по рег.№, потом по ОКПО, потом по наименованию
        var repsDtoList = repsList
            .Select(reps => new OrganizationInfo 
            {
                Okpo = reps.Master.OkpoRep.Value, 
                ShortName = reps.Master.ShortJurLicoRep.Value, 
                RegNum = reps.Master.RegNoRep.Value
            })
            .OrderBy(org => org.RegNum)
            .ThenBy(org => org.Okpo)
            .ThenBy(org => org.ShortName)
            .ToList();
            
        Organizations = new ObservableCollection<OrganizationInfo>(repsDtoList);
        _okCommand = new RelayCommand(OkAsync, CanOk);
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion

    /// <summary>
    /// Модель информации об организации для выбора в окне
    /// </summary>
    public sealed class OrganizationInfo : INotifyPropertyChanged
    {
        private string _regNum;
        private string _shortName;
        private string _okpo;

        /// <summary>
        /// Регистрационный номер организации
        /// </summary>
        public string RegNum
        {
            get => _regNum;
            set
            {
                if (_regNum != value)
                {
                    _regNum = value;
                    OnPropertyChanged(nameof(RegNum));
                }
            }
        }

        /// <summary>
        /// Сокращенное наименование организации
        /// </summary>
        public string ShortName
        {
            get => _shortName;
            set
            {
                if (_shortName != value)
                {
                    _shortName = value;
                    OnPropertyChanged(nameof(ShortName));
                }
            }
        }

        /// <summary>
        /// ОКПО организации
        /// </summary>
        public string Okpo
        {
            get => _okpo;
            set
            {
                if (_okpo != value)
                {
                    _okpo = value;
                    OnPropertyChanged(nameof(Okpo));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}