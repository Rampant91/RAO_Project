using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms1;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using Models.Forms.Form1;
using System;
using System.Threading.Tasks;
using Client_App.Views;
using Client_App.Views.Forms;
using Models.Forms;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Вставить значения из буфера обмена
/// </summary>
public class NewPasteRowsAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    //После обновления версии Авалонии нужно будет добавить вставку в формате html
    private Report Storage => formVM.CurrentReport;
    private Form SelectedForm => formVM.SelectedForm;
    private string FormType => formVM.FormType;
    public override async Task AsyncExecute(object? parameter)
    {

        var clipboard = Application.Current.Clipboard;

        var pastedString = await clipboard.GetTextAsync();
        if ((pastedString == null) || (pastedString == "")) return;

        var rows = pastedString.Split("\r\n");

        //Последняя строка пустая, поэтому выделяем память на одну ячейку меньше
        string[][] parsedRows = new string[rows.Length-1][];
        for(int i =0; i< parsedRows.Length;i++)
        {
            parsedRows[i] = rows[i].Split('\t');
        }

        int start = SelectedForm.NumberInOrder.Value-1;

        if (start + parsedRows.Length > Storage.Rows.Count)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = $"Вставка данных из буфера обмена",
                        ContentHeader = "Внимание",
                        ContentMessage =
                            $"В таблице не хватает места для некоторых строк, которые вы хотите вставить",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));
        }

        for (int i = 0; i < parsedRows.Length && i+start<Storage.Rows.Count; i++)
        {
            var form = Storage.Rows.Get<Form12>(i+start);

            form.OperationCode.Value = parsedRows[i][1];
            form.OperationDate.Value = parsedRows[i][2];
            form.PassportNumber.Value = parsedRows[i][3];
            form.NameIOU.Value = parsedRows[i][4];
            form.FactoryNumber.Value = parsedRows[i][5];
            form.Mass.Value = parsedRows[i][6];
            form.CreatorOKPO.Value = parsedRows[i][7];
            form.CreationDate.Value = parsedRows[i][8];
            form.SignedServicePeriod.Value = parsedRows[i][9];
            form.PropertyCode.Value = Convert.ToByte(parsedRows[i][10]);
            form.Owner.Value = parsedRows[i][11];
            form.DocumentVid.Value = Convert.ToByte(parsedRows[i][12]);
            form.DocumentNumber.Value = parsedRows[i][13];
            form.DocumentDate.Value = parsedRows[i][14];
            form.ProviderOrRecieverOKPO.Value = parsedRows[i][15];
            form.TransporterOKPO.Value = parsedRows[i][16];
            form.PackName.Value = parsedRows[i][17];
            form.PackType.Value = parsedRows[i][18];
            form.PackNumber.Value = parsedRows[i][19];
        }

    }
}