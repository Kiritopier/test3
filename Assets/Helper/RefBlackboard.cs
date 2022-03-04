using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RefBlackboard : MonoBehaviour
{
    public List<RefBBComponent> _Holding = new List<RefBBComponent>();
    [HideInInspector]
    public string __TempInterface;

    public T Get<T>(int Index)where T: Component
    {
        return (T)_Holding[Index]._Component;
    }
    
    public T Get<T>(string ID) where T: Component
    {
        int count = _Holding.Count;
        for(int i  = 0; i < count; i++)
        {
            if(_Holding[i]._ID == ID)
            {
                try
                {
                    return (T)_Holding[i]._Component; 
                }
                catch
                {
                    Debug.LogError("Cant cast: " + typeof(T).ToString() + " to " + _Holding[i]._Component.GetType().ToString());
                }
            }
        }
        Debug.LogError("Couldnt find a component with the ID: " + ID);
        return null;
    }

    public void Set(string ID, Component C)
    {
        int count = _Holding.Count;
        for (int i = 0; i < count; i++)
        {
            if (_Holding[i]._ID == ID)
            {
                _Holding[i]._Component = C;
                return;
            }
        }
    }
    /// <summary>
    /// This gets a very unique ID from the RefBB to find equal refbbs to create interfaces between them.
    /// </summary>
    /// <returns></returns>
    public string GetEditorString()
    {
        string Add = "";
        for (int i = 0; i < _Holding.Count; i++)
        {
            Add += _Holding[i]._ID + "_" + _Holding[i]._Component.GetType().Name + "_";
        }
        return Add;
    }

}

[Serializable]
public class RefBBComponent
{
    public Component _Component;
    public string _ID;
}

public class RefBBLink
{
    public RefBlackboard _BB;
    public string _ID;
    
    public T Get<T>() where T: Component
    {
        return _BB.Get<T>(_ID);
    }

    public void Set(Component C)
    {
        _BB.Set(_ID, C);
    }
}

public static class GOExtension
{
    public static RefBlackboard GetBB(this GameObject Go)
    {
        return Go.GetComponentInChildren<RefBlackboard>(true);
    }
    public static T GetFromBB<T>(this GameObject Go, string id) where T:Component
    {
        return Go.GetBB().Get<T>(id);
    }
}

