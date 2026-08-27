using Models.Forms.Form1;

namespace Client_App.Services.AutoReplace.Policies;

internal static class Form17Policy
{
    public static void ApplyOperationCode(Form17 form)
    {
        var opCode = form.OperationCode_DB ?? string.Empty;
        const string dash = "-";
        var masterOkpo = form.Report?.Reports?.Master_DB?.OkpoRep.Value ?? string.Empty;
        switch (opCode)
        {
            case "10" or "18" or "43" or "51" or "52" or "68" or "97" or "98":
            {
                #region ProviderOrRecieverOKPO (17) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (18) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                #region RefineOrSortRAOCode (30) — код переработки/сортировки РАО

                if (form.RefineOrSortRAOCode_DB != dash)
                {
                    form.RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }
            case "11" or "13" or "16":
            {
                #region ProviderOrRecieverOKPO (17) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (18) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                #region StatusRAO (22) — статус РАО

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.StatusRAO_DB != masterOkpo)
                {
                    form.StatusRAO.Value = masterOkpo;
                }

                #endregion

                #region RefineOrSortRAOCode (30) — код переработки/сортировки РАО

                if (form.RefineOrSortRAOCode_DB != dash)
                {
                    form.RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }
            case "21" or "26" or "27":
            {
                #region ProviderOrRecieverOKPO (17) — код ОКПО поставщика или получателя

                if (form.ProviderOrRecieverOKPO_DB is not "")
                {
                    form.ProviderOrRecieverOKPO.Value = string.Empty;
                }

                #endregion

                #region TransporterOKPO (18) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB is not "")
                {
                    form.TransporterOKPO.Value = string.Empty;
                }

                #endregion

                #region RefineOrSortRAOCode (30) — код переработки/сортировки РАО

                if (form.RefineOrSortRAOCode_DB != dash)
                {
                    form.RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }
            case "22" or "25" or "28" or "29" or "31" or "32" or "35" or "36" or "37" or "38" or "39":
            {
                #region RefineOrSortRAOCode (30) — код переработки/сортировки РАО

                if (form.RefineOrSortRAOCode_DB != dash)
                {
                    form.RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }
            case "55":
            {
                #region ProviderOrRecieverOKPO (17) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (18) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }
        }
    }
}
