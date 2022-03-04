using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class FuncDelTest : MonoBehaviour
{
    //float Val = 0.0f;

    ElapsedTimer Timer;
    int Counter = 0;


    void Start()
    {
        transform.parent = null;
        this.AddRoutine(Routine());
        this.AddUpdate((x) =>
        {
           // Debug.Log(Counter);
        });
        //Timer = this.AddRoutine(Routine());
    }

    IEnumerator Routine()
    {
        while (true)
        {
            float TimeN = Time.realtimeSinceStartup;
            yield return FYield.GoToBackgroundThread;
            while (true)
            {
                Counter++;
                if(Counter >= 1000000000)
                {
                    yield return FYield.GoToMainThread;
                    break;
                }
            }
            Debug.Log("Finished");
            Debug.Log(Time.realtimeSinceStartup - TimeN);
            Counter = 0;
        }
    }
}

