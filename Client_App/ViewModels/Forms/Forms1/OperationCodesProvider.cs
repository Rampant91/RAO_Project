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
        new() { Code = "01", Description = "Код 01" },
        new() { Code = "10", Description = "Код 10" },
        new() { Code = "11", Description = "Код 11" },
        new() { Code = "12", Description = "Код 12" },
        new() { Code = "13", Description = "Код 13" },
        new() { Code = "14", Description = "Код 14" },
        new() { Code = "15", Description = "Код 15" },
        new() { Code = "16", Description = "Код 16" },
        new() { Code = "17", Description = "Код 17" },
        new() { Code = "18", Description = "Код 18" },
        new() { Code = "21", Description = "Код 21" },
        new() { Code = "22", Description = "Код 22" },
        new() { Code = "25", Description = "Код 25" },
        new() { Code = "26", Description = "Код 26" },
        new() { Code = "27", Description = "Код 27" },
        new() { Code = "28", Description = "Код 28" },
        new() { Code = "29", Description = "Код 29" },
        new() { Code = "31", Description = "Код 31" },
        new() { Code = "32", Description = "Код 32" },
        new() { Code = "35", Description = "Код 35" },
        new() { Code = "36", Description = "Код 36" },
        new() { Code = "37", Description = "Код 37" },
        new() { Code = "38", Description = "Код 38" },
        new() { Code = "39", Description = "Код 39" },
        new() { Code = "41", Description = "Код 41" },
        new() { Code = "42", Description = "Код 42" },
        new() { Code = "43", Description = "Код 43" },
        new() { Code = "44", Description = "Код 44" },
        new() { Code = "45", Description = "Код 45" },
        new() { Code = "46", Description = "Код 46" },
        new() { Code = "47", Description = "Код 47" },
        new() { Code = "48", Description = "Код 48" },
        new() { Code = "49", Description = "Код 49" },
        new() { Code = "51", Description = "Код 51" },
        new() { Code = "52", Description = "Код 52" },
        new() { Code = "53", Description = "Код 53" },
        new() { Code = "54", Description = "Код 54" },
        new() { Code = "55", Description = "Код 55" },
        new() { Code = "56", Description = "Код 56" },
        new() { Code = "57", Description = "Код 57" },
        new() { Code = "58", Description = "Код 58" },
        new() { Code = "59", Description = "Код 59" },
        new() { Code = "61", Description = "Код 61" },
        new() { Code = "62", Description = "Код 62" },
        new() { Code = "63", Description = "Код 63" },
        new() { Code = "64", Description = "Код 64" },
        new() { Code = "65", Description = "Код 65" },
        new() { Code = "66", Description = "Код 66" },
        new() { Code = "67", Description = "Код 67" },
        new() { Code = "68", Description = "Код 68" },
        new() { Code = "71", Description = "Код 71" },
        new() { Code = "72", Description = "Код 72" },
        new() { Code = "73", Description = "Код 73" },
        new() { Code = "74", Description = "Код 74" },
        new() { Code = "75", Description = "Код 75" },
        new() { Code = "76", Description = "Код 76" },
        new() { Code = "81", Description = "Код 81" },
        new() { Code = "82", Description = "Код 82" },
        new() { Code = "83", Description = "Код 83" },
        new() { Code = "84", Description = "Код 84" },
        new() { Code = "85", Description = "Код 85" },
        new() { Code = "86", Description = "Код 86" },
        new() { Code = "87", Description = "Код 87" },
        new() { Code = "88", Description = "Код 88" },
        new() { Code = "97", Description = "Код 97" },
        new() { Code = "98", Description = "Код 98" },
        new() { Code = "99", Description = "Код 99" }
    ];

    /// <summary>
    /// Получить допустимые коды для формы 1.1
    /// </summary>
    public static ICollection<string> GetValidCodesForForm11() =>
    [
        "10", "11", "12", "15", "17", "18",
        "21", "22", "25", "27", "28", "29",
        "31", "32", "35", "37", "38", "39",
        "41", "42", "43", "46", "47", 
        "53", "54", "58",             
        "61", "62", "63", "64", "65", "66", "67", "68",
        "71", "72", "73", "74", "75", "81", "82", "83",
        "84", "85", "86", "87", "88", 
        "97", "98", "99"
    ];

    /// <summary>
    /// Получить допустимые коды для формы 1.2
    /// </summary>
    public static ICollection<string> GetValidCodesForForm12() =>
    [
        "10", "11", "12", "17", "18", 
        "21", "22", "25", "27", "28", "29", 
        "31", "32", "35", "37", "38", "39", 
        "41", "42", "46", 
        "53", "54", "58", 
        "61", "62", "63", "64", "66", "67", "68", 
        "71", "72", 
        "73", "74", "75", "81", 
        "82", "83", "84", "85", "86", "87", "88", 
        "97", "98", "99"
    ];

    /// <summary>
    /// Получить допустимые коды для формы 1.3
    /// </summary>
    public static ICollection<string> GetValidCodesForForm13() =>
    [
        "10", "11", "12", "15", "17", "18", 
        "21", "22", "25", "27", "28", "29", 
        "31", "32", "35", "37", "38", 
        "39", "41", "42", "43", "46", "47", "48", 
        "53", "54", "58", 
        "61", "62", "63", "64", "65", "67", "68", 
        "71", "72", "73", "74", "75", 
        "81", "82", "83", "84", "85", "86", "87", "88", 
        "97", "98", "99"
    ];

    /// <summary>
    /// Получить допустимые коды для формы 1.4
    /// </summary>
    public static ICollection<string> GetValidCodesForForm14() =>
    [
        "10", "11", "12", "15", "17", "18", 
        "21", "22", "25", "27", "28", "29", 
        "31", "32", "35", "37", "38", "39", 
        "41", "42", "43", "46", "47", "48", 
        "53", "54", "58", 
        "61", "62", "63", "64", "65", "67", "68", 
        "71", "72", "73", "74", "75",
        "81", "82", "83", "84", "85", "86", "87", "88", 
        "97", "98", "99"
    ];

    /// <summary>
    /// Получить допустимые коды для формы 1.5
    /// </summary>
    public static ICollection<string> GetValidCodesForForm15() =>
    [
        "01",
        "10", "14",
        "21", "22", "25", "26", "27", "28", "29",
        "31", "32", "35", "36", "37", "38", "39",
        "41", "43", "44", "45", "49",       
        "51", "52", "57", "59",             
        "63", "64", "71", "72", "73", "74", "75", "76",
        "84", "88",                         
        "97", "98", "99"
    ];

    /// <summary>
    /// Получить допустимые коды для формы 1.6
    /// </summary>
    public static ICollection<string> GetValidCodesForForm16() =>
    [
        "01",    
        "10", "11", "12", "13", "14", "16", "18",
        "21", "22", "25", "26", "27", "28", "29",
        "31", "32", "35", "36", "37", "38", "39",
        "41", "42", "43", "44", "45", "48", "49",
        "51", "52", "56", "57", "59", 
        "63", "64", "68",             
        "71", "72", "73", "74", "75", "76",
        "97", "98", "99"
    ];

    /// <summary>
    /// Получить допустимые коды для формы 1.7
    /// </summary>
    public static ICollection<string> GetValidCodesForForm17() =>
    [
        "10", "11", "12", "13", "14", "16", "18",
        "21", "22", "25", "26", "27", "28", "29",
        "31", "32", "35", "36", "37", "38", "39",
        "43", "44", "45", "51", "52", "55", "63",
        "64", "68", "71", "97", "98"
    ];

    /// <summary>
    /// Получить допустимые коды для формы 1.8
    /// </summary>
    public static ICollection<string> GetValidCodesForForm18() =>
    [
        "01",
        "10", "11", "12", "13", "18",
        "21", "25", "26", "27", "28", "29",
        "31", "32", "35", "36", "37", "38", "39",
        "42",       
        "51", "52", "55",
        "63", "64", "68",
        "97", "98"
    ];

    /// <summary>
    /// Получить допустимые коды для формы 1.9
    /// </summary>
    public static ICollection<string> GetValidCodesForForm19() =>
    [
        "10"
    ];
}
