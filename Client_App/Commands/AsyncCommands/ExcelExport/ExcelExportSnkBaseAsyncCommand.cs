using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Client_App.Commands.AsyncCommands.ExcelExport;
public abstract class ExcelExportSnkBaseAsyncCommand : ExcelBaseAsyncCommand
{
    #region Properties

    private protected static readonly string[] MinusOperation =
    [
        "21", "22", "25", "27", "28", "29", "41", "42", "43", "46", "47", "65", "67", "71", "72", "81", "82", "83", "84", "98"
    ];

    private protected static readonly string[] PlusOperation =
    [
        "11", "12", "17", "31", "32", "35", "37", "38", "39", "58", "73", "74", "75", "85", "86", "87", "88", "97"
    ];

    #endregion

    #region DTO

    private protected class ShortForm11DTO(int id, ShortReportDTO repDto, string facNum, string opCode, DateOnly opDate, string pasNum, string type)
    {
        public readonly int Id = id;

        public readonly ShortReportDTO RepDto = repDto;

        public readonly string FacNum = facNum;

        public readonly string OpCode = opCode;

        public readonly DateOnly OpDate = opDate;

        public readonly string PasNum = pasNum;

        public readonly string Type = type;
    }

    private protected class ShortForm11StringDateDTO
    {
        public int Id { get; set; }

        public int RepId { get; set; }

        public DateOnly StDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string FacNum { get; set; }

        public string OpCode { get; set; }

        public string OpDate { get; set; }

        public string PasNum { get; set; }

        public string Type { get; set; }
    }

    private protected class ShortForm11StringDatesDTO
    {
        public int Id { get; set; }

        public int RepId { get; set; }

        public string StDate { get; set; }

        public string EndDate { get; set; }

        public string FacNum { get; set; }

        public string OpCode { get; set; }

        public string OpDate { get; set; }

        public string PasNum { get; set; }

        public string Type { get; set; }
    }

    private protected class ShortReportStringDateDTO(int id, string startPeriod, string endPeriod)
    {
        public readonly int Id = id;

        public readonly string StartPeriod = startPeriod;

        public readonly string EndPeriod = endPeriod;
    }

    private protected class ShortReportDTO(int id, DateOnly startPeriod, DateOnly endPeriod)
    {
        public readonly int Id = id;

        public readonly DateOnly StartPeriod = startPeriod;

        public readonly DateOnly EndPeriod = endPeriod;
    }

    #endregion

    #region GetInventoryFormsDtoList

    /// <summary>
    /// Получение списка DTO операций инвентаризации.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="inventoryReportDtoList">Список DTO отчётов, содержащих операцию инвентаризации.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO операций инвентаризации, отсортированный по датам.</returns>
    private protected static async Task<List<ShortForm11DTO>> GetInventoryFormsDtoList(DBModel db, List<ShortReportDTO> inventoryReportDtoList,
        DateOnly endSnkDate, CancellationTokenSource cts)
    {
        List<ShortForm11DTO> inventoryFormsDtoList = [];
        foreach (var reportDto in inventoryReportDtoList)
        {
            var currentInventoryFormsStringDateDtoList = await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Rows11)
                .Where(rep => rep.Id == reportDto.Id)
                .SelectMany(rep => rep.Rows11
                    .Where(form => form.OperationCode_DB == "10")
                    .Select(form11 => new ShortForm11StringDateDTO
                    {
                        Id = form11.Id,
                        RepId = reportDto.Id,
                        StDate = reportDto.StartPeriod,
                        EndDate = reportDto.EndPeriod,
                        FacNum = form11.FactoryNumber_DB,
                        OpCode = form11.OperationCode_DB,
                        OpDate = form11.OperationDate_DB,
                        PasNum = form11.PassportNumber_DB,
                        Type = form11.Type_DB
                    }))
                .ToListAsync(cts.Token);

            var currentInventoryFormsDtoList = currentInventoryFormsStringDateDtoList
                .Where(x => DateOnly.TryParse(x.OpDate, out var opDateOnly)
                            && opDateOnly <= endSnkDate)
                .Select(x => new ShortForm11DTO(
                    x.Id,
                    reportDto,
                    x.FacNum,
                    x.OpCode,
                    DateOnly.Parse(x.OpDate),
                    x.PasNum,
                    x.Type))
                .ToList();

            inventoryFormsDtoList.AddRange(currentInventoryFormsDtoList);
        }
        return inventoryFormsDtoList
            .OrderBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ThenBy(x => x.OpDate)
            .ToList();
    }

    #endregion

    #region GetInventoryReportDtoList

    /// <summary>
    /// Получение списка DTO отчётов по форме 1.1, содержащих хотя бы одну операцию с кодом 10.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="repsId">Id выбранной организации.</param>
    /// <param name="endSnkDate">Дата, по которую нужно определить СНК.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO отчётов по форме 1.1, отсортированный по датам.</returns>
    private protected static async Task<List<ShortReportDTO>> GetInventoryReportDtoList(DBModel db, int repsId, DateOnly endSnkDate,
        CancellationTokenSource cts)
    {
        var inventoryReportDtoList = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.1" && rep.Rows11.Any(form => form.OperationCode_DB == "10"))
                .Select(rep => new ShortReportStringDateDTO(rep.Id, rep.StartPeriod_DB, rep.EndPeriod_DB)))
            .ToListAsync(cts.Token);

        return inventoryReportDtoList
            .Where(x => DateOnly.TryParse(x.StartPeriod, out var stPer)
                        && DateOnly.TryParse(x.EndPeriod, out var endPer)
                        && stPer >= DateOnly.Parse("01.01.2022")
                        && endPer <= endSnkDate)
            .Select(x => new ShortReportDTO(
                x.Id,
                DateOnly.Parse(x.StartPeriod),
                DateOnly.Parse(x.EndPeriod)))
            .OrderBy(x => x.StartPeriod)
            .ThenBy(x => x.EndPeriod)
            .ToList();
    }

    #endregion

    #region GetPlusMinusFormsDtoList

    /// <summary>
    /// Получение списка DTO форм с операциями приёма передачи.
    /// </summary>
    /// <param name="db">Модель БД.</param>
    /// <param name="repsId">Id организации.</param>
    /// <param name="cts">Токен.</param>
    /// <returns>Список DTO форм с операциями приёма передачи, отсортированный по датам.</returns>
    private protected static async Task<List<ShortForm11DTO>> GetPlusMinusFormsDtoList(DBModel db, int repsId, DateOnly endSnkDate, 
        CancellationTokenSource cts)
    {
        var plusMinusOperationDtoList = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .SelectMany(rep => rep.Rows11
                    .Where(form => form.Report != null
                                   && (PlusOperation.Contains(form.OperationCode_DB)
                                       || MinusOperation.Contains(form.OperationCode_DB)))))
                .Select(form11 => new ShortForm11StringDatesDTO
                {
                    Id = form11.Id,
                    RepId = form11.Report.Id,
                    StDate = form11.Report.StartPeriod_DB,
                    EndDate = form11.Report.EndPeriod_DB,
                    FacNum = form11.FactoryNumber_DB,
                    OpCode = form11.OperationCode_DB,
                    OpDate = form11.OperationDate_DB,
                    PasNum = form11.PassportNumber_DB,
                    Type = form11.Type_DB
                })
            .ToListAsync(cts.Token);

        return plusMinusOperationDtoList
            .Where(x => DateOnly.TryParse(x.OpDate, out var opDateOnly)
                                             && DateOnly.TryParse(x.StDate, out _)
                                             && DateOnly.TryParse(x.EndDate, out _)
                                             && opDateOnly <= endSnkDate)
            .Select(x => new ShortForm11DTO(
                x.Id,
                new ShortReportDTO(x.RepId, DateOnly.Parse(x.StDate), DateOnly.Parse(x.EndDate)),
                x.FacNum,
                x.OpCode,
                DateOnly.Parse(x.OpDate),
                x.PasNum,
                x.Type))
            .OrderBy(x => x.RepDto.StartPeriod)
            .ThenBy(x => x.RepDto.EndPeriod)
            .ThenBy(x => x.OpDate)
            .ToList();
    }

    #endregion

    #region GetUniqueAccountingUnitDtoList

    /// <summary>
    /// Получение отсортированного списка DTO уникальных учётных единиц с операциями инвентаризации, приёма или передачи.
    /// </summary>
    /// <param name="unionFormsDtoList">Список DTO всех операций инвентаризации, приёма или передачи.</param>
    /// <returns>Список DTO уникальных учётных единиц с операциями инвентаризации, приёма или передачи.</returns>
    private protected static Task<List<ShortForm11DTO>> GetUniqueAccountingUnitDtoList(List<ShortForm11DTO> unionFormsDtoList)
    {
        var uniqueAccountingUnitDtoList = unionFormsDtoList
            .DistinctBy(x => x.FacNum + x.Type + x.PasNum)
            .OrderBy(x => x.FacNum + x.Type + x.PasNum)
            .ToList();

        return Task.FromResult(uniqueAccountingUnitDtoList);
    }

    #endregion

    #region GetUnionFormsDtoList

    private protected static Task<List<ShortForm11DTO>> GetUnionFormsDtoList(List<ShortForm11DTO> inventoryFormsDtoList,
        List<ShortForm11DTO> plusMinusFormsDtoList, DateOnly firstInventoryDate)
    {
        var unionFormsDtoList = inventoryFormsDtoList
            .Union(plusMinusFormsDtoList)
            .Where(x => x.OpDate >= firstInventoryDate)
            .ToList();

        return Task.FromResult(unionFormsDtoList);
    }

    #endregion
}