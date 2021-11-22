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

    [Space(10)]
    [SerializeField] private Toggle fullscreenToggle ;

    [Header("Graficos")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessField = null;
    [SerializeField] private float defBrightness = 0.5f;
    private float _brightnessLevel;
    private bool _isFullScreen;

    [Header("Resoluciones")]
    public TMP_Dropdown resolutionDropdown = null;
    private Resolution[] resolutions;



   void Start(){
        //Resoluciones
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> opciones = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; ++i)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            opciones.Add(option);
            if(resolutions[i].width == Screen.width && resolutions[i].height== Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(opciones);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        //Volumen
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
    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessField.text = (brightness * 100).ToString("0.0");
    }
    public void SetFullScreen(bool fullscreen)
    {
        _isFullScreen = fullscreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void AplicarVolumen()
    {
        PlayerPrefs.SetFloat(BackgroundMusic, musicSlider.value);
        PlayerPrefs.SetFloat(SfxMusic, sfxSlider.value);
        StartCoroutine(ConfirmationBox());
    }

    public void AplicarGraficos()
    {
        PlayerPrefs.SetFloat("masterBrillo", _brightnessLevel);
        //Cambiar brillo del postprocesado **FALTA VERLO**

        PlayerPrefs.SetInt("masterFullScreen", _isFullScreen ? 1:0);
        Screen.fullScreen = _isFullScreen;
        StartCoroutine(ConfirmationBox());
    }
    public void ResetButtom(string MenuType)
    {

        if(MenuType == "Audio")
        {
            audioBackground.volume = defVolumen;
            for (int i = 0; i < audioSFX.Length; ++i)
            {
                audioSFX[i].volume = defVolumen;
            }

            musicSlider.value = defVolumen;
            musicTextField.text = (defVolumen*100).ToString("0.0");

            sfxSlider.value = defVolumen;
            sfxTextField.text = (defVolumen * 100).ToString("0.0");
            AplicarVolumen();
        }
        if (MenuType == "Graficos")
        {
            //Resetear Brillo **FALTA VERLO**
            brightnessSlider.value = defBrightness;
            brightnessField.text = (defBrightness * 100).ToString("0.0");

            fullscreenToggle.isOn = true;
            Screen.fullScreen = true;
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            AplicarGraficos();
        }


    }
    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
