using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaqueteVida : MonoBehaviour
{
    [SerializeField] private float heal = 20f;
    
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

        if (other.gameObject.layer == LayerMask.NameToLayer("whatIsPlayer")&& !other.gameObject.GetComponent<PlayerController>().isAtMaxHealth())
        {
            Debug.Log("curar");
            other.gameObject.GetComponent<PlayerController>().HealDamage(heal);
            Destroy(this.gameObject);
        }
        
    }
}
