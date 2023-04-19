using Avalonia.Input;
using ReactiveUI;
using System.Reactive;
using System.Windows.Input;

namespace Client_App.Controls.DataGrid;

public class KeyCommand
{
    public bool IsContextMenuCommand { get; set; }
    public bool IsDoubleTappedCommand { get; set; }
    public string[]  ContextMenuText { get; set; }
    public string ParamName { get; set; }
    public string Param { get; set; }
    public bool IsUpdateCells { get; set; }
    public Key Key { get; set; }
    public KeyModifiers KeyModifiers { get; set; }
    public ICommand Command { get; set; }

    public void DoCommand(object param)
    {
        if(Command != null)
        {
            Command.Execute(param);
        }
    }
}