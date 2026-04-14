using System.Collections.Generic;
using System.Collections.ObjectModel;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.ViewModels.Forms.Forms1.Providers;

public static class OwnershipProvider
{
    /// <summary>
    /// 
    /// </summary>
    public static ReadOnlyCollection<OwnershipItem> AllOwnershipForms { get; }

    static OwnershipProvider()
    {
        AllOwnershipForms = new ReadOnlyCollection<OwnershipItem>(
        [
            new OwnershipItem { Code = 1, Description = "Государственная. Федеральная собственность." },
            new OwnershipItem { Code = 2, Description = "Государственная. Собственность субъектов Российской Федерации" },
            new OwnershipItem { Code = 3, Description = "Муниципальная" },
            new OwnershipItem { Code = 4, Description = "Частная (собственность юридических лиц)" },
            new OwnershipItem { Code = 5, Description = "Собственность иностранного государства" },
            new OwnershipItem { Code = 6, Description = "Собственность иностранного юридического лица" },
            new OwnershipItem { Code = 9, Description = "Иная форма собственности (вещное право)" }
        ]);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerable<string> GetValidCodes()
    {
        return ["1", "2", "3", "4", "5", "6", "9"];
    }
}
