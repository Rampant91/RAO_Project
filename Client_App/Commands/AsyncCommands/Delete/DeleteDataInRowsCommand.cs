using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Models.Forms;
using Models.Forms.Form1;

namespace Client_App.Commands.AsyncCommands.Delete;

public class DeleteDataInRowsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        // Проверяем, что параметр не null и является ObservableCollection<Form12>
        if (parameter is ObservableCollection<Form> { Count: > 0 } formCollection)
        {
            if (formCollection.Any(x => x is Form12))
            {
                var form12Collection = formCollection.ToList().Cast<Form12>();
                foreach (var form in form12Collection)
                {
                    if (form is null) continue;

                    // Очищаем все поля, кроме порядкового номера (NumberInOrder)
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
        }
    }
}