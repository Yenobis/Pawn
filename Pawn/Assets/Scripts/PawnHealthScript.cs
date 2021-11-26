using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnHealthScript : MonoBehaviour
{
    public GameObject[] vidas;
    public int life = 100;
    [SerializeField] Slider vidaSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 80)
        {
            vidas[4].gameObject.GetComponent<Image>().color = Color.white;           
        }
        if (life <= 60)
        {
            vidas[3].gameObject.GetComponent<Image>().color = Color.white;
        }
        if (life <= 40)
        {
            vidas[2].gameObject.GetComponent<Image>().color = Color.white;
        }
        if (life <= 20)
        {
            vidas[1].gameObject.GetComponent<Image>().color = Color.white;
        }
        if (life == 0)
        {
            vidas[0].gameObject.GetComponent<Image>().color = Color.white;
        }
        vidaSlider.value = life;
    }
    public void TakeDamage(int amount)
    {
        life -= amount;
    }
}

