using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(RefBlackboard))]
public class RefBlackboardAddOn : Editor
{
    ReorderableList _List;
    RefBlackboard _Target;
    void OnEnable()
    {
        _Target = (RefBlackboard)target;
        _List = new ReorderableList(_Target._Holding, typeof(RefBBComponent), true, true, true, true);
        _List.onAddCallback = (x) =>
        {
            RefBBComponent New = new RefBBComponent();
            New._ID = "NewElement";
            _Target._Holding.Add(New);
        };
        _List.drawElementCallback = (x, y, z, q) =>
        {
            Rect ID = new Rect(x.x, x.y, x.width*0.5f, x.height);
            Rect Comp = new Rect(x.x+x.width*0.5f, x.y, x.width*0.5f, x.size.y);
            Component FoundFF = _Target._Holding[y]._Component;
            _Target._Holding[y]._Component = (Component)EditorGUI.ObjectField(Comp, "", _Target._Holding[y]._Component, typeof(Component), true);
            
            if (FoundFF != _Target._Holding[y]._Component)
            {
                Component C = _Target._Holding[y]._Component;
                if(C == null) { return; }
                GenericMenu Menu = new GenericMenu();
                Component[] Found = C.gameObject.GetComponents(typeof(Component));
                int Lenght = Found.Length;
                for (int i = 0; i < Lenght; i++)
                {
                    Component Current = Found[i];
                    int Index = y;
                    Menu.AddItem(new GUIContent(Current.GetType().Name), false, (J) =>
                    {
                        _Target._Holding[Index]._Component = (Component)J;
                        EditorUtility.SetDirty(_Target);
                        EditorUtility.SetDirty(_Target.gameObject);
                    }, Current);
                }
                Menu.ShowAsContext();
            }
            _Target._Holding[y]._ID = EditorGUI.TextField(ID, _Target._Holding[y]._ID);
        };
    }
    public override void OnInspectorGUI()
    {
        _List.DoLayoutList();

        //if(GUI.changed)
        //{
        //    EditorUtility.SetDirty(_Target);
        //    EditorUtility.SetDirty(_Target.gameObject);
        //}
    }
}

