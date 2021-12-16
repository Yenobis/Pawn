using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class MainMenuScript : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BackgroundMusic = "BackgroundMusic";
    private static readonly string SfxMusic = "SfxMusic";
    private static readonly string AmbientalMusic = "AmbientalMusic";
    private static readonly string MasterMusic = "MasterMusic";

    private static readonly string MasterFullScreen = "MasterFullScreen";
    private static readonly string MasterResolution = "MasterResolution";
    private static readonly string MasterBrillo = "MasterBrillo";

    int firstPlayInt = 0;

    // Start is called before the first frame update
    [Header("Niveles a Cargar")]
    private string partidaAcargar;
    [SerializeField] private GameObject noGameSaved = null;
    public string _menuACargar;

[Header("Volumen")]
    [SerializeField] private Slider masterSlider = null;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private Slider ambientalSlider = null;

    [SerializeField] private AudioMixer auxioMixer = null;
    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private float defVolumen = 1.0f;

    private float masterVolumePreSave;
    private float musicVolumePreSave;
    private float sfxVolumePreSave;
    private float ambientalVolumePreSave;

    [Header("Gráficos")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private float defBrightness = 0.5f;
    [SerializeField] private Toggle fullscreenToggle;
    private float _brightnessLevel;
    private bool _isFullScreen;

    [Header("Resoluciones")]
    public TMP_Dropdown resolutionDropdown = null;
    private Resolution[] resolutions;
    private int resolutionIndex;

    private bool fullscreenPreSave;
    private float brightnessPreSave;
    private int resolutionIndexPreSave;

    void Start(){
        //Graficos Dropdown Resulución
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray(); 
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
        //Fullscreen
        if (PlayerPrefs.GetInt(MasterFullScreen) == 1) {
            fullscreenToggle.isOn = true;
            Screen.fullScreen = true;
        }else
        {
            fullscreenToggle.isOn = false;
            Screen.fullScreen = false;
        }
       

        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);
        if (firstPlayInt == 0 )
        {
            //Volumen
            masterSlider.value = defVolumen;
            musicSlider.value = defVolumen;
            sfxSlider.value = defVolumen;
            ambientalSlider.value = defVolumen;
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else {
            //Volumen
            masterSlider.value = PlayerPrefs.GetFloat(MasterMusic);
            musicSlider.value =  PlayerPrefs.GetFloat(BackgroundMusic);
            sfxSlider.value = PlayerPrefs.GetFloat(SfxMusic);
            ambientalSlider.value = PlayerPrefs.GetFloat(AmbientalMusic);
            //Resolución
            resolutionDropdown.value = PlayerPrefs.GetInt(MasterResolution);
            //Cambiar brillo del postprocesado **FALTA VERLO**
        }
    }
    public void LoadScene(string escena){
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }
        SceneManager.LoadScene(escena);
    }
    public void GotoMainMenu()
    {
        //Podemos guardar el Estado de la partida 
        SceneManager.LoadScene(_menuACargar);
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
        if (volume == 0)
        {
            auxioMixer.SetFloat("musicVolume", -80);
        }
        else
        {
            auxioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        }
    }
    public void SetMasterVolumen(float volume)
    {
        if (volume == 0)
        {
            auxioMixer.SetFloat("masterVolume", -80);
        }
        else
        {
            auxioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
        }
        
    }
    public void SetSfxVolumen(float volume)
    {
        if (volume == 0)
        {
            auxioMixer.SetFloat("sfxVolume", -80);
        }
        else
        {
            auxioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        }
        
    }
    public void SetAmbientalVolumen(float volume)
    {
        if (volume == 0)
        {
            auxioMixer.SetFloat("ambientalVolume", -80);
        }
        else
        {
            auxioMixer.SetFloat("ambientalVolume", Mathf.Log10(volume) * 20);
        }

    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
    }
    public void SetFullScreen(bool fullscreen)
    {
        _isFullScreen = fullscreen;
        Screen.fullScreen = fullscreen;
    }
    public void SetResolution(int rIndex)
    {
        resolutionIndex = rIndex;
        Resolution resolution = resolutions[rIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

    }

    public void AplicarVolumen()
    {
        PlayerPrefs.SetFloat(MasterMusic, masterSlider.value);
        PlayerPrefs.SetFloat(BackgroundMusic, musicSlider.value);
        PlayerPrefs.SetFloat(SfxMusic, sfxSlider.value);
        PlayerPrefs.SetFloat(AmbientalMusic, ambientalSlider.value);
        StartCoroutine(ConfirmationBox());
    }
    public void DescartarVolumen()
    {
        masterSlider.value = masterVolumePreSave;
        SetMasterVolumen(masterVolumePreSave);
        musicSlider.value = musicVolumePreSave;
        SetMusicVolumen(musicVolumePreSave);
        sfxSlider.value = sfxVolumePreSave;
        SetSfxVolumen(sfxVolumePreSave);
        ambientalSlider.value = ambientalVolumePreSave;
        SetAmbientalVolumen(ambientalVolumePreSave);
    }

    public void DescartarGraficos()
    {
        SetFullScreen(fullscreenPreSave);
        fullscreenToggle.isOn = fullscreenPreSave;
        //Cambiar brillo del postprocesado **FALTA VERLO**
        //brightnessPreSave;
        //Debug.Log(fullscreenPreSave);
        SetResolution(resolutionIndexPreSave);
        resolutionDropdown.value = resolutionIndexPreSave;
    }
    public void SetPreSaves(string MenuType)
    {
        if(MenuType == "Audio")
        {
            musicVolumePreSave = musicSlider.value;
            masterVolumePreSave= masterSlider.value;
            sfxVolumePreSave= sfxSlider.value;
            ambientalVolumePreSave = ambientalSlider.value;
        }
        if (MenuType == "Gráficos")
        {
            resolutionIndexPreSave = resolutionIndex;
            fullscreenPreSave = _isFullScreen;
            Debug.Log(resolutionIndexPreSave);
            Debug.Log(fullscreenPreSave);
        }

    }
    public void AplicarGraficos()
    {
        PlayerPrefs.SetFloat(MasterBrillo, _brightnessLevel);
        //Cambiar brillo del postprocesado **FALTA VERLO**
        PlayerPrefs.SetInt(MasterResolution, resolutionIndex);
        PlayerPrefs.SetInt(MasterFullScreen, _isFullScreen ? 1:0);
        Screen.fullScreen = _isFullScreen;


        StartCoroutine(ConfirmationBox());
    }

    public void ResetButtom(string MenuType)
    {

        if(MenuType == "Audio")
        {
            masterSlider.value = defVolumen;

            musicSlider.value = defVolumen;

            sfxSlider.value = defVolumen;

            ambientalSlider.value = defVolumen;
        }
        if (MenuType == "Gráficos")
        {
            //Resetear Brillo **FALTA VERLO** postprocesing
            brightnessSlider.value = defBrightness;

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
        yield return new WaitForSeconds(1);
        confirmationPrompt.SetActive(false);
    }
}
