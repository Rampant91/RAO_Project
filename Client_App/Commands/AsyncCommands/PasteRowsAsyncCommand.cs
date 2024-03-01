using Models.Attributes;
using Models.Forms.DataAccess;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Forms;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;

namespace Client_App.Commands.AsyncCommands;

//  Вставить значения из буфера обмена
internal class PasteRowsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var param = parameter as object[];
        if (param[0] is null) return;
        var collection = param[0] as IKeyCollection;
        var minColumn = Convert.ToInt32(param[1]) + 1;
        var maxColumn = Convert.ToInt32(param[2]) + 1;
        var collectionEn = collection.GetEnumerable().ToList();
        if (minColumn == 1 && collectionEn.FirstOrDefault() is Form1 or Form2)
        {
            minColumn++;
        }

        if (Application.Current.Clipboard is not { } clip) return;

        var text = await clip.GetTextAsync();
        var rowsText = ParseInnerTextRows(text);

        foreach (var item in collectionEn.OrderBy(x => x.Order))
        {
            var props = item.GetType().GetProperties();
            var rowText = rowsText[item.Order - collectionEn.Min(x => x.Order)];
            if (Convert.ToInt32(param[1]) == 0 && collectionEn.FirstOrDefault() is not Note)
            {
                var newText = rowText.ToArray();
                var count = 0;
                foreach (var t in newText)
                {
                    count++;
                    if (t.Equals('\t'))
                    {
                        break;
                    }
                }

                rowText = rowText.Remove(0, count);
            }

            var columnsText = ParseInnerTextColumn(rowText);
            var dStructure = (IDataGridColumn)item;
            var findStructure = dStructure.GetColumnStructure();
            var level = findStructure.Level;
            var tre = findStructure.GetLevel(level - 1);

            //if (maxColumn-1 != columnsText.Length) 
            //{
            //    columnsText = columnsText[1..columnsText.Length];
            //}

            foreach (var prop in props)
            {
                var attr = (FormPropertyAttribute)prop
                    .GetCustomAttributes(typeof(FormPropertyAttribute), false)
                    .FirstOrDefault();
                if (attr == null) continue;
                try
                {
                    int columnNum;
                    if (attr.Names.Length > 1 && attr.Names.Length != 4 &&
                        attr.Names[0] is not ("null-1-1" or "Документ" or "Сведения об операции"))
                    {
                        columnNum = Convert.ToInt32(tre.FirstOrDefault(x => x.name == attr.Names[0])?.innertCol
                            .FirstOrDefault(x => x.name == attr.Names[1])?.innertCol[0].name);
                    }
                    else if (attr.Names[0] == "Документ")
                    {
                        var findDock = tre.Where(x => x.name == "null-n").ToList();
                        columnNum = Convert.ToInt32(findDock.Any()
                            ? findDock.FirstOrDefault()?.innertCol.FirstOrDefault(x => x.name == attr.Names[0])
                                ?.innertCol.FirstOrDefault(x => x.name == attr.Names[1])?.innertCol[0].name
                            : tre.FirstOrDefault(x => x.name == attr.Names[0])?.innertCol
                                .FirstOrDefault(x => x.name == attr.Names[1])?.innertCol[0].name);
                    }
                    else
                    {
                        columnNum = Convert.ToInt32(attr.Number);
                    }

                    //var columnNum = Convert.ToInt32(attr.Number);
                    if (columnNum >= minColumn && columnNum <= maxColumn)
                    {
                        var midValue = prop.GetMethod?.Invoke(item, null);
                        switch (midValue)
                        {
                            case RamAccess<int?>:
                                midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue,
                                    new object[] { int.Parse(columnsText[columnNum - minColumn]) });
                                break;
                            case RamAccess<float?>:
                                midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue,
                                    new object[] { float.Parse(columnsText[columnNum - minColumn]) });
                                break;
                            case RamAccess<short>:
                            case RamAccess<short?>:
                                midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue,
                                    new object[] { short.Parse(columnsText[columnNum - minColumn]) });
                                break;
                            case RamAccess<int>:
                                midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue,
                                    new object[] { int.Parse(columnsText[columnNum - minColumn]) });
                                break;
                            case RamAccess<string>:
                                midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue,
                                    new object[] { columnsText[columnNum - minColumn] });
                                break;
                            case RamAccess<byte?>:
                                midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue,
                                    new object[] { byte.Parse(columnsText[columnNum - minColumn]) });
                                break;
                            case RamAccess<bool>:
                                midValue.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue,
                                    new object[] { bool.Parse(columnsText[columnNum - minColumn]) });
                                break;
                            default:
                                midValue?.GetType().GetProperty("Value")?.SetMethod?.Invoke(midValue,
                                    new object[] { columnsText[columnNum - minColumn] });
                                break;
                        }
                    }
                }
                catch (Exception) { }
            }
        }
    }

    private static string[] ParseInnerTextRows(string text)
    {
        List<string> lst = new();
        var comaFlag = false;
        text = text.Replace("\r\n", "\n");
        var txt = "";
        foreach (var item in text)
        {
            switch (item)
            {
                case '\"':
                    txt += item;
                    comaFlag = !comaFlag;
                    break;
                //||(item=='\t'))
                case '\n' when !comaFlag:
                    lst.Add(txt);
                    txt = "";
                    break;
                case '\n':
                    txt += item;
                    break;
                default:
                    txt += item;
                    break;
            }
        }
        if (txt != "")
        {
            lst.Add(txt);
        }
        lst.Add("");
        return lst.ToArray();
    }

    private static string[] ParseInnerTextColumn(string text)
    {
        List<string> lst = new();
        var comaFlag = false;
        var txt = "";
        foreach (var item in text)
        {
            switch (item)
            {
                case '\"':
                    comaFlag = true;
                    break;
                case '\t' when !comaFlag:
                    lst.Add(txt);
                    txt = "";
                    break;
                case '\t':
                    txt += item;
                    break;
                default:
                    txt += item;
                    break;
            }
        }
        if (txt != "")
        {
            lst.Add(txt);
        }
        return lst.ToArray();
    }
}