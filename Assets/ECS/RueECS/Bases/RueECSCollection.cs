using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using RueECS;

namespace RueECS
{
    public static partial class StaticCollections
    {
        public static bool _Probed = false;
        public static void Probe() { _Probed = true; }
    }
}

//[Serializable]
//public class RueECSCollection
//{
//    public RueECSCollection()
//    {
//
//    }
//    public RueECSCollection(params Type[] Data)
//    {
//        _IsDirty = true;
//    }
//    protected bool _IsDirty = true;
//    //[ShowInInspector]
//    protected HashSet<BaseEntity> _Collection = new HashSet<BaseEntity>();
//    //[ShowInInspector]
//    protected List<IRueEntity> _InnerList = new List<IRueEntity>();
//
//
//   // [ShowInInspector]
//    List<Action<IRueEntity>> _OnAdded = new List<Action<IRueEntity>>(16);
//   // [ShowInInspector]
//    List<Action<IRueEntity>> _OnRemoved = new List<Action<IRueEntity>>(16);
//
//    public void EntityGetsAdded(BaseEntity Adding)
//    {
//        _IsDirty = true;
//        _Collection.Add(Adding);
//        for (int i = 0; i < _OnAdded.Count; i++) { _OnAdded[i](Adding); }
//    }
//    public void EntityGetsRemoved(BaseEntity Removing)
//    {
//        _IsDirty = true;
//        _Collection.Remove(Removing);
//        for (int i = 0; i < _OnRemoved.Count; i++)
//        {
//            _OnRemoved[i](Removing);
//        }
//    }
//    public void GetEntities(List<IRueEntity> Buffer)
//    {
//        Buffer.Clear();
//        Buffer.AddRange(_Collection);
//    }
//    public List<IRueEntity> GetEntitiesDirect() { if (_IsDirty) { _InnerList.Clear(); _InnerList.AddRange(_Collection); _IsDirty = false; } return _InnerList; }
//    public RueECSCollection OnAdded(Action<IRueEntity> Added)
//    {
//        _OnAdded.Add(Added);
//        return this;
//    }
//    public RueECSCollection OnRemoved(Action<IRueEntity> Removed)
//    {
//        _OnRemoved.Add(Removed);
//        return this;
//    }
//}