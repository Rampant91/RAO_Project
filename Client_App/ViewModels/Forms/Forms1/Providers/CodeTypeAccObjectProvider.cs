using System.Collections.Generic;
using System.Collections.ObjectModel;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.ViewModels.Forms.Forms1.Providers;

public static class CodeTypeAccObjectProvider
{
    /// <summary>
    /// 
    /// </summary>
    public static ReadOnlyCollection<CodeTypeAccObjectItem> AllCodeTypeAccObjects { get; }

    static CodeTypeAccObjectProvider()
    {
        AllCodeTypeAccObjects = new ReadOnlyCollection<CodeTypeAccObjectItem>(
        [
            new CodeTypeAccObjectItem { Code = 11, Description = "ОРИ в виде отдельных изделий, содержащие твердые РВ" },
            new CodeTypeAccObjectItem { Code = 12, Description = "ОРИ в виде отдельных изделий, содержащие жидкие РВ" },
            new CodeTypeAccObjectItem { Code = 13, Description = "ОРИ в виде отдельных изделий, содержащие газообразные РВ" },
            new CodeTypeAccObjectItem { Code = 21, Description = "ОРИ, кроме отдельных изделий, содержащие твердые РВ" },
            new CodeTypeAccObjectItem { Code = 22, Description = "ОРИ, кроме отдельных изделий, содержащие жидкие РВ" },
            new CodeTypeAccObjectItem { Code = 23, Description = "ОРИ, кроме отдельных изделий, содержащие газообразные РВ" },
            new CodeTypeAccObjectItem { Code = 31, Description = "радиофармпрепараты, в виде ОРИ, содержащие твердые PB" },
            new CodeTypeAccObjectItem { Code = 32, Description = "радиофармпрепараты, в виде ОРИ, содержащие жидкие PB" },
            new CodeTypeAccObjectItem { Code = 33, Description = "радиофармпрепараты, в виде ОРИ, содержащие газообразные РВ" },
            new CodeTypeAccObjectItem { Code = 99, Description = "РВ на основе радионуклидов с периодом полураспада " +
                                                                 "до 60 суток, включая йод-125, прочие " +
                                                                 "(укажите в примечании пояснения о составе РВ)" }
        ]);
    }

    /// <summary>
    /// Валидные коды
    /// </summary>
    public static ICollection<string> GetValidCodes()
    {
        return ["11", "12", "13", "21", "22", "23", "31", "32", "33", "99"];
    }
}