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
    public class KeyComand
    {
        public bool IsContextMenuCommand { get; set; }
        public bool IsDoubleTappedCommand { get; set; }
        public string[]  ContextMenuText { get; set; }
        public string ParamName { get; set; }
        public string Param { get; set; }
        public bool IsUpdateCells { get; set; }
        public Key Key { get; set; }
        public KeyModifiers KeyModifiers { get; set; }
        public ReactiveCommand<object,Unit> Command { get; set; }

        public void DoCommand(object param)
        {
            if(Command!=null)
            {
                Command.Execute(param);
            }
        }
    }
}
