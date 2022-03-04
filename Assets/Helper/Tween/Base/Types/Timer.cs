using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTween
{
    public class Timer : BaseRueTween, ITimer
    {
        private static Timer HeadItem;
        Timer NextItem;
        #region Pool
        public static Timer Create()
        {
            Timer Vessel = null;
            if (HeadItem == null)
            {
                Vessel = new Timer();
            }
            else
            {
                Vessel = HeadItem;
                HeadItem = HeadItem.NextItem;
            }
            return Vessel;
        }
        public void FreeMemory()
        {
            if (HeadItem != null)
            {
                HeadItem.InternalMemoryFreeing();
            }
            HeadItem = null;
        }
        private void InternalMemoryFreeing()
        {
            if (NextItem != null)
            {
                NextItem.InternalMemoryFreeing();
                NextItem = null;
            }
        }
        public override  void Recycle()
        {
            NextItem = HeadItem;
            HeadItem = this;
            base.Recycle();
        }
        #endregion

        new public ITimer OnComplete(Action Todo)
        {
            _OnComplete.Add(Todo);
            return this;
        }

        new public ITimer OnUpdate(Action<float> Todo)
        {
            _OnUpdate.Add(Todo);
            return this;
        }

        new public ITimer Loops(int Amount)
        {
            _LoopCount = Amount;
            return this;
        }
    }

    public interface ITimer: RueTweener
    {
        new ITimer OnComplete(Action Todo);
        new ITimer OnUpdate(Action<float> Todo);
        new ITimer Loops(int Amount);
    }
}