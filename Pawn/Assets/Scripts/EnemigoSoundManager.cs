using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoSoundManager : MonoBehaviour
{
    public enum Sonidos
    {
        MOVERESPADA,
        CAMINAR,
        TOTAL_SONIDOS
    }
    public AudioSource[] audios;

    string[] nombreSonidos = { "MoverEspada", "Caminar" };

    void Start()
    {
        audios = GetComponents<AudioSource>();

        for (int i = (int)Sonidos.MOVERESPADA; i < (int)Sonidos.TOTAL_SONIDOS; i++)
        {
            audios[i].clip = Resources.Load<AudioClip>(nombreSonidos[i]);
        }
    }

    private void Update()
    {
        
    }

    public void CaminarPlay()
    {
        audios[(int)Sonidos.CAMINAR].Play();
    }
    public void CaminarPause()
    {
        audios[(int)Sonidos.CAMINAR].Pause();
    }
}
