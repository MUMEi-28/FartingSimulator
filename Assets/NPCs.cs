using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCs : MonoBehaviour
{
    private Animator animator;
    private int runhash;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
       //     animator.SetFloat(runhash, transform.position.magnitude);
    }
}
