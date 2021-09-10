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
        void ExcelRow(ExcelWorksheet worksheet,int Row);
    }
}
