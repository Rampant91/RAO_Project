using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Client_App.ViewModels.Forms.Forms1;

/// <summary>
/// Провайдер кодов операций для форм 1.x
/// </summary>
public static class OperationCodesProvider
{
    /// <summary>
    /// Все коды операций с описаниями
    /// </summary>
    public static ObservableCollection<OperationCodeItem> AllOperationCodes { get; } =
    [
        new() { Code = "01", Description = "Захоронение РАО" },
        new() { Code = "02", Description = "Захоронение РАО (временное)" },
        new() { Code = "03", Description = "Захоронение РАО (контрольное)" },
        new() { Code = "04", Description = "Захоронение РАО (другое)" },
        new() { Code = "05", Description = "Захоронение РАО (иное)" },
        new() { Code = "11", Description = "Переработка РАО" },
        new() { Code = "12", Description = "Переработка РАО (временная)" },
        new() { Code = "13", Description = "Переработка РАО (контрольная)" },
        new() { Code = "14", Description = "Переработка РАО (другое)" },
        new() { Code = "15", Description = "Переработка РАО (иное)" },
        new() { Code = "21", Description = "Обработка РАО" },
        new() { Code = "22", Description = "Обработка РАО (временная)" },
        new() { Code = "23", Description = "Обработка РАО (контрольная)" },
        new() { Code = "24", Description = "Обработка РАО (другое)" },
        new() { Code = "25", Description = "Обработка РАО (иное)" },
        new() { Code = "31", Description = "Хранение РАО" },
        new() { Code = "32", Description = "Хранение РАО (временное)" },
        new() { Code = "33", Description = "Хранение РАО (контрольное)" },
        new() { Code = "34", Description = "Хранение РАО (другое)" },
        new() { Code = "35", Description = "Хранение РАО (иное)" },
        new() { Code = "41", Description = "Транспортировка РАО" },
        new() { Code = "42", Description = "Транспортировка РАО (временная)" },
        new() { Code = "43", Description = "Транспортировка РАО (контрольная)" },
        new() { Code = "44", Description = "Транспортировка РАО (другое)" },
        new() { Code = "45", Description = "Транспортировка РАО (иное)" },
        new() { Code = "51", Description = "Утилизация РАО" },
        new() { Code = "52", Description = "Утилизация РАО (временная)" },
        new() { Code = "53", Description = "Утилизация РАО (контрольная)" },
        new() { Code = "54", Description = "Утилизация РАО (другое)" },
        new() { Code = "55", Description = "Утилизация РАО (иное)" },
        new() { Code = "61", Description = "Дезактивация РАО" },
        new() { Code = "62", Description = "Дезактивация РАО (временная)" },
        new() { Code = "63", Description = "Дезактивация РАО (контрольная)" },
        new() { Code = "64", Description = "Дезактивация РАО (другое)" },
        new() { Code = "65", Description = "Дезактивация РАО (иное)" },
        new() { Code = "71", Description = "Кондиционирование РАО" },
        new() { Code = "72", Description = "Кондиционирование РАО (временное)" },
        new() { Code = "73", Description = "Кондиционирование РАО (контрольное)" },
        new() { Code = "74", Description = "Кондиционирование РАО (другое)" },
        new() { Code = "75", Description = "Кондиционирование РАО (иное)" },
        new() { Code = "81", Description = "Сбор РАО" },
        new() { Code = "82", Description = "Сбор РАО (временный)" },
        new() { Code = "83", Description = "Сбор РАО (контрольный)" },
        new() { Code = "84", Description = "Сбор РАО (другое)" },
        new() { Code = "85", Description = "Сбор РАО (иное)" }
    ];

    /// <summary>
    /// Получить допустимые коды для формы 1.1 (все коды)
    /// </summary>
    public static ICollection<string> GetValidCodesForForm11() => AllOperationCodes.Select(x => x.Code).ToList();

    /// <summary>
    /// Получить допустимые коды для формы 1.2
    /// TODO: Укажите какие коды доступны для формы 1.2
    /// </summary>
    public static ICollection<string> GetValidCodesForForm12()
    {
        // Пример: только коды захоронения
        return AllOperationCodes
            .Where(x => x.Code.StartsWith("0"))
            .Select(x => x.Code)
            .ToList();
    }

    /// <summary>
    /// Получить допустимые коды для формы 1.3
    /// TODO: Укажите какие коды доступны для формы 1.3
    /// </summary>
    public static ICollection<string> GetValidCodesForForm13()
    {
        // Пример: только коды переработки
        return AllOperationCodes
            .Where(x => x.Code.StartsWith("1"))
            .Select(x => x.Code)
            .ToList();
    }

    /// <summary>
    /// Получить допустимые коды для формы 1.4
    /// TODO: Укажите какие коды доступны для формы 1.4
    /// </summary>
    public static ICollection<string> GetValidCodesForForm14()
    {
        // Пример: только коды обработки
        return AllOperationCodes
            .Where(x => x.Code.StartsWith("2"))
            .Select(x => x.Code)
            .ToList();
    }

    /// <summary>
    /// Получить допустимые коды для формы 1.5
    /// TODO: Укажите какие коды доступны для формы 1.5
    /// </summary>
    public static ICollection<string> GetValidCodesForForm15()
    {
        // Пример: только коды хранения
        return AllOperationCodes
            .Where(x => x.Code.StartsWith("3"))
            .Select(x => x.Code)
            .ToList();
    }

    /// <summary>
    /// Получить допустимые коды для формы 1.6
    /// TODO: Укажите какие коды доступны для формы 1.6
    /// </summary>
    public static ICollection<string> GetValidCodesForForm16()
    {
        // Пример: только коды транспортировки
        return AllOperationCodes
            .Where(x => x.Code.StartsWith("4"))
            .Select(x => x.Code)
            .ToList();
    }

    /// <summary>
    /// Получить допустимые коды для формы 1.7
    /// TODO: Укажите какие коды доступны для формы 1.7
    /// </summary>
    public static ICollection<string> GetValidCodesForForm17()
    {
        // Пример: только коды утилизации
        return AllOperationCodes
            .Where(x => x.Code.StartsWith("5"))
            .Select(x => x.Code)
            .ToList();
    }

    /// <summary>
    /// Получить допустимые коды для формы 1.8
    /// TODO: Укажите какие коды доступны для формы 1.8
    /// </summary>
    public static ICollection<string> GetValidCodesForForm18()
    {
        // Пример: только коды дезактивации
        return AllOperationCodes
            .Where(x => x.Code.StartsWith("6"))
            .Select(x => x.Code)
            .ToList();
    }

    /// <summary>
    /// Получить допустимые коды для формы 1.9
    /// TODO: Укажите какие коды доступны для формы 1.9
    /// </summary>
    public static ICollection<string> GetValidCodesForForm19()
    {
        // Пример: только коды кондиционирования
        return AllOperationCodes
            .Where(x => x.Code.StartsWith("7"))
            .Select(x => x.Code)
            .ToList();
    }
}
