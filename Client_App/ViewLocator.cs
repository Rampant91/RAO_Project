using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Client_App.ViewModels;
using System;

namespace Client_App;

public class ViewLocator : IDataTemplate
{
    public bool SupportsRecycling => false;

    public IControl Build(object data)
    {
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }
        else
        {
            return new TextBlock { Text = $"Not Found: {name}" };
        }
    }

    public bool Match(object data)
    {
        return data is BaseVM;
    }
}