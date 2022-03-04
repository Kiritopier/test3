using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using System.Linq;

public class NavTaskCodeGen : ICodeGenRueECS
{
    public void DeclareUsing(StrFor generator)
    {
        return;
        generator.AddUsing("RueECS");
        generator.AddUsing("System");
        generator.AddUsing("Sirenix.OdinInspector");
        generator.AddUsing("System.Collections");
        generator.AddUsing("UnityEngine");
        generator.AddUsing("System.Collections.Generic");
    }
    public void CodeGen(StrFor generator)
    {
        return;
        List<(Type, IsGameSystemAttribute)> data = new List<(Type, IsGameSystemAttribute)>();
        var typesWithMyAttribute =
                         from a in AppDomain.CurrentDomain.GetAssemblies()
                         from t in a.GetTypes()
                         let attributes = t.GetCustomAttributes(typeof(IsGameSystemAttribute), true)
                         where attributes != null && attributes.Length > 0
                         select new { Type = t, Attributes = attributes.Cast<IsGameSystemAttribute>() };
        foreach (var item in typesWithMyAttribute)
        {
            //for every type that has ingame system attribute, create the declaration
            IsGameSystemAttribute att = item.Attributes.First(); //first and only. 
                                                                 //based on the information on this attribute, create the boilerplate partial extended class of RueECSFlows which will create and tick all the systems properly.
            data.Add(ValueTuple.Create<Type, IsGameSystemAttribute>(item.Type, att));
        }
        //sort by name
        data.Sort((x, y) =>
        {
            return x.Item1.Name.CompareTo(y.Item1.Name);
        });
        //sort by priority
        data.Sort((x, y) =>
        {
            return y.Item2._Priority.CompareTo(x.Item2._Priority);
        });
        
        
        //declare all the system navegators
        for (int i = 0; i < data.Count; i++)
        {
            //if (data[i].Item2._IsLateStepper || data[i].Item2._IsStepper)
            {
                generator.NL("[Serializable] public class On_" + data[i].Item1.Name + ":TrueSys { public On_" + data[i].Item1.Name + "() { _SystemName = \"" + data[i].Item1.Name + "\"; _Towards = true; } public override void Do() { RueECSFlows.__Activate_" + data[i].Item1.Name + "();  } }");
                generator.NL("[Serializable] public class Off_" + data[i].Item1.Name + ":TrueSys { public Off_" + data[i].Item1.Name + "() { _SystemName = \"" + data[i].Item1.Name + "\"; _Towards = false; } public override void Do() { RueECSFlows.__Deactivate_" + data[i].Item1.Name + "(); } }");
                //generator.NL("public class Toggler" + data[i].Item1.Name + ":ToggleSystemRoutine { public Toggler" + data[i].Item1.Name + "() { _SystemName = \"" + data[i].Item1.Name + "\"; } public override IEnumerator Flow() { if (_TurnTheSystem) { RueECSFlows.__Activate_" + data[i].Item1.Name + "(); } else { RueECSFlows.__Deactivate_" + data[i].Item1.Name + "(); } yield break; } }");
            }
        }

        generator.NL("namespace RueECS");
        generator.OpenFunction();
        //declare the ruenav partial class
        generator.NL("public partial class NavTask");
        generator.OpenFunction();
        for (int i = 0; i < data.Count; i++)
        {
            //if (data[i].Item2._IsLateStepper || data[i].Item2._IsStepper)
            {
                //string FFFF = data[i].Item1.Name;
                //FFFF = FFFF.Replace("_", "");
                //generator.NL("[FoldoutGroup(\"Systems\")]");
                //generator.NL("[ResponsiveButtonGroup(\"Systems/act\")]");
                //generator.NL("[Button(\"" + FFFF + "\")]");
                //generator.NL("public void AddToggler" + data[i].Item1.Name + "() { this._FlowOrder.Add(new Toggler" + data[i].Item1.Name + "()); }");
            }
            {
                string FFFF = data[i].Item1.Name;
                FFFF = FFFF.Replace("_", "");
                generator.NL("[FoldoutGroup(\"OnSystems\")]");
                generator.NL("[ResponsiveButtonGroup(\"OnSystems/act\")]");
                generator.NL("[Button(\"" + FFFF + "\")]");
                generator.NL("[DisableIf(\"HasOn"+FFFF+"\")]");
                generator.NL("[GUIColor(\"ColorOn" + FFFF + "\")]");
                //public void AddOffEntityLinkSystem() { if (SimpleHasOff()) { RemoveOff(); } else { this._FlowOrder.Add(new Off_EntityLinkSystem()); } }
                generator.NL("public void AddOn" + data[i].Item1.Name + "() { if(SimpleHasOn"+FFFF+"()) { RemoveOn"+FFFF+"(); } else { this._FlowActions.Add(new On_" + data[i].Item1.Name + "()); } }");
                generator.NL("public bool HasOn" + FFFF + "() { if ( this._FlowActions == null){ this._FlowActions = new List<TrueSys>(); } for (int i = 0; i < this._FlowActions.Count; i++) { if(this._FlowActions[i] is Off_" + data[i].Item1.Name + ") { return true; } } return false; }");
                generator.NL("public Color ColorOn" + FFFF + "() { if ( this._FlowActions == null){ this._FlowActions = new List<TrueSys>(); } for (int i = 0; i < this._FlowActions.Count; i++) { if (this._FlowActions[i] is On_" + data[i].Item1.Name + ") { return Color.green; } } return Color.white; }");
                //bool SimpleHasOff() { for (int i = 0; i < this._FlowOrder.Count; i++) { if (this._FlowOrder[i] is Off_EntityLinkSystem) { return true; } } return false; }
                generator.NL("public bool SimpleHasOn"+ FFFF + "() { if ( this._FlowActions == null){ this._FlowActions = new List<TrueSys>(); } for (int i = 0; i < this._FlowActions.Count; i++) { if(this._FlowActions[i] is On_" + data[i].Item1.Name + ") { return true; } } return false; }");
                //public Color ColorOffEntityLinkSystem() { for (int i = 0; i < this._FlowOrder.Count; i++) { if (this._FlowOrder[i] is Off_EntityLinkSystem) { return Color.green; } } return Color.white; }
                //void RemoveOff() { for (int i = 0; i < this._FlowOrder.Count; i++) { if (this._FlowOrder[i] is Off_EntityLinkSystem) { this._FlowOrder.RemoveAt(i); break; } } }
                generator.NL("void RemoveOn"+FFFF+ "() { if ( this._FlowActions == null){ this._FlowActions = new List<TrueSys>(); } for (int i = 0; i < this._FlowActions.Count; i++) { if (this._FlowActions[i] is On_" + data[i].Item1.Name + ") { this._FlowActions.RemoveAt(i); break; } } }");
            }
            {
                string FFFF = data[i].Item1.Name;
                FFFF = FFFF.Replace("_", "");
                generator.NL("[FoldoutGroup(\"OffSystems\")]");
                generator.NL("[ResponsiveButtonGroup(\"OffSystems/act\")]");
                generator.NL("[Button(\"" + FFFF + "\")]");
                generator.NL("[DisableIf(\"HasOff" + FFFF + "\")]");
                generator.NL("[GUIColor(\"ColorOff" + FFFF + "\")]");
                generator.NL("public void AddOff" + data[i].Item1.Name + "() { if(SimpleHasOff"+FFFF+"()) { RemoveOff"+FFFF+ "(); } else { this._FlowActions.Add(new Off_" + data[i].Item1.Name + "()); } }");
                generator.NL("public bool HasOff" + FFFF + "() { if ( this._FlowActions == null){ this._FlowActions = new List<TrueSys>(); } for (int i = 0; i < this._FlowActions.Count; i++) { if(this._FlowActions[i] is On_" + data[i].Item1.Name + ") { return true; } } return false; }");
                generator.NL("public Color ColorOff" + FFFF + "() { if ( this._FlowActions == null){ this._FlowActions = new List<TrueSys>(); } for (int i = 0; i < this._FlowActions.Count; i++) { if (this._FlowActions[i] is Off_" + data[i].Item1.Name + ") { return Color.red; } } return Color.white; }");
                generator.NL("public bool SimpleHasOff" + FFFF + "() { if ( this._FlowActions == null){ this._FlowActions = new List<TrueSys>(); } for (int i = 0; i < this._FlowActions.Count; i++) { if(this._FlowActions[i] is Off_" + data[i].Item1.Name + ") { return true; } } return false; }");
                generator.NL("void RemoveOff" + FFFF + "() { if ( this._FlowActions == null){ this._FlowActions = new List<TrueSys>(); } for (int i = 0; i < this._FlowActions.Count; i++) { if (this._FlowActions[i] is Off_" + data[i].Item1.Name + ") { this._FlowActions.RemoveAt(i); break; } } }");


            }
        }
        generator.CloseFunction();
        generator.CloseFunction();
    }
}