using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudMenu : MonoBehaviour
{
    [SerializeField] GameObject hudPause;
    [SerializeField] GameObject opciones;
    [SerializeField] GameObject muerte;
    [SerializeField] GameObject MenuController;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject NoPartidaGuardada;
    [SerializeField] GameObject Volumen;
    [SerializeField] GameObject Graphics;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            hudPause.SetActive(!hudPause.activeSelf);


            if (hudPause.activeSelf == false)
            {
                Resume();

                PauseMenu.SetActive(true);
                opciones.SetActive(false);
                NoPartidaGuardada.SetActive(false);
                if (Volumen.activeSelf == true)
                {
                    MenuController.GetComponent<MainMenuScript>().DescartarVolumen();
                    Volumen.SetActive(false);

                }
                if (Graphics.activeSelf == true)
                {
                    MenuController.GetComponent<MainMenuScript>().DescartarGraficos();
                    Graphics.SetActive(false);

                }
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
