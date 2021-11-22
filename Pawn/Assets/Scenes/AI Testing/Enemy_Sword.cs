using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sword : MonoBehaviour
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("whatIsPlayer"))
        {
            if (atacando)
            {
                //Debug.Log("DAÑOOOOOOOOOOOOOO");
                other.gameObject.GetComponent<HealthScript>().TakeDamage(damage);
            }
        }
    }
}
