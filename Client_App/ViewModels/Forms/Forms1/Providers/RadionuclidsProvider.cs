using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Client_App.ViewModels.Forms.Forms1.Items;
using Spravochniki;

namespace Client_App.ViewModels.Forms.Forms1.Providers;

/// <summary>
/// Провайдер радионуклидов для форм 1.x
/// </summary>
public static class RadionuclidsProvider
{
    /// <summary>
    /// Все радионуклиды из справочника
    /// </summary>
    public static ObservableCollection<RadionuclidItem> AllRadionuclids { get; } =
        new(Spravochniki.Spravochniks.SprRadionuclids
            .Select(r => new RadionuclidItem { Name = r.name })
            .OrderBy(r => r.Name));

    /// <summary>
    /// Возвращает список радионуклидов, которых ещё нет в текущей строке (доступных для добавления)
    /// </summary>
    public static List<RadionuclidItem> GetAvailableRadionuclids(string currentRadionuclids)
    {
        var existing = ParseRadionuclids(currentRadionuclids);
        return AllRadionuclids
            .Where(r => !existing.Contains(r.Name.ToLower()))
            .ToList();
    }

    /// <summary>
    /// Возвращает список уже введённых радионуклидов (для удаления)
    /// </summary>
    public static List<RadionuclidItem> GetCurrentRadionuclids(string currentRadionuclids)
    {
        var existing = ParseRadionuclids(currentRadionuclids);
        return AllRadionuclids
            .Where(r => existing.Contains(r.Name.ToLower()))
            .ToList();
    }

    /// <summary>
    /// Добавляет радионуклид в строку с разделителем "; "
    /// </summary>
    public static string AddRadionuclid(string currentRadionuclids, string radionuclidToAdd)
    {
        var existing = ParseRadionuclids(currentRadionuclids);
        if (existing.Contains(radionuclidToAdd.ToLower()))
            return currentRadionuclids;

        if (string.IsNullOrEmpty(currentRadionuclids) || currentRadionuclids.Trim() == "")
            return radionuclidToAdd;

        return $"{currentRadionuclids}; {radionuclidToAdd}";
    }

    /// <summary>
    /// Удаляет радионуклид из строки
    /// </summary>
    public static string RemoveRadionuclid(string currentRadionuclids, string radionuclidToRemove)
    {
        var parts = currentRadionuclids
            .Split(';')
            .Select(p => p.Trim())
            .Where(p => p.ToLower() != radionuclidToRemove.ToLower() && !string.IsNullOrEmpty(p))
            .ToList();

        return string.Join("; ", parts);
    }

    private static HashSet<string> ParseRadionuclids(string value)
    {
        if (string.IsNullOrEmpty(value))
            return [];

        return value
            .ToLower()
            .Split(';')
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrEmpty(p))
            .ToHashSet();
    }
}
