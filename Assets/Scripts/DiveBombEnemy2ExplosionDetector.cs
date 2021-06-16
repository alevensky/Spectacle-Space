using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveBombEnemy2ExplosionDetector : StateMachineBehaviour
{
    public static bool diveBombEnemy2DestroyedAnimPlaying = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        diveBombEnemy2DestroyedAnimPlaying = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        diveBombEnemy2DestroyedAnimPlaying = false;
    }
}
