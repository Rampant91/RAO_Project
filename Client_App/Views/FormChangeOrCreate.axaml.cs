using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Models.Collections;
using System.ComponentModel;
using System.Linq;
using Models.DBRealization;
using Models;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using Models.Abstracts;

namespace Client_App.Views
{
    public class FormChangeOrCreate : ReactiveWindow<ViewModels.ChangeOrCreateVM>
    {
        private readonly string _param = "";
        public FormChangeOrCreate(ViewModels.ChangeOrCreateVM param)
        {
            _param = param.FormType;
            DataContext = param;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(d => d(ViewModel!.ShowDialogIn.RegisterHandler(DoShowDialogAsync)));
            this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
            this.WhenActivated(d => d(ViewModel!.ShowMessageT.RegisterHandler(DoShowDialogAsyncT)));

            this.Closing += OnStandartClosing;
            
            Init();
        }
        public FormChangeOrCreate()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        System.Reactive.Subjects.AsyncSubject<string> Answ { get; set; } = null;
        bool flag = false;
        protected void OnStandartClosing(object sender, CancelEventArgs args)
        {
            if (Answ == null)
            {
                flag = false;
                var tmp = this.DataContext as ViewModels.ChangeOrCreateVM;
                Answ = tmp.ShowMessageT.Handle(new List<string>() { "Сохранить?", "Да", "Нет" }).GetAwaiter();
                Answ.Subscribe(x =>
                {
                    if (x == "Да")
                    {
                        flag = true;
                        tmp.SaveReport();
                        return;
                    }
                    if (x == "Нет")
                    {
                        flag = true;
                        var dbm = StaticConfiguration.DBModel;
                        dbm.Restore();
                        dbm.LoadTables();
                        dbm.SaveChanges();

                        var lst = tmp.Storage[tmp.FormType];

                        //tmp.Storage.Rows11.GetEnumerator();
                        foreach (Form item in lst)
                        {
                            if (item.Id == 0)
                            {
                                tmp.Storage[tmp.Storage.FormNum_DB].Remove(item);

                            }
                        }
                        var lstnote = tmp.Storage.Notes.ToList<Note>();
                        foreach (var item in lstnote)
                        {
                            if (item.Id == 0)
                            {
                                tmp.Storage.Notes.Remove(item);
                            }
                        }
                        if (tmp.FormType != "1.0" && tmp.FormType != "2.0")
                        {
                            if (tmp.FormType.Split('.')[0] == "1")
                            {
                                tmp.Storage.OnPropertyChanged(nameof(tmp.Storage.StartPeriod));
                                tmp.Storage.OnPropertyChanged(nameof(tmp.Storage.EndPeriod));
                                tmp.Storage.OnPropertyChanged(nameof(tmp.Storage.CorrectionNumber));
                            }
                            if (tmp.FormType.Split('.')[0] == "2")
                            {
                                tmp.Storage.OnPropertyChanged(nameof(tmp.Storage.Year));
                                tmp.Storage.OnPropertyChanged(nameof(tmp.Storage.CorrectionNumber));
                            }
                        }
                        else
                        {
                            if (tmp.FormType == "1.0" || tmp.FormType == "2.0")
                            {
                                tmp.Storage.OnPropertyChanged(nameof(tmp.Storage.RegNoRep));
                                tmp.Storage.OnPropertyChanged(nameof(tmp.Storage.ShortJurLicoRep));
                                tmp.Storage.OnPropertyChanged(nameof(tmp.Storage.OkpoRep));
                            }
                        }
                        return;
                    }
                });
                Answ.OnCompleted(() =>
                {
                    if (flag)
                    {
                        this.Close();
                    }
                    else
                    {
                        Answ.Dispose();
                        Answ = null;
                    }
                });

                args.Cancel = true;
            }
        }

        private void Form1Init(in Panel panel)
        {
            var dataContext = (ViewModels.ChangeOrCreateVM)this.DataContext;
            if (_param == "1.0")
            {
                panel.Children.Add(Long_Visual.Form1_Visual.Form10_Visual(this.FindNameScope()));
            }

            if (_param == "1.1")
            {   
                var grd = (ScrollViewer)Long_Visual.Form1_Visual.Form11_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm11)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }

            if (_param == "1.2")
            {
                var grd = (ScrollViewer)Long_Visual.Form1_Visual.Form12_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm12)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }

            if (_param == "1.3")
            {
                var grd = (ScrollViewer)Long_Visual.Form1_Visual.Form13_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm13)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }

            if (_param == "1.4")
            {
                var grd = (ScrollViewer)Long_Visual.Form1_Visual.Form14_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm14)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }

            if (_param == "1.5")
            {
                var grd = (ScrollViewer)Long_Visual.Form1_Visual.Form15_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm15)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }

            if (_param == "1.6")
            {
                var grd = (ScrollViewer)Long_Visual.Form1_Visual.Form16_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm16)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }

            if (_param == "1.7")
            {
                var grd = (ScrollViewer)Long_Visual.Form1_Visual.Form17_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm17)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }

            if (_param == "1.8")
            {
                var grd = (ScrollViewer)Long_Visual.Form1_Visual.Form18_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm18)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }

            if (_param == "1.9")
            {
                var grd = (ScrollViewer)Long_Visual.Form1_Visual.Form19_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm19)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
        }

        private void Form2Init(in Panel panel)
        {
            var dataContext = (ViewModels.ChangeOrCreateVM)this.DataContext;
            if (_param == "2.0")
            {
                panel.Children.Add(Long_Visual.Form2_Visual.Form20_Visual(this.FindNameScope()));
            }
            if (_param == "2.1")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form21_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm21)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
            if (_param == "2.2")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form22_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm22)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
            if (_param == "2.3")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form23_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm23)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
            if (_param == "2.4")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form24_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm24)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
            if (_param == "2.5")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form25_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm25)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
            if (_param == "2.6")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form26_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm26)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
            if (_param == "2.7")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form27_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm27)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
            if (_param == "2.8")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form28_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm28)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
            if (_param == "2.9")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form29_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm29)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
            if (_param == "2.10")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form210_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm210)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
            if (_param == "2.11")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form211_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm211)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
            if (_param == "2.12")
            {
                var grd = (ScrollViewer)Long_Visual.Form2_Visual.Form212_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (Controls.DataGrid.DataGridForm212)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.DuplicateRowsx1
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowIn
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRow
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (Controls.DataGrid.DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.DuplicateNotes
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new Controls.DataGrid.KeyComand()
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                #endregion

                panel.Children.Add(grd);
            }
        }

        private void Init()
        {
            Panel? panel = this.FindControl<Panel>("ChangingPanel");
            Form1Init(panel);
            Form2Init(panel);
        }

        private async Task DoShowDialogAsync(InteractionContext<int, int> interaction)
        {
            RowNumberIn frm = new RowNumberIn(interaction.Input);
            await frm.ShowDialog(this);
            interaction.SetOutput(Convert.ToInt32(frm.Number2));
        }

        private async Task DoShowDialogAsync(InteractionContext<object, int> interaction)
        {
            RowNumber frm = new RowNumber();
            await frm.ShowDialog(this);
            interaction.SetOutput(Convert.ToInt32(frm.Number));
        }

        private async Task DoShowDialogAsyncT(InteractionContext<List<string>, string> interaction)
        {
            MessageBox.Avalonia.DTO.MessageBoxCustomParams par = new MessageBox.Avalonia.DTO.MessageBoxCustomParams();
            List<MessageBox.Avalonia.Models.ButtonDefinition> lt = new List<MessageBox.Avalonia.Models.ButtonDefinition>();
            par.ContentMessage = interaction.Input[0];
            interaction.Input.RemoveAt(0);
            foreach (var elem in interaction.Input) 
            {
                lt.Add(new MessageBox.Avalonia.Models.ButtonDefinition
                {
                    Type = MessageBox.Avalonia.Enums.ButtonType.Default,
                    Name = elem
                });

            }
            par.ButtonDefinitions = lt;
            par.ContentTitle = "Уведомление";
            par.ContentHeader = "Уведомление";
            var mssg = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(par);
            var answ = await mssg.ShowDialog(this);

            interaction.SetOutput(answ);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
