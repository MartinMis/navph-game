using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Controller for the dog animations.
    /// </summary>
    public class DogAnimatorController : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            bool isMoving = animator.GetBool("IsMoving");
            Debug.Log(isMoving ? "Dog entered a moving state." : "Dog entered an idle state.");
        }
        

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            bool isMoving = animator.GetBool("IsMoving");
            Debug.Log(!isMoving ? "Dog is leaving a moving state." : "Dog is leaving an idle state.");
        }
    }
}
