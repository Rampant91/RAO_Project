using AvaloniaEdit.Utils;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms1;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands
{
    public class SelectAllRowsAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        private ObservableCollection<Form> FormList => formVM.FormList;
        private ObservableCollection<Form> Selected => formVM.SelectedForms;

        public override async Task AsyncExecute(object? parameter)
        {
            // Создаем новую коллекцию вместо изменения существующей
            var allRows = new ObservableCollection<Form>();

            allRows.AddRange(FormList);
            // Заменяем всю коллекцию (это вызовет PropertyChanged)
            formVM.SelectedForms = allRows;

        }
    }
}
