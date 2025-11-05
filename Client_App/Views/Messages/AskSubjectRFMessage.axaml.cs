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

    private string _text = "Выберите субъект Российской Федерации";
    public string Text 
    {
        get
        {
            return _text;
        } 
    }
    public ICollection<string> SubjectRFCollection
    {
        get
        {
            return Spravochniks.DictionaryOfSubjectRF.Values;
        }
    }

    public int? CodeOfSubjectRF
    {
        get
        {
            int? code = Spravochniks.DictionaryOfSubjectRF
                .FirstOrDefault(x => x.Value == NameOfSubjectRF)
                .Key;
            if (code == 0)
                code = null;
            return code;
        }
        set
        {
            if (Spravochniks.DictionaryOfSubjectRF.ContainsKey(value ?? 0))
                NameOfSubjectRF = Spravochniks.DictionaryOfSubjectRF[value ?? 0];

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
        Initialize();
    }
    public AskSubjectRFMessage(string text)
    {
        _text = text;
        Initialize();
    }
    private void Initialize()
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
        int code = ((AskSubjectRFMessage)DataContext).CodeOfSubjectRF ?? 0;
        string name = ((AskSubjectRFMessage)DataContext).NameOfSubjectRF;

        if ((code is 0) || (name == ""))
            Close(null);
        else
        {
            var result = code.ToString();

            if (result.Length == 1)
                result = "0" + result;

            Close(result);
        }
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