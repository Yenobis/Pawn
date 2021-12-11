using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Enemigo : MonoBehaviour
{
    public enum Sonidos
    {
        MOVERESPADA,
        CAMINAR,
        MUERTEENEMIGO,
        TOTAL_SONIDOS
    }

    private bool isWalking, isRunning, isAttacking;
    private float cur_health;
    private bool alreadyDead = false;

    public AudioSource[] audios;
    string[] nombreSonidos = { "MoverEspada", "Caminar", "MuerteEnemigo" };
    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponents<AudioSource>();

        for (int i = (int)Sonidos.MOVERESPADA; i < (int)Sonidos.TOTAL_SONIDOS; i++)
        {
            audios[i].clip = Resources.Load<AudioClip>(nombreSonidos[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        EnemyAI variables = gameObject.GetComponent<EnemyAI>();
        isWalking = variables.isWalking;
        isRunning = variables.isRunning;
        cur_health = variables.cur_health;
        isAttacking = GetComponentInChildren<Sword>().atacando;
        //isAttacking = variables.isAttacking;
        if (Time.timeScale == 1f)
        {
            if (!isAttacking && (isWalking || isRunning))
            {
                if (!audios[(int)Sonidos.CAMINAR].isPlaying)
                {
                    audios[(int)Sonidos.CAMINAR].Play();
                }
            }
            if (isAttacking)
            {
                audios[(int)Sonidos.CAMINAR].Pause();
                if (!audios[(int)Sonidos.MOVERESPADA].isPlaying)
                {
                    audios[(int)Sonidos.MOVERESPADA].time = 0.1f;
                    audios[(int)Sonidos.MOVERESPADA].Play();
                }
            }
            if (cur_health <= 0 && !alreadyDead)
            {
                alreadyDead = true;
                audios[(int)Sonidos.MUERTEENEMIGO].Play();
            }
        }else
        {
            audios[(int)Sonidos.CAMINAR].Pause();
            audios[(int)Sonidos.MOVERESPADA].Pause();
            audios[(int)Sonidos.MUERTEENEMIGO].Pause();
        }
    }
}
