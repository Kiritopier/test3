using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class FlowStep: SerializedMonoBehaviour
{
    [OnValueChanged("ChangedName")]
    public string _Name;
    [ListDrawerSettings(CustomAddFunction ="AddingConnection", ListElementLabelName = "_Name")]
    public List<FlowConnection> _Connections = new List<FlowConnection>();

    [ListDrawerSettings(CustomAddFunction ="AddChildrenStep", ListElementLabelName = "_Name")]
    [InlineEditor]
    public List<FlowStep> _InnerSteps = new List<FlowStep>();


    public FlowStep AddChildrenStep()
    {
        FlowStep NS = new GameObject("").AddComponent<FlowStep>();
        NS.transform.SetParent(transform);
        return NS;
    }

    void ChangedName()
    {
        name = _Name;
    }

    public FlowConnection AddingConnection()
    {
        FlowConnection NewConnection = new FlowConnection();
        NewConnection._Source = this;
        return NewConnection;
    }
#if UNITY_EDITOR
    public void GUIInsert(int index)
    {
        EditorGUILayout.LabelField(_Connections[index]._Name);
    }
#endif
}

[Serializable]
public class FlowConnectionToggle
{
    public FlowConnection _Connection;
}