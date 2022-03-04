using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using RueECS;
using UnityEditor;
public interface ICodeGenRueECS
{
    void DeclareUsing(StrFor generator);
    void CodeGen(StrFor generator);
    //void CodeGen(List<(Type, IsGameSystemAttribute)> data, StrFor generator);
    //void CodeGenSimple(StrFor g);
    //
    //void CodeGenComponents(List<object> Components);
    
}