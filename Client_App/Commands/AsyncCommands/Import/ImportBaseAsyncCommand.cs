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
using Client_App.ViewModels;
using System.Linq;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.Forms;

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

    private protected DBModel Db = StaticConfiguration.DBModel;
    private protected string TmpImpFilePath = "";

    public string OperationDate => IsFirstLogLine
        ? DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
        : "\t\t";

    #region CheckAnswer

    private protected async Task CheckAnswer(string an, Reports baseReps, Reports impReps, Report? oldReport = null, Report? newReport = null, bool addToDB = true)
    {
        switch (an)
        {
            #region Add

            case "Да" or "Да для всех" or "Добавить":
                ReportsStorage.LocalReports.Reports_Collection.Add(baseReps);
                await CheckTitleFormAsync(baseReps, impReps);
                if (addToDB)
                {
                    baseReps.Report_Collection.Add(newReport);
                    AtLeastOneImportDone = true;
                }
                Act = "\t\t\t";
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

            #endregion

            #region SaveBoth
            
            case "Сохранить оба":
                await CheckTitleFormAsync(baseReps, impReps);
                if (addToDB)
                {
                    baseReps.Report_Collection.Add(newReport);
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

            #endregion

            #region Replace
            
            case "Заменить" or "Заменять все формы":
                await CheckTitleFormAsync(baseReps, impReps);
                baseReps.Report_Collection.Replace(oldReport, newReport);
                StaticConfiguration.DBModel.Remove(oldReport!);
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

            #endregion

            #region Supplement
            
            case "Дополнить" when newReport != null && oldReport != null:
                await CheckTitleFormAsync(baseReps, impReps);
                newReport.Rows.AddRange<IKey>(0, oldReport.Rows.GetEnumerable());
                newReport.Notes.AddRange<IKey>(0, oldReport.Notes);
                baseReps.Report_Collection.Replace(oldReport, newReport);
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

            #endregion

            #region CancelForAll
            
            case "Отменить для всех пересечений":
                SkipInter = true;
                break; 

            #endregion

            #region Cancel

            case "Отменить импорт формы" or "Нет":
                break; 

            #endregion
        }
    }

    #endregion

    #region CheckTitleForm

    private static async Task CheckTitleFormAsync(Reports baseReps, Reports impReps)
    {
        if ((baseReps.Master.Rows10[0].SubjectRF_DB != impReps.Master.Rows10[0].SubjectRF_DB
             || baseReps.Master.Rows10[0].JurLico_DB != impReps.Master.Rows10[0].JurLico_DB
             || baseReps.Master.Rows10[0].ShortJurLico_DB != impReps.Master.Rows10[0].ShortJurLico_DB
             || baseReps.Master.Rows10[0].JurLicoAddress_DB != impReps.Master.Rows10[0].JurLicoAddress_DB
             || baseReps.Master.Rows10[0].JurLicoFactAddress_DB != impReps.Master.Rows10[0].JurLicoFactAddress_DB
             || baseReps.Master.Rows10[0].GradeFIO_DB != impReps.Master.Rows10[0].GradeFIO_DB
             || baseReps.Master.Rows10[0].Telephone_DB != impReps.Master.Rows10[0].Telephone_DB
             || baseReps.Master.Rows10[0].Fax_DB != impReps.Master.Rows10[0].Fax_DB
             || baseReps.Master.Rows10[0].Email_DB != impReps.Master.Rows10[0].Email_DB
             || baseReps.Master.Rows10[0].Okpo_DB != impReps.Master.Rows10[0].Okpo_DB
             || baseReps.Master.Rows10[0].Okved_DB != impReps.Master.Rows10[0].Okved_DB
             || baseReps.Master.Rows10[0].Okogu_DB != impReps.Master.Rows10[0].Okogu_DB
             || baseReps.Master.Rows10[0].Oktmo_DB != impReps.Master.Rows10[0].Oktmo_DB
             || baseReps.Master.Rows10[0].Inn_DB != impReps.Master.Rows10[0].Inn_DB
             || baseReps.Master.Rows10[0].Kpp_DB != impReps.Master.Rows10[0].Kpp_DB
             || baseReps.Master.Rows10[0].Okopf_DB != impReps.Master.Rows10[0].Okopf_DB
             || baseReps.Master.Rows10[0].Okfs_DB != impReps.Master.Rows10[0].Okfs_DB
             || baseReps.Master.Rows10[1].SubjectRF_DB != impReps.Master.Rows10[1].SubjectRF_DB
             || baseReps.Master.Rows10[1].JurLico_DB != impReps.Master.Rows10[1].JurLico_DB
             || baseReps.Master.Rows10[1].ShortJurLico_DB != impReps.Master.Rows10[1].ShortJurLico_DB
             || baseReps.Master.Rows10[1].JurLicoAddress_DB != impReps.Master.Rows10[1].JurLicoAddress_DB
             || baseReps.Master.Rows10[1].JurLicoFactAddress_DB != impReps.Master.Rows10[1].JurLicoFactAddress_DB
             || baseReps.Master.Rows10[1].GradeFIO_DB != impReps.Master.Rows10[1].GradeFIO_DB
             || baseReps.Master.Rows10[1].Telephone_DB != impReps.Master.Rows10[1].Telephone_DB
             || baseReps.Master.Rows10[1].Fax_DB != impReps.Master.Rows10[1].Fax_DB
             || baseReps.Master.Rows10[1].Email_DB != impReps.Master.Rows10[1].Email_DB
             || baseReps.Master.Rows10[1].Okpo_DB != impReps.Master.Rows10[1].Okpo_DB
             || baseReps.Master.Rows10[1].Okved_DB != impReps.Master.Rows10[1].Okved_DB
             || baseReps.Master.Rows10[1].Okogu_DB != impReps.Master.Rows10[1].Okogu_DB
             || baseReps.Master.Rows10[1].Oktmo_DB != impReps.Master.Rows10[1].Oktmo_DB
             || baseReps.Master.Rows10[1].Inn_DB != impReps.Master.Rows10[1].Inn_DB
             || baseReps.Master.Rows10[1].Kpp_DB != impReps.Master.Rows10[1].Kpp_DB
             || baseReps.Master.Rows10[1].Okopf_DB != impReps.Master.Rows10[1].Okopf_DB
             || baseReps.Master.Rows10[1].Okfs_DB != impReps.Master.Rows10[1].Okfs_DB))
        {
            var newTitleRep = await new CompareReportsTitleFormAsyncCommand(baseReps.Master, impReps.Master).AsyncExecute(null);
            baseReps.Master = newTitleRep;
        }
    }

    #endregion

    #region FillEmptyRegNo

    private protected static void FillEmptyRegNo(ref Reports? reps)
    {
        if (reps is null) return;
        if (reps.Master.Rows10.Count >= 2)
        {
            if (reps.Master.Rows10[0].RegNo_DB is "" && reps.Master.Rows10[1].RegNo_DB is not "" && reps.Master.Rows10[0].Okpo_DB is not "")
            {
                reps.Master.Rows10[0].RegNo_DB = reps.Master.Rows10[1].RegNo_DB;
            }
            if (reps.Master.Rows10[1].RegNo_DB is "" && reps.Master.Rows10[0].RegNo_DB is not "" && reps.Master.Rows10[1].Okpo_DB is not "")
            {
                reps.Master.Rows10[1].RegNo_DB = reps.Master.Rows10[0].RegNo_DB;
            }
        }
        if (reps.Master.Rows20.Count >= 2)
        {
            if (reps.Master.Rows20[0].RegNo_DB is "" && reps.Master.Rows20[1].RegNo_DB is not "" && reps.Master.Rows20[0].Okpo_DB is not "")
            {
                reps.Master.Rows20[0].RegNo_DB = reps.Master.Rows20[1].RegNo_DB;
            }
            if (reps.Master.Rows20[1].RegNo_DB is "" && reps.Master.Rows20[0].RegNo_DB is not "" && reps.Master.Rows20[1].Okpo_DB is not "")
            {
                reps.Master.Rows20[1].RegNo_DB = reps.Master.Rows20[0].RegNo_DB;
            }
        }
    }

    #endregion

    #region GetRaoFileName

    private protected static string GetRaoFileName()
    {
        var count = 0;
        string? file;
        do
        {
            file = Path.Combine(BaseVM.TmpDirectory, $"file_imp_{count++}.raodb");
        } while (File.Exists(file));

        return file;
    }

    #endregion

    #region GetReports11FromLocalEqual

    private protected static Reports? GetReports11FromLocalEqual(Reports item)
    {
        try
        {
            //if (!item.Report_Collection.Any(x => x.FormNum_DB[0].Equals('1')) || item.Master_DB.FormNum_DB is not "1.0")
            if (item.Master_DB.FormNum_DB is not "1.0")
            {
                return null;
            }

            return ReportsStorage.LocalReports.Reports_Collection10
                       .FirstOrDefault(t =>

                           // обособленные пусты и в базе и в импорте, то сверяем головное
                           item.Master.Rows10[0].Okpo_DB == t.Master.Rows10[0].Okpo_DB
                           && item.Master.Rows10[0].RegNo_DB == t.Master.Rows10[0].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB == ""
                           && t.Master.Rows10[1].Okpo_DB == ""

                           // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows10[0].Okpo_DB == t.Master.Rows10[0].Okpo_DB
                           && item.Master.Rows10[0].RegNo_DB == t.Master.Rows10[1].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB == ""
                           && t.Master.Rows10[1].Okpo_DB == ""

                           // обособленные не пусты, их и сверяем
                           || item.Master.Rows10[1].Okpo_DB == t.Master.Rows10[1].Okpo_DB
                           && item.Master.Rows10[1].RegNo_DB == t.Master.Rows10[1].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB != ""

                           // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows10[1].Okpo_DB == t.Master.Rows10[1].Okpo_DB
                           && item.Master.Rows10[1].RegNo_DB == t.Master.Rows10[0].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB != ""
                           && t.Master.Rows10[1].RegNo_DB == "")

                   ?? ReportsStorage.LocalReports
                       .Reports_Collection10 // если null, то ищем сбитый окпо (совпадение юр лица с обособленным)
                       .FirstOrDefault(t =>

                           // юр лицо в базе совпадает с обособленным в импорте
                           item.Master.Rows10[1].Okpo_DB != ""
                           && t.Master.Rows10[1].Okpo_DB == ""
                           && item.Master.Rows10[1].Okpo_DB == t.Master.Rows10[0].Okpo_DB
                           && item.Master.Rows10[1].RegNo_DB == t.Master.Rows10[0].RegNo_DB

                           // юр лицо в импорте совпадает с обособленным в базе
                           || item.Master.Rows10[1].Okpo_DB == ""
                           && t.Master.Rows10[1].Okpo_DB != ""
                           && item.Master.Rows10[0].Okpo_DB == t.Master.Rows10[1].Okpo_DB
                           && item.Master.Rows10[0].RegNo_DB == t.Master.Rows10[1].RegNo_DB);
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region GetReports21FromLocalEqual

    private protected static Reports? GetReports21FromLocalEqual(Reports item)
    {
        try
        {
            //if (!item.Report_Collection.Any(x => x.FormNum_DB[0].Equals('2')) || item.Master_DB.FormNum_DB is not "2.0")
            if (item.Master_DB.FormNum_DB is not "2.0")
            {
                return null;
            }

            return ReportsStorage.LocalReports.Reports_Collection20
                       .FirstOrDefault(t =>

                           // обособленные пусты и в базе и в импорте, то сверяем головное
                           item.Master.Rows20[0].Okpo_DB == t.Master.Rows20[0].Okpo_DB
                           && item.Master.Rows20[0].RegNo_DB == t.Master.Rows20[0].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB == ""
                           && t.Master.Rows20[1].Okpo_DB == ""

                           // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows20[0].Okpo_DB == t.Master.Rows20[0].Okpo_DB
                           && item.Master.Rows20[0].RegNo_DB == t.Master.Rows20[1].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB == ""
                           && t.Master.Rows20[1].Okpo_DB == ""

                           // обособленные не пусты, их и сверяем
                           || item.Master.Rows20[1].Okpo_DB == t.Master.Rows20[1].Okpo_DB
                           && item.Master.Rows20[1].RegNo_DB == t.Master.Rows20[1].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB != ""

                           // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows20[1].Okpo_DB == t.Master.Rows20[1].Okpo_DB
                           && item.Master.Rows20[1].RegNo_DB == t.Master.Rows20[0].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB != ""
                           && t.Master.Rows20[1].RegNo_DB == "")

                   ?? ReportsStorage.LocalReports.Reports_Collection20 // если null, то ищем сбитый окпо (совпадение юр лица с обособленным)
                       .FirstOrDefault(t =>

                           // юр лицо в базе совпадает с обособленным в импорте
                           item.Master.Rows20[1].Okpo_DB != ""
                           && t.Master.Rows20[1].Okpo_DB == ""
                           && item.Master.Rows20[1].Okpo_DB == t.Master.Rows20[0].Okpo_DB
                           && item.Master.Rows20[1].RegNo_DB == t.Master.Rows20[0].RegNo_DB

                           // юр лицо в импорте совпадает с обособленным в базе
                           || item.Master.Rows20[1].Okpo_DB == ""
                           && t.Master.Rows20[1].Okpo_DB != ""
                           && item.Master.Rows20[0].Okpo_DB == t.Master.Rows20[1].Okpo_DB
                           && item.Master.Rows20[0].RegNo_DB == t.Master.Rows20[1].RegNo_DB);
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region GetSelectedFilesFromDialog

    private protected static async Task<string[]?> GetSelectedFilesFromDialog(string name, params string[] extensions)
    {
        OpenFileDialog dial = new() { AllowMultiple = true };
        var filter = new FileDialogFilter
        {
            Name = name,
            Extensions = [..extensions]
        };
        dial.Filters = [filter];
        return await dial.ShowAsync(Desktop.MainWindow);
    }

    #endregion

    #region InventoryCheck

    private static string InventoryCheck(Report? rep)
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

    private protected async Task ProcessIfHasReports11(Reports baseReps, Reports impReps, List<Report> impRepList)
    {
        BaseRepsOkpo = baseReps.Master.OkpoRep.Value;
        BaseRepsRegNum = baseReps.Master.RegNoRep.Value;
        BaseRepsShortName = baseReps.Master.ShortJurLicoRep.Value;

        foreach (var impRep in impRepList) //Для каждого импортируемого отчета
        {
            ImpRepFormNum = impRep.FormNum_DB;
            ImpRepCorNum = impRep.CorrectionNumber_DB;
            ImpRepFormCount = impRep.Rows.Count;
            ImpRepStartPeriod = impRep.StartPeriod_DB;
            ImpRepEndPeriod = impRep.EndPeriod_DB;
            ImpRepExpDate = impRep.ExportDate_DB;

            var impInBase = false; //Импортируемая форма заменяет/пересекает имеющуюся в базе
            string? res;
            foreach (var key1 in baseReps.Report_Collection) //Для каждого отчета соответствующей организации в базе ищем совпадение
            {
                var baseRep = (Report)key1;
                BaseRepFormNum = baseRep.FormNum_DB;
                BaseRepCorNum = baseRep.CorrectionNumber_DB;
                BaseRepFormCount = Math.Max(ReportsStorage.GetReportRowsCount(baseRep), baseRep.Rows.Count);
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
                catch (Exception ex)
                {
                    // ignored
                }

                #endregion

                #region SamePeriod

                if (stBase == stImp && endBase == endImp && ImpRepFormNum == BaseRepFormNum)
                {
                    baseRep = await FillReportWithForms(baseReps, baseRep);
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

                        await CheckAnswer(res, baseReps, impReps, baseRep, impRep);
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
                    await CheckAnswer(res, baseReps, impReps, baseRep, impRep);
                    break;

                    #endregion
                }

                #endregion

                #region Intersect

                if (stBase < endImp && endBase > stImp && ImpRepFormNum == BaseRepFormNum)
                {
                    baseRep = await FillReportWithForms(baseReps, baseRep);
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

                    await CheckAnswer(res, baseReps, impReps, null, impRep);
                    break;
                }

                #endregion
            }

            #region TryAddEmptyOrg

            if (impRepList.Count == 0)
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

            await CheckAnswer(res, baseReps, impReps, null, impRep);

            #endregion
        }

        await baseReps.SortAsync().ConfigureAwait(false);
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
                                $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}" +
                                $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}" +
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
                                $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}" +
                                $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion

                    if (res is "Дополнить" or "Заменить")
                    {
                        baseRep = await FillReportWithForms(baseReps, baseRep);
                    }
                    await CheckAnswer(res, baseReps, impReps, baseRep, impRep);
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
                                    $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}" +
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
                                    $"{Environment.NewLine}Количество строк отчета в базе - {BaseRepFormCount}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {ImpRepFormCount}",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(Desktop.MainWindow);

                        #endregion
                    }
                }
                if (res is "Заменить" or "Заменять все формы")
                {
                    baseRep = await FillReportWithForms(baseReps, baseRep);
                }
                await CheckAnswer(res, baseReps, impReps, baseRep, impRep);
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
                                $"{Environment.NewLine}Количество строк - {ImpRepFormCount}" +
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
                                $"{Environment.NewLine}Количество строк - {ImpRepFormCount}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion
                }
            }

            await CheckAnswer(res, baseReps, impReps, null, impRep);

            #endregion
        }

        await baseReps.SortAsync().ConfigureAwait(false);
    }

    #endregion

    #region ProcessIfNoteOrder0

    private protected static void ProcessIfNoteOrder0(Reports item)
    {
        foreach (var key in item.Report_Collection)
        {
            var form = (Report)key;
            foreach (var key1 in form.Notes)
            {
                var note = (Note)key1;
                if (note.Order == 0)
                {
                    note.Order = InitializationAsyncCommand.GetNumberInOrder(form.Notes);
                }
            }
        }
    }

    #endregion

    #region FillReportWithFormsInReports

    private static async Task<Report> FillReportWithForms(Reports baseReps, Report baseRep)
    {
        var checkedRep = StaticConfiguration.DBModel.Set<Report>().Local.FirstOrDefault(entry => entry.Id.Equals(baseRep.Id));
        if (checkedRep != null && (checkedRep.Rows.ToList<Form>().Any(form => form == null) || checkedRep.Rows.Count == 0))
        {
            baseRep = await ReportsStorage.Api.GetAsync(baseRep.Id);
            StaticConfiguration.DBModel.Entry(checkedRep).State = EntityState.Detached;
            StaticConfiguration.DBModel.Set<Report>().Attach(baseRep);
            baseReps.Report_Collection.Replace(checkedRep, baseRep);
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
        return baseRep;
    }

    #endregion
}