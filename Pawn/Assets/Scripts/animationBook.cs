using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationBook : MonoBehaviour
{

        Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        bool isOpen = animator.GetBool("isOpen");

        if (Input.GetKeyDown("o")){
            animator.SetBool("isOpen", true);
        }
        
        if (Input.GetKeyDown("c")){
            animator.SetBool("isOpen", false);
        }


    }
}
