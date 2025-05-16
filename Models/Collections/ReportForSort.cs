using System;

namespace Models.Collections;

public class ReportForSort
{
    #region RegNoRep

    public string RegNoRep { get; set; }

    #endregion

    #region OkpoRep

    public string OkpoRep { get; set; }

    #endregion

    #region FormNum

    public string FormNum { get; set; }

    #endregion

    #region StartPeriod

    public DateOnly StartPeriod { get; set; }

    #endregion

    #region EndPeriod
    
    public DateOnly EndPeriod { get; set; }

    #endregion

    public string ShortYr { get; set; }
}