using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AlignmentLink))]
public class AlignmentLinkAddOncs : Editor
{
    public override void OnInspectorGUI()
    {
        AlignmentLink Setter = (AlignmentLink)target;
        base.OnInspectorGUI();
        if (GUILayout.Button(new GUIContent("CaptureChildren")))
        {
            CaptureChildren(Setter);
        }
        if (Setter._ShouldAutoUpdate)
        {
            AlignTo(Setter);
        }
        else
        {
            if (GUILayout.Button(new GUIContent("AlignFromMe")))
            {
                AlignTo(Setter);
            }
        }
        if (GUILayout.Button(new GUIContent("ReverseChildren")))
        {
            Setter._Children.Reverse();
        }
        if (GUILayout.Button(new GUIContent("SortSpriteRenderersByY")))
        {
            var sprites = Setter.GetComponentsInChildren<SpriteRenderer>();
            List<SpriteRenderer> Sorted = new List<SpriteRenderer>(sprites);
            Sorted.Sort((x, y) =>
            {
                return x.transform.position.y.CompareTo(y.transform.position.y);
            });
            Sorted.Reverse();
            for (int i = 0; i < Sorted.Count; i++)
            {
                Debug.Log(Sorted[i].transform.position.y);
            }
            float LastY = float.MaxValue;
            int Starting = Setter._StartSort - 1;
            for (int i = 0; i < Sorted.Count; i++)
            {
                var curr = Sorted[i];
                if(LastY > curr.transform.position.y)
                {
                    LastY = curr.transform.position.y;
                    Starting++;
                }

                Sorted[i].sortingOrder = Starting;
            }
        }
    }

    

    void CaptureChildren(AlignmentLink Setter)
    {
        Setter._Children.Clear();

        int c = Setter.transform.childCount;
        for (int i = 0; i < c; i++)
        {
            var cc = Setter.transform.GetChild(i).GetComponent<AlignmentLink>();
            if(cc == null) { continue; }
            Setter._Children.Add(cc);
        }

        //Setter.GetComponentsInChildren<AlignmentLink>(true, Setter._Children);
        if (Setter._Children.Contains(Setter))
        {
            Setter._Children.Remove(Setter); //in case this happens, but lets test it
        }
        //Setter._Children.Reverse();
    }

    void AlignTo(AlignmentLink P)
    {
        switch (P._Rule)
        {
            case AlignmentRule.NONE:
                return; //does nothing
            case AlignmentRule.VERTICAL:
                VerticalAlign(P);
                break;
            case AlignmentRule.HORIZONTAL:
                HorizontalAlign(P);
                break;
            case AlignmentRule.RADIAL:
                RadialAlign(P);
                break;
            default:
                break;
        }
    }

    Vector3 GetPosFromSettings(Vector3 Me, AlignmentLink P, Transform From)
    {
        if(P._IgnoreX)
        {
            Me.x = From.position.x;
        }
        if(P._IgnoreY)
        {
            Me.y = From.position.y;
        }
        if(P._IgnoreZ)
        {
            Me.z = From.position.z;
        }

        return Me;
    }

    void VerticalAlign(AlignmentLink P)
    {
        float Cf = P._Children.Count;
        if (Cf > 1)
        {
            if (P._Just == AlignmentJustification.MIDDLE)
            {
                //calculate how many children. Add the data 

                Vector3 InitialP = P.transform.position + new Vector3(0.0f, -P._Separation * Cf * 0.5f, 0.0f);
                Vector3 FinalP = P.transform.position + new Vector3(0.0f, P._Separation * Cf * 0.5f, 0.0f);
                for (int i = 0; i < Cf; i++)
                {
                    P._Children[i].transform.position = GetPosFromSettings( RueMath.Lerp(InitialP, FinalP, ((float)i) / (Cf - 1f)), P, P._Children[i].transform);
                }
            }
            else if (P._Just == AlignmentJustification.LEFT_OR_TOP)
            {
                //fromt top to bottom
                Vector3 InitialP = P.transform.position;
                Vector3 FinalP = P.transform.position + new Vector3(0.0f, -P._Separation * Cf, 0.0f);
                for (int i = 0; i < Cf; i++)
                {
                    P._Children[i].transform.position = GetPosFromSettings(RueMath.Lerp(InitialP, FinalP, ((float)i) / (Cf - 1f)), P, P._Children[i].transform);
                }
            }
            else
            {
                Vector3 InitialP = P.transform.position;
                Vector3 FinalP = P.transform.position + new Vector3(0.0f, P._Separation * Cf, 0.0f);
                for (int i = 0; i < Cf; i++)
                {
                    P._Children[i].transform.position = GetPosFromSettings(RueMath.Lerp(InitialP, FinalP, ((float)i) / (Cf - 1f)), P, P._Children[i].transform);
                }
            }
        }
        else
        {
            if (Cf > 0)
            {
                P._Children[0].transform.position = P.transform.position;
            }
        }

        for (int i = 0; i < P._Children.Count; i++)
        {
            AlignTo(P._Children[i]);
        }
    }

    void HorizontalAlign(AlignmentLink P)
    {
        float Cf = P._Children.Count;
        if (Cf > 1)
        {
            if (P._Just == AlignmentJustification.MIDDLE)
            {
                //calculate how many children. Add the data 

                Vector3 InitialP = P.transform.position + new Vector3(-P._Separation * Cf * 0.5f, 0.0f, 0.0f);
                Vector3 FinalP = P.transform.position + new Vector3(P._Separation * Cf * 0.5f, 0.0f, 0.0f);
                for (int i = 0; i < Cf; i++)
                {
                    P._Children[i].transform.position = GetPosFromSettings(RueMath.Lerp(InitialP, FinalP, ((float)i) / (Cf - 1f)), P, P._Children[i].transform);
                }
            }
            else if (P._Just == AlignmentJustification.LEFT_OR_TOP)
            {
                //fromt top to bottom
                Vector3 InitialP = P.transform.position;
                Vector3 FinalP = P.transform.position + new Vector3(-P._Separation * Cf, 0.0f, 0.0f);
                for (int i = 0; i < Cf; i++)
                {
                    P._Children[i].transform.position = GetPosFromSettings(RueMath.Lerp(InitialP, FinalP, ((float)i) / (Cf - 1f)), P, P._Children[i].transform);
                }
            }
            else
            {
                Vector3 InitialP = P.transform.position;
                Vector3 FinalP = P.transform.position + new Vector3(P._Separation * Cf, 0.0f, 0.0f);
                for (int i = 0; i < Cf; i++)
                {
                    P._Children[i].transform.position = GetPosFromSettings(RueMath.Lerp(InitialP, FinalP, ((float)i) / (Cf - 1f)), P, P._Children[i].transform);
                }
            }
        }
        else
        {
            if (Cf > 0)
            {
                P._Children[0].transform.position = GetPosFromSettings(P.transform.position, P, P._Children[0].transform);
            }
        }

        for (int i = 0; i < P._Children.Count; i++)
        {
            AlignTo(P._Children[i]);
        }
    }


    void RadialAlign(AlignmentLink P)
    {
        float Cf = P._Children.Count;
        if (Cf > 1)
        {
            if (P._Just == AlignmentJustification.MIDDLE)
            {
                Vector3 InitialP = P.transform.position;
                float Slice = (2.0f * Mathf.PI) / Cf;
                float rads = Mathf.Rad2Deg * Slice;
                for (float i = 0; i < Cf; i++)
                {
                    P._Children[(int)i].transform.position = InitialP + new Vector3(P._Separation * Mathf.Sin(Slice * i), P._Separation * Mathf.Cos(Slice * i), 0.0f);
                }
            }
            else if (P._Just == AlignmentJustification.LEFT_OR_TOP)
            {
                //fromt top to bottom
                //Vector3 InitialP = P.transform.position;
                //Vector3 FinalP = P.transform.position + new Vector3(-P._Separation * Cf, 0.0f, 0.0f);
                //for (int i = 0; i < Cf; i++)
                //{
                //    P._Children[i].transform.position = RueMath.Lerp(InitialP, FinalP, ((float)i) / (Cf - 1f));
                //}
            }
            else
            {
                //Vector3 InitialP = P.transform.position;
                //Vector3 FinalP = P.transform.position + new Vector3(P._Separation * Cf, 0.0f, 0.0f);
                //for (int i = 0; i < Cf; i++)
                //{
                //    P._Children[i].transform.position = RueMath.Lerp(InitialP, FinalP, ((float)i) / (Cf - 1f));
                //}
            }
        }
        else
        {
            if (Cf > 0)
            {
                P._Children[0].transform.position = P.transform.position;
            }
        }

        for (int i = 0; i < P._Children.Count; i++)
        {
            AlignTo(P._Children[i]);
        }
    }

}
