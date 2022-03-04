using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class ListExtensions
{
    public static T GetRandomElementFromList<T> (this List<T> Me)
    {
        if(Me.Count == 0) { return default(T); }
        int RandomIndex = RueMath.RandomRange(0, Me.Count);
        if(RandomIndex >= Me.Count)
        {
            RandomIndex = 0; //fail safe! cause I am not sure about this.
        }
        return Me[RandomIndex];
    }

    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    //public static void ForRealz<T>(this List<T> Me, Action<T> Do)
    //{
    //    int count = Me.Count;
    //    for(int i = 0; i < count; i++)
    //    {
    //        Do(Me[i]);
    //    }
    //}
}

