using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Controls.DataGrid.DataGrids;
using Client_App.Interfaces.Logger;
using Client_App.Logging;
using Client_App.Resources.CustomComparers;
using DynamicData;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.DTO;
using Models.Forms;
using Models.Forms.Form1;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Import;

/// <summary>
/// Базовый класс импорта.
/// </summary>
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
    private protected bool ExcelImportNewReps;

    private protected bool IsFirstLogLine;          // Это первая строчка в логгере ?
    public int CurrentLogLine;                      // Порядковый номер добавляемой формы в логгере для текущей операции
    public FileInfo? SourceFile;                    // Импортируемый файл
    public string Act = "\t\t\t";                   // Действие с формой для логгера

    protected readonly List<(string, string)> RepsWhereTitleFormCheckIsCancel = [];
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

    private protected string TmpImpFilePath = "";

    public string OperationDate => IsFirstLogLine
        ? DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
        : "\t\t";

    #region CheckAnswer

    /// <summary>
    /// Проверка ответа и изменение данных в БД + логирование.
    /// </summary>
    /// <param name="an">Ответ пользователя.</param>
    /// <param name="baseReps">Организация в базе.</param>
    /// <param name="impReps">Импортируемая организация.</param>
    /// <param name="oldReport">Старый отчёт.</param>
    /// <param name="newReport">Новый отчёт.</param>
    /// <param name="addToDB">Флаг добавления в БД.</param>
    /// <returns></returns>
    private protected async Task CheckAnswer(string an, Reports baseReps, Reports impReps, Report? oldReport = null,
        Report? newReport = null, bool addToDB = true)
    {
        switch (an)
        {
            #region Add

            case "Да" or "Да для всех" or "Добавить":
                if (ExcelImportNewReps) ReportsStorage.LocalReports.Reports_Collection.Add(baseReps);
                if (!RepsWhereTitleFormCheckIsCancel.Contains((BaseRepsRegNum, BaseRepsOkpo)))
                {
                    await CheckTitleFormAsync(baseReps, impReps, RepsWhereTitleFormCheckIsCancel);
                }

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
                if (!RepsWhereTitleFormCheckIsCancel.Contains((BaseRepsRegNum, BaseRepsOkpo)))
                {
                    await CheckTitleFormAsync(baseReps, impReps, RepsWhereTitleFormCheckIsCancel);
                }

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

            #region Ok

            case "Заменить" or "Заменять все формы":
                if (!RepsWhereTitleFormCheckIsCancel.Contains((BaseRepsRegNum, BaseRepsOkpo)))
                {
                    await CheckTitleFormAsync(baseReps, impReps, RepsWhereTitleFormCheckIsCancel);
                }

                baseReps.Report_Collection.Replace(oldReport, newReport);
                StaticConfiguration.DBModel.Remove(oldReport!);
                await ReportDeletionLogger.LogDeletionAsync(oldReport!);
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
                if (!RepsWhereTitleFormCheckIsCancel.Contains((BaseRepsRegNum, BaseRepsOkpo)))
                {
                    await CheckTitleFormAsync(baseReps, impReps, RepsWhereTitleFormCheckIsCancel);
                }

                newReport.Rows.AddRange<IKey>(0, oldReport.Rows.GetEnumerable());
                newReport.Notes.AddRange<IKey>(0, oldReport.Notes);
                baseReps.Report_Collection.Replace(oldReport, newReport);
                StaticConfiguration.DBModel.Remove(oldReport);
                await ReportDeletionLogger.LogDeletionAsync(oldReport);
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

    /// <summary>
    /// Сравнение двух форм организации.
    /// </summary>
    /// <param name="baseReps">Форма организации в БД.</param>
    /// <param name="impReps">Импортируемая форма организации.</param>
    /// <param name="repsWhereTitleFormCheckIsCancel">
    /// Список организаций (рег.№, ОКПО), где проверка отключена.
    /// </param>
    private static async Task CheckTitleFormAsync(
        Reports baseReps,
        Reports impReps,
        List<(string, string)> repsWhereTitleFormCheckIsCancel)
    {
        var comparator = new CustomStringTitleFormComparer();
        if (baseReps.Master.FormNum_DB is "1.0"
            && (comparator.Compare(baseReps.Master.Rows10[0].SubjectRF_DB, impReps.Master.Rows10[0].SubjectRF_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].JurLico_DB, impReps.Master.Rows10[0].JurLico_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].ShortJurLico_DB, impReps.Master.Rows10[0].ShortJurLico_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].JurLicoAddress_DB, impReps.Master.Rows10[0].JurLicoAddress_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].JurLicoFactAddress_DB, impReps.Master.Rows10[0].JurLicoFactAddress_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].GradeFIO_DB, impReps.Master.Rows10[0].GradeFIO_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].Telephone_DB, impReps.Master.Rows10[0].Telephone_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].Fax_DB, impReps.Master.Rows10[0].Fax_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].Email_DB, impReps.Master.Rows10[0].Email_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].Okpo_DB, impReps.Master.Rows10[0].Okpo_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].Okved_DB, impReps.Master.Rows10[0].Okved_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].Okogu_DB, impReps.Master.Rows10[0].Okogu_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].Oktmo_DB, impReps.Master.Rows10[0].Oktmo_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].Inn_DB, impReps.Master.Rows10[0].Inn_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].Kpp_DB, impReps.Master.Rows10[0].Kpp_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].Okopf_DB, impReps.Master.Rows10[0].Okopf_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[0].Okfs_DB, impReps.Master.Rows10[0].Okfs_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].SubjectRF_DB, impReps.Master.Rows10[1].SubjectRF_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].JurLico_DB, impReps.Master.Rows10[1].JurLico_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].ShortJurLico_DB, impReps.Master.Rows10[1].ShortJurLico_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].JurLicoAddress_DB, impReps.Master.Rows10[1].JurLicoAddress_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].JurLicoFactAddress_DB, impReps.Master.Rows10[1].JurLicoFactAddress_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].GradeFIO_DB, impReps.Master.Rows10[1].GradeFIO_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].Telephone_DB, impReps.Master.Rows10[1].Telephone_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].Fax_DB, impReps.Master.Rows10[1].Fax_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].Email_DB, impReps.Master.Rows10[1].Email_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].Okpo_DB, impReps.Master.Rows10[1].Okpo_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].Okved_DB, impReps.Master.Rows10[1].Okved_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].Okogu_DB, impReps.Master.Rows10[1].Okogu_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].Oktmo_DB, impReps.Master.Rows10[1].Oktmo_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].Inn_DB, impReps.Master.Rows10[1].Inn_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].Kpp_DB, impReps.Master.Rows10[1].Kpp_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].Okopf_DB, impReps.Master.Rows10[1].Okopf_DB) != 0
                || comparator.Compare(baseReps.Master.Rows10[1].Okfs_DB, impReps.Master.Rows10[1].Okfs_DB) != 0)
            || (baseReps.Master.FormNum_DB is "2.0"
                && (comparator.Compare(baseReps.Master.Rows20[0].SubjectRF_DB, impReps.Master.Rows20[0].SubjectRF_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].JurLico_DB, impReps.Master.Rows20[0].JurLico_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].ShortJurLico_DB, impReps.Master.Rows20[0].ShortJurLico_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].JurLicoAddress_DB, impReps.Master.Rows20[0].JurLicoAddress_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].JurLicoFactAddress_DB, impReps.Master.Rows20[0].JurLicoFactAddress_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].GradeFIO_DB, impReps.Master.Rows20[0].GradeFIO_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].Telephone_DB, impReps.Master.Rows20[0].Telephone_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].Fax_DB, impReps.Master.Rows20[0].Fax_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].Email_DB, impReps.Master.Rows20[0].Email_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].Okpo_DB, impReps.Master.Rows20[0].Okpo_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].Okved_DB, impReps.Master.Rows20[0].Okved_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].Okogu_DB, impReps.Master.Rows20[0].Okogu_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].Oktmo_DB, impReps.Master.Rows20[0].Oktmo_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].Inn_DB, impReps.Master.Rows20[0].Inn_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].Kpp_DB, impReps.Master.Rows20[0].Kpp_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].Okopf_DB, impReps.Master.Rows20[0].Okopf_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[0].Okfs_DB, impReps.Master.Rows20[0].Okfs_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].SubjectRF_DB, impReps.Master.Rows20[1].SubjectRF_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].JurLico_DB, impReps.Master.Rows20[1].JurLico_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].ShortJurLico_DB, impReps.Master.Rows20[1].ShortJurLico_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].JurLicoAddress_DB, impReps.Master.Rows20[1].JurLicoAddress_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].JurLicoFactAddress_DB, impReps.Master.Rows20[1].JurLicoFactAddress_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].GradeFIO_DB, impReps.Master.Rows20[1].GradeFIO_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].Telephone_DB, impReps.Master.Rows20[1].Telephone_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].Fax_DB, impReps.Master.Rows20[1].Fax_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].Email_DB, impReps.Master.Rows20[1].Email_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].Okpo_DB, impReps.Master.Rows20[1].Okpo_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].Okved_DB, impReps.Master.Rows20[1].Okved_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].Okogu_DB, impReps.Master.Rows20[1].Okogu_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].Oktmo_DB, impReps.Master.Rows20[1].Oktmo_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].Inn_DB, impReps.Master.Rows20[1].Inn_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].Kpp_DB, impReps.Master.Rows20[1].Kpp_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].Okopf_DB, impReps.Master.Rows20[1].Okopf_DB) != 0
                    || comparator.Compare(baseReps.Master.Rows20[1].Okfs_DB, impReps.Master.Rows20[1].Okfs_DB) != 0)))
        {
            var newTitleRep = await new CompareReportsTitleFormAsyncCommand(baseReps.Master, impReps.Master,
                repsWhereTitleFormCheckIsCancel).AsyncExecute(null);
            baseReps.Master = newTitleRep;
        }
    }

    #endregion

    #region FillEmptyRegNo

    /// <summary>
    /// Заполняет рег.№ во всех формах 1.0 и 2.0.
    /// </summary>
    /// <param name="reps">Организация.</param>
    private protected static void FillEmptyRegNo(ref Reports? reps)
    {
        if (reps is null) return;
        if (reps.Master.Rows10.Count >= 2)
        {
            if (reps.Master.Rows10[0].RegNo_DB is "" && reps.Master.Rows10[1].RegNo_DB is not "" &&
                reps.Master.Rows10[0].Okpo_DB is not "")
            {
                reps.Master.Rows10[0].RegNo_DB = reps.Master.Rows10[1].RegNo_DB;
            }

            if (reps.Master.Rows10[1].RegNo_DB is "" && reps.Master.Rows10[0].RegNo_DB is not "" &&
                reps.Master.Rows10[1].Okpo_DB is not "")
            {
                reps.Master.Rows10[1].RegNo_DB = reps.Master.Rows10[0].RegNo_DB;
            }
        }

        if (reps.Master.Rows20.Count >= 2)
        {
            if (reps.Master.Rows20[0].RegNo_DB is "" && reps.Master.Rows20[1].RegNo_DB is not "" &&
                reps.Master.Rows20[0].Okpo_DB is not "")
            {
                reps.Master.Rows20[0].RegNo_DB = reps.Master.Rows20[1].RegNo_DB;
            }

            if (reps.Master.Rows20[1].RegNo_DB is "" && reps.Master.Rows20[0].RegNo_DB is not "" &&
                reps.Master.Rows20[1].Okpo_DB is not "")
            {
                reps.Master.Rows20[1].RegNo_DB = reps.Master.Rows20[0].RegNo_DB;
            }
        }
    }

    #endregion

    #region GetReportsFromDbEqual

    private protected static async Task<Reports?> GetReports11FromDbEqualAsync(Reports reps)
    {
        if (reps.Master_DB.FormNum_DB is not "1.0")
        {
            return null;
        }

        var db = StaticConfiguration.DBModel;

        var repsList = await db.ReportsCollectionDbSet
            .Include(t => t.Master_DB).ThenInclude(m => m.Rows10)
            .Where(t => t.DBObservableId != null && t.Master_DB.FormNum_DB == "1.0")
            .ToListAsync();

        return FindMatchingReports(
            reps,
            repsList,
            r => (
                r.Master.Rows10[0].RegNo_DB,
                r.Master.Rows10[0].Okpo_DB,
                r.Master.Rows10[1].RegNo_DB,
                r.Master.Rows10[1].Okpo_DB));
    }

    private protected static async Task<Reports?> GetReports21FromDbEqualAsync(Reports reps)
    {
        if (reps.Master_DB.FormNum_DB is not "2.0")
        {
            return null;
        }

        var db = StaticConfiguration.DBModel;

        var repsList = await db.ReportsCollectionDbSet
            .Include(t => t.Master_DB).ThenInclude(m => m.Rows20)
            .Where(t => t.DBObservableId != null && t.Master_DB.FormNum_DB == "2.0")
            .ToListAsync();

        return FindMatchingReports(
            reps,
            repsList,
            r => (
                r.Master.Rows20[0].RegNo_DB,
                r.Master.Rows20[0].Okpo_DB,
                r.Master.Rows20[1].RegNo_DB,
                r.Master.Rows20[1].Okpo_DB));
    }

    #region FindMatchingReports

    private static Reports? FindMatchingReports(
        Reports reps,
        IEnumerable<Reports> candidates,
        Func<Reports, (string reg0, string okpo0, string reg1, string okpo1)> getOrgData)
    {
        try
        {
            var list = candidates as IList<Reports> ?? candidates.ToList();

            var (impReg0, impOkpo0, impReg1, impOkpo1) = getOrgData(reps);

            var first = list.FirstOrDefault(t =>
            {
                var (baseReg0, baseOkpo0, baseReg1, baseOkpo1) = getOrgData(t);

                return
                    // обособленные пусты и в базе и в импорте, то сверяем головное
                    (impReg0 == baseReg0
                     && impOkpo0 == baseOkpo0
                     && impReg1 == ""
                     && baseReg1 == "")
                    ||
                    // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                    (impOkpo0 == baseOkpo0
                     && impReg0 == baseReg1
                     && impOkpo1 == ""
                     && baseOkpo1 == "")
                    ||
                    // обособленные не пусты, их и сверяем
                    (impOkpo1 == baseOkpo1
                     && impReg1 == baseReg1
                     && impOkpo1 != "")
                    ||
                    // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                    (impOkpo1 == baseOkpo1
                     && impReg1 == baseReg0
                     && impOkpo1 != ""
                     && baseReg1 == "");
            });

            if (first != null)
            {
                return first;
            }

            return list.FirstOrDefault(t =>
            {
                var (baseReg0, baseOkpo0, baseReg1, baseOkpo1) = getOrgData(t);

                return
                    // юр лицо в базе совпадает с обособленным в импорте
                    (impOkpo1 != ""
                     && baseOkpo1 == ""
                     && impOkpo1 == baseOkpo0
                     && impReg1 == baseReg0)
                    ||
                    // юр лицо в импорте совпадает с обособленным в базе
                    (impOkpo1 == ""
                     && baseOkpo1 != ""
                     && impOkpo0 == baseOkpo1
                     && impReg0 == baseReg1);
            });
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #endregion

    #region GetSelectedFilesFromDialog

    /// <summary>
    /// Открыть окно выбора файлов с соответствующим фильтром.
    /// </summary>
    /// <param name="name">Имя фильтра.</param>
    /// <param name="extensions">Массив расширений файлов.</param>
    /// <returns>Список файлов.</returns>
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

    /// <summary>
    /// Проверка, является ли отчёт инвентаризационным. Если все строчки с кодом операции 10 - добавляет " (ИНВ)",
    /// если хотя бы одна - добавляет " (инв)".
    /// </summary>
    /// <param name="rep">Отчёт.</param>
    /// <returns>Строчка, информирующая о том, является ли отчёт инвентаризационным.</returns>
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

    /// <summary>
    /// В случае, если в БД есть организация по форме 1.0, соответствующая импортируемой,
    /// для каждого импортируемого отчёта сверяются период и номер корректировки.
    /// Пользователю предлагаются соответствующие команды по добавлению/дополнению/замене отчёта или отмене импорта.
    /// Происходит логирование и сохранение изменений.
    /// </summary>
    /// <param name="baseReps">Организация в БД.</param>
    /// <param name="impReps">Импортируемая организация.</param>
    /// <param name="impRepList">Список импортируемых отчётов.</param>
    /// <returns>Сообщение пользователю, логирование и сохранение изменений.</returns>
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
                BaseRepFormCount = Math.Max(await ReportsStorage.GetReportRowsCount(baseRep), baseRep.Rows.Count);
                BaseRepStartPeriod = baseRep.StartPeriod_DB;
                BaseRepEndPeriod = baseRep.EndPeriod_DB;
                BaseRepExpDate = baseRep.ExportDate_DB;

                #region Periods

                var stBase = DateTime.Parse(DateTime.Now.ToShortDateString()); //Начало периода у отчета в базе
                var endBase = DateTime.Parse(DateTime.Now.ToShortDateString()); //Конец периода у отчета в базе
                if (DateTime.TryParse(BaseRepStartPeriod, out var baseRepStartPeriod)
                    && DateTime.TryParse(BaseRepEndPeriod, out var baseRepEndPeriod))
                {
                    stBase = baseRepStartPeriod > baseRepEndPeriod
                        ? baseRepEndPeriod
                        : baseRepStartPeriod;
                    endBase = baseRepStartPeriod < baseRepEndPeriod
                        ? baseRepEndPeriod
                        : baseRepStartPeriod;
                }

                var stImp = DateTime.Parse(DateTime.Now.ToShortDateString()); //Начало периода у импортируемого отчета
                var endImp = DateTime.Parse(DateTime.Now.ToShortDateString()); //Конец периода у импортируемого отчета
                if (DateTime.TryParse(ImpRepStartPeriod, out var impRepStartPeriod)
                    && DateTime.TryParse(ImpRepEndPeriod, out var impRepEndPeriod))
                {
                    stImp = impRepStartPeriod > impRepEndPeriod
                        ? impRepEndPeriod
                        : impRepStartPeriod;
                    endImp = impRepStartPeriod < impRepEndPeriod
                        ? impRepEndPeriod
                        : impRepStartPeriod;
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

                        res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions =
                                [
                                    new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true },
                                    new ButtonDefinition { Name = "Пропустить для всех" }
                                ],
                                ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                            .ShowDialog(Desktop.MainWindow));

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

                        res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions =
                                [
                                    new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                    new ButtonDefinition { Name = "Дополнить" },
                                    new ButtonDefinition { Name = "Сохранить оба" },
                                    new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                ],
                                ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                            .ShowDialog(Desktop.MainWindow));

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

                            res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions =
                                    [
                                        new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                        new ButtonDefinition { Name = "Заменять все формы" },
                                        new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                    ],
                                    ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                                .ShowDialog(Desktop.MainWindow));

                            #endregion

                            if (res is "Заменять все формы") SkipReplace = true;
                        }
                        else
                        {
                            #region MessageImportReportHasHigherCorrectionNumber

                            res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions =
                                    [
                                        new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                        new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                    ],
                                    ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                                .ShowDialog(Desktop.MainWindow));

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

                    res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions =
                            [
                                new ButtonDefinition { Name = "Сохранить оба", IsDefault = true },
                                new ButtonDefinition { Name = "Отменить для всех пересечений" },
                                new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                            ],
                            ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                        .ShowDialog(Desktop.MainWindow));

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

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true }
                        ],
                        ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                    .ShowDialog(Desktop.MainWindow));

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

                    res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions =
                            [
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Да для всех" },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            ],
                            ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                        .ShowDialog(Desktop.MainWindow));

                    #endregion

                    if (res is "Да для всех") SkipNew = true;
                }
                else
                {
                    #region MessageNewReport

                    res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions =
                            [
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            ],
                            ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                        .ShowDialog(Desktop.MainWindow));

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

    /// <summary>
    /// В случае, если в БД есть организация по форме 2.0, соответствующая импортируемой,
    /// для каждого импортируемого отчёта сверяются период и номер корректировки.
    /// Пользователю предлагаются соответствующие команды по добавлению/дополнению/замене отчёта или отмене импорта.
    /// Происходит логирование и сохранение изменений.
    /// </summary>
    /// <param name="baseReps">Организация в БД.</param>
    /// <param name="impReps">Импортируемая организация.</param>
    /// <param name="impRepList">Список импортируемых отчётов.</param>
    /// <returns>Сообщение пользователю, логирование и сохранение изменений.</returns>
    private protected async Task ProcessIfHasReports21(Reports baseReps, Reports impReps, List<Report> impRepList)
    {
        BaseRepsOkpo = baseReps.Master.OkpoRep.Value;
        BaseRepsRegNum = baseReps.Master.RegNoRep.Value;
        BaseRepsShortName = baseReps.Master.ShortJurLicoRep.Value;

        foreach (var impRep in impRepList) //Для каждой импортируемой формы
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
                BaseRepFormCount = Math.Max(await ReportsStorage.GetReportRowsCount(baseRep), baseRep.Rows.Count);
                BaseRepExpDate = baseRep.ExportDate_DB;
                BaseRepYear = baseRep.Year_DB;

                if (BaseRepYear != ImpRepYear || ImpRepFormNum != BaseRepFormNum) continue;
                impInBase = true;

                #region LessCorrectionNumber

                if (ImpRepCorNum < BaseRepCorNum)
                {
                    if (SkipLess) break;

                    #region MessageImportReportHasLowerCorrectionNumber

                    res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions =
                            [
                                new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true },
                                new ButtonDefinition { Name = "Пропустить для всех" }
                            ],
                            ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                        .ShowDialog(Desktop.MainWindow));

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

                    res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions =
                            [
                                new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                new ButtonDefinition { Name = "Дополнить" },
                                new ButtonDefinition { Name = "Сохранить оба" },
                                new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                            ],
                            ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                        .ShowDialog(Desktop.MainWindow));

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

                        res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions =
                                [
                                    new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                    new ButtonDefinition { Name = "Заменять все формы" },
                                    new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                ],
                                ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                            .ShowDialog(Desktop.MainWindow));

                        #endregion

                        if (res is "Заменять все формы") SkipReplace = true;
                    }
                    else
                    {
                        #region MessageImportReportHasHigherCorrectionNumber

                        res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions =
                                [
                                    new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                    new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                ],
                                ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                            .ShowDialog(Desktop.MainWindow));

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

            if (impRepList.Count == 0)
            {
                impInBase = true;

                #region MessageNewReport

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true }
                        ],
                        ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                    .ShowDialog(Desktop.MainWindow));

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

                    res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions =
                            [
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Да для всех" },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            ],
                            ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                        .ShowDialog(Desktop.MainWindow));

                    #endregion

                    if (res == "Да для всех") SkipNew = true;
                }
                else
                {
                    #region MessageNewReport

                    res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions =
                            [
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            ],
                            ContentTitle = "Импорт из .raodb/.xlsx/.json",
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
                        .ShowDialog(Desktop.MainWindow));

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

    /// <summary>
    /// Получить порядковый номер для примечания.
    /// </summary>
    /// <param name="reps">Организация.</param>
    private protected static void ProcessIfNoteOrder0(Reports reps)
    {
        foreach (var key in reps.Report_Collection)
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

    #region SortReportsCollection

    /// <summary>
    /// Сортирует основную коллекцию организаций по рег.№, затем по ОКПО.
    /// Используется после операций импорта для восстановления привычного порядка.
    /// </summary>
    #pragma warning disable
    [SuppressMessage("ReSharper", "EntityFramework.NPlusOne.IncompleteDataUsage")]
    [SuppressMessage("ReSharper", "EntityFramework.NPlusOne.IncompleteDataQuery")]
    private protected static async Task SortReportsCollectionAsync()
    {
        var db = StaticConfiguration.DBModel;

        var dbObservable = db.DBObservableDbSet.Local.FirstOrDefault()
                           ?? await db.DBObservableDbSet.FirstAsync();

        var comparator = new CustomReportsComparer();

        var sorted = dbObservable.Reports_Collection
            .OrderBy(x => x.Master_DB.RegNoRep.Value, comparator)
            .ThenBy(x => x.Master_DB.OkpoRep.Value, comparator)
            .ToList();

        dbObservable.Reports_Collection.Clear();
        foreach (var reps in sorted)
        {
            dbObservable.Reports_Collection.Add(reps);
        }
    }
    #pragma warning restore

    #endregion

    #region SetDataGridPage

    /// <summary>
    /// Изменяет номер страницы таблицы организаций, чтобы отображалась добавленная организация (только для одной добавленной организации).
    /// </summary>
    /// <param name="impReportsList">Список организаций, добавленных в ходе импорта.</param>
    private protected static Task SetDataGridPage(List<Reports> impReportsList)
    {
        if (impReportsList.Count > 0
            && impReportsList.All(x => x.Master_DB.RegNoRep.Value == impReportsList.First().Master_DB.RegNoRep.Value
                                       && x.Master_DB.OkpoRep.Value == impReportsList.First().Master_DB.OkpoRep.Value))
        {
            var impReports = impReportsList.First();
            var repsDataGrid = impReports.Master_DB.FormNum_DB is "1.0" 
                ? (Desktop.MainWindow.FindControl<Panel>("Forms_p1_0").Children[0] as DataGridReports)!
                : (Desktop.MainWindow.FindControl<Panel>("Forms_p2_0").Children[0] as DataGridReports)!;

            var repsList = !string.IsNullOrWhiteSpace(repsDataGrid.SearchText) 
                ? repsDataGrid.ItemsWithSearch!.ToList<Reports>() 
                : repsDataGrid.Items.ToList<Reports>();

            var repsIndex = repsList.FindIndex(x => x.Master_DB.RegNoRep.Value == impReports.Master_DB.RegNoRep.Value 
                                                    && x.Master_DB.OkpoRep.Value == impReports.Master_DB.OkpoRep.Value);

            if (repsIndex != -1)
            {
                repsDataGrid.NowPage = impReports.Master_DB.FormNum_DB switch
                {
                    "1.0" => ((repsIndex + 1) / 5 + 1).ToString(),
                    "2.0" => ((repsIndex + 1) / 8 + 1).ToString(),
                    _ => repsDataGrid.NowPage
                };
            }
        }
        return Task.CompletedTask;
    }

    #endregion

    #region FillReportWithFormsInReports

    /// <summary>
    /// Находит организацию и отчёт в БД и заменяет его в локальном хранилище.
    /// </summary>
    /// <param name="baseReps">Организация в БД.</param>
    /// <param name="baseRep">Отчёт в БД.</param>
    /// <returns>Отчёт.</returns>
    private static async Task<Report> FillReportWithForms(Reports baseReps, Report baseRep)
    {
        var checkedRep = StaticConfiguration.DBModel.Set<Report>().Local
                .FirstOrDefault(entry => entry.Id.Equals(baseRep.Id));
        if (checkedRep != null &&
            (checkedRep.Rows.ToList<Form>().Any(form => form == null) || checkedRep.Rows.Count == 0))
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