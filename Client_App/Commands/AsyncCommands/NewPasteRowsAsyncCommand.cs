using Models.Attributes;
using Models.Forms.DataAccess;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Forms;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Вставить значения из буфера обмена.
/// </summary>
public class NewPasteRowsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
    }
}