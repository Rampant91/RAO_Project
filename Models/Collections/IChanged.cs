using Models.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;
using Models.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Collections
{
    public interface IChanged: INotifyPropertyChanged
    {
        bool IsChanged { get; set; }
    }
}
