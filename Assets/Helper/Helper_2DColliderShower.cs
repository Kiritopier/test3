using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class Helper_2DColliderShower : MonoBehaviour
{
    public PolygonCollider2D[] All;
    public bool Refresh = false;
   

    void OnDrawGizmos()
    {
        if(Refresh)
        {
            All = GameObject.FindObjectsOfType<PolygonCollider2D>();
            Refresh = false;
        }
        Gizmos.color = Color.blue;

        for (int x = 0; x < All.Length; x++)
        {
            Vector2[] points = All[x].points;
            Vector3 _t = All[x].transform.position;
            // for every point (except for the last one), draw line to the next point
            for (int i = 0; i < points.Length; i++)
            {
                if (i == points.Length - 1)
                {
                    Gizmos.DrawLine(new Vector3(points[i].x + _t.x, points[i].y + _t.y), new Vector3(points[0].x + _t.x, points[0].y + _t.y));
                }
                else
                {
                    Gizmos.DrawLine(new Vector3(points[i].x + _t.x, points[i].y + _t.y), new Vector3(points[i + 1].x + _t.x, points[i + 1].y + _t.y));
                }
            }
        }
    }
}

