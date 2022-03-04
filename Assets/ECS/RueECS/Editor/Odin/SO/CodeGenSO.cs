using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class CodeGenSO: ScriptableObject
{
    //public string SafeCopy = "";
    void OnEnable()
    {
        AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
    }

    void OnDisable()
    {
        AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
        AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
    }

    public void OnBeforeAssemblyReload()
    {
        //try
        //{
        //    //string DataPath = Application.dataPath + "/ECSGameData.cs";
        //    //string final = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataPath);
        //    //if (File.Exists(final))
        //    //{
        //    //    SafeCopy = File.ReadAllText(final);
        //    //}
        //    //Debug.Log("Before Assembly Reload");
        //    ExtendedRueECSFrameWorkGenerator G = new ExtendedRueECSFrameWorkGenerator();
        //    G.Commit();
        //}
        //catch(Exception e)
        //{
        //    Debug.Log("Exception in onbeforeassemblyreload");
        //}
    }

    public void OnAfterAssemblyReload()
    {
        //try
        //{
        //    AssetDatabase.Refresh();
        //    // Debug.Log("After Assembly Reload");
        //}
        //catch (Exception e)
        //{
        //    Debug.Log("Exception in onafterassemblyreload");
        //}
    }
}