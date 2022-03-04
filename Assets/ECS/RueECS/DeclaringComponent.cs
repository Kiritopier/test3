using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class DeclaringComponent
{
    [ShowInInspector]
    [LabelText("component", Text = "$_ComponentName")]
    [LabelWidth(150)]
    [SuffixLabel("$_Comment")]
    bool _IsShowing = false;


    public string _Compouneded { get { return _ComponentName + " : " + _Comment; } }

    [HideLabel]
    [HideInInspector]
    public int _ContextID;

    [ShowIf("$_IsShowing")]
    public string _ComponentName;

    [ShowIf("$_IsShowing")]
    public string _Comment;


  

    [LabelText("Fields")]
    [ShowIf("$_IsShowing")]
    public List<DCFieldPackage> _AllGathered = new List<DCFieldPackage>();
}

[Serializable]
public class DCFieldPackage
{
    [BoxGroup("Boxed", ShowLabel =false)]
    [HorizontalGroup(GroupID ="Boxed/Packed")]
    [HideLabel]
    public string _Type;

    [BoxGroup("Boxed")]
    [HorizontalGroup(GroupID = "Boxed/Packed")]
    [HideLabel]
   
    public string _Name;

    [BoxGroup("Boxed")]
    [HorizontalGroup(GroupID = "Boxed/Packed")]
    [LabelWidth(10)]
    [LabelText("H")]
   
    public bool _IsHidden;

    [BoxGroup("Boxed")]
    [HorizontalGroup(GroupID = "Boxed/Packed")]
    [LabelWidth(10)]
    [LabelText("S")]
   
    public bool _IsSimpleObserver;

    [BoxGroup("Boxed")]
    [HorizontalGroup(GroupID = "Boxed/Packed")]
    [LabelWidth(10)]
    [LabelText("C")]
   
    public bool _IsComplexObserver;

    [BoxGroup("Boxed")]
    [HorizontalGroup(GroupID = "Boxed/Packed")]
    [LabelWidth(10)]
    [LabelText("M")]
    
    public bool _IsModifyable;

}
