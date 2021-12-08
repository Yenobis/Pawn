using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudMenu : MonoBehaviour
{
    [Header("Menús")]
    [SerializeField] GameObject hudPause;
    [SerializeField] GameObject opciones;
    [SerializeField] GameObject muerte;
    [SerializeField] GameObject MenuController;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject NoPartidaGuardada;
    [SerializeField] GameObject Volumen;
    [SerializeField] GameObject Graphics;
    [Header("Pawn")]
    [SerializeField] GameObject player;
    private bool isOnDeathScreen;
    // Start is called before the first frame update
    void Start()
    {
        isOnDeathScreen = false;
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
        if (player.GetComponent<PlayerController>().cur_health == 0 && !isOnDeathScreen)
            {
                player.GetComponent<PlayerController>().enabled=false;
                isOnDeathScreen = true;
                Debug.Log("Has muerto");
                StartCoroutine(DeathMenu());

        }

    }

    public IEnumerator DeathMenu()
    {
        yield return new WaitForSeconds(2f);
        Pause();
        player.GetComponent<PlayerController>().enabled = true;
        muerte.SetActive(true);

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
