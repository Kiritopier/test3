using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using RueECS;

public interface IEntityDeclarator
{
    /// <summary>
    /// Archetypes go here
    /// </summary>
    /// <returns></returns>
     List<IEntityDeclarator> IsAlso();
    /// <summary>
    /// loose components go here
    /// </summary>
    /// <returns></returns>
     List<IComponentDeclarator> ComponentsInEntity();
     string GetEntityName();
}