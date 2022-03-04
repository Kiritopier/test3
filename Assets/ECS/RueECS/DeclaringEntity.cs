using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public partial class DeclaringEntity
{
    [VerticalGroup("IsAlso")]
    public string _EntityName;
    [HideInInspector]
    public List<string> _IsAlso = new List<string>();
    [HideInInspector]
    public List<string> _InsideCollections = new List<string>();

    [HideInInspector]
    public List<string> _Components = new List<string>();
    public string GetEntitySignatgure()
    {
        _Components.Sort((x, y) =>
        {
            return x.CompareTo(y);
        });
        string signature = _Components.Count.ToString();
        _Components.ForEach((x) =>
        {
            signature += x;
        });
        return signature;
    }
    public bool MatchesWithThis(DeclaringEntity Other)
    {
        bool DoesMatch = true;
        for (int i = 0; i < Other._Components.Count; i++)
        {
            if (!_Components.Contains(Other._Components[i]))
            {
                DoesMatch = false;
                break;
            }
        }
        return DoesMatch;
    }

    public void IsAlsoThen(DeclaringEntity e)
    {
        _IsAlso.Add(e._EntityName);
    }
}