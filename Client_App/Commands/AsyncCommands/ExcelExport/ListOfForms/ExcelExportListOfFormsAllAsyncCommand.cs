using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

/// <summary>
/// Excel -> Списки форм, присутствующих в базе, в одном файле (по листу на группу).
/// </summary>
public class ExcelExportListOfFormsAllAsyncCommand : ExcelExportListOfFormsBaseAsyncCommand
{
  public override Task AsyncExecute(object? parameter) => ExportAllFormListsAsync();
}
