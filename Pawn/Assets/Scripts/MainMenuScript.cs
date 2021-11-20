using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuScript : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BackgroundMusic = "BackgroundMusic";
    private static readonly string SfxMusic = "SfxMusic";
    int firstPlayInt = 0;

    // Start is called before the first frame update
    [Header("Niveles a Cargar")]
    private string partidaAcargar;
    [SerializeField] private GameObject noGameSaved = null;
    [SerializeField] private GameObject newGame = null;

    public string _escenaAcargar;
    [Header("Volumen")]
    [SerializeField] private TMP_Text musicTextField = null;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private TMP_Text sfxTextField = null;
    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private AudioSource audioBackground = null;
    [SerializeField] private AudioSource[] audioSFX = null;
    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private float defVolumen = 1.0f;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);
     if (firstPlayInt == 0 )
      {
            musicSlider.value = defVolumen;
            musicTextField.text = (defVolumen * 100).ToString("0.0");
            sfxSlider.value = defVolumen;
            sfxTextField.text = (defVolumen * 100).ToString("0.0");
            AplicarVolumen();
            PlayerPrefs.SetInt(FirstPlay, -1);
      }
     else {
            musicSlider.value =  PlayerPrefs.GetFloat(BackgroundMusic);
            sfxSlider.value = PlayerPrefs.GetFloat(SfxMusic);
            musicTextField.text = (musicSlider.value * 100).ToString("0.0");
            sfxTextField.text = (sfxSlider.value * 100).ToString("0.0");
            AplicarVolumen();
        }
    }
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
    public void SetMusicVolumen(float volume)
    {
        audioBackground.volume = volume;
        musicTextField.text = (volume*100).ToString("0.0");
    }
    public void SetSfxVolumen(float volume)
    {
        for (int i=0; i < audioSFX.Length; ++i)
        {
            audioSFX[i].volume = volume;
        }
        sfxTextField.text = (volume * 100).ToString("0.0");
    }

    public void AplicarVolumen()
    {
        PlayerPrefs.SetFloat(BackgroundMusic, musicSlider.value);
        PlayerPrefs.SetFloat(SfxMusic, sfxSlider.value);
        StartCoroutine(ConfirmationBox());
    }
    public void ResetButtom(string MenuType)
    {

        if(MenuType == "Audio" && AudioListener.volume != defVolumen)
        {
            Debug.Log(defVolumen);
            AudioListener.volume = defVolumen;
            musicSlider.value = defVolumen;
            musicTextField.text = (defVolumen*100).ToString("0.0");
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
