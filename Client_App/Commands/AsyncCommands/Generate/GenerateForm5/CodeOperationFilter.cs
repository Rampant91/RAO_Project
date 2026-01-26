using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Generate.GenerateForm5
{
    public static class CodeOperationFilter
    {
        public static List<string> AllOperationCodes
        {
            get
            {
                var result = new List<string>();
                result.AddRange(PlusOperationCodes);
                result.AddRange(MinusOperationCodes);
                result.AddRange(OtherOperationCodes);
                return result;
            }
        }
        public static List<string> PlusOperationCodes
        {
            get
            {
                return new List<string>()
                {
                    "31","32","33","34","35","36","37","38","39",
                    "64",
                    "74", "76",
                    "85","86","87","88"
                };
            }
        }
        public static List<string> MinusOperationCodes
        {
            get
            {
                return new List<string>()
                {
                    "21","22","23","24","25","26","27","28","29",
                    "63",
                    "81","82","83","84"
                };
            }
        }

        public static List<string> OtherOperationCodes
        {
            get
            {
                return new List<string>()
                {
                    "41"
                };
            }
        }

    }
}
