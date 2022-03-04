using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class OdinEntityDeclareWindow
{
    public OdinEntityDeclareWindow(AllDataForECS data)
    {
        _AllDeclared = new List<DeclaringEntity>(data._AllEntities);
        _AllDeclared.ForEach((xc) =>
        {
            xc._IsAlso = new List<string>();
        });
        _AllDeclared.Sort((x, y) =>
        {
            return x._Components.Count.CompareTo(y._Components.Count);
        });
        for (int i = 0; i < _AllDeclared.Count-1; i++)
        {
            DeclaringEntity c = _AllDeclared[i];
            for (int a = i+1; a < _AllDeclared.Count; a++)
            {
                DeclaringEntity v = _AllDeclared[a];
                if(c.MatchesWithThis(v))
                {
                    c.IsAlsoThen(v);
                }
                else if(v.MatchesWithThis(c))
                {
                    v.IsAlsoThen(c);
                }
            }
        }

        var allcol = data._AllCollections;
        for (int i = 0; i < _AllDeclared.Count; i++)
        {
            DeclaringEntity c = _AllDeclared[i];
            c._InsideCollections = new List<string>();
            for (int a = 0; a < allcol.Count; a++)
            {
                var curr = allcol[a];
                if(curr.DoesEntityBelong(c))
                {
                    c._InsideCollections.Add(curr._CollectionName);
                }
            }
        }

    }

    [ShowInInspector]
    [TableList(CellPadding =20)]
    public List<DeclaringEntity> _AllDeclared;
}
