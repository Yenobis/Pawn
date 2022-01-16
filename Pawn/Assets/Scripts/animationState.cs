using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationState : MonoBehaviour
{
    private int life;
    private float cur_health;
    Animator animator;
    bool estoyAtacando;

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
        cur_health = GameObject.Find("Pawn").GetComponent<PlayerController>().cur_health;
        bool attackPressed = Input.GetMouseButtonDown(0);

        if (Input.GetKey("w")|| Input.GetKey("a")|| Input.GetKey("s")|| Input.GetKey("d")){
            keyPressed=true;
        } else {
            keyPressed=false;
        }


        if (!isWalking && keyPressed){
            animator.SetBool("isWalking", true);
        } else if (isWalking && !keyPressed){
            animator.SetBool("isWalking", false);
        }
        
        if (!isRunning && keyPressed && runPressed){
            animator.SetBool("isRunning", true);
        }else if  (isRunning && (!keyPressed || !runPressed)) {
            animator.SetBool("isRunning", false);
        }

        
        if (!isAttacking && attackPressed && cur_health > 0 && !estoyAtacando)
        {
            animator.SetBool("isAttacking", true);
        } else if  (isAttacking && !attackPressed) {
            animator.SetBool("isAttacking", false);
        }

        if (cur_health <= 0)
        {
            animator.SetTrigger("Die");
        }


        }

    void dealingDamage()
    {
        GetComponentInChildren<Sword>().atacando = true;

    }

    void notDealingDamage()
    {
        GetComponentInChildren<Sword>().atacando = false;

    }

    void inicioAtaque()
    {
        estoyAtacando = true;
    }

    void finAtaque()
    {
        estoyAtacando = false;
        animator.SetBool("isAttacking", false);
    }

}
