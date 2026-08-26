using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels.Forms;
using DynamicData.Kernel;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form4;
using Models.Forms.Form5;
using Models.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Вставить значения из буфера обмена.
/// После обновления версии Avalonia нужно будет добавить вставку в формате html
/// </summary>
public class NewPasteRowsAsyncCommand : BaseAsyncCommand
{
    public NewPasteRowsAsyncCommand(IEnumerable<ICopiable> collection)
    {
        CopiableCollection = collection;
    }
    public IEnumerable<ICopiable> CopiableCollection;

    public List<ICopiable> CopiableList
    {
        get => CopiableCollection.AsList();
    }



    //При копировании из Excel, ячейки с символами \n и \t заворачиваются в кавычки
    //Так как в этих скобках могут быть \t, то эти ячейки будут дробиться при сплите
    //Этот метод не может обрабатывать все возможные случаи,
    //Например ячейка   ("Ячейка1"  - все еще ячейка1)
    //в формате TSV будет выглядеть так (\t\"\"\"Ячейка1\"\"\t- все еще ячейка1\"\t)
    //поэтому после обновления Авалонии необходимо переписать всю команду вставки
    private string CutTabulationInCells(string row)
    {
        //Начало раздробленной ячейки
        if (row.Contains("\t\""))
        {
            row = row.Replace("\t\"", "\t%border%start/");
        }
        //Конец раздробленной ячейки
        if (row.Contains("\"\t"))
        {
            row = row.Replace("\"\t", "/end%border%\t");
        }
        if ((row.Contains("%border%start/")) || (row.Contains("/end%border%")))
        {
            var splitedRow = row.Split("%border%");
            row = "";
            for (int i = 0; i < splitedRow.Length; i++)
            {
                if ((splitedRow[i].StartsWith("start/")) && (splitedRow[i].EndsWith("/end")))
                {
                    splitedRow[i] = splitedRow[i].Replace("\t", "");
                }
                row += splitedRow[i];
            }
            if (row.Contains("start/"))
            {
                row = row.Replace("start/", "");
            }
            if (row.Contains("/end"))
            {
                row = row.Replace("/end", "");
            }
        }

        return row;
    }
    private string[] PrepareRowsForParsing(string[] rows)
    {
        for (var i = 0; i < rows.Length; i++)
        {

            //Вырезаем из строк лишние \n
            if (rows[i].Contains('\n'))
            {
                rows[i] = rows[i].Replace('\n', ' ');
            }

            rows[i] = CutTabulationInCells(rows[i]);

            //Excel экранирует обычные кавычки другими кавычками
            if (rows[i].Contains("\"\""))
            {
                rows[i] = rows[i].Replace("\"\"", "\"");
            }
        }
        return rows;
    }


    public override async Task AsyncExecute(object? parameter)
    {
        if (CopiableList is null) return;

        ICopiable copiable;
        if (parameter is ICopiable)
            copiable = parameter as ICopiable;
        else if (parameter is IEnumerable<ICopiable> selectedCopiables)
            copiable = selectedCopiables.First();
        else
            return;

        var start = CopiableList.IndexOf(copiable);
        if (start < 0) return;

        var clipboard = Application.Current!.Clipboard;

        var pastedString = await clipboard.GetTextAsync();
        if (string.IsNullOrEmpty(pastedString)) return;

        var parsedRows = ICopiable.ParseTSVstring(pastedString);

        if (start + parsedRows.Length > CopiableList.Count())
        {
            #region NotEnoughSpaceMessage

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                            ContentTitle = "Вставка данных из буфера обмена",
                            ContentHeader = "Внимание",
                            ContentMessage = "В таблице не хватает места для некоторых строк, которые вы хотите вставить.",
                            MinWidth = 400,
                            MinHeight = 150,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner,
                            Topmost = true,
                        })
                        .ShowDialog(Desktop.MainWindow));

            #endregion
        }

        for (var i = 0; i < parsedRows.Length && i + start < CopiableList.Count(); i++)
        {
            var item = CopiableList[i + start];
            item.PasteParsedTSVstring(parsedRows[i]);

            

        }
    }
}