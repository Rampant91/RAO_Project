using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels.Forms;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using Models.Forms.Form1;
using System;
using System.Threading.Tasks;
using Models.Forms;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Вставить значения из буфера обмена
/// Пока что работает только с Form12+
/// </summary>
public class NewPasteRowsAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    //После обновления версии Авалонии нужно будет добавить вставку в формате html
    private Report Storage => formVM.Report;
    private Form SelectedForm => formVM.SelectedForm;
    private string FormType => formVM.FormType;
    public override async Task AsyncExecute(object? parameter)
    {
        if (SelectedForm == null) return;
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
            switch (FormType)
            {
                case "1.1":
                    Form11 form11 = Storage.Rows.Get<Form11>(i + start);
                    form11.OperationCode.Value = parsedRows[i][1];
                    form11.OperationDate.Value = parsedRows[i][2];
                    form11.PassportNumber.Value = parsedRows[i][3];
                    form11.Type.Value = parsedRows[i][4];
                    form11.Radionuclids.Value = parsedRows[i][5];
                    form11.FactoryNumber.Value = parsedRows[i][6];

                    if (parsedRows[i][7] == "")
                        form11.Quantity.Value = null;
                    else
                        form11.Quantity.Value = Convert.ToInt32(parsedRows[i][7]);

                    form11.Activity.Value = parsedRows[i][8];
                    form11.CreatorOKPO.Value = parsedRows[i][9];
                    form11.CreationDate.Value = parsedRows[i][10];

                    if (parsedRows[i][11] == "")
                        form11.Category.Value = null;
                    else
                        form11.Category.Value = Convert.ToInt16(parsedRows[i][11]);

                    if (parsedRows[i][12] == "")
                        form11.SignedServicePeriod.Value = null;
                    else
                        form11.SignedServicePeriod.Value = (float)Convert.ToDouble(parsedRows[i][12]);


                    if (parsedRows[i][13] == "")
                        form11.PropertyCode.Value = null;
                    else
                        form11.PropertyCode.Value = Convert.ToByte(parsedRows[i][13]);

                    form11.Owner.Value = parsedRows[i][14];
                    form11.ProviderOrRecieverOKPO.Value = parsedRows[i][15];
                    form11.TransporterOKPO.Value = parsedRows[i][16];
                    form11.PackName.Value = parsedRows[i][17];
                    form11.PackType.Value = parsedRows[i][18];
                    form11.PackNumber.Value = parsedRows[i][19];
                    break;
                case "1.2":
                    Form12 form12 = Storage.Rows.Get<Form12>(i + start);
                    form12.OperationCode.Value = parsedRows[i][1];
                    form12.OperationDate.Value = parsedRows[i][2];
                    form12.PassportNumber.Value = parsedRows[i][3];
                    form12.NameIOU.Value = parsedRows[i][4];
                    form12.FactoryNumber.Value = parsedRows[i][5];
                    form12.Mass.Value = parsedRows[i][6];
                    form12.CreatorOKPO.Value = parsedRows[i][7];
                    form12.CreationDate.Value = parsedRows[i][8];
                    form12.SignedServicePeriod.Value = parsedRows[i][9];
                    if (parsedRows[i][10] == "")
                        form12.PropertyCode.Value = null;
                    else
                        form12.PropertyCode.Value = Convert.ToByte(parsedRows[i][10]);

                    form12.Owner.Value = parsedRows[i][11];

                    if (parsedRows[i][12] == "")
                        form12.DocumentVid.Value = null;
                    else
                        form12.DocumentVid.Value = Convert.ToByte(parsedRows[i][12]);

                    form12.DocumentNumber.Value = parsedRows[i][13];
                    form12.DocumentDate.Value = parsedRows[i][14];
                    form12.ProviderOrRecieverOKPO.Value = parsedRows[i][15];
                    form12.TransporterOKPO.Value = parsedRows[i][16];
                    form12.PackName.Value = parsedRows[i][17];
                    form12.PackType.Value = parsedRows[i][18];
                    form12.PackNumber.Value = parsedRows[i][19];
                    break;
                case "1.3":
                    Form13 form13 = Storage.Rows.Get<Form13>(i + start);
                    form13.OperationCode.Value = parsedRows[i][1];
                    form13.OperationDate.Value = parsedRows[i][2];
                    form13.PassportNumber.Value = parsedRows[i][3];
                    form13.Type.Value = parsedRows[i][4];
                    form13.Radionuclids.Value = parsedRows[i][5];
                    form13.FactoryNumber.Value = parsedRows[i][6];
                    form13.Activity.Value = parsedRows[i][7];
                    form13.CreatorOKPO.Value = parsedRows[i][8];
                    form13.CreationDate.Value = parsedRows[i][9];

                    if (parsedRows[i][10] == "")
                        form13.AggregateState.Value = null;
                    else
                        form13.AggregateState.Value = Convert.ToByte(parsedRows[i][10]);


                    if (parsedRows[i][11] == "")
                        form13.PropertyCode.Value = null;
                    else
                        form13.PropertyCode.Value = Convert.ToByte(parsedRows[i][11]);

                    form13.Owner.Value = parsedRows[i][12];

                    if (parsedRows[i][13] == "")
                        form13.DocumentVid.Value = null;
                    else
                        form13.DocumentVid.Value = Convert.ToByte(parsedRows[i][13]);

                    form13.DocumentNumber.Value = parsedRows[i][14];
                    form13.DocumentDate.Value = parsedRows[i][15];
                    form13.ProviderOrRecieverOKPO.Value = parsedRows[i][16];
                    form13.TransporterOKPO.Value = parsedRows[i][17];
                    form13.PackName.Value = parsedRows[i][18];
                    form13.PackType.Value = parsedRows[i][19];
                    form13.PackNumber.Value = parsedRows[i][20];
                    break;
                case "1.4":
                    Form14 form14 = Storage.Rows.Get<Form14>(i + start);
                    form14.OperationCode.Value = parsedRows[i][1];
                    form14.OperationDate.Value = parsedRows[i][2];
                    form14.PassportNumber.Value = parsedRows[i][3];
                    form14.Name.Value = parsedRows[i][4];

                    if (parsedRows[i][5] == "")
                        form14.Sort.Value = null;
                    else
                        form14.Sort.Value = Convert.ToByte(parsedRows[i][5]);

                    form14.Radionuclids.Value = parsedRows[i][6];
                    form14.Activity.Value = parsedRows[i][7];
                    form14.ActivityMeasurementDate.Value = parsedRows[i][8];
                    form14.Volume.Value = parsedRows[i][9];

                    form14.Mass.Value = parsedRows[i][10];

                    if (parsedRows[i][11] == "")
                        form14.AggregateState.Value = null;
                    else
                        form14.AggregateState.Value = Convert.ToByte(parsedRows[i][11]);

                    if (parsedRows[i][12] == "")
                        form14.PropertyCode.Value = null;
                    else
                        form14.PropertyCode.Value = Convert.ToByte(parsedRows[i][12]);

                    form14.Owner.Value = parsedRows[i][13];

                    if (parsedRows[i][14] == "")
                        form14.DocumentVid.Value = null;
                    else
                        form14.DocumentVid.Value = Convert.ToByte(parsedRows[i][14]);

                    form14.DocumentNumber.Value = parsedRows[i][15];
                    form14.DocumentDate.Value = parsedRows[i][16];
                    form14.ProviderOrRecieverOKPO.Value = parsedRows[i][17];
                    form14.TransporterOKPO.Value = parsedRows[i][18];
                    form14.PackName.Value = parsedRows[i][19];
                    form14.PackType.Value = parsedRows[i][20];
                    form14.PackNumber.Value = parsedRows[i][21];
                    break;
                case "1.5":
                    Form15 form15 = Storage.Rows.Get<Form15>(i + start);
                    form15.OperationCode.Value = parsedRows[i][1];
                    form15.OperationDate.Value = parsedRows[i][2];
                    form15.PassportNumber.Value = parsedRows[i][3];
                    form15.Type.Value = parsedRows[i][4];
                    form15.Radionuclids.Value = parsedRows[i][5];
                    form15.FactoryNumber.Value = parsedRows[i][6];

                    if (parsedRows[i][7] == "")
                        form15.Quantity.Value = null;
                    else
                        form15.Quantity.Value = Convert.ToByte(parsedRows[i][7]);

                    form15.Activity.Value = parsedRows[i][8];
                    form15.CreationDate.Value = parsedRows[i][9];
                    form15.StatusRAO.Value = parsedRows[i][10];

                    if (parsedRows[i][11] == "")
                        form15.DocumentVid.Value = null;
                    else
                        form15.DocumentVid.Value = Convert.ToByte(parsedRows[i][11]);

                    form15.DocumentNumber.Value = parsedRows[i][12];
                    form15.DocumentDate.Value = parsedRows[i][13];
                    form15.ProviderOrRecieverOKPO.Value = parsedRows[i][14];
                    form15.TransporterOKPO.Value = parsedRows[i][15];
                    form15.PackName.Value = parsedRows[i][16];
                    form15.PackType.Value = parsedRows[i][17];
                    form15.PackNumber.Value = parsedRows[i][18];
                    form15.StoragePlaceName.Value = parsedRows[i][19];
                    form15.StoragePlaceCode.Value = parsedRows[i][20];
                    form15.RefineOrSortRAOCode.Value = parsedRows[i][21];
                    form15.Subsidy.Value = parsedRows[i][22];
                    form15.FcpNumber.Value = parsedRows[i][23];
                    form15.ContractNumber.Value = parsedRows[i][24];
                    break;
                case "1.6":
                    Form16 form16 = Storage.Rows.Get<Form16>(i + start);
                    form16.OperationCode.Value = parsedRows[i][1];
                    form16.OperationDate.Value = parsedRows[i][2];
                    form16.CodeRAO.Value = parsedRows[i][3];
                    form16.StatusRAO.Value = parsedRows[i][4];
                    form16.Volume.Value = parsedRows[i][5];
                    form16.Mass.Value = parsedRows[i][6];
                    form16.QuantityOZIII.Value = parsedRows[i][7];
                    form16.MainRadionuclids.Value = parsedRows[i][8];
                    form16.TritiumActivity.Value = parsedRows[i][9];
                    form16.BetaGammaActivity.Value = parsedRows[i][10];
                    form16.AlphaActivity.Value = parsedRows[i][11];
                    form16.TransuraniumActivity.Value = parsedRows[i][12];
                    form16.ActivityMeasurementDate.Value = parsedRows[i][13];

                    if (parsedRows[i][14] == "")
                        form16.DocumentVid.Value = null;
                    else
                        form16.DocumentVid.Value = Convert.ToByte(parsedRows[i][14]);

                    form16.DocumentNumber.Value = parsedRows[i][15];
                    form16.DocumentDate.Value = parsedRows[i][16];
                    form16.ProviderOrRecieverOKPO.Value = parsedRows[i][17];
                    form16.TransporterOKPO.Value = parsedRows[i][18];
                    form16.StoragePlaceName.Value = parsedRows[i][19];
                    form16.StoragePlaceCode.Value = parsedRows[i][20];
                    form16.RefineOrSortRAOCode.Value = parsedRows[i][21];
                    form16.PackName.Value = parsedRows[i][22];
                    form16.PackType.Value = parsedRows[i][23];
                    form16.PackNumber.Value = parsedRows[i][24];
                    form16.Subsidy.Value = parsedRows[i][25];
                    form16.FcpNumber.Value = parsedRows[i][26];
                    form16.ContractNumber.Value = parsedRows[i][27];
                    break;
                case "1.7":
                    Form17 form17 = Storage.Rows.Get<Form17>(i + start);
                    form17.OperationCode.Value = parsedRows[i][1];
                    form17.OperationDate.Value = parsedRows[i][2];
                    form17.PackName.Value = parsedRows[i][3];
                    form17.PackType.Value = parsedRows[i][4];
                    form17.PackFactoryNumber.Value = parsedRows[i][5];
                    form17.PackNumber.Value = parsedRows[i][6];
                    form17.FormingDate.Value = parsedRows[i][7];
                    form17.PassportNumber.Value = parsedRows[i][8];
                    form17.Volume.Value = parsedRows[i][9];
                    form17.Mass.Value = parsedRows[i][10];
                    form17.Radionuclids.Value = parsedRows[i][11];
                    form17.SpecificActivity.Value = parsedRows[i][12];

                    if (parsedRows[i][13] == "")
                        form17.DocumentVid.Value = null;
                    else
                        form17.DocumentVid.Value = Convert.ToByte(parsedRows[i][13]);

                    form17.DocumentNumber.Value = parsedRows[i][14];

                    form17.DocumentDate.Value = parsedRows[i][15];

                    form17.ProviderOrRecieverOKPO.Value = parsedRows[i][16];
                    form17.TransporterOKPO.Value = parsedRows[i][17];
                    form17.StoragePlaceName.Value = parsedRows[i][18];
                    form17.StoragePlaceCode.Value = parsedRows[i][19];
                    form17.CodeRAO.Value = parsedRows[i][20];
                    form17.StatusRAO.Value = parsedRows[i][21];
                    form17.VolumeOutOfPack.Value = parsedRows[i][22];
                    form17.MassOutOfPack.Value = parsedRows[i][23];
                    form17.Quantity.Value = parsedRows[i][24];
                    form17.TritiumActivity.Value = parsedRows[i][25];
                    form17.BetaGammaActivity.Value = parsedRows[i][26];
                    form17.AlphaActivity.Value = parsedRows[i][27];
                    form17.TransuraniumActivity.Value = parsedRows[i][28];
                    form17.RefineOrSortRAOCode.Value = parsedRows[i][29];
                    form17.Subsidy.Value = parsedRows[i][30];
                    form17.FcpNumber.Value = parsedRows[i][31];
                    form17.ContractNumber.Value = parsedRows[i][32];
                    break;
                case "1.8":
                    Form18 form18 = Storage.Rows.Get<Form18>(i + start);

                    form18.OperationCode.Value = parsedRows[i][1];
                    form18.OperationDate.Value = parsedRows[i][2];
                    form18.IndividualNumberZHRO.Value = parsedRows[i][3];
                    form18.PassportNumber.Value = parsedRows[i][4];
                    form18.Volume6.Value = parsedRows[i][5];
                    form18.Mass7.Value = parsedRows[i][6];
                    form18.SaltConcentration.Value = parsedRows[i][7];
                    form18.Radionuclids.Value = parsedRows[i][8];
                    form18.SpecificActivity.Value = parsedRows[i][9];

                    if (parsedRows[i][10] == "")
                        form18.DocumentVid.Value = null;
                    else
                        form18.DocumentVid.Value = Convert.ToByte(parsedRows[i][10]);

                    form18.DocumentNumber.Value = parsedRows[i][11];

                    form18.DocumentDate.Value = parsedRows[i][12];

                    form18.ProviderOrRecieverOKPO.Value = parsedRows[i][13];
                    form18.TransporterOKPO.Value = parsedRows[i][14];
                    form18.StoragePlaceName.Value = parsedRows[i][15];
                    form18.StoragePlaceCode.Value = parsedRows[i][16];
                    form18.CodeRAO.Value = parsedRows[i][17];
                    form18.StatusRAO.Value = parsedRows[i][18];
                    form18.Volume20.Value = parsedRows[i][19];
                    form18.Mass21.Value = parsedRows[i][20];
                    form18.TritiumActivity.Value = parsedRows[i][21];
                    form18.BetaGammaActivity.Value = parsedRows[i][22];
                    form18.AlphaActivity.Value = parsedRows[i][23];
                    form18.TransuraniumActivity.Value = parsedRows[i][24];
                    form18.RefineOrSortRAOCode.Value = parsedRows[i][25];
                    form18.Subsidy.Value = parsedRows[i][26];
                    form18.FcpNumber.Value = parsedRows[i][27];
                    form18.ContractNumber.Value = parsedRows[i][28];
                    break;
                case "1.9":
                    Form19 form19 = Storage.Rows.Get<Form19>(i + start);
                    form19.OperationCode.Value = parsedRows[i][1];
                    form19.OperationDate.Value = parsedRows[i][2];

                    if (parsedRows[i][3] == "")
                        form19.DocumentVid.Value = null;
                    else
                        form19.DocumentVid.Value = Convert.ToByte(parsedRows[i][3]);

                    form19.DocumentNumber.Value = parsedRows[i][4];

                    form19.DocumentDate.Value = parsedRows[i][5];

                    if (parsedRows[i][6] == "")
                        form19.CodeTypeAccObject.Value = null;
                    else
                        form19.CodeTypeAccObject.Value = Convert.ToInt16(parsedRows[i][6]);

                    form19.Radionuclids.Value = parsedRows[i][7];
                    form19.Activity.Value = parsedRows[i][8];
                    break;
            }
        }
        //Узкоспециализированное решение для корректного вывода пустых дат
        formVM.UpdateFormList();
    }
}