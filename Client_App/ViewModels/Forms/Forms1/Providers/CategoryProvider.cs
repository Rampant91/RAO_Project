using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.ViewModels.Forms.Forms1.Providers;

/// <summary>
/// Провайдер категорий опасности для форм 1.x
/// </summary>
public static class CategoryProvider
{
    /// <summary>
    /// Все категории с описаниями
    /// </summary>
    public static ObservableCollection<CategoryItem> AllCategories { get; } =
    [
        new() { Code = 1, Description = "Чрезвычайно опасно для человека" +
                                        $"{Environment.NewLine} (A/D >= 1000)" },
        new() { Code = 2, Description = "Очень опасно для человека" +
                                        $"{Environment.NewLine}(10 <= A/D < 1000)" },
        new() { Code = 3, Description = "Опасно для человека" +
                                        $"{Environment.NewLine}(1 <= A/D < 10)" },
        new() { Code = 4, Description = "Опасность для человека маловероятна" +
                                        $"{Environment.NewLine}(0,01 <= A/D < 1)" },
        new() { Code = 5, Description = "Опасность для человека очень маловероятна" +
                                        $"{Environment.NewLine}(A/D < 0,01)" }
    ];

    /// <summary>
    /// Получить допустимые категории для формы 1.1
    /// </summary>
    public static ICollection<short?> GetValidCategoriesForForm11() =>
    [
        1, 2, 3, 4, 5
    ];

    /// <summary>
    /// Получить допустимые категории для формы 1.5
    /// </summary>
    public static ICollection<short?> GetValidCategoriesForForm15() =>
    [
        1, 2, 3, 4, 5
    ];
}
