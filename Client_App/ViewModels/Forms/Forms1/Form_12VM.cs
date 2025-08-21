using Models.Collections;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_12VM : BaseFormVM
{
    public override string FormType => "1.2";

    #region Constructors

    public Form_12VM() { }
    public Form_12VM(Report report) : base(report) { }

    #endregion
}