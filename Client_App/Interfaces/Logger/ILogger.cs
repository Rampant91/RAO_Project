using Client_App.Interfaces.Logger.EnumLogger;

namespace Client_App.Interfaces.Logger;

public interface ILogger
{
    public void Info(string msg, ErrorCodeLogger code);
    public void Error(string msg, ErrorCodeLogger code);
    public void Debug(string msg, ErrorCodeLogger code);
    public void Warning(string msg, ErrorCodeLogger code);
    public void Import(string msg, ErrorCodeLogger code);
}