using Client_App.ViewModels;
using Models.JSON.ExecutorData;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Client_App.Properties.ExecutorDataManager;

/// <summary>
/// Менеджер данных исполнителей
/// Работает только с файлом executorData.json
/// </summary>
public static class ExecutorDataManager
{
    private const string FileName = "executorData.json";
    private static readonly string FilePath = Path.Combine(BaseVM.ConfigDirectory, FileName);

    // Опции для сериализации с правильной кодировкой кириллицы
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>
    /// Добавляет данные исполнителя
    /// </summary>
    public static void AddExecutorData(ExecutorData executorData)
    {
        var executors = GetAllExecutorData();

        if (executors.Exists
            (x => 
                x.ExecEmail == executorData.ExecEmail 
                && x.ExecPhone == executorData.ExecPhone
                && x.GradeExecutor == executorData.GradeExecutor
                && x.FIOexecutor == executorData.FIOexecutor)) return;

        // Записываем в начало списка, чтобы в начале списка хранились последние исполнители
        executors.Insert(0, executorData);
        SaveAllExecutorData(executors);
    }

    /// <summary>
    /// Загружает все данные исполнителей
    /// </summary>
    public static List<ExecutorData> GetAllExecutorData()
    {
        if (!File.Exists(FilePath))
        {
            return [];
        }

        try
        {
            var json = File.ReadAllText(FilePath, System.Text.Encoding.UTF8);
            return JsonSerializer.Deserialize<List<ExecutorData>>(json, Options) ?? [];
        }
        catch
        {
            return [];
        }
    }

    /// <summary>
    /// Получает данные исполнителя (перемещает в начало списка)
    /// </summary>
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

    /// <summary>
    /// Удаляет данные исполнителя
    /// </summary>
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

    /// <summary>
    /// Сохраняет все данные исполнителей
    /// </summary>
    public static void SaveAllExecutorData(List<ExecutorData> executors)
    {
        try
        {
            var directory = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(executors, Options);
            File.WriteAllText(FilePath, json, System.Text.Encoding.UTF8);
        }
        catch (System.Exception ex)
        {
            // Логирование ошибки
            System.Diagnostics.Debug.WriteLine($"Failed to save executor data: {ex.Message}");
        }
    }
}
