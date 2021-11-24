using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sword : MonoBehaviour
{
    public bool stay = false;
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
        Debug.Log("primera");
        if (other.gameObject.layer == LayerMask.NameToLayer("whatIsPlayer"))
        {
            Debug.Log("segunda");
            if (atacando)
            {
                Debug.Log("DAÑOOOOOOOOOOOOOO");
                other.gameObject.GetComponent<Pawn_Health>().TakeDamage(damage);
                atacando = false;
            }
            stay = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        stay = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("whatIsPlayer"))
        {
            if (atacando && stay)
            {
                other.gameObject.GetComponent<Pawn_Health>().TakeDamage(damage);
                stay = false;
                Invoke(nameof(stayToFalse), 1.5f);

            }
        }
    }

    private void stayToFalse()
    {
        stay = true;
    }
}
