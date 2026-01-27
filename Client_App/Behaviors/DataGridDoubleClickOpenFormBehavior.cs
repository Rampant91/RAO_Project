using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit.Utils;
using Client_App.Commands.AsyncCommands;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Behaviors
{
    public class DataGridDoubleClickOpenFormBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                // Подписываемся на события
                AssociatedObject.DoubleTapped += DataGrid_DoubleTapped; ;
            }
        }


        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.DoubleTapped -= DataGrid_DoubleTapped;
            }

            base.OnDetaching();
        }

        private void DataGrid_DoubleTapped(object? sender, RoutedEventArgs e)
        {
            if (AssociatedObject?.SelectedItem != null)
            {
                if(AssociatedObject?.SelectedItem is Reports reports)
                {
                    var command = new NewChangeReportsAsyncCommand();
                    command.AsyncExecute(reports);
                }
                else if (AssociatedObject?.SelectedItem is Report report)
                {
                    BaseAsyncCommand command;

                    if (report.FormNum_DB.Split('.')[0] == "2")
                        command = new ChangeFormAsyncCommand();
                    else
                        command = new NewChangeFormAsyncCommand();

                    command.Execute(report);
                }
            }

        }
    }

}
