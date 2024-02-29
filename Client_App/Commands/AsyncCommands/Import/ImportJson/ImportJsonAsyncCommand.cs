using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Models.Collections;
using Models.Forms;
using Models.JSON;
using Newtonsoft.Json;
using Avalonia.Controls;
using Client_App.Interfaces.Logger;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using Models.DTO;
using ReactiveUI;
using static Client_App.Commands.AsyncCommands.Import.ImportJson.ImportJsonMethods;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.Import.ImportJson;

public class ImportJsonAsyncCommand : ImportBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        IsFirstLogLine = true;
        CurrentLogLine = 1;
        string[] extensions = ["json", "JSON"];
        var answer = await GetSelectedFilesFromDialog("JSON", extensions);
        if (answer is null) return;
        var countReadFiles = answer.Length;
        var countNewReps = 0;
        SkipNewOrg = false;
        SkipInter = false;
        SkipLess = false;
        SkipNew = false;
        SkipReplace = false;
        HasMultipleReport = false;
        AtLeastOneImportDone = false;

        foreach (var path in answer) // Для каждого импортируемого файла JSON
        {
            try
            {
                if (path == "") continue;
                var file = GetRaoFileName();
                SourceFile = new FileInfo(path);
                SourceFile.CopyTo(file, true);
                var jsonString = await File.ReadAllTextAsync(file);
                var jsonObject = JsonConvert.DeserializeObject<JsonModel>(jsonString)!;
                List<Reports> reportsJsonCollection = [];

                foreach (var reps in jsonObject.Orgs)   // Для каждой организации в файле (получаем лист импортируемых организаций)
                {
                    #region GetImpRepsAndAddToRepsCollection

                    string formNumReps;
                    if (jsonObject.Forms.Any(form => form != null && form.FormNum.StartsWith('1')))
                    {
                        formNumReps = "1.0";
                    }
                    else if (jsonObject.Forms.Any(form => form != null && form.FormNum.StartsWith('2')))
                    {
                        formNumReps = "2.0";
                    }
                    else return;

                    var ty1 = FormCreator.Create(formNumReps);
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = FormCreator.Create(formNumReps);
                    ty2.NumberInOrder_DB = 2;

                    var impReps = new Reports
                    {
                        Master_DB = new Report
                        {
                            FormNum_DB = formNumReps
                        },
                        Id = reps[0].Id
                    };
                    switch (formNumReps)
                    {
                        case "1.0":
                        {
                            #region Bindings

                            impReps.Master_DB.Rows10.Add(ty1);
                            impReps.Master_DB.Rows10.Add(ty2);

                            impReps.Master_DB.Rows10[0].RegNo_DB = reps[0].RegNo;
                            impReps.Master_DB.Rows10[0].OrganUprav_DB = reps[0].OrganUprav;
                            impReps.Master_DB.Rows10[0].SubjectRF_DB = reps[0].SubjectRF;
                            impReps.Master_DB.Rows10[0].JurLico_DB = reps[0].JurLico;
                            impReps.Master_DB.Rows10[0].ShortJurLico_DB = reps[0].ShortJurLico;
                            impReps.Master_DB.Rows10[0].JurLicoAddress_DB = reps[0].JurLicoAddress;
                            impReps.Master_DB.Rows10[0].JurLicoFactAddress_DB = reps[0].JurLicoFactAddress;
                            impReps.Master_DB.Rows10[0].GradeFIO_DB = reps[0].GradeFIO;
                            impReps.Master_DB.Rows10[0].Telephone_DB = reps[0].Telephone;
                            impReps.Master_DB.Rows10[0].Fax_DB = reps[0].Fax;
                            impReps.Master_DB.Rows10[0].Email_DB = reps[0].Email;
                            impReps.Master_DB.Rows10[0].Okpo_DB = reps[0].Okpo;
                            impReps.Master_DB.Rows10[0].Okved_DB = reps[0].Okved;
                            impReps.Master_DB.Rows10[0].Okogu_DB = reps[0].Okogu;
                            impReps.Master_DB.Rows10[0].Oktmo_DB = reps[0].Oktmo;
                            impReps.Master_DB.Rows10[0].Inn_DB = reps[0].Inn;
                            impReps.Master_DB.Rows10[0].Kpp_DB = reps[0].Kpp;
                            impReps.Master_DB.Rows10[0].Okopf_DB = reps[0].Okopf;
                            impReps.Master_DB.Rows10[0].Okfs_DB = reps[0].Okfs;
                            if (reps.Length > 1)
                            {
                                impReps.Master_DB.Rows10[1].RegNo_DB = reps[1].RegNo;
                                impReps.Master_DB.Rows10[1].OrganUprav_DB = reps[1].OrganUprav;
                                impReps.Master_DB.Rows10[1].SubjectRF_DB = reps[1].SubjectRF;
                                impReps.Master_DB.Rows10[1].JurLico_DB = reps[1].JurLico;
                                impReps.Master_DB.Rows10[1].ShortJurLico_DB = reps[1].ShortJurLico;
                                impReps.Master_DB.Rows10[1].JurLicoAddress_DB = reps[1].JurLicoAddress;
                                impReps.Master_DB.Rows10[1].GradeFIO_DB = reps[1].GradeFIO;
                                impReps.Master_DB.Rows10[1].Telephone_DB = reps[1].Telephone;
                                impReps.Master_DB.Rows10[1].Fax_DB = reps[1].Fax;
                                impReps.Master_DB.Rows10[1].Email_DB = reps[1].Email;
                                impReps.Master_DB.Rows10[1].Okpo_DB = reps[1].Okpo;
                                impReps.Master_DB.Rows10[1].Okpo_DB = reps[1].Okpo;
                                impReps.Master_DB.Rows10[1].Okved_DB = reps[1].Okved;
                                impReps.Master_DB.Rows10[1].Okogu_DB = reps[1].Okogu;
                                impReps.Master_DB.Rows10[1].Oktmo_DB = reps[1].Oktmo;
                                impReps.Master_DB.Rows10[1].Inn_DB = reps[1].Inn;
                                impReps.Master_DB.Rows10[1].Kpp_DB = reps[1].Kpp;
                                impReps.Master_DB.Rows10[1].Okopf_DB = reps[1].Okopf;
                                impReps.Master_DB.Rows10[1].Okfs_DB = reps[1].Okfs;
                            }

                            #endregion

                            break;
                        }
                        case "2.0":
                        {
                            #region Bindings

                                impReps.Master_DB.Rows20.Add(ty1);
                                impReps.Master_DB.Rows20.Add(ty2);

                                impReps.Master_DB.Rows20[0].RegNo_DB = reps[0].RegNo;
                                impReps.Master_DB.Rows20[0].OrganUprav_DB = reps[0].OrganUprav;
                                impReps.Master_DB.Rows20[0].SubjectRF_DB = reps[0].SubjectRF;
                                impReps.Master_DB.Rows20[0].JurLico_DB = reps[0].JurLico;
                                impReps.Master_DB.Rows20[0].ShortJurLico_DB = reps[0].ShortJurLico;
                                impReps.Master_DB.Rows20[0].JurLicoAddress_DB = reps[0].JurLicoAddress;
                                impReps.Master_DB.Rows20[0].JurLicoFactAddress_DB = reps[0].JurLicoFactAddress;
                                impReps.Master_DB.Rows20[0].GradeFIO_DB = reps[0].GradeFIO;
                                impReps.Master_DB.Rows20[0].Telephone_DB = reps[0].Telephone;
                                impReps.Master_DB.Rows20[0].Fax_DB = reps[0].Fax;
                                impReps.Master_DB.Rows20[0].Email_DB = reps[0].Email;
                                impReps.Master_DB.Rows20[0].Okpo_DB = reps[0].Okpo;
                                impReps.Master_DB.Rows20[0].Okved_DB = reps[0].Okved;
                                impReps.Master_DB.Rows20[0].Okogu_DB = reps[0].Okogu;
                                impReps.Master_DB.Rows20[0].Oktmo_DB = reps[0].Oktmo;
                                impReps.Master_DB.Rows20[0].Inn_DB = reps[0].Inn;
                                impReps.Master_DB.Rows20[0].Kpp_DB = reps[0].Kpp;
                                impReps.Master_DB.Rows20[0].Okopf_DB = reps[0].Okopf;
                                impReps.Master_DB.Rows20[0].Okfs_DB = reps[0].Okfs;
                                if (reps.Length > 1)
                                {
                                    impReps.Master_DB.Rows20[1].RegNo_DB = reps[1].RegNo;
                                    impReps.Master_DB.Rows20[1].OrganUprav_DB = reps[1].OrganUprav;
                                    impReps.Master_DB.Rows20[1].SubjectRF_DB = reps[1].SubjectRF;
                                    impReps.Master_DB.Rows20[1].JurLico_DB = reps[1].JurLico;
                                    impReps.Master_DB.Rows20[1].ShortJurLico_DB = reps[1].ShortJurLico;
                                    impReps.Master_DB.Rows20[1].JurLicoAddress_DB = reps[1].JurLicoAddress;
                                    impReps.Master_DB.Rows20[1].GradeFIO_DB = reps[1].GradeFIO;
                                    impReps.Master_DB.Rows20[1].Telephone_DB = reps[1].Telephone;
                                    impReps.Master_DB.Rows20[1].Fax_DB = reps[1].Fax;
                                    impReps.Master_DB.Rows20[1].Email_DB = reps[1].Email;
                                    impReps.Master_DB.Rows20[1].Okpo_DB = reps[1].Okpo;
                                    impReps.Master_DB.Rows20[1].Okpo_DB = reps[1].Okpo;
                                    impReps.Master_DB.Rows20[1].Okved_DB = reps[1].Okved;
                                    impReps.Master_DB.Rows20[1].Okogu_DB = reps[1].Okogu;
                                    impReps.Master_DB.Rows20[1].Oktmo_DB = reps[1].Oktmo;
                                    impReps.Master_DB.Rows20[1].Inn_DB = reps[1].Inn;
                                    impReps.Master_DB.Rows20[1].Kpp_DB = reps[1].Kpp;
                                    impReps.Master_DB.Rows20[1].Okopf_DB = reps[1].Okopf;
                                    impReps.Master_DB.Rows20[1].Okfs_DB = reps[1].Okfs;
                                }

                                #endregion

                            break;
                        }
                    }

                    reportsJsonCollection.Add(impReps);

                    #endregion
                }

                foreach (var rep in jsonObject.Forms.Where(rep => rep is not null)) // Для каждого отчета, добавляем её к соответствующей организации в листе
                {
                    #region GetImpFormsAndAddToRepsCollection

                    var currentOrg = reportsJsonCollection.FirstOrDefault(reps => reps.Id == rep.ReportsId);
                    if (currentOrg is null) continue;

                    #region GetCreationTime

                    var timeCreate = new List<string>()
                    {
                        SourceFile.CreationTime.Day.ToString(),
                        SourceFile.CreationTime.Month.ToString(),
                        SourceFile.CreationTime.Year.ToString()
                    };
                    if (timeCreate[0].Length == 1)
                    {
                        timeCreate[0] = $"0{timeCreate[0]}";
                    }

                    if (timeCreate[1].Length == 1)
                    {
                        timeCreate[1] = $"0{timeCreate[1]}";
                    }

                    #endregion

                    var impRep = new Report
                    {
                        CorrectionNumber_DB = rep.CorrectionNumber,
                        FormNum_DB = rep.FormNum,
                        GradeExecutor_DB = Convert.ToString(rep.ExecutorData?.GradeExecutor as object),
                        FIOexecutor_DB = Convert.ToString(rep.ExecutorData?.FIOexecutor as object),
                        ExecPhone_DB = Convert.ToString(rep.ExecutorData?.ExecPhone as object),
                        ExecEmail_DB = Convert.ToString(rep.ExecutorData?.ExecEmail as object),
                        ExportDate_DB = $"{timeCreate[0]}.{timeCreate[1]}.{timeCreate[2]}",
                        StartPeriod_DB = DateTime.TryParse(rep.StartPeriod, out var dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : string.Empty,
                        EndPeriod_DB = DateTime.TryParse(rep.EndPeriod, out dateTimeValue)
                            ? dateTimeValue.ToShortDateString()
                            : string.Empty,
                        Year_DB = Convert.ToString(rep.Year)
                    };

                    BindFormTopSpecificData(rep, impRep); //bind data from form top table to Report
                    BindFormTableData(rep, impRep);  //bind all forms data to Report

                    var noteOrder = 1;
                    foreach (var note in rep.NotesMainTable.Notes)
                    {
                        impRep.Notes.Add(new Models.Forms.Note
                        {
                            Order = noteOrder++,
                            RowNumber_DB = note.NotePointer.Row,
                            GraphNumber_DB = ColNameToColNum(impRep.FormNum_DB, note.NotePointer.ColName),
                            Comment_DB = note.NoteValue.Text
                        });
                    }

                    currentOrg.Report_Collection.Add(impRep);

                    #endregion
                }


                if (reportsJsonCollection.Count == 0)
                {
                    #region MessageFailedToReadFile

                    await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Импорт из .json",
                            ContentHeader = "Ошибка",
                            ContentMessage =
                                $"Не удалось прочесть файл {path}," +
                                $"{Environment.NewLine}файл поврежден или не содержит данных.",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow);

                    #endregion

                    countReadFiles--;
                    continue;
                }
                if (!HasMultipleReport)
                {
                    HasMultipleReport = reportsJsonCollection.Sum(x => x.Report_Collection.Count) > 1 || answer.Length > 1;
                }

                var orgListToAdd = reportsJsonCollection
                    .Where(reps => reps.Report_Collection.Count > 0)
                    .ToList();

                orgListToAdd.ForEach(reps =>
                    {
                        var newRepCol = reps.Report_Collection
                            .OrderBy(rep => byte.Parse(rep.FormNum_DB.Split('.')[0]))
                            .ThenBy(rep => byte.Parse(rep.FormNum_DB.Split('.')[1]))
                            .ThenByDescending(rep => reps.Master_DB.FormNum_DB.StartsWith('1')
                                ? DateTime.TryParse(rep.StartPeriod_DB, out var dateTimeValue)
                                    ? StringDateReverse(dateTimeValue.ToShortDateString())
                                    : rep.StartPeriod_DB
                                : rep.Year_DB);
                        reps.Report_Collection = [];
                        reps.Report_Collection.AddRange(newRepCol);
                    });

                foreach (var impReps in orgListToAdd)
                {
                    var baseReps11 = GetReports11FromLocalEqual(impReps);
                    var baseReps21 = GetReports21FromLocalEqual(impReps);
                    FillEmptyRegNo(ref baseReps11);
                    FillEmptyRegNo(ref baseReps21);
                    impReps.CleanIds();
                    ProcessIfNoteOrder0(impReps);

                    ImpRepFormCount = impReps.Report_Collection.Count;
                    ImpRepFormNum = impReps.Master.FormNum_DB;
                    BaseRepsOkpo = impReps.Master.OkpoRep.Value;
                    BaseRepsRegNum = impReps.Master.RegNoRep.Value;
                    BaseRepsShortName = impReps.Master.ShortJurLicoRep.Value;

                    var impRepsReportList = impReps.Report_Collection.ToList();
                    if (baseReps11 != null)
                    {
                        foreach (var report in baseReps11.Report_Collection)
                        {
                            await ReportsStorage.GetReportAsync(report.Id);
                        }
                        await ProcessIfHasReports11(baseReps11, impReps, impRepsReportList);
                    }
                    else if (baseReps21 != null)
                    {
                        foreach (var report in baseReps21.Report_Collection)
                        {
                            await ReportsStorage.GetReportAsync(report.Id);
                        }
                        await ProcessIfHasReports21(baseReps21, impReps, impRepsReportList);
                    }
                    else if (baseReps11 == null && baseReps21 == null)
                    {
                        #region AddNewOrg

                        var an = "Добавить";
                        if (!SkipNewOrg)
                        {
                            if (reportsJsonCollection
                                    .Where(x => x.Report_Collection.Count > 0)
                                    .ToList()
                                    .Count > 1)
                            {
                                #region MessageNewOrg

                                an = await MessageBox.Avalonia.MessageBoxManager
                                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                    {
                                        ButtonDefinitions =
                                        [
                                            new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                            new ButtonDefinition { Name = "Да для всех" },
                                            new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                        ],
                                        ContentTitle = "Импорт из .json",
                                        ContentHeader = "Уведомление",
                                        ContentMessage =
                                            $"Будет добавлена новая организация ({ImpRepFormNum}) содержащая {ImpRepFormCount} форм отчетности." +
                                            $"{Environment.NewLine}" +
                                            $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                            $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                            $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                            $"{Environment.NewLine}" +
                                            $"{Environment.NewLine}Кнопка \"Да для всех\" позволяет без уведомлений " +
                                            $"{Environment.NewLine}импортировать все новые организации.",
                                        MinWidth = 400,
                                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                                    })
                                    .ShowDialog(Desktop.MainWindow);

                                #endregion

                                if (an is "Да для всех") SkipNewOrg = true;
                            }
                            else
                            {
                                #region MessageNewOrg

                                an = await MessageBox.Avalonia.MessageBoxManager
                                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                    {
                                        ButtonDefinitions =
                                        [
                                            new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                            new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                        ],
                                        ContentTitle = "Импорт из .json",
                                        ContentHeader = "Уведомление",
                                        ContentMessage =
                                            $"Будет добавлена новая организация ({ImpRepFormNum}) содержащая {ImpRepFormCount} форм отчетности." +
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
                        }
                        if (an is "Добавить" or "Да для всех")
                        {
                            ReportsStorage.LocalReports.Reports_Collection.Add(impReps);
                            countNewReps++;
                            AtLeastOneImportDone = true;

                            #region LoggerImport

                            var sortedRepList = impReps.Report_Collection
                                .OrderBy(x => x.FormNum_DB)
                                .ThenBy(x => StringReverse(x.StartPeriod_DB))
                                .ToList();
                            foreach (var rep in sortedRepList)
                            {
                                ImpRepCorNum = rep.CorrectionNumber_DB;
                                ImpRepFormCount = rep.Rows11.Count + rep.Rows12.Count + rep.Rows13.Count + rep.Rows14.Count + rep.Rows15.Count
                                                  + rep.Rows16.Count + rep.Rows17.Count + rep.Rows18.Count + rep.Rows19.Count + rep.Rows21.Count
                                                  + rep.Rows22.Count + rep.Rows23.Count + rep.Rows24.Count + rep.Rows25.Count + rep.Rows26.Count
                                                  + rep.Rows27.Count + rep.Rows28.Count + rep.Rows29.Count + rep.Rows210.Count + rep.Rows211.Count
                                                  + rep.Rows212.Count;
                                ImpRepFormNum = rep.FormNum_DB;
                                ImpRepStartPeriod = rep.StartPeriod_DB;
                                ImpRepEndPeriod = rep.EndPeriod_DB;
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
                            }

                            #endregion
                        }

                        #endregion
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }
        await StaticConfiguration.DBModel.SaveChangesAsync().ConfigureAwait(false);

        #region Suffix

        var suffix1 = answer.Length.ToString().EndsWith('1') && !answer.Length.ToString().EndsWith("11")
            ? "а"
            : "ов";
        var suffix2 = countNewReps.ToString().EndsWith('1') && !countNewReps.ToString().EndsWith("11")
            ? "ая"
            : countNewReps.ToString().EndsWith('2') && !countNewReps.ToString().EndsWith("12")
              || countNewReps.ToString().EndsWith('3') && !countNewReps.ToString().EndsWith("13")
              || countNewReps.ToString().EndsWith('4') && !countNewReps.ToString().EndsWith("14")
                ? "ые"
                : "ых";
        var suffix3 = countNewReps.ToString().EndsWith('1') && !countNewReps.ToString().EndsWith("11")
            ? "я"
            : countNewReps.ToString().EndsWith('2') && !countNewReps.ToString().EndsWith("12")
              || countNewReps.ToString().EndsWith('3') && !countNewReps.ToString().EndsWith("13")
              || countNewReps.ToString().EndsWith('4') && !countNewReps.ToString().EndsWith("14")
                ? "и"
                : "й";

        #endregion

        if (AtLeastOneImportDone)
        {
            #region MessageImportDone

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Импорт из .json",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Импорт {countReadFiles} из {answer.Length} файл{suffix1} .json успешно завершен." +
                    $"{Environment.NewLine}Импортировано {countNewReps} нов{suffix2} организаци{suffix3}",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow);

            #endregion
        }
        else
        {
            #region MessageImportCancel

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Импорт из .json",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Импорт из {answer.Length} файл{suffix1} .json был отменен.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow);

            #endregion
        }
    }
}