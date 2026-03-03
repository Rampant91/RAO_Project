using Avalonia.Controls;
using Client_App.ViewModels.Forms;
using Client_App.Views;
using Models.Collections;
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

namespace Client_App.Commands.AsyncCommands.Generate
{
    public class GenerateForm17AsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        Report Report => formVM.Report;

        //На вход поступает организация
        public override async Task AsyncExecute(object? parameter)
        {

            //Сообщение запрашивает коллекцию паспортов
            var mainWindow = Desktop.MainWindow as MainWindow;
            var passportCollection = await ShowAskReportMessage(mainWindow);
            if (passportCollection == null ||
                passportCollection.Count <= 0) return;
            var cts = new CancellationTokenSource();

            Report.Rows41.Clear();

            foreach (var passport in passportCollection)
            {
                var form17 = new Form17();
                Report.Rows17.Add(form17);

                form17.PackName_DB = "контейнер";

                form17.PackType_DB = passport.PackageType;
                form17.PackFactoryNumber_DB = passport.ContentCharacteristics[0].PackageIdNum.Split('/')[1];
                form17.PackNumber_DB = passport.ContentCharacteristics[0].PackageIdNum;
                form17.FormingDate_DB = passport.ManufactureDate.ToString(new CultureInfo("ru-RU"));
                form17.PassportNumber_DB = passport.PassportNum;
                form17.Volume_DB = passport.PackageVolume.ToString($"e{passport.PackageVolume.ToString().Length - 1}");
                form17.Mass_DB = (passport.PackageMass / 1000).ToString($"e{passport.PackageMass.ToString().Length - 1}");
                //form17.Radionuclids_DB =
                //form17.SpecificActivity_DB =
                //form17.ProviderOrRecieverOKPO_DB =
                //form17.TransporterOKPO_DB =
                form17.CodeRAO_DB = passport.StatusRaoCode;
                form17.StatusRAO_DB = passport.ContentCharacteristics[0].CodeRao;
                form17.VolumeOutOfPack_DB = passport.RaoVolume.ToString($"e{passport.RaoVolume.ToString().Length - 1}");
                form17.MassOutOfPack_DB = passport.RaoMass.ToString($"e{passport.RaoMass.ToString().Length - 1}");
                form17.TritiumActivity_DB = (passport.ContentCharacteristics[0].TritiumActivity * passport.RaoMass).ToString($"e{(passport.ContentCharacteristics[0].TritiumActivity * passport.RaoMass).ToString().Length - 1}");
                form17.BetaGammaActivity_DB = (passport.ContentCharacteristics[0].BetaGammaActivity * passport.RaoMass).ToString($"e{(passport.ContentCharacteristics[0].BetaGammaActivity * passport.RaoMass).ToString().Length - 1}");
                form17.AlphaActivity_DB = (passport.ContentCharacteristics[0].AlphaActivity * passport.RaoMass).ToString($"e{(passport.ContentCharacteristics[0].AlphaActivity * passport.RaoMass).ToString().Length - 1}");
                form17.TransuraniumActivity_DB = (passport.ContentCharacteristics[0].TransuraniumActivity * passport.RaoMass).ToString($"e{(passport.ContentCharacteristics[0].TransuraniumActivity * passport.RaoMass).ToString().Length - 1}");
            }
        }
        private async Task<ObservableCollection<PackagePassport>?> ShowAskReportMessage(Window owner)
        {
            var dialog = new AskPackagePassportMessage();

            var report = await dialog.ShowDialog<ObservableCollection<PackagePassport>?>(owner);
            return report;
        }
    }
}
