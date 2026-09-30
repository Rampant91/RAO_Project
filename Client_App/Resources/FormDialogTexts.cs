using System;
using MsBox.Avalonia.Models;

namespace Client_App.Resources;

/// <summary>
/// Строки диалогов окон форм.
/// </summary>
public static class FormDialogTexts
{
    public const string Yes = "Да";
    public const string No = "Нет";
    public const string Cancel = "Отмена";

    public const string SaveChangesTitle = "Сохранение изменений";
    public const string CreateOrganizationTitle = "Создание организации";
    public const string NotificationHeader = "Уведомление";
    public const string ErrorHeader = "Ошибка";

    public const string OrgTitleSaveErrorTitle =
        "Ошибка при сохранении титульного листа организации";

    public const string OrgDuplicateSaveErrorMessage =
        "Не удалось сохранить изменения в титульном листе организации, " +
        "поскольку организация с данными ОКПО и рег. № уже существует в базе данных. " +
        "Продолжите с корректировкой указанных ОКПО и рег. №.";

    /// <summary>
    /// Закрытие окна новой (ещё не сохранённой в БД) карточки организации с заполненными полями.
    /// </summary>
    public static string SaveNewOrganizationCardMessage =>
        "Карточка организации ещё не сохранена в базе данных." +
        $"{Environment.NewLine}Если отказаться, организация не будет создана." +
        $"{Environment.NewLine}Сохранить организацию?";

    public static string SaveFormMessage(string formType) =>
        $"Сохранить форму {formType}?";

    public static string PeriodIntersectionMessage(
        string orgKey,
        string formNum,
        string existingStart,
        string existingEnd,
        string currentStart,
        string currentEnd) =>
        $"В организации {orgKey}{Environment.NewLine}" +
        $"обнаружен отчёт за период {formNum} {existingStart}-{existingEnd}{Environment.NewLine}" +
        $"пересекающийся с текущим периодом {currentStart}-{currentEnd}.";

    public static string RemoveEmptyRowsMessage(string formType) =>
        $"В форме {formType} обнаружены пустые строки.{Environment.NewLine}" +
        $"Вы хотите их удалить?";

    public static string IncompleteJuridicalPersonFieldsCloseMessage =>
        "Не все обязательные поля юридического лица заполнены, " +
        $"{Environment.NewLine}часть заполненных полей будет очищена при сохранении. " +
        $"{Environment.NewLine}Вы уверены, что хотите закрыть форму, " +
        $"не сохранив внесённые изменения?";

    public const string SavePackagePassportMessage =
        "Сохранить паспорт на упаковку перед закрытием окна редактирования?";

    public const string Ok = "Ок";

    public static string SaveErrorMessage(string details) =>
        $"Произошла ошибка во время попытки сохранения:{Environment.NewLine}{details}";

    public static ButtonDefinition YesButton => new() { Name = Yes };
    public static ButtonDefinition NoButton => new() { Name = No };
    public static ButtonDefinition CancelButton => new() { Name = Cancel };
    public static ButtonDefinition OkButton => new() { Name = Ok };
}
