using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Collections;
using Models;

namespace Models.DBRealization.DBAPIFactory
{
    public static partial class EssanceMethods
    {
        private static Dictionary<string,Func<IEssenceMethods>> MethodsList { get; set; } = new Dictionary<string, Func<IEssenceMethods>>() 
        {
            {nameof(Report), ReportEssenceMethods.GetMethods}
        };
    }
}
