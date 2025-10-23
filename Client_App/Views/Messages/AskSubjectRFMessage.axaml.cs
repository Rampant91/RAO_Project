using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Client_App.ViewModels.Messages;
using Spravochniki;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
namespace Client_App;

public partial class AskSubjectRFMessage : Window, INotifyPropertyChanged
{



    public ICollection<string> SubjectRFCollection
    {
        get
        {
            return Spravochniks.DictionaryOfSubjectRF.Values;
        }
    }

    public int CodeOfSubjectRF
    {
        get
        {
            var code = Spravochniks.DictionaryOfSubjectRF
                .FirstOrDefault(x => x.Value == NameOfSubjectRF)
                .Key;
            return code;
        }
        set
        {
            if (Spravochniks.DictionaryOfSubjectRF.ContainsKey(value))
                NameOfSubjectRF = Spravochniks.DictionaryOfSubjectRF[value];

            OnPropertyChanged(nameof(NameOfSubjectRF));
            OnPropertyChanged(nameof(CodeOfSubjectRF));
        }
    }
    private string _nameOfSubjectRF = "";
    public string NameOfSubjectRF
    {
        get
        {
            return _nameOfSubjectRF;
        }
        set
        {
            _nameOfSubjectRF = value;

            OnPropertyChanged(nameof(NameOfSubjectRF));
            OnPropertyChanged(nameof(CodeOfSubjectRF));
        }
    }
    public AskSubjectRFMessage()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = this;
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var autoCompleteBox = this.FindControl<AutoCompleteBox>("autoCompleteBox");
            autoCompleteBox.Focus();
        }, DispatcherPriority.Loaded);
    }


    
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        string? result = ((AskSubjectRFMessage)DataContext).NameOfSubjectRF;
        // Return the integer result from ViewModel
        if (result == null)
            Close("");
        else
            Close(result);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        // Return a cancellation indicator (could use null or sentinel value)
        Close(null);
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion

}