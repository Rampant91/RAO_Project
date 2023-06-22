using Client_App.ViewModels;
using Models.Classes;
using Models.Collections;
using Models.Forms.Form2;
using Models.Forms;
using Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace Client_App.Commands.AsyncCommands.SumRow;

//  Группировка по наименованию в формах 2.1 и 2.2
internal class SumRowAsyncCommand : BaseAsyncCommand
{
    private readonly ChangeOrCreateVM _ChangeOrCreateViewModel;
    private Report Storage => _ChangeOrCreateViewModel.Storage;
    private bool IsSum
    {
        set => _ChangeOrCreateViewModel.isSum = value;
    }

    public SumRowAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        _ChangeOrCreateViewModel = changeOrCreateViewModel;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        switch (Storage.FormNum_DB)
        {
            case "2.1":
                await Sum21();
                IsSum = true;
                break;
            case "2.2":
                await Sum22();
                IsSum = true;
                break;
        }
    }

    public async void SumRow(object sender, Avalonia.Interactivity.RoutedEventArgs args)
    {
        await AsyncExecute(null);
    }

    public async Task Sum21()
    {
        var tItems = Storage.Rows21
            .GroupBy(x => x.RefineMachineName_DB
                          + x.MachineCode_DB
                          + x.MachinePower_DB
                          + x.NumberOfHoursPerYear_DB)
            .ToList();
        var emptyItems = tItems
            .Where(item => item.Key == "")
            .ToList();

        var ito = tItems.ToDictionary(item => item.Key, _ => new List<Form21>());

        foreach (var item in emptyItems)
        {
            foreach (var t in item)
            {
                ito[item.Key].Add(t);
            }
            tItems.Remove(item);
        }

        Parallel.ForEach(tItems, itemT =>
        {
            var sumRow = itemT.FirstOrDefault(x => x.Sum_DB);

            if (itemT.Count() > 1 && sumRow == null || itemT.Count() > 2 && sumRow != null)
            {
                if (sumRow == null)
                {
                    var first = itemT.FirstOrDefault();
                    sumRow = (Form21)FormCreator.Create("2.1");

                    sumRow._RefineMachineName_Hidden_Set = false;
                    sumRow._MachineCode_Hidden_Set = false;
                    sumRow._MachinePower_Hidden_Set = false;
                    sumRow._NumberOfHoursPerYear_Hidden_Set = false;

                    sumRow.RefineMachineName_DB = first.RefineMachineName_DB;
                    sumRow.MachineCode_DB = first.MachineCode_DB;
                    sumRow.MachinePower_DB = first.MachinePower_DB;
                    sumRow.NumberOfHoursPerYear_DB = first.NumberOfHoursPerYear_DB;

                    sumRow.CodeRAOIn.Value = "";
                    sumRow.CodeRAOIn_Hidden = true;
                    sumRow.StatusRAOIn.Value = "";
                    sumRow.StatusRAOIn_Hidden = true;
                    sumRow.CodeRAOout.Value = "";
                    sumRow.CodeRAOout_Hidden = true;
                    sumRow.StatusRAOout.Value = "";
                    sumRow.StatusRAOout_Hidden = true;

                    sumRow.Sum_DB = true;
                    sumRow._BaseColor = ColorType.Green;
                }

                sumRow.NumberInOrder_DB = 0;

                double volumeInSum = 0;
                double massInSum = 0;
                double quantityInSum = 0;
                double alphaInSum = 0;
                double betaInSum = 0;
                double tritInSum = 0;
                double transInSum = 0;
                double volumeOutSum = 0;
                double massOutSum = 0;
                double quantityOutSum = 0;
                double alphaOutSum = 0;
                double betaOutSum = 0;
                double tritOutSum = 0;
                double transOutSum = 0;

                List<Form21> lst = new();
                var u = itemT
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var form in u)
                {
                    form.SumGroup_DB = true;

                    form._RefineMachineName_Hidden_Set = false;
                    form._MachineCode_Hidden_Set = false;
                    form._MachinePower_Hidden_Set = false;
                    form._NumberOfHoursPerYear_Hidden_Set = false;

                    //form.RefineMachineName_Hidden_Get.Set(false);
                    //form.MachineCode_Hidden_Get.Set(false);
                    //form.MachinePower_Hidden_Get.Set(false);
                    //form.NumberOfHoursPerYear_Hidden_Get.Set(false);

                    form._BaseColor = ColorType.Yellow;

                    volumeInSum += StringToNumber(form.VolumeIn_DB);
                    massInSum += StringToNumber(form.MassIn_DB);
                    quantityInSum += StringToNumber(form.QuantityIn_DB);
                    alphaInSum += StringToNumber(form.AlphaActivityIn_DB);
                    betaInSum += StringToNumber(form.BetaGammaActivityIn_DB);
                    tritInSum += StringToNumber(form.TritiumActivityIn_DB);
                    transInSum += StringToNumber(form.TransuraniumActivityIn_DB);

                    volumeOutSum += StringToNumber(form.VolumeOut_DB);
                    massOutSum += StringToNumber(form.MassOut_DB);
                    quantityOutSum += StringToNumber(form.QuantityOZIIIout_DB);
                    alphaOutSum += StringToNumber(form.AlphaActivityOut_DB);
                    betaOutSum += StringToNumber(form.BetaGammaActivityOut_DB);
                    tritOutSum += StringToNumber(form.TritiumActivityOut_DB);
                    transOutSum += StringToNumber(form.TransuraniumActivityOut_DB);

                    lst.Add(form);
                }

                sumRow.VolumeIn_DB = volumeInSum >= double.Epsilon ? volumeInSum.ToString("E2") : "-";
                sumRow.MassIn_DB = massInSum >= double.Epsilon ? sumRow.MassIn_DB = massInSum.ToString("E2") : "-";
                sumRow.QuantityIn_DB = quantityInSum >= double.Epsilon ? sumRow.QuantityIn_DB = quantityInSum.ToString("F0") : "-";
                sumRow.AlphaActivityIn_DB = alphaInSum >= double.Epsilon ? sumRow.AlphaActivityIn_DB = alphaInSum.ToString("E2") : "-";
                sumRow.BetaGammaActivityIn_DB = betaInSum >= double.Epsilon ? sumRow.BetaGammaActivityIn_DB = betaInSum.ToString("E2") : "-";
                sumRow.TritiumActivityIn_DB = tritInSum >= double.Epsilon ? sumRow.TritiumActivityIn_DB = tritInSum.ToString("E2") : "-";
                sumRow.TransuraniumActivityIn_DB = transInSum >= double.Epsilon ? sumRow.TransuraniumActivityIn_DB = transInSum.ToString("E2") : "-";
                sumRow.VolumeOut_DB = volumeOutSum >= double.Epsilon ? sumRow.VolumeOut_DB = volumeOutSum.ToString("E2") : "-";
                sumRow.MassOut_DB = massOutSum >= double.Epsilon ? sumRow.MassOut_DB = massOutSum.ToString("E2") : "-";
                sumRow.QuantityOZIIIout_DB = quantityOutSum >= double.Epsilon ? sumRow.QuantityOZIIIout_DB = quantityOutSum.ToString("F0") : "-";
                sumRow.AlphaActivityOut_DB = alphaOutSum >= double.Epsilon ? sumRow.AlphaActivityOut_DB = alphaOutSum.ToString("E2") : "-";
                sumRow.BetaGammaActivityOut_DB = betaOutSum >= double.Epsilon ? sumRow.BetaGammaActivityOut_DB = betaOutSum.ToString("E2") : "-";
                sumRow.TritiumActivityOut_DB = tritOutSum >= double.Epsilon ? sumRow.TritiumActivityOut_DB = tritOutSum.ToString("E2") : "-";
                sumRow.TransuraniumActivityOut_DB = transOutSum >= double.Epsilon ? sumRow.TransuraniumActivityOut_DB = transOutSum.ToString("E2") : "-";

                ito[itemT.Key].Add(sumRow);
                foreach (var r in lst)
                {
                    ito[itemT.Key].Add(r);
                }
            }
            else
            {
                foreach (var t in itemT)
                {
                    if (!t.Sum_DB)
                    {
                        //var form = t;
                        //form.RefineMachineName_Hidden_Get.Set(true);
                        //form.MachineCode_Hidden_Get.Set(true);
                        //form.MachinePower_Hidden_Get.Set(true);
                        //form.NumberOfHoursPerYear_Hidden_Get.Set(true);
                        ito[itemT.Key].Add(t);
                    }
                }
            }
        });
        Storage.Rows21.Clear();
        var yu = ito
            .OrderBy(x => x.Value.Count)
            .ToList();
        var count = new LetterAlgebra("A");
        var lst = new List<Form21>();
        foreach (var item in yu)
        {
            if (item.Value.Count > 1)
            {
                item.Value.First().NumberInOrderSum_DB = count.ToString();
                lst.AddRange(item.Value);
                count++;
            }
            else
            {
                lst.AddRange(item.Value);
            }
        }
        Storage.Rows21.AddRange(lst);
    }

    public async Task Sum22()
    {
        var tItems = Storage.Rows22
            .GroupBy(x => x.StoragePlaceName_DB
                          + x.StoragePlaceCode_DB
                          + x.PackName_DB
                          + x.PackType_DB)
            .ToList();
        var ito = tItems.ToDictionary(item => item.Key, _ => new List<Form22>());
        var emptyItems = tItems
            .Where(item => item.Key == "")
            .ToList();
        foreach (var item in emptyItems)
        {
            foreach (var t in item)
            {
                ito[item.Key].Add(t);
            }
            tItems.Remove(item);
        }
        Parallel.ForEach(tItems, itemT =>
        {
            var sumRow = itemT.FirstOrDefault(x => x.Sum_DB);
            if (itemT.Count() > 1 && sumRow == null || itemT.Count() > 2 && sumRow != null)
            {
                if (sumRow == null)
                {
                    var first = itemT.FirstOrDefault();
                    sumRow = (Form22)FormCreator.Create("2.2");

                    sumRow._StoragePlaceName_Hidden_Set = false;
                    sumRow._StoragePlaceCode_Hidden_Set = false;
                    sumRow._PackName_Hidden_Set = false;
                    sumRow._PackType_Hidden_Set = false;
                    sumRow.StoragePlaceName_DB = first.StoragePlaceName_DB;
                    sumRow.StoragePlaceCode_DB = first.StoragePlaceCode_DB;
                    sumRow.PackName_DB = first.PackName_DB;
                    sumRow.PackType_DB = first.PackType_DB;
                    sumRow.CodeRAO_Hidden = true;
                    sumRow.StatusRAO_Hidden = true;
                    sumRow.MainRadionuclids_Hidden = true;
                    sumRow.Subsidy_Hidden = true;
                    sumRow.FcpNumber_Hidden = true;
                    sumRow.Sum_DB = true;
                    sumRow._BaseColor = ColorType.Green;
                }

                sumRow.NumberInOrder_DB = 0;
                double volumeSum = 0;
                double massSum = 0;
                var quantitySum = 0;
                double alphaSum = 0;
                double betaSum = 0;
                double tritSum = 0;
                double transSum = 0;

                List<Form22> lst = new();
                var u = itemT
                    .OrderBy(x => x.NumberInOrder_DB)
                    .ToList();
                foreach (var form in u)
                {
                    form.VolumeInPack_Hidden = true;
                    form.MassInPack_Hidden = true;

                    form.SumGroup_DB = true;
                    form._StoragePlaceName_Hidden_Set = false;
                    form._StoragePlaceCode_Hidden_Set = false;
                    form._PackName_Hidden_Set = false;
                    form._PackType_Hidden_Set = false;

                    //form.StoragePlaceName_Hidden_Get.Set(false);
                    //form.StoragePlaceCode_Hidden_Get.Set(false);
                    //form.PackName_Hidden_Get.Set(false);
                    //form.PackType_Hidden_Get.Set(false);
                    form._BaseColor = ColorType.Yellow;

                    volumeSum += StringToNumber(form.VolumeOutOfPack_DB);
                    massSum += StringToNumber(form.MassOutOfPack_DB);
                    quantitySum += StringToNumberInt(form.QuantityOZIII_DB);
                    alphaSum += StringToNumber(form.AlphaActivity_DB);
                    betaSum += StringToNumber(form.BetaGammaActivity_DB);
                    tritSum += StringToNumber(form.TritiumActivity_DB);
                    transSum += StringToNumber(form.TransuraniumActivity_DB);

                    lst.Add(form);
                }

                sumRow.VolumeOutOfPack_DB = volumeSum >= double.Epsilon ? volumeSum.ToString("E2") : "-";
                sumRow.MassOutOfPack_DB = massSum >= double.Epsilon ? massSum.ToString("E2") : "-";
                sumRow.QuantityOZIII_DB = quantitySum >= 0 ? quantitySum.ToString() : "-";
                sumRow.AlphaActivity_DB = alphaSum >= double.Epsilon ? alphaSum.ToString("E2") : "-";
                sumRow.BetaGammaActivity_DB = betaSum >= double.Epsilon ? betaSum.ToString("E2") : "-";
                sumRow.TritiumActivity_DB = tritSum >= double.Epsilon ? tritSum.ToString("E2") : "-";
                sumRow.TransuraniumActivity_DB = transSum >= double.Epsilon ? transSum.ToString("E2") : "-";

                ito[itemT.Key].Add(sumRow);
                foreach (var r in lst)
                {
                    ito[itemT.Key].Add(r);
                }
            }
            else
            {
                foreach (var t in itemT)
                {
                    if (!t.Sum_DB)
                    {
                        //var form = t;
                        //form.StoragePlaceName_Hidden_Get.Set(true);
                        //form.StoragePlaceCode_Hidden_Get.Set(true);
                        //form.PackName_Hidden_Get.Set(true);
                        //form.PackType_Hidden_Get.Set(true);

                        //form.StoragePlaceName_Hidden_Set.Set(true);
                        //form.StoragePlaceCode_Hidden_Set.Set(true);
                        //form.PackName_Hidden_Set.Set(true);
                        //form.PackType_Hidden_Set.Set(true);
                        ito[itemT.Key].Add(t);
                    }
                }
            }
        });
        Storage.Rows22.Clear();
        var yu = ito
            .OrderBy(x => x.Value.Count)
            .ToList();
        var count = new LetterAlgebra("A");
        var lst = new List<Form22>();
        foreach (var item in yu)
        {
            if (item.Value.Count > 1)
            {
                item.Value.First().NumberInOrderSum_DB = count.ToString();
                lst.AddRange(item.Value);
                //Storage.Rows22.AddRange(item.Value);
                count++;
            }
            else
            {
                lst.AddRange(item.Value);
                //Storage.Rows22.AddRange(item.Value);
            }
        }
        Storage.Rows22.AddRange(lst);
    }

    private static double StringToNumber(string num)
    {
        if (num != null)
        {
            var tmp = num;
            tmp.Replace(" ", "");
            var len = tmp.Length;
            if (len >= 1)
            {
                if (len > 2)
                {
                    if (tmp[0] == '(' && tmp[len - 1] == ')')
                    {
                        tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
                    }
                }
                if (len == 1)
                {
                    tmp = tmp.Replace("-", "0");
                }

                tmp = tmp.Replace(",", ".");
                try
                {
                    return double.Parse(tmp, NumberStyles.Any, CultureInfo.CreateSpecificCulture("en-GB"));
                }
                catch
                {
                    return 0;
                }
            }
        }
        return 0;
    }

    private static int StringToNumberInt(string num)
    {
        if (num != null)
        {
            var tmp = num;
            tmp.Replace(" ", "");
            var len = tmp.Length;
            if (len >= 1)
            {
                if (len > 2)
                {
                    if (tmp[0] == '(' && tmp[len - 1] == ')')
                    {
                        tmp = tmp.Remove(len - 1, 1).Remove(0, 1);
                    }
                }
                if (len == 1)
                {
                    tmp = tmp.Replace("-", "0");
                }

                tmp = tmp.Replace(",", ".");
                try
                {
                    return int.Parse(tmp, NumberStyles.Any, CultureInfo.CreateSpecificCulture("en-GB"));
                }
                catch
                {
                    return 0;
                }
            }
        }
        return 0;
    }
}