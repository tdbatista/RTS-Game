using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMilitiaIdle : StateMachineBehaviour
{
Militia unitData;
    float detectionDelayTimeCount;

    
    float detectionDelayTime =1;
  
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        unitData = animator.GetComponent<Militia>();
        detectionDelayTimeCount=0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if( unitData.SeekEnemys){
            if (unitData.target == null){
                //evita de detectar inimigo a cada frame, sobrecarregando
                if (detectionDelayTimeCount>= detectionDelayTime){
                    unitData.target = unitData.pickNearbyEnemys();
                    detectionDelayTimeCount =0;
                }else{
                    detectionDelayTimeCount += Time.deltaTime;
                }
                    
            } else{
                animator.SetBool("Following",true);
            }        
        }    
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
