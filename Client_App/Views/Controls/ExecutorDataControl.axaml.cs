using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Controls;
using Models.JSON.ExecutorData;

using Client_App.Resources;

namespace Client_App.Views.Controls;

public partial class ExecutorDataControl : UserControl
{
    public ExecutorDataControl()
    {
        InitializeComponent();


        //�������� ������� �� VM � ������� �� ListBoxItem (�� ���������� ��������� ������ ��������) 
        this.Initialized += (s, e) =>
        {
            var listBox = this.FindControl<ListBox>("ExecutorsListBox");
            if (listBox != null)
            {
                // ���������� ��� ������ ��������
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