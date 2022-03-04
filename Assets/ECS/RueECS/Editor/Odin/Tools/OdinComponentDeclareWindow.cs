using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class OdinComponentDeclareWindow
{
    public OdinComponentDeclareWindow(AllDataForECS data)
    {
        _AllDeclaredComps = new List<DeclaringComponent>(data._AllDeclaredComponents);
    }

    [Button]
    [FoldoutGroup("Settings")]
    public void SortByName()
    {
        if(_AllDeclaredComps == null) { return; }
        _AllDeclaredComps.Sort((x, y) =>
        {
            return x._ComponentName.CompareTo(y._ComponentName);
        });
    }

    [EnumToggleButtons]
    [OnValueChanged("Check")]
    [FoldoutGroup("Settings")]
    public DisplayingByName Sorting;

    [LabelText("Components")]
    [ShowIf("ShouldDisplayNormal")]
    public List<DeclaringComponent> _AllDeclaredComps;

    [ShowIf("ShouldDisplayFiltered")]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
    public List<DeclaringComponent> _SortedVisually;

    public bool ShouldDisplayNormal()
    {
        return Sorting == DisplayingByName.all;
    }

    public bool ShouldDisplayFiltered()
    {
        return Sorting != DisplayingByName.all;
    }

    public void Check()
    {
        if(Sorting == DisplayingByName.all) { return; }
        _SortedVisually.Clear();
        char converted = Sorting.Get();
        for (int i = 0; i < _AllDeclaredComps.Count; i++)
        {
            var s = _AllDeclaredComps[i]._ComponentName;
            if(s.Length == 0) { continue; }
            if (converted == s.ToLower()[0])
            {
                _SortedVisually.Add(_AllDeclaredComps[i]);
            }
        }
        _SortedVisually.Sort((x, y) =>
        {
            return x._ComponentName.CompareTo(y._ComponentName);
        });
    }

}

public enum DisplayingByName
{
    all,
    A,
    B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q, R, S, T, U,V,W,X,Y,Z
}
public static class DDName
{
    public static char Get(this DisplayingByName e)
    {
        switch (e)
        {
            case DisplayingByName.all:
                return ' ';
            case DisplayingByName.A:
                return 'a';
            case DisplayingByName.B:
                return 'b';
            case DisplayingByName.C:
                return 'c';
            case DisplayingByName.D:
                return 'd';
            case DisplayingByName.E:
                return 'e';
            case DisplayingByName.F:
                return 'f';
            case DisplayingByName.G:
                return 'g';
            case DisplayingByName.H:
                return 'h';
            case DisplayingByName.I:
                return 'i';
            case DisplayingByName.J:
                return 'j';
            case DisplayingByName.K:
                return 'k';
            case DisplayingByName.L:
                return 'l';
            case DisplayingByName.M:
                return 'm';
            case DisplayingByName.N:
                return 'n';
            case DisplayingByName.O:
                return 'o';
            case DisplayingByName.P:
                return 'p';
            case DisplayingByName.Q:
                return 'q';
            case DisplayingByName.R:
                return 'r';
            case DisplayingByName.S:
                return 's';
            case DisplayingByName.T:
                return 't';
            case DisplayingByName.U:
                return 'u';
            case DisplayingByName.V:
                return 'v';
            case DisplayingByName.W:
                return 'w';
            case DisplayingByName.X:
                return 'x';
            case DisplayingByName.Y:
                return 'y';
            case DisplayingByName.Z:
                return 'z';
            default:
                break;
        }
        return ' ';
    }
}