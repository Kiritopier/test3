using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
public interface IComponentDeclarator
{
    List<(string, string)> GetFields();
    List<(bool, bool)> GetFieldsAttributes();
    List<(string, string)> HiddenFields();
    List<(bool, bool)> GetHiddenFieldsAttributes();
    List<bool> GetModify();
    string GetComponentName();
    int GetContextID();
    string GetComment();
}

public static class CompDeclaratorExt
{
    public static ValueTuple<string,string> Field (this IComponentDeclarator c, string FieldType, string FieldName)
    {
        return ValueTuple.Create<string, string>(FieldType, FieldName);
    }
}