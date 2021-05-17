using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Collections;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace Client_App.Controls.Support
{
    public class DataGrid_DataContext
    {
        DataGrid.DataGrid MainControl { get; set; }
        public DataGrid_DataContext(DataGrid.DataGrid grd)
        {
            MainControl = grd;
        }
    }
}
