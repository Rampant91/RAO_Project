using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Client_App.ViewModels;
using Models.Collections;
using Models.Forms.Form1;
using Models.Forms;
using Models.JSON;
using Newtonsoft.Json;
using Avalonia.Controls;
using Client_App.Interfaces.Logger;
using Client_App.Resources;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using Models.DTO;

namespace Client_App.Commands.AsyncCommands.Import;

public class ImportJsonAsyncCommand : ImportBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        IsFirstLogLine = true;
        CurrentLogLine = 1;
        string[] extensions = { "json", "JSON" };
        var answer = await GetSelectedFilesFromDialog("JSON", extensions);
        if (answer is null) return;
        SkipNewOrg = false;
        SkipInter = false;
        SkipLess = false;
        SkipNew = false;
        SkipReplace = false;
        HasMultipleReport = false;
        AtLeastOneImportDone = false;

        foreach (var path in answer) // Для каждого импортируемого файла
        {
            try
            {
                var jsonString = await File.ReadAllTextAsync(path);
                var jsonObject = JsonConvert.DeserializeObject<JsonModel>(jsonString)!;

                foreach (var reps in jsonObject.Orgs)
                {
                    #region GetBaseReps

                    string formNumReps;
                    if (jsonObject.Forms.Any(form => form.FormNum.StartsWith('1')))
                    {
                        formNumReps = "1.0";
                    }
                    else if (jsonObject.Forms.Any(form => form.FormNum.StartsWith('2')))
                    {
                        formNumReps = "2.0";
                    }
                    else return;

                    var ty1 = (Form10)FormCreator.Create(formNumReps);
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form10)FormCreator.Create(formNumReps);
                    ty2.NumberInOrder_DB = 2;

                    var baseReps = new Reports
                    {
                        Master_DB = new Report
                        {
                            FormNum_DB = formNumReps
                        }
                    };
                    switch (formNumReps)
                    {
                        case "1.0":

                            #region Bindings

                            baseReps.Master_DB.Rows10.Add(ty1);
                            baseReps.Master_DB.Rows10.Add(ty2);

                            baseReps.Master_DB.Rows10[0].RegNo_DB = reps[0].RegNo;
                            baseReps.Master_DB.Rows10[0].OrganUprav_DB = reps[0].OrganUprav;

                            baseReps.Master_DB.Rows10[0].SubjectRF_DB = reps[0].SubjectRF;
                            baseReps.Master_DB.Rows10[0].JurLico_DB = reps[0].JurLico;
                            baseReps.Master_DB.Rows10[0].ShortJurLico_DB = reps[0].ShortJurLico;
                            baseReps.Master_DB.Rows10[0].JurLicoAddress_DB = reps[0].JurLicoAddress;
                            baseReps.Master_DB.Rows10[0].JurLicoFactAddress_DB = reps[0].JurLicoFactAddress;
                            baseReps.Master_DB.Rows10[0].GradeFIO_DB = reps[0].GradeFIO;
                            baseReps.Master_DB.Rows10[0].Telephone_DB = reps[0].Telephone;
                            baseReps.Master_DB.Rows10[0].Fax_DB = reps[0].Fax;
                            baseReps.Master_DB.Rows10[0].Email_DB = reps[0].Email;
                            baseReps.Master_DB.Rows10[0].Okpo_DB = reps[0].Okpo;
                            baseReps.Master_DB.Rows10[0].Okved_DB = reps[0].Okved;
                            baseReps.Master_DB.Rows10[0].Okogu_DB = reps[0].Okogu;
                            baseReps.Master_DB.Rows10[0].Oktmo_DB = reps[0].Oktmo;
                            baseReps.Master_DB.Rows10[0].Inn_DB = reps[0].Inn;
                            baseReps.Master_DB.Rows10[0].Kpp_DB = reps[0].Kpp;
                            baseReps.Master_DB.Rows10[0].Okopf_DB = reps[0].Okopf;
                            baseReps.Master_DB.Rows10[0].Okfs_DB = reps[0].Okfs;
                            if (reps.Length > 1)
                            {
                                baseReps.Master_DB.Rows10[1].SubjectRF_DB = reps[1].SubjectRF;
                                baseReps.Master_DB.Rows10[1].JurLico_DB = reps[1].JurLico;
                                baseReps.Master_DB.Rows10[1].ShortJurLico_DB = reps[1].ShortJurLico;
                                baseReps.Master_DB.Rows10[1].JurLicoAddress_DB = reps[1].JurLicoAddress;
                                baseReps.Master_DB.Rows10[1].GradeFIO_DB = reps[1].GradeFIO;
                                baseReps.Master_DB.Rows10[1].Telephone_DB = reps[1].Telephone;
                                baseReps.Master_DB.Rows10[1].Fax_DB = reps[1].Fax;
                                baseReps.Master_DB.Rows10[1].Email_DB = reps[1].Email;
                                baseReps.Master_DB.Rows10[1].Okpo_DB = reps[1].Okpo;
                                baseReps.Master_DB.Rows10[1].Okpo_DB = reps[1].Okpo;
                                baseReps.Master_DB.Rows10[1].Okved_DB = reps[1].Okved;
                                baseReps.Master_DB.Rows10[1].Okogu_DB = reps[1].Okogu;
                                baseReps.Master_DB.Rows10[1].Oktmo_DB = reps[1].Oktmo;
                                baseReps.Master_DB.Rows10[1].Inn_DB = reps[1].Inn;
                                baseReps.Master_DB.Rows10[1].Kpp_DB = reps[1].Kpp;
                                baseReps.Master_DB.Rows10[1].Okopf_DB = reps[1].Okopf;
                                baseReps.Master_DB.Rows10[1].Okfs_DB = reps[1].Okfs;
                            }

                            #endregion

                            break;
                        case "2.0": //TODO bind data 

                            #region Bindings

                            baseReps.Master_DB.Rows20.Add(ty1);
                            baseReps.Master_DB.Rows20.Add(ty2);

                            #endregion

                            break;
                    } 

                    #endregion

                    foreach (var form in jsonObject.Forms
                                 .Where(form => form.RegNoRep == reps[0].RegNo))
                    {
                        MainWindowVM.LocalReports.Reports_Collection10.Add(new Report()
                        {
                             
                        });
                    }

                    //MainWindowVM.LocalReports.Reports_Collection.Add(baseReps);  //TODO вынеси за foreach

                    var first11 = GetReports11FromLocalEqual(baseReps);
                    var first21 = GetReports21FromLocalEqual(baseReps);
                    FillEmptyRegNo(ref first11);
                    FillEmptyRegNo(ref first21);
                    baseReps.CleanIds();
                    ProcessIfNoteOrder0(baseReps);

                    ImpRepFormCount = baseReps.Report_Collection.Count;
                    ImpRepFormNum = baseReps.Master.FormNum_DB;
                    BaseRepsOkpo = baseReps.Master.OkpoRep.Value;
                    BaseRepsRegNum = baseReps.Master.RegNoRep.Value;
                    BaseRepsShortName = baseReps.Master.ShortJurLicoRep.Value;

                    if (first11 != null)    
                    {
                        await ProcessIfHasReports11(first11, baseReps);
                    }
                    else if (first21 != null)
                    {
                        await ProcessIfHasReports21(first21, baseReps);
                    }
                    else if (first11 == null && first21 == null)
                    {
                        #region AddNewOrg

                        var an = "Добавить";
                        if (!SkipNewOrg)
                        {
                            if (answer.Length > 1)
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
                                        ButtonDefinitions = new[]
                                        {
                                            new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                            new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                        },
                                        ContentTitle = "Импорт из .raodb",
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
                            MainWindowVM.LocalReports.Reports_Collection.Add(baseReps);
                            AtLeastOneImportDone = true;

                            #region LoggerImport

                            var sortedRepList = baseReps.Report_Collection
                                                .OrderBy(x => x.FormNum_DB)
                                                .ThenBy(x => StaticStringMethods.StringReverse(x.StartPeriod_DB))
                                                .ToList();
                            foreach (var rep in sortedRepList)
                            {
                                ImpRepCorNum = rep.CorrectionNumber_DB;
                                ImpRepFormCount = rep.Rows.Count;
                                ImpRepFormNum = rep.FormNum_DB;
                                ImpRepStartPeriod = rep.StartPeriod_DB;
                                ImpRepEndPeriod = rep.EndPeriod_DB;
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
                            }

                            #endregion
                        }

                        #endregion
                    }
                }
            }
            catch (Exception e)
            {
                //ignore
            }
        }    
        await MainWindowVM.LocalReports.Reports_Collection.QuickSortAsync();
        await StaticConfiguration.DBModel.SaveChangesAsync().ConfigureAwait(false);
    }
}