using System;
using System.Collections.Generic;
using UnityEngine;
using RueECS;

public static class TransformExtensions
{
   //public static void Follow(this Transform T, Position P)
   //{
   //    P.__ValueChanged.Add((e, prev, now) =>
   //    {
   //        T.position = e.TryGetPosition()._Value;
   //    });
   //}

    public static void X(this Transform tran, float offsetX = 0.0f)
    {
        Vector3 temp = tran.position;
        temp.x = offsetX;
        tran.position = temp;
    }

    public static void Y(this Transform tran, float offsetY = 0.0f)
    {
        Vector3 temp = tran.position;
        temp.y = offsetY;
        tran.position = temp;
    }

    public static void Z(this Transform tran, float offsetZ = 0.0f)
    {
        Vector3 temp = tran.position;
        temp.z = offsetZ;
        tran.position = temp;
    }
    public static void LocalX(this Transform tran, float offsetX = 0.0f)
    {
        Vector3 temp = tran.localPosition;
        temp.x = offsetX;
        tran.localPosition = temp;
    }

    public static void LocalY(this Transform tran, float offsetY = 0.0f)
    {
        Vector3 temp = tran.localPosition;
        temp.y = offsetY;
        tran.localPosition = temp;
    }

    public static void LocalZ(this Transform tran, float offsetZ = 0.0f)
    {
        Vector3 temp = tran.localPosition;
        temp.z = offsetZ;
        tran.localPosition = temp;
    }

    public static void AddX(this Transform tran, float offsetX = 0.0f)
    {
        Vector3 temp = tran.position;
        temp.x += offsetX;
        tran.position = temp;
    }

    public static void AddY(this Transform tran, float offsetY = 0.0f)
    {
        Vector3 temp = tran.position;
        temp.y += offsetY;
        tran.position = temp;
    }

    public static void AddZ(this Transform tran, float offsetZ = 0.0f)
    {
        Vector3 temp = tran.position;
        temp.z += offsetZ;
        tran.position = temp;
    }
    public static void AddLocalX(this Transform tran, float offsetX = 0.0f)
    {
        Vector3 temp = tran.localPosition;
        temp.x += offsetX;
        tran.localPosition = temp;
    }

    public static void AddLocalY(this Transform tran, float offsetY = 0.0f)
    {
        Vector3 temp = tran.localPosition;
        temp.y += offsetY;
        tran.localPosition = temp;
    }

    public static void SetLocalY(this Transform tran, float offsetY = 0.0f)
    {
        Vector3 temp = tran.localPosition;
        temp.y = offsetY;
        tran.localPosition = temp;
    }

    public static void AddLocalZ(this Transform tran, float offsetZ = 0.0f)
    {
        Vector3 temp = tran.localPosition;
        temp.z += offsetZ;
        tran.localPosition = temp;
    }

    public static void AddVec3(this Transform tran, Vector3 offset)
    {
        Vector3 temp = tran.position;
        temp += offset;
        tran.position = temp;
    }

    public static void AddLocalVec3(this Transform tran, Vector3 offset)
    {
        Vector3 temp = tran.localPosition;
        temp += offset;
        tran.localPosition = temp;
    }

}

