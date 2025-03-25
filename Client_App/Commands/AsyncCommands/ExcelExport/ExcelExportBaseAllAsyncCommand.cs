using Models.Collections;
using System;
using System.Linq;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Абстрактный класс для выгрузки всех форм и форм х.х
/// </summary>
public abstract class ExcelExportBaseAllAsyncCommand : ExcelBaseAsyncCommand
{
    private protected Reports CurrentReports;

    private protected int CurrentRow;

    private protected int CurrentPrimRow;

    private protected bool IsSelectedOrg;

    #region FillExportForms

    /// <summary>
    /// Вызывает соответствующий метод выгрузки данных в .xlsx, в зависимости от номера формы.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    private protected void FillExportForms(string formNum)
    {
        switch (formNum)
        {
            case "1.1":
                ExportForm11Data();
                break;
            case "1.2":
                ExportForm12Data();
                break;
            case "1.3":
                ExportForm13Data();
                break;
            case "1.4":
                ExportForm14Data();
                break;
            case "1.5":
                ExportForm15Data();
                break;
            case "1.6":
                ExportForm16Data();
                break;
            case "1.7":
                ExportForm17Data();
                break;
            case "1.8":
                ExportForm18Data();
                break;
            case "1.9":
                ExportForm19Data();
                break;
            case "2.1":
                ExportForm21Data();
                break;
            case "2.2":
                ExportForm22Data();
                break;
            case "2.3":
                ExportForm23Data();
                break;
            case "2.4":
                ExportForm24Data();
                break;
            case "2.5":
                ExportForm25Data();
                break;
            case "2.6":
                ExportForm26Data();
                break;
            case "2.7":
                ExportForm27Data();
                break;
            case "2.8":
                ExportForm28Data();
                break;
            case "2.9":
                ExportForm29Data();
                break;
            case "2.10":
                ExportForm210Data();
                break;
            case "2.11":
                ExportForm211Data();
                break;
            case "2.12":
                ExportForm212Data();
                break;
        }
    }

    #endregion

    #region ExportForms

    #region ExportForm_11

    /// <summary>
    /// Выгрузка в .xlsx данных формы 1.1.
    /// </summary>
    private void ExportForm11Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null)
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows11
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 6);
                Worksheet.Cells[CurrentRow, 7].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 7);
                Worksheet.Cells[CurrentRow, 8].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 10);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.Type_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelString(repForm.FactoryNumber_DB);
                Worksheet.Cells[CurrentRow, 15].Value = repForm.Quantity_DB is null ? "-" : repForm.Quantity_DB;
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDouble(repForm.Activity_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelString(repForm.CreatorOKPO_DB);
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelDate(repForm.CreationDate_DB, Worksheet, CurrentRow, 18);
                Worksheet.Cells[CurrentRow, 19].Value = repForm.Category_DB is null ? "-" : repForm.Category_DB;
                Worksheet.Cells[CurrentRow, 20].Value = repForm.SignedServicePeriod_DB is null ? "-" : repForm.SignedServicePeriod_DB;
                Worksheet.Cells[CurrentRow, 21].Value = repForm.PropertyCode_DB is null ? "-" : repForm.PropertyCode_DB;
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.Owner_DB);
                Worksheet.Cells[CurrentRow, 23].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 25);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 30].Value = ConvertToExcelString(repForm.PackNumber_DB);
                                            
                #endregion

                CurrentRow++;
            }
            var notes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in notes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, WorksheetPrim, CurrentPrimRow, 5);
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, WorksheetPrim, CurrentPrimRow, 6);
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 9].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_12

    /// <summary>
    /// Выгрузка в .xlsx данных формы 1.2.
    /// </summary>
    private void ExportForm12Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("1.2") && x.Rows12 != null)
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows12
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 6);
                Worksheet.Cells[CurrentRow, 7].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 7);
                Worksheet.Cells[CurrentRow, 8].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 10);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.NameIOU_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.FactoryNumber_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelDouble(repForm.Mass_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelString(repForm.CreatorOKPO_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDate(repForm.CreationDate_DB, Worksheet, CurrentRow, 16);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDouble(repForm.SignedServicePeriod_DB);
                Worksheet.Cells[CurrentRow, 18].Value = repForm.PropertyCode_DB is null ? "-" : repForm.PropertyCode_DB;
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelString(repForm.Owner_DB);
                Worksheet.Cells[CurrentRow, 20].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 22);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.PackNumber_DB);
                                            
                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, WorksheetPrim, CurrentPrimRow, 5);
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, WorksheetPrim, CurrentPrimRow, 6);
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 9].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_13

    /// <summary>
    /// Выгрузка в .xlsx данных формы 1.3.
    /// </summary>
    private void ExportForm13Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("1.3") && x.Rows13 != null)
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows13
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 6);
                Worksheet.Cells[CurrentRow, 7].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 7);
                Worksheet.Cells[CurrentRow, 8].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 10);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.Type_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelString(repForm.FactoryNumber_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.Activity_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelString(repForm.CreatorOKPO_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDate(repForm.CreationDate_DB, Worksheet, CurrentRow, 17);
                Worksheet.Cells[CurrentRow, 18].Value = repForm.AggregateState_DB is null ? "-" : repForm.AggregateState_DB;
                Worksheet.Cells[CurrentRow, 19].Value = repForm.PropertyCode_DB is null ? "-" : repForm.PropertyCode_DB;
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelString(repForm.Owner_DB);
                Worksheet.Cells[CurrentRow, 21].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 23);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.PackNumber_DB);
                                            
                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, WorksheetPrim, CurrentPrimRow, 5);
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, WorksheetPrim, CurrentPrimRow, 6);
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 9].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_14

    /// <summary>
    /// Выгрузка в .xlsx данных формы 1.4.
    /// </summary>
    private void ExportForm14Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("1.4") && x.Rows14 != null)
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .ToList();
        foreach (var rep in repList)
        {
            var repSort = rep.Rows14
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in repSort)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 6);
                Worksheet.Cells[CurrentRow, 7].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 7);
                Worksheet.Cells[CurrentRow, 8].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 10);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.Name_DB);
                Worksheet.Cells[CurrentRow, 13].Value = repForm.Sort_DB is null ? "-" : repForm.Sort_DB;
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.Activity_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDate(repForm.ActivityMeasurementDate_DB, Worksheet, CurrentRow, 16);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDouble(repForm.Volume_DB);
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelDouble(repForm.Mass_DB);
                Worksheet.Cells[CurrentRow, 19].Value = repForm.AggregateState_DB is null ? "-" : repForm.AggregateState_DB;
                Worksheet.Cells[CurrentRow, 20].Value = repForm.PropertyCode_DB is null ? "-" : repForm.PropertyCode_DB;
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelString(repForm.Owner_DB);
                Worksheet.Cells[CurrentRow, 22].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 24);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelString(repForm.PackNumber_DB);
                                            
                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, WorksheetPrim, CurrentPrimRow, 5);
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, WorksheetPrim, CurrentPrimRow, 6);
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 9].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_15

    /// <summary>
    /// Выгрузка в .xlsx данных формы 1.5.
    /// </summary>
    private void ExportForm15Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("1.5") && x.Rows15 != null)
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows15
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 6);
                Worksheet.Cells[CurrentRow, 7].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 7);
                Worksheet.Cells[CurrentRow, 8].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 10);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.Type_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelString(repForm.FactoryNumber_DB);
                Worksheet.Cells[CurrentRow, 15].Value = repForm.Quantity_DB is null ? "-" : repForm.Quantity_DB;
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDouble(repForm.Activity_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDate(repForm.CreationDate_DB, Worksheet, CurrentRow, 17);
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelString(repForm.StatusRAO_DB);
                Worksheet.Cells[CurrentRow, 19].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 21);
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.PackNumber_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.StoragePlaceName_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.StoragePlaceCode_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelString(repForm.RefineOrSortRAOCode_DB);
                Worksheet.Cells[CurrentRow, 30].Value = ConvertToExcelString(repForm.Subsidy_DB);
                Worksheet.Cells[CurrentRow, 31].Value = ConvertToExcelString(repForm.FcpNumber_DB);
                Worksheet.Cells[CurrentRow, 32].Value = ConvertToExcelString(repForm.ContractNumber_DB);
                Worksheet.Cells[CurrentRow, 33].Value = StatusRaoToDescription(repForm.StatusRAO_DB);
                                            
                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, WorksheetPrim, CurrentPrimRow, 5);
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, WorksheetPrim, CurrentPrimRow, 6);
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 9].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_16

    /// <summary>
    /// Выгрузка в .xlsx данных формы 1.6.
    /// </summary>
    private void ExportForm16Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("1.6") && x.Rows16 != null)
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows16
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 6);
                Worksheet.Cells[CurrentRow, 7].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 7);
                Worksheet.Cells[CurrentRow, 8].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 10);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.CodeRAO_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.StatusRAO_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelDouble(repForm.Volume_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelDouble(repForm.Mass_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelInt(repForm.QuantityOZIII_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelString(repForm.MainRadionuclids_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDouble(repForm.TritiumActivity_DB);
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelDouble(repForm.BetaGammaActivity_DB);
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelDouble(repForm.AlphaActivity_DB);
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelDouble(repForm.TransuraniumActivity_DB);
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelDate(repForm.ActivityMeasurementDate_DB, Worksheet, CurrentRow, 21);
                Worksheet.Cells[CurrentRow, 22].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 24);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.StoragePlaceName_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.StoragePlaceCode_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelString(repForm.RefineOrSortRAOCode_DB);
                Worksheet.Cells[CurrentRow, 30].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 31].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 32].Value = ConvertToExcelString(repForm.PackNumber_DB);
                Worksheet.Cells[CurrentRow, 33].Value = ConvertToExcelString(repForm.Subsidy_DB);
                Worksheet.Cells[CurrentRow, 34].Value = ConvertToExcelString(repForm.FcpNumber_DB);
                Worksheet.Cells[CurrentRow, 35].Value = ConvertToExcelString(repForm.ContractNumber_DB);
                Worksheet.Cells[CurrentRow, 36].Value = StatusRaoToDescription(repForm.StatusRAO_DB);
                                            
                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, WorksheetPrim, CurrentPrimRow, 5);
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, WorksheetPrim, CurrentPrimRow, 6);
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 9].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_17

    /// <summary>
    /// Выгрузка в .xlsx данных формы 1.7.
    /// </summary>
    private void ExportForm17Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("1.7") && x.Rows17 != null)
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows17
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 6);
                Worksheet.Cells[CurrentRow, 7].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 7);
                Worksheet.Cells[CurrentRow, 8].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 10);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.PackFactoryNumber_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelString(repForm.PackNumber_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDate(repForm.FormingDate_DB, Worksheet, CurrentRow, 15);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDouble(repForm.Volume_DB);
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelDouble(repForm.Mass_DB);
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelDouble(repForm.SpecificActivity_DB);
                Worksheet.Cells[CurrentRow, 21].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 23);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.StoragePlaceName_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelString(repForm.StoragePlaceCode_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelString(repForm.CodeRAO_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelString(repForm.StatusRAO_DB);
                Worksheet.Cells[CurrentRow, 30].Value = ConvertToExcelDouble(repForm.VolumeOutOfPack_DB);
                Worksheet.Cells[CurrentRow, 31].Value = ConvertToExcelDouble(repForm.MassOutOfPack_DB);
                Worksheet.Cells[CurrentRow, 32].Value = ConvertToExcelInt(repForm.Quantity_DB);
                Worksheet.Cells[CurrentRow, 33].Value = ConvertToExcelDouble(repForm.TritiumActivity_DB);
                Worksheet.Cells[CurrentRow, 34].Value = ConvertToExcelDouble(repForm.BetaGammaActivity_DB);
                Worksheet.Cells[CurrentRow, 35].Value = ConvertToExcelDouble(repForm.AlphaActivity_DB);
                Worksheet.Cells[CurrentRow, 36].Value = ConvertToExcelDouble(repForm.TransuraniumActivity_DB);
                Worksheet.Cells[CurrentRow, 37].Value = ConvertToExcelString(repForm.RefineOrSortRAOCode_DB);
                Worksheet.Cells[CurrentRow, 38].Value = ConvertToExcelString(repForm.Subsidy_DB);
                Worksheet.Cells[CurrentRow, 39].Value = ConvertToExcelString(repForm.FcpNumber_DB);
                Worksheet.Cells[CurrentRow, 40].Value = ConvertToExcelString(repForm.ContractNumber_DB);
                Worksheet.Cells[CurrentRow, 41].Value = StatusRaoToDescription(repForm.StatusRAO_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, WorksheetPrim, CurrentPrimRow, 5);
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, WorksheetPrim, CurrentPrimRow, 6);
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 9].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_18

    /// <summary>
    /// Выгрузка в .xlsx данных формы 1.8.
    /// </summary>
    private void ExportForm18Data()
    {
        var form = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("1.8") && x.Rows18 != null)
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .ToList();
        foreach (var rep in form)
        {
            var forms = rep.Rows18
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 6);
                Worksheet.Cells[CurrentRow, 7].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 7);
                Worksheet.Cells[CurrentRow, 8].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 10);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.IndividualNumberZHRO_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.PassportNumber_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelDouble(repForm.Volume6_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelDouble(repForm.Mass7_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.SaltConcentration_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDouble(repForm.SpecificActivity_DB);
                Worksheet.Cells[CurrentRow, 18].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 20);
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.TransporterOKPO_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelString(repForm.StoragePlaceName_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.StoragePlaceCode_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.CodeRAO_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.StatusRAO_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelDouble(repForm.Volume20_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelDouble(repForm.Mass21_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelDouble(repForm.TritiumActivity_DB);
                Worksheet.Cells[CurrentRow, 30].Value = ConvertToExcelDouble(repForm.BetaGammaActivity_DB);
                Worksheet.Cells[CurrentRow, 31].Value = ConvertToExcelDouble(repForm.AlphaActivity_DB);
                Worksheet.Cells[CurrentRow, 32].Value = ConvertToExcelDouble(repForm.TransuraniumActivity_DB);
                Worksheet.Cells[CurrentRow, 33].Value = ConvertToExcelString(repForm.RefineOrSortRAOCode_DB);
                Worksheet.Cells[CurrentRow, 34].Value = ConvertToExcelString(repForm.Subsidy_DB);
                Worksheet.Cells[CurrentRow, 35].Value = ConvertToExcelString(repForm.FcpNumber_DB);
                Worksheet.Cells[CurrentRow, 36].Value = ConvertToExcelString(repForm.ContractNumber_DB);
                Worksheet.Cells[CurrentRow, 37].Value = StatusRaoToDescription(repForm.StatusRAO_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, WorksheetPrim, CurrentPrimRow, 5);
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, WorksheetPrim, CurrentPrimRow, 6);
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 9].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_19

    /// <summary>
    /// Выгрузка в .xlsx данных формы 1.9.
    /// </summary>
    private void ExportForm19Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("1.9") && x.Rows19 != null)
            .OrderBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
            .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows19
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = ConvertToExcelDate(rep.StartPeriod_DB, Worksheet, CurrentRow, 6);
                Worksheet.Cells[CurrentRow, 7].Value = ConvertToExcelDate(rep.EndPeriod_DB, Worksheet, CurrentRow, 7);
                Worksheet.Cells[CurrentRow, 8].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.OperationCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDate(repForm.OperationDate_DB, Worksheet, CurrentRow, 10);
                Worksheet.Cells[CurrentRow, 11].Value = repForm.DocumentVid_DB is null ? "-" : repForm.DocumentVid_DB;
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 13);
                Worksheet.Cells[CurrentRow, 14].Value = repForm.CodeTypeAccObject_DB is null ? "-" : repForm.CodeTypeAccObject_DB;
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDouble(repForm.Activity_DB);
                                            
                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = ConvertToExcelDate(rep.StartPeriod_DB, WorksheetPrim, CurrentPrimRow, 5);
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = ConvertToExcelDate(rep.EndPeriod_DB, WorksheetPrim, CurrentPrimRow, 6);
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 9].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_21

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.1.
    /// </summary>
    private void ExportForm21Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.1") && x.Rows21 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows21
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.RefineMachineName_DB);
                Worksheet.Cells[CurrentRow, 9].Value = repForm.MachineCode.Value is null ? "-" : repForm.MachineCode.Value;
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDouble(repForm.MachinePower_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelDouble(repForm.NumberOfHoursPerYear_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.CodeRAOIn_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.StatusRAOIn_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelDouble(repForm.VolumeIn_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.MassIn_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelInt(repForm.QuantityIn_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDouble(repForm.TritiumActivityIn_DB);
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelDouble(repForm.BetaGammaActivityIn_DB);
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelDouble(repForm.AlphaActivityIn_DB);
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelDouble(repForm.TransuraniumActivityIn_DB);
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelString(repForm.CodeRAOout_DB);
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelString(repForm.StatusRAOout_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelDouble(repForm.VolumeOut_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelDouble(repForm.MassOut_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelInt(repForm.QuantityOZIIIout_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelDouble(repForm.TritiumActivityOut_DB);
                Worksheet.Cells[CurrentRow, 27].Value = ConvertToExcelDouble(repForm.BetaGammaActivityOut_DB);
                Worksheet.Cells[CurrentRow, 28].Value = ConvertToExcelDouble(repForm.AlphaActivityOut_DB);
                Worksheet.Cells[CurrentRow, 29].Value = ConvertToExcelDouble(repForm.TransuraniumActivityOut_DB);
                Worksheet.Cells[CurrentRow, 30].Value = StatusRaoToDescription(repForm.StatusRAOIn_DB);
                Worksheet.Cells[CurrentRow, 31].Value = StatusRaoToDescription(repForm.StatusRAOout_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_22

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.2.
    /// </summary>
    private void ExportForm22Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.2") && x.Rows22 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows22
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.StoragePlaceName_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.StoragePlaceCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.PackName_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.PackType_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelInt(repForm.PackQuantity_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.CodeRAO_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelString(repForm.StatusRAO_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.VolumeOutOfPack_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDouble(repForm.VolumeInPack_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDouble(repForm.MassOutOfPack_DB);
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelDouble(repForm.MassInPack_DB);
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelInt(repForm.QuantityOZIII_DB);
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelDouble(repForm.TritiumActivity_DB);
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelDouble(repForm.BetaGammaActivity_DB);
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelDouble(repForm.AlphaActivity_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelDouble(repForm.TransuraniumActivity_DB);
                Worksheet.Cells[CurrentRow, 24].Value = ConvertToExcelString(repForm.MainRadionuclids_DB);
                Worksheet.Cells[CurrentRow, 25].Value = ConvertToExcelString(repForm.Subsidy_DB);
                Worksheet.Cells[CurrentRow, 26].Value = ConvertToExcelString(repForm.FcpNumber_DB);
                Worksheet.Cells[CurrentRow, 27].Value = StatusRaoToDescription(repForm.StatusRAO_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_23

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.3.
    /// </summary>
    private void ExportForm23Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.3") && x.Rows23 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows23
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.StoragePlaceName_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.StoragePlaceCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDouble(repForm.ProjectVolume_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.CodeRAO_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelDouble(repForm.Volume_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelDouble(repForm.Mass_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelInt(repForm.QuantityOZIII_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.SummaryActivity_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelString(repForm.DocumentNumber_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelDate(repForm.DocumentDate_DB, Worksheet, CurrentRow, 17);
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelDate(repForm.ExpirationDate_DB, Worksheet, CurrentRow, 18);
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelString(repForm.DocumentName_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_24

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.4.
    /// </summary>
    private void ExportForm24Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.4") && x.Rows24 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows24
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.CodeOYAT_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.FcpNumber_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDouble(repForm.MassCreated_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelInt(repForm.QuantityCreated_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelDouble(repForm.MassFromAnothers_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelInt(repForm.QuantityFromAnothers_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelDouble(repForm.MassFromAnothersImported_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelInt(repForm.QuantityFromAnothersImported_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDouble(repForm.MassAnotherReasons_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelInt(repForm.QuantityAnotherReasons_DB);
                Worksheet.Cells[CurrentRow, 18].Value = ConvertToExcelDouble(repForm.MassTransferredToAnother_DB);
                Worksheet.Cells[CurrentRow, 19].Value = ConvertToExcelInt(repForm.QuantityTransferredToAnother_DB);
                Worksheet.Cells[CurrentRow, 20].Value = ConvertToExcelDouble(repForm.MassRefined_DB);
                Worksheet.Cells[CurrentRow, 21].Value = ConvertToExcelInt(repForm.QuantityRefined_DB);
                Worksheet.Cells[CurrentRow, 22].Value = ConvertToExcelDouble(repForm.MassRemovedFromAccount_DB);
                Worksheet.Cells[CurrentRow, 23].Value = ConvertToExcelInt(repForm.QuantityRemovedFromAccount_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_25

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.5.
    /// </summary>
    private void ExportForm25Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.5") && x.Rows25 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows25
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.StoragePlaceName_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.StoragePlaceCode_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.CodeOYAT_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.FcpNumber_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelDouble(repForm.FuelMass_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelDouble(repForm.CellMass_DB);
                Worksheet.Cells[CurrentRow, 14].Value = repForm.Quantity_DB is null ? "-" : repForm.Quantity_DB;
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.AlphaActivity_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDouble(repForm.BetaGammaActivity_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_26

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.6.
    /// </summary>
    private void ExportForm26Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.6") && x.Rows26 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows26
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.ObservedSourceNumber_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.ControlledAreaName_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.SupposedWasteSource_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelDouble(repForm.DistanceToWasteSource_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelDouble(repForm.TestDepth_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelString(repForm.RadionuclidName_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelDouble(repForm.AverageYearConcentration_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_27

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.7.
    /// </summary>
    private void ExportForm27Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.7") && x.Rows27 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows27
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.ObservedSourceNumber_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.RadionuclidName_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDouble(repForm.AllowedWasteValue_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelDouble(repForm.FactedWasteValue_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelDouble(repForm.WasteOutbreakPreviousYear_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_28

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.8.
    /// </summary>
    private void ExportForm28Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.8") && x.Rows28 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var repSort = rep.Rows28
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in repSort)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.WasteSourceName_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.WasteRecieverName_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.RecieverTypeCode_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.PoolDistrictName_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelDouble(repForm.AllowedWasteRemovalVolume_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelDouble(repForm.RemovedWasteVolume_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_29

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.9.
    /// </summary>
    private void ExportForm29Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.9") && x.Rows29 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows29
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.WasteSourceName_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.RadionuclidName_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelDouble(repForm.AllowedActivity_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelDouble(repForm.FactedActivity_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_210

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.10.
    /// </summary>
    private void ExportForm210Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.10") && x.Rows210 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows210
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.IndicatorName_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.PlotName_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.PlotKadastrNumber_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelString(repForm.PlotCode_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelDouble(repForm.InfectedArea_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelDouble(repForm.AvgGammaRaysDosePower_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelDouble(repForm.MaxGammaRaysDosePower_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.WasteDensityAlpha_DB);
                Worksheet.Cells[CurrentRow, 16].Value = ConvertToExcelDouble(repForm.WasteDensityBeta_DB);
                Worksheet.Cells[CurrentRow, 17].Value = ConvertToExcelString(repForm.FcpNumber_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_211

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.11.
    /// </summary>
    private void ExportForm211Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.11") && x.Rows211 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows211
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = ConvertToExcelString(repForm.PlotName_DB);
                Worksheet.Cells[CurrentRow, 9].Value = ConvertToExcelString(repForm.PlotKadastrNumber_DB);
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.PlotCode_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelDouble(repForm.InfectedArea_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 13].Value = ConvertToExcelDouble(repForm.SpecificActivityOfPlot_DB);
                Worksheet.Cells[CurrentRow, 14].Value = ConvertToExcelDouble(repForm.SpecificActivityOfLiquidPart_DB);
                Worksheet.Cells[CurrentRow, 15].Value = ConvertToExcelDouble(repForm.SpecificActivityOfDensePart_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #region ExportForm_212

    /// <summary>
    /// Выгрузка в .xlsx данных формы 2.12.
    /// </summary>
    private void ExportForm212Data()
    {
        var repList = CurrentReports.Report_Collection
            .Where(x => x.FormNum_DB.Equals("2.12") && x.Rows212 != null)
            .OrderBy(x => x.Year_DB)
            .ToList();
        foreach (var rep in repList)
        {
            var forms = rep.Rows212
                .OrderBy(x => x.NumberInOrder_DB)
                .ToList();
            foreach (var repForm in forms)
            {
                #region Binding

                Worksheet.Cells[CurrentRow, 1].Value = CurrentReports.Master_DB.Id;
                Worksheet.Cells[CurrentRow, 2].Value = CurrentReports.Master_DB.OkpoRep.Value;
                Worksheet.Cells[CurrentRow, 3].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                Worksheet.Cells[CurrentRow, 4].Value = CurrentReports.Master_DB.RegNoRep.Value;
                Worksheet.Cells[CurrentRow, 5].Value = rep.CorrectionNumber_DB;
                Worksheet.Cells[CurrentRow, 6].Value = rep.Year_DB;
                Worksheet.Cells[CurrentRow, 7].Value = repForm.NumberInOrder_DB;
                Worksheet.Cells[CurrentRow, 8].Value = repForm.OperationCode_DB is null ? "-" : repForm.OperationCode_DB;
                Worksheet.Cells[CurrentRow, 9].Value = repForm.ObjectTypeCode_DB is null ? "-" : repForm.ObjectTypeCode_DB;
                Worksheet.Cells[CurrentRow, 10].Value = ConvertToExcelString(repForm.Radionuclids_DB);
                Worksheet.Cells[CurrentRow, 11].Value = ConvertToExcelDouble(repForm.Activity_DB);
                Worksheet.Cells[CurrentRow, 12].Value = ConvertToExcelString(repForm.ProviderOrRecieverOKPO_DB);

                #endregion

                CurrentRow++;
            }

            var repNotes = rep.Notes
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var comment in repNotes)
            {
                #region Binding

                WorksheetPrim.Cells[CurrentPrimRow, 1].Value = CurrentReports.Master_DB.OkpoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 2].Value = CurrentReports.Master_DB.ShortJurLicoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 3].Value = CurrentReports.Master_DB.RegNoRep.Value;
                WorksheetPrim.Cells[CurrentPrimRow, 4].Value = rep.CorrectionNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 5].Value = rep.Year_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 6].Value = comment.RowNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 7].Value = comment.GraphNumber_DB;
                WorksheetPrim.Cells[CurrentPrimRow, 8].Value = comment.Comment_DB;

                #endregion

                CurrentPrimRow++;
            }
        }
    }

    #endregion

    #endregion

    #region Headers

    #region FillHeaders

    /// <summary>
    /// Заполнение заголовков в .xlsx.
    /// </summary>
    /// <param name="formNum">Номер формы.</param>
    private protected void FillHeaders(string formNum)
    {
        switch (formNum)
        {
            case "1.1":
            {
                #region Headers

                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег.№";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "Дата начала периода";
                Worksheet.Cells[1, 7].Value = "Дата конца периода";
                Worksheet.Cells[1, 8].Value = "№ п/п";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "дата";
                Worksheet.Cells[1, 11].Value = "номер паспорта (сертификата)";
                Worksheet.Cells[1, 12].Value = "тип";
                Worksheet.Cells[1, 13].Value = "радионуклиды";
                Worksheet.Cells[1, 14].Value = "номер";
                Worksheet.Cells[1, 15].Value = "количество, шт";
                Worksheet.Cells[1, 16].Value = "суммарная активность, Бк";
                Worksheet.Cells[1, 17].Value = "код ОКПО изготовителя";
                Worksheet.Cells[1, 18].Value = "дата выпуска";
                Worksheet.Cells[1, 19].Value = "категория";
                Worksheet.Cells[1, 20].Value = "НСС, мес";
                Worksheet.Cells[1, 21].Value = "код формы собственности";
                Worksheet.Cells[1, 22].Value = "код ОКПО правообладателя";
                Worksheet.Cells[1, 23].Value = "вид";
                Worksheet.Cells[1, 24].Value = "номер";
                Worksheet.Cells[1, 25].Value = "дата";
                Worksheet.Cells[1, 26].Value = "поставщика или получателя";
                Worksheet.Cells[1, 27].Value = "перевозчика";
                Worksheet.Cells[1, 28].Value = "наименование";
                Worksheet.Cells[1, 29].Value = "тип";
                Worksheet.Cells[1, 30].Value = "номер";
                NotesHeaders1();   

                #endregion

                break;
            }
            case "1.2":
            {
                #region Headers

                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег.№";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "Дата начала периода";
                Worksheet.Cells[1, 7].Value = "Дата конца периода";
                Worksheet.Cells[1, 8].Value = "№ п/п";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "дата";
                Worksheet.Cells[1, 11].Value = "номер паспорта";
                Worksheet.Cells[1, 12].Value = "наименование";
                Worksheet.Cells[1, 13].Value = "номер";
                Worksheet.Cells[1, 14].Value = "масса обедненного урана, кг";
                Worksheet.Cells[1, 15].Value = "код ОКПО изготовителя";
                Worksheet.Cells[1, 16].Value = "дата выпуска";
                Worksheet.Cells[1, 17].Value = "НСС, мес";
                Worksheet.Cells[1, 18].Value = "код формы собственности";
                Worksheet.Cells[1, 19].Value = "код ОКПО правообладателя";
                Worksheet.Cells[1, 20].Value = "вид";
                Worksheet.Cells[1, 21].Value = "номер";
                Worksheet.Cells[1, 22].Value = "дата";
                Worksheet.Cells[1, 23].Value = "поставщика или получателя";
                Worksheet.Cells[1, 24].Value = "перевозчика";
                Worksheet.Cells[1, 25].Value = "наименование";
                Worksheet.Cells[1, 26].Value = "тип";
                Worksheet.Cells[1, 27].Value = "номер";
                NotesHeaders1();   

                #endregion

                break;
            }
            case "1.3":
            {
                #region Headers

                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег.№";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "Дата начала периода";
                Worksheet.Cells[1, 7].Value = "Дата конца периода";
                Worksheet.Cells[1, 8].Value = "№ п/п";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "дата";
                Worksheet.Cells[1, 11].Value = "номер паспорта";
                Worksheet.Cells[1, 12].Value = "тип";
                Worksheet.Cells[1, 13].Value = "радионуклиды";
                Worksheet.Cells[1, 14].Value = "номер";
                Worksheet.Cells[1, 15].Value = "активность, Бк";
                Worksheet.Cells[1, 16].Value = "код ОКПО изготовителя";
                Worksheet.Cells[1, 17].Value = "дата выпуска";
                Worksheet.Cells[1, 18].Value = "агрегатное состояние";
                Worksheet.Cells[1, 19].Value = "код формы собственности";
                Worksheet.Cells[1, 20].Value = "код ОКПО правообладателя";
                Worksheet.Cells[1, 21].Value = "вид";
                Worksheet.Cells[1, 22].Value = "номер";
                Worksheet.Cells[1, 23].Value = "дата";
                Worksheet.Cells[1, 24].Value = "поставщика или получателя";
                Worksheet.Cells[1, 25].Value = "перевозчика";
                Worksheet.Cells[1, 26].Value = "наименование";
                Worksheet.Cells[1, 27].Value = "тип";
                Worksheet.Cells[1, 28].Value = "номер";
                NotesHeaders1();   

                #endregion

                break;
            }
            case "1.4":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег.№";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "Дата начала периода";
                Worksheet.Cells[1, 7].Value = "Дата конца периода";
                Worksheet.Cells[1, 8].Value = "№ п/п";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "дата";
                Worksheet.Cells[1, 11].Value = "номер паспорта";
                Worksheet.Cells[1, 12].Value = "наименование";
                Worksheet.Cells[1, 13].Value = "вид";
                Worksheet.Cells[1, 14].Value = "радионуклиды";
                Worksheet.Cells[1, 15].Value = "активность, Бк";
                Worksheet.Cells[1, 16].Value = "дата измерения активности";
                Worksheet.Cells[1, 17].Value = "объем, куб.м";
                Worksheet.Cells[1, 18].Value = "масса, кг";
                Worksheet.Cells[1, 19].Value = "агрегатное состояние";
                Worksheet.Cells[1, 20].Value = "код формы собственности";
                Worksheet.Cells[1, 21].Value = "код ОКПО правообладателя";
                Worksheet.Cells[1, 22].Value = "вид";
                Worksheet.Cells[1, 23].Value = "номер";
                Worksheet.Cells[1, 24].Value = "дата";
                Worksheet.Cells[1, 25].Value = "поставщика или получателя";
                Worksheet.Cells[1, 26].Value = "перевозчика";
                Worksheet.Cells[1, 27].Value = "наименование";
                Worksheet.Cells[1, 28].Value = "тип";
                Worksheet.Cells[1, 29].Value = "номер";
                NotesHeaders1();   
        
                #endregion

                break;
            }
            case "1.5":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег.№";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "Дата начала периода";
                Worksheet.Cells[1, 7].Value = "Дата конца периода";
                Worksheet.Cells[1, 8].Value = "№ п/п";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "дата";
                Worksheet.Cells[1, 11].Value = "номер паспорта (сертификата) ЗРИ, акта определения характеристик ОЗИИ";
                Worksheet.Cells[1, 12].Value = "тип";
                Worksheet.Cells[1, 13].Value = "радионуклиды";
                Worksheet.Cells[1, 14].Value = "номер";
                Worksheet.Cells[1, 15].Value = "количество, шт";
                Worksheet.Cells[1, 16].Value = "суммарная активность, Бк";
                Worksheet.Cells[1, 17].Value = "дата выпуска";
                Worksheet.Cells[1, 18].Value = "статус РАО";
                Worksheet.Cells[1, 19].Value = "вид";
                Worksheet.Cells[1, 20].Value = "номер";
                Worksheet.Cells[1, 21].Value = "дата";
                Worksheet.Cells[1, 22].Value = "поставщика или получателя";
                Worksheet.Cells[1, 23].Value = "перевозчика";
                Worksheet.Cells[1, 24].Value = "наименование";
                Worksheet.Cells[1, 25].Value = "тип";
                Worksheet.Cells[1, 26].Value = "заводской номер";
                Worksheet.Cells[1, 27].Value = "наименование";
                Worksheet.Cells[1, 28].Value = "код";
                Worksheet.Cells[1, 29].Value = "Код переработки / сортировки РАО";
                Worksheet.Cells[1, 30].Value = "Субсидия, %";
                Worksheet.Cells[1, 31].Value = "Номер мероприятия ФЦП";
                Worksheet.Cells[1, 32].Value = "Номер договора";
                Worksheet.Cells[1, 33].Value = "Текст статуса РАО";
                NotesHeaders1();   
        
                #endregion
                
                break;
            }
            case "1.6":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег.№";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "Дата начала периода";
                Worksheet.Cells[1, 7].Value = "Дата конца периода";
                Worksheet.Cells[1, 8].Value = "№ п/п";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "дата";
                Worksheet.Cells[1, 11].Value = "Код РАО";
                Worksheet.Cells[1, 12].Value = "Статус РАО";
                Worksheet.Cells[1, 13].Value = "объем без упаковки, куб.";
                Worksheet.Cells[1, 14].Value = "масса без упаковки";
                Worksheet.Cells[1, 15].Value = "количество ОЗИИИ";
                Worksheet.Cells[1, 16].Value = "Основные радионуклиды";
                Worksheet.Cells[1, 17].Value = "тритий";
                Worksheet.Cells[1, 18].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 19].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 20].Value = "трансурановые радионуклиды";
                Worksheet.Cells[1, 21].Value = "Дата измерения активности";
                Worksheet.Cells[1, 22].Value = "вид";
                Worksheet.Cells[1, 23].Value = "номер";
                Worksheet.Cells[1, 24].Value = "дата";
                Worksheet.Cells[1, 25].Value = "поставщика или получателя";
                Worksheet.Cells[1, 26].Value = "перевозчика";
                Worksheet.Cells[1, 27].Value = "наименование";
                Worksheet.Cells[1, 28].Value = "код";
                Worksheet.Cells[1, 29].Value = "Код переработки /";
                Worksheet.Cells[1, 30].Value = "наименование";
                Worksheet.Cells[1, 31].Value = "тип";
                Worksheet.Cells[1, 32].Value = "номер упаковки";
                Worksheet.Cells[1, 33].Value = "Субсидия, %";
                Worksheet.Cells[1, 34].Value = "Номер мероприятия ФЦП";
                Worksheet.Cells[1, 35].Value = "Номер договора";
                Worksheet.Cells[1, 36].Value = "Текст статуса РАО";
                NotesHeaders1();   
                
                #endregion
                
                break;
            }
            case "1.7":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег.№";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "Дата начала периода";
                Worksheet.Cells[1, 7].Value = "Дата конца периода";
                Worksheet.Cells[1, 8].Value = "№ п/п";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "дата";
                Worksheet.Cells[1, 11].Value = "наименование";
                Worksheet.Cells[1, 12].Value = "тип";
                Worksheet.Cells[1, 13].Value = "заводской номер";
                Worksheet.Cells[1, 14].Value = "номер упаковки (идентификационный код)";
                Worksheet.Cells[1, 15].Value = "дата формирования";
                Worksheet.Cells[1, 16].Value = "номер паспорта";
                Worksheet.Cells[1, 17].Value = "объем, куб.м";
                Worksheet.Cells[1, 18].Value = "масса брутто, т";
                Worksheet.Cells[1, 19].Value = "наименования радионуклида";
                Worksheet.Cells[1, 20].Value = "удельная активность, Бк/г";
                Worksheet.Cells[1, 21].Value = "вид";
                Worksheet.Cells[1, 22].Value = "номер";
                Worksheet.Cells[1, 23].Value = "дата";
                Worksheet.Cells[1, 24].Value = "поставщика или получателя";
                Worksheet.Cells[1, 25].Value = "перевозчика";
                Worksheet.Cells[1, 26].Value = "наименование";
                Worksheet.Cells[1, 27].Value = "код";
                Worksheet.Cells[1, 28].Value = "код";
                Worksheet.Cells[1, 29].Value = "статус";
                Worksheet.Cells[1, 30].Value = "объем без упаковки, куб.м";
                Worksheet.Cells[1, 31].Value = "масса без упаковки (нетто), т";
                Worksheet.Cells[1, 32].Value = "количество ОЗИИИ, шт";
                Worksheet.Cells[1, 33].Value = "тритий";
                Worksheet.Cells[1, 34].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 35].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 36].Value = "трансурановые радионуклиды";
                Worksheet.Cells[1, 37].Value = "Код переработки/сортировки РАО";
                Worksheet.Cells[1, 38].Value = "Субсидия, %";
                Worksheet.Cells[1, 39].Value = "Номер мероприятия ФЦП";
                Worksheet.Cells[1, 40].Value = "Номер договора";
                Worksheet.Cells[1, 41].Value = "Текст статуса РАО";
                NotesHeaders1();   
                
                #endregion
                
                break;
            }
            case "1.8":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег.№";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "Дата начала периода";
                Worksheet.Cells[1, 7].Value = "Дата конца периода";
                Worksheet.Cells[1, 8].Value = "№ п/п";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "дата";
                Worksheet.Cells[1, 11].Value = "индивидуальный номер (идентификационный код) партии ЖРО";
                Worksheet.Cells[1, 12].Value = "номер паспорта";
                Worksheet.Cells[1, 13].Value = "объем, куб.м";
                Worksheet.Cells[1, 14].Value = "масса, т";
                Worksheet.Cells[1, 15].Value = "солесодержание, г/л";
                Worksheet.Cells[1, 16].Value = "наименование радионуклида";
                Worksheet.Cells[1, 17].Value = "удельная активность, Бк/г";
                Worksheet.Cells[1, 18].Value = "вид";
                Worksheet.Cells[1, 19].Value = "номер";
                Worksheet.Cells[1, 20].Value = "дата";
                Worksheet.Cells[1, 21].Value = "поставщика или получателя";
                Worksheet.Cells[1, 22].Value = "перевозчика";
                Worksheet.Cells[1, 23].Value = "наименование";
                Worksheet.Cells[1, 24].Value = "код";
                Worksheet.Cells[1, 25].Value = "код";
                Worksheet.Cells[1, 26].Value = "статус";
                Worksheet.Cells[1, 27].Value = "объем, куб.м";
                Worksheet.Cells[1, 28].Value = "масса, т";
                Worksheet.Cells[1, 29].Value = "тритий";
                Worksheet.Cells[1, 30].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 31].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 32].Value = "трансурановые радионуклиды";
                Worksheet.Cells[1, 33].Value = "Код переработки/сортировки РАО";
                Worksheet.Cells[1, 34].Value = "Субсидия, %";
                Worksheet.Cells[1, 35].Value = "Номер мероприятия ФЦП";
                Worksheet.Cells[1, 36].Value = "Номер договора";
                Worksheet.Cells[1, 37].Value = "Текст статуса РАО";
                NotesHeaders1();   
                
                #endregion

                break;
            }
            case "1.9":
            {
                #region Headers

                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег.№";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "Дата начала периода";
                Worksheet.Cells[1, 7].Value = "Дата конца периода";
                Worksheet.Cells[1, 8].Value = "№ п/п";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "дата";
                Worksheet.Cells[1, 11].Value = "вид";
                Worksheet.Cells[1, 12].Value = "номер";
                Worksheet.Cells[1, 13].Value = "дата";
                Worksheet.Cells[1, 14].Value = "Код типа объектов учета";
                Worksheet.Cells[1, 15].Value = "радионуклиды";
                Worksheet.Cells[1, 16].Value = "активность, Бк"; 
                NotesHeaders1();   

                #endregion
                
                break;
            }
            case "2.1":
            {
                #region Headers

                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "наименование";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "мощность куб.м/год";
                Worksheet.Cells[1, 11].Value = "количество часов работы за год";
                Worksheet.Cells[1, 12].Value = "код РАО";
                Worksheet.Cells[1, 13].Value = "статус РАО";
                Worksheet.Cells[1, 14].Value = "куб.м";
                Worksheet.Cells[1, 15].Value = "т";
                Worksheet.Cells[1, 16].Value = "ОЗИИИ, шт";
                Worksheet.Cells[1, 17].Value = "тритий";
                Worksheet.Cells[1, 18].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 19].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 20].Value = "трансурановые радионуклиды";
                Worksheet.Cells[1, 21].Value = "код РАО";
                Worksheet.Cells[1, 22].Value = "статус РАО";
                Worksheet.Cells[1, 23].Value = "куб.м";
                Worksheet.Cells[1, 24].Value = "т";
                Worksheet.Cells[1, 25].Value = "ОЗИИИ, шт";
                Worksheet.Cells[1, 26].Value = "тритий";
                Worksheet.Cells[1, 27].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 28].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 29].Value = "трансурановые радионуклиды";
                Worksheet.Cells[1, 30].Value = "Текст статуса РАО для поступивших";
                Worksheet.Cells[1, 31].Value = "Текст статуса РАО для переработанных";
                NotesHeaders2(); 

                #endregion

                break;
            }
            case "2.2":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "наименование";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "наименование";
                Worksheet.Cells[1, 11].Value = "тип";
                Worksheet.Cells[1, 12].Value = "количество, шт";
                Worksheet.Cells[1, 13].Value = "код РАО";
                Worksheet.Cells[1, 14].Value = "статус РАО";
                Worksheet.Cells[1, 15].Value = "РАО без упаковки";
                Worksheet.Cells[1, 16].Value = "РАО с упаковкой";
                Worksheet.Cells[1, 17].Value = "РАО без упаковки (нетто)";
                Worksheet.Cells[1, 18].Value = "РАО с упаковкой (брутто)";
                Worksheet.Cells[1, 19].Value = "Количество ОЗИИИ, шт";
                Worksheet.Cells[1, 20].Value = "тритий";
                Worksheet.Cells[1, 21].Value = "бета-, гамма-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 22].Value = "альфа-излучающие радионуклиды (исключая";
                Worksheet.Cells[1, 23].Value = "трансурановые радионуклиды";
                Worksheet.Cells[1, 24].Value = "Основные радионуклиды";
                Worksheet.Cells[1, 25].Value = "Субсидия, %";
                Worksheet.Cells[1, 26].Value = "Номер мероприятия ФЦП";
                Worksheet.Cells[1, 27].Value = "Текст статуса РАО";
                NotesHeaders2(); 
        
                #endregion
                
                break;
            }
            case "2.3":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "наименование";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "проектный объем, куб.м";
                Worksheet.Cells[1, 11].Value = "код РАО";
                Worksheet.Cells[1, 12].Value = "объем, куб.м";
                Worksheet.Cells[1, 13].Value = "масса, т";
                Worksheet.Cells[1, 14].Value = "количество ОЗИИИ, шт";
                Worksheet.Cells[1, 15].Value = "суммарная активность, Бк";
                Worksheet.Cells[1, 16].Value = "номер";
                Worksheet.Cells[1, 17].Value = "дата";
                Worksheet.Cells[1, 18].Value = "срок действия";
                Worksheet.Cells[1, 19].Value = "наименование документа";
                NotesHeaders2(); 
        
                #endregion
                
                break;
            }
            case "2.4":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Код ОЯТ";
                Worksheet.Cells[1, 9].Value = "Номер мероприятия ФЦП";
                Worksheet.Cells[1, 10].Value = "масса образованного, т";
                Worksheet.Cells[1, 11].Value = "количество образованного, шт";
                Worksheet.Cells[1, 12].Value = "масса поступивших от сторонних, т";
                Worksheet.Cells[1, 13].Value = "количество поступивших от сторонних, шт";
                Worksheet.Cells[1, 14].Value = "масса импортированных от сторонних, т";
                Worksheet.Cells[1, 15].Value = "количество импортированных от сторонних, шт";
                Worksheet.Cells[1, 16].Value = "масса учтенных по другим причинам, т";
                Worksheet.Cells[1, 17].Value = "количество учтенных по другим причинам, шт";
                Worksheet.Cells[1, 18].Value = "масса переданных сторонним, т";
                Worksheet.Cells[1, 19].Value = "количество переданных сторонним, шт";
                Worksheet.Cells[1, 20].Value = "масса переработанных, т";
                Worksheet.Cells[1, 21].Value = "количество переработанных, шт";
                Worksheet.Cells[1, 22].Value = "масса снятия с учета, т";
                Worksheet.Cells[1, 23].Value = "количество снятых с учета, шт";
                NotesHeaders2();
        
                #endregion
                
                break;
            }
            case "2.5":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "наименование, номер";
                Worksheet.Cells[1, 9].Value = "код";
                Worksheet.Cells[1, 10].Value = "код ОЯТ";
                Worksheet.Cells[1, 11].Value = "номер мероприятия ФЦП";
                Worksheet.Cells[1, 12].Value = "топливо (нетто)";
                Worksheet.Cells[1, 13].Value = "ОТВС(ТВЭЛ, выемной части реактора) брутто";
                Worksheet.Cells[1, 14].Value = "количество, шт";
                Worksheet.Cells[1, 15].Value = "альфа-излучающих нуклидов";
                Worksheet.Cells[1, 16].Value = "бета-, гамма-излучающих нуклидов";
                NotesHeaders2(); 
        
                #endregion

                break;
            }
            case "2.6":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Номер наблюдательной скважины";
                Worksheet.Cells[1, 9].Value = "Наименование зоны контроля";
                Worksheet.Cells[1, 10].Value = "Предполагаемый источник поступления радиоактивных веществ";
                Worksheet.Cells[1, 11].Value = "Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м";
                Worksheet.Cells[1, 12].Value = "Глубина отбора проб, м";
                Worksheet.Cells[1, 13].Value = "Наименование радионуклида";
                Worksheet.Cells[1, 14].Value = "Среднегодовое содержание радионуклида, Бк/кг";
                NotesHeaders2();

                #endregion
                
                break;
            }
            case "2.7":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Наименование, номер источника выбросов";
                Worksheet.Cells[1, 9].Value = "Наименование радионуклида";
                Worksheet.Cells[1, 10].Value = "разрешенный выброс за отчетный год";
                Worksheet.Cells[1, 11].Value = "фактический выброс за отчетный год";
                Worksheet.Cells[1, 12].Value = "фактический выброс за предыдущий год";
                NotesHeaders2(); 
        
                #endregion

                break;
            }
            case "2.8":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Наименование, номер выпуска сточных вод";
                Worksheet.Cells[1, 9].Value = "наименование";
                Worksheet.Cells[1, 10].Value = "код типа приемника";
                Worksheet.Cells[1, 11].Value = "Наименование бассейнового округа";
                Worksheet.Cells[1, 12].Value = "Допустимый объем водоотведения за год, тыс.куб.м";
                Worksheet.Cells[1, 13].Value = "Отведено за отчетный период, тыс.куб.м";
                NotesHeaders2();

                #endregion
                
                break;
            }
            case "2.9":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Наименование, номер выпуска сточных вод";
                Worksheet.Cells[1, 9].Value = "Наименование радионуклида";
                Worksheet.Cells[1, 10].Value = "допустимая";
                Worksheet.Cells[1, 11].Value = "фактическая";
                NotesHeaders2();

                #endregion
                
                break;
            }
            case "2.10":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Наименование показателя";
                Worksheet.Cells[1, 9].Value = "Наименование участка";
                Worksheet.Cells[1, 10].Value = "Кадастровый номер участка";
                Worksheet.Cells[1, 11].Value = "Код участка";
                Worksheet.Cells[1, 12].Value = "Площадь загрязненной территории, кв.м";
                Worksheet.Cells[1, 13].Value = "средняя";
                Worksheet.Cells[1, 14].Value = "максимальная";
                Worksheet.Cells[1, 15].Value = "альфа-излучающие радионуклиды";
                Worksheet.Cells[1, 16].Value = "бета-излучающие радионуклиды";
                Worksheet.Cells[1, 17].Value = "Номер мероприятия ФЦП";
                NotesHeaders2(); 
        
                #endregion
                
                break;
            }
            case "2.11":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Наименование участка";
                Worksheet.Cells[1, 9].Value = "Кадастровый номер участка";
                Worksheet.Cells[1, 10].Value = "Код участка";
                Worksheet.Cells[1, 11].Value = "Площадь загрязненной территории, кв.м";
                Worksheet.Cells[1, 12].Value = "Наименование радионуклидов";
                Worksheet.Cells[1, 13].Value = "земельный участок";
                Worksheet.Cells[1, 14].Value = "жидкая фаза";
                Worksheet.Cells[1, 15].Value = "донные отложения";
                NotesHeaders2();

                #endregion
                
                break;
            }
            case "2.12":
            {
                #region Headers
        
                Worksheet.Cells[1, 1].Value = "ID";
                Worksheet.Cells[1, 2].Value = "ОКПО";
                Worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                Worksheet.Cells[1, 4].Value = "Рег. №";
                Worksheet.Cells[1, 5].Value = "Номер корректировки";
                Worksheet.Cells[1, 6].Value = "отчетный год";
                Worksheet.Cells[1, 7].Value = "№ п/п";
                Worksheet.Cells[1, 8].Value = "Код операции";
                Worksheet.Cells[1, 9].Value = "Код типа объектов учета";
                Worksheet.Cells[1, 10].Value = "радионуклиды";
                Worksheet.Cells[1, 11].Value = "активность, Бк";
                Worksheet.Cells[1, 12].Value = "ОКПО поставщика/получателя";
                NotesHeaders2();

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

    /// <summary>
    /// Заполнение заголовков примечаний форм 1.x в .xlsx.
    /// </summary>
    private void NotesHeaders1()
    {
        WorksheetPrim.Cells[1, 1].Value = "ОКПО";
        WorksheetPrim.Cells[1, 2].Value = "Сокращенное наименование";
        WorksheetPrim.Cells[1, 3].Value = "Рег. №";
        WorksheetPrim.Cells[1, 4].Value = "Номер корректировки";
        WorksheetPrim.Cells[1, 5].Value = "Дата начала периода";
        WorksheetPrim.Cells[1, 6].Value = "Дата конца периода";
        WorksheetPrim.Cells[1, 7].Value = "№ строки";
        WorksheetPrim.Cells[1, 8].Value = "№ графы";
        WorksheetPrim.Cells[1, 9].Value = "Пояснение";
        WorksheetPrim.View.FreezePanes(2, 1);
    }

    /// <summary>
    /// Заполнение заголовков примечаний форм 2.x в .xlsx.
    /// </summary>
    private void NotesHeaders2()
    {
        WorksheetPrim.Cells[1, 1].Value = "ОКПО";
        WorksheetPrim.Cells[1, 2].Value = "Сокращенное наименование";
        WorksheetPrim.Cells[1, 3].Value = "Рег. №";
        WorksheetPrim.Cells[1, 4].Value = "Номер корректировки";
        WorksheetPrim.Cells[1, 5].Value = "Отчетный год";
        WorksheetPrim.Cells[1, 6].Value = "№ строки";
        WorksheetPrim.Cells[1, 7].Value = "№ графы";
        WorksheetPrim.Cells[1, 8].Value = "Пояснение";
        WorksheetPrim.View.FreezePanes(2, 1);
    }

    #endregion

    #endregion

    #region StatusRaoToDescription

    /// <summary>
    /// Преобразовывает статус РАО в его описание.
    /// </summary>
    /// <param name="status">Статус РАО.</param>
    /// <returns>Описание статуса РАО.</returns>
    private static string StatusRaoToDescription(string? status)
    {
        var tmp = status?.Trim();
        return tmp switch
        {
            "1" => "накопленные",
            "2" => "федеральные",
            "3" => "собственность субъекта РФ",
            "4" => "муниципальная собственность",
            "6" => "бесхозяйные",
            "9" => "прочая собственность",
            "-" or "" or null => "-",
            _ => "вновь образованные"
        };
    }

    #endregion
}