using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Models.DTO;

namespace Client_App.Services;

/// <summary>
/// Сервис для проверки обновлений через сайт
/// </summary>
public class UpdateChecker
{
    private const string UpdateUrl = "https://www.norao.ru/sguk/software/mpzf/";
    private readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(10)
    };

    /// <summary>
    /// Проверяет наличие обновлений
    /// </summary>
    /// <returns>Информация об обновлении или null, если обновлений нет или нет подключения</returns>
    public async Task<UpdateInfo?> CheckForUpdatesAsync()
    {
        try
        {
            // Увеличиваем таймаут для медленных соединений
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
            
            var response = await _httpClient.GetStringAsync(UpdateUrl);
            return ParseVersionFromHtml(response);
        }
        catch (HttpRequestException ex)
        {
            // Логируем ошибку сети, но не прерываем работу
            System.Diagnostics.Debug.WriteLine($"Network error during update check: {ex.Message}");
            return null;
        }
        catch (TaskCanceledException ex)
        {
            // Таймаут запроса
            System.Diagnostics.Debug.WriteLine($"Update check timeout: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            // Любые другие ошибки
            System.Diagnostics.Debug.WriteLine($"Update check failed: {ex.Message}");
            return null; // Нет интернета или сайт недоступен
        }
    }
    
    /// <summary>
    /// Парсит версию из HTML страницы
    /// </summary>
    /// <param name="html">HTML код страницы</param>
    /// <returns>Информация об обновлении или null</returns>
    private UpdateInfo? ParseVersionFromHtml(string html)
    {
        // Ищем текст: "Актуальная версия МПЗФ, рекомендуемая для загрузки и установки - 1.3.0.3 от 29.12.2025"
        // Учитываем HTML теги и атрибуты стилей
        var pattern = @"Актуальная версия МПЗФ.*?-\s*([\d.]+)\s+от\s*([\d.]+)";
        var match = Regex.Match(html, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        
        if (!match.Success)
        {
            // Альтернативный паттерн - ищем просто версию и дату после "Актуальная версия МПЗФ"
            var altPattern = @"Актуальная версия МПЗФ[^0-9]*([\d.]+)[^0-9]*от[^0-9]*([\d.]+)";
            match = Regex.Match(html, altPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
        
        if (!match.Success)
        {
            // Еще один вариант - ищем версию в формате X.X.X.X и дату в формате DD.MM.YYYY
            var versionPattern = @"(\d+\.\d+\.\d+\.\d+)[^0-9]*от[^0-9]*(\d{2}\.\d{2}\.\d{4})";
            match = Regex.Match(html, versionPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
        
        if (match.Success)
        {
            var version = match.Groups[1].Value;
            var date = match.Groups[2].Value;
            
            return new UpdateInfo
            {
                Version = Version.Parse(version),
                ReleaseDate = date,
                DownloadUrl = GetDownloadUrlForCurrentPlatform(),
                Description = "Доступна новая версия программы МПЗФ с улучшениями и исправлениями."
            };
        }
        
        return null;
    }
    
    /// <summary>
    /// Возвращает URL для скачивания в зависимости от текущей платформы
    /// </summary>
    /// <returns>URL страницы загрузки</returns>
    private string GetDownloadUrlForCurrentPlatform()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "https://www.norao.ru/sguk/software/mpzf/windows/";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "https://www.norao.ru/sguk/software/mpzf/linux/";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "https://www.norao.ru/sguk/software/mpzf/"; // Основная страница для macOS
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Platform detection failed: {ex.Message}");
        }
        
        // Запасной вариант - основная страница
        return "https://www.norao.ru/sguk/software/mpzf/";
    }
    
    /// <summary>
    /// Получает текущую версию приложения
    /// </summary>
    /// <returns>Текущая версия</returns>
    public static Version GetCurrentVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version ?? new Version("1.0.0.0");
    }
}
