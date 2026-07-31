using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Multi-form registry (prep for 1.2+)

    /// <summary>
    /// Идентификатор формы в конвейере приёма-передачи.
    /// Новые формы: значение enum + строка в <see cref="ImplementedFormDescriptors"/> + load/Excel.
    /// </summary>
    public enum TransferReceiveFormId
    {
        Form11 = 11,
        Form13 = 13
        // Form12 = 12 — следующий шаг (РАО-колонки, отдельный layout/поля soft).
    }

    /// <summary>Описание реализованной формы: лист Excel, номер, стиль колонок.</summary>
    public sealed record TransferReceiveFormDescriptor(
        TransferReceiveFormId Id,
        string FormNum,
        string SheetName,
        /// <summary>true — колонка qty заменена на агрегатное состояние (как 1.3).</summary>
        bool UsesAggregateStateColumn);

    /// <summary>
    /// Реестр форм, уже подключённых к выгрузке.
    /// Порядок = порядок discover / bulk / Excel.
    /// </summary>
    public static IReadOnlyList<TransferReceiveFormDescriptor> ImplementedFormDescriptors { get; } =
    [
        new(TransferReceiveFormId.Form11, "1.1", "Форма 1.1", UsesAggregateStateColumn: false),
        new(TransferReceiveFormId.Form13, "1.3", "Форма 1.3", UsesAggregateStateColumn: true)
    ];

    public static TransferReceiveFormDescriptor GetFormDescriptor(TransferReceiveFormId id) =>
        ImplementedFormDescriptors.FirstOrDefault(d => d.Id == id)
        ?? throw new ArgumentOutOfRangeException(nameof(id), id, "Форма не зарегистрирована в ImplementedFormDescriptors.");

    /// <summary>Параметры сверки одной формы (поля Check*).</summary>
    public sealed record TransferReceiveFormParams(
        bool CheckOperationCode = true,
        bool CheckOperationDate = true,
        bool CheckPassportNumber = true,
        bool CheckType = true,
        bool CheckRadionuclids = true,
        bool CheckFactoryNumber = true,
        bool CheckQuantity = true,
        bool CheckActivity = true,
        bool CheckCreatorOkpo = true,
        bool CheckCreationDate = true,
        bool CheckProviderOrRecieverOkpo = true,
        bool CheckPackNumber = true,
        bool CheckAggregateState = false);

    /// <summary>
    /// Набор параметров диалога по реализованным формам.
    /// При добавлении формы: свойство + аргумент ctor + ветка в <see cref="GetParams"/>.
    /// </summary>
    public sealed class TransferReceiveParamsSet
    {
        public TransferReceiveParamsSet(TransferReceiveFormParams form11, TransferReceiveFormParams form13)
        {
            Form11 = form11;
            Form13 = form13;
            // Form12 = form12; — следующий шаг
        }

        public TransferReceiveFormParams Form11 { get; }
        public TransferReceiveFormParams Form13 { get; }
        // public TransferReceiveFormParams Form12 { get; }

        public TransferReceiveFormParams GetParams(TransferReceiveFormId id) =>
            id switch
            {
                TransferReceiveFormId.Form11 => Form11,
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
        || options.CheckCreatorOkpo
        || options.CheckCreationDate
        || options.CheckProviderOrRecieverOkpo
        || options.CheckPackNumber
        || options.CheckAggregateState;

    #endregion
}
