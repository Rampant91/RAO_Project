namespace Models.Collections;

public class ReportForSort
{
    #region RegNoRep
    private string _regNoRep;
    public string RegNoRep
    {
        get => _regNoRep;
        set
        {
            _regNoRep = value;
        }
    }
    #endregion

    #region OkpoRep
    private string _okpoRep;
    public string OkpoRep
    {
        get => _okpoRep;
        set
        {
            _okpoRep = value;
        }
    }
    #endregion

    #region FormNum
    private string _formNum;
    public string FormNum
    {
        get => _formNum;
        set 
        {
            _formNum = value;
        }
    }
    #endregion

    #region StartPeriod
    private long _startPeriod;
    public long StartPeriod
    {
        get => _startPeriod;
        set
        {
            _startPeriod = value;
        }
    }
    #endregion

    #region EndPeriod
    private long _endPeriod;
    public long EndPeriod
    {
        get => _endPeriod;
        set
        {
            _endPeriod = value;
        }
    }
    #endregion

    private string _shortYr;
    public string ShortYr
    {
        get => _shortYr;
        set
        {
            _shortYr = value;
        }
    }
}