using Models.Collections;

namespace Models.Interfaces;

public interface IDataGridColumn
{
    DataGridColumns GetColumnStructure(string param = "");
}