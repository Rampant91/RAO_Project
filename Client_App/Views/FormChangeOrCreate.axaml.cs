using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System.ComponentModel;
using Models.DBRealization;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Controls.DataGrid;
using Client_App.Controls.DataGrid.DataGrids;
using Client_App.VisualRealization.Long_Visual;
using MessageBox.Avalonia.Models;
using Models.Forms;
using Client_App.ViewModels;

namespace Client_App.Views;

public class FormChangeOrCreate : BaseWindow<ChangeOrCreateVM>
{
    private readonly string _param = "";
    public FormChangeOrCreate(ChangeOrCreateVM param)
    {
        _param = param.FormType;
        DataContext = param;
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.WhenActivated(d =>
        {
            var vm = (ChangeOrCreateVM)ViewModel;
            d((vm!.ShowDialogIn.RegisterHandler(DoShowDialogAsync)));
            d((vm.ShowDialog.RegisterHandler(DoShowDialogAsync)));
            d((vm.ShowMessageT.RegisterHandler(DoShowDialogAsyncT)));
        });

        Closing += OnStandartClosing;
            
        Init();
    }
    public FormChangeOrCreate()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    System.Reactive.Subjects.AsyncSubject<string> Answ { get; set; }

    private void OnStandartClosing(object sender, CancelEventArgs args)
    {
        bool flag;
        if (Answ == null)
        {
            flag = false;
            var tmp = DataContext as ChangeOrCreateVM;
            Answ = tmp.ShowMessageT.Handle(new List<string> { "Сохранить?", "Да", "Нет" }).GetAwaiter();
            Answ.Subscribe(async x =>
            {
                switch (x)
                {
                    case "Да":
                        flag = true;
                        await new SaveReportAsyncCommand(tmp).AsyncExecute(null);
                        return;
                    case "Нет":
                    {
                        flag = true;
                        var dbm = StaticConfiguration.DBModel;
                        dbm.Restore();
                        await dbm.LoadTablesAsync();
                        await dbm.SaveChangesAsync();

                        var lst = tmp.Storage[tmp.FormType];

                        //tmp.Storage.Rows11.GetEnumerator();
                        foreach (var key in lst)
                        {
                            var item = (Form)key;
                            if (item.Id == 0)
                            {
                                tmp.Storage[tmp.Storage.FormNum_DB].Remove(item);
                            }
                        }

                        var lstNote = tmp.Storage.Notes.ToList<Note>();
                        foreach (var item in lstNote.Where(item => item.Id == 0))
                        {
                            tmp.Storage.Notes.Remove(item);
                        }

                        if (tmp.FormType is not "1.0" and not "2.0")
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
                            tmp.Storage.OnPropertyChanged(nameof(tmp.Storage.RegNoRep));
                            tmp.Storage.OnPropertyChanged(nameof(tmp.Storage.ShortJurLicoRep));
                            tmp.Storage.OnPropertyChanged(nameof(tmp.Storage.OkpoRep));
                        }
                        break;
                    }
                }
            });
            Answ.OnCompleted(() =>
            {
                if (flag)
                {
                    Close();
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

    #region Init
    private void Form1Init(in Panel panel)
    {
        var dataContext = (ChangeOrCreateVM)DataContext;
        switch (_param)
        {
            case "1.0":
                panel.Children.Add(Form1_Visual.Form10_Visual(this.FindNameScope()));
                break;
            case "1.1":
            {
                var grd = (ScrollViewer)Form1_Visual.Form11_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm11)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new[] { "Выделить все                                            Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку                                      Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк                                    Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Добавить N строк перед                         Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                                               Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                                                    Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    IsUpdateCells = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки                                         Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Выставить номер п/п                               Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки                                      Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.P,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Открыть паспорт                                      Ctrl+P" },
                    Command = dataContext.OpenPas
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.E,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Выгрузка в Excel движения источника   Ctrl+E" },
                    Command = dataContext.ExcelExportSourceMovementHistory
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.K,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Скопировать в буфер имя паспорта      Ctrl+K" },
                    Command = dataContext.CopyPasName
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
                case "1.2":
            {
                var grd = (ScrollViewer)Form1_Visual.Form12_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm12)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "1.3":
            {
                var grd = (ScrollViewer)Form1_Visual.Form13_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm13)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "1.4":
            {
                var grd = (ScrollViewer)Form1_Visual.Form14_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm14)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "1.5":
            {
                var grd = (ScrollViewer)Form1_Visual.Form15_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm15)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                //Rgrd.CommandsList.Add(new KeyCommand
                //{
                //    Key = Avalonia.Input.Key.P,
                //    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                //    IsDoubleTappedCommand = false,
                //    IsContextMenuCommand = true,
                //    ParamName = "Copy",
                //    ContextMenuText = new[] { "Открыть паспорт                                      Ctrl+P" },
                //    Command = dataContext.OpenPassport
                //});
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.E,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Выгрузка в Excel движения источника   Ctrl+E" },
                    Command = dataContext.ExcelExportSourceMovementHistory
                });
                //Rgrd.CommandsList.Add(new KeyCommand
                //{
                //    Key = Avalonia.Input.Key.K,
                //    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                //    IsDoubleTappedCommand = false,
                //    IsContextMenuCommand = true,
                //    ParamName = "Copy",
                //    ContextMenuText = new[] { "Скопировать в буфер имя паспорта      Ctrl+K" },
                //    Command = dataContext.CopyPasName
                //});
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "1.6":
            {
                var grd = (ScrollViewer)Form1_Visual.Form16_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm16)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "1.7":
            {
                var grd = (ScrollViewer)Form1_Visual.Form17_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm17)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "1.8":
            {
                var grd = (ScrollViewer)Form1_Visual.Form18_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm18)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "1.9":
            {
                var grd = (ScrollViewer)Form1_Visual.Form19_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm19)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
        }
    }

    private void Form2Init(in Panel panel)
    {
        var dataContext = (ChangeOrCreateVM)DataContext;
        switch (_param)
        {
            case "2.0":
            {
                panel.Children.Add(Form2_Visual.Form20_Visual(this.FindNameScope()));
                break;
            }
            case "2.1":
            {
                var grd = (ScrollViewer)Form2_Visual.Form21_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm21)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "2.2":
            {
                var grd = (ScrollViewer)Form2_Visual.Form22_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm22)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "2.3":
            {
                var grd = (ScrollViewer)Form2_Visual.Form23_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm23)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "2.4":
            {
                var grd = (ScrollViewer)Form2_Visual.Form24_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm24)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "2.5":
            {
                var grd = (ScrollViewer)Form2_Visual.Form25_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm25)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "2.6":
            {
                var grd = (ScrollViewer)Form2_Visual.Form26_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm26)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "2.7":
            {
                var grd = (ScrollViewer)Form2_Visual.Form27_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm27)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "2.8":
            {
                var grd = (ScrollViewer)Form2_Visual.Form28_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm28)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "2.9":
            {
                var grd = (ScrollViewer)Form2_Visual.Form29_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm29)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "2.10":
            {
                var grd = (ScrollViewer)Form2_Visual.Form210_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm210)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "2.11":
            {
                var grd = (ScrollViewer)Form2_Visual.Form211_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm211)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
            case "2.12":
            {
                var grd = (ScrollViewer)Form2_Visual.Form212_Visual(this.FindNameScope());

                #region Rows Context Menu
                var Rgrd = (DataGridForm212)((StackPanel)grd.Content).Children[1];
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.A,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectAll",
                    ContextMenuText = new string[] { "Выделить все                    Ctrl+A" },
                    Command = null
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку              Ctrl+T" },
                    Command = dataContext.AddRow
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк            Ctrl+N" },
                    Command = dataContext.AddRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.I,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Добавить N строк перед Ctrl+I" },
                    Command = dataContext.AddRowsIn
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                      Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                            Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки                 Ctrl+D" },
                    Command = dataContext.DeleteRows
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.O,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Выставить номер п/п              Ctrl+O" },
                    Command = dataContext.SetNumberOrder
                });
                Rgrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                #region Notes Context Menu
                var Ngrd = (DataGridNote)((Panel)((StackPanel)grd.Content).Children[3]).Children[0];
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.T,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить строку          Ctrl+T" },
                    Command = dataContext.AddNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.N,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "",
                    ContextMenuText = new string[] { "Добавить N строк        Ctrl+N" },
                    Command = dataContext.AddNotes
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.C,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Copy",
                    ContextMenuText = new string[] { "Копировать                  Ctrl+C" },
                    Command = dataContext.CopyRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.V,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Paste",
                    ContextMenuText = new string[] { "Вставить                        Ctrl+V" },
                    Command = dataContext.PasteRows
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.D,
                    KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "SelectedItems",
                    ContextMenuText = new string[] { "Удалить строки             Ctrl+D" },
                    Command = dataContext.DeleteNote
                });
                Ngrd.CommandsList.Add(new KeyCommand
                {
                    Key = Avalonia.Input.Key.Delete,
                    IsDoubleTappedCommand = false,
                    IsContextMenuCommand = true,
                    ParamName = "Del",
                    ContextMenuText = new string[] { "Очистить ячейки              Delete" },
                    Command = dataContext.DeleteDataInRows
                });
                #endregion

                panel.Children.Add(grd);
                break;
            }
        }
    }

    private void Init()
    {
        var panel = this.FindControl<Panel>("ChangingPanel");
        Form1Init(panel);
        Form2Init(panel);
    }
    #endregion

    #region DoShowDialog
    private async Task DoShowDialogAsync(InteractionContext<int, int> interaction)
    {
        RowNumberIn frm = new(interaction.Input);
        await frm.ShowDialog(this);
        interaction.SetOutput(Convert.ToInt32(frm.Number2));
    }

    private async Task DoShowDialogAsync(InteractionContext<object, int> interaction)
    {
        RowNumber frm = new();
        await frm.ShowDialog(this);
        interaction.SetOutput(Convert.ToInt32(frm.Number));
    }

    private async Task DoShowDialogAsyncT(InteractionContext<List<string>, string> interaction)
    {
        MessageBox.Avalonia.DTO.MessageBoxCustomParams par = new()
        {
            ContentHeader = "Уведомление",
            ContentMessage = interaction.Input[0],
            ContentTitle = "Уведомление",
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        interaction.Input.RemoveAt(0);
        par.ButtonDefinitions = interaction.Input
            .Select(elem => new ButtonDefinition
            {
                Type = MessageBox.Avalonia.Enums.ButtonType.Default,
                Name = elem
            });
        
        var mssg = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(par);
        var answ = await mssg.ShowDialog(this);

        interaction.SetOutput(answ);
    }
    #endregion

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}