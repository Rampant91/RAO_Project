namespace Client_App.Services.DataAccess;

/// <summary>
/// Размеры страниц org/report в гридах главного окна (вкладки 1/2/4/5).
/// Должны совпадать с <see cref="ViewModels.MainWindowTabs.FormsTabControlBaseVM"/> defaults и prefetch.
/// </summary>
public static class MainWindowPagingDefaults
{
    public const byte DefaultOrgsPerPage = 8;
    public const byte DefaultFormsPerPage = 10;
}
