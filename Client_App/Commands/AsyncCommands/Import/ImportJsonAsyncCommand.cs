using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client_App.Commands.AsyncCommands.Import
{
    public class ImportJsonAsyncCommand : ImportBaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            string[] extensions = { "json", "JSON" };
            var answer = await GetSelectedFilesFromDialog("JSON", extensions);
            if (answer is null) return;

            foreach (var path in answer) // Для каждого импортируемого файла
            {
                try
                {
                    var jsonString = await File.ReadAllTextAsync(path);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    var jsonObject = JsonConvert.DeserializeObject<MyJsonModel2>(jsonString, settings);
                }
                catch (Exception e)
                {
                    //ignore
                }
                

            }            
        }
    }

    //public class JsonModel
    //{
    //    [JsonIgnore]
    //    [JsonPropertyName("vocs")]
    //    public List<Voc> vocs;

    //    [JsonIgnore]
    //    [JsonPropertyName("forms")]
    //    public List<Forms> forms;

    //    [JsonPropertyName("orgs")]
    //    public List<Org> orgs;
    //}

    //public class Org
    //{

    //}

    //public class Voc
    //{

    //}

    //public class Forms
    //{

    //}
}