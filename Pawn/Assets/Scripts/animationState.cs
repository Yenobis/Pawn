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
<<<<<<< HEAD
        
=======
>>>>>>> parent of 635ce24 (Cambios en el ataque de Pawn y en la vida y velocidad entre ataques del jefe)
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

<<<<<<< HEAD

        if (!isAttacking && attackPressed && cur_health > 0)
        {
            animator.SetBool("isAttacking", true);
        }
        else if (isAttacking && !attackPressed)
        {
            animator.SetBool("isAttacking", false);
=======
        
        if (!isAttacking && attackPressed && cur_health > 0 && !estoyAtacando)
        {
            animator.SetTrigger("isAttacking");
        } else if  (isAttacking && !attackPressed) {
            //animator.SetBool("isAttacking", false);
>>>>>>> parent of 635ce24 (Cambios en el ataque de Pawn y en la vida y velocidad entre ataques del jefe)
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
        
    }

}
