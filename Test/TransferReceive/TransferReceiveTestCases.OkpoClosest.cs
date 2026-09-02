using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>
/// O/H — closest при ошибке в кол.19: идентичность по паспорту/зав.№, но чужой ОКПО пост./пол.
/// Данные обобщены (не копия конкретных строк БД).
/// </summary>
internal static partial class TransferReceiveTestCases
{
    private const string WrongProviderOkpo = "30000099";
    private const string SharedOpDate = "2023-08-10";
    private const string SharedPas = "С-ААА-24";
    private const string SharedFac = "51020-24-07";
    private const string SharedType = "ИРИК-М";
    private const string SharedRads = "Co-60";
    private const string SharedPack = "PKG-8841";
    private const string SharedActivity = "2.2e+08";
    private const string SharedCreator = "07545903";

    private static IEnumerable<TransferReceiveTestCase> OkpoClosestCases()
    {
        yield return O06_CounterpartWrongProviderOkpo_ClosestIdentityMismatchOkpo();
    }

    /// <summary>
    /// O06. У контрагента совпадают ключевые поля, но в кол.19 указан чужой ОКПО → непарная;
    /// closest — эта строка (не «decoy» с правильным ОКПО, но другим изделием).
    /// </summary>
    private static TransferReceiveTestCase O06_CounterpartWrongProviderOkpo_ClosestIdentityMismatchOkpo() => new()
    {
        Name = "O06. Ошибка в кол.19 контрагента — closest по паспорту/зав.№, ОКПО красный.",
        OurOkpo = DefaultOurOkpo,
        OurOps =
        [
            RowTransfer(
                1,
                opCode: "28",
                opDate: SharedOpDate,
                pasNum: SharedPas,
                facNum: SharedFac,
                type: SharedType,
                rads: SharedRads,
                pack: SharedPack,
                providerOkpo: DefaultCounterpartOkpo,
                activity: SharedActivity,
                creatorOkpo: SharedCreator,
                creationDate: SharedOpDate)
        ],
        CounterpartOps =
        [
            // «Правильная» по изделию, но ошибочный ОКПО в кол.19.
            RowReceive(
                101,
                opCode: "38",
                opDate: SharedOpDate,
                pasNum: SharedPas,
                facNum: SharedFac,
                type: SharedType,
                rads: SharedRads,
                pack: SharedPack,
                providerOkpo: WrongProviderOkpo,
                activity: SharedActivity,
                creatorOkpo: SharedCreator,
                creationDate: SharedOpDate),
            // Decoy: кол.19 указывает на нас, но другое изделие и дата +14 дн.
            RowReceive(
                102,
                opCode: "38",
                opDate: "2023-08-24",
                pasNum: "С-БББ-99",
                facNum: "51020-24-29",
                type: "ПГЛ-М",
                rads: "Ge-68",
                pack: "PKG-1200",
                providerOkpo: DefaultOurOkpo,
                activity: "5.18e+07",
                creatorOkpo: SharedCreator,
                creationDate: "2023-08-16")
        ],
        ExpectedUnpairedIds = [1],
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 },
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.ProviderOrRecieverOkpo] = FieldMatchLevel.Mismatch,
                [TransferReceiveField.PassportNumber] = FieldMatchLevel.Exact,
                [TransferReceiveField.FactoryNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedConfidenceMinPercent = new Dictionary<int, int> { [1] = 80 }
    };
}
