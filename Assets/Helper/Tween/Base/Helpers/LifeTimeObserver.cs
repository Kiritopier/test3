using System;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeObserver : MonoBehaviour
{
    List<Action> _Triggers = new List<Action>();
    public void AddObserver(Action Trigger)
    {
        _Triggers.Add(Trigger);
    }
    public void RemoveTrigger(Action Trigger)
    {
        _Triggers.Remove(Trigger);
    }

    void OnDisable()
    {
        DisposeAllTweens();
    }

    public void DisposeAllTweens()
    {
        int count = _Triggers.Count;
        for (int i = 0; i < count; i++)
        {
            _Triggers[i]();
        }
        _Triggers.Clear();
    }
}

