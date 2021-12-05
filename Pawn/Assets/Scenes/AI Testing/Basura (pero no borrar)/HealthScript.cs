using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    AudioSource audioData;
    public float max_health = 100f;
    public float cur_health = 0f;

    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        cur_health = max_health;
    }

    public void TakeDamage(float amount)
    {
        if (cur_health > 0)
        {
            audioData.time = 0.2f;
            audioData.Play(0);
            cur_health -= amount;
        } else
        {
            audioData.time = 0.2f;
            audioData.Play(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cur_health <= 0)
        {
            GameObject.Find("Eje_Y_Puerta").GetComponent<Giro_Puerta>().abrirPuerta();
            
            transform.localScale = new Vector3(0f, 0f, 0f);
            Destroy(gameObject, 2f);
        }
    }
}
