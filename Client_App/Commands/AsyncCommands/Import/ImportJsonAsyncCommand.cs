using System;
using System.Collections.Generic;
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
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using Models.DTO;
using System.Diagnostics;

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
        var countReadFiles = answer.Length;
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
                List<Reports> reportsJsonCollection = new();

                foreach (var reps in jsonObject.Orgs)   // Для каждой организации в файле (получаем лист импортируемых организаций)
                {
                    #region GetImpRepsAndAddToRepsCollection

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

                        case "2.0": //TODO bind data 
                        {
                            #region Bindings

                            impReps.Master_DB.Rows20.Add(ty1);
                            impReps.Master_DB.Rows20.Add(ty2);

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
                        FormNum_DB = rep.FormNum,
                        ExportDate_DB = $"{timeCreate[0]}.{timeCreate[1]}.{timeCreate[2]}",
                        CorrectionNumber_DB = rep.CorrectionNumber,
                        StartPeriod_DB = rep.StartPeriod,
                        EndPeriod_DB = rep.EndPeriod
                    };
                    var numberInOrder = 1;

                    switch (rep.FormNum)
                    {
                        #region Form11

                        case "1.1":
                        {
                            var formsList11 = new ObservableCollectionWithItemPropertyChanged<Form11>();
                            var repForm = (JsonForm11)rep;
                            impRep.GradeExecutor_DB = repForm.ExecutorData.GradeExecutor;
                            impRep.FIOexecutor_DB = repForm.ExecutorData.FIOexecutor;
                            impRep.ExecPhone_DB = repForm.ExecutorData.ExecPhone;
                            impRep.ExecEmail_DB = repForm.ExecutorData.ExecEmail;

                            foreach (var form in repForm.TableData.TableData)   // Для каждой строчки формы
                            {
                                impRep.Rows11.Add( new Form11
                                {
                                    NumberInOrder_DB = numberInOrder++,
                                    OperationCode_DB = form.OperationCode,
                                    OperationDate_DB = form.OperationDate,
                                    PassportNumber_DB = form.PassportNumber,
                                    Type_DB = form.Type,
                                    Radionuclids_DB = form.Radionuclids,
                                    FactoryNumber_DB = form.Radionuclids,
                                    Quantity_DB = int.TryParse(form.Quantity, out var intValue) ? intValue : null,
                                    Activity_DB = form.Activity,
                                    CreationDate_DB = form.CreationDate,
                                    CreatorOKPO_DB = form.CreatorOKPO,
                                    Category_DB = short.TryParse(form.Category, out var shortValue) ? shortValue : null,
                                    SignedServicePeriod_DB = float.TryParse(form.SignedServicePeriod, out var floatValue)
                                        ? floatValue
                                        : null,
                                    PropertyCode_DB = byte.TryParse(form.PropertyCode, out var byteValue)
                                        ? byteValue
                                        : null,
                                    Owner_DB = form.Owner,
                                    ProviderOrRecieverOKPO_DB = form.ProviderOrRecieverOKPO,
                                    TransporterOKPO_DB = form.TransporterOKPO,
                                    PackName_DB = form.PackName,
                                    PackType_DB = form.PackType,
                                    PackNumber_DB = form.PackNumber
                                });
                            }
                            break;
                        }

                        #endregion

                        #region Form12
                        
                        case "1.2":
                        {
                            var forms12List = new ObservableCollectionWithItemPropertyChanged<Form12>();
                            var repForm = rep as JsonForm12;
                            foreach (var form in repForm.TableData.TableData)   // Для каждой строчки в форме
                            {
                                var curForm = new Form12
                                {
                                    NumberInOrder_DB = numberInOrder++,
                                    OperationCode_DB = form.OperationCode,
                                    OperationDate_DB = form.OperationDate,
                                    PassportNumber_DB = form.PassportNumber,
                                    NameIOU_DB = form.NameIOU,
                                    FactoryNumber_DB = form.FactoryNumber,
                                    Mass_DB = form.Mass,
                                    CreatorOKPO_DB = form.CreatorOKPO,
                                    CreationDate_DB = form.CreationDate,
                                    SignedServicePeriod_DB = form.SignedServicePeriod,
                                    PropertyCode_DB = byte.TryParse(form.SignedServicePeriod, out var byteValue)
                                        ? byteValue
                                        : null,
                                    Owner_DB = form.Owner,
                                    DocumentVid_DB = byte.TryParse(form.DocumentVid, out byteValue)
                                        ? byteValue
                                        : null,
                                    DocumentNumber_DB = form.DocumentNumber,
                                    DocumentDate_DB = form.DocumentDate,
                                    ProviderOrRecieverOKPO_DB = form.ProviderOrRecieverOKPO,
                                    TransporterOKPO_DB = form.TransporterOKPO,
                                    PackName_DB = form.PackName,
                                    PackType_DB = form.PackType,
                                    PackNumber_DB = form.PackNumber
                                };
                                forms12List.Add(curForm);
                            }
                            currentOrg.Master_DB.Rows12.Add(forms12List);
                            break;
                        } 
                            
                        #endregion
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
                            ContentTitle = "Импорт из .raodb",
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

                foreach (var impReps in reportsJsonCollection)
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

                    if (baseReps11 != null)
                    {
                        await ProcessIfHasReports11(baseReps11, impReps);
                    }
                    else if (baseReps21 != null)
                    {
                        await ProcessIfHasReports21(baseReps21, impReps);
                    }
                    else if (baseReps11 == null && baseReps21 == null)
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
                            MainWindowVM.LocalReports.Reports_Collection.Add(impReps);
                            AtLeastOneImportDone = true;

                            #region LoggerImport

                            var sortedRepList = impReps.Report_Collection
                                .OrderBy(x => x.FormNum_DB)
                                .ThenBy(x => StaticStringMethods.StringReverse(x.StartPeriod_DB))
                                .ToList();
                            foreach (var rep in sortedRepList)
                            {
                                ImpRepCorNum = rep.CorrectionNumber_DB;
                                ImpRepFormCount = rep.Rows11.Count + rep.Rows12.Count + rep.Rows13.Count + rep.Rows14.Count + rep.Rows15.Count + rep.Rows16.Count + rep.Rows17.Count + rep.Rows18.Count + rep.Rows19.Count;
                                ImpRepFormNum = rep.FormNum_DB;
                                ImpRepStartPeriod = rep.StartPeriod_DB;
                                ImpRepEndPeriod = rep.EndPeriod_DB;
                                Act = "\t\t\t";
                                LoggerImportDTO = new LoggerImportDTO
                                {
                                    Act = Act, CorNum = ImpRepCorNum, CurrentLogLine = CurrentLogLine,
                                    EndPeriod = ImpRepEndPeriod,
                                    FormCount = ImpRepFormCount, FormNum = ImpRepFormNum,
                                    StartPeriod = ImpRepStartPeriod,
                                    Okpo = BaseRepsOkpo, OperationDate = OperationDate, RegNum = BaseRepsRegNum,
                                    ShortName = BaseRepsShortName, SourceFileFullPath = SourceFile!.FullName,
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
            catch (Exception e)
            {
                //ignore
            }
        }    
        await MainWindowVM.LocalReports.Reports_Collection.QuickSortAsync();
        await StaticConfiguration.DBModel.SaveChangesAsync().ConfigureAwait(false);

        var suffix = answer.Length.ToString().EndsWith('1') && !answer.Length.ToString().EndsWith("11")
            ? "а"
            : "ов";
        if (AtLeastOneImportDone)
        {
            #region MessageImportDone
            
            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Импорт из .raodb",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Импорт {countReadFiles} из {answer.Length} файл{suffix} .raodb успешно завершен.",
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
                    ContentTitle = "Импорт из .raodb",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Импорт из {answer.Length} файл{suffix} .raodb был отменен.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow);

            #endregion
        }
    }
}