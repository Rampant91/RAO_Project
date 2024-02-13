using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Client_App.Views;

namespace Client_App;

public partial class CompareReportsTitleForm : BaseWindow<CompareReportsTitleFormVM>
{
    public CompareReportsTitleForm()
    {
        InitializeComponent();
        Show();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new CompareReportsTitleFormVM();
    }
}