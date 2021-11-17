using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationState : MonoBehaviour
{
    private int life; 
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        

    }

    // Update is called once per frame

    

    void Update()
    {
        bool keyPressed = false;
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool isAttacking = animator.GetBool("isAttacking");
        bool runPressed = Input.GetKey("left shift");

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

        
        if (!isAttacking && attackPressed){
            animator.SetBool("isAttacking", true);
        }else if  (isAttacking && !attackPressed) {
            animator.SetBool("isAttacking", false);
        }

        life = GameObject.Find("Pawn").GetComponent<PawnHealthScript>().life;
        if (life == 0) {
            animator.SetTrigger("Die");
        }
        

    }
}
