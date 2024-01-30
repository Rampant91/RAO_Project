using Models.CheckForm;
using Models.Forms.Form1;
using System.Collections.Generic;

internal static class CheckF11Helpers
{

    private static IEnumerable<CheckError> Check_002(IReadOnlyList<Form11> forms, int line)
    {
        List<CheckError> result = new();
        if (forms[line].Id < 1)
        {
            result.Add(new CheckError
            {
                param1 = "form_11",
                param2 = (line + 1).ToString(),
                param3 = "Id",
                param4 = forms[line].Id.ToString(),
                Message = "-"
            });
        }
        return result;
    }
}