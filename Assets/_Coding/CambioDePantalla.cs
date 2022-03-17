using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDePantalla : MonoBehaviour
{
    public void LoadLevel(string Nombrenivel)
    {
        
        {
            SceneManager.LoadScene(Nombrenivel);
        }
    }
}
