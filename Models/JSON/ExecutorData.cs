using Newtonsoft.Json;

namespace Models.JSON;

public class ExecutorData
{
    [JsonProperty("email")]
    public string ExecEmail { get; set; }

    [JsonProperty("phone")]
    public string ExecPhone { get; set; }

    [JsonProperty("position")]
    public string GradeExecutor { get; set; }

    [JsonProperty("full_name")]
    public string FIOexecutor { get; set; }
}