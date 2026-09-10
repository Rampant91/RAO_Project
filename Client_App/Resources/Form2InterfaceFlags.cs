namespace Client_App.Resources;

/// <summary>
/// Временный переключатель: открывать формы 2.1–2.12 в новых окнах (Form_21…Form_212)
/// вместо <see cref="Views.FormChangeOrCreate"/>.
/// </summary>
public static class Form2InterfaceFlags
{
#if DEBUG
    /// <summary>В Debug можно включить для тестирования нового UI форм 2.x.</summary>
    public static bool UseNewInterface { get; set; } = true;
#else
    public const bool UseNewInterface = true;
#endif

    public static bool IsEnabledFor(string? formNum) =>
        UseNewInterface && Form2NewInterfaceOpener.IsSupportedForm(formNum);
}
