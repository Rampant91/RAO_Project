using System;
using System.Collections.Generic;
using static Client_App.Commands.AsyncCommands.ExcelExport.Snk.ExcelExportSnkBaseAsyncCommand;

namespace Client_App.Resources.CustomComparers.SnkComparers;

public class SnkParamsComparer : IEqualityComparer<ShortFormDTO>
{
    public bool Equals(ShortFormDTO? x, ShortFormDTO? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;

        var numberComparer = new SnkNumberEqualityComparer();
        var radsComparer = new SnkRadionuclidsEqualityComparer();
        var stringComparer = new SnkEqualityComparer();

        return numberComparer.Equals(x.PasNum, y.PasNum)
               && stringComparer.Equals(x.Type, y.Type)
               && numberComparer.Equals(x.FacNum, y.FacNum)
               && radsComparer.Equals(x.Radionuclids, y.Radionuclids)
               && stringComparer.Equals(x.PackNumber, y.PackNumber);
    }

    public int GetHashCode(ShortFormDTO obj) => 0;
}