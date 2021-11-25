using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationScript_IA_Version : MonoBehaviour
{
    private int life;
    private float cur_health;

    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        
        bool keyPressed = false;
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool isAttacking = animator.GetBool("isAttacking");
        bool runPressed = Input.GetKey("left shift");
        //life = GameObject.Find("Pawn").GetComponent<PawnHealthScript>().life;
        cur_health = GameObject.Find("Pawn").GetComponent<Pawn_Health>().cur_health;
        bool attackPressed = Input.GetMouseButtonDown(0);

        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            keyPressed = true;
        }
        else
        {
            keyPressed = false;
        }


        if (!isWalking && keyPressed)
        {
            animator.SetBool("isWalking", true);
        }
        else if (isWalking && !keyPressed)
        {
            animator.SetBool("isWalking", false);
        }

        if (!isRunning && keyPressed && runPressed)
        {
            animator.SetBool("isRunning", true);
        }
        else if (isRunning && (!keyPressed || !runPressed))
        {
            animator.SetBool("isRunning", false);
        }


        if (!isAttacking && attackPressed)
        {
            animator.SetBool("isAttacking", true);
        }


        else if (isAttacking && !attackPressed)
        {
            animator.SetBool("isAttacking", false);

        }

        if (cur_health <= 0)
        {
            animator.SetTrigger("Die");
            //Destroy(gameObject, 3f);
        }
    }

    void dealingDamage()
    {
        if (gameObject.layer == LayerMask.NameToLayer("whatIsEnemy"))
        {
            GetComponentInChildren<Enemy_Sword>().atacando = true;
            //Debug.Log("ENEMIGO ATACANDO");
        }  else
        {
            GetComponentInChildren<Sword>().atacando = true;
            //Debug.Log("PAWN ATACANDO");
        }
        
    }

    void notDealingDamage()
    {
        if (gameObject.layer == LayerMask.NameToLayer("whatIsEnemy"))
        {
            GetComponentInChildren<Enemy_Sword>().atacando = false;
            //Debug.Log("ENEMIGO NO ATACANDO");
        }
        else
        {
            GetComponentInChildren<Sword>().atacando = false;
            //Debug.Log("PAWN NO ATACANDO");
        }
        
    }
}
