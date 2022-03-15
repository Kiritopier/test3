using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Comp_Pelota : MonoBehaviour
{
    public float speed = 100.0f;
    public Vector2 startpos;
    public Rigidbody2D rb;
    public Transform Movimientopelota;
   
    
    public void OnEnable()
    {
        startpos = transform.position;
        rb.velocity = Vector2.zero;
        Inicio();
    }

    public void Inicio()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;

        rb.velocity = new Vector2(speed * x, speed * y);
    }

    

    public void Reseteo()
    {
        transform.position = startpos;
        rb.velocity = Vector2.zero;
        Inicio();
        
    }
    
}
