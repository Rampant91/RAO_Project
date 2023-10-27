using System;
using Models.Collections;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.JSON;
using Models.JSON.TableDataMain;

namespace Client_App.Commands.AsyncCommands.Import.ImportJson;

public static class ImportJsonMethods
{
    #region BindData

    public static void BindData(JsonForm rep, Report impRep)
    {
        var numberInOrder = 1;
        switch (rep.FormNum)
        {
            #region Form1

            #region Form11

            case "1.1":
            {
                var repForm = (JsonForm11)rep;
                foreach (var tableDataMain in  repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain11)tableDataMain;
                    impRep.Rows11.Add(new Form11
                    {
                        NumberInOrder_DB = numberInOrder++,
                        OperationCode_DB = Convert.ToString(form.OperationCode),
                        OperationDate_DB = DateTime.TryParse(form.OperationDate, out var dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : Convert.ToString(form.OperationDate),
                        PassportNumber_DB = Convert.ToString(form.PassportNumber),
                        Type_DB = Convert.ToString(form.Type),
                        Radionuclids_DB = Convert.ToString(form.Radionuclids),
                        FactoryNumber_DB = Convert.ToString(form.FactoryNumber),
                        Quantity_DB = int.TryParse(form.Quantity, out var intValue)
                            ? intValue
                            : null,
                        Activity_DB = Convert.ToString(form.Activity),
                        CreationDate_DB = DateTime.TryParse(form.CreationDate, out dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : Convert.ToString(form.CreationDate),
                        CreatorOKPO_DB = Convert.ToString(form.CreatorOKPO),
                        Category_DB = short.TryParse(form.Category, out var shortValue)
                            ? shortValue
                            : null,
                        SignedServicePeriod_DB = float.TryParse(form.SignedServicePeriod, out var floatValue)
                            ? floatValue
                            : null,
                        PropertyCode_DB = byte.TryParse(form.PropertyCode, out var byteValue)
                            ? byteValue
                            : null,
                        Owner_DB = Convert.ToString(form.Owner),
                        DocumentVid_DB = byte.TryParse(form.DocumentVid, out byteValue)
                            ? byteValue
                            : null,
                        DocumentNumber_DB = Convert.ToString(form.DocumentNumber),
                        DocumentDate_DB = DateTime.TryParse(form.DocumentDate, out dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : Convert.ToString(form.DocumentDate),
                        ProviderOrRecieverOKPO_DB = Convert.ToString(form.ProviderOrRecieverOKPO),
                        TransporterOKPO_DB = Convert.ToString(form.TransporterOKPO),
                        PackName_DB = Convert.ToString(form.PackName),
                        PackType_DB = Convert.ToString(form.PackType),
                        PackNumber_DB = Convert.ToString(form.PackNumber)
                    });
                }
                break;
            }

            #endregion

            #region Form12

            case "1.2":
            {
                var repForm = (JsonForm12)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain12)tableDataMain;
                    impRep.Rows12.Add(new Form12
                    {
                        NumberInOrder_DB = numberInOrder++,
                        OperationCode_DB = form.OperationCode,
                        OperationDate_DB = DateTime.TryParse(form.OperationDate, out var dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : form.OperationDate,
                        PassportNumber_DB = form.PassportNumber,
                        NameIOU_DB = form.NameIOU,
                        FactoryNumber_DB = form.FactoryNumber,
                        Mass_DB = form.Mass,
                        CreatorOKPO_DB = form.CreatorOKPO,
                        CreationDate_DB = DateTime.TryParse(form.CreationDate, out dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : form.CreationDate,
                        SignedServicePeriod_DB = form.SignedServicePeriod,
                        PropertyCode_DB = byte.TryParse(form.PropertyCode, out var byteValue)
                            ? byteValue
                            : null,
                        Owner_DB = form.Owner,
                        DocumentVid_DB = byte.TryParse(form.DocumentVid, out byteValue)
                            ? byteValue
                            : null,
                        DocumentNumber_DB = form.DocumentNumber,
                        DocumentDate_DB = DateTime.TryParse(form.DocumentDate, out dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : form.DocumentDate,
                        ProviderOrRecieverOKPO_DB = form.ProviderOrRecieverOKPO,
                        TransporterOKPO_DB = form.TransporterOKPO,
                        PackName_DB = form.PackName,
                        PackType_DB = form.PackType,
                        PackNumber_DB = form.PackNumber
                    });
                }
                break;
            }

            #endregion

            #region Form13

            case "1.3":
            {
                var repForm = (JsonForm13)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain13)tableDataMain;
                    impRep.Rows13.Add(new Form13
                    {
                        NumberInOrder_DB = numberInOrder++,
                        OperationCode_DB = form.OperationCode,
                        OperationDate_DB = DateTime.TryParse(form.OperationDate, out var dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : form.OperationDate,
                        PassportNumber_DB = form.PassportNumber,
                        Type_DB = form.Type,
                        Radionuclids_DB = form.Radionuclids,
                        FactoryNumber_DB = form.FactoryNumber,
                        Activity_DB = form.Activity,
                        CreatorOKPO_DB = form.CreatorOKPO,
                        CreationDate_DB = DateTime.TryParse(form.CreationDate, out dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : form.CreationDate,
                        AggregateState_DB = byte.TryParse(form.AggregateState, out var byteValue)
                            ? byteValue
                            : null,
                        PropertyCode_DB = byte.TryParse(form.PropertyCode, out byteValue)
                            ? byteValue
                            : null,
                        Owner_DB = form.Owner,
                        DocumentVid_DB = byte.TryParse(form.DocumentVid, out byteValue)
                            ? byteValue
                            : null,
                        DocumentNumber_DB = form.DocumentNumber,
                        DocumentDate_DB = DateTime.TryParse(form.DocumentDate, out dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : form.DocumentDate,
                        ProviderOrRecieverOKPO_DB = form.ProviderOrRecieverOKPO,
                        TransporterOKPO_DB = form.TransporterOKPO,
                        PackName_DB = form.PackName,
                        PackType_DB = form.PackType,
                        PackNumber_DB = form.PackNumber
                    });
                }
                break;
            }

            #endregion

            #region Form14

            case "1.4":
            {
                var repForm = (JsonForm14)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain14)tableDataMain;
                    impRep.Rows14.Add(new Form14
                    {
                        NumberInOrder_DB = numberInOrder++,
                        OperationCode_DB = form.OperationCode,
                        OperationDate_DB = DateTime.TryParse(form.OperationDate, out var dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : form.OperationDate,
                        PassportNumber_DB = form.PassportNumber,
                        Name_DB = form.Name,
                        Sort_DB = byte.TryParse(form.Sort, out var byteValue)
                            ? byteValue
                            : null,
                        Radionuclids_DB = form.Radionuclids,
                        Activity_DB = form.Activity,
                        ActivityMeasurementDate_DB = DateTime.TryParse(form.ActivityMeasurementDate, out dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : form.ActivityMeasurementDate,
                        Volume_DB = form.Volume,
                        Mass_DB = form.Mass,
                        AggregateState_DB = byte.TryParse(form.AggregateState, out byteValue)
                            ? byteValue
                            : null,
                        PropertyCode_DB = byte.TryParse(form.PropertyCode, out byteValue)
                            ? byteValue
                            : null,
                        Owner_DB = form.Owner,
                        DocumentVid_DB = byte.TryParse(form.DocumentVid, out byteValue)
                            ? byteValue
                            : null,
                        DocumentNumber_DB = form.DocumentNumber,
                        DocumentDate_DB = DateTime.TryParse(form.DocumentDate, out dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : form.DocumentDate,
                        ProviderOrRecieverOKPO_DB = form.ProviderOrRecieverOKPO,
                        TransporterOKPO_DB = form.TransporterOKPO,
                        PackName_DB = form.PackName,
                        PackType_DB = form.PackType,
                        PackNumber_DB = form.PackNumber
                    });
                }
                break;
            }

            #endregion

            #region Form15

            case "1.5":
            {
                var repForm = (JsonForm15)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain15)tableDataMain;
                    impRep.Rows15.Add(new Form15
                    {
                        NumberInOrder_DB = numberInOrder++,
                        OperationCode_DB = form.OperationCode,
                        OperationDate_DB = form.OperationDate,
                        PassportNumber_DB = form.PassportNumber,
                        Type_DB = form.Type,
                        Radionuclids_DB = form.Radionuclids,
                        FactoryNumber_DB = form.FactoryNumber,
                        Quantity_DB = int.TryParse(form.Quantity, out var intValue)
                            ? intValue
                            : null,
                        Activity_DB = form.Activity,
                        CreationDate_DB = form.CreationDate,
                        StatusRAO_DB = form.StatusRAO,
                        DocumentVid_DB = byte.TryParse(form.DocumentVid, out var byteValue)
                            ? byteValue
                            : null,
                        DocumentNumber_DB = form.DocumentNumber,
                        DocumentDate_DB = form.DocumentDate,
                        ProviderOrRecieverOKPO_DB = form.ProviderOrRecieverOKPO,
                        TransporterOKPO_DB = form.TransporterOKPO,
                        PackName_DB = form.PackName,
                        PackType_DB = form.PackType,
                        PackNumber_DB = form.PackNumber,
                        StoragePlaceName_DB = form.StoragePlaceName,
                        StoragePlaceCode_DB = form.StoragePlaceCode,
                        RefineOrSortRAOCode_DB = form.RefineOrSortRAOCode,
                        Subsidy_DB = form.Subsidy,
                        FcpNumber_DB = form.FcpNumber
                    });
                }
                break;
            }

            #endregion

            #region Form16

            case "1.6":
            {
                var repForm = (JsonForm16)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain16)tableDataMain;
                    impRep.Rows16.Add(new Form16
                    {
                        NumberInOrder_DB = numberInOrder++,
                        OperationCode_DB = form.OperationCode,
                        OperationDate_DB = form.OperationDate,
                        CodeRAO_DB = form.CodeRAO,
                        StatusRAO_DB = form.StatusRAO,
                        Volume_DB = form.Volume,
                        Mass_DB = form.Mass,
                        QuantityOZIII_DB = form.QuantityOZIII,
                        MainRadionuclids_DB = form.MainRadionuclids,
                        TritiumActivity_DB = form.TritiumActivity,
                        BetaGammaActivity_DB = form.BetaGammaActivity,
                        AlphaActivity_DB = form.AlphaActivity,
                        TransuraniumActivity_DB = form.TransuraniumActivity,
                        ActivityMeasurementDate_DB = form.ActivityMeasurementDate,
                        DocumentVid_DB = byte.TryParse(form.DocumentVid, out var byteValue)
                            ? byteValue
                            : null,
                        DocumentNumber_DB = form.DocumentNumber,
                        DocumentDate_DB = form.DocumentDate,
                        ProviderOrRecieverOKPO_DB = form.ProviderOrRecieverOKPO,
                        TransporterOKPO_DB = form.TransporterOKPO,
                        StoragePlaceName_DB = form.StoragePlaceName,
                        StoragePlaceCode_DB = form.StoragePlaceCode,
                        RefineOrSortRAOCode_DB = form.RefineOrSortRAOCode,
                        PackName_DB = form.PackName,
                        PackType_DB = form.PackType,
                        PackNumber_DB = form.PackNumber,
                        Subsidy_DB = form.Subsidy,
                        FcpNumber_DB = form.FcpNumber
                    });
                }
                break;
            }

            #endregion

            #region Form17

            case "1.7":
            {
                var repForm = (JsonForm17)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain17)tableDataMain;
                    impRep.Rows17.Add(new Form17
                    {
                        NumberInOrder_DB = numberInOrder++,
                        OperationCode_DB = form.OperationCode,
                        OperationDate_DB = form.OperationDate,
                        PackName_DB = form.PackName,
                        PackType_DB = form.PackType,
                        PackFactoryNumber_DB = form.PackFactoryNumber,
                        PackNumber_DB = form.PackNumber,
                        FormingDate_DB = form.FormingDate,
                        PassportNumber_DB = form.PassportNumber,
                        Volume_DB = form.Volume,
                        Mass_DB = form.Mass,
                        Radionuclids_DB = form.Radionuclids,
                        SpecificActivity_DB = form.SpecificActivity,
                        DocumentVid_DB = byte.TryParse(form.DocumentVid, out var byteValue)
                            ? byteValue
                            : null,
                        DocumentNumber_DB = form.DocumentNumber,
                        DocumentDate_DB = form.DocumentDate,
                        ProviderOrRecieverOKPO_DB = form.ProviderOrRecieverOKPO,
                        TransporterOKPO_DB = form.TransporterOKPO,
                        StoragePlaceName_DB = form.StoragePlaceName,
                        StoragePlaceCode_DB = form.StoragePlaceCode,
                        CodeRAO_DB = form.CodeRAO,
                        StatusRAO_DB = form.StatusRAO,
                        VolumeOutOfPack_DB = form.VolumeOutOfPack,
                        MassOutOfPack_DB = form.MassOutOfPack,
                        Quantity_DB = form.Quantity,
                        TritiumActivity_DB = form.TritiumActivity,
                        BetaGammaActivity_DB = form.BetaGammaActivity,
                        AlphaActivity_DB = form.AlphaActivity,
                        TransuraniumActivity_DB = form.TransuraniumActivity,
                        RefineOrSortRAOCode_DB = form.RefineOrSortRAOCode,
                        Subsidy_DB = form.Subsidy,
                        FcpNumber_DB = form.FcpNumber
                    });
                }
                break;
            }

            #endregion

            #region Form18

            case "1.8":
            {
                var repForm = (JsonForm18)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain18)tableDataMain;
                    impRep.Rows18.Add(new Form18
                    {
                        NumberInOrder_DB = numberInOrder++,
                        OperationCode_DB = form.OperationCode,
                        OperationDate_DB = form.OperationDate,
                        IndividualNumberZHRO_DB = form.IndividualNumberZHRO,
                        PassportNumber_DB = form.PassportNumber,
                        Volume6_DB = form.Volume6,
                        Mass7_DB = form.Mass7,
                        SaltConcentration_DB = form.SaltConcentration,
                        Radionuclids_DB = form.Radionuclids,
                        SpecificActivity_DB = form.SpecificActivity,
                        DocumentVid_DB = byte.TryParse(form.DocumentVid, out var byteValue)
                            ? byteValue
                            : null,
                        DocumentNumber_DB = form.DocumentNumber,
                        DocumentDate_DB = form.DocumentDate,
                        ProviderOrRecieverOKPO_DB = form.ProviderOrRecieverOKPO,
                        TransporterOKPO_DB = form.TransporterOKPO,
                        StoragePlaceName_DB = form.StoragePlaceName,
                        StoragePlaceCode_DB = form.StoragePlaceCode,
                        CodeRAO_DB = form.CodeRAO,
                        StatusRAO_DB = form.StatusRAO,
                        Volume20_DB = form.Volume20,
                        Mass21_DB = form.Mass21,
                        TritiumActivity_DB = form.TritiumActivity,
                        BetaGammaActivity_DB = form.BetaGammaActivity,
                        AlphaActivity_DB = form.AlphaActivity,
                        TransuraniumActivity_DB = form.TransuraniumActivity,
                        RefineOrSortRAOCode_DB = form.RefineOrSortRAOCode,
                        Subsidy_DB = form.Subsidy,
                        FcpNumber_DB = form.FcpNumber
                    });
                }
                break;
            }

            #endregion

            #region Form19

            case "1.9":
            {
                var repForm = (JsonForm19)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain19)tableDataMain;
                    impRep.Rows19.Add(new Form19
                    {
                        NumberInOrder_DB = numberInOrder++,
                        OperationCode_DB = form.OperationCode,
                        OperationDate_DB = form.OperationDate,
                        DocumentVid_DB = byte.TryParse(form.DocumentVid, out var byteValue)
                            ? byteValue
                            : null,
                        DocumentNumber_DB = form.DocumentNumber,
                        DocumentDate_DB = form.DocumentDate,
                        CodeTypeAccObject_DB = short.TryParse(form.CodeTypeAccObject, out var shortValue)
                            ? shortValue
                            : null,
                        Radionuclids_DB = form.Radionuclids,
                        Activity_DB = form.Activity
                    });
                }
                break;
            }

            #endregion

            #endregion

            #region Form2

            #region Form21

            case "2.1":
            {
                var repForm = (JsonForm21)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain21)tableDataMain;
                    impRep.Rows21.Add(new Form21
                    {
                        NumberInOrder_DB = numberInOrder++,
                        RefineMachineName_DB = form.RefineMachineName,
                        MachineCode_DB = byte.TryParse(form.MachineCode, out var byteValue)
                            ? byteValue
                            : null,
                        MachinePower_DB = form.MachinePower,
                        NumberOfHoursPerYear_DB = form.NumberOfHoursPerYear,
                        CodeRAOIn_DB = form.CodeRAOIn,
                        StatusRAOIn_DB = form.StatusRAOIn,
                        VolumeIn_DB = form.VolumeIn,
                        MassIn_DB = form.MassIn,
                        QuantityIn_DB = form.QuantityIn,
                        TritiumActivityIn_DB = form.TritiumActivityIn,
                        BetaGammaActivityIn_DB = form.BetaGammaActivityIn,
                        AlphaActivityIn_DB = form.AlphaActivityIn,
                        TransuraniumActivityIn_DB = form.TransuraniumActivityIn,
                        CodeRAOout_DB = form.CodeRAOout,
                        StatusRAOout_DB = form.StatusRAOout,
                        VolumeOut_DB = form.VolumeOut,
                        MassOut_DB = form.MassOut,
                        QuantityOZIIIout_DB = form.QuantityOZIIIout,
                        TritiumActivityOut_DB = form.TritiumActivityOut,
                        BetaGammaActivityOut_DB = form.BetaGammaActivityOut,
                        AlphaActivityOut_DB = form.AlphaActivityOut,
                        TransuraniumActivityOut_DB = form.TransuraniumActivityOut
                    });
                }
                break;
            }

            #endregion

            #region Form22

            case "2.2":
            {
                var repForm = (JsonForm22)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain22)tableDataMain;
                    impRep.Rows22.Add(new Form22
                    {
                        NumberInOrder_DB = numberInOrder++,
                        StoragePlaceName_DB = form.StoragePlaceName,
                        StoragePlaceCode_DB = form.StoragePlaceCode,
                        PackName_DB = form.PackName,
                        PackType_DB = form.PackType,
                        PackQuantity_DB = form.PackQuantity,
                        CodeRAO_DB = form.CodeRAO,
                        StatusRAO_DB = form.StatusRAO,
                        VolumeOutOfPack_DB = form.VolumeOutOfPack,
                        VolumeInPack_DB = form.VolumeInPack,
                        MassOutOfPack_DB = form.MassOutOfPack,
                        MassInPack_DB = form.MassInPack,
                        QuantityOZIII_DB = form.QuantityOZIII,
                        TritiumActivity_DB = form.TritiumActivity,
                        BetaGammaActivity_DB = form.BetaGammaActivity,
                        AlphaActivity_DB = form.AlphaActivity,
                        TransuraniumActivity_DB = form.TransuraniumActivity,
                        MainRadionuclids_DB = form.MainRadionuclids,
                        Subsidy_DB = form.Subsidy,
                        FcpNumber_DB = form.FcpNumber
                    });
                }
                break;
            }

            #endregion

            #region Form23

            case "2.3":
            {
                var repForm = (JsonForm23)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain23)tableDataMain;
                    impRep.Rows23.Add(new Form23
                    {
                        NumberInOrder_DB = numberInOrder++,
                        StoragePlaceName_DB = form.StoragePlaceName,
                        StoragePlaceCode_DB = form.StoragePlaceCode,
                        ProjectVolume_DB = form.ProjectVolume,
                        CodeRAO_DB = form.CodeRAO,
                        Volume_DB = form.Volume,
                        Mass_DB = form.Mass,
                        QuantityOZIII_DB = form.QuantityOZIII,
                        SummaryActivity_DB = form.SummaryActivity,
                        DocumentNumber_DB = form.DocumentNumber,
                        DocumentDate_DB = form.DocumentDate,
                        ExpirationDate_DB = form.ExpirationDate,
                        DocumentName_DB = form.DocumentName
                    });
                }
                break;
            }

            #endregion

            #region Form24

            case "2.4":
            {
                var repForm = (JsonForm24)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain24)tableDataMain;
                    impRep.Rows24.Add(new Form24
                    {
                        NumberInOrder_DB = numberInOrder++,
                        CodeOYAT_DB = form.CodeOYAT,
                        FcpNumber_DB = form.FcpNumber,
                        MassCreated_DB = form.MassCreated,
                        QuantityCreated_DB = form.QuantityCreated,
                        MassFromAnothers_DB = form.MassFromAnothers,
                        QuantityFromAnothers_DB = form.QuantityFromAnothers,
                        MassFromAnothersImported_DB = form.MassFromAnothersImported,
                        QuantityFromAnothersImported_DB = form.QuantityFromAnothersImported,
                        MassAnotherReasons_DB = form.MassAnotherReasons,
                        QuantityAnotherReasons_DB = form.QuantityAnotherReasons,
                        MassTransferredToAnother_DB = form.MassTransferredToAnother,
                        QuantityTransferredToAnother_DB = form.QuantityTransferredToAnother,
                        MassRefined_DB = form.MassRefined,
                        QuantityRefined_DB = form.QuantityRefined,
                        MassRemovedFromAccount_DB = form.MassRemovedFromAccount,
                        QuantityRemovedFromAccount_DB = form.QuantityRemovedFromAccount
                    });
                }
                break;
            }

            #endregion

            #region Form25

            case "2.5":
            {
                var repForm = (JsonForm25)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain25)tableDataMain;
                    impRep.Rows25.Add(new Form25
                    {
                        NumberInOrder_DB = numberInOrder++,
                        StoragePlaceName_DB = form.StoragePlaceName,
                        StoragePlaceCode_DB = form.StoragePlaceCode,
                        CodeOYAT_DB = form.CodeOYAT,
                        FcpNumber_DB = form.FcpNumber,
                        FuelMass_DB = form.FuelMass,
                        CellMass_DB = form.CellMass,
                        Quantity_DB = int.TryParse(form.Quantity, out var intValue)
                            ? intValue
                            : null,
                        AlphaActivity_DB = form.AlphaActivity,
                        BetaGammaActivity_DB = form.BetaGammaActivity
                    });
                }
                break;
            }

            #endregion

            #region Form26

            case "2.6":
            {
                var repForm = (JsonForm26)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain26)tableDataMain;
                    impRep.Rows26.Add(new Form26
                    {
                        NumberInOrder_DB = numberInOrder++,
                        ObservedSourceNumber_DB = form.ObservedSourceNumber,
                        ControlledAreaName_DB = form.ControlledAreaName,
                        SupposedWasteSource_DB = form.SupposedWasteSource,
                        DistanceToWasteSource_DB = form.DistanceToWasteSource,
                        TestDepth_DB = form.TestDepth,
                        RadionuclidName_DB = form.RadionuclidName,
                        AverageYearConcentration_DB = form.AverageYearConcentration
                    });
                }
                break;
            }

            #endregion

            #region Form27

            case "2.7":
            {
                var repForm = (JsonForm27)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain27)tableDataMain;
                    impRep.Rows27.Add(new Form27
                    {
                        NumberInOrder_DB = numberInOrder++,
                        ObservedSourceNumber_DB = form.ObservedSourceNumber,
                        RadionuclidName_DB = form.RadionuclidName,
                        AllowedWasteValue_DB = form.AllowedWasteValue,
                        FactedWasteValue_DB = form.FactedWasteValue,
                        WasteOutbreakPreviousYear_DB = form.WasteOutbreakPreviousYear
                    });
                }
                break;
            }

            #endregion

            #region Form28

            case "2.8":
            {
                var repForm = (JsonForm28)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain28)tableDataMain;
                    impRep.Rows28.Add(new Form28
                    {
                        NumberInOrder_DB = numberInOrder++,
                        WasteSourceName_DB = form.WasteSourceName,
                        WasteRecieverName_DB = form.WasteRecieverName,
                        RecieverTypeCode_DB = form.RecieverTypeCode,
                        PoolDistrictName_DB = form.PoolDistrictName,
                        AllowedWasteRemovalVolume_DB = form.AllowedWasteRemovalVolume,
                        RemovedWasteVolume_DB = form.RemovedWasteVolume
                    });
                }
                break;
            }

            #endregion

            #region Form29

            case "2.9":
            {
                var repForm = (JsonForm29)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain29)tableDataMain;
                    impRep.Rows29.Add(new Form29
                    {
                        NumberInOrder_DB = numberInOrder++,
                        WasteSourceName_DB = form.WasteSourceName,
                        RadionuclidName_DB = form.RadionuclidName,
                        AllowedActivity_DB = form.AllowedActivity,
                        FactedActivity_DB = form.FactedActivity
                    });
                }
                break;
            }

            #endregion

            #region Form210

            case "2.10":
            {
                var repForm = (JsonForm210)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain210)tableDataMain;
                    impRep.Rows210.Add(new Form210
                    {
                        NumberInOrder_DB = numberInOrder++,
                        IndicatorName_DB = form.IndicatorName,
                        PlotName_DB = form.PlotName,
                        PlotKadastrNumber_DB = form.PlotKadastrNumber,
                        PlotCode_DB = form.PlotCode,
                        InfectedArea_DB = form.InfectedArea,
                        AvgGammaRaysDosePower_DB = form.AvgGammaRaysDosePower,
                        MaxGammaRaysDosePower_DB = form.MaxGammaRaysDosePower,
                        WasteDensityAlpha_DB = form.WasteDensityAlpha,
                        WasteDensityBeta_DB = form.WasteDensityBeta,
                        FcpNumber_DB = form.FcpNumber
                    });
                }
                break;
            }

            #endregion

            #region Form211

            case "2.11":
            {
                var repForm = (JsonForm211)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain211)tableDataMain;
                    impRep.Rows211.Add(new Form211
                    {
                        NumberInOrder_DB = numberInOrder++,
                        PlotName_DB = form.PlotName,
                        PlotKadastrNumber_DB = form.PlotKadastrNumber,
                        PlotCode_DB = form.PlotCode,
                        InfectedArea_DB = form.InfectedArea,
                        Radionuclids_DB = form.Radionuclids,
                        SpecificActivityOfPlot_DB = form.SpecificActivityOfPlot,
                        SpecificActivityOfLiquidPart_DB = form.SpecificActivityOfLiquidPart,
                        SpecificActivityOfDensePart_DB = form.SpecificActivityOfDensePart
                    });
                }
                break;
            }

            #endregion

            #region Form212

            case "2.12":
            {
                var repForm = (JsonForm212)rep;
                foreach (var tableDataMain in repForm.FormTable.TableData)   // Для каждой строчки формы
                {
                    var form = (TableDataMain212)tableDataMain;
                    impRep.Rows212.Add(new Form212
                    {
                        NumberInOrder_DB = numberInOrder++,
                        OperationCode_DB = short.TryParse(form.OperationCode, out var shortValue)
                            ? shortValue
                            : null,
                        ObjectTypeCode_DB = short.TryParse(form.ObjectTypeCode, out shortValue)
                            ? shortValue
                            : null,
                        Radionuclids_DB = form.Radionuclids,
                        Activity_DB = form.Activity,
                        ProviderOrRecieverOKPO_DB = form.ProviderOrRecieverOKPO
                    });
                }
                break;
            }

            #endregion

            #endregion
        }
    }

    #endregion

    #region ColNameToColNum

    internal static string ColNameToColNum(string formNum, string colName)
    {
        return formNum switch
        {
            #region 1.1

            "1.1" => colName switch
            {
                "OpCod" => "2",
                "OpDate" => "3",
                "PaspN" => "4",
                "Typ" => "5",
                "Nuclid" => "6",
                "Numb" => "7",
                "Sht" => "8",
                "Activn" => "9",
                "IzgotOKPO" => "10",
                "IzgotDate" => "11",
                "Kateg" => "12",
                "Nss" => "13",
                "FormSobst" => "14",
                "Pravoobl" => "15",
                "DocVid" => "16",
                "DocN" => "17",
                "DocDate" => "18",
                "OkpoPIP" => "19",
                "OkpoPrv" => "20",
                "PrName" => "21",
                "UktPrTyp" => "22",
                "UktPrN" => "23",
                _ => ""
            },

            #endregion

            #region 1.2

            "1.2" => colName switch
            {
                "OpCod" => "2",
                "OpDate" => "3",
                "PaspN" => "4",
                "Typ" => "5",
                "Numb" => "6",
                "Kg" => "7",
                "IzgotOKPO" => "8",
                "IzgotDate" => "9",
                "Nss" => "10",
                "FormSobst" => "11",
                "Pravoobl" => "12",
                "DocVid" => "13",
                "DocN" => "14",
                "DocDate" => "15",
                "OkpoPIP" => "16",
                "OkpoPrv" => "17",
                "PrName" => "18",
                "UktPrTyp" => "19",
                "UktPrN" => "20",
                _ => ""
            },

            #endregion

            #region 1.3

            "1.3" => colName switch
            {
                "OpCod" => "2",
                "OpDate" => "3",
                "PaspN" => "4",
                "Typ" => "5",
                "Nuclid" => "6",
                "Numb" => "7",
                "Activn" => "8",
                "IzgotOKPO" => "9",
                "IzgotDate" => "10",
                "Agr" => "11",
                "FormSobst" => "12",
                "Pravoobl" => "13",
                "DocVid" => "14",
                "DocN" => "15",
                "DocDate" => "16",
                "OkpoPIP" => "17",
                "OkpoPrv" => "18",
                "PrName" => "19",
                "UktPrTyp" => "20",
                "UktPrN" => "21",
                _ => ""
            },

            #endregion

            #region 1.4

            "1.4" => colName switch
            {
                "OpCod" => "2",
                "OpDate" => "3",
                "PaspN" => "4",
                "IstName" => "5",
                "Typ" => "6",
                "Nuclid" => "7",
                "Activn" => "8",
                "ActDate" => "9",
                "Kbm" => "10",
                "Kg" => "11",
                "Arg" => "12",
                "FormSobst" => "13",
                "Pravoobl" => "14",
                "DocVid" => "15",
                "DocN" => "16",
                "DocDate" => "17",
                "OkpoPIP" => "18",
                "OkpoPrv" => "19",
                "PrName" => "20",
                "UktPrTyp" => "21",
                "UktPrN" => "22",
                _ => ""
            },

            #endregion

            #region 1.5

            "1.5" => colName switch
            {
                "OpCod" => "2",
                "OpDate" => "3",
                "PaspN" => "4",
                "Typ" => "5",
                "Nuclid" => "6",
                "Numb" => "7",
                "Sht" => "8",
                "Activn" => "9",
                "IzgotDate" => "10",
                "BCod" => "11",
                "DocVid" => "12",
                "DocN" => "13",
                "DocDate" => "14",
                "OkpoPIP" => "15",
                "OkpoPrv" => "16",
                "PrName" => "17",
                "UktPrTyp" => "18",
                "UktPrN" => "19",
                "PH_Name" => "20",
                "PH_Cod" => "21",
                "CodRAO" => "22",
                "Subsid" => "23",
                "FCP" => "24",
                _ => ""
            },

            #endregion

            #region 1.6

            "1.6" => colName switch
            {
                "OpCod" => "2",
                "OpDate" => "3",
                "RAOCod" => "4",
                "BCod" => "5",
                "Kbm" => "6",
                "Tonne" => "7",
                "Sht" => "8",
                "RAOCodMax" => "9",
                "ActTR" => "10",
                "ActBG" => "11",
                "ActA" => "12",
                "ActUR" => "13",
                "ActDate" => "14",
                "DocVid" => "15",
                "DocN" => "16",
                "DocDate" => "17",
                "OkpoPIP" => "18",
                "OkpoPrv" => "19",
                "PH_Name" => "20",
                "PH_Cod" => "21",
                "UpCod" => "22",
                "PrName" => "23",
                "UktPrTyp" => "24",
                "UktPrN" => "25",
                "Subsid" => "26",
                "FCP" => "27",
                _ => ""
            },

            #endregion

            #region 1.7

            "1.7" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g4" => "4",
                "g5" => "5",
                "g6" => "6",
                "g7" => "7",
                "g8" => "8",
                "g9" => "9",
                "g10" => "10",
                "g11" => "11",
                "g12" => "12",
                "g13" => "13",
                "g14" => "14",
                "g15" => "15",
                "g16" => "16",
                "g17" => "17",
                "g18" => "18",
                "g19" => "19",
                "g20" => "20",
                "g21" => "21",
                "g22" => "22",
                "g23" => "23",
                "g24" => "24",
                "g25" => "25",
                "g26" => "26",
                "g27" => "27",
                "g28" => "28",
                "g29" => "29",
                "g30" => "30",
                "g31" => "31",
                "g32" => "32",
                _ => ""
            },

            #endregion

            #region 1.8

            "1.8" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g4" => "4",
                "g5" => "5",
                "g6" => "6",
                "g7" => "7",
                "g8" => "8",
                "g9" => "9",
                "g10" => "10",
                "g11" => "11",
                "g12" => "12",
                "g13" => "13",
                "g14" => "14",
                "g15" => "15",
                "g16" => "16",
                "g17" => "17",
                "g18" => "18",
                "g19" => "19",
                "g20" => "20",
                "g21" => "21",
                "g22" => "22",
                "g23" => "23",
                "g24" => "24",
                "g25" => "25",
                "g26" => "26",
                "g27" => "27",
                "g28" => "28",
                _ => ""
            },

            #endregion

            #region 1.9

            "1.9" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g4" => "4",
                "g5" => "5",
                "g6" => "6",
                "g7" => "7",
                "g8" => "8",
                "g9" => "9",
                _ => ""
            },

            #endregion

            #region 2.1

            "2.1" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g19" => "4",
                "g4" => "5",
                "g5" => "6",
                "g6" => "7",
                "g7" => "8",
                "g8" => "9",
                "g9" => "10",
                "g20" => "11",
                "g21" => "12",
                "g10" => "13",
                "g11" => "14",
                "g12" => "15",
                "g13" => "16",
                "g14" => "17",
                "g15" => "18",
                "g16" => "19",
                "g22" => "20",
                "g23" => "21",
                "g17" => "22",
                "g18" => "23",
                _ => ""
            },

            #endregion

            #region 2.2

            "2.2" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g11" => "4",
                "g12" => "5",
                "g13" => "6",
                "g4" => "7",
                "g5" => "8",
                "g6" => "9",
                "g7" => "10",
                "g8" => "11",
                "g9" => "12",
                "g10" => "13",
                "g19" => "14",
                "g15" => "15",
                "g14" => "16",
                "g20" => "17",
                "g16" => "18",
                "g21" => "19",
                "g22" => "20",
                _ => ""
            },

            #endregion

            #region 2.3

            "2.3" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g13" => "4",
                "g4" => "5",
                "g5" => "6",
                "g6" => "7",
                "g7" => "8",
                "g8" => "9",
                "g9" => "10",
                "g10" => "11",
                "g11" => "12",
                "g12" => "13",
                _ => ""
            },

            #endregion

            #region 2.4

            "2.4" => colName switch
            {
                "g2" => "2",
                "g17" => "3",
                "g3" => "4",
                "g4" => "5",
                "g5" => "6",
                "g6" => "7",
                "g7" => "8",
                "g8" => "9",
                "g9" => "10",
                "g10" => "11",
                "g11" => "12",
                "g12" => "13",
                "g13" => "14",
                "g14" => "15",
                "g15" => "16",
                "g16" => "17",
                _ => ""
            },

            #endregion

            #region 2.5

            "2.5" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g4" => "4",
                "g10" => "5",
                "g5" => "6",
                "g6" => "7",
                "g7" => "8",
                "g8" => "9",
                "g9" => "10",
                _ => ""
            },

            #endregion

            #region 2.6

            "2.6" => colName switch
            {
                "num_s" => "2",
                "zone" => "3",
                "ist" => "4",
                "rust" => "5",
                "g1" => "6",
                "nuk" => "7",
                "radio" => "8",
                _ => ""
            },

            #endregion

            #region 2.7

            "2.7" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g4" => "4",
                "g5" => "5",
                "g6" => "6",
                _ => ""
            },

            #endregion

            #region 2.8

            "2.8" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g4" => "4",
                "g5" => "5",
                "g6" => "6",
                "g7" => "7",
                _ => ""
            },

            #endregion

            #region 2.9

            "2.9" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g4" => "4",
                "g5" => "5",
                _ => ""
            },

            #endregion

            #region 2.10

            "2.10" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g4" => "4",
                "g5" => "5",
                "g6" => "6",
                "g7" => "7",
                "g8" => "8",
                "g9" => "9",
                "g10" => "10",
                "g11" => "11",
                _ => ""
            },

            #endregion

            #region 2.11

            "2.11" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g4" => "4",
                "square" => "5",
                "g5" => "6",
                "g6" => "7",
                "g7" => "8",
                "g8" => "9",
                _ => ""
            },

            #endregion

            #region 2.12

            "2.12" => colName switch
            {
                "g2" => "2",
                "g3" => "3",
                "g4" => "4",
                "g5" => "5",
                "g6" => "6",
                _ => ""
            },

            #endregion

            _ => ""
        };
    }

    #endregion
}