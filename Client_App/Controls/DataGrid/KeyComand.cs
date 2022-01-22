using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Models.Collections;
using ReactiveUI;
using System.Reactive;

namespace Client_App.Controls.DataGrid
{
    public class KeyComand
    {
        Key Key { get; set; }
        KeyModifiers KeyModifiers { get; set; }
         Func { get; set; }
    }
}
