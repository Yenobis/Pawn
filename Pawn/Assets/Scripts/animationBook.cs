using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationBook : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", true);
        StartCoroutine(lanza());

    }
    public IEnumerator lanza()
    {
        yield return new WaitForSeconds(3.6f);
        canvas.SetActive(true);
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
