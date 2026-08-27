using System;
using System.Globalization;
using Models.Forms.Form1;

namespace Client_App.Services.AutoReplace.Policies;

internal static class Form12Policy
{
    public static void ApplyOperationCode(Form12 form)
    {
        var opCode = form.OperationCode_DB ?? string.Empty;
        const string dash = "-";
        var masterOkpo = form.Report?.Reports?.Master_DB?.OkpoRep.Value ?? string.Empty;
        switch (opCode)
        {
            case "10" or "12" or "42" or "97" or "98" or "99":
            {
                #region ProviderOrRecieverOKPO (16) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (17) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }
            case "11":
            {
                #region CreatorOKPO (8) — код ОКПО изготовителя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.CreatorOKPO_DB != masterOkpo)
                {
                    form.CreatorOKPO.Value = masterOkpo;
                }

                #endregion

                #region CreationDate (9) — дата выпуска

                if (DateOnly.TryParse(form.OperationDate_DB, new CultureInfo("ru-RU", false), out var operationDate)
                    && form.CreationDate_DB != operationDate.ToShortDateString())
                {
                    form.CreationDate.Value = operationDate.ToShortDateString();
                }

                #endregion

                #region DocumentVid (13) — документ, вид

                if (form.DocumentVid_DB != 9)
                {
                    form.DocumentVid.Value = 9;
                }

                #endregion

                #region DocumentNumber (14) — документ, номер

                if (form.DocumentNumber_DB != form.PassportNumber_DB)
                {
                    form.DocumentNumber.Value = form.PassportNumber_DB;
                }

                #endregion

                #region DocumentDate (15) — документ, дата

                if (DateOnly.TryParse(form.OperationDate_DB, new CultureInfo("ru-RU", false), out operationDate)
                    && form.DocumentDate_DB != operationDate.ToShortDateString())
                {
                    form.DocumentDate.Value = operationDate.ToShortDateString();
                }

                #endregion

                #region ProviderOrRecieverOKPO (16) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (17) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }
            case "17" or "18" or "43" or "46" or "53" or "58" or "65" or "67" or "68" or "71" or "72" or "73" or "74" or "75":
            {
                #region DocumentDate (15) — документ, дата

                if (DateOnly.TryParse(form.OperationDate_DB, new CultureInfo("ru-RU", false), out var operationDate)
                    && form.DocumentDate_DB != operationDate.ToShortDateString())
                {
                    form.DocumentDate.Value = operationDate.ToShortDateString();
                }

                #endregion

                #region ProviderOrRecieverOKPO (16) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (17) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }
            case "21" or "25" or "27" or "28" or "29" or "31" or "35" or "37" or "38" or "39" or "81" or "82" or "83" or "84" or "85" or "86" or "87" or "88":
            {
                #region ProviderOrRecieverOKPO (16) — код ОКПО поставщика или получателя

                if (form.ProviderOrRecieverOKPO_DB is not "")
                {
                    form.ProviderOrRecieverOKPO.Value = string.Empty;
                }

                #endregion

                #region TransporterOKPO (17) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB is not "")
                {
                    form.TransporterOKPO.Value = string.Empty;
                }

                #endregion

                break;
            }
            case "22" or "32":
            {
                #region ProviderOrRecieverOKPO (16) — код ОКПО поставщика или получателя

                if (form.ProviderOrRecieverOKPO_DB != "Минобороны")
                {
                    form.ProviderOrRecieverOKPO.Value = "Минобороны";
                }

                #endregion

                break;
            }
            case "41":
            {
                #region DocumentVid (13) — документ, вид

                if (form.DocumentVid_DB != 1)
                {
                    form.DocumentVid.Value = 1;
                }

                #endregion

                #region DocumentDate (15) — документ, дата

                if (DateOnly.TryParse(form.OperationDate_DB, new CultureInfo("ru-RU", false), out var operationDate)
                    && form.DocumentDate_DB != operationDate.ToShortDateString())
                {
                    form.DocumentDate.Value = operationDate.ToShortDateString();
                }

                #endregion

                #region ProviderOrRecieverOKPO (16) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                #region TransporterOKPO (17) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }
            case "54" or "66":
            {
                #region DocumentDate (15) — документ, дата

                if (DateOnly.TryParse(form.OperationDate_DB, new CultureInfo("ru-RU", false), out var operationDate)
                    && form.DocumentDate_DB != operationDate.ToShortDateString())
                {
                    form.DocumentDate.Value = operationDate.ToShortDateString();
                }

                #endregion

                #region TransporterOKPO (17) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }
            case "61" or "62":
            {
                #region ProviderOrRecieverOKPO (16) — код ОКПО поставщика или получателя

                if (!string.IsNullOrWhiteSpace(masterOkpo) && form.ProviderOrRecieverOKPO_DB != masterOkpo)
                {
                    form.ProviderOrRecieverOKPO.Value = masterOkpo;
                }

                #endregion

                break;
            }
            case "63" or "64":
            {
                #region TransporterOKPO (17) — код ОКПО перевозчика

                if (form.TransporterOKPO_DB != dash)
                {
                    form.TransporterOKPO.Value = dash;
                }

                #endregion

                break;
            }
        }
    }

    public static void ApplyOperationDate(Form12 form)
    {
        if (!DateOnly.TryParse(form.OperationDate_DB, new CultureInfo("ru-RU", false), out var opDate))
        {
            return;
        }

        switch (form.OperationCode_DB)
        {
            case "11":
            {
                #region CreationDate (9) — дата выпуска

                form.CreationDate.Value = opDate.ToShortDateString();

                #endregion

                #region DocumentDate (15) — документ, дата

                form.DocumentDate.Value = opDate.ToShortDateString();

                #endregion

                break;
            }
            case "17" or "18" or "41" or "43" or "46" or "53" or "54" or "58" or "65" or "66" or "67" or "68" or "71" or "72" or "73" or "74" or "75":
            {
                #region DocumentDate (15) — документ, дата

                form.DocumentDate.Value = opDate.ToShortDateString();

                #endregion

                break;
            }
        }
    }

    public static void ApplyPassportNumber(Form12 form)
    {
        if (form.OperationCode_DB is not "11")
        {
            return;
        }

        #region DocumentNumber (14) — документ, номер

        if (form.DocumentNumber_DB != form.PassportNumber_DB)
        {
            form.DocumentNumber.Value = form.PassportNumber_DB;
        }

        #endregion
    }
}
