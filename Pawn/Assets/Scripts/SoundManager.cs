using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;

    private AudioSource audio;

    //Reconocer el componente AudioSource
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void GestionAudios(int ind, float vol)
    {
        audio.PlayOneShot(audioClips[ind], vol);
    }
}
