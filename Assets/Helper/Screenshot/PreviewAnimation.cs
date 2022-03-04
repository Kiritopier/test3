using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class PreviewAnimation: SerializedMonoBehaviour
{
    public Sprite[] Sprites;
    public SpriteRenderer _Sprite;
    public float _PerFrame;
    float Elapsed = 0.0f;
    
    private void Update()
    {
        if(Sprites ==null || Sprites.Length == 0) { return; }
        float max = (_PerFrame * (float)Sprites.Length);
        if(_PerFrame == 0) { return; }
        if(_Sprite == null) { return; }
        Elapsed += Time.deltaTime;
        if(Elapsed > max)
        {
            Elapsed = max;
        }
        _Sprite.sprite = Sprites[(int)RueMath.Lerp(0, Sprites.Length-1, Elapsed /max)];
        if (Elapsed== max)
        {
            Elapsed = 0;
        }
    }
}