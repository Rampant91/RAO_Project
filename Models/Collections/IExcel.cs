using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Models.Collections
{
    public interface IExcel
    {
        int ExcelRow(ExcelWorksheet worksheet,int Row,int Column,bool Tranpon=true);
    }
}
