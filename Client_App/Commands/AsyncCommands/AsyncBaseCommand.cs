using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

public abstract class AsyncBaseCommand
{
    private bool _isExecute = false;

    public bool isExecute
    {
        get => _isExecute;
        set
        {
            _isExecute = value;
            OnCanExecuteChanged();
        }
    }
}