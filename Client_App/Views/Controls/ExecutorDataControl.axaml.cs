using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Client_App.Controls;
using Client_App.ViewModels.Controls;
using Models.JSON.ExecutorData;
using System.Diagnostics;
using System.Numerics;

namespace Client_App.Controls;

public partial class ExecutorDataControl : UserControl
{
    public ExecutorDataControl()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

}


