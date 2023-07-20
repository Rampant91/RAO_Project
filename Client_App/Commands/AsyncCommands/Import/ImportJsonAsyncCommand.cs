using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Import
{
    public class ImportJsonAsyncCommand : ImportBaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            string[] extensions = { "json", "JSON" };
            var answer = await GetSelectedFilesFromDialog("RAODB", extensions);
            if (answer is null) return;
            foreach (var path in answer) // Для каждого импортируемого файла
            {
                using FileStream fs = new(path, FileMode.OpenOrCreate);
                //var orgsList = await JsonSerializer.DeserializeAsync<>(fs)
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