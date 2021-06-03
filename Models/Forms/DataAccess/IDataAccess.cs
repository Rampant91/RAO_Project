using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace Models.DataAccess
{
    public interface IDataAccess<T>
    {
        Func<IDataAccess<T>,bool> Handler { get; set; }
        T Value { get; set; }

        void ClearErrors();
        void AddError(string error);
    }
}
