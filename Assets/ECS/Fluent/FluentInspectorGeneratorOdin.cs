using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.IO;

public class FluentInspectorGeneratorOdin : SerializedMonoBehaviour
{
    public string _ClassName;
    public string _SavePath;

#if UNITY_EDITOR
    [Button("Commit")]
    public void Commit()
    {
        NonEditorFluentCreator Com = new NonEditorFluentCreator();
        var creator = Com.__Start(_ClassName);
        List<FlowStep> _AllStepsEverywhere = new List<FlowStep>(transform.GetComponentsInChildren<FlowStep>());
        for (int i = 0; i < _AllStepsEverywhere.Count; i++)
        {
            var step = _AllStepsEverywhere[i];
            creator._Step(step._Name);
            for (int a = 0; a < step._Connections.Count; a++)
            {
                var connection = step._Connections[a];
                string returnt = connection._ReturnType == null ? _ClassName : connection._ReturnType._Name;
                var addingargs = Com._Function(connection._Name, returnt);
                for (int q = 0; q < connection._Data.Count; q++)
                {
                    var data = connection._Data[q];
                    addingargs._Argument(data._VariableType, data._VariableName);
                }
                addingargs._FinishFunction();
            }
        }
        Com.Commit(_SavePath);
    }
#endif
}


public class NonEditorFluentCreator : IStepDeclareU, IAddFluentFunctionsU, IAddFunctionArgumentsU
{
    string _Name;
    List<string> _Usings = new List<string>();
    InnerStepDataU _CurrentStep;
    Dictionary<string, InnerStepDataU> _AllSteps = new Dictionary<string, InnerStepDataU>();
    List<InnerFunctionDataU> _AllFunctions = new List<InnerFunctionDataU>();
    InnerFunctionDataU _CurrentFunction;

    public IStepDeclareU __Start(string Name)
    {
        _Name = Name;
        return this;
    }

    public NonEditorFluentCreator _AddUsing(string Using)
    {
        _Usings.Add(Using);
        return this;
    }

    public void Commit(string path)
    {
#if UNITY_EDITOR
        //todo: create the code that will parse all this into a generated c# code so the fluent api can be used.
        string DataPath = Application.dataPath + "/" + path + "/" + _Name + ".cs";
        string final = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataPath);

        using (StreamWriter W = new StreamWriter(final, false))
        {
            StrFor F = new StrFor(W);
            string AllSteps = "";
            foreach (var item in _AllSteps.Keys)
            {
                AllSteps += _Name + "." + item + ", ";
            }

            for (int i = 0; i < _Usings.Count; i++)
            {
                F.NL("using " + _Usings[i] + ";");
            }

            if (AllSteps.Length > 0)
            {
                AllSteps = AllSteps.Remove(AllSteps.Length - 2);
                F.NL("public abstract class " + _Name + " : " + AllSteps);
            }
            else
            {
                F.NL("public abstract class " + _Name);
            }
            F.OpenFunction();


            foreach (var item in _AllSteps.Keys)
            {
                F.NL("public interface " + item);
                F.OpenFunction();
                var data = _AllSteps[item];
                for (int i = 0; i < data._FunctionsDeclared.Count; i++)
                {
                    F.NL(data._FunctionsDeclared[i].ToInterfaceString());
                }
                F.CloseFunction();
                for (int i = 0; i < data._FunctionsDeclared.Count; i++)
                {
                    F.NL(data._FunctionsDeclared[i].ToImplementVirtual());
                }

            }
            F.CloseFunction();
            W.Flush();
            W.Close();
            AssetDatabase.Refresh(ImportAssetOptions.Default);


        }
#endif

    }


    public IAddFunctionArgumentsU _Argument(string FieldType, string FieldName)
    {
        _CurrentFunction._Arguments.Add(new InnerArgumentsDataU() { _ArgumentName = FieldName, _ArgumentType = FieldType });
        return this;
    }

    public IAddFluentFunctionsU _FinishFunction()
    {
        return this;
    }

    public IStepDeclareU _FinishStep()
    {
        return this;
    }

    public IAddFunctionArgumentsU _Function(string FunctionName, string ReturnType)
    {
        for (int i = 0; i < _AllFunctions.Count; i++)
        {
            if (_AllFunctions[i]._FunctionName == FunctionName)
            {
                FunctionName += "_";
                return _Function(FunctionName, ReturnType);
            }
        }
        _CurrentFunction = new InnerFunctionDataU();
        _CurrentFunction._ParentStep = _CurrentStep;
        _CurrentFunction._FunctionName = FunctionName;
        _CurrentFunction._ReturnType = ReturnType;
        _CurrentStep._FunctionsDeclared.Add(_CurrentFunction);
        _AllFunctions.Add(_CurrentFunction);
        return this;
    }

    public IAddFluentFunctionsU _Step(string StepName)
    {
        InnerStepDataU before = null;
        if (_AllSteps.TryGetValue(StepName, out before))
        {
            _CurrentStep = before;
            return this;
        }
        InnerStepDataU NewStep = new InnerStepDataU();
        NewStep._StepName = StepName;
        _AllSteps.Add(NewStep._StepName, NewStep);

        _CurrentStep = NewStep;

        return this;
    }

    public NonEditorFluentCreator _Finalize()
    {
        return this;
    }
}

public class InnerStepDataU
{
    public string _StepName;
    //public string _PreviousStep;
    //public string _NextStep;
    public List<InnerFunctionDataU> _FunctionsDeclared = new List<InnerFunctionDataU>();
}
/// <summary>
/// Can there only be one per string, if a second one is detected it will complain.
/// </summary>
public class InnerFunctionDataU
{
    public bool IsEqualTo(InnerFunctionDataU Other)
    {
        
        return false;
    }
    public InnerStepDataU _ParentStep;
    public string _FunctionName;
    public string _ReturnType;
    public List<InnerArgumentsDataU> _Arguments = new List<InnerArgumentsDataU>();

    public string ToInterfaceString()
    {
        if (_Arguments.Count > 0)
        {
            string Arguments = "(";
            for (int i = 0; i < _Arguments.Count; i++)
            {
                Arguments += _Arguments[i]._ArgumentType + " " + _Arguments[i]._ArgumentName + ", ";
            }
            if (Arguments.Length > 2)
            {
                Arguments = Arguments.Remove(Arguments.Length - 2);
            }
            Arguments += ");";
            return _ReturnType + " " + _FunctionName + " " + Arguments;

        }
        else
        {
            return _ReturnType + " " + _FunctionName + " { get; }";
        }
    }

    public string ToImplementVirtual()
    {
        if (_Arguments.Count > 0)
        {
            string Arguments = "(";
            for (int i = 0; i < _Arguments.Count; i++)
            {
                Arguments += _Arguments[i]._ArgumentType + " " + _Arguments[i]._ArgumentName + ", ";
            }
            if (Arguments.Length > 2)
            {
                Arguments = Arguments.Remove(Arguments.Length - 2);
            }
            Arguments += ");";
            return "public abstract " + _ReturnType + " " + _FunctionName + " " + Arguments;
        }
        else
        {
            return "public abstract " + _ReturnType + " " + _FunctionName + " { get; }";
        }
    }

}

public class InnerArgumentsDataU
{
    public string _ArgumentType, _ArgumentName;
}


public interface IStepDeclareU
{
    IAddFluentFunctionsU _Step(string StepName);
}

public interface IAddFluentFunctionsU
{
    NonEditorFluentCreator _Finalize();
    IAddFunctionArgumentsU _Function(string FunctionName, string ReturnType);
    IStepDeclareU _FinishStep();
}

public interface IAddFunctionArgumentsU
{
    IAddFunctionArgumentsU _Argument(string FieldType, string FieldName);
    IAddFluentFunctionsU _FinishFunction();
}