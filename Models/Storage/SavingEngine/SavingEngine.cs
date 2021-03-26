using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models.Storage
{
    public interface SavingEngine
    {
        Task Save(LocalDictionary Dictionary,string Path);
        Task<LocalDictionary> Load(string Path);
    }
}
