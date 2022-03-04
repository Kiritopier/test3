
using System;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsHelper
{
    public static Collider2D[] _ColliderHit = new Collider2D[20];
    public static RaycastHit2D[] _RaycastHit = new RaycastHit2D[20];

    public static bool IntersectsOrWithin(this Rect R, Rect Other)
    {
        if(R.Overlaps(Other))
        {
            return true;
        }
        return false;
    }

    /*public static bool Intersects(this ICircleCollider C, ICircleCollider Other)
    {
       return  ICollideExt.OverlapsCC(C, C.GetOwner().TryGetPosition()._Value, Other._Radius, Other.GetOwner().TryGetPosition()._Value);
    }
    
    public static bool Intersects(this ICircleCollider C, float otherradius, Vector2 pos)
    {
        if (!C.GetOwner().HasPosition()) { 
            
            return false;
        }
        return ICollideExt.OverlapsCC(C, C.GetOwner().TryGetPosition()._Value, otherradius, pos);
    }
    public static bool IntersectsOrWithin(this Rect R, ICircleCollider Other)
    {
        var own = Other.GetOwner();
        var pos = own.TryGetPosition();
        Vector2 circle = pos._Value;
        float radius = Other._Radius;
    
        var distX = Mathf.Abs(circle.x - R.x - R.width / 2.0f);
        var distY = Mathf.Abs(circle.y - R.y - R.height / 2.0f);
    
        if (distX > (R.width / 2.0f + radius)) { return false; }
        if (distY > (R.height / 2.0f + radius)) { return false; }
    
        if (distX <= (R.width / 2.0f)) { return true; }
        if (distY <= (R.height / 2.0f)) { return true; }
    
        var dx = distX - R.width / 2.0f;
        var dy = distY - R.height / 2.0f;
        return (dx * dx + dy * dy <= (radius * radius));
    
    }*/
    /// <summary>
    /// Virtual circle.
    /// </summary>
    /// <param name="R"></param>
    /// <param name="radius"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool IntersectsOrWithin(this Rect R, float radius, Vector2 position)
    {
        Vector2 circle = position;

        var distX = Mathf.Abs(circle.x - R.x - R.width / 2.0f);
        var distY = Mathf.Abs(circle.y - R.y - R.height / 2.0f);

        if (distX > (R.width / 2.0f + radius)) { return false; }
        if (distY > (R.height / 2.0f + radius)) { return false; }

        if (distX <= (R.width / 2.0f)) { return true; }
        if (distY <= (R.height / 2.0f)) { return true; }

        var dx = distX - R.width / 2.0f;
        var dy = distY - R.height / 2.0f;
        return (dx * dx + dy * dy <= (radius * radius));

    }
    public static bool ContainsX(this Rect R, float X)
    {
        return R.xMin <= X && R.xMax >= X;
    }
    public static bool ContainsY(this Rect R, float Y)
    {
        return R.yMin <= Y && R.yMax >= Y;
    }
    public static void SplitIn4(this Rect R, out Rect TopRight, out Rect TopLeft, out Rect BotRight, out Rect BotLeft)
    {

        BotLeft = new Rect(R.x, R.y, R.width * 0.5f, R.height * 0.5f);
        BotRight = new Rect(R.x + R.width * 0.5f, R.y, R.width * 0.5f, R.height * 0.5f);
        TopLeft = new Rect(R.x, R.y + R.height * 0.5f, R.width * 0.5f, R.height * 0.5f);
        TopRight = new Rect(R.x + R.width * 0.5f, R.y + R.height * 0.5f, R.width * 0.5f, R.height * 0.5f);
    }
    public static void SplitIn2WidthWise(this Rect R, out Rect one, out Rect two)
    {
        one = new Rect(R.x, R.y, R.width / 2.0f, R.height);
        two = new Rect(one.xMax, R.y, one.width, R.height);
    }
    public static void SplitIn2HeightWise(this Rect R, out Rect one, out Rect two)
    {
        one = new Rect(R.x, R.y, R.width , R.height/2.0f);
        two = new Rect(R.x, one.yMax, one.width, R.height);
    }

    public static void SplitIn3WidthWise(this Rect R, out Rect one, out Rect two, out Rect three)
    {
        one = new Rect(R.x, R.y, R.width /3.0f, R.height);
        two = new Rect(one.xMax, R.y, one.width, R.height);
        three = new Rect(two.xMax, R.y, one.width, R.height);
    }
    public static void SplitIn4WidthWise(this Rect R, out Rect one, out Rect two, out Rect three, out Rect four)
    {
        one = new Rect(R.x, R.y, R.width /4f, R.height);
        two = new Rect(one.xMax, R.y, one.width, R.height);
        three = new Rect(two.xMax, R.y, one.width, R.height);
        four = new Rect(three.xMax, R.y, one.width, R.height);
    }
    public static void SplitIn5WidthWise(this Rect R, out Rect one, out Rect two, out Rect three, out Rect four, out Rect five)
    {
        one = new Rect(R.x, R.y, R.width /5f, R.height);
        two = new Rect(one.xMax, R.y, one.width, R.height);
        three = new Rect(two.xMax, R.y, one.width, R.height);
        four = new Rect(three.xMax, R.y, one.width, R.height);
        five = new Rect(four.xMax, R.y, one.width, R.height);
    }
    public static void SplitIn6WidthWise(this Rect R, out Rect one, out Rect two, out Rect three, out Rect four, out Rect five, out Rect six)
    {
        R.SplitIn2WidthWise(out one, out four);
        one.SplitIn3WidthWise(out one, out two, out three);
        four.SplitIn3WidthWise(out four, out five, out six);
    }
}

