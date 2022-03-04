using System;
using System.Collections.Generic;
using UnityEngine;
using RueECS;
namespace RTween
{
    [Serializable]
    public class BaseRueTween: RueTweener
    {
        public static int EverCreated = 0;
        public static int Alive = 0;

        protected BaseRueTween() { EverCreated++; _Destroyed = ObserverDestroyed; }
        private Action _Destroyed;
        public LifeTimeObserver _Obs;
        public float _Delay;
        public float _CurrentElapsedTime;
        public float _Duration;
        public int _LoopCount;
        public float _TimeMultiplier = 1.0f;
        public RueEase _Ease;
        public float _Overshoot = 0.0f;

        public BaseRueTween _Next;
        public BaseRueTween _Previous;

        //Flags
        public bool _OnRecycleRow = false;
        public bool _Pausable = true;
        public bool _IsPaused = false;
        public bool _IsInverted = false;
        public bool _UseRealTime = false;
        public bool _DoesPingPong = false;

        //links
        public RueTween _Owner;
        public GameObject _User;
        public BaseEntity _ECSUser;
        int _ECSVersioning = -1;

        //Callbacks
        public List<Action> _OnComplete = new List<Action>();
        public List<Action<float>> _OnCompleteWithOvershoot = new List<Action<float>>();
        public List<Action> _OnLoopsComplete = new List<Action>();
        public List<Action<float>> _OnUpdate = new List<Action<float>>();

        public virtual void Start(float Duration, RueTween Owner, BaseEntity e = null)
        {
            _Pausable = true;
            _User = null;
            _ECSUser = e;
            _ECSVersioning = e.GetVersion();
            _Overshoot = 0.0f;
            _TimeMultiplier = 1.0f;
            _Ease = RueEase.Linear;
            _Owner = Owner;
            _UseRealTime = false;
            _OnRecycleRow = false;
            _Duration = Duration;
            if (_Duration <= 0) { _Duration = 0.001f; Debug.LogError("Tweens cant have a duration of 0"); }
            _CurrentElapsedTime = 0;
            _Previous = null;
            _Next = Owner._Head;
            _IsPaused = false;
            _DoesPingPong = false;
            _LoopCount = 0;
            if (Owner._Head != null)
            {
                Owner._Head._Previous = this;
            }
            Owner._Head = this;
            Alive++;
        }

        public virtual void Start(float Duration, RueTween Owner, GameObject TargetUser = null) 
        {
            _Pausable = true;
            _User = TargetUser;
            if(_User != null)
            {
                _Obs = _User.GetComponent<LifeTimeObserver>();
                if(_Obs == null)
                {
                    _Obs = _User.AddComponent<LifeTimeObserver>();
                    _Obs.hideFlags = HideFlags.HideInInspector;
                }
                _Obs.AddObserver(_Destroyed);
            }
            _Overshoot = 0.0f;
            _TimeMultiplier = 1.0f;
            _Ease = RueEase.Linear;
            _Owner = Owner;
            _UseRealTime = false;
            _OnRecycleRow = false;
            _Duration = Duration;
            if(_Duration <= 0) { _Duration = 0.001f; Debug.LogError("Tweens cant have a duration of 0"); }
            _CurrentElapsedTime = 0;
            _Previous = null;
            _Next = Owner._Head;
            _IsPaused = false;
            _DoesPingPong = false;
            _LoopCount = 0;
            if(Owner._Head != null)
            {
                Owner._Head._Previous = this;
            }
            Owner._Head = this;
            Alive++;
        }
        public virtual void Start(float Duration, RueTween Owner)
        {
            _Pausable = true;
            _User = null;
            if (_User != null)
            {
                _Obs = _User.GetComponent<LifeTimeObserver>();
                if (_Obs == null)
                {
                    _Obs = _User.AddComponent<LifeTimeObserver>();
                    _Obs.hideFlags = HideFlags.HideInInspector;
                }
                _Obs.AddObserver(_Destroyed);
            }
            _Overshoot = 0.0f;
            _TimeMultiplier = 1.0f;
            _Ease = RueEase.Linear;
            _Owner = Owner;
            _UseRealTime = false;
            _OnRecycleRow = false;
            _Duration = Duration;
            if (_Duration <= 0) { _Duration = 0.001f; Debug.LogError("Tweens cant have a duration of 0"); }
            _CurrentElapsedTime = 0;
            _Previous = null;
            _Next = Owner._Head;
            _IsPaused = false;
            _DoesPingPong = false;
            _LoopCount = 0;
            if (Owner._Head != null)
            {
                Owner._Head._Previous = this;
            }
            Owner._Head = this;
            Alive++;
        }
        public virtual void Stop(bool Completed = false) 
        {
            if(Completed)
            {
                int OnCompleteCount = _OnComplete.Count;
                for (int i = 0; i < OnCompleteCount; i++)
                {
                    _OnComplete[i]();
                }
            }
            if (!_OnRecycleRow)
            {
                _OnRecycleRow = true;
                _Owner._ToRecycle.Add(this);
            }
            else
            {
                Debug.LogError("TERRIBLY WRONG IN STOP");
            }
            //_OnRecycleRow = true;
            //_Owner._ToRecycle.Add(this);
        }

        /// <summary>
        /// Called internally by the RueTween
        /// </summary>
        /// <param name="ElapsedTime"></param>
        public void Step(float ElapsedTime) 
        {
            if(_IsPaused)
            {
                return;
            }
            float Ratio = _CurrentElapsedTime / _Duration;
            _Delay -= ElapsedTime;
            if(_Delay > 0)
            {
                return;
            }
            if (_OnRecycleRow)
            {
                //Debug.Log("Stepping something recycled");
                return;
            }
            if(_ECSUser!=null)
            {
                if(_ECSVersioning!=_ECSUser.GetVersion())
                {
                    ObserverDestroyed();
                    return;
                }
            }
            if (_UseRealTime)
            {
                _CurrentElapsedTime += RoutineTimer.RealDeltaTime * _TimeMultiplier;
            }
            else
            {
                _CurrentElapsedTime += ElapsedTime * _TimeMultiplier;
            }
            if (_CurrentElapsedTime > _Duration)
            {
                _Overshoot = _CurrentElapsedTime - _Duration;
                _CurrentElapsedTime = _Duration;
            }
            int OnUpdateCount = _OnUpdate.Count;
            for(int i = 0; i < OnUpdateCount; i++)
            {
                _OnUpdate[i](Ratio);
            }
            Ratio = _CurrentElapsedTime / _Duration;
            ActualStep(RueMath.EaseValue(_Ease, Ratio));
            if(Ratio == 1)
            {
                if (_LoopCount > 1)
                {
                    _LoopCount--;
                    if (_DoesPingPong)
                    {
                        Invert();
                    }
                    else
                    {
                        Reset();
                    }
                    TriggerOnComplete();
                    return;
                }
                else
                {
                    if (_LoopCount == -1) //infinite loops
                    {
                        if (_DoesPingPong)
                        {
                            Invert();
                        }
                        else
                        {
                            Reset();
                        }
                        TriggerOnComplete();
                        return;
                    }
                    else
                    {
                        //the tween is over, recycle
                        TriggerOnComplete();
                        TriggerOnLoopsComplete();
                        if (!_OnRecycleRow)
                        {
                            _OnRecycleRow = true;
                            _Owner._ToRecycle.Add(this);
                        }
                        else
                        {
                            //Debug.LogError("IN TRIGGERING COMPLETE OF THE LOOPS OR SINGLE SHOT");
                        }
                        return;
                    }
                }
            }
        }
        //public void ForceComplete()
        //{
        //    _IsPaused = false;
        //    _Delay = 0;
        //    ActualStep(1.0f);
        //    TriggerOnComplete();
        //    _OnRecycleRow = true;
        //    _Owner._ToRecycle.Add(this);
        //}
        void TriggerOnComplete()
        {
            int OnCompleteCount = _OnComplete.Count;
            for (int i = 0; i < OnCompleteCount; i++)
            {
                _OnComplete[i]();
            }
            int OnCompleteOvershoot = _OnCompleteWithOvershoot.Count;
            for (int i = 0; i < OnCompleteOvershoot; i++)
            {
                _OnCompleteWithOvershoot[i](_Overshoot);
            }
        }
        void TriggerOnLoopsComplete()
        {
            int OnCompleteCount = _OnLoopsComplete.Count;
            for (int i = 0; i < OnCompleteCount; i++)
            {
                _OnLoopsComplete[i]();
            }
        }

        protected virtual void ActualStep(float Ratio)
        {

        }

        protected void ObserverDestroyed()
        {
            if(!_OnRecycleRow)
            {
                _OnRecycleRow = true;
                if (_Owner._ToRecycle.Contains(this))
                {
                    Debug.LogError("this is terribly wrong");
                }
                else
                {
                    _Owner._ToRecycle.Add(this);
                }
            }
        }

        public virtual void Recycle()
        {
            _Delay = 0.0f;
            Alive--;
            _User = null;
            if(_Obs != null)
            {
                _Obs.RemoveTrigger(_Destroyed);
            }
            _Obs = null;
            _ECSUser = null;
            _ECSVersioning = -1;
          
            //take yourself out of the pool 
            if(_Previous != null)
            {
                _Previous._Next = _Next;
                if(_Next != null)
                {
                    _Next._Previous = _Previous;
                }
            }
            else
            {
                _Owner._Head = _Next;
                if(_Next != null)
                {
                    _Next._Previous = null;
                }
            }
            _Previous = null;
            _Next = null;
            //clean all callbacks
            _OnComplete.Clear();
            _OnLoopsComplete.Clear();
            _OnCompleteWithOvershoot.Clear();
            _OnUpdate.Clear();
        }

        public RueTweener PingPong(bool Pingpong)
        {
            _DoesPingPong = Pingpong;
            return this;
        }
        public RueTweener OnComplete(Action Add)
        {
            _OnComplete.Add(Add);
            return this;
        }
        public RueTweener OnLoopsCompleted(Action Add)
        {
            _OnLoopsComplete.Add(Add);
            return this;
        }
        public RueTweener SetEase(RueEase Ease)
        {
            _Ease = Ease;
            return this;
        }
        public RueTweener Delay(float Delay)
        {
            _Delay = Delay;
            return this;
        }
        public RueTweener UseRealTime(bool Use)
        {
            _UseRealTime = Use;
            return this;
        }
        public RueTweener OnUpdate(Action<float> Add)
        {
            _OnUpdate.Add(Add);
            return this;
        }
        public RueTweener Loops(int Amount)
        {
            _LoopCount = Amount;
            return this;
        }
        public virtual RueTweener Invert()
        {
            _IsInverted = !_IsInverted;
            float D = _Duration - _CurrentElapsedTime;
            _CurrentElapsedTime = D;
            return this;
        }
        public RueTweener Reset()
        {
            _CurrentElapsedTime = 0;
            return this;
        }
        public RueTweener Pause(bool IsPaused)
        {
            _IsPaused = IsPaused;
            return this;
        }
        public RueTweener SetNewDuration(float Duration)
        {
            _Duration = Duration;
            return this;
        }
        public float GetDuration()
        {
            return _Duration;
        }

        public void Kill(bool ShouldCallOnComplete)
        {
            if (_OnRecycleRow)
            {
                //Debug.Log("TRYING TO KILL SOMETHING THATS DEAD ALREADY");
                return;
            }
            if(ShouldCallOnComplete)
            {
                TriggerOnComplete();
                TriggerOnLoopsComplete();
            }
            if (!_OnRecycleRow)
            {
                _OnRecycleRow = true;
                _Owner._ToRecycle.Add(this);
            }
            else
            {
                //Debug.LogError("IN KILL!");
            }
            //Recycle();
        }

        public RueTweener SetTimeMultiplier(float Multi)
        {
            _TimeMultiplier = Multi;
            return this;
        }

        public RueTweener OnCompleteOvershoot(Action<float> Add)
        {
            _OnCompleteWithOvershoot.Add(Add);
            return this;
        }

        public RueTweener GoTo(float ElapsedTime)
        {
            _CurrentElapsedTime = ElapsedTime;
            return this;
        }

        public float GetElapsedTime()
        {
            if(_CurrentElapsedTime == 0.0f)
            {
                _CurrentElapsedTime = 0.0000001f; //fail safe in case this happens right away, the tweens cannot have a 0 duration. (division by 0)
            }
            return _CurrentElapsedTime;
        }

        public bool BelongsTo(BaseEntity e)
        {
            if(_ECSUser == e && _ECSVersioning == e.GetVersion())
            {
                return true;
            }

            return false;
        }
    }


    public interface RueTweener
    {
        bool BelongsTo(BaseEntity e);
        RueTweener SetTimeMultiplier(float Multi);
        RueTweener PingPong(bool pingpong);
        RueTweener UseRealTime(bool Use);
        RueTweener OnUpdate(Action<float> Add);
        RueTweener OnComplete(Action Add);
        RueTweener OnCompleteOvershoot(Action<float> Add);
        RueTweener OnLoopsCompleted(Action Add);
        RueTweener SetEase(RueEase Ease);
        RueTweener Delay(float Delay);
        RueTweener Loops(int Amount);
        RueTweener Invert();
        RueTweener Reset();
        RueTweener Pause(bool IsPaused);
        RueTweener SetNewDuration(float Duration);
        RueTweener GoTo(float ElapsedTime);
        void Kill(bool ShouldCallOnComplete);
        float GetDuration();
        float GetElapsedTime();
    }
    
}

