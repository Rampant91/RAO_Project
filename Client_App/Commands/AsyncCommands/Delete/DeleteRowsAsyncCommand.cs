using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Forms.Form2;
using Models.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Interfaces;
using Client_App.ViewModels;
using Models.Collections;

namespace Client_App.Commands.AsyncCommands.Delete;

//  Удалить выбранные строчки из формы
internal class DeleteRowsAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        var param = ((IEnumerable<IKey>)parameter).ToArray();

        #region MessageDeleteLine

        var answer = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да", IsDefault = true },
                    new ButtonDefinition { Name = "Нет", IsCancel = true }
                ],
                ContentTitle = "Удаление строки",
                ContentHeader = "Уведомление",
                ContentMessage = "Вы действительно хотите удалить строчку?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow);

        #endregion

        if (answer is "Да")
        {
            var minItem = param.Min(x => x.Order);
            foreach (var item in param)
            {
                if (item != null)
                {
                    Storage.Rows.Remove(item);
                }
            }

            var rows = Storage[Storage.FormNum_DB]
                .GetEnumerable()
                .Cast<Form>()
                .ToList();
            switch (rows.First().FormNum_DB)
            {
                case "2.1":
                {
                    var count = 1;
                    foreach (var key in rows)
                    {
                        var row = (Form21)key;
                        row.NumberInOrder_DB = count;
                        count++;
                        row.NumberInOrderSum_DB = "";
                        //row.NumberInOrderSum = new RamAccess<string>(null, "");   //выполняется 0.2c. на итерацию, вроде работает и с заменой выше
                    }

                    break;
                }
                case "2.2":
                {
                    var count = 1;
                    foreach (var key in rows)
                    {
                        var row = (Form22)key;
                        row.NumberInOrder_DB = count;
                        count++;
                        row.NumberInOrderSum_DB = "";
                    }

                    break;
                }
                default:
                {
                    await Storage.SortAsync();
                    var itemQ = Storage.Rows
                        .GetEnumerable()
                        .Where(x => x.Order > minItem)
                        .Select(x => x as Form)
                        .ToList();
                    foreach (var form in itemQ)
                    {
                        //form.SetOrder(minItem);   //выполняется полсекунды на итерацию, вроде работает и с заменой ниже
                        form.NumberInOrder_DB = (int)minItem;
                        form.NumberInOrder.OnPropertyChanged();
                        minItem++;
                    }

                    break;
                }
            }
        }
    }
}