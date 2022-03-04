using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public partial class DeclaringCollection
{

    //[ShowInInspector]
    //[LabelText("component", Text = "$_CollectionName")]
    //[LabelWidth(150)]
    ////[SuffixLabel("$_Comment")]
    //bool _IsShowing = false;




    [HideIf("$True")]
    public HashSet<string> _Aliases = new HashSet<string>();

    //[VerticalGroup("Ins")]
    //[HideLabel]
    //[TableColumnWidth(300, false)]
    [ReadOnly]
    [ShowInInspector]
    public string _CollectionName {  get
        {
            return ID() + "Archetype";
        }
    }
    //[VerticalGroup("Ins")]
    //[HideLabel]
    public int _Priority = 0;
    [HideIf("$True")]
    public List<string> _EntitiesPresent;


    const bool True = true;
    [HideIf("$True")]
    public List<string> _Components = new List<string>();
    [HideIf("$True")]
    public List<string> _NoComponents = new List<string>();
    


    public bool DoesEntityBelong(DeclaringEntity e)
    {
        for (int i = 0; i < _Components.Count; i++)
        {
            if(!e._Components.Contains(_Components[i]))
            {
                return false;
            }
        }
        for (int i = 0; i < _NoComponents.Count; i++)
        {
            if (e._Components.Contains(_NoComponents[i]))
            {
                return false;
            }
        }
        return true;
    }

    public string ID()
    {
        _Components.Sort();
        _NoComponents.Sort();

        string parse = "";
        for (int i = 0; i < _Components.Count; i++)
        {
            parse += _Components[i];
        }
        if (_NoComponents.Count > 0)
        {
            parse += "Ignore";
        }
        for (int i = 0; i < _NoComponents.Count; i++)
        {
            parse += _NoComponents[i];
        }
        return parse;
    }
}
