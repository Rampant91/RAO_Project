using Client_App.ViewModels.Forms.Forms1;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Forms.Form1;
using AvaloniaEdit.Utils;
namespace Client_App.Commands.AsyncCommands
{
    public class SelectAllRowsAsyncCommand(Form_12VM formVM) : BaseAsyncCommand
    {
        private ObservableCollection<Form12> FormList => formVM.FormList;
        private ObservableCollection<Form12> Selected => formVM.SelectedForms;

        public override async Task AsyncExecute(object? parameter)
        {
            // Создаем новую коллекцию вместо изменения существующей
            var allRows = new ObservableCollection<Form12>();

            allRows.AddRange(FormList);
            // Заменяем всю коллекцию (это вызовет PropertyChanged)
            formVM.SelectedForms = allRows;

        }
    }
}
