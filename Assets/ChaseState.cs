using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    readonly int isChasing_Hash = Animator.StringToHash("IsChasing");
    readonly int isAttacking_Hash = Animator.StringToHash("IsAttacking");

    NavMeshAgent agent;
    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 태그를 가진 오브젝트 찾기
        agent.speed = 3.5f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(player.position, animator.transform.position); // 자신과 플레이어의 거리 구하기
        if (distance > 15.0f) // 자신과 플레이어의 거리가 일정거리 이상이면
        {
            animator.SetBool(isChasing_Hash, false); // 달리기 애니메이션 설정
        }

        if (distance < 2.5f) // 자신과 플레이어의 거리가 2.5f 이하이면
        {
            animator.SetBool(isAttacking_Hash, true); // 공격 애니메이션 설정
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }

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
