using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp_paleta : MonoBehaviour
{
    public Transform TransformPaleta;

    public void OnEnable()
    {
        Debug.Log("hola");
        this.AddUpdate((x) =>
        {
            bool arriba = Input.GetKey(KeyCode.UpArrow);
            bool abajo = Input.GetKey(KeyCode.DownArrow);
            Vector3 posicionPaleta = TransformPaleta.position;
            if (arriba)
            {
                posicionPaleta.y += 2.0f * x;
            }

            if (abajo)
            {
                posicionPaleta.y -= 2.0f * x;
            }

            TransformPaleta.position = posicionPaleta;
        });
    }

    public void OnDisable()
    {
        Debug.Log("chao");
    }
}
