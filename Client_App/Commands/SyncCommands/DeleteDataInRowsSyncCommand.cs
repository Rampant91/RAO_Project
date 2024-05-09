using Models.Attributes;
using Models.Forms.DataAccess;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Interfaces;
using System;
using System.Linq;

namespace Client_App.Commands.SyncCommands;

//  Удалить данные в выделенных ячейках
internal class DeleteDataInRowsSyncCommand : BaseCommand
{
    public override bool CanExecute(object? parameter)
    {
        return true;
    }

    public override void Execute(object? parameter)
    {
        var param = parameter as object[];
        if (param[0] is null) return;
        var collection = param[0] as IKeyCollection;
        var minColumn = Convert.ToInt32(param[1]) + 1;
        var maxColumn = Convert.ToInt32(param[2]) + 1;
        if (minColumn == 1 && collection.GetEnumerable().FirstOrDefault() is Form1 or Form2)
        {
            minColumn++;
        }
        var orderedCollection = collection
            .GetEnumerable()
            .OrderBy(x => x.Order)
            .ToList();
        foreach (var item in orderedCollection)
        {
            var props = item.GetType().GetProperties();
            var dStructure = (IDataGridColumn)item;
            var findStructure = dStructure.GetColumnStructure();
            var level = findStructure.Level;
            var tre = findStructure.GetLevel(level - 1);
            foreach (var prop in props)
            {
                var attr = (FormPropertyAttribute?)prop
                    .GetCustomAttributes(typeof(FormPropertyAttribute), false)
                    .FirstOrDefault();
                if (attr is null) continue;
                try
                {
                    var columnNum = attr.Names.Length > 1 && attr.Names[0] != "null-1-1"
                        ? Convert.ToInt32(tre.FirstOrDefault(x => x.name == attr.Names[0])?.innertCol
                            .FirstOrDefault(x => x.name == attr.Names[1])?.innertCol[0].name)
                        : Convert.ToInt32(attr.Number);
                    if (columnNum >= minColumn && columnNum <= maxColumn)
                    {
                        var midValue = prop.GetMethod?.Invoke(item, null);
                        switch (midValue)
                        {
                            case RamAccess<int?>:
                            case RamAccess<float?>:
                            case RamAccess<short?>:
                            case RamAccess<byte?>:
                            case RamAccess<bool>:
                                midValue.GetType().GetProperty("Value")?.SetMethod
                                    ?.Invoke(midValue, new object?[1]);
                                break;
                            case RamAccess<short>:
                                midValue.GetType().GetProperty("Value")?.SetMethod
                                    ?.Invoke(midValue, [short.Parse("")]);
                                break;
                            case RamAccess<int>:
                                midValue.GetType().GetProperty("Value")?.SetMethod
                                    ?.Invoke(midValue, [int.Parse("")]);
                                break;
                            case RamAccess<string>:
                                midValue.GetType().GetProperty("Value")?.SetMethod
                                    ?.Invoke(midValue, [""]);
                                break;
                            default:
                                midValue?.GetType().GetProperty("Value")?.SetMethod
                                    ?.Invoke(midValue, [""]);
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}