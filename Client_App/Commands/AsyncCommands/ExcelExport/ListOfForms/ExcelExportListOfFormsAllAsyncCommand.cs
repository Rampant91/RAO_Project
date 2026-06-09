using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

/// <summary>
/// Excel -> Списки форм 1, 2, 4 и 5 в одном файле (по листу на группу).
/// </summary>
public class ExcelExportListOfFormsAllAsyncCommand : ExcelExportListOfFormsBaseAsyncCommand
{
  public override Task AsyncExecute(object? parameter) => ExportAllFormListsAsync();
}
