using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class OnEvents : MonoBehaviour
{
    public static OnEvents Add(GameObject Go)
    {
        OnEvents Look = Go.GetComponent<OnEvents>();
        if (!Look)
        {
            Look = Go.AddComponent<OnEvents>();
        }
        else
        {
            Look.CleanEvents();
        }
        return Look;
    }

    public OnEvents Enabled(Action Add)
    {
        OnceEnabled += Add;
        return this;
    }

    public OnEvents Disabled(Action Add)
    {
        OnceDisabled += Add;
        return this;
    }

    public OnEvents Destroyed(Action Add)
    {
        OnceDestroyed += Add;
        return this;
    }

    public OnEvents CollidedEnter2D(Action<Collision2D> Add)
    {
        OnceCollided2D += Add;
        return this;
    }

    void OnEnable()
    {
        if(OnceEnabled != null)
        {
            OnceEnabled();
        }
    }

    void OnDisable()
    {
        if(OnceDisabled != null)
        {
            OnceDisabled();
        }
    }

    void OnDestroy()
    {
        if(OnceDestroyed != null)
        {
            OnceDestroyed();
        }
    }

    void OnCollisionEnter2D(Collision2D Col)
    {
        if(OnceCollided2D != null)
        {
            OnceCollided2D(Col);
        }
    }



    private event Action OnceEnabled;
    private event Action OnceDisabled;
    private event Action OnceDestroyed;
    private event Action<Collision2D> OnceCollided2D;

    public OnEvents CleanEvents()
    {
        OnceEnabled = null;
        OnceDisabled = null;
        OnceDestroyed = null;
        OnceCollided2D = null;
        return this;
    }

   
}

