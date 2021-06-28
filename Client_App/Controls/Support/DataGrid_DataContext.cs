namespace Client_App.Controls.Support
{
    public class DataGrid_DataContext
    {
        private DataGrid.DataGrid MainControl { get; set; }
        public DataGrid_DataContext(DataGrid.DataGrid grd)
        {
            MainControl = grd;
        }
    }
}
