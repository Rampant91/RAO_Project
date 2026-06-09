using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

/// <summary>
/// Excel -> Список форм 5.
/// </summary>
public class ExcelExportListOfForms5AsyncCommand : ExcelExportListOfFormsBaseAsyncCommand
{
  public override Task AsyncExecute(object? parameter) =>
    ExportSingleFormListAsync(FormListExportGroup.Forms5);
}
