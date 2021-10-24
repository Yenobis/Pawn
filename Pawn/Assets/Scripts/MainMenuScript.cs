using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame(){

    }

    public void QuitGame()
    {

        Debug.Log("quit");
        Application.Quit();
    }
}
