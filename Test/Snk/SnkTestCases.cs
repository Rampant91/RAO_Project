using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Наборы тестовых кейсов: входные операции, ожидаемый СНК на EndDate
/// и ожидаемый СНК по датам инвентаризации.
/// </summary>
internal static class SnkTestCases
{
    public static IEnumerable<object[]> All()
    {
        yield return
        [
            NotInFirstInventory_ReceiveRechargeInventorySameDay().Name,
            NotInFirstInventory_ReceiveRechargeInventorySameDay()
        ];

        yield return
        [
            InFirstInventory_RechargeInventorySameDay().Name,
            InFirstInventory_RechargeInventorySameDay()
        ];

        yield return
        [
            NotInFirstInventory_ReceiveInventoryRechargeTransferSameDay().Name,
            NotInFirstInventory_ReceiveInventoryRechargeTransferSameDay()
        ];

        yield return
        [
            InFirstInventory_ReceiveInventoryRechargeTransferSameDay().Name,
            InFirstInventory_ReceiveInventoryRechargeTransferSameDay()
        ];
    }

    /// <summary>
    /// Поступление, затем перезарядка и инвентаризация в один день.
    /// Первая дата инвентаризации была раньше, чем поступление.
    /// </summary>
    private static SnkTestCase NotInFirstInventory_ReceiveRechargeInventorySameDay() => new()
    {
        Name = "1. Поступление, затем перезарядка и инвентаризация в один день. Нет в первой инвентаризации.",
        FormNum = "1.1",
        EndDate = DateOnly.FromDateTime(DateTime.Today),
        Operations =
        [
            Operation(
                opCode: "10",
                opDate: new DateOnly(2022, 1, 19),
                pasNum: "999",
                facNum: "001",
                type: "Тип-A",
                radionuclids: "кобальт-60",
                packNumber: "1",
                quantity: 1),
            Operation(
                opCode: "38",
                opDate: new DateOnly(2023, 11, 10),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52",
                quantity: 1),
            Operation(
                opCode: "53",
                opDate: new DateOnly(2023, 11, 29),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1),
            Operation(
                opCode: "10",
                opDate: new DateOnly(2023, 11, 29),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1),
        ],
        ExpectedSnkStock =
        [
            Stock(
                pasNum: "999",
                facNum: "001",
                type: "Тип-A",
                radionuclids: "кобальт-60",
                packNumber: "1",
                quantity: 1),
            Stock(
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1),
        ],
        ExpectedInventoryStockByDate = new Dictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>
        {
            [new DateOnly(2022, 1, 19)] =
            [
                Stock(
                    pasNum: "999",
                    facNum: "001",
                    type: "Тип-A",
                    radionuclids: "кобальт-60",
                    packNumber: "1",
                    quantity: 1),
            ],
            [new DateOnly(2023, 11, 29)] =
            [
                Stock(
                    pasNum: "999",
                    facNum: "001",
                    type: "Тип-A",
                    radionuclids: "кобальт-60",
                    packNumber: "1",
                    quantity: 1),
                Stock(
                    pasNum: "510",
                    facNum: "083",
                    type: "ГИК-5-3",
                    radionuclids: "кобальт-60",
                    packNumber: "52-1",
                    quantity: 1),
            ],
            [DateOnly.FromDateTime(DateTime.Today)] =
            [
                Stock(
                    pasNum: "999",
                    facNum: "001",
                    type: "Тип-A",
                    radionuclids: "кобальт-60",
                    packNumber: "1",
                    quantity: 1),
                Stock(
                    pasNum: "510",
                    facNum: "083",
                    type: "ГИК-5-3",
                    radionuclids: "кобальт-60",
                    packNumber: "52-1",
                    quantity: 1),
            ],
        }
    };

    /// <summary>
    /// Инвентаризация, затем перезарядка и инвентаризация в один день.
    /// Первая инвентаризация в первую дату инвентаризации у организации.
    /// </summary>
    private static SnkTestCase InFirstInventory_RechargeInventorySameDay() => new()
    {
        Name = "2. В первой инвентаризации, затем перезарядка и инвентаризация в один день.",
        FormNum = "1.1",
        EndDate = DateOnly.FromDateTime(DateTime.Today),
        Operations =
        [
            Operation(
                opCode: "10",
                opDate: new DateOnly(2022, 1, 19),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52",
                quantity: 1),
            Operation(
                opCode: "53",
                opDate: new DateOnly(2023, 11, 29),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1),
            Operation(
                opCode: "10",
                opDate: new DateOnly(2023, 11, 29),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1),
        ],
        ExpectedSnkStock =
        [
            Stock(
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1),
        ],
        ExpectedInventoryStockByDate = new Dictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>
        {
            [new DateOnly(2022, 1, 19)] =
            [
                Stock(
                    pasNum: "510",
                    facNum: "083",
                    type: "ГИК-5-3",
                    radionuclids: "кобальт-60",
                    packNumber: "52",
                    quantity: 1),
            ],
            [new DateOnly(2023, 11, 29)] =
            [
                Stock(
                    pasNum: "510",
                    facNum: "083",
                    type: "ГИК-5-3",
                    radionuclids: "кобальт-60",
                    packNumber: "52-1",
                    quantity: 1),
            ],
            [DateOnly.FromDateTime(DateTime.Today)] =
            [
                Stock(
                    pasNum: "510",
                    facNum: "083",
                    type: "ГИК-5-3",
                    radionuclids: "кобальт-60",
                    packNumber: "52-1",
                    quantity: 1),
            ],
        }
    };

    /// <summary>
    /// Поступление, затем перезарядка и инвентаризация в один день.
    /// Первая дата инвентаризации была раньше, чем поступление.
    /// </summary>
    private static SnkTestCase NotInFirstInventory_ReceiveInventoryRechargeTransferSameDay() => new()
    {
        Name = "3. Поступление, инвентаризация, перезарядка и передача в один день. Нет в первой инвентаризации.",
        FormNum = "1.1",
        EndDate = DateOnly.FromDateTime(DateTime.Today),
        Operations =
        [
            Operation(
                opCode: "10",
                opDate: new DateOnly(2022, 1, 19),
                pasNum: "999",
                facNum: "001",
                type: "Тип-A",
                radionuclids: "кобальт-60",
                packNumber: "1",
                quantity: 1),
            Operation(
                opCode: "38",
                opDate: new DateOnly(2023, 11, 29),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52",
                quantity: 1),
            Operation(
                opCode: "53",
                opDate: new DateOnly(2023, 11, 29),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1),
            Operation(
                opCode: "10",
                opDate: new DateOnly(2023, 11, 29),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1),
            Operation(
                opCode: "28",
                opDate: new DateOnly(2024, 1, 29),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1)
        ],
        ExpectedSnkStock =
        [
            Stock(
                pasNum: "999",
                facNum: "001",
                type: "Тип-A",
                radionuclids: "кобальт-60",
                packNumber: "1",
                quantity: 1)
        ],
        ExpectedInventoryStockByDate = new Dictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>
        {
            [new DateOnly(2022, 1, 19)] =
            [
                Stock(
                    pasNum: "999",
                    facNum: "001",
                    type: "Тип-A",
                    radionuclids: "кобальт-60",
                    packNumber: "1",
                    quantity: 1),
            ],
            [new DateOnly(2023, 11, 29)] =
            [
                Stock(
                    pasNum: "999",
                    facNum: "001",
                    type: "Тип-A",
                    radionuclids: "кобальт-60",
                    packNumber: "1",
                    quantity: 1)
            ],
            [DateOnly.FromDateTime(DateTime.Today)] =
            [
                Stock(
                    pasNum: "999",
                    facNum: "001",
                    type: "Тип-A",
                    radionuclids: "кобальт-60",
                    packNumber: "1",
                    quantity: 1),
            ],
        }
    };

    /// <summary>
    /// Поступление, затем перезарядка и инвентаризация в один день.
    /// Первая дата инвентаризации была раньше, чем поступление.
    /// </summary>
    private static SnkTestCase InFirstInventory_ReceiveInventoryRechargeTransferSameDay() => new()
    {
        Name = "4. Поступление, инвентаризация, перезарядка и передача в один день первой инвентаризации.",
        FormNum = "1.1",
        EndDate = DateOnly.FromDateTime(DateTime.Today),
        Operations =
        [
            Operation(
                opCode: "10",
                opDate: new DateOnly(2022, 1, 19),
                pasNum: "999",
                facNum: "001",
                type: "Тип-A",
                radionuclids: "кобальт-60",
                packNumber: "1",
                quantity: 1),
            Operation(
                opCode: "38",
                opDate: new DateOnly(2022, 1, 19),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52",
                quantity: 1),
            Operation(
                opCode: "54",
                opDate: new DateOnly(2022, 1, 19),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1),
            Operation(
                opCode: "10",
                opDate: new DateOnly(2022, 1, 19),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1),
            Operation(
                opCode: "10",
                opDate: new DateOnly(2023, 1, 1),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1),
            Operation(
                opCode: "28",
                opDate: new DateOnly(2023, 11, 29),
                pasNum: "510",
                facNum: "083",
                type: "ГИК-5-3",
                radionuclids: "кобальт-60",
                packNumber: "52-1",
                quantity: 1)
        ],
        ExpectedSnkStock =
        [
            Stock(
                pasNum: "999",
                facNum: "001",
                type: "Тип-A",
                radionuclids: "кобальт-60",
                packNumber: "1",
                quantity: 1)
        ],
        ExpectedInventoryStockByDate = new Dictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>
        {
            [new DateOnly(2022, 1, 19)] =
            [
                Stock(
                    pasNum: "999",
                    facNum: "001",
                    type: "Тип-A",
                    radionuclids: "кобальт-60",
                    packNumber: "1",
                    quantity: 1),
                Stock(
                    pasNum: "510",
                    facNum: "083",
                    type: "ГИК-5-3",
                    radionuclids: "кобальт-60",
                    packNumber: "52-1",
                    quantity: 1),
            ],
            [new DateOnly(2023, 1, 1)] =
            [
                Stock(
                    pasNum: "999",
                    facNum: "001",
                    type: "Тип-A",
                    radionuclids: "кобальт-60",
                    packNumber: "1",
                    quantity: 1),
                Stock(
                    pasNum: "510",
                    facNum: "083",
                    type: "ГИК-5-3",
                    radionuclids: "кобальт-60",
                    packNumber: "52-1",
                    quantity: 1),
            ],
            [DateOnly.FromDateTime(DateTime.Today)] =
            [
                Stock(
                    pasNum: "999",
                    facNum: "001",
                    type: "Тип-A",
                    radionuclids: "кобальт-60",
                    packNumber: "1",
                    quantity: 1),
            ],
        }
    };

    private static SnkTestOperationSpec Operation(
        string opCode,
        DateOnly opDate,
        string pasNum,
        string facNum,
        string type,
        string radionuclids,
        string packNumber,
        int quantity = 1) =>
        new (
            OpCode: opCode,
            OpDate: opDate,
            PasNum: pasNum,
            FacNum: facNum,
            Type: type,
            Radionuclids: radionuclids,
            PackNumber: packNumber,
            Quantity: quantity);

    /// <summary>Ожидаемая строка СНК: только ключевые поля (форма 1.1).</summary>
    private static SnkStockSnapshot Stock(
        string pasNum,
        string facNum,
        string type,
        string radionuclids,
        string packNumber,
        int quantity = 1) =>
        new (
            PasNum: pasNum,
            FacNum: facNum,
            Type: type,
            Radionuclids: radionuclids,
            PackNumber: packNumber,
            OpCode: "",
            OpDate: default,
            Quantity: quantity);
}
