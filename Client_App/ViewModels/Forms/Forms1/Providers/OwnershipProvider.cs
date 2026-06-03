using System.Collections.Generic;
using System.Collections.ObjectModel;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.ViewModels.Forms.Forms1.Providers;

public static class OwnershipProvider
{
    /// <summary>
    /// 
    /// </summary>
    public static ObservableCollection<OwnershipItem> AllOwnershipForms { get; } = 
    [
            new() { Code = 1, Description = "Государственная. Федеральная собственность." },
            new() { Code = 2, Description = "Государственная. Собственность субъектов Российской Федерации" },
            new() { Code = 3, Description = "Муниципальная" },
            new() { Code = 4, Description = "Частная (собственность юридических лиц)" },
            new() { Code = 5, Description = "Собственность иностранного государства" },
            new() { Code = 6, Description = "Собственность иностранного юридического лица" },
            new() { Code = 9, Description = "Иная форма собственности (вещное право)" }
    ];
    

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> GetValidCodes()
    {
        return ["1", "2", "3", "4", "5", "6", "9"];
    }
}