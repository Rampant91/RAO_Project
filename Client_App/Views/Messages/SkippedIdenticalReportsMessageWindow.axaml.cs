using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

public partial class SkippedIdenticalReportsMessageWindow : BaseWindow<BaseVM>
{
    public SkippedIdenticalReportsMessageWindow()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new SkippedIdenticalReportsMessageWindowVM();
        InitializeDataGrids(ImportSummaryFormGroup.Form1);
    }

    public SkippedIdenticalReportsMessageWindow(
        IReadOnlyList<ImportReportSummaryInfo> importedReports,
        IReadOnlyList<ImportReportSummaryInfo> skippedReports,
        ImportSummaryFormGroup formGroup)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new SkippedIdenticalReportsMessageWindowVM(importedReports, skippedReports, formGroup);
        InitializeDataGrids(formGroup);
    }

    private void InitializeDataGrids(ImportSummaryFormGroup formGroup)
    {
        ConfigureDataGridColumns(this.FindControl<DataGrid>("ImportedReportsGrid"), formGroup, includeReason: false);
        ConfigureDataGridColumns(this.FindControl<DataGrid>("SkippedReportsGrid"), formGroup, includeReason: true);
    }

    private static void ConfigureDataGridColumns(DataGrid? grid, ImportSummaryFormGroup formGroup, bool includeReason)
    {
        if (grid is null)
            return;

        grid.Columns.Clear();

        switch (formGroup)
        {
            case ImportSummaryFormGroup.Form1:
                AddTextColumn(grid, "Рег. №", nameof(ImportReportSummaryInfo.OrgColumn1), 120);
                AddTextColumn(grid, "ОКПО", nameof(ImportReportSummaryInfo.OrgColumn2), 100);
                AddTextColumn(grid, "Номер формы", nameof(ImportReportSummaryInfo.FormNum), 110);
                AddTextColumn(grid, "Начало периода", nameof(ImportReportSummaryInfo.StartPeriod), "*");
                AddTextColumn(grid, "Конец периода", nameof(ImportReportSummaryInfo.EndPeriod), "*");
                break;
            case ImportSummaryFormGroup.Form2:
                AddTextColumn(grid, "Рег. №", nameof(ImportReportSummaryInfo.OrgColumn1), 120);
                AddTextColumn(grid, "ОКПО", nameof(ImportReportSummaryInfo.OrgColumn2), 100);
                AddTextColumn(grid, "Номер формы", nameof(ImportReportSummaryInfo.FormNum), 110);
                AddTextColumn(grid, "Отчётный год", nameof(ImportReportSummaryInfo.Year), "*");
                break;
            case ImportSummaryFormGroup.Form4:
                AddTextColumn(grid, "Код субъекта", nameof(ImportReportSummaryInfo.OrgColumn1), 120);
                AddTextColumn(grid, "Номер формы", nameof(ImportReportSummaryInfo.FormNum), 110);
                AddTextColumn(grid, "Отчётный год", nameof(ImportReportSummaryInfo.Year), "*");
                break;
            case ImportSummaryFormGroup.Form5:
                AddStarTextColumn(grid, "Полное наименование", nameof(ImportReportSummaryInfo.OrgColumn1), 200);
                AddTextColumn(grid, "Номер формы", nameof(ImportReportSummaryInfo.FormNum), 110);
                AddTextColumn(grid, "Отчётный год", nameof(ImportReportSummaryInfo.Year), "*");
                break;
        }

        if (includeReason)
            AddTextColumn(grid, "Причина", nameof(ImportReportSummaryInfo.Reason), 200);
    }

    private static void AddStarTextColumn(DataGrid grid, string header, string bindingPath, int minWidth)
    {
        grid.Columns.Add(new DataGridTextColumn
        {
            Header = header,
            Binding = new Avalonia.Data.Binding(bindingPath),
            IsReadOnly = true,
            Width = new DataGridLength(1, DataGridLengthUnitType.Star),
            MinWidth = minWidth
        });
    }

    private static void AddTextColumn(DataGrid grid, string header, string bindingPath, object width)
    {
        var column = new DataGridTextColumn
        {
            Header = header,
            Binding = new Avalonia.Data.Binding(bindingPath),
            IsReadOnly = true
        };

        if (width is string starWidth && starWidth == "*")
            column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        else if (width is int pixelWidth)
            column.Width = new DataGridLength(pixelWidth);

        grid.Columns.Add(column);
    }

    private void OnOkClicked(object? sender, RoutedEventArgs e) => Close();
}
