using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TMPExtensions 
{
    /// <summary>
    /// Used for single words or short sentences inlined.
    /// </summary>
    /// <param name="Subject">The Textmesh we are going to be modifying</param>
    /// <param name="From">From this starting color</param>
    /// <param name="To">Towards this end color</param>
    /// <param name="Duration">How long it will take to do this "animation"</param>
    /// <param name="OnFinish">Callback for when the animation finishes</param>
    /// <param name="RemainerDelay">Before executing OnFinish, it will wait this many seconds.</param>
    /// <returns></returns>
    public static RTween.RueTweener InlinedKaraoke(this TextMeshPro Subject, Color From, Color To, float Duration, System.Action OnFinish, float RemainerDelay = 0.0f)
    {
        TMP_TextInfo Info = Subject.textInfo;
        if (Info.wordCount == 0) { Debug.LogError("Attempting to karaoke an empty text mesh pro element"); return null; }
        PaintText(Subject, From);

        RTween.RueTweener ToReturn = RueTween.Generic(Duration, (x) =>
        {
            float Remainder = RueMath.Lerp(0, Info.characterCount, x);
            int PaintedMax = Mathf.FloorToInt(Remainder);
            float Inbetween = Remainder - PaintedMax;
            for (int i = 0; i < PaintedMax; ++i)
            {
                int QuadInitialVertex = Info.characterInfo[i].vertexIndex;
                int MeshIndex = Info.characterInfo[i].materialReferenceIndex;
                Color32[] VertexColors = Info.meshInfo[MeshIndex].colors32;
                VertexColors[QuadInitialVertex] =
                VertexColors[QuadInitialVertex + 1] =
                VertexColors[QuadInitialVertex + 2] =
                VertexColors[QuadInitialVertex + 3] = To;
            }

            if (Inbetween > 0.0f)
            {
                int QuadInitialVertex = Info.characterInfo[PaintedMax].vertexIndex;
                int MeshIndex = Info.characterInfo[PaintedMax].materialReferenceIndex;
                if (PaintedMax <= MeshIndex) // spaces apparently count as 0, fun stuff!
                {
                    Color32[] VertexColors = Info.meshInfo[MeshIndex].colors32;
                    VertexColors[QuadInitialVertex] =
                    VertexColors[QuadInitialVertex + 1] =
                    VertexColors[QuadInitialVertex + 2] =
                    VertexColors[QuadInitialVertex + 3] = RueMath.Lerp(From, To, Inbetween);
                }
            }

            Subject.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        }, Subject.gameObject);
        if (RemainerDelay > 0.0f)
        {
            ToReturn.OnComplete(() =>
            {
                RueTween.Time(RemainerDelay, Subject.gameObject).OnComplete(OnFinish);
            });
        }
        else
        {
            ToReturn.OnComplete(OnFinish);
        }
        return ToReturn;
    }

    public static void PaintText(this TextMeshPro Subject, Color32 To)
    {
        TMP_TextInfo Info = Subject.textInfo;
        for (int i = 0; i < Info.characterCount; ++i)
        {
            if (Info.characterInfo[i].isVisible && Info.characterInfo[i].elementType.Equals(TMPro.TMP_TextElementType.Character))
            {
                int QuadInitialVertex = Info.characterInfo[i].vertexIndex;
                int MeshIndex = Info.characterInfo[i].materialReferenceIndex;
                Color32[] vertexColors = Info.meshInfo[MeshIndex].colors32;
                vertexColors[QuadInitialVertex] =
                vertexColors[QuadInitialVertex + 1] =
                vertexColors[QuadInitialVertex + 2] =
                vertexColors[QuadInitialVertex + 3] = To;
            }
        }
        Subject.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    public static void PaintSingle(this TextMeshPro Subject, int Index, Color32 To)
    {
        TMP_TextInfo Info = Subject.textInfo;
        if (Info.characterInfo[Index].isVisible && Info.characterInfo[Index].elementType.Equals(TMPro.TMP_TextElementType.Character))
        {
            int QuadInitialVertex = Info.characterInfo[Index].vertexIndex;
            int MeshIndex = Info.characterInfo[Index].materialReferenceIndex;
            Color32[] vertexColors = Info.meshInfo[MeshIndex].colors32;
            vertexColors[QuadInitialVertex] =
            vertexColors[QuadInitialVertex + 1] =
            vertexColors[QuadInitialVertex + 2] =
            vertexColors[QuadInitialVertex + 3] = To;

            Subject.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
    }

    public static void PaintText(this TextMeshProUGUI Subject, Color32 To)
    {
        TMP_TextInfo Info = Subject.textInfo;
        for (int i = 0; i < Info.characterCount; ++i)
        {
            if (Info.characterInfo[i].isVisible && Info.characterInfo[i].elementType.Equals(TMPro.TMP_TextElementType.Character))
            {
                int QuadInitialVertex = Info.characterInfo[i].vertexIndex;
                int MeshIndex = Info.characterInfo[i].materialReferenceIndex;
                Color32[] vertexColors = Info.meshInfo[MeshIndex].colors32;
                vertexColors[QuadInitialVertex] =
                vertexColors[QuadInitialVertex + 1] =
                vertexColors[QuadInitialVertex + 2] =
                vertexColors[QuadInitialVertex + 3] = To;
            }
        }
        Subject.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    public static void PaintSingle(this TextMeshProUGUI Subject, int Index, Color32 To)
    {
        TMP_TextInfo Info = Subject.textInfo;

        if (Info.characterInfo[Index].isVisible && Info.characterInfo[Index].elementType.Equals(TMPro.TMP_TextElementType.Character))
        {

            int QuadInitialVertex = Info.characterInfo[Index].vertexIndex;
            int MeshIndex = Info.characterInfo[Index].materialReferenceIndex;
            Color32[] vertexColors = Info.meshInfo[MeshIndex].colors32;
            vertexColors[QuadInitialVertex] =
            vertexColors[QuadInitialVertex + 1] =
            vertexColors[QuadInitialVertex + 2] =
            vertexColors[QuadInitialVertex + 3] = To;
        }
        else
        {
            Debug.Log("No painting : " + Index);
        }

        Subject.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

}
