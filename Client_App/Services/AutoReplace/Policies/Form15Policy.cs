using Models.Forms.Form1;

namespace Client_App.Services.AutoReplace.Policies;

internal static class Form15Policy
{
    public static void ApplyOperationCode(Form15 form)
    {
        var opCode = form.OperationCode_DB ?? string.Empty;
        const string dash = "-";
        var masterOkpo = form.Report?.Reports?.Master_DB?.OkpoRep.Value ?? string.Empty;
        switch (opCode)
        {
            case "10" or "18" or "43" or "45" or "51" or "52" or "57" or "63" or "64" or "68" or "71" or "72" or "74" or "75" or "76" or "97" or "98":
            {
                #region ProviderOrRecieverOKPO (15) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (16) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                #region RefineOrSortRAOCode (22) — код переработки/сортировки РАО

                if (form.RefineOrSortRAOCode_DB != dash)
                {
                    form.RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }
            case "11" or "12" or "14" or "41" or "42" or "73":
            {
                #region StatusRAO (11) — статус РАО

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.StatusRAO_DB != masterOkpo)
                {
                    form.StatusRAO.Value = masterOkpo;
                }

                #endregion

                #region ProviderOrRecieverOKPO (15) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (16) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                #region RefineOrSortRAOCode (22) — код переработки/сортировки РАО

                if (form.RefineOrSortRAOCode_DB != dash)
                {
                    form.RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }
            case "21" or "25" or "26" or "27" or "28" or "29" or "31" or "35" or "36" or "37" or "38" or "39":
            {
                #region ProviderOrRecieverOKPO (15) — код ОКПО поставщика или получателя

                if (form.ProviderOrRecieverOKPO_DB is not "")
                {
                    form.ProviderOrRecieverOKPO.Value = string.Empty;
                }

                #endregion

                #region TransporterOKPO (16) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB is not "")
                {
                    form.TransporterOKPO.Value = string.Empty;
                }

                #endregion

                #region RefineOrSortRAOCode (22) — код переработки/сортировки РАО

                if (form.RefineOrSortRAOCode_DB != dash)
                {
                    form.RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }
            case "84" or "88":
            {
                #region RefineOrSortRAOCode (22) — код переработки/сортировки РАО

                if (form.RefineOrSortRAOCode_DB != dash)
                {
                    form.RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }
            case "22" or "32":
            {
                #region ProviderOrRecieverOKPO (15) — код ОКПО поставщика или получателя

                if (form.ProviderOrRecieverOKPO_DB != "Минобороны")
                {
                    form.ProviderOrRecieverOKPO.Value = "Минобороны";
                }

                #endregion

                #region RefineOrSortRAOCode (22) — код переработки/сортировки РАО

                if (form.RefineOrSortRAOCode_DB != dash)
                {
                    form.RefineOrSortRAOCode.Value = dash;
                }

                #endregion

                break;
            }
            case "44":
            {
                #region ProviderOrRecieverOKPO (15) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (16) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                #region RefineOrSortRAOCode (22) — код переработки/сортировки РАО

                if (form.RefineOrSortRAOCode_DB is not "")
                {
                    form.RefineOrSortRAOCode.Value = string.Empty;
                }

                #endregion

                break;
            }
            case "49" or "55" or "56" or "59" or "99":
            {
                #region ProviderOrRecieverOKPO (15) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (16) — код ОКПО перевозчика

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
