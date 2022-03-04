using System;
using System.Collections.Generic;
using UnityEngine;

public static class Collider2DExtensions
{
    /// <summary>
    /// Overlaps the bounds of both the box colliders
    /// </summary>
    /// <param name="First"></param>
    /// <param name="Other"></param>
    /// <returns></returns>
    public static bool Overlaps(this Collider2D First, Collider2D Other)
    {
        if (First is BoxCollider2D)
        {
            if (Other is BoxCollider2D)
            {
                return ((BoxCollider2D)First).Overlaps((BoxCollider2D)Other);
            }
            else
            {
                return ((BoxCollider2D)First).Overlaps((CircleCollider2D)Other);
            }
        }
        else
        {
            if (Other is BoxCollider2D)
            {
                return ((CircleCollider2D)First).Overlaps((BoxCollider2D)Other);
            }
            else
            {
                return ((CircleCollider2D)First).Overlaps((CircleCollider2D)Other);
            }
        }
    }

    public static bool Overlaps(this BoxCollider2D First, BoxCollider2D Other)
    {
        return First.bounds.Intersects(Other.bounds);// || Box.bounds.Contains(Other.bounds.center);
    }

    public static bool Overlaps(this BoxCollider2D First, CircleCollider2D Other)
    {
        Vector2 CenterA = First.bounds.center;
        Vector2 CenterB = Other.bounds.center;

        float RecWidth = First.size.x / 2.0f;
        float RecHeight = First.size.y / 2.0f;

        float CircleRadius = Other.radius;

        float XDistance = RueMath.Abs(CenterA.x - CenterB.x);
        float YDistance = RueMath.Abs(CenterA.y - CenterB.y);

        if (XDistance > (RecWidth + CircleRadius))
        {
            return false;
        }
        if (YDistance > (RecHeight + CircleRadius))
        {
            return false;
        }
        if (XDistance <= RecWidth)
        {
            return true;
        }
        if (YDistance <= RecHeight)
        {
            return true;
        }

        float FirstExpo = XDistance - RecWidth;
        float SecondExpo = YDistance - RecHeight;
        FirstExpo *= FirstExpo;
        SecondExpo *= SecondExpo;

        return FirstExpo + SecondExpo <= RueMath.Square(CircleRadius);
    }

    public static bool Overlaps(this CircleCollider2D Other, BoxCollider2D First)
    {
        Bounds A = First.bounds;
        Bounds B = Other.bounds;

        Vector2 CenterA = A.center;
        Vector2 CenterB = B.center;

        float RecWidth = First.size.x;
        float RecHeight = First.size.y;

        float CircleRadius = Other.radius;

        float XDistance = RueMath.Abs(CenterA.x - CenterB.x);
        float YDistance = RueMath.Abs(CenterA.y - CenterB.y);

        if (XDistance > (RecWidth / 2.0f + CircleRadius))
        {
            return false;
        }
        if (YDistance > (RecHeight / 2.0f + CircleRadius))
        {
            return false;
        }
        if (XDistance <= RecWidth / 2.0f)
        {
            return true;
        }
        if (YDistance <= RecHeight / 2.0f)
        {
            return true;
        }

        float FirstExpo = XDistance - RecWidth / 2.0f;
        float SecondExpo = YDistance - RecHeight / 2.0f;
        FirstExpo *= FirstExpo;
        SecondExpo *= SecondExpo;

        return FirstExpo + SecondExpo <= RueMath.Square(CircleRadius);
    }

    public static bool Overlaps(this CircleCollider2D First, CircleCollider2D Other)
    {
        float RSquare = First.radius * First.radius;
        float RSquare2 = Other.radius * Other.radius;
        float SqrDist = RueMath.SqrDistance(First.bounds.center, Other.bounds.center);
        return SqrDist <= RSquare + RSquare2;
    }

    public static Rect Bounds2D(this BoxCollider2D Element)
    {
        Bounds F = Element.bounds;
        //if (F.size == Vector3.zero) //off, the bounds can have size 0 even tho it is not.
        //{
        //    return new Rect(Element.transform.position - Element.size.ToVec3() * 0.5f, Element.size);
        //}
        //else
        {
            return new Rect(F.min, F.size);
        }
    }

    public static bool Intersects(this CircleCollider2D Element, CircleCollider2D Other)
    {
        return Element.IsTouching(Other);
    }

    public static bool Intersects(this CircleCollider2D Element, BoxCollider2D Other)
    {
        Rect Rec = Other.Bounds2D();
        float RectHalfWidth = Rec.width * 0.5f;
        float RectHalfHeight = Rec.height * 0.5f;
        float RecCenterX = Rec.center.x;
        float RecCenterY = Rec.center.y;
        Vector3 Center = Element.bounds.center;
        float _Radius = Element.radius;
        float CircleDistanceX = Center.x - RecCenterX;
        float CircleDistanceY = Center.y - RecCenterY;

        if (CircleDistanceX < 0)
        {
            CircleDistanceX = -CircleDistanceX;
        }
        if (CircleDistanceY < 0)
        {
            CircleDistanceY = -CircleDistanceY;
        }

        if (CircleDistanceX > (RectHalfWidth + _Radius))
        {
            return false;
        }
        if (CircleDistanceY > (RectHalfHeight + _Radius))
        {
            return false;
        }
        if (CircleDistanceX <= (RectHalfWidth))
        {
            return true;
        }
        if (CircleDistanceY <= (RectHalfHeight))
        {
            return true;
        }
        float FirstExpo = CircleDistanceX - RectHalfWidth;
        float SecondExpo = CircleDistanceY - RectHalfHeight;
        FirstExpo *= FirstExpo;
        SecondExpo *= SecondExpo;

        return FirstExpo + SecondExpo <= RueMath.Square(_Radius);
    }

    /*
     * public function CheckVsRec(Rec:RueRectangle):Bool
	{
		var RecCenterX:Float = Rec.X + Rec.HalfWidth;
		var RecCenterY:Float = Rec.Y + Rec.HalfHeight;
		
		var circleDistanceX:Float = _X - RecCenterX;
		var circleDistanceY:Float = _Y - RecCenterY;
		
		if (circleDistanceX < 0)
		{
			circleDistanceX = -circleDistanceX;
		}
		if (circleDistanceY < 0)
		{
			circleDistanceY = -circleDistanceY;
		}
		
		if (circleDistanceX > (Rec.HalfWidth + _Radius))
		{
			return false;
		}
		if (circleDistanceY > (Rec.HalfHeight + _Radius))
		{
			return false;
		}
		if (circleDistanceX <= (Rec.HalfWidth))
		{
			return true;
		}
		if (circleDistanceY <= (Rec.HalfHeight))
		{
			return true;
		}
		
		var FirstExpo:Float = circleDistanceX - Rec.HalfWidth;
		var SecondExpo:Float = circleDistanceY - Rec.HalfHeight;
		FirstExpo *= FirstExpo;
		SecondExpo *= SecondExpo;
		
		return FirstExpo + SecondExpo <= RueMath.Square(_Radius);
	}
     * */


   

}

