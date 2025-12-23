using Client_App.ViewModels;
using Models.JSON.ExecutorData;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Client_App.Properties.ExecutorDataManager;

public static class ExecutorDataManager
{
    private static readonly string SettingsPath = Path.Combine(BaseVM.ConfigDirectory, "executorData.json");

    // Опции для сериализации с обработкой ошибок
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,

    };
        
    public static void AddExecutorData(ExecutorData executorData)
    {
        var executors = GetAllExecutorData();

        if (executors.Exists
            (x => 
                x.ExecEmail == executorData.ExecEmail 
                && x.ExecPhone == executorData.ExecPhone
                && x.GradeExecutor == executorData.GradeExecutor
                && x.FIOexecutor == executorData.FIOexecutor)) return;

        //Записываем в начало списка, чтобы в начале списка хранились последние исполнители  
        executors.Insert(0,executorData);
        SaveAllExecutorData(executors);
    }

    public static List<ExecutorData> GetAllExecutorData()
    {
        if (!File.Exists(SettingsPath))
            return [];

        try
        {
            var json = File.ReadAllText(SettingsPath);
            return JsonSerializer.Deserialize<List<ExecutorData>> (json, Options) ?? [];
        }
        catch
        {
            return [];
        }
    }

    //Этот метод нужен только для того, чтобы переместить выбранного исполнителя в начало списка
    //В начале списка хранится последний исполнитель
    public static ExecutorData? GetExecutorData(ExecutorData executorData)
    {
        var executors = GetAllExecutorData();

        var executor = executors.FirstOrDefault
        (x =>
            x.ExecEmail == executorData.ExecEmail
            && x.ExecPhone == executorData.ExecPhone
            && x.GradeExecutor == executorData.GradeExecutor
            && x.FIOexecutor == executorData.FIOexecutor);
        if (executor == null) return null;

        executors.Remove(executor);
        executors.Insert(0, executor);

        SaveAllExecutorData(executors);

        return executor;

    }
    public static void DeleteExecutorData(ExecutorData executorData)
    {
        var executors = GetAllExecutorData();
        var executor = executors.FirstOrDefault
        (x =>
            x.ExecEmail == executorData.ExecEmail
            && x.ExecPhone == executorData.ExecPhone
            && x.GradeExecutor == executorData.GradeExecutor
            && x.FIOexecutor == executorData.FIOexecutor);
        if (executor == null) return;
        executors.Remove(executor);

        SaveAllExecutorData(executors);
    }
    public static void SaveAllExecutorData(List<ExecutorData> executors)
    {
        try
        {
            var json = JsonSerializer.Serialize(executors, Options);
            File.WriteAllText(SettingsPath, json);
        }
        catch (System.Exception ex)
        {
            // Логирование ошибки
            System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
        }
    }
}