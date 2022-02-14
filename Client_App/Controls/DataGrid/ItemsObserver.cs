﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Models.Collections;

namespace Client_App.Controls.DataGrid
{
    public class ItemsObserver : IObserver<AvaloniaPropertyChangedEventArgs<IEnumerable<IKey>>>
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

        public void OnNext(AvaloniaPropertyChangedEventArgs<IEnumerable<IKey>> obj)
        {
            if (obj.NewValue.Value != null)
                if (Handler != null)
                    if (Handler.Target != null)
                        if (((Control)obj.Sender).Name == ((Control)Handler.Target).Name)
                        {
                            var rt = new List<INotifyPropertyChanged>(obj.NewValue.Value);
                            if (rt.Count > 0)
                            {
                                Handler(rt, null);
                            }
                        }
        }
    }
}