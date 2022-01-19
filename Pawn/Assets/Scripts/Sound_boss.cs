using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_boss : MonoBehaviour
{
    public enum Sonidos
    {
        GOLPE,
        CAMINAR,
        MUERTEENEMIGO,
        TOTAL_SONIDOS
    }

    private bool isWalking, isRunning, isAttacking, isHit;
    private float cur_health;
    private bool alreadyDead = false;

    public AudioSource[] audios;
    string[] nombreSonidos = { "Golpe", "Caminar", "MuerteEnemigo" };
    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponents<AudioSource>();

        for (int i = (int)Sonidos.GOLPE; i < (int)Sonidos.TOTAL_SONIDOS; i++)
        {
            audios[i].clip = Resources.Load<AudioClip>(nombreSonidos[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        BossAI variables = gameObject.GetComponent<BossAI>();
        isWalking = variables.isWalking;
        isRunning = variables.isRunning;
        isHit = variables.isHit;
        cur_health = variables.cur_health;
        isAttacking = GetComponentInChildren<Sword>().atacando;
        //isAttacking = variables.isAttacking;
        if (Time.timeScale == 1f)
        {
            if ((!isAttacking || !isHit) && (isWalking || isRunning))
            {
                if (!audios[(int)Sonidos.CAMINAR].isPlaying)
                {
                    audios[(int)Sonidos.CAMINAR].Play();
                }
            }
            if (isAttacking || isHit)
            {
                audios[(int)Sonidos.CAMINAR].Pause();
                if (!audios[(int)Sonidos.GOLPE].isPlaying)
                {
                    audios[(int)Sonidos.GOLPE].time = 0.1f;
                    audios[(int)Sonidos.GOLPE].Play();
                }
            }
            if (cur_health <= 0 && !alreadyDead)
            {
                alreadyDead = true;
                audios[(int)Sonidos.MUERTEENEMIGO].Play();
            }
        }
        else
        {
            audios[(int)Sonidos.CAMINAR].Pause();
            audios[(int)Sonidos.GOLPE].Pause();
            audios[(int)Sonidos.MUERTEENEMIGO].Pause();
        }
    }
}
