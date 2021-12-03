using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public enum Sonidos { 
        MOVERESPADA,
        SALTOTIERRA,
        CAMINAR,
        TOTAL_SONIDOS
    }
    public AudioSource[] audios;

    string[] nombreSonidos = { "MoverEspada", "SaltoTierra", "Caminar" };

    void Start()
    {
        audios = GetComponents<AudioSource>();

        for (int i = (int)Sonidos.MOVERESPADA; i < (int)Sonidos.TOTAL_SONIDOS; i++)
        {
            audios[i].clip = Resources.Load<AudioClip>(nombreSonidos[i]);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  //Detecta la pulsacion del clic izquierdo
        {
            if (!audios[(int)Sonidos.MOVERESPADA].isPlaying) {
                audios[(int)Sonidos.MOVERESPADA].Play();
            }
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (!audios[(int)Sonidos.CAMINAR].isPlaying)
            {
                audios[(int)Sonidos.CAMINAR].Play();
            }
        } else
        {
            if (audios[(int)Sonidos.CAMINAR].isPlaying)
            {
                audios[(int)Sonidos.CAMINAR].Pause();
            }
        }
    }

}
