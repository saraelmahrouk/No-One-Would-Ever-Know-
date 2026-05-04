using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScriptController : MonoBehaviour
{
    private Animator animator;
    // private bool isHolding = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (animator == null) return;
        bool isWalking =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D);

        animator.SetBool("Iswalking", isWalking);

    //    if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         isHolding = !isHolding;
    //         animator.SetBool("IsHolding", isHolding);
    //           Debug.Log("IsHolding = " + isHolding);
    //     }
    
     if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
        }
    }
}