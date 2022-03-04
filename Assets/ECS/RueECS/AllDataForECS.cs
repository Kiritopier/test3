using System;
using System.Collections.Generic;
using System.Linq;

public class AllDataForECS
{
    public Dictionary<string, DeclaringCollection> RepeatedCollections;
    public List<DeclaringComponent> _AllDeclaredComponents;
    public List<DeclaringEntity> _AllEntities;
    public List<DeclaringCollection> _AllCollections;
    public List<(Type, IsGameSystemAttribute)> _SystemsDeclaredWithTypes;
}