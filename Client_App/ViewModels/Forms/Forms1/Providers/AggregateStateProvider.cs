using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Client_App.ViewModels.Forms.Forms1.Items;

namespace Client_App.ViewModels.Forms.Forms1.Providers;

public static class AggregateStateProvider
{
    /// <summary>
    /// 
    /// </summary>
    public static ReadOnlyCollection<AggregateStateItem> AllAggregateStates { get; }

    static AggregateStateProvider()
    {
        AllAggregateStates = new ReadOnlyCollection<AggregateStateItem>(
        [
            new AggregateStateItem { Code = 1, Description = "жидкие" },
            new AggregateStateItem { Code = 2, Description = "твёрдые" },
            new AggregateStateItem { Code = 3, Description = "газообразные" }
        ]);
    }

    /// <summary>
    /// 
    /// </summary>
    public static IEnumerable<string> GetValidCodes()
    {
        return ["1", "2", "3"];
    }
}
