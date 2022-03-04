using System;
using System.Collections.Generic;
using UnityEngine;
using RTween;
using RueECS;
public class RueTween// : MonoBehaviour
{
    #region Tween creation
    #region Timers
    public static ITimer Time(float Duration)
    {
        Ins();
        Timer Ret = Timer.Create();
        Ret.Start(Duration, _Instance);
        return Ret;
    }
    public static ITimer Time(float Duration, GameObject Target)
    {
        Ins();
        Timer Ret = Timer.Create();
        Ret.Start(Duration, _Instance, Target);
        return Ret;
    }
    public static ITimer Time(float Duration, BaseEntity Target, int Channel = 0)
    {
        Ins();
        Timer Ret = Timer.Create();
        Ret.Start(Duration, _Instance, Target);
        //var ent = EM._TweenTrackerEntity().__RueTweenTracker(Ret, Channel).__Target(Target.UniqueID()).__Finish();
        //int ver = ent.GetVersion();
        //Ret.OnComplete(() => { if (ent.GetVersion() == ver) { ent.Destroy(); } });
        return Ret;
    }
    public static ITimer Time(RueTween T, float Duration)
    {
        Ins();
        Timer Ret = Timer.Create();
        Ret.Start(Duration, T);
        return Ret;
    }
    public static ITimer Time(RueTween T, float Duration, GameObject Target)
    {
        Ins();
        Timer Ret = Timer.Create();
        Ret.Start(Duration, T, Target);
        return Ret;
    }
    public static ITimer Time(RueTween T, float Duration, BaseEntity Target, int Channel = 0)
    {
        Ins();
        Timer Ret = Timer.Create();
        Ret.Start(Duration, T, Target);
        //var ent = EM._TweenTrackerEntity().__RueTweenTracker(Ret, Channel).__Target(Target.UniqueID()).__Finish();
        //int ver = ent.GetVersion();
        //Ret.OnComplete(() => { if (ent.GetVersion() == ver) { ent.Destroy(); } });
        return Ret;
    }
    #endregion
    #region Direct floats
    public static RueTweener FloatAdHoc(float Start, float End, float Duration, Action<float> Observer)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(Start, End, Observer);
        GameObject N = null;
        Ret.Start(Duration, _Instance, N);
        return Ret;
    }
    public static RueTweener Float(float Start, float End, float Duration, Action<float> Observer, GameObject Target)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(Start, End, Observer);
        Ret.Start(Duration, _Instance, Target);
        return Ret;
    }
    public static RueTweener Float(float Start, float End, float Duration, Action<float> Observer, BaseEntity Target, int Channel = 0)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(Start, End, Observer);
        Ret.Start(Duration, _Instance, Target);

        //create a 
        //var ent = EM._TweenTrackerEntity().__RueTweenTracker(Ret, Channel).__Target(Target.UniqueID()).__Finish();
        //int ver = ent.GetVersion();
        //Ret.OnComplete(() => { if (ent.GetVersion() == ver) { ent.Destroy(); } });
        return Ret;
    }
    public static RueTweener Generic(float Duration, Action<float> Observer, GameObject Target)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(0.0f, 1.0f, Observer);
        Ret.Start(Duration, _Instance, Target);
        return Ret;
    }
    public static RueTweener Generic(float Duration, Action<float> Observer)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(0.0f, 1.0f, Observer);
        Ret.Start(Duration, _Instance);
        return Ret;

    }
    public static RueTweener Generic(float Duration, Action<float> Observer, BaseEntity Target)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(0.0f, 1.0f, Observer);
        Ret.Start(Duration, _Instance, Target);
        return Ret;
    }


    public static RueTweener FloatAdHoc(RueTween T, float Start, float End, float Duration, Action<float> Observer)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(Start, End, Observer);
        GameObject N = null;
        Ret.Start(Duration, T, N);
        return Ret;
    }
    public static RueTweener Float(RueTween T, float Start, float End, float Duration, Action<float> Observer, GameObject Target)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(Start, End, Observer);
        Ret.Start(Duration, T, Target);
        return Ret;
    }
    public static RueTweener Float(RueTween T, float Start, float End, float Duration, Action<float> Observer, BaseEntity Target, int Channel = 0)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(Start, End, Observer);
        Ret.Start(Duration, T, Target);

        //create a 
        //var ent = EM._TweenTrackerEntity().__RueTweenTracker(Ret, Channel).__Target(Target.UniqueID()).__Finish();
        //int ver = ent.GetVersion();
        //Ret.OnComplete(() => { if (ent.GetVersion() == ver) { ent.Destroy(); } });
        return Ret;
    }
    public static RueTweener Generic(RueTween T, float Duration, Action<float> Observer, GameObject Target)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(0.0f, 1.0f, Observer);
        Ret.Start(Duration, T, Target);
        return Ret;
    }
    public static RueTweener Generic(RueTween T, float Duration, Action<float> Observer)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(0.0f, 1.0f, Observer);
        Ret.Start(Duration, T);
        return Ret;

    }
    public static RueTweener Generic(RueTween T, float Duration, Action<float> Observer, BaseEntity Target)
    {
        Ins();
        FloatTween Ret = FloatTween.Create(0.0f, 1.0f, Observer);
        Ret.Start(Duration, T, Target);
        return Ret;
    }



    #endregion

    //static Dictionary<int, ECSTracker> _ECSTracking;
    private static void Ins()
    {
        if(_Instance == null)
        {
            //_ECSTracking = new Dictionary<int, ECSTracker>();
            //StaticCollections._RueTweenTrackers.OnAdded((e) =>
            //{
            //    int id = e.GetTarget()._ID;
            //    ECSTracker TweensFound = null;
            //    if (!_ECSTracking.TryGetValue(id, out TweensFound))
            //    {
            //        TweensFound = new ECSTracker();
            //        _ECSTracking.Add(id, TweensFound);
            //    }
            //    TweensFound._TweensAlive.Add(e.GetRueTweenTracker()._Tracking);
            //    TweensFound._Trackers.Add(e);
            //}).OnRemoved((e) =>
            //{
            //    var t = _ECSTracking[e.GetTarget()._ID];
            //    var tween = e.GetRueTweenTracker()._Tracking;
            //    if(tween.BelongsTo(e))
            //    {
            //        tween.Kill(false);
            //    }
            //    t._TweensAlive.Remove(tween);
            //    t._Trackers.Remove(e);
            //});
            _Instance = new RueTween();
        }
    }
    //public class ECSTracker
    //{
    //    public List<RueTweener> _TweensAlive = new List<RueTweener>();
    //    public List<Collection_RueTweenTrackerTarget.IType> _Trackers = new List<Collection_RueTweenTrackerTarget.IType>();
    //}
    #endregion

    public static void DisposeAllTweensFrom(GameObject Go)
    {
        LifeTimeObserver Obs = Go.GetComponent<LifeTimeObserver>();
        if (Obs)
        {
            Obs.DisposeAllTweens();
        }
    }

    public static void DisposeAllTweensFrom(BaseEntity Go, int TweenChannel)
    {
        int id = Go._UniqueID;
        //ECSTracker A = null;
        //List<RueTweener> All = null;
        //if(_ECSTracking.TryGetValue(id, out A))
        //{
        //    All = A._TweensAlive;
        //    for (int i = 0; i < All.Count; i++)
        //    {
        //        if(All[i].BelongsTo(Go) && A._Trackers[i].GetRueTweenTracker()._Channel == TweenChannel)
        //        {
        //            All[i].Pause(true);
        //            A._Trackers[i].Destroy();
        //        }
        //    }
        //}
    }

    private static RueTween _Instance;
    public List<BaseRueTween> _ToRecycle = new List<BaseRueTween>();
    public BaseRueTween _Head;

    //public int EverCreatedTweens = 0;
    public int Alive = 0;
    public bool _IsPaused = false;

    public static void DirectUpdate(float Delta)
    {
        Ins();
        _Instance.GUpdate(Delta);
    }
    public void GUpdate(float DeltaTime)
    {
        if (_IsPaused) { return; }
        //EverCreatedTweens = BaseRueTween.EverCreated;
        Alive = BaseRueTween.Alive;
        BaseRueTween Current = _Head;
        while(Current != null)
        {
            BaseRueTween Next = Current._Next;
            Current.Step(DeltaTime);
            Current = Next;
        }
        int count = _ToRecycle.Count;
        for(int i  = 0; i < count; i++)
        {
            _ToRecycle[i].Recycle();
        }
        _ToRecycle.Clear();
    }

    public static void StopAllTweensOf(BaseEntity Ent, bool CallsOnComplete)
    {
        BaseRueTween Current = _Instance._Head;
        while (Current != null)
        {
            BaseRueTween Next = Current._Next;
            if(Current._ECSUser == Ent)
            {
                Current.Kill(CallsOnComplete);
            }
        }
        int count = _Instance._ToRecycle.Count;
        for (int i = 0; i < count; i++)
        {
            _Instance._ToRecycle[i].Recycle();
        }
        _Instance._ToRecycle.Clear();
    }
    
}

