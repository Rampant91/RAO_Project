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

            Report.Rows17.Clear();

            foreach (var passport in passportCollection)
            {
                var characteristic = passport.ContentCharacteristics[0];
                var form17 = new Form17();
                Report.Rows17.Add(form17);

                form17.PackName_DB = "контейнер";

                form17.PackType_DB = passport.PackageType;

                //TODO
                //Заменить try catch на проверку через Regex
                try
                {
                    form17.PackFactoryNumber_DB = characteristic.PackageIdNum.Split('/')[1];
                }
                catch
                { }
                form17.PackNumber_DB = characteristic.PackageIdNum;
                form17.FormingDate_DB = passport.ManufactureDate.ToString(new CultureInfo("ru-RU"));
                form17.PassportNumber_DB = passport.PassportNum;
                form17.Volume_DB = passport.PackageVolume.ToString($"e{passport.PackageVolume.ToString().Length - 1}");
                form17.Mass_DB = (passport.PackageMass / 1000).ToString($"e{passport.PackageMass.ToString().Length - 1}");
                //form17.Radionuclids_DB =
                //form17.SpecificActivity_DB =
                //form17.ProviderOrRecieverOKPO_DB =
                //form17.TransporterOKPO_DB =
                form17.CodeRAO_DB = passport.StatusRaoCode;
                form17.StatusRAO_DB = characteristic.CodeRao;
                form17.VolumeOutOfPack_DB = passport.RaoVolume.ToString($"e{passport.RaoVolume.ToString().Length - 1}");
                form17.MassOutOfPack_DB = passport.RaoMass.ToString($"e{passport.RaoMass.ToString().Length - 1}");
                form17.TritiumActivity_DB = (characteristic.TritiumActivity * passport.RaoMass).ToString($"e{(characteristic.TritiumActivity * passport.RaoMass).ToString().Length - 1}");
                form17.BetaGammaActivity_DB = (characteristic.BetaGammaActivity * passport.RaoMass).ToString($"e{(characteristic.BetaGammaActivity * passport.RaoMass).ToString().Length - 1}");
                form17.AlphaActivity_DB = (characteristic.AlphaActivity * passport.RaoMass).ToString($"e{(characteristic.AlphaActivity * passport.RaoMass).ToString().Length - 1}");
                form17.TransuraniumActivity_DB = (characteristic.TransuraniumActivity * passport.RaoMass).ToString($"e{(characteristic.TransuraniumActivity * passport.RaoMass).ToString().Length - 1}");
            }

            for(int i=0; i<Report.Rows17.Count; i++)
            {
                Report.Rows17[i].NumberInOrder_DB = i + 1;
            }
            formVM.UpdateFormList();
            formVM.UpdatePageInfo();
        }
        private async Task<ObservableCollection<PackagePassport>?> ShowAskReportMessage(Window owner)
        {
            var dialog = new AskPackagePassportMessage();

            var report = await dialog.ShowDialog<ObservableCollection<PackagePassport>?>(owner);
            return report;
        }
    }
}
