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
using Models.DTO;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.Import;

public abstract class ImportBaseAsyncCommand : BaseAsyncCommand
{
    private protected LoggerImportDTO? LoggerImportDTO;

    private protected bool SkipNewOrg;              // Пропустить уведомления о добавлении новой организации
    private protected bool SkipInter;               // Пропускать уведомления и отменять импорт при пересечении дат
    private protected bool SkipLess;                // Пропускать уведомления о том, что номер корректировки у импортируемого отчета меньше
    private protected bool SkipNew;                 // Пропускать уведомления о добавлении новой формы для уже имеющейся в базе организации
    private protected bool SkipReplace;             // Пропускать уведомления о замене форм
    private protected bool HasMultipleReport;       // Имеет множество форм
    private protected bool AtLeastOneImportDone;    // Не отменена хотя бы одна операция импорта файлов/организаций/форм

    private protected bool IsFirstLogLine;          // Это первая строчка в логгере ?
    public int CurrentLogLine;                      // Порядковый номер добавляемой формы в логгере для текущей операции
    public FileInfo? SourceFile;                    // Импортируемый файл
    public string Act = "\t\t\t";                   // Действие с формой для логгера

    public string BaseRepsOkpo = "";
    public string BaseRepsRegNum = "";
    public string BaseRepsShortName = "";
    
    public string BaseRepFormNum = "";
    public string BaseRepStartPeriod = "";
    public string BaseRepEndPeriod = "";
    public string BaseRepExpDate = "";
    public string BaseRepYear = "";
    public byte BaseRepCorNum;
    public int BaseRepFormCount;

    public string ImpRepFormNum = "";
    public string ImpRepStartPeriod = "";
    public string ImpRepEndPeriod = "";
    public string ImpRepExpDate = "";
    public string ImpRepYear = "";
    public byte ImpRepCorNum;
    public int ImpRepFormCount;

    public string OperationDate => IsFirstLogLine
        ? DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
        : "\t\t";

    #region CheckAnswer

    private protected async Task CheckAnswer(string an, Reports first, Report? elem = null, Report? it = null, bool addToDB = true)
    {
        switch (an)
        {
            case "Да" or "Да для всех" or "Добавить":
                if (addToDB)
                {
                    first.Report_Collection.Add(it);
                    AtLeastOneImportDone = true;
                }
                Act = "\t\t\t";
                LoggerImportDTO = new LoggerImportDTO
                    {
                        Act = Act, CorNum = ImpRepCorNum, CurrentLogLine = CurrentLogLine, EndPeriod = ImpRepEndPeriod,
                        FormCount = ImpRepFormCount, FormNum = ImpRepFormNum, StartPeriod = ImpRepStartPeriod,
                        Okpo = BaseRepsOkpo, OperationDate = OperationDate, RegNum = BaseRepsRegNum,
                        ShortName = BaseRepsShortName, SourceFileFullPath = SourceFile!.FullName, Year = ImpRepYear
                    };
                ServiceExtension.LoggerManager.Import(LoggerImportDTO);
                IsFirstLogLine = false;
                CurrentLogLine++;
                break;
            case "Сохранить оба":
                if (addToDB)
                {
                    first.Report_Collection.Add(it);
                    AtLeastOneImportDone = true;
                }
                Act = "Сохранены оба (пересечение)";
                LoggerImportDTO = new LoggerImportDTO
                {
                    Act = Act,
                    CorNum = ImpRepCorNum,
                    CurrentLogLine = CurrentLogLine,
                    EndPeriod = ImpRepEndPeriod,
                    FormCount = ImpRepFormCount,
                    FormNum = ImpRepFormNum,
                    StartPeriod = ImpRepStartPeriod,
                    Okpo = BaseRepsOkpo,
                    OperationDate = OperationDate,
                    RegNum = BaseRepsRegNum,
                    ShortName = BaseRepsShortName,
                    SourceFileFullPath = SourceFile!.FullName,
                    Year = ImpRepYear
                };
                ServiceExtension.LoggerManager.Import(LoggerImportDTO); 
                IsFirstLogLine = false;
                CurrentLogLine++;
                break;
            case "Заменить" or "Заменять все формы":
                first.Report_Collection.Remove(elem);
                first.Report_Collection.Add(it);
                AtLeastOneImportDone = true;
                Act = "Замена (пересечение)\t";
                LoggerImportDTO = new LoggerImportDTO
                {
                    Act = Act,
                    CorNum = ImpRepCorNum,
                    CurrentLogLine = CurrentLogLine,
                    EndPeriod = ImpRepEndPeriod,
                    FormCount = ImpRepFormCount,
                    FormNum = ImpRepFormNum,
                    StartPeriod = ImpRepStartPeriod,
                    Okpo = BaseRepsOkpo,
                    OperationDate = OperationDate,
                    RegNum = BaseRepsRegNum,
                    ShortName = BaseRepsShortName,
                    SourceFileFullPath = SourceFile!.FullName,
                    Year = ImpRepYear
                };
                ServiceExtension.LoggerManager.Import(LoggerImportDTO);
                IsFirstLogLine = false;
                CurrentLogLine++;
                break;
            case "Дополнить" when it != null && elem != null:
                first.Report_Collection.Remove(elem);
                it.Rows.AddRange<IKey>(0, elem.Rows.GetEnumerable());
                it.Notes.AddRange<IKey>(0, elem.Notes);
                first.Report_Collection.Add(it);
                AtLeastOneImportDone = true;
                Act = "Дополнение (совпадение)\t";
                LoggerImportDTO = new LoggerImportDTO
                {
                    Act = Act,
                    CorNum = ImpRepCorNum,
                    CurrentLogLine = CurrentLogLine,
                    EndPeriod = ImpRepEndPeriod,
                    FormCount = ImpRepFormCount,
                    FormNum = ImpRepFormNum,
                    StartPeriod = ImpRepStartPeriod,
                    Okpo = BaseRepsOkpo,
                    OperationDate = OperationDate,
                    RegNum = BaseRepsRegNum,
                    ShortName = BaseRepsShortName,
                    SourceFileFullPath = SourceFile!.FullName,
                    Year = ImpRepYear
                };
                ServiceExtension.LoggerManager.Import(LoggerImportDTO);
                IsFirstLogLine = false;
                CurrentLogLine++;
                break;
            case "Отменить для всех пересечений":
                SkipInter = true;
                break;
            case "Отменить импорт формы":
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
        BaseRepsOkpo = baseReps.Master.OkpoRep.Value;
        BaseRepsRegNum = baseReps.Master.RegNoRep.Value;
        BaseRepsShortName = baseReps.Master.ShortJurLicoRep.Value;

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
            ImpRepFormNum = impRep.FormNum_DB;
            ImpRepCorNum = impRep.CorrectionNumber_DB;
            ImpRepFormCount = impRep.Rows.Count;
            ImpRepStartPeriod = impRep.StartPeriod_DB;
            ImpRepEndPeriod = impRep.EndPeriod_DB;
            ImpRepExpDate = impRep.ExportDate_DB;

            var impInBase = false; //Импортируемая форма заменяет/пересекает имеющуюся в базе
            string? res;
            foreach (var key1 in baseReps.Report_Collection) //Для каждой формы соответствующей организации в базе ищем совпадение
            {
                var baseRep = (Report)key1;
                BaseRepFormNum = baseRep.FormNum_DB;
                BaseRepCorNum = baseRep.CorrectionNumber_DB;
                BaseRepFormCount = baseRep.Rows.Count;
                BaseRepStartPeriod = baseRep.StartPeriod_DB;
                BaseRepEndPeriod = baseRep.EndPeriod_DB;
                BaseRepExpDate = baseRep.ExportDate_DB;
                
                #region Periods

                var stBase = DateTime.Parse(DateTime.Now.ToShortDateString()); //Начало периода у отчета в базе
                var endBase = DateTime.Parse(DateTime.Now.ToShortDateString()); //Конец периода у отчета в базе
                try
                {
                    stBase = DateTime.Parse(BaseRepStartPeriod) > DateTime.Parse(BaseRepEndPeriod)
                        ? DateTime.Parse(BaseRepEndPeriod)
                        : DateTime.Parse(BaseRepStartPeriod);
                    endBase = DateTime.Parse(BaseRepStartPeriod) < DateTime.Parse(BaseRepEndPeriod)
                        ? DateTime.Parse(BaseRepEndPeriod)
                        : DateTime.Parse(BaseRepStartPeriod);
                }
                catch (Exception)
                {
                    // ignored
                }

                var stImp = DateTime.Parse(DateTime.Now.ToShortDateString()); //Начало периода у импортируемого отчета
                var endImp = DateTime.Parse(DateTime.Now.ToShortDateString()); //Конец периода у импортируемого отчета
                try
                {
                    stImp = DateTime.Parse(ImpRepStartPeriod) > DateTime.Parse(ImpRepEndPeriod)
                        ? DateTime.Parse(ImpRepEndPeriod)
                        : DateTime.Parse(ImpRepStartPeriod);
                    endImp = DateTime.Parse(ImpRepStartPeriod) < DateTime.Parse(ImpRepEndPeriod)
                        ? DateTime.Parse(ImpRepEndPeriod)
                        : DateTime.Parse(ImpRepStartPeriod);
                }
                catch (Exception)
                {
                    // ignored
                }

                #endregion

                #region SamePeriod

                if (stBase == stImp && endBase == endImp && ImpRepFormNum == BaseRepFormNum)
                {
                    impInBase = true;

                    #region LessCorrectionNumber

                    if (ImpRepCorNum < BaseRepCorNum)
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
                                    $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                    $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                    $"{Environment.NewLine}Начало отчетного периода - {ImpRepStartPeriod}" +
                                    $"{Environment.NewLine}Конец отчетного периода - {ImpRepEndPeriod}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {BaseRepExpDate}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {ImpRepExpDate}" +
                                    $"{Environment.NewLine}Номер корректировки отчета в базе - {BaseRepCorNum}" +
                                    $"{Environment.NewLine}Номер корректировки импортируемого отчета - {ImpRepCorNum}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}{InventoryCheck(impRep)}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Кнопка \"Пропустить для всех\" позволяет не показывать данное уведомление для всех случаев," +
                                    $"{Environment.NewLine}когда номер корректировки импортируемого отчета меньше, чем у имеющегося в базе.",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(Desktop.MainWindow);

                        #endregion

                        if (res is "Пропустить для всех") SkipLess = true;
                        Act = "не загружен (меньший № корр.)";
                        LoggerImportDTO = new LoggerImportDTO
                        {
                            Act = Act, CorNum = ImpRepCorNum, CurrentLogLine = CurrentLogLine, EndPeriod = ImpRepEndPeriod,
                            FormCount = ImpRepFormCount, FormNum = ImpRepFormNum, StartPeriod = ImpRepStartPeriod,
                            Okpo = BaseRepsOkpo, OperationDate = OperationDate, RegNum = BaseRepsRegNum,
                            ShortName = BaseRepsShortName, SourceFileFullPath = SourceFile!.FullName, Year = ImpRepYear
                        };
                        ServiceExtension.LoggerManager.Import(LoggerImportDTO);
                        IsFirstLogLine = false;
                        break;
                    }

                    #endregion

                    #region SameCorrectionNumber

                    if (ImpRepCorNum == BaseRepCorNum)
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
                                    $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                    $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                    $"{Environment.NewLine}Начало отчетного периода - {ImpRepStartPeriod}" +
                                    $"{Environment.NewLine}Конец отчетного периода - {ImpRepEndPeriod}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {BaseRepExpDate}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {ImpRepExpDate}" +
                                    $"{Environment.NewLine}Номер корректировки - {ImpRepCorNum}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}{InventoryCheck(impRep)}",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(Desktop.MainWindow);

                        #endregion

                        await CheckAnswer(res, baseReps, baseRep, impRep);
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
                                        $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                        $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                        $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                        $"{Environment.NewLine}Начало отчетного периода - {ImpRepStartPeriod}" +
                                        $"{Environment.NewLine}Конец отчетного периода - {ImpRepEndPeriod}" +
                                        $"{Environment.NewLine}Дата выгрузки отчета в базе - {BaseRepExpDate}" +
                                        $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {ImpRepExpDate}" +
                                        $"{Environment.NewLine}Номер корректировки отчета в базе - {BaseRepCorNum}" +
                                        $"{Environment.NewLine}Номер корректировки импортируемого отчета - {ImpRepCorNum}" +
                                        $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}{InventoryCheck(baseRep)}" +
                                        $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}{InventoryCheck(impRep)}" +
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
                                        $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                        $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                        $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                        $"{Environment.NewLine}Начало отчетного периода - {ImpRepStartPeriod}" +
                                        $"{Environment.NewLine}Конец отчетного периода - {ImpRepEndPeriod}" +
                                        $"{Environment.NewLine}Дата выгрузки отчета в базе - {BaseRepExpDate}" +
                                        $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {ImpRepExpDate}" +
                                        $"{Environment.NewLine}Номер корректировки отчета в базе - {BaseRepCorNum}" +
                                        $"{Environment.NewLine}Номер корректировки импортируемого отчета - {ImpRepCorNum}" +
                                        $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}{InventoryCheck(baseRep)}" +
                                        $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}{InventoryCheck(impRep)}",
                                    MinWidth = 400,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                })
                                .ShowDialog(Desktop.MainWindow);

                            #endregion
                        }
                    }

                    await CheckAnswer(res, baseReps, baseRep, impRep);
                    break;

                    #endregion
                }

                #endregion

                #region Intersect

                if (stBase < endImp && endBase > stImp && ImpRepFormNum == BaseRepFormNum)
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
                                $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                $"{Environment.NewLine}Начало периода отчета в базе - {BaseRepStartPeriod}" +
                                $"{Environment.NewLine}Конец периода отчета в базе - {BaseRepEndPeriod}" +
                                $"{Environment.NewLine}Начало периода импортируемого отчета - {ImpRepStartPeriod}" +
                                $"{Environment.NewLine}Конец периода импортируемого отчета - {ImpRepEndPeriod}" +
                                $"{Environment.NewLine}Дата выгрузки отчета в базе - {BaseRepExpDate}" +
                                $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {ImpRepExpDate}" +
                                $"{Environment.NewLine}Номер корректировки отчета в базе - {BaseRepCorNum}" +
                                $"{Environment.NewLine}Номер корректировки импортируемого отчета- {ImpRepCorNum}" +
                                $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}{InventoryCheck(baseRep)}" +
                                $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion

                    await CheckAnswer(res, baseReps, null, impRep);
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
                            $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                            $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                            $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}",
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
                                $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                $"{Environment.NewLine}Начало отчетного периода - {ImpRepStartPeriod}" +
                                $"{Environment.NewLine}Конец отчетного периода - {ImpRepEndPeriod}" +
                                $"{Environment.NewLine}Дата выгрузки - {ImpRepExpDate}" +
                                $"{Environment.NewLine}Номер корректировки - {ImpRepCorNum}" +
                                $"{Environment.NewLine}Количество строк - {ImpRepFormCount}{InventoryCheck(impRep)}" +
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
                                $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                $"{Environment.NewLine}Начало отчетного периода - {ImpRepStartPeriod}" +
                                $"{Environment.NewLine}Конец отчетного периода - {ImpRepEndPeriod}" +
                                $"{Environment.NewLine}Дата выгрузки - {ImpRepExpDate}" +
                                $"{Environment.NewLine}Номер корректировки - {ImpRepCorNum}" +
                                $"{Environment.NewLine}Количество строк - {ImpRepFormCount}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion
                }
            }

            await CheckAnswer(res, baseReps, null, impRep);

            #endregion
        }

        await baseReps.SortAsync();
    }

    #endregion

    #region ProcessIfHasReports21

    private protected async Task ProcessIfHasReports21(Reports baseReps, Reports? impReps = null, Report? impReport = null)
    {
        BaseRepsOkpo = baseReps.Master.OkpoRep.Value;
        BaseRepsRegNum = baseReps.Master.RegNoRep.Value;
        BaseRepsShortName = baseReps.Master.ShortJurLicoRep.Value;

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
            ImpRepFormNum = impRep.FormNum_DB;
            ImpRepCorNum = impRep.CorrectionNumber_DB;
            ImpRepFormCount = impRep.Rows.Count;
            ImpRepExpDate = impRep.ExportDate_DB;
            ImpRepYear = impRep.Year_DB;

            var impInBase = false; //Импортируемая форма заменяет/пересекает имеющуюся в базе
            string? res;
            foreach (var key1 in baseReps.Report_Collection) //Для каждой формы соответствующей организации в базе
            {
                var baseRep = (Report)key1;
                BaseRepFormNum = baseRep.FormNum_DB;
                BaseRepCorNum = baseRep.CorrectionNumber_DB;
                BaseRepFormCount = baseRep.Rows.Count;
                BaseRepExpDate = baseRep.ExportDate_DB;
                BaseRepYear = baseRep.Year_DB;

                if (BaseRepYear != ImpRepYear || ImpRepFormNum != BaseRepFormNum) continue;
                impInBase = true;

                #region LessCorrectionNumber

                if (ImpRepCorNum < BaseRepCorNum)
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
                                $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                $"{Environment.NewLine}Отчетный год - {ImpRepYear}" +
                                $"{Environment.NewLine}Дата выгрузки отчета в базе - {BaseRepExpDate}" +
                                $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {ImpRepExpDate}" +
                                $"{Environment.NewLine}Номер корректировки отчета в базе - {BaseRepCorNum}" +
                                $"{Environment.NewLine}Номер корректировки импортируемого отчета - {ImpRepCorNum}" +
                                $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}{InventoryCheck(baseRep)}" +
                                $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}{InventoryCheck(impRep)}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Кнопка \"Пропустить для всех\" позволяет не показывать данное уведомление для всех случаев," +
                                $"{Environment.NewLine}когда номер корректировки импортируемого отчета меньше, чем у имеющегося в базе.",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion

                    if (res == "Пропустить для всех") SkipLess = true;
                    Act = "не загружен (меньший № корр.)";
                    LoggerImportDTO = new LoggerImportDTO
                    {
                        Act = Act, CorNum = ImpRepCorNum, CurrentLogLine = CurrentLogLine, EndPeriod = ImpRepEndPeriod,
                        FormCount = ImpRepFormCount, FormNum = ImpRepFormNum, StartPeriod = ImpRepStartPeriod,
                        Okpo = BaseRepsOkpo, OperationDate = OperationDate, RegNum = BaseRepsRegNum,
                        ShortName = BaseRepsShortName, SourceFileFullPath = SourceFile!.FullName, Year = ImpRepYear
                    };
                    ServiceExtension.LoggerManager.Import(LoggerImportDTO);
                    IsFirstLogLine = false;
                    break;
                }

                #endregion

                #region SameCorrectionNumber

                if (ImpRepCorNum == BaseRepCorNum)
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
                                $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                $"{Environment.NewLine}Отчетный год - {ImpRepYear}" +
                                $"{Environment.NewLine}Дата выгрузки отчета в базе - {BaseRepExpDate}" +
                                $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {ImpRepExpDate}" +
                                $"{Environment.NewLine}Номер корректировки - {ImpRepCorNum}" +
                                $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}{InventoryCheck(baseRep)}" +
                                $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion

                    await CheckAnswer(res, baseReps, baseRep, impRep);
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
                                    $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                    $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                    $"{Environment.NewLine}Отчетный год - {ImpRepYear}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {BaseRepExpDate}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {ImpRepExpDate}" +
                                    $"{Environment.NewLine}Номер корректировки отчета в базе - {BaseRepCorNum}" +
                                    $"{Environment.NewLine}Номер корректировки импортируемого отчета - {ImpRepCorNum}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}{InventoryCheck(impRep)}" +
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
                                    $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                    $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                    $"{Environment.NewLine}Отчетный год - {ImpRepYear}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {BaseRepExpDate}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {ImpRepExpDate}" +
                                    $"{Environment.NewLine}Номер корректировки отчета в базе - {BaseRepCorNum}" +
                                    $"{Environment.NewLine}Номер корректировки импортируемого отчета - {ImpRepCorNum}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}{InventoryCheck(impRep)}",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(Desktop.MainWindow);

                        #endregion
                    }
                }

                await CheckAnswer(res, baseReps, baseRep, impRep);
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
                            $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                            $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                            $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow);

                #endregion

                Act = "не загружен (имп. орг. без форм)";
                LoggerImportDTO = new LoggerImportDTO
                {
                    Act = Act, CorNum = ImpRepCorNum, CurrentLogLine = CurrentLogLine, EndPeriod = ImpRepEndPeriod,
                    FormCount = ImpRepFormCount, FormNum = ImpRepFormNum, StartPeriod = ImpRepStartPeriod,
                    Okpo = BaseRepsOkpo, OperationDate = OperationDate, RegNum = BaseRepsRegNum,
                    ShortName = BaseRepsShortName, SourceFileFullPath = SourceFile!.FullName, Year = ImpRepYear
                };
                ServiceExtension.LoggerManager.Import(LoggerImportDTO);
                IsFirstLogLine = false;
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
                                $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                $"{Environment.NewLine}Отчетный год - {ImpRepYear}" +
                                $"{Environment.NewLine}Дата выгрузки - {ImpRepExpDate}" +
                                $"{Environment.NewLine}Номер корректировки - {ImpRepCorNum}" +
                                $"{Environment.NewLine}Количество строк - {ImpRepFormCount}{InventoryCheck(impRep)}" +
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
                                $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {ImpRepFormNum}" +
                                $"{Environment.NewLine}Отчетный год - {ImpRepYear}" +
                                $"{Environment.NewLine}Дата выгрузки - {ImpRepExpDate}" +
                                $"{Environment.NewLine}Номер корректировки - {ImpRepCorNum}" +
                                $"{Environment.NewLine}Количество строк - {ImpRepFormCount}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion
                }
            }

            await CheckAnswer(res, baseReps, null, impRep);

            #endregion
        }

        await baseReps.SortAsync();
    }

    #endregion
}