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
using System.Collections;

namespace Client_App.Controls.DataGrid
{
    public class KeyComand<T>
    {
        public bool IsContextCommand { get; set; }
        public string ContextMenuText { get; set; }
        public Key Key { get; set; }
        public KeyModifiers KeyModifiers { get; set; }
        public ReactiveCommand<T,Unit> Command { get; set; }

        public void DoCommand(T param)
        {
            if(Command!=null)
            {
                Command.Execute(param);
            }
        }
    }
}
