using Avalonia.Controls;
using AvaloniaEdit.Utils;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.Commands.SyncCommands;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1
{
    public class Form_12VM : BaseFormVM
    {
        public override string FormType { get { return "1.2"; } }
       

        #region Constructors
        public Form_12VM() { }
        public Form_12VM(Report report) : base(report) { }
        #endregion
    }
}
