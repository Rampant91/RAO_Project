using Client_App.ViewModels;
using Client_App.Views;
using Models.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.VisualRealization.Long_Visual;
using Models.Interfaces;
using System.Reactive.Linq;
using Client_App.Commands.AsyncCommands.SumRow;

namespace Client_App.Commands.AsyncCommands;

//  Открыть окно редактирования выбранной формы
internal class ChangeFormAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is ObservableCollectionWithItemPropertyChanged<IKey> param && param.First() is { } obj)
        {
            var t = Desktop.MainWindow as MainWindow;
            var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);
            var rep = (Report)obj;
            var tre = ReportsStorage.LocalReports.Reports_Collection
                .FirstOrDefault(i => i.Report_Collection.Contains(rep));
            var numForm = rep.FormNum.Value;
            var frm = new ChangeOrCreateVM(numForm, rep, tre, ReportsStorage.LocalReports);
            switch (numForm)
            {
                case "2.1":
                {
                    Form2_Visual.tmpVM = frm;
                    if (frm.isSum)
                    {
                        //var sumRow = frm.Storage.Rows21.Where(x => x.Sum_DB == true);
                        await new CancelSumRowAsyncCommand(frm).AsyncExecute(null);
                        await new SumRowAsyncCommand(frm).AsyncExecute(null);
                        //var newSumRow = frm.Storage.Rows21.Where(x => x.Sum_DB == true);
                    }

                    break;
                }
                case "2.2":
                {
                    Form2_Visual.tmpVM = frm;
                    if (frm.isSum)
                    {
                        var sumRow = frm.Storage.Rows22
                            .Where(x => x.Sum_DB)
                            .ToList();
                        Dictionary<long, List<string>> dic = new();
                        foreach (var oldR in sumRow)
                        {
                            dic[oldR.NumberInOrder_DB] = new List<string>
                                { oldR.PackQuantity_DB, oldR.VolumeInPack_DB, oldR.MassInPack_DB };
                        }
                        await new CancelSumRowAsyncCommand(frm).AsyncExecute(null);
                        await new SumRowAsyncCommand(frm).AsyncExecute(null);
                        var newSumRow = frm.Storage.Rows22
                            .Where(x => x.Sum_DB)
                            .ToList();

                        foreach (var newR in newSumRow)
                        {
                            var matchDic = dic
                                .Where(oldR => newR.NumberInOrder_DB == oldR.Key)
                                .ToList();
                            foreach (var oldR in matchDic)
                            {
                                newR.PackQuantity_DB = oldR.Value[0];
                                newR.VolumeInPack_DB = oldR.Value[1];
                                newR.MassInPack_DB = oldR.Value[2];
                            }
                        }
                    }
                    break;
                }
            }
            await MainWindowVM.ShowDialog.Handle(frm);
            t.SelectedReports = tmp;
        }
    }
}