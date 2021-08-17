using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collections;

namespace FormExport
{
    public interface IExport
    {
        public void DoExport(Report Report,string Path);
    }
}
