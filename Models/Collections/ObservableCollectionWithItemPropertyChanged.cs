using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using OfficeOpenXml;
using System.Linq;
using System.Threading.Tasks;
using Models.Interfaces;

namespace Models.Collections;

public class ObservableCollectionWithItemPropertyChanged<T> : ObservableCollection<T>, IKey, IKeyCollection
    where T : class,IKey,INumberInOrder
{
    /// <summary>
    /// Occurs when a property is changed within an item.
    /// </summary>
    public event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;

    [NotMapped]
    public long Order => throw new NotImplementedException();

    public int Id { get; set; }

    public void SetOrder(long index) { }

    public ObservableCollectionWithItemPropertyChanged() { }

    public void CleanIds()
    {
        foreach (var item in Items)
        {
            item.Id = 0;
        }
    }

    public ObservableCollectionWithItemPropertyChanged(List<T> list) : base(list)
    {
        try
        {
            ObserveAll();
        }
        catch (Exception ex)
        {
            //ignored
        }
    }

    public ObservableCollectionWithItemPropertyChanged(IEnumerable<T> enumerable) : base(enumerable)
    {
        try
        {
            ObserveAll();
        }
        catch (Exception ex)
        {
            //ignored
        }
    }

    public bool Sorted { get; set; }
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is NotifyCollectionChangedAction.Remove or NotifyCollectionChangedAction.Replace && e.OldItems != null)
        {
            foreach (T item in e.OldItems)
            {
                item.PropertyChanged -= ChildPropertyChanged;
                Sorted = false;
            }
        }

        if (e.Action is NotifyCollectionChangedAction.Add or NotifyCollectionChangedAction.Replace && e.NewItems != null)
        {
            foreach (T item in e.NewItems)
            {
                if (item == null) continue;
                item.PropertyChanged += ChildPropertyChanged;
                Sorted = false;
            }
        }
        base.OnCollectionChanged(e);
    }

    //метод для обмена элементов массива
    private void Swap(int index1, int index2)
    {
        (Items[index1], Items[index2]) = (Items[index2], Items[index1]);
    }
    private int Partition(int minIndex, int maxIndex)
    {
        var pivot = minIndex - 1;
        for (var i = minIndex; i < maxIndex; i++)
        {
            if (Items[i].Order >= Items[maxIndex].Order) continue;
            pivot++;
            Swap(pivot, i);
        }
        pivot++;
        Swap(pivot, maxIndex);
        return pivot;
    }

    private void QuickSort(int minIndex, int maxIndex)
    {
        while (true)
        {
            if (minIndex >= maxIndex)
            {
                return;
            }
            var pivotIndex = Partition(minIndex, maxIndex);
            QuickSort(minIndex, pivotIndex - 1);
            minIndex = pivotIndex + 1;
        }
    }

    public void QuickSort()
    {
        if (Sorted) return;
        try
        {
            if (CheckForSort()) return;
            QuickSort(0, Items.Count - 1);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            Sorted = true;
        }
        catch
        {
            // ignored
        }
    }

    public async Task QuickSortAsync()
    {
        if (!Sorted)
        {
            try
            {
                if (!CheckForSort())
                {
                    QuickSort(0, Items.Count - 1);
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    Sorted = true;
                }
            }
            catch
            {
                // ignored
            }
        }
    }

    private bool CheckForSort()
    {
        var count = 1;
        var flag = true;
        foreach(var item in Items)
        {
            if(item.Order != count)
            {
                flag = false;
                break;
            }
            count++;
        }
        return flag;
    }

    private void OnItemPropertyChanged(ItemPropertyChangedEventArgs e)
    {
        ItemPropertyChanged?.Invoke(this, e);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    private void OnItemPropertyChanged(int index, PropertyChangedEventArgs e)
    {
        OnItemPropertyChanged(new ItemPropertyChangedEventArgs(index, e));
    }

    protected override void ClearItems()
    {
        foreach (var item in Items)
        {
            item.PropertyChanged -= ChildPropertyChanged;
        }
        base.ClearItems();
    }

    protected override void RemoveItem(int index)
    {
        this[index].PropertyChanged -= ChildPropertyChanged;
        base.RemoveItem(index);
    }

    private void ObserveAll()
    {
        foreach (var item in Items)
        {
            item.PropertyChanged += ChildPropertyChanged;
        }
        // QuickSort();
    }

    public void AddRange(IEnumerable<T> items)
    {
        var itemsList = items.ToList();
        foreach (var item in itemsList)
        {
            Items.Add(item);
        }
        Sorted = false;
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, itemsList));
    }

    public void AddRangeNoChange(IEnumerable<T> items)
    {
        var itemsList = items.ToList();
        foreach (var item in itemsList)
        {
            item.PropertyChanged += ChildPropertyChanged;
            Items.Add(item);
        }
    }

    private void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        var typedSender = (T) sender;
        var i = Items.IndexOf(typedSender);
        if (i < 0)
        {
            throw new ArgumentException("Received property notification from item not in collection");
        }
        OnItemPropertyChanged(i, e);
    }

    public void Add<T1>(T1 obj) where T1 : class, IKey
    {
        base.Add(obj as T);
    }

    public void Remove<T1>(T1 obj) where T1 : class, IKey
    {
        base.Remove(obj as T);
    }

    public void RemoveAt<T1>(int obj) where T1 : class, IKey
    {
        RemoveAt(obj);
    }

    public void AddRange<T1>(IEnumerable<T1> obj) where T1 : class, IKey
    {
        AddRange(obj.Cast<T>());
    }

    public void AddRange<T1>(int index, IEnumerable<T1> obj) where T1 : class, IKey
    {
        var count = index;
        var objList = obj.ToList();
        var countObj = objList.Count;
        long minOrder = 0;
        try
        {
            minOrder = objList.Min(x => x.Order);
        }
        catch (Exception)
        {
            // ignored
        }
        var lst = new List<T>(Items);
        foreach (var item in objList)
        {
            item.PropertyChanged += ChildPropertyChanged;
            Items.Insert(count, item as T);
            count++;
        }
        var itemQ = lst.Where(x => x.Order >= minOrder);
        foreach (var it in itemQ)
        {
            it.SetOrder(it.Order + countObj);
        }
        Sorted = false;
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public T1 Get<T1>(int index) where T1 : class, IKey
    {
        return this[index] as T1;
    }

    public void Clear<T1>() where T1 : class, IKey
    {
        Clear();
    }

    public List<T1> ToList<T1>() where T1 : class, IKey
    {
        return Items.Select(item => item as T1).ToList();
    }

    public IEnumerable<IKey> GetEnumerable()
    {
        return this;
    }

    public IEnumerator<IKey> GetEnumerator()
    {
        var lst = Items.ToList();
        foreach(var item in lst)
        {
            yield return item;
        }
    }
    public int Count => base.Count;

    #region IExcel
    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        throw new NotImplementedException();
    }
    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose=true, string sumNumber = "")
    {
        throw new NotImplementedException();
    }

    public int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true)
    {
        throw new NotImplementedException();
    }
    #endregion
}