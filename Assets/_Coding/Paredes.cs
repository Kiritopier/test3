using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paredes : MonoBehaviour
{
    public bool player1Goal;
    public GameObject gamemanager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pelota"))
        {
            if(player1Goal)
            {
                gamemanager.GetComponent<MainContador>().Player1Scored();
            }

        }
    }
}
