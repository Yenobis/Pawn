using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Pawn").transform;
        Vector3 targetPosition = target.transform.position;
        //Debug.Log(targetPosition);
        //targetPosition.y = player.position.y;
        transform.LookAt(2 * player.position - target.transform.position);
    }
}
