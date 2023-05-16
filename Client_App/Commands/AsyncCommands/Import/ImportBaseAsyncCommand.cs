using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Client_App.Interfaces.Logger;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.Import;

internal abstract class ImportBaseAsyncCommand : BaseAsyncCommand
{
    private protected bool SkipNewOrg;              // Пропустить уведомления о добавлении новой организации
    private protected bool SkipInter;               // Пропускать уведомления и отменять импорт при пересечении дат
    private protected bool SkipLess;                // Пропускать уведомления о том, что номер корректировки у импортируемого отчета меньше
    private protected bool SkipNew;                 // Пропускать уведомления о добавлении новой формы для уже имеющейся в базе организации
    private protected bool SkipReplace;             // Пропускать уведомления о замене форм
    private protected bool HasMultipleReport;       // Имеет множество форм
    private protected bool IsFirstLogLine = true;   // Это первая строчка в логгере ?
    private protected int CurrentLogLine = 1;       // Порядковый номер добавляемой формы в логгере для текущей операции
    private protected FileInfo? SourceFile;

    private protected string OperationDate => IsFirstLogLine
        ? DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
        : "\t\t";

    #region CheckAanswer

    private async Task ChechAanswer(string an, Reports first, Report? elem = null, Report? it = null, bool doSomething = false)
    {
        switch (an)
        {
            case "Сохранить оба" or "Да" or "Да для всех":
                {
                    if (!doSomething)
                        first.Report_Collection.Add(it);
                    break;
                }
            case "Заменить" or "Заменять все формы" or "Загрузить новую" or "Загрузить новую форму":
                first.Report_Collection.Remove(elem);
                first.Report_Collection.Add(it);
                break;
            case "Дополнить" when it != null && elem != null:
                first.Report_Collection.Remove(elem);
                it.Rows.AddRange<IKey>(0, elem.Rows.GetEnumerable());
                it.Notes.AddRange<IKey>(0, elem.Notes);
                first.Report_Collection.Add(it);
                break;
        }
    }

    #endregion

    #region GetSelectedFilesFromDialog

    private protected async Task<string[]?> GetSelectedFilesFromDialog(string name, params string[] extensions)
    {
        OpenFileDialog dial = new() { AllowMultiple = true };
        var filter = new FileDialogFilter
        {
            Name = name,
            Extensions = new List<string>(extensions)
        };
        dial.Filters = new List<FileDialogFilter> { filter };
        return await dial.ShowAsync(Desktop.MainWindow);
    }

    #endregion

    #region InventoryCheck

    private protected static string InventoryCheck(Report? rep)
    {
        if (rep is null)
        {
            return "";
        }

        var countCode10 = 0;
        foreach (var key in rep.Rows)
        {
            if (key is Form1 { OperationCode_DB: "10" })
            {
                countCode10++;
            }
        }

        return countCode10 == rep.Rows.Count
            ? " (ИНВ)"
            : countCode10 > 0
                ? " (инв)"
                : "";
    }

    #endregion

    #region ProcessIfHasReports11

    private protected async Task ProcessIfHasReports11(Reports baseReps, Reports? impReps = null, Report? impReport = null)
    {
        var baseRepsOkpo = baseReps.Master.OkpoRep.Value;
        var baseRepsRegNum = baseReps.Master.RegNoRep.Value;
        var baseRepsShortName = baseReps.Master.ShortJurLicoRep.Value;

        var listImpRep = new List<Report>();
        if (impReps != null)
        {
            listImpRep.AddRange(impReps.Report_Collection);
        }
        if (impReport != null)
        {
            listImpRep.Add(impReport);
        }
        foreach (var impRep in listImpRep) //Для каждой импортируемой формы
        {
            var impRepFormNum = impRep.FormNum_DB;
            var impRepOkpo = impRep.OkpoRep.Value;
            var impRepCorNum = impRep.CorrectionNumber_DB;
            var impRepFormCount = impRep.Rows.Count;
            var impRepStartPeriod = impRep.StartPeriod_DB;
            var impRepEndPeriod = impRep.EndPeriod_DB;
            var impRepShortName = impRep.ShortJurLicoRep.Value;

            var impInBase = false; //Импортируемая форма заменяет/пересекает имеющуюся в базе
            string? res;
            foreach (var key1 in baseReps.Report_Collection) //Для каждой формы соответствующей организации в базе ищем совпадение
            {
                var baseRep = (Report)key1;
                var baseRepFormNum = baseRep.FormNum_DB;
                var baseRepCorNum = baseRep.CorrectionNumber_DB;
                var baseRepFormCount = baseRep.Rows.Count;
                var baseRepStartPeriod = baseRep.StartPeriod_DB;
                var baseRepEndPeriod = baseRep.EndPeriod_DB;
                
                #region Periods

                var stBase = DateTime.Parse(DateTime.Now.ToShortDateString()); //Начало периода у отчета в базе
                var endBase = DateTime.Parse(DateTime.Now.ToShortDateString()); //Конец периода у отчета в базе
                try
                {
                    stBase = DateTime.Parse(baseRepStartPeriod) > DateTime.Parse(baseRepEndPeriod)
                        ? DateTime.Parse(baseRepEndPeriod)
                        : DateTime.Parse(baseRepStartPeriod);
                    endBase = DateTime.Parse(baseRepStartPeriod) < DateTime.Parse(baseRepEndPeriod)
                        ? DateTime.Parse(baseRepEndPeriod)
                        : DateTime.Parse(baseRepStartPeriod);
                }
                catch (Exception)
                {
                    // ignored
                }

                var stImp = DateTime.Parse(DateTime.Now.ToShortDateString()); //Начало периода у импортируемого отчета
                var endImp = DateTime.Parse(DateTime.Now.ToShortDateString()); //Конец периода у импортируемого отчета
                try
                {
                    stImp = DateTime.Parse(impRepStartPeriod) > DateTime.Parse(impRepEndPeriod)
                        ? DateTime.Parse(impRepEndPeriod)
                        : DateTime.Parse(impRepStartPeriod);
                    endImp = DateTime.Parse(impRepStartPeriod) < DateTime.Parse(impRepEndPeriod)
                        ? DateTime.Parse(impRepEndPeriod)
                        : DateTime.Parse(impRepStartPeriod);
                }
                catch (Exception)
                {
                    // ignored
                }

                #endregion

                #region SamePeriod

                if (stBase == stImp && endBase == endImp && impRepFormNum == baseRepFormNum)
                {
                    impInBase = true;

                    #region LessCorrectionNumber

                    if (impRepCorNum < baseRepCorNum)
                    {
                        if (SkipLess) break;

                        #region MessageImportReportHasLowerCorrectionNumber

                        res = await MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions = new[]
                                {
                                    new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true },
                                    new ButtonDefinition { Name = "Пропустить для всех" }
                                },
                                ContentTitle = "Импорт из .raodb",
                                ContentHeader = "Уведомление",
                                ContentMessage =
                                    "Отчет не будет импортирован, поскольку вы пытаетесь загрузить форму" +
                                    $"{Environment.NewLine}с меньшим номером корректировки, чем у текущего отчета в базе." +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Регистрационный номер - {baseRepsRegNum}" +
                                    $"{Environment.NewLine}ОКПО - {baseRepsOkpo}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {baseRepsShortName}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {impRepFormNum}" +
                                    $"{Environment.NewLine}Начало отчетного периода - {impRepStartPeriod}" +
                                    $"{Environment.NewLine}Конец отчетного периода - {impRepEndPeriod}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRepCorNum}" +
                                    $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRepCorNum}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {baseRepFormCount}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {impRepFormCount}{InventoryCheck(impRep)}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Кнопка \"Пропустить для всех\" позволяет не показывать данное уведомление для всех случаев," +
                                    $"{Environment.NewLine}когда номер корректировки импортируемого отчета меньше, чем у имеющегося в базе.",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(Desktop.MainWindow);

                        #endregion

                        if (res is "Пропустить для всех") SkipLess = true;

                        ServiceExtension.LoggerManager.Import($"{OperationDate}" +
                                                              $"\t{CurrentLogLine++}" +
                                                              $"\t{impRep.RegNoRep.Value}" +
                                                              $"\t{impRepOkpo}" +
                                                              $"\t{impRepFormNum}" +
                                                              $"\t{impRepCorNum}" +
                                                              $"\t{impRepFormCount} зап." +
                                                              "\tне загружен (меньший № корр.)" +
                                                              $"\t{impRepStartPeriod} - {impRepEndPeriod}" +
                                                              $"\t{impRepShortName}" +
                                                              $"\t{SourceFile?.Name}");

                        break;
                    }

                    #endregion

                    #region SameCorrectionNumber

                    if (impRep.CorrectionNumber_DB == baseRep.CorrectionNumber_DB)
                    {
                        #region MessageImportReportHasSamePeriodCorrectionNumberAndExportDate

                        res = await MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions = new[]
                                {
                                    new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                    new ButtonDefinition { Name = "Дополнить" },
                                    new ButtonDefinition { Name = "Сохранить оба" },
                                    new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                },
                                ContentTitle = "Импорт из .raodb",
                                ContentHeader = "Уведомление",
                                ContentMessage =
                                    "Импортируемый отчет имеет тот же период, номер корректировки, что и имеющийся в базе." +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Регистрационный номер - {baseRepsRegNum}" +
                                    $"{Environment.NewLine}ОКПО - {baseRepsOkpo}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {baseRepsShortName}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {impRepFormNum}" +
                                    $"{Environment.NewLine}Начало отчетного периода - {impRepStartPeriod}" +
                                    $"{Environment.NewLine}Конец отчетного периода - {impRepEndPeriod}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Номер корректировки - {impRepCorNum}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {baseRepFormCount}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {impRepFormCount}{InventoryCheck(impRep)}",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(Desktop.MainWindow);

                        #endregion

                        await ChechAanswer(res, baseReps, baseRep, impRep);
                        break;
                    }

                    #endregion

                    #region HigherCorrectionNumber

                    res = "Заменить";
                    if (!SkipReplace)
                    {
                        if (HasMultipleReport)
                        {
                            #region MessageImportReportHasHigherCorrectionNumber

                            res = await MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions = new[]
                                    {
                                        new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                        new ButtonDefinition { Name = "Заменять все формы" },
                                        new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                    },
                                    ContentTitle = "Импорт из .raodb",
                                    ContentHeader = "Уведомление",
                                    ContentMessage =
                                        "Импортируемый отчет имеет больший номер корректировки, чем имеющийся в базе." +
                                        $"{Environment.NewLine}Форма с предыдущим номером корректировки будет безвозвратно удалена." +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Регистрационный номер - {baseRepsRegNum}" +
                                        $"{Environment.NewLine}ОКПО - {baseRepsOkpo}" +
                                        $"{Environment.NewLine}Сокращенное наименование - {baseRepsShortName}" +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Номер формы - {impRepFormNum}" +
                                        $"{Environment.NewLine}Начало отчетного периода - {impRepStartPeriod}" +
                                        $"{Environment.NewLine}Конец отчетного периода - {impRepEndPeriod}" +
                                        $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                        $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                        $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRepCorNum}" +
                                        $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRepCorNum}" +
                                        $"{Environment.NewLine}Количество строк отчета в базе - {baseRepFormCount}{InventoryCheck(baseRep)}" +
                                        $"{Environment.NewLine}Количество строк импортируемого отчета - {impRepFormCount}{InventoryCheck(impRep)}" +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Кнопка \"Заменять все формы\" заменит без уведомлений" +
                                        $"{Environment.NewLine}все формы с меньшим номером корректировки.",
                                    MinWidth = 400,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                })
                                .ShowDialog(Desktop.MainWindow);

                            #endregion

                            if (res is "Заменять все формы") SkipReplace = true;
                        }
                        else
                        {
                            #region MessageImportReportHasHigherCorrectionNumber

                            res = await MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions = new[]
                                    {
                                        new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                        new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                    },
                                    ContentTitle = "Импорт из .raodb",
                                    ContentHeader = "Уведомление",
                                    ContentMessage =
                                        "Импортируемый отчет имеет больший номер корректировки чем имеющийся в базе." +
                                        $"{Environment.NewLine}Форма с предыдущим номером корректировки будет безвозвратно удалена." +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Регистрационный номер - {baseRepsRegNum}" +
                                        $"{Environment.NewLine}ОКПО - {baseRepsOkpo}" +
                                        $"{Environment.NewLine}Сокращенное наименование - {baseRepsShortName}" +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Номер формы - {impRepFormNum}" +
                                        $"{Environment.NewLine}Начало отчетного периода - {impRepStartPeriod}" +
                                        $"{Environment.NewLine}Конец отчетного периода - {impRepEndPeriod}" +
                                        $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                        $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                        $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRepCorNum}" +
                                        $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRepCorNum}" +
                                        $"{Environment.NewLine}Количество строк отчета в базе - {baseRepFormCount}{InventoryCheck(baseRep)}" +
                                        $"{Environment.NewLine}Количество строк импортируемого отчета - {impRepFormCount}{InventoryCheck(impRep)}",
                                    MinWidth = 400,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                })
                                .ShowDialog(Desktop.MainWindow);

                            #endregion
                        }
                    }

                    await ChechAanswer(res, baseReps, baseRep, impRep);
                    break;

                    #endregion
                }

                #endregion

                #region Intersect

                if (stBase < endImp && endBase > stImp && impRepFormNum == baseRepFormNum)
                {
                    impInBase = true;

                    if (SkipInter) break;

                    #region MessagePeriodsIntersect

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Сохранить оба", IsDefault = true },
                                new ButtonDefinition { Name = "Отменить для всех пересечений" },
                                new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Периоды импортируемого и имеющегося в базе отчетов пересекаются, но не совпадают." +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseRepsRegNum}" +
                                $"{Environment.NewLine}ОКПО - {baseRepsOkpo}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseRepsShortName}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRepFormNum}" +
                                $"{Environment.NewLine}Начало периода отчета в базе - {baseRepStartPeriod}" +
                                $"{Environment.NewLine}Конец периода отчета в базе - {baseRepEndPeriod}" +
                                $"{Environment.NewLine}Начало периода импортируемого отчета - {impRepStartPeriod}" +
                                $"{Environment.NewLine}Конец периода импортируемого отчета - {impRepEndPeriod}" +
                                $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRepCorNum}" +
                                $"{Environment.NewLine}Номер корректировки импортируемого отчета- {impRepCorNum}" +
                                $"{Environment.NewLine}Количество строк отчета в базе - {baseRepFormCount}{InventoryCheck(baseRep)}" +
                                $"{Environment.NewLine}Количество строк импортируемого отчета - {impRepFormCount}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion

                    if (res is "Отменить для всех пересечений")
                    {
                        SkipInter = true;
                        break;
                    }

                    if (res is "Отменить импорт формы") break;

                    await ChechAanswer(res, baseReps, null, impRep);
                    break;
                }

                #endregion
            }

            #region TryAddEmptyOrg

            if (impReps?.Report_Collection.Count == 0)
            {
                impInBase = true;

                #region MessageNewReport

                await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions = new[]
                        {
                            new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true }
                        },
                        ContentTitle = "Импорт из .raodb",
                        ContentHeader = "Уведомление",
                        ContentMessage =
                            "Импортируемая организация не содержит отчетов и уже присутствует в базе." +
                            $"{Environment.NewLine}" +
                            $"{Environment.NewLine}Регистрационный номер - {baseRepsRegNum}" +
                            $"{Environment.NewLine}ОКПО - {baseRepsOkpo}" +
                            $"{Environment.NewLine}Сокращенное наименование - {baseRepsShortName}",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow);

                #endregion
            }

            #endregion

            if (impInBase) continue;

            #region AddNewForm

            res = "Да";
            if (!SkipNew)
            {
                if (HasMultipleReport)
                {
                    #region MessageNewReport

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Да для всех" },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Импортировать новый отчет в уже имеющуюся в базе организацию?" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseRepsRegNum}" +
                                $"{Environment.NewLine}ОКПО - {baseRepsOkpo}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseRepsShortName}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRepFormNum}" +
                                $"{Environment.NewLine}Начало отчетного периода - {impRepStartPeriod}" +
                                $"{Environment.NewLine}Конец отчетного периода - {impRepEndPeriod}" +
                                $"{Environment.NewLine}Дата выгрузки - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки - {impRepCorNum}" +
                                $"{Environment.NewLine}Количество строк - {impRepFormCount}{InventoryCheck(impRep)}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Кнопка \"Да для всех\" позволяет без уведомлений импортировать" +
                                $"{Environment.NewLine}все новые формы для уже имеющихся в базе организаций.",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion

                    if (res is "Да для всех") SkipNew = true;
                }
                else
                {
                    #region MessageNewReport

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Импортировать новый отчет в уже имеющуюся в базе организацию?" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseRepsRegNum}" +
                                $"{Environment.NewLine}ОКПО - {baseRepsOkpo}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseRepsShortName}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRepFormNum}" +
                                $"{Environment.NewLine}Начало отчетного периода - {impRepStartPeriod}" +
                                $"{Environment.NewLine}Конец отчетного периода - {impRepEndPeriod}" +
                                $"{Environment.NewLine}Дата выгрузки - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки - {impRepCorNum}" +
                                $"{Environment.NewLine}Количество строк - {impRepFormCount}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion
                }
            }

            await ChechAanswer(res, baseReps, null, impRep);

            #endregion
        }

        await baseReps.SortAsync();
    }

    #endregion

    #region ProcessIfHasReports21

    private protected async Task ProcessIfHasReports21(Reports baseReps, Reports? impReps = null, Report? impReport = null)
    {
        var listImpRep = new List<Report>();
        if (impReps != null)
        {
            listImpRep.AddRange(impReps.Report_Collection);
        }
        if (impReport != null)
        {
            listImpRep.Add(impReport);
        }
        foreach (var key in listImpRep) //Для каждой импортируемой формы
        {
            var impRep = key;
            var impInBase = false; //Импортируемая форма заменяет/пересекает имеющуюся в базе
            string? res;
            foreach (var key1 in baseReps.Report_Collection) //Для каждой формы соответствующей организации в базе
            {
                var baseRep = (Report)key1;
                if (baseRep.Year_DB != impRep.Year_DB || impRep.FormNum_DB != baseRep.FormNum_DB) continue;
                impInBase = true;

                #region LessCorrectionNumber

                if (impRep.CorrectionNumber_DB < baseRep.CorrectionNumber_DB)
                {
                    if (SkipLess) break;

                    #region MessageImportReportHasLowerCorrectionNumber

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true },
                                new ButtonDefinition { Name = "Пропустить для всех" }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Отчет не будет импортирован, поскольку вы пытаетесь загрузить форму" +
                                $"{Environment.NewLine}с меньшим номером корректировки, чем у текущего отчета в базе." +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Кнопка \"Пропустить для всех\" позволяет не показывать данное уведомление для всех случаев," +
                                $"{Environment.NewLine}когда номер корректировки импортируемого отчета меньше, чем у имеющегося в базе.",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion

                    if (res == "Пропустить для всех") SkipLess = true;
                    break;
                }

                #endregion

                #region SameCorrectionNumber

                if (impRep.CorrectionNumber_DB == baseRep.CorrectionNumber_DB)
                {
                    #region MessageImportReportHasSameYearCorrectionNumberAndExportDate

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                new ButtonDefinition { Name = "Дополнить" },
                                new ButtonDefinition { Name = "Сохранить оба" },
                                new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Импортируемый отчет имеет тот же год и номер корректировки, что и имеющийся в базе." +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки - {impRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion

                    await ChechAanswer(res, baseReps, baseRep, impRep);
                    break;
                }

                #endregion

                #region HigherCorrectionNumber

                res = "Заменить";
                if (!SkipReplace)
                {
                    if (HasMultipleReport)
                    {
                        #region MessageImportReportHasHigherCorrectionNumber

                        res = await MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions = new[]
                                {
                                    new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                    new ButtonDefinition { Name = "Заменять все формы" },
                                    new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                },
                                ContentTitle = "Импорт из .raodb",
                                ContentHeader = "Уведомление",
                                ContentMessage =
                                    "Импортируемый отчет имеет больший номер корректировки, чем имеющийся в базе." +
                                    $"{Environment.NewLine}Форма с предыдущим номером корректировки будет безвозвратно удалена." +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                    $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                    $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRep.CorrectionNumber_DB}" +
                                    $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRep.CorrectionNumber_DB}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Кнопка \"Заменять все формы\" заменит без уведомлений" +
                                    $"{Environment.NewLine}все формы с меньшим номером корректировки.",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(Desktop.MainWindow);

                        #endregion

                        if (res is "Заменять все формы") SkipReplace = true;
                    }
                    else
                    {
                        #region MessageImportReportHasHigherCorrectionNumber

                        res = await MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions = new[]
                                {
                                    new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                    new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                },
                                ContentTitle = "Импорт из .raodb",
                                ContentHeader = "Уведомление",
                                ContentMessage =
                                    "Импортируемый отчет имеет больший номер корректировки, чем имеющийся в базе." +
                                    $"{Environment.NewLine}Форма с предыдущим номером корректировки будет безвозвратно удалена." +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                    $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                    $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRep.CorrectionNumber_DB}" +
                                    $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRep.CorrectionNumber_DB}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(Desktop.MainWindow);

                        #endregion
                    }

                    if (res is "Отменить импорт формы") break;
                }

                await ChechAanswer(res, baseReps, baseRep, impRep);
                break;

                #endregion
            }

            #region TryAddEmptyOrg

            if (impReps?.Report_Collection.Count == 0)
            {
                impInBase = true;

                #region MessageNewReport

                await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions = new[]
                        {
                            new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true }
                        },
                        ContentTitle = "Импорт из .raodb",
                        ContentHeader = "Уведомление",
                        ContentMessage =
                            "Импортируемая организация не содержит отчетов и уже присутствует в базе." +
                            $"{Environment.NewLine}" +
                            $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                            $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                            $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow);

                #endregion
            }

            #endregion

            if (impInBase) continue;

            #region AddNewForm

            res = "Да";
            if (!SkipNew)
            {
                if (HasMultipleReport)
                {
                    #region MessageNewReport

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Да для всех" },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Импортировать новый отчет в уже имеющуюся в базе организацию?" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                $"{Environment.NewLine}Дата выгрузки - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки - {impRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Количество строк - {impRep.Rows.Count}{InventoryCheck(impRep)}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Кнопка \"Да для всех\" позволяет без уведомлений импортировать" +
                                $"{Environment.NewLine}все новые формы для уже имеющихся в базе организаций.",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion

                    if (res == "Да для всех") SkipNew = true;
                }
                else
                {
                    #region MessageNewReport

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Импортировать новый отчет в уже имеющуюся в базе организацию?" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                $"{Environment.NewLine}Дата выгрузки - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки - {impRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Количество строк - {impRep.Rows.Count}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion
                }
            }

            await ChechAanswer(res, baseReps, null, impRep);

            #endregion
        }

        await baseReps.SortAsync();
    }

    #endregion
}