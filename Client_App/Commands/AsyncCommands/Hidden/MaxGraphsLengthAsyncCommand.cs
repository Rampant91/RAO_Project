using Client_App.Views;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Linq;
using Models.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Client_App.Commands.AsyncCommands.ExcelExport;

namespace Client_App.Commands.AsyncCommands.Hidden;

// Excel -> Максимальное число символов в каждой колонке
public class MaxGraphsLengthAsyncCommand : ExcelBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var mainWindow = Desktop.MainWindow as MainWindow;
        var fileName = $"Максимальное количество символов по графам_{Assembly.GetExecutingAssembly().GetName().Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;

        var repsList = new List<Reports>();
        repsList.AddRange(ReportsStorage.LocalReports.Reports_Collection.OrderBy(x => x.Master_DB.RegNoRep.Value));
        HashSet<string> formNums = [];
        foreach (var rep in repsList
                     .SelectMany(reps => reps.Report_Collection)
                     .OrderBy(x => byte.Parse(x.FormNum_DB.Split('.')[0]))
                     .ThenBy(x => byte.Parse(x.FormNum_DB.Split('.')[1]))
                     .ToList())
        {
            formNums.Add(rep.FormNum_DB);
        }
        WorksheetPrim = excelPackage.Workbook.Worksheets.Add("Примечания");
        foreach (var formNum in formNums)
        {
            Worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {formNum}");
            FormsHeaders(formNum);
            await CountFormsGraphMaxLength(formNum);
        }
        NotesHeaders();
        excelPackage.Workbook.Worksheets.MoveToEnd(WorksheetPrim.Name);
        await CountNotesLength();

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts);
    }

    #region CountFormsGraphMaxLength

    private async Task CountFormsGraphMaxLength(string formNum)
    {
        await using var db = new DBModel(StaticConfiguration.DBPath);

        switch (formNum)
        {
            case "1.0":
            {
                #region Form10

                Worksheet.Cells[2, 1].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                Worksheet.Cells[2, 2].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.RegNo_DB == null ? 0 : x.RegNo_DB.Length);
                Worksheet.Cells[2, 3].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.OrganUprav_DB == null ? 0 : x.OrganUprav_DB.Length);
                Worksheet.Cells[2, 4].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.SubjectRF_DB == null ? 0 : x.SubjectRF_DB.Length);
                Worksheet.Cells[2, 5].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.JurLico_DB == null ? 0 : x.JurLico_DB.Length);
                Worksheet.Cells[2, 6].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.ShortJurLico_DB == null ? 0 : x.ShortJurLico_DB.Length);
                Worksheet.Cells[2, 7].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.JurLicoAddress_DB == null ? 0 : x.JurLicoAddress_DB.Length);
                Worksheet.Cells[2, 8].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.JurLicoFactAddress_DB == null ? 0 : x.JurLicoFactAddress_DB.Length);
                Worksheet.Cells[2, 9].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.GradeFIO_DB == null ? 0 : x.GradeFIO_DB.Length);
                Worksheet.Cells[2, 10].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.Telephone_DB == null ? 0 : x.Telephone_DB.Length);
                Worksheet.Cells[2, 11].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.Fax_DB == null ? 0 : x.Fax_DB.Length);
                Worksheet.Cells[2, 12].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.Email_DB == null ? 0 : x.Email_DB.Length);
                Worksheet.Cells[2, 13].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.Okpo_DB == null ? 0 : x.Okpo_DB.Length);
                Worksheet.Cells[2, 14].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.Okved_DB == null ? 0 : x.Okved_DB.Length);
                Worksheet.Cells[2, 15].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.Okogu_DB == null ? 0 : x.Okogu_DB.Length);
                Worksheet.Cells[2, 16].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.Oktmo_DB == null ? 0 : x.Oktmo_DB.Length);
                Worksheet.Cells[2, 17].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.Inn_DB == null ? 0 : x.Inn_DB.Length);
                Worksheet.Cells[2, 18].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.Kpp_DB == null ? 0 : x.Kpp_DB.Length);
                Worksheet.Cells[2, 19].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.Okopf_DB == null ? 0 : x.Okopf_DB.Length);
                Worksheet.Cells[2, 20].Value = await db.form_10
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .MaxAsync(x => x.Okfs_DB == null ? 0 : x.Okfs_DB.Length);

                #endregion

                break;
            }
            case "1.1":
            {
                #region Form11

                    Worksheet.Cells[2, 1].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationCode_DB == null ? 0 : x.OperationCode_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationDate_DB == null ? 0 : x.OperationDate_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PassportNumber_DB == null ? 0 : x.PassportNumber_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Type_DB == null ? 0 : x.Type_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Radionuclids_DB == null ? 0 : x.Radionuclids_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FactoryNumber_DB == null ? 0 : x.FactoryNumber_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Quantity_DB == null ? 0 : x.Quantity_DB.ToString()!.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Activity_DB == null ? 0 : x.Activity_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CreatorOKPO_DB == null ? 0 : x.CreatorOKPO_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CreationDate_DB == null ? 0 : x.CreationDate_DB.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Category_DB == null ? 0 : x.Category_DB.ToString()!.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.SignedServicePeriod_DB == null ? 0 : x.SignedServicePeriod_DB.ToString()!.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PropertyCode_DB == null ? 0 : x.PropertyCode_DB.ToString()!.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Owner_DB == null ? 0 : x.Owner_DB.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentVid_DB == null ? 0 : x.DocumentVid_DB.ToString()!.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentNumber_DB == null ? 0 : x.DocumentNumber_DB.Length);
                    Worksheet.Cells[2, 18].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentDate_DB == null ? 0 : x.DocumentDate_DB.Length);
                    Worksheet.Cells[2, 19].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ProviderOrRecieverOKPO_DB == null ? 0 : x.ProviderOrRecieverOKPO_DB.Length);
                    Worksheet.Cells[2, 20].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransporterOKPO_DB == null ? 0 : x.TransporterOKPO_DB.Length);
                    Worksheet.Cells[2, 21].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackName_DB == null ? 0 : x.PackName_DB.Length);
                    Worksheet.Cells[2, 22].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackType_DB == null ? 0 : x.PackType_DB.Length);
                    Worksheet.Cells[2, 23].Value = await db.form_11
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackNumber_DB == null ? 0 : x.PackNumber_DB.Length);

                    #endregion

                break;
            }
            case "1.2":
            {
                #region Form12

                    Worksheet.Cells[2, 1].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationCode_DB == null ? 0 : x.OperationCode_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationDate_DB == null ? 0 : x.OperationDate_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PassportNumber_DB == null ? 0 : x.PassportNumber_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NameIOU_DB == null ? 0 : x.NameIOU_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FactoryNumber_DB == null ? 0 : x.FactoryNumber_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Mass_DB == null ? 0 : x.Mass_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CreatorOKPO_DB == null ? 0 : x.CreatorOKPO_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CreationDate_DB == null ? 0 : x.CreationDate_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.SignedServicePeriod_DB == null ? 0 : x.SignedServicePeriod_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PropertyCode_DB == null ? 0 : x.PropertyCode_DB.ToString()!.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Owner_DB == null ? 0 : x.Owner_DB.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentVid_DB == null ? 0 : x.DocumentVid_DB.ToString()!.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentNumber_DB == null ? 0 : x.DocumentNumber_DB.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentDate_DB == null ? 0 : x.DocumentDate_DB.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ProviderOrRecieverOKPO_DB == null ? 0 : x.ProviderOrRecieverOKPO_DB.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransporterOKPO_DB == null ? 0 : x.TransporterOKPO_DB.Length);
                    Worksheet.Cells[2, 18].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackName_DB == null ? 0 : x.PackName_DB.Length);
                    Worksheet.Cells[2, 19].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackType_DB == null ? 0 : x.PackType_DB.Length);
                    Worksheet.Cells[2, 20].Value = await db.form_12
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackNumber_DB == null ? 0 : x.PackNumber_DB.Length);

                    #endregion

                break;
            }
            case "1.3":
            {
                #region Form13

                    Worksheet.Cells[2, 1].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationCode_DB == null ? 0 : x.OperationCode_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationDate_DB == null ? 0 : x.OperationDate_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PassportNumber_DB == null ? 0 : x.PassportNumber_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Type_DB == null ? 0 : x.Type_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Radionuclids_DB == null ? 0 : x.Radionuclids_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FactoryNumber_DB == null ? 0 : x.FactoryNumber_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Activity_DB == null ? 0 : x.Activity_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CreatorOKPO_DB == null ? 0 : x.CreatorOKPO_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CreationDate_DB == null ? 0 : x.CreationDate_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AggregateState_DB == null ? 0 : x.AggregateState_DB.ToString()!.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PropertyCode_DB == null ? 0 : x.PropertyCode_DB.ToString()!.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Owner_DB == null ? 0 : x.Owner_DB.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentVid_DB == null ? 0 : x.DocumentVid_DB.ToString()!.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentNumber_DB == null ? 0 : x.DocumentNumber_DB.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentDate_DB == null ? 0 : x.DocumentDate_DB.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ProviderOrRecieverOKPO_DB == null ? 0 : x.ProviderOrRecieverOKPO_DB.Length);
                    Worksheet.Cells[2, 18].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransporterOKPO_DB == null ? 0 : x.TransporterOKPO_DB.Length);
                    Worksheet.Cells[2, 19].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackName_DB == null ? 0 : x.PackName_DB.Length);
                    Worksheet.Cells[2, 20].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackType_DB == null ? 0 : x.PackType_DB.Length);
                    Worksheet.Cells[2, 21].Value = await db.form_13
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackNumber_DB == null ? 0 : x.PackNumber_DB.Length);

                    #endregion

                break;
            }
            case "1.4":
            {
                #region Form14

                    Worksheet.Cells[2, 1].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationCode_DB == null ? 0 : x.OperationCode_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationDate_DB == null ? 0 : x.OperationDate_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PassportNumber_DB == null ? 0 : x.PassportNumber_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Name_DB == null ? 0 : x.Name_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Sort_DB == null ? 0 : x.Sort_DB.ToString()!.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Radionuclids_DB == null ? 0 : x.Radionuclids_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Activity_DB == null ? 0 : x.Activity_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ActivityMeasurementDate_DB == null ? 0 : x.ActivityMeasurementDate_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Volume_DB == null ? 0 : x.Volume_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Mass_DB == null ? 0 : x.Mass_DB.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AggregateState_DB == null ? 0 : x.AggregateState_DB.ToString()!.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PropertyCode_DB == null ? 0 : x.PropertyCode_DB.ToString()!.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Owner_DB == null ? 0 : x.Owner_DB.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentVid_DB == null ? 0 : x.DocumentVid_DB.ToString()!.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentNumber_DB == null ? 0 : x.DocumentNumber_DB.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentDate_DB == null ? 0 : x.DocumentDate_DB.Length);
                    Worksheet.Cells[2, 18].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ProviderOrRecieverOKPO_DB == null ? 0 : x.ProviderOrRecieverOKPO_DB.Length);
                    Worksheet.Cells[2, 19].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransporterOKPO_DB == null ? 0 : x.TransporterOKPO_DB.Length);
                    Worksheet.Cells[2, 20].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackName_DB == null ? 0 : x.PackName_DB.Length);
                    Worksheet.Cells[2, 21].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackType_DB == null ? 0 : x.PackType_DB.Length);
                    Worksheet.Cells[2, 22].Value = await db.form_14
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackNumber_DB == null ? 0 : x.PackNumber_DB.Length);

                    #endregion

                break;
            }
            case "1.5":
            {
                #region Form15

                    Worksheet.Cells[2, 1].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationCode_DB == null ? 0 : x.OperationCode_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationDate_DB == null ? 0 : x.OperationDate_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PassportNumber_DB == null ? 0 : x.PassportNumber_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Type_DB == null ? 0 : x.Type_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Radionuclids_DB == null ? 0 : x.Radionuclids_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FactoryNumber_DB == null ? 0 : x.FactoryNumber_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Quantity_DB == null ? 0 : x.Quantity_DB.ToString()!.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Activity_DB == null ? 0 : x.Activity_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CreationDate_DB == null ? 0 : x.CreationDate_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StatusRAO_DB == null ? 0 : x.StatusRAO_DB.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentVid_DB == null ? 0 : x.DocumentVid_DB.ToString()!.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentNumber_DB == null ? 0 : x.DocumentNumber_DB.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentDate_DB == null ? 0 : x.DocumentDate_DB.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ProviderOrRecieverOKPO_DB == null ? 0 : x.ProviderOrRecieverOKPO_DB.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransporterOKPO_DB == null ? 0 : x.TransporterOKPO_DB.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackName_DB == null ? 0 : x.PackName_DB.Length);
                    Worksheet.Cells[2, 18].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackType_DB == null ? 0 : x.PackType_DB.Length);
                    Worksheet.Cells[2, 19].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackNumber_DB == null ? 0 : x.PackNumber_DB.Length);
                    Worksheet.Cells[2, 20].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceName_DB == null ? 0 : x.StoragePlaceName_DB.Length);
                    Worksheet.Cells[2, 21].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceCode_DB == null ? 0 : x.StoragePlaceCode_DB.Length);
                    Worksheet.Cells[2, 22].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.RefineOrSortRAOCode_DB == null ? 0 : x.RefineOrSortRAOCode_DB.Length);
                    Worksheet.Cells[2, 23].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Subsidy_DB == null ? 0 : x.Subsidy_DB.Length);
                    Worksheet.Cells[2, 24].Value = await db.form_15
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FcpNumber_DB == null ? 0 : x.FcpNumber_DB.Length);

                    #endregion

                break;
            }
            case "1.6":
            {
                #region Form16

                    Worksheet.Cells[2, 1].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationCode_DB == null ? 0 : x.OperationCode_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationDate_DB == null ? 0 : x.OperationDate_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CodeRAO_DB == null ? 0 : x.CodeRAO_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StatusRAO_DB == null ? 0 : x.StatusRAO_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Volume_DB == null ? 0 : x.Volume_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Mass_DB == null ? 0 : x.Mass_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityOZIII_DB == null ? 0 : x.QuantityOZIII_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MainRadionuclids_DB == null ? 0 : x.MainRadionuclids_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TritiumActivity_DB == null ? 0 : x.TritiumActivity_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.BetaGammaActivity_DB == null ? 0 : x.BetaGammaActivity_DB.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AlphaActivity_DB == null ? 0 : x.AlphaActivity_DB.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransuraniumActivity_DB == null ? 0 : x.TransuraniumActivity_DB.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ActivityMeasurementDate_DB == null ? 0 : x.ActivityMeasurementDate_DB.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentVid_DB == null ? 0 : x.DocumentVid_DB.ToString()!.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentNumber_DB == null ? 0 : x.DocumentNumber_DB.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentDate_DB == null ? 0 : x.DocumentDate_DB.Length);
                    Worksheet.Cells[2, 18].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ProviderOrRecieverOKPO_DB == null ? 0 : x.ProviderOrRecieverOKPO_DB.Length);
                    Worksheet.Cells[2, 19].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransporterOKPO_DB == null ? 0 : x.TransporterOKPO_DB.Length);
                    Worksheet.Cells[2, 20].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceName_DB == null ? 0 : x.StoragePlaceName_DB.Length);
                    Worksheet.Cells[2, 21].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceCode_DB == null ? 0 : x.StoragePlaceCode_DB.Length);
                    Worksheet.Cells[2, 22].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.RefineOrSortRAOCode_DB == null ? 0 : x.RefineOrSortRAOCode_DB.Length);
                    Worksheet.Cells[2, 23].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackName_DB == null ? 0 : x.PackName_DB.Length);
                    Worksheet.Cells[2, 24].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackType_DB == null ? 0 : x.PackType_DB.Length);
                    Worksheet.Cells[2, 25].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackNumber_DB == null ? 0 : x.PackNumber_DB.Length);
                    Worksheet.Cells[2, 26].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Subsidy_DB == null ? 0 : x.Subsidy_DB.Length);
                    Worksheet.Cells[2, 27].Value = await db.form_16
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FcpNumber_DB == null ? 0 : x.FcpNumber_DB.Length);

                    #endregion

                break;
            }
            case "1.7":
            {
                #region Form17

                    Worksheet.Cells[2, 1].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationCode_DB == null ? 0 : x.OperationCode_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationDate_DB == null ? 0 : x.OperationDate_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackName_DB == null ? 0 : x.PackName_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackType_DB == null ? 0 : x.PackType_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackFactoryNumber_DB == null ? 0 : x.PackFactoryNumber_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackNumber_DB == null ? 0 : x.PackNumber_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FormingDate_DB == null ? 0 : x.FormingDate_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PassportNumber_DB == null ? 0 : x.PassportNumber_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Volume_DB == null ? 0 : x.Volume_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Mass_DB == null ? 0 : x.Mass_DB.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Radionuclids_DB == null ? 0 : x.Radionuclids_DB.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.SpecificActivity_DB == null ? 0 : x.SpecificActivity_DB.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentVid_DB == null ? 0 : x.DocumentVid_DB.ToString()!.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentNumber_DB == null ? 0 : x.DocumentNumber_DB.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentDate_DB == null ? 0 : x.DocumentDate_DB.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ProviderOrRecieverOKPO_DB == null ? 0 : x.ProviderOrRecieverOKPO_DB.Length);
                    Worksheet.Cells[2, 18].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransporterOKPO_DB == null ? 0 : x.TransporterOKPO_DB.Length);
                    Worksheet.Cells[2, 19].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceName_DB == null ? 0 : x.StoragePlaceName_DB.Length);
                    Worksheet.Cells[2, 20].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceCode_DB == null ? 0 : x.StoragePlaceCode_DB.Length);
                    Worksheet.Cells[2, 21].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CodeRAO_DB == null ? 0 : x.CodeRAO_DB.Length);
                    Worksheet.Cells[2, 22].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StatusRAO_DB == null ? 0 : x.StatusRAO_DB.Length);
                    Worksheet.Cells[2, 23].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.VolumeOutOfPack_DB == null ? 0 : x.VolumeOutOfPack_DB.Length);
                    Worksheet.Cells[2, 24].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassOutOfPack_DB == null ? 0 : x.MassOutOfPack_DB.Length);
                    Worksheet.Cells[2, 25].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Quantity_DB == null ? 0 : x.Quantity_DB.Length);
                    Worksheet.Cells[2, 26].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TritiumActivity_DB == null ? 0 : x.TritiumActivity_DB.Length);
                    Worksheet.Cells[2, 27].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.BetaGammaActivity_DB == null ? 0 : x.BetaGammaActivity_DB.Length);
                    Worksheet.Cells[2, 28].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AlphaActivity_DB == null ? 0 : x.AlphaActivity_DB.Length);
                    Worksheet.Cells[2, 29].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransuraniumActivity_DB == null ? 0 : x.TransuraniumActivity_DB.Length);
                    Worksheet.Cells[2, 30].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.RefineOrSortRAOCode_DB == null ? 0 : x.RefineOrSortRAOCode_DB.Length);
                    Worksheet.Cells[2, 31].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Subsidy_DB == null ? 0 : x.Subsidy_DB.Length);
                    Worksheet.Cells[2, 32].Value = await db.form_17
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FcpNumber_DB == null ? 0 : x.FcpNumber_DB.Length);

                    #endregion

                break;
            }
            case "1.8":
            {
                #region Form18

                    Worksheet.Cells[2, 1].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationCode_DB == null ? 0 : x.OperationCode_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationDate_DB == null ? 0 : x.OperationDate_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.IndividualNumberZHRO_DB == null ? 0 : x.IndividualNumberZHRO_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PassportNumber_DB == null ? 0 : x.PassportNumber_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Volume6_DB == null ? 0 : x.Volume6_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Mass7_DB == null ? 0 : x.Mass7_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.SaltConcentration_DB == null ? 0 : x.SaltConcentration_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Radionuclids_DB == null ? 0 : x.Radionuclids_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.SpecificActivity_DB == null ? 0 : x.SpecificActivity_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentVid_DB == null ? 0 : x.DocumentVid_DB.ToString()!.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentNumber_DB == null ? 0 : x.DocumentNumber_DB.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentDate_DB == null ? 0 : x.DocumentDate_DB.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ProviderOrRecieverOKPO_DB == null ? 0 : x.ProviderOrRecieverOKPO_DB.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransporterOKPO_DB == null ? 0 : x.TransporterOKPO_DB.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceName_DB == null ? 0 : x.StoragePlaceName_DB.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceCode_DB == null ? 0 : x.StoragePlaceCode_DB.Length);
                    Worksheet.Cells[2, 18].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CodeRAO_DB == null ? 0 : x.CodeRAO_DB.Length);
                    Worksheet.Cells[2, 19].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StatusRAO_DB == null ? 0 : x.StatusRAO_DB.Length);
                    Worksheet.Cells[2, 20].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Volume20_DB == null ? 0 : x.Volume20_DB.Length);
                    Worksheet.Cells[2, 21].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Mass21_DB == null ? 0 : x.Mass21_DB.Length);
                    Worksheet.Cells[2, 22].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TritiumActivity_DB == null ? 0 : x.TritiumActivity_DB.Length);
                    Worksheet.Cells[2, 23].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.BetaGammaActivity_DB == null ? 0 : x.BetaGammaActivity_DB.Length);
                    Worksheet.Cells[2, 24].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AlphaActivity_DB == null ? 0 : x.AlphaActivity_DB.Length);
                    Worksheet.Cells[2, 25].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransuraniumActivity_DB == null ? 0 : x.TransuraniumActivity_DB.Length);
                    Worksheet.Cells[2, 26].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.RefineOrSortRAOCode_DB == null ? 0 : x.RefineOrSortRAOCode_DB.Length);
                    Worksheet.Cells[2, 27].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Subsidy_DB == null ? 0 : x.Subsidy_DB.Length);
                    Worksheet.Cells[2, 28].Value = await db.form_18
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FcpNumber_DB == null ? 0 : x.FcpNumber_DB.Length);

                    #endregion

                break;
            }
            case "1.9":
            {
                #region Form19

                    Worksheet.Cells[2, 1].Value = await db.form_19
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_19
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationCode_DB == null ? 0 : x.OperationCode_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_19
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationDate_DB == null ? 0 : x.OperationDate_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_19
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentVid_DB == null ? 0 : x.DocumentVid_DB.ToString()!.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_19
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentNumber_DB == null ? 0 : x.DocumentNumber_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_19
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentDate_DB == null ? 0 : x.DocumentDate_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_19
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CodeTypeAccObject_DB == null ? 0 : x.CodeTypeAccObject_DB.ToString()!.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_19
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Radionuclids_DB == null ? 0 : x.Radionuclids_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_19
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Activity_DB == null ? 0 : x.Activity_DB.Length);

                    #endregion

                break;
            }
            case "2.0":
            {
                #region Form20

                    Worksheet.Cells[2, 1].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.RegNo_DB == null ? 0 : x.RegNo_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OrganUprav_DB == null ? 0 : x.OrganUprav_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.SubjectRF_DB == null ? 0 : x.SubjectRF_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.JurLico_DB == null ? 0 : x.JurLico_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ShortJurLico_DB == null ? 0 : x.ShortJurLico_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.JurLicoAddress_DB == null ? 0 : x.JurLicoAddress_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.JurLicoFactAddress_DB == null ? 0 : x.JurLicoFactAddress_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.GradeFIO_DB == null ? 0 : x.GradeFIO_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Telephone_DB == null ? 0 : x.Telephone_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Fax_DB == null ? 0 : x.Fax_DB.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Email_DB == null ? 0 : x.Email_DB.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Okpo_DB == null ? 0 : x.Okpo_DB.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Okved_DB == null ? 0 : x.Okved_DB.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Okogu_DB == null ? 0 : x.Okogu_DB.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Oktmo_DB == null ? 0 : x.Oktmo_DB.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Inn_DB == null ? 0 : x.Inn_DB.Length);
                    Worksheet.Cells[2, 18].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Kpp_DB == null ? 0 : x.Kpp_DB.Length);
                    Worksheet.Cells[2, 19].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Okopf_DB == null ? 0 : x.Okopf_DB.Length);
                    Worksheet.Cells[2, 20].Value = await db.form_20
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Okfs_DB == null ? 0 : x.Okfs_DB.Length);

                    #endregion

                break;
            }
            case "2.1":
            {
                #region Form21

                    Worksheet.Cells[2, 1].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.RefineMachineName_DB == null ? 0 : x.RefineMachineName_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MachineCode == null ? 0 : x.MachineCode.ToString()!.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MachinePower_DB == null ? 0 : x.MachinePower_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberOfHoursPerYear_DB == null ? 0 : x.NumberOfHoursPerYear_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CodeRAOIn_DB == null ? 0 : x.CodeRAOIn_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StatusRAOIn_DB == null ? 0 : x.StatusRAOIn_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.VolumeIn_DB == null ? 0 : x.VolumeIn_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassIn_DB == null ? 0 : x.MassIn_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityIn_DB == null ? 0 : x.QuantityIn_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TritiumActivityIn_DB == null ? 0 : x.TritiumActivityIn_DB.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.BetaGammaActivityIn_DB == null ? 0 : x.BetaGammaActivityIn_DB.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AlphaActivityIn_DB == null ? 0 : x.AlphaActivityIn_DB.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransuraniumActivityIn_DB == null ? 0 : x.TransuraniumActivityIn_DB.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CodeRAOout_DB == null ? 0 : x.CodeRAOout_DB.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StatusRAOout_DB == null ? 0 : x.StatusRAOout_DB.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.VolumeOut_DB == null ? 0 : x.VolumeOut_DB.Length);
                    Worksheet.Cells[2, 18].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassOut_DB == null ? 0 : x.MassOut_DB.Length);
                    Worksheet.Cells[2, 19].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityOZIIIout_DB == null ? 0 : x.QuantityOZIIIout_DB.Length);
                    Worksheet.Cells[2, 20].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TritiumActivityOut_DB == null ? 0 : x.TritiumActivityOut_DB.Length);
                    Worksheet.Cells[2, 21].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.BetaGammaActivityOut_DB == null ? 0 : x.BetaGammaActivityOut_DB.Length);
                    Worksheet.Cells[2, 22].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AlphaActivityOut_DB == null ? 0 : x.AlphaActivityOut_DB.Length);
                    Worksheet.Cells[2, 23].Value = await db.form_21
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransuraniumActivityOut_DB == null ? 0 : x.TransuraniumActivityOut_DB.Length);

                    #endregion

                break;
            }
            case "2.2":
            {
                #region Form22

                    Worksheet.Cells[2, 1].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceName_DB == null ? 0 : x.StoragePlaceName_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceCode_DB == null ? 0 : x.StoragePlaceCode_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackName_DB == null ? 0 : x.PackName_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackType_DB == null ? 0 : x.PackType_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PackQuantity_DB == null ? 0 : x.PackQuantity_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CodeRAO_DB == null ? 0 : x.CodeRAO_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StatusRAO_DB == null ? 0 : x.StatusRAO_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.VolumeOutOfPack_DB == null ? 0 : x.VolumeOutOfPack_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.VolumeInPack_DB == null ? 0 : x.VolumeInPack_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassOutOfPack_DB == null ? 0 : x.MassOutOfPack_DB.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassInPack_DB == null ? 0 : x.MassInPack_DB.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityOZIII_DB == null ? 0 : x.QuantityOZIII_DB.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TritiumActivity_DB == null ? 0 : x.TritiumActivity_DB.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.BetaGammaActivity_DB == null ? 0 : x.BetaGammaActivity_DB.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AlphaActivity_DB == null ? 0 : x.AlphaActivity_DB.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TransuraniumActivity_DB == null ? 0 : x.TransuraniumActivity_DB.Length);
                    Worksheet.Cells[2, 18].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MainRadionuclids_DB == null ? 0 : x.MainRadionuclids_DB.Length);
                    Worksheet.Cells[2, 19].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Subsidy_DB == null ? 0 : x.Subsidy_DB.Length);
                    Worksheet.Cells[2, 20].Value = await db.form_22
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FcpNumber_DB == null ? 0 : x.FcpNumber_DB.Length);

                    #endregion

                break;
            }
            case "2.3":
            {
                #region Form23

                    Worksheet.Cells[2, 1].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceName_DB == null ? 0 : x.StoragePlaceName_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceCode_DB == null ? 0 : x.StoragePlaceCode_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ProjectVolume_DB == null ? 0 : x.ProjectVolume_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CodeRAO_DB == null ? 0 : x.CodeRAO_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Volume_DB == null ? 0 : x.Volume_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Mass_DB == null ? 0 : x.Mass_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityOZIII_DB == null ? 0 : x.QuantityOZIII_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.SummaryActivity_DB == null ? 0 : x.SummaryActivity_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentNumber_DB == null ? 0 : x.DocumentNumber_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentDate_DB == null ? 0 : x.DocumentDate_DB.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ExpirationDate_DB == null ? 0 : x.ExpirationDate_DB.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_23
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DocumentName_DB == null ? 0 : x.DocumentName_DB.Length);

                    #endregion

                break;
            }
            case "2.4":
            {
                #region Form24

                    Worksheet.Cells[2, 1].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CodeOYAT_DB == null ? 0 : x.CodeOYAT_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FcpNumber_DB == null ? 0 : x.FcpNumber_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassCreated_DB == null ? 0 : x.MassCreated_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityCreated_DB == null ? 0 : x.QuantityCreated_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassFromAnothers_DB == null ? 0 : x.MassFromAnothers_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityFromAnothers_DB == null ? 0 : x.QuantityFromAnothers_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassFromAnothersImported_DB == null ? 0 : x.MassFromAnothersImported_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityFromAnothersImported_DB == null ? 0 : x.QuantityFromAnothersImported_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassAnotherReasons_DB == null ? 0 : x.MassAnotherReasons_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityAnotherReasons_DB == null ? 0 : x.QuantityAnotherReasons_DB.Length);
                    Worksheet.Cells[2, 12].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassTransferredToAnother_DB == null ? 0 : x.MassTransferredToAnother_DB.Length);
                    Worksheet.Cells[2, 13].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityTransferredToAnother_DB == null ? 0 : x.QuantityTransferredToAnother_DB.Length);
                    Worksheet.Cells[2, 14].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassRefined_DB == null ? 0 : x.MassRefined_DB.Length);
                    Worksheet.Cells[2, 15].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityRefined_DB == null ? 0 : x.QuantityRefined_DB.Length);
                    Worksheet.Cells[2, 16].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MassRemovedFromAccount_DB == null ? 0 : x.MassRemovedFromAccount_DB.Length);
                    Worksheet.Cells[2, 17].Value = await db.form_24
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.QuantityRemovedFromAccount_DB == null ? 0 : x.QuantityRemovedFromAccount_DB.Length);

                    #endregion

                break;
            }
            case "2.5":
            {
                #region Form25

                    Worksheet.Cells[2, 1].Value = await db.form_25
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_25
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceName_DB == null ? 0 : x.StoragePlaceName_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_25
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.StoragePlaceCode_DB == null ? 0 : x.StoragePlaceCode_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_25
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CodeOYAT_DB == null ? 0 : x.CodeOYAT_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_25
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FcpNumber_DB == null ? 0 : x.FcpNumber_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_25
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FuelMass_DB == null ? 0 : x.FuelMass_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_25
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.CellMass_DB == null ? 0 : x.CellMass_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_25
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Quantity_DB == null ? 0 : x.Quantity_DB.ToString()!.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_25
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AlphaActivity_DB == null ? 0 : x.AlphaActivity_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_25
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.BetaGammaActivity_DB == null ? 0 : x.BetaGammaActivity_DB.Length);

                    #endregion

                break;
            }
            case "2.6":
            {
                #region Form26

                    Worksheet.Cells[2, 1].Value = await db.form_26
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_26
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ObservedSourceNumber_DB == null ? 0 : x.ObservedSourceNumber_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_26
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ControlledAreaName_DB == null ? 0 : x.ControlledAreaName_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_26
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.SupposedWasteSource_DB == null ? 0 : x.SupposedWasteSource_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_26
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.DistanceToWasteSource_DB == null ? 0 : x.DistanceToWasteSource_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_26
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.TestDepth_DB == null ? 0 : x.TestDepth_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_26
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.RadionuclidName_DB == null ? 0 : x.RadionuclidName_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_26
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AverageYearConcentration_DB == null ? 0 : x.AverageYearConcentration_DB.ToString()!.Length);

                    #endregion

                break;
            }
            case "2.7":
            {
                #region Form27

                    Worksheet.Cells[2, 1].Value = await db.form_27
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_27
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ObservedSourceNumber_DB == null ? 0 : x.ObservedSourceNumber_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_27
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.RadionuclidName_DB == null ? 0 : x.RadionuclidName_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_27
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AllowedWasteValue_DB == null ? 0 : x.AllowedWasteValue_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_27
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FactedWasteValue_DB == null ? 0 : x.FactedWasteValue_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_27
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.WasteOutbreakPreviousYear_DB == null ? 0 : x.WasteOutbreakPreviousYear_DB.Length);

                    #endregion

                break;
            }
            case "2.8":
            {
                #region Form28

                    Worksheet.Cells[2, 1].Value = await db.form_28
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_28
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.WasteSourceName_DB == null ? 0 : x.WasteSourceName_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_28
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.WasteRecieverName_DB == null ? 0 : x.WasteRecieverName_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_28
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.RecieverTypeCode_DB == null ? 0 : x.RecieverTypeCode_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_28
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PoolDistrictName_DB == null ? 0 : x.PoolDistrictName_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_28
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AllowedWasteRemovalVolume_DB == null ? 0 : x.AllowedWasteRemovalVolume_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_28
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.RemovedWasteVolume_DB == null ? 0 : x.RemovedWasteVolume_DB.Length);

                    #endregion

                break;
            }
            case "2.9":
            {
                #region Form29

                    Worksheet.Cells[2, 1].Value = await db.form_29
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_29
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.WasteSourceName_DB == null ? 0 : x.WasteSourceName_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_29
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.RadionuclidName_DB == null ? 0 : x.RadionuclidName_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_29
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AllowedActivity_DB == null ? 0 : x.AllowedActivity_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_29
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FactedActivity_DB == null ? 0 : x.FactedActivity_DB.Length);

                    #endregion

                break;
            }
            case "2.10":
            {
                #region Form210

                    Worksheet.Cells[2, 1].Value = await db.form_210
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_210
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.IndicatorName_DB == null ? 0 : x.IndicatorName_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_210
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PlotName_DB == null ? 0 : x.PlotName_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_210
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PlotKadastrNumber_DB == null ? 0 : x.PlotKadastrNumber_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_210
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PlotCode_DB == null ? 0 : x.PlotCode_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_210
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.InfectedArea_DB == null ? 0 : x.InfectedArea_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_210
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.AvgGammaRaysDosePower_DB == null ? 0 : x.AvgGammaRaysDosePower_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_210
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.MaxGammaRaysDosePower_DB == null ? 0 : x.MaxGammaRaysDosePower_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_210
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.WasteDensityAlpha_DB == null ? 0 : x.WasteDensityAlpha_DB.Length);
                    Worksheet.Cells[2, 10].Value = await db.form_210
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.WasteDensityBeta_DB == null ? 0 : x.WasteDensityBeta_DB.Length);
                    Worksheet.Cells[2, 11].Value = await db.form_210
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.FcpNumber_DB == null ? 0 : x.FcpNumber_DB.Length);

                    #endregion

                break;
            }
            case "2.11":
            {
                #region Form211

                    Worksheet.Cells[2, 1].Value = await db.form_211
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_211
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PlotName_DB == null ? 0 : x.PlotName_DB.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_211
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PlotKadastrNumber_DB == null ? 0 : x.PlotKadastrNumber_DB.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_211
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.PlotCode_DB == null ? 0 : x.PlotCode_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_211
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.InfectedArea_DB == null ? 0 : x.InfectedArea_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_211
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Radionuclids_DB == null ? 0 : x.Radionuclids_DB.Length);
                    Worksheet.Cells[2, 7].Value = await db.form_211
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.SpecificActivityOfPlot_DB == null ? 0 : x.SpecificActivityOfPlot_DB.Length);
                    Worksheet.Cells[2, 8].Value = await db.form_211
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.SpecificActivityOfLiquidPart_DB == null ? 0 : x.SpecificActivityOfLiquidPart_DB.Length);
                    Worksheet.Cells[2, 9].Value = await db.form_211
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.SpecificActivityOfDensePart_DB == null ? 0 : x.SpecificActivityOfDensePart_DB.Length);

                    #endregion

                break;
            }
            case "2.12":
            {
                #region Form212

                    Worksheet.Cells[2, 1].Value = await db.form_212
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.NumberInOrder_DB.ToString().Length);
                    Worksheet.Cells[2, 2].Value = await db.form_212
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.OperationCode_DB == null ? 0 : x.OperationCode_DB.ToString()!.Length);
                    Worksheet.Cells[2, 3].Value = await db.form_212
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ObjectTypeCode_DB == null ? 0 : x.ObjectTypeCode_DB.ToString()!.Length);
                    Worksheet.Cells[2, 4].Value = await db.form_212
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Radionuclids_DB == null ? 0 : x.Radionuclids_DB.Length);
                    Worksheet.Cells[2, 5].Value = await db.form_212
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.Activity_DB == null ? 0 : x.Activity_DB.Length);
                    Worksheet.Cells[2, 6].Value = await db.form_212
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .MaxAsync(x => x.ProviderOrRecieverOKPO_DB == null ? 0 : x.ProviderOrRecieverOKPO_DB.Length);

                    #endregion

                break;
            }
        }
    }

    #endregion

    #region CountNotes

    private async Task CountNotesLength()
    {
        await using var db = new DBModel(StaticConfiguration.DBPath);
        WorksheetPrim.Cells[2, 1].Value = await db.notes
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .MaxAsync(x => x.RowNumber_DB == null ? 0 : x.RowNumber_DB.Length);
        WorksheetPrim.Cells[2, 2].Value = await db.notes
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .MaxAsync(x => x.GraphNumber_DB == null ? 0 : x.GraphNumber_DB.Length);
        WorksheetPrim.Cells[2, 3].Value = await db.notes
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .MaxAsync(x => x.Comment_DB == null ? 0 : x.Comment_DB.Length);
    }

    #endregion

    #region Headers

    #region FormHeaders

    private void FormsHeaders(string formNum)
    {
        switch (formNum)
        {
            case "1.1":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "код";
                    Worksheet.Cells[1, 3].Value = "дата";
                    Worksheet.Cells[1, 4].Value = "номер паспорта (сертификата)";
                    Worksheet.Cells[1, 5].Value = "тип";
                    Worksheet.Cells[1, 6].Value = "радионуклиды";
                    Worksheet.Cells[1, 7].Value = "номер";
                    Worksheet.Cells[1, 8].Value = "количество, шт";
                    Worksheet.Cells[1, 9].Value = "суммарная активность, Бк";
                    Worksheet.Cells[1, 10].Value = "код ОКПО изготовителя";
                    Worksheet.Cells[1, 11].Value = "дата выпуска";
                    Worksheet.Cells[1, 12].Value = "категория";
                    Worksheet.Cells[1, 13].Value = "НСС, мес";
                    Worksheet.Cells[1, 14].Value = "код формы собственности";
                    Worksheet.Cells[1, 15].Value = "код ОКПО правообладателя";
                    Worksheet.Cells[1, 16].Value = "вид";
                    Worksheet.Cells[1, 17].Value = "номер";
                    Worksheet.Cells[1, 18].Value = "дата";
                    Worksheet.Cells[1, 19].Value = "поставщика или получателя";
                    Worksheet.Cells[1, 20].Value = "перевозчика";
                    Worksheet.Cells[1, 21].Value = "наименование";
                    Worksheet.Cells[1, 22].Value = "тип";
                    Worksheet.Cells[1, 23].Value = "номер";

                    #endregion

                    break;
                }
            case "1.2":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "код";
                    Worksheet.Cells[1, 3].Value = "дата";
                    Worksheet.Cells[1, 4].Value = "номер паспорта";
                    Worksheet.Cells[1, 5].Value = "наименование";
                    Worksheet.Cells[1, 6].Value = "номер";
                    Worksheet.Cells[1, 7].Value = "масса объединенного урана, кг";
                    Worksheet.Cells[1, 8].Value = "код ОКПО изготовителя";
                    Worksheet.Cells[1, 9].Value = "дата выпуска";
                    Worksheet.Cells[1, 10].Value = "НСС, мес";
                    Worksheet.Cells[1, 11].Value = "код формы собственности";
                    Worksheet.Cells[1, 12].Value = "код ОКПО правообладателя";
                    Worksheet.Cells[1, 13].Value = "вид";
                    Worksheet.Cells[1, 14].Value = "номер";
                    Worksheet.Cells[1, 15].Value = "дата";
                    Worksheet.Cells[1, 16].Value = "поставщика или получателя";
                    Worksheet.Cells[1, 17].Value = "перевозчика";
                    Worksheet.Cells[1, 18].Value = "наименование";
                    Worksheet.Cells[1, 19].Value = "тип";
                    Worksheet.Cells[1, 20].Value = "номер";

                    #endregion

                    break;
                }
            case "1.3":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "код";
                    Worksheet.Cells[1, 3].Value = "дата";
                    Worksheet.Cells[1, 4].Value = "номер паспорта";
                    Worksheet.Cells[1, 5].Value = "тип";
                    Worksheet.Cells[1, 6].Value = "радионуклиды";
                    Worksheet.Cells[1, 7].Value = "номер";
                    Worksheet.Cells[1, 8].Value = "активность, Бк";
                    Worksheet.Cells[1, 9].Value = "код ОКПО изготовителя";
                    Worksheet.Cells[1, 10].Value = "дата выпуска";
                    Worksheet.Cells[1, 11].Value = "агрегатное состояние";
                    Worksheet.Cells[1, 12].Value = "код формы собственности";
                    Worksheet.Cells[1, 13].Value = "код ОКПО правообладателя";
                    Worksheet.Cells[1, 14].Value = "вид";
                    Worksheet.Cells[1, 15].Value = "номер";
                    Worksheet.Cells[1, 16].Value = "дата";
                    Worksheet.Cells[1, 17].Value = "поставщика или получателя";
                    Worksheet.Cells[1, 18].Value = "перевозчика";
                    Worksheet.Cells[1, 19].Value = "наименование";
                    Worksheet.Cells[1, 20].Value = "тип";
                    Worksheet.Cells[1, 21].Value = "номер";

                    #endregion

                    break;
                }
            case "1.4":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "код";
                    Worksheet.Cells[1, 3].Value = "дата";
                    Worksheet.Cells[1, 4].Value = "номер паспорта";
                    Worksheet.Cells[1, 5].Value = "наименование";
                    Worksheet.Cells[1, 6].Value = "вид";
                    Worksheet.Cells[1, 7].Value = "радионуклиды";
                    Worksheet.Cells[1, 8].Value = "активность, Бк";
                    Worksheet.Cells[1, 9].Value = "дата измерения активности";
                    Worksheet.Cells[1, 10].Value = "объем, куб.м";
                    Worksheet.Cells[1, 11].Value = "масса, кг";
                    Worksheet.Cells[1, 12].Value = "агрегатное состояние";
                    Worksheet.Cells[1, 13].Value = "код формы собственности";
                    Worksheet.Cells[1, 14].Value = "код ОКПО правообладателя";
                    Worksheet.Cells[1, 15].Value = "вид";
                    Worksheet.Cells[1, 16].Value = "номер";
                    Worksheet.Cells[1, 17].Value = "дата";
                    Worksheet.Cells[1, 18].Value = "поставщика или получателя";
                    Worksheet.Cells[1, 19].Value = "перевозчика";
                    Worksheet.Cells[1, 20].Value = "наименование";
                    Worksheet.Cells[1, 21].Value = "тип";
                    Worksheet.Cells[1, 22].Value = "номер";

                    #endregion

                    break;
                }
            case "1.5":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "код";
                    Worksheet.Cells[1, 3].Value = "дата";
                    Worksheet.Cells[1, 4].Value = "номер паспорта (сертификата) ЗРИ, акта определения характеристик ОЗИИ";
                    Worksheet.Cells[1, 5].Value = "тип";
                    Worksheet.Cells[1, 6].Value = "радионуклиды";
                    Worksheet.Cells[1, 7].Value = "номер";
                    Worksheet.Cells[1, 8].Value = "количество, шт";
                    Worksheet.Cells[1, 9].Value = "суммарная активность, Бк";
                    Worksheet.Cells[1, 10].Value = "дата выпуска";
                    Worksheet.Cells[1, 11].Value = "статус РАО";
                    Worksheet.Cells[1, 12].Value = "вид";
                    Worksheet.Cells[1, 13].Value = "номер";
                    Worksheet.Cells[1, 14].Value = "дата";
                    Worksheet.Cells[1, 15].Value = "поставщика или получателя";
                    Worksheet.Cells[1, 16].Value = "перевозчика";
                    Worksheet.Cells[1, 17].Value = "наименование";
                    Worksheet.Cells[1, 18].Value = "тип";
                    Worksheet.Cells[1, 19].Value = "заводской номер";
                    Worksheet.Cells[1, 20].Value = "наименование";
                    Worksheet.Cells[1, 21].Value = "код";
                    Worksheet.Cells[1, 22].Value = "Код переработки / сортировки РАО";
                    Worksheet.Cells[1, 23].Value = "Субсидия, %";
                    Worksheet.Cells[1, 24].Value = "Номер мероприятия ФЦП";

                    #endregion

                    break;
                }
            case "1.6":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "код";
                    Worksheet.Cells[1, 3].Value = "дата";
                    Worksheet.Cells[1, 4].Value = "Код РАО";
                    Worksheet.Cells[1, 5].Value = "Статус РАО";
                    Worksheet.Cells[1, 6].Value = "объем без упаковки, куб.";
                    Worksheet.Cells[1, 7].Value = "масса без упаковки";
                    Worksheet.Cells[1, 8].Value = "количество ОЗИИИ";
                    Worksheet.Cells[1, 9].Value = "Основные радионуклиды";
                    Worksheet.Cells[1, 10].Value = "тритий";
                    Worksheet.Cells[1, 11].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 12].Value = "альфа-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 13].Value = "трансурановые радионуклиды";
                    Worksheet.Cells[1, 14].Value = "Дата измерения активности";
                    Worksheet.Cells[1, 15].Value = "вид";
                    Worksheet.Cells[1, 16].Value = "номер";
                    Worksheet.Cells[1, 17].Value = "дата";
                    Worksheet.Cells[1, 18].Value = "поставщика или получателя";
                    Worksheet.Cells[1, 19].Value = "перевозчика";
                    Worksheet.Cells[1, 20].Value = "наименование";
                    Worksheet.Cells[1, 21].Value = "код";
                    Worksheet.Cells[1, 22].Value = "Код переработки /";
                    Worksheet.Cells[1, 23].Value = "наименование";
                    Worksheet.Cells[1, 24].Value = "тип";
                    Worksheet.Cells[1, 25].Value = "номер упаковки";
                    Worksheet.Cells[1, 26].Value = "Субсидия, %";
                    Worksheet.Cells[1, 27].Value = "Номер мероприятия ФЦП";

                    #endregion

                    break;
                }
            case "1.7":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "код";
                    Worksheet.Cells[1, 3].Value = "дата";
                    Worksheet.Cells[1, 4].Value = "наименование";
                    Worksheet.Cells[1, 5].Value = "тип";
                    Worksheet.Cells[1, 6].Value = "заводской номер";
                    Worksheet.Cells[1, 7].Value = "номер упаковки (идентификационный код)";
                    Worksheet.Cells[1, 8].Value = "дата формирования";
                    Worksheet.Cells[1, 9].Value = "номер паспорта";
                    Worksheet.Cells[1, 10].Value = "объем, куб.м";
                    Worksheet.Cells[1, 11].Value = "масса брутто, т";
                    Worksheet.Cells[1, 12].Value = "наименования радионуклида";
                    Worksheet.Cells[1, 13].Value = "удельная активность, Бк/г";
                    Worksheet.Cells[1, 14].Value = "вид";
                    Worksheet.Cells[1, 15].Value = "номер";
                    Worksheet.Cells[1, 16].Value = "дата";
                    Worksheet.Cells[1, 17].Value = "поставщика или получателя";
                    Worksheet.Cells[1, 18].Value = "перевозчика";
                    Worksheet.Cells[1, 19].Value = "наименование";
                    Worksheet.Cells[1, 20].Value = "код";
                    Worksheet.Cells[1, 21].Value = "код";
                    Worksheet.Cells[1, 22].Value = "статус";
                    Worksheet.Cells[1, 23].Value = "объем без упаковки, куб.м";
                    Worksheet.Cells[1, 24].Value = "масса без упаковки (нетто), т";
                    Worksheet.Cells[1, 25].Value = "количество ОЗИИИ, шт";
                    Worksheet.Cells[1, 26].Value = "тритий";
                    Worksheet.Cells[1, 27].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 28].Value = "альфа-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 29].Value = "трансурановые радионуклиды";
                    Worksheet.Cells[1, 30].Value = "Код переработки/сортировки РАО";
                    Worksheet.Cells[1, 31].Value = "Субсидия, %";
                    Worksheet.Cells[1, 32].Value = "Номер мероприятия ФЦП";

                    #endregion

                    break;
                }
            case "1.8":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "код";
                    Worksheet.Cells[1, 3].Value = "дата";
                    Worksheet.Cells[1, 4].Value = "индивидуальный номер (идентификационный код) партии ЖРО";
                    Worksheet.Cells[1, 5].Value = "номер паспорта";
                    Worksheet.Cells[1, 6].Value = "объем, куб.м";
                    Worksheet.Cells[1, 7].Value = "масса, т";
                    Worksheet.Cells[1, 8].Value = "солесодержание, г/л";
                    Worksheet.Cells[1, 9].Value = "наименование радионуклида";
                    Worksheet.Cells[1, 10].Value = "удельная активность, Бк/г";
                    Worksheet.Cells[1, 11].Value = "вид";
                    Worksheet.Cells[1, 12].Value = "номер";
                    Worksheet.Cells[1, 13].Value = "дата";
                    Worksheet.Cells[1, 14].Value = "поставщика или получателя";
                    Worksheet.Cells[1, 15].Value = "перевозчика";
                    Worksheet.Cells[1, 16].Value = "наименование";
                    Worksheet.Cells[1, 17].Value = "код";
                    Worksheet.Cells[1, 18].Value = "код";
                    Worksheet.Cells[1, 19].Value = "статус";
                    Worksheet.Cells[1, 20].Value = "объем, куб.м";
                    Worksheet.Cells[1, 21].Value = "масса, т";
                    Worksheet.Cells[1, 22].Value = "тритий";
                    Worksheet.Cells[1, 23].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 24].Value = "альфа-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 25].Value = "трансурановые радионуклиды";
                    Worksheet.Cells[1, 26].Value = "Код переработки/сортировки РАО";
                    Worksheet.Cells[1, 27].Value = "Субсидия, %";
                    Worksheet.Cells[1, 28].Value = "Номер мероприятия ФЦП";

                    #endregion

                    break;
                }
            case "1.9":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "код";
                    Worksheet.Cells[1, 3].Value = "дата";
                    Worksheet.Cells[1, 4].Value = "вид";
                    Worksheet.Cells[1, 5].Value = "номер";
                    Worksheet.Cells[1, 6].Value = "дата";
                    Worksheet.Cells[1, 7].Value = "Код типа объектов учета";
                    Worksheet.Cells[1, 8].Value = "радионуклиды";
                    Worksheet.Cells[1, 9].Value = "активность, Бк";

                    #endregion

                    break;
                }
            case "2.1":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "наименование";
                    Worksheet.Cells[1, 3].Value = "код";
                    Worksheet.Cells[1, 4].Value = "мощность куб.м/год";
                    Worksheet.Cells[1, 5].Value = "количество часов работы за год";
                    Worksheet.Cells[1, 6].Value = "код РАО";
                    Worksheet.Cells[1, 7].Value = "статус РАО";
                    Worksheet.Cells[1, 8].Value = "куб.м";
                    Worksheet.Cells[1, 9].Value = "т";
                    Worksheet.Cells[1, 10].Value = "ОЗИИИ, шт";
                    Worksheet.Cells[1, 11].Value = "тритий";
                    Worksheet.Cells[1, 12].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 13].Value = "альфа-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 14].Value = "трансурановые радионуклиды";
                    Worksheet.Cells[1, 15].Value = "код РАО";
                    Worksheet.Cells[1, 16].Value = "статус РАО";
                    Worksheet.Cells[1, 17].Value = "куб.м";
                    Worksheet.Cells[1, 18].Value = "т";
                    Worksheet.Cells[1, 19].Value = "ОЗИИИ, шт";
                    Worksheet.Cells[1, 20].Value = "тритий";
                    Worksheet.Cells[1, 21].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 22].Value = "альфа-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 23].Value = "трансурановые радионуклиды";

                    #endregion

                    break;
                }
            case "2.2":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "наименование";
                    Worksheet.Cells[1, 3].Value = "код";
                    Worksheet.Cells[1, 4].Value = "наименование";
                    Worksheet.Cells[1, 5].Value = "тип";
                    Worksheet.Cells[1, 6].Value = "количество, шт";
                    Worksheet.Cells[1, 7].Value = "код РАО";
                    Worksheet.Cells[1, 8].Value = "статус РАО";
                    Worksheet.Cells[1, 9].Value = "РАО без упаковки";
                    Worksheet.Cells[1, 10].Value = "РАО с упаковкой";
                    Worksheet.Cells[1, 11].Value = "РАО без упаковки (нетто)";
                    Worksheet.Cells[1, 12].Value = "РАО с упаковкой (брутто)";
                    Worksheet.Cells[1, 13].Value = "Количество ОЗИИИ, шт";
                    Worksheet.Cells[1, 14].Value = "тритий";
                    Worksheet.Cells[1, 15].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 16].Value = "альфа-излучающие радионуклиды (исключая";
                    Worksheet.Cells[1, 17].Value = "трансурановые радионуклиды";
                    Worksheet.Cells[1, 18].Value = "Основные радионуклиды";
                    Worksheet.Cells[1, 19].Value = "Субсидия, %";
                    Worksheet.Cells[1, 20].Value = "Номер мероприятия ФЦП";

                    #endregion

                    break;
                }
            case "2.3":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "наименование";
                    Worksheet.Cells[1, 3].Value = "код";
                    Worksheet.Cells[1, 4].Value = "проектный объем, куб.м";
                    Worksheet.Cells[1, 5].Value = "код РАО";
                    Worksheet.Cells[1, 6].Value = "объем, куб.м";
                    Worksheet.Cells[1, 7].Value = "масса, т";
                    Worksheet.Cells[1, 8].Value = "количество ОЗИИИ, шт";
                    Worksheet.Cells[1, 9].Value = "суммарная активность, Бк";
                    Worksheet.Cells[1, 10].Value = "номер";
                    Worksheet.Cells[1, 11].Value = "дата";
                    Worksheet.Cells[1, 12].Value = "срок действия";
                    Worksheet.Cells[1, 13].Value = "наименование документа";

                    #endregion

                    break;
                }
            case "2.4":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "Код ОЯТ";
                    Worksheet.Cells[1, 3].Value = "Номер мероприятия ФЦП";
                    Worksheet.Cells[1, 4].Value = "масса образованного, т";
                    Worksheet.Cells[1, 5].Value = "количество образованного, шт";
                    Worksheet.Cells[1, 6].Value = "масса поступивших от сторонних, т";
                    Worksheet.Cells[1, 7].Value = "количество поступивших от сторонних, шт";
                    Worksheet.Cells[1, 8].Value = "масса импортированных от сторонних, т";
                    Worksheet.Cells[1, 9].Value = "количество импортированных от сторонних, шт";
                    Worksheet.Cells[1, 10].Value = "масса учтенных по другим причинам, т";
                    Worksheet.Cells[1, 11].Value = "количество учтенных по другим причинам, шт";
                    Worksheet.Cells[1, 12].Value = "масса переданных сторонним, т";
                    Worksheet.Cells[1, 13].Value = "количество переданных сторонним, шт";
                    Worksheet.Cells[1, 14].Value = "масса переработанных, т";
                    Worksheet.Cells[1, 15].Value = "количество переработанных, шт";
                    Worksheet.Cells[1, 16].Value = "масса снятия с учета, т";
                    Worksheet.Cells[1, 17].Value = "количество снятых с учета, шт";

                    #endregion

                    break;
                }
            case "2.5":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "наименование, номер";
                    Worksheet.Cells[1, 3].Value = "код";
                    Worksheet.Cells[1, 4].Value = "код ОЯТ";
                    Worksheet.Cells[1, 5].Value = "номер мероприятия ФЦП";
                    Worksheet.Cells[1, 6].Value = "топливо (нетто)";
                    Worksheet.Cells[1, 7].Value = "ОТВС(ТВЭЛ, выемной части реактора) брутто";
                    Worksheet.Cells[1, 8].Value = "количество, шт";
                    Worksheet.Cells[1, 9].Value = "альфа-излучающих нуклидов";
                    Worksheet.Cells[1, 10].Value = "бета-, гамма-излучающих нуклидов";

                    #endregion

                    break;
                }
            case "2.6":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "Номер наблюдательной скважины";
                    Worksheet.Cells[1, 3].Value = "Наименование зоны контроля";
                    Worksheet.Cells[1, 4].Value = "Предполагаемый источник поступления радиоактивных веществ";
                    Worksheet.Cells[1, 5].Value = "Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м";
                    Worksheet.Cells[1, 6].Value = "Глубина отбора проб, м";
                    Worksheet.Cells[1, 7].Value = "Наименование радионуклида";
                    Worksheet.Cells[1, 8].Value = "Среднегодовое содержание радионуклида, Бк/кг";

                    #endregion

                    break;
                }
            case "2.7":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "Наименование, номер источника выбросов";
                    Worksheet.Cells[1, 3].Value = "Наименование радионуклида";
                    Worksheet.Cells[1, 4].Value = "разрешенный выброс за отчетный год";
                    Worksheet.Cells[1, 5].Value = "фактический выброс за отчетный год";
                    Worksheet.Cells[1, 6].Value = "фактический выброс за предыдущий год";

                    #endregion

                    break;
                }
            case "2.8":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "Наименование, номер выпуска сточных вод";
                    Worksheet.Cells[1, 3].Value = "наименование";
                    Worksheet.Cells[1, 4].Value = "код типа приемника";
                    Worksheet.Cells[1, 5].Value = "Наименование бассейнового округа";
                    Worksheet.Cells[1, 6].Value = "Допустимый объем водоотведения за год, тыс.куб.м";
                    Worksheet.Cells[1, 7].Value = "Отведено за отчетный период, тыс.куб.м";

                    #endregion

                    break;
                }
            case "2.9":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "Наименование, номер выпуска сточных вод";
                    Worksheet.Cells[1, 3].Value = "Наименование радионуклида";
                    Worksheet.Cells[1, 4].Value = "допустимая";
                    Worksheet.Cells[1, 5].Value = "фактическая";

                    #endregion

                    break;
                }
            case "2.10":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "Наименование показателя";
                    Worksheet.Cells[1, 3].Value = "Наименование участка";
                    Worksheet.Cells[1, 4].Value = "Кадастровый номер участка";
                    Worksheet.Cells[1, 5].Value = "Код участка";
                    Worksheet.Cells[1, 6].Value = "Площадь загрязненной территории, кв.м";
                    Worksheet.Cells[1, 7].Value = "средняя";
                    Worksheet.Cells[1, 8].Value = "максимальная";
                    Worksheet.Cells[1, 9].Value = "альфа-излучающие радионуклиды";
                    Worksheet.Cells[1, 10].Value = "бета-излучающие радионуклиды";
                    Worksheet.Cells[1, 11].Value = "Номер мероприятия ФЦП";

                    #endregion

                    break;
                }
            case "2.11":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "Наименование участка";
                    Worksheet.Cells[1, 3].Value = "Кадастровый номер участка";
                    Worksheet.Cells[1, 4].Value = "Код участка";
                    Worksheet.Cells[1, 5].Value = "Площадь загрязненной территории, кв.м";
                    Worksheet.Cells[1, 6].Value = "Наименование радионуклидов";
                    Worksheet.Cells[1, 7].Value = "земельный участок";
                    Worksheet.Cells[1, 8].Value = "жидкая фаза";
                    Worksheet.Cells[1, 9].Value = "донные отложения";

                    #endregion

                    break;
                }
            case "2.12":
                {
                    #region Headers

                    Worksheet.Cells[1, 1].Value = "№ п/п";
                    Worksheet.Cells[1, 2].Value = "Код операции";
                    Worksheet.Cells[1, 3].Value = "Код типа объектов учета";
                    Worksheet.Cells[1, 4].Value = "радионуклиды";
                    Worksheet.Cells[1, 5].Value = "активность, Бк";
                    Worksheet.Cells[1, 6].Value = "ОКПО поставщика/получателя";

                    #endregion

                    break;
                }
        }

        if (OperatingSystem.IsWindows())
        {
            Worksheet.Cells.AutoFitColumns(); // Под Astra Linux эта команда крашит программу без GDI дров
            WorksheetPrim.Cells.AutoFitColumns();
        }
        Worksheet.View.FreezePanes(2, 1);
        WorksheetPrim.View.FreezePanes(2, 1);
    }

    #endregion

    #region NotesHeader

    private void NotesHeaders()
    {
        WorksheetPrim.Cells[1, 1].Value = "№ строки";
        WorksheetPrim.Cells[1, 2].Value = "№ графы";
        WorksheetPrim.Cells[1, 3].Value = "Пояснение";
        WorksheetPrim.View.FreezePanes(2, 1);
    }

    #endregion

    #region ReportHeader

    private void ReportHeader()
    {
        Worksheet.Cells[1, 1].Value = "";
        Worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #endregion
}