using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtom : MonoBehaviour
{
    public AudioSource audio
    {
        get
        {
            return GetComponent<AudioSource>();
        }
    }

    public Button button
    {
        get
        {
            return GetComponent<Button>();
        }
    }

    public AudioClip clip;

    void Start()
    {
        gameObject.AddComponent<AudioSource>();

        button.onClick.AddListener(PlaySound);
    }

    // Update is called once per frame
    void PlaySound()
    {
        audio.PlayOneShot(clip);
    }
}
