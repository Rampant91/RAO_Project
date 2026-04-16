using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.ViewModels.Forms.Forms1.Providers;

public static class DocumentVidProvider
{
    /// <summary>
    /// 
    /// </summary>
    public static ObservableCollection<DocumentVidItem> AllDocumentVids { get; } =
    [
            new() { Code = null, Description = "Значение головной строчки (для форм 1.7 и 1.8)" },
            new() { Code = 1, Description = "Акт" },
            new() { Code = 2, Description = "Ведомость" },
            new() { Code = 3, Description = "Грузовая таможенная декларация" },
            new() { Code = 4, Description = "Журнал" },
            new() { Code = 5, Description = "Карта" },
            new() { Code = 6, Description = "Накладная" },
            new() { Code = 7, Description = "Наряд" },
            new() { Code = 8, Description = "Ордер" },
            new() { Code = 9, Description = "Паспорт" },
            new() { Code = 10, Description = "Приказ" },
            new() { Code = 11, Description = "Протокол" },
            new() { Code = 12, Description = "Распоряжение" },
            new() { Code = 13, Description = "Решение о продлении НСС" },
            new() { Code = 14, Description = "Требование" },
            new() { Code = 15, Description = "Сертификат" },
            new() { Code = 19, Description = "Другой документ (заполните примечание" +
                                                           $"{Environment.NewLine}к ячейке наименованием документа)" }
    ];
    

    /// <summary>
    /// Допустимые коды вида документа для форм 1.1-1.6
    /// </summary>
    public static ICollection<string?> GetValidCodesForForms11To16()
    {
        return ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "19"];
    }

    /// <summary>
    /// Допустимые коды вида документа для форм 1.7-1.8
    /// </summary>
    public static ICollection<string?> GetValidCodesForForms17To18()
    {
        return ["-", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "19"];
    }

    /// <summary>
    /// Допустимые коды вида документа для форм 1.1-1.8
    /// </summary>
    public static ICollection<string?> GetValidCodesForForms19()
    {
        return ["1"];
    }
}