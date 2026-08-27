using Models.Forms.Form1;
using Client_App.Services.AutoReplace.Policies;

namespace Client_App.Services.AutoReplace;

public sealed class Form1AutoReplaceUiService
{
    public void RunIfNeeded(object? rowItem, string? columnBinding)
    {
        if (rowItem is not Form1 { Report.AutoReplace: true } form1)
        {
            return;
        }

        switch (form1)
        {
            case Form11 form11:
                RunByColumn(form11, columnBinding);
                break;
            case Form12 form12:
                RunByColumn(form12, columnBinding);
                break;
            case Form13 form13:
                RunByColumn(form13, columnBinding);
                break;
            case Form14 form14:
                RunByColumn(form14, columnBinding);
                break;
            case Form15 form15 when columnBinding == nameof(Form1.OperationCode):
                Form15Policy.ApplyOperationCode(form15);
                break;
            case Form16 form16 when columnBinding == nameof(Form1.OperationCode):
                Form16Policy.ApplyOperationCode(form16);
                break;
            case Form17 form17 when columnBinding == nameof(Form1.OperationCode):
                Form17Policy.ApplyOperationCode(form17);
                break;
            case Form18 form18 when columnBinding == nameof(Form1.OperationCode):
                Form18Policy.ApplyOperationCode(form18);
                break;
        }
    }

    private static void RunByColumn(Form11 form, string? columnBinding)
    {
        switch (columnBinding)
        {
            case nameof(Form1.OperationCode):
                Form11Policy.ApplyOperationCode(form);
                break;
            case nameof(Form1.OperationDate):
                Form11Policy.ApplyOperationDate(form);
                break;
            case "PassportNumber":
                Form11Policy.ApplyPassportNumber(form);
                break;
        }
    }

    private static void RunByColumn(Form12 form, string? columnBinding)
    {
        switch (columnBinding)
        {
            case nameof(Form1.OperationCode):
                Form12Policy.ApplyOperationCode(form);
                break;
            case nameof(Form1.OperationDate):
                Form12Policy.ApplyOperationDate(form);
                break;
            case "PassportNumber":
                Form12Policy.ApplyPassportNumber(form);
                break;
        }
    }

    private static void RunByColumn(Form13 form, string? columnBinding)
    {
        switch (columnBinding)
        {
            case nameof(Form1.OperationCode):
                Form13Policy.ApplyOperationCode(form);
                break;
            case nameof(Form1.OperationDate):
                Form13Policy.ApplyOperationDate(form);
                break;
            case "PassportNumber":
                Form13Policy.ApplyPassportNumber(form);
                break;
        }
    }

    private static void RunByColumn(Form14 form, string? columnBinding)
    {
        switch (columnBinding)
        {
            case nameof(Form1.OperationCode):
                Form14Policy.ApplyOperationCode(form);
                break;
            case nameof(Form1.OperationDate):
                Form14Policy.ApplyOperationDate(form);
                break;
            case "PassportNumber":
                Form14Policy.ApplyPassportNumber(form);
                break;
        }
    }
}
