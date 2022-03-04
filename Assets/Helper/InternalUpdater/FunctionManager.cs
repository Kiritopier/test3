using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

[AddComponentMenu("")]
public class InternalUpdaterManager : MonoBehaviour
{
    public static bool IsPaused = false;
    private static InternalUpdaterManager Manager;
    private static bool _Lock = false;
    public static void Init()
    {
        if (!_Lock)
        {
            if (Manager == null)
            {
                Manager = new GameObject("Internal Updater Manager").AddComponent<InternalUpdaterManager>();
                GameObject.DontDestroyOnLoad(Manager.gameObject);
                _Lock = true;
            }
        }
    }

    void Awake()
    {
        RoutineTimer.LastRealTime = Time.realtimeSinceStartup;
        Stacktraces.Clear();
    }

    public static List<string> Stacktraces = new List<string>();
    void Update()
    {
        float D = Time.deltaTime;
        RoutineTimer.DeltaTime = D;
        RoutineTimer.RealTimeSinceStartUp = Time.realtimeSinceStartup;
        RoutineTimer.RealDeltaTime = Time.realtimeSinceStartup - RoutineTimer.LastRealTime;
        RoutineTimer.LastRealTime = Time.realtimeSinceStartup;
        int i = 0;
        while (InternalUpdater.UpdateAll(D, i))
        {
            i++;
        }
    }
    void FixedUpdate()
    {
        InternalUpdater.FixedUpdate(Time.fixedDeltaTime);
    }
    void LateUpdate()
    {
        InternalUpdater.LateUpdate(Time.deltaTime);
    }
    void OnDestroy()
    {
        _Lock = false;
        Manager = null;
        InternalUpdater.DestroyAll();
    }

}

