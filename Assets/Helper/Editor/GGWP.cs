using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class GGWP 
{
    public static void ColoredButton(Rect R, Color C, String Title, Action OnClicked)
    {
        Color A = GUI.color;
        GUI.color = C;
        if(GUI.Button(R, new GUIContent(Title)))
        {
            OnClicked();
        }
        GUI.color = A; 
    }

    public static string HorizontalTextField(string Label, string TextField)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(Label);
        if(string.IsNullOrEmpty(TextField))
        {
            TextField = "";
        }
        if (string.IsNullOrEmpty(Label))
        {
            Label = "";
        }
        string Cached = GUILayout.TextField(TextField);
        GUILayout.EndHorizontal();
        return Cached;
    }

    public static void Property(string Name, SerializedProperty Prop)
    {
        EditorGUILayout.PropertyField(Prop, new GUIContent(Name), true);
        Prop.serializedObject.ApplyModifiedProperties();
    }

    public static void Property(string Name, SerializedObject OBJ, string PropName, Action OnChanged = null)
    {
        SerializedProperty Prop = OBJ.FindProperty(PropName);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(Prop, new GUIContent(Name), true);
        if (EditorGUI.EndChangeCheck())
        {
            Prop.serializedObject.ApplyModifiedProperties();
            if(OnChanged != null)
            {
                OnChanged();
            }
        }
    }

    public static void VerticalBox(Action Do, float EndSpace)
    {
        GUILayout.BeginVertical(GUI.skin.box);
        Do();
        GUILayout.EndVertical();
        GUILayout.Space(EndSpace);
    }
    public static void HorizontalBox(Action Do, float EndSpace)
    {
        GUILayout.BeginHorizontal(GUI.skin.box);
        Do();
        GUILayout.EndHorizontal();
        GUILayout.Space(EndSpace);
    }
}

