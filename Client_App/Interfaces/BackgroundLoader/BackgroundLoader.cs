using System;
using System.Threading.Tasks;

namespace Client_App.Interfaces.BackgroundLoader;
internal class BackgroundLoader : IBackgroundLoader
{
    public async void BackgroundWorker(Action action, Action onCompleted)
    {
        var task = Task.Factory.StartNew(action);
        await task;
        onCompleted();
    }
}