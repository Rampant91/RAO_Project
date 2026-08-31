using System;

namespace Client_App.Services.Updates;

/// <summary>
/// Когда при автопроверке можно не показывать диалог обновления снова.
/// Новый releaseId / версия с сайта всегда показываются; отложить можно только уже виденное.
/// </summary>
public static class UpdateAutoPromptPolicy
{
    /// <summary>
    /// Подавить автодиалог: тот же ключ релиза уже предлагали и с тех пор не прошло суток.
    /// </summary>
    /// <param name="availableKey">releaseId (сеть) или строка версии (сайт).</param>
    /// <param name="lastNotifiedKey">Ключ последнего показанного предложения.</param>
    /// <param name="lastNotifiedAt">Время последнего предложения / автопроверки с показом.</param>
    /// <param name="now">Текущее время (для тестов).</param>
    public static bool ShouldSuppressAutoPrompt(
        string availableKey,
        string? lastNotifiedKey,
        DateTime lastNotifiedAt,
        DateTime now)
    {
        if (string.IsNullOrWhiteSpace(availableKey))
        {
            return false;
        }

        if (!string.Equals(
                (lastNotifiedKey ?? string.Empty).Trim(),
                availableKey.Trim(),
                StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return (now - lastNotifiedAt).TotalDays < 1;
    }
}
