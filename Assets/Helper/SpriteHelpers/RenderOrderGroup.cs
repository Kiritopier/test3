using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOrderGroup : MonoBehaviour
{
    List<RenderOrderItem> _Renderers;
    int _Initial;
    private void Awake()
    {
        _Initial = 0;
        _Renderers = new List<RenderOrderItem>(GetComponentsInChildren<RenderOrderItem>(true));
    }
    private void OnEnable()
    {
        _Initial = 0;
    }
    private void OnDisable()
    {
        _Initial = 0;
    }

    public void SetRenderOrder(int Render)
    {
        if (Render == _Initial) { return; }
        int Dif = Render - _Initial;
        for (int i = 0; i < _Renderers.Count; i++)
        {
            _Renderers[i].Offset(Dif, _Renderers.Count);
        }
        _Initial = Render;
    }
}
