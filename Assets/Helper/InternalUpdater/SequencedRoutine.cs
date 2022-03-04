using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SequencedRoutine : MonoBehaviour
{
    public Dictionary<int, Queue<IEnumerator>> _Queues;// = new Dictionary<int, Queue<IEnumerator>>();//= new Queue<IEnumerator>();

    private void Awake()
    {
        _Queues = new Dictionary<int, Queue<IEnumerator>>();
    }

    public void AddRoutineToThis(IEnumerator A, int Index)
    {
        if(_Queues.ContainsKey(Index))
        {
            _Queues[Index].Enqueue(A);
        }
        else
        {
            this.AddRoutine(Routiner(Index));
            Queue<IEnumerator> _Collection = new Queue<IEnumerator>();
            _Collection.Enqueue(A);
            _Queues.Add(Index, _Collection);
        }
    }

    IEnumerator Routiner(int index)
    {
        while(true)
        {
            if (_Queues[index].Count > 0)
            {
                IEnumerator Current = _Queues[index].Dequeue();
                while (Current.MoveNext())
                {
                    yield return Current.Current;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    void OnDisable()
    {
        _Queues.Clear();
    }
}

