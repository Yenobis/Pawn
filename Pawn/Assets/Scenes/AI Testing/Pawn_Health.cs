using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn_Health : MonoBehaviour
{
    public GameObject[] vidas;
    public float max_health = 100f;
    public float cur_health = 0f;

    // Start is called before the first frame update
    void Start()
    {
        cur_health = max_health;
    }

    public void TakeDamage(float amount)
    {
        if (cur_health > 0)
        {
            cur_health -= amount;
            int vida = (int)cur_health / 20;
            //Debug.Log(vida);
            vidas[vida].gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
