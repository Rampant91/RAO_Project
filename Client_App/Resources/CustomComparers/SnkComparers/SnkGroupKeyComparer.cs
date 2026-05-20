using System;
using System.Collections.Generic;

namespace Client_App.Resources.CustomComparers.SnkComparers;

/// <summary>
/// Сравнение учётной единицы по номеру паспорта, заводскому номеру, радионуклидам и типу.
/// </summary>
public sealed class SnkGroupKeyComparer : IEqualityComparer<(string PasNum, string FacNum, string Radionuclids, string Type)>
{
    private readonly SnkNumberEqualityComparer _numberComparer = new();
    private readonly SnkRadionuclidsEqualityComparer _radsComparer = new();

    public bool Equals((string PasNum, string FacNum, string Radionuclids, string Type) x,
        (string PasNum, string FacNum, string Radionuclids, string Type) y)
    {
        return _numberComparer.Equals(x.PasNum, y.PasNum)
               && _numberComparer.Equals(x.FacNum, y.FacNum)
               && _radsComparer.Equals(x.Radionuclids, y.Radionuclids)
               && _numberComparer.Equals(x.Type, y.Type);
    }

    public int GetHashCode((string PasNum, string FacNum, string Radionuclids, string Type) obj)
    {
        return HashCode.Combine(
            _numberComparer.GetHashCode(obj.PasNum),
            _numberComparer.GetHashCode(obj.FacNum),
            _radsComparer.GetHashCode(obj.Radionuclids),
            _numberComparer.GetHashCode(obj.Type));
    }
}
