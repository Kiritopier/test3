using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteColorGroup : MonoBehaviour
{
    public List<SpriteRenderer> _Renderers = new List<SpriteRenderer>();
    public List<TextMeshPro> _Texts = new List<TextMeshPro>();
    float _CurrentAlpha = -1.0f;
    public bool _IgnoreOnEnableColor = false;

    private void OnEnable()
    {
        if(!_IgnoreOnEnableColor)
        SetColor(Color.white);
    }

    public void SetColor(Color Given)
    {
        for (int i = 0; i < _Renderers.Count; i++)
        {
            if(_Renderers[i]!=null)
            _Renderers[i].color = Given;
        }
        for (int i = 0; i < _Texts.Count; i++)
        {
            if(_Texts[i]!=null)
            _Texts[i].color = Given;
        }
        _CurrentAlpha = Given.a;
    }

    public void SetAlpha(float Alpha)
    {
        if(_CurrentAlpha==Alpha)
        {
            return;
        }
        _CurrentAlpha = Alpha;
        for (int i = 0; i < _Renderers.Count; i++)
        {
            if (_Renderers[i] != null)
            {
                Color Cache = _Renderers[i].color;
                Cache.a = Alpha;
                _Renderers[i].color = Cache;
            }
        }
        for (int i = 0; i < _Texts.Count; i++)
        {
            if (_Texts[i] != null)
            {
                Color Cache = _Texts[i].color;
                Cache.a = Alpha;
                _Texts[i].color = Cache;
            }
        }
    }

    public void SetColorOnly(float R, float G, float B)
    {
        for (int i = 0; i < _Renderers.Count; i++)
        {
            Color Cache = _Renderers[i].color;
            Cache.r = R;
            Cache.b = B;
            Cache.g = G;
            _Renderers[i].color = Cache;
        }
        for (int i = 0; i < _Texts.Count; i++)
        {
            Color Cache = _Texts[i].color;
            Cache.r = R;
            Cache.b = B;
            Cache.g = G;
            _Texts[i].color = Cache;
        }
    }
}
