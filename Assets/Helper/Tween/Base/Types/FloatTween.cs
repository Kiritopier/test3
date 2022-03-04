using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTween
{
    public class FloatTween : BaseRueTween
    {
        private static Queue<FloatTween> _Stack = new Queue<FloatTween>();
        Action<float> Callback;
        float FStart;
        float End;
        public static FloatTween Create(float Start, float End, Action<float> Callback)
        {
            FloatTween Vessel = null;
            if(_Stack.Count > 0)
            {
                Vessel = _Stack.Dequeue();
            }
            else
            {
                Vessel = new FloatTween();
            }
            
            Vessel.Callback = Callback;
            Vessel.FStart = Start;
            Vessel.End = End;
            return Vessel;
        }

        public override RueTweener Invert()
        {
            float T = FStart;
            FStart = End;
            End = T;
            return base.Invert();
        }

        protected override void ActualStep(float Ratio)
        {
            Callback(RueMath.Lerp(FStart, End, Ratio));
        }

        public override void Recycle()
        {
            if (!_Stack.Contains(this))
            {
                _Stack.Enqueue(this);
                base.Recycle();
            }
            else
            {
                Debug.LogError("This is terrible");
            }
            Callback = null;
        }

    }
}

