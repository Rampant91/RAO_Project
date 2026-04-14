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
    public static ReadOnlyCollection<DocumentVidItem> AllDocumentVids { get; }

    static DocumentVidProvider()
    {
        AllDocumentVids = new ReadOnlyCollection<DocumentVidItem>(
        [
            new DocumentVidItem { Code = 1, Description = "Акт" },
            new DocumentVidItem { Code = 2, Description = "Ведомость" },
            new DocumentVidItem { Code = 3, Description = "Грузовая таможенная декларация" },
            new DocumentVidItem { Code = 4, Description = "Журнал" },
            new DocumentVidItem { Code = 5, Description = "Карта" },
            new DocumentVidItem { Code = 6, Description = "Накладная" },
            new DocumentVidItem { Code = 7, Description = "Наряд" },
            new DocumentVidItem { Code = 8, Description = "Ордер" },
            new DocumentVidItem { Code = 9, Description = "Паспорт" },
            new DocumentVidItem { Code = 10, Description = "Приказ" },
            new DocumentVidItem { Code = 11, Description = "Протокол" },
            new DocumentVidItem { Code = 12, Description = "Распоряжение" },
            new DocumentVidItem { Code = 13, Description = "Решение о продлении НСС" },
            new DocumentVidItem { Code = 14, Description = "Требование" },
            new DocumentVidItem { Code = 15, Description = "Сертификат" },
            new DocumentVidItem { Code = 19, Description = "Другой документ (заполните примечание" +
                                                           $"{Environment.NewLine}к ячейке наименованием документа)" }
        ]);
    }

    /// <summary>
    /// Допустимые коды вида документа
    /// </summary>
    public static IEnumerable<string> GetValidCodes()
    {
        return ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "19"];
    }
}