using Models.Attributes;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Скопировать в буфер обмена выделенную строку/ячейки.
/// </summary>
public class CopyRowsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not object[] param || param[0] is not IKeyCollection collection) return;
        var minColumn = Convert.ToInt32(param[1]) + 1;
        var maxColumn = Convert.ToInt32(param[2]) + 1;
        if (minColumn == 1 && param[0] is Form1 or Form2)
        {
            minColumn++;
        }
        Dictionary<long, Dictionary<int, string>> dic = new();
        foreach (var item in collection.GetEnumerable().OrderBy(x => x.Order))
        {
            dic.Add(item.Order, new Dictionary<int, string>());
            var dStructure = (IDataGridColumn)item;
            var findStructure = dStructure.GetColumnStructure();
            var level = findStructure.Level;
            var tre = findStructure.GetLevel(level - 1);
            var props = item
                .GetType()
                .GetProperties()
                .Where(x => x.CustomAttributes
                    .Any(y => y.AttributeType.Name is "FormPropertyAttribute"));
            foreach (var prop in props)
            {
                var attr = (FormPropertyAttribute?)prop.GetCustomAttributes(typeof(FormPropertyAttribute), false).FirstOrDefault();
                if (attr == null) continue;

                byte newNum;
                if (attr.Names.Length <= 1 || attr.Names[0] == "null-1-1" && attr.Names[^1] == "1")
                {
                    _ = byte.TryParse(attr.Number, out newNum);
                }
                else
                {
                    switch (attr.Names.Length)
                    {
                        case 3:
                            _ = byte.TryParse(tre.FirstOrDefault(x => x.name == attr.Names[0])
                                ?.innertCol.FirstOrDefault(x => x.name == attr.Names[1])
                                ?.innertCol[0].name, out newNum);
                            break;
                        case 4:
                            _ = byte.TryParse(tre.FirstOrDefault(x => x.name == attr.Names[0])
                                ?.innertCol.FirstOrDefault(x => x.name == attr.Names[1])
                                ?.innertCol.FirstOrDefault(x => x.name == attr.Names[2])
                                ?.innertCol[0].name, out newNum);
                            break;
                        default:
                            continue;
                    }
                }
                try
                {
                    if (newNum >= minColumn && newNum <= maxColumn)
                    {
                        var midValue = prop.GetMethod?.Invoke(item, null);
                        var value = midValue?.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null);
                        if (value != null)
                        {
                            try
                            {
                                if (dic[item.Order][newNum] == "")
                                {
                                    dic[item.Order][newNum] = value.ToString();
                                }
                            }
                            catch
                            {
                                dic[item.Order].Add(newNum, value.ToString());
                            }
                        }
                        else
                        {
                            dic[item.Order].Add(newNum, "");
                        }
                    }
                }
                catch
                {
                    //ignored
                }
            }
        }
        var textInSelectedCells = "";
        foreach (var item in dic.OrderBy(x => x.Key))
        {
            foreach (var it in item.Value.OrderBy(x => x.Key))
            {
                if (it.Value.Contains('\n') || it.Value.Contains('\r'))
                {
                    textInSelectedCells += $"\"{it.Value}\"\t";
                }
                else
                {
                    textInSelectedCells += $"{it.Value}\t";
                }
            }
            textInSelectedCells = textInSelectedCells.Remove(textInSelectedCells.Length - 1, 1) + "\n";
        }
        textInSelectedCells = textInSelectedCells.Remove(textInSelectedCells.Length - 1, 1);
        if (Application.Current.Clipboard is { } clip)
        {
            var currentClipboard = "";
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (await clip.GetTextAsync() is not null)  //GetTextAsync под Linux может выдавать null
            {
                currentClipboard = await clip.GetTextAsync();
            }
            if (minColumn == maxColumn && !string.IsNullOrEmpty(currentClipboard) && textInSelectedCells.Contains(currentClipboard))
            {
                return;
            }
            if (OperatingSystem.IsWindows())
            {
                var thread = new Thread(() =>
                {
                    clip.ClearAsync();
                    clip.SetTextAsync(textInSelectedCells);
                });
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                thread.Join();
            }
            else
            {
                await clip.ClearAsync();
                await clip.SetTextAsync(textInSelectedCells);
            }
        }
    }
}