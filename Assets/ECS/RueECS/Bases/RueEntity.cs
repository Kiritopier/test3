using RueECS;
//using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RueECS
{
    public partial class BaseEntity
    {
        public static BaseEntity Get(int id) { return _Pool[id]; }
        public static BaseEntity GetByName(string name)
        {
            for (int i = 0; i < _Pool.Count; i++)
            {
                if(_Pool[i]._Name == name)
                {
                    return _Pool[i];
                }
            }
            return null;
        }
        public static List<BaseEntity> _Pool = new List<BaseEntity>();
        public static List<BaseEntity> _ToDestroy = new List<BaseEntity>();

        public static Dictionary<int, BaseEntity> _Orderly = new Dictionary<int, BaseEntity>();

        public static void KillAll()
        {
            for (int i = 0; i < _ToDestroy.Count; i++)
            {
               // Debug.Log("Destroying: " + _ToDestroy[i]._UniqueID);
                if(_ToDestroy[i]._IsInmortal)
                {
                    Debug.LogError("Attempting to destroy an inmortal entity");
                    continue;
                }
                _ToDestroy[i].InnerDestroy();
            }
            _ToDestroy.Clear();
        }

        int _Version;
        public readonly int _UniqueID;
        public string _Name = "Blank";
        protected bool _WasDestroyed;
        public bool _IsInmortal = false;
        public bool WasDestroyed {  get { return _WasDestroyed; } }
        List<Action> _OnDestroy;

        protected BaseEntity()
        {
            _WasDestroyed = false;
            _OnDestroy = new List<Action>(4);
            _UniqueID = _Pool.Count;
            _Pool.Add(this);
            _Version = 0;
            _Orderly.Add(_UniqueID, this);
        }

        /// <summary>
        /// Will only kill the entity if the version is the correct one
        /// </summary>
        /// <param name="Key"></param>
        public void Destroy(int Key)
        {
            if(Key != _Version) { return; }
            if (_IsInmortal) { return; }
            if (_WasDestroyed)
            {
                return;
            }
            _Name = "Blank";
            _WasDestroyed = true;
           // Debug.Log("Adding Destruction: " + _UniqueID);
            _ToDestroy.Add(this);
        }
        
        public void Destroy()
        {
            if (_IsInmortal) { return; }
            if(_WasDestroyed)
            {
                return;
            }
        
            _WasDestroyed = true;
            //Debug.Log("Adding Destruction: " + _UniqueID);
            _ToDestroy.Add(this);
        }

        protected virtual void InnerDestroy()
        {
            for (int i = _OnDestroy.Count-1; i >-1; i--)
            {
                _OnDestroy[i]();
               
            }
            _OnDestroy.Clear();
            _Version++;
        }

        public int GetVersion()
        {
            return _Version;
        }

        public void OverrideVersion(int Version)
        {
            _Version = Version;
        }
        /// <summary>
        /// Validates the entity a couple ways... by version and if it was destroyed.
        /// </summary>
        /// <param name="Version"></param>
        /// <returns></returns>
        public bool IsValidEntity(int Version)
        {
            if (WasDestroyed) { return false; }
            if(_Version != Version) { return false; }
            return true;
        }

        public BaseEntity AddOnDestroy(Action Callback)
        {
            if(Callback == null) {
                Debug.LogError("Callback null!!");
                return this; }
            _OnDestroy.Add(Callback);
            return this;
        }

        public BaseEntity RemoveOnDestroy(Action Callback)
        {
            _OnDestroy.Remove(Callback);
            return this;
        }

    }


    public static partial class StaticCollections
    {
        public static void SyncEntitiesComponents(int Parent, int Children)
        {
            InnerSyncEntitiesComponents(Parent, Children);
        }
        static partial void InnerSyncEntitiesComponents(int Parent, int Children);
    }

}

