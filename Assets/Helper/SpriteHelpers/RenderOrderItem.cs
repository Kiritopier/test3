using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class RenderOrderItem : MonoBehaviour
{
    Renderer _Renderer;
    SortingGroup _Group;
    int _Initial;
    bool _IsGroup = false;
    int Agregate = 0;
    private void Awake()
    {
        _IsGroup = false;
        _Renderer = GetComponent<Renderer>();
        _Group = GetComponent<SortingGroup>();
        if(_Renderer!=null && _Group == null)
        {
            _Initial = _Renderer.sortingOrder;
        }
        if(_Group!=null)
        {
            _IsGroup = true;
            _Initial = _Group.sortingOrder;
        }
    }
    private void OnEnable()
    {
        if (_IsGroup)
        {
            _Group.sortingOrder = _Initial + Agregate;
        }
        else
        {
            if (_Renderer != null)
            {
                _Renderer.sortingOrder = _Initial + Agregate;

            }
        }
    }
    public void Offset(int Offset, int Amount)
    {
        Agregate += Offset * Amount;
        if (_IsGroup)
        {
            _Group.sortingOrder = _Initial+Agregate;
        }
        else
        {
            if (_Renderer != null)
            {
                _Renderer.sortingOrder = _Initial + Agregate;

            }
        }
    }
    private void OnDisable()
    {
        Agregate = 0;
        if (_Group != null)
        {
            _Group.sortingOrder = _Initial;
        }
        else
        {
            if (_Renderer != null)
            {
                _Renderer.sortingOrder = _Initial;
            }
        }
    }
}
