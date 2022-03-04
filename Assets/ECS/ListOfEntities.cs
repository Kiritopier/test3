using RueECS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfEntities
{
    public bool _IsValid = true;
    public List<BaseEntity> _Entities = new List<BaseEntity>();
    public List<int> _Versioning = new List<int>();
    public Action<BaseEntity> _OnRemovedHandle;
    public int Count {  get { return _Entities.Count; } }

    public ListOfEntities()
    {
       
    }

    public void Add(BaseEntity Entity)
    {
        int version = Entity.GetVersion();
        Entity.AddOnDestroy(() => {
            if (!this._IsValid) { return; }
            if (version == Entity.GetVersion())
            {
                if(_OnRemovedHandle!=null)
                {
                    _OnRemovedHandle(Entity);
                }
                _Entities.Remove(Entity);
            }
        });
        _Entities.Add(Entity);
        _Versioning.Add(version);
    }

    public void Remove(BaseEntity Entity)
    {
        int index = _Entities.IndexOf(Entity);
        _Versioning.RemoveAt(index);
        _Entities.RemoveAt(index);
    }

    public BaseEntity Get(int index)
    {
        if(index >= _Entities.Count) { return null; }
       
        return _Entities[index];
    }

    public void CleanUp()
    {
        for (int i = 0; i < _Entities.Count; i++)
        {
            if (_Versioning[i] != _Entities[i].GetVersion())
            {
                Debug.Log("Cleaning up entity: " + _Entities[i]._UniqueID);
                _Entities.RemoveAt(i);
                _Versioning.RemoveAt(i);
                i--;
            }
        }
    }

    public bool Contains(BaseEntity Entity)
    {
        return _Entities.Contains(Entity);
    }

    public void Clear()
    {
        _Entities.Clear();
    }
}