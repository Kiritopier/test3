using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class SimpleRueECSGenerator: SerializedMonoBehaviour
{
    [Button]
    public void BuildSourceAndTool()
    {
        var alldatafromsource = ExtendedRueECSFrameWorkGenerator.CreateAndGetBackup();//this gets all the stuff I use all the time, already packaged.
        //now use reflection to find all the components declarations and add them to the declarations from the other types of definitions




        ExtendedRueECSFrameWorkGenerator g = new ExtendedRueECSFrameWorkGenerator();
        g.CreateFiles(alldatafromsource);
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

    }

    public static void CreateFiles(
        Dictionary<string, DeclaringCollection> RepeatedCollections
       , List<DeclaringComponent> ParsedAllComponents,
        List<(Type, IsGameSystemAttribute)> Organized,
        List<string> CollectionNames, List<DeclaringEntity> AllEntities,
        List<DeclaringCollection> AllCollections,
        bool IsDebug)
    {

        Dictionary<string, DeclaringComponent> _ComponentDictionaryByName = new Dictionary<string, DeclaringComponent>();
        List<DeclaringComponent> _AllComponents = new List<DeclaringComponent>();
        List<DeclaringEntity> _AllEntities = new List<DeclaringEntity>();
        List<DeclaringCollection> _AllCollections = new List<DeclaringCollection>();

        ParsedAllComponents.Sort((x, y) =>
        {
            return x._ComponentName.CompareTo(y._ComponentName);
        });
        RepeatedCollections = new Dictionary<string, DeclaringCollection>(RepeatedCollections); //this is funny
        _ComponentDictionaryByName.Clear();
        _AllEntities.Clear();
        _AllEntities.AddRange(AllEntities);
        _AllCollections.Clear();
        _AllCollections.AddRange(AllCollections);
        _AllCollections.Sort((x, y) =>
        {
            return x._CollectionName.CompareTo(y._CollectionName);
        });
        Dictionary<DeclaringCollection, List<DeclaringEntity>> EntitiesBelongingTo = new Dictionary<DeclaringCollection, List<DeclaringEntity>>();
     
        {//flows and systems
            string DataPath = Application.dataPath + "/ECSFlowDeclarations.cs";
            string final = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataPath);

            //Directory.Delete(final, true);
            using (StreamWriter W = new StreamWriter(final, false))
            {
                StrFor F = new StrFor(W);
                F.NL("using System;");
                F.NL("using System.Collections.Generic;");
                F.NL("using UnityEngine;");
                //F.NL("using Sirenix.OdinInspector;");
                F.NL("using System.Collections;");



                F.NL("namespace RueECS");
                F.OpenFunction();
                F.NL("public partial class RueECSFlows");
                F.OpenFunction();
                for (int i = 0; i < Organized.Count; i++)
                {
                    var T = Organized[i].Item1;
                    var S = Organized[i].Item2;
                    F.NL("public static " + T.Name + " _" + T.Name + ";");

                }




                F.NL("static partial void SetupAll()");
                F.OpenFunction();
                for (int i = 0; i < Organized.Count; i++)
                {
                    var T = Organized[i].Item1;
                    var S = Organized[i].Item2;
                    F.NL("_" + T.Name + " = new " + T.Name + "();");

                }

                for (int i = 0; i < Organized.Count; i++)
                {
                    var T = Organized[i].Item1;
                    var methods = T.GetMethods();
                    bool containsmethod = false;
                    for (int a = 0; a < methods.Length; a++)
                    {
                        if (methods[a].Name == "PostInstantiation")
                        {
                            containsmethod = true;
                            break;
                        }
                    }
                    if (containsmethod)
                        F.NL("_" + T.Name + ".PostInstantiation();");

                }

                #region Multipostinstan
                for (int l = 0; l < 100; l++)
                {
                    for (int i = 0; i < Organized.Count; i++)
                    {
                        var T = Organized[i].Item1;
                        var methods = T.GetMethods();
                        bool containsmethod = false;
                        for (int a = 0; a < methods.Length; a++)
                        {
                            if (methods[a].Name == "PostInstantiation" + l.ToString())
                            {
                                containsmethod = true;
                                break;
                            }
                        }
                        if (containsmethod)
                            F.NL("_" + T.Name + ".PostInstantiation" + l.ToString() + "();");

                    }
                }


                #endregion


                for (int i = 0; i < Organized.Count; i++)
                {
                    var T = Organized[i].Item1;
                    var methods = T.GetMethods();
                    bool containsmethod = false;
                    for (int a = 0; a < methods.Length; a++)
                    {
                        if (methods[a].Name == "EndSetup")
                        {
                            containsmethod = true;
                            break;
                        }
                    }
                    if (containsmethod)
                        F.NL("_" + T.Name + ".EndSetup();");

                }


                F.CloseFunction();

                if (IsDebug)
                {
                    for (int i = 0; i < Organized.Count; i++)
                    {
                        var T = Organized[i].Item1;
                        var methods = T.GetMethods();
                        bool containsmethod = false;
                        for (int a = 0; a < methods.Length; a++)
                        {
                            if (methods[a].Name == "Stepper")
                            {
                                containsmethod = true;
                                break;
                            }
                        }
                        bool containsproperty = false;
                        var props = T.GetProperties();
                        for (int a = 0; a < props.Length; a++)
                        {
                            if (props[a].Name == "ShouldRun")
                            {
                                containsproperty = true;
                            }
                        }
                        if (containsmethod && containsproperty)
                        {

                            F.NL("public static float " + T.Name + "ElapsedTime = 0.0f;");

                        }

                    }

                    for (int i = 0; i < Organized.Count; i++)
                    {
                        var T = Organized[i].Item1;
                        var methods = T.GetMethods();
                        bool containsmethod = false;
                        for (int a = 0; a < methods.Length; a++)
                        {
                            if (methods[a].Name == "LateStepper")
                            {
                                containsmethod = true;
                                break;
                            }
                        }
                        bool containsproperty = false;
                        var props = T.GetProperties();
                        for (int a = 0; a < props.Length; a++)
                        {
                            if (props[a].Name == "ShouldRunLate")
                            {
                                containsproperty = true;
                            }
                        }
                        if (containsmethod && containsproperty)
                        {
                            F.NL("public static float " + T.Name + "LateElapsedTime = 0.0f;");
                        }


                    }

                }


                F.NL("static partial void Step()");
                F.OpenFunction();

                if (IsDebug)
                {
                    F.NL("float S = 0.0f;");
                }

                for (int i = 0; i < Organized.Count; i++)
                {
                    var T = Organized[i].Item1;
                    var methods = T.GetMethods();
                    bool containsmethod = false;
                    for (int a = 0; a < methods.Length; a++)
                    {
                        if (methods[a].Name == "Stepper")
                        {
                            containsmethod = true;
                            break;
                        }
                    }
                    bool containsproperty = false;
                    var props = T.GetProperties();
                    for (int a = 0; a < props.Length; a++)
                    {
                        if (props[a].Name == "ShouldRun")
                        {
                            containsproperty = true;
                        }
                    }
                    if (containsmethod && containsproperty)
                    {
                        if (IsDebug)
                        {
                            F.NL("S = Time.realtimeSinceStartup;");
                            F.NL("if (_" + T.Name + ".ShouldRun) { _" + T.Name + ".Stepper(); } ");
                            F.NL(T.Name + "ElapsedTime = (Time.realtimeSinceStartup - S)*1000f;");
                        }
                        else
                        {
                            F.NL("if (_" + T.Name + ".ShouldRun) { _" + T.Name + ".Stepper(); } ");
                        }

                    }

                }

                for (int i = 0; i < Organized.Count; i++)
                {
                    var T = Organized[i].Item1;
                    var methods = T.GetMethods();
                    bool containsmethod = false;
                    for (int a = 0; a < methods.Length; a++)
                    {
                        if (methods[a].Name == "LateStepper")
                        {
                            containsmethod = true;
                            break;
                        }
                    }
                    bool containsproperty = false;
                    var props = T.GetProperties();
                    for (int a = 0; a < props.Length; a++)
                    {
                        if (props[a].Name == "ShouldRunLate")
                        {
                            containsproperty = true;
                        }
                    }
                    if (containsmethod && containsproperty)
                    {
                        if (IsDebug)
                        {
                            F.NL("S = Time.realtimeSinceStartup;");
                            F.NL("if (_" + T.Name + ".ShouldRunLate) { _" + T.Name + ".LateStepper(); } ");
                            F.NL(T.Name + "LateElapsedTime = (Time.realtimeSinceStartup - S)*1000f;");
                        }
                        else
                        {
                            F.NL("if (_" + T.Name + ".ShouldRunLate) { _" + T.Name + ".LateStepper(); } ");
                        }
                    }


                }
                F.NL("BaseEntity.KillAll();");
                F.CloseFunction();

                F.CloseFunction();

                F.CloseFunction();
                #region //old
                {
                    /*
                    #region Flow declaration
                    //declare all the flows, and all the systems here, based on reflection


                    F.NL("public static class SystemInnateExtensions");
                    F.OpenFunction();
                    for (int i = 0; i < Organized.Count; i++)
                    {
                        var T = Organized[i].Item1;
                        var S = Organized[i].Item2;
                        if (T == null || S == null) { continue; }
                        F.NL("public static bool IsActive(this " + T.Name + " e) { return RueECSFlows._Flows._" + T.Name + "Active; }");
                        F.NL("public static void SetActive(this " + T.Name + " e, bool Towards) { if(Towards) { RueECSFlows.__Activate_" + T.Name + "(); } else { RueECSFlows.__Deactivate_" + T.Name + "(); } }");
                        //F.NL("public static " + T.Name +" (this " + T.Name + " e) { return RueECSFlows._Flows._" + T.Name + "Active; }");
                    }
                    F.CloseFunction();


                    //declare the flows
                    F.NL("public partial class RueECSFlows");
                    F.OpenFunction();
                    for (int i = 0; i < Organized.Count; i++)
                    {
                        var T = Organized[i].Item1;
                        var S = Organized[i].Item2;
                        F.NL("public static " + T.Name + " _" + T.Name + ";");
                        //if (S._IsStepper || S._IsLateStepper)
                        //{
                        F.NL("public bool _" + T.Name + "Active = false;");//" + S._InitialState.ToString().ToLower() + ";");

                        if (S._OnActivated)
                        {
                            F.NL("public static void __Activate_" + T.Name + "() { if(!_Flows._" + T.Name + "Active) {  RueECSFlows._" + T.Name + ".Activated(); } _Flows._" + T.Name + "Active = true; }");
                        }
                        else
                        {
                            F.NL("public static void __Activate_" + T.Name + "() { _Flows._" + T.Name + "Active = true; }");
                        }
                        if (S._OnDeactivated)
                        {
                            F.NL("public static void __Deactivate_" + T.Name + "() { if(_Flows._" + T.Name + "Active) {  RueECSFlows._" + T.Name + ".Deactivated(); } _Flows._" + T.Name + "Active = false; }");
                        }
                        else
                        {
                            F.NL("public static void __Deactivate_" + T.Name + "() { _Flows._" + T.Name + "Active = false; }");
                        }
                        //}
                    }
                    for (int i = 0; i < Organized.Count; i++)
                    {
                        var T = Organized[i].Item1;
                        F.NL("private " + T.Name + " _Local" + T.Name + ";");
                    }
                    F.NL("static partial void SetupAll()");
                    F.OpenFunction();
                    for (int i = 0; i < Organized.Count; i++)
                    {
                        var T = Organized[i].Item1;
                        var S = Organized[i].Item2;
                        F.NL("_" + T.Name + " = new " + T.Name + "();");
                        //if (S._InitialState && S._OnActivated)
                        {
                            //F.NL("_" + T.Name + ".Activated();");
                        }
                        F.NL("RueECSFlows._Flows._Local" + T.Name + " = " + "_" + T.Name + ";");
                    }
                    F.CloseFunction();
                    //var AllFlows = Enum.GetValues(typeof(FlowID)).Cast<FlowID>();

                    F.NL("public static void DeactivateAllSystems()");
                    F.OpenFunction();
                    for (int i = 0; i < Organized.Count; i++)
                    {
                        var T = Organized[i].Item1;

                        F.NL("__Deactivate_" + T.Name + "();");
                    }
                    F.CloseFunction();

                    F.NL("static partial void Step()");
                    F.OpenFunction();
                    for (int i = 0; i < Organized.Count; i++)
                    {
                        var T = Organized[i].Item1;
                        var S = Organized[i].Item2;
                        if (S._IsStepper)
                        {
                            F.NL("if ( _Flows._" + T.Name + "Active ) { _" + T.Name + ".Stepper(); } ");
                        }
                    }
                    for (int i = 0; i < Organized.Count; i++)
                    {
                        var T = Organized[i].Item1;
                        var S = Organized[i].Item2;
                        if (S._IsLateStepper)
                        {
                            F.NL("if ( _Flows._" + T.Name + "Active ) { _" + T.Name + ".LateStepper(); } ");
                        }
                    }
                    F.NL("BaseEntity.KillAll();");
                    F.CloseFunction();

                    F.CloseFunction();

                    #endregion
                    */
                }
                #endregion
                W.Flush();
                W.Close();
            }
        }
        {//components
            string DataPath = Application.dataPath + "/ECSComponentDeclarations.cs";
            string final = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataPath);
            //Directory.Delete(final);
            using (StreamWriter W = new StreamWriter(final, false))
            {
                StrFor F = new StrFor(W);
                F.NL("using System;");
                F.NL("using System.Collections.Generic;");
                F.NL("using UnityEngine;");
                F.NL("using System.Collections;");
                F.NL("using Sirenix.OdinInspector;");
                F.NL("namespace RueECS");
                F.OpenFunction();
                {
                    //F.NL("public static class CompsCount{ static int InnerTick = 0; public static int UniqueTick { get { if (InnerTick + 1 >= int.MaxValue) { InnerTick = -1; } return InnerTick++; } } }");
                    #region components declaration


                    for (int i = 0; i < ParsedAllComponents.Count - 1; i++)
                    {
                        string UniqueName = ParsedAllComponents[i]._ComponentName;
                        for (int a = i + 1; a < ParsedAllComponents.Count; a++)
                        {
                            string CompareTo = ParsedAllComponents[a]._ComponentName;
                            if (UniqueName == CompareTo)
                            {
                                Debug.LogError("Component declared multiple times: " + UniqueName);
                                ParsedAllComponents.RemoveAt(a);
                                a--;
                            }
                        }
                    }






                    for (int i = 0; i < ParsedAllComponents.Count; i++)
                    {
                        DeclaringComponent declaring = ParsedAllComponents[i];
                        _ComponentDictionaryByName.Add(declaring._ComponentName, declaring);
                        string ComponentName = declaring._ComponentName;

                        //_AllEntities.Add(new DeclaringEntity() { _EntityName = ComponentName + "_Ent", _Components = new List<string> { ComponentName } });

                        //F.NL("public static partial class Ko");
                        //F.OpenFunction();
                        //F.NL("public static class " + ComponentName);
                        //F.OpenFunction();
                        //F.NL("public static Type T() { return typeof(RueECS." + ComponentName + "); }");
                        //F.CloseFunction();
                        //F.CloseFunction();

                        //F.NL("public partial class BaseEntity");
                        //F.OpenFunction();
                        ////F.NL("public static "+ ComponentName + " Get"+ ComponentName + "(int id) { var Ent = BaseEntity._Pool[id]; if (Ent.Has"+ ComponentName + "()) { return (Ent as I"+ComponentName+").Get"+ ComponentName + "(); } else { return null; } }");
                        ////F.NL("public static " + ComponentName + " Get" + ComponentName + "(IRueEntity id) { var Ent = BaseEntity._Pool[id.UniqueID()]; if (Ent.Has" + ComponentName + "()) { return (Ent as I" + ComponentName + ").Get" + ComponentName + "(); } else { return null; } }");
                        //F.CloseFunction();
                        //F.NL("public interface I" + ComponentName);
                        //F.OpenFunction();
                        //F.NL(ComponentName + " Get" + ComponentName + "();");
                        //F.CloseFunction();

                        F.NL("public class " + ComponentName + " : IRueComp");
                        F.OpenFunction();
                        //F.NL("public const int ID = " + i + ";");
                        // F.NL("public static List<" + ComponentName + "> _Orderly = new List<"+ComponentName+">();");
                        //F.NL("int UniqueID; public int GetUniqueID() { return UniqueID; }");
                        F.NL("BaseEntity _Owner; public BaseEntity GetOwner() { return _Owner; }");
                        F.NL(ComponentName + "(BaseEntity With) { _Owner = With; }");
                        //F.NL("public " + ComponentName + "() { _Owner = null; }");
                        F.NL("static Queue<" + ComponentName + "> _Pool = new Queue<" + ComponentName + ">();");
                        F.NL("public static " + ComponentName + " Make(BaseEntity With)");
                        F.OpenFunction();
                        F.NL("if(_Pool.Count > 0)");
                        F.OpenFunction();
                        F.NL(ComponentName + " Comp = _Pool.Dequeue();");
                        //for (int a = 0; a < declaring._AllGathered.Count; a++)
                        //{
                        //   // if (declaring._AllGathered[a]._IsHidden) { continue; }
                        //    string FieldType = declaring._AllGathered[a]._Type;
                        //    string FieldName = declaring._AllGathered[a]._Name;
                        //    if(declaring._AllGathered[a]._IsComplexObserver || declaring._AllGathered[a]._IsSimpleObserver)
                        //    F.NL("Comp.__" + FieldName + "Changed.Clear();");
                        //    if(declaring._AllGathered[a]._IsModifyable)
                        //    F.NL("Comp.__" + FieldName + "Modifiers.Clear();");
                        //}
                        F.NL("Comp._Owner = With;");
                        F.NL("return Comp;");
                        F.CloseFunction();
                        F.NL("return new " + ComponentName + "(With);");
                        F.CloseFunction();
                        F.NL("public void ReturnToPool()");
                        F.OpenFunction();
                        for (int a = 0; a < declaring._AllGathered.Count; a++)
                        {
                            // if (declaring._AllGathered[a]._IsHidden) { continue; }
                            string FieldType = declaring._AllGathered[a]._Type;
                            string FieldName = declaring._AllGathered[a]._Name;
                            if (declaring._AllGathered[a]._IsComplexObserver || declaring._AllGathered[a]._IsSimpleObserver)
                                F.NL("__" + FieldName + "Changed.Clear();");
                            if (declaring._AllGathered[a]._IsModifyable)
                                F.NL("__" + FieldName + "Modifiers.Clear();");

                            F.NL("_" + FieldName + " = default(" + FieldType + ");");
                        }
                        F.NL("_Pool.Enqueue(this);");
                        F.CloseFunction();

                        /*for (int a = 0; a < declaring._VariableNames.Count; a++)
                        {
                            string FieldType = declaring._VariablesTypes[a];
                            string FieldName = declaring._VariableNames[a];
                            if (declaring._IsHidden[a])
                            {
                                F.NL("public List<Action<IRueEntity, " + FieldType + ", " + FieldType + ">> __" + FieldName + 
                                    "Changed = new List<Action<IRueEntity, " + FieldType + ", " + FieldType + ">>(); " + FieldType 
                                    + " " + FieldName + " = default(" + FieldType + "); public " + FieldType + " _"
                                    + FieldName + " { get { return " + FieldName +
                                    "; } set { for (int i = 0; i < __" + FieldName + "Changed.Count; i++) {  __" + FieldName + "Changed[i].Invoke(_Owner, " + FieldName + ", value); } " + FieldName + " = value; } }");
                            }
                            else
                            {
                                F.NL("public List<Action<IRueEntity, " + FieldType + ", " + FieldType + ">> __" + FieldName 
                                    + "Changed = new List<Action<IRueEntity, " + FieldType + ", " + FieldType + ">>(); " + FieldType + " "
                                    + FieldName + "; public " + FieldType + " _" + FieldName + " { get { return " 
                                    + FieldName + "; } set {  for (int i = 0; i < __" + FieldName + "Changed.Count; i++) {  __" + FieldName + "Changed[i].Invoke(_Owner, " + FieldName + ", value); } " + FieldName + " = value; } }");
                            }
                        }*/

                        for (int a = 0; a < declaring._AllGathered.Count; a++)
                        {
                            string FieldType = declaring._AllGathered[a]._Type;
                            string FieldName = declaring._AllGathered[a]._Name;
                            bool IsSimple = declaring._AllGathered[a]._IsSimpleObserver;
                            bool IsComplex = declaring._AllGathered[a]._IsComplexObserver;
                            bool IsMod = declaring._AllGathered[a]._IsModifyable;

                            if (IsComplex)
                            {
                                F.NL("public void Refresh" + FieldName + "() { _" + FieldName + " = " + FieldName + "; }");
                                if (IsMod)
                                {
                                    F.NL("public " + FieldType + " Unmodified" + FieldName + " { get { return " + FieldName + "; } }");
                                    F.NL("public List<Func<" + FieldType + "," + FieldType + ">> __" + FieldName + "Modifiers = new List<Func<" + FieldType + "," + FieldType + ">>();");
                                }
                                F.NL("public delegate void " + FieldName + "ChangedDelegate ( BaseEntity ent, " + FieldType + " prev, " + FieldType + " now);");
                                F.NL("public List<" + FieldName + "ChangedDelegate> __" + FieldName + "Changed = new List<" + FieldName + "ChangedDelegate>();");
                                F.NL(FieldType + " " + FieldName + " = default(" + FieldType + ");");
                                F.NL("public " + FieldType + " _" + FieldName);
                                F.OpenFunction();
                                F.NL("get");
                                F.OpenFunction();
                                if (IsMod)
                                {
                                    F.NL("if ( __" + FieldName + "Modifiers.Count == 0) { return " + FieldName + "; } var Temp = " + FieldName + "; for (int i = 0; i < __" + FieldName + "Modifiers.Count; i++) { Temp = __" + FieldName + "Modifiers[i](Temp); } return Temp;");
                                }
                                else
                                {
                                    F.NL(" return " + FieldName + ";");
                                }
                                F.CloseFunction();
                                F.NL("set");
                                F.OpenFunction();
                                F.NL("if( __" + FieldName + "Changed.Count > 0) { " + FieldType + " _Temp_ = _" + FieldName + "; " + FieldName + " = value; var ___NewVal = _" + FieldName + "; for (int i = 0; i < __" + FieldName + "Changed.Count; i++) {  __" + FieldName + "Changed[i].Invoke(_Owner, _Temp_, ___NewVal); } } else { " + FieldName + " = value; }");
                                F.CloseFunction();
                                F.CloseFunction();


                            }
                            else if (IsSimple)
                            {
                                F.NL("public void Refresh" + FieldName + "() { _" + FieldName + " = " + FieldName + "; }");
                                if (IsMod)
                                {
                                    F.NL("public " + FieldType + " Unmodified" + FieldName + " { get { return " + FieldName + "; } }");
                                    F.NL("public List<Func<" + FieldType + "," + FieldType + ">> __" + FieldName + "Modifiers = new List<Func<" + FieldType + "," + FieldType + ">>();");
                                }
                                F.NL("public List<Action> __" + FieldName + "Changed = new List<Action>();");
                                F.NL(FieldType + " " + FieldName + " = default(" + FieldType + ");");
                                F.NL("public " + FieldType + " _" + FieldName);
                                F.OpenFunction();
                                F.NL("get");
                                F.OpenFunction();
                                if (IsMod)
                                {
                                    F.NL("if ( __" + FieldName + "Modifiers.Count == 0) { return " + FieldName + "; } var Temp = " + FieldName + "; for (int i = 0; i < __" + FieldName + "Modifiers.Count; i++) { Temp = __" + FieldName + "Modifiers[i](Temp); } return Temp;");
                                }
                                else
                                {
                                    F.NL(" return " + FieldName + ";");
                                }
                                F.CloseFunction();
                                F.NL("set");
                                F.OpenFunction();
                                F.NL(FieldName + " = value;");
                                F.NL(" if ( __" + FieldName + "Changed.Count > 0)");
                                F.OpenFunction();
                                F.NL("for (int i = 0; i < __" + FieldName + "Changed.Count; i++) {  __" + FieldName + "Changed[i].Invoke(); }");
                                F.CloseFunction();
                                F.CloseFunction();
                                F.CloseFunction();

                            }
                            else
                            {
                                if (!IsMod)
                                {
                                    F.NL("public " + FieldType + " _" + FieldName + " = default(" + FieldType + ");");
                                }
                                else
                                {
                                    F.NL("public " + FieldType + " Unmodified" + FieldName + " { get { return " + FieldName + "; } }");
                                    F.NL("public List<Func<" + FieldType + "," + FieldType + ">> __" + FieldName + "Modifiers = new List<Func<" + FieldType + "," + FieldType + ">>();");


                                    F.NL(FieldType + " " + FieldName + " = default(" + FieldType + ");");

                                    F.NL("public " + FieldType + " _" + FieldName);
                                    F.OpenFunction();
                                    F.NL("get");
                                    F.OpenFunction();

                                    F.NL("if ( __" + FieldName + "Modifiers.Count == 0) { return " + FieldName + "; } var Temp = " + FieldName + "; for (int i = 0; i < __" + FieldName + "Modifiers.Count; i++) { Temp = __" + FieldName + "Modifiers[i](Temp); } return Temp;");

                                    F.CloseFunction();
                                    F.NL("set");
                                    F.OpenFunction();
                                    F.NL(FieldName + " = value;");
                                    F.CloseFunction();
                                    F.CloseFunction();
                                }






                            }


                            // F.NL("public void Refresh" + FieldName + "() { _" + FieldName + " = " + FieldName + "; }");
                            // F.NL("public " + FieldType + " Unmodified" + FieldName + " { get { return " + FieldName + "; } }");
                            //     F.NL("public List<Func<" + FieldType + "," + FieldType + ">> __" + FieldName + "Modifiers = new List<Func<"+FieldType+","+FieldType + ">>();");
                            //     F.NL("public List<Action<BaseEntity, " + FieldType + ", " + FieldType + ">> __" + FieldName +
                            //         "Changed = new List<Action<BaseEntity, " + FieldType + ", " + FieldType + ">>(); " + FieldType
                            //         + " " + FieldName + " = default(" + FieldType + "); public " + FieldType + " _"
                            //         + FieldName + " { get { if ( __"+FieldName+"Modifiers.Count == 0) { return "+FieldName+"; } var Temp = "+FieldName+"; for (int i = 0; i < __"+FieldName+"Modifiers.Count; i++) { Temp = __"+FieldName+"Modifiers[i](Temp); } return Temp; "+
                            //         " } set { if( __"+FieldName+"Changed.Count > 0) { " + FieldType + " _Temp_ = _" + FieldName + "; " + FieldName + " = value; var ___NewVal = _"+FieldName+"; for (int i = 0; i < __" + FieldName + "Changed.Count; i++) {  __" + FieldName + "Changed[i].Invoke(_Owner, _Temp_, ___NewVal); } } else { "+FieldName+" = value; } } }");

                        }

                        F.CloseFunction();
                    }

                    #endregion
                }
                F.CloseFunction();

                F.NL("public partial class DeclaringEntity");
                F.OpenFunction();
                for (int i = 0; i < ParsedAllComponents.Count; i++)
                {
                    string UniqueName = ParsedAllComponents[i]._ComponentName;
                    F.NL("[FoldoutGroup(\"Components\")]");
                    F.NL("[Button]");
                    F.NL("[BoxGroup(\"Components/C\")]");
                    F.NL("[ResponsiveButtonGroup(\"Components/C/Arranged\")]");
                    F.NL("[GUIColor(\"Color" + UniqueName + "\")]");
                    //F.NL("[ResponsiveButtonGroup(\"Comps\")]");
                    F.NL("public void _" + UniqueName + "()");
                    F.NL("{ if (_Components.Contains(\"" + UniqueName + "\")) { _Components.Remove(\"" + UniqueName + "\"); } else { { _Components.Add(\"" + UniqueName + "\"); } } }");
                    F.NL("public Color Color" + UniqueName + "() { if (_Components.Contains(\"" + UniqueName + "\")) { return Color.green; } else { return Color.gray; } }");
                }
                F.CloseFunction();

                F.NL("public partial class DeclaringCollection");
                F.OpenFunction();
                for (int i = 0; i < ParsedAllComponents.Count; i++)
                {
                    string UniqueName = ParsedAllComponents[i]._ComponentName;
                    F.NL("[FoldoutGroup(\"All\")]");
                    F.NL("[FoldoutGroup(\"All/InComponents\")]");
                    F.NL("[Button]");
                    F.NL("[BoxGroup(\"All/InComponents/C\")]");
                    F.NL("[ResponsiveButtonGroup(\"All/InComponents/C/Arranged\")]");
                    F.NL("[GUIColor(\"ColorIn" + UniqueName + "\")]");
                    F.NL("public void " + UniqueName + "In()");
                    F.NL("{ if (_Components.Contains(\"" + UniqueName + "\")) { _Components.Remove(\"" + UniqueName + "\"); } else { { _Components.Add(\"" + UniqueName + "\"); _NoComponents.Remove(\"" + UniqueName + "\"); } } }");
                    F.NL("public Color ColorIn" + UniqueName + "() { if (_Components.Contains(\"" + UniqueName + "\")) { return Color.green; } else { if(_NoComponents.Contains(\"" + UniqueName + "\")) { return Color.red; } return Color.gray; } }");

                    F.NL("[FoldoutGroup(\"All\")]");
                    F.NL("[FoldoutGroup(\"All/OutComponents\")]");
                    F.NL("[Button]");
                    F.NL("[BoxGroup(\"All/OutComponents/C\")]");
                    F.NL("[ResponsiveButtonGroup(\"All/OutComponents/C/Arranged\")]");
                    F.NL("[GUIColor(\"ColorOut" + UniqueName + "\")]");
                    F.NL("public void " + UniqueName + "Out()");
                    F.NL("{ if (_NoComponents.Contains(\"" + UniqueName + "\")) { _NoComponents.Remove(\"" + UniqueName + "\"); } else { { _NoComponents.Add(\"" + UniqueName + "\"); _Components.Remove(\"" + UniqueName + "\"); } } }");
                    F.NL("public Color ColorOut" + UniqueName + "() { if (_NoComponents.Contains(\"" + UniqueName + "\")) { return Color.yellow; } else { if (_Components.Contains(\"" + UniqueName + "\")) { return Color.blue; } return Color.gray; } }");
                }
                F.CloseFunction();

                W.Flush();
                W.Close();
            }
        }
        {//Collections
            string DataPath = Application.dataPath + "/ECSCollectionsDeclarations.cs";
            string final = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataPath);
            using (StreamWriter W = new StreamWriter(final, false))
            {
                StrFor F = new StrFor(W);
                F.NL("using System;");
                F.NL("using System.Collections.Generic;");
                F.NL("using UnityEngine;");
                F.NL("using System.Collections;");
                F.NL("namespace RueECS");
                F.OpenFunction();
                {
                    #region Collection declaration

                    for (int i = 0; i < ParsedAllComponents.Count; i++)
                    {

                        DeclaringCollection ColFound = new DeclaringCollection()
                        {
                            _Components = new List<string>() { ParsedAllComponents[i]._ComponentName },
                            //_CollectionName = ParsedAllComponents[i]._ComponentName + "Collection"
                        };
                        string nams = ColFound.ID();

                        if (RepeatedCollections.ContainsKey(nams)) { continue; }
                        RepeatedCollections.Add(nams, ColFound);

                    }



                    for (int i = 0; i < _AllCollections.Count; i++)
                    {
                        string CollectionName = _AllCollections[i].ID();
                        DeclaringCollection ColFound = null;
                        if (!RepeatedCollections.TryGetValue(CollectionName, out ColFound))
                        {
                            RepeatedCollections.Add(CollectionName, _AllCollections[i]);
                        }
                        else
                        {
                            if (ColFound._CollectionName != _AllCollections[i]._CollectionName)
                                ColFound._Aliases.Add(_AllCollections[i]._CollectionName);
                        }
                    }

                    foreach (var col in RepeatedCollections.Values)
                    {
                        var Collection = col;
                        var ComponentsInCollection = Collection._Components;
                        var NoComponents = Collection._NoComponents;
                        string TypeName = "Collection_";
                        string interfaceName = "BaseEntity";
                        for (int pe = 0; pe < ComponentsInCollection.Count; pe++)
                        {
                            TypeName += ComponentsInCollection[pe];
                            // interfaceName += ComponentsInCollection[pe];
                        }
                        if (NoComponents.Count > 0)
                        {
                            TypeName += "_Ignore_";
                            //interfaceName += "_Ignore_";
                        }
                        for (int pe = 0; pe < NoComponents.Count; pe++)
                        {
                            TypeName += NoComponents[pe];
                            //interfaceName += NoComponents[pe];
                        }
                        //interfaceName += "Type";
                        F.NL("public class " + TypeName);
                        F.OpenFunction();
                        F.NL("protected HashSet<" + interfaceName + "> _Collection = new HashSet<" + interfaceName + ">();");
                        F.NL("protected List<" + interfaceName + "> _InnerList = new List<" + interfaceName + ">();");
                        F.NL("public List<Action<" + interfaceName + ">> _OnAdded = new List<Action<" + interfaceName + ">>(4);");
                        F.NL("public List<Action<" + interfaceName + ">> _OnRemoved = new List<Action<" + interfaceName + ">>(4);");
                        F.NL("protected bool _IsDirty = true;");
                        F.NL("public int Count = 0;");
                        //F.NL("public interface " + interfaceName + " : IRueEntity");
                        //F.OpenFunction();
                        //for (int a = 0; a < ComponentsInCollection.Count; a++)
                        //{
                        //    var ComponentName = ComponentsInCollection[a];
                        //    F.NL(ComponentName + " Get" + ComponentName + " ();");
                        //}
                        //F.CloseFunction();
                        F.NL("public void EntityGetsAdded(" + interfaceName + " Adding)");
                        F.OpenFunction();

                        F.NL("if ( _Collection.Add(Adding) )");
                        F.OpenFunction();
                        F.NL("Count++;");
                        F.NL("_IsDirty = true;");
                        F.NL("for (int i = 0; i < _OnAdded.Count; i++) { _OnAdded[i](Adding); }");
                        F.CloseFunction();
                        F.CloseFunction();

                        F.NL("public void EntityGetsRemoved(" + interfaceName + " Removing)");
                        F.OpenFunction();

                        F.NL("if ( _Collection.Remove(Removing) )");
                        F.OpenFunction();
                        F.NL("Count--;");
                        F.NL("_IsDirty = true;");
                        F.NL("for (int i = 0; i < _OnRemoved.Count; i++) { _OnRemoved[i](Removing); }");
                        F.CloseFunction();
                        F.CloseFunction();
                        F.NL("public List<" + interfaceName + "> GetEntitiesDirect() { if (_IsDirty) { _InnerList.Clear(); _InnerList.AddRange(_Collection); _IsDirty = false; } return _InnerList; }");
                        F.NL("public " + TypeName + " OnAdded( Action<" + interfaceName + "> Added ) { _OnAdded.Add(Added); return this; }");
                        F.NL("public " + TypeName + " OnRemoved( Action<" + interfaceName + "> Removed ) { _OnRemoved.Add(Removed); return this; }");
                        //F.NL("public bool Belongs(IRueEntity e) { return e is " + interfaceName + "; }");
                        F.NL("public void Nuke() { var ent = GetEntitiesDirect(); for (int i = 0; i < ent.Count; i++) { ent[i].Destroy(); } }");

                        F.CloseFunction();
                    }

                    #endregion
                }
                F.CloseFunction();
                W.Flush();
                W.Close();
            }
        }
        {//static collections
            string DataPath = Application.dataPath + "/ECSStaticCollections.cs";
            string final = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataPath);
            //Directory.Delete(final);
            using (StreamWriter W = new StreamWriter(final, false))
            {
                StrFor F = new StrFor(W);
                F.NL("using System;");
                F.NL("using System.Collections.Generic;");
                F.NL("using UnityEngine;");
                F.NL("using System.Collections;");
                F.NL("namespace RueECS");
                F.OpenFunction();
                {

                    #region Static collections
                    F.NL("public static partial class StaticCollections");
                    F.OpenFunction();



                    List<string> Nukers = new List<string>();
                    //for (int i = 0; i < _AllCollections.Count; i++)
                    foreach (var item in RepeatedCollections.Values)
                    {
                        string CollectionName = item._CollectionName;
                        var Collection = item;
                        var ComponentsInCollection = Collection._Components;
                        var IgnoreComponents = Collection._NoComponents;
                        string Comment = "";
                        //string TheTypesOf = "";
                        string TypeName = "Collection_";
                        for (int pe = 0; pe < ComponentsInCollection.Count; pe++)
                        {
                            Comment += ComponentsInCollection[pe] + "   ";
                            TypeName += ComponentsInCollection[pe];
                        }
                        if (IgnoreComponents.Count > 0)
                        {
                            TypeName += "_Ignore_";
                            Comment += " Ignore: ";
                        }
                        for (int pe = 0; pe < IgnoreComponents.Count; pe++)
                        {
                            Comment += IgnoreComponents[pe] + "   ";
                            TypeName += IgnoreComponents[pe];
                        }
                        // TheTypesOf = TheTypesOf.Remove(TheTypesOf.Length - 2, 2);
                        EntitiesBelongingTo.Add(Collection, new List<DeclaringEntity>());
                        for (int a = 0; a < _AllEntities.Count; a++)
                        {
                            bool BelongsToThisCollection = true;
                            var ComponentsInEntity = _AllEntities[a]._Components;
                            for (int q = 0; q < ComponentsInCollection.Count; q++)
                            {
                                if (!ComponentsInEntity.Contains(ComponentsInCollection[q]))
                                {
                                    BelongsToThisCollection = false;
                                    break;
                                }
                            }

                            for (int q = 0; q < IgnoreComponents.Count; q++)
                            {
                                if (ComponentsInEntity.Contains(IgnoreComponents[q]))
                                {
                                    BelongsToThisCollection = false;
                                    break;
                                }
                            }

                            if (BelongsToThisCollection)
                            {
                                EntitiesBelongingTo[Collection].Add(_AllEntities[a]);
                            }
                        }
                        F.NL("///<summary>" + Comment + "</summary>");
                        F.NL("public static " + TypeName + " _" + CollectionName + " = new " + TypeName + "();");
                        Nukers.Add(" _" + CollectionName);
                        if (Collection._Aliases.Count > 0)
                        {
                            foreach (var qqq in Collection._Aliases)
                            {
                                F.NL("public static " + TypeName + " _" + qqq + " { get { return _" + CollectionName + "; } }");
                            }
                            //for (int i = 0; i < Collection._Aliases.Count; i++)
                            {
                                //F.NL("public static " + TypeName + " _" + Collection._Aliases[i] + " { get { return _" + CollectionName + "; } }");
                            }
                        }
                        //F.NL("public static RueECSCollection _" + CollectionName + " = new RueECSCollection(" + TheTypesOf + ");");
                    }

                    F.NL("public static void NukeAll()");
                    F.OpenFunction();
                    for (int i = 0; i < Nukers.Count; i++)
                    {
                        F.NL(Nukers[i] + ".Nuke();");
                    }
                    F.CloseFunction();

                    //F.NL("static partial void InnerSyncEntitiesComponents(int Parent, int Children)");
                    //F.OpenFunction();
                    //F.NL("BaseEntity A = BaseEntity._Pool[Parent];");
                    //F.NL("BaseEntity B = BaseEntity._Pool[Children];");
                    //
                    //for (int w = 0; w < ParsedAllComponents.Count; w++)
                    //{
                    //    DeclaringComponent declaring = ParsedAllComponents[w];
                    //    if (declaring._AllGathered.Count == 0) { continue; }
                    //    F.NL("if( A.Has" + declaring._ComponentName + "() && B.Has" + declaring._ComponentName + "()) { Sync_" + declaring._ComponentName + "(Parent, Children); }");
                    //}
                    //F.CloseFunction();
                    //
                    //for (int w = 0; w < ParsedAllComponents.Count; w++)
                    //{
                    //    DeclaringComponent declaring = ParsedAllComponents[w];
                    //
                    //    if (declaring._AllGathered.Count == 0) { continue; }
                    //    string ComponentName = declaring._ComponentName;
                    //    F.NL("public static void Sync_" + ComponentName + "(int Parent, int Child)");
                    //    F.OpenFunction();
                    //    F.NL("BaseEntity A = BaseEntity._Pool[Parent];");
                    //    F.NL("BaseEntity B = BaseEntity._Pool[Child];");
                    //    F.NL("var C1 = A.TryGet" + ComponentName + "(); var C2 = B.TryGet" + ComponentName + "(); int VersionA = A.GetVersion(); int VersionB = B.GetVersion();");
                    //
                    //    for (int l = 0; l < declaring._AllGathered.Count; l++)
                    //    {
                    //        if (declaring._AllGathered[l]._IsHidden) { continue; }
                    //        string FieldType = declaring._AllGathered[l]._Type;
                    //        string FieldName = declaring._AllGathered[l]._Name;
                    //        string FieldChangedA = "OnChangedA" + FieldName;
                    //
                    //        F.NL("Action<BaseEntity, " + FieldType + ", " + FieldType + "> " + FieldChangedA + " = (InnerA, val1, val2) => { if (B.GetVersion() != VersionB) { return; } if (C1._" + FieldName + " == C2._" + FieldName + ") { return; } C2._" + FieldName + " = C1._" + FieldName + "; };");
                    //
                    //        F.NL(" C1.__" + FieldName + "Changed.Add(" + FieldChangedA + ");");
                    //
                    //    }
                    //
                    //    F.CloseFunction();
                    //}

                    F.CloseFunction();

                    #endregion
                }
                F.CloseFunction();
                W.Flush();
                W.Close();
            }
        }
        {//Entities declarations
            _AllEntities.Add(new DeclaringEntity() { _Components = new List<string>(), _EntityName = "EmptyEntity" });

            Dictionary<int, List<DeclaringComponent>> _ContextsSeparated = new Dictionary<int, List<DeclaringComponent>>();
            for (int i = 0; i < ParsedAllComponents.Count; i++)
            {
                List<DeclaringComponent> comps = null;
                if (!_ContextsSeparated.TryGetValue(ParsedAllComponents[i]._ContextID, out comps))
                {
                    comps = new List<DeclaringComponent>();
                    _ContextsSeparated.Add(ParsedAllComponents[i]._ContextID, comps);
                }
                comps.Add(ParsedAllComponents[i]);
            }

            //foreach (var item in _ContextsSeparated)
            //{
            //    var allcomps = item.Value;
            //    for (int i = 0; i < allcomps.Count; i++)
            //    {
            //        
            //    }
            //}


            string DataPath = Application.dataPath + "/ECSEntitiesDeclarations.cs";
            string final = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataPath);
            //Directory.Delete(final);
            using (StreamWriter W = new StreamWriter(final, false))
            {
                StrFor F = new StrFor(W);
                F.NL("using System;");
                F.NL("using System.Collections.Generic;");
                F.NL("using UnityEngine;");
                F.NL("using System.Collections;");
                if (IsDebug)
                {
                    F.NL("using Sirenix.OdinInspector;");
                }
                F.NL("namespace RueECS");
                F.OpenFunction();
                {
                    F.NL("public partial class BaseEntity");
                    F.OpenFunction();
                    for (int w = 0; w < ParsedAllComponents.Count; w++)
                    {
                        DeclaringComponent declaring = ParsedAllComponents[w];
                        F.NL("public virtual bool Has" + declaring._ComponentName + "()");
                        F.OpenFunction();
                        F.NL("return false;");
                        F.CloseFunction();

                        F.NL("public virtual " + declaring._ComponentName + " TryGet" + declaring._ComponentName + "()");
                        F.OpenFunction();
                        F.NL("return null;");
                        F.CloseFunction();
                    }


                    F.CloseFunction();



                    F.NL("public class GameEntity: BaseEntity");
                    F.OpenFunction();
                    //F.NL("bool __Subscribed = false;");
                    F.NL("static Queue<GameEntity> _Queue = new Queue<GameEntity>();  public static GameEntity Make() { if(_Queue.Count > 0) { var rr = _Queue.Dequeue(); rr._WasDestroyed = false; return rr; } return new GameEntity(); }");
                    F.NL("protected GameEntity(): base() { }");
                    for (int w = 0; w < ParsedAllComponents.Count; w++)
                    {
                        DeclaringComponent declaring = ParsedAllComponents[w];
                        if (IsDebug)
                        {
                            F.NL("[ShowInInspector]");
                        }
                        F.NL(declaring._ComponentName + " _" + declaring._ComponentName + ";");
                        F.NL("public override " + declaring._ComponentName + " TryGet" + declaring._ComponentName + "()");
                        F.OpenFunction();

                        F.NL("if ( _" + declaring._ComponentName + " == null)");
                        F.OpenFunction();
                        F.NL("Debug.LogError(\"Adding component : " + declaring._ComponentName + "\");");
                        F.NL("_" + declaring._ComponentName + " = " + declaring._ComponentName + ".Make(this); ");
                        F.NL("SubscribeToCollections();");
                        F.CloseFunction();
                        F.NL("return _" + declaring._ComponentName + ";");
                        F.CloseFunction();
                        F.NL("public override bool Has" + declaring._ComponentName + "()");
                        F.OpenFunction();
                        F.NL("return _" + declaring._ComponentName + " != null;");
                        F.CloseFunction();


                        string argumentspassing = "";
                        var currentcomponentin = declaring;
                        int correctedamount = 0;
                        for (int q = 0; q < currentcomponentin._AllGathered.Count; q++)
                        {
                            if (currentcomponentin._AllGathered[q]._IsHidden) { correctedamount++; }
                        }

                        for (int q = 0; q < currentcomponentin._AllGathered.Count; q++)
                        {
                            if (currentcomponentin._AllGathered[q]._IsHidden) { continue; }
                            argumentspassing += currentcomponentin._AllGathered[q]._Type + " " + currentcomponentin._AllGathered[q]._Name;
                            if (q + 1 < currentcomponentin._AllGathered.Count - correctedamount)
                            {
                                argumentspassing += ", ";
                            }
                            else
                            {
                                argumentspassing += ")";
                            }
                        }
                        if (argumentspassing == "") { argumentspassing = " )"; }




                        F.NL("public " + declaring._ComponentName + " Add" + declaring._ComponentName + "(" + argumentspassing);
                        F.OpenFunction();
                        F.NL("if(_" + declaring._ComponentName + " != null) { return _" + declaring._ComponentName + "; }");
                        F.NL("_" + declaring._ComponentName + " = " + declaring._ComponentName + ".Make(this);");
                        for (int m = 0; m < currentcomponentin._AllGathered.Count; m++)
                        {
                            if (currentcomponentin._AllGathered[m]._IsHidden) { continue; }
                            string FieldType = currentcomponentin._AllGathered[m]._Type;
                            string FieldName = currentcomponentin._AllGathered[m]._Name;
                            F.NL("_" + currentcomponentin._ComponentName + "._" + FieldName + " = " + FieldName + ";");
                        }


                        F.NL("return _" + declaring._ComponentName + ";");
                        F.CloseFunction();
                    }
                    F.NL("public void SubscribeToCollections()");
                    F.OpenFunction();
                    //F.NL("if( __Subscribed) { return; }");
                    //F.NL("__Subscribed = true;");
                    foreach (var item in RepeatedCollections.Values)
                    {
                        string Ifs = "";
                        for (int i = 0; i < item._Components.Count; i++)
                        {
                            Ifs += "if ( _" + item._Components[i] + " != null) ";
                        }
                        for (int i = 0; i < item._NoComponents.Count; i++)
                        {
                            Ifs += "if ( _" + item._NoComponents[i] + " == null) ";
                        }
                        Ifs += " { StaticCollections._" + item._CollectionName + ".EntityGetsAdded(this); }";
                        F.NL(Ifs);
                    }
                    F.CloseFunction();
                    F.NL("protected override void InnerDestroy()");
                    F.OpenFunction();
                    //F.NL("__Subscribed = false;");
                    foreach (var item in RepeatedCollections.Values)
                    {
                        string Ifs = "";
                        for (int i = 0; i < item._Components.Count; i++)
                        {
                            Ifs += "if ( _" + item._Components[i] + " != null) ";
                        }
                        for (int i = 0; i < item._NoComponents.Count; i++)
                        {
                            Ifs += "if ( _" + item._NoComponents[i] + " == null) ";
                        }
                        Ifs += " { StaticCollections._" + item._CollectionName + ".EntityGetsRemoved(this); }";
                        F.NL(Ifs);
                    }

                    for (int w = 0; w < ParsedAllComponents.Count; w++)
                    {
                        DeclaringComponent declaring = ParsedAllComponents[w];
                        F.NL("if( _" + declaring._ComponentName + " != null) { _" + declaring._ComponentName + ".ReturnToPool(); _" + declaring._ComponentName + " = null; }");
                    }


                    F.NL("base.InnerDestroy();");
                    F.NL("_Queue.Enqueue(this);");
                    F.CloseFunction();

                    F.CloseFunction();


                    //foreach (var item in RepeatedCollections.Values)
                    //{
                    //    var Collection = item;
                    //    var ComponentsInCollection = Collection._Components;
                    //    var NoComponents = Collection._NoComponents;
                    //    string TypeName = "Collection_";
                    //    string interfaceName = "IType";
                    //    for (int pe = 0; pe < ComponentsInCollection.Count; pe++)
                    //    {
                    //        TypeName += ComponentsInCollection[pe];
                    //        // interfaceName += ComponentsInCollection[pe];
                    //    }
                    //    if (NoComponents.Count > 0)
                    //    {
                    //        TypeName += "_Ignore_";
                    //        //interfaceName += "_Ignore_";
                    //    }
                    //    for (int pe = 0; pe < NoComponents.Count; pe++)
                    //    {
                    //        TypeName += NoComponents[pe];
                    //        //interfaceName += NoComponents[pe];
                    //    }
                    //
                    //
                    //
                    //}
                    //
                    //F.NL("public class Ditto: BaseEntity");
                    //F.OpenFunction();
                    //for (int w = 0; w < ParsedAllComponents.Count; w++)
                    //{
                    //    DeclaringComponent declaring = ParsedAllComponents[w];
                    //    F.NL("int _" + declaring._ComponentName + " = -1;");
                    //}
                    //
                    //
                    //F.NL("public virtual bool HasComponent(int ID)");
                    //F.OpenFunction();
                    //F.NL("switch(ID)");
                    //F.OpenFunction();
                    //for (int w = 0; w < ParsedAllComponents.Count; w++)
                    //{
                    //    DeclaringComponent declaring = ParsedAllComponents[w];
                    //    F.NL("case " + declaring._ComponentName + ".ID: ");
                    //    F.OpenFunction();
                    //    F.NL("return _" + declaring._ComponentName + " != -1;");
                    //    F.CloseFunction();
                    //   
                    //}
                    //F.CloseFunction();
                    //F.NL("return false;");
                    //F.CloseFunction();
                    //
                    //for (int w = 0; w < ParsedAllComponents.Count; w++)
                    //{
                    //    DeclaringComponent declaring = ParsedAllComponents[w];
                    //    F.NL("public override bool Has" + declaring._ComponentName+"()");
                    //    F.OpenFunction();
                    //    F.NL("return _" + declaring._ComponentName + " != -1;");
                    //    F.CloseFunction();
                    //
                    //    F.NL("public " + declaring._ComponentName + " Add" + declaring._ComponentName + "()");
                    //    F.OpenFunction();
                    //    F.NL("if(Has" + declaring._ComponentName + "()) { return Get" + declaring._ComponentName +"(); }");
                    //    F.NL("_" + declaring._ComponentName + " = " + declaring._ComponentName + ".Make(this).GetUniqueID();");
                    //    F.NL("return " + declaring._ComponentName + "._Orderly[_" + declaring._ComponentName + "];");
                    //    F.CloseFunction();
                    //
                    //    F.NL("public " + declaring._ComponentName + " Get" + declaring._ComponentName + "()");
                    //    F.OpenFunction();
                    //
                    //    F.NL("return " + declaring._ComponentName + "._Orderly[_" + declaring._ComponentName + "];");
                    //
                    //    F.CloseFunction();
                    //}



                    #region Entity declarations
                    for (int i = 0; i < _AllEntities.Count; i++)
                    {
                        var entity = _AllEntities[i];
                        var componentsinentity = entity._Components;
                        componentsinentity.Sort((x, y) =>
                        {
                            return x.CompareTo(y);
                        });

                        int c = entity._Components.Count;
                        if (c == 0 && entity._EntityName != "EmptyEntity") { continue; }
                        string ComponentsPresent = ": BaseEntity";
                        List<string> CollectionsFoundInEnt = new List<string>();
                        List<string> ExtrasC = new List<string>();
                        //ComponentsPresent += ": BaseEntity, " + entity._EntityName + ".FinalStep, ";
                        if (c == 0)
                        {
                            if (entity._EntityName != "EmptyEntity")
                            {
                                ComponentsPresent = ": BaseEntity, " + entity._EntityName + ".FinalStep";
                            }
                            else
                            {
                                ComponentsPresent = ": BaseEntity, " + entity._EntityName + ".Step1, " + entity._EntityName + ".FinalStep";
                            }
                        }
                        else
                        {
                            //foreach (var item in EntitiesBelongingTo.Keys)
                            //{
                            //    var gj = EntitiesBelongingTo[item];
                            //    if (gj.Contains(entity))
                            //    {
                            //        //DidBelongToAny = true;
                            //        var ComponentsInCollection = item._Components;
                            //        var NoComponents = item._NoComponents;
                            //        string TypeName = "Collection_";
                            //        string interfaceName = "IType";
                            //        for (int pe = 0; pe < ComponentsInCollection.Count; pe++)
                            //        {
                            //            TypeName += ComponentsInCollection[pe];
                            //            //interfaceName += ComponentsInCollection[pe];
                            //        }
                            //        if (NoComponents.Count > 0)
                            //        {
                            //            //interfaceName += "_Ignore_";
                            //            TypeName += "_Ignore_";
                            //        }
                            //        for (int pe = 0; pe < NoComponents.Count; pe++)
                            //        {
                            //            TypeName += NoComponents[pe];
                            //            //interfaceName += NoComponents[pe];
                            //        }
                            //        //interfaceName += "Type";
                            //        CollectionsFoundInEnt.Add("," + TypeName + "." + interfaceName);
                            //        //ComponentsPresent += 
                            //    }
                            //}
                            //for (int a = 0; a < componentsinentity.Count; a++)
                            //{
                            //    ComponentsPresent += (", I" + componentsinentity[a]);
                            //}
                            for (int a = 0; a < componentsinentity.Count; a++)
                            {
                                ComponentsPresent += (", " + entity._EntityName + ".Step" + (a + 1).ToString());
                            }
                            ComponentsPresent += "," + entity._EntityName + ".FinalStep";

                        }
                        F.NL("public class " + entity._EntityName + ComponentsPresent);
                        if (CollectionsFoundInEnt.Count > 0)
                        {
                            F.NL("");
                            F.AddIndent();
                            for (int k = 0; k < CollectionsFoundInEnt.Count; k++)
                            {
                                F.NL(CollectionsFoundInEnt[k]);
                            }
                            F.RemoveIndent();
                        }
                        F.NL("#region " + entity._EntityName);
                        F.OpenFunction();
                        F.NL("protected " + entity._EntityName + "():base(){}");
                        F.NL("private static List<" + entity._EntityName + "> _Buffer = new List<" + entity._EntityName + ">();");
                        for (int co = 0; co < componentsinentity.Count; co++)
                        {
                            F.NL("public interface Step" + (co + 1).ToString());
                            F.OpenFunction();

                            string argumentspassing = "";
                            var currentcomponentin = _ComponentDictionaryByName[componentsinentity[co]];
                            int correctedamount = 0;
                            for (int q = 0; q < currentcomponentin._AllGathered.Count; q++)
                            {
                                if (currentcomponentin._AllGathered[q]._IsHidden) { correctedamount++; }
                            }

                            for (int q = 0; q < currentcomponentin._AllGathered.Count; q++)
                            {
                                if (currentcomponentin._AllGathered[q]._IsHidden) { continue; }
                                argumentspassing += currentcomponentin._AllGathered[q]._Type + " " + currentcomponentin._AllGathered[q]._Name;
                                if (q + 1 < currentcomponentin._AllGathered.Count - correctedamount)
                                {
                                    argumentspassing += ", ";
                                }
                                else
                                {
                                    argumentspassing += ");";
                                }
                            }
                            if (co + 1 < componentsinentity.Count)
                            {
                                if (argumentspassing.Length == 0)
                                {
                                    argumentspassing = ");";
                                }
                                F.NL("Step" + (co + 2).ToString() + " __" + componentsinentity[co] + "(" + argumentspassing);
                                F.CloseFunction();
                            }
                            else
                            {
                                if (argumentspassing.Length == 0)
                                {
                                    argumentspassing = ");";
                                }
                                F.NL("FinalStep __" + componentsinentity[co] + "(" + argumentspassing);
                                F.CloseFunction();
                            }
                        }
                        if (entity._EntityName == "EmptyEntity")
                        {
                            F.NL("public interface Step1 { FinalStep Declare(); }");
                            F.NL("public FinalStep Declare() { return this; }");
                        }

                        F.NL("public interface FinalStep { BaseEntity __Finish(); int __FinishWithID(); }");

                        for (int co = 0; co < componentsinentity.Count; co++)
                        {
                            // F.NL("[ShowInInspector]");
                            F.NL(componentsinentity[co] + " _" + componentsinentity[co] + ";");
                            //F.NL("Type _" + componentsinentity[co] + "Type;");
                            F.NL("public " + componentsinentity[co] + " Get" + componentsinentity[co] + "() { return _" + componentsinentity[co] + "; }");
                            F.NL("public override " + componentsinentity[co] + " TryGet" + componentsinentity[co] + "() { return _" + componentsinentity[co] + "; }");


                            F.NL("public override bool Has" + componentsinentity[co] + "() { return true; }");
                        }

                        F.NL("public static Step1 __Make() { " + entity._EntityName + " D = null; if (_Buffer.Count > 0) { D = _Buffer[_Buffer.Count - 1]; _Buffer.RemoveAt(_Buffer.Count - 1); } else { D = new " + entity._EntityName + "(); } D._WasDestroyed = false; return D; }");
                        for (int co = 0; co < componentsinentity.Count; co++)
                        {
                            string argumentspassing = "";
                            var currentcomponentin = _ComponentDictionaryByName[componentsinentity[co]];
                            int correctedamount = 0;

                            for (int q = 0; q < currentcomponentin._AllGathered.Count; q++)
                            {
                                if (currentcomponentin._AllGathered[q]._IsHidden) { correctedamount++; }
                            }

                            for (int q = 0; q < currentcomponentin._AllGathered.Count; q++)
                            {
                                if (currentcomponentin._AllGathered[q]._IsHidden) { continue; }
                                argumentspassing += currentcomponentin._AllGathered[q]._Type + " " + currentcomponentin._AllGathered[q]._Name;
                                if (q + 1 < currentcomponentin._AllGathered.Count - correctedamount)
                                {
                                    argumentspassing += ", ";
                                }
                                else
                                {
                                    argumentspassing += ")";
                                }
                            }
                            if (argumentspassing.Length == 0)
                            {
                                argumentspassing = ")";
                            }
                            if (co + 1 < componentsinentity.Count)
                            {
                                F.NL("public Step" + (co + 2).ToString() + " __" + componentsinentity[co] + "(" + argumentspassing);
                            }
                            else
                            {
                                F.NL("public FinalStep __" + componentsinentity[co] + "(" + argumentspassing);
                            }

                            F.OpenFunction();
                            F.NL("_" + componentsinentity[co] + " = " + componentsinentity[co] + ".Make(this);");
                            //F.NL("_" + componentsinentity[co] + " = new " + componentsinentity[co] + "(this);");
                            //F.NL("_" + componentsinentity[co] + "Type = typeof(" + componentsinentity[co] + ");");
                            for (int w = 0; w < currentcomponentin._AllGathered.Count; w++)
                            {
                                if (currentcomponentin._AllGathered[w]._IsHidden) { continue; }
                                string FieldType = currentcomponentin._AllGathered[w]._Type;
                                string FieldName = currentcomponentin._AllGathered[w]._Name;
                                F.NL("_" + componentsinentity[co] + "._" + FieldName + " = " + FieldName + ";");
                            }

                            F.NL("return this;");
                            F.CloseFunction();
                        }


                        //F.NL("public override T GetComponent<T>()");
                        //F.OpenFunction();
                        //for (int co = 0; co < componentsinentity.Count; co++)
                        //{
                        //    F.NL("if (typeof(T) == _" + componentsinentity[co] + "Type) return (T)((IRueComp)_" + componentsinentity[co] + ");");
                        //}
                        //F.NL("return default(T);");
                        //F.CloseFunction();

                        F.NL("public BaseEntity __Finish()");
                        F.OpenFunction();

                        List<DeclaringCollection> InThisDef = new List<DeclaringCollection>();
                        foreach (var item in EntitiesBelongingTo.Keys)
                        {
                            var gj = EntitiesBelongingTo[item];
                            if (gj.Contains(entity))
                            {
                                InThisDef.Add(item);
                                //F.NL("StaticCollections._" + item._CollectionName + ".EntityGetsAdded(this);");
                            }
                        }
                        InThisDef.Sort((x, y) =>
                        {
                            return x._Priority.CompareTo(y._Priority);
                        });
                        InThisDef.Sort((x, y) =>
                        {
                            return x._Components.Count.CompareTo(y._Components.Count);
                        });
                        for (int a = 0; a < InThisDef.Count; a++)
                        {
                            F.NL("StaticCollections._" + InThisDef[a]._CollectionName + ".EntityGetsAdded(this);");
                        }
                        F.NL("return this;");
                        F.CloseFunction();

                        F.NL("public int __FinishWithID()");
                        F.OpenFunction();
                        //foreach (var item in EntitiesBelongingTo.Keys)
                        //{
                        //    var gj = EntitiesBelongingTo[item];
                        //    if (gj.Contains(entity))
                        //    {
                        //        F.NL("StaticCollections._" + item._CollectionName + ".EntityGetsAdded(this);");
                        //    }
                        //}
                        for (int a = 0; a < InThisDef.Count; a++)
                        {
                            F.NL("StaticCollections._" + InThisDef[a]._CollectionName + ".EntityGetsAdded(this);");
                        }
                        F.NL("return _UniqueID;");
                        F.CloseFunction();

                        F.NL("protected override void InnerDestroy()");
                        F.OpenFunction();
                        //F.NL("if(_WasDestroyed) { return; }");
                        //F.NL("base.Destroy();");

                        for (int a = 0; a < InThisDef.Count; a++)
                        {
                            F.NL("StaticCollections._" + InThisDef[a]._CollectionName + ".EntityGetsRemoved(this);");
                        }

                        //foreach (var item in EntitiesBelongingTo.Keys)
                        //{
                        //    var gj = EntitiesBelongingTo[item];
                        //    if (gj.Contains(entity))
                        //    {
                        //        F.NL("StaticCollections._" + item._CollectionName + ".EntityGetsRemoved(this);");
                        //    }
                        //}
                        for (int co = 0; co < componentsinentity.Count; co++)
                        {
                            var compoagain = _ComponentDictionaryByName[componentsinentity[co]];

                            F.NL("_" + compoagain._ComponentName + ".ReturnToPool();");
                        }
                        F.NL("_Buffer.Add(this);");
                        F.NL("base.InnerDestroy();");
                        F.CloseFunction();
                        F.CloseFunction();
                        F.NL("#endregion");
                    }
                    #endregion
                    #region Entity maker factory
                    //F.NL("public static class EntityMake");
                    //F.OpenFunction();
                    //for (int i = 0; i < _AllEntities.Count; i++)
                    //{
                    //    var currententitydeclaration = _AllEntities[i];
                    //    if (currententitydeclaration._Components.Count == 0 && currententitydeclaration._EntityName != "EmptyEntity") { continue; }
                    //    //create a factory per entity
                    //    //public static BaseEntity EntityName_Make()
                    //    F.NL("public static " + currententitydeclaration._EntityName + ".Step1 Make_" + currententitydeclaration._EntityName + " ()");
                    //    F.OpenFunction();
                    //    F.NL("return " + currententitydeclaration._EntityName + ".__Make();");
                    //    F.CloseFunction();
                    //}
                    //F.CloseFunction();

                    F.NL("public static class EM");
                    F.OpenFunction();
                    for (int i = 0; i < _AllEntities.Count; i++)
                    {
                        var currententitydeclaration = _AllEntities[i];
                        if (currententitydeclaration._Components.Count == 0 && currententitydeclaration._EntityName != "EmptyEntity") { continue; }
                        F.NL("public static " + currententitydeclaration._EntityName + ".Step1 _" + currententitydeclaration._EntityName + " ()");
                        F.OpenFunction();
                        F.NL("return " + currententitydeclaration._EntityName + ".__Make();");
                        F.CloseFunction();
                    }
                    F.CloseFunction();

                    #endregion
                }
                F.CloseFunction();
                W.Flush();
                W.Close();
            }
        }
        #region CustomCode
        try
        {
            //Organized.Clear();
            //var typesWithMyAttribute =
            //              from a in AppDomain.CurrentDomain.GetAssemblies()
            //              from t in a.GetTypes()
            //              let attributes = t.GetCustomAttributes(typeof(IsGameSystemAttribute), true)
            //              where attributes != null && attributes.Length > 0
            //              select new { Type = t, Attributes = attributes.Cast<IsGameSystemAttribute>() };
            //foreach (var item in typesWithMyAttribute)
            //{
            //    //for every type that has ingame system attribute, create the declaration
            //    IsGameSystemAttribute att = item.Attributes.First(); //first and only. 
            //                                                         //based on the information on this attribute, create the boilerplate partial extended class of RueECSFlows which will create and tick all the systems properly.
            //    Organized.Add(ValueTuple.Create<Type, IsGameSystemAttribute>(item.Type, att));
            //}
            ////sort by name
            //Organized.Sort((x, y) =>
            //{
            //    return x.Item1.Name.CompareTo(y.Item1.Name);
            //});
            ////sort by priority
            //Organized.Sort((x, y) =>
            //{
            //    return y.Item2._Priority.CompareTo(x.Item2._Priority);
            //});
            string support = Application.dataPath + "/ECSCustomCode.cs";
            string finalsupport = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), support);


            using (StreamWriter W = new StreamWriter(finalsupport, false))
            {
                StrFor F = new StrFor(W);

                {
                    var type = typeof(ICodeGenRueECS);
                    var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(a => type.IsAssignableFrom(a));

                    foreach (var item in types)
                    {
                        if (item.IsClass)
                        {
                            ICodeGenRueECS getCodeGen = Activator.CreateInstance(item) as ICodeGenRueECS;
                            getCodeGen.DeclareUsing(F);
                        }
                    }
                    foreach (var item in types)
                    {
                        if (item.IsClass)
                        {
                            ICodeGenRueECS getCodeGen = Activator.CreateInstance(item) as ICodeGenRueECS;
                            getCodeGen.CodeGen(F);
                        }
                    }
                }

                W.Flush();
                W.Close();
            }
        }
        catch (Exception e)
        {
            throw new Exception("Exception in code generated custom");
            //WasError = true;
        }

        #endregion


    }

}