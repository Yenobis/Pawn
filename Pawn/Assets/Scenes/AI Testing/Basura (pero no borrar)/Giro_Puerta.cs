using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giro_Puerta : MonoBehaviour
{
    private bool sonando = false;
    AudioSource audioData;
    public Boolean abierta = false;
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (abierta)
        {
            if (transform.rotation.y > -0.5f)
            {
                transform.Rotate(0, -15 * (1 - transform.rotation.y * -2) * Time.deltaTime, 0, Space.World);
            } else
            {
                this.enabled = false;
            }
        }
    }

    public void abrirPuerta()
    {
        abierta = true;

        if (!sonando)
        {
            audioData.time = 0.1f;
            audioData.Play(0);
            sonando = true;
        }
    }
}
