using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class OdinCollectionDeclareWindow
{
    public OdinCollectionDeclareWindow(AllDataForECS data)
    {
        _AllDeclaredColls = new List<DeclaringCollection>(data._AllCollections);
        for (int i = 0; i < _AllDeclaredColls.Count; i++)
        {
            _AllDeclaredColls[i]._EntitiesPresent = new List<string>();
        }
        var allent = data._AllEntities;
        for (int i = 0; i < _AllDeclaredColls.Count; i++)
        {
            for (int a = 0; a < allent.Count; a++)
            {
                if(_AllDeclaredColls[i].DoesEntityBelong(allent[a]))
                {
                    _AllDeclaredColls[i]._EntitiesPresent.Add(allent[a]._EntityName);
                }
            }
        }
    }

    [ShowInInspector]
    //[TableList(ShowPaging = false, CellPadding = 20, AlwaysExpanded =true, NumberOfItemsPerPage =5)]
    public List<DeclaringCollection> _AllDeclaredColls;
}