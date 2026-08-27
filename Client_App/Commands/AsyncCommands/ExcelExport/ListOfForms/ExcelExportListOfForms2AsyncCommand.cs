using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

/// <summary>
/// Excel -> Список форм 2.
/// </summary>
public class ExcelExportListOfForms2AsyncCommand : ExcelExportListOfFormsBaseAsyncCommand
{
  public override Task AsyncExecute(object? parameter) =>
    ExportSingleFormListAsync(FormListExportGroup.Forms2);
}
