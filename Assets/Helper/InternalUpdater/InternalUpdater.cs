using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class RoutineTimer
{
    public static float DeltaTime = 0.0f;
    public static float RealDeltaTime = 0.0f;
    public static float LastRealTime = 0.0f;
    public static float RealTimeSinceStartUp = 0.0f;
}

public class InternalUpdater
{
    public static List<InternalUpdater> _AllStreams = new List<InternalUpdater>();
    public static List<InternalUpdater> _AllLateStreams = new List<InternalUpdater>();
    public static List<InternalUpdater> _AllFStreams = new List<InternalUpdater>();
    public static List<InternalUpdater> _AllEnumeratorStreams = new List<InternalUpdater>();
    public static List<InternalUpdater> _AllFixedEnumeratorStreams = new List<InternalUpdater>();

    private InternalUpdater _Next; //for update
    private InternalUpdater _Previous; //for update

    public InternalUpdater _CompPrevious; //for internal component
    public InternalUpdater _CompNext;
    public bool _ComponentDisabled;

    private Action<float> _Function;
    private IEnumerator _Enumerator;
    private InternalUpdater _YieldingFor;
    public bool _IsRoutine() { return _Enumerator != null; }
    public bool _IsThisRoutine(IEnumerator E)
    {
        return E == _Enumerator;
    }
    public ElapsedTimer _InternalElapsedTime;

    private static InternalUpdater _Head;
    private InternalUpdater _NextElement;
    public bool _ToRecycle;
    public FuncComponent _Holder;
    public int _Stream;
    private float _FrameRate;
    private bool _IsFixed;
    private bool _IsLate;

    private float _RealElapsedTime;
    private float _FrameQuota;

    private InternalUpdater()
    {
        _ToRecycle = false;
        _InternalElapsedTime = new ElapsedTimer();
    }

    public static InternalUpdater Create(Action<float> function, int Stream, float Framerate)
    {
        InternalUpdater Vessel = null;
        if (_Head != null) { Vessel = _Head; _Head = _Head._NextElement; }
        else { Vessel = new InternalUpdater(); }
        Vessel._Function = function;
        Vessel._Stream = Stream;
        Vessel._FrameRate = Framerate;
        Vessel._RealElapsedTime = 0.0f;
        Vessel._FrameQuota = 0.0f;
        Vessel._IsFixed = false;
        Vessel._IsLate = false;
        if (_AllStreams.Count <= Stream)
        {
            for (int i = _AllStreams.Count; i < Stream + 1; i++)
            {
                _AllStreams.Add(null);
            }
        }
        if (_AllStreams[Stream] != null)
        {
            _AllStreams[Stream]._Previous = Vessel;
            Vessel._Next = _AllStreams[Stream];
        }
        _AllStreams[Stream] = Vessel;
        Vessel._ComponentDisabled = true; //componentless updating element
        return Vessel;
    }

    public static InternalUpdater Create(GameObject target, Action<float> function, int Stream, float Framerate)
    {
        InternalUpdater Vessel = null;
        if (_Head != null) { Vessel = _Head; _Head = _Head._NextElement; }
        else { Vessel = new InternalUpdater(); }
        Vessel._Function = function;
        Vessel._Stream = Stream;
        Vessel._FrameRate = Framerate;
        Vessel._RealElapsedTime = 0.0f;
        Vessel._FrameQuota = 0.0f;
        Vessel._IsFixed = false;
        Vessel._IsLate = false;
        if (_AllStreams.Count <= Stream)
        {
            for (int i = _AllStreams.Count; i < Stream + 1; i++)
            {
                _AllStreams.Add(null);
            }
        }
        if (_AllStreams[Stream] != null)
        {
            _AllStreams[Stream]._Previous = Vessel;
            Vessel._Next = _AllStreams[Stream];
        }
        _AllStreams[Stream] = Vessel;
        Vessel._ComponentDisabled = false;
        FuncComponent Comp = target.GetComponent<FuncComponent>();
        if (Comp == null)
        {
            Comp = target.AddComponent<FuncComponent>();
            Comp.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }
        Vessel._CompNext = Comp._Head;
        if (Comp._Head != null)
        {
            Comp._Head._CompPrevious = Vessel;
        }
        Comp._Head = Vessel;
        Vessel._Holder = Comp;
        return Vessel;
    }

    public static InternalUpdater CreateF(GameObject target, Action<float> function, int Stream, float Framerate)
    {
        InternalUpdater Vessel = null;
        if (_Head != null) { Vessel = _Head; _Head = _Head._NextElement; }
        else { Vessel = new InternalUpdater(); }
        Vessel._Function = function;
        Vessel._Stream = Stream;
        Vessel._FrameRate = Framerate;
        Vessel._RealElapsedTime = 0.0f;
        Vessel._FrameQuota = 0.0f;
        Vessel._IsFixed = true;
        Vessel._IsLate = false;
        if (_AllFStreams.Count <= Stream)
        {
            for (int i = _AllFStreams.Count; i < Stream + 1; i++)
            {
                _AllFStreams.Add(null);
            }
        }
        if (_AllFStreams[Stream] != null)
        {
            _AllFStreams[Stream]._Previous = Vessel;
            Vessel._Next = _AllFStreams[Stream];
        }
        _AllFStreams[Stream] = Vessel;
        Vessel._ComponentDisabled = false;
        FuncComponent Comp = target.GetComponent<FuncComponent>();
        if (Comp == null)
        {
            Comp = target.AddComponent<FuncComponent>();
            Comp.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }
        Vessel._CompNext = Comp._Head;
        if (Comp._Head != null)
        {
            Comp._Head._CompPrevious = Vessel;
        }
        Comp._Head = Vessel;
        Vessel._Holder = Comp;
        return Vessel;
    }

    public static InternalUpdater CreateL(GameObject target, Action<float> function, int Stream, float Framerate)
    {
        InternalUpdater Vessel = null;
        if (_Head != null) { Vessel = _Head; _Head = _Head._NextElement; }
        else { Vessel = new InternalUpdater(); }
        Vessel._Function = function;
        Vessel._Stream = Stream;
        Vessel._FrameRate = Framerate;
        Vessel._RealElapsedTime = 0.0f;
        Vessel._FrameQuota = 0.0f;
        Vessel._IsFixed = false;
        Vessel._IsLate = true;
        if (_AllLateStreams.Count <= Stream)
        {
            for (int i = _AllLateStreams.Count; i < Stream + 1; i++)
            {
                _AllLateStreams.Add(null);
            }
        }
        if (_AllLateStreams[Stream] != null)
        {
            _AllLateStreams[Stream]._Previous = Vessel;
            Vessel._Next = _AllLateStreams[Stream];
        }
        _AllLateStreams[Stream] = Vessel;
        Vessel._ComponentDisabled = false;
        FuncComponent Comp = target.GetComponent<FuncComponent>();
        if (Comp == null)
        {
            Comp = target.AddComponent<FuncComponent>();
            Comp.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }
        Vessel._CompNext = Comp._Head;
        if (Comp._Head != null)
        {
            Comp._Head._CompPrevious = Vessel;
        }
        Comp._Head = Vessel;
        Vessel._Holder = Comp;
        return Vessel;
    }

    public static InternalUpdater Create(GameObject target, IEnumerator function, int Stream, float Framerate)
    {
        InternalUpdater Vessel = null;
        if (_Head != null) { Vessel = _Head; _Head = _Head._NextElement; }
        else { Vessel = new InternalUpdater(); }
        Vessel._Enumerator = function;
        Vessel._Stream = Stream;
        Vessel._FrameRate = Framerate;
        Vessel._RealElapsedTime = 0.0f;
        Vessel._FrameQuota = 0.0f;
        Vessel._IsFixed = false;
        Vessel._IsLate = false;
        if (_AllEnumeratorStreams.Count <= Stream)
        {
            for (int i = _AllEnumeratorStreams.Count; i < Stream + 1; i++)
            {
                _AllEnumeratorStreams.Add(null);
            }
        }
        if (_AllEnumeratorStreams[Stream] != null)
        {
            _AllEnumeratorStreams[Stream]._Previous = Vessel;
            Vessel._Next = _AllEnumeratorStreams[Stream];
        }
        _AllEnumeratorStreams[Stream] = Vessel;
        Vessel._ComponentDisabled = false;
        FuncComponent Comp = target.GetComponent<FuncComponent>();
        if (Comp == null)
        {
            Comp = target.AddComponent<FuncComponent>();
            Comp.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }
        Vessel._CompNext = Comp._Head;
        if (Comp._Head != null)
        {
            Comp._Head._CompPrevious = Vessel;
        }
        Comp._Head = Vessel;
        Vessel._Holder = Comp;
        return Vessel;
    }
    public static InternalUpdater CreateFixed(GameObject target, IEnumerator function, int Stream, float Framerate)
    {
        InternalUpdater Vessel = null;
        if (_Head != null) { Vessel = _Head; _Head = _Head._NextElement; }
        else { Vessel = new InternalUpdater(); }
        Vessel._Enumerator = function;
        Vessel._Stream = Stream;
        Vessel._FrameRate = Framerate;
        Vessel._RealElapsedTime = 0.0f;
        Vessel._FrameQuota = 0.0f;
        Vessel._IsFixed = true;
        Vessel._IsLate = false;
        if (_AllFixedEnumeratorStreams.Count <= Stream)
        {
            for (int i = _AllFixedEnumeratorStreams.Count; i < Stream + 1; i++)
            {
                _AllFixedEnumeratorStreams.Add(null);
            }
        }
        if (_AllFixedEnumeratorStreams[Stream] != null)
        {
            _AllFixedEnumeratorStreams[Stream]._Previous = Vessel;
            Vessel._Next = _AllFixedEnumeratorStreams[Stream];
        }
        _AllFixedEnumeratorStreams[Stream] = Vessel;
        Vessel._ComponentDisabled = false;
        FuncComponent Comp = target.GetComponent<FuncComponent>();
        if (Comp == null)
        {
            Comp = target.AddComponent<FuncComponent>();
            Comp.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }
        Vessel._CompNext = Comp._Head;
        if (Comp._Head != null)
        {
            Comp._Head._CompPrevious = Vessel;
        }
        Comp._Head = Vessel;
        Vessel._Holder = Comp;
        return Vessel;
    }

    public static void LateUpdate(float DeltaTime)
    {
        int count = _AllLateStreams.Count;
        for (int i = 0; i < count; i++)
        {
            InternalUpdater Current = _AllLateStreams[i];
            while (Current != null)
            {
                InternalUpdater Next = Current._Next;
                if (Current._ToRecycle)
                {
                    Current.Recycle();
                }
                else
                {
                    Current._RealElapsedTime += DeltaTime;
                    Current._FrameQuota += Current._FrameRate;
                    while (Current._FrameQuota >= 1.0f)
                    {
                        Current._Function(Current._RealElapsedTime);
                        Current._RealElapsedTime = 0.0f;
                        Current._FrameQuota -= 1.0f;
                    }
                }
                Current = Next;
            }
        }
    }

    public static void FixedUpdate(float DeltaTime)
    {
        int count = _AllFStreams.Count;
        for (int i = 0; i < count; i++)
        {
            InternalUpdater Current = _AllFStreams[i];
            while (Current != null)
            {
                InternalUpdater Next = Current._Next;
                if (Current._ToRecycle)
                {
                    Current.Recycle();
                }
                else
                {
                    Current._RealElapsedTime += DeltaTime;
                    Current._FrameQuota += Current._FrameRate;
                    while (Current._FrameQuota >= 1.0f)
                    {
                        Current._Function(Current._RealElapsedTime);
                        Current._RealElapsedTime = 0.0f;
                        Current._FrameQuota -= 1.0f;
                    }
                }
                Current = Next;
            }
        }
        for (int i = 0; i < _AllFixedEnumeratorStreams.Count; i++)
        {
            InternalUpdater Current = _AllFixedEnumeratorStreams[i];
            while (Current != null)
            {
                InternalUpdater Next = Current._Next;
                if (Current._ToRecycle)
                {
                    Current.Recycle();
                }
                else
                {
                    if (Current._YieldingFor != null)
                    {
                        if (!Current._YieldingFor._ToRecycle)
                        {
                            Current = Next;
                            continue; //just yield.
                        }
                    }
                    Current._YieldingFor = null;
                    Current._InternalElapsedTime._DeltaTime += DeltaTime;
                    Current._FrameQuota += Current._FrameRate;
                    while (Current._FrameQuota >= 1.0f && Current._YieldingFor == null)
                    {
                        if (!Current._Enumerator.MoveNext())
                        {
                            Current._ToRecycle = true;
                        }
                        Current._YieldingFor = (InternalUpdater)Current._Enumerator.Current;
                        if (Current._YieldingFor != null)
                        {
                            if (Current._YieldingFor._Stream < Current._Stream)
                            {
                                Debug.LogError("CANNOT HAVE A YIELDING ROUTINE WITH LOWER PRIORITY THAN ROUTINE YIELDING FOR");
                                Current._YieldingFor = null;
                            }
                        }
                        //Current._Instruction = (FYield)Current._Enumerator.Current;
                        Current._InternalElapsedTime._DeltaTime = 0.0f;
                        Current._FrameQuota -= 1.0f;
                    }
                }
                Current = Next;
            }
        }
    }

    public static bool UpdateAll(float DeltaTime, int Index)
    {
        int EnumCount = _AllEnumeratorStreams.Count;
        int NormalUpdateCount = _AllStreams.Count;
        bool SomethingLeft = false;
        #region normal updates
        int count = _AllStreams.Count;
        if (Index < NormalUpdateCount)
        {
            SomethingLeft = true;
            int i = Index;
            InternalUpdater Current = _AllStreams[i];
            while (Current != null)
            {
                InternalUpdater Next = Current._Next;
                if (Current._ToRecycle)
                {
                    Current.Recycle();
                }
                else
                {
                    Current._RealElapsedTime += DeltaTime;
                    Current._FrameQuota += Current._FrameRate;
                    while (Current._FrameQuota >= 1.0f)
                    {
                        Current._Function(Current._RealElapsedTime);
                        Current._RealElapsedTime = 0.0f;
                        Current._FrameQuota -= 1.0f;
                    }
                }
                Current = Next;
            }
        }


        #endregion
        #region enums
        if (Index < EnumCount)
        {
            SomethingLeft = true;
            int i = Index;
            InternalUpdater Current = _AllEnumeratorStreams[i];
            while (Current != null)
            {
                InternalUpdater Next = Current._Next;
                if (Current._ToRecycle)
                {
                    Current.Recycle();
                }
                else
                {
                    if (Current._YieldingFor != null)
                    {
                        if (!Current._YieldingFor._ToRecycle)
                        {
                            Current = Next;
                            continue; //just yield.
                        }
                    }
                    Current._YieldingFor = null;
                    Current._InternalElapsedTime._DeltaTime += DeltaTime;
                    Current._FrameQuota += Current._FrameRate;
                    while (Current._FrameQuota >= 1.0f && Current._YieldingFor==null)
                    {
                        //try
                        //{
                            if (!Current._Enumerator.MoveNext())
                            {
                                Current._ToRecycle = true;
                            }
                       //}
                       //catch(Exception e)
                       //{
                       //    Debug.LogError("Error in: " + Current._Holder.gameObject.name + " //" + e.Message) ;
                       //}
                        Current._YieldingFor = (InternalUpdater)Current._Enumerator.Current;
                        if(Current._YieldingFor!=null)
                        {
                            if(Current._YieldingFor._Stream < Current._Stream)
                            {
                                Debug.LogError("CANNOT HAVE A YIELDING ROUTINE WITH LOWER PRIORITY THAN ROUTINE YIELDING FOR");
                                Current._YieldingFor = null;
                            }
                        }
                        //Current._Instruction = (FYield)Current._Enumerator.Current;
                        Current._InternalElapsedTime._DeltaTime = 0.0f;
                        Current._FrameQuota -= 1.0f;
                    }
                }
                Current = Next;
            }
        }
        #endregion
        return SomethingLeft;
    }

    public static void DestroyAll()
    {
        _AllStreams = new List<InternalUpdater>();
        _AllLateStreams = new List<InternalUpdater>();
        _AllFStreams = new List<InternalUpdater>();
        _AllEnumeratorStreams = new List<InternalUpdater>();
        _Head = null;
    }

    private void Recycle()
    {
        _NextElement = _Head;
        _Head = this;

        if (_Previous == null)
        {
            if (_Function != null)//normal functions case.
            {
                if (_IsFixed) //fixed updates
                {
                   
                        _AllFStreams[_Stream] = _Next;
                    
                }
                else
                {
                    if (_IsLate)
                    {
                        _AllLateStreams[_Stream] = _Next;
                    }
                    else
                    {
                        _AllStreams[_Stream] = _Next;
                    }
                }
            }
            else //enumerations case.
            {
                if (_IsFixed)
                {
                    _AllFixedEnumeratorStreams[_Stream] = _Next;
                }
                else
                {
                    _AllEnumeratorStreams[_Stream] = _Next;
                }
            }
            if (_Next != null)
            {
                _Next._Previous = null;
            }
        }
        else
        {
            _Previous._Next = _Next;
            if (_Next != null)
            {
                _Next._Previous = _Previous;
            }
        }

        if (!_ComponentDisabled)
        {
            //this means we are recycling before the component actually dies
            if (_CompPrevious == null)
            {
                _Holder._Head = _CompNext;

                if (_CompNext != null)
                {
                    _CompNext._CompPrevious = null;
                }
            }
            else
            {
                _CompPrevious._CompNext = _CompNext;
                if (_CompNext != null)
                {
                    _CompNext._CompPrevious = _CompPrevious;
                }
            }
        }

        _CompNext = null;
        _CompPrevious = null;

        _Function = null;
        _Enumerator = null;
        _ComponentDisabled = false;
        _Previous = null;
        _Next = null;
        _ToRecycle = false;
        _Holder = null;
    }

}


