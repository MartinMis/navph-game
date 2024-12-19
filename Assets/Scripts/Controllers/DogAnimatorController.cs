using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimatorController : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool isMoving = animator.GetBool("IsMoving");
        if (isMoving)
        {
            Debug.Log("Dog entered a moving state.");
        }
        else
        {
            Debug.Log("Dog entered an idle state.");
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool isMoving = animator.GetBool("IsMoving");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool isMoving = animator.GetBool("IsMoving");
        if (!isMoving)
        {
            Debug.Log("Dog is leaving a moving state.");
        }
        else
        {
            Debug.Log("Dog is leaving an idle state.");
        }
    }
}
