using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO;

public class CalculatorRadionuclidDTO
{
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    public required double Halflife { get; set; }
    public required string Unit { get; set; }
    public required string Code { get; set; }
}