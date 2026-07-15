using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Client_App.ViewModels.ProgressBar;



namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;



/// <summary>

/// Внутренний доступ к private protected методам СНК для unit-тестов.

/// </summary>

internal sealed class ExcelExportSnkTestHarness : ExcelExportSnkBaseAsyncCommand

{

    public static ExcelExportSnkTestHarness Instance { get; } = new();



    private ExcelExportSnkTestHarness()

    {

    }



    public override Task AsyncExecute(object? parameter) => Task.CompletedTask;



    public override bool CanExecute(object? parameter) => false;



    /// <summary>

    /// СНК на дату, как в ExcelExportSnkAsyncCommand (union только с первой инвентаризации).

    /// </summary>

    public async Task<IReadOnlyList<SnkStockSnapshot>> RunSnkExportAsync(SnkTestCase testCase, DateOnly asOfDate)

    {

        var operations = testCase.Operations

            .Where(x => x.OpDate <= asOfDate)

            .ToList();



        var (inventoryList, plusMinusList, rechargeList, _) =

            SnkTestOperationSplitter.Split(testCase.FormNum, operations);



        var firstInventoryDate = inventoryList.Count == 0

            ? DateOnly.MinValue

            : inventoryList.Min(x => x.OpDate);



        var dictionary = await GetDictionary_UniqueUnitsWithOperations(

            testCase.FormNum,

            inventoryList,

            plusMinusList,

            rechargeList,

            zeroFormsDtoList: null);



        var unitsInStock = await GetUnitInStockDtoList(

            dictionary,

            testCase.FormNum,

            firstInventoryDate,

            new AnyTaskProgressBarVM());



        return unitsInStock.Select(SnkTestOperationSplitter.ToSnapshot).ToList();

    }



    public async Task<SnkScenarioResult> RunScenarioAsync(SnkTestScenario scenario)

    {

        var (inventoryList, plusMinusList, rechargeList, zeroSplit) =

            SnkTestOperationSplitter.Split(scenario.FormNum, scenario.Operations);



        var firstInventoryDate = inventoryList.Count == 0

            ? DateOnly.MinValue

            : inventoryList.Min(x => x.OpDate);



        List<ShortFormDTO>? zeroList = scenario.UnionMode == SnkInventoryUnionMode.CheckInventories

            ? zeroSplit

            : null;



        var dictionary = await GetDictionary_UniqueUnitsWithOperations(

            scenario.FormNum,

            inventoryList,

            plusMinusList,

            rechargeList,

            zeroList);



        var unitsInStock = await GetUnitInStockDtoList(

            dictionary,

            scenario.FormNum,

            firstInventoryDate,

            new AnyTaskProgressBarVM());



        return new SnkScenarioResult

        {

            FirstInventoryDate = firstInventoryDate,

            GroupedUnits = dictionary

                .Select(pair => new SnkUnitSnapshot(

                    pair.Key.PasNum,

                    pair.Key.FacNum,

                    pair.Key.Type,

                    pair.Key.Radionuclids,

                    pair.Key.PackNumber,

                    pair.Value.Select(x => x.OpCode).ToList()))

                .ToList(),

            UnitsInStock = unitsInStock.Select(SnkTestOperationSplitter.ToSnapshot).ToList()

        };

    }



    public static bool IsInventoryOperation(string opCode) => opCode is "10";



    public static bool IsRechargeOperation(string opCode) => opCode is "53" or "54";



    public static bool IsPlusMinusOperation(string formNum, string opCode) =>

        GetPlusOperationsArray(formNum).Contains(opCode)

        || GetMinusOperationsArray(formNum).Contains(opCode);

}


