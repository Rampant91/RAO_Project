using System;
using System.Threading;
using System.Threading.Tasks;
using Models.DBRealization;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Очередь к Firebird embedded для фоновых snapshot-<see cref="DBModel"/> на основной .raodb.
/// Параллельные соединения на один файл дают нативные падения
/// (в т.ч. ExecutionEngineException в LoadReportStubs).
/// Не оборачивает долгоживущий <c>StaticConfiguration.DBModel</c> и tmp-копии БД.
/// </summary>
internal static class MainWindowDbGate
{
    private static readonly SemaphoreSlim Gate = new(1, 1);

    public static T Run<T>(string dbPath, Func<DBModel, T> work)
    {
        Gate.Wait();
        try
        {
            using var db = new DBModel(dbPath);
            return work(db);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            FirebirdLogger.LogError("MainWindowDbGate.Run failed path=" + dbPath, ex);
            throw;
        }
        finally
        {
            Gate.Release();
        }
    }

    public static void Run(string dbPath, Action<DBModel> work)
    {
        Gate.Wait();
        try
        {
            using var db = new DBModel(dbPath);
            work(db);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            FirebirdLogger.LogError("MainWindowDbGate.Run(action) failed path=" + dbPath, ex);
            throw;
        }
        finally
        {
            Gate.Release();
        }
    }

    public static async Task RunAsync(
        string dbPath,
        Func<DBModel, CancellationToken, Task> work,
        CancellationToken cancellationToken = default)
    {
        await Gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await using var db = new DBModel(dbPath);
            await work(db, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            FirebirdLogger.LogError("MainWindowDbGate.RunAsync failed path=" + dbPath, ex);
            throw;
        }
        finally
        {
            Gate.Release();
        }
    }

    public static async Task<T> RunAsync<T>(
        string dbPath,
        Func<DBModel, CancellationToken, Task<T>> work,
        CancellationToken cancellationToken = default)
    {
        await Gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await using var db = new DBModel(dbPath);
            return await work(db, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            FirebirdLogger.LogError("MainWindowDbGate.RunAsync<T> failed path=" + dbPath, ex);
            throw;
        }
        finally
        {
            Gate.Release();
        }
    }

    /// <summary>Только для тестов: секция без открытия DBModel.</summary>
    internal static async Task RunExclusiveForTestsAsync(
        Func<CancellationToken, Task> work,
        CancellationToken cancellationToken = default)
    {
        await Gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await work(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            Gate.Release();
        }
    }
}
