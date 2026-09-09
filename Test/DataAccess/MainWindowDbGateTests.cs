using System.Threading.Tasks;
using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class MainWindowDbGateTests
{
    [Fact]
    public async Task RunExclusive_DoesNotOverlap()
    {
        var entered = 0;
        var maxConcurrent = 0;
        var sync = new object();

        async Task Worker()
        {
            await MainWindowDbGate.RunExclusiveForTestsAsync(async ct =>
            {
                lock (sync)
                {
                    entered++;
                    if (entered > maxConcurrent)
                        maxConcurrent = entered;
                }

                await Task.Delay(50, ct);

                lock (sync)
                    entered--;
            });
        }

        await Task.WhenAll(Worker(), Worker(), Worker());
        Assert.Equal(1, maxConcurrent);
    }
}
