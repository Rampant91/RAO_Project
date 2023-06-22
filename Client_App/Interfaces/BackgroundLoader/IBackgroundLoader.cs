using System;

namespace Client_App.Interfaces.BackgroundLoader;

public interface IBackgroundLoader
{
    public void BackgroundWorker(Action action, Action onCompleted);
}