using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HogueraSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audios;

    void Start()
    {
        audios = GetComponent<AudioSource>();
        audios.clip = Resources.Load<AudioClip>("Hoguera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1f)
        {
            audios.Play();
        }
        else
        {
            audios.Pause();
        }
    }
}
