using UnityEngine;
using System.Collections;

public class SendMessageBackToParent : StateMachineBehaviour
{
    public enum BehaviourType
    {
        Entering,
        Exiting,
        Halfway,
        OnTimeInterval
    }

    public BehaviourType behaviourType;
    public string messageToSend;

    public float timeInterval;
    private bool sentHalfwayMessage;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sentHalfwayMessage = false;
        if(behaviourType == BehaviourType.Entering)
        {
            animator.gameObject.SendMessage(messageToSend, SendMessageOptions.DontRequireReceiver);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (behaviourType == BehaviourType.Halfway)
        {
            if(stateInfo.normalizedTime >= 0.5f && !sentHalfwayMessage)
            {
                animator.gameObject.SendMessage(messageToSend, SendMessageOptions.DontRequireReceiver);
                sentHalfwayMessage = true;
            }

          
        }
        else if (behaviourType == BehaviourType.OnTimeInterval)
        {
           if(stateInfo.normalizedTime >= timeInterval)
           animator.gameObject.SendMessage(messageToSend, SendMessageOptions.DontRequireReceiver);
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (behaviourType == BehaviourType.Exiting)
        {
            animator.gameObject.SendMessage(messageToSend, SendMessageOptions.DontRequireReceiver);
        }

    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    //OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
