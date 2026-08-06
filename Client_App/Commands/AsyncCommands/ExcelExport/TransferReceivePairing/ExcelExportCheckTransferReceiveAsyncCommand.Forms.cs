using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Multi-form registry

    /// <summary>
    /// Идентификатор формы в конвейере приёма-передачи.
    /// Новые формы: значение enum + строка в <see cref="ImplementedFormDescriptors"/> + load/Excel.
    /// </summary>
    public enum TransferReceiveFormId
    {
        Form11 = 11,
        Form12 = 12,
        Form13 = 13
    }

    /// <summary>Вариант колонок Excel для формы.</summary>
    public enum TransferReceiveSheetLayout
    {
        /// <summary>1.1: тип, радионуклиды, количество, активность.</summary>
        Form11,

        /// <summary>1.2: наименование, тип УКТ, масса (без количества).</summary>
        Form12,

        /// <summary>1.3: тип, радионуклиды, агрегатное состояние, активность.</summary>
        Form13
    }

    /// <summary>Описание реализованной формы: лист Excel, номер, стиль колонок.</summary>
    public sealed record TransferReceiveFormDescriptor(
        TransferReceiveFormId Id,
        string FormNum,
        string SheetName,
        TransferReceiveSheetLayout Layout)
    {
        public bool UsesAggregateStateColumn => Layout == TransferReceiveSheetLayout.Form13;
    }

    /// <summary>
    /// Реестр форм, уже подключённых к выгрузке.
    /// Порядок = порядок discover / bulk / Excel.
    /// </summary>
    public static IReadOnlyList<TransferReceiveFormDescriptor> ImplementedFormDescriptors { get; } =
    [
        new(TransferReceiveFormId.Form11, "1.1", "Форма 1.1", TransferReceiveSheetLayout.Form11),
        new(TransferReceiveFormId.Form12, "1.2", "Форма 1.2", TransferReceiveSheetLayout.Form12),
        new(TransferReceiveFormId.Form13, "1.3", "Форма 1.3", TransferReceiveSheetLayout.Form13)
    ];

    public static TransferReceiveFormDescriptor GetFormDescriptor(TransferReceiveFormId id) =>
        ImplementedFormDescriptors.FirstOrDefault(d => d.Id == id)
        ?? throw new ArgumentOutOfRangeException(nameof(id), id, "Форма не зарегистрирована в ImplementedFormDescriptors.");

    /// <summary>Параметры сверки одной формы (поля Check*).</summary>
    public sealed record TransferReceiveFormParams(
        bool CheckOperationCode = true,
        bool CheckOperationDate = true,
        bool CheckPassportNumber = true,
        /// <summary>Для 1.1/1.3 — тип; для 1.2 — наименование (NameIOU).</summary>
        bool CheckType = true,
        bool CheckRadionuclids = true,
        bool CheckFactoryNumber = true,
        bool CheckQuantity = true,
        bool CheckActivity = true,
        /// <summary>Масса обеднённого урана, кг (форма 1.2).</summary>
        bool CheckMass = false,
        bool CheckCreatorOkpo = true,
        bool CheckCreationDate = true,
        bool CheckProviderOrRecieverOkpo = true,
        bool CheckPackNumber = true,
        /// <summary>Тип УКТ (форма 1.2; на 1.1/1.3 не сравнивается).</summary>
        bool CheckPackType = false,
        bool CheckAggregateState = false);

    /// <summary>
    /// Набор параметров диалога по реализованным формам (порядок: 1.1, 1.2, 1.3).
    /// </summary>
    public sealed class TransferReceiveParamsSet
    {
        public TransferReceiveParamsSet(
            TransferReceiveFormParams form11,
            TransferReceiveFormParams form12,
            TransferReceiveFormParams form13)
        {
            Form11 = form11;
            Form12 = form12;
            Form13 = form13;
        }

        /// <summary>Фабрика с явным порядком форм 1.1 → 1.2 → 1.3.</summary>
        public static TransferReceiveParamsSet Create(
            TransferReceiveFormParams form11,
            TransferReceiveFormParams form12,
            TransferReceiveFormParams form13) =>
            new(form11, form12, form13);

        /// <summary>Обратная совместимость: слот 1.2 выключен.</summary>
        public static TransferReceiveParamsSet Form11And13(
            TransferReceiveFormParams form11,
            TransferReceiveFormParams form13) =>
            new(form11, DisabledFormParams(), form13);

        public TransferReceiveFormParams Form11 { get; }
        public TransferReceiveFormParams Form12 { get; }
        public TransferReceiveFormParams Form13 { get; }

        public TransferReceiveFormParams GetParams(TransferReceiveFormId id) =>
            id switch
            {
                TransferReceiveFormId.Form11 => Form11,
                TransferReceiveFormId.Form12 => Form12,
                TransferReceiveFormId.Form13 => Form13,
                _ => throw new ArgumentOutOfRangeException(nameof(id), id, "Нет слота параметров для формы.")
            };

        public bool IsEnabled(TransferReceiveFormId id) => IsFormCheckEnabled(GetParams(id));

        public bool AnyFormEnabled => ImplementedFormDescriptors.Any(d => IsEnabled(d.Id));

        public IReadOnlyList<TransferReceiveFormDescriptor> EnabledForms =>
            ImplementedFormDescriptors.Where(d => IsEnabled(d.Id)).ToList();

        public HashSet<TransferReceiveFormId> EnabledFormIds =>
            EnabledForms.Select(d => d.Id).ToHashSet();
    }

    /// <summary>Все Check* = false — форма не участвует в выгрузке.</summary>
    public static TransferReceiveFormParams DisabledFormParams() =>
        new(
            CheckOperationCode: false,
            CheckOperationDate: false,
            CheckPassportNumber: false,
            CheckType: false,
            CheckRadionuclids: false,
            CheckFactoryNumber: false,
            CheckQuantity: false,
            CheckActivity: false,
            CheckMass: false,
            CheckCreatorOkpo: false,
            CheckCreationDate: false,
            CheckProviderOrRecieverOkpo: false,
            CheckPackNumber: false,
            CheckPackType: false,
            CheckAggregateState: false);

    /// <summary>
    /// Параметры формы 1.2 по умолчанию: без количества/радионуклидов/активности/агрегатного состояния;
    /// масса и тип УКТ включены; наименование через CheckType.
    /// </summary>
    public static TransferReceiveFormParams DefaultForm12Params() =>
        new(
            CheckQuantity: false,
            CheckRadionuclids: false,
            CheckActivity: false,
            CheckAggregateState: false,
            CheckMass: true,
            CheckPackType: true);

    /// <summary>
    /// Параметры формы 1.3 по умолчанию: количество не сравнивается (всегда 1),
    /// агрегатное состояние включено.
    /// </summary>
    public static TransferReceiveFormParams DefaultForm13Params() =>
        new(CheckQuantity: false, CheckAggregateState: true);

    /// <summary>
    /// Форма участвует в выгрузке, если выбрано хотя бы одно поле.
    /// «Выбрать все параметры» = false → форма не загружается и не пишется в Excel.
    /// </summary>
    public static bool IsFormCheckEnabled(TransferReceiveFormParams options) =>
        options.CheckOperationCode
        || options.CheckOperationDate
        || options.CheckPassportNumber
        || options.CheckType
        || options.CheckRadionuclids
        || options.CheckFactoryNumber
        || options.CheckQuantity
        || options.CheckActivity
        || options.CheckMass
        || options.CheckCreatorOkpo
        || options.CheckCreationDate
        || options.CheckProviderOrRecieverOkpo
        || options.CheckPackNumber
        || options.CheckPackType
        || options.CheckAggregateState;

    #endregion
}
