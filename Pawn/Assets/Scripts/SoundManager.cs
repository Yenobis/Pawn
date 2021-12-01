using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip moverEspadaSound;
    public AudioClip choqueEspadaSound;

    AudioSource audio;

    //Reconocer el componente AudioSource
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audio.clip = moverEspadaSound;
            audio.Play();
        }
    }
}
