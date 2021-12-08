using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrow : MonoBehaviour
{
    [SerializeField]
    private Transform[] target;
    private Transform player;
    private int puntero = 0;
    private float minima = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Pawn").transform;
        minima = Vector3.Distance(player.position, target[0].transform.position);
        puntero = 0;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Pawn").transform;
        try
        {
            minima = Vector3.Distance(player.position, GameObject.Find(target[0].name).transform.position);
            puntero = 0;
        } catch (MissingReferenceException)
        {
            minima = float.MaxValue;
            puntero = -1;
        }
        for (int i = 1; i < target.Length; i++)
        {
            try
            {
                Vector3 targetPosition = GameObject.Find(target[i].name).transform.position;
                if (Vector3.Distance(player.position, targetPosition) < minima)
                {
                    minima = Vector3.Distance(player.position, targetPosition);
                    puntero = i;
                }
            }
            catch (MissingReferenceException)
            {
                // Est� muerto
            }
        }

        //targetPosition.y = player.position.y;
        try
        {
            //transform.LookAt(2 * player.position - target[puntero].transform.position);
            Vector3 enemigo = target[puntero].transform.position;
            enemigo.y = 0;
            Vector3 enemigo2 = (enemigo - player.position);
            enemigo2.y = 0;
            transform.LookAt((transform.position + transform.forward), enemigo2);

            //Vector2 dir = new Vector2(enemigo.position.x - player.position.x, enemigo.position.z - player.position.z);
            //float angle = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            //gameObject.transform.eulerAngles = new Vector3(0, 0, (enemigo - player.position).y);
            //gameObject.GetComponent<MeshRenderer>().material = m[0];
        }
        catch (IndexOutOfRangeException)
        {
            //gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
            //gameObject.GetComponent<MeshRenderer>().material = m[1];
            //transform.LookAt(2 * player.position - new Vector3(0,10000,0));
        }
    }
}
