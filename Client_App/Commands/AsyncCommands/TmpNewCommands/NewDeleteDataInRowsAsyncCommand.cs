using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Models.Forms.Form1;

namespace Client_App.Commands.AsyncCommands.TmpNewCommands;

public class NewDeleteDataInRowsAsyncCommand : BaseAsyncCommand
{
    public override bool CanExecute(object? parameter)
    {
        // Сначала проверяем базовую логику (не выполняется ли команда)
        if (!base.CanExecute(parameter)) return false;
        
        // Затем проверяем, что параметр подходящий
        return parameter is ObservableCollection<Form12> { Count: > 0 };
    }

    public override async Task AsyncExecute(object? parameter)
    {
        // Проверяем, что параметр не null и является ObservableCollection<Form12>
        if (parameter is ObservableCollection<Form12> { Count: > 0 } formCollection)
        {
            foreach (var form in formCollection)
            {
                if (form != null)
                {
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