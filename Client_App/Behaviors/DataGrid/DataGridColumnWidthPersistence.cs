namespace Client_App.Behaviors.DataGrid;

/// <summary>
/// Правила персистенции ширин колонок (без UI).
/// </summary>
internal static class DataGridColumnWidthPersistence
{
    /// <summary>0 или нечисло — оставить ширину из XAML (обычно Star).</summary>
    public static bool ShouldKeepXamlWidth(double savedWidth) =>
        !double.IsFinite(savedWidth) || savedWidth <= 0;

    /// <summary>
    /// Star-restore (наименование забирает остаток, ОКПО Absolute) — только для списков организаций
    /// или явного PreserveStarLayout. У отчётов несколько Star: restore сбрасывает ручной ресайз.
    /// </summary>
    public static bool ShouldPreserveStarLayout(bool preserveStarLayoutProperty, string? formNum) =>
        preserveStarLayoutProperty || IsMainWindowOrgsFormNum(formNum);

    public static bool IsMainWindowOrgsFormNum(string? formNum) =>
        !string.IsNullOrEmpty(formNum)
        && formNum.StartsWith("MainWindow.Orgs.", System.StringComparison.Ordinal);

    /// <summary>
    /// Ширина для записи в конфиг. Avalonia при drag-resize обновляет ActualWidth/DisplayValue,
    /// а Width.Value у Absolute часто остаётся прежним — поэтому ActualWidth в приоритете.
    /// </summary>
    public static double GetPersistableWidth(
        double actualWidth,
        bool widthIsAbsolute,
        bool widthIsStar,
        double widthValue,
        bool preserveStarLayout)
    {
        if (double.IsFinite(actualWidth) && actualWidth > 0)
        {
            return actualWidth;
        }

        // Ещё не измеряли: для нетронутого Star оставляем 0 (XAML Star при следующей загрузке).
        if (preserveStarLayout && widthIsStar)
        {
            return 0;
        }

        if (widthIsAbsolute && double.IsFinite(widthValue) && widthValue > 0)
        {
            return widthValue;
        }

        return 0;
    }
}
