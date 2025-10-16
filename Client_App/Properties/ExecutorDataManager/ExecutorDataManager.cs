using Client_App.Controls;
using Client_App.ViewModels;
using Models.JSON.ExecutorData;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Client_App.Properties.ColumnWidthSettings
{
    public static class ExecutorDataManager
    {
        private static readonly string SettingsPath = BaseVM.ConfigDirectory + "\\executorData.json";

        // Опции для сериализации с обработкой ошибок
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };
        public static void AddExecutorData(ExecutorData executorData)
        {
            var executors = GetAllExecutorData();

            if (executors.Contains(executorData)) return;

            //Записываем в начало списка, чтобы в начале списка хранились последние исполнители  
            executors.Insert(0,executorData);
            SaveAllExecutorData(executors);
        }

        private static List<ExecutorData> GetAllExecutorData()
        {
            if (!File.Exists(SettingsPath))
                return new List<ExecutorData>();

            try
            {
                var json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<List<ExecutorData>> (json, Options) ?? new List<ExecutorData>();
            }
            catch
            {
                return new List<ExecutorData>();
            }
        }

        //Этот метод нужен только для того чтобы переместить выбранного исполнителя в начало списка
        //В начале списка хранится последний исполнитель
        private static ExecutorData GetExecutorData(ExecutorData executorData)
        {
            var executors = GetAllExecutorData();

            if (!executors.Contains(executorData)) return new ExecutorData();

            executors.Remove(executorData);
            executors.Insert(0,executorData);

            SaveAllExecutorData(executors);

            return executorData;

        }
        public static void DeleteExecutorData(ExecutorData executorData)
        {
            var executors = GetAllExecutorData();

            if (executors.Contains(executorData))
                executors.Remove(executorData);

            SaveAllExecutorData(executors);
        }
        private static void SaveAllExecutorData(List<ExecutorData> executors)
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
}