using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
    {
        base.OnStateEnter(animator, animatorStateInfo, layerIndex);
        animator.SetBool("isWalking", true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
    {
        base.OnStateExit(animator, animatorStateInfo, layerIndex);
        animator.SetBool("isWalking", false);
    }
}
