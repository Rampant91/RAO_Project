using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>Группа P — снятие Check* превращает расхождение в пару.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> ParamsCases()
    {
        yield return P01_CheckPassportOff_MismatchPaired();
        yield return P02_CheckTypeOff_MismatchPaired();
        yield return P03_CheckOperationCodeOff_WrongCodePaired();
        yield return P04_CheckOperationDateOff_OneDayDiffPaired();
        yield return P05_CheckActivityOff_ActivityMismatchPaired();
        yield return P06_CheckProviderOkpoOff_WrongPointerPaired();
    }

    /// <summary>P01. Разный паспорт, CheckPassportNumber=false → пара.</summary>
    private static TransferReceiveTestCase P01_CheckPassportOff_MismatchPaired() => new()
    {
        Name = "P01. CheckPassportNumber=false — разный паспорт не мешает.",
        Params = new TransferReceiveFormParams(CheckPassportNumber: false),
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, pasNum: "P-001")],
        CounterpartOps = [RowReceive(101, pasNum: "P-OTHER")],
        ExpectedUnpairedIds = []
    };

    /// <summary>P02. Разный тип, CheckType=false → пара.</summary>
    private static TransferReceiveTestCase P02_CheckTypeOff_MismatchPaired() => new()
    {
        Name = "P02. CheckType=false — разный тип не мешает.",
        Params = new TransferReceiveFormParams(CheckType: false),
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, type: "ИИИ")],
        CounterpartOps = [RowReceive(101, type: "ДРУГОЙ")],
        ExpectedUnpairedIds = []
    };

    /// <summary>P03. 21↔32, CheckOperationCode=false → пара.</summary>
    private static TransferReceiveTestCase P03_CheckOperationCodeOff_WrongCodePaired() => new()
    {
        Name = "P03. CheckOperationCode=false — 21↔32 сходится.",
        Params = new TransferReceiveFormParams(CheckOperationCode: false),
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, opCode: "21")],
        CounterpartOps = [RowReceive(101, opCode: "32")],
        ExpectedUnpairedIds = []
    };

    /// <summary>P04. Дата +1 день, CheckOperationDate=false → пара.</summary>
    private static TransferReceiveTestCase P04_CheckOperationDateOff_OneDayDiffPaired() => new()
    {
        Name = "P04. CheckOperationDate=false — дата +1 день сходится.",
        Params = new TransferReceiveFormParams(CheckOperationDate: false),
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, opDate: "2024-06-15")],
        CounterpartOps = [RowReceive(101, opDate: "2024-06-16")],
        ExpectedUnpairedIds = []
    };

    /// <summary>P05. Активность далеко, CheckActivity=false → пара.</summary>
    private static TransferReceiveTestCase P05_CheckActivityOff_ActivityMismatchPaired() => new()
    {
        Name = "P05. CheckActivity=false — сильное расхождение активности сходится.",
        Params = new TransferReceiveFormParams(CheckActivity: false),
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, activity: "1.0e+6")],
        CounterpartOps = [RowReceive(101, activity: "9.0e+6")],
        ExpectedUnpairedIds = []
    };

    /// <summary>P06. Candidate не указывает на нас, CheckProviderOrRecieverOkpo=false → пара.</summary>
    private static TransferReceiveTestCase P06_CheckProviderOkpoOff_WrongPointerPaired() => new()
    {
        Name = "P06. CheckProviderOrRecieverOkpo=false — чужой указатель в кол.19 сходится.",
        Params = new TransferReceiveFormParams(CheckProviderOrRecieverOkpo: false),
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1)],
        CounterpartOps = [RowReceive(101, providerOkpo: "99999999")],
        ExpectedUnpairedIds = []
    };
}
