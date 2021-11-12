using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{

    public float max_health = 100f;
    public float cur_health = 0f;

    // Start is called before the first frame update
    void Start()
    {
        cur_health = max_health;
    }

    public void TakeDamage(float amount)
    {
        cur_health -= amount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
