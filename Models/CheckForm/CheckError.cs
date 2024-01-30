using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CheckForm;

public class CheckError
{
    public string param1 { get; set; } = "";
    public string param2 { get; set; } = "";
    public string param3 { get; set; } = "";
    public string? param4 { get; set; } = "";
    public string Message { get; set; } = "";
}