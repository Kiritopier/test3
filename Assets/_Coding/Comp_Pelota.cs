using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp_Pelota : MonoBehaviour
{
    public float speed = 100.0f;
    private Vector2 startpos;
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
        float x = Random.Range(0 ,2) == 0 ? -1 : 1; 
        float y = Random.Range(0 ,2) == 0 ? -1 : 1;

        rb.velocity = new Vector2(speed*x,speed*y);
    }
}
