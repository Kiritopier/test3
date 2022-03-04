using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using Newtonsoft.Json;
using System.Linq;
using System.IO;

public class RueECSVisualTools : OdinEditorWindow
{
    [MenuItem("Tools/RueECSVisualEditor")]
    private static void OpenWindow()
    {
        var w = GetWindow<RueECSVisualTools>();
        w.Show(); 
        w.LoadData();
    }

    [HideInInspector]
    public AllDataForECS _Data;
    public void LoadData()
    {
        _Data = ExtendedRueECSFrameWorkGenerator.CreateAndGetBackup();//JsonConvert.DeserializeObject<AllDataForECS>(ExtendedRueECSFrameWorkGenerator.CreateAndGetBackup());


        _DeclareEntities = new OdinEntityDeclareWindow(_Data);
        _DeclareCollections = new OdinCollectionDeclareWindow(_Data);
        _DeclareComponents = new OdinComponentDeclareWindow(_Data);
    }

    [ShowInInspector]
    [TabGroup("Entities")]
    public OdinEntityDeclareWindow _DeclareEntities; 

    [ShowInInspector]
    [TabGroup("Components")]
    public OdinComponentDeclareWindow _DeclareComponents;

    [ShowInInspector]
    [TabGroup("Collections")]
    public OdinCollectionDeclareWindow _DeclareCollections;



    public static bool _DebugMode = false;
    [Button]
    public void CreateDebugMode()
    {
        _DebugMode = true;
        CreateAllFiles();
    }
    [Button]
    public void CreateAllFiles()
    {
        if (_Data != null)
        {
            //save the component data
            _Data._AllDeclaredComponents.Clear();
            _Data._AllDeclaredComponents.AddRange(_DeclareComponents._AllDeclaredComps);
            _Data._AllDeclaredComponents.Sort((x, y) =>
            {
                return x._ComponentName.CompareTo(y._ComponentName);
            });
            //save the entity data
            _Data._AllEntities.Clear();
            _Data._AllEntities.AddRange(_DeclareEntities._AllDeclared);
            _Data._AllEntities.Sort((x, y) =>
            {
                return x._EntityName.CompareTo(y._EntityName);
            });
            //save the collection data
            _Data._AllCollections.Clear();
            _Data._AllCollections.AddRange(_DeclareCollections._AllDeclaredColls);

            List<string> Componentnames = new List<string>();
            for (int i = 0; i < _Data._AllDeclaredComponents.Count; i++)
            {
                var comp = _Data._AllDeclaredComponents[i];
                Componentnames.Add(comp._ComponentName);
            }

            //check that the components defined on entities and collections actually exist.
            for (int e = 0; e < _Data._AllEntities.Count; e++)
            {
                var ent = _Data._AllEntities[e];
                for (int entc = 0; entc < ent._Components.Count; entc++)
                {
                    if(!Componentnames.Contains(ent._Components[entc]))
                    {
                        ent._Components.RemoveAt(entc);
                        entc--;
                    }
                }

            }
            for (int e = 0; e < _Data._AllCollections.Count; e++)
            {
                var ent = _Data._AllCollections[e];
                for (int entc = 0; entc < ent._Components.Count; entc++)
                {
                    if (!Componentnames.Contains(ent._Components[entc]))
                    {
                        ent._Components.RemoveAt(entc);
                        entc--;
                    }
                }
                for (int entc = 0; entc < ent._NoComponents.Count; entc++)
                {
                    if (!Componentnames.Contains(ent._NoComponents[entc]))
                    {
                        ent._NoComponents.RemoveAt(entc);
                        entc--;
                    }
                }

            }

            {
                string cpath = Application.dataPath + "/GenComponents";
              
                if (Directory.Exists(cpath))
                {
                    //var files = Directory.GetFiles(cpath);
                    try
                    {
                        Directory.Delete(cpath, true);
                    }
                    catch(Exception e)
                    {
                        //revert changes!

                    }
                }
                {
                    var components = _Data._AllDeclaredComponents;

                    for (int i = 0; i < components.Count; i++)
                    {
                        var currentcomponent = components[i];
                        try
                        {
                            CreateDeclaratorFile(currentcomponent);
                        }
                        catch(Exception e)
                        {

                        }
                    }
                }
            }

            {
                string cpath = Application.dataPath + "/GenEntities";
                if (Directory.Exists(cpath))
                {
                    try
                    {
                        Directory.Delete(cpath, true);
                    }
                    catch (Exception e)
                    {
                        //revert changes!

                    }
                }
                {
                    var entities = _Data._AllEntities;
                    //get all currently available declarators
                    //var declarators = GetAllEntityDeclarators();

                    for (int i = 0; i < entities.Count; i++)
                    {
                        var currententity = entities[i];
                        var entName = currententity._EntityName;
                        if (entName == "") { continue; }
                        try
                        {
                            CreateEntityDeclarator(currententity);
                        }
                        catch(Exception e)
                        {

                        }
                    }
                }
            }

            {

                string cpath = Application.dataPath + "/GenCollections";
                if (Directory.Exists(cpath))
                {
                    try
                    {
                        Directory.Delete(cpath, true);
                    }
                    catch (Exception e)
                    {
                        //revert changes!

                    }
                }
                {
                    var entities = _Data._AllCollections;
                    //get all currently available declarators
                    //var declarators = GetAllEntityDeclarators();

                    for (int i = 0; i < entities.Count; i++)
                    {
                        var currententity = entities[i];
                        var entName = currententity._CollectionName;
                        if (entName == "") { continue; }
                        if(currententity._Components.Count <= 0) { continue; }
                        try
                        {
                            CreateCollectionDeclarator(currententity);
                        }
                        catch(Exception e)
                        {

                        }
                    }
                }
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
           

            ExtendedRueECSFrameWorkGenerator g = new ExtendedRueECSFrameWorkGenerator();
            g.CreateFiles(_Data);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            //LoadData();
        }

        _DebugMode = false;
    }

    List<IEntityDeclarator> GetAllEntityDeclarators()
    {
        List<IEntityDeclarator> dec = new List<IEntityDeclarator>();
        var type = typeof(IEntityDeclarator);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(a => type.IsAssignableFrom(a));
        foreach (var item in types)
        {
            if (item.IsClass)
            {
                IEntityDeclarator getCodeGen = Activator.CreateInstance(item) as IEntityDeclarator;
                dec.Add(getCodeGen);
            }
        }

        return dec;
    }
    void CreateEntityDeclarator(DeclaringEntity dec)
    {
        string cpath = Application.dataPath + "/GenEntities";
        Directory.CreateDirectory(cpath);
        string DataPath = cpath + "/" + dec._EntityName + "Declaration.cs";
        string final = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataPath);
        FlushEntityDeclarator(dec, final);
    }

    void FlushEntityDeclarator(DeclaringEntity dec, string final)
    {
        using (StreamWriter W = new StreamWriter(final, false))
        {
            StrFor F = new StrFor(W);
            var components = dec._Components;
            //public string GetEntityName()
            F.NL("using System.Collections.Generic;");
            F.NL("public class " + dec._EntityName + "Declaration : IEntityDeclarator");
            F.OpenFunction();
            F.NL("public string GetEntityName() { return \"" + dec._EntityName + "\"; }");
            F.NL("public List<IComponentDeclarator> ComponentsInEntity()");
            F.OpenFunction();

            F.NL("return new List<IComponentDeclarator>()");
            F.OpenFunction();
            for (int i = 0; i < dec._Components.Count; i++)
            {
                string TT = "new " + dec._Components[i] + "Declaration()";
                if (i + 1 < dec._Components.Count)
                {
                    TT += ",";
                }
                F.NL(TT);
            }
            F.CloseFunctionPeriod();
            F.CloseFunction();

            F.NL("public List<IEntityDeclarator> IsAlso() { return null; }");

            F.CloseFunction();
            W.Flush();
            W.Close();
        }
    }

    void CreateCollectionDeclarator(DeclaringCollection dec)
    {
        string cpath = Application.dataPath + "/GenCollections";
        Directory.CreateDirectory(cpath);
        string DataPath = cpath + "/" + dec._CollectionName + "Declaration.cs";
        string final = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataPath);
        FlushCollectionDeclaration(dec, final);
    }

    void FlushCollectionDeclaration(DeclaringCollection dec, string final)
    {
        using (StreamWriter W = new StreamWriter(final, false))
        {
            StrFor F = new StrFor(W);
            var components = dec._Components;
            //public string GetEntityName()
            F.NL("using System.Collections.Generic;");
            F.NL("public class " + dec._CollectionName + "Declarator : ICollectionDeclarator");
            F.OpenFunction();
           
            F.NL("public List<CollectionDeclarationForDeclarator> GetDeclaring()");
            F.OpenFunction();

            F.NL("return new List<CollectionDeclarationForDeclarator>()");
            F.OpenFunction();
            F.NL("this.Declare(\"" + dec._CollectionName + "\", this.Collection(");
            string TT = "";
            for (int i = 0; i < dec._Components.Count; i++)
            {
                TT += "new " + dec._Components[i] + "Declaration()";
                if (i + 1 < dec._Components.Count)
                {
                    TT += ",";
                }
            }
            TT += ")";
            if(dec._NoComponents.Count > 0)
            {
                TT += ", this.Collection(";
                for (int i = 0; i < dec._NoComponents.Count; i++)
                {
                    TT += "new " + dec._NoComponents[i] + "Declaration()";
                    if (i + 1 < dec._NoComponents.Count)
                    {
                        TT += ",";
                    }
                }
                TT += ")";
            }
            TT += ")";
            F.NL(TT);
            F.CloseFunctionPeriod();
            F.CloseFunction();

            F.CloseFunction();
            W.Flush();
            W.Close();
        }
    }

    List<IComponentDeclarator> GetAllComponentDeclarations()
    {
        List<IComponentDeclarator> dec = new List<IComponentDeclarator>();
        var type = typeof(IComponentDeclarator);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(a => type.IsAssignableFrom(a));
        foreach (var item in types)
        {
            if (item.IsClass)
            {
                IComponentDeclarator getCodeGen = Activator.CreateInstance(item) as IComponentDeclarator;
                dec.Add(getCodeGen);
            }
        }

        return dec;
    }
    void CreateDeclaratorFile(DeclaringComponent dec)
    {
        //this will make sure that the component declaration we just made gets created in file.
        string cpath = Application.dataPath + "/GenComponents";
        Directory.CreateDirectory(cpath);
        string DataPath = cpath + "/" + dec._ComponentName + "Declaration.cs";
        FlushComponentDeclarator(dec, DataPath);
    }

    void FlushComponentDeclarator(DeclaringComponent dec, string DataPath)
    {
        string final = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataPath);
        using (StreamWriter W = new StreamWriter(final, false))
        {
            StrFor F = new StrFor(W);
            List<(string, string)> NormalFields = new List<(string, string)>();
            List<(bool, bool)> AttributesNormalFields = new List<(bool, bool)>();
            List<(string, string)> HiddenFields = new List<(string, string)>();
            List<(bool, bool)> AttributesHiddenFields = new List<(bool, bool)>();
            List<bool> ModAtt = new List<bool>();
            for (int i = 0; i < dec._AllGathered.Count; i++)
            {
                if (dec._AllGathered[i]._IsHidden)
                {
                    HiddenFields.Add(ValueTuple.Create<string, string>(dec._AllGathered[i]._Type, dec._AllGathered[i]._Name));
                    AttributesHiddenFields.Add(ValueTuple.Create<bool, bool>(dec._AllGathered[i]._IsSimpleObserver, dec._AllGathered[i]._IsComplexObserver));
                }
                else
                {
                    NormalFields.Add(ValueTuple.Create<string, string>(dec._AllGathered[i]._Type, dec._AllGathered[i]._Name));
                    AttributesNormalFields.Add(ValueTuple.Create<bool, bool>(dec._AllGathered[i]._IsSimpleObserver, dec._AllGathered[i]._IsComplexObserver));
                }
                ModAtt.Add(dec._AllGathered[i]._IsModifyable);
            }

            F.NL("using System.Collections.Generic;");
            F.NL("using System;");
            F.NL("public class " + dec._ComponentName + "Declaration : IComponentDeclarator");
            F.OpenFunction();
            F.NL("public int GetContextID() { return " + dec._ContextID + "; }");

            F.NL("public List<bool> GetModify() { return new List<bool>() { ");
            for (int i = 0; i < ModAtt.Count; i++)
            {
                string TT = ModAtt[i].ToString().ToLower();
                if (i + 1 < ModAtt.Count)
                {
                    TT += ",";
                }
                F.NL(TT);

            }
            F.NL("};}");


            F.NL("public List<(bool, bool)> GetFieldsAttributes()");
            F.OpenFunction();
            if (AttributesNormalFields.Count == 0)
            {
                F.NL("return null;");
            }
            else
            {
                F.NL("return new List<(bool, bool)>()");
                F.OpenFunction();
                for (int i = 0; i < AttributesNormalFields.Count; i++)
                {
                    string TT = "ValueTuple.Create<bool, bool>(" + AttributesNormalFields[i].Item1.ToString().ToLower() + ", " + AttributesNormalFields[i].Item2.ToString().ToLower() + ")";
                    if (i + 1 < AttributesNormalFields.Count)
                    {
                        TT += ",";
                    }
                    F.NL(TT);
                }
                F.CloseFunctionPeriod();
            }
            F.CloseFunction();




            F.NL("public string GetComment() { return \""+dec._Comment+"\"; }");
            F.NL("public string GetComponentName() { return \"" + dec._ComponentName + "\"; }");
            F.NL("public List<(string, string)> GetFields()");
            F.OpenFunction();
            if (NormalFields.Count == 0)
            {
                F.NL("return null;");
            }
            else
            {
                F.NL("return new List<(string, string)>()");
                F.OpenFunction();
                for (int i = 0; i < NormalFields.Count; i++)
                {
                    string TT = "this.Field(\"" + NormalFields[i].Item1 + "\", \"" + NormalFields[i].Item2 + "\")";
                    if (i + 1 < NormalFields.Count)
                    {
                        TT += ",";
                    }
                    F.NL(TT);
                }
                F.CloseFunctionPeriod();
            }
            F.CloseFunction();


            F.NL("public List<(bool, bool)> GetHiddenFieldsAttributes()");
            F.OpenFunction();
            if (AttributesHiddenFields.Count == 0)
            {
                F.NL("return null;");
            }
            else
            {
                F.NL("return new List<(bool, bool)>()");
                F.OpenFunction();
                for (int i = 0; i < AttributesHiddenFields.Count; i++)
                {
                    string TT = "ValueTuple.Create<bool, bool>(" + AttributesHiddenFields[i].Item1.ToString().ToLower() + ", " + AttributesHiddenFields[i].Item2.ToString().ToLower() + ")";
                    if (i + 1 < AttributesHiddenFields.Count)
                    {
                        TT += ",";
                    }
                    F.NL(TT);
                }
                F.CloseFunctionPeriod();
            }
            F.CloseFunction();





            F.NL("public List<(string, string)> HiddenFields()");
            F.OpenFunction();
            if (HiddenFields.Count == 0)
            {
                F.NL("return null;");
            }
            else
            {
                F.NL("return new List<(string, string)>()");
                F.OpenFunction();
                for (int i = 0; i < HiddenFields.Count; i++)
                {
                    string TT = "this.Field(\"" + HiddenFields[i].Item1 + "\", \"" + HiddenFields[i].Item2 + "\")";
                    if (i + 1 < HiddenFields.Count)
                    {
                        TT += ",";
                    }
                    F.NL(TT);
                }
                F.CloseFunctionPeriod();
            }
            F.CloseFunction();
            F.CloseFunction();
            W.Flush();
            W.Close();
        }
    }
}
