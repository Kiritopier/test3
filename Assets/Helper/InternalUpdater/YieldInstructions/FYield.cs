using System;
using System.Collections.Generic;

public class FYield
{
    //private static FYield _Head;
    //FYield _Next;

    public FYieldInstruction _Instruction;
    public float _Value;

    private FYield()
    {

    }

    public static FYield Wait(float WaitTime)
    {
        FYield Vessel = new FYield();
        //if (_Head != null) { Vessel = _Head; _Head = _Head._Next; } else { Vessel = new FYield(); }
        Vessel._Value = WaitTime;
        Vessel._Instruction = FYieldInstruction.WAIT;
        return Vessel;
    }

    public static FYield SkipFrames(int Frames)
    {
        FYield Vessel = new FYield();
        //if (_Head != null) { Vessel = _Head; _Head = _Head._Next; } else { Vessel = new FYield(); }
        Vessel._Value = Frames;
        Vessel._Instruction = FYieldInstruction.SKIP_FRAMES;
        return Vessel;
    }

    public static FYield GoToMainThread
    {
        get
        {
            FYield Vessel = new FYield();
            //if (_Head != null) { Vessel = _Head; _Head = _Head._Next; } else { Vessel = new FYield(); }
            Vessel._Instruction = FYieldInstruction.GO_TO_MAIN_THREAD;
            return Vessel;
        }
    }

    public static FYield GoToBackgroundThread
    {
        get
        {
            FYield Vessel = new FYield();
            //if (_Head != null) { Vessel = _Head; _Head = _Head._Next; } else { Vessel = new FYield(); }
            Vessel._Instruction = FYieldInstruction.GO_TO_BACKGROUND_THREAD;
            return Vessel;
        }
    }

    public void Recycle()
    {
        //_Next = _Head;
        //_Head = this;
    }
}

public enum FYieldInstruction
{
    WAIT,
    SKIP_FRAMES,
    GO_TO_MAIN_THREAD,
    GO_TO_BACKGROUND_THREAD,
}

