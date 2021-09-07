using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;

namespace Client_App.Controls.DataGrid
{
    public class ItemsObserver : IObserver<AvaloniaPropertyChangedEventArgs<IEnumerable<INotifyPropertyChanged>>>
    {
        public ItemsObserver(PropertyChangedEventHandler handler)
        {
            Handler = handler;
        }

        private PropertyChangedEventHandler Handler { get; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception e)
        {
        }

        public void OnNext(AvaloniaPropertyChangedEventArgs<IEnumerable<INotifyPropertyChanged>> obj)
        {
            if (obj.NewValue.Value != null)
                if (Handler != null)
                    if (Handler.Target != null)
                        if (((Control) obj.Sender).Name == ((Control) Handler.Target).Name)
                            Handler(obj.NewValue.Value, null);
        }
    }
}