using System;
using System.Collections.Generic;
using UnityEngine;



public class FuncComponent : MonoBehaviour
{
    public InternalUpdater _Head;

    void OnDisable()
    {
        InternalUpdater _Current = _Head;
        while(_Current != null)
        {
            _Current._ToRecycle = true;
            _Current._ComponentDisabled = true;
            _Current = _Current._CompNext;
        }
        _Head = null;
    }

    public bool ContainsRoutinesInIndex(int Layer)
    {
        InternalUpdater _Current = _Head;
        while (_Current != null)
        {
            if(_Current._Stream == Layer)
            {
                if(_Current._IsRoutine())
                {
                    return true;
                }
            }
            _Current = _Current._CompNext;
        }

        return false;
    }

    public int HowManyRoutinesIn(int Layer)
    {
        int Count = 0; 
        InternalUpdater _Current = _Head;
        while (_Current != null)
        {
            if (_Current._Stream == Layer)
            {
                if (_Current._IsRoutine())
                {
                    Count++;
                }
            }
            _Current = _Current._CompNext;
        }

        return Count;
    }

    public InternalUpdater ReturnFirstRoutineFound(int Layer)
    {
        InternalUpdater _Current = _Head;
        while (_Current != null)
        {
            if (_Current._Stream == Layer)
            {
                if (_Current._IsRoutine())
                {
                    return _Current;
                }
            }
            _Current = _Current._CompNext;
        }

        return null;
    }

    public void DisableAllRoutinesIn(int Layer)
    {
        InternalUpdater _Current = _Head;
        while (_Current != null)
        {
            if (_Current._Stream == Layer)
            {
                if (_Current._IsRoutine())
                {
                    _Current._ToRecycle = true;
                }
            }
            _Current = _Current._CompNext;
        }
    }

}

