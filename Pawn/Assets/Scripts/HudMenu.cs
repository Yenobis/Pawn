using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudMenu : MonoBehaviour
{
    [SerializeField] GameObject opciones;
    [SerializeField] GameObject muerte;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            opciones.SetActive(!opciones.activeSelf);
            if(opciones.activeSelf == false)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }
    public void Resume()
    {
        Time.timeScale = 1f;

    }
    void Pause()
    {
        Time.timeScale = 0f;
    }
}
