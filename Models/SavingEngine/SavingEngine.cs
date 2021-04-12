using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models.Saving
{
    public interface SavingEngine
    {
        Task Save(Forms Dictionary,string Path);
        Task<Forms> Load(string Path);
    }
}
