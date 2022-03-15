using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainContador : MonoBehaviour
{
    public GameObject pelota;
    public GameObject player1;
    public GameObject player1Goal;

    public Text player1Text;
    private int playerScore;

    public void Player1Scored()
    {
        playerScore++;
        player1Text.text = playerScore.ToString();
        ResetPosition();
    }

    private void ResetPosition()
    {
        pelota.GetComponent<Comp_Pelota>().Reseteo();
        
    }
}
