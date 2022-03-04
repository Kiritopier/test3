using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;


public interface ICollectionDeclarator
{
    List<CollectionDeclarationForDeclarator> GetDeclaring();
}

public static class ICollectionDeclaratorExt
{
    public static List<string> Collection(this ICollectionDeclarator e, params IComponentDeclarator[] types)
    {
        var f = new List<string>();
        for (int i = 0; i < types.Length; i++)
        {
            f.Add(types[i].GetComponentName());
        }
        return f;
    }

    public static CollectionDeclarationForDeclarator Declare(this ICollectionDeclarator e, string CollectionName, List<string> With, List<string> Without = null)
    {
        CollectionDeclarationForDeclarator g = new CollectionDeclarationForDeclarator();
        g._With = new List<string>(With);
        if(Without == null)
        {
            g._Without = new List<string>();
        }
        else
        {
            g._Without = new List<string>(Without);
        }
        
        g._CollectionName = CollectionName;
        return g;
    }

    public static CollectionDeclarationForDeclarator Declare(this ICollectionDeclarator e, string CollectionName, IEntityDeclarator FitsThis)
    {
        CollectionDeclarationForDeclarator g = new CollectionDeclarationForDeclarator();
        g._With = new List<string>();
        g._Without = new List<string>();
        HashSet<string> F = new HashSet<string>();
        processdeclarator(FitsThis, F);
        g._With.AddRange(F);
        g._CollectionName = CollectionName;
        return g;
    }

    public static void processdeclarator(IEntityDeclarator d, HashSet<string> AllTypesFoundInEntity)
    {
        var Comps = d.ComponentsInEntity();
        if (Comps != null)
        {
            foreach (var ccc in Comps)
            {
                AllTypesFoundInEntity.Add(ccc.GetComponentName());
            }
        }

        var extradefs = d.IsAlso();

        if (extradefs != null)
        {
            for (int i = 0; i < extradefs.Count; i++)
            {
                processdeclarator(extradefs[i], AllTypesFoundInEntity);
            }
        }
    }

}

public class CollectionDeclarationForDeclarator
{
    public string _CollectionName;
    public List<string> _With = new List<string>();
    public List<string> _Without = new List<string>();
    public void Sort()
    {
        _With.Sort((x, y) =>
        {
            return x.CompareTo(y);
        });
        _Without.Sort((x, y) =>
        {
            return x.CompareTo(y);
        });
    }
}