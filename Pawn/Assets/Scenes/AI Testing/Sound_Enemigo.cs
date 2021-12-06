using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Enemigo : MonoBehaviour
{
    public enum Sonidos
    {
        MOVERESPADA,
        CAMINAR,
        TOTAL_SONIDOS
    }
    private bool isWalking, isRunning, isAttacking;

    public AudioSource[] audios;
    string[] nombreSonidos = { "MoverEspada", "Caminar" };
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
        isAttacking = GetComponentInChildren<Sword>().atacando;
        //isAttacking = variables.isAttacking;
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
    }
}
