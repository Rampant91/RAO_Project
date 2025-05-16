using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

public partial class AskDatePeriodMessageWindow : BaseWindow<AskDatePeriodMessageVM>
{
    public AskDatePeriodMessageWindow()
    {
        AvaloniaXamlLoader.Load(this);
    }
}