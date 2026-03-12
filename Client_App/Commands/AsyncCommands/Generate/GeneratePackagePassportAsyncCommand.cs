using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.ViewModels.Forms;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Passports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Models.Collections.Report;

namespace Client_App.Commands.AsyncCommands.Generate
{
    public class GeneratePackagePassportAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        Report Report => formVM.Report;
        Window owner => (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                    .FirstOrDefault(w => w.Name == "1.7");

        //На вход поступает строки формы 1.7
        public override async Task AsyncExecute(object? parameter)
        {
            if (parameter is not IEnumerable<Form> forms17Collection) return;



            foreach (var form in forms17Collection)
            {
                if (form is not Form17 form17) continue;
                var passport = new PackagePassport();
                var characteristic = passport.ContentCharacteristics[0];

                passport.PackageType = form17.PackType_DB;

                characteristic.PackageIdNum = form17.PackNumber_DB;
                passport.ManufactureDate = DateOnly.TryParse(form17.FormingDate_DB, out var date) ? date : DateOnly.MinValue;
                passport.PassportNum = form17.PassportNumber_DB;
                passport.PackageVolume = double.TryParse(form17.Volume_DB, out var value) ? value: 0;
                passport.PackageMass = double.TryParse(form17.Mass_DB, out value) ? value * 1000 : 0;
                //form17.Radionuclids_DB =
                //form17.SpecificActivity_DB =
                //form17.ProviderOrRecieverOKPO_DB =
                //form17.TransporterOKPO_DB =
                passport.StatusRaoCode = form17.CodeRAO_DB;
                characteristic.CodeRao = form17.StatusRAO_DB;
                passport.RaoVolume = double.TryParse(form17.VolumeOutOfPack_DB, out value)? value:0;
                passport.RaoMass = double.TryParse(form17.MassOutOfPack_DB, out value) ? value : 0;
                characteristic.TritiumActivity = double.TryParse(form17.TritiumActivity_DB, out value) ? value : 0;
                characteristic.BetaGammaActivity = double.TryParse(form17.BetaGammaActivity_DB, out value) ? value : 0;
                characteristic.AlphaActivity = double.TryParse(form17.AlphaActivity_DB, out value) ? value : 0;
                characteristic.TransuraniumActivity = double.TryParse(form17.TransuraniumActivity_DB, out value) ? value : 0;

                try
                {
                    StaticConfiguration.DBModel.package_passport.Add(passport); 
                    StaticConfiguration.DBModel.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Ок" },
                        ],
                        CanResize = true,
                        ContentTitle = "Формирование паспорта на упаковку",
                        ContentMessage = "Во время формирования произошла ошибка\n" +
                        "Описание:\n" +
                        $"{ex.Message}",
                        MinWidth = 300,
                        MinHeight = 125,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(owner));
                }

            }

            Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Ок" },
                ],
                CanResize = true,
                ContentTitle = "Формирование паспорта на упаковку",
                ContentMessage = "Формирование завершено\n",
                MinWidth = 300,
                MinHeight = 125,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(owner));

        }
        private async Task<ObservableCollection<PackagePassport>?> ShowAskPackagePassportMessage(Window owner)
        {
            var dialog = new AskPackagePassportMessage();

            var report = await dialog.ShowDialog<ObservableCollection<PackagePassport>?>(owner);
            return report;
        }
    }
}
