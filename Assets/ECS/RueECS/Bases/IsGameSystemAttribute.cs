using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using RueECS;
using System.Linq;

public class IsGameSystemAttribute: Attribute
{
    //public bool _IsStepper = false;
    //public bool _IsLateStepper = false;
    public int _Priority = 0; //the higher the number, it will be called first
    //public bool _InitialState = false;
    //public bool _OnActivated = false;
    //public bool _OnDeactivated = false;


    public IsGameSystemAttribute()
    {
       
    }
}

