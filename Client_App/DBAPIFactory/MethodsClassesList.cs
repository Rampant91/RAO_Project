using System;
using System.Collections.Generic;
using Models.Collections;

namespace Client_App.DBAPIFactory
{
    public static partial class EssenceMethods
    {
        private static Dictionary<string, Func<IEssenceMethods>> MethodsList { get; set; } = new Dictionary<string, Func<IEssenceMethods>>()
        {
            {nameof(Report), ReportEssenceMethods.GetMethods},
            {nameof(Reports), EssenceMethods.ReportsEssenceMethods.GetMethods}
        };
    }
}
