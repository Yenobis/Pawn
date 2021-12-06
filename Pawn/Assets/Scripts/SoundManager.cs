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

    private void Update()
    {
        if (Time.timeScale == 1f)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                if (!audios[(int)Sonidos.CAMINAR].isPlaying)
                {
                    audios[(int)Sonidos.CAMINAR].Play();
                }
            }
            else
            {
                if (audios[(int)Sonidos.CAMINAR].isPlaying)
                {
                    audios[(int)Sonidos.CAMINAR].Pause();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!audios[(int)Sonidos.SALTOTIERRA].isPlaying)
                {
                    audios[(int)Sonidos.SALTOTIERRA].Play();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!audios[(int)Sonidos.MOVERESPADA].isPlaying)
                {
                    audios[(int)Sonidos.MOVERESPADA].Play();
                }
            }
        } else
        {
            audios[(int)Sonidos.CAMINAR].Pause();
            audios[(int)Sonidos.SALTOTIERRA].Pause();
            audios[(int)Sonidos.MOVERESPADA].Pause();
        }
    }
}
