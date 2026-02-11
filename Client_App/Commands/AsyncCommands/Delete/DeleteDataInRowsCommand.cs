using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form4;
using Models.Forms.Form5;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Очищаем все поля, кроме порядкового номера (NumberInOrder).
/// </summary>
public class NewDeleteDataInRowsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is ObservableCollection<Form> { Count: > 0 } formCollection)
        {
            #region 1.1

            if (formCollection.Any(x => x is Form11))
            {
                var form11Collection = formCollection.ToList().Cast<Form11>();
                foreach (var form in form11Collection)
                {
                    if (form is null) continue;

                    form.OperationCode.Value = null;
                    form.OperationDate.Value = null;
                    form.PassportNumber.Value = null;
                    form.Type.Value = null;
                    form.Radionuclids.Value = null;
                    form.FactoryNumber.Value = null;
                    form.Quantity.Value = null;
                    form.Activity.Value = null;
                    form.CreatorOKPO.Value = null;
                    form.CreationDate.Value = null;
                    form.Category.Value = null;
                    form.SignedServicePeriod.Value = null;
                    form.PropertyCode.Value = null;
                    form.Owner.Value = null;
                    form.DocumentVid.Value = null;
                    form.DocumentNumber.Value = null;
                    form.DocumentDate.Value = null;
                    form.ProviderOrRecieverOKPO.Value = null;
                    form.TransporterOKPO.Value = null;
                    form.PackName.Value = null;
                    form.PackType.Value = null;
                    form.PackNumber.Value = null;
                }
            }

            #endregion

            #region 1.2
            
            else if (formCollection.Any(x => x is Form12))
            {
                var form12Collection = formCollection.ToList().Cast<Form12>();
                foreach (var form in form12Collection)
                {
                    if (form is null) continue;

                    form.OperationCode.Value = null;
                    form.OperationDate.Value = null;
                    form.PassportNumber.Value = null;
                    form.NameIOU.Value = null;
                    form.FactoryNumber.Value = null;
                    form.Mass.Value = null;
                    form.CreatorOKPO.Value = null;
                    form.CreationDate.Value = null;
                    form.SignedServicePeriod.Value = null;
                    form.PropertyCode.Value = null;
                    form.Owner.Value = null;
                    form.DocumentVid.Value = null;
                    form.DocumentNumber.Value = null;
                    form.DocumentDate.Value = null;
                    form.ProviderOrRecieverOKPO.Value = null;
                    form.TransporterOKPO.Value = null;
                    form.PackName.Value = null;
                    form.PackType.Value = null;
                    form.PackNumber.Value = null;
                }
            }

            #endregion

            #region 1.3

            else if (formCollection.Any(x => x is Form13))
            {
                var form13Collection = formCollection.ToList().Cast<Form13>();
                foreach (var form in form13Collection)
                {
                    if (form is null) continue;

                    form.OperationCode.Value = null;
                    form.OperationDate.Value = null;
                    form.PassportNumber.Value = null;
                    form.Type.Value = null;
                    form.Radionuclids.Value = null;
                    form.FactoryNumber.Value = null;
                    form.Activity.Value = null;
                    form.CreatorOKPO.Value = null;
                    form.CreationDate.Value = null;
                    form.AggregateState.Value = null;
                    form.PropertyCode.Value = null;
                    form.Owner.Value = null;
                    form.DocumentVid.Value = null;
                    form.DocumentNumber.Value = null;
                    form.DocumentDate.Value = null;
                    form.ProviderOrRecieverOKPO.Value = null;
                    form.TransporterOKPO.Value = null;
                    form.PackName.Value = null;
                    form.PackType.Value = null;
                    form.PackNumber.Value = null;
                }
            }

            #endregion

            #region 1.4

            else if (formCollection.Any(x => x is Form14))
            {
                var form14Collection = formCollection.ToList().Cast<Form14>();
                foreach (var form in form14Collection)
                {
                    if (form is null) continue;

                    form.OperationCode.Value = null;
                    form.OperationDate.Value = null;
                    form.PassportNumber.Value = null;
                    form.Name.Value = null;
                    form.Sort.Value = null;
                    form.Radionuclids.Value = null;
                    form.Activity.Value = null;
                    form.ActivityMeasurementDate.Value = null;
                    form.Volume.Value = null;
                    form.Mass.Value = null;
                    form.AggregateState.Value = null;
                    form.PropertyCode.Value = null;
                    form.Owner.Value = null;
                    form.DocumentVid.Value = null;
                    form.DocumentNumber.Value = null;
                    form.DocumentDate.Value = null;
                    form.ProviderOrRecieverOKPO.Value = null;
                    form.TransporterOKPO.Value = null;
                    form.PackName.Value = null;
                    form.PackType.Value = null;
                    form.PackNumber.Value = null;
                }
            }

            #endregion

            #region 1.5

            if (formCollection.Any(x => x is Form15))
            {
                var form15Collection = formCollection.ToList().Cast<Form15>();
                foreach (var form in form15Collection)
                {
                    if (form is null) continue;

                    form.OperationCode.Value = null;
                    form.OperationDate.Value = null;
                    form.PassportNumber.Value = null;
                    form.Type.Value = null;
                    form.Radionuclids.Value = null;
                    form.FactoryNumber.Value = null;
                    form.Quantity.Value = null;
                    form.Activity.Value = null;
                    form.CreationDate.Value = null;
                    form.StatusRAO.Value = null;
                    form.DocumentVid.Value = null;
                    form.DocumentNumber.Value = null;
                    form.DocumentDate.Value = null;
                    form.ProviderOrRecieverOKPO.Value = null;
                    form.TransporterOKPO.Value = null;
                    form.PackName.Value = null;
                    form.PackType.Value = null;
                    form.PackNumber.Value = null;
                    form.StoragePlaceName.Value = null;
                    form.StoragePlaceCode.Value = null;
                    form.RefineOrSortRAOCode.Value = null;
                    form.Subsidy.Value = null;
                    form.FcpNumber.Value = null;
                    form.ContractNumber.Value = null;
                }
            }

            #endregion

            #region 1.6

            if (formCollection.Any(x => x is Form16))
            {
                var form16Collection = formCollection.ToList().Cast<Form16>();
                foreach (var form in form16Collection)
                {
                    if (form is null) continue;

                    form.OperationCode.Value = null;
                    form.OperationDate.Value = null;
                    form.CodeRAO.Value = null;
                    form.StatusRAO.Value = null;
                    form.Volume.Value = null;
                    form.Mass.Value = null;
                    form.QuantityOZIII.Value = null;
                    form.MainRadionuclids.Value = null;
                    form.TritiumActivity.Value = null;
                    form.BetaGammaActivity.Value = null;
                    form.AlphaActivity.Value = null;
                    form.TransuraniumActivity.Value = null;
                    form.ActivityMeasurementDate.Value = null;
                    form.DocumentVid.Value = null;
                    form.DocumentNumber.Value = null;
                    form.DocumentDate.Value = null;
                    form.ProviderOrRecieverOKPO.Value = null;
                    form.TransporterOKPO.Value = null; 
                    form.StoragePlaceName.Value = null;
                    form.StoragePlaceCode.Value = null;
                    form.RefineOrSortRAOCode.Value = null;
                    form.PackName.Value = null;
                    form.PackType.Value = null;
                    form.PackNumber.Value = null;
                    form.Subsidy.Value = null;
                    form.FcpNumber.Value = null;
                    form.ContractNumber.Value = null;
                }
            }

            #endregion

            #region 1.7

            if (formCollection.Any(x => x is Form17))
            {
                var form17Collection = formCollection.ToList().Cast<Form17>();
                foreach (var form in form17Collection)
                {
                    if (form is null) continue;

                    form.OperationCode.Value = null;
                    form.OperationDate.Value = null;
                    form.PackName.Value = null;
                    form.PackType.Value = null;
                    form.PackFactoryNumber.Value = null;
                    form.PackNumber.Value = null;
                    form.FormingDate.Value = null;
                    form.PassportNumber.Value = null;
                    form.Volume.Value = null;
                    form.Mass.Value = null;
                    form.Radionuclids.Value = null;
                    form.SpecificActivity.Value = null;
                    form.DocumentVid.Value = null;
                    form.DocumentNumber.Value = null;
                    form.DocumentDate.Value = null;
                    form.ProviderOrRecieverOKPO.Value = null;
                    form.TransporterOKPO.Value = null;
                    form.StoragePlaceName.Value = null;
                    form.StoragePlaceCode.Value = null;
                    form.CodeRAO.Value = null;
                    form.StatusRAO.Value = null;
                    form.VolumeOutOfPack.Value = null;
                    form.MassOutOfPack.Value = null;
                    form.Quantity.Value = null;
                    form.TritiumActivity.Value = null;
                    form.BetaGammaActivity.Value = null;
                    form.AlphaActivity.Value = null;
                    form.TransuraniumActivity.Value = null;
                    form.RefineOrSortRAOCode.Value = null;
                    form.Subsidy.Value = null;
                    form.FcpNumber.Value = null;
                    form.ContractNumber.Value = null;
                }
            }

            #endregion

            #region 1.8

            if (formCollection.Any(x => x is Form18))
            {
                var form18Collection = formCollection.ToList().Cast<Form18>();
                foreach (var form in form18Collection)
                {
                    if (form is null) continue;

                    form.OperationCode.Value = null;
                    form.OperationDate.Value = null;
                    form.IndividualNumberZHRO.Value = null;
                    form.PassportNumber.Value = null;
                    form.Volume6.Value = null;
                    form.Mass7.Value = null;
                    form.SaltConcentration.Value = null;
                    form.Radionuclids.Value = null;
                    form.SpecificActivity.Value = null;
                    form.DocumentVid.Value = null;
                    form.DocumentNumber.Value = null;
                    form.DocumentDate.Value = null;
                    form.ProviderOrRecieverOKPO.Value = null;
                    form.TransporterOKPO.Value = null;
                    form.StoragePlaceName.Value = null;
                    form.StoragePlaceCode.Value = null;
                    form.CodeRAO.Value = null;
                    form.StatusRAO.Value = null;
                    form.Volume20.Value = null;
                    form.Mass21.Value = null;
                    form.TritiumActivity.Value = null;
                    form.BetaGammaActivity.Value = null;
                    form.AlphaActivity.Value = null;
                    form.TransuraniumActivity.Value = null;
                    form.RefineOrSortRAOCode.Value = null;
                    form.Subsidy.Value = null;
                    form.FcpNumber.Value = null;
                    form.ContractNumber.Value = null;
                }
            }

            #endregion

            #region 1.9

            if (formCollection.Any(x => x is Form19))
            {
                var form19Collection = formCollection.ToList().Cast<Form19>();
                foreach (var form in form19Collection)
                {
                    if (form is null) continue;

                    form.OperationCode.Value = null;
                    form.OperationDate.Value = null;
                    form.DocumentVid.Value = null;
                    form.DocumentNumber.Value = null;
                    form.DocumentDate.Value = null;
                    form.CodeTypeAccObject.Value = null;
                    form.Radionuclids.Value = null;
                    form.Activity.Value = null;
                }
            }

            #endregion

            #region 4.1

            if (formCollection.Any(x => x is Form41))
            {
                var form41Collection = formCollection.ToList().Cast<Form41>();
                foreach (var form in form41Collection)
                {
                    if (form is null) continue;

                    form.RegNo.Value = null;
                    form.Okpo.Value = null;
                    form.OrganizationName.Value = null;
                    form.LicenseOrRegistrationInfo.Value = null;
                    form.NumOfFormsWithInventarizationInfo.Value = 0;
                    form.NumOfFormsWithoutInventarizationInfo.Value = 0;
                    form.NumOfForms212.Value = 0;
                    form.Note.Value = null;
                }
            }

            #endregion

            #region 5.1

            if (formCollection.Any(x => x is Form51))
            {
                var form51Collection = formCollection.ToList().Cast<Form51>();
                foreach (var form in form51Collection)
                {
                    if (form is null) continue;

                    form.OperationCode.Value = null;
                    form.Category.Value = null;
                    form.Radionuclids.Value = null;
                    form.Quantity.Value = null;
                    form.Activity.Value = null;
                    form.ProviderOrRecieverOKPO.Value = null;
                }
            }

            #endregion

            #region 5.2

            if (formCollection.Any(x => x is Form52))
            {
                var form52Collection = formCollection.ToList().Cast<Form52>();
                foreach (var form in form52Collection)
                {
                    if (form is null) continue;

                    form.Category.Value = null;
                    form.Radionuclids.Value = null;
                    form.Quantity.Value = null;
                    form.Activity.Value = null;
                }
            }

            #endregion

            #region 5.3

            if (formCollection.Any(x => x is Form53))
            {
                var form53Collection = formCollection.ToList().Cast<Form53>();
                foreach (var form in form53Collection)
                {
                    if (form is null) continue;

                    form.OperationCode.Value = null;
                    form.TypeORI.Value = null;
                    form.VarietyORI.Value = null;
                    form.AggregateState.Value = null;
                    form.ProviderOrRecieverOKPO.Value = null;
                    form.Radionuclids.Value = null;
                    form.Activity.Value = null;
                    form.Mass.Value = null;
                    form.Volume.Value = null;
                    form.Quantity.Value = null;
                }
            }

            #endregion

            #region 5.4

            if (formCollection.Any(x => x is Form54))
            {
                var form54Collection = formCollection.ToList().Cast<Form54>();
                foreach (var form in form54Collection)
                {
                    if (form is null) continue;

                    form.TypeORI.Value = null;
                    form.VarietyORI.Value = null;
                    form.AggregateState.Value = null;
                    form.Radionuclids.Value = null;
                    form.Activity.Value = null;
                    form.Mass.Value = null;
                    form.Volume.Value = null;
                    form.Quantity.Value = null;
                }
            }

            #endregion

            #region 5.5

            if (formCollection.Any(x => x is Form55))
            {
                var form55Collection = formCollection.ToList().Cast<Form55>();
                foreach (var form in form55Collection)
                {
                    if (form is null) continue;

                    form.Name.Value = null;
                    form.OperationCode.Value = null;
                    form.ProviderOrRecieverOKPO.Value = null;
                    form.Quantity.Value = null;
                    form.Mass.Value = null;
                }
            }

            #endregion

            #region 5.6

            if (formCollection.Any(x => x is Form56))
            {
                var form56Collection = formCollection.ToList().Cast<Form56>();
                foreach (var form in form56Collection)
                {
                    if (form is null) continue;

                    form.Name.Value = null;
                    form.Quantity.Value = null;
                    form.Mass.Value = null;
                }
            }

            #endregion

            #region 5.7

            if (formCollection.Any(x => x is Form57))
            {
                var form57Collection = formCollection.ToList().Cast<Form57>();
                foreach (var form in form57Collection)
                {
                    if (form is null) continue;

                    form.RegNo.Value = null;
                    form.OKPO.Value = null;
                    form.Name.Value = null;
                    form.Recognizance.Value = null;
                    form.License.Value = null;
                    form.Practice.Value = null;
                    form.Note.Value = null;
                }
            }

            #endregion
        }
    }
    
}