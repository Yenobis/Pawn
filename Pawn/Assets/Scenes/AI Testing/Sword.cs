using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private bool soyPawn;
    public bool stay = false;
    public bool atacando = false;
    float damage = 20f;
    Collider m_ObjectCollider;
    // Start is called before the first frame update
    void Start()
    {
        m_ObjectCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (soyPawn)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("whatIsEnemy"))
            {
                if (atacando)
                {
                    //Debug.Log("DAÑOOOOOOOOOOOOOO");
                    other.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
                    atacando = false;

                }
                stay = true;
            }
        } else
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
    }
    private void OnTriggerExit(Collider other)
    {
        stay = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (soyPawn)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("whatIsEnemy"))
            {
                if (atacando && stay)
                {
                    other.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
                    stay = false;
                    Invoke(nameof(stayToFalse), 1.5f);

                }
            }
        } else
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
    }

    private void stayToFalse()
    {
        stay = true;
    }
}
