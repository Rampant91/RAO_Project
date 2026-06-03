using System.Collections.Generic;
using System.Collections.ObjectModel;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.ViewModels.Forms.Forms1.Providers;

public static class RefineOrSortRAOCodeProvider
{
    /// <summary>
    /// 
    /// </summary>
    public static ReadOnlyCollection<RefineOrSortRAOCodeItem> AllRefineOrSortRAOCodes { get; }

    static RefineOrSortRAOCodeProvider()
    {
        AllRefineOrSortRAOCodes = new ReadOnlyCollection<RefineOrSortRAOCodeItem>(
        [
            new RefineOrSortRAOCodeItem { Code = "-", Description = "пустое значение / " +
                                                                    "значение головной формы (для форм 1.7 и 1.8)" },

            new RefineOrSortRAOCodeItem { Code = "11", Description = "установка сорбционная (ионообменная)" },
            
            new RefineOrSortRAOCodeItem { Code = "12", Description = "установка мембранная" },
            
            new RefineOrSortRAOCodeItem { Code = "13", Description = "установка мембранно-сорбционная" },
            
            new RefineOrSortRAOCodeItem { Code = "14", Description = "установка фильтрации" },
            
            new RefineOrSortRAOCodeItem { Code = "15", Description = "установка фракционирования радионуклидов" },
            
            new RefineOrSortRAOCodeItem { Code = "16", Description = "установка осадительная" },
            
            new RefineOrSortRAOCodeItem { Code = "17", Description = "установка коагуляционная" },
            
            new RefineOrSortRAOCodeItem { Code = "19", Description = "установки разделения прочие (укажите в примечании " +
                                                                     "наименование установки и используемую технологию)" },
            
            new RefineOrSortRAOCodeItem { Code = "21", Description = "установка упаривания" },
            
            new RefineOrSortRAOCodeItem { Code = "22", Description = "установка сжигания" },
            
            new RefineOrSortRAOCodeItem { Code = "23", Description = "установка плавления" },
            
            new RefineOrSortRAOCodeItem { Code = "24", Description = "установка кальцинации" },
            
            new RefineOrSortRAOCodeItem { Code = "29", Description = "установки термообработки прочие (укажите в примечании " +
                                                                     "наименование установки и используемую технологию)" },
            
            new RefineOrSortRAOCodeItem { Code = "31", Description = "установка прессования" },
            
            new RefineOrSortRAOCodeItem { Code = "32", Description = "установка суперкомпактирования" },
            
            new RefineOrSortRAOCodeItem { Code = "39", Description = "установки компактирования прочие (укажите в примечании " +
                                                                     "наименование установки и используемую технологию)" },
            
            new RefineOrSortRAOCodeItem { Code = "41", Description = "установка битумирования" },
            
            new RefineOrSortRAOCodeItem { Code = "42", Description = "установка цементирования" },
            
            new RefineOrSortRAOCodeItem { Code = "43", Description = "установка остекловывания" },
            
            new RefineOrSortRAOCodeItem { Code = "49", Description = "установки отверждения прочие (укажите в примечании " +
                                                                     "наименование установки и используемую технологию)" },
            
            new RefineOrSortRAOCodeItem { Code = "51", Description = "установка дезактивации" },
            
            new RefineOrSortRAOCodeItem { Code = "52", Description = "установка сортировки" },
            
            new RefineOrSortRAOCodeItem { Code = "53", Description = "установка измельчения" },
            
            new RefineOrSortRAOCodeItem { Code = "54", Description = "установка фрагментации" },
            
            new RefineOrSortRAOCodeItem { Code = "55", Description = "установка утилизации жидкометаллических теплоносителей" },
            
            new RefineOrSortRAOCodeItem { Code = "61", Description = "комплекс спецводоочистки" },
            
            new RefineOrSortRAOCodeItem { Code = "62", Description = "комплексы переработки жидких РАО прочие (укажите в примечании " +
                                                                     "наименование установки и используемую технологию)" },
            
            new RefineOrSortRAOCodeItem { Code = "63", Description = "комплексы переработки твердых РАО прочие (укажите в примечании " +
                                                                     "наименование установки и используемую технологию)" },
            
            new RefineOrSortRAOCodeItem { Code = "71", Description = "цементирование" },

            new RefineOrSortRAOCodeItem { Code = "72", Description = "сортировка" },

            new RefineOrSortRAOCodeItem { Code = "73", Description = "фрагментация" },

            new RefineOrSortRAOCodeItem { Code = "74", Description = "горячая камера" },

            new RefineOrSortRAOCodeItem { Code = "79", Description = "прочие (укажите в примечании способ переработки)" },
            
            new RefineOrSortRAOCodeItem { Code = "99", Description = "прочие типы установок переработки (укажите в примечании " +
                                                                     "наименование установки и используемую технологию)" }
        ]);
    }

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> GetValidCodes() =>
    [
        "-",
        "11", "12", "13", "14", "15", "16", "17", "19", 
        "21", "22", "23", "24", "29", 
        "31", "32", "39", 
        "41", "42", "43", "49", 
        "51", "52", "53", "54", "55", 
        "61", "62", "63", 
        "71", "72", "73", "74", "79", 
        "99"
    ];
}