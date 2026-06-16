using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.Views.Forms.Forms2;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.SpecificCommands
{

    //Команда для подсчета кол-ва строк во всех отчетах за выбранный год в выбранном регионе

    //Нигде не используется, но через какое то время может пригодится
    public class CountRowsInAllReportByRegionAndYearCommand : BaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            try
            {
                //Хардкод параметров
                string codeSubjectRF = "24";
                int year = 2026;

                var result = new int[12];
                var dbm = StaticConfiguration.DBModel;
                var reportList = dbm.ReportCollectionDbSet
                      .Include(rep => rep.Reports)
                      .Where(rep => rep.Reports != null )
                    .ToList()
                    .Where(rep => rep.FormNum_DB.StartsWith("1") || rep.FormNum_DB.StartsWith("2"))
                    .Where(rep => rep.Reports.Master_DB.RegNoRep.Value.StartsWith(codeSubjectRF))
                    .ToList();

                List<int> reportIdList = new List<int>();

                foreach (Report report in reportList)
                {

                    if (!DateOnly.TryParse(report.ExportDate_DB, out var exportDate)) continue;

                    if (exportDate.Year != year) continue;

                    int count = 0;
                    count += StaticConfiguration.DBModel.form_11.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_12.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_13.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_14.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_15.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_16.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_17.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_18.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_19.Count(form => report.Id == form.ReportId);


                    count += StaticConfiguration.DBModel.form_21.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_22.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_23.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_24.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_25.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_26.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_27.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_28.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_29.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_210.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_211.Count(form => report.Id == form.ReportId);
                    count += StaticConfiguration.DBModel.form_212.Count(form => report.Id == form.ReportId);

                    result[exportDate.Month - 1] += count;
                }


                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Да" },
                        new ButtonDefinition { Name = "Нет" },
                        ],
                        CanResize = true,
                        ContentTitle = "Формирование нового отчета",
                        ContentMessage =
                        $"Январь - {result[0]}" +
                        $"Февраль - {result[1]}" +
                        $"Март - {result[2]}" +
                        $"Апрель - {result[3]}" +
                        $"Май - {result[4]}" +
                        $"Июнь - {result[5]}" +
                        $"Июль - {result[6]}" +
                        $"Август - {result[7]}" +
                        $"Сентябрь - {result[8]}" +
                        $"Октябрь - {result[9]}" +
                        $"Ноябрь - {result[10]}" +
                        $"Декабрь - {result[11]}",
                        MinWidth = 300,
                        MinHeight = 125,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    })
                    .ShowDialog(Desktop.MainWindow);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

