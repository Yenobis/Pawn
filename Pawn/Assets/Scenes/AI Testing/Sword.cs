using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool atacando = false;
    float damage = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        atacando = GetComponentInParent<animationScript_IA_Version>().probando;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (atacando)
            {
                //Debug.Log("DAÑOOOOOOOOOOOOOO");
                other.gameObject.GetComponent<HealthScript>().TakeDamage(damage);
            }
        }
        /*
        if (other.gameObject.layer == 7)
        {
            Debug.Log("DAÑO a mí mismo");
        }
        */
    }
}
