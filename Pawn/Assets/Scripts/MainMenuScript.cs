using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Niveles a Cargar")]
    private string partidaAcargar;
    [SerializeField] private GameObject noGameSaved = null;
    [SerializeField] private GameObject newGame = null;

    public string _escenaAcargar;
    [Header("Volumen")]
    [SerializeField] private TMP_Text volumeTextField = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private float defVolumen = 1;
    public void PlayGame(){
        SceneManager.LoadScene(_escenaAcargar);
    }

    public void QuitGame()
    {

        Debug.Log("quit");
        Application.Quit();
    }
    public void CargarLevel()
    {
        if (PlayerPrefs.HasKey("Partida_Guardada"))
        {
            partidaAcargar = PlayerPrefs.GetString("Partida_Guardada");
            SceneManager.LoadScene(partidaAcargar);
        }
        else
        {
            noGameSaved.SetActive(true);

        }

    }
    public void SetVolumen(float volume)
    {
        AudioListener.volume = volume;
        volumeTextField.text = (volume*100).ToString("0.0");
    }
    public void AplicarVolumen()
    {
        PlayerPrefs.SetFloat("volumenGlobal",AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }
    public void ResetButtom(string MenuType)
    {

        if(MenuType == "Audio")
        {
            AudioListener.volume = defVolumen;
            volumeSlider.value = defVolumen;
            volumeTextField.text = (defVolumen*100).ToString("0.0");
            AplicarVolumen();
        }
    }
    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
