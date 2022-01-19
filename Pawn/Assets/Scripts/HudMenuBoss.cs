using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudMenuBoss : MonoBehaviour
{

    [Header("Men�s")]
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
    [SerializeField] GameObject boss;
    [SerializeField] GameObject menuVictoria;
    private bool isOnDeathScreen;
    public bool victory = false;
    // Start is called before the first frame update
    void Start()
    {
        isOnDeathScreen = false;
        Cursor.visible = false;
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
        if (boss.GetComponent<BossAI>().destruido == true)
        {

            StartCoroutine(EndDemo());
        }

        if (player.GetComponent<PlayerController>().cur_health == 0 && !isOnDeathScreen)
        {
            player.GetComponent<PlayerController>().enabled = false;
            isOnDeathScreen = true;
            muerte.SetActive(true);
            muerte.GetComponent<Animator>().SetBool("muerte", true);
            StartCoroutine(DeathMenu());
        }

    }

    public IEnumerator DeathMenu()
    {

        yield return new WaitForSeconds(3f);
        MenuController.GetComponent<MainMenuScript>().LoadScene("Boss");


    }
    public IEnumerator EndDemo()
    {
        yield return new WaitForSeconds(6.2f);
        victory = true;
        Cursor.visible = true;
        menuVictoria.SetActive(true);
        //GameObject.Find("HUDContinuara").SetActive(true);
    }
    public void Resume()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;

    }
    void Pause()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}
