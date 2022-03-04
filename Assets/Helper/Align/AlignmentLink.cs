using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentLink : MonoBehaviour
{
    public List<AlignmentLink> _Children = new List<AlignmentLink>();
    public AlignmentRule _Rule;
    public AlignmentJustification _Just;
    public int _StartSort = 0;
    public bool _IgnoreX, _IgnoreY, _IgnoreZ = false;
    public bool _ShouldAutoUpdate = false;

    public float _Separation = 0.0f; //used in the editor to know how much we gonna "add" to the spacing for whatever align algorithm we use.
}

public enum AlignmentRule
{
    NONE,//STATIC OBJECTS
    VERTICAL,
    HORIZONTAL, 
    RADIAL,
}

public enum AlignmentJustification
{
    MIDDLE, //the parent object will be the center of the alignment
    LEFT_OR_TOP, //if the alignment is vertical, it will use from top to bottom
    RIGHT_OR_BOT, //horizontal :right justified
}
