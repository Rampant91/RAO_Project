namespace Models.Collections
{
    public interface IDataGridColumn
    {
        DataGridColumns GetColumnStructure(string param = "");
    }
}
