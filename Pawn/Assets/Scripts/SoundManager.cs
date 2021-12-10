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
        MUERTEPAWN,
        TOTAL_SONIDOS
    }
    public AudioSource[] audios; 
    private float cur_health;

    string[] nombreSonidos = { "MoverEspada", "SaltoTierra", "Caminar", "MuertePawn" };

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
        cur_health = GameObject.Find("Pawn").GetComponent<PlayerController>().cur_health;

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

            if (cur_health <= 0)
            {
                if (!audios[(int)Sonidos.MUERTEPAWN].isPlaying)
                {
                    audios[(int)Sonidos.MUERTEPAWN].Play();
                } else
                {

                    audios[(int)Sonidos.MUERTEPAWN].Pause();
                }
            }
        } else
        {
            audios[(int)Sonidos.CAMINAR].Pause();
            audios[(int)Sonidos.SALTOTIERRA].Pause();
            audios[(int)Sonidos.MOVERESPADA].Pause();
            audios[(int)Sonidos.MUERTEPAWN].Pause();
        }
    }
}
