using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Collections;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_12VM : BaseFormVM
{
    public override string FormType => "1.2";

    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    #region Constructors

    public Form_12VM() { }
    public Form_12VM(Report report) : base(report) { }

    #endregion
}