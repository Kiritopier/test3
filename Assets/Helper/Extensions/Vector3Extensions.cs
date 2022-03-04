using System;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector2 ToVec2(this Vector3 obj)
    {
        return new Vector2(obj.x, obj.y);
    }
    public static Vector3 ToVec3(this Vector2 obj)
    {
        return new Vector3(obj.x, obj.y, 0.0f);
    }
    public static void SetX(this Vector3 obj, float NewX)
    {
        obj.x = NewX;
    }

    public static void SetY(this Vector3 obj, float NewY)
    {
        obj.y = NewY;
    }

    public static Vector2 PerpendicularClockwise(this Vector2 vector2)
    {
        return new Vector2(vector2.y, -vector2.x);
    }

    public static Vector2 PerpendicularCounterClockwise(this Vector2 vector2)
    {
        return new Vector2(-vector2.y, vector2.x);
    }

    public static Vector3 PerpendicularClockwise(this Vector3 vector3)
    {
        return new Vector3(vector3.y, -vector3.x, vector3.z);
    }

    public static Vector3 PerpendicularCounterClockwise(this Vector3 vector3)
    {
        return new Vector3(-vector3.y, vector3.x, vector3.z);
    }

}

