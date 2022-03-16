using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainContador : MonoBehaviour
{
    public GameObject pelota;
    
    public GameObject player1;
    public GameObject player1Goal;

    public GameObject player2;
    public GameObject player2Goal;

    public Text player1Text;
    public Text player2Text;

    private int playerScore;
    private int playerScore2;

    public bool IA;

    public void Player1Scored()
    {
        playerScore++;
        player1Text.text = playerScore.ToString();
        ResetPosition();
    }

    public void Player2Scored()
    {
        playerScore2++;
        player2Text.text = playerScore2.ToString();
        ResetPosition();
    }

    private void ResetPosition()
    {
        
       
        if (IA)
        {
            pelota.GetComponent<Comp_Pelota>().Reseteo();
            
        }
       
        
    }
    

}
