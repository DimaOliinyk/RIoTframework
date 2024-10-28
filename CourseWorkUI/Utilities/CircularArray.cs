using System.Collections;
using System.Numerics;

namespace CourseWorkUI.Utilities;

/// <summary>
/// Container where elements are added in 
/// cycle and read starting from recently
/// added
/// </summary>
/// <typeparam name="T"></typeparam>
public class CircularArray<T> 
    : ICloneable, IEnumerable where T 
        : INumber<T>
{
    private T[] _array;         // Linear array
    private int _indexSlider;   // Shifted starting index

    public CircularArray(int size) 
    {
        if (size <= 0) 
        {
            throw new ArgumentOutOfRangeException("size must be greater than 0");
        }
        _indexSlider = 0;       // By default starting index is 0
        _array = new T[size];   
    }

    /// <summary>
    /// Adds new value by replacing the elemnet 
    /// that was added the earliest
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CircularArray<T> AddValue(T value) 
    {
        // Computes the index by obeying the rules of circular arrays
        _indexSlider %= _array.Length;      
        _array[_indexSlider++] = value;     // Adds value and shifts the original position  qw
        return this;
    }

    /// <summary>
    /// Clones the CircularArray
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
        var arr = new CircularArray<T>(_array.Length);
        arr._indexSlider = _indexSlider;
        
        int i = 0;
        foreach (var item in _array)
        {
            arr._array[i] = item;
        }
        return arr;
    }

    public IEnumerator GetEnumerator()
    {
        return new CircularArrEnum<T>(this, _indexSlider);
    }

    public T this[int index] 
    {
        get => _array[(index += _indexSlider) % _array.Length];
        set => _array[(index += _indexSlider) % _array.Length] = value;
    }

    /// <summary>
    /// Returns count of elements
    /// </summary>
    public int Count => _array.Length;
}


public class CircularArrEnum<T> 
    : IEnumerator where T 
        : INumber<T>
{
    private int _position;
    private readonly CircularArray<T> _array;
    public CircularArrEnum(CircularArray<T> arr, int startIndex)
    {
        _position = -1;     // Position counter
        _array = arr;
    }

    public object Current => _array[_position]!;

    public bool MoveNext() => _position++ < _array.Count - 1;

    public void Reset()
    {
    }
}