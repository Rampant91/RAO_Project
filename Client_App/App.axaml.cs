using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Client_App.Views;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;

namespace Client_App;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var instanceCheckMutex = new Mutex(true, "<Client_App>", out var isNew);
            if (!isNew)
            {
                instanceCheckMutex.Dispose();
                #region MessageAppAlreadyOpen
                var res = await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions = new[]
                        {
                            new ButtonDefinition { Name = "��" },
                            new ButtonDefinition { Name = "���" }
                        },
                        ContentTitle = "������ ���������",
                        ContentHeader = "�����������",
                        ContentMessage = "��������� ��� ���� �������� �����." +
                                         $"{Environment.NewLine}�� �������, ��� ������ ������� ��� ���� ����� ���������?",
                        MinWidth = 400,
                        MinHeight = 120,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(desktop.MainWindow); 
                #endregion
                if (res is "���") Environment.Exit(0);
            }
            desktop.MainWindow = new OnStartProgressBar();
        }
        base.OnFrameworkInitializationCompleted();
    }
}