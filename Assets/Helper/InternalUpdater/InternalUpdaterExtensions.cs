using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public static class InternalUpdaterExtensions
{
    /// <summary>
    /// Adds an update function to this mono behaviour
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="Function"> The function to execute when calling update, this function takes a float that represents the real elapsed time since the last update</param>
    /// <param name="Stream"> The order in which this update function will be called, the higher the number the higher the priority (0 will be called after 1, and on)</param>
    /// <param name="FrameRate"> The rate this function will be called (if it is 1, it will be called every frame, if it is 0.5, it will be called every 2 frames</param>
    public static InternalUpdater AddUpdate(this MonoBehaviour Target, Action<float> Function, int Stream = 0, float FrameRate = 1.0f)
    {
        InternalUpdaterManager.Init();
        return InternalUpdater.Create(Target.gameObject, Function, Stream, FrameRate);
    }

    public static void AddFixedUpdate(this MonoBehaviour Target, Action<float> Function, int Stream = 0, float FrameRate = 1.0f)
    {
        InternalUpdaterManager.Init();
        InternalUpdater.CreateF(Target.gameObject, Function, Stream, FrameRate);
    }

    public static InternalUpdater AddRoutine(this MonoBehaviour Target, IEnumerator Function, int Stream = 0, float FrameRate = 1.0f)
    {
        InternalUpdaterManager.Init();
        InternalUpdater newele = InternalUpdater.Create(Target.gameObject, Function, Stream, FrameRate);
        return newele;
    }

    public static InternalUpdater AddRoutineFixed(this MonoBehaviour Target, IEnumerator Function, int Stream = 0, float FrameRate = 1.0f)
    {
        InternalUpdaterManager.Init();
        InternalUpdater newele = InternalUpdater.CreateFixed(Target.gameObject, Function, Stream, FrameRate);
        return newele;
    }

    public static InternalUpdater AddLateUpdate(this MonoBehaviour Target, Action<float> Function, int Stream = 0, float FrameRate = 1.0f)
    {
        InternalUpdaterManager.Init();
        return InternalUpdater.CreateL(Target.gameObject, Function, Stream, FrameRate);
    }

    public static bool ContainsSequencesAt(this MonoBehaviour Target, int Index)
    {
        InternalUpdaterManager.Init();
        FuncComponent R = Target.GetComponent<FuncComponent>();
        if (!R)
        {
            return false;
        }
        return R.ContainsRoutinesInIndex(Index);
    }

    public static int NumberOfRoutinesIn(this MonoBehaviour Target, int Index)
    {
        InternalUpdaterManager.Init();
        FuncComponent R = Target.GetComponent<FuncComponent>();
        if (!R)
        {
            return 0;
        }
        return R.HowManyRoutinesIn(Index);
    }

    public static void DisableAllRoutines(this MonoBehaviour Target, int Index)
    {
        InternalUpdaterManager.Init();
        FuncComponent R = Target.GetComponent<FuncComponent>();
        if (!R)
        {
            return;
        }
        R.DisableAllRoutinesIn(Index);
    }


    public static void AddSequencedRoutine(this MonoBehaviour Target, IEnumerator Function, int Stream = 0, float FrameRate = 1.0f)
    {
        InternalUpdaterManager.Init();
        SequencedRoutine R = Target.GetComponent<SequencedRoutine>();
        if (!R)
        {
            R = Target.gameObject.AddComponent<SequencedRoutine>();
        }
        R.AddRoutineToThis(Function, Stream);//._Queue.Enqueue(Function);
    }
    
    public static bool HasSequencedRoutines(this MonoBehaviour Target, int Index)
    {
        InternalUpdaterManager.Init();
        SequencedRoutine R = Target.GetComponent<SequencedRoutine>();
        if (!R)
        {
            return false;
        }
        Queue<IEnumerator> N = null;
        if(!R._Queues.TryGetValue(Index, out N))
        {
            return false;
        }

        return N.Count > 0;
    }
}

