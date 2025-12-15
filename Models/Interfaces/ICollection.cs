using System.Collections;
using System.Collections.Generic;

namespace Models.Interfaces;

public interface IKeyCollection : IEnumerable
{
    void Add<T1>(T1 obj) where T1 : class, IKey;
    void Remove<T1>(T1 obj) where T1 : class, IKey;
    void RemoveAt<T1>(int obj) where T1 : class, IKey;
    void AddRange<T1>(IEnumerable<T1> obj) where T1 : class, IKey;
    void AddRange<T1>(int index,IEnumerable<T1> obj) where T1 : class, IKey;
    T1 Get<T1>(int index) where T1 : class, IKey;
    List<T1> ToList<T1>() where T1 : class, IKey;

    void Clear<T1>() where T1 : class, IKey;
    IEnumerator<IKey> GetEnumerator();
    IEnumerable<IKey> GetEnumerable();
    int Count { get; }
}