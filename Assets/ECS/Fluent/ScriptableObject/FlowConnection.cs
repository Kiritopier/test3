using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class FlowConnection
{
    public string _Name;

    [HideInInspector]
    public FlowStep _Source;

    [ValueDropdown("GetListOfSteps")]
    public FlowStep _ReturnType;

    [HorizontalGroup("Data")]
    public List<FlowConnectionPair> _Data;
  

    public IEnumerable<FlowStep> GetListOfSteps()
    {
        List<FlowStep> All = new List<FlowStep>(_Source.transform.GetComponentInParent(typeof(FluentInspectorGeneratorOdin)).transform.GetComponentsInChildren<FlowStep>());
        
        return All;
    }



}

[Serializable]
public class FlowConnectionPair
{
    public string _VariableType;
    public string _VariableName;
}