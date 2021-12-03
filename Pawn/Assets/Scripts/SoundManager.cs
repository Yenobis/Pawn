using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Sonidos { MOVERESPADA, TOTAL_SONIDOS };
public class SoundManager : MonoBehaviour
{
    public AudioSource[] audios;

    string[] nombreSonidos = { "MoverEspada.mp3"};

    //Reconocer el componente AudioSource
    void Start()
    {
        audios = GetComponents<AudioSource>();

        for (int i = (int) Sonidos.MOVERESPADA; i < (int) Sonidos.TOTAL_SONIDOS; i++)
        {
            audios[i] = AdComponent<AudioSource>();
            audios[i].clip = Resources.Load<AudioClip>(nombreSonidos[i]);
        }
    }

    private T AdComponent<T>()
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  //Detecta la pulsacion del clic izquierdo
        {
            if (audios[(int) Sonidos.MOVERESPADA].isPlaying) //Si el audio se esta reproduciendo
            {
                audios[(int) Sonidos.MOVERESPADA].Pause(); 
            } else
            {
                audios[(int)Sonidos.MOVERESPADA].Play();
            }
        }
    }
}
