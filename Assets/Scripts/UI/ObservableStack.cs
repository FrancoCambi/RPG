using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void UpdateStackEvent();

public class ObservableStack<T> : Stack<T>
{

    public event UpdateStackEvent onPush;
    public event UpdateStackEvent onPop;
    public event UpdateStackEvent onClear;

    public ObservableStack(ObservableStack<T> items) : base(items)
    {

    }
    
    public ObservableStack()
    {

    }

    public new void Push(T item)
    {
        base.Push(item);

        if (onPush != null)
        {
            onPush();
        }
    }

    public new T Pop()
    {
        T item = base.Pop();

        if (onPop != null)
        {
            onPop();
        }

        return item;
    }

    public new void Clear()
    {
        base.Clear();

        if (onClear != null)
        {
            onClear();
        }
    }

 
}
