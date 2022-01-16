using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAttack : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [HideInInspector] public bool stay = false;
    [HideInInspector] public bool atacando = false;
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
                other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
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
                other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
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
