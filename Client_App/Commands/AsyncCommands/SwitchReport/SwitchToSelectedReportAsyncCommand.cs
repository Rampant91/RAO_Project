using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.Resources;
using Client_App.ViewModels.Forms;
using Client_App.Views.Forms.Forms4;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Models.Classes;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.SwitchReport
{
    public class SwitchToSelectedReportAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"> Принимает SelectedReport на который нужно переключиться</param>
        /// <returns></returns>
        public override async Task AsyncExecute(object? parameter)
        {
            if (parameter is null) return;

            if (parameter is not Report selectedReport) return;

            // Проверяем изменения и предлагаем сохранить
            var shouldContinue = await new CheckForChangesAndSaveCommand(formVM).AsyncExecute(null);
            if (!shouldContinue) return;

            var window = Desktop.Windows.First(x => x.Name == formVM.FormType);

            if (selectedReport.FormNum.Value == "4.1")
            {
                var form41 = window as Form_41;
                await new NewChangeFormAsyncCommand().AsyncExecute(selectedReport).ConfigureAwait(false);
                form41.Close();
            }
            else
            {
                var windowParam = new FormParameter()
                {
                    Parameter = new ObservableCollectionWithItemPropertyChanged<IKey>(new List<Report> { selectedReport }),
                    Window = window
                };

                await new ChangeFormAsyncCommand(windowParam).AsyncExecute(null).ConfigureAwait(false);
            }

        }
    }
}