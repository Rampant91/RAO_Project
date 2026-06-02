using Models.Forms.Form1;

namespace Client_App.Services.AutoReplace.Policies;

internal static class Form18Policy
{
    public static void ApplyOperationCode(Form18 form)
    {
        var opCode = form.OperationCode_DB ?? string.Empty;
        const string dash = "-";
        var masterOkpo = form.Report?.Reports?.Master_DB?.OkpoRep.Value ?? string.Empty;
        switch (opCode)
        {
            case "10" or "18" or "43" or "51" or "52" or "68" or "97" or "98":
                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo) form.ProviderOrRecieverOKPO.Value = masterOkpo;
                if (form.TransporterOKPO_DB != dash) form.TransporterOKPO.Value = dash;
                if (form.RefineOrSortRAOCode_DB != dash) form.RefineOrSortRAOCode.Value = dash;
                break;
            case "11" or "13" or "16":
                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo) form.ProviderOrRecieverOKPO.Value = masterOkpo;
                if (form.TransporterOKPO_DB != dash) form.TransporterOKPO.Value = dash;
                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.StatusRAO_DB != masterOkpo) form.StatusRAO.Value = masterOkpo;
                if (form.RefineOrSortRAOCode_DB != dash) form.RefineOrSortRAOCode.Value = dash;
                break;
            case "21" or "22" or "25" or "26" or "27" or "28" or "29" or "31" or "32" or "35" or "36" or "37" or "38" or "39":
                if (form.RefineOrSortRAOCode_DB != dash) form.RefineOrSortRAOCode.Value = dash;
                break;
            case "55":
                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo) form.ProviderOrRecieverOKPO.Value = masterOkpo;
                if (form.TransporterOKPO_DB != dash) form.TransporterOKPO.Value = dash;
                break;
        }
    }
}
