using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class RectExtensions
{
    public static void VerticalSplit(this Rect Self, out Rect Left, out Rect Right, float Balance = 50.0f)
    {
        if(Balance<1)
        {
            Balance = 1;
        }
        // Self
        float Ratio =  Balance / 100.0f;
       
        Left = new Rect(Self.x, Self.y, Self.width*Ratio, Self.height);
        
        Right = new Rect(Left.width, Left.y, Self.width-Left.width, Self.height);
    }

    public static void HorizontalSplit(this Rect Self, out Rect Top, out Rect Bot, float Balance = 50.0f)
    {
        if (Balance < 1)
        {
            Balance = 1;
        }
        // Self
        float Ratio = Balance / 100.0f;

        Top = new Rect(Self.x, Self.y, Self.width , Self.height * Ratio);

        Bot = new Rect(Top.x, Top.height, Self.width, Self.height - Top.height);
    }

    //public static void SplitIn3Vertical(this Rect Self, out Rect one, out Rect two, out Rect three)
    //{
    //    VerticalSplit(Self, out one, out two, 33.33f);
    //    VerticalSplit(two, out two, out three);
    //}


    public static Vector3 RandomPerimeterPoint(this Rect F)
    {
        if (F.height == 0 || F.width == 0) { return new Vector3(9000.0f, 0.0f, 0.0f); }
        float RectLenght = F.width * 2.0f + F.height * 2.0f;
        float RandVal = RueMath.RandomRange(0, RectLenght);
        //return F.max;
        //decide which size

        if (!RueMath.InBetween(RandVal, 0, F.width))
        {
            RandVal -= F.width;
            if (!RueMath.InBetween(RandVal, 0, F.height))
            {
                RandVal -= F.height;
                if (!RueMath.InBetween(RandVal, 0, F.width))
                {
                    RandVal -= F.width;
                    //left side
                    Vector3 Start = F.min;
                    Vector3 End = new Vector3(Start.x, Start.y + F.height, 0.0f);
                    float Ratio = RandVal / F.height;
                    if (Ratio > 1) {  Ratio = 1; }
                    return RueMath.Lerp(Start, End, Ratio);

                }
                else
                {
                    //bottom
                    Vector3 Start = F.max + new Vector2(0.0f, -F.height);
                    Vector3 End = new Vector3(Start.x - F.width, Start.y, 0.0f);
                    float Ratio = RandVal / F.width;
                    if (Ratio > 1) {  Ratio = 1; }
                    return RueMath.Lerp(Start, End, Ratio);
                }
            }
            else
            {
                //Right side
                Vector3 Start = F.max;
                Vector3 End = new Vector3(Start.x, Start.y - F.height, 0.0f); //could be -, we have to see
                float Ratio = RandVal / F.height;
                if (Ratio > 1) {  Ratio = 1; }
                return RueMath.Lerp(Start, End, Ratio);
            }
        }
        else
        {
            //top
            Vector3 Start = F.min + new Vector2(0.0f, F.height);
            Vector3 End = new Vector3(Start.x + F.width, Start.y, 0.0f);
            float Ratio = RandVal / F.width;
            if (Ratio > 1) { Ratio = 1; }
            return RueMath.Lerp(Start, End, Ratio);
        }
    }

    public static Vector3 GivenPerimeterPoint(this Rect F, float Percentage)
    {
        if(F.height == 0 || F.width == 0) {return new Vector3(9000.0f, 0.0f, 0.0f); }
        if (Percentage > 100) { Percentage = 100; }
        else if (Percentage < 0) { Percentage = 0; }
        float RectLenght = F.width * 2.0f + F.height * 2.0f;
        float RandVal = RectLenght * Percentage / 100.0f;
        //return F.max;
        //decide which size

        if (!RueMath.InBetween(RandVal, 0, F.width))
        {
            RandVal -= F.width;
            if (!RueMath.InBetween(RandVal, 0, F.height))
            {
                RandVal -= F.height;
                if (!RueMath.InBetween(RandVal, 0, F.width))
                {
                    RandVal -= F.width;
                    //left side
                    Vector3 Start = F.min;
                    Vector3 End = new Vector3(Start.x, Start.y + F.height, 0.0f);
                    float Ratio = RandVal / F.height;
                    if (Ratio > 1) { Ratio = 1; }
                    else if(Ratio < 0) { Ratio = 0; }
                    return RueMath.Lerp(Start, End, Ratio);

                }
                else
                {
                    //bottom
                    Vector3 Start = F.max + new Vector2(0.0f, -F.height);
                    Vector3 End = new Vector3(Start.x - F.width, Start.y, 0.0f);
                    float Ratio = RandVal / F.width;
                    if (Ratio > 1) {  Ratio = 1; }
                    return RueMath.Lerp(Start, End, Ratio);
                }
            }
            else
            {
                //Right side
                Vector3 Start = F.max;
                Vector3 End = new Vector3(Start.x, Start.y - F.height, 0.0f); //could be -, we have to see
                float Ratio = RandVal / F.height;
                if (Ratio > 1) {  Ratio = 1; }
                return RueMath.Lerp(Start, End, Ratio);
            }
        }
        else
        {
            //top
            Vector3 Start = F.min + new Vector2(0.0f, F.height);
            Vector3 End = new Vector3(Start.x + F.width, Start.y, 0.0f);
            float Ratio = RandVal / F.width;
            if (Ratio > 1) {  Ratio = 1; }
            return RueMath.Lerp(Start, End, Ratio);
        }
    }

    public static Vector3 RandomPointWithin(this Rect F)
    {
        float X = RueMath.RandomRange(F.xMin, F.xMax);
        float Y = RueMath.RandomRange(F.yMin, F.yMax);
        return new Vector3(X, Y, 0.0f);
    }
}


