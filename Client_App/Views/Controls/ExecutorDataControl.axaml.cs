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


        //Привязка комманд из VM к кнопкам из ListBoxItem (Не получилось привязать другим способом) 
        this.Initialized += (s, e) =>
        {
            var listBox = this.FindControl<ListBox>("ExecutorsListBox");
            if (listBox != null)
            {
                // Обработчик для кнопок удаления
                listBox.AddHandler(Button.ClickEvent, (sender, args) =>
                {
                    if (args.Source is Button button && button.DataContext is ExecutorData executor)
                    {
                        var vm = DataContext as ExecutorDataControlVM;
                        vm?.DeleteExecutor.Execute(executor);
                    }
                });
                listBox.SelectionChanged += (sender, args) =>
                {
                    if (listBox.SelectedItem is ExecutorData selectedExecutor)
                    {
                        var vm = DataContext as ExecutorDataControlVM;
                        vm?.GetExecutor?.Execute(selectedExecutor);
                    }
                    listBox.SelectedItem = null;
                };
            }
        };
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

    }

}


