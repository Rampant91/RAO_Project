using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

/// <summary>
/// Excel -> Список форм 4.
/// </summary>
public class ExcelExportListOfForms4AsyncCommand : ExcelExportListOfFormsBaseAsyncCommand
{
  public override Task AsyncExecute(object? parameter) =>
    ExportSingleFormListAsync(FormListExportGroup.Forms4);
}
