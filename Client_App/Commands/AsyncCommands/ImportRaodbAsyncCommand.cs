using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

//  Импорт -> Из RAODB
internal class ImportRaodbAsyncCommand : BaseImportAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        string[] extensions = { "raodb", "RAODB" };
        var answer = await GetSelectedFilesFromDialog("RAODB", extensions);
        if (answer is null) return;
        skipNewOrg = false;
        skipInter = false;
        skipLess = false;
        skipNew = false;
        skipReplace = false;
        hasMultipleReport = false;

        foreach (var res in answer) //Для каждого импортируемого файла
        {
            if (res == "") continue;
            var file = await GetRaoFileName();
            var sourceFile = new FileInfo(res);
            sourceFile.CopyTo(file, true);
            var reportsCollection = await GetReportsFromDataBase(file);

            if (!hasMultipleReport)
            {
                hasMultipleReport = (reportsCollection.Sum(x => x.Report_Collection.Count) > 1 || answer.Length > 1);
            }

            foreach (var item in reportsCollection) //Для каждой импортируемой организации
            {
                await item.SortAsync();
                await RestoreReportsOrders(item);
                if (item.Master.Rows10.Count != 0)
                {
                    item.Master.Rows10[1].RegNo_DB = item.Master.Rows10[0].RegNo_DB;
                }

                if (item.Master.Rows20.Count != 0)
                {
                    item.Master.Rows20[1].RegNo_DB = item.Master.Rows20[0].RegNo_DB;
                }

                var first11 = await GetReports11FromLocalEqual(item);
                var first21 = await GetReports21FromLocalEqual(item);
                FillEmptyRegNo(ref first11);
                FillEmptyRegNo(ref first21);
                item.CleanIds();
                await ProcessIfNoteOrder0(item);

                if (first11 != null)
                {
                    await ProcessIfHasReports11(first11, item);
                }
                else if (first21 != null)
                {
                    await ProcessIfHasReports21(first21, item);
                }
                else if (first11 == null && first21 == null)
                {
                    #region AddNewOrg

                    var an = "Добавить";
                    if (!skipNewOrg)
                    {
                        if (answer.Length > 1 || reportsCollection.Count > 1)
                        {
                            #region MessageNewOrg

                            an = await MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions = new[]
                                    {
                                        new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                        new ButtonDefinition { Name = "Да для всех" },
                                        new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                    },
                                    ContentTitle = "Импорт из .raodb",
                                    ContentHeader = "Уведомление",
                                    ContentMessage =
                                        $"Будет добавлена новая организация ({item.Master.FormNum_DB}) содержащая {item.Report_Collection.Count} форм отчетности." +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Регистрационный номер - {item.Master.RegNoRep.Value}" +
                                        $"{Environment.NewLine}ОКПО - {item.Master.OkpoRep.Value}" +
                                        $"{Environment.NewLine}Сокращенное наименование - {item.Master.ShortJurLicoRep.Value}" +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Кнопка \"Да для всех\" позволяет без уведомлений " +
                                        $"{Environment.NewLine}импортировать все новые организации.",
                                    MinWidth = 400,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                })
                                .ShowDialog(desktop.MainWindow);

                            #endregion

                            if (an is "Да для всех") skipNewOrg = true;
                        }
                        else
                        {
                            #region MessageNewOrg

                            an = await MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions = new[]
                                    {
                                        new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                        new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                    },
                                    ContentTitle = "Импорт из .raodb",
                                    ContentHeader = "Уведомление",
                                    ContentMessage =
                                        $"Будет добавлена новая организация ({item.Master.FormNum_DB}) содержащая {item.Report_Collection.Count} форм отчетности." +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Регистрационный номер - {item.Master.RegNoRep.Value}" +
                                        $"{Environment.NewLine}ОКПО - {item.Master.OkpoRep.Value}" +
                                        $"{Environment.NewLine}Сокращенное наименование - {item.Master.ShortJurLicoRep.Value}",
                                    MinWidth = 400,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                })
                                .ShowDialog(desktop.MainWindow);

                            #endregion
                        }
                    }

                    if (an is "Добавить" or "Да для всех")
                    {
                        LocalReports.Reports_Collection.Add(item);
                    }

                    #endregion
                }

                switch (item.Master_DB.FormNum_DB)
                {
                    case "1.0":
                        await item.Master_DB.Rows10.QuickSortAsync();
                        break;
                    case "2.0":
                        await item.Master_DB.Rows20.QuickSortAsync();
                        break;
                }
            }
        }

        await LocalReports.Reports_Collection.QuickSortAsync();
        await StaticConfiguration.DBModel.SaveChangesAsync();
    }
}